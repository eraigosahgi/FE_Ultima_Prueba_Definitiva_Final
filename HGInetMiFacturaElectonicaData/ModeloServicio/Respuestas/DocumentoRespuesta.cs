using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.Error;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	/// <summary>
	/// Datos de respuesta para los documentos
	/// </summary>
	public class DocumentoRespuesta
	{
		/// <summary>
		/// Id único del documento generado por la Plataforma
		/// </summary>
		public string IdDocumento { get; set; }

		/// <summary>
		/// Fecha de recepción del documento por la Plataforma
		/// </summary>
		public DateTime FechaRecepcion { get; set; }

		/// <summary>
		/// Id único de Registro del Obligado a Facturar
		/// </summary>
		public string CodigoRegistro { get; set; }

		/// <summary>
		/// Identificación adquiriente.
		/// </summary>
		public string Identificacion { get; set; }

		/// <summary>
		/// Número de Resolución asignado por la DIAN.(Aplica para Documento tipo Factura)
		/// </summary>
		public string NumeroResolucion { get; set; }

		/// <summary>
		/// Prefijo de la Factura. (Aplica para Documento tipo Factura)
		/// </summary>
		public string Prefijo { get; set; }

		/// <summary>
		/// Indica el tipo de Documento(1: factura - 2: nota débito - 3: nota crédito)
		/// </summary>
		public int DocumentoTipo { get; set; }

		/// <summary>
		/// Número de Documento
		/// </summary>
		public long Documento { get; set; }

		/// <summary>
		/// Código identificador del documento ante la DIAN
		/// </summary>
		public string Cufe { get; set; }

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
		/// Ruta http de los archivos anexos relacionado con el documento.
		/// </summary>
		public string UrlAnexo { get; set; }

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

		/// <summary>
		/// Indica el id de la categoria actual del documento en la Plataforma.
		/// </summary>
		public int IdEstado { get; set; }

		/// <summary>
		/// Descripción de la categoria actual del documento en la Plataforma.
		/// </summary>
		public string DescripcionEstado { get; set; }
		/// <summary>
		/// Indica si descuenta del saldo del plan en caso de que si lo reciba la plataforma
		/// </summary>
		public bool DescuentaSaldo { get; set; }
		/// <summary>
		/// Id de seguridad del plan de donde se va a descontar el presente documento
		/// </summary>
		public Guid IdPlan { get; set; }

		/// <summary>
		/// Número de identificación del obligado.
		/// </summary>
		public string IdentificacionObligado { get; set; }

		/// <summary>
		/// id único de identificación de la plataforma
		/// </summary>
		public Guid IdPeticion { get; set; }

		public string UrlAuditoria { get; set; }
	}
}
