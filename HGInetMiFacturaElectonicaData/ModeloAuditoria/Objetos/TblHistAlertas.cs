using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos
{
	public class TblHistAlertas: TblGenerica
	{	

		[BsonElement("IntIdAlerta")]
		public int IntIdAlerta { get; set; }

		[BsonElement("DatFecha")]
		public DateTime DatFecha { get; set; }

		[BsonElement("StrIdSeguridadFact")]
		public string StrIdSeguridadFact { get; set; }

		[BsonElement("StrIdentificacion")]
		public string StrIdentificacion { get; set; }

		[BsonElement("IntIdEstado")]
		public int IntIdEstado { get; set; }

		[BsonElement("StrObservaciones")]
		public string StrObservaciones { get; set; }

		[BsonElement("IntTipo")]
		public int IntTipo { get; set; }

		[BsonElement("StrIdSeguridadPlan")]
		public string StrIdSeguridadPlan { get; set; }
	}
}

