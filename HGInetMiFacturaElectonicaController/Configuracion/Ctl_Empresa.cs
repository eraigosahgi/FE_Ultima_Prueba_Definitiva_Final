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
using HGInetMiFacturaElectonicaData.Objetos;
using LibreriaGlobalHGInet.RegistroLog;
using System.Web;
using System.IO;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetFirmaDigital;
using LibreriaGlobalHGInet.Peticiones;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_Empresa : BaseObject<TblEmpresas>
	{
		#region Constructores 

		public Ctl_Empresa() : base(new ModeloAutenticacion()) { }
		public Ctl_Empresa(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_Empresa(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion





		public CertificadoDigital GuardarCertificadoDigital(HttpPostedFile File, System.Guid IdSeguridad, string clave, int certificadora)
		{
			string carpeta_certificado = string.Empty;
			EnumCertificadoras _certificadora = Enumeracion.GetEnumObjectByValue<EnumCertificadoras>(certificadora);

			try
			{
				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;
				//Ruta temporal para crear el archivo y validarlo
				carpeta_certificado = string.Format("{0}\\{1}\\Temp\\{2}.pfx", plataforma_datos.RutaDmsFisica, Constantes.CarpetaCertificadosDigitales, IdSeguridad);
				//Validamos que 
				if (File != null && File.ContentLength > 0)
				{
					string RutaDirectorio = carpeta_certificado.Replace(string.Format(@"\{0}.pfx", IdSeguridad), "");
					//Guardamos el archivo en la ruta temporal
					if (Directorio.ValidarExistenciaArchivo(RutaDirectorio))
					{
						File.SaveAs(carpeta_certificado);
					}
					else
					{

						Directorio.CrearDirectorio(RutaDirectorio);
						File.SaveAs(carpeta_certificado);
					}

				}
				else
				{
					//Generamos excepción si no tenemos datos en el archivo
					throw new ApplicationException(string.Format("Error al guardar el certificado: {0}", "El archivo no contiene información"));
				}

				//Validamos si el certificado esta correcto con respecto a la clave 
				X509Certificate2 Certificado = new X509Certificate2(carpeta_certificado, clave);
				CertificadoDigital Datos = new CertificadoDigital();

				if (Certificado.Issuer.Contains(Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<EnumCertificadoras>(certificadora))) == false)
				{
					throw new ApplicationException("El certificado a importar no coincide con la empresa certificadora seleccionada");
				}

				//Se obtiene el facturador
				TblEmpresas facturador = Obtener(IdSeguridad).FirstOrDefault();

				//Se valida que el nit del facturador en plataforma sea el mismo del certificado
				if (!Certificado.Subject.Contains(facturador.StrIdentificacion))
					throw new ApplicationException(string.Format("Certificado digital {0} no corresponde a  la identificación {1}", Path.GetFileNameWithoutExtension(carpeta_certificado).Substring(0, 8), facturador.StrIdentificacion));

				//Se valida que el certificado no este vencido
				if (Certificado.NotAfter < Fecha.GetFecha())
					throw new ApplicationException(string.Format("Certificado digital {0} con fecha de vigencia {1}, se encuentra vencido", Path.GetFileNameWithoutExtension(carpeta_certificado).Substring(0, 8), Certificado.NotAfter));


				Datos.Fechavenc = Certificado.NotAfter;
				Datos.Serial = Certificado.SerialNumber;
				Datos.Certificadora = Certificado.Issuer;
				//Elimino el archivo Temporal
				System.IO.File.Delete(carpeta_certificado);
				//Copiamos el archivo en la ruta que debe estar
				carpeta_certificado = string.Format("{0}\\{1}\\{2}.pfx", plataforma_datos.RutaDmsFisica, Constantes.CarpetaCertificadosDigitales, IdSeguridad);
				//Guardamos el archivo
				File.SaveAs(carpeta_certificado);
				//Leemos el archivo para garantizar que si esta copiado en la ruta original
				Certificado = new X509Certificate2(carpeta_certificado, clave);

				//Se valida que el nit del facturador en plataforma sea el mismo del certificado
				if (!Certificado.Subject.Contains(facturador.StrIdentificacion))
					throw new ApplicationException(string.Format("Certificado digital {0} no corresponde a  la identificación {1}", Path.GetFileNameWithoutExtension(carpeta_certificado).Substring(0, 8), facturador.StrIdentificacion));

				//Se valida que el certificado no este vencido
				if (Certificado.NotAfter < Fecha.GetFecha())
					throw new ApplicationException(string.Format("Certificado digital {0} con fecha de vigencia {1}, se encuentra vencido", Path.GetFileNameWithoutExtension(carpeta_certificado).Substring(0, 8), Certificado.NotAfter));

				Datos = new CertificadoDigital();
				Datos.Fechavenc = Certificado.NotAfter;
				Datos.Propietario = facturador.StrIdentificacion;
				Datos.Serial = Certificado.SerialNumber;
				Datos.Certificadora = Certificado.Issuer;
				//Retornamos los datos para mostrarlos al usuario
				return Datos;
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta);
				throw new ApplicationException(string.Format("Error al guardar el certificado: {0}", excepcion.Message), excepcion);
			}
		}



		/// <summary>
		/// Valida Autenticacion del Datakey respecto a los campos enviados 
		/// </summary>
		/// <param name="datakey"></param>
		/// <param name="identificacion_obligado"></param>
		/// <returns></returns>
		public TblEmpresas Validar(string datakey, string identificacion_obligado)
		{
			context.Configuration.LazyLoadingEnabled = false;

			TblEmpresas datos = (from item in context.TblEmpresas
								 where item.StrIdentificacion.Equals(identificacion_obligado)
								 && (!string.IsNullOrEmpty(item.StrSerial))
								 select item).FirstOrDefault();

			if (datos != null)
			{
				if (datos.IntIdEstado == 2)
					throw new ApplicationException(string.Format("El Facturador con la identificación {0} no se encuentra activo.", identificacion_obligado));

				if (datos.IntIdEstado == 3 && datos.IntVersionDian == 2)
					throw new ApplicationException(string.Format("El Facturador con la identificación {0} no ha confirmado sus correos.", identificacion_obligado));

				//Validación debug versión 1
				//Fecha.GetFecha()
				if (Fecha.GetFecha() > Convert.ToDateTime("2019-12-02 00:00") && (datos.IntVersionDian == 1 && datos.IntDebug != true))
				{
					throw new ApplicationException(string.Format("Según la normatividad, el documento debe enviarse a la DIAN por la plataforma de validación previa, por favor verifique su habilitación en la plataforma"));
				}

				if (datos.IntCertFirma == 1 && string.IsNullOrEmpty(datos.StrCertClave))
				{
					throw new ApplicationException(string.Format("El Facturador con la identificación {0} no cuenta con un certificado digital.", identificacion_obligado));
				}

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
				else if (datakey_encriptado_Sha512.Equals(datakey) || datakey_encriptado_may_sha512.Equals(datakey) || datakey_encriptado_may_sha512.ToString().ToUpper().Equals(datakey))
				{
					return datos;
				}

			}

			throw new ApplicationException(string.Format("El DataKey de seguridad {0} para la identificación {1} es inválido.", datakey, identificacion_obligado));

		}


		/// <summary>
		/// Validar tercero Facturador desde Interoperabilidad
		/// </summary>
		/// <param name="identificacion_obligado"></param>
		/// <returns></returns>
		public TblEmpresas ValidarInteroperabilidad(string identificacion_obligado)
		{
			context.Configuration.LazyLoadingEnabled = false;

			TblEmpresas datos = (from item in context.TblEmpresas
								 where item.StrIdentificacion.Equals(identificacion_obligado)
								 && (!string.IsNullOrEmpty(item.StrSerial))
								 select item).FirstOrDefault();

			if (datos != null)
			{
				if (datos.IntIdEstado == 2)
					throw new ApplicationException(string.Format("La identificación {0} no se encuentra activo.", identificacion_obligado));

				if (datos.IntVersionDian != 2)
					throw new ApplicationException(string.Format("La identificación {0} no se encuentra en la versión 2.", identificacion_obligado));

				return datos;
			}

			throw new ApplicationException(string.Format("No se encontró la identificación {0}", identificacion_obligado));
		}


		/// <summary>
		/// Obtiene empresa con la identificacion
		/// </summary>
		/// <param name="identificacion">Identificacion de Obligado o Adquiriente</param>
		/// <returns></returns>
		public TblEmpresas Obtener(string identificacion, bool LazyLoading = true)
		{

			context.Configuration.LazyLoadingEnabled = LazyLoading;

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
			context.Configuration.LazyLoadingEnabled = false;
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

				if (empresa.IntHabilitacionNomina != null)
				{
					CrearPermisosNomina(empresa.StrIdentificacion);
				}



				return empresa;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		/// <summary>
		/// Agrega los permisos de nomina del usuario principal de la empresa
		/// </summary>
		/// <param name="identificacion"></param>
		public void CrearPermisosNomina(string identificacion)
		{
			try
			{
				Ctl_OpcionesUsuario _controlador = new Ctl_OpcionesUsuario();

				TblOpcionesUsuario permiso = new TblOpcionesUsuario();

				//Crea el Pemiso Principal de nomina (Nómina Electrónica)
				permiso.StrEmpresa = identificacion;
				permiso.StrUsuario = identificacion;
				permiso.IntIdOpcion = 23;
				permiso.IntConsultar = true;
				permiso.IntAgregar = true;
				permiso.IntEditar = true;
				permiso.IntEliminar = true;
				permiso.IntAnular = true;
				permiso.IntGestion = true;
				_controlador.Crear(permiso);

				//Crea el Pemiso (Emisor)
				permiso = new TblOpcionesUsuario();
				permiso.StrEmpresa = identificacion;
				permiso.StrUsuario = identificacion;
				permiso.IntIdOpcion = 231;
				permiso.IntConsultar = true;
				permiso.IntAgregar = true;
				permiso.IntEditar = true;
				permiso.IntEliminar = true;
				permiso.IntAnular = true;
				permiso.IntGestion = true;
				_controlador.Crear(permiso);

				//Crea el Pemiso (Documentos Emisor)
				permiso = new TblOpcionesUsuario();
				permiso.StrEmpresa = identificacion;
				permiso.StrUsuario = identificacion;
				permiso.IntIdOpcion = 2311;
				permiso.IntConsultar = true;
				permiso.IntAgregar = true;
				permiso.IntEditar = true;
				permiso.IntEliminar = true;
				permiso.IntAnular = true;
				permiso.IntGestion = true;
				_controlador.Crear(permiso);

			}
			catch (Exception)
			{

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
				bool crear_permiso_nomina = false;

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
					//Validamos si debemos crear permisos de nomina
					if (EmpresaActualiza.IntHabilitacionNomina == null && empresa.IntHabilitacionNomina != null)
					{
						crear_permiso_nomina = true;
					}
					EmpresaActualiza.IntHabilitacionNomina = empresa.IntHabilitacionNomina;
					EmpresaActualiza.IntObligado = empresa.IntObligado;
					EmpresaActualiza.StrEmpresaAsociada = empresa.StrEmpresaAsociada;
					EmpresaActualiza.StrResolucionDian = empresa.StrResolucionDian;
					EmpresaActualiza.StrObservaciones = empresa.StrObservaciones;
					EmpresaActualiza.StrSerial = empresa.StrSerial;
					EmpresaActualiza.IntIntegrador = empresa.IntIntegrador;
					EmpresaActualiza.IntNumUsuarios = empresa.IntNumUsuarios;

					EmpresaActualiza.IntManejaAnexos = empresa.IntManejaAnexos;
					EmpresaActualiza.IntManejaPagoE = empresa.IntManejaPagoE;
					EmpresaActualiza.IntPagoEParcial = empresa.IntPagoEParcial;


					EmpresaActualiza.StrEmpresaDescuento = empresa.StrEmpresaDescuento;
					EmpresaActualiza.IntIdEstado = empresa.IntIdEstado;
					EmpresaActualiza.IntCobroPostPago = empresa.IntCobroPostPago;
					EmpresaActualiza.IntEnvioMailRecepcion = empresa.IntEnvioMailRecepcion;
					EmpresaActualiza.IntVersionDian = empresa.IntVersionDian;
					EmpresaActualiza.IntTimeout = empresa.IntTimeout;

					EmpresaActualiza.IntDebug = empresa.IntDebug;
					EmpresaActualiza.IntInteroperabilidad = empresa.IntInteroperabilidad;
					EmpresaActualiza.StrSerialCloudServices = empresa.StrSerialCloudServices;
					EmpresaActualiza.IntTipoPlan = empresa.IntTipoPlan;
					EmpresaActualiza.IntCompraPlan = empresa.IntTipoPlan == 0 ? true : false;

					#region Certificado
					EmpresaActualiza.IntCertFirma = empresa.IntCertFirma;
					EmpresaActualiza.IntCertProveedor = empresa.IntCertProveedor;
					EmpresaActualiza.IntCertResponsableHGI = empresa.IntCertResponsableHGI;
					EmpresaActualiza.IntCertNotificar = empresa.IntCertNotificar;
					EmpresaActualiza.StrCertClave = empresa.StrCertClave;
					EmpresaActualiza.DatCertVence = empresa.DatCertVence;
					#endregion

				}

				//Si firma el Facturador
				if (empresa.IntCertFirma == 1)
				{
					EmpresaActualiza.IntCertProveedor = empresa.IntCertProveedor;
					EmpresaActualiza.StrCertClave = empresa.StrCertClave;
					//Si la fecha de vencimiento esta null y la clave es distinta de null, entonces buscamos la fecha de vencimiento del certificado.
					if (empresa.DatCertVence == null && !string.IsNullOrEmpty(empresa.StrCertClave))
					{
						try
						{
							//Buscamos la fecha de vencimiento del certificado
							var datos = ObtenerInfCert(EmpresaActualiza.StrIdSeguridad, empresa.StrCertClave, (int)empresa.IntCertProveedor);
							EmpresaActualiza.DatCertVence = datos.Fechavenc;
						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion);
						}
					}
					else
					{
						//Si no es null, entonces guardamos la fecha de vencimiento.
						EmpresaActualiza.DatCertVence = empresa.DatCertVence;
					}

				}

				EmpresaActualiza.IntRadian = empresa.IntRadian;

				if (EmpresaActualiza.IntAcuseTacito == null)
					EmpresaActualiza.IntAcuseTacito = 0;

				//Se ajusta si tiene Radian y no las horas minimas de acuse, le agrega las horas que se ingresan en vista
				EmpresaActualiza.IntAcuseTacito = (EmpresaActualiza.IntAcuseTacito < 72) ? empresa.IntAcuseTacito : EmpresaActualiza.IntAcuseTacito; //empresa.IntAcuseTacito;
				EmpresaActualiza.IntEnvioNominaMail = empresa.IntEnvioNominaMail;

				if (string.IsNullOrEmpty(empresa.StrMailAdmin))
					throw new ApplicationException("El Email Administrativo es obligatorio");

				EmpresaActualiza.StrMailAdmin = empresa.StrMailAdmin;
				EmpresaActualiza.StrMailRecepcion = (string.IsNullOrEmpty(empresa.StrMailRecepcion)) ? string.Empty : empresa.StrMailRecepcion;
				EmpresaActualiza.StrMailEnvio = (string.IsNullOrEmpty(empresa.StrMailEnvio)) ? string.Empty : empresa.StrMailEnvio;
				EmpresaActualiza.StrMailAcuse = (string.IsNullOrEmpty(empresa.StrMailAcuse)) ? string.Empty : empresa.StrMailAcuse;
				EmpresaActualiza.StrMailPagos = (string.IsNullOrEmpty(empresa.StrMailPagos)) ? string.Empty : empresa.StrMailPagos;
				EmpresaActualiza.DatFechaActualizacion = Fecha.GetFecha();
				EmpresaActualiza.StrTelefono = empresa.StrTelefono;

				//Si la empresa no maneja pagos, entonces no puede permitir que consulten los documentos
				if (!empresa.IntManejaPagoE)
				{
					EmpresaActualiza.IntPagosPermiteConsTodos = false;
				}

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

						clase_usuario.ValidarPermisosUsuario(EmpresaActualiza, UsuarioBd);
					}
					catch (Exception excepcion)
					{
						Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion);
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


				if (crear_permiso_nomina)
				{
					CrearPermisosNomina(EmpresaActualiza.StrIdentificacion);
				}

				return EmpresaActualiza;
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion);
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
						Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.envio);
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
				Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion);
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

			if (empresa.Email.Contains(";"))
			{
				foreach (var item_mail in Coleccion.ConvertirLista(empresa.Email, ';'))
				{
					// recibe el email el adquiriente
					tbl_empresa.StrMailAdmin = item_mail;
					break;
				}
			}
			else
			{
				tbl_empresa.StrMailAdmin = empresa.Email;
			}

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
			tbl_empresa.IntAcuseTacito = 0;

			tbl_empresa.StrMailAcuse = String.Empty;
			tbl_empresa.StrMailEnvio = String.Empty;
			tbl_empresa.StrMailRecepcion = String.Empty;
			tbl_empresa.StrMailPagos = String.Empty;

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
			empresa_obj.CodigoTributo = "01";

			return empresa_obj;
		}

		public Tercero ConvertirTrabajador(Trabajador empleado)
		{
			if (empleado == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "empresa", "Ctl_Empresa"));

			Tercero empresa_obj = new Tercero();

			empresa_obj.Identificacion = empleado.Identificacion;
			empresa_obj.IdentificacionDv = 0;
			empresa_obj.TipoIdentificacion = Convert.ToInt16(empleado.TipoDocumento);
			empresa_obj.RazonSocial = string.Format("{0} {1} {2} {3}", empleado.PrimerApellido, empleado.SegundoApellido, empleado.PrimerNombre, empleado.OtrosNombres);
			empresa_obj.Email = empleado.Email;
			empresa_obj.Telefono = empleado.Telefono;
			empresa_obj.CodigoTributo = "01";

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
			context.Configuration.LazyLoadingEnabled = false;
			List<TblEmpresas> datos = (from item in context.TblEmpresas.AsNoTracking()
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
		public object ObtenerMisFacturadores(string indentificacion)
		{

			context.Configuration.LazyLoadingEnabled = false;

			var datos = (from d in context.TblDocumentos
						 where d.StrEmpresaAdquiriente.Equals(indentificacion)
						 group d by new { d.StrEmpresaFacturador } into hh
						 select new
						 {
							 hh.Key.StrEmpresaFacturador,
						 }).ToList();

			List<Facturador> lista = new List<Facturador>();

			Facturador Fact = new Facturador();

			foreach (var item in datos)
			{

				try
				{
					Fact = new Facturador();
					Fact.Identificacion = item.StrEmpresaFacturador;
					Fact.RazonSocial = Obtener(item.StrEmpresaFacturador, false).StrRazonSocial;
					lista.Add(Fact);
				}
				catch (Exception)
				{
				}
			}


			return lista;
		}


		public class Facturador
		{
			public string Identificacion { get; set; }
			public string RazonSocial { get; set; }
		}


		/// <summary>
		/// Obtiene Todas las empresas
		/// </summary>        
		/// /// <param name="IdSeguridad">id de seguridad</param>
		/// <returns></returns>
		public List<TblEmpresas> Obtener(System.Guid IdSeguridad, bool LazyLoading = true)
		{

			context.Configuration.LazyLoadingEnabled = LazyLoading;
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
		public CertificadoDigital ObtenerInfCert(System.Guid IdSeguridad, string clave, int certificadora)
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

				//******Se debe agregar la validacion del certificado vs certificadora7
				if (certificadora == EnumCertificadoras.Gse.GetHashCode())
				{
					List<string> list_subject = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(Certificado.Subject, ',');
					foreach (string item in list_subject)
					{
						if (item.Contains("CN="))
						{
							Datos.Propietario = item.Substring(4);
						}

					}
				}
				else if (certificadora == EnumCertificadoras.Certicamara.GetHashCode())
				{
					List<string> list_subject = LibreriaGlobalHGInet.Formato.Coleccion.ConvertirLista(Certificado.Subject, ',');
					foreach (string item in list_subject)
					{
						if (item.Contains("CN="))
						{
							Datos.Propietario = item.Substring(3);
						}

					}
				}
				else
				{
					Datos.Propietario = Certificado.FriendlyName;
				}

				Datos.Fechavenc = Certificado.NotAfter;

				//Datos.Propietario = Certificado.FriendlyName;

				Datos.Serial = Certificado.SerialNumber;

				Datos.Certificadora = Certificado.Issuer;

				return Datos;
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta);
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

			context.Configuration.LazyLoadingEnabled = false;

			List<TblEmpresas> ListaEmpresas = (from empresas in context.TblEmpresas.AsNoTracking()
											   where empresas.IntIdEstado == 1
											   && empresas.IntCobroPostPago == 1
											   && empresas.IntObligado == true
											   select empresas).ToList();

			return ListaEmpresas;
		}

		#endregion

		#region Filtros

		//public object ObtenerAdquirientes(string identificacion)
		//{

		//	var datos = (from item in context.TblDocumentos
		//				 where item.TblEmpresasAdquiriente.IntAdquiriente == true
		//				 group item by new { item.StrEmpresaAdquiriente } into Adquirientes
		//				 select new
		//				 {
		//					 ID = Adquirientes.FirstOrDefault().StrEmpresaAdquiriente,
		//					 Texto = Adquirientes.FirstOrDefault().TblEmpresasAdquiriente.StrRazonSocial,
		//					 Fact = Adquirientes.FirstOrDefault().TblEmpresasFacturador.StrIdentificacion
		//				 }).Where(x => x.Fact.Equals(identificacion)).OrderBy(x => x.Texto).ToList();

		//	return datos;
		//}

		/// <summary>
		/// Obtiene los adquirientes de un Facturador
		/// </summary>
		/// <param name="identificacion"></param>
		/// <returns></returns>
		public object ObtenerAdquirientes(string identificacion)
		{

			var datos = (from Adquirientes in context.QryAdquirientes
						 where Adquirientes.StrEmpresaFacturador.Equals(identificacion)
						 select new
						 {
							 ID = Adquirientes.StrEmpresaAdquiriente,
							 Texto = Adquirientes.StrRazonSocial,
							 Fact = Adquirientes.StrEmpresaFacturador
						 }).OrderBy(x => x.Texto).ToList();

			return datos;
		}

		/// <summary>
		/// Obtiene los empleados de un Emisor
		/// </summary>
		/// <param name="identificacion"></param>
		/// <returns></returns>
		public object ObtenerEmpleados(string identificacion)
		{

			var datos = (from Adquirientes in context.QryEmpleados
						 where Adquirientes.StrEmpresaFacturador.Equals(identificacion)
						 select new
						 {
							 ID = Adquirientes.StrEmpresaAdquiriente,
							 Texto = Adquirientes.StrRazonSocial,
							 Fact = Adquirientes.StrEmpresaFacturador
						 }).OrderBy(x => x.Texto).ToList();

			return datos;
		}




		/// <summary>
		/// Obtiene todos los adquirientes de tblempresas que sean adquirientes
		/// </summary>
		/// <returns></returns>
		public object ObtenerTodosAdquirientes()
		{

			var datos = (from item in context.TblEmpresas
						 where item.IntAdquiriente == true
						 select new
						 {
							 ID = item.StrIdentificacion,
							 Texto = item.StrRazonSocial
						 }).OrderBy(x => x.Texto).ToList();

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

				//Obtiene correo de la tabla que tiene registrado en la DIAN
				Ctl_ObtenerCorreos correo_dian = new Ctl_ObtenerCorreos();
				datos_retorno.EmailRecepcionDian = correo_dian.Obtener(datos_empresa.StrIdentificacion);

				return datos_retorno;
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna);
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



		#region Empresas con paginación
		/// <summary>
		/// Obtiene las Primeras 20 empresas
		/// </summary>        
		/// <returns></returns>
		public List<ObjEmpresa> Pag_ObtenerEmpresas(int Desde, int Hasta, int Tipo, string Nit, string razon_social)
		{

			bool Obligado = false;
			string strObligado = "";
			bool Adquiriente = false;
			string StrAdquiriente = "";
			//Facturador
			if (Tipo == 0)
			{
				Obligado = true;
				StrAdquiriente = "*";
			}
			//Adquiriente
			if (Tipo == 1)
			{
				Adquiriente = true;
				strObligado = "*";
			}
			//Todos
			if (Tipo == 2)
			{
				strObligado = "*";
				StrAdquiriente = "*";
			}

			if (string.IsNullOrEmpty(Nit))
			{
				Nit = "*";
			}

			if (string.IsNullOrEmpty(razon_social))
			{
				razon_social = "*";
			}


			List<ObjEmpresa> datos = (from d in context.TblEmpresas
									  where (d.IntObligado == Obligado || strObligado.Equals("*"))
									  && (d.IntAdquiriente == Adquiriente || StrAdquiriente.Equals("*"))
									  && (d.StrIdentificacion == Nit || Nit.Equals("*"))
									  && (d.StrRazonSocial.Contains(razon_social) || razon_social.Equals("*"))
									  select new ObjEmpresa
									  {
										  Identificacion = d.StrIdentificacion,
										  RazonSocial = d.StrRazonSocial,
										  Email = d.StrMailAdmin,
										  Serial = d.StrSerial,
										  Perfil = d.IntAdquiriente && d.IntObligado ? "Facturador y Adquiriente" : d.IntAdquiriente ? "Adquiriente" : d.IntObligado ? "Facturador" : "",
										  Habilitacion = d.IntHabilitacion.ToString(),
										  IdSeguridad = d.StrIdSeguridad,
										  Asociado = d.StrEmpresaAsociada,
										  EmpresaDescuento = d.StrEmpresaDescuento,
										  Estado = d.IntIdEstado,
										  Postpago = (d.IntCobroPostPago == 1) ? "SI" : "NO",
										  Nusuaurios = d.IntNumUsuarios,
										  HorasAcuse = (d.IntAcuseTacito > 0) ? d.IntAcuseTacito.ToString() : "NO",
										  NotificacionMail = (d.IntEnvioMailRecepcion) ? "SI" : "NO",
										  StrMailEnvio = d.StrMailEnvio,
										  StrMailPagos = d.StrMailPagos,
										  StrMailRecepcion = d.StrMailRecepcion,
										  StrMailAcuse = d.StrMailAcuse
									  }).OrderBy(x => x.Identificacion).Skip(Desde).Take(Hasta).ToList();

			return datos;
		}


		/// <summary>
		/// Obtiene Todas las empresas
		/// </summary>        
		/// <returns></returns>
		public List<ObjEmpresa> Pag_ObtenerTodas()
		{
			List<ObjEmpresa> datos = (from d in context.TblEmpresas
									  select new ObjEmpresa
									  {
										  Identificacion = d.StrIdentificacion,
										  RazonSocial = d.StrRazonSocial,
										  Email = d.StrMailAdmin,
										  Serial = d.StrSerial,
										  Perfil = d.IntAdquiriente && d.IntObligado ? "Facturador y Adquiriente" : d.IntAdquiriente ? "Adquiriente" : d.IntObligado ? "Facturador" : "",
										  Habilitacion = d.IntHabilitacion.ToString(),
										  IdSeguridad = d.StrIdSeguridad,
										  Asociado = d.StrEmpresaAsociada,
										  EmpresaDescuento = d.StrEmpresaDescuento,
										  Estado = d.IntIdEstado,
										  Postpago = (d.IntCobroPostPago == 1) ? "SI" : "NO",
										  Nusuaurios = d.IntNumUsuarios,
										  HorasAcuse = (d.IntAcuseTacito > 0) ? d.IntAcuseTacito.ToString() : "NO",
										  NotificacionMail = (d.IntEnvioMailRecepcion) ? "SI" : "NO",
										  StrMailEnvio = d.StrMailEnvio,
										  StrMailPagos = d.StrMailPagos,
										  StrMailRecepcion = d.StrMailRecepcion,
										  StrMailAcuse = d.StrMailAcuse
									  }).ToList();

			return datos;
		}

		/// <summary>
		/// Obtiene todas las empresa asociadas al facturador
		/// </summary>
		/// <param name="identificacion">Identificacion de Obligado o Adquiriente</param>
		/// <returns></returns>
		public List<ObjEmpresa> Pag_ObtenerAsociadas(string identificacion, int Desde, int Hasta, string Nit, string razon_social)
		{
			if (string.IsNullOrEmpty(Nit))
			{
				Nit = "*";
			}

			if (string.IsNullOrEmpty(razon_social))
			{
				razon_social = "*";
			}

			List<ObjEmpresa> datos = (from d in context.TblEmpresas
									  where d.StrIdentificacion.Equals(identificacion) || d.StrEmpresaAsociada.Equals(identificacion)
									   && (d.StrIdentificacion == Nit || Nit.Equals("*"))
									  && (d.StrRazonSocial.Contains(razon_social) || razon_social.Equals("*"))
									  select new ObjEmpresa
									  {
										  Identificacion = d.StrIdentificacion,
										  RazonSocial = d.StrRazonSocial,
										  Email = d.StrMailAdmin,
										  Serial = d.StrSerial,
										  Perfil = d.IntAdquiriente && d.IntObligado ? "Facturador y Adquiriente" : d.IntAdquiriente ? "Adquiriente" : d.IntObligado ? "Facturador" : "",
										  Habilitacion = d.IntHabilitacion.ToString(),
										  IdSeguridad = d.StrIdSeguridad,
										  Asociado = d.StrEmpresaAsociada,
										  EmpresaDescuento = d.StrEmpresaDescuento,
										  Estado = d.IntIdEstado,
										  Postpago = (d.IntCobroPostPago == 1) ? "SI" : "NO",
										  Nusuaurios = d.IntNumUsuarios,
										  HorasAcuse = (d.IntAcuseTacito > 0) ? d.IntAcuseTacito.ToString() : "NO",
										  NotificacionMail = (d.IntEnvioMailRecepcion) ? "SI" : "NO",
										  StrMailEnvio = d.StrMailEnvio,
										  StrMailPagos = d.StrMailPagos,
										  StrMailRecepcion = d.StrMailRecepcion,
										  StrMailAcuse = d.StrMailAcuse
									  }).OrderBy(x => x.Identificacion).Skip(Desde).Take(Hasta).ToList();

			return datos;
		}
		#endregion


		/// <summary>
		/// Obtiene todas las empresa asociadas al facturador
		/// </summary>
		/// <param name="identificacion">Identificacion de Obligado o Adquiriente</param>
		/// <returns></returns>
		public List<TblEmpresas> ObtenerEmpresaAcuse()
		{
			context.Configuration.LazyLoadingEnabled = false;

			var datos = (from item in context.TblEmpresas
						 where item.IntAcuseTacito >= 72
						 select item).ToList();

			return datos;
		}

		#region Configuracion de Comercios
		public TblEmpresas EditarConfigPago(Guid Stridseguridad, bool Permitepagosparciales, string IdComercio, string DescripcionComercio)
		{
			try
			{
				TblEmpresas tbl = context.TblEmpresas.Where(x => x.StrIdSeguridad == Stridseguridad).FirstOrDefault();

				tbl.IntPagoEParcial = Permitepagosparciales;
				try
				{
					tbl.ComercioConfigId = null;
					tbl.ComercioConfigId = Guid.Parse(IdComercio);
				}
				catch (Exception)
				{
				}
				tbl.ComercioConfigDescrip = DescripcionComercio;

				this.Edit(tbl);

				return tbl;
			}
			catch (Exception e)
			{
				Ctl_Log.Guardar(e, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.actualizacion, "");
				throw;
			}
		}

		public TblEmpresas EditarCamposDian(Guid StrIdseguridad, decimal posicion_x, decimal posicion_y)
		{
			try
			{
				TblEmpresas tbl_empresa = Obtener(StrIdseguridad, false).FirstOrDefault();

				tbl_empresa.IntPdfCampoDian = true;
				try
				{
					tbl_empresa.IntPdfCampoDianPosX = posicion_x;
					tbl_empresa.IntPdfCampoDianPosY = posicion_y;
				}
				catch (Exception)
				{
				}

				this.Edit(tbl_empresa);

				return tbl_empresa;
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.ninguna);
				throw new ApplicationException(string.Format("Error : {0}", excepcion.Message), excepcion);
			}
		}


		public string ObtenerSerialCloud(string Tercero)
		{

			PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
			var url = plataforma.RutaHginetMail;

			ClienteRest<string> cliente = new ClienteRest<string>(string.Format("{0}/Api/ObtenerSerialCloud?tercero={1}", url, Tercero), TipoContenido.Applicationjson.GetHashCode(), "");
			try
			{
				string data = cliente.GET();
				return data;
			}
			catch (Exception ex)
			{
				var cod = cliente.CodHttp;
				throw ex;
			}

		}

		#endregion

		/// <summary>
		/// Metodo para crear o actualizar un empresa desde el ERP de HGI
		/// </summary>
		/// <param name="empresa"></param>
		/// <returns></returns>
		public TblEmpresas ConvertirEmpresaErP(Empresa empresa)
		{
			//Obtiene los datos de la empresa.
			TblEmpresas tbl_empresa = Obtener(empresa.Identificacion);

			List<ObjVerificacionEmail> ListaEmailRegistro = new List<ObjVerificacionEmail>();

			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			//Valida si se obtuvieron datos y convierte la tbl a Empresa.
			if (tbl_empresa == null)
			{
				
				tbl_empresa = new TblEmpresas();

				tbl_empresa.StrTipoIdentificacion = empresa.TipoIdentificacion.ToString();
				tbl_empresa.StrIdentificacion = empresa.Identificacion;
				tbl_empresa.IntIdentificacionDv = Convert.ToInt16(empresa.IdentificacionDv);
				tbl_empresa.StrRazonSocial = empresa.RazonSocial;
				tbl_empresa.StrTelefono = empresa.Telefono;

				tbl_empresa.IntAdquiriente = true;
				tbl_empresa.IntObligado = true;

				tbl_empresa.StrSerial = empresa.PinSoftware;
				tbl_empresa.IntCertFirma = 0;
				tbl_empresa.IntAcuseTacito = 0;

				tbl_empresa.IntIdEstado = Convert.ToInt16(EstadoEmpresa.ACTIVA.GetHashCode());
				tbl_empresa.IntVersionDian = 2;
				
				tbl_empresa.StrEmpresaAsociada = empresa.Identificacion;
				tbl_empresa.StrEmpresaDescuento = empresa.Identificacion;
				tbl_empresa.IntNumUsuarios = 3;

				tbl_empresa.StrMailAdmin = empresa.EmailAdmin;
				tbl_empresa.StrMailAcuse = empresa.EmailAdmin;
				tbl_empresa.StrMailEnvio = empresa.EmailAdmin;
				tbl_empresa.StrMailRecepcion = empresa.EmailAdmin;
				tbl_empresa.StrMailPagos = empresa.EmailAdmin;

				ListaEmailRegistro.Add(new ObjVerificacionEmail { email = empresa.EmailAdmin });
				
				if (!plataforma_datos.RutaPublica.Contains("habilitacion") && !plataforma_datos.RutaPublica.Contains("localhost"))
				{
					tbl_empresa.IntHabilitacion = Convert.ToByte(Habilitacion.Valida_Objeto.GetHashCode());
				}
				else
				{
					tbl_empresa.IntHabilitacion = empresa.FacturaE == true ? Convert.ToByte(Habilitacion.Pruebas.GetHashCode()) : Convert.ToByte(Habilitacion.Valida_Objeto.GetHashCode());
					tbl_empresa.IntHabilitacionNomina = empresa.NominaE == true ? Convert.ToByte(Habilitacion.Pruebas.GetHashCode()) : tbl_empresa.IntHabilitacionNomina;
					tbl_empresa.IntTipoPlan = 1;
					tbl_empresa.IntCompraPlan = false;
					
				}

				tbl_empresa = Guardar(tbl_empresa, ListaEmailRegistro);

				if ((empresa.FacturaE == true || empresa.NominaE == true) && (plataforma_datos.RutaPublica.Contains("habilitacion") || plataforma_datos.RutaPublica.Contains("localhost")))
				{
					try
					{
						Ctl_PlanesTransacciones ctl_plan = new Ctl_PlanesTransacciones();
						TblPlanesTransacciones plan_pruebas = new TblPlanesTransacciones();

						plan_pruebas.IntMesesVence = 12;
						plan_pruebas.IntEstado = EstadoPlan.Habilitado.GetHashCode();
						plan_pruebas.IntNumTransaccCompra = 100;
						plan_pruebas.IntTipoDocumento = TipoDocPlanes.Mixto.GetHashCode();
						plan_pruebas.IntTipoProceso = Convert.ToByte(TipoCompra.Cortesia.GetHashCode());
						plan_pruebas.StrEmpresaFacturador = tbl_empresa.StrIdentificacion;
						plan_pruebas.StrUsuario = empresa.Identificacion_EmpresaEmisor;
						plan_pruebas.StrEmpresaUsuario = empresa.Identificacion_EmpresaEmisor;

						ctl_plan.Crear(plan_pruebas, true);
					}
					catch (Exception excepcion)
					{
						Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, "Error Creando el Plan de documentos para las pruebas");
					}
					
				}


			}
			else
			{
				tbl_empresa.IntObligado = (tbl_empresa.IntObligado == false) ? true : tbl_empresa.IntObligado;
				tbl_empresa.IntVersionDian = 2;
				
				if (plataforma_datos.RutaPublica.Contains("habilitacion") || plataforma_datos.RutaPublica.Contains("localhost"))
				{
					if (!tbl_empresa.StrMailAdmin.Equals(empresa.EmailAdmin))
					{
						tbl_empresa.StrMailAdmin = empresa.EmailAdmin;
						ListaEmailRegistro.Add(new ObjVerificacionEmail { email = empresa.EmailAdmin });
					}

					bool crear_plan = false;

					if (empresa.NominaE == true && (tbl_empresa.IntHabilitacionNomina == null || tbl_empresa.IntHabilitacionNomina == Convert.ToByte(Habilitacion.Valida_Objeto.GetHashCode())))
						crear_plan = true;

					if (empresa.FacturaE == true && (tbl_empresa.IntHabilitacion == null || tbl_empresa.IntHabilitacion == Convert.ToByte(Habilitacion.Valida_Objeto.GetHashCode())))
						crear_plan = true;

					tbl_empresa.IntHabilitacion = (empresa.FacturaE == true && tbl_empresa.IntHabilitacion == Convert.ToByte(Habilitacion.Valida_Objeto.GetHashCode())) ? Convert.ToByte(Habilitacion.Pruebas.GetHashCode()) : tbl_empresa.IntHabilitacion;
					tbl_empresa.IntHabilitacionNomina = (empresa.NominaE == true && (tbl_empresa.IntHabilitacionNomina == null || tbl_empresa.IntHabilitacionNomina == Convert.ToByte(Habilitacion.Valida_Objeto.GetHashCode()))) ? Convert.ToByte(Habilitacion.Pruebas.GetHashCode()) : tbl_empresa.IntHabilitacionNomina;

					tbl_empresa = Editar(tbl_empresa, true, ListaEmailRegistro);

					Ctl_PlanesTransacciones ctl_plan = new Ctl_PlanesTransacciones();
					List<TblPlanesTransacciones> list_plan = ctl_plan.Obtener(empresa.Identificacion, TipoCompra.Cortesia.GetHashCode().ToString(), EstadoPlan.Habilitado.GetHashCode().ToString(), 1, Fecha.GetFecha().AddYears(-1), Fecha.GetFecha().AddDays(1));

					TblPlanesTransacciones plan_prueba_activo = list_plan.Where(x => x.IntEstado == 0 && x.DatFechaVencimiento >= Fecha.GetFecha().AddMonths(1)).FirstOrDefault();

					if (crear_plan == true || plan_prueba_activo == null)
					{
						try
						{
							TblPlanesTransacciones plan_pruebas = new TblPlanesTransacciones();

							plan_pruebas.IntMesesVence = 12;
							plan_pruebas.IntEstado = EstadoPlan.Habilitado.GetHashCode();
							plan_pruebas.IntNumTransaccCompra = 100;
							plan_pruebas.IntTipoDocumento = TipoDocPlanes.Mixto.GetHashCode();
							plan_pruebas.IntTipoProceso = Convert.ToByte(TipoCompra.Cortesia.GetHashCode());
							plan_pruebas.StrEmpresaFacturador = tbl_empresa.StrIdentificacion;
							plan_pruebas.StrUsuario = empresa.Identificacion_EmpresaEmisor;
							plan_pruebas.StrEmpresaUsuario = empresa.Identificacion_EmpresaEmisor;

							ctl_plan.Crear(plan_pruebas, true);
						}
						catch (Exception excepcion)
						{
							Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion, "Error Creando el Plan de documentos actualizando la empresa para las pruebas");
						}
					}
				}
				else
				{
					tbl_empresa.IntHabilitacion = (empresa.FacturaE == true && (tbl_empresa.IntHabilitacion == null || tbl_empresa.IntHabilitacion == Convert.ToByte(Habilitacion.Valida_Objeto.GetHashCode()))) ? Convert.ToByte(Habilitacion.Produccion.GetHashCode()) : tbl_empresa.IntHabilitacion;
					tbl_empresa.IntHabilitacionNomina = (empresa.NominaE == true && (tbl_empresa.IntHabilitacionNomina == null || tbl_empresa.IntHabilitacionNomina == Convert.ToByte(Habilitacion.Valida_Objeto.GetHashCode()))) ? Convert.ToByte(Habilitacion.Produccion.GetHashCode()) : tbl_empresa.IntHabilitacionNomina;
					tbl_empresa.IntNumUsuarios = (tbl_empresa.IntNumUsuarios < 3) ? 3 : tbl_empresa.IntNumUsuarios;

					ListaEmailRegistro.Add(new ObjVerificacionEmail { email = tbl_empresa.StrMailAdmin });

					tbl_empresa = Editar(tbl_empresa, true, ListaEmailRegistro);
				}

			}

			return tbl_empresa;
		}

	}
}
