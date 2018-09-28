using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Servicios
{
    public static class Ctl_ClienteWebApi
    {
        #region Metodos para consumir Api de interoperabilidad


        /// <summary>
        /// Se conecta a la plataforma de un proveedor tecnologico especifico
        /// y obtiene el token de seguridad que se debe enviar en cada petición
        /// </summary>
        /// <param name="usuario">Usuario suministrado por el proveedor tecnologico con su respectiva clave. todo en formato json</param>
        /// <param name="Dominio">Dominio del proveedor</param>
        /// <returns></returns>
        public static string Inter_login(string usuario, string Dominio)
        {
            try
            {

                string postData = usuario.ToString();
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Dominio + "/interoperabilidad/api/v1_0/login");

                request.Method = "POST";
                request.ContentType = "application/json";


                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] bytes = encoding.GetBytes(postData);

                request.ContentLength = bytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                string resp = string.Empty;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resp = reader.ReadToEnd();
                }
                return resp;
            }

            catch (Exception Exception)
            {

                return Exception.Message;
            }
        }

        /// <summary>
        /// Obtiene el estado de un documento enviado  a otro proveedor tecnologico
        /// </summary>
        /// <param name="UUID">traking id o UUID de seguridad suministrado por el proveedor tecnologico receptor</param>
        /// <param name="Token">Token de seguridad emitido por el proveedor tecnologico receptor</param>
        /// <returns></returns>
        public static string Inter_ConsultarEstado(string UUID, string Token, string Dominio)
        {
            //Consultar documentos

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Dominio + "/interoperabilidad/api/v1_0/Consultar?UUID");

                request.Method = "GET";
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";
                request.Headers.Add("Authorization", "Bearer " + Token);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                string resp = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resp = reader.ReadToEnd();
                }



                return resp;
            }
            catch (Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// cambia la contraseña en un proveedor tecnologico especifico
        /// </summary>
        /// <param name="NITProveedor"></param>
        /// <param name="ContrasenaNueva"></param>
        /// <param name="ContrasenaActual"></param>
        /// <returns></returns>
        public static string Inter_CambiarContrasena(string NITProveedor, string ContrasenaNueva, string ContrasenaActual, string Dominio)
        {
            //Cambiar Clave
            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Dominio + "interoperabilidad/api/v1_0/cambioContrasena");

                request.Method = "PUT";
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";

                request.Headers.Add("NITProveedor:" + NITProveedor);
                request.Headers.Add("ContrasenaNueva:" + ContrasenaNueva);
                request.Headers.Add("ContrasenaActual:" + ContrasenaActual);


                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] bytes = encoding.GetBytes(request.Headers.ToString());

                request.ContentLength = bytes.Length;


                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                string resp = string.Empty;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resp = reader.ReadToEnd();
                }


                return resp;
            }
            catch (Exception)
            {

                throw;
            }
        }



        /// <summary>
        /// Envia los documentos a un proveedor tecnologico para el registro en su plataforma
        /// Este es el caso cuando un documento llega a nuestra plataforma y el facturador 
        /// tiene otro proveedor tecnologico
        /// </summary>
        /// <param name="ListaDocumentos">Json con los documentos a enviar</param>
        /// <param name="Token">Token emitido en la autenticación con el proveedor tecnologico</param>
        /// <param name="Dominio">Dominio del proveedor tecnologico al que se desea enviar los documentos</param>
        /// <returns></returns>
        public static string Inter_Registrar(string ListaDocumentos, string Token, string Dominio)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Dominio + "/interoperabilidad/api/v1_0/Registrar");

                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";

                request.Headers.Add("Authorization", "Bearer " + Token);

                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] bytes = encoding.GetBytes(ListaDocumentos);

                request.ContentLength = bytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                string resp = string.Empty;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resp = reader.ReadToEnd();
                }

                return resp;
            }
            catch (Exception exception)
            {

                return exception.Message;
            }
        }

        public static string Inter_ConsultaAcuse(string UUID, string Token, string Dominio)
        {
            //Consultar documentos
            try
            {

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Dominio + "/interoperabilidad/api/v1_0/application?UUID");

                request.Method = "GET";
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json; charset=utf-8";
                request.Headers.Add("Authorization", "Bearer " + Token);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                string resp = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    resp = reader.ReadToEnd();
                }

                return resp;
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion
    }
}
