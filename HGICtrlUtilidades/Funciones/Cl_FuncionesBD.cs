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

	public static class Cl_FuncionesBD
	{
		/// <summary>
		/// Validar Dato de Null
		/// </summary>
		/// <param name="Dato">Dato</param>
		/// <returns>Dato del Objeto o DBNull</returns>
		public static object ValidarDato(object Dato)
		{
			if (Dato != null)
			{
				return Dato;
			}
			return DBNull.Value;
		}

		public static void ActualizaConsulta(string PStrConsulta, string PStrSQL, string conexion_sql)
		{
			if (PStrSQL != "")
			{
				// Strsql = "Drop View " & Pstrconsulta
				// Call Cl_Funciones.EjecutaInstruccion(Strsql, 0)

				if (ReemplazarVista(PStrConsulta, PStrSQL, conexion_sql) == false)
					CrearVista(PStrConsulta, PStrSQL, conexion_sql);
			}
		}

		public static bool ReemplazarVista(string PstrTabla, string pstrSQL, string conexion_sql)
		{
			try
			{
				string Strsql;
				bool ReemplazarVista = false;
				if (Strings.Left(PstrTabla, 1) == "~")
					return ReemplazarVista;

				if (Strings.UCase(Strings.Left(pstrSQL, 6)) != "SELECT")
					return ReemplazarVista;
				// PstrTabla = QuitaEspacios(PstrTabla)
				// pstrSQL = ReemplazaCaracter(pstrSQL, ";", " ")

				PstrTabla = PstrTabla;
				pstrSQL = pstrSQL;

				Strsql = "Alter View " + PstrTabla + Environment.NewLine;
				Strsql += "As" + Environment.NewLine;
				Strsql += pstrSQL + Environment.NewLine;
				if (Cl_Funciones.EjecutaInstruccion(Strsql, 0, conexion_sql) == "OK")
					ReemplazarVista = true;
				else
					ReemplazarVista = false;

				return ReemplazarVista;
			}
			catch (Exception exception)
			{
				return false;
			}
		}


		public static bool CrearVista(string PstrTabla, string pstrSQL, string conexion_sql)
		{
			try
			{
				string Strsql;
				bool CrearVista = false;
				if (Strings.Left(PstrTabla, 1) == "~")
					return CrearVista;
				if (Strings.UCase(Strings.Left(pstrSQL, 6)) != "SELECT")
					return CrearVista;
				// PstrTabla = QuitaEspacios(PstrTabla)
				// pstrSQL = ReemplazaCaracter(pstrSQL, ";", " ")

				PstrTabla = PstrTabla;
				pstrSQL = pstrSQL;

				Strsql = "Create View " + PstrTabla + Environment.NewLine;
				Strsql += "As" + Environment.NewLine;
				Strsql += pstrSQL + Environment.NewLine;
				if (Cl_Funciones.EjecutaInstruccion(Strsql, 1, conexion_sql) == "OK")
					CrearVista = true;
				else
					throw new ApplicationException(PstrTabla);

				return CrearVista;
			}
			catch (Exception exception)
			{
				return false;
			}
		}

		public static object RetornaSql(string Pstrsql, string conexion_sql = "", bool Pmensaje = true)
		{
			Cl_EjecutarSQL cmd = new Cl_EjecutarSQL();
			try
			{
				return cmd.SelectSqlString(Pstrsql, Pmensaje, conexion_sql);
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
			cmd = null/* TODO Change to default(_) if this is not a reference type */;
		}

		public static void GeneraSaldosXBanco(int PintAno, int PintPeriodo, string PstrBanco, string ServidorSQL, string BaseDatosSql, int ModoAut, string UsrSql, string PwdSql, int IdAplicativo, string Usuario, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			string Banco;
			Cl_EjecutarSQL Cmd;
			DataTable Lector;
			int J;
			J = 0;
			try
			{
				Strsql = " SELECT StrIdBanco as Banco" + Environment.NewLine;
				Strsql += "FROM TblBancos" + Environment.NewLine;

				if (Strings.Len(PstrBanco) > 0)
				{
				}
				Cmd = new Cl_EjecutarSQL(1, ServidorSQL, BaseDatosSql, ModoAut, UsrSql, PwdSql, IdAplicativo, Usuario);
				Lector = new DataTable();
				Lector = Cmd.SelectSqlDataSet(Strsql, "TB1").Tables[0];

				// paralelismo con for para crear los saldos del banco
				Parallel.For(1, Lector.Rows.Count, i =>
				{
					Banco = Convert.ToString(Lector.Rows[i]["Banco"]);
					GetSaldoBancoPeriodo(Banco, PintAno, PintPeriodo, ServidorSQL, BaseDatosSql, ModoAut, UsrSql, PwdSql, IdAplicativo, Usuario, EmpresaActual, conexion_sql);
				});
			}
			catch (Exception ex)
			{
				//AMTR Cl_FuncionesForms.MensajeCritico(ex.Message);
			}
		}

		public static void GetSaldoBancoPeriodo(string PBanco, int PAno, int PPeriodo, string ServidorSQL, string BaseDatosSql, int ModoAut, string UsrSql, string PwdSql, int IdAplicativo, string Usuario, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Cl_EjecutarSQL Cmd = new Cl_EjecutarSQL(1, ServidorSQL, BaseDatosSql, ModoAut, UsrSql, PwdSql, IdAplicativo, Usuario);
			DataTable Lector = new DataTable();
			double Saldo;
			double IntValor;
			byte IntTipo;
			try
			{
				Cl_Version.EstablecerConfiguracionRegional();

				Saldo = GetBancoSaldoCampoDouble(PBanco, PAno, PPeriodo, "IntSaldoF", true, EmpresaActual, PAno, conexion_sql);
				SetSaldoBancoCampoDouble(PBanco, PAno, PPeriodo, "IntSaldoI", Saldo, EmpresaActual, conexion_sql);

				Strsql = "SELECT Tblforpagos.IntEmpresa as Empresa" + Environment.NewLine;
				Strsql += ",Tblforpagos.IntTipoRecibo as Transaccion" + Environment.NewLine;
				Strsql += ",Tblforpagos.IntRecibo as Documento" + Environment.NewLine;
				Strsql += ",Tblforpagos.IntRegistro as Id" + Environment.NewLine;
				Strsql += ",Tblforpagos.IntForPago as ForPago" + Environment.NewLine;
				Strsql += ",Tblforpagos.StrBanco as Banco" + Environment.NewLine;
				Strsql += ",Tblforpagos.StrDocumento as DocRef" + Environment.NewLine;
				Strsql += ",Tblforpagos.IntValor as Valor" + Environment.NewLine;
				Strsql += ",Tblforpagos.IntSaldo as Saldo" + Environment.NewLine;
				Strsql += ",Tblforpagos.IntNaturaleza as Naturaleza" + Environment.NewLine;

				Strsql += ",TblDocPagos.DatFecha" + Environment.NewLine;
				Strsql += ",TblTransacciones.IntTipoPago" + Environment.NewLine;
				Strsql += ",TblTransacciones.IntCuadre as Cuadre" + Environment.NewLine;
				Strsql += "FROM TblForPagos " + Environment.NewLine;
				Strsql += "INNER JOIN TblDocPagos ON Tblforpagos.IntEmpresa = TblDocPagos.IntEmpresa And  Tblforpagos.IntTipoRecibo = TblDocPagos.IntTipoRecibo  AND Tblforpagos.IntRecibo = TblDocPagos.IntRecibo " + Environment.NewLine;
				Strsql += "INNER JOIN TblTransacciones ON TblDocPagos.IntEmpresa = TblTransacciones.IntEmpresa And TblDocPagos.IntTipoRecibo = TblTransacciones.IntidTransaccion " + Environment.NewLine;
				Strsql += "Where TblDocPagos.IntEmpresa = " + EmpresaActual + Environment.NewLine;
				Strsql += "And TblDocPagos.DatFecha >= '" + GetCalendarioAdmCampoDate(PAno, PPeriodo, "DatFechaInicial", conexion_sql) + "'" + Environment.NewLine;
				Strsql += "And TblDocPagos.DatFecha <= '" + GetCalendarioAdmCampoDate(PAno, PPeriodo, "DatFechaFinal", conexion_sql) + "'" + Environment.NewLine;
				Strsql += "And Tblforpagos.StrBanco = '" + PBanco + "'" + Environment.NewLine;
				// Strsql += "ORDER BY TblDocPagos.DatFecha, TblDocPagos.InttipoRecibo, TblDocPagos.IntRecibo, TblforPAgos.StrDocumento" & vbCrLf
				Strsql += "ORDER BY TblDocPagos.DatFecha, TblForPagos.IntRegistro,TblForpagos.IntTipoRecibo,TblForpagos.IntRecibo,TblForpagos.StrDocumento" + Environment.NewLine;

				Cmd = new Cl_EjecutarSQL(1, ServidorSQL, BaseDatosSql, ModoAut, UsrSql, PwdSql, IdAplicativo, Usuario);
				Lector = new DataTable();
				Lector = Cmd.SelectSqlDataSet(Strsql, "TB1").Tables[0];
				int J;
				int Id;
				J = 0;
				for (J = 0; J <= Lector.Rows.Count - 1; J++)
				{
					IntTipo = Convert.ToByte(Lector.Rows[J]["Naturaleza"]);
					Id = Convert.ToInt32(Lector.Rows[J]["Id"]);
					try
					{
						IntValor = Convert.ToDouble(Lector.Rows[J]["Valor"]);
					}
					catch (Exception ex)
					{
						IntValor = 0;
					}

					if (IntValor < 0)
					{
						if (IntTipo == 1)
							IntTipo = 2;
						else
							IntTipo = 1;
					}

					IntValor = Math.Abs(IntValor * Convert.ToDouble(Lector.Rows[J]["Cuadre"]));

					if (IntValor == 150000)
						IntTipo = IntTipo;

					if (IntTipo == 1)
						Saldo = Saldo + IntValor;
					else
						Saldo = Saldo - IntValor;
					if (Saldo != Convert.ToDouble(Lector.Rows[J]["Saldo"]))
						Cl_FuncionesBD.SetForPagoCampoDouble(Id, Convert.ToString(Lector.Rows[J]["Transaccion"]), Convert.ToInt64(Lector.Rows[J]["Documento"]), Convert.ToInt32(Lector.Rows[J]["ForPago"]), Convert.ToString(Lector.Rows[J]["Banco"]), Convert.ToString(Lector.Rows[J]["DocRef"]), "IntSaldo", Saldo, EmpresaActual, conexion_sql);
				}
				SetSaldoBancoCampoDouble(PBanco, PAno, PPeriodo, "IntSaldoF", Saldo, EmpresaActual, conexion_sql);
			}
			catch (Exception ex)
			{
			}
		}

		#region GET Campos String

		public static string GetTransaccionCampoString(string PTransaccion, string Pcampo, int empresa_actual)
		{
			string Strsql;
			Strsql = "SELECT Isnull(" + Pcampo + ",'')" + Environment.NewLine;
			Strsql += "FROM Tbltransacciones" + Environment.NewLine;
			Strsql += "where IntIdTransaccion = '" + PTransaccion + "'" + Environment.NewLine;
			Strsql += "And Intempresa = " + empresa_actual + Environment.NewLine;
			try
			{
				return RetornaSql(Strsql).ToString();
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return string.Empty;
			}
		}

		public static string GetEmpresaCampoString(string Pcampo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "SELECT Isnull(" + Pcampo + ",'')" + Environment.NewLine;
			Strsql += "FROM TblEmpresas" + Environment.NewLine;
			Strsql += "where IntIdEmpresa = " + EmpresaActual + Environment.NewLine;
			try
			{
				return RetornaSql(Strsql, conexion_sql).ToString();
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return string.Empty;
			}
		}

		public static string GetParametroCiaCampoString(string Pcampo, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "SELECT isnull(" + Pcampo + ",0)" + Environment.NewLine;
			Strsql += "FROM TblParametrosCia";

			try
			{
				return RetornaSql(Strsql, conexion_sql).ToString();
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return string.Empty;
			}
		}

		public static string GetUsuarioCampoString(string Pusuario, string PCampo, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "SELECT ISNULL(" + PCampo + ",'')" + Environment.NewLine;
			Strsql += "FROM TblUsuarios" + Environment.NewLine;
			Strsql += "where StrIdUsuario = '" + Pusuario + "'" + Environment.NewLine;
			try
			{
				return RetornaSql(Strsql, conexion_sql).ToString();
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return string.Empty;
			}
		}

		public static string GetTerceroCampoString(string PTercero, string Pcampo, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "select isnull(" + Pcampo + ",'')" + Environment.NewLine;
			Strsql += "from tblTerceros" + Environment.NewLine;
			Strsql += "where stridTercero = '" + PTercero + "'" + Environment.NewLine;
			try
			{
				return RetornaSql(Strsql, conexion_sql).ToString();
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return string.Empty;
			}
		}

		public static string GetEmpleadoCampoString(string PCodigo, string PCampo, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "select Isnull(" + PCampo + ",'') from tblempleados ";
			Strsql = Strsql + " where StrIdEmpleado = '" + PCodigo + "'";
			try
			{
				return RetornaSql(Strsql, conexion_sql).ToString();
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return string.Empty;
			}
		}

		public static string GetCampoTablaString(string PTabla, string Pcampo, string Pcondicion, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "select isnull(" + Pcampo + ",'')" + Environment.NewLine;
			Strsql += "from " + PTabla + Environment.NewLine;
			Strsql += Pcondicion + Environment.NewLine + Environment.NewLine;
			try
			{
				return RetornaSql(Strsql, conexion_sql).ToString();
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return string.Empty;
			}
		}

		public static string GetOrdenKardex(bool POrdenAsc, int IntCostoxLote, int EmpresaActual, string conexion_sql = "")
		{
			string Orden;
			if (POrdenAsc)
				Orden = " Asc ";
			else
				Orden = " Desc";
			string Strsql;
			Strsql = " ORDER BY TblProductos.StrIdProducto" + Orden + Environment.NewLine;

			if (IntCostoxLote == 1)
				Strsql += ",TblDetalleDocumentos.StrLote" + Orden + Environment.NewLine;

			Strsql += ",TblDetalleDocumentos.IntBodega" + Orden + Environment.NewLine;
			Strsql += ",cast(left(TblDocumentos.DatFecha,11) as datetime)" + Orden + Environment.NewLine;

			if (GetEmpresaCampoByte("IntProcesaPEntradas", EmpresaActual, conexion_sql) == 0)
				Strsql += ",TblDocumentos.DatFechaGra" + Orden + Environment.NewLine;

			Strsql += ",TblTransacciones.IntTipoTransaccion" + Orden + Environment.NewLine;
			Strsql += ",TblDocumentos.IntTransaccion" + Orden + Environment.NewLine;
			Strsql += ",TblDocumentos.IntDocumento " + Orden + Environment.NewLine;
			Strsql += ",TblDetalleDocumentos.StrSerie" + Orden + Environment.NewLine;
			Strsql += ",TblDetalleDocumentos.StrSerie1" + Orden + Environment.NewLine;
			return Strsql;
		}

		public static string GetFormatoCampoString(int PCodigo, string Pcampo, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "Select isnull(" + Pcampo + ",'')" + Environment.NewLine;
			Strsql += "From tblFormatosNet" + Environment.NewLine;
			Strsql += "Where IntIdFormato = " + PCodigo;
			try
			{
				return RetornaSql(Strsql, conexion_sql).ToString();
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return "0";
			}
		}

		public static string GetDocumentoCampoString(string PTransaccion, long PDocumento, string PCampo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "select LTRIM(RTRIM(" + PCampo + "))" + Environment.NewLine;
			Strsql += "From tblDocumentos" + Environment.NewLine;
			Strsql += "Where intempresa = " + EmpresaActual + Environment.NewLine;
			Strsql += "And inttransaccion = '" + PTransaccion + "'" + Environment.NewLine;
			Strsql += "And intdocumento = " + PDocumento + Environment.NewLine;

			try
			{
				return RetornaSql(Strsql, conexion_sql).ToString();
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}

		public static string GetDocPagosCampoString(string PTransaccion, long PDocumento, string PCampo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "select " + PCampo;
			Strsql = Strsql + " From TblDocPagos";
			Strsql = Strsql + " WHERE IntEmpresa = " + EmpresaActual;
			Strsql = Strsql + " AND IntTipoRecibo = '" + PTransaccion + "'";
			Strsql = Strsql + " AND IntRecibo = " + PDocumento;

			try
			{
				return RetornaSql(Strsql, conexion_sql).ToString();
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}

		#endregion

		#region GET Campos Integer

		public static int GetEmpleadoCampoInteger(string PCodigo, string PCampo, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "select Isnull(" + PCampo + ",0) " + Environment.NewLine;
			Strsql += "From TblEmpleados" + Environment.NewLine;
			Strsql += "where StrIdEmpleado = '" + PCodigo + "'" + Environment.NewLine;
			try
			{
				return Convert.ToInt32(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
		}

		public static int GetFormatoCampoInteger(int PCodigo, string Pcampo, string conexion_sql = "")
		{
			string Strsql;

			Strsql = "Select isnull(" + Pcampo + ",0)" + Environment.NewLine;
			Strsql += "From tblFormatosNet" + Environment.NewLine;
			Strsql += "Where IntIdFormato = " + PCodigo;
			try
			{
				return (int)RetornaSql(Strsql, conexion_sql);
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return 0;
			}
		}

		public static int GetEmpresaCampoInteger(string Pcampo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "SELECT isnull(" + Pcampo + ",0)" + Environment.NewLine;
			Strsql += "FROM TblEmpresas" + Environment.NewLine;
			Strsql += "Where IntIdEmpresa = " + EmpresaActual + Environment.NewLine;
			try
			{
				return Convert.ToInt32(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				//AMTRCl_Funciones.DesplegarError(ex);
				return 0;
			}
		}

		public static int GetCalendarioNomCampoInteger(int PintAno, int PintPeriodo, string Pcampo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;

			Strsql = "SELECT " + Pcampo + Environment.NewLine;
			Strsql += "FROM TBLCalendarioNom" + Environment.NewLine;
			Strsql += "WHERE IntEmpresa = " + EmpresaActual + Environment.NewLine;
			Strsql += "And intano = " + PintAno + Environment.NewLine;
			Strsql += "And intPeriodo = " + PintPeriodo + Environment.NewLine;
			try
			{
				return Convert.ToInt32(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		public static int GetTransaccionCampoInteger(string pTransaccion, string Pcampo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "SELECT isnull(" + Pcampo + ",0) FROM Tbltransacciones where IntIdTransaccion = '" + pTransaccion + "'";
			Strsql = Strsql + "  And Intempresa = " + EmpresaActual;
			try
			{
				return Convert.ToInt32(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return 0;
			}
		}


		#endregion

		#region GET Campos Byte 

		public static byte GetEmpresaCampoByte(string Pcampo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "SELECT isnull(" + Pcampo + ",0)" + Environment.NewLine;
			Strsql += "FROM TblEmpresas" + Environment.NewLine;
			Strsql += "Where IntIdEmpresa = " + EmpresaActual + Environment.NewLine;
			try
			{
				return Convert.ToByte(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				/*AMTR if (Cl_VariablesGlobales.IdAplicativo != 6)
					Cl_Funciones.DesplegarError(ex);*/
				return 0;
			}
		}

		public static byte GetAnoPeriodosAno(int PintAno, byte PbytApl, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql = "";
			switch (PbytApl)
			{
				case 1:
				case 4:
					{
						Strsql = "select intPeriodosAdm" + Environment.NewLine;
						break;
					}
				case 2:
					{
						Strsql = "select intPeriodosCon" + Environment.NewLine;
						break;
					}
				case 3:
					{
						Strsql = "select intPeriodosNom" + Environment.NewLine;
						break;
					}
				default:
					{
						Strsql = "select intPeriodos" + Environment.NewLine;
						break;
					}
			}
			switch (PbytApl)
			{
				case 3:
					{
						Strsql += "From TblEmpresas Where IntIdEmpresa =" + EmpresaActual + Environment.NewLine;
						break;
					}
				default:
					{
						Strsql += "From tblAnos where IntIdAno = " + PintAno + Environment.NewLine;
						break;
					}
			}

			try
			{
				return Convert.ToByte(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		public static byte GetTransaccionCampoByte(string pTransaccion, string Pcampo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "SELECT isnull(" + Pcampo + ",0)" + Environment.NewLine;
			Strsql += "FROM Tbltransacciones" + Environment.NewLine;
			Strsql += "where IntIdTransaccion = '" + pTransaccion + "'" + Environment.NewLine;
			Strsql += "And Intempresa = " + EmpresaActual;
			try
			{
				return Convert.ToByte(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				/*AMTR if (Cl_VariablesGlobales.IdAplicativo == 10 | Cl_VariablesGlobales.IdAplicativo == 11)
				{
				}
				else
					Cl_Funciones.DesplegarError(ex, Strsql);*/

				return 0;
			}
		}

		public static byte GetUsuarioCampoByte(string Pusuario, string PCampo, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "SELECT isnull(" + PCampo + ",0) FROM TblUsuarios where StrIdUsuario = '" + Pusuario + "'";
			try
			{
				return Convert.ToByte(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		#endregion

		#region GET double

		public static double GetBancoSaldoCampoDouble(string PstrBanco, int PintAno, int PintPeriodo, string PCampo, bool PAnterior, int EmpresaActual, int AgnoActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "SELECT " + PCampo;
			Strsql = Strsql + " From TblSaldosBancos";
			Strsql = Strsql + " WHERE Strbanco = '" + PstrBanco + "'";
			Strsql = Strsql + " And IntEmpresa  = " + EmpresaActual;
			if (PAnterior)
			{
				if (PintPeriodo == 1)
				{
					Strsql = Strsql + " AND Intano = " + (PintAno - 1);
					Strsql = Strsql + " AND IntPeriodo = " + GetAnoPeriodosAno(AgnoActual, 1, EmpresaActual, conexion_sql);
				}
				else
				{
					Strsql = Strsql + " AND Intano = " + PintAno;
					Strsql = Strsql + " AND IntPeriodo = " + (PintPeriodo - 1);
				}
			}
			else
			{
				Strsql = Strsql + " AND Intano = " + PintAno;
				Strsql = Strsql + " AND IntPeriodo = " + PintPeriodo;
			}
			try
			{
				return Convert.ToDouble(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		public static void SetSaldoBancoCampoDouble(string PBanco, int Pano, int PPeriodo, string Pcampo, double PValor, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			if (ValidaSaldoBanco(PBanco, Pano, PPeriodo, EmpresaActual, conexion_sql) == false)
			{
				Strsql = "INSERT INTO TblSaldosBancos ( IntAno, IntPeriodo, StrBanco,IntEmpresa)";
				Strsql = Strsql + " SELECT " + Pano + " AS Expr1, " + PPeriodo + " AS Expr2, '" + PBanco + "' AS Expr3, " + EmpresaActual + " as Expr4";

				Cl_Funciones.EjecutaInstruccion(Strsql, 1, conexion_sql);
			}

			Strsql = "UPDATE TblSaldosBancos SET " + Pcampo + " = " + PValor;
			Strsql = Strsql + " WHERE IntEmpresa = " + EmpresaActual;
			Strsql = Strsql + " And IntAno = " + Pano;
			Strsql = Strsql + " AND IntPeriodo = " + PPeriodo;
			Strsql = Strsql + " AND StrBanco = '" + PBanco + "'";
			Cl_Funciones.EjecutaInstruccion(Strsql, 1, conexion_sql);
		}

		public static string SetForPagoCampoDouble(int PId, string PTransaccion, long PDocumento, int PForPago, string PBanco, string PdocRef, string PCampo, double Pvalor, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "Update TblForPagos Set " + PCampo + "= " + Pvalor + Environment.NewLine;
			Strsql += "WHERE IntRegistro = " + PId + Environment.NewLine;
			Strsql += "And IntEmpresa = " + EmpresaActual + Environment.NewLine;
			Strsql += "AND IntTipoRecibo = '" + PTransaccion + "'" + Environment.NewLine;
			Strsql += "AND IntRecibo = " + PDocumento + Environment.NewLine;
			Strsql += "AND IntForPago = " + PForPago + Environment.NewLine;
			Strsql += "AND StrBanco = '" + PBanco + "'" + Environment.NewLine;
			Strsql += "AND StrDocumento = '" + PdocRef + "'" + Environment.NewLine;

			try
			{
				return Cl_Funciones.EjecutaInstruccion(Strsql, 1, conexion_sql);
			}
			catch (Exception ex)
			{
				return "";
			}
		}


		#endregion

		#region GET Boolean

		public static bool ValidaSaldoBanco(string PBanco, int PAno, int PPeriodo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			int Veces;

			Strsql = "Select Count(StrBanco)  From TblSaldosBancos ";
			Strsql = Strsql + " WHERE IntEmpresa = " + EmpresaActual;
			Strsql = Strsql + " And IntAno = " + PAno;
			Strsql = Strsql + " AND IntPeriodo = " + PPeriodo;
			Strsql = Strsql + " AND StrBanco = '" + PBanco + "'";
			try
			{
				Veces = Convert.ToInt32(RetornaSql(Strsql, conexion_sql));
				if (Veces > 0)
					return true;
				else
					return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		#endregion

		#region GET DateTime

		public static DateTime GetCalendarioAdmCampoDate(int PIntAno, int PIntPeriodo, string PCampo, string conexion_sql = "")
		{
			string Strsql;

			Strsql = "SELECT " + PCampo + Environment.NewLine;
			Strsql += "FROM TBLCalendarioAdm" + Environment.NewLine;
			Strsql += "Where intano = " + PIntAno + Environment.NewLine;
			Strsql += "And intPeriodo = " + PIntPeriodo + Environment.NewLine;
			try
			{
				return Convert.ToDateTime(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				return new DateTime();
			}
		}

		public static DateTime GetDocumentoCampoFecha(string PTransaccion, long PDocumento, string PCampo, int EmpresaActual, string conexion_sql = "")
		{
			string Strsql;
			Strsql = "select " + PCampo;
			Strsql = Strsql + " from tblDocumentos ";
			Strsql = Strsql + " Where intempresa = " + EmpresaActual;
			Strsql = Strsql + " And inttransaccion = '" + PTransaccion + "'";
			Strsql = Strsql + " And intdocumento = " + PDocumento;

			try
			{
				return Convert.ToDateTime(RetornaSql(Strsql, conexion_sql));
			}
			catch (Exception ex)
			{
				// Cl_Funciones.DesplegarError(ex)
				return new DateTime();
			}
		}


		#endregion

		#region GET DataTable

		public static DataTable GetParametrosCorreo(string PProceso, int EmpresaActual, string ServidorSQL, string BaseDatosSql, int ModoAut, string UsrSql, string PwdSql, int IdAplicativo, string Usuario)
		{
			DataTable Dt = new DataTable();
			string Strsql = string.Empty;
			try
			{
				Strsql = "Select  	* " + Environment.NewLine;
				Strsql += "from TblParametrosCorreo" + Environment.NewLine;
				Strsql += "Where IntIdEmpresa = " + EmpresaActual + Environment.NewLine;
				Strsql += "And StrTipoCorreo='" + PProceso + "'";
				var CMD = new Cl_EjecutarSQL(1, ServidorSQL, BaseDatosSql, ModoAut, UsrSql, PwdSql, IdAplicativo, Usuario);
				Dt = CMD.SelectSqlDataSet(Strsql, "TB1").Tables["TB1"];

				if (Dt.Rows.Count == 0)
				{
					Strsql = "Select  	*  " + Environment.NewLine;

					Strsql += "from TblParametrosCorreo" + Environment.NewLine;
					Strsql += "Where IntIdEmpresa = " + EmpresaActual + Environment.NewLine;
					Strsql += "And StrTipoCorreo='*'";
					Dt = CMD.SelectSqlDataSet(Strsql, "TB1").Tables["TB1"];
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
			return Dt;
		}

		#endregion

		#region Qry

		public static void QryCarteraEdad(DateTime PFecha, string conexion_sql = "")
		{
			string StrSql;

			Cl_Version.EstablecerConfiguracionRegional();

			StrSql = "Select IntEmpresa" + Environment.NewLine;
			StrSql += ",IntAno" + Environment.NewLine;
			StrSql += ",IntPeriodo" + Environment.NewLine;
			StrSql += ",IntTransaccion" + Environment.NewLine;
			StrSql += ",IntDocumento" + Environment.NewLine;
			StrSql += ",StrClase" + Environment.NewLine;
			StrSql += ",IntCuota" + Environment.NewLine;
			StrSql += ",max(StrPrefijo) as StrPrefijo" + Environment.NewLine;
			StrSql += ",StrGrupo" + Environment.NewLine;
			StrSql += ",StrTipoCartera" + Environment.NewLine;
			StrSql += ",max(DatFecha) as DatFecha" + Environment.NewLine;
			StrSql += ",max(DatVencimiento) as DatVencimiento" + Environment.NewLine;
			StrSql += ",max(DatVencimientoDoc) as DatVencimientoDoc" + Environment.NewLine;
			StrSql += ",cast('" + PFecha + "' as DateTime)  as DatFechaActual" + Environment.NewLine;
			StrSql += ",Sum(IntVrDocumento) as IntVrDocumento" + Environment.NewLine;
			StrSql += ",Sum(IntSaldoI) as IntSaldoI" + Environment.NewLine;
			StrSql += ",Sum(IntIngresos) as IntIngresos" + Environment.NewLine;
			StrSql += ",Sum(IntPagos) as IntPagos" + Environment.NewLine;
			StrSql += ",Sum(IntSaldoI + IntIngresos - IntPagos) as IntSaldoF" + Environment.NewLine;
			StrSql += ",Sum(IntIntereses) as IntIntereses" + Environment.NewLine;

			StrSql += "from qrycarteradet" + Environment.NewLine;
			StrSql += "Group by IntEmpresa" + Environment.NewLine;
			StrSql += ",IntAno" + Environment.NewLine;
			StrSql += ",IntPeriodo" + Environment.NewLine;
			StrSql += ",IntTransaccion" + Environment.NewLine;
			StrSql += ",IntDocumento" + Environment.NewLine;
			StrSql += ",StrClase" + Environment.NewLine;
			StrSql += ",IntCuota" + Environment.NewLine;
			StrSql += ",StrGrupo" + Environment.NewLine;
			StrSql += ",StrTipoCartera" + Environment.NewLine;

			ActualizaConsulta("QryCarteraEdad", StrSql, conexion_sql);
		}

		#endregion

		#region Script
		public static string StringEdadPago()
		{
			// Return "CAST(TblDocPagos.DatFechaPago AS int) - CAST(TblDocumentos.DatFecha AS int)"
			// StrSql = "Select Sum(DATEDIFF(day,TblDocumentos.DatVencimiento,TblDocPagos.datFecha))/Count(TblDetallePagos.IntDocumento) AS Expr1" & vbCrLf
			return "DATEDIFF(day,TblDocumentos.DatVencimiento,TblDocPagos.datFecha)" + Environment.NewLine;
		}

		#endregion

		public static bool ValidaEmail(string PSql, byte POrigen)
		{
			string StrSql;
			StrSql = PSql;

			StrSql += " Where (Mail  not like '%@%'" + Environment.NewLine;
			StrSql += "or Mail  is null)" + Environment.NewLine;

			string Strmsg;

			Strmsg = "Las Siguientes Correos Electronicos no son válidos" + Environment.NewLine;
			Strmsg += "Estos mensajes no se enviarán" + Environment.NewLine;
			// If Cl_FuncionesForms.MensajedeInconsistencia(StrSql, Strmsg) = False Then
			// Return False
			// End If
			return true;
			//AMTR return Cl_FuncionesForms.MensajedeInconsistencia(StrSql, Strmsg);
		}

		public static int GetCorreoEnvio(string conexion_sql = "")
		{
			string Strsql;
			Strsql = "select Max(IntEnvio)" + Environment.NewLine;
			Strsql += "from tblCorreosEnviados" + Environment.NewLine;
			try
			{
				return Convert.ToInt32(RetornaSql(Strsql, conexion_sql)) + 1;
			}
			catch (InvalidCastException ex)
			{
				return 1;
			}
			catch (Exception ex)
			{
				//AMTR Cl_Funciones.DesplegarError(ex);
				return 0;
			}
		}

		public static string GetCondRadioG(string hgiRadioG, string Pcond, int EmpresaActual)
		{
			//GetCondRadioG = "";
			string Str;
			Str = " And " + Pcond + " IN (Select IntIdTransaccion From TblTransacciones ";
			Str = Str + " Where  IntEmpresa = " + EmpresaActual;

			switch (hgiRadioG)
			{
				case "CxC":
					{
						Str = Str + " And  IntCxC = 1";
						break;
					}

				case "CxP":
					{
						Str = Str + " And  IntCxP = 1";
						break;
					}
			}
			Str = Str + ")";
			return Str;
		}

		public static bool TblCorreosBloqueadosValida(string PEmail)
		{
			string Strsql;
			int Registros;

			Strsql = "SELECT " + Environment.NewLine;
			Strsql += "Count(Strmail) From TblCorreosBloqueados" + Environment.NewLine;
			Strsql += "Where StrMail = '" + PEmail + "'" + Environment.NewLine;
			try
			{
				Registros = Convert.ToInt32(RetornaSql(Strsql));
				if (Registros > 0)
					return true;
				else
					return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static string TblCorreosBloqueadosDescripcion(string PEmail)
		{
			string Strsql;

			Strsql = "SELECT " + Environment.NewLine;
			Strsql += "StrDescripcion From TblCorreosBloqueados" + Environment.NewLine;
			Strsql += "Where StrMail = '" + PEmail + "'" + Environment.NewLine;
			try
			{
				return RetornaSql(Strsql).ToString();
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}


		public static DataTable MuestraResultadoMail(int PEnvio, int EmpresaActual, string ServidorSQL, string BaseDatosSql, int ModoAut, string UsrSql, string PwdSql, int IdAplicativo, string Usuario)
		{
			//string conexion_sql = Cl_EjecutarSQL.StringConeccionSQL(ServidorSQL, BaseDatosSql, ModoAut, UsrSql, PwdSql, IdAplicativo, Usuario);

			DataTable Dt = new DataTable();
			string StrSql = string.Empty;
			try
			{
				StrSql = "Select DatFecha as Fecha" + Environment.NewLine;
				StrSql += ",StrMail as Mail" + Environment.NewLine;
				StrSql += ",StrAsunto as Asunto" + Environment.NewLine;
				StrSql += ",StrAdjunto as Adjunto" + Environment.NewLine;
				StrSql += ",StrResultado as Resultado" + Environment.NewLine;

				StrSql += "from TblCorreosEnviados " + Environment.NewLine;
				StrSql += "Where IntEnvio = " + PEnvio + Environment.NewLine;
				// StrSql += "Order By StrResultado " & vbCrLf

				var CMD = new Cl_EjecutarSQL(1, ServidorSQL, BaseDatosSql, ModoAut, UsrSql, PwdSql, IdAplicativo, Usuario);
				Dt = CMD.SelectSqlDataSet(StrSql, "TB1").Tables["TB1"];

			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex.InnerException);
			}
			return Dt;

			/*string Strmsg;
			Strmsg = "Resultado de Envio de Mensajes" + Environment.NewLine;
			
			 Return Cl_FuncionesForms.MensajedeInconsistencia(StrSql, Strmsg, False)*/

		}


	}
}
