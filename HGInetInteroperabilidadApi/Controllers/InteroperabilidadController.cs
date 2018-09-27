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

            if (CheckUser(usuario.username, usuario.password))
            {                

                AutenticacionRespuesta respuesta = new AutenticacionRespuesta();

                respuesta.jwtToken = JwtManager.GenerateToken(usuario.username,"811021438", "HGI SAS");
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

            var usernameClaim = identity.FindFirst("iss");

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
            var usernameClaim = identity.FindFirst("iss");

            //Aqui envio los datos al controlador de proceso
            string Proveedor = usernameClaim.Value;


            RegistroListaDocRespuesta respuesta = Ctl_Recepcion.Procesar(registroRespuesta, @"E:\Desarrollo\jzea\Ubl-Lmsoftware","");


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

            var usernameClaim = identity.FindFirst("iss");

            //Aqui debo generar el proceso que valida el estado del documento


            //Se debe enviar esta respuesta
            MensajeGlobal Respuesta = new MensajeGlobal();

            Respuesta.timeStamp = Fecha.GetFecha();
            Respuesta.mensajeGlobal = "QWxleCBDaGFjw7NuIFNvZnR3YXJlIENvbG9tYmlhDQo8ZGl2IGlkPSJsb2dpbmJveCIgc3R5bGU9Im1hcmdpbi10b3A6IDEwcHg7Ig0KCWNsYXNzPSJtY WluYm94IGNvbC1tZC0xMiI+DQoJPGRpdiBjbGFzcz0icGFuZWwgcGFuZWwtaW5mbyI+DQoJCTxkaXYgY2xhc3M9InBhbmVsLWhlYWRpbmciPg0KCQk JPGRpdiBjbGFzcz0icGFuZWwtdGl0bGUiPkNyZWRlbmNpYWxlcyBkZSBhY2Nlc288L2Rpdj4NCgkJPC9kaXY+DQoJCTxkaXYgc3R5bGU9InBhZGRpbmc tdG9wOiAzMHB4IiBjbGFzcz0icGFuZWwtYm9keSI+DQoJCQk8Zm9ybSBuYW1lPSJsb2dpbkZvcm0iIGNsYXNzPSJmb3JtLWhvcml6b250YWwiIG1ldGhvZ D0icG9zdCINCgkJCQluZy1zdWJtaXQ9InN1Ym1pdCgpIiBub3ZhbGlkYXRlIGZvcm0tYXV0b2ZpbGwtZml4Pg0KCQkJCQ0KCQkJCTxkaXYgc3R5bGU9Im 1hcmdpbi10b3A6IDI1cHgiIGNsYXNzPSJpbnB1dC1ncm91cCI+DQoJCQkJCTxzcGFuIGNsYXNzPS";

            return Ok(Respuesta);

        }





        //Validacion de inicio de session del proveedor tecnologico
        public bool CheckUser(string username, string password)
        {
            // should check in the database
            return true;
        }







    }
}
