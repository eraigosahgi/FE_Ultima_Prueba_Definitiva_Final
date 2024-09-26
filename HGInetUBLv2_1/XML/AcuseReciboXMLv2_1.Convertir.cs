using FirmaXadesNet;
using FirmaXadesNet.Crypto;
using FirmaXadesNet.Signature;
using FirmaXadesNet.Signature.Parameters;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace HGInetUBLv2_1
{
	public partial class AcuseReciboXMLv2_1
	{
		/// <summary>
		/// Convierte el XML-UBL2.1 en un objeto de Servicio 
		/// </summary>
		/// <param name="acuse_ubl">Objeto Acuse para convertir</param>
		/// <returns>Objeto tipo Acuse</returns>
		public static List<Acuse> Convertir(ApplicationResponseType acuse_ubl)
		{

			List<Acuse> Objeto_acuse = new List<Acuse>();

			try
			{

				for (int i = 0; i < acuse_ubl.DocumentResponse.Length; i++)
				{
					Acuse doc_acuse = new Acuse();

					Match numero_doc = Regex.Match(acuse_ubl.DocumentResponse[i].DocumentReference[0].ID.Value, "\\d+");

					try
					{
						doc_acuse.Documento = Convert.ToInt64(numero_doc.Value);
					}
					catch (Exception)
					{
						
					}

					doc_acuse.Prefijo = acuse_ubl.DocumentResponse[i].DocumentReference[0].ID.Value.Substring(0, acuse_ubl.DocumentResponse[i].DocumentReference[0].ID.Value.Length - doc_acuse.Documento.ToString().Length);

					doc_acuse.CufeDocumento = acuse_ubl.DocumentResponse[i].DocumentReference[0].UUID.Value;

					doc_acuse.IdAcuse = acuse_ubl.ID.Value;
					//doc_acuse.IdSeguridad = acuse.DocumentResponse[0].DocumentReference.UUID.Value;
					if (acuse_ubl.DocumentResponse[i].DocumentReference[0].DocumentType != null && !string.IsNullOrEmpty(acuse_ubl.DocumentResponse[i].DocumentReference[0].DocumentType.Value))
						doc_acuse.TipoDocumento = acuse_ubl.DocumentResponse[i].DocumentReference[i].DocumentType.Value;

					doc_acuse.CodigoRespuesta = acuse_ubl.DocumentResponse[i].Response.ResponseCode.Value;
					doc_acuse.MvoRespuesta = acuse_ubl.DocumentResponse[i].Response.Description[0].Value;

					Tercero adquiriente = new Tercero();
					adquiriente.Identificacion = acuse_ubl.SenderParty.PartyTaxScheme[0].CompanyID.Value;
					adquiriente.RazonSocial = acuse_ubl.SenderParty.PartyTaxScheme[0].RegistrationName.Value;
					doc_acuse.DatosAdquiriente = adquiriente;

					Tercero obligado = new Tercero();
					obligado.Identificacion = acuse_ubl.ReceiverParty.PartyTaxScheme[0].CompanyID.Value;
					obligado.RazonSocial = acuse_ubl.ReceiverParty.PartyTaxScheme[0].RegistrationName.Value;
					doc_acuse.DatosObligado = obligado;

					DateTime fecha = acuse_ubl.IssueDate.Value;
					DateTime hora = Convert.ToDateTime(acuse_ubl.IssueTime.Value);

					DateTime fecha_hora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);

					doc_acuse.Fecha = Convert.ToDateTime(fecha_hora);

					//Si en el documento viene repetido el evento no lo agrega.
					if (!Objeto_acuse.Any(x => x.CodigoRespuesta.Equals(doc_acuse.CodigoRespuesta)))
						Objeto_acuse.Add(doc_acuse);
				}

				return Objeto_acuse;

			}
			catch (Exception ex)
			{
				string mensaje = string.Format("Se presento inconsistencia convirtiendo el xml de Acuse a objeto. Detalle: {0}", ex.Message);
				RegistroLog.EscribirLog(ex, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna, mensaje);
				throw new ApplicationException(mensaje, ex.InnerException);
			}


		}

		/// <summary>
		/// Verifica la firma de un documento XML.
		/// </summary>
		/// <param name="Doc">
		/// Documento XML a verificar.
		/// </param>
		/// <param name="Key">
		/// Llave con la que fue firmado el documento XML.
		/// </param>
		/// <returns>
		/// Valor booleano que indica éxito o error
		/// </returns>
		public static Boolean VerificarXML(XmlDocument xmlDoc, RSA llave)
		{
			// Comprobamos los argumentos.
			if (xmlDoc == null)
				throw new ArgumentException("El argumento Doc no puede ser null.");
			if (llave == null)
				throw new ArgumentException("El argumento Key no puede ser null.");

			// Cree un nuevo objeto SignedXml y pásele el objeto XmlDocument.
			SignedXml xmlFirmado = new SignedXml(xmlDoc);

			XmlElement nodo_sign = null;
			bool valido = false;

			foreach (XmlElement item in xmlDoc.DocumentElement)
			{
				if (item.LocalName.Equals("UBLExtensions"))
				{
					foreach (XmlNode item_int in item.ChildNodes)
					{

						if (item_int.LocalName.Equals("UBLExtension"))
						{
							foreach (XmlNode item_interno in item_int.FirstChild)
							{

								if (item_interno.LocalName.Equals("Signature"))
								{
									valido = true;
									nodo_sign = (XmlElement)item_int.FirstChild.ChildNodes[0];
									break;
								}


							}
							
						}

						if (valido == true)
							break;
					}


				}
				if (valido == true)
					break;
			}

			// Busque el elemento <signature> y cree un nuevo objeto XmlNodeList.
			XmlNodeList listaNodos = nodo_sign.GetElementsByTagName("Signature");

			// Se lanza una excepción si no se ha encontrado una firma.
			if (listaNodos.Count <= 0)
			{
				throw new CryptographicException("La verificación de la firma ha fallado: No se ha encontrado una firma en el documento.");
			}

			// Este ejemplo solo suporta una firma para el documento xml entero.
			// Se lanza una excepción si se encuentra más de una firma.
			if (listaNodos.Count >= 2)
			{
				throw new CryptographicException("La verificación de la firma ha fallado: Se ha encontrado más de una firma en el documento.");
			}

			// Cargue el XML del primer elemento <signature> en el objeto SignedXml.
			xmlFirmado.LoadXml(nodo_sign);

			// Compruebe la firma mediante el método CheckSignature y la clave pública RSA.
			// Este método devuelve un valor booleano que indica éxito o error.

			bool prueba = xmlFirmado.CheckSignature(llave);

			return prueba;
		}
	}
}
