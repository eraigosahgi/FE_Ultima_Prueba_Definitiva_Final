using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
    public partial class Ctl_Documentos
    {

        /// <summary>
        /// Valida la información del documento como objeto
        /// </summary>
        /// <param name="documento_obj">información del documento</param>
        /// <param name="tipo_doc">tipo de documento</param>
        /// <param name="resolucion">información de la resolución</param>
        /// <param name="respuesta">datos de respuesta del documento</param>
        /// <returns>información adicional de respuesta del documento</returns>
        public static DocumentoRespuesta Validar(object documento_obj, TipoDocumento tipo_doc, TblEmpresasResoluciones resolucion, ref DocumentoRespuesta respuesta)
        {
            try
            {
                respuesta.DescripcionProceso = "Valida la información del documento.";
                respuesta.FechaUltimoProceso = Fecha.GetFecha();
                respuesta.IdProceso = ProcesoEstado.Validacion.GetHashCode();
                
                if (tipo_doc == TipoDocumento.Factura)
                    documento_obj = Validar((Factura)documento_obj, resolucion);
                else if (tipo_doc == TipoDocumento.NotaCredito)
                    documento_obj = ValidarNotaCredito((NotaCredito)documento_obj, resolucion);
				else if (tipo_doc == TipoDocumento.NotaDebito)
					documento_obj = ValidarNotaDebito((NotaDebito)documento_obj, resolucion);


			}
            catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en la validación del documento. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				LogExcepcion.Guardar(excepcion);
			}
            return respuesta;
        }

        /// <summary>
        /// Valida si la respuesta generó error 
        /// </summary>
        /// <param name="respuesta">información de respuesta</param>
        public static void ValidarRespuesta(DocumentoRespuesta respuesta)
        {
            if (respuesta.Error != null)
                if (!string.IsNullOrWhiteSpace(respuesta.Error.Mensaje))
                    throw new ApplicationException(respuesta.Error.Mensaje);
        }
    }
}
