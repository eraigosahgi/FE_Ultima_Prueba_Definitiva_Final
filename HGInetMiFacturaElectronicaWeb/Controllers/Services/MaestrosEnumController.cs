using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class MaestrosEnumController : ApiController
    {

        public IHttpActionResult Get(int tipo_enum)
        {
            try
            {
                List<string[]> datos = new List<string[]>();

                switch (tipo_enum)
                {
                    case 0:
                        foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.ProcesoEstado)))
                        {
                            string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>((int)value)))).Split(',');

                            datos.Add(datos_enum);
                        }
                        break;
                    case 1:
                        foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.AdquirienteRecibo)))
                        {
                            string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.AdquirienteRecibo>((int)value)))).Split(',');

                            datos.Add(datos_enum);
                        }
                        break;
                }

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
