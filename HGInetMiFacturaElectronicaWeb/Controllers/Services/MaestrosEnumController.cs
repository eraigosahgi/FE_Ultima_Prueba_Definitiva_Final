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
                List<string[]> datos = new List<string[]>();

                switch (tipo_enum)
                {
                    case 0:
                        
                        foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.ProcesoEstado)))
                        {


                            FieldInfo fi = value.GetType().GetField(value.ToString());

                            AmbientValueAttribute[] ambiente = (AmbientValueAttribute[])fi.GetCustomAttributes(typeof(AmbientValueAttribute), false);
                            if (tipo_ambiente != "*")
                            {
                                if (ambiente[0].Value.ToString() == tipo_ambiente)
                                {
                                    string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>((int)value)))).Split(',');

                                    datos.Add(datos_enum);
                                }
                            }
                            else
                            {
                                string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>((int)value)))).Split(',');

                                datos.Add(datos_enum);
                            }
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
