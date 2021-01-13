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

namespace HGInetUBLv2_1
{
	public partial class AttachedDocument
	{
		public static Attached Convertir(AttachedDocumentType attach_ubl)
		{
			Attached attach_document = new Attached();

			attach_document.IdAttached = attach_ubl.ID.Value;
			DateTime hora = Convert.ToDateTime(attach_ubl.IssueTime.Value).AddHours(-5);
			DateTime fecha = attach_ubl.IssueDate.Value;
			DateTime fecha_hora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);
			attach_document.Fecha = fecha_hora;

			//Documento con Prefijo
			string documento = attach_ubl.ParentDocumentID.Value;

			Match numero_doc = Regex.Match(attach_ubl.ParentDocumentID.Value, "\\d+");

			attach_document.Documento = Convert.ToInt64(numero_doc.Value);

			attach_document.Prefijo = attach_ubl.ParentDocumentID.Value.Substring(0, attach_ubl.ParentDocumentID.Value.Length - attach_document.Documento.ToString().Length);

			attach_document.CufeDocumentoElectronico = attach_ubl.ParentDocumentLineReference[0].DocumentReference.UUID.Value;

			attach_document.IdentificacionFacturador = attach_ubl.SenderParty.PartyTaxScheme[0].CompanyID.Value;

			attach_document.Identificacionadquiriente = attach_ubl.ReceiverParty.PartyTaxScheme[0].CompanyID.Value;

			attach_document.RespuestaDianXml = attach_ubl.ParentDocumentLineReference[0].DocumentReference.Attachment.ExternalReference.Description[0].Value;

			attach_document.DocumentoElectronico = attach_ubl.Attachment.ExternalReference.Description[0].Value;

			return attach_document;
		}


	}
}
