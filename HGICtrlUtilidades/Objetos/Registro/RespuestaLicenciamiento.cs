using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class RespuestaLicenciamiento
	{
		public string CompaniaCodigo { get; set; }
		public string CompaniaDescripcion { get; set; }
		public string VersionERP { get; set; }
		/// <summary>
		/// Número de identificación de la empresa
		/// </summary>
		public string IdentificacionEmpresa { get; set; }

		/// <summary>
		/// Código del aplicativo
		/// </summary>
		public string CodigoAplicativo { get; set; }

		/// <summary>
		/// Pin de la estación.
		/// </summary>
		public string PinEstacion { get; set; }

		/// <summary>
		/// Edición del aplicativo
		/// </summary>
		public int EdicionAplicativo { get; set; }

		/// <summary>
		/// objeto InformacionAdicional en formato json
		/// </summary>
		public string InformacionAdicional { get; set; }

		/// <summary>
		///Información anexa de la respuesta.
		/// </summary>
		public InformacionAdicional DatosAdicionales { get; set; }

		public TipoBloqueo ValidacionBloqueo { get; set; }

		/// <summary>
		/// Error Generado
		/// </summary>
		public Notificacion Notificacion { get; set; }
	}


	public class InformacionAdicional
	{
		/// <summary>
		/// Indica si la licencia es válida
		/// </summary>
		public bool LicenciaValida { get; set; }

		/// <summary>
		/// Fecha de vencimiento del contrato actual.
		/// </summary>
		public DateTime FechaContrato { get; set; }

		/// <summary>
		/// Última versión disponible.
		/// </summary>
		public string VersionDisponible { get; set; }

		public string RutaServiciosWeb { get; set; }

		/// <summary>
		/// Mensaje de validación de licencia.
		/// </summary>
		public string Mensaje { get; set; }

		/// <summary>
		/// Serial de activación del aplicativo
		/// </summary>
		public string SerialAplicativo { get; set; }

		/// <summary>
		/// Digito de verificación del tercero.
		/// </summary>
		public short DvTercero { get; set; }

		/// <summary>
		/// Tipo de identificación del tercero.
		/// </summary>
		public string TipoIdTercero { get; set; }

		/// <summary>
		/// Razón Social del tercero
		/// </summary>
		public string RazonSocialTercero { get; set; }

		public string EmailTercero { get; set; }
		public string DireccionTercero { get; set; }
		public string TelefonoTercero { get; set; }
		public short TipoPersonaTercero { get; set; }

		/// <summary>
		/// Descripción del aplicativo
		/// </summary>
		public string DescripcionAplicativo { get; set; }

		/// <summary>
		/// Descripción de la edición
		/// </summary>
		public string DescripcionEdicion { get; set; }

		/// <summary>
		/// Número de compañías de la licencia de uso del producto.
		/// </summary>
		public int NumeroCompaniasApp { get; set; }

		/// <summary>
		/// Número de empresas de la licencia de uso del producto.
		/// </summary>
		public int NumeroEmpresasApp { get; set; }

		/// <summary>
		/// Número de accesos de la licencia de uso del producto.
		/// </summary>
		public int NumeroAccesosApp { get; set; }

		/// <summary>
		/// Código de la empresa para la autenticación
		/// </summary>
		public short CodigoEmpresa { get; set; }

		/// <summary>
		/// Indica la fecha de la próxima validación de la licencia
		/// </summary>
		public DateTime FechaProximaValidacion { get; set; }

		public string RutaPublicaEcommerce { get; set; }
		/// <summary>
		/// Información del registro de servidor
		/// </summary>
		public InformacionServidor InfoServidor { get; set; }
	}


	public class RespuestaEliminar
	{
		public bool ProcesoExitoso { get; set; }
		public Notificacion Notificacion { get; set; }
	}

	public class RespuestaLicenciaHappgi
	{
		public string ProductoCodigo { get; set; }
		public string ProductoDescripcion { get; set; }
		public string CompaniaCodigo { get; set; }
		public DateTime FechaContrato { get; set; }
		public DateTime FechaVencimiento { get; set; }
		public DateTime FechaProximaValidacion { get; set; }
		public DateTime FechaVencimientoGracia { get; set; }
		public string TerceroTipoId { get; set; }
		public string TerceroIdentificacion { get; set; }
		public short TerceroDv { get; set; }
		public string TerceroNombre { get; set; }
		public string TerceroEmail { get; set; }
		public string TerceroDireccion { get; set; }
		public string TerceroTelefono { get; set; }
		public short TerceroTipoPersona { get; set; }
		public int NumeroDocumento { get; set; }
		public string IP { get; set; }
		/// <summary>
		/// Información del servidor
		/// </summary>
		public InformacionServidor InfoServidor { get; set; }
		/// <summary>
		/// Lista de licencias registradas para el tercero
		/// </summary>
		public List<Licencia> LicenciasRegistradas { get; set; }
		/// <summary>
		/// Error Generado
		/// </summary>
		public Notificacion Notificacion { get; set; }

		/// <summary>
		/// Indica si ha realizado la actualización del registro
		/// </summary>
		public bool ActualizaRegistro { get; set; }
	}

	public class Licencia
	{
		public int LicenciaCodigo { get; set; }
		public string AplicacionCodigo { get; set; }
		public string AplicacionInicialCodigo { get; set; }
		public string AplicacionDescripcion { get; set; }
		public int EdicionAplicativo { get; set; }
		public string EdicionDescripcion { get; set; }
		public int NumeroCompanias { get; set; }
		public int NumeroEmpresas { get; set; }
		public int NumeroAccesos { get; set; }
		public int EstadoLicencia { get; set; }
		/// <summary>
		/// Error Generado
		/// </summary>
		public Notificacion Notificacion { get; set; }
	}

}
