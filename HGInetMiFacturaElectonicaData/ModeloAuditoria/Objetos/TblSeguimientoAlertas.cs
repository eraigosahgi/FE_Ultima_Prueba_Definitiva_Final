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
	public class TblSeguimientoAlertas: TblGenerica
	{


		[BsonElement("DatFecha")]
		public DateTime DatFecha { get; set; }

		[BsonElement("IntIdTipo")]
		public int IntIdTipo { get; set; }

		[BsonElement("IntIdAlerta")]
		public int IntIdAlerta { get; set; }

		[BsonElement("StrIdSeguridadEmpresa")]
		public string StrIdSeguridadEmpresa { get; set; }

		[BsonElement("StrIdentificacion")]
		public string StrIdentificacion { get; set; }

		[BsonElement("IntIdEstado")]
		public int IntIdEstado { get; set; }

		[BsonElement("StrMensaje")]
		public string StrMensaje { get; set; }

		[BsonElement("StrResultadoProceso")]
		public string StrResultadoProceso { get; set; }

		[BsonElement("StrIdSeguridadPlan")]
		public string StrIdSeguridadPlan { get; set; }
	}
}

