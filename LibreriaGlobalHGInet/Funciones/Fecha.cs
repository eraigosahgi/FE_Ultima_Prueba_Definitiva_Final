using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LibreriaGlobalHGInet.Funciones
{
	public class Fecha
	{
		/// <summary>
		/// Formato de fecha:  yyyy-MM-dd
		/// </summary>
		public static readonly string formato_fecha_hginet = @"yyyy-MM-dd";

		/// <summary>
		/// Formato de fecha:  yyyyMMdd
		/// </summary>
		public static readonly string formato_fecha_archivo = @"yyyyMMdd";

		/// <summary>
		/// Formato de fecha:  yyyy-MM-dd HH:mm
		/// </summary>
		public static readonly string formato_fecha_hora = @"yyyy-MM-dd HH:mm";

		/// <summary>
		/// Formato de fecha:  yyyy-MM-dd HH:mm:ss
		/// </summary>
		public static readonly string formato_fecha_hora_completa = @"yyyy-MM-dd HH:mm:ss";

		/// <summary>
		/// Formato de fecha:  yyyyMMdd_HHmm
		/// </summary>
		public static readonly string formato_fecha_hora_archivo = @"yyyyMMdd_HHmm";

		/// <summary>
		/// Formato de fecha:  HH:mm
		/// </summary>
		public static readonly string formato_hora = @"HH:mm";

		/// <summary>
		/// Formato de fecha:  HH:mm:ss
		/// </summary>
		public static readonly string formato_hora_completa = @"HH:mm:ss";

		/// <summary>
		/// Formato de Hora zona:  HH:mm:sszzz
		/// </summary>
		public static readonly string formato_hora_zona = @"HH:mm:sszzz";

		/// <summary>
		/// Formato de fecha:  yyyy-MM-dd HH:mm:ss
		/// </summary>
		public static readonly string formato_fecha_hora_completa_sql = @"yyyy-MM-dd HH:mm:ss";

		/// <summary>
		/// Formato de fecha: yyyyMMddHHmmss
		/// </summary>
		public static readonly string formato_fecha_java = @"yyyyMMddHHmmss";

		/// <summary>
		/// Formato de fecha Factura Electronica V2:  yyyy-MM-ddHH:mm:sszzz
		/// </summary>
		public static readonly string formato_fecha_hora_zona = @"yyyy-MM-ddHH:mm:sszzz";

		public enum DateInterval
		{
			Day,
			DayOfYear,
			Hour,
			Minute,
			Month,
			Quarter,
			Second,
			Weekday,
			WeekOfYear,
			Year
		}


		/// <summary>
		/// Obtiene la fecha actual en UTC Local
		/// </summary>
		/// <returns type="DateTime">fecha completa actual</returns>
		public static DateTime GetFecha()
		{
			TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
			return TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
		}

		public static long Diferencia(DateTime fecha_inicial, DateTime fecha_final, DateInterval intervalo)
		{
			switch (intervalo)
			{
				case DateInterval.Day:

				case DateInterval.DayOfYear:
					System.TimeSpan spanForDays = fecha_final - fecha_inicial;
					return (long)spanForDays.TotalDays;

				case DateInterval.Hour:
					System.TimeSpan spanForHours = fecha_final - fecha_inicial;
					return (long)spanForHours.TotalHours;

				case DateInterval.Minute:
					System.TimeSpan spanForMinutes = fecha_final - fecha_inicial;
					return (long)spanForMinutes.TotalMinutes;

				case DateInterval.Month:
					return ((fecha_final.Year - fecha_inicial.Year) * 12) + (fecha_final.Month - fecha_inicial.Month);

				case DateInterval.Quarter:
					long dateOneQuarter = (long)System.Math.Ceiling(fecha_inicial.Month / 3.0);
					long dateTwoQuarter = (long)System.Math.Ceiling(fecha_final.Month / 3.0);
					return (4 * (fecha_final.Year - fecha_inicial.Year)) + dateTwoQuarter - dateOneQuarter;

				case DateInterval.Second:
					System.TimeSpan spanForSeconds = fecha_final - fecha_inicial;
					return (long)spanForSeconds.TotalSeconds;

				case DateInterval.Weekday:
					System.TimeSpan spanForWeekdays = fecha_final - fecha_inicial;
					return (long)(spanForWeekdays.TotalDays / 7.0);

				case DateInterval.WeekOfYear:
					System.DateTime dateOneModified = fecha_inicial;
					System.DateTime dateTwoModified = fecha_final;
					while (dateTwoModified.DayOfWeek != System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
					{
						dateTwoModified = dateTwoModified.AddDays(-1);
					}
					while (dateOneModified.DayOfWeek != System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
					{
						dateOneModified = dateOneModified.AddDays(-1);
					}
					System.TimeSpan spanForWeekOfYear = dateTwoModified - dateOneModified;
					return (long)(spanForWeekOfYear.TotalDays / 7.0);

				case DateInterval.Year:
					return fecha_final.Year - fecha_inicial.Year;

				default:
					return 0;
			}
		}

		/// <summary>
		/// Obtener el día en letras de una fecha específica
		/// </summary>
		/// <param name="fecha" type="DateTime">fecha</param>
		/// <returns type="string">día en letras</returns>
		public static string DiaLetras(DateTime fecha)
		{
			TextInfo myTI = new CultureInfo("es-CO", false).TextInfo;

			return myTI.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(fecha.DayOfWeek));
		}

		/// <summary>
		/// Obtener el mes en letras de una fecha específica
		/// </summary>
		/// <param name="fecha" type="DateTime">fecha</param>
		/// <returns type="string">mes en letras</returns>
		public static string MesLetras(DateTime fecha)
		{
			TextInfo myTI = new CultureInfo("es-CO", false).TextInfo;

			return myTI.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fecha.Month));
		}

		/// <summary>
		/// obtiene las quincenas
		/// </summary>
		/// <param name="quincenas">cantidad de quincenas</param>
		/// <param name="fecha">fecha vencimiento</param>
		/// <returns></returns>
		public static DateTime GetFechaQuincenal(int quincenas, DateTime fecha)
		{
			int dia = 0;
			int mes = 0;
			int año = 0;

			System.DateTime Fechas = default(System.DateTime);
			int UltDia = 0;

			dia = fecha.Day;
			mes = fecha.Month;
			año = fecha.Year;

			for (int x = 1; x <= quincenas; x++)
			{
				if (dia <= 15)
				{
					Fechas = new DateTime(año, mes, 15);
					dia = 16;

				}
				else
				{
					UltDia = new DateTime(año, mes, 1).AddMonths(1).Day;

					if (UltDia == 31)
					{
						Fechas = new DateTime(año, mes, 1).AddMonths(1);
					}
					else
					{
						Fechas = new DateTime(año, mes, 1).AddMonths(1);
					}

					Fechas = Fechas.AddDays(-1);
					mes = mes + 1;
					dia = 15;
				}
			}
			return Fechas;
		}

		/// <summary>
		/// Corrige el formato de fechas en archivos XML
		/// </summary>
		/// <param name="XmlResponse">datos xml</param>
		/// <returns>datos xml</returns>
		public static XDocument CorregirFormato(XDocument XmlResponse)
		{
			// https://www.codeproject.com/Articles/206330/Learning-REGEX-regular-expression-in-the-most-ea
			// patrón de comparación de fecha del tipo 2018-03-13 sin truncamiento al final (sin $)
			string pattern_date = @"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])";

			Regex exp_regular = new Regex(pattern_date, RegexOptions.IgnoreCase);

			// recorre los elementos del xml
			foreach (XElement item in XmlResponse.Elements().Descendants())
			{
				//Console.WriteLine("Node Attribute Name : {0} Value : {1}", item.Name.LocalName, item.Value);

				// compara si el valor del elemento corresponde a una fecha através de expresiones regulares
				bool date = exp_regular.IsMatch(item.Value);

				// reemplaza caracteres en la fecha
				if (date)
					item.Value = item.Value.Replace("-05:00", "").Replace("Z", "");
			}

			return XmlResponse;
		}

		/// <summary>
		/// Obtiene el formato para DateTime.ParseExact de acuerdo con la fecha en texto 
		/// </summary>
		/// <param name="fecha">fecha en texto</param>
		/// <returns>formato de conversión</returns>
		public static string ObtenerFormato(string fecha)
        {
            // patrón de comparación de fecha del tipo yyyy-MM-ddTHH:mm:ss.fff con truncamiento al final (con $)
            string pattern_date = @"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])T\d{2}:\d{2}:\d{2}.\d{3}$";

            Regex exp_regular = new Regex(pattern_date, RegexOptions.IgnoreCase);
            
            // compara si el valor del elemento corresponde a una fecha através de expresiones regulares
            if(exp_regular.IsMatch(fecha))
                return "yyyy-MM-ddTHH:mm:ss.fff";

            // patrón de comparación de fecha del tipo yyyy-MM-ddTHH:mm:ss con truncamiento al final (con $)
            pattern_date = @"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])T\d{2}:\d{2}:\d{2}$";

            exp_regular = new Regex(pattern_date, RegexOptions.IgnoreCase);
            
             // compara si el valor del elemento corresponde a una fecha através de expresiones regulares
            if(exp_regular.IsMatch(fecha))
                return "yyyy-MM-ddTHH:mm:ss";

            // patrón de comparación de fecha del tipo yyyy-MM-ddTHH:mm con truncamiento al final (con $)
            pattern_date = @"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])T\d{2}:\d{2}$";

            exp_regular = new Regex(pattern_date, RegexOptions.IgnoreCase);
            
             // compara si el valor del elemento corresponde a una fecha através de expresiones regulares
            if(exp_regular.IsMatch(fecha))
                return "yyyy-MM-ddTHH:mm";

			// patrón de comparación de fecha del tipo yyyy-MM-dd con truncamiento al final (con $)
            pattern_date = @"^\d{4}-((0\d)|(1[012]))-(([012]\d)|3[01])$";

            exp_regular = new Regex(pattern_date, RegexOptions.IgnoreCase);
            
             // compara si el valor del elemento corresponde a una fecha através de expresiones regulares
            if(exp_regular.IsMatch(fecha))
                return "yyyy-MM-dd";

			return "yyyy-MM-dd";
        }


        /// <summary>
        /// Convierte una fecha en Hora universal (utc)
        /// </summary>
        /// <param name="fecha">Fecha y hora que se desea convertir UTC</param>
        /// <returns></returns>
        public static string FechaUtc(DateTime fecha)
        {
            return  TimeZoneInfo.ConvertTimeToUtc(fecha).ToString("o");
        }

    }
}
