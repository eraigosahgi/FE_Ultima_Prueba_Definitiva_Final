using HGInetFeAPI.ServicioFactura;
using HGInetMiFacturaElectonicaController.Auditorias;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMonitorServController.Properties;
using System.Diagnostics;
using System.Globalization;
using HGInetMonitorServController.Utilitarios;

namespace HGInetMonitorServController.Procesos
{
	public class Ctl_Documento
	{

		public async Task SondaEnviarPeticion()
		{
			try
			{
				var Tarea = TareaEnviarPeticion();
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}

		}

		public async Task TareaEnviarPeticion()
		{
			await Task.Factory.StartNew(() =>
			{				
				procesar();
			});
		}

		public void procesar()
		{

			Ctl_EnvioCorreo correo = new Ctl_EnvioCorreo();
			string url_plataforma_intermedia = Ctl_Utilidades.ObtenerAppSettings("RutaPlataformaServicios").Replace("#","");

			string identificacion_facturador = Ctl_Utilidades.ObtenerAppSettings("identificacion_facturador");
			string serial = Ctl_Utilidades.ObtenerAppSettings("serial");

			bool respuesta_ruta = false;

			string url_plataforma_hgi = string.Empty;
			string url_plataforma_serv = string.Empty;

			//Valido respuesta de plataforma intermedia clouservices y cloudservices2
			try
			{
				url_plataforma_hgi = ObtenerUrl(url_plataforma_intermedia, 1, 1);
				respuesta_ruta = true;

			}
			catch (Exception e)
			{
				//Ctl_Log.Guardar(e, MensajeCategoria.Conexion, MensajeTipo.Error, MensajeAccion.envio);
				correo.EnviarAlerta("Ruta cloudservices no da respuesta");
				RegistroLog.EscribirLog(e, MensajeCategoria.Conexion, MensajeTipo.Error, MensajeAccion.envio, "Ruta cloudservices no da respuesta");

			}


			try
			{
				url_plataforma_intermedia = Ctl_Utilidades.ObtenerAppSettings("RutaPlataformaServicios").Replace("#", "2"); ;
				url_plataforma_hgi = ObtenerUrl(url_plataforma_intermedia, 1, 1);
				respuesta_ruta = true;

			}
			catch (Exception excepcion)
			{
				//Ctl_Log.Guardar(excepcion, MensajeCategoria.Conexion, MensajeTipo.Error, MensajeAccion.envio);
				correo.EnviarAlerta("Ruta cloudservices2 no da respuesta");
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Conexion, MensajeTipo.Error, MensajeAccion.envio, "Ruta cloudservices2 no da respuesta");
				//throw new ApplicationException("No hay respuesta de ninguna de las plataformas intermedias");
			}

			//Se valida que el otro metodo web para rutas de integradores este funcionando
			if (respuesta_ruta == true)
			{
				try
				{
					url_plataforma_serv = ObtenerServidor(url_plataforma_intermedia, 1, identificacion_facturador);
				}
				catch (Exception e)
				{
					correo.EnviarAlerta("No hay respuesta para la Ruta de integradores");
					RegistroLog.EscribirLog(e, MensajeCategoria.Conexion, MensajeTipo.Error, MensajeAccion.envio, "No hay respuesta para la Ruta de integradores");
				}
			}


			#region Crear Archivo json no funcional
			/*string directorio_objetos = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), Constantes.CarpetaObjetoJson);
			string ruta_archivo_envio = string.Format(@"{0}\Doc-Plantilla.json", directorio_objetos);

			//Valido si existe el archivo y su no lo creo con la informacion que tengo
			if (!Archivo.ValidarExistencia(ruta_archivo_envio))
			{
				Directorio.CrearDirectorio(directorio_objetos);
				string objeto = Constantes.ObjetoJson;
				System.IO.File.WriteAllText(ruta_archivo_envio, Newtonsoft.Json.JsonConvert.SerializeObject(objeto));
			} */
			#endregion

			bool ruta_hgi = true;

			string objeto = Constantes.ObjetoJson;

			int sitios_disponibles = 26;

			for (int i = 21; i <= sitios_disponibles; i++)
			{

				#region Leyendo archivo no funcional
				//string ruta_archivo_envio = string.Format(@"{0}\\Doc-{1}", directorio_objetos, i);
				//Se lee un archivo json y se convierte a objeto Factura para pruebas
				//string objeto = System.IO.File.ReadAllText(@"E:\Desarrollo\jzea\Proyectos\HGInetMiFacturaElectronica\Codigo\HGInetMiFacturaElectronicaWeb\dms\Debug\811021438-SETP-990000005.json").ToString();
				/*
				try
				{
					objeto = System.IO.File.ReadAllText(ruta_archivo_envio).ToString();
				}
				catch (Exception)
				{
					objeto = Constantes.ObjetoJson;
				}*/ 
				#endregion

				Factura objeto_factura = new Factura();
				try
				{

					objeto_factura = JsonConvert.DeserializeObject<Factura>(objeto);

				}
				catch (Exception e)
				{
					correo.EnviarAlerta(string.Format("Error al deserializar objeto factura - Excepcion :{0}", e.Message));
					RegistroLog.EscribirLog(e, MensajeCategoria.Conexion, MensajeTipo.Error, MensajeAccion.envio, "Error al deserializar objeto factura");
				}


				objeto_factura.Documento = i;
				objeto_factura.Fecha = Fecha.GetFecha();
				objeto_factura.FechaVence = Fecha.GetFecha();
				objeto_factura.TipoOperacion = 50;

				string ruta_envio = string.Empty;

				if (ruta_hgi)
				{
					ruta_envio = Constantes.RutaPlataformaHgi.Replace("#", i.ToString());
				}
				else
				{
					ruta_envio = Constantes.RutaPlataformaIntegrador.Replace("#", i.ToString());
				}


				List<HGInetFeAPI.ServicioFactura.Factura> Lista_documentos = new List<HGInetFeAPI.ServicioFactura.Factura>();
				List<HGInetFeAPI.ServicioFactura.DocumentoRespuesta> respuesta_Fe = new List<HGInetFeAPI.ServicioFactura.DocumentoRespuesta>();

				//Logica del envio y la respuesta
				try
				{
					DateTime fecha_inicio = Fecha.GetFecha();
					DateTime fecha_fin = Fecha.GetFecha();
					Lista_documentos.Add(objeto_factura);

					Stopwatch stopWatch = new Stopwatch();
					stopWatch.Start();
					try
					{
						respuesta_Fe = HGInetFeAPI.Ctl_Factura.Enviar(ruta_envio, serial, identificacion_facturador, Lista_documentos, false);
						
					}
					catch (Exception excepcion)
					{
						correo.EnviarAlerta(string.Format("Se presentó inconsistencia probando la ruta: {0} - Excepcion :{1}", ruta_envio, excepcion.Message));
						throw new ApplicationException(string.Format("Se presentó inconsistencia probando la ruta: {0} - Excepcion :{1}", ruta_envio, excepcion.Message), excepcion);
					}
					stopWatch.Stop();

					TimeSpan tiempo_limite = new TimeSpan(0, 10, 0);

					if (stopWatch.Elapsed.Minutes > tiempo_limite.Minutes)
					{
						correo.EnviarAlerta(string.Format("La respuesta del sitio {0} presento una demora de {1} minutos", ruta_envio, stopWatch.Elapsed.Minutes));
						throw new ApplicationException(string.Format("La respuesta del sitio {0} presento una demora de {1} minutos", ruta_envio, stopWatch.Elapsed.Minutes));
					}

					if (respuesta_Fe[0].Error != null && !respuesta_Fe[0].Error.Mensaje.Contains("proceso completo"))
					{
						correo.EnviarAlerta(string.Format("Respuesta de procesamiento de un documento en la ruta: {0}. Mensaje de error: {1}", ruta_envio, respuesta_Fe[0].Error.Mensaje));
						throw new ApplicationException(string.Format("Respuesta de procesamiento de un documento en la ruta: {0}. Mensaje de error: {1}", ruta_envio, respuesta_Fe[0].Error.Mensaje));
					}

				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.Conexion, MensajeTipo.Error, MensajeAccion.envio, string.Format("Se presentó inconsistencia probando la ruta: {0} - Excepcion: {1}", ruta_envio, excepcion.Message));
				}

				if (i == sitios_disponibles && ruta_hgi == true)
				{
					i = 20;
					ruta_hgi = false;
				}

			}

			RegistroLog.EscribirLog(new ApplicationException("Finaliza proceso de monitoreo."), MensajeCategoria.Sonda, MensajeTipo.Ninguno, MensajeAccion.ninguna);

		}

		public string ObtenerUrl(string url_plataforma, int ambiente, int version)
		{
			try
			{
				string url_retorno = string.Empty;

				LibreriaGlobalHGInet.Peticiones.ClienteRest<string> cliente_rest = new LibreriaGlobalHGInet.Peticiones.ClienteRest<string>(string.Format("{0}/api/ObtenerUrl?ambiente={1}&version={2}", url_plataforma, ambiente, version), 1, "");
				try
				{
					url_retorno = cliente_rest.GET();
				}
				catch (Exception ex)
				{
					var cod = cliente_rest.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				if (string.IsNullOrWhiteSpace(url_retorno))
					throw new ApplicationException("Ruta principal de licencia vacía.");

				return url_retorno;
			}
			catch (Exception e)
			{
				throw new ApplicationException(e.Message, e.InnerException);
			}
		}

		public string ObtenerServidor(string url_plataforma, int ambiente, string identificacion_empresa)
		{
			try
			{
				string url_retorno = string.Empty;

				LibreriaGlobalHGInet.Peticiones.ClienteRest<string> cliente_rest = new LibreriaGlobalHGInet.Peticiones.ClienteRest<string>(string.Format("{0}/api/facturae/ObtenerServidorFE?ambiente={1}&version={2}&identificacion_empresa={3}", url_plataforma, ambiente,1, identificacion_empresa), 1, "");
				try
				{
					url_retorno = cliente_rest.GET();
				}
				catch (Exception ex)
				{
					var cod = cliente_rest.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				if (string.IsNullOrWhiteSpace(url_retorno))
					throw new ApplicationException("Ruta principal de licencia vacía.");

				return url_retorno;
			}
			catch (Exception e)
			{
				throw new ApplicationException(e.Message, e.InnerException);
			}
		}

	}
}
