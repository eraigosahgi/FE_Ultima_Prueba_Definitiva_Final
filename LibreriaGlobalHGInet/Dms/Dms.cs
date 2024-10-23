using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web;

namespace LibreriaGlobalHGInet
{
	/// <summary>
	/// Clase para la gestión de rutas y archivos
	/// </summary>
	public class Dms
	{

		/// <summary>
		/// Tipos de archivos
		/// </summary>
		public enum DmsTipo
		{
			Login,
			ProductoImagen,
			Documento,
			Caso,
			PlanillaPago,
			TerceroImagen,
			Comprobantes,
			XmlFacturaE,
			FacturaEFirmado,
			PuntosColombiaRequest,
			PuntosColombiaResponse
		}

		#region Rutas físicas

		/// <summary>
		/// Obtiene la ruta física completa de la carpeta principal
		/// Si el directorio base se encuentra vacío retorna la ruta de ejecución del aplicativo
		/// </summary>
		/// <param name="directorio_base">ruta de destino principal</param>
		/// <param name="nit">número de documento</param>
		/// <returns>ruta física del equipo</returns>
		public static string ObtenerCarpetaPrincipal(string directorio_base, string nit)
		{
			string ruta = RecursoDms.CarpetaCliente;

			if (string.IsNullOrWhiteSpace(directorio_base))
				ruta = ruta.Replace("{ruta_app}", Directorio.ObtenerDirectorioRaiz());
			else
			{
				if (!directorio_base.Substring(directorio_base.Length - 1, 1).Equals(@"\"))
					directorio_base = directorio_base + @"\";

				ruta = ruta.Replace("{ruta_app}", directorio_base);
			}
			ruta = ruta.Replace("{nit}", nit);

			return ruta;
		}

		/// <summary>
		/// Obtiene la ruta física completa de la carpeta
		/// </summary>
		/// <param name="directorio_base">ruta de destino principal</param>
		/// <param name="nit">número de documento</param>
		/// <param name="compania">código compañía</param>
		/// <param name="empresa">código empresa</param>
		/// <param name="tipo_archivo">tipo de archivo (enum)</param>
		/// <param name="crear_carpeta">indica si crea la carpeta al no existir</param>
		/// <returns>ruta física del equipo</returns>
		public static string ObtenerCarpeta(string directorio_base, string nit, string compania, int empresa, DmsTipo tipo_archivo, bool crear_carpeta = false)
		{
			// carpeta principal
			string ruta_principal = ObtenerCarpetaPrincipal(directorio_base, nit);

			// nombre de la carpeta con el número de la compañía
			string carpeta_cia = RecursoDms.CarpetaCompania.Replace("{cod_cia}", compania);

			// nombre de la carpeta con el número de la empresa
			string carpeta_emp = RecursoDms.CarpetaEmpresa.Replace("{cod_emp}", empresa.ToString());

			string carpeta_completa = "";

			switch (tipo_archivo)
			{
				case DmsTipo.Login:
					carpeta_completa = string.Format(@"{0}{1}\{2}\{3}\", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaLogin);
					break;
				case DmsTipo.ProductoImagen:
					carpeta_completa = string.Format(@"{0}{1}\{2}\", ruta_principal, carpeta_cia, RecursoDms.CarpetaProductos);
					break;
				case DmsTipo.TerceroImagen:
					carpeta_completa = string.Format(@"{0}{1}\{2}\", ruta_principal, carpeta_cia, RecursoDms.CarpetaTerceros);
					break;
				case DmsTipo.Documento:
					carpeta_completa = string.Format(@"{0}{1}\{2}\{3}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos);
					break;
				case DmsTipo.Caso:
					carpeta_completa = string.Format(@"{0}{1}\{2}\{3}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaCasos);
					break;
				case DmsTipo.PlanillaPago:
					carpeta_completa = string.Format(@"{0}{1}\{2}\{3}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaPlanillaPago);
					break;
				case DmsTipo.Comprobantes:
					carpeta_completa = string.Format(@"{0}{1}\{2}\{3}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaComprobantes);
					break;
				case DmsTipo.XmlFacturaE:
					carpeta_completa = string.Format(@"{0}{1}\{2}\{3}\{4}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos, RecursoDms.CarpetaXmlFacturaE);
					break;
				case DmsTipo.FacturaEFirmado:
					carpeta_completa = string.Format(@"{0}{1}\{2}\{3}\{4}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos, RecursoDms.CarpetaFacturaEFirmado);
					break;
				case DmsTipo.PuntosColombiaRequest:
					carpeta_completa = string.Format(@"{0}{1}\{2}\{3}\PuntosColombia\{4}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos, RecursoDms.CarpetaPuntosColombiaRequest);
					break;
				case DmsTipo.PuntosColombiaResponse:
					carpeta_completa = string.Format(@"{0}{1}\{2}\{3}\PuntosColombia\{4}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos, RecursoDms.CarpetaPuntosColombiaResponse);
					break;
				default:
					return "";
			}

			// si se debe crear el directorio obtenido
			if (crear_carpeta)
				Directorio.CrearDirectorio(carpeta_completa);

			return carpeta_completa;

		}

		/// <summary>
		/// Obtiene la ruta física completa del archivo de tipo Documento
		/// </summary>
		/// <param name="directorio_base">ruta de destino principal</param>
		/// <param name="nit">número de documento</param>
		/// <param name="compania">código compañía</param>
		/// <param name="empresa">código empresa</param>
		/// <param name="cod_transaccion">código de la transacción</param>
		/// <param name="numero_documento">número del documento</param>
		/// <returns>ruta física del archivo</returns>
		public static string ObtenerArchivoDocumento(string directorio_base, string nit, string compania, int empresa, string cod_transaccion, int numero_documento)
		{
			// carpeta principal
			string ruta_principal = ObtenerCarpeta(directorio_base, nit, compania, empresa, DmsTipo.Documento);

			string nombre_archivo = RecursoDms.ArchivoDocumento.Replace("{cod_trans}", cod_transaccion).Replace("{num_doc}", numero_documento.ToString());

			return string.Format(@"{0}\{1}.pdf", ruta_principal, nombre_archivo);
		}

		/// <summary>
		/// Obtiene la ruta física completa del archivo de tipo Caso
		/// </summary>
		/// <param name="directorio_base">ruta de destino principal</param>
		/// <param name="nit">número de documento</param>
		/// <param name="compania">código compañía</param>
		/// <param name="empresa">código empresa</param>
		/// <param name="numero_caso">número del caso</param>
		/// <returns>ruta física del archivo</returns>
		public static string ObtenerArchivoCaso(string directorio_base, string nit, string compania, int empresa, int numero_caso)
		{
			// carpeta principal
			string ruta_principal = ObtenerCarpeta(directorio_base, nit, compania, empresa, DmsTipo.Caso);

			string nombre_archivo = RecursoDms.ArchivoCaso.Replace("{num_caso}", numero_caso.ToString());

			return string.Format(@"{0}\{1}.pdf", ruta_principal, nombre_archivo);
		}

		/// <summary>
		/// Obtiene la ruta física completa del archivo de tipo Caso
		/// </summary>
		/// <param name="nit">número de documento</param>
		/// <param name="compania">código compañía</param>
		/// <param name="empresa">código empresa</param>
		/// <param name="anyo">año</param>
		/// <param name="periodo">periodo</param>
		/// <param name="doc_empleado">número de documento del empleado</param>
		/// <returns>ruta física del archivo</returns>
		public static string ObtenerArchivoPlanillaPago(string directorio_base, string nit, string compania, int empresa, int anyo, int periodo, string doc_empleado)
		{
			// carpeta principal
			string ruta_principal = ObtenerCarpeta(directorio_base, nit, compania, empresa, DmsTipo.Caso);

			string nombre_archivo = RecursoDms.ArchivoPlanillaPago.Replace("{anyo}", anyo.ToString()).Replace("{periodo}", periodo.ToString()).Replace("{doc}", doc_empleado);

			return string.Format(@"{0}\{1}.pdf", ruta_principal, nombre_archivo);
		}

		/// <summary>
		/// Obtiene la ruta física completa del archivo de tipo Comprobante
		/// </summary>
		/// <param name="directorio_base">ruta de destino principal</param>
		/// <param name="nit">número de documento</param>
		/// <param name="compania">código compañía</param>
		/// <param name="empresa">código empresa</param>
		/// <param name="cod_transaccion">código de la transacción</param>
		/// <param name="numero_comprobante">número del comprobante</param>
		/// <returns>ruta física del archivo</returns>
		public static string ObtenerArchivoComprobante(string directorio_base, string nit, string compania, int empresa, string cod_transaccion, int numero_comprobante)
		{
			// carpeta principal
			string ruta_principal = ObtenerCarpeta(directorio_base, nit, compania, empresa, DmsTipo.Comprobantes);

			string nombre_archivo = RecursoDms.ArchivoDocumento.Replace("{cod_trans}", cod_transaccion).Replace("{num_doc}", numero_comprobante.ToString());

			return string.Format(@"{0}\{1}.pdf", ruta_principal, nombre_archivo);
		}

		#endregion


		#region Rutas URL

		/// <summary>
		/// Obtiene la ruta url completa de la carpeta principal
		/// </summary>
		/// <param name="autenticacion">datos de autenticación</param>
		/// <returns>ruta url dms</returns>
		public static string ObtenerUrlPrincipal(string rutaWS, string nit)
		{
			try
			{
				string url_base = "";


				if (string.IsNullOrWhiteSpace(nit))
				{
					throw new ApplicationException("Nit vacio");
				}

				if (string.IsNullOrWhiteSpace(rutaWS))
				{
					if (HttpContext.Current != null)
						//Obtiene la ruta para web
						url_base = string.Format("{0}{1}/", "http://", HttpContext.Current.Request.Url.Authority);
					else
					{
						//Obtiene la ruta para servicios
						List<Uri> cole = OperationContext.Current.Host.BaseAddresses.ToList();
						url_base = string.Format("{0}{1}/", "http://", cole.FirstOrDefault().Authority);
					}
				}
				else
				{
					if (!rutaWS.EndsWith(@"/"))
						rutaWS = string.Format(@"{0}/", rutaWS);


					url_base = rutaWS;
				}
				string ruta = RecursoDms.CarpetaCliente.Replace("\\", "/").Replace(@"\", "/");

				ruta = ruta.Replace("{ruta_app}", url_base);

				ruta = ruta.Replace("{nit}", nit);

				return ruta;
			}
			catch (Exception excep)
			{
				throw new ApplicationException("Error al obtener la url principal", excep);
			}
		}

		/// <summary>
		/// Obtiene la ruta url completa con el tipo de archivo
		/// </summary>
		/// <param name="autenticacion">datos de autenticación</param>
		/// <param name="tipo_archivo">tipo de archivo</param>
		/// <returns>ruta url dms</returns>
		public static string ObtenerUrl(string rutaWS, string nit, string compania, int empresa, DmsTipo tipo_archivo)
		{
			// url principal
			string ruta_principal = ObtenerUrlPrincipal(rutaWS, nit);

			// nombre de la carpeta con el número de la compañía
			string carpeta_cia = RecursoDms.CarpetaCompania.Replace("{cod_cia}", compania);

			// nombre de la carpeta con el número de la empresa
			string carpeta_emp = RecursoDms.CarpetaEmpresa.Replace("{cod_emp}", empresa.ToString());

			switch (tipo_archivo)
			{
				case DmsTipo.Login:
					return string.Format("{0}{1}/{2}/{3}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaLogin);
				case DmsTipo.ProductoImagen:
					return string.Format("{0}{1}/{2}", ruta_principal, carpeta_cia, RecursoDms.CarpetaProductos);
				case DmsTipo.TerceroImagen:
					return string.Format("{0}{1}/{2}", ruta_principal, carpeta_cia, RecursoDms.CarpetaTerceros);
				case DmsTipo.Documento:
					return string.Format("{0}{1}/{2}/{3}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos);
				case DmsTipo.Caso:
					return string.Format("{0}{1}/{2}/{3}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaCasos);
				case DmsTipo.PlanillaPago:
					return string.Format("{0}{1}/{2}/{3}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaPlanillaPago);
				case DmsTipo.Comprobantes:
					return string.Format("{0}{1}/{2}/{3}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaComprobantes);
				case DmsTipo.XmlFacturaE:
					return string.Format(@"{0}{1}/{2}/{3}/{4}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos, RecursoDms.CarpetaXmlFacturaE);
				case DmsTipo.FacturaEFirmado:
					return string.Format(@"{0}{1}/{2}/{3}/{4}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos, RecursoDms.CarpetaFacturaEFirmado);
				case DmsTipo.PuntosColombiaRequest:
					return string.Format(@"{0}{1}/{2}/{3}/PuntosColombia/{4}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos, RecursoDms.CarpetaPuntosColombiaRequest);					
				case DmsTipo.PuntosColombiaResponse:
					return  string.Format(@"{0}{1}/{2}/{3}/PuntosColombia/{4}", ruta_principal, carpeta_cia, carpeta_emp, RecursoDms.CarpetaDocumentos, RecursoDms.CarpetaPuntosColombiaResponse);					
				default:
					return "";
			}
		}

		/// <summary>
		/// Obtiene la ruta url completa del archivo de tipo documento
		/// </summary>
		/// <param name="nit">número de documento</param>
		/// <param name="compania">código compañía</param>
		/// <param name="empresa">código empresa</param>
		/// <param name="cod_transaccion">código de la transacción</param>
		/// <param name="numero_documento">número del documento</param>
		/// <returns>url del archivo</returns>
		public static string ObtenerUrlDocumento(string rutaWS, string nit, string compania, int empresa, string cod_transaccion, int numero_documento, DmsTipo tipo = DmsTipo.Documento)
		{
			// url principal
			string ruta_principal = ObtenerUrl(rutaWS, nit, compania, empresa, DmsTipo.Documento);

			string nombre_archivo = string.Empty;

			nombre_archivo = RecursoDms.ArchivoDocumento.Replace("{cod_trans}", cod_transaccion).Replace("{num_doc}", numero_documento.ToString());

			if (tipo == DmsTipo.XmlFacturaE)
				//Retorna carpeta de xml factura electronica
				return string.Format("{0}/{1}/{2}.xml", ruta_principal, RecursoDms.CarpetaXmlFacturaE, nombre_archivo);
			else if (tipo == DmsTipo.FacturaEFirmado)
				//retorna la carpeta de factura electronica firmada
				return string.Format("{0}/{1}/{2}.xml", ruta_principal, RecursoDms.CarpetaFacturaEFirmado, nombre_archivo);
			//Retorna la carpeta de documentos
			return string.Format("{0}/{1}.pdf", ruta_principal, nombre_archivo);
		}

		/// <summary>
		/// Obtiene la ruta url completa del archivo de tipo caso
		/// </summary>
		/// <param name="nit">número de documento</param>
		/// <param name="compania">código compañía</param>
		/// <param name="empresa">código empresa</param>
		/// <param name="numero_caso">número del caso</param>
		/// <returns>url del archivo</returns>
		public static string ObtenerUrlCaso(string rutaWS, string nit, string compania, int empresa, int numero_caso)
		{
			// url principal
			string ruta_principal = ObtenerUrl(rutaWS, nit, compania, empresa, DmsTipo.Caso);

			string nombre_archivo = string.Empty;

			nombre_archivo = RecursoDms.ArchivoCaso.Replace("{num_caso}", numero_caso.ToString());

			return string.Format("{0}/{1}.pdf", ruta_principal, nombre_archivo);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nit">número de documento</param>
		/// <param name="compania">código compañía</param>
		/// <param name="empresa">código empresa</param>
		/// <param name="anyo">año</param>
		/// <param name="periodo">periodo</param>
		/// <param name="doc_empleado">número de documento del empleado</param>
		/// <returns>url del archivo</returns>
		public static string ObtenerUrlPlanillaPago(string rutaWS, string nit, string compania, int empresa, int anyo, int periodo, string doc_empleado)
		{
			// url principal
			string ruta_principal = ObtenerUrl(rutaWS, nit, compania, empresa, DmsTipo.PlanillaPago);

			string nombre_archivo = RecursoDms.ArchivoPlanillaPago.Replace("{anyo}", anyo.ToString()).Replace("{periodo}", periodo.ToString()).Replace("{doc}", doc_empleado);

			return string.Format("{0}/{1}.pdf", ruta_principal, nombre_archivo);
		}


		/// <summary>
		/// Obtiene la ruta url completa del archivo de tipo comprobante
		/// </summary>
		/// <param name="nit">número de documento</param>
		/// <param name="compania">código compañía</param>
		/// <param name="empresa">código empresa</param>
		/// <param name="cod_transaccion">código de la transacción</param>
		/// <param name="numero_comprobante">número del comprobante</param>
		/// <returns>url del archivo</returns>
		public static string ObtenerUrlComprobante(string rutaWS, string nit, string compania, int empresa, string cod_transaccion, int numero_comprobante)
		{
			// url principal
			string ruta_principal = ObtenerUrl(rutaWS, nit, compania, empresa, DmsTipo.Comprobantes);

			string nombre_archivo = string.Empty;

			nombre_archivo = RecursoDms.ArchivoDocumento.Replace("{cod_trans}", cod_transaccion).Replace("{num_doc}", numero_comprobante.ToString());

			return string.Format("{0}/{1}.pdf", ruta_principal, nombre_archivo);
		}


		#endregion



	}
}
