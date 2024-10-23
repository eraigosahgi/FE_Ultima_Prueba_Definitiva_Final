using LibreriaGlobalHGInet.ObjetosComunes.ParametrosHGI;
using LibreriaGlobalHGInet.Peticiones;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.HgiNet.Controladores
{
	public class Ctl_ObtenerParametros
	{

		public ObjParametrosAnyo ObtenerPrmAnyoApi(string url_plataforma, int anyo)
		{
			try
			{
				ObjParametrosAnyo parametros_anyo = new ObjParametrosAnyo();


				//Realiza la petición para obtener los parametros del año.
				ClienteRest<ObjParametrosAnyo> cliente = new ClienteRest<ObjParametrosAnyo>(string.Format("{0}/Api/ParametrosHGI/ObtenerPrmAnyo?anyo={1}", url_plataforma, anyo), TipoContenido.Applicationjson.GetHashCode(), "");
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
				RegistroLog.RegistroLog.EscribirLog(new Exception(string.Format("VerificarLicencia: {0}", JsonConvert.SerializeObject(excepcion))), MensajeCategoria.Licencia, MensajeTipo.Ninguno, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
