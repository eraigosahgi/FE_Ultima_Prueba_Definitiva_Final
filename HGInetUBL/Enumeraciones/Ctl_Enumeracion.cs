using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBL
{
	/// <summary>
	/// Controlador de Enumeraciones definidas en el UBL
	/// como: Moneda, Unidad de Medida, etc.
	/// </summary>
	public class Ctl_Enumeracion
	{
		/// <summary>
		/// Obtiene la moneda por código de texto
		/// </summary>
		/// <param name="codigo">código texto</param>
		/// <returns>tipo de moneda</returns>
		public static CurrencyCodeContentType ObtenerMoneda(string codigo)
		{
			try
			{
				CurrencyCodeContentType enumerable = Enumeracion.ParseToEnum<CurrencyCodeContentType>(codigo);

				return enumerable;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(string.Format("Error al obtener la enumeración {0} con código {1}. Detalle: {2}", "CurrencyCodeContentType", codigo, excepcion.Message));
			}
		}

		/// <summary>
		/// Obtiene la unidad de medida del producto por código de texto
		/// http://www.datypic.com/sc/ubl20/t-clm66411_UnitCodeContentType.html
		/// </summary>
		/// <param name="codigo">código texto</param>
		/// <returns>tipo de moneda</returns>
		public static UnitCodeContentType ObtenerUnidadMedida(string codigo)
		{
			try
			{
				UnitCodeContentType enumerable = Enumeracion.ParseToEnum<UnitCodeContentType>(codigo);

				return enumerable;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(string.Format("Error al obtener la enumeración {0} con código {1}. Detalle: {2}", "UnitCodeContentType", codigo, excepcion.Message));
			}
		}

        /// <summary>
        /// Obtiene el Medio de Pago del documento
        /// http://www.unece.org/trade/untdid/d16a/tred/tred4461.htm
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Medio de pago</returns>
        public static Meanscode ObtenerMedioPago(int codigo)
        {
            try
            {
                Meanscode enumerable = Enumeracion.ParseToEnum<Meanscode>(codigo);

                return enumerable;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(string.Format("Error al obtener la enumeración {0} con código {1}. Detalle: {2}", "Meanscode", codigo, excepcion.Message));
            }
        }


        public static termstype ObtenerTerminoPago(int codigo)
        {
            try
            {
                termstype enumerable = Enumeracion.ParseToEnum<termstype>(codigo);

                return enumerable;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(string.Format("Error al obtener la enumeración {0} con código {1}. Detalle: {2}", "Meanscode", codigo, excepcion.Message));
            }
        }

    }
}
