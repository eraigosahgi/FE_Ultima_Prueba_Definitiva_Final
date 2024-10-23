using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.Formato
{
	public static class Colores
	{

		/// <summary>
		/// convierte los colores a formato hexadecimal o rgba
		/// </summary>
		/// <param name="color">codigo del color</param>
		/// <returns></returns>
		public static string ConvertirColor(int color)
		{
			if (color != 0)
				return string.Format("{0},{1},{2},{3}",System.Drawing.Color.FromArgb(color).R, System.Drawing.Color.FromArgb(color).G, System.Drawing.Color.FromArgb(color).B, System.Drawing.Color.FromArgb(color).A);
			else
				return System.Drawing.Color.LightGray.ToString();
		}

	}
}
