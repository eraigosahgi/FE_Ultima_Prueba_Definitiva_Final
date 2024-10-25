using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos
{
	/// <summary>
	/// Propiedades genericas de MongoDB, los objetos relacionados con este motor de base de datos,
	/// deberán heredar de esta clase, con el fin de contener propiedades especficas del mismo.
	/// </summary>
	public class TblGenerica
	{
		[BsonId]
		public ObjectId _id { get; set; }

		[DataMember]
		public string Id
		{
			get { return _id.ToString(); }
			set { _id = ObjectId.Parse(value); }
		}

	}
}
