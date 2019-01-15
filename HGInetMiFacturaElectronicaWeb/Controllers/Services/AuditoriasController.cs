using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
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
				Sesion.ValidarSesion();

				Ctl_DocumentosAudit clase_audit_doc = new Ctl_DocumentosAudit();

				//Realiza la consulta de los datos en la base de datos.
				List<TblAuditDocumentos> datos_audit = clase_audit_doc.Obtener(id_seguridad_doc, "*").OrderByDescending(x => x.DatFecha).ToList();

				if (datos_audit == null)
				{
					return NotFound();
				}

				var datos_retorno = datos_audit.Select(d => new
				{
					StrIdSeguridad = d.StrIdSeguridad,
					StrIdPeticion = d.StrIdPeticion,
					DatFecha = d.DatFecha.AddHours(-5).ToString("yyyy-MM-dd HH:mm:ss"),
					StrObligado = d.StrObligado,
					IntIdEstado = d.IntIdEstado,
					StrDesEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(d.IntIdEstado)),
					IntIdProceso = d.IntIdProceso,
					StrDesProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(d.IntIdProceso)),
					IntTipoRegistro = d.IntTipoRegistro,
					IntIdProcesadoPor = d.IntIdProcesadoPor,
					StrDesProcesadoPor = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Procedencia>(d.IntIdProcesadoPor)),
					StrRealizadoPor = d.StrRealizadoPor,
					StrMensaje = d.StrMensaje,
					StrResultadoProceso = d.StrResultadoProceso,
					StrPrefijo = d.StrPrefijo,
					StrNumero = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.StrNumero),
				}).ToList();

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		[HttpGet]
		[Route("Api/DetallesRespuesta")]
		public IHttpActionResult DetallesRespuesta(int id_proceso, string respuesta)
		{
			try
			{

				//Valida los datos de la sesión.
				Sesion.ValidarSesion();

				if (!respuesta.Substring(0, 1).Equals("{"))
					return Ok(respuesta);

				dynamic datos_retorno = JsonConvert.DeserializeObject(respuesta);


				if (id_proceso == ProcesoEstado.EnvioEmailAcuse.GetHashCode() || id_proceso == ProcesoEstado.EnvioRespuestaAcuse.GetHashCode())
				{
					PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

					MensajeResumenGlobal obj_peticion = new MensajeResumenGlobal();
					obj_peticion.identificacion = plataforma_datos.IdentificacionHGInetMail;
					obj_peticion.serial = plataforma_datos.LicenciaHGInetMail;
					obj_peticion.id_mensaje = (long)datos_retorno.MessageID;

					//MensajeResumen datos_retorno = new MensajeResumen();

					ClienteRest<MensajeResumen> cliente = new ClienteRest<MensajeResumen>(string.Format("{0}/Api/ObtenerResumenMensaje", plataforma_datos.RutaHginetMail), TipoContenido.Applicationjson.GetHashCode(), "");
					try
					{
						datos_retorno = cliente.POST(obj_peticion);
						//dynamic json_respuesta = JsonConvert.SerializeObject(datos_respuesta);
						//datos_retorno = JsonConvert.DeserializeObject(json_respuesta);
					}
					catch (Exception ex)
					{
						var cod = cliente.CodHttp;
					}
				}

				/*
				dynamic propiedad = new ExpandoObject();
				List<dynamic> lista_retorno = new List<dynamic>();

				foreach (JProperty key in datos_retorno)
				{
					if (key.Parent.Count > 1)
					{
						List<dynamic> lista_datos = new List<dynamic>();

						foreach (var item in key)
						{
							propiedad.Nombre = key.Name;
							propiedad.Valor = key.Value;

							lista_datos.Add(propiedad);
						}
					}
					else
					{
						propiedad.Nombre = key.Name;
						propiedad.Valor = key.Value;

						lista_retorno.Add(propiedad);
					}

				}
				*/


				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
