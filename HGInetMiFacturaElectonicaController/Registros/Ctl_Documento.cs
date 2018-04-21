using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Registros
{
    public class Ctl_Documento : BaseObject<TblDocumentos>
    {
        #region Constructores 

        public Ctl_Documento() : base(new ModeloAutenticacion()) { }
        public Ctl_Documento(ModeloAutenticacion autenticacion) : base(autenticacion) { }

        public Ctl_Documento(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
        #endregion

        /// <summary>
        /// Crea un documento en la Base de Datos
        /// </summary>
        /// <param name="documento"></param>
        /// <returns></returns>
        public TblDocumentos Crear(TblDocumentos documento)
        {
            documento = this.Add(documento);

            return documento;
        }


        public void Validar(TblDocumentos documento)
        {


        }
   
        /// <summary>
        /// Convierte un Objeto de Servicio a un Objeto de Bd
        /// </summary>
        /// <param name="respuesta">Objeto de respuesta</param>
        /// <param name="factura">Objeto enviado por el usuario</param>
        /// <param name="empresa"></param>
        /// <param name="adquiriente"></param>
        /// <param name="tipo_doc"></param>
        /// <returns></returns>
        public static TblDocumentos Convertir(DocumentoRespuesta respuesta, object documento, TblEmpresas empresa,TblEmpresas adquiriente, TipoDocumento tipo_doc)
        {
            try
            {
                //Valida el tipo de documento enviado por el usuario
                var documento_obj = (dynamic)null;

                if (tipo_doc == TipoDocumento.Factura)
                {
                    documento_obj = documento;
                }
                else if (tipo_doc == TipoDocumento.NotaCredito)
                {
                    documento_obj = documento;
                }
                else if (tipo_doc == TipoDocumento.NotaDebito)
                    documento_obj = documento;

                TblDocumentos tbl_documento = new TblDocumentos();

                tbl_documento.DatFechaIngreso = respuesta.FechaRecepcion;
                tbl_documento.IntDocTipo = Convert.ToInt16(tipo_doc);
                tbl_documento.IntNumero = documento_obj.Documento;
                tbl_documento.StrPrefijo = documento_obj.Prefijo;
                tbl_documento.DatFechaDocumento = documento_obj.Fecha;
                tbl_documento.DatFechaVencDocumento = documento_obj.FechaVence;
                tbl_documento.StrObligadoIdRegistro = documento_obj.CodigoRegistro;
                tbl_documento.IntIdEmpresaResolucion = empresa.TblEmpresasResoluciones.FirstOrDefault().IntId;
                tbl_documento.IntIdEmpresa = empresa.IntId;
                tbl_documento.IntIdEmpresaAdquiriente = adquiriente.IntId;
                tbl_documento.StrCufe = respuesta.Cufe;
                tbl_documento.IntVlrTotal = documento_obj.Total;
                tbl_documento.StrIdSeguridad = Guid.NewGuid();
                tbl_documento.IntAdquirienteRecibo = 0;
                tbl_documento.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);
                tbl_documento.DatFechaActualizaEstado = Fecha.GetFecha();
                

                return tbl_documento;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }
       

    }


}
