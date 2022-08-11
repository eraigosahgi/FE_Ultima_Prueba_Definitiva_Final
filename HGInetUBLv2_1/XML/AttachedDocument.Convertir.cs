using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using LibreriaGlobalHGInet.Funciones;
using HGInetMiFacturaElectonicaData;
using System.Security.Cryptography.X509Certificates;
using LibreriaGlobalHGInet.RegistroLog;

namespace HGInetUBLv2_1
{
	public partial class AttachedDocument
	{
		public static Attached Convertir(AttachedDocumentType attach_ubl)
		{
			Attached attach_document = new Attached();

			try
			{
				attach_document.IdAttached = attach_ubl.ID.Value;
				DateTime hora = new DateTime();
				try
				{
					hora = attach_ubl.IssueTime != null ? Convert.ToDateTime(attach_ubl.IssueTime.Value).AddHours(-5) : new DateTime();
				}
				catch (Exception)
				{
				}
				DateTime fecha = attach_ubl.IssueDate.Value;
				DateTime fecha_hora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);
				attach_document.Fecha = fecha_hora;

				//Documento con Prefijo
				Match numero_doc = Regex.Match(attach_ubl.ParentDocumentID.Value, "\\d+");

				try
				{
					attach_document.Documento = Convert.ToInt64(numero_doc.Value);
					attach_document.Prefijo = attach_ubl.ParentDocumentID.Value.Substring(0, attach_ubl.ParentDocumentID.Value.Length - attach_document.Documento.ToString().Length);
				}
				catch (Exception)
				{
					numero_doc = Regex.Match(attach_ubl.ParentDocumentLineReference[0].DocumentReference.ID.Value, "\\d+");
					attach_document.Documento = Convert.ToInt64(numero_doc.Value);
					attach_document.Prefijo = attach_ubl.ParentDocumentLineReference[0].DocumentReference.ID.Value.Substring(0, attach_ubl.ParentDocumentLineReference[0].DocumentReference.ID.Value.Length - attach_document.Documento.ToString().Length);
				}

				attach_document.CufeDocumentoElectronico = (attach_ubl.ParentDocumentLineReference != null) ? (attach_ubl.ParentDocumentLineReference[0].DocumentReference.UUID != null) ? attach_ubl.ParentDocumentLineReference[0].DocumentReference.UUID.Value : string.Empty: string.Empty;

				if (attach_ubl.SenderParty != null && attach_ubl.SenderParty.PartyTaxScheme != null)
					attach_document.IdentificacionFacturador = attach_ubl.SenderParty.PartyTaxScheme[0].CompanyID.Value;
				else
					throw new ArgumentException("No se encontró identificación del Facturador en el AttachDocument, validar estructura de este tipo de documento");

				if (attach_ubl.ReceiverParty != null && attach_ubl.ReceiverParty.PartyTaxScheme != null)
					attach_document.Identificacionadquiriente = attach_ubl.ReceiverParty.PartyTaxScheme[0].CompanyID.Value;
				else
					throw new ArgumentException("No se encontró identificación del Adquiriente en el AttachDocument, validar estructura de este tipo de documento");

				attach_document.RespuestaDianXml = (attach_ubl.ParentDocumentLineReference != null) ? attach_ubl.ParentDocumentLineReference[0].DocumentReference.Attachment.ExternalReference.Description[0].Value.Trim() : string.Empty;

				attach_document.DocumentoElectronico = attach_ubl.Attachment.ExternalReference.Description[0].Value.Trim();
			}
			catch (Exception ex)
			{
				string mensaje = string.Format("Se presento inconsistencia convirtiendo el xml de attach a objeto. Detalle: {0}", ex.Message);
				RegistroLog.EscribirLog(ex, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna, mensaje);
				throw new ApplicationException(mensaje, ex.InnerException);
			}

			return attach_document;
		}


	}
}
