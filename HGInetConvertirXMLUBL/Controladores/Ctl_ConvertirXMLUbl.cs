using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetConvertirXMLUBL.Controladores
{
	public class Ctl_ConvertirXMLUbl
	{

		public static object Convertir(string ruta_url_xml, int tipo_documento)
		{
			var documento_obj = (dynamic)null;

			// lee el archivo XML en UBL desde la ruta pública
			string contenido_xml = Archivo.ObtenerContenido(ruta_url_xml);

			// valida el contenido del archivo
			if (string.IsNullOrWhiteSpace(contenido_xml))
				throw new ArgumentException("El archivo XML UBL se encuentra vacío.");

			// convierte el contenido de texto a xml
			System.Xml.XmlReader xml_reader = System.Xml.XmlReader.Create(new System.IO.StringReader(contenido_xml));

			// convierte el objeto de acuerdo con el tipo de documento
			System.Xml.Serialization.XmlSerializer serializacion = null;

			if (tipo_documento == LibreriaGlobalHGInet.Objetos.TipoDocumento.Factura.GetHashCode())
			{
				serializacion = new System.Xml.Serialization.XmlSerializer(typeof(InvoiceType));

				InvoiceType conversion = (InvoiceType)serializacion.Deserialize(xml_reader);

				documento_obj = HGInetUBLv2_1.FacturaXMLv2_1.Convertir(conversion, null);
				
			}
			else if (tipo_documento == LibreriaGlobalHGInet.Objetos.TipoDocumento.NotaCredito.GetHashCode())
			{
				serializacion = new System.Xml.Serialization.XmlSerializer(typeof(CreditNoteType));

				CreditNoteType conversion = (CreditNoteType)serializacion.Deserialize(xml_reader);

				documento_obj = HGInetUBLv2_1.NotaCreditoXMLv2_1.Convertir(conversion, null);
			}
			else if (tipo_documento == LibreriaGlobalHGInet.Objetos.TipoDocumento.NotaDebito.GetHashCode())
			{
				serializacion = new System.Xml.Serialization.XmlSerializer(typeof(DebitNoteType));

				DebitNoteType conversion = (DebitNoteType)serializacion.Deserialize(xml_reader);

				documento_obj = HGInetUBLv2_1.NotaDebitoXMLv2_1.Convertir(conversion, null);
			}


			return documento_obj;
		}

	}
}
