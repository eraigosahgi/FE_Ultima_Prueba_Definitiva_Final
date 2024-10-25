using HGInetMiFacturaElectonicaController.Configuracion;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class MaestrosEnumController : ApiController
    {
        /// <summary>
        /// Retorna una lista de enumerables, segun el parametro: 
        /// tipo_enum : 0 - ProcesoEstado, 1 - AdquirienteRecibo
        /// Si es publico, se refleja en los filtros de documentos
        /// </summary>
        /// <param name="tipo_enum"></param>
        /// <param name="tipo_ambiente"></param>
        /// <returns></returns>
        public IHttpActionResult Get(int tipo_enum, string tipo_ambiente="*")
        {
            try
            {
                List<string[]> datos = Ctl_MaestrosEnum.ListaEnum(tipo_enum, tipo_ambiente);

                var retorno = datos.Select(d => new
                {
                    ID = d[0],
                    Descripcion = d[1]
                });

                return Ok(retorno);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

    }
}
