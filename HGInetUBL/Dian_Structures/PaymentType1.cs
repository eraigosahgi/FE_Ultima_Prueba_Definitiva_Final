using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HGInetUBL
{
	/// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(TypeName="PaymentType", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1")]
	[System.Xml.Serialization.XmlRootAttribute("PrepaidPayment", Namespace="http://www.dian.gov.co/contratos/facturaelectronica/v1", IsNullable=false)]
	public partial class PaymentType1 {
        
		private IDType idField;
        
		private PaidAmountType paidAmountField;
        
		private ReceivedDateType receivedDateField;
        
		private PaidDateType paidDateField;
        
		private PaidTimeType paidTimeField;
        
		private InstructionIDType instructionIDField;
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public IDType ID {
			get {
				return this.idField;
			}
			set {
				this.idField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PaidAmountType PaidAmount {
			get {
				return this.paidAmountField;
			}
			set {
				this.paidAmountField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public ReceivedDateType ReceivedDate {
			get {
				return this.receivedDateField;
			}
			set {
				this.receivedDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PaidDateType PaidDate {
			get {
				return this.paidDateField;
			}
			set {
				this.paidDateField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public PaidTimeType PaidTime {
			get {
				return this.paidTimeField;
			}
			set {
				this.paidTimeField = value;
			}
		}
        
		/// <comentarios/>
		[System.Xml.Serialization.XmlElementAttribute(Namespace="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
		public InstructionIDType InstructionID {
			get {
				return this.instructionIDField;
			}
			set {
				this.instructionIDField = value;
			}
		}
	}

    
    /// <summary>
    /// Enumerable de las Formas de pagos
    /// </summary>
    public enum Meanscode
    {
        /// <comentarios/>
		[Description("10 - Efectivo")]
        Efectivo = 10,

        /// <comentarios/>
        [Description("20 - Cheque")]
        Cheque = 20,

        /// <comentarios/>
        [Description("24 - Factura electr�nica esperando que el adquirente la ACEPTE")]
        Factura_electronica = 24,

        /// <comentarios/>
        [Description("41 - Transferencia bancaria")]
        Transferencia = 41,

        /// <comentarios/>
        [Description("42 - Consignaci�n bancaria")]
        Consignaci�n = 42,

        /// <comentarios/>
        [Description("54 - Tarjeta cr�dito")]
        Tarjeta_credito = 54,

        /// <comentarios/>
        [Description("55 - Tarjeta d�bito")]
        Tarjeta_debito = 55,

        /// <comentarios/>
        [Description("68 - Servicio de pago en l�nea")]
        Pago_linea = 68,

    }

    /// <summary>
    /// Enumerable Terminos de Pago
    /// </summary>
    public enum termstype
    {

        /// <comentarios/>
		[Description("2 - Fin de mes")]
        Fin_mes = 2,

        /// <comentarios/>
        [Description("3 - Fecha fija")]
        Fecha_fija = 3,

        /// <comentarios/>
        [Description("14 - Pago contra entrega")]
        Contra_entrega = 14,

        /// <comentarios/>
        [Description("78 - Factoring")]
        Factoring = 78,
    }
}