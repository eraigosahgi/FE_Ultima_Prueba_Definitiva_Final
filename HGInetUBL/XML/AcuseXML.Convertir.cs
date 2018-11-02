using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

				Match numero_doc = Regex.Match(acuse.DocumentResponse[0].DocumentReference.ID.Value, "\\d+");

				doc_acuse.Documento = Convert.ToInt64(numero_doc.Value);

				doc_acuse.Prefijo = acuse.DocumentResponse[0].DocumentReference.ID.Value.Substring(0, acuse.DocumentResponse[0].DocumentReference.ID.Value.Length - doc_acuse.Documento.ToString().Length);


				doc_acuse.IdAcuse = acuse.ID.Value;
                doc_acuse.IdSeguridad = acuse.DocumentResponse[0].DocumentReference.UUID.Value;
               
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

				DateTime fecha_acuse = acuse.IssueDate.Value;
				var hora_acuse = acuse.IssueTime.Value.Split(':');

				fecha_acuse = fecha_acuse.AddHours(Convert.ToDouble(hora_acuse[0])).AddMinutes(Convert.ToDouble(hora_acuse[1])).AddSeconds(Convert.ToDouble(hora_acuse[2]));
				doc_acuse.Fecha = Convert.ToDateTime(fecha_acuse);



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
