using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.HgiNet
{
	public class Edicion
	{
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

	}
}
