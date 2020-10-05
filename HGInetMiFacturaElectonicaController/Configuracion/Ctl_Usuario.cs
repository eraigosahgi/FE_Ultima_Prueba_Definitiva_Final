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
	public class Ctl_Usuario : BaseObject<TblUsuarios>
	{

		#region Constructores 

		public Ctl_Usuario() : base(new ModeloAutenticacion()) { }
		public Ctl_Usuario(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_Usuario(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		#region Guardar
		public TblUsuarios Crear(TblUsuarios usuario)
		{
			usuario = this.Add(usuario);

			return usuario;
		}

		/// <summary>
		/// Crea el usuario principal para la empresa
		/// </summary>
		/// <param name="empresa">información de la empresa</param>
		/// <returns>información del usuario</returns>
		public TblUsuarios Crear(TblEmpresas empresa)
		{
			TblUsuarios tbl_usuario = new TblUsuarios();

			tbl_usuario.StrEmpresa = empresa.StrIdentificacion;
			tbl_usuario.StrUsuario = empresa.StrIdentificacion;
			tbl_usuario.StrClave = Encriptar.Encriptar_SHA512(empresa.StrIdentificacion);
			tbl_usuario.StrNombres = empresa.StrRazonSocial;
			tbl_usuario.StrApellidos = "";
			tbl_usuario.StrMail = empresa.StrMailAdmin;
			tbl_usuario.DatFechaIngreso = Fecha.GetFecha();
			tbl_usuario.DatFechaActualizacion = Fecha.GetFecha();
			tbl_usuario.DatFechaCambioClave = Fecha.GetFecha();
			tbl_usuario.IntIdEstado = 1;
			tbl_usuario.StrIdSeguridad = Guid.NewGuid();
			tbl_usuario.StrIdCambioClave = Guid.NewGuid();

			// agrega el usuario en la base de datos
			tbl_usuario = Crear(tbl_usuario);

			AsignarPermisos(empresa);

			return tbl_usuario;
		}

		/// <summary>
		/// Asigna los permisos al usuario según el perfil de la empresa (Obligado ó Adquiriente).
		/// </summary>
		/// <param name="datos_empresa">datos de la empresa</param>
		public void AsignarPermisos(TblEmpresas datos_empresa)
		{
			try
			{
				Ctl_OpcionesPerfil clase_permisos = new Ctl_OpcionesPerfil();
				List<TblOpcionesPerfil> opciones_perfil = new List<TblOpcionesPerfil>();

				//Obtiene permisos del facturador.
				if (datos_empresa.IntObligado)
					opciones_perfil.AddRange(clase_permisos.ObtenerOpcionesPorPerfil((short)Perfiles.Facturador));

				//Obtiene permisos del adquiriente.
				if (datos_empresa.IntAdquiriente)
					opciones_perfil.AddRange(clase_permisos.ObtenerOpcionesPorPerfil((short)Perfiles.Adquiriente));

				//Obtiene permisos del Integrador.
				if (datos_empresa.IntIntegrador)
					opciones_perfil.AddRange(clase_permisos.ObtenerOpcionesPorPerfil((short)Perfiles.Integrador));

				List<TblOpcionesUsuario> opciones_usuario = new List<TblOpcionesUsuario>();

				//Añade las opciones TblOpcionesPerfil en una lista de tipo TblOpcionesUsuario.
				if (opciones_perfil.Count() > 0)
				{
					foreach (var permiso_perfil in opciones_perfil)
					{
						if (permiso_perfil.IntAnular || permiso_perfil.IntEditar || permiso_perfil.IntEliminar || permiso_perfil.IntGestion || permiso_perfil.IntAgregar || permiso_perfil.IntConsultar)
						{
							TblOpcionesUsuario permiso = new TblOpcionesUsuario();

							permiso.IntAgregar = permiso_perfil.IntAgregar;
							permiso.IntAnular = permiso_perfil.IntAnular;
							permiso.IntConsultar = permiso_perfil.IntConsultar;
							permiso.IntEditar = permiso_perfil.IntEditar;
							permiso.IntEliminar = permiso_perfil.IntEliminar;
							permiso.IntGestion = permiso_perfil.IntGestion;
							permiso.IntIdOpcion = permiso_perfil.IntIdOpcion;
							permiso.StrUsuario = datos_empresa.StrIdentificacion;
							permiso.StrEmpresa = datos_empresa.StrIdentificacion;

							opciones_usuario.Add(permiso);
						}
					}
				}

				//Almacena la información de las opciones de permiso del usuario en base de datos.
				if (opciones_usuario.Count > 0)
				{
					Ctl_OpcionesUsuario clase_opciones_usuario = new Ctl_OpcionesUsuario();

					List<TblOpcionesUsuario> opciones_permisos = opciones_usuario.GroupBy(x => x.IntIdOpcion).Select(d => d.First()).ToList();

					clase_opciones_usuario.CrearOpciones(opciones_permisos);
				}
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Crea el usuario principal para la empresa
		/// </summary>
		/// <param name="empresa">información de la empresa</param>
		/// <returns>información del usuario</returns>
		public bool Crear(TblUsuarios usuario, TblEmpresas empresa = null)
		{

			try
			{

				//validar cantidad de usuarios por empresa
				if (usuario.IntIdEstado == 1)
				{
					if (CantUsuariosActivos(usuario.StrEmpresa) >= CantUsuariosEmpresa(usuario.StrEmpresa))
						throw new ApplicationException("Superó el máximo de usuarios activos por empresa, si desea crear uno nuevo, debe inactivar uno existente");

				}

				//Valido si es el usuario existe para la misma empresa
				List<TblUsuarios> ConsultaUsuario = ObtenerUsuarios(usuario.StrUsuario, usuario.StrEmpresa);
				if (ConsultaUsuario.Count > 0)
					throw new ApplicationException("El Usuario :  " + usuario.StrUsuario + " ya existe");

				//Aqui se deben validar los campos del objeto
				TblUsuarios tbl_usuario = new TblUsuarios();
				usuario.StrClave = System.Guid.NewGuid().ToString();
				DateTime FechaExp = Fecha.GetFecha();
				usuario.DatFechaIngreso = Fecha.GetFecha();
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
						   where a.StrIdentificacion.Equals(usuario.StrEmpresa)
						   select a).FirstOrDefault();

				Email.Bienvenida(empresa, usuario);


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
		public TblUsuarios ActualizarClave(Guid idseguridad, string clave)
		{
			TblUsuarios usuario = (from item in context.TblUsuarios
								   where item.StrIdSeguridad == idseguridad
								   select item).FirstOrDefault();

			usuario.StrClave = clave;

			TblUsuarios usuarioclave = Actualizar_usuario(usuario);

			return usuario;
		}





		/// <summary>
		/// Genera un nuevo token en la tabla de usuario y actualiza la fecha de ultimo ingreso
		/// </summary>
		/// <param name="idseguridad">id de seguridad del usuario</param>		
		/// <returns></returns>
		public TblUsuarios ActualizarToken(Guid idseguridad)
		{
			TblUsuarios usuario = (from item in context.TblUsuarios
								   where item.StrIdSeguridad == idseguridad
								   select item).FirstOrDefault();

			usuario.StrToken = Guid.NewGuid();
			usuario.DatFechaUltimoIngreso = Fecha.GetFecha();

			TblUsuarios usuarioclave = Actualizar_usuario(usuario);

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

				TblUsuarios usuario = (from item in context.TblUsuarios
									   where item.StrIdSeguridad == Idseguridad
									   && item.StrEmpresa.Equals(Empresa)
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
				TblUsuarios usuario = (from item in context.TblUsuarios
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

		public TblUsuarios Actualizar_usuario(TblUsuarios usuario)
		{
			usuario = this.Edit(usuario);

			return usuario;

		}


		/// <summary>
		/// Recibe parte del usuario y luego se envia a actualizar usuario para guardarlo en db
		/// </summary>
		/// <param name="usuario">información del usuario</param>
		/// <returns>información del usuario</returns>
		public bool Actualizar(TblUsuarios usuario)
		{
			try
			{

				TblUsuarios UsuarioActiliza = (from item in context.TblUsuarios
											   where item.StrUsuario.Equals(usuario.StrUsuario)
											   && item.StrEmpresa.Equals(usuario.StrEmpresa)
											   select item).FirstOrDefault();

				//Creo variable para validar la cantidad de usuarios activos
				int cantidadUsuariosActivos = CantUsuariosActivos(usuario.StrEmpresa);
				//Creo variables para validar cantidad usuarios activos permitidos para esa empresa
				int CantUsuariosPermitidos = CantUsuariosEmpresa(usuario.StrEmpresa);


				if (cantidadUsuariosActivos == CantUsuariosPermitidos)
					//Si entra aqui pueda actualizar cualquier usuario pero no puede pasar un usuario de Inactivo a Activo
					if (usuario.IntIdEstado == 1 && UsuarioActiliza.IntIdEstado != 1)
					{
						throw new ApplicationException(string.Format("Superó el máximo de usuarios activos({0}) de la empresa", CantUsuariosPermitidos));
					}

				if (cantidadUsuariosActivos > CantUsuariosPermitidos)
					if (usuario.IntIdEstado != 2)
					{
						throw new ApplicationException(string.Format("Superó el máximo de usuarios activos({0}) de la empresa", CantUsuariosPermitidos));
					}



				if (UsuarioActiliza != null)
				{
					UsuarioActiliza.StrEmpresa = usuario.StrEmpresa;
					UsuarioActiliza.StrNombres = usuario.StrNombres;
					UsuarioActiliza.StrApellidos = usuario.StrApellidos;
					UsuarioActiliza.StrMail = usuario.StrMail;
					UsuarioActiliza.StrTelefono = usuario.StrTelefono;
					UsuarioActiliza.StrExtension = usuario.StrExtension;
					UsuarioActiliza.StrCelular = usuario.StrCelular;
					UsuarioActiliza.StrCargo = usuario.StrCargo;
					UsuarioActiliza.IntIdEstado = usuario.IntIdEstado;

					UsuarioActiliza.DatFechaActualizacion = Fecha.GetFecha();
					UsuarioActiliza.DatFechaCambioClave = Fecha.GetFecha();
					//UsuarioActiliza.StrIdSeguridad = Guid.NewGuid();					

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
		/// Cantidad de Usuarios Activos por Empresa, se debe pasar el StrIdEmpresa
		/// </summary>
		/// <param name="IdentificacionEmpresa"></param>
		/// <returns></returns>
		public int CantUsuariosActivos(string IdentificacionEmpresa)
		{
			int C_usuarios = (from u in context.TblUsuarios
							  where u.StrEmpresa.Equals(IdentificacionEmpresa)
							  && u.IntIdEstado.Equals(1)
							  select u).Count();
			return C_usuarios;
		}

		/// <summary>
		/// Retorna la cantidad de usuarios Activos para una empresa especifica
		/// </summary>
		/// <param name="IdentificacionEmpresa"></param>
		/// <returns></returns>
		public int CantUsuariosEmpresa(string IdentificacionEmpresa)
		{
			int C_usuarios = (from empresa in context.TblEmpresas
							  where empresa.StrIdentificacion.Equals(IdentificacionEmpresa)
							  select empresa.IntNumUsuarios).FirstOrDefault();

			return C_usuarios;
		}



		/// <summary>
		/// Inicio de sesión
		/// </summary>
		/// <param name="codigo_empresa">Numero de identificación de la empresa</param>
		/// <param name="codigo_usuario">Codigo del usuario</param>
		/// <param name="clave">Clave del usuario</param>
		/// <returns></returns>
		public List<TblUsuarios> ValidarExistencia(string codigo_empresa, string codigo_usuario, string clave)
		{
			try
			{
				//Cifro la clave en SHA512 para validar si tiene MD5 O SHA512
				string claveSha512 = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA512(clave);
				clave = LibreriaGlobalHGInet.General.Encriptar.Encriptar_MD5(clave);

				var respuesta = (from usuario in context.TblUsuarios
								 join empresa in context.TblEmpresas on usuario.StrEmpresa equals empresa.StrIdentificacion
								 where empresa.StrIdentificacion.Equals(codigo_empresa)
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


				///Si el usuario que ingresa es el generico, entonces validamos los permisos del perfil
				try
				{
					if (codigo_empresa.Equals(codigo_usuario))
					{
						//Envio los datos de la empresa y usuario en la actualización de los permisos
						Ctl_Empresa Controlador_Empresa = new Ctl_Empresa();
						TblEmpresas Empresa = Controlador_Empresa.Obtener(codigo_empresa);
						ValidarPermisosUsuario(Empresa, respuesta.FirstOrDefault());
					}
				}
				catch (Exception)
				{

					throw;
				}


				return respuesta.ToList();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Valida los permisos del usuario y los actualiza.
		/// </summary>
		/// <param name="datos_empresa"></param>
		/// <param name="datos_usuario"></param>
		public void ValidarPermisosUsuario(TblEmpresas datos_empresa, TblUsuarios datos_usuario)
		{
			List<TblOpcionesPerfil> opciones_perfil = new List<TblOpcionesPerfil>();
			List<TblOpcionesUsuario> opciones_usuario_bd = new List<TblOpcionesUsuario>();
			Ctl_OpcionesUsuario clase_opc_usuarios = new Ctl_OpcionesUsuario();
			Ctl_OpcionesPerfil clase_permisos = new Ctl_OpcionesPerfil();
			Ctl_OpcionesUsuario clase_opc_usuario = new Ctl_OpcionesUsuario();
			List<TblOpcionesUsuario> opciones_grabar = new List<TblOpcionesUsuario>();

			//Obtiene las opciones del usuario en base de datos
			opciones_usuario_bd = clase_opc_usuario.ObtenerOpcionesUsuarios(datos_usuario.StrUsuario, datos_usuario.StrEmpresa);



			//Obtiene permisos del Administrador
			if (datos_empresa.IntAdministrador)
				opciones_perfil.AddRange(clase_permisos.ObtenerOpcionesPorPerfil((short)Perfiles.Administrador));

			//Obtiene permisos del Integrador
			if (datos_empresa.IntIntegrador)
				opciones_perfil.AddRange(clase_permisos.ObtenerOpcionesPorPerfil((short)Perfiles.Integrador));

			//Obtiene permisos del facturador.
			if (datos_empresa.IntObligado)
				opciones_perfil.AddRange(clase_permisos.ObtenerOpcionesPorPerfil((short)Perfiles.Facturador));

			//Obtiene permisos del adquiriente.
			if (datos_empresa.IntAdquiriente)
				opciones_perfil.AddRange(clase_permisos.ObtenerOpcionesPorPerfil((short)Perfiles.Adquiriente));

			opciones_perfil = opciones_perfil.GroupBy(x => x.IntIdOpcion).Select(d => d.First()).ToList();

			//Recorre las opciones de base de datos y valida si estan contenidas en las opciones por perfil
			//sino estan contenidas las elimina
			foreach (var item_bd in opciones_usuario_bd)
			{
				TblOpcionesPerfil registro_bd = opciones_perfil.Where(x => x.IntIdOpcion == item_bd.IntIdOpcion).FirstOrDefault();

				if (registro_bd == null)
				{
					clase_opc_usuarios.Eliminar(item_bd);
				}

			}

			//Recorre las opciones por perfil y valida si existen o no en base de datos
			foreach (TblOpcionesPerfil item in opciones_perfil)
			{
				TblOpcionesUsuario registro_nuevo = opciones_usuario_bd.Where(x => x.IntIdOpcion == item.IntIdOpcion).FirstOrDefault();

				if (registro_nuevo == null)
				{
					TblOpcionesUsuario permiso = new TblOpcionesUsuario();

					permiso.IntAgregar = item.IntAgregar;
					permiso.IntAnular = item.IntAnular;
					permiso.IntConsultar = item.IntConsultar;
					permiso.IntEditar = item.IntEditar;
					permiso.IntEliminar = item.IntEliminar;
					permiso.IntGestion = item.IntGestion;
					permiso.IntIdOpcion = item.IntIdOpcion;
					permiso.StrUsuario = datos_empresa.StrIdentificacion;
					permiso.StrEmpresa = datos_empresa.StrIdentificacion;

					opciones_grabar.Add(permiso);
				}
			}

			if (opciones_grabar.Count() > 0)
				clase_opc_usuarios.CrearOpciones(opciones_grabar);
		}



		#endregion

		#region Obtener

		/// <summary>
		/// Obtiene los datos por código de usuario y empresa.
		/// </summary>
		/// <param name="codigo_usuario"></param>
		/// <returns></returns>
		public List<TblUsuarios> ObtenerUsuarios(string codigo_usuario, string codigo_empresa)
		{
			try
			{
				var respuesta = (from usuario in context.TblUsuarios
								 where (usuario.StrUsuario.Equals(codigo_usuario) || codigo_usuario.Equals("*"))
								 && (usuario.StrEmpresa.Equals(codigo_empresa) || codigo_empresa.Equals("*"))
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
		public List<ObjUsuario> ConsultaUsuarios(string codigo_usuario, string codigo_empresa, string nombre_usuario, int Desde, int Hasta)
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

				var respuesta = (from d in context.TblUsuarios
								 where (d.StrUsuario.Equals(codigo_usuario) || codigo_usuario.Equals("*"))
								 && (d.StrEmpresa.Equals(codigo_empresa) || codigo_empresa.Equals("*"))
								 && (d.StrNombres.Contains(nombre_usuario) || nombre_usuario.Equals("*"))
								 select new ObjUsuario
								 {
									 FechaActualizacion = d.DatFechaActualizacion,
									 FechaCambioClave = d.DatFechaCambioClave.ToString(),
									 FechaIngreso = d.DatFechaIngreso,
									 Estado = d.IntIdEstado,
									 Apellidos = d.StrApellidos,
									 Cargo = d.StrCargo,
									 Celular = d.StrCelular,
									 Clave = d.StrClave,
									 Empresa = d.StrEmpresa,
									 Extension = d.StrExtension,
									 IdCambioClave = d.StrIdCambioClave.ToString(),
									 IdSeguridad = d.StrIdSeguridad,
									 Mail = d.StrMail,
									 Nombres = d.StrNombres,
									 NombreCompleto = d.StrNombres + d.StrApellidos,
									 Telefono = d.StrTelefono,
									 Usuario = d.StrUsuario,
									 RazonSocial = d.TblEmpresas.StrRazonSocial
								 }).OrderBy(x => x.RazonSocial).Skip(Desde).Take(Hasta).ToList();


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
		public List<ObjUsuario> ObtenerListaUsuarios(string codigo_usuario, string codigo_empresa,string nombre_usuario, int Desde, int Hasta)
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

				List<ObjUsuario> respuesta = (from d in context.TblUsuarios
											  where (d.StrUsuario.Equals(codigo_usuario) || codigo_usuario.Equals("*"))
											  && (d.StrEmpresa.Equals(codigo_empresa) || d.TblEmpresas.StrEmpresaAsociada.Equals(codigo_empresa))
											  && (d.StrNombres.Contains(nombre_usuario) || nombre_usuario.Equals("*"))
											  select new ObjUsuario
											  {
												  FechaActualizacion = d.DatFechaActualizacion,
												  FechaCambioClave = d.DatFechaCambioClave.ToString(),
												  FechaIngreso = d.DatFechaIngreso,
												  Estado = d.IntIdEstado,
												  Apellidos = d.StrApellidos,
												  Cargo = d.StrCargo,
												  Celular = d.StrCelular,
												  Clave = d.StrClave,
												  Empresa = d.StrEmpresa,
												  Extension = d.StrExtension,
												  IdCambioClave = d.StrIdCambioClave.ToString(),
												  IdSeguridad = d.StrIdSeguridad,
												  Mail = d.StrMail,
												  Nombres = d.StrNombres,
												  NombreCompleto = d.StrNombres + d.StrApellidos,
												  Telefono = d.StrTelefono,
												  Usuario = d.StrUsuario,
												  RazonSocial = d.TblEmpresas.StrRazonSocial

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
		public TblUsuarios ObtenerIdSeguridad(System.Guid id_seguridad)
		{
			try
			{
				context.Configuration.LazyLoadingEnabled = false;

				var respuesta = from usuario in context.TblUsuarios.AsNoTracking()
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
		public string ValidarExistenciaRestablecer(string codigo_empresa, string codigo_usuario)
		{
			try
			{
				var respuesta = from usuario in context.TblUsuarios
								join empresa in context.TblEmpresas on usuario.StrEmpresa equals empresa.StrIdentificacion
								where empresa.StrIdentificacion.Equals(codigo_empresa)
								&& usuario.StrUsuario.Equals(codigo_usuario)
								select usuario;


				if (!respuesta.Any())
					throw new ApplicationException("Datos Incorrectos.");

				TblUsuarios usuarioRestablecer = respuesta.FirstOrDefault();

				usuarioRestablecer.DatFechaCambioClave = Fecha.GetFecha();
				usuarioRestablecer.StrIdCambioClave = Guid.NewGuid();
				//usuarioRestablecer.StrClave = Encriptar.Encriptar_SHA512(usuarioRestablecer.StrClave);
				Actualizar_usuario(usuarioRestablecer);

				TblEmpresas Empresas = (from empresa in context.TblEmpresas
										where empresa.StrIdentificacion.Equals(codigo_empresa)
										select empresa).FirstOrDefault();

				//Aqui debo enviar el correo a usuarioRestablecer.StrMail
				Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();

				Email.RestablecerClave(Empresas, usuarioRestablecer);

				return usuarioRestablecer.StrMail;
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
		#endregion

		#region Cambio de Clave
		public bool RestablecerClave(System.Guid IdSeguridad, string clave)
		{
			try
			{
				TblUsuarios datos = (from item in context.TblUsuarios
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
					return true;
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
		public bool ValidarIdSeguridad(System.Guid IdSeguridad)
		{
			try
			{
				List<TblUsuarios> datos = (from item in context.TblUsuarios
										   where item.StrIdCambioClave == IdSeguridad
										   select item).ToList();

				if (datos.Any())
				{

					if (datos.FirstOrDefault() != null)
					{
						if (!datos.FirstOrDefault().DatFechaCambioClave.HasValue || !(datos.FirstOrDefault().DatFechaCambioClave.Value.AddHours(24.0) >= Fecha.GetFecha()))
						{
							throw new ApplicationException("El ID de seguridad ha expirado; por favor realice el proceso de recuperación de contraseña nuevamente.");
						}
						return true;
					}
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
