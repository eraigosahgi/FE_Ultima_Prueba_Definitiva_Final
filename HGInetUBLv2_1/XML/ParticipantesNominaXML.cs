using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public class ParticipantesNominaXML
	{

		public static NominaIndividualTypeEmpleador ObtenerEmpleador(Empleador empresa)
		{
			try
			{
				if (empresa == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				NominaIndividualTypeEmpleador empleador = new NominaIndividualTypeEmpleador();

				empleador.NIT = empresa.Identificacion;
				empleador.DV = empresa.DV.ToString();
				empleador.RazonSocial = empresa.RazonSocial;

				if (!empresa.TipoDocumento.Equals(31))
				{
					empleador.PrimerApellido = empresa.PrimerApellido;
					empleador.SegundoApellido = empresa.SegundoApellido;
					empleador.PrimerNombre = empresa.PrimerNombre;
					empleador.OtrosNombres = empresa.OtrosNombres;
				}
				
				empleador.Direccion = empresa.Direccion;
				empleador.MunicipioCiudad = empresa.MunicipioCiudad;
				empleador.DepartamentoEstado = empresa.DepartamentoEstado;
				empleador.Pais = empresa.Pais;

				return empleador;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public static NominaIndividualTypeTrabajador ObtenerTrabajador(Trabajador trabajador)
		{
			try
			{
				if (trabajador == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				NominaIndividualTypeTrabajador empleado = new NominaIndividualTypeTrabajador();

				empleado.TipoTrabajador = trabajador.TipoTrabajador;
				empleado.SubTipoTrabajador = trabajador.SubTipoTrabajador;
				empleado.AltoRiesgoPension = trabajador.AltoRiesgoPension;

				empleado.TipoDocumento = trabajador.TipoDocumento.ToString();
				empleado.NumeroDocumento = trabajador.Identificacion;
				empleado.PrimerApellido = trabajador.PrimerApellido;
				empleado.SegundoApellido = trabajador.SegundoApellido;
				empleado.PrimerNombre = trabajador.PrimerNombre;
				empleado.OtrosNombres = trabajador.OtrosNombres;
				empleado.LugarTrabajoPais = trabajador.LugarTrabajoPais;
				empleado.LugarTrabajoDepartamentoEstado = trabajador.LugarTrabajoDepartamentoEstado;
				empleado.LugarTrabajoMunicipioCiudad = trabajador.LugarTrabajoMunicipioCiudad;
				empleado.LugarTrabajoDireccion = trabajador.LugarTrabajoDireccion;
				empleado.SalarioIntegral = trabajador.SalarioIntegral;

				empleado.TipoContrato = trabajador.TipoContrato.ToString();
				empleado.Sueldo = trabajador.Sueldo;
				empleado.CodigoTrabajador = trabajador.CodigoTrabajador;
				
			   

				return empleado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
