using HGInetMiFacturaElectonicaController;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class EmpresaResolucionController: ApiController
    {


        #region Get--Obtener
        /// <summary>
        /// Obtiene la lista de resoluciones de una empresa en especifica
        /// </summary>
        /// <param name="codigo_facturador"></param>        
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string codigo_facturador)
        {
            try
            {
                Sesion.ValidarSesion();

                Ctl_EmpresaResolucion Resolucion = new Ctl_EmpresaResolucion();

                List<TblEmpresasResoluciones> datos = Resolucion.ObtenerResoluciones(codigo_facturador,"*");

                var retorno = datos.Select(d => new
                {
                    ID =1,
                    Descripcion = d.StrNumResolucion
                });

                return Ok(retorno);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Obtiene la lista de resoluciones con prefijo para una empresa espeficifa
        /// </summary>
        /// <param name="codigo_facturador"></param>        
        /// <returns></returns>
        [HttpGet]
        [Route("Api/ObtenerResPrefijo")]
        public IHttpActionResult ObtenerResPrefijo(string codigo_facturador)
        {
            try
            {
                Sesion.ValidarSesion();

                Ctl_EmpresaResolucion Resolucion = new Ctl_EmpresaResolucion();

                List<TblEmpresasResoluciones> datos = Resolucion.ObtenerResoluciones(codigo_facturador, "*");

                var retorno = datos.Select(d => new
                {                    
                    ID = d.StrNumResolucion,
                    Descripcion = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<TipoDocumento>(d.IntTipoDoc)) + "-" + ((d.StrPrefijo==null)? "S / PREFIJO":(!d.StrPrefijo.Equals("")) ? d.StrPrefijo : "S/PREFIJO")  + ((d.IntTipoDoc==1)? "-" + d.StrNumResolucion :"") 
                });

                return Ok(retorno);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}

