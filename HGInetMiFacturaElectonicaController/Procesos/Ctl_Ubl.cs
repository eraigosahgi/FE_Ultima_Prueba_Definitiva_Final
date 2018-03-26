using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using HGInetUBL.Objetos;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	/// <summary>
	/// Controlador para gestionar la generación del estándar UBL
	/// </summary>
	public class Ctl_Ubl
	{

		/// <summary>
		/// Genera la información del documento en formato UBL
		/// </summary>
		/// <param name="id">id único de identificación de la plataforma</param>
		/// <param name="documento">datos del documento</param>
		/// <param name="pruebas">indica si el documento es de pruebas (true)</param>
		/// <returns>datos del documento</returns>
		public static FacturaE_Documento Generar(Guid id, Factura documento, bool pruebas = false)
		{
			TipoDocumento tipo = TipoDocumento.Factura;

			// resolución del documento
			HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();

			if (pruebas)
			{
				/*
					# Resolucion : 9000000033394696 
					Desde 990000000
					Hasta 995000000
					Identificador 606f5740-c6b9-494f-931c-5a6b3e22d72c
					Pin Pfe2017
					Clave Tecnica dd85db55545bd6566f36b0fd3be9fd8555c36e
					Clave Ambiente Prueba2017
					Ruta Servicio https://facturaelectronica.dian.gov.co/habilitacion/B2BIntegrationEngine/FacturaElectronica/facturaElectronica.wsdl
				 */

				// obtiene los datos de prueba del proveedor tecnológico de la DIAN
				DianProveedorTest data_dian = HgiConfiguracion.GetConfiguration().DianProveedorTest;

				//Ctl_ResolucionDian.Obtener(id, data_dian.IdSoftware, data_dian.ClaveAmbiente, documento.DatosObligado.Identificacion, data_dian.NitProveedor);

				// convierte la información de la resolución a la extensión DIAN
				extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
				{
					TipoDocumento = tipo.GetHashCode(),
					NumResolucion = "9000000033394696",
					Prefijo = "",
					FechaResIni = new DateTime(2017, 07, 02),
					FechaResFin = new DateTime(2027, 07, 02),
					RangoIni = 990000000,
					RangoFin = 995000000,
					IdSoftware = data_dian.IdSoftware,
					NitProveedor = data_dian.NitProveedor,
					ClaveTecnicaDIAN = "dd85db55545bd6566f36b0fd3be9fd8555c36e",
					PinSoftware = data_dian.Pin
				};
			}
			else
			{
				// obtiene los datos del proveedor tecnológico de la DIAN
				DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;



			}

			// convierte el documento 
			FacturaE_Documento resultado = FacturaXML.CrearDocumento(documento, extension_documento);
			resultado.DocumentoTipo = tipo;
			resultado.ID = id;

			// genera el nombre del archivo ZIP
			resultado.NombreZip = NombramientoArchivo.ObtenerZip(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo);

			return resultado;
		}

		public static FacturaE_Documento Generar(Guid id, NotaCredito documento, bool pruebas = false)
		{
			return null;
		}

		public static FacturaE_Documento Generar(Guid id, NotaDebito documento, bool pruebas = false)
		{
			return null;
		}

		/// <summary>
		/// Almacena el archivo Xml físicamente
		/// </summary>
		/// <param name="documento">datos del documento</param>
		/// <returns>datos del documento</returns>
		public static FacturaE_Documento Almacenar(FacturaE_Documento documento)
		{
			System.IO.StreamWriter file = null;

			try
			{
				
				string nit_obligado = string.Empty;

				switch (documento.DocumentoTipo)
				{
					case TipoDocumento.Factura:
						Factura doc_factura = ((Factura)documento.Documento);
						nit_obligado = doc_factura.DatosObligado.Identificacion;
						break;
					case TipoDocumento.NotaCredito:
						NotaCredito doc_nota_credito = ((NotaCredito)documento.Documento);
						nit_obligado = doc_nota_credito.DatosObligado.Identificacion;
						break;
					case TipoDocumento.NotaDebito:
						/*NotaDebito doc_nota_debito = ((NotaDebito)documento.Documento);
						nit_obligado = doc_nota_debito.DatosObligado.Identificacion;
						*/
						break;
					default:
						break;
				}


				// carpeta del xml
				string carpeta_xml = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), nit_obligado);
				carpeta_xml = string.Format(@"{0}{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

				// valida la existencia de la carpeta
				carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

				// ruta del xml
				string ruta_xml = string.Format(@"{0}\{1}.xml", carpeta_xml, documento.NombreXml);
				
				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);
				
				// almacena el archivo xml
				using (file = new System.IO.StreamWriter(ruta_xml))
				{
					file.WriteLine(documento.DocumentoXml.ToString());
				}

				return documento;
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
			finally
			{
				if (file != null)
					file.Close();
			}
		}

	}
}
