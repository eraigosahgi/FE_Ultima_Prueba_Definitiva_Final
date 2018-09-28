using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
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
		/// Valida Autenticacion del Proveedor Tecnológico
		/// </summary>
		/// <param name="usuario">usuario</param>
		/// <param name="clave">clave sin encripción</param>
		/// <returns>datos de configuración si es encontrado o null si no</returns>
		public TblConfiguracionInteroperabilidad Validar(string usuario, string clave)
		{
            // se encripta la clave en SHA256 para comparación con la base de datos
            //string clave_sha256 = Encriptar.Encriptar_SHA256(clave);
            string clave_sha256 = clave;
            TblConfiguracionInteroperabilidad datos = (from item in context.TblConfiguracionInteroperabilidad
													   where item.StrUsuario.Equals(usuario) && item.StrClave.Equals(clave_sha256)
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



        public bool GuardarToken(string Token)
        {
            return true;
        }



    }
}
