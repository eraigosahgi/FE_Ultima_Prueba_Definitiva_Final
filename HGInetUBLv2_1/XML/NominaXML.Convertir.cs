using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class NominaXML
	{

		public static Nomina Convertir(NominaIndividualType nomina_ubl, TblDocumentos documento_bd)
		{
			Nomina nomina_objeto = new Nomina();

			try
			{
				if (nomina_ubl.Empleador != null)
				{
					nomina_objeto.DatosEmpleador = new Empleador();
					nomina_objeto.DatosEmpleador = ParticipantesNominaXML.Obtener_Empleador(nomina_ubl.Empleador, 0);
				}

				if (nomina_ubl.Trabajador != null)
				{
					nomina_objeto.DatosTrabajador = new Trabajador();
					nomina_objeto.DatosTrabajador = ParticipantesNominaXML.Obtener_Trabajador(nomina_ubl.Trabajador, 0);
				}

				if (nomina_ubl.NumeroSecuenciaXML != null)
				{
					nomina_objeto.Prefijo = nomina_ubl.NumeroSecuenciaXML.Prefijo;
					nomina_objeto.Documento = Convert.ToInt64(nomina_ubl.NumeroSecuenciaXML.Consecutivo);
					if (nomina_objeto.DatosTrabajador != null && !string.IsNullOrEmpty(nomina_ubl.NumeroSecuenciaXML.CodigoTrabajador))
						nomina_objeto.DatosTrabajador.CodigoTrabajador = nomina_ubl.NumeroSecuenciaXML.CodigoTrabajador;
				}

				if (nomina_ubl.Periodo != null)
				{
					nomina_objeto.DatosPeriodo = new Periodo();
					nomina_objeto.DatosPeriodo.FechaIngreso = nomina_ubl.Periodo.FechaIngreso;
					nomina_objeto.DatosPeriodo.FechaLiquidacionInicio = nomina_ubl.Periodo.FechaLiquidacionInicio;
					nomina_objeto.DatosPeriodo.FechaLiquidacionFin = nomina_ubl.Periodo.FechaLiquidacionFin;
					nomina_objeto.DatosPeriodo.TiempoLaborado = Convert.ToInt32(nomina_ubl.Periodo.TiempoLaborado);

				}

				if (nomina_ubl.InformacionGeneral != null)
				{
					DateTime hora = Convert.ToDateTime(nomina_ubl.InformacionGeneral.HoraGen);
					DateTime fecha = nomina_ubl.InformacionGeneral.FechaGen;
					DateTime fecha_hora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);
					nomina_objeto.FechaGen = fecha_hora;
					nomina_objeto.Cune = nomina_ubl.InformacionGeneral.CUNE;
					nomina_objeto.PeriodoNomina = Convert.ToInt32(nomina_ubl.InformacionGeneral.PeriodoNomina);
					nomina_objeto.Moneda = nomina_ubl.InformacionGeneral.TipoMoneda;
				}

				if (nomina_ubl.Pago != null)
				{
					nomina_objeto.DatosPago = new Pago();
					nomina_objeto.DatosPago.Forma = Convert.ToInt32(nomina_ubl.Pago.Forma);
					nomina_objeto.DatosPago.Metodo = Convert.ToInt32(nomina_ubl.Pago.Metodo);
					nomina_objeto.DatosPago.TipoCuenta = nomina_ubl.Pago.TipoCuenta;
					nomina_objeto.DatosPago.Banco = nomina_ubl.Pago.Banco;
					nomina_objeto.DatosPago.NumeroCuenta = nomina_ubl.Pago.NumeroCuenta;
				}

				if (nomina_ubl.FechasPagos != null && nomina_ubl.FechasPagos.Length > 0)
				{
					nomina_objeto.FechasPagos = new List<DateTime>();
					nomina_objeto.FechasPagos = nomina_ubl.FechasPagos.ToList();
				}

				nomina_objeto.DevengadosTotal = nomina_ubl.DevengadosTotal;
				nomina_objeto.DeduccionesTotal = nomina_ubl.DeduccionesTotal;
				nomina_objeto.ComprobanteTotal = nomina_ubl.ComprobanteTotal;

				nomina_objeto.CodigoRegistro = documento_bd.StrObligadoIdRegistro;
				nomina_objeto.NumeroResolucion = documento_bd.StrNumResolucion;

				if (nomina_ubl.Notas != null && nomina_ubl.Notas.Length > 0)
				{
					nomina_objeto.Notas = new List<string>();
					nomina_objeto.Notas = nomina_ubl.Notas.ToList();
				}

				if (nomina_ubl.Devengados != null)
				{
					nomina_objeto.DatosDevengados = NovedadesNominaXML.Obtener_Devengados(nomina_ubl.Devengados, 0);
				}

				if (nomina_ubl.Deducciones != null)
				{
					nomina_objeto.DatosDeducciones = NovedadesNominaXML.Obtener_Deducciones(nomina_ubl.Deducciones, 0);
				}
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Convirtiendo Documento de Nomina";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(string.Format("{0} - {1}", msg_custom, excepcion.Message), excepcion.InnerException);
			}

			return nomina_objeto;
		}


	}
}
