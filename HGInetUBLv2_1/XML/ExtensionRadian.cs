﻿using HGInetMiFacturaElectonicaData;
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
		public static XmlElement ObtenerRadianInscripcion(decimal valor_fe, CodigoResponseV2 tipo_acuse, decimal tasa_descuento)
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
					pagoend = (pagoend - (pagoend * ((double)tasa_descuento / 100)));
					nodo_value.InnerText = pagoend.ToString().Replace(",", ".");
					root.AppendChild(nodo_value);

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "TasaDescuento";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					nodo_value.InnerText = tasa_descuento.ToString().Replace(",",".");
					root.AppendChild(nodo_value);

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "MedioPago";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					nodo_value.InnerText = "47";
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
				case CodigoResponseV2.InformePago:

					InformacionParaelPagoType informe_pago = new InformacionParaelPagoType();
					informe_pago.Name = new Name();
					informe_pago.Name.Value = "ValorFEV-TV";
					informe_pago.Value = new Value();
					informe_pago.Value.Value = valor_fe.ToString().Replace(",", ".");

					TagGeneral.InformacionParaelPago = informe_pago;

					xmlSerializer = new XmlSerializer(TagGeneral.GetType());
					stWriter = new StreamWriter(memStream);

					xmlSerializer.Serialize(stWriter, TagGeneral);
					buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

					extension_sector.LoadXml(buffer);

					extension_sector.DocumentElement.RemoveAllAttributes();

					root = extension_sector.DocumentElement.FirstChild;

					break;
				case CodigoResponseV2.Aval:

					InformacionAvalarType informacion_aval = new InformacionAvalarType();

					informacion_aval.Name = new Name();
					informacion_aval.Name.Value = "ValorFEVavala";
					informacion_aval.Value = new Value();
					informacion_aval.Value.Value = valor_fe.ToString().Replace(",", ".");

					TagGeneral.InformacionAvalar = informacion_aval;

					xmlSerializer = new XmlSerializer(TagGeneral.GetType());

					stWriter = new StreamWriter(memStream);

					xmlSerializer.Serialize(stWriter, TagGeneral);
					buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

					extension_sector.LoadXml(buffer);

					extension_sector.DocumentElement.RemoveAllAttributes();

					root = extension_sector.DocumentElement.FirstChild;

					break;
				case CodigoResponseV2.PagoFvTV:

					InformacionPagoType pago = new InformacionPagoType();
					pago.Name = new Name();
					pago.Name.Value = "ValorActualTituloValor";
					pago.Value = new Value();
					pago.Value.Value = valor_fe.ToString().Replace(",", ".");

					TagGeneral.InformacionPagos = pago;

					xmlSerializer = new XmlSerializer(TagGeneral.GetType());
					stWriter = new StreamWriter(memStream);

					xmlSerializer.Serialize(stWriter, TagGeneral);
					buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

					extension_sector.LoadXml(buffer);

					extension_sector.DocumentElement.RemoveAllAttributes();

					root = extension_sector.DocumentElement.FirstChild;

					nodo_name = extension_sector.DocumentElement.FirstChild.FirstChild.Clone();
					nodo_name.InnerText = "ValorPendienteTituloValor";
					root.AppendChild(nodo_name);

					nodo_value = extension_sector.DocumentElement.FirstChild.ChildNodes[1].Clone();
					decimal pendiente = valor_fe - tasa_descuento;
					nodo_value.InnerText = pendiente.ToString().Replace(",", "."); //"00";
					root.AppendChild(nodo_value);

					break;
				default:
					break;
			}  


			return extension_sector.DocumentElement;

		}	

	}
}
