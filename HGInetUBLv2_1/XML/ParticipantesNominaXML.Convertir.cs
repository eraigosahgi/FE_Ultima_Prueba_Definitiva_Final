using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class ParticipantesNominaXML
	{
		/// <summary>
		/// Convierte la empresa de XML Nomina a objeto
		/// </summary>
		/// <param name="empresa"></param>
		/// <param name="tipo_doc">0 - Nomina, 1 - Ajuste Reemplazo, 2 - Ajuste Elminacion </param>
		/// <returns></returns>
		public static Empleador Obtener_Empleador(object empresa, int tipo_doc)
		{

			try
			{
				Empleador documento_obj = new Empleador();

				// representación de datos en objeto
				var obj = (dynamic)null;

				if (tipo_doc == 0)
				{
					obj = new NominaIndividualTypeEmpleador();
					obj = empresa;
				}
				else if (tipo_doc == 1)
				{
					obj = new NominaIndividualDeAjusteTypeReemplazarEmpleador();
					obj = empresa;
				}
				else
				{
					obj = new NominaIndividualDeAjusteTypeEliminarEmpleador();
					obj = empresa;
				}


				documento_obj.DV = Convert.ToInt16(obj.DV);
				documento_obj.Identificacion = obj.NIT;
				documento_obj.RazonSocial = obj.RazonSocial;

				documento_obj.TipoDocumento = 31;

				if (!string.IsNullOrEmpty(obj.PrimerApellido) && !string.IsNullOrEmpty(obj.SegundoApellido) && !string.IsNullOrEmpty(obj.PrimerNombre))
				{
					documento_obj.PrimerApellido = obj.PrimerApellido;
					documento_obj.SegundoApellido = obj.SegundoApellido;
					documento_obj.PrimerNombre = obj.PrimerNombre;
					documento_obj.OtrosNombres = obj.OtrosNombres;

					documento_obj.TipoDocumento = 13;
				}

				documento_obj.Direccion = obj.Direccion;
				documento_obj.MunicipioCiudad = obj.MunicipioCiudad;
				documento_obj.DepartamentoEstado = obj.DepartamentoEstado;
				documento_obj.Pais = obj.Pais;

				return documento_obj;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Empleador";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(string.Format("{0} - {1}", msg_custom, excepcion.Message), excepcion.InnerException);
			}

		}

		public static Trabajador Obtener_Trabajador(object trabajador, int tipo_doc)
		{

			Trabajador documento_obj = new Trabajador();

			try
			{
				// representación de datos en objeto
				var obj = (dynamic)null;

				if (tipo_doc == 0)
				{
					obj = new NominaIndividualTypeTrabajador();
					obj = trabajador;
				}
				else if (tipo_doc == 1)
				{
					obj = new NominaIndividualDeAjusteTypeReemplazarTrabajador();
					obj = trabajador;
				}

				documento_obj.TipoTrabajador = obj.TipoTrabajador;
				documento_obj.SubTipoTrabajador = obj.SubTipoTrabajador;
				documento_obj.AltoRiesgoPension = obj.AltoRiesgoPension;

				documento_obj.TipoDocumento = Convert.ToInt32(obj.TipoDocumento);
				documento_obj.Identificacion = obj.NumeroDocumento;
				documento_obj.PrimerApellido = obj.PrimerApellido;
				documento_obj.SegundoApellido = obj.SegundoApellido;
				documento_obj.PrimerNombre = obj.PrimerNombre;
				documento_obj.OtrosNombres = obj.OtrosNombres;
				documento_obj.LugarTrabajoPais = obj.LugarTrabajoPais;
				documento_obj.LugarTrabajoDepartamentoEstado = obj.LugarTrabajoDepartamentoEstado;
				documento_obj.LugarTrabajoMunicipioCiudad = obj.LugarTrabajoMunicipioCiudad;
				documento_obj.LugarTrabajoDireccion = obj.LugarTrabajoDireccion;
				documento_obj.SalarioIntegral = obj.SalarioIntegral;

				documento_obj.TipoContrato = Convert.ToInt32(obj.TipoContrato);
				documento_obj.Sueldo = obj.Sueldo;
				documento_obj.CodigoTrabajador = obj.CodigoTrabajador;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Trabajador";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(string.Format("{0} - {1}", msg_custom, excepcion.Message), excepcion.InnerException);
			}

			return documento_obj;

		}

	}
}
