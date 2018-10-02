using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBL
{
    public class AcuseXML
    {

        /// <summary>
        /// Convierte el XML-UBL en un objeto de Servicio 
        /// </summary>
        /// <param name="acuse">Objeto Acuse para convertir</param>
        /// <returns>Objeto tipo Acuse</returns>
        public static Acuse Convertir(ApplicationResponseType acuse)
        {

            Acuse doc_acuse = new Acuse();

            try
            {
                doc_acuse.IdAcuse = acuse.ID.Value;
                doc_acuse.IdSeguridad = acuse.DocumentResponse[0].DocumentReference.UUID.Value;
                doc_acuse.Documento = Convert.ToInt64(acuse.DocumentResponse[0].DocumentReference.ID.Value);
                doc_acuse.TipoDocumento = acuse.DocumentResponse[0].DocumentReference.DocumentType.Value;
                doc_acuse.CodigoRespuesta = acuse.DocumentResponse[0].Response.ResponseCode.Value;
                doc_acuse.MvoRespuesta = acuse.DocumentResponse[0].Response.Description[0].Value;

                Tercero adquiriente = new Tercero();
                adquiriente.Identificacion = acuse.SenderParty.PartyIdentification[0].ID.Value;
                adquiriente.RazonSocial = acuse.SenderParty.PartyName[0].Name.Value;
                doc_acuse.DatosAdquiriente = adquiriente;

                Tercero obligado = new Tercero();
                obligado.Identificacion = acuse.ReceiverParty.PartyIdentification[0].ID.Value;
                obligado.RazonSocial = acuse.ReceiverParty.PartyName[0].Name.Value;
                doc_acuse.DatosObligado = obligado;

                //doc_acuse.Fecha = Convert.ToDateTime(string.Format("{0}{1}", acuse.IssueDate.Value, acuse.IssueTime.Value));
                doc_acuse.Fecha = acuse.IssueDate.Value;



                return doc_acuse;

            }
            catch (Exception ex)
            {
                LogExcepcion.Guardar(ex);
                throw new ApplicationException(ex.Message, ex.InnerException);
            }


        }




    }
}
