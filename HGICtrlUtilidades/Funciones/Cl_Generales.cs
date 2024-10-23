using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
	public class Cl_Generales
	{

		/// <summary>
		/// Calcula el dígito de verificación(DV) de un NIT
		/// </summary>
		/// <param name="PstrNit">NIT</param>
		/// <relation>Utilidades</relation>
		/// <returns>Número dígito de verificación</returns>
		/// <remarks></remarks>
		public static byte CalculoDv(String PstrNit)
		{
			try
			{
				byte Pdigitos;
				int j;
				Pdigitos = 11;
				Double[] LarrNit = new Double[Pdigitos];
				double MintValor1;
				double MintValor2;
				double MintValor3;
				double MintValor4;
				double MintValor5;
				double MintValor6;
				Double[] LArrValores = new Double[Pdigitos];
				if (PstrNit.Length > Pdigitos)
				{
					return 0;
				}
				if (!Char.IsNumber(PstrNit, 3))
				{
					return 0;
				}

				PstrNit = new string('0', Pdigitos - PstrNit.Length) + PstrNit;
				for (j = 0; j <= Pdigitos - 1; j++)
				{
					LarrNit[j] = Convert.ToDouble(PstrNit.Substring(j, 1));
				}
				LArrValores[0] = LarrNit[0] * 47;
				LArrValores[1] = LarrNit[1] * 43;
				LArrValores[2] = LarrNit[2] * 41;
				LArrValores[3] = LarrNit[3] * 37;
				LArrValores[4] = LarrNit[4] * 29;
				LArrValores[5] = LarrNit[5] * 23;
				LArrValores[6] = LarrNit[6] * 19;
				LArrValores[7] = LarrNit[7] * 17;
				LArrValores[8] = LarrNit[8] * 13;
				LArrValores[9] = LarrNit[9] * 7;
				LArrValores[10] = LarrNit[10] * 3;
				MintValor1 = 0;
				for (j = 0; j <= 10; j++)
				{
					MintValor1 = MintValor1 + LArrValores[j];
				}

				MintValor2 = MintValor1 / 11;
				MintValor3 = Math.Truncate(MintValor2);

				MintValor4 = MintValor3 * 11;
				MintValor5 = MintValor1 - MintValor4;

				switch (Convert.ToInt32(MintValor5))
				{
					case 0:
						MintValor6 = 0;
						break;
					case 1:
						MintValor6 = 1;
						break;
					default:
						MintValor6 = 11 - MintValor5;
						break;
				}
				return Convert.ToByte(MintValor6);
			}
			catch (Exception excepcion)
			{
				//throw new ApplicationException(excepcion.Message, excepcion.InnerException);
				return Convert.ToByte(0);
			}
		}

		public static bool NumeroPar(int numero)
		{
			int resultado = 0;
			bool respuesta = false;

			resultado = (numero % 2);
			if (resultado == 0)
				respuesta = true;

			return respuesta;

		}


	}
}
