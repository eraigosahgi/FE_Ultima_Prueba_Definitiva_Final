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
        public bool Validar(string datakey, string identificacion_obligado)
        {


            var datos = (from item in context.TblEmpresas
                         where item.StrIdentificacion.Equals(identificacion_obligado)
                         select item).FirstOrDefault();

            if (datos != null)
            {
                string datakey_construido = datos.StrSerial.ToString() + datos.StrIdentificacion.ToString();
                string datakey_encriptado = LibreriaGlobalHGInet.General.Encriptar.Encriptar_SHA1(datakey_construido);

                if (datakey_encriptado == datakey)
                    return true;

            }
            return false;

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
        public TblEmpresas Crear(TblEmpresas empresa)
        {
            empresa = this.Add(empresa);

            return empresa;
        }

        /// <summary>
        /// Crea una empresa en la BD
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
            tbl_empresa.StrSerial = Guid.NewGuid();
            tbl_empresa.IntAdquiriente = true;
            tbl_empresa.IntObligado = false;
            tbl_empresa.DatFechaActualizacion = Fecha.GetFecha();
            tbl_empresa.StrIdSeguridad = Guid.NewGuid();

            return tbl_empresa;
        }
    }
}
