using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_Formatos : BaseObject<TblFormatos>
	{
		List<Factura> DatosPlantilla()
		{

			List<Factura> datos_reporte = new List<Factura>();
			Factura datos = new Factura();
			datos.CodigoRegistro = "1";
			datos.Cufe = "c1c2885ac587ef5869e12a04bd06ca89dc9a8868";
			datos.DataKey = "690c8a6436cc20dfe11d159393e5374d1b4fca6c";
			datos.Documento = 990000909;
			datos.DocumentoRef = "1002B";
			datos.Fecha = new DateTime(2018, 07, 31);
			datos.FechaVence = new DateTime(2018, 07, 31);
			datos.Moneda = "COP";
			datos.Neto = 0.00M;
			datos.NumeroResolucion = "9000000033394696";
			datos.Prefijo = "PRUE";
			datos.Total = 0.00M;
			datos.Valor = 0.00M;
			datos.ValorDescuento = 0.00M;
			datos.ValorImpuestoConsumo = 0.00M;
			datos.ValorIva = 0.00M;
			datos.ValorReteFuente = 0.00M;
			datos.ValorReteIca = 0.00M;
			datos.ValorReteIva = 0.00M;
			datos.ValorSubtotal = 0.00M;
			datos.DatosObligado = new Tercero();
			datos.DatosAdquiriente = new Tercero();

			Tercero adquiriente = new Tercero();

			Tercero obj_tercero = new Tercero();
			obj_tercero = new Tercero();
			obj_tercero.Ciudad = "Ciudad";
			obj_tercero.CodigoPais = "Pais";
			obj_tercero.Departamento = "Departamento";
			obj_tercero.Direccion = "Dirección Facturador";
			obj_tercero.Email = "facturador@dominio.com";
			obj_tercero.Identificacion = "Identificación Facturador";
			obj_tercero.IdentificacionDv = 0;
			obj_tercero.NombreComercial = "Nombre Comercial Facturador";
			obj_tercero.PaginaWeb = "Página Web Facturador";
			obj_tercero.PrimerApellido = "Primer Apellido";
			obj_tercero.PrimerNombre = "Primer Nombre";
			obj_tercero.RazonSocial = "Razón social Facturador";
			obj_tercero.Regimen = 1;
			obj_tercero.SegundoApellido = "Segundo Apellido";
			obj_tercero.SegundoNombre = "Segundo Nombre";
			obj_tercero.Telefono = "Teléfono";
			obj_tercero.TipoIdentificacion = 13;
			obj_tercero.TipoPersona = 2;
			datos.DatosObligado = obj_tercero;

			//DATOS ADQUIRIENTE
			obj_tercero = new Tercero();
			obj_tercero.Ciudad = "Ciudad";
			obj_tercero.CodigoPais = "Pais";
			obj_tercero.Departamento = "Departamento";
			obj_tercero.Direccion = "Dirección Adquiriente";
			obj_tercero.Email = "adquiriente@dominio.com";
			obj_tercero.Identificacion = "Identificación Adquiriente";
			obj_tercero.IdentificacionDv = 0;
			obj_tercero.NombreComercial = "Nombre Comercial Adquiriente";
			obj_tercero.PaginaWeb = "Página Web";
			obj_tercero.PrimerApellido = "Primer Apellido";
			obj_tercero.PrimerNombre = "Primer Nombre";
			obj_tercero.RazonSocial = "Razón social Adquiriente";
			obj_tercero.Regimen = 1;
			obj_tercero.SegundoApellido = "Segundo Apellido";
			obj_tercero.SegundoNombre = "Segundo Nombre";
			obj_tercero.Telefono = "Teléfono";
			obj_tercero.TipoIdentificacion = 13;
			obj_tercero.TipoPersona = 2;
			datos.DatosAdquiriente = obj_tercero;

			//DETALLES DOCUMENTO
			datos.DocumentoDetalles = new List<DocumentoDetalle>();
			DocumentoDetalle detalles_doc = new DocumentoDetalle();
			detalles_doc.Bodega = "100";
			detalles_doc.Cantidad = 1;
			detalles_doc.Codigo = 1;
			detalles_doc.DescuentoPorcentaje = 0.00M;
			detalles_doc.DescuentoValor = 0.00M;
			detalles_doc.ImpoConsumoPorcentaje = 0.00M;
			detalles_doc.IvaPorcentaje = 0.00M;
			detalles_doc.IvaValor = 0.00M;
			detalles_doc.ProductoCodigo = "1001";
			detalles_doc.ProductoDescripcion = "Producto de venta n.1";
			detalles_doc.ProductoGratis = false;
			detalles_doc.ProductoNombre = "";
			detalles_doc.ReteFuentePorcentaje = 0.00M;
			detalles_doc.ReteFuenteValor = 0.00M;
			detalles_doc.ReteIcaPorcentaje = 0.00M;
			detalles_doc.ReteIcaValor = 0.00M;
			detalles_doc.UnidadCodigo = "Und";
			detalles_doc.ValorImpuestoConsumo = 0.00M;
			detalles_doc.ValorSubtotal = 0.00M;
			detalles_doc.ValorUnitario = 0.00M;
			datos.DocumentoDetalles.Add(detalles_doc);

			datos.DocumentoDetalles.Add(detalles_doc);

			List<FormatoCampo> campos_predeterminados = new List<FormatoCampo>();
			datos.DocumentoFormato = new Formato();
			datos.DocumentoFormato.CamposPredeterminados = new List<FormatoCampo>();
			datos.DocumentoFormato.CamposPredeterminados.AddRange(campos_predeterminados);

			datos_reporte.Add(datos);

			return datos_reporte;
		}

		#region Agregar
		/// <summary>
		/// Almacena el formato en la base de datos
		/// </summary>
		/// <param name="formato"></param>
		/// <returns></returns>
		public TblFormatos Crear(TblFormatos formato)
		{
			formato = this.Add(formato);

			return formato;
		}

		public TblFormatos AlmacenarFormatoPdf(TblFormatos datos_formato)
		{
			try
			{
				System.Guid id_seguridad = System.Guid.NewGuid();
				datos_formato.DatFechaRegistro = Fecha.GetFecha();
				datos_formato.StrIdSeguridad = id_seguridad;
				datos_formato.IntCodigoFormato = ObtenerIdFormato(datos_formato.StrEmpresa);
				datos_formato.IntTipo = TipoFormato.FormatoPDF.GetHashCode();

				//Crea el registro en base de datos.
				TblFormatos datos_respueta = Crear(datos_formato);

				return datos_respueta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el código consecutivo del formato
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		public int ObtenerIdFormato(string identificacion_empresa)
		{
			try
			{
				int formato_resultado = 0;

				IEnumerable<TblFormatos> datos = (from formato in context.TblFormatos
												  where formato.StrEmpresa.Equals(identificacion_empresa)
												  select formato);

				if (datos.Count() > 0)
					formato_resultado = datos.Max(x => x.IntCodigoFormato);

				formato_resultado = formato_resultado + 1;

				return formato_resultado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion


		#region Actualizar


		public TblFormatos Actualizar(TblFormatos formato)
		{
			formato = this.Edit(formato);

			return formato;
		}

		/// <summary>
		/// Actualiza los datos del formato (diseño)
		/// </summary>
		/// <param name="id_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <param name="byte_formato"></param>
		/// <param name="tipo_formato"></param>
		/// <returns></returns>
		public TblFormatos ActualizarFormato(int id_formato, string identificacion_empresa, byte[] byte_formato, int tipo_formato)
		{
			try
			{
				TblFormatos datos_formato = Obtener(id_formato, identificacion_empresa, tipo_formato);
				datos_formato.DatFechaActualización = Fecha.GetFecha();
				datos_formato.Formato = byte_formato;

				//Actualiza el registro en base de datos.
				TblFormatos datos_respueta = Actualizar(datos_formato);

				return datos_respueta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Actualiza el estado del formato
		/// </summary>
		/// <param name="id_formato"></param>
		/// <param name="identificacion_empresa"></param>
		/// <param name="estado_actual"></param>
		/// <param name="tipo_formato"></param>
		/// <returns></returns>
		public TblFormatos ActualizarEstadoFormato(int id_formato, string identificacion_empresa, bool estado_actual, int tipo_formato)
		{
			try
			{
				TblFormatos datos_formato = Obtener(id_formato, identificacion_empresa, tipo_formato);
				datos_formato.DatFechaActualización = Fecha.GetFecha();

				bool estado_formato = false;

				if (estado_actual)
					estado_formato = false;
				else
					estado_formato = true;

				datos_formato.IntEstado = estado_formato;

				//Actualiza el registro en base de datos.
				TblFormatos datos_respueta = Actualizar(datos_formato);

				return datos_respueta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		#endregion

		#region Obtener

		/// <summary>
		/// Obtiene el formato por código e identificación de la empresa
		/// </summary>
		/// <param name="id_formato">código del formato</param>
		/// <param name="identificacion_empresa">número de identificación de la empresa</param>
		/// <returns></returns>
		public TblFormatos Obtener(int id_formato, string identificacion_empresa, int tipo_formato)
		{
			try
			{

				TblFormatos formato_resultado = (from formato in context.TblFormatos
												 where formato.IntCodigoFormato == id_formato
												 && formato.StrEmpresa.Equals(identificacion_empresa)
												 && formato.IntTipo == tipo_formato
												 select formato).FirstOrDefault();

				return formato_resultado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		///  Obtiene los formatos de la empresa
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <param name="tipo_formato">indica si es plantilla pdf (1) -- plantilla html (2)</param>
		/// <returns></returns>
		public List<TblFormatos> ObtenerFormatosEmpresa(string identificacion_empresa, int tipo_formato)
		{
			try
			{
				Ctl_Empresa clase_empresa = new Ctl_Empresa();
				TblEmpresas datos_empresa = clase_empresa.Obtener(identificacion_empresa);

				List<TblFormatos> listado_formatos = new List<TblFormatos>();

				List<string> empresas_asociadas = new List<string>();
				empresas_asociadas.Add(identificacion_empresa);

				if (datos_empresa.IntAdministrador)
				{
					return listado_formatos = (from formato in context.TblFormatos
											   where formato.IntTipo == tipo_formato
											   select formato).ToList();
				}
				else if (datos_empresa.IntIntegrador)
				{
					empresas_asociadas = clase_empresa.ObtenerAsociadas(identificacion_empresa).Select(x => x.StrIdentificacion).ToList();

					foreach (string item in empresas_asociadas)
					{
						listado_formatos.AddRange((from formato in context.TblFormatos
												   where formato.StrEmpresa.Equals(item)
													&& formato.IntTipo == tipo_formato
												   select formato).ToList());
					}
				}
				else if (datos_empresa.IntObligado && !datos_empresa.IntIntegrador)
				{
					listado_formatos = (from formato in context.TblFormatos
										where formato.StrEmpresa.Equals(identificacion_empresa)
										&& formato.IntTipo == tipo_formato
										&& formato.IntEstado
										select formato).ToList();
				}

				listado_formatos.AddRange((from formato in context.TblFormatos
										   where formato.IntGenerico == true
											&& formato.IntTipo == tipo_formato
										   select formato).ToList());

				return listado_formatos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

	}
}
