﻿using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectonicaData.ModeloServicio.Documentos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
    public class NotaDebito
    {

        #region Propiedades

        /// <summary>
        /// Código de seguridad (autenticación)
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string DataKey { get; set; }

        /// <summary>
        /// Identificador del Documento asigando por el Facturador Electrónico
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string CodigoRegistro { get; set; }

        /// <summary>
        /// Numero de Documento
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public long Documento { get; set; }

        /// <summary>
        /// Documento afectado
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string DocumentoRef { get; set; }

		/// <summary>
		/// Documento de pedido
		/// </summary>
		public string PedidoRef { get; set; }

		/// <summary>
		/// Cufe identificador unico de la Nota Crédito
		/// </summary>
		public string Cufe { get; set; }

        /// <summary>
        /// Cufe identificador unico de la Factura afectada
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string CufeFactura { get; set; }

        /// <summary>
        /// Número de Resolución del Documento de Factura afectado.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression("^\\d+$", ErrorMessage = "El {0} debe contener sólo números.")]
        public string NumeroResolucion { get; set; }

		/// <summary>
		/// Prefijo del Documento según la Resolución asignada por la DIAN
		/// </summary>
		public string Prefijo { get; set; }

		/// <summary>
		/// Prefijo del Documento de factura afectado
		/// </summary>
		public string PrefijoFactura { get; set; }

		/// <summary>
		/// Fecha de la Nota Credito
		/// </summary>
		public DateTime Fecha { get; set; }

        /// <summary>
        /// Fecha de la Factura afectada
        /// </summary>
        public DateTime FechaFactura { get; set; }

        /// <summary>
        /// Observaciones del documento
        /// </summary>
        public string Nota { get; set; }

		/// <summary>
		/// Notas (observaciones) del documento
		/// </summary>
		public List<string> Notas { get; set; }

		/// <summary>
		/// Concepto por el cual genera la nota, codigo Según Tabla DIAN.
		/// </summary>
		[Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public string Concepto { get; set; }

		/// <summary>
		/// Descripcion del concepto por el cual genera la nota, codigo Según Tabla DIAN.
		/// </summary>
		public string ConceptoDescripcion { get; set; }

		/// <summary>
		/// Codigo Moneda del documento segun tabla DIAN
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio")]
        [MaxLength(3, ErrorMessage = "La {0} no puede superar los {1} caracteres")]
        public string Moneda { get; set; }

        /// <summary>
        /// Datos del Obligado a facturar
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public Tercero DatosObligado { get; set; }

        /// <summary>
        /// Datos del Adquiriente de la Factura
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public Tercero DatosAdquiriente { get; set; }

        /// <summary>
        /// Valor del documento sin descuentos y sin impuestos
        /// </summary>
        //[Required(ErrorMessage = "{0} es un campo obligatorio")]
        //[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Subtotal del documento: valor del documento con descuentos y sin impuestos
        /// </summary>
        //[Required(ErrorMessage = "{0} es un campo obligatorio")]
        //[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorSubtotal { get; set; }

        /// <summary>
        /// Valor de descuento total del documento
        /// </summary>
        //[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorDescuento { get; set; }

        /// <summary>
        /// Valor Total de Descuento del detalle - Es calculado por la plataforma para la generacion de Formatos en V2
        /// </summary>
        public decimal ValorDescuentoDet { get; set; }

		/// <summary>
		/// Valor Total de Cargos que suman al total del documento
		/// </summary>
		public decimal ValorCargo { get; set; }

        /// <summary>
        /// Valor Total de Anticipos que restan al total del documento
        /// </summary>
        public decimal ValorAnticipo { get; set; }

		/// <summary>
		/// Valor total de IVA del documento
		/// </summary>
		//[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorIva { get; set; }

        /// <summary>
        /// Valor total de Retención de IVA del documento
        /// </summary>
        //[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorReteIva { get; set; }

        /// <summary>
        /// Valor total de Impuesto al consumo del documento
        /// </summary>
        //[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorImpuestoConsumo { get; set; }

        /// <summary>
        /// Valor total de Retención en la Fuente del documento
        /// </summary>
        //[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorReteFuente { get; set; }

        /// <summary>
        /// Valor total de Retención de ICA del documento.
        /// </summary>
        //[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal ValorReteIca { get; set; }

        /// <summary>
        /// Valor total del documento: Subtotal incluyendo descuentos e impuestos agregados
        /// </summary>
        //[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Total { get; set; }

        /// <summary>
        /// Valor neto del documento aplicando impuestos de retención
        /// </summary>
        //[Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "El valor de {0} debe estar entre {1} y {2}")]
        public decimal Neto { get; set; }

        /// <summary>
        /// Detalle del documento
        /// </summary>
        public List<DocumentoDetalle> DocumentoDetalles { get; set; }

		/// <summary>
		/// Datos del formato del documento
		/// </summary>
		public Formato DocumentoFormato { get; set; }

        /// <summary>
		/// Version de la aplicacion donde fue generado el documento
		/// </summary>
		public string VersionAplicativo { get; set; }
		
		/// <summary>
		/// Número de identificación del proveedor tecnológico 
		/// </summary>
		public string IdentificacionProveedor { get; set; }

		/// <summary>
		/// Archivos anexos
		/// </summary>
		public Anexo ArchivoAnexos { get; set; }
        /// <summary>
        /// Id de seguridad del plan de donde se va a descontar el presente documento
        /// </summary>
        public Guid IdPlan { get; set; }

        /// <summary>
        /// Lista de Descuentos que afectan el total a Pagar
        /// </summary>
        public List<Descuento> Descuentos { get; set; }

        /// <summary>
        /// Lista de Cargos que afectan el total a pagar
        /// </summary>
        public List<Cargo> Cargos { get; set; }

        /// <summary>
        /// Anticipos generados al documento
        /// </summary>
        public List<Anticipo> Anticipos { get; set; }

		/// <summary>
		/// Tipo Operacion del documento ( 30 - referencia una factura electrónica, 32 - sin referencia a facturas, 33 - facturación electrónica V1 (Decreto 2242))
		/// </summary>
		public int TipoOperacion { get; set; }

		/// <summary>
		/// Lista de documentos de referencia adicionales (AdditionalDocumentReference)
		/// </summary>
		public List<ReferenciaAdicional> DocumentosReferencia { get; set; }

		/// <summary>
		/// documento de referencia adicionales (OrderReference)
		/// </summary>
		public ReferenciaAdicional OrderReference { get; set; }

		/// <summary>
		/// Lista de documentos de referencia adicionales (DespatchDocumentReference)
		/// </summary>
		public List<ReferenciaAdicional> DespatchDocument { get; set; }

		/// <summary>
		/// Lista de documentos de referencia adicionales (ReceiptDocument)
		/// </summary>
		public List<ReferenciaAdicional> ReceiptDocument { get; set; }

		/// <summary>
		/// Si hay un acuerdo entre un Facturador y Adquiriente se muestra en el asunto del correo
		/// </summary>
		public string LineaNegocio { get; set; }

		/// <summary>
		/// Tasa de cambio de moneda extranjera
		/// </summary>
		public TasaCambio Trm { get; set; }

		/// <summary>
		/// Campos Adicionales requeridos para informar datos necesarios para el Sector Salud Resolucion 084
		/// </summary>
		public Salud SectorSalud { get; set; }

		/// <summary>
		/// Indica si se envia notificacion SMS al Adquiriente cuando un documento es valido en la DIAN: 0 - No, 1 - Sí
		/// </summary>
		public int EnvioSms { get; set; }

		/// <summary>
		/// Identificacion de la empresa generadora (Integrador) del documento
		/// </summary>
		public string IdentificacionIntegrador { get; set; }

		/// <summary>
		/// Campo donde la plataforma llenera la informacion correspondiente a la firma digital solo para la generacion del PDF
		/// </summary>
		public string FirmaDoc { get; set; }

		#endregion
	}
}
