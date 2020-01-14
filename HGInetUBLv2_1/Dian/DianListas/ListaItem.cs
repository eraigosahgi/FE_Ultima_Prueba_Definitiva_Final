using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		public enum ListasFE
		{
			[Description("Codigo Postal")]
			CodPostal = 1,

			[Description("Medios de Pago")]
			MedioPago = 2,

			[Description("Paises")]
			Paises = 3,

			[Description("Productos")]
			Productos = 4,

			[Description("Tipo Codigo Productos")]
			TipoCodProductos = 5,

			[Description("Tipo Entrega")]
			TipoEntrega = 6,

			[Description("Tipo Moneda")]
			TipoMoneda = 7,

			[Description("Unidades de Medida")]
			UnidadMedida = 8,
		}

	}
}
