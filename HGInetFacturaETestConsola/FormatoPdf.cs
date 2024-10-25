using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetFacturaETestConsola
{

	public class FormatoPdf
	{
		public static void VerySimpleReplaceText(string OrigFile, string ResultFile, string origText, string replaceText)
		{
			using (PdfReader reader = new PdfReader(OrigFile))
			{
				for (int i = 1; i <= reader.NumberOfPages; i++)
				{
					byte[] contentBytes = reader.GetPageContent(i);
					string contentString = PdfEncodings.ConvertToString(contentBytes, PdfObject.NOTHING);
					contentString = contentString.Replace(origText, replaceText);
					reader.SetPageContent(i, PdfEncodings.ConvertToBytes(contentString, PdfObject.NOTHING));
				}
				new PdfStamper(reader, new FileStream(ResultFile, FileMode.Create, FileAccess.Write)).Close();
			}
		}
		public static void Leer()
		{

			String inFile = @"E:\FacturaFormato.pdf";
			String inFile1 = @"E:\FacturaFormato-v2.pdf";
			Rectangle rect = new Rectangle(0f, 0f, 800f, 800f);
			RenderFilter[] filter = { new RegionTextRenderFilter(rect) };
			ITextExtractionStrategy strategy;
			StringBuilder sb = new StringBuilder();
			PdfReader reader = new PdfReader(inFile);

			for (int i = 1; i <= reader.NumberOfPages; i++)
			{
				strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);
				sb.Append(PdfTextExtractor.GetTextFromPage(reader, i, strategy));
			}

			Console.WriteLine(sb.ToString());

			processPDF(inFile, inFile1);
		}

		public static void processPDF(String src, String dest)
		{

			/*
			PdfReader reader = new PdfReader(src);
			PdfStamper stamper = new PdfStamper(reader, new FileOutputStream(dest));
			Font fontNormal = new Font(FontFamily.HELVETICA, 11, Font.NORMAL, BaseColor.BLACK);
			Rectangle rect = new Rectangle(50f, 550f, 500f, 445f);
			List<PdfCleanUpLocation> cleanUpLocations = new ArrayList<PdfCleanUpLocation>();
			cleanUpLocations.add(new PdfCleanUpLocation(1, rect, BaseColor.WHITE));

			PdfContentByte cb = stamper.getOverContent(1);
			ColumnText ct = new ColumnText(cb);
			ct.setSimpleColumn(140f, 490f, 500f, 445f);
			Paragraph pz = new Paragraph(new Phrase(0, "HERE IS THE DYNAMIC TEXT", fontNormal));
			ct.addElement(pz);
			ct.go();

			PdfCleanUpProcessor cleaner = new PdfCleanUpProcessor(cleanUpLocations, stamper);
			cleaner.cleanUp();
			stamper.close();
			reader.close();*/

		}



		/*
			static iTextSharp.text.pdf.PdfStamper stamper = null;

			/// <summary>
			/// This method is used to search for the location words in pdf and update it with the words given from replacingText variable
			/// </summary>
			/// <param name="pSearch">Searchable String</param>
			/// <param name="replacingText">Replacing String</param>
			/// <param name="SC">Case Ignorance</param>
			/// <param name="SourceFile">Path of the source file</param>
			/// <param name="DestinationFile">Path of the destination file</param>
			public static void PDFTextGetter(string pSearch, string replacingText, StringComparison SC, string SourceFile, string DestinationFile)
			{
				try
				{
					iTextSharp.text.pdf.PdfContentByte cb = null;
					iTextSharp.text.pdf.PdfContentByte cb2 = null;
					iTextSharp.text.pdf.PdfWriter writer = null;
					iTextSharp.text.pdf.BaseFont bf = null;

					if (System.IO.File.Exists(SourceFile))
					{
						PdfReader pReader = new PdfReader(SourceFile);


						for (int page = 1; page <= pReader.NumberOfPages; page++)
						{
							//iTextSharp.text.pdf.parser.LocationTextExtractionStrategy.
							LocationTextExtractionStrategy strategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), filter);
							cb = stamper.GetOverContent(page);
							cb2 = stamper.GetOverContent(page);

							//Send some data contained in PdfContentByte, looks like the first is always cero for me and the second 100, 
							//but i'm not sure if this could change in some cases
							strategy.UndercontentCharacterSpacing = (int)cb.CharacterSpacing;
							strategy.UndercontentHorizontalScaling = (int)cb.HorizontalScaling;

							//It's not really needed to get the text back, but we have to call this line ALWAYS, 
							//because it triggers the process that will get all chunks from PDF into our strategy Object
							string currentText = PdfTextExtractor.GetTextFromPage(pReader, page, strategy);

							//The real getter process starts in the following line
							List<iTextSharp.text.Rectangle> MatchesFound = strategy.GetTextLocations(pSearch, SC);

							//Set the fill color of the shapes, I don't use a border because it would make the rect bigger
							//but maybe using a thin border could be a solution if you see the currect rect is not big enough to cover all the text it should cover
							cb.SetColorFill(BaseColor.WHITE);

							//MatchesFound contains all text with locations, so do whatever you want with it, this highlights them using PINK color:

							foreach (iTextSharp.text.Rectangle rect in MatchesFound)
							{
								//width
								cb.Rectangle(rect.Left, rect.Bottom, 60, rect.Height);
								cb.Fill();
								cb2.SetColorFill(BaseColor.BLACK);
								bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

								cb2.SetFontAndSize(bf, 9);

								cb2.BeginText();
								cb2.ShowTextAligned(0, replacingText, rect.Left, rect.Bottom, 0);
								cb2.EndText();
								cb2.Fill();
							}

						}
					}

				}
				catch (Exception ex)
				{

				}

			}
			*/

	}
}
