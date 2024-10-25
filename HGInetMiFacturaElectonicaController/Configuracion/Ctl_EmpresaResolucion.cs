using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetDIANServicios.DianResolucion;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Peticiones;
using LibreriaGlobalHGInet.ObjetosComunes.PagosEnLinea;
using HGInetMiFacturaElectonicaController.Auditorias;
using LibreriaGlobalHGInet.RegistroLog;
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectonicaData.Enumerables;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_EmpresaResolucion : BaseObject<TblEmpresasResoluciones>
	{
		#region Constructores 

		public Ctl_EmpresaResolucion() : base(new ModeloAutenticacion()) { }
		public Ctl_EmpresaResolucion(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_EmpresaResolucion(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		#region Obtener

		/// <summary>
		/// Valida si la resolucion maneja pagos parciales
		/// </summary>
		/// <returns></returns>
		public int ManejaPagosParciales(string numero_resolucion, string documento_empresa)
		{

			try
			{
				//Validamos si la resoluciona maneja pagos parciales
				var datos = (from item in context.TblEmpresasResoluciones
							 where (item.StrNumResolucion.Equals(numero_resolucion))
							 && (item.StrEmpresa.Equals(documento_empresa))
							 select item).FirstOrDefault();

				//Si no existe resolucion para esa empresa, entonces retornamos cero(0)
				if (datos != null)
				{
					//Si la resolucion maneja la configuracion del pago, entonces retornamos esa configurracion de pagos parciales
					if (!string.IsNullOrEmpty(datos.ComercioConfigId.ToString()) && !string.IsNullOrEmpty(datos.ComercioConfigDescrip))
					{
						return datos.PermiteParciales;
					}
					else
					{
						///si no maneja la configuración en la resolución, entonces la buscamos en la empresa
						var empresa = context.TblEmpresas.Where(x => x.StrIdentificacion.Equals(documento_empresa)).FirstOrDefault();
						if (empresa != null)
						{
							return (empresa.IntPagoEParcial) ? 1 : 0;
						}
					}

				}

				return 0;
			}
			catch (Exception)
			{
				return 0;
			}
		}

		/// <summary>
		/// Obtiene una lista de resoluciones
		/// </summary>
		/// <param name="documento_empresa">documento de la empresa</param>
		/// <param name="numero_resolucion">número de resolución</param>
		/// <returns>datos de la resolución</returns>
		public List<TblEmpresasResoluciones> Obtener(string documento_empresa, string numero_resolucion, string prefijo, string clave_tecnica)
		{
			var Factura = TipoDocumento.Factura.GetHashCode();

			var datos = (from item in context.TblEmpresasResoluciones
						 where (item.StrNumResolucion.Equals(numero_resolucion) || numero_resolucion.Equals("*"))
						 && (item.TblEmpresas.StrIdentificacion.Equals(documento_empresa) || documento_empresa.Equals("*"))
						 && (item.StrClaveTecnica.Equals(clave_tecnica) || clave_tecnica.Equals("*"))
						 && item.IntTipoDoc == Factura
						 select item).ToList();

			return datos;

		}

		/// <summary>
		/// Obtiene las resoluciones de las empresas asociadas
		/// </summary>
		/// <param name="documento_empresa"></param>
		/// <param name="numero_resolucion"></param>
		/// <param name="prefijo"></param>
		/// <param name="clave_tecnica"></param>
		/// <returns></returns>
		public List<TblEmpresasResoluciones> ObtenerAsociadas(string documento_empresa, string numero_resolucion, string prefijo, string clave_tecnica)
		{
			var Factura = TipoDocumento.Factura.GetHashCode();

			var datos = (from item in context.TblEmpresasResoluciones
						 where item.IntTipoDoc == Factura
						 && (item.StrEmpresa.Equals(documento_empresa) || item.TblEmpresas.StrEmpresaAsociada.Equals(documento_empresa))
						 && (item.StrNumResolucion.Equals(numero_resolucion) || numero_resolucion.Equals("*"))
						 //&& (item.TblEmpresas.StrIdentificacion.Equals(documento_empresa) || documento_empresa.Equals("*"))
						 //&& (item.StrClaveTecnica.Equals(clave_tecnica) || clave_tecnica.Equals("*"))

						 select item).ToList();

			return datos;

		}





		/// <summary>
		/// Obtiene la resolución de una empresa
		/// </summary>
		/// <param name="documento_empresa">documento de la empresa</param>
		/// <param name="numero_resolucion">número de resolución</param>
		/// <returns>datos de la resolución</returns>
		public TblEmpresasResoluciones Obtener(string documento_empresa, string numero_resolucion, string prefijo, bool LazyLoading = true)
		{

			context.Configuration.LazyLoadingEnabled = LazyLoading;

			var datos = (from item in context.TblEmpresasResoluciones
						 where (item.StrNumResolucion.Equals(numero_resolucion) || numero_resolucion.Equals("*"))
						 && item.TblEmpresas.StrIdentificacion.Equals(documento_empresa)
						 && (item.StrPrefijo.Equals(prefijo) || prefijo.Equals("*"))
						 select item).FirstOrDefault();

			return datos;

		}

		/// <summary>
		/// Obtiene todas las resoluciones
		/// </summary>
		/// <returns></returns>
		public List<TblEmpresasResoluciones> ObtenerTodas()
		{
			List<TblEmpresasResoluciones> datos = (from item in context.TblEmpresasResoluciones
												   select item).ToList();

			return datos;
		}


		/// <summary>
		/// Obtiene todas las resoluciónes de una empresa
		/// </summary>
		/// <param name="documento_empresa">documento de la empresa</param>
		/// <param name="numero_resolucion">número de resolución</param>
		/// <param name="LazyLoading">LazyLoading</param>
		/// <returns>lista de resoluciones</returns>
		public List<TblEmpresasResoluciones> ObtenerResoluciones(string documento_empresa, string numero_resolucion, bool LazyLoading = true)
		{

			context.Configuration.LazyLoadingEnabled = LazyLoading;

			List<TblEmpresasResoluciones> datos = (from item in context.TblEmpresasResoluciones
												   where (item.StrNumResolucion.Equals(numero_resolucion) || numero_resolucion.Equals("*"))
												   && item.TblEmpresas.StrIdentificacion.Equals(documento_empresa)
												   orderby item.IntTipoDoc, item.StrNumResolucion, item.StrPrefijo
												   select item).ToList();

			return datos;

		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="documento_empresa"></param>
		/// <param name="numero_resolucion"></param>
		/// <returns></returns>
		public List<TblEmpresasResoluciones> ObtenerResolucionesPorTipo(string documento_empresa, int Tipo)
		{

			List<TblEmpresasResoluciones> datos = (from item in context.TblEmpresasResoluciones
												   where item.IntTipoDoc == Tipo
												   && item.TblEmpresas.StrIdentificacion.Equals(documento_empresa)
												   orderby item.IntTipoDoc, item.StrNumResolucion, item.StrPrefijo
												   select item).ToList();

			return datos;

		}

		#endregion

		#region Crear
		/// <summary>
		/// Crea la Resolucion por Empresa en la BD
		/// </summary>
		/// <param name="resoluciones">Resoluciones de la empresa</param>
		/// <returns>Resoluciones guardadas</returns>
		public TblEmpresasResoluciones Crear(TblEmpresasResoluciones resoluciones)
		{
			resoluciones = this.Add(resoluciones);

			return resoluciones;
		}

		/// <summary>
		/// Valida y crea resoluciones
		/// </summary>
		/// <param name="resoluciones">Objeto de resoluciones obtenida de la DIAN</param>
		/// <param name="obligado">Identificacion del facturdor</param>
		/// <returns>Lista de resoluciones</returns>
		public List<TblEmpresasResoluciones> Crear(ResolucionesFacturacion resoluciones, string obligado, string setidpruebas = "")
		{

			Ctl_Empresa empresa = new Ctl_Empresa();
			TblEmpresas empresaBd = empresa.Obtener(obligado, false);

			List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

			foreach (RangoFacturacion item in resoluciones.RangoFacturacion)
			{
				if (string.IsNullOrEmpty(item.Prefijo))
					item.Prefijo = string.Empty;

				//Valida si la Resolucion ya existe en BD
				TblEmpresasResoluciones tbl_resolucion_actual = Obtener(empresaBd.StrIdentificacion, item.NumeroResolucion.ToString(), item.Prefijo, false);

				// convierte el objeto del servicio a base de datos
				TblEmpresasResoluciones tbl_resolucion = Convertir(item, empresaBd, setidpruebas);

				if (tbl_resolucion_actual == null)
				{

					if (empresaBd.IntHabilitacion == Habilitacion.Produccion.GetHashCode() && empresaBd.IntManejaPagoE == true)
					{
						TblEmpresasResoluciones tbl_resolucion_comercio = null;
						try
						{
							tbl_resolucion_comercio = ObtenerResoluciones(empresaBd.StrIdentificacion, "*").Where(x => x.ComercioConfigId != null && x.StrPrefijo.Equals(item.Prefijo)).First();
						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta, string.Format("Consultando resolucion y la empresa {0} maneja pagos y resolucion {1}", empresaBd.StrIdentificacion, tbl_resolucion.StrNumResolucion));
						}
						if (tbl_resolucion_comercio != null)
						{
							tbl_resolucion.ComercioConfigId = tbl_resolucion_comercio.ComercioConfigId;
							tbl_resolucion.ComercioConfigDescrip = tbl_resolucion_comercio.ComercioConfigDescrip;
							tbl_resolucion.PermiteParciales = tbl_resolucion_comercio.PermiteParciales;
						}

					}

					// crea el registro en base de datos
					tbl_resolucion = Crear(tbl_resolucion);

					lista_resolucion.Add(tbl_resolucion);
				}
				else
				{
					tbl_resolucion_actual.DatFechaVigenciaDesde = tbl_resolucion.DatFechaVigenciaDesde;
					tbl_resolucion_actual.DatFechaVigenciaHasta = tbl_resolucion.DatFechaVigenciaHasta;
					tbl_resolucion_actual.IntRangoFinal = tbl_resolucion.IntRangoFinal;
					tbl_resolucion_actual.IntRangoInicial = tbl_resolucion.IntRangoInicial;
					tbl_resolucion_actual.StrClaveTecnica = tbl_resolucion.StrClaveTecnica;
					tbl_resolucion_actual.StrPrefijo = tbl_resolucion.StrPrefijo;
					tbl_resolucion_actual.DatFechaActualizacion = Fecha.GetFecha();
					tbl_resolucion_actual.StrIdSetDian = tbl_resolucion.StrIdSetDian;
					tbl_resolucion_actual.IntVersionDian = tbl_resolucion.IntVersionDian;
					if (!string.IsNullOrEmpty(setidpruebas))
						tbl_resolucion.StrIdSetDian = setidpruebas;

					this.Edit(tbl_resolucion_actual);
					lista_resolucion.Add(tbl_resolucion_actual);
				}
			}

			return lista_resolucion;

		}
		#endregion

		#region Editar
		/// <summary>
		/// Actualiza la configuración de pago de una resolución
		/// </summary>
		/// <param name="Stridseguridad">id de seguridad de la resolución</param>
		/// <param name="Permitepagosparciales">Permite pagos parciales</param>
		/// <param name="IdComercio">Guid del id de comercio</param>
		/// <param name="DescripcionComercio">Descripción de la configuración</param>
		/// <returns>TblEmpresasResoluciones</returns>
		public TblEmpresasResoluciones EditarConfigPago(Guid Stridseguridad, bool Permitepagosparciales, string IdComercio, string DescripcionComercio)
		{
			try
			{
				TblEmpresasResoluciones tbl = context.TblEmpresasResoluciones.Where(x => x.StrIdSeguridad == Stridseguridad).FirstOrDefault();

				tbl.PermiteParciales = (Permitepagosparciales) ? 1 : 0;
				try
				{
					tbl.ComercioConfigId = null;
					tbl.ComercioConfigId = Guid.Parse(IdComercio);
				}
				catch (Exception)
				{
				}
				tbl.ComercioConfigDescrip = DescripcionComercio;

				this.Edit(tbl);

				return tbl;
			}
			catch (Exception e)
			{
				Ctl_Log.Guardar(e, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion, "");
				throw;
			}
		}
		#endregion

		#region Convertir
		/// <summary>
		/// Convierte un Objeto de servicio en un Objeto de Base de Datos
		/// </summary>
		/// <param name="item">Resolucion obtenida de la DIAN</param>
		/// <param name="empresa">Identificacion del facturador electrónico</param>
		/// <returns>Tbl de resolucion</returns>
		public static TblEmpresasResoluciones Convertir(RangoFacturacion item, TblEmpresas empresa, string setidpruebas)
		{
			TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones()
			{
				StrEmpresa = empresa.StrIdentificacion,
				StrNumResolucion = item.NumeroResolucion.ToString(),
				StrPrefijo = (!string.IsNullOrEmpty(item.Prefijo)) ? item.Prefijo : "",
				IntRangoInicial = Convert.ToInt32(item.RangoInicial),
				IntRangoFinal = Convert.ToInt32(item.RangoFinal),
				DatFechaVigenciaDesde = item.FechaVigenciaDesde,
				DatFechaVigenciaHasta = item.FechaVigenciaHasta,
				StrClaveTecnica = (!string.IsNullOrEmpty(item.ClaveTecnica)) ? item.ClaveTecnica : "0",
				StrIdSeguridad = Guid.NewGuid(),
				DatFechaIngreso = Fecha.GetFecha(),
				DatFechaActualizacion = Fecha.GetFecha(),
				StrIdSetDian = setidpruebas,
				IntVersionDian = empresa.IntVersionDian,
				IntTipoDoc = 1


			};

			return tbl_resolucion;
		}

		/// <summary>
		/// Convierte una resolucion de un Objeto tipo Nota Credito, Nota Debito o de Interopeabilidad a Objeto de BD
		/// </summary>
		/// <param name="facturador">Identificacion del Obligado a Factuar</param>
		/// <param name="prefijo">Prefijo del documento</param>
		/// <param name="tipo_doc">Tipo de documento</param>
		/// <param name="resolucion">Resolucion del Facturador Emisor registrada en el Documento</param>
		/// <returns>Tbl de resolucion</returns>
		public static TblEmpresasResoluciones Convertir(string facturador, string prefijo, int tipo_doc, int version_dian, string resolucion = "")
		{

			DateTime fecha_actual = Fecha.GetFecha();

			Guid resolucion_info = Guid.NewGuid();

			TblEmpresasResoluciones tbl_resolucion = new TblEmpresasResoluciones()
			{
				StrEmpresa = facturador,
				StrNumResolucion = (!string.IsNullOrEmpty(resolucion)) ? resolucion : resolucion_info.ToString(),
				StrPrefijo = (!string.IsNullOrEmpty(prefijo)) ? prefijo : "",
				IntRangoInicial = 0,
				IntRangoFinal = 0,
				DatFechaVigenciaDesde = fecha_actual,
				DatFechaVigenciaHasta = fecha_actual,
				StrClaveTecnica = resolucion_info.ToString(),
				StrIdSeguridad = resolucion_info,
				DatFechaIngreso = fecha_actual,
				DatFechaActualizacion = fecha_actual,
				IntVersionDian = (short)version_dian,
				IntTipoDoc = tipo_doc

			};
			return tbl_resolucion;
		}

		/// <summary>
		/// Convierte una Objeto de Base de Datos a un Objeto de respuesta de Resolución
		/// </summary>
		/// <param name="resolucion_bd">Resolución obtenida de BD</param>
		/// <returns>Objeto de respuesta de Resolución</returns>
		public static Resolucion Convertir(TblEmpresasResoluciones resolucion_bd)
		{
			Resolucion resolucion_respuesta = new Resolucion()
			{
				ClaveTecnica = resolucion_bd.StrClaveTecnica,
				FechaResolucion = resolucion_bd.DatFechaVigenciaDesde,
				FechaVigenciaInicial = resolucion_bd.DatFechaVigenciaDesde,
				FechaVigenciaFinal = resolucion_bd.DatFechaVigenciaHasta,
				NumeroResolucion = resolucion_bd.StrNumResolucion,
				Prefijo = resolucion_bd.StrPrefijo,
				RangoInicial = resolucion_bd.IntRangoInicial,
				RangoFinal = resolucion_bd.IntRangoFinal,
				VersionDian = resolucion_bd.IntVersionDian,
				SetIdDian = resolucion_bd.StrIdSetDian
			};

			return resolucion_respuesta;

		}
		#endregion

		#region Plataforma de Servicios
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Tercero"></param>
		/// <param name="Serial"></param>
		/// <returns></returns>
		public List<ConfigPasarelas> ObtenerComercios(string Tercero, string Serial)
		{

			PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
			var url = plataforma.RutaHginetMail;

			ClienteRest<List<ConfigPasarelas>> cliente = new ClienteRest<List<ConfigPasarelas>>(string.Format("{0}/Api/ObtenerConfigPasarelas?Tercero={1}&Serial={2}", url, Tercero, Serial), TipoContenido.Applicationjson.GetHashCode(), "");
			try
			{
				List<ConfigPasarelas> data = cliente.GET();

				return data;
				//if (data != null)
				//	respuesta.Add(data);
			}
			catch (Exception ex)
			{
				var cod = cliente.CodHttp;
				throw;
			}

		}



		#endregion

	}
}
