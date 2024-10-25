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

	public class TblAuditDocumentos : TblGenerica
	{
		[BsonElement("StrIdSeguridad")]
		public string StrIdSeguridad { get; set; }


		[BsonElement("StrIdPeticion")]
		public string StrIdPeticion { get; set; }


		[BsonElement("DatFecha")]
		public DateTime DatFecha { get; set; }


		[BsonElement("StrObligado")]
		public string StrObligado { get; set; }


		[BsonElement("IntIdEstado")]
		public int IntIdEstado { get; set; }


		[BsonElement("IntIdProceso")]
		public int IntIdProceso { get; set; }


		[BsonElement("IntTipoRegistro")]
		public int IntTipoRegistro { get; set; }


		[BsonElement("IntIdProcesadoPor")]
		public int IntIdProcesadoPor { get; set; }


		[BsonElement("StrRealizadoPor")]
		public string StrRealizadoPor { get; set; }


		[BsonElement("StrMensaje")]
		public string StrMensaje { get; set; }


		[BsonElement("StrResultadoProceso")]
		public string StrResultadoProceso { get; set; }

		[BsonElement("StrPrefijo")]
		public string StrPrefijo { get; set; }

		[BsonElement("StrNumero")]
		public string StrNumero { get; set; }

	}
}
