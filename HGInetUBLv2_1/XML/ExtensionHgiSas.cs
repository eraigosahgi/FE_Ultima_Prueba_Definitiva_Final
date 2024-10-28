using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectonicaData.ModeloServicio;

namespace HGInetUBLv2_1
{
    /// <summary>
	/// Genera el elemento XML para la extensión de HGI SAS
	/// </summary>
    public class ExtensionHgiSas
    {

        /// <summary>
		/// Obtiene la extension de HGI SAS
		/// </summary>
		/// <returns> XmlElement que contiene la extension HGI SAS</returns>
		public static XmlElement Obtener(Guid id_documento, dynamic documento)
        {
            HGInet hgiExtensions = new HGInet()
            {
                Aplicacion = new DescriptionType()
                {   Value = "HGI Facturación Electrónica"
                },

                ProveedorTecnologico = new DescriptionType()
                {
                    Value = "HGI SAS proveedor tecnológico de facturación electrónica autorizado por la DIAN." // MEDIANTE LA RESOLUCIÓN 000559 DEL 22 DE ENERO DE 2018
                },

                IdSeguridad = new CodeType()
                {
                    Value = id_documento.ToString(),
                    name = "Radicado"
                },

                PdfFormat = new DescriptionType()
                {
                    Value = FormatoNotas.CamposPredeterminados(documento.DocumentoFormato)[0]
                }
            };

            XmlSerializerNamespaces serializer = NamespacesXML.ObtenerExtensionHgi();
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            string buffer;

            xmlSerializer = new XmlSerializer(hgiExtensions.GetType());
            MemoryStream memStream = new MemoryStream();
            stWriter = new StreamWriter(memStream);

            xmlSerializer.Serialize(stWriter, hgiExtensions, serializer);
            buffer = Encoding.UTF8.GetString(memStream.GetBuffer());

            XmlDocument extension_dian = new XmlDocument();
            extension_dian.LoadXml(buffer);
            return extension_dian.DocumentElement;
        }

    }
}
