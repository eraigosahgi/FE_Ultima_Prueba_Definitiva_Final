using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HGInetFacturaETestConsola
{
	/// <summary>
	/// Utilidad para archivos PDF
	/// </summary>
	public class Pdf
	{

		/// <summary>
		/// Combina diferentes archivos PDF en uno
		/// </summary>
		/// <param name="ruta_archivo_combinado">ruta física para almacenar el archivo final</param>
		/// <param name="nombre_archivo_combinado">nombre del archivo combinado sin la extensión</param>
		/// <param name="ruta_archivos">rutas físicas de los archivos que se van a combinar</param>
		/// <returns>ruta física del archivo generado</returns>
		public static string Combinar(string ruta_archivo_combinado, string nombre_archivo_combinado, List<string> ruta_archivos)
		{
			try
			{
				string extension = ".pdf";

				if (nombre_archivo_combinado.Contains(extension))
					extension = string.Empty;

				// ruta física y nombre del archivo nuevo
				string archivo_final = System.IO.Path.Combine(new string[] { string.Format("{0}{1}{2}", ruta_archivo_combinado, nombre_archivo_combinado, extension) });

				// crea el archivo para combinar
				Document objDocument = new Document();
				var writer = new PdfCopy(objDocument, new FileStream(archivo_final, FileMode.Create));
				objDocument.Open();

				// recorre los archivos
				foreach (var item in ruta_archivos)
				{
					// obtiene el archivo pdf
					using (PdfReader objReader = new PdfReader(item))
					{
						// recorre las páginas del archivo pdf
						for (var i = 1; i <= objReader.NumberOfPages; i++)
						{
							// guarda la página actual en el nuevo archivo
							var page = writer.GetImportedPage(objReader, i);
							writer.AddPage(page);
						}
						objReader.Close();
					}
				}
				writer.Close();
				objDocument.Close();

				return archivo_final;
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
		}

		/// <summary>
		/// Descompone un archivo Pdf en varios por cada página
		/// </summary>
		/// <param name="ruta_archivo_combinado">ruta física completa del archivo</param>
		/// <param name="ruta_archivos_descomponer">ruta física de la carpeta para descomponer</param>
		/// <param name="nombre_archivo">nombre inicial del archivo al cual se le concatenará el número de página</param>
		/// <returns>lista de rutas de archivos generados</returns>
		public static List<string> DescomponerPdf(string ruta_archivo_combinado, string ruta_carpeta_descomponer, string nombre_archivo)
		{
			try
			{
				List<string> archivos = new List<string>();

				string extension = ".pdf";

				if (nombre_archivo.Contains(extension))
					extension = string.Empty;

				// obtiene el archivo pdf
				using (PdfReader objReader = new PdfReader(ruta_archivo_combinado))
				{
					int startPage = 1;
					int endPage = objReader.NumberOfPages;

					// recorre las páginas del archivo pdf combinado
					for (int index = startPage; index <= endPage; index++)
					{
						Document objDocument = new Document(objReader.GetPageSizeWithRotation(startPage));

						// ruta física y nombre del archivo nuevo
						string destination = System.IO.Path.Combine(new string[] { string.Format("{0}{1}_{2}{3}", ruta_carpeta_descomponer, nombre_archivo, index, extension) });

						PdfCopy pdfCopyProvider = new PdfCopy(objDocument, new FileStream(destination, FileMode.Create));

						// abre el archivo
						objDocument.Open();

						// guarda la página actual en el nuevo archivo
						PdfImportedPage importedPage = pdfCopyProvider.GetImportedPage(objReader, index);
						pdfCopyProvider.AddPage(importedPage);
						objDocument.Close();

						archivos.Add(destination);
					}

					objReader.Close();
				}

				return archivos;
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
		}

		/// <summary>
		/// Obtener el texto de un documento Pdf por cada página
		/// </summary>
		/// <param name="ruta_archivo">ruta física completa del archivo lectura</param>
		/// <returns>texto por página</returns>
		public static List<string> ExtraerTexto(string ruta_archivo)
		{
			List<string> texto_paginas = new List<string>();

			try
			{	// obtiene el archivo pdf
				using (PdfReader reader = new PdfReader(ruta_archivo))
				{
					// obtiene el texto de cada página del archivo pdf
					for (int page = 1; page <= reader.NumberOfPages; page++)
					{
						ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
						String strText = PdfTextExtractor.GetTextFromPage(reader, page, its);

						strText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(strText)));

						texto_paginas.Add(strText);
					}
					reader.Close();
				}

				return texto_paginas;
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
		}


		public static void LeerEstructura1(string ruta_archivo)
		{ // obtiene el archivo pdf
			using (PdfReader reader = new PdfReader(ruta_archivo))
			{
				// obtiene el texto de cada página del archivo pdf
				for (int page = 1; page <= reader.NumberOfPages; page++)
				{
					PdfDictionary pg = reader.GetPageResources(page);

					PdfDictionary cont = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.CONTENT));
					PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
					PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));

					if (xobj != null)
					{
						foreach (PdfName name in xobj.Keys)
						{
							PdfObject obj = xobj.Get(name);

						}
					}
				}
			}

		}



		public static void LeerEstructura(string ruta_archivo)
		{ // obtiene el archivo pdf
			using (PdfReader reader = new PdfReader(ruta_archivo))
			{
				FindImageInPDFDictionary(reader.GetPageResources(1));
			}

		}

		public static PdfObject FindImageInPDFDictionary(PdfDictionary pg)
		{
			PdfDictionary cont = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.CONTENT));
			PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
			PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
			if (xobj != null)
			{
				foreach (PdfName name in xobj.Keys)
				{
					PdfObject obj = xobj.Get(name);
					if (obj.IsIndirect())
					{
						PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
						PdfName type =
						(PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
						//image at the root of the pdf
						if (PdfName.IMAGE.Equals(type))
						{
							return obj;
						}// image inside a form
						else if (PdfName.FORM.Equals(type))
						{
							return FindImageInPDFDictionary(tg);
						} //image inside a group
						else if (PdfName.GROUP.Equals(type))
						{
							return FindImageInPDFDictionary(tg);
						}
					}
				}

			}

			return null;
		}

	}
}
