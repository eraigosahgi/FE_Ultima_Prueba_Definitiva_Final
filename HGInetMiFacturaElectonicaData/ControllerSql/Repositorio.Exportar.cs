using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Mail;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{
	/// <summary>
	/// Repositorio.Exportar
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract partial class BaseObject<T> : IBaseObject<T> where T : class
	{
		/// <summary>
		/// envia un correo
		/// </summary>>
		protected virtual void EnviarCorreo(T entity, TipoCorreo tipo_correo, List<string> rutas_adjuntos = null)
		{
		}
	}
}
