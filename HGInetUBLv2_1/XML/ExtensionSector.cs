using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
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
	public class ExtensionSector
	{
		//Campos constantes para mostrar en el XML	
		private static string name1 = "Responsable";
		private static string value1 = "url www.minsalud.gov.co";
		private static string name2 = "Tipo, identificador:año del acto administrativo";
		private static string value2 = "Resolucion 084:2021";
		private static string sector = "Sector Salud";
		private static string usuario = "Usuario";

		/// <summary>
		/// Obtiene la extension de HGI SAS
		/// </summary>
		/// <returns> XmlElement que contiene la extension HGI SAS</returns>
		public static XmlElement Obtener(Salud datos, TipoDocumento tipo_doc)
		{

			CustomTagGeneral TagGeneral = new CustomTagGeneral();

			TagGeneral.Name = new Name[2];
			TagGeneral.Value = new Value[2];

			TagGeneral.Name[0] = new Name();
			TagGeneral.Name[0].Value = name1;
			TagGeneral.Value[0] = new Value();
			TagGeneral.Value[0].Value = value1;
			TagGeneral.Name[1] = new Name();
			TagGeneral.Name[1].Value = name2;
			TagGeneral.Value[1] = new Value();
			TagGeneral.Value[1].Value = value2;

			InteroperabilidadType interoperabilidad = new InteroperabilidadType();
			interoperabilidad.Group = new Group();
			interoperabilidad.Group.schemeName = sector;
			interoperabilidad.Group.Collection = new Collection[1];
			interoperabilidad.Group.Collection[0] = new Collection();
			interoperabilidad.Group.Collection[0].schemeName = usuario;
			interoperabilidad.InteroperabilidadPT = new InteroperabilidadPtType();
			interoperabilidad.InteroperabilidadPT.URLDescargaAdjuntos = new URLDescagaAdjuntosType();
			interoperabilidad.InteroperabilidadPT.URLDescargaAdjuntos.URL = datos.URLDescargaAdjuntos;

			if (datos.ParametrosDescargaAdjuntos != null && datos.ParametrosDescargaAdjuntos.Count > 0)
			{
				List<CampoValorType> list_parametros = new List<CampoValorType>();
				foreach (CampoValor item in datos.ParametrosDescargaAdjuntos)
				{
					CampoValorType ParametroArgumento = new CampoValorType();
					ParametroArgumento.Name = new Name();
					ParametroArgumento.Name.Value = item.Descripcion;
					ParametroArgumento.Value = new Value();
					ParametroArgumento.Value.Value = item.Valor;
					list_parametros.Add(ParametroArgumento);
				}

				List<ParametrosArgumentosType> list_parametros_descarga = new List<ParametrosArgumentosType>();
				ParametrosArgumentosType parametros_descarga = new ParametrosArgumentosType();
				parametros_descarga.ParametroArgumento = list_parametros.ToArray();
				list_parametros_descarga.Add(parametros_descarga);
				interoperabilidad.InteroperabilidadPT.URLDescargaAdjuntos.ParametroArgumentos = list_parametros_descarga.ToArray();
			}

			interoperabilidad.InteroperabilidadPT.EntregaDocumento = new EntregaDocumentoType();
			interoperabilidad.InteroperabilidadPT.EntregaDocumento.WS = datos.URLWebService;

			if (datos.ParametrosWebService != null && datos.ParametrosWebService.Count > 0)
			{
				List<CampoValorType> list_parametros = new List<CampoValorType>();
				foreach (CampoValor item in datos.ParametrosWebService)
				{
					CampoValorType ParametroArgumento = new CampoValorType();
					ParametroArgumento.Name = new Name();
					ParametroArgumento.Name.Value = item.Descripcion;
					ParametroArgumento.Value = new Value();
					ParametroArgumento.Value.Value = item.Valor;
					list_parametros.Add(ParametroArgumento);
				}

				List<ParametrosArgumentosType> list_parametros_descarga = new List<ParametrosArgumentosType>();
				ParametrosArgumentosType parametros_descarga = new ParametrosArgumentosType();
				parametros_descarga.ParametroArgumento = list_parametros.ToArray();
				list_parametros_descarga.Add(parametros_descarga);
				interoperabilidad.InteroperabilidadPT.EntregaDocumento.ParametroArgumentos = list_parametros_descarga.ToArray();
			}

			List<AdditionalType> datos_adicionales = new List<AdditionalType>();

			for (int i = 0; i < datos.CamposSector.Count; i++)
			{
				AdditionalType coleccion_datos = new AdditionalType();
				coleccion_datos.Name = new NameType1();
				coleccion_datos.Name.Value = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CamposSalud>(Convert.ToInt16(datos.CamposSector[i].Descripcion)));
				coleccion_datos.Value = new ValueType();
				coleccion_datos.Value.Value = datos.CamposSector[i].Valor;

				if (i == 1)
				{
					TipoIdentificacionSalud dato_enum = Enumeracion.GetValueFromAmbiente<TipoIdentificacionSalud>(datos.CamposSector[i].Valor);
					coleccion_datos.Value.Value = Enumeracion.GetDescription(dato_enum);
					coleccion_datos.schemeName = "salud_identificación.gc";
					coleccion_datos.schemeID = datos.CamposSector[i].Valor;
				}

				if (i == 7)
				{
					TipoUsuarioSalud dato_enum = Enumeracion.GetEnumObjectByValue<TipoUsuarioSalud>(Convert.ToInt16(datos.CamposSector[i].Valor));
					coleccion_datos.Value.Value = Enumeracion.GetDescription(dato_enum);
					coleccion_datos.schemeName = "salud_tipo_usuario.gc";
					coleccion_datos.schemeID = Enumeracion.GetAmbiente(dato_enum);
				}

				if (i == 9)
				{
					CoberturaSalud dato_enum = Enumeracion.GetEnumObjectByValue<CoberturaSalud>(Convert.ToInt16(datos.CamposSector[i].Valor));
					coleccion_datos.Value.Value = Enumeracion.GetDescription(dato_enum);
					coleccion_datos.schemeName = "salud_cobertura.gc";
					coleccion_datos.schemeID = Enumeracion.GetAmbiente(dato_enum);
				}

				datos_adicionales.Add(coleccion_datos);
			}

			interoperabilidad.Group.Collection[0].AdditionalInformation = datos_adicionales.ToArray();

			TagGeneral.Interoperabilidad = interoperabilidad;

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
			return extension_sector.DocumentElement;
		}


	}
}
