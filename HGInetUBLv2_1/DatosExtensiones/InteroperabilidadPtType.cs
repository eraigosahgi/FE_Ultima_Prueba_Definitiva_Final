using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class InteroperabilidadPtType
	{
		private URLDescagaAdjuntosType URLDescargaAdjuntosField;

		private EntregaDocumentoType EntregaDocumentoField;


		/// <comentarios/>
		//[System.Xml.Serialization.XmlAttributeAttribute()]
		public URLDescagaAdjuntosType URLDescargaAdjuntos
		{
			get
			{
				return this.URLDescargaAdjuntosField;
			}
			set
			{
				this.URLDescargaAdjuntosField = value;
			}
		}

		public EntregaDocumentoType EntregaDocumento
		{
			get
			{
				return this.EntregaDocumentoField;
			}
			set
			{
				this.EntregaDocumentoField = value;
			}
		}
	}
}
