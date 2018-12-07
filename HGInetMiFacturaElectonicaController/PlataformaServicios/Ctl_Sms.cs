using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.HgiNet.Controladores;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Respuesta;
using LibreriaGlobalHGInet.Peticiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController
{
    public class Ctl_Sms
    {
        public static void Enviar(string mensaje, string id, List<string> numeros)
        {
            try
            {
                MensajeContenidoSms sms = new MensajeContenidoSms()
                {
                    Contenido = mensaje,
                    Id = id,
                    NumerosCelulares = numeros
                };

                List<MensajeContenidoSms> mensajes_sms = new List<MensajeContenidoSms>();
                mensajes_sms.Add(sms);

                PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
                                
                List<MensajeEnvioSms> respuesta = Ctl_CloudMensajeria.EnviarSms(plataforma.RutaHginetMail, plataforma.LicenciaHGInetMail, plataforma.IdentificacionHGInetMail, mensajes_sms);

            }
            catch (Exception excepcion)
            {
                string msg = excepcion.Message;
            }

        }

        /// <summary>
        /// Valida procesos del documento para hacer el envio de sms
        /// </summary>
        /// <param name="respuesta">objeto respuesta</param>
        /// <param name="id_peticion">Id de la peticion de la Plataforma</param>
        /// <param name="facturador_electronico">Informacion del Facturador Electronico</param>
        /// <param name="documentos">Documentos procesados</param>
        public static void EnviarSms(List<DocumentoRespuesta> respuesta, Guid id_peticion, TblEmpresas facturador_electronico, dynamic documentos)
        {
            int docs_proc = respuesta.Where(_x => _x.IdProceso > ProcesoEstado.Validacion.GetHashCode()).Count();

            if (docs_proc > 0)
            {

                string hora = Fecha.GetFecha().ToString(Fecha.formato_hora);

                string ambiente = Enumeracion.GetEnumObjectByValue<Habilitacion>(Convert.ToInt32(facturador_electronico.IntHabilitacion)).ToString();

                int docs_ok = respuesta.Where(_x => _x.IdProceso == ProcesoEstado.EnvioEmailAcuse.GetHashCode()).Count();

                int docs_error = respuesta.Where(_x => (!(_x.Error.Codigo.Equals(LibreriaGlobalHGInet.Error.CodigoError.OK)) && (_x.Error.Mensaje != ""))).Count();

                int docs_pd = documentos.Count - docs_ok - docs_error;

                string mensaje_sms = hora + " " + "HGInetFacturaE " + facturador_electronico.StrIdentificacion + " " + facturador_electronico.StrRazonSocial
                    + " " + ambiente + " env= " + documentos.Count + " proc= " + docs_proc + " ok= " + docs_ok + " pd= " + docs_pd + " error= " + docs_error;

                if (docs_pd > 0)
                {
                    List<string> celulares = Constantes.SmsCelulares.Split(',').ToList();

                    Enviar(mensaje_sms, id_peticion.ToString(), celulares);
                }

            }
        }

    }
}
