using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.ObjetosComunes.Licenciamiento
{
	/// <summary>
	/// Información base, registrada del lado del cliente
	/// </summary>
	public class ValidarLicencia
	{
		public string IdentificacionEmpresa { get; set; }
		public string CodigoAplicativo { get; set; }
		public string SerialAplicativo { get; set; }
		public int EdicionAplicativo { get; set; }
		public string PinEstacion { get; set; }
		public string SerialEstacion { get; set; }
		public string Version { get; set; }
		public int NumeroAccesos { get; set; }
		public int NumeroCiasRegistro { get; set; }
		public DateTime FechaConsulta { get; set; }
		public Sql Sql { get; set; }
		public List<Impresoras> Impresoras { get; set; }
		public List<InfoFramework> Frameworks { get; set; }
		public InfoOS InfoSistemaOperativo { get; set; }
		public string Mensaje { get; set; }

		//Datos para validación de registro de servidor
		public string CodigoLicencia { get; set; }
		public string CodigoCompania { get; set; }
		public string Servidor { get; set; }
		public string BaseDatos { get; set; }
	}

	/// <summary>
	/// Información de conexión de sql
	/// </summary>
	public class Sql
	{
		public string Instancia { get; set; }
		public string Nombre { get; set; }
		public string Notificacion { get; set; }
		public List<Propiedad> InfoPropiedades { get; set; }
	}

	/// <summary>
	/// Información de las impresoras.
	/// </summary>
	public class Impresoras
	{
		public string Nombre { get; set; }
		public List<Propiedad> InfoPropiedades { get; set; }
	}

	/// <summary>
	/// Objeto principal de la petición.
	/// </summary>
	public class Peticion
	{
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
		/// NOTA:
		/// En el caso de HGInet Móvil este parametro será recibido con la siguiente estructura, 
		/// ya que cuenta con dos posibles códigos de estación (Imei - DeviceID)
		/// (1,2
		/// </summary>
		public string PinEstacion { get; set; }

		public int EdicionAplicativo { get; set; }
		public string DatosAdicionales { get; set; }

	}


	/// <summary>
	/// Propiedades del servidor sql
	/// </summary>
	public class Propiedad
	{
		public string Nombre { get; set; }
		public string Valor { get; set; }
	}


	public class ArchivoConfiguracion
	{
		/// <summary>
		/// Identificación de la empresa.
		/// </summary>
		public string IdentificacionEmpresa { get; set; }
		public string CodigoUsuario { get; set; }

		/// <summary>
		/// Archivo
		/// </summary>
		public byte[] Archivo { get; set; }

		public string Contenido { get; set; }
		public string DescripcionContenido { get; set; }
	}

}
