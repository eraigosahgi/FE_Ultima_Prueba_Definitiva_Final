using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Properties;

namespace HGInetMiFacturaElectonicaData.ControllerSql
{

	public abstract partial class BaseObject<T> : IBaseObject<T> where T : class
	{
		/// <summary>
		/// Cierre de conexión de ejecución de comandos
		/// </summary>
		private int cmd_time_out = 90;

		/// <summary>
		/// Modelo de datos
		/// </summary>
		protected virtual ModeloConexion context { get; set; }

		/// <summary>
		/// Autenticación de base de datos
		/// </summary>
		protected virtual ModeloAutenticacion autenticacion { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public BaseObject()
		{
			DataBaseServer server_bd = HgiConfiguracion.GetConfiguration().DataBaseServer;

			int motor = 0;
			int.TryParse(server_bd.Motor, out motor);

			if (this.context == null)
				setBaseObject(server_bd.Servidor, server_bd.BaseDatos, server_bd.Usuario, server_bd.Clave, motor);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="autenticacion">datos de autenticación con la base de datos</param>
		public BaseObject(ModeloAutenticacion autenticacion)
		{
			if (autenticacion == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "autenticacion", "ModeloAutenticacion"));

			this.autenticacion = autenticacion;

			if (this.context == null)
			{
				this.context = new ModeloConexion(autenticacion.Servidor, autenticacion.Basedatos, autenticacion.Usuario, autenticacion.Clave, autenticacion.Motor);
				this.context.Database.CommandTimeout = this.cmd_time_out;

			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="servidor">nombre del servidor SQL</param>
		/// <param name="basedatos">nombre de la base de datos</param>
		/// <param name="usuario">usuario de acceso a la base de datos</param>
		/// <param name="clave">clave de acceso a la base de datos</param>
		public BaseObject(string servidor, string basedatos, string usuario, string clave, int motor = 0)
		{
			if (string.IsNullOrEmpty(servidor))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "servidor", "String"));
			else if (string.IsNullOrEmpty(basedatos))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "basedatos", "String"));
			else if (string.IsNullOrEmpty(usuario))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "usuario", "String"));
			else if (string.IsNullOrEmpty(clave))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "clave", "String"));

			this.autenticacion = new ModeloAutenticacion(servidor, basedatos, usuario, clave, autenticacion.Motor);

			if (this.context == null)
			{
				this.context = new ModeloConexion(autenticacion.Servidor, autenticacion.Basedatos, autenticacion.Usuario, autenticacion.Clave, autenticacion.Motor);
				this.context.Database.CommandTimeout = this.cmd_time_out;
			}
		}

		/// <summary>
		/// Asigna los datos de conexión al objeto
		/// </summary>
		/// <param name="servidor">nombre del servidor SQL</param>
		/// <param name="basedatos">nombre de la base de datos</param>
		/// <param name="usuario">usuario de acceso a la base de datos</param>
		/// <param name="clave">clave de acceso a la base de datos</param>
		public void setBaseObject(string servidor, string basedatos, string usuario, string clave, int motor)
		{
			if (string.IsNullOrEmpty(servidor))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "servidor", "String"));
			else if (string.IsNullOrEmpty(basedatos))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "basedatos", "String"));
			else if (string.IsNullOrEmpty(usuario))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "usuario", "String"));
			else if (string.IsNullOrEmpty(clave))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "clave", "String"));

			this.autenticacion = new ModeloAutenticacion(servidor, basedatos, usuario, clave, motor);

			this.context = new ModeloConexion(autenticacion.Servidor, autenticacion.Basedatos, autenticacion.Usuario, autenticacion.Clave, autenticacion.Motor);
			this.context.Database.CommandTimeout = this.cmd_time_out;

		}


		/// <summary>
		/// Cerrar la conexión con el modelo de datos
		/// </summary>
		public void Close()
		{
			this.context.Dispose();
			this.context = null;
			this.context = new ModeloConexion();
		}

		/// <summary>
		/// Desenlazar el objeto de datos del modelo
		/// </summary>
		/// <param name="entity"></param>
		public void Detach(T entity)
		{
			this.context.Detach(entity);
		}

	}
}