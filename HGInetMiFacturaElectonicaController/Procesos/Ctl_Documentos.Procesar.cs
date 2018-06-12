using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{
		/// <summary>
		/// Continúa el procesamiento de varios documentos registrados en la base de datos de acuerdo con el estado actual del mismo
		/// </summary>
		/// <param name="documentos">datos de los documentos en base de datos</param>
		/// <returns>datos de resultado para los documentos</returns>
		public static List<DocumentoRespuesta> Procesar(List<TblDocumentos> documentos)
		{
			List<DocumentoRespuesta> documentos_respuestas = new List<DocumentoRespuesta>();

			foreach (TblDocumentos documento in documentos)
			{
				DocumentoRespuesta item_respuesta = new DocumentoRespuesta();

				// obtiene el proceso actual del documento
				ProcesoEstado proceso_actual = Enumeracion.ParseToEnum<ProcesoEstado>((int)documento.IntIdEstado);

				try
				{   // procesa el documento
					item_respuesta = Procesar(documento);
				}
				catch (Exception excepcion)
				{
					item_respuesta = new DocumentoRespuesta()
					{
						Aceptacion = documento.IntAdquirienteRecibo,
						CodigoRegistro = documento.StrObligadoIdRegistro,
						Cufe = documento.StrCufe,
						DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
						Documento = documento.IntNumero,
						Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
						FechaRecepcion = documento.DatFechaIngreso,
						FechaUltimoProceso = documento.DatFechaActualizaEstado,
						IdDocumento = documento.StrIdSeguridad.ToString(),
						Identificacion = documento.StrEmpresaFacturador,
						IdProceso = proceso_actual.GetHashCode(),
						MotivoRechazo = documento.StrAdquirienteMvoRechazo,
						NumeroResolucion = documento.StrNumResolucion,
						Prefijo = documento.StrPrefijo,
						ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
						UrlPdf = documento.StrUrlArchivoPdf,
						UrlXmlUbl = documento.StrUrlArchivoUbl
					};
				}

				documentos_respuestas.Add(item_respuesta);
			}

			return documentos_respuestas;
		}

		/// <summary>
		/// Continúa el procesamiento de un documento registrado en la base de datos de acuerdo con el estado actual del mismo
		/// </summary>
		/// <param name="documento">datos del documento en base de datos</param>
		/// <returns>datos de resultado para el documento</returns>
		public static DocumentoRespuesta Procesar(TblDocumentos documento)
		{
			// valida que la empresa se encuentre como facturador electrónico
			if (documento.TblEmpresasFacturador.IntHabilitacion == 0)
				throw new ArgumentException(string.Format("No se encuentra habilitado como Facturador Electrónico - {0} {1}", documento.StrEmpresaFacturador, documento.TblEmpresasFacturador.StrRazonSocial));

			// obtiene el proceso actual del documento
			ProcesoEstado proceso_actual = Enumeracion.ParseToEnum<ProcesoEstado>((int)documento.IntIdEstado);

			// valida que el proceso del documento cuente con el XML en UBL físicamente
			if (proceso_actual.GetHashCode() <= ProcesoEstado.AlmacenaXML.GetHashCode())
				throw new ArgumentException(string.Format("No se puede procesar el documento {0} en estado {1} del Facturador Electrónico - {2} {3}", documento.StrIdSeguridad, Enumeracion.GetDescription(proceso_actual), documento.StrEmpresaFacturador, documento.TblEmpresasFacturador.StrRazonSocial));

			// obtiene el tipo de documento
			TipoDocumento tipo_documento = Enumeracion.ParseToEnum<TipoDocumento>(documento.IntDocTipo);

			// genera el estado actual de respuesta del documento
			DocumentoRespuesta respuesta = new DocumentoRespuesta()
			{
				Aceptacion = documento.IntAdquirienteRecibo,
				CodigoRegistro = documento.StrObligadoIdRegistro,
				Cufe = documento.StrCufe,
				DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
				Documento = documento.IntNumero,
				Error = null,
				FechaRecepcion = documento.DatFechaIngreso,
				FechaUltimoProceso = documento.DatFechaActualizaEstado,
				IdDocumento = documento.StrIdSeguridad.ToString(),
				Identificacion = documento.StrEmpresaFacturador,
				IdProceso = proceso_actual.GetHashCode(),
				MotivoRechazo = documento.StrAdquirienteMvoRechazo,
				NumeroResolucion = documento.StrNumResolucion,
				Prefijo = documento.StrPrefijo,
				ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
				UrlPdf = documento.StrUrlArchivoPdf,
				UrlXmlUbl = documento.StrUrlArchivoUbl
			};

			if (respuesta.ProcesoFinalizado == 1)
				return respuesta;

			DateTime fecha_actual = Fecha.GetFecha();

			try
			{
				// representación de datos en objeto
				var documento_obj = (dynamic)null;

				// lee el archivo XML en UBL desde la ruta pública
				XmlTextReader xml_reader = new XmlTextReader(documento.StrUrlArchivoUbl);

				// convierte el objeto de acuerdo con el tipo de documento
				XmlSerializer serializacion = null;

				if (tipo_documento == TipoDocumento.Factura)
				{
					serializacion = new XmlSerializer(typeof(InvoiceType));

					InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

					documento_obj = FacturaXML.Convertir(conversion);
				}
				else if (tipo_documento == TipoDocumento.NotaCredito)
				{
					documento_obj = documento;
				}
				else if (tipo_documento == TipoDocumento.NotaDebito)
				{
					documento_obj = documento;
				}

				// convierte los datos del objeto en texto XML 
				StringBuilder txt_xml = new StringBuilder();
				txt_xml.Append(xml_reader.ReadString());

				// cerrar la lectura del archivo xml
				xml_reader.Close();

				// valida la conversión del objeto
				if (documento_obj == null)
					throw new ArgumentException("No se recibieron datos para realizar el proceso");

				// resolución asociada al documento
				TblEmpresasResoluciones resolucion = documento.TblEmpresasResoluciones;

				// facturador electrónico del documento
				TblEmpresas empresa = documento.TblEmpresasFacturador;

				// carpeta del xml
				string carpeta_xml = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), empresa.StrIdSeguridad.ToString());
				carpeta_xml = string.Format(@"{0}{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

				// carpeta del zip
				string carpeta_zip = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), empresa.StrIdSeguridad.ToString());
				carpeta_zip = string.Format(@"{0}{1}", carpeta_zip, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);


				// información del procesamiento del archivo
				FacturaE_Documento documento_result = new FacturaE_Documento()
				{
					IdSeguridad = Guid.NewGuid(),
					IdSeguridadTercero = empresa.StrIdSeguridad,
					DocumentoTipo = tipo_documento,
					CUFE = documento.StrCufe,
					Documento = documento_obj,
					DocumentoXml = txt_xml,
					RutaArchivosProceso = carpeta_xml,
					RutaArchivosEnvio = carpeta_zip
				};

				// genera el nombre del archivo XML y PDF
				documento_result.NombreXml = NombramientoArchivo.ObtenerXml(documento_obj.Documento.ToString(), documento_obj.DatosObligado.Identificacion, tipo_documento);
				documento_result.NombrePdf = documento_result.NombreXml;

				// genera el nombre del archivo ZIP
				documento_result.NombreZip = NombramientoArchivo.ObtenerZip(documento_obj.Documento.ToString(), documento_obj.DatosObligado.Identificacion, tipo_documento);

				// firma el xml (valida si no ha realizado el envío a la DIAN vuelve a firmar)
				if (respuesta.IdProceso < ProcesoEstado.EnvioZip.GetHashCode())
				{
					respuesta = UblFirmar(documento, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta);
				}

				// comprime el archivo xml firmado
				if (respuesta.IdProceso < ProcesoEstado.EnvioZip.GetHashCode())
				{
					respuesta = UblComprimir(documento, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta);
				}

				// envía el archivo zip con el xml firmado a la DIAN
				if (respuesta.IdProceso < ProcesoEstado.EnvioZip.GetHashCode())
				{
					HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documento, empresa, ref respuesta, ref documento_result);
					ValidarRespuesta(respuesta);
				}


				//Valida estado del documento en la Plataforma de la DIAN
				//if (respuesta.IdProceso == ProcesoEstado.EnvioZip.GetHashCode())
				// xxxxxxxx


				// envía el mail de documentos al adquiriente
				//if (respuesta.IdProceso < ProcesoEstado.EnvioEmailAcuse.GetHashCode())
				// xxxxxxxx


				// envía el mail de acuse de recibo al facturador electrónico
				//if (respuesta.IdProceso < ProcesoEstado.EnvioRespuestaAcuse.GetHashCode())
				// xxxxxxxx

			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException);

				LogExcepcion.Guardar(excepcion);
				// no se controla excepción
			}

			return respuesta;

		}

	}
}
