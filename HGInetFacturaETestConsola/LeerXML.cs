using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace HGInetFacturaETestConsola
{
	
    public class LeerXML
	{

		public static Documento Procesar(XDocument xml)
		{
			try
			{


				XNamespace cbc = @"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";

				var doc = new Documento();


				//UBLVersionID
				var UBLVersionID = (from x in xml.Descendants(cbc + "UBLVersionID")
									select x).FirstOrDefault();
				if (UBLVersionID != null)
				{
					doc.UBLVersionID = (UBLVersionID.Value == null ? "0.0" : UBLVersionID.Value.ToString());
				}
				//CustomizationID
				var ProfileID = (from x in xml.Descendants(cbc + "ProfileID")
									   select x).FirstOrDefault();
				if (ProfileID != null)
				{
					doc.ProfileID = (ProfileID.Value == null ? "0.0" : ProfileID.Value.ToString());
				}
				//CustomizationID
				var ID = (from x in xml.Descendants(cbc + "ID")
						  select x).FirstOrDefault();
				if (ID != null)
				{
					doc.ID = Int64.Parse(ID.Value == null ? "0" : ID.Value.ToString());
				}
				//IssueDate
				var IssueDate = (from x in xml.Descendants(cbc + "IssueDate")
								 select x).FirstOrDefault();
				if (IssueDate != null)
				{
					doc.IssueDate = DateTime.Parse(IssueDate.Value == null ? "1900-01-01" : IssueDate.Value.ToString());
				}
				//IssueTime
				var IssueTime = (from x in xml.Descendants(cbc + "IssueTime")
								 select x).FirstOrDefault();
				if (IssueTime != null)
				{
					doc.IssueTime = DateTime.Parse(IssueTime.Value == null ? "1900-01-01" : "1900-01-01 " + IssueTime.Value.ToString());
				}
				//IssueDate
				var ResponseDate = (from x in xml.Descendants(cbc + "ResponseDate")
									select x).FirstOrDefault();
				if (ResponseDate != null)
				{
					doc.ResponseDate = DateTime.Parse(ResponseDate.Value == null ? "1900-01-01" : ResponseDate.Value.ToString());
				}
				//IssueTime
				var ResponseTime = (from x in xml.Descendants(cbc + "ResponseTime")
									select x).FirstOrDefault();
				if (ResponseTime != null)
				{
					doc.ResponseTime = DateTime.Parse(ResponseTime.Value == null ? "1900-01-01" : "1900-01-01 " + ResponseTime.Value.ToString());
				}
				//IssueTime
				var Notes = (from x in xml.Descendants(cbc + "Note")
							 select x).ToList();
				doc.Notes = new List<string>();
				foreach (var n in Notes)
				{
					doc.Notes.Add(n.Value ?? "");
				}

				//DocumentCurrencyCode
				var DocumentCurrencyCode = (from x in xml.Descendants(cbc + "DocumentCurrencyCode")
											select x).FirstOrDefault();
				if (DocumentCurrencyCode != null)
				{
					doc.DocumentCurrencyCode = (DocumentCurrencyCode.Value == null ? " " : DocumentCurrencyCode.Value);
				}


				return doc;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}

