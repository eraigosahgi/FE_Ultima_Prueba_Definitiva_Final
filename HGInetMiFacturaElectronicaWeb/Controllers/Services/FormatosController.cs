using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class FormatosController : ApiController
	{
		/// <summary>
		/// Obtiene los formatos
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/FormatosPDFEmpresa")]
		public IHttpActionResult FormatosPdfEmpresa(string identificacion_empresa)
		{
			try
			{
				Sesion.ValidarSesion();

				List<TblFormatos> datos_formatos = new List<TblFormatos>();

				Ctl_Formatos clase_formatos = new Ctl_Formatos();

				datos_formatos = clase_formatos.ObtenerFormatosEmpresa(identificacion_empresa, TipoFormato.FormatoPDF.GetHashCode());

				if (datos_formatos == null)
				{
					return NotFound();
				}

				var datos_retorno = datos_formatos.Select(d => new
				{
					ClavePrimaria = string.Format("{0}-{1}", d.IntCodigoFormato, d.StrEmpresa),
					CodigoFormato = d.IntCodigoFormato,
					Generico = (Sesion.DatosEmpresa.IntAdministrador) ? false : d.IntGenerico,
					FechaRegistro = d.DatFechaRegistro.ToString(Fecha.formato_fecha_hginet),
					Titulo = (d.IntGenerico) ? "Prediseñado" : "Personalizado",
					Estado = d.IntEstado,
					TipoDoc = d.IntDocTipo,
					NitEmpresa = d.StrEmpresa,
					RazonSocial = d.TblEmpresas.StrRazonSocial,
					Administrador = (Sesion.DatosEmpresa.IntAdministrador) ? true : false,
					EstadoDescripcion = DescripcionEstado(d.IntEstado),
				});

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		private string DescripcionEstado(int codigo_enum)
		{
			try
			{
				return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EstadosFormato>(codigo_enum));
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message);
			}
		}

		[HttpPost]
		[Route("Api/ActualizarEstadoFormato")]
		public IHttpActionResult ActualizarEstadoFormato(int id_formato, string identificacion_empresa, int estado_actual, int tipo_proceso, string observaciones)
		{
			try
			{
				Sesion.ValidarSesion();

				TblFormatos datos_formatos = new TblFormatos();

				Ctl_Formatos clase_formatos = new Ctl_Formatos();

				TiposProceso tipo_proceso_bd = Enumeracion.GetEnumObjectByValue<TiposProceso>(tipo_proceso);
				datos_formatos = clase_formatos.ActualizarEstadoFormato(id_formato, identificacion_empresa, estado_actual, TipoFormato.FormatoPDF.GetHashCode(), Sesion.DatosEmpresa, Sesion.DatosUsuario, tipo_proceso_bd, observaciones);

				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		[HttpPost]
		[Route("Api/AlmacenarFormatoPdf")]
		public IHttpActionResult AlmacenarFormatoPdf(string identificacion_empresa, int estado, int categoria, string observaciones, int formato_base, string empresa_base, int tipo_documento)
		{
			try
			{
				Sesion.ValidarSesion();
				Ctl_Formatos clase_formatos = new Ctl_Formatos();

				TblFormatos datos_formatos = new TblFormatos();
				if (formato_base == 0 && empresa_base.Equals("0"))
				{
					string ruta = Directorio.ObtenerDirectorioRaiz() + @"Views\ReportDesigner\XmlReporteBase.xml";

					XmlDocument doc = new XmlDocument();
					doc.Load(ruta);
					byte[] byteXml = Encoding.UTF8.GetBytes(doc.InnerXml);
					datos_formatos.Formato = byteXml;
				}
				else
				{
					//int formato_base, string empresa_base
					TblFormatos datos_base = clase_formatos.Obtener(formato_base, empresa_base, TipoFormato.FormatoPDF.GetHashCode());
					datos_formatos.Formato = datos_base.Formato;
				}
				datos_formatos.IntDocTipo = tipo_documento;
				datos_formatos.StrEmpresa = identificacion_empresa;
				datos_formatos.IntEstado = Convert.ToInt16(estado);
				datos_formatos.IntGenerico = Convert.ToBoolean(categoria);
				datos_formatos.StrObservaciones = observaciones;

				TblFormatos datos_retorno = clase_formatos.AlmacenarFormatoPdf(datos_formatos, Sesion.DatosUsuario.StrIdSeguridad);

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		[HttpGet]
		[Route("Api/EnviarFormatoPrueba")]
		public IHttpActionResult EnviarFormatoPrueba(int id_formato, string identificacion_empresa, string email_destino)
		{
			try
			{
				Sesion.ValidarSesion();
				Ctl_Formatos clase_formatos = new Ctl_Formatos();

				clase_formatos.EnviarFormatoPrueba(Sesion.DatosEmpresa, id_formato, identificacion_empresa, TipoFormato.FormatoPDF.GetHashCode(), email_destino);

				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}
