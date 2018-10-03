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
            if (Proveedor!=null)
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

            RegistroDocRespuesta datos = new RegistroDocRespuesta();

            Ctl_int_Consulta Consulta = new Ctl_int_Consulta();

            datos = Consulta.ConsultarUUID(usernameClaim.Value, UUID);

            return Ok(datos);

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
            Respuesta.mensajeGlobal = "Contraseña actualizada satisfactoriamente";

            //Aqui recibo la respuesta y la envio

            return Ok(Respuesta);

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

            //Aqui debo generar el proceso que valida el estado del documento


            //Se debe enviar esta respuesta
            MensajeGlobal Respuesta = new MensajeGlobal();

            Respuesta.timeStamp = Fecha.GetFecha();
            Respuesta.mensajeGlobal = @"PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4KPGZlOkFwcGxpY2F0aW9uUmVzcG9uc2UgeG1sbnM6Y2xtNjY0MTE9InVybjp1bjp1bmVjZTp1bmNlZmFjdDpjb2RlbGlzdDpzcGVjaWZpY2F0aW9uOjY2NDExOjIwMDEiIHhtbG5zOmV4dD0idXJuOm9hc2lzOm5hbWVzOnNwZWNpZmljYXRpb246dWJsOnNjaGVtYTp4c2Q6Q29tbW9uRXh0ZW5zaW9uQ29tcG9uZW50cy0yIiB4bWxuczpjbG1JQU5BTUlNRU1lZGlhVHlwZT0idXJuOnVuOnVuZWNlOnVuY2VmYWN0OmNvZGVsaXN0OnNwZWNpZmljYXRpb246SUFOQU1JTUVNZWRpYVR5cGU6MjAwMyIgeG1sbnM6aGFjPSJ1cm46aGdpbmV0Om5hbWVzOnNwZWNpZmljYXRpb246dWJsOmNvbG9tYmlhOnNjaGVtYTp4c2Q6SGdpTmV0QWdncmVnYXRlQ29tcG9uZW50cy0xIiB4bWxuczpzY2hlbWFMb2NhdGlvbj0iaHR0cDovL3d3dy5kaWFuLmdvdi5jby9jb250cmF0b3MvZmFjdHVyYWVsZWN0cm9uaWNhL3YxIGh0dHA6Ly93d3cuZGlhbi5nb3YuY28vbWljcm9zaXRpb3MvZmFjX2VsZWN0cm9uaWNhL2RvY3VtZW50b3MvWFNEL3IxL0RJQU5fVUJMLnhzZCB1cm46dW46dW5lY2U6dW5jZWZhY3Q6ZGF0YTpzcGVjaWZpY2F0aW9uOlVucXVhbGlmaWVkRGF0YVR5cGVzU2NoZW1hTW9kdWxlOjIgaHR0cDovL3d3dy5kaWFuLmdvdi5jby9taWNyb3NpdGlvcy9mYWNfZWxlY3Ryb25pY2EvZG9jdW1lbnRvcy9jb21tb24vVW5xdWFsaWZpZWREYXRhVHlwZVNjaGVtYU1vZHVsZS0yLjAueHNkIHVybjpvYXNpczpuYW1lczpzcGVjaWZpY2F0aW9uOnVibDpzY2hlbWE6eHNkOlF1YWxpZmllZERhdGF0eXBlcy0yIGh0dHA6Ly93d3cuZGlhbi5nb3YuY28vbWljcm9zaXRpb3MvZmFjX2VsZWN0cm9uaWNhL2RvY3VtZW50b3MvY29tbW9uL1VCTC1RdWFsaWZpZWREYXRhdHlwZXMtMi4wLnhzZCIgeG1sbnM6cWR0PSJ1cm46b2FzaXM6bmFtZXM6c3BlY2lmaWNhdGlvbjp1Ymw6c2NoZW1hOnhzZDpRdWFsaWZpZWREYXRhdHlwZXMtMiIgeG1sbnM6aHN0PSJodHRwOi8vd3d3Lm1pZmFjdHVyYWVubGluZWEuY29tLmNvL3YxL1N0cnVjdHVyZXMiIHhtbG5zOnVkdD0idXJuOnVuOnVuZWNlOnVuY2VmYWN0OmRhdGE6c3BlY2lmaWNhdGlvbjpVbnF1YWxpZmllZERhdGFUeXBlc1NjaGVtYU1vZHVsZToyIiB4bWxuczpzdHM9Imh0dHA6Ly93d3cuZGlhbi5nb3YuY28vY29udHJhdG9zL2ZhY3R1cmFlbGVjdHJvbmljYS92MS9TdHJ1Y3R1cmVzIiB4bWxuczpjYWM9InVybjpvYXNpczpuYW1lczpzcGVjaWZpY2F0aW9uOnVibDpzY2hlbWE6eHNkOkNvbW1vbkFnZ3JlZ2F0ZUNvbXBvbmVudHMtMiIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgeG1sbnM6Y2xtNTQyMTc9InVybjp1bjp1bmVjZTp1bmNlZmFjdDpjb2RlbGlzdDpzcGVjaWZpY2F0aW9uOjU0MjE3OjIwMDEiIHhtbG5zOmNiYz0idXJuOm9hc2lzOm5hbWVzOnNwZWNpZmljYXRpb246dWJsOnNjaGVtYTp4c2Q6Q29tbW9uQmFzaWNDb21wb25lbnRzLTIiIHhtbG5zOmZlPSJodHRwOi8vd3d3LmRpYW4uZ292LmNvL2NvbnRyYXRvcy9mYWN0dXJhZWxlY3Ryb25pY2EvdjEiPgogIDxjYmM6VUJMVmVyc2lvbklEPlVCTCAyLjA8L2NiYzpVQkxWZXJzaW9uSUQ+CiAgPGNiYzpQcm9maWxlSUQ+RElBTiAxLjA8L2NiYzpQcm9maWxlSUQ+CiAgPGNiYzpJRD40OTg1Y2ViNi02NTBlLTQyODYtOThlZS0xYWRkYmI0YTcxYzQ8L2NiYzpJRD4KICA8Y2JjOklzc3VlRGF0ZT4yMDE4LTA5LTI4PC9jYmM6SXNzdWVEYXRlPgogIDxjYmM6SXNzdWVUaW1lPjE2OjM4OjM4PC9jYmM6SXNzdWVUaW1lPgogIDxjYmM6Tm90ZT5UcmFja2luZyBJRCBhMzgwMzFmYy1iNWU0LTQ5NTQtOTNlZi0wOGEyNWUyYmE2ZmE8L2NiYzpOb3RlPgogIDxjYWM6U2VuZGVyUGFydHk+CiAgICA8Y2FjOlBhcnR5SWRlbnRpZmljYXRpb24+CiAgICAgIDxjYmM6SUQgc2NoZW1lSUQ9IjEzIiBzY2hlbWVBZ2VuY3lJRD0iMTk1IiBzY2hlbWVBZ2VuY3lOYW1lPSJDTywgRElBTiAoRGlyZWNjaW9uIGRlIEltcHVlc3RvcyB5IEFkdWFuYXMgTmFjaW9uYWxlcykiPjE2ODU8L2NiYzpJRD4KICAgIDwvY2FjOlBhcnR5SWRlbnRpZmljYXRpb24+CiAgICA8Y2FjOlBhcnR5TmFtZT4KICAgICAgPGNiYzpOYW1lPlRhbWF5byBSb2Ryw61ndWV6IEFuYSBNYXLDrWE8L2NiYzpOYW1lPgogICAgPC9jYWM6UGFydHlOYW1lPgogICAgPGZlOkFnZW50UGFydHk+CiAgICAgIDxjYWM6UGFydHlJZGVudGlmaWNhdGlvbj4KICAgICAgICA8Y2JjOklEPjgxMTAyMTQzODwvY2JjOklEPgogICAgICA8L2NhYzpQYXJ0eUlkZW50aWZpY2F0aW9uPgogICAgICA8Y2FjOlBhcnR5TmFtZT4KICAgICAgICA8Y2JjOk5hbWU+RkFDVFVSQURPUiAxPC9jYmM6TmFtZT4KICAgICAgPC9jYWM6UGFydHlOYW1lPgogICAgPC9mZTpBZ2VudFBhcnR5PgogIDwvY2FjOlNlbmRlclBhcnR5PgogIDxjYWM6UmVjZWl2ZXJQYXJ0eT4KICAgIDxjYWM6UGFydHlJZGVudGlmaWNhdGlvbj4KICAgICAgPGNiYzpJRCBzY2hlbWVJRD0iMzEiIHNjaGVtZUFnZW5jeUlEPSIxOTUiIHNjaGVtZUFnZW5jeU5hbWU9IkNPLCBESUFOIChEaXJlY2Npb24gZGUgSW1wdWVzdG9zIHkgQWR1YW5hcyBOYWNpb25hbGVzKSI+ODExMDIxNDM4PC9jYmM6SUQ+CiAgICA8L2NhYzpQYXJ0eUlkZW50aWZpY2F0aW9uPgogICAgPGNhYzpQYXJ0eU5hbWU+CiAgICAgIDxjYmM6TmFtZT5GQUNUVVJBRE9SIDE8L2NiYzpOYW1lPgogICAgPC9jYWM6UGFydHlOYW1lPgogICAgPGZlOkFnZW50UGFydHk+CiAgICAgIDxjYWM6UGFydHlJZGVudGlmaWNhdGlvbj4KICAgICAgICA8Y2JjOklEPjgxMTAyMTQzODwvY2JjOklEPgogICAgICA8L2NhYzpQYXJ0eUlkZW50aWZpY2F0aW9uPgogICAgICA8Y2FjOlBhcnR5TmFtZT4KICAgICAgICA8Y2JjOk5hbWU+RkFDVFVSQURPUiAxPC9jYmM6TmFtZT4KICAgICAgPC9jYWM6UGFydHlOYW1lPgogICAgPC9mZTpBZ2VudFBhcnR5PgogIDwvY2FjOlJlY2VpdmVyUGFydHk+CiAgPGNhYzpEb2N1bWVudFJlc3BvbnNlPgogICAgPGNhYzpSZXNwb25zZT4KICAgICAgPGNiYzpSZWZlcmVuY2VJRD45ODAwMDAxMDQ8L2NiYzpSZWZlcmVuY2VJRD4KICAgICAgPGNiYzpSZXNwb25zZUNvZGU+UkVKRUNURUQ8L2NiYzpSZXNwb25zZUNvZGU+CiAgICAgIDxjYmM6RGVzY3JpcHRpb24+UHJ1ZWJhIG5vbWJyZSBhcmNoaXZvczwvY2JjOkRlc2NyaXB0aW9uPgogICAgPC9jYWM6UmVzcG9uc2U+CiAgICA8Y2FjOkRvY3VtZW50UmVmZXJlbmNlPgogICAgICA8Y2JjOklEPjk4MDAwMDEwNDwvY2JjOklEPgogICAgICA8Y2JjOlVVSUQ+YTM4MDMxZmMtYjVlNC00OTU0LTkzZWYtMDhhMjVlMmJhNmZhPC9jYmM6VVVJRD4KICAgICAgPGNiYzpEb2N1bWVudFR5cGU+RlY8L2NiYzpEb2N1bWVudFR5cGU+CiAgICA8L2NhYzpEb2N1bWVudFJlZmVyZW5jZT4KICA8L2NhYzpEb2N1bWVudFJlc3BvbnNlPgo8L2ZlOkFwcGxpY2F0aW9uUmVzcG9uc2U+";

            return Ok(Respuesta);

        }


    }
}
