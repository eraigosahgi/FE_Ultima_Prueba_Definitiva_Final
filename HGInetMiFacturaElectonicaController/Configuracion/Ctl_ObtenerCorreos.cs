using HGInetDIANServicios;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Properties;
using HGInetMiFacturaElectonicaData.ControllerSql;
using LibreriaGlobalHGInet.Funciones;
using Newtonsoft.Json;
using Microsoft.VisualBasic.FileIO;
using System.Data.SqlClient;
using HGInetFirmaDigital;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_ObtenerCorreos : BaseObject<TblCorreosRecepcion>
	{
		#region Constructores 

		public Ctl_ObtenerCorreos() : base(new ModeloAutenticacion()) { }
		public Ctl_ObtenerCorreos(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_ObtenerCorreos(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		public void ObtenerCorreos()
		{
			MensajeCategoria log_categoria = MensajeCategoria.Certificado;
			MensajeAccion log_accion = MensajeAccion.lectura;

			try
			{
				// obtiene los datos de prueba del proveedor tecnológico de la DIAN
				DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

				CertificadoDigital certificado = HgiConfiguracion.GetConfiguration().CertificadoDigitalData;

				// información del certificado digital
				string ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), certificado.RutaLocal);

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del xml
				string ruta_archivo = string.Format("{0}\\{1}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica);

				//obtengo los correos del servicio de la DIAN convierto el el base64 y guardo el archivo
				log_categoria = MensajeCategoria.ServicioDian;
				log_accion = MensajeAccion.consulta;
				ruta_archivo = Ctl_CorreosRecepcionDoc.ObtenerCorreos(ruta_certificado, certificado.Clave, data_dian.UrlServicioWeb, plataforma_datos.RutaDmsFisica);

				DataTable dt = new DataTable();

				//Convierto el archivo en Datatable
				log_categoria = MensajeCategoria.Convertir;
				log_accion = MensajeAccion.creacion;
				ProcesarCsv(ruta_archivo, ',');
				EliminarVacios();
				
			}
			catch (Exception exec)
			{
				Ctl_Log.Guardar(exec, log_categoria, MensajeTipo.Error, log_accion);
				throw new ApplicationException("Error obteniendo correos", exec);
			}
		}

		public void ProcesarCsv(string path, char separator)
		{
			var createdCount = 0;

			try
			{
				using (var textFieldParser = new TextFieldParser(path))
				{
					textFieldParser.TextFieldType = FieldType.Delimited;
					textFieldParser.Delimiters = new[] { separator.ToString() };
					textFieldParser.HasFieldsEnclosedInQuotes = true;

					var dataTable = new DataTable("CorreosDian");
					dataTable.Columns.Add("StrIdentificacion");
					dataTable.Columns.Add("StrMailRegistrado");
					dataTable.Columns.Add("DatFechaRegistro");

					using (var sqlConnection = new SqlConnection(context.Database.Connection.ConnectionString.ToString()))
					{
						sqlConnection.Open();

						// Elimina los datos de la tabla
						using (var sqlCommand = new SqlCommand(@"TRUNCATE TABLE TblCorreosRecepcion", sqlConnection))
						{
							sqlCommand.ExecuteNonQuery();
						}

						var sqlBulkCopy = new SqlBulkCopy(sqlConnection)
						{
							DestinationTableName = "TblCorreosRecepcion"
						};

						sqlBulkCopy.ColumnMappings.Add("StrIdentificacion", "StrIdentificacion");
						sqlBulkCopy.ColumnMappings.Add("StrMailRegistrado", "StrMailRegistrado");
						sqlBulkCopy.ColumnMappings.Add("DatFechaRegistro", "DatFechaRegistro");

						while (!textFieldParser.EndOfData)
						{
							dataTable.Rows.Add(textFieldParser.ReadFields());

							createdCount++;

							if (createdCount > 1000)
							{
								InsertDataTable(sqlBulkCopy, sqlConnection, dataTable);

								dataTable.Clear();
								createdCount = 0;
							}
						}
						InsertDataTable(sqlBulkCopy, sqlConnection, dataTable);

						sqlConnection.Close();
					}
				}
			}
			catch (Exception exec)
			{
				MensajeCategoria log_categoria = MensajeCategoria.Convertir;
				MensajeAccion log_accion = MensajeAccion.creacion;
				Ctl_Log.Guardar(exec, log_categoria, MensajeTipo.Error, log_accion);
				throw new ApplicationException("Error al almacenar los datos de correos electrónicos de la DIAN", exec);
			}
		}

		protected void InsertDataTable(SqlBulkCopy sqlBulkCopy, SqlConnection sqlConnection, DataTable dataTable)
		{
			sqlBulkCopy.WriteToServer(dataTable);

			dataTable.Rows.Clear();
		}


		/// <summary>
		/// Sonda para obtener los correos registrados en la DIAN y actualizarlo en la BD
		/// </summary>
		/// <returns></returns>
		public async Task SondaObtenerCorreosDian()
		{
			try
			{
				var Tarea = TareaObtenerCorreosDian();
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.actualizacion);
			}

		}

		public async Task TareaObtenerCorreosDian()
		{
			await Task.Factory.StartNew(() =>
			{

				ObtenerCorreos();

			});
		}

		/// <summary>
		/// obtiene los datos del archivo y los retorna en un datatable
		/// </summary>
		public static DataTable ObtenerDatosExcel2(string path, char separator)
		{
			MensajeCategoria log_categoria = MensajeCategoria.Convertir;
			MensajeAccion log_accion = MensajeAccion.creacion;

			DataTable dt = new DataTable();
			dt.Columns.Add("StrIdentificacion");
			dt.Columns.Add("StrMailRegistrado");
			dt.Columns.Add("DatFechaRegistro");

			try
			{
				using (StreamReader readFile = new StreamReader(path))
				{
					string line;
					string[] rows;

					//line = readFile.ReadLine().Replace('"', ' ').Trim();
					//rows = line.Split(separator);

					//foreach (string row in rows)
					//{
					//	string nrow = "";//row.Replace('"', ' ').Trim();
					//	dt.Columns.Add(nrow);
					//}

					while ((line = readFile.ReadLine()) != null)
					{
						string nline = line.Replace('"', ' ').Trim();
						rows = nline.Split(separator);
						dt.Rows.Add(rows);
					}
				}
			}
			catch (Exception exec)
			{
				Ctl_Log.Guardar(exec, log_categoria, MensajeTipo.Error, log_accion);
				throw new ApplicationException("Error al  tratar de leer el excel", exec);
			}

			return dt;
		}

		/// <summary>
		/// Proceso para llenar la bd desde un datatable
		/// </summary>
		/// <param name="dtcorreos"></param>
		public void Crear(DataTable dtcorreos)
		{
			MensajeCategoria log_categoria = MensajeCategoria.BaseDatos;
			MensajeAccion log_accion = MensajeAccion.creacion;
			try
			{

				if (dtcorreos == null)
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "respuesta", "TblDocumentos"));

				for (int j = 0; j < dtcorreos.Rows.Count; j++)
				{
					TblCorreosRecepcion correo = new TblCorreosRecepcion();

					correo.StrIdentificacion = dtcorreos.Rows[j]["StrIdentificacion"].ToString().Trim();
					correo.StrMailRegistrado = dtcorreos.Rows[j]["StrMailRegistrado"].ToString().Trim();
					correo.DatFechaRegistro = Convert.ToDateTime(dtcorreos.Rows[j]["DatFechaRegistro"].ToString());

					try
					{
						if (correo.StrIdentificacion.Equals(Constantes.NitResolucionconPrefijo))
						{
							PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

							// ruta física de la carpeta
							string carpeta_correo_Dian = string.Format("{0}\\{1}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaDocumentosDebug);

							// valida la existencia de la carpeta
							carpeta_correo_Dian = Directorio.CrearDirectorio(carpeta_correo_Dian);

							// nombre del archivo
							string archivo_debug = string.Format(@"CorreoDian -{0}-{1}-{2}.json", correo.StrIdentificacion, correo.StrMailRegistrado, Fecha.GetFecha().Minute);

							string ruta_archivo = string.Format("{0}\\{1}", carpeta_correo_Dian, archivo_debug);

							// almacena el objeto en archivo json
							File.WriteAllText(ruta_archivo, JsonConvert.SerializeObject(correo));
						}
					}
					catch (Exception e)
					{

						string ex = e.Message;
					}

					Crear(correo);
				}
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, log_categoria, MensajeTipo.Error, log_accion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		public void Crear(TblCorreosRecepcion correo)
		{
			this.Add(correo);

		}

		public void EliminarData()
		{
			this.DeleteAll("TblCorreosRecepcion");
		}

		public string Obtener(string identificacion)
		{
			string respuesta = string.Empty;
			var objeto = (from item in context.TblCorreosRecepcion
						  where item.StrIdentificacion.Equals(identificacion)
						  select item).FirstOrDefault();

			if (objeto != null)
			{
				respuesta = objeto.StrMailRegistrado;
			}

			return respuesta;
		}

		public void EliminarVacios()
		{
			string query = "delete from TblCorreosRecepcion WHERE StrMailRegistrado = 'NULL'";

			var datos = context.Database.ExecuteSqlCommand(query);

		}


	}
}
