using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Modelo
{

	public partial class ModeloConexion : DbContext
	{
		/// <summary>
		/// Constructor para la conexión con la base de datos
		/// </summary>
		/// <param name="conexion">conexión al modelo de datos EntityFramework</param>
		public ModeloConexion(string conexion) : base(conexion)
		{
		}

		/// <summary>
		/// Constructor para la conexión con la base de datos
		/// </summary>
		/// <param name="servidor">nombre del servidor de base de datos</param>
		/// <param name="basedatos">nombre de la base de datos</param>
		/// <param name="usuario">nombre de usuario para el acceso</param>
		/// <param name="clave">clave del usuario para el acceso</param>
		public ModeloConexion(string servidor, string basedatos, string usuario = "", string clave = "") : base(GetConnectionString(servidor, basedatos, usuario, clave))
		{
		}

		/// <summary>
		/// Constructor para la conexión con la base de datos
		/// </summary>
		/// <param name="auth">datos de autenticación</param>
		public ModeloConexion(ModeloAutenticacion auth) : base(GetConnectionString(auth.Servidor, auth.Basedatos, auth.Usuario, auth.Clave))
		{
		}


		/// <summary>
		/// Obtiene la conexión con la base de datos de acuerdo con los datos ingresados
		/// </summary>
		/// <param name="servidor"></param>
		/// <param name="basedatos"></param>
		/// <param name="usuario"></param>
		/// <param name="clave"></param>
		/// <returns>datos de conexión con la base de datos</returns>
		public static string GetConnectionString(string servidor, string basedatos, string usuario = "", string clave = "")
		{
			// valida los datos de servidor y base de datos
			if (string.IsNullOrWhiteSpace(servidor) || string.IsNullOrWhiteSpace(basedatos))
				throw new ApplicationException("Error al obtener los datos de conexión con la base de datos.");

			// datos adicionales de la conexión
			string metadataModel = "/Modelo.ModeloDatos.csdl|res://*/Modelo.ModeloDatos.ssdl|res://*/Modelo.ModeloDatos.msl";

			// construye los datos básicos de conexión a SQL Server
			string conexion = string.Format("server={0};database={1};", servidor, basedatos);

			// valida si se ingresaron datos de autenticación
			if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrWhiteSpace(clave))
				conexion = string.Format("{0}uid={1};pwd={2};", conexion, usuario, clave);

			// construye el texto de conexión de SQL Server
			var sqlBuilder = new SqlConnectionStringBuilder(conexion);
			sqlBuilder.ConnectTimeout = 300;
			sqlBuilder.MultipleActiveResultSets = true;
			//sqlBuilder.PersistSecurityInfo = false;

			// valida si se ingresaron o no los datos de autenticación
			if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrWhiteSpace(clave))
				sqlBuilder.IntegratedSecurity = false;
			else
				sqlBuilder.IntegratedSecurity = true;

			// construye los datos de conexión necesarios para Entity Framework
			var efConnection = new EntityConnectionStringBuilder();

			// proveedor de base de datos
			efConnection.Provider = "System.Data.SqlClient";

			// asigna la conexión creada de SQL Server
			efConnection.ProviderConnectionString = sqlBuilder.ConnectionString;

			// asigna datos adicionales de Entity Framework
			efConnection.Metadata = string.Format(@"res://*{0}", metadataModel);

			// devuelve la cadena de conexión de Entity Framework
			return efConnection.ToString();
		}
	}
}