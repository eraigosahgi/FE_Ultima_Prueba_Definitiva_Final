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
	public partial class NominaAjusteXML
	{
		public static NominaAjuste Convertir(NominaIndividualDeAjusteType nomina_ubl, TblDocumentos documento_bd)
		{
			NominaAjuste nomina_objeto = new NominaAjuste();

			nomina_objeto.TipoNota = 1;

			try
			{
			   
				if (nomina_ubl.TipoNota.Equals("1"))
				{
					NominaIndividualDeAjusteTypeReemplazar reemplazo  = new NominaIndividualDeAjusteTypeReemplazar();

					reemplazo = nomina_ubl.Reemplazar;

					nomina_objeto.NumeroPred = reemplazo.ReemplazandoPredecesor.NumeroPred;
					nomina_objeto.CUNEPred = reemplazo.ReemplazandoPredecesor.CUNEPred;
					nomina_objeto.FechaGenPred = reemplazo.ReemplazandoPredecesor.FechaGenPred;

					if (reemplazo.Empleador != null)
					{
						nomina_objeto.DatosEmpleador = new Empleador();
						nomina_objeto.DatosEmpleador = ParticipantesNominaXML.Obtener_Empleador(reemplazo.Empleador, 1);
					}

					if (reemplazo.Trabajador != null)
					{
						nomina_objeto.DatosTrabajador = new Trabajador();
						nomina_objeto.DatosTrabajador = ParticipantesNominaXML.Obtener_Trabajador(reemplazo.Trabajador, 1);
					}

					if (reemplazo.NumeroSecuenciaXML != null)
					{
						nomina_objeto.Prefijo = reemplazo.NumeroSecuenciaXML.Prefijo;
						nomina_objeto.Documento = Convert.ToInt64(reemplazo.NumeroSecuenciaXML.Consecutivo);
						if (nomina_objeto.DatosTrabajador != null && !string.IsNullOrEmpty(reemplazo.NumeroSecuenciaXML.CodigoTrabajador))
							nomina_objeto.DatosTrabajador.CodigoTrabajador = reemplazo.NumeroSecuenciaXML.CodigoTrabajador;
					}

					if (reemplazo.Periodo != null)
					{
						nomina_objeto.DatosPeriodo = new Periodo();
						nomina_objeto.DatosPeriodo.FechaIngreso = reemplazo.Periodo.FechaIngreso;
						nomina_objeto.DatosPeriodo.FechaLiquidacionInicio = reemplazo.Periodo.FechaLiquidacionInicio;
						nomina_objeto.DatosPeriodo.FechaLiquidacionFin = reemplazo.Periodo.FechaLiquidacionFin;
						nomina_objeto.DatosPeriodo.TiempoLaborado = Convert.ToInt32(reemplazo.Periodo.TiempoLaborado);

					}

					if (reemplazo.InformacionGeneral != null)
					{
						DateTime hora = Convert.ToDateTime(reemplazo.InformacionGeneral.HoraGen);
						DateTime fecha = reemplazo.InformacionGeneral.FechaGen;
						DateTime fecha_hora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);
						nomina_objeto.FechaGen = fecha_hora;
						nomina_objeto.Cune = reemplazo.InformacionGeneral.CUNE;
						nomina_objeto.PeriodoNomina = Convert.ToInt32(reemplazo.InformacionGeneral.PeriodoNomina);
						nomina_objeto.Moneda = reemplazo.InformacionGeneral.TipoMoneda;
					}

					if (reemplazo.Pago != null)
					{
						nomina_objeto.DatosPago = new Pago();
						nomina_objeto.DatosPago.Forma = Convert.ToInt32(reemplazo.Pago.Forma);
						nomina_objeto.DatosPago.Metodo = Convert.ToInt32(reemplazo.Pago.Metodo);
						nomina_objeto.DatosPago.TipoCuenta = reemplazo.Pago.TipoCuenta;
						nomina_objeto.DatosPago.Banco = reemplazo.Pago.Banco;
						nomina_objeto.DatosPago.NumeroCuenta = reemplazo.Pago.NumeroCuenta;
					}

					if (reemplazo.FechasPagos != null && reemplazo.FechasPagos.Length > 0)
					{
						nomina_objeto.FechasPagos = new List<DateTime>();
						nomina_objeto.FechasPagos = reemplazo.FechasPagos.ToList();
					}

					nomina_objeto.DevengadosTotal = reemplazo.DevengadosTotal;
					nomina_objeto.DeduccionesTotal = reemplazo.DeduccionesTotal;
					nomina_objeto.ComprobanteTotal = reemplazo.ComprobanteTotal;

					nomina_objeto.CodigoRegistro = documento_bd.StrObligadoIdRegistro;
					nomina_objeto.NumeroResolucion = documento_bd.StrNumResolucion;

					if (reemplazo.Notas != null && reemplazo.Notas.Length > 0)
					{
						nomina_objeto.Notas = new List<string>();
						nomina_objeto.Notas = reemplazo.Notas.ToList();
					}

					if (reemplazo.Devengados != null)
					{
						nomina_objeto.DatosDevengados = NovedadesNominaXML.Obtener_Devengados(reemplazo.Devengados, 1);
					}

					if (reemplazo.Deducciones != null)
					{
						nomina_objeto.DatosDeducciones = NovedadesNominaXML.Obtener_Deducciones(reemplazo.Deducciones, 1);
					}


				}
				else
				{
					NominaIndividualDeAjusteTypeEliminar eliminar = new NominaIndividualDeAjusteTypeEliminar();
					eliminar = nomina_ubl.Eliminar;

					nomina_objeto.TipoNota = 2;

					nomina_objeto.NumeroPred = eliminar.EliminandoPredecesor.NumeroPred;
					nomina_objeto.CUNEPred = eliminar.EliminandoPredecesor.CUNEPred;
					nomina_objeto.FechaGenPred = eliminar.EliminandoPredecesor.FechaGenPred;

					if (eliminar.Empleador != null)
					{
						nomina_objeto.DatosEmpleador = new Empleador();
						nomina_objeto.DatosEmpleador = ParticipantesNominaXML.Obtener_Empleador(eliminar.Empleador, 1);
					}

					if (eliminar.NumeroSecuenciaXML != null)
					{
						nomina_objeto.Prefijo = eliminar.NumeroSecuenciaXML.Prefijo;
						nomina_objeto.Documento = Convert.ToInt64(eliminar.NumeroSecuenciaXML.Consecutivo);
					}

					if (eliminar.InformacionGeneral != null)
					{
						DateTime hora = Convert.ToDateTime(eliminar.InformacionGeneral.HoraGen);
						DateTime fecha = eliminar.InformacionGeneral.FechaGen;
						DateTime fecha_hora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hour, hora.Minute, hora.Second);
						nomina_objeto.FechaGen = fecha_hora;
						nomina_objeto.Cune = eliminar.InformacionGeneral.CUNE;
					}

					if (eliminar.Notas != null && eliminar.Notas.Length > 0)
					{
						nomina_objeto.Notas = new List<string>();
						nomina_objeto.Notas = eliminar.Notas.ToList();
					}
				}	

				
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Convirtiendo Documento de Ajuste de Nomina";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(string.Format("{0} - {1}", msg_custom, excepcion.Message), excepcion.InnerException);
			}

			return nomina_objeto;
		}
	}
}
