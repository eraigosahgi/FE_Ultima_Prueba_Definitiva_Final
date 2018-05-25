using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
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
				{	email.Bienvenida(adquiriente, adquiriente_usuario);
				}

				if (obligado.IntHabilitacion < 99)
				{	//envío de los documentos al Obligado
					email.NotificacionDocumento(documentoBd, documento_obj.DatosObligado.Telefono, documento_obj.DatosAdquiriente.Email);
				}
				else
				{	//envío de los documentos al Adquiriente
					email.NotificacionDocumento(documentoBd, documento_obj.DatosObligado.Telefono);
				}

				//Actualiza la respuesta
				respuesta.DescripcionProceso = "Envío correo adquiriente.";
				respuesta.FechaUltimoProceso = Fecha.GetFecha();
				respuesta.IdProceso = ProcesoEstado.EnvioEmailAcuse.GetHashCode();

				//Actualiza Documento en Base de Datos
				documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
				documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

				Ctl_Documento documento_tmp = new Ctl_Documento();
				documento_tmp.Actualizar(documentoBd);
			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el Envío correo adquiriente. Detalle: {0} -", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				
			}
			return respuesta;
		}



    }
}
