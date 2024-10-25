using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.PagosElectronicos
{
	public class Ctl_PagosDetalles : BaseObject<TblPagosDetalles>
	{
		/// <summary>
		/// Crear detalle pagos
		/// </summary>
		/// <param name="datos_pago"></param>
		/// <returns></returns>
		public TblPagosDetalles Crear(TblPagosDetalles datos_pago)
		{
			try
			{
				datos_pago = this.Add(datos_pago);

				return datos_pago;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}




		/// <summary>
		/// Obtiene un detalle por documento
		/// </summary>
		/// <param name="documento"></param>
		/// <returns></returns>
		public TblPagosDetalles Obtener(Guid documento, Guid id_pago)
		{
			try
			{
				TblPagosDetalles datos = (from d in context.TblPagosDetalles
										  where d.StrIdSeguridadDoc == documento
										  && d.StrIdPagoPrincipal == id_pago
										  select d).FirstOrDefault();
				return datos;
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene un detalle por documento
		/// </summary>
		/// <param name="documento"></param>
		/// <returns></returns>
		public List<TblPagosDetalles> Obtener(Guid documento)
		{
			try
			{
				var datos = (from d in context.TblPagosDetalles
							 where d.StrIdSeguridadDoc == documento
							 //&& d.StrIdPagoPrincipal == id_pago
							 select d).ToList();
				return datos;
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}
