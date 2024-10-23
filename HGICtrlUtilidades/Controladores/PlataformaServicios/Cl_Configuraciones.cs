using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_Configuraciones
	{
		/// <summary>
		/// Obtiene las rutas para ejecucion de peticiones en plataforma de servicios
		/// </summary>
		/// <param name="url_plataforma"></param>
		/// <param name="ambiente"></param>
		/// <param name="version"></param>
		/// <returns></returns>
		public string ObtenerUrl(string url_plataforma, AmbientesUrl ambiente, int version)
		{
			try
			{
				string url_retorno = string.Empty;

				Cl_ClienteRest<string> cliente_rest = new Cl_ClienteRest<string>(string.Format("{0}/api/ObtenerUrl?ambiente={1}&version={2}", url_plataforma, ambiente.GetHashCode(), version), 1, "");
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

		/// <summary>
		/// Obtiene los parametros configurados para un año especifico
		/// en plataforma intermedia
		/// </summary>
		/// <param name="url_plataforma">url de plataforma intermedia</param>
		/// <param name="anyo">año de búsqueda</param>
		/// <returns></returns>
		public ObjParametrosAnyo ObtenerPrmAnyoApi(string url_plataforma, int anyo)
		{
			try
			{
				ObjParametrosAnyo parametros_anyo = new ObjParametrosAnyo();

				//Realiza la petición para obtener los parametros del año.
				Cl_ClienteRest<ObjParametrosAnyo> cliente = new Cl_ClienteRest<ObjParametrosAnyo>(string.Format("{0}/Api/ParametrosHGI/ObtenerPrmAnyo?anyo={1}", url_plataforma, anyo), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					parametros_anyo = cliente.GET();
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				return parametros_anyo;
			}
			catch (Exception excepcion)
			{
				Cl_RegistroLog.EscribirLog(new Exception(string.Format("VerificarLicencia: {0}", JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene la información contenida en TblConfiguración desde plataforma intermedia
		/// </summary>
		/// <param name="url_plataforma"></param>
		/// <param name="tipo">tipo de configuración (1:correos - 2:mensajes de texto - 3:envios)</param>
		/// <returns></returns>
		public ObjConfiguracion ObtenerConfig(string url_plataforma, int tipo)
		{
			try
			{
				ObjConfiguracion obj_retorno = new ObjConfiguracion();

				Cl_ClienteRest<ObjConfiguracion> cliente_rest = new Cl_ClienteRest<ObjConfiguracion>(string.Format("{0}/api/ObtenerConfiguracion?tipo={1}", url_plataforma, tipo), 1, "");
				try
				{
					obj_retorno = cliente_rest.GET();
				}
				catch (Exception ex)
				{
					var cod = cliente_rest.CodHttp;
					throw new ApplicationException(ex.Message, ex.InnerException);
				}

				if (obj_retorno == null)
					throw new ApplicationException("Parámetros de configuración inválidos");

				return obj_retorno;
			}
			catch (Exception e)
			{
				throw new ApplicationException(e.Message, e.InnerException);
			}
		}

	}
}
