using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class FormatosController : ApiController
	{

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
					CodigoFormato = d.IntCodigoFormato,
					FechaRegistro = d.DatFechaRegistro.ToString(Fecha.formato_fecha_hginet)
				});

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
