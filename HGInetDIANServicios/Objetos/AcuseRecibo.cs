using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HGInetDIANServicios.DianFactura
{
	
	public partial class AcuseRecibo : object, System.ComponentModel.INotifyPropertyChanged
	{

		/// <summary>
		/// Campo para Validación Previa
		/// DianWSValidacionPrevia.UploadDocumentResponse.ZipKey;
		/// </summary>
		[System.Runtime.Serialization.OptionalFieldAttribute()]
		private string v2KeyField;

		/// <summary>
		/// Campo para Validación Previa
		/// DianWSValidacionPrevia.UploadDocumentResponse.ZipKey;
		/// </summary>
		[System.Runtime.Serialization.OptionalFieldAttribute()]
		private DianWSValidacionPrevia.XmlParamsResponseTrackId[] v2MessagesField;


		/// <summary>
		/// Campo para Validación Previa
		/// DianWSValidacionPrevia.UploadDocumentResponse.ZipKey;
		/// </summary>
		[System.Xml.Serialization.XmlElementAttribute(Order = 6)]
		public string KeyV2
		{
			get
			{
				return this.v2KeyField;
			}
			set
			{
				this.v2KeyField = value;
				this.RaisePropertyChanged("v2KeyField");
			}
		}

		[System.Runtime.Serialization.DataMemberAttribute()]
		[System.Xml.Serialization.XmlElementAttribute(Order = 7)]
		public HGInetDIANServicios.DianWSValidacionPrevia.XmlParamsResponseTrackId[] MessagesFieldV2
		{
			get
			{
				return this.v2MessagesField;
			}
			set
			{
				if ((object.ReferenceEquals(this.v2MessagesField, value) != true))
				{
					this.v2MessagesField = value;
					this.RaisePropertyChanged("v2MessagesField");
				}
			}
		}


	}

}
