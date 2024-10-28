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
		private static string value2 = "Resolucion 2275:2023";
		private static string sector = "Sector Salud";
		private static string usuario = "Usuario";

		/// <summary>
		/// Obtiene la extension de HGI SAS
		/// </summary>
		/// <returns> XmlElement que contiene la extension HGI SAS</returns>
		public static XmlElement Obtener(Salud datos, TipoDocumento tipo_doc, bool diferente_orden)
		{

			CustomTagGeneral TagGeneral = new CustomTagGeneral();




			TagGeneral.Name= new Name();
			TagGeneral.Name.Value = name1;
			TagGeneral.Value = new Value();
			TagGeneral.Value.Value = value1;

			InteroperabilidadType interoperabilidad = new InteroperabilidadType();
			interoperabilidad.Group = new Group();
			interoperabilidad.Group.schemeName = sector;
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

			List<AdditionalInformationType1> datos_adicionales = new List<AdditionalInformationType1>();

			Collection datos_usuario = new Collection();

			int campo_validador = 0;
			//Informacion como indica Resolucion 510 y 2275 que corresponde a 11 campos
			if (datos.CamposSector.Count > 0 && datos.CamposSector.Count == 5)
			{
				for (int i = 0; i < datos.CamposSector.Count; i++)
				{
					AdditionalInformationType1 coleccion_datos = new AdditionalInformationType1();
					coleccion_datos.Name = new Name();
					try
					{

						coleccion_datos.Name.Value = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CamposSalud510>(Convert.ToInt16(datos.CamposSector[i].Descripcion)));

					}
					catch (Exception)
					{

						coleccion_datos.Name.Value = datos.CamposSector[i].Descripcion;
					}
					coleccion_datos.Value = new Value();
					coleccion_datos.Value.Value = datos.CamposSector[i].Valor;

					if (i == campo_validador + 1)
					{
						ModalidadDePago dato_enum = Enumeracion.GetValueFromAmbiente<ModalidadDePago>(datos.CamposSector[i].Valor);
						coleccion_datos.Value.Value = Enumeracion.GetDescription(dato_enum);
						coleccion_datos.Value.schemeName = "salud_modalidad_pago.gc";
						coleccion_datos.Value.schemeID = datos.CamposSector[i].Valor;
					}

					if (i == campo_validador + 2)
					{
						CoberturaSalud dato_enum = Enumeracion.GetEnumObjectByValue<CoberturaSalud>(Convert.ToInt16(datos.CamposSector[i].Valor));
						coleccion_datos.Value.Value = Enumeracion.GetDescription(dato_enum);
						coleccion_datos.Value.schemeName = "salud_cobertura.gc";
						coleccion_datos.Value.schemeID = Enumeracion.GetAmbiente(dato_enum);
						campo_validador += datos.CamposSector.Count;
					}

					datos_adicionales.Add(coleccion_datos);

					if (i == campo_validador - 1)
					{
						Collection datos_coleccion = new Collection();
						datos_coleccion.schemeName = usuario;
						datos_coleccion.AdditionalInformation = datos_adicionales.ToArray();
						datos_adicionales = new List<AdditionalInformationType1>();
						datos_usuario = datos_coleccion;
					}
				}
			}

			//Información como indica la resolucion 084 respecto a 21 campos
			if (datos.CamposSector.Count >= 21)
			{
				campo_validador = 0;
				for (int i = 0; i < datos.CamposSector.Count; i++)
				{
					AdditionalInformationType1 coleccion_datos = new AdditionalInformationType1();
					coleccion_datos.Name = new Name();
					try
					{

						coleccion_datos.Name.Value = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CamposSalud>(Convert.ToInt16(datos.CamposSector[i].Descripcion)));

					}
					catch (Exception)
					{

						coleccion_datos.Name.Value = datos.CamposSector[i].Descripcion;
					}
					coleccion_datos.Value = new Value();
					coleccion_datos.Value.Value = datos.CamposSector[i].Valor;

					if (i == campo_validador + 1)
					{
						TipoIdentificacionSalud dato_enum = Enumeracion.GetValueFromAmbiente<TipoIdentificacionSalud>(datos.CamposSector[i].Valor);
						coleccion_datos.Value.Value = Enumeracion.GetDescription(dato_enum);
						coleccion_datos.Value.schemeName = "salud_identificación.gc";
						coleccion_datos.Value.schemeID = datos.CamposSector[i].Valor;
					}

					if (i == campo_validador + 7)
					{
						TipoUsuarioSalud dato_enum = Enumeracion.GetEnumObjectByValue<TipoUsuarioSalud>(Convert.ToInt16(datos.CamposSector[i].Valor));
						coleccion_datos.Value.Value = Enumeracion.GetDescription(dato_enum);
						coleccion_datos.Value.schemeName = "salud_tipo_usuario.gc";
						coleccion_datos.Value.schemeID = Enumeracion.GetAmbiente(dato_enum);
					}

					if (i == campo_validador + 9)
					{
						CoberturaSalud dato_enum = Enumeracion.GetEnumObjectByValue<CoberturaSalud>(Convert.ToInt16(datos.CamposSector[i].Valor));
						coleccion_datos.Value.Value = Enumeracion.GetDescription(dato_enum);
						coleccion_datos.Value.schemeName = "salud_cobertura.gc";
						coleccion_datos.Value.schemeID = Enumeracion.GetAmbiente(dato_enum);
						campo_validador += 21;
					}

					datos_adicionales.Add(coleccion_datos);

					if (i == campo_validador - 1)
					{
						Collection datos_coleccion = new Collection();
						datos_coleccion.schemeName = usuario;
						datos_coleccion.AdditionalInformation = datos_adicionales.ToArray();
						datos_adicionales = new List<AdditionalInformationType1>();
						datos_usuario = datos_coleccion;
					}
				}
			}
			
			
			interoperabilidad.Group.Collection = datos_usuario;

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

			XmlNode root = extension_sector.DocumentElement;

			extension_sector.DocumentElement.RemoveAllAttributes();

			XmlNode nodo_name = extension_sector.DocumentElement.FirstChild.Clone();
			nodo_name.InnerText = name2;
			root.InsertBefore(nodo_name, extension_sector.DocumentElement.ChildNodes[2]);

			XmlNode nodo_value = extension_sector.DocumentElement.ChildNodes[1].Clone();
			nodo_value.InnerText = value2;
			root.InsertBefore(nodo_value, extension_sector.DocumentElement.ChildNodes[3]);

			return extension_sector.DocumentElement;
		}

	}
}
