using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.ObjetosComunes.Licenciamiento;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.Peticiones;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class AuditoriasController : ApiController
	{
		[HttpGet]
		[Route("Api/AuditoriaDocumento")]
		public IHttpActionResult AuditoriaDocumento(string id_seguridad_doc)
		{
			try
			{
				//Valida los datos de la sesión.
				//Sesion.ValidarSesion();

				Ctl_DocumentosAudit clase_audit_doc = new Ctl_DocumentosAudit();

				//Realiza la consulta de los datos en la base de datos.
				List<TblAuditDocumentos> datos_audit = clase_audit_doc.Obtener(id_seguridad_doc, "*").OrderByDescending(x => x.DatFecha).ToList();

				if (datos_audit == null)
				{
					return NotFound();
				}

				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos del documento en la base de datos.
				TblDocumentos datos_doc = ctl_documento.ObtenerPorIdSeguridad(new Guid(id_seguridad_doc)).FirstOrDefault();

				var datos_retorno = datos_audit.Select(d => new
				{
					StrIdSeguridad = d.StrIdSeguridad,
					StrIdPeticion = d.StrIdPeticion,
					DatFecha = d.DatFecha.AddHours(-5).ToString("yyyy-MM-dd HH:mm:ss.fff"),
					StrObligado = d.StrObligado,
					IntIdEstado = d.IntIdEstado,
					StrDesEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(d.IntIdEstado)),
					IntIdProceso = d.IntIdProceso,
					StrDesProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(d.IntIdProceso)),
					IntTipoRegistro = d.IntTipoRegistro,
					IntIdProcesadoPor = d.IntIdProcesadoPor,
					StrDesProcesadoPor = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Procedencia>(d.IntIdProcesadoPor)),
					StrRealizadoPor = d.StrRealizadoPor,
					StrDesRealizadoPor = (!string.IsNullOrWhiteSpace(d.StrRealizadoPor)) ? NombreUsuario(new Guid(d.StrRealizadoPor)) : string.Empty,
					StrMensaje = d.StrMensaje,
					StrResultadoProceso = d.StrResultadoProceso,
					StrPrefijo = d.StrPrefijo,
					StrNumero = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.StrNumero),
					//Asigna las rutas de los archivos a las propiedades de retorno, estas se obtiene de la consulta del documento a la bd.
					RutaXml = (datos_doc != null) ? datos_doc.StrUrlArchivoUbl : string.Empty,
					RutaPdf = (datos_doc != null) ? datos_doc.StrUrlArchivoPdf : string.Empty,
					RutaXmlAcuse = (datos_doc != null) ? datos_doc.StrUrlAcuseUbl : string.Empty,
					TipoDocumento = (datos_doc != null) ? Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(datos_doc.IntDocTipo)) : "Documento",
				}).ToList();

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Retorna el nombre completo del usuario.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		private string NombreUsuario(Guid id_seguridad)
		{
			try
			{
				if (id_seguridad == null)
					return string.Empty;

				Ctl_Usuario clase_usuario = new Ctl_Usuario();
				TblUsuarios datos_usuario = clase_usuario.ObtenerIdSeguridad(id_seguridad);

				if (datos_usuario != null)
					return string.Format("{0} {1}", datos_usuario.StrNombres, datos_usuario.StrApellidos);

				return string.Empty;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		[HttpGet]
		[Route("Api/DetallesRespuesta")]
		public IHttpActionResult DetallesRespuesta(string respuesta)
		{
			try
			{
				//Valida los datos de la sesión.
				//Sesion.ValidarSesion();
				respuesta = respuesta.Replace("[", "").Replace("]", "");
				dynamic datos_retorno = JsonConvert.DeserializeObject(respuesta);

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				MensajeResumenGlobal obj_peticion = new MensajeResumenGlobal();
				obj_peticion.identificacion = plataforma_datos.IdentificacionHGInetMail;
				obj_peticion.serial = plataforma_datos.LicenciaHGInetMail;
				obj_peticion.id_mensaje = (long)datos_retorno.MessageID;

				ClienteRest<MensajeResumen> cliente = new ClienteRest<MensajeResumen>(string.Format("{0}/Api/ObtenerResumenMensaje", plataforma_datos.RutaHginetMail), TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					datos_retorno = cliente.POST(obj_peticion);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
				}
				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
