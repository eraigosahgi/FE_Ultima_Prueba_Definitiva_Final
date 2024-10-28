using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos
{
	public class TblAuditFormatos : TblGenerica
	{
		[BsonElement("IntCodigoFormato")]
		public int IntCodigoFormato { get; set; }


		[BsonElement("StrEmpresa")]
		public string StrEmpresa { get; set; }


		[BsonElement("StrIdSeguridad")]
		public string StrIdSeguridad { get; set; }


		[BsonElement("IntTipoProceso")]
		public int IntTipoProceso { get; set; }


		[BsonElement("StrUsuarioProceso")]
		public string StrUsuarioProceso { get; set; }


		[BsonElement("DatFechaProceso")]
		public DateTime DatFechaProceso { get; set; }


		[BsonElement("StrObservaciones")]
		public string StrObservaciones { get; set; }


	}
}
