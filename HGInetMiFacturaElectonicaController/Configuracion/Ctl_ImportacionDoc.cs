using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static HGInetMiFacturaElectonicaController.Configuracion.Ctl_PlanesTransacciones;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_ImportacionDoc : BaseObject<TblImportacionDoc>
	{
		#region Constructores 

		public Ctl_ImportacionDoc() : base(new ModeloAutenticacion()) { }
		public Ctl_ImportacionDoc(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_ImportacionDoc(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		public TblImportacionDoc Crear(TblImportacionDoc doc)
		{
			doc.IdSeguridad = Guid.NewGuid();
			doc = this.Add(doc);

			return doc;
		}

		public TblImportacionDoc Actualizar(TblImportacionDoc documento)
		{

			documento = this.Edit(documento);

			return documento;

		}

		public List<TblImportacionDoc> Obtener(string identificacion, int Operacion)
		{
			List<TblImportacionDoc> datos = null;

			//0 - Documentos emitidos; 1 - Documentos Recibidos
			if (Operacion == 0)
			{
				datos = (from item in context.TblImportacionDoc
						 where (item.StrEmpresaFacturador.Equals(identificacion))
						 orderby item.DatFechaRecepcion descending
						 select item).ToList();
			}
			else
			{
				datos = (from item in context.TblImportacionDoc
						 where item.StrEmpresaAdquiriente.Equals(identificacion)
						 orderby item.DatFechaRecepcion descending
						 select item).ToList();
			}

			return datos;
		}

		public List<TblImportacionDoc> ObtenerDocImportados(List<System.Guid> List_id_seguridad)
		{
			try
			{

				List<TblImportacionDoc> retorno = (from doc in context.TblImportacionDoc
											   where List_id_seguridad.Contains(doc.IdSeguridad)
											   select doc).ToList();


				return retorno;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public void EliminarLista(List<TblImportacionDoc> datos)
		{

			if (datos != null && datos.Count > 0)
			{
				foreach (TblImportacionDoc item in datos)
				{
					this.Delete(item);
				}
			}
		}


		public bool ImportarArchivo(HttpPostedFile File, System.Guid IdSeguridad, int Operacion)
		{
			bool importa_archivo = true;

			string ruta_importacion = string.Empty;

			try
			{
				ruta_importacion = GuardarArchivoExcel(File, IdSeguridad);
			}
			catch (Exception excepcion)
			{
				importa_archivo = false;
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, "Proceso de Importacion de archivo");
			}

			if (!string.IsNullOrEmpty(ruta_importacion))
			{
				//Se obtiene la empresa que esta importanto el archivo
				Ctl_Empresa _empresa = new Ctl_Empresa();
				TblEmpresas empresa = _empresa.Obtener(IdSeguridad).FirstOrDefault();

				//Se elimina datos de esta empresa en tabla
				try
				{
					List<TblImportacionDoc> datos = Obtener(empresa.StrIdentificacion, Operacion);

					EliminarLista(datos);
				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.eliminacion, "Proceso de Eliminación de información en tabla antes de subir el archivo de excel");
					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}

				try
				{
					ImportarExcel(ruta_importacion, empresa.StrIdentificacion, Operacion);
				}
				catch (Exception excepcion)
				{
					importa_archivo = false;
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.eliminacion, "Proceso lectura del archivo de excel y creacion en la tabla");
					throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				}
			}
			else
			{
				importa_archivo = false;
			}

			return importa_archivo;
		}

		public void ImportarExcel(string rutaArchivo, string empresa_importacion, int operacion)
		{

			try
			{

				string StrConexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rutaArchivo + ";Extended Properties='Excel 12.0;HDR=NO;IMEX=1;';";

				string nombreDt = "ImportacionDoc";

				DataTable tbl_temporal = new DataTable();

				try
				{
					using (OleDbConnection objConn = new OleDbConnection(StrConexion))
					{
						objConn.Open();
						OleDbCommand cmd = new OleDbCommand();
						OleDbDataAdapter oleda = new OleDbDataAdapter();
						DataSet ds = new DataSet();
						DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
						string sheetName = string.Empty;

						if (dt != null)
						{
							var tempDataTable = (from dataRow in dt.AsEnumerable()
												 where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
												 select dataRow).CopyToDataTable();
							dt = tempDataTable;
							sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
						}

						cmd.Connection = objConn;
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
						oleda = new OleDbDataAdapter(cmd);
						oleda.Fill(ds, nombreDt);
						tbl_temporal = ds.Tables[nombreDt];
						objConn.Close();
					}
				}
				catch (Exception excepcion)
				{
					RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, "Obteniendo informacion del archivo de excel");
				}

				bool archivo_valido = false;

				if (tbl_temporal.Columns.Count > 0 && tbl_temporal.Columns.Count < 17)
				{
					DataRow fila1 = tbl_temporal.Rows[0];

					for (int i = 0; i < fila1.ItemArray.Count(); i++)
					{
						string campo_mayus = fila1.ItemArray[i].ToString().ToUpper();
						if (campo_mayus.Equals("FOLIO"))
						{
							archivo_valido = true;
							i = fila1.ItemArray.Count();
						}
							

					}
				}

				if (tbl_temporal.Rows.Count > 0 && archivo_valido == true)
				{
					try
					{
						foreach (DataRow item_row in tbl_temporal.Rows)
						{
							//Se valida que no sea la primera fila que es los nombres de los campos
							if (!item_row[2].Equals("Folio"))
							{
								TblImportacionDoc import = new TblImportacionDoc();
								int tipo_doc = 1;
								for (int i = 0; i < item_row.ItemArray.Count(); i++)
								{
									switch (i)
									{
										case 0:
											//Valida
											if (item_row[i].ToString().Contains("débito"))
												tipo_doc = 2;
											if (item_row[i].ToString().Contains("Crédito"))
												tipo_doc = 3;
											else if (item_row[i].ToString().Contains("Application"))
											{
												tipo_doc = 4;
												i = item_row.ItemArray.Count();
											}
												
											import.StrTipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(tipo_doc));
											break;
										case 1:
											import.StrCufe = item_row[i].ToString();
											break;
										case 2:
											import.IntNumero = item_row[i].ToString();
											break;
										case 3:
											import.StrPrefijo = item_row[i].ToString();
											break;
										case 4:
											import.DatFechaEmision = Convert.ToDateTime(item_row[i].ToString());
											break;
										case 5:
											import.DatFechaRecepcion = Convert.ToDateTime(item_row[i].ToString());
											break;
										case 6:
											import.StrEmpresaFacturador = item_row[i].ToString();
											break;
										case 7:
											import.StrNombreFacturador = item_row[i].ToString();
											break;
										case 8:
											import.StrEmpresaAdquiriente = item_row[i].ToString();
											break;
										case 9:
											import.StrNombreAdquiriente = item_row[i].ToString();
											break;
										case 10:
											import.IntVlrIva = Convert.ToDecimal(item_row[i]);
											break;
										case 11:
											import.IntVlrIca = Convert.ToDecimal(item_row[i]);
											break;
										case 12:
											import.IntVlrIPC = Convert.ToDecimal(item_row[i]);
											break;
										case 13:
											import.IntVlrTotal = Convert.ToDecimal(item_row[i]);
											break;
										case 14:
											import.StrEstadoDian = item_row[i].ToString();
											break;
										case 15:
											import.StrGrupo = item_row[i].ToString();
											break;

										default:
											break;
									}
								}

								if (tipo_doc < 4)
								{

									bool consulta_doc = true;

									//0 - Documentos emitidos; 1 - Documentos Recibidos
									if (operacion == 1)
									{
										if (!import.StrEmpresaAdquiriente.Equals(empresa_importacion))
										{
											import.IdProceso = 2;
											import.StrObservaciones = string.Format("Documento con nit de Recepción {0} no puede ingresar a plataforma", import.StrEmpresaAdquiriente);
											consulta_doc = false;
										}
									}
									else
									{
										if (!import.StrEmpresaFacturador.Equals(empresa_importacion))
										{
											import.IdProceso = 2;
											import.StrObservaciones = string.Format("Documento con nit de Emisión {0} no puede ingresar a plataforma", import.StrEmpresaFacturador);
											consulta_doc = false;
										}
									}

									if (consulta_doc == true)
									{
										Ctl_Documento ctl_doc = new Ctl_Documento();

										TblDocumentos doc_bd = ctl_doc.Obtener(import.StrEmpresaFacturador, Convert.ToInt64(import.IntNumero), import.StrPrefijo, tipo_doc);

										if (doc_bd == null)
										{
											import.IdProceso = 1;
											import.StrObservaciones = "Documento No existe en Plataforma";
										}
										else
										{
											import.IdProceso = 2;
											import.StrObservaciones = string.Format("Documento ya existe en nuestra Plataforma, Fecha de ingreso {0}", doc_bd.DatFechaIngreso);
										}
									}

									//Proceso para crear item en la tabla
									try
									{
										Ctl_ImportacionDoc ctl_import = new Ctl_ImportacionDoc();
										ctl_import.Crear(import);
									}
									catch (Exception excepcion)
									{
										RegistroLog.EscribirLog(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.creacion, "Creando informacion del archivo de excel en BD");
									}

								}
								//else
								//{
								//	import.StrObservaciones = "Documento No aplica para importar a Plataforma";
								//}
							}
						}
						
					}
					catch (Exception excepcion)
					{
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.cargando, "Recorriendo informacion del archivo de excel");
					}
				}
				else if (archivo_valido == false)
					throw new ApplicationException("El archivo no cuenta con la información para procesar");

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, "Importacion del archivo de excel");
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		public string GuardarArchivoExcel(HttpPostedFile File, System.Guid IdSeguridad)
		{
			string carpeta_archivo = string.Empty;



			//Se obtiene la empresa que esta importanto el archivo
			Ctl_Empresa _empresa = new Ctl_Empresa();
			TblEmpresas empresa = _empresa.Obtener(IdSeguridad).FirstOrDefault();


			try
			{
				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;
				string nombre_archivo = File.FileName; //string.Format("imp - {0}", Fecha.GetFecha().ToString("yyyy-MM-dd"));

				//Ruta temporal para crear el archivo y validarlo
				carpeta_archivo = string.Format("{0}\\{1}\\{2}\\ImportacionArchivos\\{3}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, empresa.StrIdSeguridad,nombre_archivo);
				//Validamos que 
				if (File != null && File.ContentLength > 0)
				{
					string RutaDirectorio = carpeta_archivo.Replace(string.Format(@"\{0}", nombre_archivo), "");
					//Guardamos el archivo en la ruta temporal
					if (Directorio.ValidarExistenciaArchivo(RutaDirectorio))
					{
						File.SaveAs(carpeta_archivo);
					}
					else
					{

						Directorio.CrearDirectorio(RutaDirectorio);
						File.SaveAs(carpeta_archivo);
					}

				}
				else
				{
					//Generamos excepción si no tenemos datos en el archivo
					throw new ApplicationException(string.Format("Error al guardar el archivo: {0}", "El archivo no contiene información"));
				}

				
				return carpeta_archivo;

			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Archivos, MensajeTipo.Error, MensajeAccion.lectura, "Importacion del archivo de excel");
				throw new ApplicationException(string.Format("Error al importar archivo de excel: {0}", excepcion.Message), excepcion);
			}
		}


		public List<TblImportacionDoc> ProcesarDocImp(List<TblImportacionDoc> lista_docs, int Operacion)
		{
			List<TblImportacionDoc> resultado = new List<TblImportacionDoc>();

			foreach (TblImportacionDoc item in lista_docs)
			{
				Ctl_Documento ctl_doc = new Ctl_Documento();

				TblDocumentos doc_bd = ctl_doc.Obtener(item.StrEmpresaFacturador, Convert.ToInt64(item.IntNumero), item.StrPrefijo, 1);

				if (doc_bd == null)
				{
					string numero_resolucion = string.Empty;

					string empresa_descuenta_plan = string.Empty;

					TblEmpresas empresa = new TblEmpresas();

					//0 - Documentos emitidos; 1 - Documentos Recibidos
					if (Operacion == 1)
					{
						//Se necesita Crear al Facturador
						empresa = CrearEmpresa(item, false);
						numero_resolucion = empresa.TblEmpresasResoluciones.FirstOrDefault(x => x.StrPrefijo == item.StrPrefijo).StrNumResolucion;
						empresa_descuenta_plan = item.StrEmpresaAdquiriente;
					}
					else
					{
						//Se necesita crear el Adquiriente
						empresa = CrearEmpresa(item, true);

						//Obtengo la empresa emisora con su respectiva resolucion.
						TblEmpresas emisor = CrearEmpresa(item, false);
						numero_resolucion = emisor.TblEmpresasResoluciones.FirstOrDefault(x => x.StrPrefijo == item.StrPrefijo).StrNumResolucion;
						empresa_descuenta_plan = item.StrEmpresaFacturador;
					}

					doc_bd = Convertir(item, numero_resolucion);

					Ctl_PlanesTransacciones Planestransacciones = new Ctl_PlanesTransacciones();
					List<ObjPlanEnProceso> ListaPlanes = null;

					//asignacion plan de documentos
					try
					{
						ListaPlanes = Planestransacciones.ObtenerPlanesActivos(empresa_descuenta_plan, 1, TipoDocPlanes.Documento.GetHashCode());
					}
					catch (Exception excepcion)
					{
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, string.Format("Se genera inconsistencias en el proceso de planes. Detalle: {0}", excepcion.Message));
						item.IdProceso = 1;
						item.StrObservaciones = string.Format("Se genera inconsistencias en el proceso de planes. Detalle: {0}", excepcion.Message);
					}

					if (ListaPlanes == null)
					{
						///Validación de alertas y notificaciones
						try
						{
							Ctl_Alertas controlador = new Ctl_Alertas();
							controlador.alertaSinSaldo(empresa_descuenta_plan);
						}
						catch (Exception excepcion)
						{
							LogExcepcion.Guardar(excepcion);
						}

						item.IdProceso = 1;
						item.StrObservaciones = "No se encontró saldo disponible para procesar los documentos";
					}

					doc_bd.StrIdPlanTransaccion = Guid.Parse(ListaPlanes.FirstOrDefault().plan.ToString());
					ListaPlanes.FirstOrDefault().reservado += 1;

					//Guardo el documento en BD
					try
					{
						doc_bd = ctl_doc.Crear(doc_bd);

						//Se hace proceso de conciliacion de planes
						if (ListaPlanes.FirstOrDefault().reservado == 1)
						{
							ListaPlanes.FirstOrDefault().procesado += 1;
							var datos = Planestransacciones.ConciliarPlanProceso(ListaPlanes.FirstOrDefault());
						}
					}

					catch (Exception excepcion)
					{
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
						item.StrObservaciones = string.Format("Error al guardar el documento {0} Detalle: {1}", item.IntNumero, excepcion.Message);
					}

					item.IdProceso = 3;
					item.StrObservaciones = "Se crea Documento Exitosamente";
				}
				else
				{
					item.IdProceso = 2;
					item.StrObservaciones = string.Format("Documento ya existe en nuestra Plataforma, Fecha de ingreso {0}", doc_bd.DatFechaIngreso);
				}

				Actualizar(item);

			}


			return resultado;

		}

		public static TblDocumentos Convertir(TblImportacionDoc documento_obj, string numero_resolucion)
		{
			try
			{
				//Generacion del tracking
				Guid tracking = Guid.NewGuid();

				TblDocumentos tbl_documento = new TblDocumentos();

				tbl_documento.DatFechaIngreso = Fecha.GetFecha();
				tbl_documento.IntDocTipo = 1;
				tbl_documento.IntNumero = Convert.ToInt64(documento_obj.IntNumero);
				tbl_documento.StrPrefijo = (!string.IsNullOrEmpty(documento_obj.StrPrefijo)) ? documento_obj.StrPrefijo : "";
				tbl_documento.DatFechaDocumento = documento_obj.DatFechaEmision;
				tbl_documento.DatFechaVencDocumento = documento_obj.DatFechaEmision;
				tbl_documento.StrObligadoIdRegistro = tracking.ToString();
				tbl_documento.StrNumResolucion = numero_resolucion;
				tbl_documento.StrEmpresaFacturador = documento_obj.StrEmpresaFacturador;
				tbl_documento.StrEmpresaAdquiriente = documento_obj.StrEmpresaAdquiriente;
				tbl_documento.StrCufe = documento_obj.StrCufe;
				tbl_documento.IntVlrTotal = documento_obj.IntVlrTotal;
				tbl_documento.IntValorSubtotal = documento_obj.IntVlrTotal - (documento_obj.IntVlrIva + documento_obj.IntVlrIPC);
				tbl_documento.IntValorNeto = documento_obj.IntVlrTotal - documento_obj.IntVlrIca;
				tbl_documento.StrIdSeguridad = tracking;
				tbl_documento.IntAdquirienteRecibo = 0;
				tbl_documento.IntIdEstado = Convert.ToInt16(ProcesoEstado.EnvioEmailAcuse.GetHashCode());
				tbl_documento.IdCategoriaEstado = Convert.ToInt16(CategoriaEstado.ValidadoDian.GetHashCode());
				tbl_documento.DatFechaActualizaEstado = Fecha.GetFecha();
				tbl_documento.StrIdInteroperabilidad = tracking;
				//tbl_documento.StrUrlArchivoUbl = UrlXmlUbl;
				//tbl_documento.StrUrlArchivoPdf = url_ppal_pdf;
				//tbl_documento.StrUrlAnexo = url_ppal_zip;
				tbl_documento.StrProveedorReceptor = "888888888";
				tbl_documento.StrProveedorEmisor = "888888888";
				tbl_documento.IntVersionDian = 2;


				return tbl_documento;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		public static TblEmpresas CrearEmpresa(TblImportacionDoc documento_obj, bool Adquiriente)
		{


			try
			{
				TblEmpresas Empresa = new TblEmpresas();

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				try
				{
					if (Adquiriente == false)
						Empresa = ctl_empresa.Obtener(documento_obj.StrEmpresaFacturador);
					else
						Empresa = ctl_empresa.Obtener(documento_obj.StrEmpresaAdquiriente);
				}
				catch (Exception ex)
				{
					RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				}
				
				if (Empresa == null)
				{
					Tercero empresa_new = new Tercero();
					empresa_new.Identificacion = documento_obj.StrEmpresaFacturador;
					empresa_new.TipoIdentificacion = 31;
					empresa_new.RazonSocial = documento_obj.StrNombreFacturador;
					empresa_new.NombreComercial = documento_obj.StrNombreFacturador;
					empresa_new.Email = "sincorreo@facturador.com";

					Empresa = ctl_empresa.Crear(empresa_new, Adquiriente);
				}

				//Se crea Resolucion
				TblEmpresasResoluciones resolucion = new TblEmpresasResoluciones();

				Ctl_EmpresaResolucion ctl_resolucion = new Ctl_EmpresaResolucion();

				try
				{
					resolucion = ctl_resolucion.Obtener(documento_obj.StrEmpresaFacturador, "*", documento_obj.StrPrefijo);
				}
				catch (Exception ex)
				{
					//LogExcepcion.Guardar(new Exception(string.Format("Error al Obtener Resolución {0}", ex), ex));
					RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.consulta);
				}

				if (resolucion == null && Adquiriente == false)
				{

					try
					{
						resolucion = Ctl_EmpresaResolucion.Convertir(documento_obj.StrEmpresaFacturador, documento_obj.StrPrefijo, 1, 2, Guid.NewGuid().ToString());
					}
					catch (Exception ex)
					{
						//LogExcepcion.Guardar(new Exception(string.Format("Error al guardar Resolución {0}", ex), ex));
						RegistroLog.EscribirLog(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
					}

					// crea el registro en base de datos
					ctl_resolucion = new Ctl_EmpresaResolucion();
					resolucion = ctl_resolucion.Crear(resolucion);
					Empresa.TblEmpresasResoluciones.Add(resolucion);
				}

				return Empresa;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



	}
}
