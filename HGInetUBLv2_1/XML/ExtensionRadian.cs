using HGInetMiFacturaElectonicaData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBLv2_1
{
	public class ExtensionRadian
	{
		public static XmlElement ObtenerRadianInscripcion(decimal valor_fe, CodigoResponseV2 tipo_acuse)
		{

			CustomTagGeneral1 TagGeneral = new CustomTagGeneral1();
			XmlDocument extension_sector = new XmlDocument();
			StreamWriter stWriter = null;
			XmlSerializer xmlSerializer;
			string buffer;
			MemoryStream memStream = new MemoryStream();
			XmlNode root = null;
			XmlNode nodo_name = null;
			XmlNode nodo_value = null;
			InformacionNegociacionType informacion = new InformacionNegociacionType();
			//XmlNode nodo_name1 = null;
			//XmlNode nodo_value1 = null;
			//XmlNode nodo_name2 = null;
			//XmlNode nodo_value2 = null;
			//XmlNode nodo_name3 = null;
			//XmlNode nodo_value3 = null;

			switch (tipo_acuse)
			{
				case CodigoResponseV2.Inscripcion:

					ConstanciadePagosType Constancia = new ConstanciadePagosType();
					Constancia.Name = new Name();
					Constancia.Name.Value = "ValorFEV-TV";
					Constancia.Value = new Value();
					Constancia.Value.Value = valor_fe.ToString().Replace(",", ".");

					TagGeneral.ConstanciadePagos = Constancia;

					xmlSerializer = new XmlSerializer(TagGeneral.GetType());
					stWriter = new StreamWriter(memStream);

					xmlSerializer.Serialize(stWriter, TagGeneral);
					buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

					extension_sector.LoadXml(buffer);

					extension_sector.DocumentElement.RemoveAllAttributes();

					root = extension_sector.DocumentElement.FirstChild;

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "ValorPagado";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					nodo_value.InnerText = "0.00";
					root.AppendChild(nodo_value);

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "NuevoValorTV";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					nodo_value.InnerText = valor_fe.ToString().Replace(",", ".");
					root.AppendChild(nodo_value);

					break;
				case CodigoResponseV2.EndosoPp:

					//informacion = new InformacionNegociacionType();

					informacion.Name = new Name();
					informacion.Name.Value = "ValorTotalEndoso";
					informacion.Value = new Value();
					informacion.Value.Value = valor_fe.ToString().Replace(",", ".");

					TagGeneral.InformacionNegociacion = informacion;

					xmlSerializer = new XmlSerializer(TagGeneral.GetType());

					stWriter = new StreamWriter(memStream);

					xmlSerializer.Serialize(stWriter, TagGeneral);
					buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

					extension_sector.LoadXml(buffer);

					extension_sector.DocumentElement.RemoveAllAttributes();

					root = extension_sector.DocumentElement.FirstChild;

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "PrecioPagarseFEV";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					double pagoend = (double)valor_fe;
					pagoend = (pagoend - (pagoend * (0.05)));
					nodo_value.InnerText = pagoend.ToString().Replace(",", ".");
					root.AppendChild(nodo_value);

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "TasaDescuento";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					nodo_value.InnerText = "5";
					root.AppendChild(nodo_value);

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "MedioPago";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					nodo_value.InnerText = "13";
					root.AppendChild(nodo_value);

					break;
				case CodigoResponseV2.EndosoG:
				case CodigoResponseV2.EndosoPc:

					//informacion = new InformacionNegociacionType();

					informacion.Name = new Name();
					informacion.Name.Value = "ValorTotalEndoso";
					informacion.Value = new Value();
					informacion.Value.Value = valor_fe.ToString().Replace(",", ".");

					TagGeneral.InformacionNegociacion = informacion;

					xmlSerializer = new XmlSerializer(TagGeneral.GetType());

					stWriter = new StreamWriter(memStream);

					xmlSerializer.Serialize(stWriter, TagGeneral);
					buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

					extension_sector.LoadXml(buffer);

					extension_sector.DocumentElement.RemoveAllAttributes();

					root = extension_sector.DocumentElement.FirstChild;

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "PrecioPagarseFEV";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					nodo_value.InnerText = string.Empty;
					root.AppendChild(nodo_value);

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "TasaDescuento";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					nodo_value.InnerText = string.Empty; ;
					root.AppendChild(nodo_value);

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "MedioPago";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					nodo_value.InnerText = string.Empty; ;
					root.AppendChild(nodo_value);

					break;
				default:
					break;
			}

			

			return extension_sector.DocumentElement;
		}

	}
}
