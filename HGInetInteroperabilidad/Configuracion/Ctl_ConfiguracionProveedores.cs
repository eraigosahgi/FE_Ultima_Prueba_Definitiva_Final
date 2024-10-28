using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetInteroperabilidad.Configuracion
{
    public class Ctl_ConfiguracionProveedores
    {


        //Validacion de inicio de session del proveedor tecnologico
        public TblConfiguracionInteroperabilidad CheckUser(string username, string password)
        {

            Ctl_ConfiguracionInteroperabilidad ValidarUsuario = new Ctl_ConfiguracionInteroperabilidad();
            TblConfiguracionInteroperabilidad usuarioValido = ValidarUsuario.Validar(username, password);

            return usuarioValido;

        }


    }
}
