using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio.General
{
	public class DestinatarioEmail
	{

		#region Propiedades

		private string nombre;
		private string email;

		/// <summary>
		/// Nombre
		/// </summary>
		public string Nombre
		{
			get { return nombre; }
			set { nombre = value; }
		}

		/// <summary>
		/// Email
		/// </summary>
		public string Email
		{
			get { return email; }
			set { email = value; }
		}

		#endregion

	}
}
