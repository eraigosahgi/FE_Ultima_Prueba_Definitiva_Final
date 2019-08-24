using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaGlobalHGInet.HgiNet;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta;
using LibreriaGlobalHGInet.Formato;
using HGInetMiFacturaElectonicaData.Enumerables;
using LibreriaGlobalHGInet.Enumerables;
using System.Security.Cryptography.X509Certificates;
using HGInetMiFacturaElectonicaController.Properties;
using LibreriaGlobalHGInet.General;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_Empresa : BaseObject<TblEmpresas>
	{
		#region Constructores 

		public Ctl_Empresa() : base(new ModeloAutenticacion()) { }
		public Ctl_Empresa(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_Empresa(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		/// <summary>
		/// Valida Autenticacion del Datakey respecto a los campos enviados 
		/// </summary>
		/// <param name="datakey"></param>
		/// <param name="identificacion_obligado"></param>
		/// <returns></returns>
		public TblEmpresas Validar(string datakey, string identificacion_obligado)
		{
			TblEmpresas datos = (from item in context.TblEmpresas
								 where item.StrIdentificacion.Equals(identificacion_obligado)
								 && (!string.IsNullOrEmpty(item.StrSerial))
								 select item).FirstOrDefault();

			if (datos != null)
			{

				string datakey_construido = datos.StrSerial.ToString() + datos.StrIdentificacion.ToString();
				string datakey_encriptado_Sha1 = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA1(datakey_construido);

				string datakey_construido_may = datos.StrSerial.ToString().ToUpper() + datos.StrIdentificacion.ToString();
				string datakey_encriptado_may_sha1 = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA1(datakey_construido_may);

				//Encripcion en 512
				string datakey_encriptado_Sha512 = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA512(datakey_construido);
				string datakey_encriptado_may_sha512 = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA512(datakey_construido_may);


				if (datakey_encriptado_Sha1.Equals(datakey) || datakey_encriptado_may_sha1.Equals(datakey))
				{
					return datos;
				}
				else if (datakey_encriptado_Sha512.Equals(datakey) || datakey_encriptado_may_sha512.Equals(datakey))
				{
					return datos;
				}

			}

			throw new ApplicationException(string.Format("El DataKey de seguridad {0} para la identificación {1} es inválido.", datakey, identificacion_obligado));

		}

		/// <summary>
		/// Obtiene empresa con la identificacion
		/// </summary>
		/// <param name="identificacion">Identificacion de Obligado o Adquiriente</param>
		/// <returns></returns>
		public TblEmpresas Obtener(string identificacion)
		{

			var datos = (from item in context.TblEmpresas
						 where item.StrIdentificacion.Equals(identificacion)
						 select item).FirstOrDefault();
			return datos;
		}

		/// <summary>
		/// Obtiene todas las empresa asociadas al facturador
		/// </summary>
		/// <param name="identificacion">Identificacion de Obligado o Adquiriente</param>
		/// <returns></returns>
		public List<TblEmpresas> ObtenerAsociadas(string identificacion)
		{

			var datos = (from item in context.TblEmpresas
						 where item.StrIdentificacion.Equals(identificacion) || item.StrEmpresaAsociada.Equals(identificacion)
						 select item).ToList();

			return datos;
		}

		/// <summary>
		/// Crea una empresa en la BD
		/// </summary>
		/// <param name="empresa">Objeto BD de la empresa a crear</param>
		/// <returns></returns>
		public TblEmpresas Guardar(TblEmpresas empresa, List<ObjVerificacionEmail> ListaEmailRegistro)
		{
			try
			{


				TblEmpresas ConsultaEmpresa = Obtener(empresa.StrIdentificacion);
				if (ConsultaEmpresa != null)
					throw new ApplicationException("La empresa :  " + ConsultaEmpresa.StrRazonSocial + " ya existe");

				if (empresa == null)
					throw new ApplicationException("La empresa es incorrecta!, Error");

				if (String.IsNullOrEmpty(empresa.StrTipoIdentificacion))
					throw new ApplicationException("Debe ingresar el Tipo de Identificación");

				if (String.IsNullOrEmpty(empresa.StrIdentificacion))
					throw new ApplicationException("Debe ingresar el Numero de Identificación");

				if (String.IsNullOrEmpty(empresa.StrRazonSocial))
					throw new ApplicationException("Debe ingresar La Razón Social");

				if (String.IsNullOrEmpty(empresa.StrMailAdmin))
					throw new ApplicationException("Debe ingresar el Email");

				if (empresa.IntAdquiriente == false && empresa.IntObligado == false)
					throw new ApplicationException("Debe Indicar el Perfil");

				if (String.IsNullOrEmpty(empresa.StrSerial))
					throw new ApplicationException("Debe ingresar el serial de la empresa");

				empresa.IntIdentificacionDv = FuncionesIdentificacion.Dv(empresa.StrIdentificacion);


				empresa.StrMailAdmin = empresa.StrMailAdmin;
				empresa.StrMailEnvio = (string.IsNullOrEmpty(empresa.StrMailEnvio)) ? string.Empty : empresa.StrMailEnvio;
				empresa.StrMailRecepcion = (string.IsNullOrEmpty(empresa.StrMailRecepcion)) ? string.Empty : empresa.StrMailRecepcion;
				empresa.StrMailAcuse = (string.IsNullOrEmpty(empresa.StrMailAcuse)) ? string.Empty : empresa.StrMailAcuse;
				empresa.StrMailPagos = (string.IsNullOrEmpty(empresa.StrMailPagos)) ? string.Empty : empresa.StrMailPagos;

				//Automaticos                
				empresa.DatFechaIngreso = Fecha.GetFecha();
				empresa.DatFechaActualizacion = Fecha.GetFecha();
				empresa.StrIdSeguridad = Guid.NewGuid();

				empresa = this.Add(empresa);

				//Creacion de usuario generico 
				TblUsuarios UsuarioBd = null;
				Ctl_Usuario Usuario = new Ctl_Usuario();
				UsuarioBd = Usuario.Crear(empresa);

				//Envia correo de bienvenida al usuario creado
				Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();

				//Objeto de respuesta
				List<MensajeEnvio> Enviarmail = Email.Bienvenida(empresa, UsuarioBd);

				//Lista de email a confirmar
				//Proceso de verificación de email a confirmar
				List<ObjVerificacionEmail> ListEmailConfirmar = new List<ObjVerificacionEmail>();
				foreach (var item in ListaEmailRegistro)
				{
					//Y tampoco esta en la lista de email a enviar para confirmación, entonces lo agrego
					if (!EmailVerificado(ListEmailConfirmar, item.email))
					{
						//Esta validación es para no agregar el mismo correo dos veces
						ListEmailConfirmar.Add(item);
					}
				}
				//Proceso asincrono para el envio de emails
				var Tarea = EnvioEmailsConfirmacion(empresa.StrIdentificacion, ListEmailConfirmar);
				Task.WhenAny(Tarea);


				//#region Envio de correo con serial de activación
				//try
				//{	//Envia Email de activación de Serial
				//	Ctl_EnvioCorreos Ctrl_Email = new Ctl_EnvioCorreos();
				//	Ctrl_Email.EnviaSerial(empresa.StrIdentificacion, empresa.StrMailAdmin);
				//}
				//catch (Exception excepcion)
				//{
				//	LogExcepcion.Guardar(excepcion);
				//	throw new ApplicationException(string.Format("Error en el envio de correo con el serial : {0}", excepcion.Message), excepcion);
				//}
				//#endregion



				return empresa;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		/// <summary>
		/// Actualizar una empresa en la BD
		/// </summary>
		/// <param name="empresa">Objeto BD de la empresa a Actualizar</param>
		/// <returns></returns>
		public TblEmpresas Editar(TblEmpresas empresa, bool Administrador, List<ObjVerificacionEmail> ListaEmailRegistro)
		{
			try
			{
				bool EnviaCorreoSerial = false;

				if (empresa == null)
					throw new ApplicationException("La empresa es incorrecta!, Error");

				TblEmpresas EmpresaActualiza = (from item in context.TblEmpresas
												where item.StrIdentificacion.Equals(empresa.StrIdentificacion)
												select item).FirstOrDefault();

				if (EmpresaActualiza == null)
					throw new ApplicationException("La empresa que desea Actualizar no Existe");


				//Solo si es administrador, podra guardar toda la información, de resto solo actualizara la información de Facturador o Integrador.
				if (Administrador)
				{
					//If el Serial estaba en null o empty y viene con un dato para actualizar, entonces se debe enviar el correo 
					if (string.IsNullOrEmpty(EmpresaActualiza.StrSerial))
					{
						EnviaCorreoSerial = true;
					}

					EmpresaActualiza.StrRazonSocial = empresa.StrRazonSocial;

					EmpresaActualiza.IntAdquiriente = empresa.IntAdquiriente;
					EmpresaActualiza.IntHabilitacion = empresa.IntHabilitacion;
					EmpresaActualiza.IntObligado = empresa.IntObligado;
					EmpresaActualiza.StrEmpresaAsociada = empresa.StrEmpresaAsociada;
					EmpresaActualiza.StrResolucionDian = empresa.StrResolucionDian;
					EmpresaActualiza.StrObservaciones = empresa.StrObservaciones;
					EmpresaActualiza.StrSerial = empresa.StrSerial;
					EmpresaActualiza.IntIntegrador = empresa.IntIntegrador;
					EmpresaActualiza.IntNumUsuarios = empresa.IntNumUsuarios;

					EmpresaActualiza.IntManejaAnexos = empresa.IntManejaAnexos;

					EmpresaActualiza.StrEmpresaDescuento = empresa.StrEmpresaDescuento;
					EmpresaActualiza.IntIdEstado = empresa.IntIdEstado;
					EmpresaActualiza.IntCobroPostPago = empresa.IntCobroPostPago;
					EmpresaActualiza.IntEnvioMailRecepcion = empresa.IntEnvioMailRecepcion;
					EmpresaActualiza.IntVersionDian = empresa.IntVersionDian;
					EmpresaActualiza.IntTimeout = empresa.IntTimeout;

					#region Certificado
					EmpresaActualiza.IntCertFirma = empresa.IntCertFirma;
					EmpresaActualiza.IntCertProveedor = empresa.IntCertProveedor;
					EmpresaActualiza.IntCertResponsableHGI = empresa.IntCertResponsableHGI;
					EmpresaActualiza.IntCertNotificar = empresa.IntCertNotificar;
					EmpresaActualiza.StrCertClave = empresa.StrCertClave;
					EmpresaActualiza.DatCertVence = empresa.DatCertVence;
					#endregion

				}


				EmpresaActualiza.IntAcuseTacito = empresa.IntAcuseTacito;

				if (string.IsNullOrEmpty(empresa.StrMailAdmin))
					throw new ApplicationException("El Email Administrativo es obligatorio");

				EmpresaActualiza.StrMailAdmin = empresa.StrMailAdmin;
				EmpresaActualiza.StrMailRecepcion = (string.IsNullOrEmpty(empresa.StrMailRecepcion)) ? string.Empty : empresa.StrMailRecepcion;
				EmpresaActualiza.StrMailEnvio = (string.IsNullOrEmpty(empresa.StrMailEnvio)) ? string.Empty : empresa.StrMailEnvio;
				EmpresaActualiza.StrMailAcuse = (string.IsNullOrEmpty(empresa.StrMailAcuse)) ? string.Empty : empresa.StrMailAcuse;
				EmpresaActualiza.StrMailPagos = (string.IsNullOrEmpty(empresa.StrMailPagos)) ? string.Empty : empresa.StrMailPagos;
				EmpresaActualiza.DatFechaActualizacion = Fecha.GetFecha();
				EmpresaActualiza.StrTelefono = empresa.StrTelefono;

				//Creamos una lista para agregar los emails que ya estan confirmados y asi poder validar si uno que esta en proceso de verificación ya existe en la lista de correos confirmados
				List<ObjVerificacionEmail> ListaEmailVerificados = new List<ObjVerificacionEmail>();

				//Si el Email esta verificado entonces lo agrego a la lista de emails verificados para poder consultarlos alli
				if (empresa.IntMailAdminVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && !string.IsNullOrEmpty(EmpresaActualiza.StrMailAdmin))
				{
					ListaEmailVerificados.Add(new ObjVerificacionEmail { email = EmpresaActualiza.StrMailAdmin });
				}
				//Si el Email esta verificado entonces lo agrego a la lista de emails verificados para poder consultarlos alli
				if (empresa.IntMailRecepcionVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && !string.IsNullOrEmpty(EmpresaActualiza.StrMailRecepcion))
				{
					ListaEmailVerificados.Add(new ObjVerificacionEmail { email = EmpresaActualiza.StrMailRecepcion });
				}
				//Si el Email esta verificado entonces lo agrego a la lista de emails verificados para poder consultarlos alli
				if (empresa.IntMailEnvioVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && !string.IsNullOrEmpty(EmpresaActualiza.StrMailEnvio))
				{
					ListaEmailVerificados.Add(new ObjVerificacionEmail { email = EmpresaActualiza.StrMailEnvio });
				}
				//Si el Email esta verificado entonces lo agrego a la lista de emails verificados para poder consultarlos alli
				if (empresa.IntMailAcuseVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && !string.IsNullOrEmpty(EmpresaActualiza.StrMailAcuse))
				{
					ListaEmailVerificados.Add(new ObjVerificacionEmail { email = EmpresaActualiza.StrMailAcuse });
				}
				//Si el Email esta verificado entonces lo agrego a la lista de emails verificados para poder consultarlos alli
				if (empresa.IntMailPagosVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && !string.IsNullOrEmpty(EmpresaActualiza.StrMailPagos))
				{
					ListaEmailVerificados.Add(new ObjVerificacionEmail { email = EmpresaActualiza.StrMailPagos });
				}

				//Verificación de Emails
				EmpresaActualiza.IntMailAdminVerificado = (EmailVerificado(ListaEmailVerificados, EmpresaActualiza.StrMailAdmin)) ? (short)EstadoVerificacionEmail.Verificado.GetHashCode() : empresa.IntMailAdminVerificado;
				EmpresaActualiza.IntMailEnvioVerificado = (EmailVerificado(ListaEmailVerificados, EmpresaActualiza.StrMailEnvio)) ? (short)EstadoVerificacionEmail.Verificado.GetHashCode() : empresa.IntMailEnvioVerificado;
				EmpresaActualiza.IntMailAcuseVerificado = (EmailVerificado(ListaEmailVerificados, EmpresaActualiza.StrMailAcuse)) ? (short)EstadoVerificacionEmail.Verificado.GetHashCode() : empresa.IntMailAcuseVerificado;
				EmpresaActualiza.IntMailRecepcionVerificado = (EmailVerificado(ListaEmailVerificados, EmpresaActualiza.StrMailRecepcion)) ? (short)EstadoVerificacionEmail.Verificado.GetHashCode() : empresa.IntMailRecepcionVerificado;
				EmpresaActualiza.IntMailPagosVerificado = (EmailVerificado(ListaEmailVerificados, EmpresaActualiza.StrMailPagos)) ? (short)EstadoVerificacionEmail.Verificado.GetHashCode() : empresa.IntMailPagosVerificado;

				Actualizar(EmpresaActualiza);

				//Obtiene el usuario principal de la empresa.
				Ctl_Usuario clase_usuario = new Ctl_Usuario();
				TblUsuarios usuario_principal = clase_usuario.ObtenerUsuarios(EmpresaActualiza.StrIdentificacion, EmpresaActualiza.StrIdentificacion).FirstOrDefault();
				//Valida los permisos del usuario y los actualiza
				if (usuario_principal != null)
				{
					clase_usuario.ValidarPermisosUsuario(EmpresaActualiza, usuario_principal);
					//Actualiza el email del usuario generico
					clase_usuario.ActualizarEmail(EmpresaActualiza.StrIdentificacion, EmpresaActualiza.StrMailAdmin);
				}
				else
				{
					try
					{
						//Creacion de usuario generico 
						TblUsuarios UsuarioBd = null;
						Ctl_Usuario Usuario = new Ctl_Usuario();
						UsuarioBd = Usuario.Crear(EmpresaActualiza);

						clase_usuario.ValidarPermisosUsuario(EmpresaActualiza, usuario_principal);
						//Actualiza el email del usuario generico						
						clase_usuario.ActualizarEmail(EmpresaActualiza.StrIdentificacion, EmpresaActualiza.StrMailAdmin);

					}
					catch (Exception excepcion)
					{
						LogExcepcion.Guardar(excepcion);
						throw new ApplicationException(string.Format("Error al crear usuario generico de la empresa o sus permisos : {0}", excepcion.Message), excepcion);
					}

				}

				#region Proceso de verificación de email para envio de correos de confirmación
				//Lista de email a confirmar
				//Proceso de verificación de email a confirmar
				List<ObjVerificacionEmail> ListEmailConfirmar = new List<ObjVerificacionEmail>();
				foreach (var item in ListaEmailRegistro)
				{
					//Si no esta en la lista de email confirmados
					if (!EmailVerificado(ListaEmailVerificados, item.email))
					{
						//Y tampoco esta en la lista de email a enviar para confirmación, entonces lo agrego
						if (!EmailVerificado(ListEmailConfirmar, item.email))
						{
							//Esta validación es para no agregar el mismo correo dos veces
							ListEmailConfirmar.Add(item);
						}
					}

				}

				//Validamos si todos los emails estan confirmados
				if (EmpresaActualiza.IntMailAdminVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && EmpresaActualiza.IntMailEnvioVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && EmpresaActualiza.IntMailRecepcionVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && EmpresaActualiza.IntMailAcuseVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && EmpresaActualiza.IntMailPagosVerificado == EstadoVerificacionEmail.Verificado.GetHashCode())
				{
					//Si la empresa esta en proceso de registro y todos los emails estan confirmados, entonces activamos la empresa
					if (EmpresaActualiza.IntIdEstado == EstadoEmpresa.REGISTRO.GetHashCode())//Registro
					{
						EmpresaActualiza.IntIdEstado = (short)EstadoEmpresa.ACTIVA.GetHashCode();//Activa
						this.Actualizar(EmpresaActualiza);
					}

				}
				else
				{
					{
						//Proceso asincrono para el envio de emails
						var Tarea = EnvioEmailsConfirmacion(empresa.StrIdentificacion, ListEmailConfirmar);
						Task.WhenAny(Tarea);
					}
				}

				#endregion

				//#region Envio de correo con serial de activación
				//try
				//{
				//	//Esta variable se activa solo si la empresa autenticada es administradora y el serial de activación paso de null a tener un valor real
				//	if (EnviaCorreoSerial)
				//	{
				//		Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();
				//		List<MensajeEnvio> Enviarmail = Email.EnviaSerial(EmpresaActualiza.StrIdentificacion, EmpresaActualiza.StrMailAdmin);
				//	}
				//}
				//catch (Exception excepcion)
				//{
				//	LogExcepcion.Guardar(excepcion);
				//	throw new ApplicationException(string.Format("Error en el envio de correo con el serial : {0}", excepcion.Message), excepcion);
				//}
				//#endregion







				return EmpresaActualiza;
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
				throw new ApplicationException(string.Format("Error : {0}", excepcion.Message), excepcion);
			}

		}



		/// <summary>
		/// Proceso para el envio de emails de confirmación
		/// </summary>
		/// <param name="ListEmailConfirmar">Lista de emails a confirmar</param>
		/// <returns>Tarea</returns>
		public async Task EnvioEmailsConfirmacion(string Empresa, List<ObjVerificacionEmail> ListEmailConfirmar)
		{
			await Task.Factory.StartNew(() =>
			{
				Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();
				//Recorremos la lista para enviar cada uno de los Emails
				foreach (var Datos in ListEmailConfirmar)
				{
					try
					{
						//Hacemos el envio de la confirmación del email
						var Enviarmail = Email.EnviaConfirmacionEmail(Empresa, Datos.email);
					}
					catch (Exception excepcion)
					{
						LogExcepcion.Guardar(excepcion);
					}
				}
			});
		}

		/// <summary>
		/// Confirma los email enviados al correo
		/// </summary>
		/// <param name="IdSeguridad">Guid de seguridad de la empresa</param>
		/// <param name="Mail">Email que se desea validar o confirmar</param>
		/// <returns>Retorna un string indicando si se actualizo la información exitosamente</returns>
		public ObjResultado ConfirmarMail(Guid IdSeguridad, string Mail)
		{
			bool Concidencia = false;
			bool Actualizo = false;

			ObjResultado Result = new ObjResultado();

			TblEmpresas Empresa = Obtener(IdSeguridad).FirstOrDefault();

			//Caso Correo administrativo
			if (Empresa.StrMailAdmin.Equals(Mail))
			{
				//Valido si el correo ya fue verificado para no actualizar nuevamente el correo
				if (Empresa.IntMailAdminVerificado != EstadoVerificacionEmail.Verificado.GetHashCode())
				{
					Empresa.IntMailAdminVerificado = (short)EstadoVerificacionEmail.Verificado.GetHashCode();
					Actualizo = true;
				}
				Concidencia = true;
			}

			//Caso Correo de Envio
			if (Empresa.StrMailEnvio.Equals(Mail))
			{
				//Valido si el correo ya fue verificado para no actualizar nuevamente el correo
				if (Empresa.IntMailEnvioVerificado != EstadoVerificacionEmail.Verificado.GetHashCode())
				{
					Empresa.IntMailEnvioVerificado = (short)EstadoVerificacionEmail.Verificado.GetHashCode();
					Actualizo = true;
				}
				Empresa.IntMailEnvioVerificado = (short)EstadoVerificacionEmail.Verificado.GetHashCode();
				Concidencia = true;
			}

			//Caso Correo de Acuse
			if (Empresa.StrMailAcuse.Equals(Mail))
			{
				//Valido si el correo ya fue verificado para no actualizar nuevamente el correo
				if (Empresa.IntMailAcuseVerificado != EstadoVerificacionEmail.Verificado.GetHashCode())
				{
					Empresa.IntMailAcuseVerificado = (short)EstadoVerificacionEmail.Verificado.GetHashCode();
					Actualizo = true;
				}
				Empresa.IntMailAcuseVerificado = (short)EstadoVerificacionEmail.Verificado.GetHashCode();
				Concidencia = true;
			}

			//Caso Correo de Recepción
			if (Empresa.StrMailRecepcion.Equals(Mail))
			{
				//Valido si el correo ya fue verificado para no actualizar nuevamente el correo
				if (Empresa.IntMailRecepcionVerificado != EstadoVerificacionEmail.Verificado.GetHashCode())
				{
					Empresa.IntMailRecepcionVerificado = (short)EstadoVerificacionEmail.Verificado.GetHashCode();
					Actualizo = true;
				}
				Empresa.IntMailRecepcionVerificado = (short)EstadoVerificacionEmail.Verificado.GetHashCode();
				Concidencia = true;
			}

			//Caso Correo de Recepción
			if (Empresa.StrMailPagos.Equals(Mail))
			{
				//Valido si el correo ya fue verificado para no actualizar nuevamente el correo
				if (Empresa.IntMailPagosVerificado != EstadoVerificacionEmail.Verificado.GetHashCode())
				{
					Empresa.IntMailPagosVerificado = (short)EstadoVerificacionEmail.Verificado.GetHashCode();
					Actualizo = true;
				}
				Empresa.IntMailPagosVerificado = (short)EstadoVerificacionEmail.Verificado.GetHashCode();
				Concidencia = true;
			}

			// ! No existe el correo indicado
			if (Concidencia)
			{
				// ! Correo confirmado anteriormente
				if (Actualizo)
				{
					//Validamos si todos los emails estan confirmados
					if (Empresa.IntMailAdminVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && Empresa.IntMailEnvioVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && Empresa.IntMailRecepcionVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && Empresa.IntMailAcuseVerificado == EstadoVerificacionEmail.Verificado.GetHashCode() && Empresa.IntMailPagosVerificado == EstadoVerificacionEmail.Verificado.GetHashCode())
					{
						//Si la empresa esta en proceso de registro y todos los emails estan confirmados, entonces activamos la empresa
						if (Empresa.IntIdEstado == EstadoEmpresa.REGISTRO.GetHashCode())//Registro
						{
							Empresa.IntIdEstado = (short)EstadoEmpresa.ACTIVA.GetHashCode();//Activa
							this.Actualizar(Empresa);
							//Correo actualizado con exito y empresa activada
							Result.Codigo = 1;
							Result.Descripcion = string.Format("Correo: {0} confirmado exitosamente y empresa {1} activada con exito", Mail, Empresa.StrRazonSocial);
							return Result;
						}
					}
					//Correo actualizado exitosamente pero la empresa aun no se ha activado
					this.Actualizar(Empresa);					
					Result.Codigo = 1;
					Result.Descripcion = string.Format("Correo: {0} confirmado exitosamente ", Mail);
					return Result;
				}
				else
				{
					//Correo confirmado anteriormente					
					Result.Codigo = 2;
					Result.Descripcion = string.Format("Correo: {0} ya habia sido confirmado anteriormente", Mail);
					return Result;
				}
			}
			else
			{
				//No existe el correo indicado				
				Result.Codigo = 3;
				Result.Descripcion = string.Format("Correo: {0} No se encontro el correo que desea confirmar", Mail);
				return Result;
			}

		}


		public class ObjResultado
		{
			public string Descripcion { get; set; }
			public int Codigo { get; set; }
		}



		/// <summary>
		/// Actualizar el serial de la empresa  en la BD
		/// </summary>
		/// <param name="Codigo_Identificacion"></param>
		/// <param name="Codigo_Serial"></param>
		/// <param name="Codigo_Resolucion"></param> 
		/// <returns></returns>
		public TblEmpresas Editar(string Identificacion, string Serial, string Resolucion)
		{
			try
			{

				if (String.IsNullOrEmpty(Identificacion) || string.IsNullOrEmpty(Serial) || string.IsNullOrEmpty(Resolucion))
					throw new ApplicationException("Los datos estan incompletos");


				TblEmpresas EmpresaActualiza = (from item in context.TblEmpresas
												where item.StrIdentificacion.Equals(Identificacion)
												select item).FirstOrDefault();


				if (EmpresaActualiza == null)
					throw new ApplicationException("La empresa que desea Actualizar no Existe");


				Ctl_Usuario clUsuario = new Ctl_Usuario();

				List<TblUsuarios> lUsuario = new List<TblUsuarios>();

				lUsuario = clUsuario.ObtenerUsuarios(EmpresaActualiza.StrIdentificacion, EmpresaActualiza.StrIdentificacion);

				if (lUsuario.Count < 1)
					throw new ApplicationException("La empresa no se puede activar ya que no posee ningun usuario asociado");

				TblUsuarios Usuario = lUsuario.FirstOrDefault();

				EmpresaActualiza.StrSerial = Serial.Trim();
				EmpresaActualiza.StrResolucionDian = Resolucion.Trim();

				Actualizar(EmpresaActualiza);

				Ctl_EnvioCorreos Email = new Ctl_EnvioCorreos();

				List<MensajeEnvio> Enviarmail = Email.EnviaSerial(EmpresaActualiza.StrIdentificacion, EmpresaActualiza.StrMailAdmin);

				return EmpresaActualiza;
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
				throw new ApplicationException(string.Format("Error : {0}", excepcion.Message), excepcion);
			}

		}


		/// <summary>
		/// Crea una empresa en la BD
		/// </summary>
		/// <param name="empresa">Objeto BD de la empresa a crear</param>
		/// <returns></returns>
		public TblEmpresas Crear(TblEmpresas empresa)
		{
			empresa = this.Add(empresa);

			return empresa;
		}

		/// <summary>
		/// Convierte y Crea un objeto de servicio a un objeto de BD
		/// </summary>
		/// <param name="empresa">Objeto de servicio de la empresa a crear</param>
		/// <returns></returns>
		public TblEmpresas Crear(Tercero empresa, bool adquiriente = true)
		{
			// convierte el objeto del servicio
			TblEmpresas tbl_empresa = Convertir(empresa, adquiriente);

			// agrega el objeto
			tbl_empresa = Crear(tbl_empresa);

			return tbl_empresa;

		}

		/// <summary>
		/// Convierte un objeto de tercero a un objeto de Bd
		/// </summary>
		/// <param name="empresa">Informacion de la empresa</param>
		/// <returns></returns>
		public static TblEmpresas Convertir(Tercero empresa, bool adquiriente = true)
		{

			if (empresa == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "empresa", "Ctl_Empresa"));

			TblEmpresas tbl_empresa = new TblEmpresas();

			//if (empresa.Email.Contains(";"))
			//{
			//	foreach (var item_mail in Coleccion.ConvertirLista(empresa.Email, ';'))
			//	{
			//		// recibe el email el adquiriente
			//		tbl_empresa.StrMailAdmin = item_mail;
			//		break;
			//	}
			//}
			//else
			//{
			//	tbl_empresa.StrMailAdmin = empresa.Email;
			//}

			tbl_empresa.StrTipoIdentificacion = empresa.TipoIdentificacion.ToString();
			tbl_empresa.StrIdentificacion = empresa.Identificacion;
			tbl_empresa.IntIdentificacionDv = Convert.ToInt16(empresa.IdentificacionDv);
			tbl_empresa.StrRazonSocial = empresa.RazonSocial;

			tbl_empresa.DatFechaIngreso = Fecha.GetFecha();
			tbl_empresa.IntAdquiriente = adquiriente;
			tbl_empresa.IntObligado = (adquiriente == false) ? (true) : false;
			tbl_empresa.IntHabilitacion = 0;
			tbl_empresa.DatFechaActualizacion = Fecha.GetFecha();
			tbl_empresa.StrIdSeguridad = Guid.NewGuid();
			tbl_empresa.IntNumUsuarios = 1;
			tbl_empresa.StrTelefono = empresa.Telefono;
			tbl_empresa.StrEmpresaAsociada = empresa.Identificacion;
			tbl_empresa.StrEmpresaDescuento = empresa.Identificacion;

			//tbl_empresa.StrMailAcuse = tbl_empresa.StrMailAdmin;
			//tbl_empresa.StrMailEnvio = tbl_empresa.StrMailAdmin;
			//tbl_empresa.StrMailRecepcion = tbl_empresa.StrMailAdmin;
			//tbl_empresa.StrMailPagos = tbl_empresa.StrMailAdmin;

			return tbl_empresa;
		}

		/// <summary>
		/// Convierte un Objeto de Bd en objeto de Servicio
		/// </summary>
		/// <param name="empresa">Objeto BD</param>
		/// <returns>Objeto de Servicio Tercero</returns>
		public static Tercero Convertir(TblEmpresas empresa)
		{
			if (empresa == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "empresa", "Ctl_Empresa"));

			Tercero empresa_obj = new Tercero();

			empresa_obj.Identificacion = empresa.StrIdentificacion;
			empresa_obj.IdentificacionDv = empresa.IntIdentificacionDv;
			empresa_obj.TipoIdentificacion = Convert.ToInt16(empresa.StrTipoIdentificacion);
			empresa_obj.RazonSocial = empresa.StrRazonSocial;
			empresa_obj.Email = empresa.StrMailAdmin;
			empresa_obj.Telefono = empresa.StrTelefono;

			return empresa_obj;
		}

		/// <summary>
		/// Obtiene Todas las empresas
		/// </summary>        
		/// <returns></returns>
		public List<TblEmpresas> ObtenerTodas()
		{
			List<TblEmpresas> datos = (from item in context.TblEmpresas
									   select item).ToList();

			return datos;
		}

		/// <summary>
		/// Obtiene las empresas facturadoras
		/// </summary>        
		/// <returns></returns>
		public List<TblEmpresas> ObtenerFacturadores()
		{
			List<TblEmpresas> datos = (from item in context.TblEmpresas
									   where item.IntObligado.Equals(true)
									   select item).ToList();

			return datos;
		}

		/// <summary>
		/// Obtiene las empresas propias facturadoras  y las asociadas
		/// </summary>        
		/// <returns></returns>
		public List<TblEmpresas> ObtenerFacturadores(string Identificacion)
		{
			List<TblEmpresas> datos = (from item in context.TblEmpresas
									   where item.IntObligado.Equals(true)
									   && item.StrIdentificacion.Equals(Identificacion) || item.StrEmpresaAsociada.Equals(Identificacion)
									   select item).ToList();

			return datos;
		}

		/// <summary>
		/// Obtiene Todas las empresas
		/// </summary>        
		/// /// <param name="IdSeguridad">id de seguridad</param>
		/// <returns></returns>
		public List<TblEmpresas> Obtener(System.Guid IdSeguridad)
		{
			List<TblEmpresas> datos = (from item in context.TblEmpresas
									   where item.StrIdSeguridad.Equals(IdSeguridad)
									   select item).ToList();

			return datos;
		}


		/// <summary>
		/// Obtiene los datos del certificado
		/// </summary>
		/// <param name="IdSeguridad">Id de seguridad de la empresa</param>
		/// <param name="clave">Clave del certificado</param>
		/// <returns>DatosCertificados con datos del nombre y fecha de vencimiento del certificado </returns>
		public CertificadoDigital ObtenerInfCert(System.Guid IdSeguridad, string clave)
		{
			string carpeta_certificado = string.Empty;

			try
			{
				TblEmpresas Empresa = new TblEmpresas();
				Empresa = Obtener(IdSeguridad).FirstOrDefault();

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				carpeta_certificado = string.Format("{0}\\{1}\\{2}.pfx", plataforma_datos.RutaDmsFisica, Constantes.CarpetaCertificadosDigitales, Empresa.StrIdSeguridad);

				X509Certificate2 Certificado = new X509Certificate2(carpeta_certificado, clave);

				CertificadoDigital Datos = new CertificadoDigital();

				Datos.Fechavenc = Certificado.NotAfter;

				Datos.Propietario = Certificado.FriendlyName;

				Datos.Serial = Certificado.SerialNumber;

				Datos.Certificadora = Certificado.Issuer;

				return Datos;
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
				throw new ApplicationException(string.Format("Error en la lectura del certificado: {0}", excepcion.Message), excepcion);
			}
		}

		/// <summary>
		/// Actualizar Empresa
		/// </summary>        
		/// /// <param name="TblEmpresa">Objeto empresa a actualizar en BD</param>
		/// <returns></returns>
		public TblEmpresas Actualizar(TblEmpresas empresa)
		{
			empresa = this.Edit(empresa);

			return empresa;

		}

		#region Lista empresas para crear plan post pago
		/// <summary>
		/// Retorna la lista de empresas que estan activas y requieren plan postpago mensual
		/// </summary>
		/// <returns></returns>
		public List<TblEmpresas> ObtenerEmpPostPago()
		{
			//Estado empresa     -- 1 Activa
			//IntObligado = true -- Es Facturador
			//IntCobroPostPago   -- 1 Tiene plan post pago automatico
			List<TblEmpresas> ListaEmpresas = (from empresas in context.TblEmpresas
											   where empresas.IntIdEstado == 1
											   && empresas.IntCobroPostPago == 1
											   && empresas.IntObligado == true
											   select empresas).ToList();

			return ListaEmpresas;
		}

		#endregion

		#region Filtros

		public object ObtenerAdquirientes(string identificacion)
		{

			var datos = (from item in context.TblDocumentos
						 where item.TblEmpresasAdquiriente.IntAdquiriente == true
						 group item by new { item.StrEmpresaAdquiriente } into Adquirientes
						 select new
						 {
							 ID = Adquirientes.FirstOrDefault().StrEmpresaAdquiriente,
							 Texto = Adquirientes.FirstOrDefault().TblEmpresasAdquiriente.StrRazonSocial,
							 Fact = Adquirientes.FirstOrDefault().TblEmpresasFacturador.StrIdentificacion
						 }).Where(x => x.Fact.Equals(identificacion)).OrderBy(x => x.Texto).ToList();

			return datos;
		}

		#endregion


		/// <summary>
		/// Retorna la razón social de un obligado o un adquiriente
		/// </summary>
		/// <param name="Identificacion">Numero de identificación</param>
		/// <returns></returns>
		public string ObtenerRazonSocial(string Identificacion)
		{
			try
			{
				TblEmpresas empresa = context.TblEmpresas.Where(x => x.StrIdentificacion.Equals(Identificacion)).FirstOrDefault();
				return string.Format("{0}--{1}", Identificacion, empresa.StrRazonSocial.ToString());
			}
			catch (Exception)
			{
				return Identificacion;
			}
		}

		/// <summary>
		/// Convierte un objeto de tipo TblEmpresas a Empresa
		/// </summary>
		/// <param name="datos_empresa">datos de la empresa</param>
		/// <returns></returns>
		public static Empresa ConvertirEmpresa(TblEmpresas datos_empresa)
		{
			try
			{
				if (datos_empresa == null)
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "empresa", "Ctl_Empresa"));

				Empresa datos_retorno = new Empresa();
				datos_retorno.Identificacion = datos_empresa.StrIdentificacion;
				datos_retorno.IdentificacionDv = datos_empresa.IntIdentificacionDv;
				datos_retorno.RazonSocial = datos_empresa.StrRazonSocial;
				datos_retorno.Telefono = datos_empresa.StrTelefono;
				datos_retorno.HorasAcuseTacito = datos_empresa.IntAcuseTacito.Value;
				datos_retorno.ManejaAnexo = datos_empresa.IntManejaAnexos;
				datos_retorno.EmailAdmin = datos_empresa.StrMailAdmin;
				datos_retorno.EmailEnvio = datos_empresa.StrMailEnvio;
				datos_retorno.EmailRecepcion = datos_empresa.StrMailRecepcion;
				datos_retorno.EmailAcuse = datos_empresa.StrMailAcuse;
				datos_retorno.EmailPagos = datos_empresa.StrMailPagos;
				datos_retorno.VersionDian = datos_empresa.IntVersionDian;
				return datos_retorno;
			}
			catch (Exception excepcion)
			{
				LogExcepcion.Guardar(excepcion);
				throw new ApplicationException(string.Format("Error : {0}", excepcion.Message), excepcion);
			}
		}


		#region Porceso de verificación de Email
		/// <summary>
		/// Valida si un email ya esta verificado 
		/// </summary>
		/// <param name="Lista">Lista de Emails verificados</param>
		/// <param name="Email">Email que se desea validar</param>
		/// <returns>Retorna true si el email esta verificado</returns>
		public bool EmailVerificado(List<ObjVerificacionEmail> Lista, string Email)
		{
			var Result = Lista.Where(x => x.email == Email).FirstOrDefault();

			if (Result == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Se crea clase anidada para el envio de Correos de confirmación
		/// </summary>
		public class ObjVerificacionEmail
		{
			public string email { get; set; }
		}
		#endregion

	}
}
