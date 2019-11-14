using HGInetDIANServicios;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace HGInetFacturaETestConsola
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				HGInetFeAPI.ServicioDocumento.DocumentoCufe doc1 = new HGInetFeAPI.ServicioDocumento.DocumentoCufe()
				{
					ClaveTecnica = "",
					Cufe = "",
					DataKey = "",
					Documento = 0,
					Error = null,
					DocumentoTipo = 1,
					Fecha = new DateTime(),
					IdentificacionAdquiriente = "",
					IdentificacionObligado = "",
					IdVersionDian = 2,
					Prefijo = "",
					QR = "",
					Total = 0,
					ValorIca = 0,
					ValorImpuestoConsumo = 0,
					ValorIva = 0,
					ValorSubtotal = 0
				};

				HGInetFeAPI.ServicioDocumento.DocumentoCufe doc2 = new HGInetFeAPI.ServicioDocumento.DocumentoCufe()
				{
					ClaveTecnica = "",
					Cufe = "",
					DataKey = "",
					Documento = 0,
					Error = null,
					DocumentoTipo = 1,
					Fecha = new DateTime(),
					IdentificacionAdquiriente = "",
					IdentificacionObligado = "",
					IdVersionDian = 2,
					Prefijo = "",
					QR = "",
					Total = 0,
					ValorIca = 0,
					ValorImpuestoConsumo = 0,
					ValorIva = 0,
					ValorSubtotal = 0
				};

				HGInetFeAPI.ServicioDocumento.DocumentoCufe doc3 = new HGInetFeAPI.ServicioDocumento.DocumentoCufe()
				{
					ClaveTecnica = "",
					Cufe = "",
					DataKey = "",
					Documento = 0,
					Error = null,
					DocumentoTipo = 1,
					Fecha = new DateTime(),
					IdentificacionAdquiriente = "",
					IdentificacionObligado = "",
					IdVersionDian = 2,
					Prefijo = "",
					QR = "",
					Total = 0,
					ValorIca = 0,
					ValorImpuestoConsumo = 0,
					ValorIva = 0,
					ValorSubtotal = 0
				};

				List<HGInetFeAPI.ServicioDocumento.DocumentoCufe> docs = new List<HGInetFeAPI.ServicioDocumento.DocumentoCufe>();
				docs.Add(doc1);
				docs.Add(doc2);
				docs.Add(doc3);

				HGInetFeAPI.Ctl_Documentos.CalcularCufe("url", "serial", "nit", docs);


			/*


				//Se lee un xml de una ruta
				FileStream xml_reader = new FileStream(@"E:\Desarrollo\jzea\Pruebas\face_f0811021438003B023973.xml", FileMode.Open);

				XmlSerializer xml_ser = new XmlSerializer(typeof(InvoiceType));

				InvoiceType objeto = (InvoiceType)xml_ser.Deserialize(xml_reader);

				// representación de datos en objeto
				var documento_obj = (dynamic)null;

				//Asigno objeto dinamico a Objeto tipo Factura
				Factura factura = new Factura();

				//Convierto XML-UBL en Objeto
				factura = FacturaXML.Convertir(objeto,null);

				List<HGInetFeAPI.ServicioFactura.Factura> list_factura = new List<HGInetFeAPI.ServicioFactura.Factura>();

				HGInetFeAPI.ServicioFactura.Factura factura_envio = new HGInetFeAPI.ServicioFactura.Factura();

				documento_obj = factura;

				factura_envio = documento_obj;

				list_factura.Add(factura_envio);

				//Ejecuto libreria de servicios para el envio de documentos
				HGInetFeAPI.Ctl_Factura.Enviar("http://habilitacion.mifacturaenlinea.com.co", "4C2B77AFA43D621B091EF6802", "811021438", list_factura);

				// cerrar la lectura del archivo xml
				xml_reader.Close();


				try
				{

					Pdf.LeerEstructura1(@"E:\testpdf\FacturaFormato.pdf");

					Pdf.DescomponerPdf(@"E:\testpdf\FacturaFormato.pdf", @"E:\testpdf\pdf2", "FacturaFormato.pdf");

					FormatoPdf.VerySimpleReplaceText(@"E:\FacturaFormato.pdf", @"E:\FacturaFormato2.pdf", "@RazonSocial", "HGI S.A.S.");

					FormatoPdf.Leer();

					using (PdfReader pdf = new PdfReader(@"E:\FacturaFormato.pdf"))
					{

						ITextExtractionStrategy texto = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();

						string texto_pfd = PdfTextExtractor.GetTextFromPage(pdf, 1);

					}
				}
				catch (Exception excepcion)
				{

					throw excepcion;
				}











				List<HGInetFeAPI.ServicioResolucion.Resolucion> resoluciones = HGInetFeAPI.Ctl_Resolucion.Obtener("http://localhost:61499/", "CE61061C-9AD2-4942-97F5-A48206F2680D", "811021438");








				DateTime fecha = DateTime.Now;

				string url_ws_consulta = "https://facturaelectronica.dian.gov.co/habilitacion/B2BIntegrationEngine/FacturaElectronica/consultaDocumentos.wsdl";
				string id_software = "606f5740-c6b9-494f-931c-5a6b3e22d72c";
				string clave = "Prueba2018";
				string nit_facturador = "811021438";
				string cufe = "66535b95ced25329480369470cab595e31a08241";
				string prefijo = "";
				string documento = "990000410";
				DateTime fecha_documento = new DateTime(2018, 5, 3);


				// error documento duplicado
				documento = "990000371";
				cufe = "2b517089694b8c5df3d45012907a29a1305604a7";
				prefijo = "";
				fecha_documento = new DateTime(2018, 4, 24);


				// error documento duplicado con prefijo
				documento = "980000036";
				cufe = "0559e058645a442416b85733c1d20b49c778540c";
				prefijo = "PRUE";
				fecha_documento = new DateTime(2018, 4, 10);

				// error documento cufe incorrecto
				documento = "990000348";
				cufe = "ba12f6a59c07ed5c168d74418fbf5a2eafcc21c0";
				prefijo = "";
				fecha_documento = new DateTime(2018, 4, 10);


				// documento correcto
				documento = "990000396";
				cufe = "1de20e628084240a92f19d6019d92cd711dfe782";
				prefijo = "";
				fecha_documento = new DateTime(2018, 5, 1);

				// documento inexistente
				documento = "396";
				cufe = "1de20e628084240a92f19d6019d92cd711dfe782";
				prefijo = "";
				fecha_documento = new DateTime(2018, 5, 1);



				// error documento fuera de los terminos
				documento = "980000036";
				cufe = "0559e058645a442416b85733c1d20b49c778540c";
				prefijo = "PRUE";
				fecha_documento = new DateTime(2018, 4, 6);





				// carpeta del xml
				string carpeta_xml = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), nit_facturador);
				carpeta_xml = string.Format(@"{0}{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEConsultaDian);

				// valida la existencia de la carpeta
				carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

				// ruta del xml
				string archivo_xml = string.Format(@"{0}{1}.xml", prefijo, documento);

				// ruta del xml
				string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, archivo_xml);

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);

				HGInetDIANServicios.DianResultadoTransacciones.DocumentosRecibidos resultado = Ctl_ConsultaTransacciones.Consultar(Guid.NewGuid(), id_software, clave, 1, prefijo, documento, nit_facturador, fecha_documento, cufe, url_ws_consulta, ruta_xml);

				ConsultaDocumento resultado_doc = Ctl_ConsultaTransacciones.ValidarTransaccion(resultado);

				//PruebaCufe();
				*/
			}
			catch (Exception excepcion)
			{
				System.Diagnostics.Debug.WriteLine("ERROR: " + excepcion.Message);
			}

		}

		/*
		public static void PruebaCufe()
		{
			try
			{

				string cufe = HGInetFeAPI.Ctl_Factura.CalcularCUFE("dd85db55545bd6566f36b0fd3be9fd8555c36e", "PRE", "990000402", new DateTime(2018, 4, 23), "811021438", "13", "1020395355", 47500.00M, 39916.00M, 7584.00M, 0.00M, 0.00M);

				System.Diagnostics.Debug.WriteLine("CUFE GENERADO : " + cufe);


				string cufeNC = HGInetFeAPI.Ctl_NotaCredito.CalcularCUFE("5790af0483698d226240b7c4abafd49eb0dc8d480f6a7e44dfe4a523fed2129a", "PRE", "3d03ae717f28dab49c7588ffa4ef006f776ac468", "602", new DateTime(2018, 4, 23), "811021438", "13", "79758318", 687196.00M, 607870.00M, 109720.00M, 0.00M, 0.00M);

				System.Diagnostics.Debug.WriteLine("cufeNC GENERADO : " + cufeNC);

			}
			catch (Exception excepcion)
			{
				System.Diagnostics.Debug.WriteLine("ERRO GENERANDO CUFE" + excepcion.Message);
			}

		}
		*/

		public static void test1()
		{


			//HGInetFeAPI.ServicioFactura.Factura factura = new HGInetFeAPI.ServicioFactura.Factura();

			HGInetFeAPI.Ctl_Factura.Test("http://habilitacion.mifacturaenlinea.com.co");


			Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

			//  ^[0-9]+(\\,[0-9]{2,2})?$

			//  ^[0-9]([.,][0-9]{1,3})?$

			//  [0-9]+(\.[0-9][0-9]?)?

			//  ^\d+(\.\d{1,2})?$

			// @"\d{1,12}\.\d\d$"

			//  @"(^(0|([1-9][0-9]*))(\.[0-9]{1,2})?$)|(^(0{0,1}|([1-9][0-9]*))(\.[0-9]{2,2})?$)"

			decimal valor = 102.3m;
			string valor_txt = Convert.ToString(valor).Replace(",", ".");

			bool decimal1 = isnumber.IsMatch(valor_txt);


			valor = 2365478523102.30m;
			valor_txt = Convert.ToString(valor).Replace(",", ".");

			bool decimal2 = isnumber.IsMatch(valor_txt);



			valor = 102.307m;
			valor_txt = Convert.ToString(valor).Replace(",", ".");

			bool decimal3 = isnumber.IsMatch(valor_txt);


			valor = 0.30m;
			valor_txt = Convert.ToString(valor).Replace(",", ".");

			bool decimal4 = isnumber.IsMatch(valor_txt);



			decimal x = 0.00m;

			decimal y = 12;

			x = y;
		}
	}
}
