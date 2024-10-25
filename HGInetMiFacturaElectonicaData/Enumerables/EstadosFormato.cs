using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
	public enum EstadosFormato
	{
		[Description("Inactivo")]
		Inactivo = 0,

		[Description("Activo")]
		Activo = 1,

		[Description("Pendiente Solicitud Aprobación")]
		SolicitarAprobacion = 2,

		[Description("Pendiente de Aprobación")]
		PendienteAprobacion = 3,

		[Description("Aprobado")]
		Aprobado = 4,

		[Description("Pendiente Publicación")]
		PendientePublicacion = 5,
	}


	public enum TiposProceso
	{
		[Description("Creación")]
		Creacion = 1,

		[Description("Edición de Formato")]
		Edicion = 2,

		[Description("Cambio de Estado")]
		CambioEstado = 3,

		[Description("Solicitud Aprobación de Formato")]
		SolicitudAprobacion = 4,

		[Description("Aprobación de Formato")]
		Aprobacion = 5,

		[Description("Rechazo de Formato")]
		Rechazo = 6,

		[Description("Publicación de Formato")]
		Publicacion = 7,
	}
}
