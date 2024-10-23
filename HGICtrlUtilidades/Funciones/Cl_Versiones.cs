using HGICtrlUtilidades.ManejoDatos;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Funciones
{
	public class Cl_Versiones
	{
		public static string PermisosExtras(int PTipo, int PTipoCampo, int PPerfil, int EmpresaActual, string Usuario)
		{
			// Verificar las opciones que tiene activa tanto en grupos como en usuarios
			Cl_EjecutarSQL cmd = new Cl_EjecutarSQL();
			DataTable dt;
			string sql = "";
			int i, n;
			string ncampo = "";
			string Valores = "";
			// Si es string  
			if (PTipoCampo == 0)
				ncampo = "StrValor as Valor";
			else
				ncampo = "IntValor as Valor";

			// Opciones x Perfil  
			sql = "SELECT" + Environment.NewLine;
			sql += "TblOpcExtrasXPerfil." + ncampo + Environment.NewLine;
			sql += "FROM TblOpcExtrasXPerfil" + Environment.NewLine;
			sql += "INNER JOIN TblUsuariosXPerfil ON TblOpcExtrasXPerfil.Perfil = TblUsuariosXPerfil.Perfil" + Environment.NewLine;
			sql += "WHERE TblUsuariosXPerfil.Usuario='" + Usuario + "'" + Environment.NewLine;
			sql += "And IntEmpresa=" + EmpresaActual + Environment.NewLine;
			sql += "And IntTipo=" + PTipo + Environment.NewLine;

			switch (PPerfil)
			{
				case 0: { break; }
				case 1: { sql += "And TblOpcExtrasXPerfil.Activado = 1" + Environment.NewLine; break; }
				case 2: { sql += "And TblOpcExtrasXPerfil.Editar = 1" + Environment.NewLine; break; }
				case 3: { sql += "And TblOpcExtrasXPerfil.Ingresar = 1" + Environment.NewLine; break; }
				case 4: { sql += "And TblOpcExtrasXPerfil.Eliminar = 1" + Environment.NewLine; break; }
				case 5: { sql += "And TblOpcExtrasXPerfil.Anular = 1" + Environment.NewLine; break; }
				case 6: { sql += "And TblOpcExtrasXPerfil.Imprimir = 1" + Environment.NewLine; break; }
				case 7: { sql += "And TblOpcExtrasXPerfil.Gestion = 1" + Environment.NewLine; break; }
				default: { break; }
			}

			sql += "UNION" + Environment.NewLine;

			// Opciones Por usuario.
			sql += "SELECT" + Environment.NewLine;
			sql += "TblOpcExtrasXUsuario." + ncampo + Environment.NewLine;
			sql += "FROM TblOpcExtrasXUsuario" + Environment.NewLine;
			sql += "WHERE Usuario='" + Usuario + "'" + Environment.NewLine;
			sql += "And IntEmpresa=" + EmpresaActual + Environment.NewLine;
			sql += "And IntTipo=" + PTipo;
			switch (PPerfil)
			{
				case 0: { break; }
				case 1: { sql += "And TblOpcExtrasXUsuario.Activado = 1" + Environment.NewLine; break; }
				case 2: { sql += "And TblOpcExtrasXUsuario.Editar = 1" + Environment.NewLine; break; }
				case 3: { sql += "And TblOpcExtrasXUsuario.Ingresar = 1" + Environment.NewLine; break; }
				case 4: { sql += "And TblOpcExtrasXUsuario.Eliminar = 1" + Environment.NewLine; break; }
				case 5: { sql += "And TblOpcExtrasXUsuario.Anular = 1" + Environment.NewLine; break; }
				case 6: { sql += "And TblOpcExtrasXUsuario.Imprimir = 1" + Environment.NewLine; break; }
				case 7: { sql += "And TblOpcExtrasXUsuario.Gestion = 1" + Environment.NewLine; break; }
				default: { break; }
			}

			dt = cmd.SelectSql(sql);
			n = dt.Rows.Count;
			if (n > 0)
			{
				for (i = 0; i <= n - 1; i++)
				{
					try
					{
						if (PTipoCampo == 0)
							Valores += "'" + dt.Rows[i].Field<string>("Valor") + "',";
						else
							Valores += dt.Rows[i].Field<string>("Valor") + ",";
					}
					catch (Exception ex)
					{
						throw new ApplicationException(ex.Message, ex.InnerException);
					}
				}

				Valores = Valores.Substring(1, Valores.Length - 1);
			}
			else
				// If POpcion = 0 Then
				Valores = "NO";

			return Valores;
		}

		/// <summary>
		/// Asigna el nombre de un intervalo
		/// </summary>
		/// <param name="PintNumero"Número: 0 Semana, 1 Década, 2 Catorcena, 3 Quincena, 4 Mes></param>
		/// <returns></returns>
		public static string NombreIntervalo(int PintNumero)
		{
			string NombreIntervalo = "Quincena";
			switch (PintNumero)
			{
				case 0: { NombreIntervalo = "Semana"; break; }
				case 1: { NombreIntervalo = "Decada"; break; }
				case 2: { NombreIntervalo = "Catorcena"; break; }
				case 3: { NombreIntervalo = "Quincena"; break; }
				case 4: { NombreIntervalo = "Mes"; break; }
			}

			return NombreIntervalo;
		}

		/// <summary>
		/// Asigna nombre ordenativo
		/// </summary>
		/// <param name="PintNumero">Número: 1 Primera, 2 Segunda, 3 Tercera, 4 Cuarta, 5 Quinta</param>
		/// <param name="PintIntervalo">Intervalo</param>
		/// <returns></returns>
		public static string Ordenativo(int PintNumero, int PintIntervalo)
		{
			string Ordenativo = "Primera";
			switch (PintNumero)
			{
				case 1: { Ordenativo = "Primera"; break; }
				case 2: { Ordenativo = "Segunda"; break; }
				case 3: { Ordenativo = "Tercera"; break; }
				case 4: { Ordenativo = "Cuarta"; break; }
				case 5: { Ordenativo = "Quinta"; break; }
			}
			if (PintIntervalo == 4)
				Ordenativo = "";

			return Ordenativo;
		}


		public static string DetallesPeriodoNomina(int empresa_actual, string BD_Servidor, string BD_Nombre, string BD_Usuario, string BD_Clave, string App_Usuario, int AgnoActual, int PeriodoActual)
		{
			try
			{
				string descripcion_periodo = string.Empty;

				string conexion_sql = Cl_EjecutarSQL.StringConeccionSQL(BD_Servidor, BD_Nombre, 1, BD_Usuario, BD_Clave, 1, App_Usuario);

				descripcion_periodo += "  " + Ordenativo(Cl_FuncionesBD.GetCalendarioNomCampoInteger(AgnoActual, PeriodoActual, "IntEvento", empresa_actual, conexion_sql), Cl_FuncionesBD.GetEmpresaCampoInteger("IntIntervalo", empresa_actual, conexion_sql));
				descripcion_periodo += "  " + NombreIntervalo(Cl_FuncionesBD.GetEmpresaCampoInteger("IntIntervalo", empresa_actual, conexion_sql));

				int mes_actual = Cl_FuncionesBD.GetCalendarioNomCampoInteger(AgnoActual, PeriodoActual, "IntMes", empresa_actual, conexion_sql);

				if (mes_actual != 0)
					descripcion_periodo += " de " + Cl_Fecha.MesLetras(mes_actual);

				return descripcion_periodo;
			}
			catch (Exception exception)
			{
				throw new ApplicationException(exception.Message, exception.InnerException);
			}
		}

	}
}
