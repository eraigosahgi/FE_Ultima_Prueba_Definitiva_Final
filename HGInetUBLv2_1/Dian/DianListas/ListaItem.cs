using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1.DianListas
{
	public class ListaItem
	{
		public string Codigo;

		public string Nombre;

		public string Descripcion;

		public ListaItem(string codigo, string nombre, string descripcion)
		{
			this.Codigo = codigo;
			this.Nombre = nombre;
			this.Descripcion = descripcion;
		}

	}
}
