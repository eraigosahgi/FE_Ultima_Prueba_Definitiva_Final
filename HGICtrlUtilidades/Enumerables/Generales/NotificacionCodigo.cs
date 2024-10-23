using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public enum NotificacionCodigo
	{
		OK = 0,
		ERROR_NO_CONTROLADO = 1,
		ERROR_EN_SERVIDOR = 2,
		VALIDACION = 3,
		ERROR_AGREGAR = 4,
		ERROR_EDITAR = 5,
		ERROR_ELIMINAR = 6,
		ERROR_CALENDARIO_INEXISTENTE = 7,
		ERROR_EMPRESA_INEXISTENTE = 8,
		ERROR_ACCESSO_NO_DISPONIBLE = 9,
		ERROR_LICENCIA = 98,
		NINGUNO = 99
	}
}
