using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.PagosElectronicos
{
    public class Ctl_EmpresasPasarela : BaseObject<TblEmpresasPasarela>
    {
        /// <summary>
        /// Obtiene los datos de la pasarela por identificación de la empresa (obligado) y id del comercio.
        /// </summary>
        /// <param name="identificacion_empresa"></param>
        /// <param name="id_comercio"></param>
        /// <returns></returns>
        public TblEmpresasPasarela Obtener(string identificacion_empresa, int id_comercio)
        {
            try
            {
                TblEmpresasPasarela datos_pasarela = (from pasarela in context.TblEmpresasPasarela
                                                      where pasarela.StrEmpresa.Equals(identificacion_empresa)
                                                      && pasarela.IntComercioId == id_comercio
                                                      select pasarela).FirstOrDefault();

                return datos_pasarela;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

    }
}
