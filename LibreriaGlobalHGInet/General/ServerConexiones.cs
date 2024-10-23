using LibreriaGlobalHGInet.ObjetosComunes.Licenciamiento;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaGlobalHGInet.General
{
	public class ServerConexiones
	{
		/// <summary>
		/// Obtiene las propiedades configuradas para el servidor sql.
		/// </summary>
		/// <param name="servidor">nombre servidor/instancia</param>
		/// <param name="usuario">Usuario de conexión al servidor</param>
		/// <param name="clave">Clave de conexión al servidor</param>
		/// <param name="aplicacion"></param>
		/// <param name="modo_autenticacion"></param>
		/// <returns></returns>
		public List<Propiedad> ObtenerDataServer(string servidor, string usuario = "", string clave = "", string aplicacion = "", bool modo_autenticacion = false)
		{
			try
			{
				List<Propiedad> propiedades_retorno = new List<Propiedad>();

				string string_conexion = string.Empty;

				if (string.IsNullOrWhiteSpace(servidor))
					throw new ApplicationException("Error en la conexión con la base de datos.");

				// construye los datos básicos de conexión a SQL Server
				string_conexion = string.Format("server={0};", servidor);

				//Valida el modo de autenticación (si requiere datos de usuario o captura los de la autenticación de windows)
				if (!modo_autenticacion)
					string_conexion = string.Format("{0}Integrated Security=SSPI;", string_conexion, modo_autenticacion);
				else
				{
					// valida si se ingresaron datos de autenticación
					if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrWhiteSpace(clave))
						string_conexion = string.Format("{0}uid={1};pwd={2};", string_conexion, usuario, clave);
					else
						throw new ApplicationException("Datos del usuario de conexión inválidos.");
				}

				if (!string.IsNullOrWhiteSpace(aplicacion))
					string_conexion = string.Format("{0}App={1};", string_conexion, aplicacion);

				//Realiza la consulta de las propiedades del servidor sql y las añade a un DataSet
				string string_query = "create table #server(ID int,  Name  sysname null, Internal_Value int null, Value nvarchar(512) null) insert #server exec master.dbo.xp_msver select* from #server";
				SqlDataAdapter data_adapter = new SqlDataAdapter(string_query, string_conexion);
				DataSet DataServer = new DataSet();
				data_adapter.Fill(DataServer, "DataServer");

				if (DataServer.Tables != null && DataServer.Tables.Count > 0)
				{
					DataTable dt = DataServer.Tables[0];

					//Construye la lista de propiedades de retorno
					foreach (DataRow row in dt.Rows)
					{
						Propiedad propiedad_servidor = new Propiedad();
						propiedad_servidor.Nombre = row.ItemArray[1].ToString();
						propiedad_servidor.Valor = row.ItemArray[3].ToString();
						propiedades_retorno.Add(propiedad_servidor);
					}

				}
				return propiedades_retorno;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

	}



}
