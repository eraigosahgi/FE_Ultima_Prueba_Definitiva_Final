using LibreriaGlobalHGInet.Error;
using System;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Objeto de retorno al consultar documentos de tipo Nota Débito
	/// </summary>
	public class NotaDebitoConsulta
	{

		/// <summary>
		/// Información de la nota débito
		/// </summary>
		public NotaDebito DatosNotaDebito { get; set; }

		/// <summary>
		/// Id único del documento generado por la Plataforma
		/// </summary>
		public string IdDocumento { get; set; }

		/// <summary>
		/// Id único de Registro del Obligado a Facturar
		/// </summary>
		public string CodigoRegistro { get; set; }

		/// <summary>
		/// Identificación facturador electrónico
		/// </summary>
		public string IdentificacionFacturador { get; set; }

		/// <summary>
		/// Número de Documento
		/// </summary>
		public int Documento { get; set; }

		/// <summary>
		/// Indica el id del proceso actual del documento en la Plataforma.
		/// </summary>
		public int IdProceso { get; set; }

		/// <summary>
		/// Descripción del proceso actual del documento en la Plataforma.
		/// </summary>
		public string DescripcionProceso { get; set; }

		/// <summary>
		/// Indica la aceptación o no del documento. 0: Pendiente, 1: Aceptación, 2: Rechazo
		/// </summary>
		public int Aceptacion { get; set; }

		/// <summary>
		/// Observaciones del Adquiriente de acuerdo con el rechazo del documento.
		/// </summary>
		public string MotivoRechazo { get; set; }

		/// <summary>
		/// Ruta http del archivo XML en estándar UBL relacionado con el documento.
		/// </summary>
		public string UrlXmlUbl { get; set; }

		/// <summary>
		/// Ruta http del archivo PDF relacionado con el documento.
		/// </summary>
		public string UrlPdf { get; set; }

		/// <summary>
		/// Fecha del último proceso del documento realizado por la Plataforma
		/// </summary>
		public DateTime FechaUltimoProceso { get; set; }

		/// <summary>
		/// Indica si el documento ha finalizado todos los procesos en la Plataforma (0: Procesos pendientes, 1: Procesos finalizados)
		/// </summary>
		public int ProcesoFinalizado { get; set; }

		/// <summary>
		/// Objeto de tipo Error 
		/// </summary>
		public Error Error { get; set; }


		/// <summary>
		/// Objeto de tipo Respuesta entregada por la Dian despues de una consulta
		/// </summary>
		public RespuestaDian EstadoDian { get; set; }

	}
}
