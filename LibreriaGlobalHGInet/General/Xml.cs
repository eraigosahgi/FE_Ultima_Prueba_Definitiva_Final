using LibreriaGlobalHGInet.Parametros;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using LibreriaGlobalHGInet.Formato;

namespace LibreriaGlobalHGInet.General
{
	public class Xml
	{

		/// <summary>
		/// Obtiene los datos de un archivo XML
		/// </summary>
		/// <typeparam name="T" type="Object">objeto</typeparam>
		/// <param name="ruta" type="string">ruta física del archivo XML</param>
		/// <returnstype="Object">objeto</returns>
		public static T ObtenerObjetoXml<T>(string ruta) where T : new()
		{
			string ruta_directorio = AppDomain.CurrentDomain.BaseDirectory;

			if (!ruta.Contains(ruta_directorio))
				ruta = string.Format("{0}{1}", ruta_directorio, ruta);

			if (Archivo.ValidarExistencia(ruta))
			{
				T objeto = new T();

				XmlReader xr = XmlReader.Create(ruta);
				try
				{
					XmlSerializer xmls = new XmlSerializer(objeto.GetType());
					objeto = (T)xmls.Deserialize(xr);

					return objeto;
				}
				catch (Exception exec)
				{
					throw new ApplicationException(string.Format("Error al obtener los objetos {0} : {1}", typeof(T).Name, exec));
				}
				finally
				{
					if (xr != null)
						xr.Close();
				}
			}
			else
				throw new ArgumentException(string.Format("La ruta {0} no existe o es inválida", ruta));
		}

		/// <summary>
		/// Obtiene los datos de un archivo XML
		/// </summary>
		/// <typeparam name="T" type="Object">objeto</typeparam>
		/// <param name="ruta" type="string">ruta física del archivo XML</param>
		/// <returnstype="List">objetos</returns>
		public static List<T> ObtenerListaObjetoXml<T>(string ruta) where T : new()
		{
			if (Archivo.ValidarExistencia(ruta))
			{
				List<T> source = new List<T>();

				XmlReader xr = XmlReader.Create(ruta);
				try
				{
					XmlSerializer xmls = new XmlSerializer(typeof(List<T>));
					source = (List<T>)xmls.Deserialize(xr);

					return source;
				}
				catch (Exception exec)
				{
					throw new ApplicationException(string.Format("Error al obtener los objetos {0} : {1}", typeof(T).Name, exec));
				}
				finally
				{
					if (xr != null)
						xr.Close();
				}
			}
			else
				throw new ArgumentException(string.Format("La ruta {0} no existe o es inválida", ruta));
		}

		/// <summary>
		/// Guarda los datos del objeto enviado en un XML en la ruta específica
		/// </summary>
		/// <typeparam name="T" type="Object">objeto</typeparam>
		/// <param name="objeto" type="Object">objeto</param>
		/// <param name="ruta_directorio" type="string">ruta física del archivo XML</param>
		/// <param name="archivo_xml" type="string">nombre archivo XML</param>
		public static void GuardarObjetoXml<T>(T objeto, string ruta_directorio, string archivo_xml)
		{
			StreamWriter sw = null;

			try
			{
				ruta_directorio = AppDomain.CurrentDomain.BaseDirectory + ruta_directorio;

				if (!Directorio.ValidarExistenciaArchivo(ruta_directorio))
					Directorio.CrearDirectorio(ruta_directorio);
				
				string ruta_xml = string.Format("{0}\\{1}", ruta_directorio, archivo_xml);

				using (sw = new StreamWriter(ruta_xml, false))
				{	XmlSerializer xmls = new XmlSerializer(objeto.GetType());
					xmls.Serialize(sw, objeto);
				}
			}
			catch (Exception exec)
			{
				throw new ApplicationException(string.Format("Error al guardar el objeto {0} : {1}", typeof(T).Name, exec));
			}
			finally
			{
				if (sw != null)
					sw.Close();
			}
		}
		

		/// <summary>
		/// Guarda los datos de los objetos enviados en un XML en la ruta específica
		/// </summary>
		/// <typeparam name="T" type="Object">objeto</typeparam>
		/// <param name="source" type="List">lista de objetos</param>
		/// <param name="ruta_directorio" type="string">ruta física del archivo XML</param>
		/// <param name="archivo_xml" type="string">nombre archivo XML</param>
		public static void GuardarListaObjetoXml<T>(List<T> source, string ruta_directorio, string archivo_xml)
		{
			StreamWriter sw = null;

			try
			{
				if (!ruta_directorio.EndsWith(@"\"))
					ruta_directorio = string.Format(@"{0}\", ruta_directorio);

				if (!Directorio.ValidarExistenciaArchivo(ruta_directorio))
					Directorio.CrearDirectorio(ruta_directorio);

				string ruta_xml = string.Format("{0}{1}", ruta_directorio, archivo_xml);

				using (sw = new StreamWriter(ruta_xml, false))
				{
					XmlSerializer xmls = new XmlSerializer(typeof(List<T>));
					xmls.Serialize(sw, source);
				}
			}
			catch (Exception exec)
			{
				throw new ApplicationException(string.Format("Error al guardar los objetos {0} : {1}", typeof(T).Name, exec));
			}
			finally
			{
				if (sw != null)
					sw.Close();
			}
		}

		/// <summary>
		/// Guarda los datos del objeto enviado en un XML en la ruta específica
		/// </summary>
		/// <typeparam name="T" type="Object">objeto</typeparam>
		/// <param name="objeto" type="Object">objeto</param>
		/// <param name="ruta_directorio" type="string">ruta física del archivo XML</param>
		/// <param name="archivo_xml" type="string">nombre archivo XML</param>
		/// <param name="NamespacesXML" type="XmlSerializerNamespaces">Namespaces del XML</param>
		/// <returns>Ruta donde se guardo el archivo</returns>      
		public static string GuardarObjeto<T>(T objeto, string ruta_directorio, string archivo_xml, XmlSerializerNamespaces NamespacesXML = null)
		{
			StreamWriter sw = null;
			try
			{
				if (!ruta_directorio.EndsWith(@"\"))
					ruta_directorio = string.Format(@"{0}\", ruta_directorio);

				if (!Directorio.ValidarExistenciaArchivo(ruta_directorio))
					Directorio.CrearDirectorio(ruta_directorio);

				string ruta_xml = string.Format("{0}{1}", ruta_directorio, archivo_xml);

				using (sw = new StreamWriter(ruta_xml, false))
				{
					XmlSerializer xmls = new XmlSerializer(objeto.GetType());
					if (NamespacesXML == null)
					{
						xmls.Serialize(sw, objeto);
					}
					else
					{
						xmls.Serialize(sw, objeto, NamespacesXML);
					}
				}
				return ruta_xml;
			}
			catch (Exception exec)
			{
				throw new ApplicationException(string.Format("Error al guardar el objeto {0} : {1}", typeof(T).Name, exec));
			}
			finally
			{
				if (sw != null)
					sw.Close();
			}
		}

		/// <summary>
		/// Guarda un xml en un archivo físico
		/// </summary>
		/// <param name="documentoXML">datos del xml</param>
		/// <param name="ruta_directorio">ruta del directorio</param>
		/// <param name="archivo_xml">nombre del archivo</param>
		public static string GuardarXml(XmlDocument documentoXML, string ruta_directorio, string archivo_xml)
		{
			StreamWriter sw = null;
			string ruta_xml = string.Empty;

			try
			{
				if (!ruta_directorio.EndsWith(@"\"))
					ruta_directorio = string.Format(@"{0}\", ruta_directorio);

				if (!Directorio.ValidarExistenciaArchivo(ruta_directorio))
					Directorio.CrearDirectorio(ruta_directorio);

				ruta_xml = string.Format("{0}{1}", ruta_directorio, archivo_xml);

				// valida si está definida la extensión xml
				if (!ruta_xml.ToLower().Contains(".xml"))
					ruta_xml = string.Format("{0}.xml", ruta_xml);

				// propiedades para el xml como: codificación, identación
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
				{
					Encoding = new UTF8Encoding(),
					Indent = true,
                    IndentChars = "\t",
					CheckCharacters = true,
					ConformanceLevel = ConformanceLevel.Document
				};
                
                using (sw = new StreamWriter(ruta_xml, false))
				{
					using (XmlWriter xmlWriter = XmlWriter.Create(sw, xmlWriterSettings))
					{
						documentoXML.Save(sw);
						xmlWriter.Close();
					}

					sw.Close();
				}

				return ruta_xml;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(string.Format("Error al guardar el archivo {0} : {1}", ruta_xml, excepcion));
			}
			finally
			{
				if (sw != null)
					sw.Close();
			}
		}

		/// <summary>
		/// Guarda un xml en un archivo físico
		/// </summary>
		/// <param name="documentoXML">datos del xml</param>
		/// <param name="ruta_directorio">ruta del directorio</param>
		/// <param name="archivo_xml">nombre del archivo</param>
		/// <returns>ruta física del archivo</returns>
		public static string GuardarXml(XDocument documentoXML, string ruta_directorio, string archivo_xml)
		{
			string ruta_xml = string.Empty;

			try
			{
				if (!ruta_directorio.EndsWith(@"\"))
					ruta_directorio = string.Format(@"{0}\", ruta_directorio);

				if (!Directorio.ValidarExistenciaArchivo(ruta_directorio))
					Directorio.CrearDirectorio(ruta_directorio);

				ruta_xml = string.Format("{0}{1}", ruta_directorio, archivo_xml);

				// valida si está definida la extensión xml
				if (!ruta_xml.ToLower().Contains(".xml"))
					ruta_xml = string.Format("{0}.xml", ruta_xml);

				// convierte la respuesta a string
				string xml_doc = Xml.Convertir(documentoXML);

				using (StreamWriter _SW = new StreamWriter(ruta_xml, false))
				{
					_SW.Write(xml_doc);
					_SW.Close();
				}

				return ruta_xml;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Guarda un texto xml en un archivo físico
		/// </summary>
		/// <param name="texto">datos del xml</param>
		/// <param name="ruta_directorio">ruta del directorio</param>
		/// <param name="archivo_xml">nombre del archivo</param>
		/// <returns>ruta física del archivo</returns>
		public static string Guardar(StringBuilder texto, string ruta_directorio, string archivo_xml)
		{
			try
			{
				System.Xml.XmlDocument xml_objeto = new XmlDocument();
				xml_objeto.PreserveWhitespace = true;

				xml_objeto = Xml.ConvertirXmlDocument(texto);

				string ruta_xml = Xml.GuardarXml(xml_objeto, ruta_directorio, archivo_xml);

				return ruta_xml;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Convierte los datos del objeto enviado en un XML
		/// </summary>
		/// <typeparam name="T" type="Object">objeto</typeparam>
		/// <param name="objeto" type="Object">objeto</param>
		/// <param name="NamespacesXML" type="XmlSerializerNamespaces">Namespaces del XML</param>
		/// <returns>xml como texto</returns>      
		public static StringBuilder Convertir<T>(T objeto, XmlSerializerNamespaces NamespacesXML = null)
		{
			StringBuilder xml_txt = new StringBuilder();

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
            {
                Encoding = new UTF8Encoding(),
                Indent = true,
                IndentChars = "\t",
				CheckCharacters = true,
				ConformanceLevel = ConformanceLevel.Document,
                NewLineChars = "\n"
            };


			XmlWriter sw = null;

			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));

				using (StringWriterUtf8 textWriter = new StringWriterUtf8(xml_txt))
				{
					using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, xmlWriterSettings))
					{
						if (NamespacesXML == null)
							serializer.Serialize(xmlWriter, objeto);
						else
							serializer.Serialize(xmlWriter, objeto, NamespacesXML);

						xmlWriter.Close();
					}

					textWriter.Close();
					return textWriter.GetStringBuilder();
				}
			}
			catch (Exception exec)
			{
				throw new ApplicationException(string.Format("Error al convertir el objeto {0} : {1}", typeof(T).Name, exec));
			}
			finally
			{
				if (sw != null)
					sw.Close();
			}
		}


		/// <summary>
		/// Convierte el XDocument a texto
		/// </summary>
		/// <param name="documentoXML">xml</param>
		/// <returns>texto</returns>
		public static string Convertir(XDocument documentoXML)
		{
			StringWriterUtf8 writer = null;

			try
			{
				StringBuilder builder = new StringBuilder();

				writer = new StringWriterUtf8(builder);

				documentoXML.Save(writer);

				writer.Flush();

				return builder.ToString();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(string.Format("Error al obtener el texto del XML"), excepcion);
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}

		/// <summary>
		/// Convierte el texto en xml a XDocument
		/// </summary>
		/// <param name="textoXML">texto en xml</param>
		/// <returns>XDocument</returns>
		public static XDocument ConvertirXDocument(StringBuilder textoXML)
		{
			TextReader textReader = new StringReader(textoXML.ToString());
			XDocument xmlDocument = XDocument.Load(textReader);

			return xmlDocument;
		}

		/// <summary>
		/// Convierte el texto en xml a XmlDocument
		/// </summary>
		/// <param name="textoXML">texto en xml</param>
		/// <returns>XmlDocument</returns>
		public static XmlDocument ConvertirXmlDocument(StringBuilder textoXML)
		{
			TextReader textReader = new StringReader(textoXML.ToString().Replace("<<", "&lt;<"));

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.Load(textReader);

			return xmlDocument;
		}

		/// <summary>
		/// Convierte los datos de un XmlDocument a texto en xml
		/// </summary>
		/// <param name="xml">XmlDocument</param>
		/// <returns>texto</returns>
		public static StringBuilder Convertir(XmlDocument documentoXML)
		{/*
			StringWriter sw = new StringWriter();
			xml.Save(sw);

			return sw.GetStringBuilder();
			*/

			StringWriterUtf8 writer = null;

			try
			{
				StringBuilder builder = new StringBuilder();

				writer = new StringWriterUtf8(builder);

				documentoXML.Save(writer);

				writer.Flush();

				return builder;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(string.Format("Error al obtener el texto del XML"), excepcion);
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}

	}
}
