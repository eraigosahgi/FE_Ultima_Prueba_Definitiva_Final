using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
    public class Ctl_MaestrosEnum
    {
        public static List<string[]> ListaEnum(int tipo_enum,string tipo_ambiente = "*")
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
                    case 2:
                        //Metodo para obtener los datos del enumerable de tipo de documento
                        foreach (var value in Enum.GetValues(typeof(LibreriaGlobalHGInet.Objetos.TipoDocumento)))
                        {
                            string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<LibreriaGlobalHGInet.Objetos.TipoDocumento>((int)value)))).Split(',');

                            datos.Add(datos_enum);
                        }
                        break;
                }

                
                return (datos);
               
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

    }
}
