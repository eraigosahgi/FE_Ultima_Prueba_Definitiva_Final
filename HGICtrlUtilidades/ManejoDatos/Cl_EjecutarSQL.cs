using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HGICtrlUtilidades.ManejoDatos
{
	public class Cl_EjecutarSQL
	{
		/// <summary>
		/// String de conexión gneral
		/// </summary>
		public string StringConnSQL;

		/// <summary>
		/// Objeto de conexion general de la aplicación
		/// </summary>
		public SqlConnection objconexionSQL;
		public SqlCommand commandoSQL;

		/// <summary>
		/// String de conexión OLDB
		/// </summary>
		public string stringconnOLEDB;

		/// <summary>
		/// Objeto conexion OleDB
		/// </summary>
		public OleDbConnection objconexionOLEDB;

		/// <summary>
		/// Adapatador de datos oledb
		/// </summary>
		public SqlDataAdapter da, da2;

		#region Variables Comunes
		/// <summary>
		/// Data set para los informes
		/// </summary>
		public DataSet ds = new DataSet();

		/// <summary>
		/// DataTable
		/// </summary>
		public DataTable dt = new DataTable(), dt2 = new DataTable();
		#endregion

		public struct Campos
		{
			/// <summary>
			/// Posición en la tabla
			/// </summary>
			public int posicion;

			/// <summary>
			/// Nombre del Campo
			/// </summary>
			public string nombre;

			/// <summary>
			///  Tipo de Dato
			/// </summary>
			public string tipodato;

			/// <summary>
			/// Valor por defecto
			/// </summary>
			public string valordefecto;

			/// <summary>
			/// Nombre de la tabla contenedora
			/// </summary>
			public string nombretabla;
		}

		public Campos[] datos;

		public Cl_EjecutarSQL() { }

		/// <summary>
		/// Constructor según tipo de operacion si con archivo de compañias o base de datos
		/// </summary>
		/// <param name="tipo">0=Base de datos Modelo;1=Base de datos sql; 2 =Base de datos access; 3=Base de datos Wap; 4=Base de datos Master</param>
		public Cl_EjecutarSQL(int tipo, string ServidorSQL, string BaseDatosSql, int ModoAut, string UsrSql, string PwdSql, int IdAplicativo = 0, string Usuario = "")
		{
			switch (tipo)
			{
				case 0:
					{
						StringConnSQL = StringConeccionModelo(ServidorSQL, BaseDatosSql, ModoAut, UsrSql, PwdSql);
						objconexionSQL = new SqlConnection();
						objconexionSQL.ConnectionString = StringConnSQL;
						break;
					}
				case 1:
					{
						StringConnSQL = StringConeccionSQL(ServidorSQL, BaseDatosSql, ModoAut, UsrSql, PwdSql, IdAplicativo, Usuario);
						objconexionSQL = new SqlConnection();
						objconexionSQL.ConnectionString = StringConnSQL;
						break;
					}
					/*AMTR case 2:
						{
							stringconnOLEDB = StringConeccionCia(Cl_VariablesGlobales.rutaBaseCompania);
							objconexionOLEDB.ConnectionString = stringconnOLEDB;
							break;
						}
					case 3:
						{
							StringConnSQL = StringConeccionWap();
							objconexionSQL.ConnectionString = StringConnSQL;
							break;
						}
					case 4:
						{
							StringConnSQL = StringConeccionMaster1();
							objconexionSQL.ConnectionString = StringConnSQL;
							break;
						}*/
			}
		}


		/// <summary>
		/// Constructor general de la clase
		/// </summary>
		public Cl_EjecutarSQL(string conexion_sql)
		{
			objconexionSQL = new SqlConnection();
			StringConnSQL = conexion_sql;
			objconexionSQL.ConnectionString = StringConnSQL;
		}


		public bool AbrirConexionSQL(bool PMensaje, string conexion_sql = "")
		{
			try
			{
				CerrarConexionSQL();

				if (objconexionSQL == null)
					objconexionSQL = new SqlConnection();

				if (!string.IsNullOrWhiteSpace(conexion_sql))
					objconexionSQL.ConnectionString = conexion_sql;

				objconexionSQL.Open();

				commandoSQL = new SqlCommand();
				commandoSQL.Connection = objconexionSQL;
				commandoSQL.CommandTimeout = 0;
				commandoSQL.CommandText = "SET dateformat ymd SET LANGUAGE spanish";
				commandoSQL.ExecuteNonQuery();

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}


		public bool CerrarConexionSQL()
		{
			try
			{
				if (this.objconexionSQL != null)
				{
					if (this.objconexionSQL.State.Equals(ConnectionState.Open))
					{
						objconexionSQL.Close();
						//Cl_VariablesGlobales.NroConexionesCerradas += 1;
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex);
			}
		}


		public string EjecutarSql(string Psql, bool PMensaje = false, int PCodError = 0)
		{
			try
			{
				AbrirConexionSQL(true);
				commandoSQL.Connection = objconexionSQL;
				commandoSQL.CommandTimeout = 0;
				commandoSQL.CommandText = Psql;
				commandoSQL.ExecuteNonQuery();
				CerrarConexionSQL();
			}
			catch (Exception ex)
			{
				objconexionSQL.Close();
				throw new ApplicationException(ex.Message, ex);
			}
			return "OK";
		}


		public static string StringConeccionSQL(string ServidorSQL, string BaseDatosSql, int ModoAut, string UsrSql, string PwdSql, int IdAplicativo, string Usuario)
		{
			string strcon = "";
			try
			{
				strcon += "Data Source=";
				strcon += ServidorSQL;
				strcon += ";Initial Catalog=";
				strcon += BaseDatosSql;

				// Modo de autenticacion windows
				if (ModoAut == 0)
					strcon += ";Integrated Security=SSPI";
				else
				{
					strcon += ";User Id=" + UsrSql;
					strcon += ";Password=" + PwdSql;
				}
				// Nombre de la aplicacion que invoca el 
				strcon += ";App=" + Cl_Version.ObtenerNombreAplicacionBaseDatos(IdAplicativo) + "-" + Usuario;

				return strcon;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
		}


		public static string StringConeccionModelo(string ServidorSQL, string BaseDatosSql, int ModoAut, string UsrSql, string PwdSql)
		{
			string strcon = "";
			try
			{
				strcon += "Data Source=";
				strcon += ServidorSQL;
				strcon += ";Initial Catalog=";
				strcon += BaseDatosSql + ";";

				// Modo de autenticacion windows
				if (ModoAut == 0)
					strcon += ";Integrated Security=SSPI";
				else
				{
					strcon += ";User Id=" + UsrSql;
					strcon += ";Password=" + PwdSql + Environment.NewLine;
				}
				// Nombre de la aplicacion que invoca el 
				// strcon += "App=" & Cl_Versiones.NombreAplicacionSQL(Cl_VariablesGlobales.IdAplicativo) & "-" & Cl_VariablesGlobales.Usuario.Usuario & ";"

				return strcon;
			}
			catch (Exception ex)
			{
				return strcon;
			}
		}

		/// <summary>
		/// Sql de varias tablas
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="ntabla"></param>
		/// <returns></returns>
		public DataSet SelectSqlDataSetVariasTablas(string sql, string ntabla)
		{
			try
			{
				AbrirConexionSQL(true);
				commandoSQL.Connection = objconexionSQL;
				commandoSQL.CommandTimeout = 0;
				commandoSQL.CommandText = sql;
				commandoSQL.CommandType = CommandType.Text;
				da = new SqlDataAdapter(commandoSQL);
				da.Fill(ds, ntabla);
				CerrarConexionSQL();

				return ds;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex);
			}
		}

		public string ActualizarDatosDataSet2(string ntabla, bool Pmensaje = false)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				SqlCommandBuilder cmdejecucion = new SqlCommandBuilder(da);
				//ds.AcceptChanges();
				da.Update(ds, ntabla);
				CerrarConexionSQL();
				Cursor.Current = Cursors.Default;
				return "OK";
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
		}


		public object SelectSqlString(string sql, bool Pmensaje, string conexion_sql = "")
		{
			try
			{
				object str;
				AbrirConexionSQL(Pmensaje, conexion_sql);
				commandoSQL.Connection = objconexionSQL;
				commandoSQL.CommandTimeout = 0;
				commandoSQL.CommandText = "SET dateformat ymd " + sql;
				commandoSQL.CommandType = CommandType.Text;
				str = commandoSQL.ExecuteScalar();
				CerrarConexionSQL();
				return str;
			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				CerrarConexionSQL();
				// Cl_Funciones.DesplegarError(ex)
				return "Error";
			}
			catch (Exception ex)
			{
				CerrarConexionSQL();
				// Cl_Funciones.DesplegarError(ex)
				return "";
			}
		}

		public DataTable SelectSql(string sql)
		{
			if (AbrirConexionSQL(true))
			{
				try
				{
					commandoSQL.Connection = objconexionSQL;
					commandoSQL.CommandTimeout = 0;
					commandoSQL.CommandText = "SET dateformat ymd " + Environment.NewLine + sql;
					commandoSQL.CommandType = CommandType.Text;
					commandoSQL.CommandTimeout = 0;
					da = new SqlDataAdapter(commandoSQL);
					dt.Reset();
					da.Fill(dt);
					CerrarConexionSQL();
					return dt;
				}
				catch (Exception ex)
				{
					// ojo 
					//Cl_Funciones.DesplegarError(ex);
					CerrarConexionSQL();
					return dt;
				}
			}
			else
				return dt;
		}

		/// <summary>
		/// Select con SQl
		/// </summary>
		/// <param name="sql">String con sentencia SQL</param>
		/// <param name="ndtaset">Nombre del DataSet</param>
		/// <param name="Pmensaje"></param>
		/// <returns></returns>
		public DataSet SelectSqlDataSet(string sql, string ndtaset, bool Pmensaje = true)
		{
			try
			{
				/*AMTR
				if (Cl_VariablesGlobales.IdAplicativo == 10 | Cl_VariablesGlobales.IdAplicativo == 11)
				{
					Pmensaje = false;
					Cl_VariablesGlobales.BarraProgresoMostrarTiempo = false;
				}*/
				AbrirConexionSQL(true);
				commandoSQL.Connection = objconexionSQL;
				commandoSQL.CommandTimeout = 0;
				commandoSQL.CommandText = sql;
				commandoSQL.CommandType = CommandType.Text;
				da = new SqlDataAdapter(commandoSQL);
				ds.Clear();
				da.Fill(ds, ndtaset);
				LlenarDatos(ndtaset);
				CerrarConexionSQL();
				return ds;
			}
			catch (Exception ex)
			{
				// Se realiza la validación para saber el proceso de donde los estan llamado
				// True=ERP y False=HGINetCrons
				/* AMTR
				if (Cl_VariablesGlobales.BarraProgresoMostrarTiempo)
				{
					Cl_Funciones.PararBarraProgreso();
					if (Pmensaje)
						Cl_Funciones.DesplegarError(ex);
					else
						Cl_Funciones.DesplegarError(ex);
				}*/

				CerrarConexionSQL();
				return ds;
			}
		}

		private void LlenarDatos(string ntabla)
		{
			int nroregistros;
			int i = 0;
			nroregistros = ds.Tables[ntabla].Columns.Count;

			datos = new Campos[nroregistros];
			nroregistros = nroregistros - 1;

			for (i = 0; i <= nroregistros; i++)
			{
				datos[nroregistros].posicion = i + 1;
				datos[nroregistros].nombre = ds.Tables[ntabla].Columns[i].ColumnName;
				datos[nroregistros].tipodato = ds.Tables[ntabla].Columns[i].DataType.Name;
			}
		}


	}
}
