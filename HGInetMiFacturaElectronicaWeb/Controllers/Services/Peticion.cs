using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class Peticion
	{
		/// <summary>
		/// Valida la DataKey.
		/// </summary>
		/// <param name="DataKey"></param>
		/// <param name="identificacion_obligado"></param>
		public static void Validar(string DataKey, string identificacion_obligado)
		{
			try
			{
				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				//Válida que la key sea correcta.
				ctl_empresa.Validar(DataKey, identificacion_obligado);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}



		/// <summary>
		/// Almacena la solicitud (petición) wcf en un XML
		/// </summary>	
		/// <param name="contexto">solicitud</param>
		/// <param name="nombre_servicio_web">nombre de la solicitud</param>
		/// <param name="carpeta_nit">identificación para la carpeta</param>
		/// <param name="registro">texto para identificar el registro en el nombre del archivo</param>
		public static void ToXml(RequestContext contexto, string nombre_servicio_web, string carpeta_nit, string registro)
		{
			try
			{
				// Initialize soap request XML
				System.Xml.XmlDocument xmlSoapRequest = new System.Xml.XmlDocument();

				string mensaje = contexto.RequestMessage.ToString();

				xmlSoapRequest.LoadXml(mensaje);

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// carpeta del xml
				string ruta_carpeta = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, carpeta_nit);
				ruta_carpeta = string.Format(@"{0}{1}\\{2}\\", ruta_carpeta, "ws_logs", nombre_servicio_web);

				// valida la existencia de la carpeta
				ruta_carpeta = Directorio.CrearDirectorio(ruta_carpeta);

				string nombre_archivo = string.Format("{0}-{1}.xml", Fecha.GetFecha().ToString(Fecha.formato_fecha_hora_archivo), registro);

				Xml.GuardarXml(xmlSoapRequest, ruta_carpeta, nombre_archivo);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		#region GuardarPetición
		/// <summary>
		/// Guarda los datos  de las peticiones
		/// </summary>				
		public static async Task GuardarPeticionAsync(params string[] args)
		{
			try
			{
				var Tarea = TareaGuardarPeticionAsync(args);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{				
			}
		}

		public static async Task TareaGuardarPeticionAsync(params string[] args)
		{
			await Task.Factory.StartNew(() =>
			{
				string ruta_directorio = "logs\\ws";
				ruta_directorio = AppDomain.CurrentDomain.BaseDirectory + ruta_directorio;

				if (!Directorio.ValidarExistenciaArchivo(ruta_directorio))
					Directorio.CrearDirectorio(ruta_directorio);


				string peticion = "";
				foreach (var item in args)
				{
					peticion += string.Format("{0};", item);
				}

				string ruta = string.Format("{0}\\{1}.txt", ruta_directorio, Fecha.GetFecha().ToString("yyyyMMdd"));

				using (StreamWriter outputFile = new StreamWriter(Path.Combine(ruta), true))
				{
					outputFile.WriteLine(peticion);
				}

			});
		}
		#endregion

	}
}