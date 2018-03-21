using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_EmpresaResolucion : BaseObject<TblEmpresasResoluciones>
	{
		#region Constructores 

		public Ctl_EmpresaResolucion() : base(new ModeloAutenticacion()) { }
		public Ctl_EmpresaResolucion(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_EmpresaResolucion(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		/// <summary>
		/// Obtiene la resolución de una empresa
		/// </summary>
		/// <param name="documento_empresa">documento de la empresa</param>
		/// <param name="numero_resolucion">número de resolución</param>
		/// <returns>datos de la resolución</returns>
		public TblEmpresasResoluciones Obtener(string documento_empresa, string numero_resolucion)
		{
			try
			{
				var datos = (from item in context.TblEmpresasResoluciones
							where item.StrNumResolucion.Equals(numero_resolucion)
							&& item.TblEmpresas.StrIdentificacion.Equals(documento_empresa)
							select item).FirstOrDefault();

				if (datos == null)
					throw new ApplicationException(string.Format("No se encontró el número de resolución {0} para el documento {1}.", numero_resolucion, documento_empresa));
								
				return datos;
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message);
			}
		}
	}
}
