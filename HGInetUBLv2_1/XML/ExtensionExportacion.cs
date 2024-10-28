using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
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
	public class ExtensionExportacion
	{
		//Campos constantes para mostrar en el XML	
		private static string sector = "Exportación";
		private static string coleccion_fijos = "DATOS ADICIONALES";

		/// <summary>
		/// Obtiene la extension de HGI SAS
		/// </summary>
		/// <returns> XmlElement que contiene la extension HGI SAS</returns>
		public static XmlElement Obtener(TasaCambio datos)
		{

			CustomTagGeneral TagGeneral = new CustomTagGeneral();

			InteroperabilidadType interoperabilidad = new InteroperabilidadType();
			interoperabilidad.Group = new Group();
			interoperabilidad.Group.schemeName = sector;
			//interoperabilidad.Group.Collection = new Collection[1];

			//List<Collection> datos_usuario = new List<Collection>();

			Collection coleccion = new Collection();
			coleccion.schemeName = coleccion_fijos;
			//coleccion.AdditionalInformation = new AdditionalInformationType1[11];

			List<AdditionalInformationType1> datos_adicionales = new List<AdditionalInformationType1>();

			for (int i = 0; i < 11; i++)
			{
				AdditionalInformationType1 coleccion_datos = new AdditionalInformationType1();
				coleccion_datos.Name = new Name();
				try
				{

					coleccion_datos.Name.Value = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ColeccionEntrega>(i));

				}
				catch (Exception)
				{

					//coleccion_datos.Name.Value = datos.CamposSector[i].Descripcion;
				}
				//coleccion_datos.Value = new Value();
				//coleccion_datos.Value.Value = string.Empty; //datos.CamposSector[i].Valor;
				datos_adicionales.Add(coleccion_datos);
			}

			coleccion.AdditionalInformation = datos_adicionales.ToArray();
			//datos_usuario.Add(coleccion);
			interoperabilidad.Group.Collection = coleccion;
			TagGeneral.Interoperabilidad = interoperabilidad;

			TotalesCopType totales = new TotalesCopType();
			totales.FctConvCop = new FctConvCopType();
			totales.FctConvCop.Value = datos.FctConvCop.ToString().Replace(",", ".");
			totales.DescuentoDetalleCop = new DescuentoDetalleCopType();
			totales.DescuentoDetalleCop.Value = datos.DescuentoDetalleCop.ToString().Replace(",", ".");
			totales.ImpOtroCop = new ImpOtroCopType();
			totales.ImpOtroCop.Value = datos.ImpOtroCop.ToString().Replace(",", ".");
			totales.MntDctoCop = new MntDctoCopType();
			totales.MntDctoCop.Value = datos.MntDctoCop.ToString().Replace(",", ".");
			totales.MntImpCop = new MntImpCopType();
			totales.MntImpCop.Value = datos.MntImpCop.ToString().Replace(",", ".");
			totales.MntRcgoCop = new MntRcgoCopType();
			totales.MntRcgoCop.Value = datos.MntRcgoCop.ToString().Replace(",", ".");
			totales.MonedaCop = new MonedaCopType();
			totales.MonedaCop.Value = datos.Moneda;
			totales.RecargoDetalleCop = new RecargoDetalleCopType();
			totales.RecargoDetalleCop.Value = datos.RecargoDetalleCop.ToString().Replace(",", ".");
			totales.ReteFueCop = new ReteFueCopType();
			totales.ReteFueCop.Value = datos.ReteFueCop.ToString().Replace(",", ".");
			totales.ReteIcaCop = new ReteIcaCopType();
			totales.ReteIcaCop.Value = datos.ReteIcaCop.ToString().Replace(",", ".");
			totales.ReteIvaCop = new ReteIvaCopType();
			totales.ReteIvaCop.Value = datos.ReteIvaCop.ToString().Replace(",", ".");
			totales.SubTotalCop = new SubTotalCopType();
			totales.SubTotalCop.Value = datos.SubTotalCop.ToString().Replace(",", ".");
			totales.TotalBrutoFacturaCop = new TotalBrutoFacturaCopType();
			totales.TotalBrutoFacturaCop.Value = datos.TotalBrutoFacturaCop.ToString().Replace(",", ".");
			totales.TotalNetoFacturaCop = new TotalNetoFacturaCopType();
			totales.TotalNetoFacturaCop.Value = datos.TotalNetoFacturaCop.ToString().Replace(",", ".");
			totales.TotAnticiposCop = new TotAnticiposCopType();
			totales.TotAnticiposCop.Value = datos.TotAnticiposCop.ToString().Replace(",", ".");
			totales.TotBolCop = new TotBolCopType();
			totales.TotBolCop.Value = datos.TotBolCop.ToString().Replace(",", ".");
			totales.TotIncCop = new TotIncCopType();
			totales.TotIncCop.Value = datos.TotIncCop.ToString().Replace(",", ".");
			totales.TotIvaCop = new TotIvaCopType();
			totales.TotIvaCop.Value = datos.TotIvaCop.ToString().Replace(",", ".");
			totales.VlrPagarCop = new VlrPagarCopType();
			totales.VlrPagarCop.Value = datos.VlrPagarCop.ToString().Replace(",", ".");

			TagGeneral.TotalesCop = totales;

			//XmlSerializerNamespaces serializer = NamespacesXML.ObtenerExtensionSector("Salud");
			StreamWriter stWriter = null;
			XmlSerializer xmlSerializer;
			string buffer;

			xmlSerializer = new XmlSerializer(TagGeneral.GetType());
			MemoryStream memStream = new MemoryStream();
			stWriter = new StreamWriter(memStream);

			xmlSerializer.Serialize(stWriter, TagGeneral);
			buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

			XmlDocument extension_sector = new XmlDocument();
			extension_sector.LoadXml(buffer);

			//XmlNode root = extension_sector.DocumentElement;

			//extension_sector.DocumentElement.RemoveAllAttributes();

			//XmlNode nodo_name = extension_sector.DocumentElement.FirstChild.Clone();
			//nodo_name.InnerText = name2;
			//root.InsertBefore(nodo_name, extension_sector.DocumentElement.ChildNodes[2]);

			//XmlNode nodo_value = extension_sector.DocumentElement.ChildNodes[1].Clone();
			//nodo_value.InnerText = value2;
			//root.InsertBefore(nodo_value, extension_sector.DocumentElement.ChildNodes[3]);

			return extension_sector.DocumentElement;
		}

		public enum ColeccionEntrega
		{
			[Description("Responsable/Encargado")]
			c1 = 1,

			[Description("Lugar de Salida")]
			c2 = 2,

			[Description("Medio de transporte")]
			c3 = 3,

			[Description("Tipo de Doc.de transporte")]
			c4 = 4,

			[Description("N° de Doc. de transporte")]
			c5 = 5,

			[Description("Transportadora o Tramitadora")]
			c6 = 6,

			[Description("País de Origen de la M/cia")]
			c7 = 7,

			[Description("Destino")]
			c8 = 8,

			[Description("Términos de pago")]
			c9 = 9,

			[Description("Seguro")]
			c10 = 10,

			[Description("Observaciones")]
			c11 = 11
		}

	}
}
