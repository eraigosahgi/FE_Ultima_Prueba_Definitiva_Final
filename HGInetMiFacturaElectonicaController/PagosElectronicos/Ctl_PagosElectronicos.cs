using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
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

            //Datos de la pasarela electrónica.
            int comercio_id = 2651;
            string comercio_clave = "2651HGI";
            string comercio_ruta = "t_pruebasHGI";
            string codigo_servicio = "2701";

            Ctl_Documento clase_documento = new Ctl_Documento();
            TblDocumentos datos_documento = clase_documento.ObtenerPorIdSeguridad(id_seguridad_documento).FirstOrDefault();

            //Objetos para reportar el pago
            HGInetZonaPagos.Cliente datos_cliente = new HGInetZonaPagos.Cliente();
            datos_cliente.email = datos_documento.TblEmpresasAdquiriente.StrMail;
            datos_cliente.id_cliente = datos_documento.StrEmpresaAdquiriente;
            datos_cliente.nombre = datos_documento.TblEmpresasAdquiriente.StrRazonSocial;
            //Empresa no tiene telefono.
            datos_cliente.telefono = "444 45 84";
            datos_cliente.tipo_id = datos_documento.IntDocTipo.ToString();

            HGInetZonaPagos.Pago datos_pago = new HGInetZonaPagos.Pago();

            datos_pago.id_pago = id_seguridad_documento.ToString();
            datos_pago.descripcion_pago = string.Format("{0}", datos_pago.id_pago);
            datos_pago.total_con_iva = Convert.ToDouble(datos_documento.IntVlrTotal);
            datos_pago.valor_iva = 0;
            datos_pago.codigo_servicio_principal = codigo_servicio;
            datos_pago.info_opcional1 = "";
            datos_pago.info_opcional2 = "";
            datos_pago.info_opcional3 = "";
            datos_pago.lista_codigos_servicio_multicredito = null;
            datos_pago.lista_nit_codigos_servicio_multicredito = null;
            datos_pago.lista_valores_con_iva = null;
            datos_pago.lista_valores_iva = null;
            datos_pago.total_codigos_servicio = 0;

            ProcesoPago clase_proceso_pago = new ProcesoPago(comercio_id, comercio_clave, comercio_ruta);

            ruta_inicio = clase_proceso_pago.ReportarPago(datos_cliente, datos_pago, true);

            return ruta_inicio;
        }

    }
}
