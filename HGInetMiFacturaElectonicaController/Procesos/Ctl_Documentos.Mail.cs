using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
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

        public static DocumentoRespuesta MailDocumentos(object documento, TblDocumentos documentoBd, TblEmpresas obligado, bool adquiriente_nuevo, TblEmpresas adquiriente, TblUsuarios adquiriente_usuario, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
        {
            Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();

            try
            {
                var documento_obj = (dynamic)null;
                documento_obj = documento;

                respuesta.DescripcionProceso = string.Format("{0} - En estado EXITOSA", respuesta.DescripcionProceso);

                //Si es nuevo en la Plataforma envia Bienvenida a la plataforma
                if (adquiriente_nuevo == true)
                {
                    email.Bienvenida(adquiriente, adquiriente_usuario);
                }

                //Envia correo al adquiriente que tiene el objeto
                email.NotificacionDocumento(documentoBd, documento_obj.DatosObligado.Telefono, documento_obj.DatosAdquiriente.Email);

                //Actualiza la respuesta
                respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.EnvioEmailAcuse);
                respuesta.FechaUltimoProceso = Fecha.GetFecha();
                respuesta.IdProceso = ProcesoEstado.EnvioEmailAcuse.GetHashCode();

                //Actualiza Documento en Base de Datos
                documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
                documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

                Ctl_Documento documento_tmp = new Ctl_Documento();
                documento_tmp.Actualizar(documentoBd);
				
				//Actualiza la categoria con el nuevo estado
				respuesta.IdCategoriaEstado = documentoBd.IdCategoriaEstado;
				respuesta.DescripcionIdCategoria = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));
            }
            catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el Envío correo adquiriente. Detalle: {0} -", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);

            }
            return respuesta;
        }

        /// <summary>
        /// Valida usuario del adquiriente y hace el envio de la informacion
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="documentoBd"></param>
        /// <param name="empresa"></param>
        /// <param name="respuesta"></param>
        /// <param name="documento_result"></param>
        /// <returns></returns>
        public static DocumentoRespuesta Envio(object documento, TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
        {

            try
            {
                TblUsuarios usuarioBd = null;
                Ctl_Empresa empresa_config = new Ctl_Empresa();

                TblEmpresas adquirienteBd = null;

                //Obtiene la informacion del Adquiriente que se tiene en BD
                adquirienteBd = empresa_config.Obtener(respuesta.Identificacion);

                bool adquiriente_nuevo = false;

                try
                {

                    Ctl_Usuario _usuario = new Ctl_Usuario();

                    usuarioBd = _usuario.ObtenerUsuarios(respuesta.Identificacion, respuesta.Identificacion).FirstOrDefault();

                    //Creacion del Usuario del Adquiriente
                    if (usuarioBd == null)
                    {
                        _usuario = new Ctl_Usuario();
                        usuarioBd = _usuario.Crear(adquirienteBd);

                        adquiriente_nuevo = true;
                    }
                }
                catch (Exception excepcion)
                {
                    string msg_excepcion = Excepcion.Mensaje(excepcion);

                    if (!msg_excepcion.ToLowerInvariant().Contains("insert duplicate key"))
                        throw excepcion;
                    else
                        adquiriente_nuevo = false;
                }

                // envía el mail de documentos y de creación de adquiriente
                respuesta = Ctl_Documentos.MailDocumentos(documento, documentoBd, empresa, adquiriente_nuevo, adquirienteBd, usuarioBd, ref respuesta, ref documento_result);
                ValidarRespuesta(respuesta);

            }
            catch (Exception excepcion)
            {
                respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el envío correo adquiriente. Detalle: {0} -", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
            }

            return respuesta;


        }
    }
}
