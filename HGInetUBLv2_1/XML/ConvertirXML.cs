using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectonicaData.ModeloServicio;

namespace HGInetUBLv2_1
{
	public class ConvertirXML
	{

		/// <summary>
		/// Convierte el objeto en texto XML
		/// </summary>
		/// <param name="factura">Objeto de tipo InvoiceType que contiene la informacion de la factura</param>
		/// <param name="namespaces_xml">Namespaces</param>
		/// <param name="tipo_doc">Tipo de documento que se esta convirtiendo</param> 
		/// <returns>Ruta donde se guardo el archivo XML</returns>     
		public static StringBuilder Convertir(object documento, XmlSerializerNamespaces namespaces_xml, TipoDocumento tipo_doc)
		{
			try
			{

				if (documento == null)
					throw new Exception("El documento es inválido.");

				if (namespaces_xml == null)
					throw new Exception("Los Namespaces son inválidos.");

				// representación de datos en objeto
				var documento_obj = (dynamic)null;

				StringBuilder texto_xml = new StringBuilder();

				if (tipo_doc == TipoDocumento.Factura)
				{
					documento_obj = (InvoiceType)documento;
					texto_xml = Xml.Convertir<InvoiceType>(documento_obj, namespaces_xml);
				}
				else if (tipo_doc == TipoDocumento.NotaCredito)
				{
					documento_obj = (CreditNoteType)documento;
					texto_xml = Xml.Convertir<CreditNoteType>(documento_obj, namespaces_xml);
				}
				else if (tipo_doc == TipoDocumento.NotaDebito)
				{
					documento_obj = (DebitNoteType)documento;
					texto_xml = Xml.Convertir<DebitNoteType>(documento_obj, namespaces_xml);
				}
				else if (tipo_doc == TipoDocumento.Attached)
				{
					documento_obj = (AttachedDocumentType)documento;
					texto_xml = Xml.Convertir<AttachedDocumentType>(documento_obj, namespaces_xml);
				}

				return texto_xml;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


	}
}
