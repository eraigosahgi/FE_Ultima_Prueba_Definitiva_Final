using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	[System.Xml.Serialization.XmlRootAttribute("CustomTagGeneral", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2", IsNullable = false)]
	public partial class CustomTagGeneral1
	{
		private InformacionNegociacionType InformacionNegociacionField;

		private InformacionAvalarType InformacionAvalarField;

		private ConstanciadePagosType ConstanciadePagosField;

		private InformacionParaelPagoType InformacionParaelPagoField;

		private InformacionPagoType InformacionPagoField;

		public InformacionNegociacionType InformacionNegociacion
		{
			get
			{
				return this.InformacionNegociacionField;
			}
			set
			{
				this.InformacionNegociacionField = value;
			}
		}

		public InformacionAvalarType InformacionAvalar
		{
			get
			{
				return this.InformacionAvalarField;
			}
			set
			{
				this.InformacionAvalarField = value;
			}
		}

		public ConstanciadePagosType ConstanciadePagos
		{
			get
			{
				return this.ConstanciadePagosField;
			}
			set
			{
				this.ConstanciadePagosField = value;
			}
		}

		public InformacionParaelPagoType InformacionParaelPago
		{
			get
			{
				return this.InformacionParaelPagoField;
			}
			set
			{
				this.InformacionParaelPagoField = value;
			}
		}

		public InformacionPagoType InformacionPagos
		{
			get
			{
				return this.InformacionPagoField;
			}
			set
			{
				this.InformacionPagoField = value;
			}
		}
	}
}
