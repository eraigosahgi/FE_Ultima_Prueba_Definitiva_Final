using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.General;
using HGInetMiFacturaElectonicaData.ModeloServicio.General;
using System.Net;
using System.Net.Sockets;
using HGInetMiFacturaElectonicaData.Objetos;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_UsuarioPagos : BaseObject<TblUsuariosPagos>
	{

		#region Constructores 

		public Ctl_UsuarioPagos() : base(new ModeloAutenticacion()) { }
		public Ctl_UsuarioPagos(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_UsuarioPagos(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		#region Guardar
		public TblUsuariosPagos Crear(TblUsuariosPagos usuario)
		{
			usuario = this.Add(usuario);

			return usuario;
		}

		/// <summary>
		/// Crea el usuario principal para la empresa
		/// </summary>
		/// <param name="empresa">información de la empresa</param>
		/// <returns>información del usuario</returns>
		//public TblUsuariosPagos Crear(TblEmpresas empresa)
		//{
		//	TblUsuariosPagos tbl_usuario = new TblUsuariosPagos();

		//	tbl_usuario.StrEmpresaFacturador = empresa.StrIdentificacion;
		//	tbl_usuario.StrUsuario = empresa.StrIdentificacion;
		//	tbl_usuario.StrClave = Encriptar.Encriptar_SHA512(empresa.StrIdentificacion);
		//	tbl_usuario.StrNombres = empresa.StrRazonSocial;
		//	tbl_usuario.StrApellidos = "";
		//	tbl_usuario.StrMail = empresa.StrMailAdmin;
		//	tbl_usuario.DatFechaRegistro = Fecha.GetFecha();
		//	tbl_usuario.DatFechaActualizacion = Fecha.GetFecha();
		//	tbl_usuario.DatFechaCambioClave = Fecha.GetFecha();
		//	tbl_usuario.IntIdEstado = 1;
		//	tbl_usuario.StrIdSeguridad = Guid.NewGuid();
		//	tbl_usuario.StrIdCambioClave = Guid.NewGuid();

		//	// agrega el usuario en la base de datos
		//	tbl_usuario = Crear(tbl_usuario);

		//	AsignarPermisos(empresa);

		//	return tbl_usuario;
		//}


		/// <summary>
		/// Crea el usuario principal para la empresa
		/// </summary>
		/// <param name="empresa">información de la empresa</param>
		/// <returns>información del usuario</returns>
		public bool Crear(TblUsuariosPagos usuario, TblEmpresas empresa = null)
		{

			try
			{

				//Valido si es el usuario existe para la misma empresa
				List<TblUsuariosPagos> ConsultaUsuario = ObtenerUsuarios(usuario.StrUsuario, usuario.StrEmpresaFacturador);
				if (ConsultaUsuario.Count > 0)
					throw new ApplicationException("El Usuario :  " + usuario.StrUsuario + " ya existe");

				//Aqui se deben validar los campos del objeto
				TblUsuariosPagos tbl_usuario = new TblUsuariosPagos();
				usuario.StrClave = System.Guid.NewGuid().ToString();
				DateTime FechaExp = Fecha.GetFecha();
				usuario.DatFechaRegistro = Fecha.GetFecha();
				usuario.DatFechaActualizacion = Fecha.GetFecha();
				usuario.DatFechaCambioClave = FechaExp.AddDays(30);
				usuario.IntIdEstado = usuario.IntIdEstado;
				usuario.StrIdSeguridad = Guid.NewGuid();
				usuario.StrIdCambioClave = Guid.NewGuid();

				// agrega el usuario en la base de datos
				tbl_usuario = Crear(usuario);


				//Aqui debo enviar el correo a usuarioRestablecer.StrMail
				Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();

				empresa = (from a in context.TblEmpresas
						   where a.StrIdentificacion.Equals(usuario.StrEmpresaFacturador)
						   select a).FirstOrDefault();

				Email.BienvenidaPagos(empresa, usuario); //debo crear un objeto de tipo TblUsuario para ver si es posible reutilizar la bienvenida


				return true;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Actualizar
		/// <summary>
		/// cambia la clave por la nueva clave en sha512, la logica del sha512 esta desde donde se valida si es valida o no la clave
		/// </summary>
		/// <param name="idseguridad">id de seguridad del usuario</param>
		/// <param name="clave">clave en sha512</param>
		/// <returns></returns>
		public TblUsuariosPagos ActualizarClave(Guid idseguridad, string clave)
		{
			TblUsuariosPagos usuario = (from item in context.TblUsuariosPagos
										where item.StrIdSeguridad == idseguridad
										select item).FirstOrDefault();

			usuario.StrClave = clave;

			TblUsuariosPagos usuarioclave = Actualizar_usuario(usuario);

			return usuario;
		}





		/// <summary>
		/// Genera un nuevo token en la tabla de usuario y actualiza la fecha de ultimo ingreso
		/// </summary>
		/// <param name="idseguridad">id de seguridad del usuario</param>		
		/// <returns></returns>
		public TblUsuariosPagos ActualizarToken(Guid idseguridad)
		{
			TblUsuariosPagos usuario = (from item in context.TblUsuariosPagos
										where item.StrIdSeguridad == idseguridad
										select item).FirstOrDefault();

			usuario.StrToken = Guid.NewGuid();
			usuario.DatFechaUltimoIngreso = Fecha.GetFecha();

			TblUsuariosPagos usuarioclave = Actualizar_usuario(usuario);

			return usuario;
		}



		/// <summary>
		/// Valida el token en la tabla de usuario
		/// </summary>
		/// <param name="idseguridad">id de seguridad del usuario</param>		
		/// <returns></returns>
		public bool ValidarToken(string Empresa, Guid Idseguridad, Guid Token)
		{
			try
			{
				context.Configuration.LazyLoadingEnabled = false;

				TblUsuariosPagos usuario = (from item in context.TblUsuariosPagos
											where item.StrIdSeguridad == Idseguridad
											&& item.StrEmpresaFacturador.Equals(Empresa)
											&& item.StrToken == Token
											select item).FirstOrDefault();

				if (usuario != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}

		}

		/// <summary>
		/// Actualiza el Email de un usuario en especifico
		/// </summary>
		/// <param name="Usuario">Codigo de usuario</param>
		/// <param name="Email">Email que se desea actualizar</param>
		/// <returns>Retirna una variable booleana indicando si actualizo el email</returns>
		public bool ActualizarEmail(string Usuario, string Email)
		{
			try
			{
				TblUsuariosPagos usuario = (from item in context.TblUsuariosPagos
											where item.StrUsuario.Equals(Usuario)
											select item).FirstOrDefault();

				if (usuario != null)
				{
					usuario.StrMail = Email;
					Actualizar_usuario(usuario);
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}

		}

		/// <summary>
		/// Actualiza el usuario en la base ded atos
		/// </summary>
		/// <param name="usuario">información del Usuario</param>
		/// <returns>información del usuario</returns>

		public TblUsuariosPagos Actualizar_usuario(TblUsuariosPagos usuario)
		{
			usuario = this.Edit(usuario);

			return usuario;

		}

		/// <summary>
		/// Recibe parte del usuario y luego se envia a actualizar usuario para guardarlo en db
		/// </summary>
		/// <param name="usuario">información del usuario</param>
		/// <returns>información del usuario</returns>
		public bool Actualizar(TblUsuariosPagos usuario)
		{
			try
			{

				TblUsuariosPagos UsuarioActiliza = (from item in context.TblUsuariosPagos
													where item.StrUsuario.Equals(usuario.StrUsuario)
													&& item.StrEmpresaFacturador.Equals(usuario.StrEmpresaFacturador)
													select item).FirstOrDefault();



				if (UsuarioActiliza != null)
				{
					//UsuarioActiliza.StrEmpresaFacturador = usuario.StrEmpresaFacturador;
					//UsuarioActiliza.StrEmpresaAdquiriente = usuario.StrEmpresaAdquiriente;
					UsuarioActiliza.StrNombres = usuario.StrNombres;
					UsuarioActiliza.StrApellidos = usuario.StrApellidos;
					UsuarioActiliza.StrMail = usuario.StrMail;
					UsuarioActiliza.StrTelefono = usuario.StrTelefono;
					UsuarioActiliza.IntIdEstado = usuario.IntIdEstado;
					UsuarioActiliza.DatFechaActualizacion = Fecha.GetFecha();
					UsuarioActiliza.DatFechaCambioClave = Fecha.GetFecha();

					// agrega el usuario en la base de datos
					usuario = Actualizar_usuario(UsuarioActiliza);

					return true;
				}
				else
				{
					throw new ApplicationException("Datos Invalidos, el Usuario no coincide con la empresa");
				}
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
		#endregion

		#region Validar




		/// <summary>
		/// Inicio de sesión
		/// </summary>
		/// <param name="codigo_empresa">Numero de identificación de la empresa</param>
		/// <param name="codigo_usuario">Codigo del usuario</param>
		/// <param name="clave">Clave del usuario</param>
		/// <returns></returns>
		public List<TblUsuariosPagos> ValidarExistencia(Guid IdSeguridad, string codigo_usuario, string clave)
		{
			try
			{

				Ctl_Empresa _controlador_empresa = new Ctl_Empresa();
				TblEmpresas emp = new TblEmpresas();

				emp = _controlador_empresa.Obtener(IdSeguridad, false).FirstOrDefault();

				if (emp == null)
				{
					throw new ApplicationException("Serial invalido o empresa no existe");
				}

				//Cifro la clave en SHA512 para validar si tiene MD5 O SHA512
				string claveSha512 = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA512(clave);
				clave = LibreriaGlobalHGInet.General.Encriptar.Encriptar_MD5(clave);

				var respuesta = (from usuario in context.TblUsuariosPagos
								 where usuario.StrEmpresaFacturador.Equals(emp.StrIdentificacion)
								 && usuario.StrUsuario.Equals(codigo_usuario)
								 && (usuario.StrClave.Equals(clave) || usuario.StrClave.Equals(claveSha512))
								 select usuario).ToList();

				if (respuesta.Count > 0)
				{
					///Valida si el usuario esta activo
					if (respuesta.FirstOrDefault().IntIdEstado != 1)
						throw new ApplicationException("Usuario Inactivo");

					// Actualiza la clave de MD5 a SHA512
					if (respuesta.FirstOrDefault().StrClave.Equals(clave))
					{
						ActualizarClave(respuesta.FirstOrDefault().StrIdSeguridad, claveSha512);
					}
					//Actualiza el token del usuario
					ActualizarToken(respuesta.FirstOrDefault().StrIdSeguridad);

				}
				else
					throw new ApplicationException("Datos de autenticación inválidos.");


				return respuesta.ToList();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		#endregion

		#region Obtener

		/// <summary>
		/// Obtiene los datos por código de usuario y empresa.
		/// </summary>
		/// <param name="codigo_usuario"></param>
		/// <returns></returns>
		public List<TblUsuariosPagos> ObtenerUsuarios(string codigo_usuario, string codigo_empresa)
		{
			try
			{
				var respuesta = (from usuario in context.TblUsuariosPagos
								 where (usuario.StrUsuario.Equals(codigo_usuario))
								 && (usuario.StrEmpresaFacturador.Equals(codigo_empresa))
								 select usuario).ToList();

				return respuesta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene la lista de usuarios
		/// </summary>
		/// <param name="codigo_usuario">Codigo de usuario</param>
		/// <param name="codigo_empresa">Codigo de empresa</param>
		/// <param name="nombre_usuario">nombre de usuario</param>
		/// <param name="Desde">desde</param>
		/// <param name="Hasta">hasta</param>
		/// <returns>lista de ObjUsuario</returns>
		public List<ObjUsuario> ConsultaUsuarios(string codigo_usuario, string codigo_empresa, string nombre_usuario)
		{
			try
			{

				if (string.IsNullOrEmpty(codigo_usuario))
				{
					codigo_usuario = "*";
				}

				if (string.IsNullOrEmpty(nombre_usuario))
				{
					nombre_usuario = "*";
				}

				var respuesta = (from d in context.TblUsuariosPagos
								 where (d.StrUsuario.Equals(codigo_usuario) || codigo_usuario.Equals("*"))
								 && (d.StrEmpresaFacturador.Equals(codigo_empresa))
								 && (d.StrNombres.Contains(nombre_usuario) || nombre_usuario.Equals("*"))
								 select new ObjUsuario
								 {
									 Empresa = d.StrEmpresaAdquiriente,
									 FechaActualizacion = d.DatFechaActualizacion,
									 FechaCambioClave = d.DatFechaCambioClave.ToString(),
									 FechaIngreso = d.DatFechaRegistro,
									 Estado = d.IntIdEstado,
									 Apellidos = d.StrApellidos,
									 Clave = d.StrClave,
									 IdCambioClave = d.StrIdCambioClave.ToString(),
									 IdSeguridad = d.StrIdSeguridad,
									 Mail = d.StrMail,
									 Nombres = d.StrNombres,
									 NombreCompleto = d.StrNombres + " " + d.StrApellidos,
									 Telefono = d.StrTelefono,
									 Usuario = d.StrUsuario,
								 }).OrderBy(x => x.Nombres).ToList();


				return respuesta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene la lista de usuarios
		/// </summary>
		/// <param name="codigo_usuario">Codigo de usuario</param>
		/// <param name="codigo_empresa">Codigo de empresa</param>
		/// <param name="nombre_usuario">nombre de usuario</param>
		/// <param name="Desde">desde</param>
		/// <param name="Hasta">hasta</param>
		/// <returns>lista de ObjUsuario</returns>
		public ObjUsuario ConsultaPorIdSeguridad(string codigo_empresa, Guid id_seguridad_usuario)
		{
			try
			{

				var respuesta = (from d in context.TblUsuariosPagos
								 where (d.StrEmpresaFacturador.Equals(codigo_empresa))
								 && (d.StrIdSeguridad == id_seguridad_usuario)
								 select new ObjUsuario
								 {
									 Empresa = d.StrEmpresaAdquiriente,
									 FechaActualizacion = d.DatFechaActualizacion,
									 FechaCambioClave = d.DatFechaCambioClave.ToString(),
									 FechaIngreso = d.DatFechaRegistro,
									 Estado = d.IntIdEstado,
									 Apellidos = d.StrApellidos,
									 Clave = d.StrClave,
									 IdCambioClave = d.StrIdCambioClave.ToString(),
									 IdSeguridad = d.StrIdSeguridad,
									 Mail = d.StrMail,
									 Nombres = d.StrNombres,
									 NombreCompleto = d.StrNombres + " " + d.StrApellidos,
									 Telefono = d.StrTelefono,
									 Usuario = d.StrUsuario,
								 }).OrderBy(x => x.Nombres).FirstOrDefault();


				return respuesta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene la lista de usuarios de la empresa y de la lista de empresas asociadas
		/// </summary>
		/// <param name="codigo_usuario"></param>
		/// <returns></returns>
		public List<ObjUsuario> ObtenerListaUsuarios(string codigo_usuario, string codigo_empresa, string nombre_usuario, int Desde, int Hasta)
		{
			try
			{

				if (string.IsNullOrEmpty(codigo_usuario))
				{
					codigo_usuario = "*";
				}

				if (string.IsNullOrEmpty(nombre_usuario))
				{
					nombre_usuario = "*";
				}

				List<ObjUsuario> respuesta = (from d in context.TblUsuariosPagos
											  where (d.StrUsuario.Equals(codigo_usuario) || codigo_usuario.Equals("*"))
											  && (d.StrNombres.Contains(nombre_usuario) || nombre_usuario.Equals("*"))
											  select new ObjUsuario
											  {
												  FechaActualizacion = d.DatFechaActualizacion,
												  FechaCambioClave = d.DatFechaCambioClave.ToString(),
												  FechaIngreso = d.DatFechaRegistro,
												  Estado = d.IntIdEstado,
												  Apellidos = d.StrApellidos,
												  Clave = d.StrClave,
												  IdCambioClave = d.StrIdCambioClave.ToString(),
												  IdSeguridad = d.StrIdSeguridad,
												  Mail = d.StrMail,
												  Nombres = d.StrNombres,
												  NombreCompleto = d.StrNombres + d.StrApellidos,
												  Telefono = d.StrTelefono,
												  Usuario = d.StrUsuario,

											  }).OrderBy(x => x.RazonSocial).Skip(Desde).Take(Hasta).ToList();


				return respuesta;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene el usuario por id seguridad
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		public TblUsuariosPagos ObtenerIdSeguridad(System.Guid id_seguridad)
		{
			try
			{
				context.Configuration.LazyLoadingEnabled = false;

				var respuesta = from usuario in context.TblUsuariosPagos.AsNoTracking()
								where (usuario.StrIdSeguridad.Equals(id_seguridad))
								select usuario;

				return respuesta.FirstOrDefault();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Validar documento y usuario para restablecer
		public string ValidarExistenciaRestablecer(Guid IdSeguridadFacturador, string codigo_usuario)
		{
			try
			{

				Ctl_Empresa _controlador_empresa = new Ctl_Empresa();
				TblEmpresas emp = new TblEmpresas();

				emp = _controlador_empresa.Obtener(IdSeguridadFacturador, false).FirstOrDefault();

				if (emp == null)
				{
					throw new ApplicationException("Serial invalido o empresa no existe");
				}


				var respuesta = from usuario in context.TblUsuariosPagos
								where usuario.StrEmpresaFacturador.Equals(emp.StrIdentificacion)
								&& usuario.StrUsuario.Equals(codigo_usuario)
								select usuario;


				if (!respuesta.Any())
					throw new ApplicationException("Datos Incorrectos.");

				TblUsuariosPagos usuarioRestablecer = respuesta.FirstOrDefault();

				usuarioRestablecer.DatFechaCambioClave = Fecha.GetFecha();
				usuarioRestablecer.StrIdCambioClave = Guid.NewGuid();
				//usuarioRestablecer.StrClave = Encriptar.Encriptar_SHA512(usuarioRestablecer.StrClave);
				Actualizar_usuario(usuarioRestablecer);


				//Aqui debo enviar el correo a usuarioRestablecer.StrMail
				Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();

				Email.RestablecerClavePagos(emp, usuarioRestablecer);

				return usuarioRestablecer.StrMail;
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
		#endregion

		#region Cambio de Clave
		public string RestablecerClave(System.Guid IdSeguridad, string clave)
		{
			try
			{
				TblUsuariosPagos datos = (from item in context.TblUsuariosPagos
										  where item.StrIdCambioClave == IdSeguridad
										  select item).FirstOrDefault();
				if (datos != null)
				{

					if (!(datos.DatFechaCambioClave.Value.AddHours(24.0) >= Fecha.GetFecha()))
					{
						throw new ApplicationException("El ID de seguridad ha expirado; por favor realice el proceso de recuperación de contraseña nuevamente.");
					}


					clave = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA512(clave);

					datos.StrClave = clave;
					datos.DatFechaCambioClave = Fecha.GetFecha();
					datos.StrIdCambioClave = Guid.NewGuid();
					Actualizar_usuario(datos);
					return datos.StrEmpresaFacturador;
				}
				else
				{
					throw new ApplicationException("Id de seguridad incorrecto.");
				}
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException("Id de seguridad incorrecto.", excepcion.InnerException);
			}
		}

		/// <summary>
		/// Valida el id de seguridad para cambio de contraseña
		/// </summary>
		/// <param name="IdSeguridad">id de seguridad</param>
		/// <returns>indica si es válido el cambio de clave</returns>
		public string ValidarIdSeguridad(System.Guid IdSeguridad)
		{
			try
			{
				TblUsuariosPagos datos = (from item in context.TblUsuariosPagos
										  where item.StrIdCambioClave == IdSeguridad
										  select item).FirstOrDefault();



				if (datos != null)
				{
					if (!datos.DatFechaCambioClave.HasValue || !(datos.DatFechaCambioClave.Value.AddHours(24.0) >= Fecha.GetFecha()))
					{
						throw new ApplicationException("El ID de seguridad ha expirado; por favor realice el proceso de recuperación de contraseña nuevamente.");
					}
					return datos.StrEmpresaFacturador;
				}

				throw new ApplicationException("Id de seguridad incorrecto.");
			}
			catch (ApplicationException excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		#endregion

		#region Permisos Usuarios
		/// <summary>
		/// Renorta los permisos de consulta del usuario(Menu)
		/// </summary>
		/// <param name="codigo_usuario"></param>Usuario Logeado
		/// /// <param name="codigo_empresa"></param>Empresa Logeada
		/// <returns></returns>
		public List<TblOpciones> ObtenerPermisos(string codigo_usuario, string codigo_empresa)
		{
			try
			{
				context.Configuration.LazyLoadingEnabled = false;
				var respuesta = (from opciones in context.TblOpciones.AsNoTracking()
								 join opcionesUsuario in context.TblOpcionesUsuario on opciones.IntId equals opcionesUsuario.IntIdOpcion
								 where (opcionesUsuario.StrUsuario.Equals(codigo_usuario) && opcionesUsuario.StrEmpresa.Equals(codigo_empresa))
								 && opciones.IntHabilitado == true
								 && opciones.IntTipo == 0
								 && opcionesUsuario.IntConsultar == true

								 select opciones);


				return respuesta.ToList();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}





		#endregion

	}
}
