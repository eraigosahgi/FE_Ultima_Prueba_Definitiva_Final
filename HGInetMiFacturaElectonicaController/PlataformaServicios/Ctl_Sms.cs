using HGInetEmailServicios.ServicioEnvio;
using HGInetMiFacturaElectonicaData;
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
                //MensajeContenidoSms sms = new MensajeContenidoSms()
                //{
                //    Contenido = mensaje,
                //    Id = id,
                //    NumerosCelulares = numeros
                //};
                
                //List<MensajeContenidoSms> mensajes_sms = new List<MensajeContenidoSms>();
                //mensajes_sms.Add(sms);

                //PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
                
                //List<MensajeEnvioSms> respuesta = HGInetEmailServicios.Ctl_Envio.EnviarSms(plataforma.RutaHginetMail, plataforma.LicenciaHGInetMail, plataforma.IdentificacionHGInetMail, mensajes_sms);

            }
            catch (Exception excepcion)
            {
                string msg = excepcion.Message;
            }

        }

    }
}
