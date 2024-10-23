using System.Net;
using System.Security.Claims;
using System.Web.Http;
using WebApi.Jwt.Filters;
using HGInetInteroperabilidad.Configuracion;
using HGInetInteroperabilidad.Objetos;
using HGInetMiFacturaElectonicaController;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Linq;
using LibreriaGlobalHGInet.Funciones;
using HGInetInteroperabilidad.Procesos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaController.Properties;
using Org.BouncyCastle.Asn1.Ocsp;
using HGInetInteroperabilidad.Servicios;

namespace WebApi.Jwt.Controllers
{
    public class InteroperabilidadController : ApiController
    {
        /// <summary>
        /// Login del proceso de interoperabilidad, para generar el token que se debe mantener
        /// </summary>
        /// <param name="usuario">Objeto usuario que contiene username y password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("interoperabilidad/api/v1_0/login")]
        public HttpResponseMessage login(Usuario usuario)
        {


            AutenticacionRespuesta respuesta = new AutenticacionRespuesta();
            try
            {   
                //Hora de expiracion, 24 depues de generar dicho token
                respuesta.passwordExpiration = DateTime.UtcNow.AddHours(24).ToString("o");

                TblConfiguracionInteroperabilidad Proveedor = new TblConfiguracionInteroperabilidad();
                Ctl_ConfiguracionProveedores conf = new Ctl_ConfiguracionProveedores();

                Proveedor = conf.CheckUser(usuario.u, usuario.p);

                if (Proveedor != null)
                {
                    //Aqui se utiliza para el Token una constante llamada NitResolucionconPrefijo
                    respuesta.jwtToken = JwtManager.GenerateToken(Constantes.NitResolucionconPrefijo, Proveedor.StrIdentificacion, Proveedor.StrRazonSocial);

                    //200
                    return Request.CreateResponse(HttpStatusCode.OK, respuesta);
                }

                //401                                
                return Request.CreateResponse(HttpStatusCode.Unauthorized, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterLogin>(RespuestaInterLogin.claveinvalida.GetHashCode())));
            }
            catch (Exception)
            {
                //500

                return Request.CreateResponse(HttpStatusCode.Unauthorized, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterLogin>(RespuestaInterLogin.ErrorInterno.GetHashCode())));
            }
        }

        /// <summary>
        /// Consultar documento por UUID, aqui debo retornar un objeto que contiene el historico del Documento 
        /// GET ​ ​https://[Dominio_Proveedor]/interoperabilidad/api/v1_0/consultar/{UUID}
        /// </summary>
        /// <param name="UUID">id de seguridad del documento</param>
        /// <returns></returns>
        [JwtAuthentication]
        [HttpGet]
        [Route("interoperabilidad/api/v1_0/consultar/{uuid}")]
        public HttpResponseMessage Consultar(string uuid)
        {


            try
            {
                //[FromUri]string UUID
                var identity = User?.Identity as ClaimsIdentity;

                var usernameClaim = identity.FindFirst("username");


                //Aqui envio los datos al controlador de proceso
                string Proveedor = usernameClaim.Value;

                List<DetalleDocRespuesta> ListaRespuesta = new List<DetalleDocRespuesta>();

                RegistroDocRespuesta docRespuesta = new RegistroDocRespuesta();

                TblDocumentos doc = new TblDocumentos();

                Ctl_Documento Documentos = new Ctl_Documento();

                doc = Documentos.ObtenerHistorialDococumento(Guid.Parse(uuid));

                //No existe Documento
                if (doc == null)
                {
                    docRespuesta.timeStamp = DateTime.UtcNow.ToString("o");
                    docRespuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidadUUID>(RespuestaInterOperabilidadUUID.Noexiste.GetHashCode()));

                    return Request.CreateResponse(HttpStatusCode.Conflict, docRespuesta);

                }
                else
                {

                    if (doc.StrProveedorEmisor != Proveedor)
                    {
                        //406
                        docRespuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
                        docRespuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidadUUID>(RespuestaInterOperabilidadUUID.Noexisteparaproveedorcosnsultado.GetHashCode()));
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, docRespuesta);
                    }
                    else
                    {
                        //Documento Radicado                
                        DetalleDocRespuesta Resp_Radicado = new DetalleDocRespuesta();
                        Resp_Radicado.nombre = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDDocumento>(RespuestaUUIDDocumento.Radicado.GetHashCode()));
                        Resp_Radicado.mensaje = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(RespuestaUUIDDocumento.Radicado.GetHashCode()));
                        Resp_Radicado.timeStamp = Fecha.FechaUtc(doc.DatFechaIngreso);
                        ListaRespuesta.Add(Resp_Radicado);
                        
                        //Documento Entregado                
                        DetalleDocRespuesta Resp_Entregado = new DetalleDocRespuesta();
                        Resp_Entregado.nombre = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDDocumento>(RespuestaUUIDDocumento.Entregado.GetHashCode()));
                        Resp_Entregado.mensaje = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(RespuestaUUIDDocumento.Entregado.GetHashCode()));
                        Resp_Entregado.timeStamp = Fecha.FechaUtc(doc.DatFechaIngreso);
                        ListaRespuesta.Add(Resp_Entregado);

                        //Mensajes Global
                        docRespuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(RespuestaUUIDDocumento.Entregado.GetHashCode()));

                        //Documento Aprovado o Rechazado
                        if (doc.IntAdquirienteRecibo > 0)
                        {
                            DetalleDocRespuesta Resp = new DetalleDocRespuesta();
                            Resp.nombre = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDDocumento>(doc.IntAdquirienteRecibo));
                            Resp.mensaje = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(doc.IntAdquirienteRecibo));
                            Resp.timeStamp = Fecha.FechaUtc(Convert.ToDateTime(doc.DatAdquirienteFechaRecibo));
                            ListaRespuesta.Add(Resp);

                            //Mensajes Global
                            docRespuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(doc.IntAdquirienteRecibo));
                        }

                        //Documento pagado
                        if (doc.IntIdEstadoPago == 1)
                        {
                            //Documento Aprovado o Rechazado                    
                            DetalleDocRespuesta Resp_pagado = new DetalleDocRespuesta();
                            Resp_pagado.nombre = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDDocumento>(RespuestaUUIDDocumento.Pagado.GetHashCode()));
                            Resp_pagado.mensaje = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(RespuestaUUIDDocumento.Pagado.GetHashCode()));
                            Resp_pagado.timeStamp = Fecha.FechaUtc(doc.DatFechaActualizaEstado);
                            ListaRespuesta.Add(Resp_pagado);

                            //Mensajes Global
                            docRespuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(RespuestaUUIDDocumento.Pagado.GetHashCode()));
                        }

                        //Encabezado de la respuesta                             
                        docRespuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
                        docRespuesta.historial = ListaRespuesta;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.Created, docRespuesta);
                //return Ok(docRespuesta);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidadUUID>(RespuestaInterOperabilidadUUID.ErrorInterno.GetHashCode())));
            }

        }


        /// <summary>
        /// Registra la lista de documentos enviados por el proveedor Emisor
        /// 
        /// POST https://[Dominio_Proveedor]/interoperabilidad/api/v1_0/registrar 
        /// </summary>
        /// <param name="registroRespuesta"></param>
        /// <returns></returns>
        [JwtAuthentication]
        [HttpPost]
        [Route("interoperabilidad/api/v1_0/registrar")]
        public HttpResponseMessage Registrar(RegistroListaDoc registroRespuesta)
        {
            try
            {

                var identity = User?.Identity as ClaimsIdentity;
                var usernameClaim = identity.FindFirst("username");

                //Aqui envio los datos al controlador de proceso
                string Proveedor = usernameClaim.Value;

                RegistroListaDocRespuesta respuesta = Ctl_Descomprimir.Procesar(registroRespuesta, Proveedor);

                //Ubico el codigo de respuesta del proceso de documentos
                int codigo = Split.Codigo(respuesta.mensajeGlobal);

                //Asigo el codigo de respuesta al encabezado
                respuesta.mensajeGlobal = Split.Mensaje(respuesta.mensajeGlobal);


                switch (codigo)
                {
                    case 200:
                        return Request.CreateResponse(HttpStatusCode.OK, respuesta);
                        
                    case 201:
                        return Request.CreateResponse(HttpStatusCode.Created, respuesta);

                    case 406:
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, respuesta);

                    case 412:
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, respuesta);

                    case 414:
                        return Request.CreateResponse(HttpStatusCode.RequestUriTooLong, respuesta);

                    case 415:
                        return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, respuesta);

                    case 409:
                        return Request.CreateResponse(HttpStatusCode.Conflict, respuesta);

                    default:                    
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, respuesta);
                        
                }
                
            }
            catch (Exception)
            {
                //Aqui se debe retornar error 500
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ""); 
            }

        }

        //


        /// <summary>
        /// Servicio para el cambio de contraseña del proveedor
        /// PUT ​ https://[Dominio_Proveedor]/interoperabilidad/api/v1_0/cambioContrasena 
        /// </summary>        
        /// <returns></returns>        
        [AllowAnonymous]
        [HttpPut]
        [Route("interoperabilidad/api/v1_0/cambioContrasena")]
        public HttpResponseMessage cambioContrasena()
        {
            MensajeGlobal Respuesta = new MensajeGlobal();
            try
            {
                System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                string NITProveedor = string.Empty;
                string ContrasenaNueva = string.Empty;
                string ContrasenaActual = string.Empty;


                Respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);

                //Obtengo el codigo del Proveedor
                //Si le falta alguno de los parametros, se debe retornar un error
                if (headers.Contains("NITProveedor"))
                {
                    NITProveedor = headers.GetValues("NITProveedor").First();
                }

                if (headers.Contains("ContrasenaNueva"))
                {
                    ContrasenaNueva = headers.GetValues("ContrasenaNueva").First();
                }

                if (headers.Contains("ContrasenaActual"))
                {
                    ContrasenaActual = headers.GetValues("ContrasenaActual").First();
                }

                //Aqui se debe hacer el proceso de cambio de contraseña
                ///------------------------------------------------------

                //Y luego se debe enviar esta respuesta

                Ctl_ConfiguracionInteroperabilidad Controlador = new Ctl_ConfiguracionInteroperabilidad();

                if (!Ctl_Funciones.ContrasenaValida(ContrasenaActual, ContrasenaNueva))
                {
                    //406
                    Respuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterCambioClave>(RespuestaInterCambioClave.NivelIncorrecto.GetHashCode()));
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, Respuesta);
                }
                
                TblConfiguracionInteroperabilidad Datos = Controlador.CambiarContraseña(NITProveedor, ContrasenaActual, ContrasenaNueva);

                if (Datos == null)
                {
                    //409
                    Respuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterCambioClave>(RespuestaInterCambioClave.Noexiste.GetHashCode()));
                    return Request.CreateResponse(HttpStatusCode.Conflict, Respuesta);
                }

                //201
                Respuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterCambioClave>(RespuestaInterCambioClave.CambioExitoso.GetHashCode()));
                return Request.CreateResponse(HttpStatusCode.Created, Respuesta);
            }
            catch (Exception)
            {
                //En caso de generar un error                
                Respuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterCambioClave>(RespuestaInterCambioClave.ErrorInterno.GetHashCode()));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Respuesta);

            }

        }

        /// <summary>
        /// Servicio encargado de retornar, en codificación base 64, el application response definido por la DIAN y el cual se
        /// encuentra asociado a la lógica de procesamiento, aceptación o rechazo de una factura electrónica
        /// 
        ///  GET ​ ​https://[Dominio_Proveedor]/interoperabilidad/api/v1_0/application/{UUID} 
        /// </summary>
        /// <param name="UUID">Identificador del documento electrónico a retornar el application response asociado</param>
        /// <returns></returns>
        [JwtAuthentication]
        [HttpGet]
        [Route("interoperabilidad/api/v1_0/application/{uuid}")]
        public HttpResponseMessage application(string uuid)
        {
            try
            {
                var identity = User?.Identity as ClaimsIdentity;

                var usernameClaim = identity.FindFirst("username");

                string Proveedor = usernameClaim.Value;
                //Aqui debo generar el proceso que valida el estado del documento

                Ctl_Documento Controlador = new Ctl_Documento();
                TblDocumentos datos = Controlador.ObtenerPorIdInteroperabilidad(Guid.Parse(uuid));
                if (datos == null)
                {
                    //409
                    //Se debe enviar esta respuesta
                    MensajeGlobal Respuesta = new MensajeGlobal();
                    Respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
                    Respuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidadUUID>(RespuestaInterAcuse.Noexiste.GetHashCode()));
                    return Request.CreateResponse(HttpStatusCode.Conflict, Respuesta);
                }

                if (datos.StrProveedorEmisor != Proveedor)
                {
                    //406
                    MensajeGlobal Respuesta = new MensajeGlobal();
                    Respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
                    Respuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidadUUID>(RespuestaInterAcuse.ExisiteParaOtro.GetHashCode()));
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, Respuesta);
                }

                //201
                MensajeGlobal acuse = Ctl_Envio.ObtenerAcusebase64(Guid.Parse(uuid), Proveedor);

                return Request.CreateResponse(HttpStatusCode.Created, acuse);
            }
            catch (Exception ex)
            {
                //500
                MensajeGlobal Respuesta = new MensajeGlobal();
                Respuesta.timeStamp = Fecha.FechaUtc(DateTime.Now);
                Respuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaInterOperabilidadUUID>(RespuestaInterOperabilidadUUID.ErrorInterno.GetHashCode()));
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Respuesta);
            }

        }


    }
}
