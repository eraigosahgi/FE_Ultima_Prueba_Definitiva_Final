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
        public IHttpActionResult login(Usuario usuario)
        {
            TblConfiguracionInteroperabilidad Proveedor = new TblConfiguracionInteroperabilidad();
            Ctl_ConfiguracionProveedores conf = new Ctl_ConfiguracionProveedores();                       

            Proveedor = conf.CheckUser(usuario.username, usuario.password);

            if (Proveedor != null)
            {
                AutenticacionRespuesta respuesta = new AutenticacionRespuesta();

                respuesta.jwtToken = JwtManager.GenerateToken("811021438", Proveedor.StrIdentificacion, Proveedor.StrRazonSocial);
                respuesta.passwordExpiration = Fecha.GetFecha();

                return Ok(respuesta);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Consultar documento por UUID, aqui debo retornar un objeto que contiene el historico del Documento 
        /// GET ​ ​https://[Dominio_Proveedor]/interoperabilidad/api/v1_0/consultar/{UUID}
        /// </summary>
        /// <param name="UUID">id de seguridad del documento</param>
        /// <returns></returns>
        [JwtAuthentication]                
        [HttpGet]
        [Route("interoperabilidad/api/v1_0/Consultar")]
        public IHttpActionResult Consultar(string UUID)
        {            
            var identity = User?.Identity as ClaimsIdentity;

            var usernameClaim = identity.FindFirst("username");

            //Aqui envio los datos al controlador de proceso
            string Proveedor = usernameClaim.Value;

            List<DetalleDocRespuesta> ListaRespuesta = new List<DetalleDocRespuesta>();

            RegistroDocRespuesta docRespuesta = new RegistroDocRespuesta();

            TblDocumentos doc = new TblDocumentos();

            Ctl_Documento Documentos = new Ctl_Documento();

            doc = Documentos.ObtenerHistorialDococumento(Guid.Parse(UUID), Proveedor);

            //No existe Documento
            if (doc == null)
            {                
                docRespuesta.mensajeGlobal = string.Format("{0} . {1}", RespuestaInterOperabilidadUUID.Noexiste.GetHashCode(), Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue< RespuestaInterOperabilidadUUID> (RespuestaInterOperabilidadUUID.Noexiste.GetHashCode()))); 
            }
            else
            {
                //Documento Radicado                
                DetalleDocRespuesta Resp_Radicado = new DetalleDocRespuesta();
                Resp_Radicado.nombre = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDDocumento>(RespuestaUUIDDocumento.Radicado.GetHashCode()));                
                Resp_Radicado.mensaje = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(RespuestaUUIDDocumento.Radicado.GetHashCode()));
                Resp_Radicado.timeStamp = doc.DatFechaIngreso;
                ListaRespuesta.Add(Resp_Radicado);

                //Mensajes Global
                docRespuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(RespuestaUUIDDocumento.Radicado.GetHashCode()));
             
                //Documento Aprovado o Rechazado
                if (doc.IntAdquirienteRecibo > 0)
                {                    
                    DetalleDocRespuesta Resp = new DetalleDocRespuesta();
                    Resp.nombre = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDDocumento>(doc.IntAdquirienteRecibo));
                    Resp.mensaje = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(doc.IntAdquirienteRecibo));
                    Resp.timeStamp = Convert.ToDateTime(doc.DatAdquirienteFechaRecibo);
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
                    Resp_pagado.timeStamp = doc.DatFechaActualizaEstado;                    
                    ListaRespuesta.Add(Resp_pagado);

                    //Mensajes Global
                    docRespuesta.mensajeGlobal = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<RespuestaUUIDMensaje>(RespuestaUUIDDocumento.Pagado.GetHashCode()));
                }

                //Encabezado de la respuesta                             
                docRespuesta.timeStamp = Fecha.GetFecha();
                docRespuesta.historial = ListaRespuesta;
            }

            return Ok(docRespuesta);

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
        [Route("interoperabilidad/api/v1_0/Registrar")]
        public IHttpActionResult Registrar(RegistroListaDoc registroRespuesta)
        {

            var identity = User?.Identity as ClaimsIdentity;
            var usernameClaim = identity.FindFirst("username");

            //Aqui envio los datos al controlador de proceso
            string Proveedor = usernameClaim.Value;

            RegistroListaDocRespuesta respuesta = Ctl_Descomprimir.Procesar(registroRespuesta, Proveedor);

            //Aqui recibo la respuesta y la envio

            return Ok(respuesta);

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
        public IHttpActionResult cambioContrasena()
        {

            try
            {
                System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                string NITProveedor = string.Empty;
                string ContrasenaNueva = string.Empty;
                string ContrasenaActual = string.Empty;

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
                MensajeGlobal Respuesta = new MensajeGlobal();
                Respuesta.timeStamp = Fecha.GetFecha();

                Ctl_ConfiguracionInteroperabilidad Controlador = new Ctl_ConfiguracionInteroperabilidad();


                TblConfiguracionInteroperabilidad Datos = Controlador.CambiarContraseña(NITProveedor, ContrasenaActual, ContrasenaNueva);

                //201 Contraseña actualizada satisfactoriamente Object
                //409 Usuario o contraseña inexistentes Object
                //406 La nueva contraseña no cumple con las políticas mínimas de seguridad Object
                //500 Error interno del receptor del documento electrónico

                Respuesta.timeStamp = Fecha.GetFecha();

                if (Datos == null)
                {
                    Respuesta.mensajeGlobal = "409 Usuario o contraseña inexistentes";
                    return Ok(Respuesta);
                }

                Respuesta.mensajeGlobal = "201 Contraseña actualizada satisfactoriamente";
                //Aqui recibo la respuesta y la envio

                return Ok(Respuesta);
            }
            catch (Exception)
            {
                //En caso de generar un error
                MensajeGlobal Respuesta = new MensajeGlobal();
                Respuesta.mensajeGlobal = "500 Error interno del receptor del documento electrónico";
                Respuesta.timeStamp = Fecha.GetFecha();
                return Ok(Respuesta);

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
        [Route("interoperabilidad/api/v1_0/application")]
        public IHttpActionResult application(string UUID)
        {
            var identity = User?.Identity as ClaimsIdentity;

            var usernameClaim = identity.FindFirst("username");

            string Proveedor = usernameClaim.Value;
            //Aqui debo generar el proceso que valida el estado del documento

            MensajeGlobal acuse = Ctl_Envio.ObtenerAcusebase64(Guid.Parse(UUID), Proveedor);

            return Ok(acuse);
          
        }


    }
}
