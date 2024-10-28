﻿using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
    public class Ctl_ConfiguracionInteroperabilidad : BaseObject<TblConfiguracionInteroperabilidad>
    {
        #region Constructores 
        public Ctl_ConfiguracionInteroperabilidad() : base(new ModeloAutenticacion()) { }
        public Ctl_ConfiguracionInteroperabilidad(ModeloAutenticacion autenticacion) : base(autenticacion) { }
        public Ctl_ConfiguracionInteroperabilidad(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
        #endregion

        /// <summary>
        /// Obtiene la configuración con la identificacion del proveedor tecnológico
        /// </summary>
        /// <param name="identificacion">Identificacion del proveedor tecnológico</param>
        /// <returns>configuración</returns>
        public TblConfiguracionInteroperabilidad Obtener(string identificacion)
        {

            var datos = (from item in context.TblConfiguracionInteroperabilidad
                         where item.StrIdentificacion.Equals(identificacion)
                         select item).FirstOrDefault();

            return datos;
        }

        /// <summary>
        /// Obtiene la configuración con el id de seguridad del proveedor tecnológico
        /// </summary>
        /// <param name="identificacion">Identificacion del proveedor tecnológico</param>
        /// <returns>configuración</returns>
        public TblConfiguracionInteroperabilidad Obtener(Guid id_seguridad)
        {
            var datos = (from item in context.TblConfiguracionInteroperabilidad
                         where item.StrIdSeguridad.Equals(id_seguridad)
                         select item).FirstOrDefault();

            return datos;
        }

        /// <summary>
        /// Obtiene la lista de proveedores
        /// </summary>
        /// <param name="identificacion"></param>
        /// <returns></returns>
        public List<TblConfiguracionInteroperabilidad> ObtenerProveedores(string identificacion)
        {
            var datos = (from item in context.TblConfiguracionInteroperabilidad
                         where item.StrIdentificacion.Equals(identificacion) || identificacion.Equals("*")
                         select item).ToList();

            return datos;
        }


        /// <summary>
		/// Obtiene la lista de proveedores
		/// </summary>
		/// <param name="identificacion"></param>
		/// <returns></returns>
		public IEnumerable<TblConfiguracionInteroperabilidad> ObtenerProveedores(Guid identificacion)
        {
            var datos = (from item in context.TblConfiguracionInteroperabilidad
                         where item.StrIdSeguridad.Equals(identificacion)
                         select item).ToList();

            return datos;
        }


        /// <summary>
        /// Valida Autenticacion del Proveedor Tecnológico
        /// </summary>
        /// <param name="usuario">usuario</param>
        /// <param name="clave">clave sin encripción</param>
        /// <returns>datos de configuración si es encontrado o null si no</returns>
        public TblConfiguracionInteroperabilidad Validar(string usuario, string clave)
        {
            // se encripta la clave en SHA256 para comparación con la base de datos
            string clave_sha256 = clave;// Encriptar.Encriptar_SHA256(clave);
            TblConfiguracionInteroperabilidad datos = (from item in context.TblConfiguracionInteroperabilidad
                                                       where item.StrUsuario.Equals(usuario) && item.StrClave.Equals(clave_sha256)
                                                       && item.BitActivo == true
                                                       select item).FirstOrDefault();
            return datos;
        }



        /// <summary>
        /// Obtiene una lista de documentos pendientes para enviar
        /// Esto debe ir en el controlador de documentos.interoperabilidad
        /// </summary>
        /// <returns></returns>
		public List<TblDocumentos> ObtenerDocumentosProveedores(string IdentificacionProveedor)
        {
            int DocPendiente = ProcesoEstado.PendienteEnvioProveedorDoc.GetHashCode();
            int AcusePendiente = ProcesoEstado.PendienteEnvioProveedorAcuse.GetHashCode();

            List<TblDocumentos> Doc = (from doc in context.TblDocumentos
                                       where (doc.IntIdEstado == DocPendiente || doc.IntIdEstado == AcusePendiente)
                                       select doc).ToList();


            return Doc;
        }


        /// <summary>
        /// Actualiza el usuario en la base ded atos
        /// </summary>
        /// <param name="usuario">información del Usuario</param>
        /// <returns>información del usuario</returns>

        public bool GuardarToken(string Token)
        {
            return true;
        }



        /// <summary>
		/// Valida Autenticacion del Proveedor Tecnológico
		/// </summary>
		/// <param name="usuario">usuario</param>
		/// <param name="clave">clave sin encripción</param>
		/// <returns>datos de configuración si es encontrado o null si no</returns>
		public TblConfiguracionInteroperabilidad CambiarContraseña(string nit, string clave, string NuevaClave)
        {
            // se encripta la clave en SHA256 para comparación con la base de datos
            string clave_sha256 = clave;// Encriptar.Encriptar_SHA256(clave);
            TblConfiguracionInteroperabilidad datos = (from item in context.TblConfiguracionInteroperabilidad
                                                       where item.StrIdentificacion.Equals(nit) && item.StrClave.Equals(clave_sha256)
                                                       select item).FirstOrDefault();

            if (datos == null)
            {
                return null;
            }

            datos.StrClave = NuevaClave;// Encriptar.Encriptar_SHA256(NuevaClave);

            datos = this.Edit(datos);

            return datos;
        }
        /// <summary>
        /// Guarda un Proveedor tecnologio enviandole una lista de parametros
        /// </summary>
        /// <param name="StrIdentificacion"></param>
        /// <param name="StrIdSeguridad"></param>
        /// <param name="StrRazonSocial"></param>
        /// <param name="StrMail"></param>
        /// <param name="StrTelefono"></param>
        /// <param name="StrObservaciones"></param>
        /// <param name="StrUsuario"></param>
        /// <param name="StrClave"></param>
        /// <param name="StrHgiUsuario"></param>
        /// <param name="StrHgiClave"></param>
        /// <param name="StrUrlApi"></param>
        /// <param name="StrUrlFtp"></param>
        /// <param name="BitActivo"></param>
        /// <param name="FechaExpProveedor"></param>
        /// <param name="FechaExpHgi"></param>
        /// <returns></returns>
        public TblConfiguracionInteroperabilidad GuardarProveedor(string StrIdentificacion, string StrIdSeguridad, string StrRazonSocial, string StrMail, string StrTelefono, string StrObservaciones, string StrUsuario, string StrClave, string StrHgiUsuario, string StrHgiClave, string StrUrlApi, string StrUrlFtp, bool BitActivo, DateTime FechaExpProveedor, DateTime FechaExpHgi)
        {

            try
            {
                TblConfiguracionInteroperabilidad Proveedor = new TblConfiguracionInteroperabilidad();

                Proveedor.StrIdentificacion = StrIdentificacion;
                Proveedor.StrIdSeguridad = Guid.NewGuid();
                Proveedor.StrRazonSocial = StrRazonSocial;
                Proveedor.StrMail = (string.IsNullOrEmpty(StrMail)) ? "0" : StrMail;//StrMail;
				Proveedor.StrTelefono = (string.IsNullOrEmpty(StrTelefono)) ? "0" : StrTelefono;//StrTelefono;
				Proveedor.StrObservaciones = (string.IsNullOrEmpty(StrObservaciones))? string.Empty: StrObservaciones;
                Proveedor.StrUsuario = (string.IsNullOrEmpty(StrUsuario)) ? StrIdentificacion : StrUsuario;//StrUsuario;
				Proveedor.StrClave = (string.IsNullOrEmpty(StrClave)) ? Encriptar.Encriptar_SHA256(StrIdentificacion) : Encriptar.Encriptar_SHA256(StrClave);// Encriptar.Encriptar_SHA256(StrClave);
				Proveedor.StrHgiUsuario = (string.IsNullOrEmpty(StrUsuario)) ? StrIdentificacion : StrUsuario;//StrUsuario;
				Proveedor.StrHgiClave = Proveedor.StrClave;//StrClave;
				Proveedor.StrUrlApi = (string.IsNullOrEmpty(StrUrlApi)) ? Directorio.ValidarUrl("Ruta") : Directorio.ValidarUrl(StrUrlApi);//Directorio.ValidarUrl(StrUrlApi);
				Proveedor.StrUrlFtp = (string.IsNullOrEmpty(StrUsuario)) ? Proveedor.StrUrlApi : StrUrlFtp;//StrUrlFtp;
				Proveedor.BitActivo = BitActivo;
                Proveedor.DatFechaActualizacion = Fecha.GetFecha();
                Proveedor.DatFechaExpiracion = FechaExpProveedor;
                Proveedor.DatFechaIngreso = Fecha.GetFecha();
                Proveedor.DatHgiFechaToken = Fecha.GetFecha().AddDays(1);
                Proveedor.DatHgiFechaExpiracion = FechaExpHgi;

                Proveedor = this.Add(Proveedor);
                return Proveedor;
            }
            catch (Exception)
            {

                throw;
            }
        }  
        
        
        /// <summary>
        /// Edita un proveedor
        /// </summary>
        /// <param name="Proveedor">Objeto tblConfiguracion de interoperabilidad</param>
        /// <returns></returns>
        public TblConfiguracionInteroperabilidad Editar(TblConfiguracionInteroperabilidad Proveedor)
        {
            Proveedor = this.Edit(Proveedor);

            return Proveedor;
        }
    }
}
