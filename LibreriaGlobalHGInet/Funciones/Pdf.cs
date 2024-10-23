using iTextSharp.text;
using iTextSharp.text.pdf;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Funciones
{
	/// <summary>
	/// Utilidad para PDF
	/// </summary>
	public class Pdf
	{

		/// <summary>
		/// Agrega un texto en una posición (X,Y) en el PDF
		/// </summary>
		/// <param name="ruta_archivo_pdf">ruta del pdf original</param>
		/// <param name="ruta_archivo_resultado">ruta del pdf resultado</param>
		/// <param name="texto">texto que se agregará</param>
		/// <param name="posicion_x">posición X del pdf</param>
		/// <param name="posicion_y">posición Y del pdf</param>
		/// <param name="reemplaza_archivo">indica si reemplaza el archivo pdf original por el resultante</param>
		/// <returns>ruta del archivo pdf resultante</returns>
		public static string AgregarTexto(string ruta_archivo_pdf, string ruta_archivo_resultado, string texto, float posicion_x, float posicion_y, bool reemplaza_archivo = false)
		{
			bool borrado = false;
			bool renombrar = false;
			bool borrado_resultado = false;

			try
			{

				// valida que si no trae texto por ser el visor lo haga con la fecha y hora actual para la prueba
				if (string.IsNullOrWhiteSpace(texto))
				{
					string fecha_doc_resp = Fecha.GetFecha().ToString("yyyy-MM-dd");
					string hora_doc_resp = Fecha.GetFecha().ToString(Fecha.formato_hora_zona);

					// texto para generar en el PDF
					texto = string.Format("Fecha Validación DIAN: {0} {1}  DOCUMENTO ELECTRÓNICO GENERADO POR HGI S.A.S NIT 811021438-4", fecha_doc_resp, hora_doc_resp);
				}

				// valida que la ruta del PDF a procesar no esté vacía
				if (string.IsNullOrEmpty(ruta_archivo_pdf))
					throw new ApplicationException("No se encuentra la ruta del archivo PDF para procesar.");

				// valida la extensión del archivo PDF
				if (!Path.GetExtension(ruta_archivo_pdf).ToUpperInvariant().Equals(".PDF"))
					throw new ApplicationException(string.Format("La extensión del archivo para procesar '{0}' no es válida.", Path.GetExtension(ruta_archivo_pdf)));
				
				// valida la posición X del texto en el PDF
				if(posicion_x < 0)
					throw new ApplicationException(string.Format("La posición X del texto para procesar '{0}' no es válida.", posicion_x.ToString()));

				// valida la posición Y del texto en el PDF
				if (posicion_y < 0)
					throw new ApplicationException(string.Format("La posición y del texto para procesar '{0}' no es válida.", posicion_x.ToString()));
					
				// construye la ruta del resultado si no la envían en el parámetro
				if (string.IsNullOrEmpty(ruta_archivo_resultado))
					ruta_archivo_resultado = Path.Combine(Path.GetPathRoot(ruta_archivo_pdf), Path.GetFileNameWithoutExtension(ruta_archivo_pdf)+"_resultado.pdf");

				//create PdfReader object to read from the existing document
				using (PdfReader reader = new PdfReader(ruta_archivo_pdf))
				{
					//create PdfStamper object to write to get the pages from reader 
					using (PdfStamper stamper = new PdfStamper(reader, new FileStream(ruta_archivo_resultado, FileMode.Append)))
					{
						// recorre las páginas del pdf
						for (int pagina = 1; pagina <= reader.NumberOfPages; pagina++)
						{
							// PdfContentByte from stamper to add content to the pages over the original content
							PdfContentByte pbover = stamper.GetOverContent(pagina);

							//add content to the page using ColumnText						
							Font fuente = new Font(Font.FontFamily.HELVETICA, 7, 1);
							ColumnText.ShowTextAligned(pbover, Element.ALIGN_LEFT, new Phrase(texto, fuente), posicion_x, posicion_y, 0);

							// PdfContentByte from stamper to add content to the pages under the original content
							PdfContentByte pbunder = stamper.GetUnderContent(pagina);

						}

						//close the stamper
						stamper.Close();
					}

					reader.Close();
				}

				if (reemplaza_archivo)
				{
					try
					{
						Archivo.Borrar(ruta_archivo_pdf);
					}
					catch (Exception exception)
					{
						borrado = true;
						throw new ApplicationException("Error borrando el archivo original",exception);
					}
					try
					{
						Archivo.Mover(ruta_archivo_resultado, ruta_archivo_pdf, "");
					}
					catch (Exception exception)
					{
						string ex_message = string.Empty;
						try
						{
							//Se pone una pausa de 1 segundo para que libere el proceso y asi pueda renombrar el archivo
							System.Threading.Thread.Sleep(1000);
							if (Archivo.ValidarExistencia(ruta_archivo_resultado))
							{
								//create PdfReader object to read from the existing document
								using (PdfReader reader = new PdfReader(ruta_archivo_resultado))
								{
									reader.Close();
								}
								
							}
						}
						catch (Exception e)
						{
							ex_message = string.Format("Renombrando el archivo 2. Detalle: {0}", e.Message);
						}
						renombrar = true;
						throw new ApplicationException(string.Format("Error renombrando el archivo con el texto. Detalle: {0} - {1}", exception.Message, ex_message), exception);
					}
					try
					{
						Archivo.Borrar(ruta_archivo_resultado);
					}
					catch (Exception exception)
					{
						borrado_resultado = true;
						throw new ApplicationException("Error borrando el archivo resultante", exception);
					}

					ruta_archivo_resultado = ruta_archivo_pdf;
				}

			}
			catch (Exception excepcion)
			{
				if (borrado == true)
				{
					if (!Archivo.ValidarExistencia(ruta_archivo_pdf))
					{
						try
						{
							Archivo.Mover(ruta_archivo_resultado, ruta_archivo_pdf, "");
						}
						catch (Exception)
						{
							renombrar = true;
						}
					}
					else
					{
						Archivo.Borrar(ruta_archivo_pdf);
						renombrar = true;
					}
				}

				if (renombrar == true)
				{
					if (!Archivo.ValidarExistencia(ruta_archivo_pdf))
					{
						try
						{
							Archivo.Mover(ruta_archivo_resultado, ruta_archivo_pdf, "");
							borrado_resultado = true;
						}
						catch (Exception)
						{
							renombrar = true;
						}

					}
					else
					{
						renombrar = false;
					}
				}

				if (borrado_resultado == true)
				{
					if (Archivo.ValidarExistencia(ruta_archivo_resultado))
					{
						try
						{
							if (Archivo.ValidarExistencia(ruta_archivo_pdf))
							{
								Archivo.Borrar(ruta_archivo_resultado);
							}

							borrado = false;
							renombrar = false;
							borrado_resultado = false;
						}
						catch (Exception)
						{
							borrado = false;
							renombrar = false;
							borrado_resultado = false;
						}

					}
					else if (!Archivo.ValidarExistencia(ruta_archivo_pdf))
					{
						borrado = false;
						renombrar = false;
						borrado_resultado = false;
					}
				}

				if (borrado == false && renombrar == false && borrado_resultado == false)
					throw new ApplicationException(string.Format("Error agregando el texto '{0}' en el PDF {1}. Detalle: {2}", texto, ruta_archivo_pdf, excepcion.Message), excepcion);
			}

			return ruta_archivo_resultado;
		}

		public static bool BuscarTextoPDF(string ruta_archivo_pdf, string texto_consulta)
		{
			try
			{
				// valida que la ruta del PDF a procesar no esté vacía
				if (string.IsNullOrEmpty(ruta_archivo_pdf))
					throw new ApplicationException("No se encuentra la ruta del archivo PDF para procesar.");

				// valida la extensión del archivo PDF
				if (!Path.GetExtension(ruta_archivo_pdf).ToUpperInvariant().Equals(".PDF"))
					throw new ApplicationException(string.Format("La extensión del archivo para procesar '{0}' no es válida.", Path.GetExtension(ruta_archivo_pdf)));

				//create PdfReader object to read from the existing document
				PdfReader reader = new PdfReader(ruta_archivo_pdf);
				string texto_pdf = string.Empty;
				for (int page = 1; page <= reader.NumberOfPages; page++)
				{
					texto_pdf += iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, page);
				}
				reader.Close();

				if (texto_pdf.Contains(texto_consulta))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(string.Format("Error buscando el texto '{0}' en el PDF {1}. Detalle: {2}", texto_consulta, ruta_archivo_pdf, excepcion.Message), excepcion);
			}

		}



	}
}
