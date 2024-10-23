using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class PeticionLicencia
	{
		/// <summary>
		/// Identificación de la empresa.
		/// </summary>
		public string IdentificacionEmpresa { get; set; }

		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string CodigoAplicativo { get; set; }

		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string CodigoLicencia { get; set; }

		/// <summary>
		/// Edición del aplicativo.
		/// </summary>
		public int CodigoEdicion { get; set; }
	}

	public class PeticionEstacion
	{
		/// <summary>
		/// Identificación de la empresa.
		/// </summary>
		public string IdentificacionEmpresa { get; set; }

		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string CodigoAplicativo { get; set; }

		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string CodigoLicencia { get; set; }

		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string PinEstacion { get; set; }

		/// <summary>
		/// Edición del aplicativo.
		/// </summary>
		public int CodigoEdicion { get; set; }
		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Nombre del servidor.
		/// </summary>
		public int NumeroAccesos { get; set; }

		/// <summary>
		/// Versión de la estación.
		/// </summary>
		public string Version { get; set; }

	}

	public class PeticionServidor
	{
		/// <summary>
		/// Identificación de la empresa.
		/// </summary>
		public string IdentificacionEmpresa { get; set; }

		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string CodigoLicencia { get; set; }

		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string CodigoCompania { get; set; }

		/// <summary>
		/// Servidor
		/// </summary>
		public string Servidor { get; set; }

		/// <summary>
		/// Servidor
		/// </summary>
		public string BaseDatos { get; set; }

		/// <summary>
		/// Usuario de la base de datos
		/// </summary>
		public string UsuarioBD { get; set; }

		/// <summary>
		/// Clave de la base de datos
		/// </summary>
		public string ClaveBD { get; set; }
		/// <summary>
		/// Servidor
		/// </summary>
		public bool NuevoRegistro { get; set; }
	}

	public class InformacionLicencia
	{

		/// <summary>
		/// Identificación de la empresa.
		/// </summary>
		public string IdentificacionEmpresa { get; set; }

		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string CodigoAplicativo { get; set; }

		/// <summary>
		/// Serial de Activación de la estación.
		/// </summary>
		public string SerialActivacion { get; set; }

		/// <summary>
		/// Edición del aplicativo.
		/// </summary>
		public int EdicionAplicativo { get; set; }

		/// <summary>
		/// Número de estaciones permitidas.
		/// </summary>
		public int NumeroEstaciones { get; set; }

		/// <summary>
		/// Número de accesos permitidas.
		/// </summary>
		public int NumeroAccesos { get; set; }

		/// <summary>
		/// Número de empresas permitidas.
		/// </summary>
		public int NumeroEmpresas { get; set; }

		/// <summary>
		/// Número de empresas permitidas.
		/// </summary>
		public int NumeroCompanias { get; set; }

		/// <summary>
		/// Indica si bloquea o no el proceso.
		/// </summary>
		public bool BloqueaProceso { get; set; }

		/// <summary>
		/// Error Generado
		/// </summary>
		public Notificacion Notificacion { get; set; }
	}

	public class InformacionEstacion
	{
		/// <summary>
		/// Id del registro de la estación.
		/// </summary>
		public int CodigoEstacion { get; set; }

		/// <summary>
		/// Identificación de la empresa.
		/// </summary>
		public string IdentificacionEmpresa { get; set; }

		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string CodigoAplicativo { get; set; }

		/// <summary>
		/// Serial de Activación de la estación.
		/// </summary>
		public string PinEstacion { get; set; }

		/// <summary>
		/// Serial de Activación de la estación.
		/// </summary>
		public string SerialActivacion { get; set; }

		/// <summary>
		/// Serial de Activación de la estación.
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Número de accesos permitidas.
		/// </summary>
		public int NumeroAccesos { get; set; }

		/// <summary>
		/// Indica si bloquea o no el proceso.
		/// </summary>
		public bool BloqueaProceso { get; set; }

		/// <summary>
		/// Indica si la estación ya se encuentra registrada.
		/// </summary>
		public bool EstacionExistente { get; set; }

		/// <summary>
		/// Indica la fecha del registro de la estación
		/// </summary>
		public DateTime FechaRegistro { get; set; }

		/// <summary>
		/// Error Generado
		/// </summary>
		public Notificacion Notificacion { get; set; }
	}

	public class InformacionServidor
	{
		/// <summary>
		/// Código del aplicativo.
		/// </summary>
		public string CodigoLicencia { get; set; }

		/// <summary>
		/// Identificación de la empresa.
		/// </summary>
		public string IdentificacionEmpresa { get; set; }

		/// <summary>
		/// Servidor
		/// </summary>
		public string Servidor { get; set; }

		/// <summary>
		/// Descripción del registro de servidores
		/// </summary>
		public string DescripcionServidor { get; set; }

		/// <summary>
		/// clave de la base de datos del registro de servidor.
		/// </summary>
		public string ClaveBaseDatos { get; set; }

		/// <summary>
		/// Servidor
		/// </summary>
		public string BaseDatos { get; set; }

		/// <summary>
		/// Usuario para conexión a la base de datos.
		/// </summary>
		public string UsuarioBd { get; set; }

		/// <summary>
		/// Fecha de registro
		/// </summary>
		public DateTime FechaRegistro { get; set; }

		/// <summary>
		/// Servidor
		/// </summary>
		public byte EstadoRegistro { get; set; }

		/// <summary>
		/// Indica si bloquea o no el proceso.
		/// </summary>
		public bool BloqueaProceso { get; set; }

		/// <summary>
		/// indica si se ha generado un nuevo registro
		/// </summary>
		public bool NuevoRegistro { get; set; }

		/// <summary>
		/// Código de la empresa configurada para ecommerce
		/// </summary>
		public short CodigoEmpresaEcommerce { get; set; }

		/// <summary>
		/// Error Generado
		/// </summary>
		public Notificacion Notificacion { get; set; }
	}
}
