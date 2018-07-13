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
                                 select item).FirstOrDefault();

            if (datos != null)
            {

                string datakey_construido = datos.StrSerial.ToString() + datos.StrIdentificacion.ToString();
                string datakey_encriptado = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA1(datakey_construido);

                string datakey_construido_may = datos.StrSerial.ToString().ToUpper() + datos.StrIdentificacion.ToString();
                string datakey_encriptado_may = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA1(datakey_construido_may);


                if (datakey_encriptado.Equals(datakey) || datakey_encriptado_may.Equals(datakey))
                    return datos;

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
        /// Crea una empresa en la BD
        /// </summary>
        /// <param name="empresa">Objeto BD de la empresa a crear</param>
        /// <returns></returns>
        public TblEmpresas Guardar(TblEmpresas empresa)
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

                if (String.IsNullOrEmpty(empresa.StrMail))
                    throw new ApplicationException("Debe ingresar el Email");

                if (empresa.IntAdquiriente == false && empresa.IntObligado == false)
                    throw new ApplicationException("Debe Indicar el Perfil");


                empresa.IntIdentificacionDv = FuncionesIdentificacion.Dv(empresa.StrIdentificacion);

                //Automaricos                
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

				bool Enviarmail = Email.Bienvenida(empresa, UsuarioBd);

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
        public TblEmpresas Editar(TblEmpresas empresa)
        {
            try
            {

                if (empresa == null)
                    throw new ApplicationException("La empresa es incorrecta!, Error");

                //if (String.IsNullOrEmpty(empresa.StrTipoIdentificacion))
                //    throw new ApplicationException("Debe ingresar el Tipo de Identificación");

                //if (String.IsNullOrEmpty(empresa.StrIdentificacion))
                //    throw new ApplicationException("Debe ingresar el Numero de Identificación");

                //if (String.IsNullOrEmpty(empresa.StrRazonSocial))
                //    throw new ApplicationException("Debe ingresar La Razón Social");

                //if (String.IsNullOrEmpty(empresa.StrMail))
                //    throw new ApplicationException("Debe ingresar el Email");

                //if (empresa.IntAdquiriente == false && empresa.IntObligado == false)
                //    throw new ApplicationException("Debe Indicar el Perfil");

                TblEmpresas EmpresaActualiza = (from item in context.TblEmpresas
                                                where item.StrIdentificacion.Equals(empresa.StrIdentificacion)
                                                select item).FirstOrDefault();

                if (EmpresaActualiza == null)
                    throw new ApplicationException("La empresa que desea Actualizar no Existe");

                EmpresaActualiza.StrRazonSocial = empresa.StrRazonSocial;
                EmpresaActualiza.StrMail = empresa.StrMail;
                EmpresaActualiza.IntAdquiriente = empresa.IntAdquiriente;
                EmpresaActualiza.IntHabilitacion = empresa.IntHabilitacion;
                EmpresaActualiza.IntObligado = empresa.IntObligado;
                EmpresaActualiza.StrEmpresaAsociada = empresa.StrEmpresaAsociada;
                EmpresaActualiza.StrResolucionDian = empresa.StrResolucionDian;
                EmpresaActualiza.StrObservaciones = empresa.StrObservaciones;
                EmpresaActualiza.IntIntegrador = empresa.IntIntegrador;
                EmpresaActualiza.IntNumUsuarios = empresa.IntNumUsuarios;

                empresa.DatFechaActualizacion = Fecha.GetFecha();

                Actualizar(EmpresaActualiza);

                //Obtiene el usuario principal de la empresa.
                Ctl_Usuario clase_usuario = new Ctl_Usuario();
                TblUsuarios usuario_principal = clase_usuario.ObtenerUsuarios(EmpresaActualiza.StrIdentificacion, EmpresaActualiza.StrIdentificacion).FirstOrDefault();
                //Valida los permisos del usuario y los actualiza
                clase_usuario.ValidarPermisosUsuario(EmpresaActualiza, usuario_principal);

                return EmpresaActualiza;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }

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

                bool Enviarmail= Email.EnviaSerial(EmpresaActualiza.StrIdentificacion,EmpresaActualiza.StrMail);

                return EmpresaActualiza;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
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
        public TblEmpresas Crear(Tercero empresa)
        {
            // convierte el objeto del servicio
            TblEmpresas tbl_empresa = Convertir(empresa);

            // agrega el objeto
            tbl_empresa = Crear(tbl_empresa);

            return tbl_empresa;

        }

        /// <summary>
        /// Convierte un objeto de tercero a un objeto de Bd
        /// </summary>
        /// <param name="empresa">Informacion de la empresa</param>
        /// <returns></returns>
        public static TblEmpresas Convertir(Tercero empresa)
        {

            if (empresa == null)
                throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "empresa", "Ctl_Empresa"));

            TblEmpresas tbl_empresa = new TblEmpresas();

            tbl_empresa.StrTipoIdentificacion = empresa.TipoIdentificacion.ToString();
            tbl_empresa.StrIdentificacion = empresa.Identificacion;
            tbl_empresa.IntIdentificacionDv = Convert.ToInt16(empresa.IdentificacionDv);
            tbl_empresa.StrRazonSocial = empresa.RazonSocial;
            tbl_empresa.StrMail = empresa.Email;
            tbl_empresa.DatFechaIngreso = Fecha.GetFecha();
            tbl_empresa.IntAdquiriente = true;
            tbl_empresa.IntObligado = false;
			tbl_empresa.IntHabilitacion = 0;
            tbl_empresa.DatFechaActualizacion = Fecha.GetFecha();
            tbl_empresa.StrIdSeguridad = Guid.NewGuid();

            return tbl_empresa;
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
        /// Actualizar Empresa
        /// </summary>        
        /// /// <param name="TblEmpresa">Objeto empresa a actualizar en BD</param>
        /// <returns></returns>
        public TblEmpresas Actualizar(TblEmpresas empresa)
        {
            empresa = this.Edit(empresa);

            return empresa;

        }



    }
}
