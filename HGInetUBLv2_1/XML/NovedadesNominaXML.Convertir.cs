using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
{
	public partial class NovedadesNominaXML
	{

		public static Devengados Obtener_Devengados(object devengado_xml, int tipo_doc)
		{

			Devengados devengado_doc = new Devengados();

			try
			{
				var obj = (dynamic)null;

				if (tipo_doc == 0)
				{
					obj = new NominaIndividualTypeDevengados();
					obj = devengado_xml;
				}
				else if (tipo_doc == 1)
				{
					obj = new NominaIndividualDeAjusteTypeReemplazarDevengados();
					obj = devengado_xml;
				}

				if (obj.Basico != null)
				{
					devengado_doc.DiasTrabajados = Convert.ToInt32(obj.Basico.DiasTrabajados);
					devengado_doc.SueldoTrabajado = obj.Basico.SueldoTrabajado;
				}

				devengado_doc.ApoyoSostenimiento = obj.ApoyoSost;

				if (obj.Anticipos != null && obj.Anticipos.Length > 0)
				{
					devengado_doc.Anticipos = obj.Anticipos.ToList();
				}

				if (obj.Auxilios != null && obj.Auxilios.Length > 0)
				{

					foreach (var item in obj.Auxilios)
					{
						NovedadSalNoSal novedad = new NovedadSalNoSal();
						novedad.Pago = item.AuxilioS;
						novedad.PagoNS = item.AuxilioNS;
						devengado_doc.Auxilios.Add(novedad);

					}

				}

				if (obj.Bonificaciones != null && obj.Bonificaciones.Length > 0)
				{
					foreach (var item in obj.Bonificaciones)
					{
						NovedadSalNoSal novedad = new NovedadSalNoSal();
						novedad.Pago = item.BonificacionS;
						novedad.PagoNS = item.BonificacionNS;
						devengado_doc.Bonificaciones.Add(novedad);

					}
				}

				devengado_doc.BonifRetiro = obj.BonifRetiro;

				if (obj.BonoEPCTVs != null && obj.BonoEPCTVs.Length > 0)
				{
					foreach (var item in obj.BonoEPCTVs)
					{
						NovedadSalNoSal novedad = new NovedadSalNoSal();
						novedad.Pago = item.PagoS;
						novedad.PagoNS = item.PagoNS;
						if ((novedad.Pago > 0 || novedad.PagoNS > 0) && devengado_doc.BonoEPCTV == null)
						{
							devengado_doc.BonoEPCTV = new List<NovedadSalNoSal>();
							devengado_doc.BonoEPCTV.Add(novedad);
						}
						else if (devengado_doc.BonoEPCTV != null)
						{
							devengado_doc.BonoEPCTV.Add(novedad);
						}

						novedad = new NovedadSalNoSal();
						novedad.Pago = item.PagoAlimentacionS;
						novedad.PagoNS = item.PagoAlimentacionNS;
						if ((novedad.Pago > 0 || novedad.PagoNS > 0) && devengado_doc.BonoAlimentacion == null)
						{
							devengado_doc.BonoAlimentacion = new List<NovedadSalNoSal>();
							devengado_doc.BonoAlimentacion.Add(novedad);
						}
						else if (devengado_doc.BonoAlimentacion != null)
						{
							devengado_doc.BonoAlimentacion.Add(novedad);
						}
					}
				}

				if (obj.Cesantias != null)
				{
					devengado_doc.PagoCesantias = new Cesantias();
					devengado_doc.PagoCesantias.Pago = Convert.ToDecimal(obj.Cesantias.Pago);
					devengado_doc.PagoCesantias.PagoIntereses = obj.Cesantias.PagoIntereses;
					devengado_doc.PagoCesantias.Porcentaje = obj.Cesantias.Porcentaje;
				}

				if (obj.Comisiones != null && obj.Comisiones.Length > 0)
				{
					devengado_doc.Comisiones = new List<decimal>();
					devengado_doc.Comisiones = obj.Comisiones.ToList();
				}

				if (obj.Compensaciones != null && obj.Compensaciones.Length > 0)
				{
					devengado_doc.Compensaciones = new List<NovedadSalNoSal>();

					foreach (var item in obj.Compensaciones)
					{
						NovedadSalNoSal novedad = new NovedadSalNoSal();
						novedad.Pago = item.CompensacionO;
						novedad.PagoNS = item.CompensacionE;
						devengado_doc.Bonificaciones.Add(novedad);
					}

				}

				devengado_doc.Dotacion = obj.Dotacion;

				if (obj.HEDDFs != null && obj.HEDDFs.Length > 0)
				{
					devengado_doc.DatosHoras = new List<Hora>();

					foreach (var item in obj.HEDDFs)
					{
						Hora novedad = new Hora();
						novedad.Cantidad = item.Cantidad;
						novedad.TipoHora = TipoHoraNomina.RecargoDominicalesFestivas.GetHashCode();
						novedad.CodigoConcepto = Enumeracion.GetDescription(TipoHoraNomina.RecargoDominicalesFestivas);
						novedad.Porcentaje = item.Porcentaje;
						novedad.Valor = item.Pago;

						devengado_doc.DatosHoras.Add(novedad);
					}
				}

				if (obj.HEDs != null && obj.HEDs.Length > 0)
				{
					if (devengado_doc.DatosHoras == null)
					{
						devengado_doc.DatosHoras = new List<Hora>();
					}

					foreach (var item in obj.HEDs)
					{
						Hora novedad = new Hora();
						novedad.Cantidad = item.Cantidad;
						novedad.TipoHora = TipoHoraNomina.ExtraDiurna.GetHashCode();
						novedad.CodigoConcepto = Enumeracion.GetDescription(TipoHoraNomina.ExtraDiurna);
						novedad.Porcentaje = item.Porcentaje;
						novedad.Valor = item.Pago;

						devengado_doc.DatosHoras.Add(novedad);
					}
				}

				if (obj.HENDFs != null && obj.HENDFs.Length > 0)
				{
					if (devengado_doc.DatosHoras == null)
					{
						devengado_doc.DatosHoras = new List<Hora>();
					}

					foreach (var item in obj.HENDFs)
					{
						Hora novedad = new Hora();
						novedad.Cantidad = item.Cantidad;
						novedad.TipoHora = TipoHoraNomina.ExtraDominicalesFestivasNocturnas.GetHashCode();
						novedad.CodigoConcepto = Enumeracion.GetDescription(TipoHoraNomina.ExtraDominicalesFestivasNocturnas);
						novedad.Porcentaje = item.Porcentaje;
						novedad.Valor = item.Pago;

						devengado_doc.DatosHoras.Add(novedad);
					}
				}

				if (obj.HENs != null && obj.HENs.Length > 0)
				{
					if (devengado_doc.DatosHoras == null)
					{
						devengado_doc.DatosHoras = new List<Hora>();
					}

					foreach (var item in obj.HENs)
					{
						Hora novedad = new Hora();
						novedad.Cantidad = item.Cantidad;
						novedad.TipoHora = TipoHoraNomina.ExtraNocturna.GetHashCode();
						novedad.CodigoConcepto = Enumeracion.GetDescription(TipoHoraNomina.ExtraNocturna);
						novedad.Porcentaje = item.Porcentaje;
						novedad.Valor = item.Pago;

						devengado_doc.DatosHoras.Add(novedad);
					}
				}

				if (obj.HRDDFs != null && obj.HRDDFs.Length > 0)
				{
					if (devengado_doc.DatosHoras == null)
					{
						devengado_doc.DatosHoras = new List<Hora>();
					}

					foreach (var item in obj.HRDDFs)
					{
						Hora novedad = new Hora();
						novedad.Cantidad = item.Cantidad;
						novedad.TipoHora = TipoHoraNomina.RecargoDominicalesFestivas.GetHashCode();
						novedad.CodigoConcepto = Enumeracion.GetDescription(TipoHoraNomina.RecargoDominicalesFestivas);
						novedad.Porcentaje = item.Porcentaje;
						novedad.Valor = item.Pago;

						devengado_doc.DatosHoras.Add(novedad);
					}
				}

				if (obj.HRNDFs != null && obj.HRNDFs.Length > 0)
				{
					if (devengado_doc.DatosHoras == null)
					{
						devengado_doc.DatosHoras = new List<Hora>();
					}

					foreach (var item in obj.HRNDFs)
					{
						Hora novedad = new Hora();
						novedad.Cantidad = item.Cantidad;
						novedad.TipoHora = TipoHoraNomina.RecargoDominicalesFestivasNocturnas.GetHashCode();
						novedad.CodigoConcepto = Enumeracion.GetDescription(TipoHoraNomina.RecargoDominicalesFestivasNocturnas);
						novedad.Porcentaje = item.Porcentaje;
						novedad.Valor = item.Pago;

						devengado_doc.DatosHoras.Add(novedad);
					}
				}

				if (obj.HRNs != null && obj.HRNs.Length > 0)
				{
					if (devengado_doc.DatosHoras == null)
					{
						devengado_doc.DatosHoras = new List<Hora>();
					}

					foreach (var item in obj.HRNs)
					{
						Hora novedad = new Hora();
						novedad.Cantidad = item.Cantidad;
						novedad.TipoHora = TipoHoraNomina.RecargoNocturno.GetHashCode();
						novedad.CodigoConcepto = Enumeracion.GetDescription(TipoHoraNomina.RecargoNocturno);
						novedad.Porcentaje = item.Porcentaje;
						novedad.Valor = item.Pago;

						devengado_doc.DatosHoras.Add(novedad);
					}
				}

				if (obj.HuelgasLegales != null && obj.HuelgasLegales.Length > 0)
				{
					devengado_doc.HuelgaLegal = new List<NovedadGeneral>();

					foreach (var item in obj.HuelgasLegales)
					{
						NovedadGeneral novedad = new NovedadGeneral();
						novedad.Cantidad = Convert.ToInt32(item.Cantidad);
						novedad.FechaInicio = item.FechaInicio;
						novedad.FechaFin = item.FechaFin;

						devengado_doc.HuelgaLegal.Add(novedad);
					}
				}

				if (obj.Incapacidades != null && obj.Incapacidades.Length > 0)
				{
					devengado_doc.Incapacidades = new List<NovedadGeneral>();

					foreach (var item in obj.Incapacidades)
					{
						NovedadGeneral novedad = new NovedadGeneral();
						novedad.Cantidad = Convert.ToInt32(item.Cantidad);
						novedad.Tipo = Convert.ToInt32(item.Tipo);
						novedad.Pago = item.Pago;
						novedad.FechaInicio = item.FechaInicio;
						novedad.FechaFin = item.FechaFin;

						devengado_doc.Incapacidades.Add(novedad);
					}
				}

				devengado_doc.Indemnizacion = obj.Indemnizacion;

				if (obj.Licencias != null)
				{
					devengado_doc.Licencias = new List<NovedadGeneral>();

					if (obj.Licencias.LicenciaMP != null && obj.Licencias.LicenciaMP.Length > 0)
					{
						foreach (var item in obj.Licencias.LicenciaMP)
						{
							NovedadGeneral novedad = new NovedadGeneral();
							novedad.Cantidad = Convert.ToInt32(item.Cantidad);
							novedad.Tipo = 1;
							novedad.Pago = item.Pago;
							novedad.FechaInicio = item.FechaInicio;
							novedad.FechaFin = item.FechaFin;

							devengado_doc.Licencias.Add(novedad);

						}
					}

					if (obj.Licencias.LicenciaR != null && obj.Licencias.LicenciaR.Length > 0)
					{
						foreach (var item in obj.Licencias.LicenciaR)
						{
							NovedadGeneral novedad = new NovedadGeneral();
							novedad.Cantidad = Convert.ToInt32(item.Cantidad);
							novedad.Tipo = 2;
							novedad.Pago = item.Pago;
							novedad.FechaInicio = item.FechaInicio;
							novedad.FechaFin = item.FechaFin;

							devengado_doc.Licencias.Add(novedad);

						}
					}

					if (obj.Licencias.LicenciaNR != null && obj.Licencias.LicenciaNR.Length > 0)
					{
						foreach (var item in obj.Licencias.LicenciaNR)
						{
							NovedadGeneral novedad = new NovedadGeneral();
							novedad.Cantidad = Convert.ToInt32(item.Cantidad);
							novedad.Tipo = 3;
							novedad.FechaInicio = item.FechaInicio;
							novedad.FechaFin = item.FechaFin;

							devengado_doc.Licencias.Add(novedad);

						}
					}

				}

				if (obj.OtrosConceptos != null && obj.OtrosConceptos.Length > 0)
				{
					devengado_doc.OtrosConceptos = new List<OtroConcepto>();

					foreach (var item in obj.OtrosConceptos)
					{
						OtroConcepto novedad = new OtroConcepto();
						novedad.DescripcionConcepto = item.DescripcionConcepto;
						novedad.PagoConcepto = new NovedadSalNoSal();
						novedad.PagoConcepto.Pago = item.ConceptoS;
						novedad.PagoConcepto.PagoNS = item.ConceptoNS;

						devengado_doc.OtrosConceptos.Add(novedad);

					}
				}

				if (obj.PagosTerceros != null && obj.PagosTerceros.Length > 0)
				{
					devengado_doc.PagosTerceros = new List<decimal>();
					devengado_doc.PagosTerceros = obj.PagosTerceros.ToList();
				}

				if (obj.Primas != null)
				{
					devengado_doc.PagoPrima = new Prima();
					devengado_doc.PagoPrima.Cantidad = Convert.ToInt32(obj.Primas.Cantidad);
					devengado_doc.PagoPrima.Pago = new NovedadSalNoSal();
					devengado_doc.PagoPrima.Pago.Pago = obj.Primas.Pago;
					devengado_doc.PagoPrima.Pago.PagoNS = obj.Primas.PagoNS;
				}

				devengado_doc.Reintegro = obj.Reintegro;

				devengado_doc.Teletrabajo = obj.Teletrabajo;

				if (obj.Transporte != null && obj.Transporte.Length > 0)
				{
					devengado_doc.DatosTransporte = new List<Transporte>();

					foreach (var item in obj.Transporte)
					{
						Transporte novedad = new Transporte();
						novedad.AuxilioTransporte = item.AuxilioTransporte;
						novedad.ViaticoManuAlojS = item.ViaticoManutAlojS;
						novedad.ViaticoManuAlojNS = item.ViaticoManutAlojNS;

						devengado_doc.DatosTransporte.Add(novedad);
					}
				}

				if (obj.Vacaciones != null)
				{
					devengado_doc.Vacaciones = new List<NovedadGeneral>();

					if (obj.Vacaciones.VacacionesComunes != null && obj.Vacaciones.VacacionesComunes.Length > 0)
					{
						foreach (var item in obj.Vacaciones.VacacionesComunes)
						{
							NovedadGeneral novedad = new NovedadGeneral();
							novedad.Cantidad = Convert.ToInt32(item.Cantidad);
							novedad.Tipo = 1;
							novedad.Pago = item.Pago;
							novedad.FechaInicio = item.FechaInicio;
							novedad.FechaFin = item.FechaFin;

							devengado_doc.Vacaciones.Add(novedad);

						}
					}

					if (obj.Vacaciones.VacacionesCompensadas != null && obj.Vacaciones.VacacionesCompensadas.Length > 0)
					{
						foreach (var item in obj.Vacaciones.VacacionesCompensadas)
						{
							NovedadGeneral novedad = new NovedadGeneral();
							novedad.Cantidad = Convert.ToInt32(item.Cantidad);
							novedad.Tipo = 2;
							novedad.Pago = item.Pago;

							devengado_doc.Vacaciones.Add(novedad);

						}
					}
				}
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Devengados";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(string.Format("{0} - {1}", msg_custom, excepcion.Message), excepcion.InnerException);
			}

			return devengado_doc;
		}


		public static Deducciones Obtener_Deducciones(object deduccion_xml, int tipo_doc)
		{

			Deducciones deduccion_doc = new Deducciones();

			try
			{
				var obj = (dynamic)null;

				if (tipo_doc == 0)
				{
					obj = new NominaIndividualTypeDeducciones();
					obj = deduccion_xml;
				}
				else if (tipo_doc == 1)
				{
					obj = new NominaIndividualDeAjusteTypeReemplazarDevengados();
					obj = deduccion_xml;
				}

				deduccion_doc.AFC = obj.AFC;
				
				if (obj.Anticipos != null && obj.Anticipos.Length > 0)
				{
					deduccion_doc.Anticipos = obj.Anticipos.ToList();
				}

				deduccion_doc.Cooperativa = obj.Cooperativa;
				deduccion_doc.Deuda = obj.Deuda;
				deduccion_doc.Educacion = obj.Educacion;
				deduccion_doc.EmbargoFiscal = obj.EmbargoFiscal;
				
				if (obj.FondoPension != null)
				{
					deduccion_doc.Pension = new NovedadDeduccion();
					deduccion_doc.Pension.Deduccion = obj.FondoPension.Deduccion;
					deduccion_doc.Pension.Porcentaje = obj.FondoPension.Porcentaje;
				}

				if (obj.FondoSP != null)
				{
					deduccion_doc.DatosFondoSP = new FondoSP();
					deduccion_doc.DatosFondoSP.Porcentaje = obj.FondoSP.Porcentaje;
					deduccion_doc.DatosFondoSP.DeduccionFSP = obj.FondoSP.Deduccion;
					deduccion_doc.DatosFondoSP.PorcentajeSub = obj.FondoSP.PorcentajeSub;
					deduccion_doc.DatosFondoSP.DeduccionSub = obj.FondoSP.DeduccionSub;

				}

				if (obj.Libranzas != null && obj.Libranzas.Length > 0)
				{
					deduccion_doc.DatosLibranzas = new List<Libranza>();
					
					foreach (var item in obj.Libranzas)
					{
						Libranza novedad = new Libranza();
						novedad.Deduccion = item.Deduccion;
						novedad.Descripcion = item.Descripcion;

						deduccion_doc.DatosLibranzas.Add(novedad);
					}
				}

				if (obj.OtrasDeducciones != null && obj.OtrasDeducciones.Length > 0)
				{
					deduccion_doc.OtrasDeducciones = new List<decimal>();
					deduccion_doc.OtrasDeducciones = obj.OtrasDeducciones.ToList();
				}

				if (obj.PagosTerceros != null && obj.PagosTerceros.Length > 0)
				{
					deduccion_doc.PagosTerceros = new List<decimal>();
					deduccion_doc.PagosTerceros = obj.PagosTerceros.ToList();
				}

				deduccion_doc.PensionVoluntaria = obj.PensionVoluntaria;
				deduccion_doc.PlanComplementarios = obj.PlanComplementarios;
				deduccion_doc.Reintegro = obj.Reintegro;
				deduccion_doc.RetencionFuente = obj.RetencionFuente;
				
				if (obj.Salud != null)
				{
					deduccion_doc.Salud = new NovedadDeduccion();
					deduccion_doc.Salud.Deduccion = obj.Salud.Deduccion;
					deduccion_doc.Salud.Porcentaje = obj.Salud.Porcentaje;
				}

				if (obj.Sanciones != null && obj.Sanciones.Length > 0)
				{
					deduccion_doc.DatosSanciones = new List<Sancion>();

					foreach (var item in obj.Sanciones)
					{
						Sancion novedad = new Sancion();
						novedad.SancionPriv = item.SancionPriv;
						novedad.SancionPublic = item.SancionPublic;

						deduccion_doc.DatosSanciones.Add(novedad);
					}
				}

				if (obj.Sindicatos != null && obj.Sindicatos.Length > 0)
				{
					deduccion_doc.DatosSindicatos = new List<NovedadDeduccion>();

					foreach (var item in obj.Sindicatos)
					{
						NovedadDeduccion novedad = new NovedadDeduccion();
						novedad.Deduccion = item.Deduccion;
						novedad.Porcentaje = item.Porcentaje;

						deduccion_doc.DatosSindicatos.Add(novedad);
					}
				}
													


			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Deducciones";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(string.Format("{0} - {1}", msg_custom, excepcion.Message), excepcion.InnerException);
			}

			return deduccion_doc;
		}

	}
}
