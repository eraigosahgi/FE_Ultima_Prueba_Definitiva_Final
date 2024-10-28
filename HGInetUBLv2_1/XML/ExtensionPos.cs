using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBLv2_1
{
	public class ExtensionPos
	{

		public static XmlElement ObtenerSW(FabricanteSoftwarePos datos)
		{

			FabricanteSoftware fabricanteSW = new FabricanteSoftware();
			
			fabricanteSW.InformacionDelFabricanteDelSoftware = new InformacionDelFabricanteDelSoftware();
			InformacionDelFabricanteDelSoftware Informacion = new InformacionDelFabricanteDelSoftware();

			CampoValorType campo_valor = new CampoValorType();

			Informacion.Name = new Name();
			Informacion.Name.Value = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<DatosFijosPosSW>(1));//"NombreApellido";//
			Informacion.Value = new Value();
			Informacion.Value.Value = datos.NombreApellido;


			//name1 = "RazonSocial";
			//value1 = datos.RazonSocial;

			//name2 = "NombreSoftware";
			//value2 = datos.NombreSoftware;

			fabricanteSW.InformacionDelFabricanteDelSoftware = Informacion;
			
			StreamWriter stWriter = null;
			XmlSerializer xmlSerializer;
			string buffer;

			xmlSerializer = new XmlSerializer(fabricanteSW.GetType());
			MemoryStream memStream = new MemoryStream();
			stWriter = new StreamWriter(memStream);

			xmlSerializer.Serialize(stWriter, fabricanteSW);
			buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

			XmlDocument extension_sector = new XmlDocument();
			extension_sector.LoadXml(buffer);

			XmlNode root = extension_sector.DocumentElement.ChildNodes[0];

			extension_sector.DocumentElement.RemoveAllAttributes();

			int j = 2;
			for (int i = 1; i < 3; i++)
			{
				XmlNode nodo_name = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
				nodo_name.InnerText = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<DatosFijosPosSW>(j));
				root.InsertBefore(nodo_name, extension_sector.DocumentElement.ChildNodes[j]);

				XmlNode nodo_value = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
				switch (i)
				{
					case 1:
						nodo_value.InnerText = datos.RazonSocial;//value1;
						break;
					case 2:
						nodo_value.InnerText = datos.NombreSoftware;//value2;
						break;
					default:
						break;
				}
				root.InsertBefore(nodo_value, extension_sector.DocumentElement.ChildNodes[j + 1]);

				j++;
			}

			//XmlNode nodo_name1 = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
			//nodo_name1.InnerText = name1;
			//root.InsertBefore(nodo_name1, extension_sector.DocumentElement.ChildNodes[2]);

			//XmlNode nodo_value1 = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
			//nodo_value1.InnerText = value1;
			//root.InsertBefore(nodo_value1, extension_sector.DocumentElement.ChildNodes[3]);

			//XmlNode nodo_name2 = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
			//nodo_name2.InnerText = name2;
			//root.InsertBefore(nodo_name2, extension_sector.DocumentElement.ChildNodes[4]);

			//XmlNode nodo_value2 = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
			//nodo_value2.InnerText = value2;
			//root.InsertBefore(nodo_value2, extension_sector.DocumentElement.ChildNodes[5]);

			return extension_sector.DocumentElement;
		}

		public static XmlElement ObtenerBenComprador(InfoComprador datos)
		{

			BeneficiosComprador Beneficios = new BeneficiosComprador();
			InformacionBeneficiosComprador Informacion = new InformacionBeneficiosComprador();

			Informacion.Name = new Name();
			Informacion.Name.Value = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<DatosFijosPosBenCom>(1));//"Codigo";//
			Informacion.Value = new Value();
			Informacion.Value.Value = datos.Codigo;

			Beneficios.InformacionBeneficiosComprador = Informacion;

			//name1 = "NombresApellidos";
			//value1 = datos.NombreApellido;

			//name2 = "Puntos";
			//value2 = datos.Puntos;


			//XmlSerializerNamespaces serializer = NamespacesXML.ObtenerExtensionSector("Salud");
			StreamWriter stWriter = null;
			XmlSerializer xmlSerializer;
			string buffer;

			xmlSerializer = new XmlSerializer(Beneficios.GetType());
			MemoryStream memStream = new MemoryStream();
			stWriter = new StreamWriter(memStream);

			xmlSerializer.Serialize(stWriter, Beneficios);
			buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

			XmlDocument extension_sector = new XmlDocument();
			extension_sector.LoadXml(buffer);

			XmlNode root = extension_sector.DocumentElement.ChildNodes[0];

			extension_sector.DocumentElement.RemoveAllAttributes();

			int j = 2;
			for (int i = 1; i < 3; i++)
			{
				XmlNode nodo_name = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
				nodo_name.InnerText = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<DatosFijosPosBenCom>(j));
				root.InsertBefore(nodo_name, extension_sector.DocumentElement.ChildNodes[j]);

				XmlNode nodo_value = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
				switch (i)
				{
					case 1:
						nodo_value.InnerText = datos.NombreApellido;//value1;
						break;
					case 2:
						nodo_value.InnerText = datos.Puntos;//value2;
						break;
					default:
						break;
				}
				root.InsertBefore(nodo_value, extension_sector.DocumentElement.ChildNodes[j + 1]);

				j++;
			}

			//XmlNode nodo_name1 = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
			//nodo_name1.InnerText = name1;
			//root.InsertBefore(nodo_name1, extension_sector.DocumentElement.ChildNodes[2]);

			//XmlNode nodo_value1 = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
			//nodo_value1.InnerText = value1;
			//root.InsertBefore(nodo_value1, extension_sector.DocumentElement.ChildNodes[3]);

			//XmlNode nodo_name2 = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
			//nodo_name2.InnerText = name2;
			//root.InsertBefore(nodo_name2, extension_sector.DocumentElement.ChildNodes[4]);

			//XmlNode nodo_value2 = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
			//nodo_value2.InnerText = value2;
			//root.InsertBefore(nodo_value2, extension_sector.DocumentElement.ChildNodes[5]);

			return extension_sector.DocumentElement;
		}

		public static XmlElement ObtenerPuntoVenta(InfoPuntoVenta datos)
		{

			PuntoVenta puntoventa = new PuntoVenta();

			InformacionCajaVenta Informacion = new InformacionCajaVenta();

			Informacion.Name = new Name();
			Informacion.Name.Value = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<DatosFijosPosPV>(0));//"PlacaCaja";
			Informacion.Value = new Value();
			Informacion.Value.Value = datos.PlacaCaja;

			puntoventa.InformacionCajaVenta = Informacion;

			//name1 = "UbicaciónCaja";
			//value1 = datos.UbicaciónCaja;

			//name2 = "Cajero";
			//value2 = datos.Cajero;

			//name3 = "UbicaciónCaja";
			//value3 = datos.UbicaciónCaja;

			//name4 = "TipoCaja";
			//value4 = datos.TipoCaja;

			//name5 = "CódigoVenta";
			//value5 = datos.CodigoVenta;

			//name6 = "SubTotal";
			//value6 = datos.SubTotal.ToString().Replace(",", ".");


			//XmlSerializerNamespaces serializer = NamespacesXML.ObtenerExtensionSector("Salud");
			StreamWriter stWriter = null;
			XmlSerializer xmlSerializer;
			string buffer;

			xmlSerializer = new XmlSerializer(puntoventa.GetType());
			MemoryStream memStream = new MemoryStream();
			stWriter = new StreamWriter(memStream);

			xmlSerializer.Serialize(stWriter, puntoventa);
			buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

			XmlDocument extension_sector = new XmlDocument();
			extension_sector.LoadXml(buffer);

			XmlNode root = extension_sector.DocumentElement.ChildNodes[0];

			extension_sector.DocumentElement.RemoveAllAttributes();

			int j = 2;
			for (int i = 1; i < 7; i++)
			{
				XmlNode nodo_name = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
				nodo_name.InnerText = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<DatosFijosPosPV>(i));
				root.InsertBefore(nodo_name, extension_sector.DocumentElement.ChildNodes[j]);

				XmlNode nodo_value = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
				switch (i)
				{
					case 1:
						nodo_value.InnerText = datos.UbicacionCaja;//value1;
						break;
					case 2:
						nodo_value.InnerText = datos.Cajero;//value2;
						break;
					case 3:
						nodo_value.InnerText = datos.UbicacionCaja;//value3;
						break;
					case 4:
						nodo_value.InnerText = datos.TipoCaja;//value4;
						break;
					case 5:
						nodo_value.InnerText = datos.CodigoVenta;//value5;
						break;
					case 6:
						nodo_value.InnerText = datos.SubTotal.ToString().Replace(",", ".");//value6;
						break;
					default:
						break;
				}
				root.InsertBefore(nodo_value, extension_sector.DocumentElement.ChildNodes[j+1]);

				j++;
			}

			//XmlNode nodo_name1 = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
			//nodo_name1.InnerText = name1;
			//root.InsertBefore(nodo_name1, extension_sector.DocumentElement.ChildNodes[2]);

			//XmlNode nodo_value1 = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
			//nodo_value1.InnerText = value1;
			//root.InsertBefore(nodo_value1, extension_sector.DocumentElement.ChildNodes[3]);

			//XmlNode nodo_name2 = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
			//nodo_name2.InnerText = name2;
			//root.InsertBefore(nodo_name2, extension_sector.DocumentElement.ChildNodes[4]);

			//XmlNode nodo_value2 = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
			//nodo_value2.InnerText = value2;
			//root.InsertBefore(nodo_value2, extension_sector.DocumentElement.ChildNodes[5]);

			//XmlNode nodo_name3 = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
			//nodo_name3.InnerText = name2;
			//root.InsertBefore(nodo_name3, extension_sector.DocumentElement.ChildNodes[6]);

			//XmlNode nodo_value3 = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
			//nodo_value3.InnerText = value2;
			//root.InsertBefore(nodo_value3, extension_sector.DocumentElement.ChildNodes[7]);

			//XmlNode nodo_name4 = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
			//nodo_name4.InnerText = name2;
			//root.InsertBefore(nodo_name4, extension_sector.DocumentElement.ChildNodes[8]);

			//XmlNode nodo_value4 = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
			//nodo_value4.InnerText = value2;
			//root.InsertBefore(nodo_value4, extension_sector.DocumentElement.ChildNodes[8]);

			//XmlNode nodo_name5 = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
			//nodo_name5.InnerText = name2;
			//root.InsertBefore(nodo_name5, extension_sector.DocumentElement.ChildNodes[8]);

			//XmlNode nodo_value5 = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
			//nodo_value5.InnerText = value2;
			//root.InsertBefore(nodo_value5, extension_sector.DocumentElement.ChildNodes[8]);

			return extension_sector.DocumentElement;
		}

		public static XmlElement ObtenerInfoTicket(InfoTicket datos)
		{

			InformacionAdicional ticket = new InformacionAdicional();

			InformacionTicket Informacion = new InformacionTicket();

			Informacion.Name = new Name();
			Informacion.Name.Value = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<DatosFijosPosTicket>(0));//"PlacaCaja";
			Informacion.Value = new Value();
			Informacion.Value.Value = datos.ModoTransporte;

			ticket.InformacionTicket = Informacion;


			//XmlSerializerNamespaces serializer = NamespacesXML.ObtenerExtensionSector("Salud");
			StreamWriter stWriter = null;
			XmlSerializer xmlSerializer;
			string buffer;

			xmlSerializer = new XmlSerializer(ticket.GetType());
			MemoryStream memStream = new MemoryStream();
			stWriter = new StreamWriter(memStream);

			xmlSerializer.Serialize(stWriter, ticket);
			buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

			XmlDocument extension_sector = new XmlDocument();
			extension_sector.LoadXml(buffer);

			XmlNode root = extension_sector.DocumentElement.ChildNodes[0];

			extension_sector.DocumentElement.RemoveAllAttributes();

			int j = 2;
			for (int i = 1; i < 5; i++)
			{
				XmlNode nodo_name = extension_sector.DocumentElement.ChildNodes[0].FirstChild.Clone();
				nodo_name.InnerText = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<DatosFijosPosTicket>(i));
				root.InsertBefore(nodo_name, extension_sector.DocumentElement.ChildNodes[j]);

				XmlNode nodo_value = extension_sector.DocumentElement.ChildNodes[0].ChildNodes[1].Clone();
				switch (i)
				{
					case 1:
						nodo_value.InnerText = datos.IDMediodeTransporte;//value1;
						break;
					case 2:
						nodo_value.InnerText = datos.Mediodetransporte;//value2;
						break;
					case 3:
						nodo_value.InnerText = datos.LugardeOrigen;//value3;
						break;
					case 4:
						nodo_value.InnerText = datos.LugardeDestino;//value4;
						break;
					default:
						break;
				}
				root.InsertBefore(nodo_value, extension_sector.DocumentElement.ChildNodes[j + 1]);

				j++;
			}

			return extension_sector.DocumentElement;
		}

		public enum DatosFijosPosSW
		{
			[Description("NombreApellido")]
			name1 = 1,

			[Description("RazonSocial")]
			name2 = 2,

			[Description("NombreSoftware")]
			name3 = 3,
		}

		public enum DatosFijosPosBenCom
		{
			[Description("Codigo")]
			name1 = 1,

			[Description("NombresApellidos")]
			name2 = 2,

			[Description("Puntos")]
			name3 = 3,
		}

		public enum DatosFijosPosPV
		{
			[Description("PlacaCaja")]
			name = 0,

			[Description("UbicaciónCaja")]
			name1 = 1,

			[Description("Cajero")]
			name2 = 2,

			[Description("UbicaciónCaja")]
			name3 = 3,

			[Description("TipoCaja")]
			name4 = 4,

			[Description("CódigoVenta")]
			name5 = 5,

			[Description("SubTotal")]
			name6 = 6
		}

		public enum DatosFijosPosTicket
		{
			[Description("ModoTransporte")]
			name = 0,

			[Description("IDMediodeTransporte")]
			name1 = 1,

			[Description("Mediodetransporte")]
			name2 = 2,

			[Description("LugardeOrigen")]
			name3 = 3,

			[Description("LugardeDestino")]
			name4 = 4
		}

	}
}
