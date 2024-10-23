using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBLv2_1.DianListas;
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
		/// Convierte el objeto empleador en un objeto tipo nomina
		/// </summary>
		/// <param name="empresa"></param>
		/// <returns></returns>
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
					empleador.OtrosNombres = (string.IsNullOrWhiteSpace(empresa.OtrosNombres)) ? " " : empresa.OtrosNombres;
				}
				
				empleador.Direccion = empresa.Direccion;
				empleador.MunicipioCiudad = empresa.MunicipioCiudad;
				empleador.DepartamentoEstado = empresa.DepartamentoEstado;
				empleador.Pais = empresa.Pais;

				//Se agrega por que no se esta haciendo validaciones y ahi es donde se llena, se requiere para la representacion grafica
				try
				{
					ListaPaises list_paises = new ListaPaises();
					ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(empleador.Pais)).FirstOrDefault();
					empresa.PaisNombre = pais.Descripcion;

					ListaMunicipio list_municipio = new ListaMunicipio();
					ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(empleador.MunicipioCiudad)).FirstOrDefault();
					empresa.CiudadNombre = municipio.Nombre;

					ListaDepartamentos list_depart = new ListaDepartamentos();
					ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(empleador.DepartamentoEstado)).FirstOrDefault();
					empresa.DepartamentoNombre = departamento.Nombre;
				}
				catch (Exception)
				{
				}

				return empleador;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Empleador";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Convierte el objeto Trabajador en un objeto tipo nomina
		/// </summary>
		/// <param name="trabajador"></param>
		/// <returns></returns>
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
				empleado.OtrosNombres = (string.IsNullOrWhiteSpace(trabajador.OtrosNombres)) ? " " : trabajador.OtrosNombres;
				empleado.LugarTrabajoPais = trabajador.LugarTrabajoPais;
				empleado.LugarTrabajoDepartamentoEstado = trabajador.LugarTrabajoDepartamentoEstado;
				empleado.LugarTrabajoMunicipioCiudad = trabajador.LugarTrabajoMunicipioCiudad;
				empleado.LugarTrabajoDireccion = trabajador.LugarTrabajoDireccion;
				empleado.SalarioIntegral = trabajador.SalarioIntegral;

				empleado.TipoContrato = trabajador.TipoContrato.ToString();
				empleado.Sueldo = trabajador.Sueldo;
				empleado.CodigoTrabajador = trabajador.CodigoTrabajador;

				//Se agrega por que no se esta haciendo validaciones y ahi es donde se llena, se requiere para la representacion grafica
				try
				{
					ListaPaises list_paises = new ListaPaises();
					ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(trabajador.LugarTrabajoPais)).FirstOrDefault();
					trabajador.PaisNombre = pais.Descripcion;

					ListaMunicipio list_municipio = new ListaMunicipio();
					ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(trabajador.LugarTrabajoMunicipioCiudad)).FirstOrDefault();
					trabajador.CiudadNombre = municipio.Nombre;

					ListaDepartamentos list_depart = new ListaDepartamentos();
					ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(trabajador.LugarTrabajoDepartamentoEstado)).FirstOrDefault();
					trabajador.DepartamentoNombre = departamento.Nombre;
				}
				catch (Exception)
				{
				}


				return empleado;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Trabajador";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Convierte el objeto empleador en un objeto tipo ajuste nomina que reemplaza
		/// </summary>
		/// <param name="empresa"></param>
		/// <returns></returns>
		public static NominaIndividualDeAjusteTypeReemplazarEmpleador ObtenerEmpleadorAjusteR(Empleador empresa)
		{
			try
			{
				if (empresa == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				NominaIndividualDeAjusteTypeReemplazarEmpleador empleador = new NominaIndividualDeAjusteTypeReemplazarEmpleador();

				empleador.NIT = empresa.Identificacion;
				empleador.DV = empresa.DV.ToString();
				empleador.RazonSocial = empresa.RazonSocial;

				if (!empresa.TipoDocumento.Equals(31))
				{
					empleador.PrimerApellido = empresa.PrimerApellido;
					empleador.SegundoApellido = empresa.SegundoApellido;
					empleador.PrimerNombre = empresa.PrimerNombre;
					empleador.OtrosNombres = (string.IsNullOrWhiteSpace(empresa.OtrosNombres)) ? " " : empresa.OtrosNombres;
				}

				empleador.Direccion = empresa.Direccion;
				empleador.MunicipioCiudad = empresa.MunicipioCiudad;
				empleador.DepartamentoEstado = empresa.DepartamentoEstado;
				empleador.Pais = empresa.Pais;

				//Se agrega por que no se esta haciendo validaciones y ahi es donde se llena, se requiere para la representacion grafica
				try
				{
					ListaPaises list_paises = new ListaPaises();
					ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(empleador.Pais)).FirstOrDefault();
					empresa.PaisNombre = pais.Descripcion;

					ListaMunicipio list_municipio = new ListaMunicipio();
					ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(empleador.MunicipioCiudad)).FirstOrDefault();
					empresa.CiudadNombre = municipio.Nombre;

					ListaDepartamentos list_depart = new ListaDepartamentos();
					ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(empleador.DepartamentoEstado)).FirstOrDefault();
					empresa.DepartamentoNombre = departamento.Nombre;
				}
				catch (Exception)
				{
				}

				return empleador;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Empleador en Reemplazo";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Convierte el objeto trabajador en un objeto tipo ajuste de nomina que reemplaza
		/// </summary>
		/// <param name="trabajador"></param>
		/// <returns></returns>
		public static NominaIndividualDeAjusteTypeReemplazarTrabajador ObtenerTrabajadorAjusteR(Trabajador trabajador)
		{
			try
			{
				if (trabajador == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				NominaIndividualDeAjusteTypeReemplazarTrabajador empleado = new NominaIndividualDeAjusteTypeReemplazarTrabajador();

				empleado.TipoTrabajador = trabajador.TipoTrabajador;
				empleado.SubTipoTrabajador = trabajador.SubTipoTrabajador;
				empleado.AltoRiesgoPension = trabajador.AltoRiesgoPension;

				empleado.TipoDocumento = trabajador.TipoDocumento.ToString();
				empleado.NumeroDocumento = trabajador.Identificacion;
				empleado.PrimerApellido = trabajador.PrimerApellido;
				empleado.SegundoApellido = trabajador.SegundoApellido;
				empleado.PrimerNombre = trabajador.PrimerNombre;
				empleado.OtrosNombres = (string.IsNullOrWhiteSpace(trabajador.OtrosNombres)) ? " " : trabajador.OtrosNombres;
				empleado.LugarTrabajoPais = trabajador.LugarTrabajoPais;
				empleado.LugarTrabajoDepartamentoEstado = trabajador.LugarTrabajoDepartamentoEstado;
				empleado.LugarTrabajoMunicipioCiudad = trabajador.LugarTrabajoMunicipioCiudad;
				empleado.LugarTrabajoDireccion = trabajador.LugarTrabajoDireccion;
				empleado.SalarioIntegral = trabajador.SalarioIntegral;

				empleado.TipoContrato = trabajador.TipoContrato.ToString();
				empleado.Sueldo = trabajador.Sueldo;
				empleado.CodigoTrabajador = trabajador.CodigoTrabajador;

				//Se agrega por que no se esta haciendo validaciones y ahi es donde se llena, se requiere para la representacion grafica
				try
				{
					ListaPaises list_paises = new ListaPaises();
					ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(trabajador.LugarTrabajoPais)).FirstOrDefault();
					trabajador.PaisNombre = pais.Descripcion;

					ListaMunicipio list_municipio = new ListaMunicipio();
					ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(trabajador.LugarTrabajoMunicipioCiudad)).FirstOrDefault();
					trabajador.CiudadNombre = municipio.Nombre;

					ListaDepartamentos list_depart = new ListaDepartamentos();
					ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(trabajador.LugarTrabajoDepartamentoEstado)).FirstOrDefault();
					trabajador.DepartamentoNombre = departamento.Nombre;
				}
				catch (Exception)
				{
				}



				return empleado;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Trabajador en Reemplazo";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Convierte el objeto empleador en un objeto tipo ajuste de nomina que elimina
		/// </summary>
		/// <param name="empresa"></param>
		/// <returns></returns>
		public static NominaIndividualDeAjusteTypeEliminarEmpleador ObtenerEmpleadorAjusteE(Empleador empresa)
		{
			try
			{
				if (empresa == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				NominaIndividualDeAjusteTypeEliminarEmpleador empleador = new NominaIndividualDeAjusteTypeEliminarEmpleador();

				empleador.NIT = empresa.Identificacion;
				empleador.DV = empresa.DV.ToString();
				empleador.RazonSocial = empresa.RazonSocial;

				if (!empresa.TipoDocumento.Equals(31))
				{
					empleador.PrimerApellido = empresa.PrimerApellido;
					empleador.SegundoApellido = empresa.SegundoApellido;
					empleador.PrimerNombre = empresa.PrimerNombre;
					empleador.OtrosNombres = (string.IsNullOrWhiteSpace(empresa.OtrosNombres)) ? " " : empresa.OtrosNombres;
				}

				empleador.Direccion = empresa.Direccion;
				empleador.MunicipioCiudad = empresa.MunicipioCiudad;
				empleador.DepartamentoEstado = empresa.DepartamentoEstado;
				empleador.Pais = empresa.Pais;

				//Se agrega por que no se esta haciendo validaciones y ahi es donde se llena, se requiere para la representacion grafica
				try
				{
					ListaPaises list_paises = new ListaPaises();
					ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(empleador.Pais)).FirstOrDefault();
					empresa.PaisNombre = pais.Descripcion;

					ListaMunicipio list_municipio = new ListaMunicipio();
					ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(empleador.MunicipioCiudad)).FirstOrDefault();
					empresa.CiudadNombre = municipio.Nombre;

					ListaDepartamentos list_depart = new ListaDepartamentos();
					ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(empleador.DepartamentoEstado)).FirstOrDefault();
					empresa.DepartamentoNombre = departamento.Nombre;
				}
				catch (Exception)
				{
				}

				return empleador;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Empleador en Eliminacion";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
