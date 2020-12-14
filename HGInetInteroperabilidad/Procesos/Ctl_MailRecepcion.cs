using HGICtrlUtilidades;
using HGInetInteroperabilidad.Configuracion;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Procesos
{
	public class Ctl_MailRecepcion
	{
		
		public static void Procesar()
		{
			try
			{
				// https://webmail.mifacturaenlinea.com.co/
				// hostname: "mifacturaenlinea.com.co", username: "recepcion.dev@mifacturaenlinea.com.co", password: "gUx&819a#2ge", port: 995, isUseSsl: true

				// obtener los parámetros de configuración para la lectura POP
				string servidor = Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.servidor");
				int puerto = Convert.ToInt32(Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.puerto"));
				string usuario = Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.usuario");
				string clave = Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.clave");
				bool habilitar_ssl = Convert.ToBoolean(Cl_InfoConfiguracionServer.ObtenerAppSettings("pop.ssl"));
				
				Cl_MailPop cliente_pop = null;
				List<PopMessage> mensajes = null;

				// obtener los correos electrónicos
				try
				{
					cliente_pop = new Cl_MailPop(servidor, puerto, usuario, clave, habilitar_ssl);
					mensajes = cliente_pop.Obtener();				
				}
				catch (Exception excepcion)
				{
					string msg = string.Format("Error al obtener los correos electrónicos del servidor POP3");
					throw new ExcepcionHgi(excepcion, HGICtrlUtilidades.NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
				}

				// procesa los correos electrónicos obtenidos
				foreach (PopMessage mensaje in mensajes)
				{
					try
					{	
						/*						 
							-	Tamaño máximo de 2MB
							-	Estructura asunto (separador por carácter punto y coma ; )
								NIT del Facturador Electrónico
								Nombre del Facturador Electrónico
								Número del Documento Electrónico
								Código del tipo de documento
								Nombre comercial del facturador
								Línea de negocio (este último opcional, acuerdo comercial entre las partes)
							-	Extensión archivo ZIP adjunto (Attached Document, PDF, ZIP anexo)

						*/

						// obtiene el tamaño de los adjuntos del correo electrónico
						long tamano = cliente_pop.ObtenerTamanoAdjuntos(mensaje);

						// 0 Bytes
						if (tamano == 0)
							throw new ApplicationException("No se encontró archivo adjunto en el correo electrónico.");

						// 2097152 Bytes
						if (tamano > 2097152)
							throw new ApplicationException("Los archivos adjuntos del correo electrónico supera la capacidad máxima de 2MB.");
							
						// obtiene el asunto del correo electrónico
						string asunto = mensaje.Mensaje.Headers.Subject;

						List<string> asunto_params = asunto.Split(';').ToList();

						if(asunto_params.Count < 5)
							throw new ApplicationException("El correo electrónico no cumple con los parámetros del asunto.");

						// validar y obtener la empresa
						Ctl_Empresa _empresa = new Ctl_Empresa();
						TblEmpresas empresa = _empresa.ValidarInteroperabilidad(asunto_params[0]);

						// valida las extensiones de archivos adjuntos
						List<string> extensiones = new List<string> { "zip" };
						cliente_pop.ValidarExtensionesAdjuntos(mensaje, extensiones);

						// procesar archivo adjunto temporal
						PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;
						string ruta_archivos = string.Format("{0}\\{1}{2}\\", plataforma_datos.RutaDmsFisica, Constantes.RutaInteroperabilidadRecepcion, empresa.StrIdSeguridad);

						ruta_archivos = Directorio.CrearDirectorio(ruta_archivos);

						Guid id_mail = Guid.NewGuid();

						// almacena el correo electrónico temporalmente
						string ruta_mail = cliente_pop.Guardar(mensaje, ruta_archivos, id_mail.ToString());
						
						// almacena los adjuntos del correo electrónico temporalmente
						List<string> rutas_archivos = cliente_pop.GuardarAdjuntos(mensaje, ruta_archivos);

						if(rutas_archivos.Count > 1)
							throw new ApplicationException("Los archivos adjuntos del correo electrónico superan la cantidad permitida.");
						
						// descomprime el zip adjunto
						string ruta_descomprimir = Path.Combine(Path.GetDirectoryName(ruta_mail), Path.GetFileNameWithoutExtension(ruta_mail));

						Ctl_Descomprimir.Procesar(empresa, rutas_archivos[0], ruta_descomprimir);
											

					}
					catch (Exception excepcion)
					{	string msg = string.Format("Error al procesar el correos electrónico");						
					}
				}
			}
			catch (Exception excepcion)
			{
				string msg = string.Format("Error al procesar los correos electrónicos");
				throw new ExcepcionHgi(excepcion, HGICtrlUtilidades.NotificacionCodigo.ERROR_EN_SERVIDOR, msg);
			}
		}
		

	}
}
