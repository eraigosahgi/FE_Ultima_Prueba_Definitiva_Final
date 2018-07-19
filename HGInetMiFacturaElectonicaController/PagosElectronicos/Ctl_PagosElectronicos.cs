using HGInetZonaPagos.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.PagosElectronicos
{
    public class Ctl_PagosElectronicos
    {

        /// <summary>
        /// Realiza el registro del pago en la plataforma electrónica.
        /// Consulta el documento por id de seguridad para la construcción del objeto de envío de los datos del pago.
        /// Retorna la url con el id virtual para la redirección a la página de inicio del proceso de pago.
        /// </summary>
        /// <param name="id_seguridad_documento">id de seguridad del documento.</param>
        /// <returns></returns>
        public string ReportePagoElectronico(System.Guid id_seguridad_documento)
        {
            //Ruta retornada por el servicio, redirecciona a la  página de inicio del pago (Selección de tipo persona, forma pago y banco).
            string ruta_inicio = string.Empty;

            //Objetos para reportar el pago
            HGInetZonaPagos.Cliente datos_cliente = new HGInetZonaPagos.Cliente();
            HGInetZonaPagos.Pago datos_pago = new HGInetZonaPagos.Pago();

            //LOS CAMPOS EMAIL Y TELEFONO DEL CLIENTE DEBEN SER OBLIGATORIOS.

            // EL PARAMETRO ID_PAGO DEL OBJETO PAGO (datos_pago), CORRESPONDE AL ID DE SEGURIDAD DEL DOCUMENTO.
            //info_opcional1, info_opcional2 Y info_opcional3 EN DEMO NO PUEDEN SER NULOS.

            //LAS LISTAS lista_codigos_servicio_multicredito, lista_nit_codigos_servicio_multicredito, lista_valores_con_iva Y lista_valores_iva, EN OBJETO PAGO (datos_pago) PUEDEN SER NULL


            //Datos de la pasarela electrónica.
            int comercio_id = 2651;
            string comercio_clave = "2651HGI";
            string comercio_ruta = "t_pruebasHGI";

            ProcesoPago clase_proceso_pago = new ProcesoPago(comercio_id, comercio_clave, comercio_ruta);

            ruta_inicio = clase_proceso_pago.ReportarPago(datos_cliente, datos_pago);

            return ruta_inicio;
        }

    }
}
