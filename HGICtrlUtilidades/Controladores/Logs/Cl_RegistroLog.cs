using HGICtrlUtilidades.Recursos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_RegistroLog
	{
		private static readonly string directorio_logs = string.Format("{0}\\logs\\", RecursoArchivosParametros.DirectorioArchivosApp);

		/// <summary>
		/// Guarda un mensaaje tipo log
		/// </summary>
		public static void EscribirLog(Exception e, MensajeCategoria categoria_mensaje, MensajeTipo tipo_mensaje, MensajeAccion accion_mensaje, string error_custom = "")
		{
			string directorio = string.Empty;

			try
			{
				// Declaracion del objeto que representa el Log XML
				LogArchivo logXml;

				try
				{

					directorio = string.Format("{0}{1}{2}\\", Cl_Directorio.ObtenerDirectorioRaiz(), directorio_logs, Cl_Fecha.GetFecha().ToString(Cl_Fecha.formato_fecha_archivo));

					Cl_Directorio.CrearDirectorio(directorio);

					directorio = string.Format("{0}{1}\\", directorio_logs, Cl_Fecha.GetFecha().ToString(Cl_Fecha.formato_fecha_archivo));

					string ruta_xml = string.Format("{0}{1}.json", directorio, categoria_mensaje);

					// Cargar Archivo XML ya existe
					logXml = Cl_JsonLog.ObtenerLog(ruta_xml);

					// si logXML no existe, trae nulo y por defecto se crea uno nuevo, si no es nulo continua con el que carga
					logXml = logXml ?? new LogArchivo();
				}
				catch (Exception)
				{
					// si ocurre algun error de apertura o carga del XML se crea un archivo nuevo (sobreescribe OJO)
					logXml = new LogArchivo();
				}

				// Estructura del Log XML
				LogClase registroLog = new LogClase();
				LogMensaje mensajeLog = new LogMensaje();

				if (!string.IsNullOrEmpty(error_custom))
				{
					// Construir el detalle de Error con los datos de la excepcion
					LogDetalle detalleTrace = new LogDetalle();
					detalleTrace.Linea = "";
					detalleTrace.Archivo = "";

					// Obtener Metodo en la pila
					detalleTrace.Metodo = error_custom;

					mensajeLog.Detalle.Add(detalleTrace);
				}


				// si hay alguna exection
				if (e != null)
				{
					// obtener pila de llamadas
					StackTrace st = new StackTrace(e, true);

					if (st.FrameCount > 0)
					{
						// obtener el primer stack de la excepción
						StackFrame stack = st.GetFrames().FirstOrDefault();

						if (stack != null)
						{
							// Construir el detalle de Error con los datos de la excepcion
							LogDetalle detalle = new LogDetalle();
							detalle.Linea = stack.GetFileLineNumber().ToString();
							detalle.Archivo = stack.GetFileName();

							// Obtener Metodo en la pila
							MethodBase metodo = stack.GetMethod();

							if (metodo != null)
							{
								detalle.Clase = metodo.DeclaringType.Name;
								detalle.Metodo = metodo.ToString();
								detalle.Modulo = metodo.Module.Name;
							}

							mensajeLog.Detalle.Add(detalle);

							// obtener el último stack de la excepción
							stack = st.GetFrames().LastOrDefault();

							if (stack != null)
							{
								// Construir el detalle de Error con los datos de la excepcion
								detalle = new LogDetalle();
								detalle.Linea = stack.GetFileLineNumber().ToString();
								detalle.Archivo = stack.GetFileName();

								// Obtener Metodo en la pila
								metodo = stack.GetMethod();

								if (metodo != null)
								{
									detalle.Clase = metodo.DeclaringType.Name;
									detalle.Metodo = metodo.ToString();
									detalle.Modulo = metodo.Module.Name;
								}

								mensajeLog.Detalle.Add(detalle);
							}
						}
					}


					//Agregar Mensaje-Detalles al registro
					registroLog.Mensaje = mensajeLog;

					// Fecha y Accion
					registroLog.Fecha = Cl_Fecha.GetFecha();
					registroLog.Categoria = accion_mensaje;

					//Construir mensaje de Exception
					StringBuilder excepcion = new StringBuilder("");
					excepcion.Append(e.Message + "\n" + e.StackTrace + "\n");

					if (e.InnerException != null)
					{
						//Agregar InnerException
						excepcion.Append("Detalle\r\n\n\r");
						excepcion.Append(e.InnerException);

						if (e.InnerException.StackTrace != null)
							excepcion.Append(e.InnerException.StackTrace.ToString());

					}
					registroLog.Mensaje.Excepcion = excepcion.ToString();

					//Agregar Registro
					logXml.Logs.Add(registroLog);



					//Guardar en JSON
					Cl_JsonLog.GuardarObjetoJson(logXml, directorio, categoria_mensaje.ToString() + ".json");
				}
			}
			catch (Exception excepcion)
			{
				string mensaje = excepcion.Message;
			}
		}

		public static void Guardar(string archivo, string carpeta, string mensaje, string mensaje_adicional)
		{
			try
			{
				string filepath = string.Format(@"{0}\logs\{1}\", Cl_Directorio.ObtenerDirectorioRaiz(), carpeta);

				if (!Directory.Exists(filepath))
				{
					Directory.CreateDirectory(filepath);

				}
				filepath = filepath + DateTime.Today.ToString(Cl_Fecha.formato_fecha_java) + "-" + archivo + ".txt";

				if (!File.Exists(filepath))
				{
					File.Create(filepath).Dispose();
				}

				using (StreamWriter sw = File.AppendText(filepath))
				{
					string fecha = Cl_Fecha.GetFecha().ToString(Cl_Fecha.formato_fecha_hora_completa);
					sw.WriteLine(fecha);
					sw.WriteLine(mensaje_adicional);
					sw.WriteLine("\r\n\r\n");
					sw.WriteLine(mensaje);

					sw.Flush();
					sw.Close();

				}

			}
			catch (Exception e)
			{
				e.ToString();
				throw new ApplicationException(e.Message, e.InnerException);
			}
		}

		/// <summary>
		/// Retorna objeto con el detalle del error que se guardara en base de datos
		/// </summary>
		/// <param name="e"></param>
		/// <param name="categoria_mensaje">Categoria</param>
		/// <param name="tipo_mensaje">Tipo</param>
		/// <param name="accion_mensaje">Acción</param>
		/// <param name="error_custom">Mensaje</param>
		/// <returns>LogArchivo</returns>
		public static LogArchivo ConvertirLog(Exception e, MensajeCategoria categoria_mensaje, MensajeTipo tipo_mensaje, MensajeAccion accion_mensaje, string error_custom = "")
		{
			string directorio = string.Empty;

			// Declaracion del objeto que representa el Log XML
			LogArchivo Log = new LogArchivo();

			try
			{
				// Estructura del Log XML
				LogClase registroLog = new LogClase();
				LogMensaje mensajeLog = new LogMensaje();

				if (!string.IsNullOrEmpty(error_custom))
				{
					// Construir el detalle de Error con los datos de la excepcion
					LogDetalle detalleTrace = new LogDetalle();
					detalleTrace.Linea = "";
					detalleTrace.Archivo = "";

					// Obtener Metodo en la pila
					detalleTrace.Metodo = error_custom;

					mensajeLog.Detalle.Add(detalleTrace);
				}


				// si hay alguna exection
				if (e != null)
				{
					// obtener pila de llamadas
					StackTrace st = new StackTrace(e, true);

					if (st.FrameCount > 0)
					{
						// obtener el primer stack de la excepción
						StackFrame stack = st.GetFrames().FirstOrDefault();

						if (stack != null)
						{
							// Construir el detalle de Error con los datos de la excepcion
							LogDetalle detalle = new LogDetalle();
							detalle.Linea = stack.GetFileLineNumber().ToString();
							detalle.Archivo = stack.GetFileName();

							// Obtener Metodo en la pila
							MethodBase metodo = stack.GetMethod();

							if (metodo != null)
							{
								detalle.Clase = metodo.DeclaringType.Name;
								detalle.Metodo = metodo.ToString();
								detalle.Modulo = metodo.Module.Name;
							}

							mensajeLog.Detalle.Add(detalle);

							// obtener el último stack de la excepción
							stack = st.GetFrames().LastOrDefault();

							if (stack != null)
							{
								// Construir el detalle de Error con los datos de la excepcion
								detalle = new LogDetalle();
								detalle.Linea = stack.GetFileLineNumber().ToString();
								detalle.Archivo = stack.GetFileName();

								// Obtener Metodo en la pila
								metodo = stack.GetMethod();

								if (metodo != null)
								{
									detalle.Clase = metodo.DeclaringType.Name;
									detalle.Metodo = metodo.ToString();
									detalle.Modulo = metodo.Module.Name;
								}

								mensajeLog.Detalle.Add(detalle);
							}
						}

					}


					//Agregar Mensaje-Detalles al registro
					registroLog.Mensaje = mensajeLog;

					// Fecha y Accion
					registroLog.Fecha = Cl_Fecha.GetFecha();
					registroLog.Categoria = accion_mensaje;

					//Construir mensaje de Exception
					StringBuilder excepcion = new StringBuilder("");
					excepcion.Append(e.Message + "\n" + e.StackTrace + "\n");

					if (e.InnerException != null)
					{
						//Agregar InnerException
						excepcion.Append("Detalle\r\n\n\r");
						excepcion.Append(e.InnerException);

						if (e.InnerException.StackTrace != null)
							excepcion.Append(e.InnerException.StackTrace.ToString());

					}
					registroLog.Mensaje.Excepcion = excepcion.ToString();

					//Agregar Registro
					Log.Logs.Add(registroLog);

					//Guardar en JSON
					//Json.GuardarObjetoJson(logXml, directorio, categoria_mensaje.ToString() + ".json");

				}
				return Log;
			}

			catch (Exception excepcion)
			{
				string mensaje = excepcion.Message;
				return Log;
			}
		}

	}
}
