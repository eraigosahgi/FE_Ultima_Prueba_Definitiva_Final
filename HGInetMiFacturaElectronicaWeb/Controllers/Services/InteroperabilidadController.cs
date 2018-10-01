using HGInetInteroperabilidad.Procesos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using static HGInetMiFacturaElectronicaWeb.Controllers.Services.DocumentosController;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class InteroperabilidadController : ApiController
	{
		/// <summary>
		/// Obtiene los formatos
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ObtenerPendientesProceso")]
		public IHttpActionResult ObtenerPendientesProceso(string identificacion_proveedor)
		{
			try
			{
				Sesion.ValidarSesion();
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<TblDocumentos> docs_pendientes = new List<TblDocumentos>();

				Ctl_Documento clase_documentos = new Ctl_Documento();

				docs_pendientes = clase_documentos.ObtenerDocumentosProveedores(identificacion_proveedor);

				if (docs_pendientes == null)
				{
					return NotFound();
				}

				var datos_retorno = docs_pendientes.Select(d => new
				{
					IdFacturador = d.TblEmpresasFacturador.StrIdentificacion,
					Facturador = d.TblEmpresasFacturador.StrRazonSocial,
					NumeroDocumento = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.IntNumero),
					d.DatFechaDocumento,
					d.DatFechaIngreso,
					d.DatFechaVencDocumento,
					IntVlrTotal = (d.IntDocTipo == 3) ? -d.IntVlrTotal : d.IntVlrTotal,
					EstadoFactura = DescripcionEstadoFactura(d.IntIdEstado),
					EstadoAcuse = DescripcionEstadoAcuse(d.IntAdquirienteRecibo),
					MotivoRechazo = d.StrAdquirienteMvoRechazo,
					d.StrAdquirienteMvoRechazo,
					IdentificacionAdquiriente = d.TblEmpresasAdquiriente.StrIdentificacion,
					NombreAdquiriente = d.TblEmpresasAdquiriente.StrRazonSocial,
					MailAdquiriente = d.TblEmpresasAdquiriente.StrMail,
					Xml = d.StrUrlArchivoUbl,
					Pdf = d.StrUrlArchivoPdf,
					d.StrIdSeguridad,
					RutaAcuse = string.Format("{0}{1}", plataforma.RutaPublica, Constantes.PaginaAcuseRecibo.Replace("{id_seguridad}", d.StrIdSeguridad.ToString())),
					tipodoc = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntDocTipo))
				});

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene los formatos
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Api/ObtenerProveedores")]
		public IHttpActionResult ObtenerProveedores()
		{
			try
			{
				Sesion.ValidarSesion();
				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				List<TblConfiguracionInteroperabilidad> proveedores = new List<TblConfiguracionInteroperabilidad>();

				Ctl_ConfiguracionInteroperabilidad clase_inter = new Ctl_ConfiguracionInteroperabilidad();

				proveedores = clase_inter.ObtenerProveedores("*");

				if (proveedores == null)
				{
					return NotFound();
				}

				var datos_retorno = proveedores.Select(d => new
				{
					Identificacion = d.StrIdentificacion,
					RazonSocial = d.StrRazonSocial
				});

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Retorna la descripción del estado de la factura.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private string DescripcionEstadoFactura(short e)
		{
			try
			{
				return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>(e));
			}
			catch (Exception excepcion)
			{
				return string.Format("Desconocido {0}", e);
			}
		}

		/// <summary>
		/// Retorna la descripción del estado del acuse.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private string DescripcionEstadoAcuse(short e)
		{
			try
			{
				return Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.AdquirienteRecibo>(e));
			}
			catch (Exception excepcion)
			{
				return string.Format("Desconocido {0}", e);
			}
		}


		/// <summary>
		/// Recibe lista de Documentos 
		/// </summary>
		/// <param name="objeto"></param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult Post(Object objeto)
		{
			try
			{
				Ctl_Documento clase_doc = new Ctl_Documento();

				var jobjeto = (dynamic)objeto;

				string ListaDoc = jobjeto.Documentos;

				List<DocumentosJSON> ListadeDocumentos = new JavaScriptSerializer().Deserialize<List<DocumentosJSON>>(ListaDoc);

				List<TblDocumentos> ListaDocumentos = new List<TblDocumentos>();

				foreach (var item in ListadeDocumentos)
				{
					TblDocumentos doc = clase_doc.ObtenerPorIdSeguridad(item.Documentos).First();
					if (doc != null)
						ListaDocumentos.Add(doc);
				}

				Ctl_Envio.Procesar(ListaDocumentos);

				return Ok();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}
