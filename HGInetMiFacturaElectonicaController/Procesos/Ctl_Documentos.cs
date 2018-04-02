using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetFirmaDigital;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Funciones;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	/// <summary>
	/// Controlador para gestionar los documentos
	/// </summary>
	public class Ctl_Documentos
	{

		/// <summary>
		/// Realiza el proceso de la plataforma para el documento
		/// 1. Generar UBL - 2. Firmar - 3. Almacenar XML - 4. Comprimir - 5. Enviar DIAN
		/// </summary>
		/// <param name="id_peticion">id único de identificación de la plataforma</param>
		/// <param name="documento_obj">datos del documento</param>
		/// <param name="pruebas">indica si el documento es de pruebas (true)</param>
		/// <returns>datos de resultado para el documento</returns>
		public static FacturaE_Documento Procesar(Guid id_peticion, Factura documento_obj, bool pruebas = false)
		{
			// genera el xml en ubl
			FacturaE_Documento documento_result = Ctl_Ubl.Generar(id_peticion, documento_obj, pruebas);

			// almacena el xml
			documento_result = Ctl_Ubl.Almacenar(documento_result);

			// firma el xml 
			string ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), "certificado_test.p12");
			documento_result = Ctl_Firma.Generar(ruta_certificado, "6c 0b 07 62 62 6d a0 e2", "persona_juridica_pruebas1", EnumCertificadoras.Andes, documento_result);

			// comprime el archivo xml
			Ctl_Compresion.Comprimir(documento_result);

			// envia el documento a la DIAN
			HGInetDIANServicios.DianFactura.AcuseRecibo acuse = Ctl_DocumentoDian.Enviar(documento_result);

			return documento_result;

		}

		/// <summary>
		/// Procesa una lista de documentos
		/// </summary>
		/// <param name="documentos"></param>
		/// <returns></returns>
		public static List<DocumentoRespuesta> Procesar(List<Factura> documentos)
		{

			DateTime fecha_actual = Fecha.GetFecha();

			List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

			foreach (Factura item in documentos)
			{
				DocumentoRespuesta respuesta_tmp = new DocumentoRespuesta()
				{
					Aceptacion = 0,
					CodigoRegistro = item.CodigoRegistro,
					Cufe = "",
					DescripcionProceso = "Recepción - Información del documento.",
					Documento = item.Documento,
					Error = new LibreriaGlobalHGInet.Error.Error() { Codigo = LibreriaGlobalHGInet.Error.CodigoError.OK, Fecha = fecha_actual, Mensaje = "Pruebas de recepción de documentos." },
					FechaRecepcion = fecha_actual,
					FechaUltimoProceso = fecha_actual,
					IdDocumento = Guid.NewGuid().ToString(),
					Identificacion = item.DatosObligado.Identificacion,
					IdProceso = 1,
					IdUltimoProceso = 1,
					MotivoRechazo = "",
					NumeroResolucion = item.NumeroResolucion,
					Prefijo = item.Prefijo,
					ProcesoFinalizado = 0,
					UltimoProceso = "Recepción - Información del documento.",
					UrlPdf = "",
					UrlXmlUbl = ""
				};

				respuesta.Add(respuesta_tmp);
			}

			return respuesta;

		}





	}
}
