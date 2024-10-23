using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController
{
    public class Ctl_Permisos : BaseObject<TblOpciones>
    {


        /// <summary>
        /// Obtiene la opción por código
        /// </summary>
        /// <param name="id_opcion"></param>
        /// <returns></returns>
        public TblOpciones ObtenerOpcion(int id_opcion)
        {
            try
            {
				context.Configuration.LazyLoadingEnabled = false;

                TblOpciones opcion = (from opc in context.TblOpciones
                                      where (opc.IntId == id_opcion)
                                      select opc).FirstOrDefault();

                return opcion;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene todas las opciones
        /// </summary>
        /// <returns></returns>
        public List<TblOpciones> ObtenerOpciones()
        {
            try
            {
				context.Configuration.LazyLoadingEnabled = false;

                List<TblOpciones> opciones = (from opc in context.TblOpciones
                                              select opc).ToList();

                return opciones;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Obtiene las opciones asociadas al permiso
        /// </summary>
        /// <param name="codigo_opcion"></param>
        /// <param name="incluir_opcion"></param>
        /// <param name="nivel"></param>
        /// <returns></returns>
        public List<TblOpciones> ObtenerAsociadas(int codigo_opcion, bool incluir_opcion, int nivel = -1)
        {
            int nivelActual = 0;

            // obtener las opciones de base de datos
            List<TblOpciones> opciones = ObtenerOpciones();

            if (!opciones.Any())
                return new List<TblOpciones>();

            List<TblOpciones> opcionesGrupo = new List<TblOpciones>();

            int codigo_tmp = Convert.ToInt32(codigo_opcion);

            while (!codigo_tmp.Equals("0"))
            {
                bool incluir = true;

                if (nivel != -1 && nivel <= nivelActual)
                    break;

                TblOpciones opcion = opciones.Where(_opcion => _opcion.IntId == codigo_tmp).FirstOrDefault();

                if (opcion == null)
                    break;

                if (!incluir_opcion && opcion.IntId.Equals(codigo_opcion))
                    incluir = false;

                if (!opcion.IntIdDependencia.Equals("0") && incluir)
                    opcionesGrupo.Add(opcion);

                codigo_tmp = opcion.IntIdDependencia.Value;
                nivelActual++;

                if (nivelActual == opciones.Count())
                    break;

                if (opcion.IntIdDependencia.Value == opcion.IntId)
                    break;
            }

            return opcionesGrupo.OrderBy(_opcion => _opcion.IntId).ToList();
        }

        /// <summary>
        /// Obtiene la ruta (migas de pan).
        /// </summary>
        /// <param name="codigo_opcion"></param>
        /// <param name="incluir_opcion"></param>
        /// <param name="nivel"></param>
        /// <param name="opciones_asociadas"></param>
        /// <returns></returns>
        public string ObtenerRutaGrupo(int codigo_opcion, bool incluir_opcion, int nivel = -1, List<TblOpciones> opciones_asociadas = null)
        {
            string ruta_grupo = "";

            // obtener las opciones de base de datos
            if (opciones_asociadas == null)
                opciones_asociadas = ObtenerAsociadas(codigo_opcion, incluir_opcion, nivel);

            if (!opciones_asociadas.Any())
                return "";

            foreach (TblOpciones item in opciones_asociadas)
            {
                bool incluir = true;

                if (!incluir_opcion && item.IntId.Equals(codigo_opcion))
                    incluir = false;

                if (incluir)
                {
                    if (string.IsNullOrWhiteSpace(ruta_grupo))
                        ruta_grupo = string.Format("{0}", item.StrDescripcion);
                    else
                        ruta_grupo = string.Format(" {0} / {1}", ruta_grupo, item.StrDescripcion);
                }
            }
            return ruta_grupo;
        }

        /// <summary>
        /// Obtiene las opciones dependientes de un código de permiso.
        /// </summary>
        /// <param name="codigo_opcion"></param>
        /// <returns></returns>
        public List<TblOpciones> ObtenerDependencias(int codigo_opcion)
        {
            try
            {
                List<TblOpciones> opciones = (from opc in context.TblOpciones
                                              where opc.IntIdDependencia == codigo_opcion
                                              select opc).ToList();

                return opciones;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }

        }

    }
}
