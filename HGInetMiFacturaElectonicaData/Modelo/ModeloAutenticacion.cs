using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public class ModeloAutenticacion
	{
		private string servidor;
		private string basedatos;
		private string usuario;
		private string clave;

		public string Servidor
		{
			get
			{
				return servidor;
			}
		}

		public string Basedatos
		{
			get
			{
				return basedatos;
			}
		}

		public string Usuario
		{
			get
			{
				return usuario;
			}

		}

		public string Clave
		{
			get
			{
				return clave;
			}
		}
		
		public ModeloAutenticacion(string servidor, string basedatos, string usuario, string clave)
		{
			this.servidor = servidor;
			this.basedatos = basedatos;
			this.usuario = usuario;
			this.clave = clave;
		}

	}
}
