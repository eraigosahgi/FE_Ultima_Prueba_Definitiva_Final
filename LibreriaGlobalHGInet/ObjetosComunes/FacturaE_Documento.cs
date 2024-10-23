using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Objetos
{
	public class FacturaE_Documento
	{
		/// <summary>
		/// Guid de seguridad de la petición
		/// </summary>
		public Guid IdSeguridadPeticion;

		/// <summary>
		/// Guid de seguridad del documento al almacenarlo en base de datos
		/// </summary>
		public Guid IdSeguridadDocumento;

		/// <summary>
		/// Guid del facturador electrónico
		/// </summary>
		public Guid IdSeguridadTercero;

		/// <summary>
		/// Datos del documento como objeto
		/// </summary>
		public object Documento;

		/// <summary>
		/// Tipo de documento (Factura, Nota Débito, Nota Crédito)
		/// </summary>
		public TipoDocumento DocumentoTipo;

		/// <summary>
		/// Ruta de archivos con los que se esta haciendo el proceso
		/// </summary>
		public string RutaArchivosProceso;

		/// <summary>
		/// Ruta de archivos que se utilizan para hacer envio a la DIAN
		/// </summary>
        public string RutaArchivosEnvio;

		/// <summary>
		/// Nombre del archivo XML-UBL con la notacion de la DIAN y sin extension
		/// </summary>
        public string NombreXml;

		/// <summary>
		/// Nombre del archivo pdf con la notacion de la DIAN y sin extension
		/// </summary>
		public string NombrePdf;

		/// <summary>
		/// Nombre del archivo zip 
		/// </summary>
        public string NombreZip;

		/// <summary>
		/// Texto CUFE del documento
		/// </summary>
		public string CUFE;

		/// <summary>
		/// Texto del xml
		/// </summary>
		public StringBuilder DocumentoXml;

		public ApplicationException Excepcion;

		/// <summary>
		/// Versión de la DIAN (1: 2018 - 2: Validación Previa 2019)
		/// </summary>
		public int VersionDian;

	}

	/// <summary>
	/// Tipos de documentos para Facturación Electrónica
	/// </summary>
	public enum TipoDocumento
	{
		[Description("Factura Electrónica de Venta")]
		Factura = 1,

		[Description("Nota Débito")]
		NotaDebito = 2,

		[Description("Nota Crédito")]
		NotaCredito = 3,

        [Description("Acuse de Recibo")]
        AcuseRecibo = 4,

        [Description("Attached de Recibo")]
        Attached = 5,

        [Description("Documento Soporte de Pago de Nómina Electrónica")]
        Nomina = 10,

		[Description("Nota de Ajuste de Documento Soporte de Pago de Nómina Electrónica")]
		NominaAjuste = 11
	}

}
