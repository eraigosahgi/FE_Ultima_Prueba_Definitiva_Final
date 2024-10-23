using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Enumerables
{

	/// <summary>
	/// Tipo de Expresiones Regulares
	/// </summary>
	public enum TipoExpresion
	{
		PaginaWeb = 1,
		Email = 2,
		Numero = 3,
		Decimal = 4,

		Celular = 5,

		/// <summary>
		/// Para números que no comiencen con cero
		/// ^[1-9]\d*$
		/// </summary>
		NumeroNotStartZero = 6,

		Alfanumerico = 7,
		EspaciosEnBlanco

	}
}
