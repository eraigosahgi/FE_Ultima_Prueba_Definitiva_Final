using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.HgiNet
{
	public class Aplicativo
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

					case "HGInet Ecommerce Magento":
						id_aplicativo = 13; break;

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
						aplicativo = "HGI Administrativo"; break;

					case 2:
						aplicativo = "HGI Contable"; break;

					case 3:
						aplicativo = "HGI Nómina"; break;

					case 4:
						aplicativo = "HGI Pos"; break;

					case 6:
						aplicativo = "HGI Ganadero"; break;

					case 10:
						aplicativo = "HGI Móvil"; break;

					case 11:
						aplicativo = "HGI Servicios Web"; break;

					case 12:
						aplicativo = "HGI Smart"; break;

					case 13:
						aplicativo = "HGI Ecommerce Magento"; break;




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
						aplicativo = "HgiAdmin";
						break;

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


		public static int ObtenerDiasValidacionLicencia(int id_aplicativo)
		{
			int dias_validacion = 0;

			try
			{
				switch (id_aplicativo)
				{
					case 1:
						dias_validacion = 1; break;

					case 2:
						dias_validacion = 1; break;

					case 3:
						dias_validacion = 1; break;

					case 4:
						dias_validacion = 1; break;

					case 6:
						dias_validacion = 1; break;

					case 10:
						dias_validacion = 1; break;

					case 11:
						dias_validacion = 1; break;

					case 12:
						dias_validacion = 1; break;

					case 13:
						dias_validacion = 1; break;

					case 18:
						dias_validacion = 1; break;

					case 19:
						dias_validacion = 1; break;

					case 20:
						dias_validacion = 1; break;

					case 21:
						dias_validacion = 1; break;

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
						dias_validacion = 7; break;

					default:
						throw new Exception(string.Format("El aplicativo con id {0} no se encuentra registrado.", id_aplicativo));
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return dias_validacion;
		}

	}
}

