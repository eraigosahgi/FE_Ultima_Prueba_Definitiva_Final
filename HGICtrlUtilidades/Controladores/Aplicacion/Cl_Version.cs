using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_Version
	{
		/// <summary>
		/// Obtiene el id del aplicativo
		/// </summary>
		/// <param name="aplicativo">nombre del aplicativo</param>
		/// <returns>id del aplicativo</returns>
		public static int ObtenerIdAplicacion(String aplicativo)
		{
			int id_aplicativo = 0;

			try
			{
				switch (aplicativo)
				{
					case "HGInet Administrativo":
						id_aplicativo = 1; break;

					case "HGInet Contable":
						id_aplicativo = 2; break;

					case "HGInet Nómina":
						id_aplicativo = 3; break;

					case "HGInet Pos":
						id_aplicativo = 4; break;

					case "HGInet Ganadero":
						id_aplicativo = 6; break;

					case "HGInet Móvil":
						id_aplicativo = 10; break;

					case "HGInet Servicios Web":
						id_aplicativo = 11; break;

					case "HGInet Smart":
						id_aplicativo = 12; break;

					case "HGInet Factura Electrónica":
						id_aplicativo = 13; break;

					case "HGInet Backup":
						id_aplicativo = 18; break;

					case "HGInet Email":
						id_aplicativo = 19; break;

					case "HGInet SMS":
						id_aplicativo = 20; break;

					case "HGInet Pagos Electrónicos":
						id_aplicativo = 21; break;

					case "Happgi":
						id_aplicativo = 23; break;

					default:
						throw new Exception(string.Format("El aplicativo {0} no se encuentra registrado.", aplicativo));
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return id_aplicativo;
		}


		public static string ObtenerSerialAplicacion(int id_aplicativo)
		{
			string serial_app = string.Empty;

			try
			{
				switch (id_aplicativo)
				{
					case 1:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 2:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 3:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 4:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 6:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 10:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 11:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 12:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 13:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 18:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 19:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 20:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 21:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					case 23:
					case 24:
					case 25:
					case 26:
					case 27:
					case 28:
					case 33:
					case 34:
					case 35:
					case 36:
						serial_app = "5824e879-008c-4688-b7aa-28702ca37a4f"; break;

					default:
						throw new Exception(string.Format("El aplicativo con id {0} no se encuentra registrado.", id_aplicativo));
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return serial_app;
		}

		/// <summary>
		/// Obtiene el nombre del aplicativo
		/// </summary>
		/// <param name="id_aplicativo">id del aplicativo</param>
		/// <returns>nombre del aplicativo</returns>
		public static string ObtenerNombreAplicacion(int id_aplicativo)
		{
			string aplicativo = string.Empty;

			try
			{
				switch (id_aplicativo)
				{
					case 1:
						aplicativo = "HGInet Administrativo"; break;

					case 2:
						aplicativo = "HGInet Contable"; break;

					case 3:
						aplicativo = "HGInet Nómina"; break;

					case 4:
						aplicativo = "HGInet Pos"; break;

					case 6:
						aplicativo = "HGInet Ganadero"; break;

					case 10:
						aplicativo = "HGInet Móvil"; break;

					case 11:
						aplicativo = "HGInet Servicios Web"; break;

					case 12:
						aplicativo = "HGInet Smart"; break;

					case 13:
						aplicativo = "HGInet Factura Electrónica"; break;

					case 18:
						aplicativo = "HGInet Backup"; break;

					case 19:
						aplicativo = "HGInet Email"; break;

					case 20:
						aplicativo = "HGInet SMS"; break;

					case 21:
						aplicativo = "HGInet Pagos Electrónicos"; break;

					case 23:
						aplicativo = "Happgi"; break;

					case 24:
						aplicativo = "Happgi Contable"; break;

					case 25:
						aplicativo = "Happgi Nómina"; break;

					case 26:
						aplicativo = "Happgi Pos"; break;

					case 27:
						aplicativo = "CRM"; break;

					case 28:
						aplicativo = "WMS"; break;

					case 30:
						aplicativo = "HGI Store"; break;

					case 33:
						aplicativo = "ERP Web"; break;

					case 34:
						aplicativo = "ERP Web Contable"; break;

					case 35:
						aplicativo = "ERP Web Nómina"; break;

					case 36:
						aplicativo = "ERP Web Pos"; break;

					default:
						throw new Exception(string.Format("El aplicativo con id {0} no se encuentra registrado.", id_aplicativo));
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return aplicativo;
		}

		/// <summary>
		/// Obtiene el nombre de la edición
		/// </summary>
		/// <param name="edicion">nombre de la edición</param>
		/// <returns>número de la edición</returns>
		public static int ObtenerIdEdicion(string edicion)
		{
			int id_edicion = 0;

			try
			{
				switch (edicion)
				{
					case "Edición Factura":
						id_edicion = -1; break;

					case "Edición Express":
						id_edicion = 0; break;

					case "Edición Básica":
						id_edicion = 1; break;

					case "Edición Estándar":
						id_edicion = 2; break;

					case "Edición Avanzada":
						id_edicion = 3; break;

					case "Edición Contador":
						id_edicion = 4; break;

					default:
						throw new Exception(string.Format("La edición {0} no se encuentra registrada.", edicion));
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return id_edicion;
		}

		/// <summary>
		/// Obtiene el nombre de la edición
		/// </summary>
		/// <param name="id_edicion">número de la edición</param>
		/// <returns>nombre de la edición</returns>
		public static string ObtenerNombreEdicion(int id_edicion)
		{
			string edicion = string.Empty;

			try
			{
				switch (id_edicion)
				{
					case -1:
						edicion = "Edición Factura"; break;

					case 0:
						edicion = "Edición Express"; break;

					case 1:
						edicion = "Edición Básica"; break;

					case 2:
						edicion = "Edición Estándar"; break;

					case 3:
						edicion = "Edición Avanzada"; break;

					case 4:
						edicion = "Edición Contador"; break;

					default:
						throw new Exception(string.Format("El id {0} de edición no se encuentra registrado.", id_edicion));
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return edicion;
		}

		/// <summary>
		/// Obtiene el nombre del aplicativo para la conexión con la base de datos
		/// </summary>
		/// <param name="id_aplicativo">id del aplicativo</param>
		/// <returns>nombre del aplicativo</returns>
		public static string ObtenerNombreAplicacionBaseDatos(int id_aplicativo)
		{
			string aplicativo = string.Empty;

			try
			{
				switch (id_aplicativo)
				{
					case 1:
						aplicativo = "HgiAdmin"; break;

					case 2:
						aplicativo = "HgiConta"; break;

					case 3:
						aplicativo = "HgiNomin"; break;

					case 4:
						aplicativo = "HgiPospv"; break;

					case 6:
						aplicativo = "HgiAdmin"; break;

					case 10:
						aplicativo = "HgiAdmin"; break;

					case 11:
						aplicativo = "HgiAdmin"; break;

					case 12:
						aplicativo = "HgiAdmin"; break;

					case 13:
						aplicativo = "HgiAdmin"; break;

					case 18:
						aplicativo = "HgiAdmin"; break;

					case 19:
						aplicativo = "HgiAdmin"; break;

					case 20:
						aplicativo = "HgiAdmin"; break;

					case 21:
						aplicativo = "HgiAdmin"; break;

					case 23:
						aplicativo = "Happgi__"; break;

					case 24:
						aplicativo = "Happgi__"; break;

					case 25:
						aplicativo = "Happgi__"; break;

					case 26:
						aplicativo = "Happgi__"; break;

					case 27:
						aplicativo = "Happgi__"; break;

					case 28:
						aplicativo = "Happgi__"; break;

					case 30:
						aplicativo = "Happgi__"; break;
					case 33:
						aplicativo = "Happgi__"; break;
					case 34:
						aplicativo = "Happgi__"; break;
					case 35:
						aplicativo = "Happgi__"; break;
					case 36:
						aplicativo = "Happgi__"; break;

					default:
						throw new Exception(string.Format("El aplicativo con id {0} no se encuentra registrado.", id_aplicativo));
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return aplicativo;
		}


		public static void EstablecerConfiguracionRegional()
		{

			// Establecer configuracion regional 
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es");
			Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator = "-";
			Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
			Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
			Thread.CurrentThread.CurrentCulture.DateTimeFormat.AMDesignator = "Am";
			Thread.CurrentThread.CurrentCulture.DateTimeFormat.PMDesignator = "Pm";
			Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

			Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
			Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ",";
			Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol = "$";
			Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
			Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits = 0;
			Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ",";

			Thread.CurrentThread.CurrentCulture.NumberFormat.PercentGroupSeparator = ",";
			Thread.CurrentThread.CurrentCulture.NumberFormat.PercentDecimalSeparator = ".";
			Thread.CurrentThread.CurrentCulture.NumberFormat.PercentSymbol = "%";
		}


	}
}
