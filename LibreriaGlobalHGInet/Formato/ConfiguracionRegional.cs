using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace LibreriaGlobalHGInet.Formato
{
    public class ConfiguracionRegional
    {
        /// <summary>
        /// Establece la configuración regional
        /// </summary>
        public static void Predeterminar()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es");
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.AMDesignator = "Am";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.PMDesignator = "Pm";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ",";
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol = "$";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits = 0;
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ".";

            Thread.CurrentThread.CurrentCulture.NumberFormat.PercentGroupSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.PercentDecimalSeparator = ",";
            Thread.CurrentThread.CurrentCulture.NumberFormat.PercentSymbol = "%";
        }

        /// <summary>
        /// Obtiene la lista de países por códigos según ISO 3166-1  y lo compara con el enviado.
        /// </summary>
        /// <param name="codigo">codigo de pais</param>
        /// <returns>retorna verdadero mientra el codigo sea correcto </returns>
        public static bool ValidarCodigoPais(string codigo)
        {
            try
            {
                RegionInfo regional = new RegionInfo(codigo);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Obtiene la lista de países por códigos según ISO 3166-1 y compara el codigo moneda según ISO 4217
        /// </summary>
        /// <param name="codigo">codigo de moneda segun el pais</param>
        /// <returns>retorna verdadero mientra el codigo sea correcto </returns>
        public static bool ValidarCodigoMoneda(string codigo)
        {
            List<RegionInfo> countries = new List<RegionInfo>();
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo country = new RegionInfo(culture.LCID);

                if (country.ISOCurrencySymbol.Equals(codigo))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Retorna la descripción del tipo de moneda por código según ISO 3166-1 y compara el codigo moneda según ISO 4217
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static string TipoMoneda(string codigo)
        {
            List<RegionInfo> countries = new List<RegionInfo>();
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo country = new RegionInfo(culture.LCID);

                if (country.ISOCurrencySymbol.Equals(codigo))
                    return country.CurrencyNativeName;
            }

            return string.Empty;
        }


    }
}
