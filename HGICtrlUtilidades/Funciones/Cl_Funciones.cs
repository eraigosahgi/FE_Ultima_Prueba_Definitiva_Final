using HGICtrlUtilidades.ManejoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Funciones
{
	public class Cl_Funciones
	{
		public static string EjecutaInstruccion(string PStrSql, byte PbytMensaje, string conexion_sql)
		{
			Cl_EjecutarSQL Cmd = new Cl_EjecutarSQL(conexion_sql);
			Cl_Version.EstablecerConfiguracionRegional();
			string Ejecuta;

			try
			{
				Ejecuta = Cmd.EjecutarSql(PStrSql);

				if (Ejecuta != "OK")
				{
					if (PbytMensaje == 1)
					{
						Ejecuta = Ejecuta + Environment.NewLine + Environment.NewLine + Environment.NewLine + PStrSql;
						throw new ApplicationException(Ejecuta);
					}
				}
				return Ejecuta;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex);
			}
		}

		/// <summary>
		/// Convierte el Archivo recibido en un arrglo de Byte
		/// </summary>
		/// <param name="RutaArchivo">Ruta fisica del Archivo</param>
		/// <returns></returns>
		public static byte[] ConvertirArchvoByte(string RutaArchivo)
		{
			byte[] bytes = new byte[] { };
			try
			{
				if (File.Exists(RutaArchivo))
					bytes = File.ReadAllBytes(RutaArchivo);

				return bytes;
			}
			catch (Exception ex)
			{
				return bytes;
			}
		}

		/// <summary>
		/// Ejecuta cualquier instrucción sql
		/// </summary>
		/// <param name="PStrSql"></param>
		/// <param name="PbytMensaje"></param>
		/// <returns></returns>
		public static string EjecutaInstruccion(string PStrSql, byte PbytMensaje)
		{
			Cl_EjecutarSQL Cmd = new Cl_EjecutarSQL();
			Cl_Version.EstablecerConfiguracionRegional();
			string Ejecuta;

			try
			{
				Ejecuta = Cmd.EjecutarSql(PStrSql);

				if (Ejecuta != "OK")
				{
					if (PbytMensaje == 1)
					{
						Ejecuta = Ejecuta + Environment.NewLine + Environment.NewLine + Environment.NewLine + PStrSql;

						//AMTR Cl_FuncionesForms.MensajeError(Ejecuta);
						throw new ApplicationException(Ejecuta);
					}
				}
				return Ejecuta;
			}
			catch (Exception ex)
			{
				return "";
			}
		}



	}

}
