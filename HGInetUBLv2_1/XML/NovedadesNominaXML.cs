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

		public static NominaIndividualTypeDevengados ObtenerDevengados(Devengados devengados_doc, bool practicante)
		{

			try
			{
				if (devengados_doc == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				NominaIndividualTypeDevengados devengado = new NominaIndividualTypeDevengados();

				devengado.Basico = new NominaIndividualTypeDevengadosBasico();
				devengado.Basico.SueldoTrabajado = devengados_doc.SueldoTrabajado;
				devengado.Basico.DiasTrabajados = devengados_doc.DiasTrabajados.ToString();

				if (devengados_doc.DatosTransporte != null && devengados_doc.DatosTransporte.Count() > 0)
				{
					NominaIndividualTypeDevengadosTransporte[] transporte_doc = new NominaIndividualTypeDevengadosTransporte[devengados_doc.DatosTransporte.Count()];
					NominaIndividualTypeDevengadosTransporte transporte = new NominaIndividualTypeDevengadosTransporte();
					transporte.AuxilioTransporte = devengados_doc.DatosTransporte.Sum(x => x.AuxilioTransporte);
					if (transporte.AuxilioTransporte > 0)
						transporte.AuxilioTransporteSpecified = true;
					transporte.ViaticoManuAlojNS = devengados_doc.DatosTransporte.Sum(x => x.ViaticoManuAlojNS);
					if (transporte.ViaticoManuAlojNS > 0)
						transporte.ViaticoManuAlojNSSpecified = true;
					transporte.ViaticoManuAlojS = devengados_doc.DatosTransporte.Sum(x => x.ViaticoManuAlojS);
					if (transporte.ViaticoManuAlojS > 0)
						transporte.ViaticoManuAlojSSpecified = true;
					transporte_doc[0] = transporte;

					devengado.Transporte = transporte_doc;
				}

				if (devengados_doc.DatosHoras != null && devengados_doc.DatosHoras.Count > 0)
				{

					List<NominaIndividualTypeDevengadosHED> list_HED = new List<NominaIndividualTypeDevengadosHED>();
					List<NominaIndividualTypeDevengadosHEN> list_HEN = new List<NominaIndividualTypeDevengadosHEN>();
					List<NominaIndividualTypeDevengadosHRN> list_HRN = new List<NominaIndividualTypeDevengadosHRN>();
					List<NominaIndividualTypeDevengadosHEDDF> list_HEDDF = new List<NominaIndividualTypeDevengadosHEDDF>();
					List<NominaIndividualTypeDevengadosHRDDF> list_HRDDF = new List<NominaIndividualTypeDevengadosHRDDF>();
					List<NominaIndividualTypeDevengadosHENDF> list_HENDF = new List<NominaIndividualTypeDevengadosHENDF>();
					List<NominaIndividualTypeDevengadosHRNDF> list_HRNDF = new List<NominaIndividualTypeDevengadosHRNDF>();

					foreach (Hora item in devengados_doc.DatosHoras)
					{

						TipoHoraNomina Hora = Enumeracion.GetEnumObjectByValue<TipoHoraNomina>(item.TipoHora);
						
						switch (Hora)
						{
							case TipoHoraNomina.ExtraDiurna:
								NominaIndividualTypeDevengadosHED HED = new NominaIndividualTypeDevengadosHED();
								
								HED.Cantidad = item.Cantidad;
								HED.Pago = item.Valor;
								HED.Porcentaje = item.Porcentaje;
								HED.HoraInicioSpecified = false;
								HED.HoraFinSpecified = false;

								list_HED.Add(HED);

								//if (devengado.HEDs == null)
								//{
								//	int cant_tipo = devengados_doc.DatosHoras.Where(x => x.TipoHora.Equals(TipoHoraNomina.ExtraDiurna.GetHashCode())).Count();
								//	devengado.HEDs = new NominaIndividualTypeDevengadosHED[cant_tipo];
								//}
								//else
								//{
								//	contador = devengado.HEDs.Count() + 1;
								//}

								//devengado.HEDs[contador] = HED;

								break;

							case TipoHoraNomina.ExtraNocturna:
								NominaIndividualTypeDevengadosHEN HEN = new NominaIndividualTypeDevengadosHEN();
								
								HEN.Cantidad = item.Cantidad;
								HEN.Pago = item.Valor;
								HEN.Porcentaje = item.Porcentaje;
								HEN.HoraInicioSpecified = false;
								HEN.HoraFinSpecified = false;

								list_HEN.Add(HEN);

								break;
							case TipoHoraNomina.RecargoNocturno:
								NominaIndividualTypeDevengadosHRN HRN = new NominaIndividualTypeDevengadosHRN();
								
								HRN.Cantidad = item.Cantidad;
								HRN.Pago = item.Valor;
								HRN.Porcentaje = item.Porcentaje;
								HRN.HoraInicioSpecified = false;
								HRN.HoraFinSpecified = false;

								list_HRN.Add(HRN);

								break;
							case TipoHoraNomina.DominicalesFestivas:
								NominaIndividualTypeDevengadosHEDDF HEDDF = new NominaIndividualTypeDevengadosHEDDF();
								
								HEDDF.Cantidad = item.Cantidad;
								HEDDF.Pago = item.Valor;
								HEDDF.Porcentaje = item.Porcentaje;
								HEDDF.HoraInicioSpecified = false;
								HEDDF.HoraFinSpecified = false;

								list_HEDDF.Add(HEDDF);

								break;
							case TipoHoraNomina.RecargoDominicalesFestivas:
								NominaIndividualTypeDevengadosHRDDF HRDDF = new NominaIndividualTypeDevengadosHRDDF();
								
								HRDDF.Cantidad = item.Cantidad;
								HRDDF.Pago = item.Valor;
								HRDDF.Porcentaje = item.Porcentaje;
								HRDDF.HoraInicioSpecified = false;
								HRDDF.HoraFinSpecified = false;

								list_HRDDF.Add(HRDDF);

								break;
							case TipoHoraNomina.ExtraDominicalesFestivasNocturnas:
								NominaIndividualTypeDevengadosHENDF HENDF = new NominaIndividualTypeDevengadosHENDF();
								
								HENDF.Cantidad = item.Cantidad;
								HENDF.Pago = item.Valor;
								HENDF.Porcentaje = item.Porcentaje;
								HENDF.HoraInicioSpecified = false;
								HENDF.HoraFinSpecified = false;

								list_HENDF.Add(HENDF);

								break;
							case TipoHoraNomina.RecargoDominicalesFestivasNocturnas:
								NominaIndividualTypeDevengadosHRNDF HRNDF = new NominaIndividualTypeDevengadosHRNDF();
								
								HRNDF.Cantidad = item.Cantidad;
								HRNDF.Pago = item.Valor;
								HRNDF.Porcentaje = item.Porcentaje;
								HRNDF.HoraInicioSpecified = false;
								HRNDF.HoraFinSpecified = false;

								list_HRNDF.Add(HRNDF);

								break;
							default:
								break;
						}

					}

					if (list_HED.Count() > 0)
						devengado.HEDs = list_HED.ToArray();

					if (list_HEN.Count() > 0)
						devengado.HENs = list_HEN.ToArray();

					if (list_HRN.Count() > 0)
						devengado.HRNs = list_HRN.ToArray();

					if (list_HEDDF.Count() > 0)
						devengado.HEDDFs = list_HEDDF.ToArray();

					if (list_HRDDF.Count() > 0)
						devengado.HRDDFs = list_HRDDF.ToArray();

					if (list_HENDF.Count() > 0)
						devengado.HENDFs = list_HENDF.ToArray();

					if (list_HRNDF.Count() > 0)
						devengado.HRNDFs = list_HRNDF.ToArray();

				}


				if (devengados_doc.Vacaciones != null && devengados_doc.Vacaciones.Count() > 0)
				{
					
					devengado.Vacaciones = new NominaIndividualTypeDevengadosVacaciones();
					int cont = 0;
					int cant_vac = 0;

					if (devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)) != null)
					{
						cant_vac = devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)).Count();
						
						if (cant_vac > 0)
						{
							devengado.Vacaciones.VacacionesComunes = new NominaIndividualTypeDevengadosVacacionesVacacionesComunes[cant_vac];
							foreach (NovedadGeneral item in devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)))
							{
								NominaIndividualTypeDevengadosVacacionesVacacionesComunes vacacion = new NominaIndividualTypeDevengadosVacacionesVacacionesComunes();
								vacacion.Cantidad = item.Cantidad.ToString();
								vacacion.Pago = item.Pago;
								vacacion.FechaInicioSpecified = false;
								vacacion.FechaFinSpecified = false;
								devengado.Vacaciones.VacacionesComunes[cont] = vacacion;
								cont++;
							}
							
						}

						if (devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(2)) != null)
						{
							cont = 0;
							cant_vac = devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)).Count();

							if (cant_vac > 0)
							{
								devengado.Vacaciones.VacacionesCompensadas = new NominaIndividualTypeDevengadosVacacionesVacacionesCompensadas[cant_vac];
								foreach (NovedadGeneral item in devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)))
								{
									NominaIndividualTypeDevengadosVacacionesVacacionesCompensadas vacacion = new NominaIndividualTypeDevengadosVacacionesVacacionesCompensadas();
									vacacion.Cantidad = item.Cantidad.ToString();
									vacacion.Pago = item.Pago;
									devengado.Vacaciones.VacacionesCompensadas[cont] = vacacion;
									cont++;
								}

							}
						}

					}


				}

				if (devengados_doc.PagoPrima != null && devengados_doc.PagoPrima.Pago != null)
				{

					devengado.Primas = new NominaIndividualTypeDevengadosPrimas();

					devengado.Primas.Cantidad = devengados_doc.PagoPrima.Cantidad.ToString();
					devengado.Primas.Pago = devengados_doc.PagoPrima.Pago.Pago;
					if (devengados_doc.PagoPrima.Pago.PagoNS > 0)
					{
						devengado.Primas.PagoNSSpecified = true;
						devengado.Primas.PagoNS = devengados_doc.PagoPrima.Pago.PagoNS;
					} 

				}

				if (devengados_doc.PagoCesantias != null)
				{
					devengado.Cesantias = new NominaIndividualTypeDevengadosCesantias();

					devengado.Cesantias.Pago = devengados_doc.PagoCesantias.Pago;
					devengado.Cesantias.PagoIntereses = devengados_doc.PagoCesantias.PagoIntereses;
					devengado.Cesantias.Porcentaje = devengados_doc.PagoCesantias.Porcentaje;

				}

				if (devengados_doc.Incapacidades != null && devengados_doc.Incapacidades.Count() > 0)
				{

					List<NominaIndividualTypeDevengadosIncapacidad> list_incapacidad = new List<NominaIndividualTypeDevengadosIncapacidad>();

					foreach (NovedadGeneral item in devengados_doc.Incapacidades)
					{
						NominaIndividualTypeDevengadosIncapacidad incapacidad = new NominaIndividualTypeDevengadosIncapacidad();
						incapacidad.Tipo = item.Tipo.ToString();
						incapacidad.Pago = item.Pago;
						incapacidad.Cantidad = item.Cantidad.ToString();
						incapacidad.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
						incapacidad.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
						list_incapacidad.Add(incapacidad);

					}

					devengado.Incapacidades = list_incapacidad.ToArray();
					
				}

				if (devengados_doc.Licencias != null && devengados_doc.Licencias.Count() > 0)
				{

					NominaIndividualTypeDevengadosLicencias novedad = new NominaIndividualTypeDevengadosLicencias();
					List<NominaIndividualTypeDevengadosLicenciasLicenciaMP> list_MP = new List<NominaIndividualTypeDevengadosLicenciasLicenciaMP>();
					List<NominaIndividualTypeDevengadosLicenciasLicenciaR> list_R = new List<NominaIndividualTypeDevengadosLicenciasLicenciaR>();
					List<NominaIndividualTypeDevengadosLicenciasLicenciaNR> list_NR = new List<NominaIndividualTypeDevengadosLicenciasLicenciaNR>();
					
					foreach (NovedadGeneral item in devengados_doc.Licencias)
					{
						
						switch (item.Tipo)
						{
							case 1:

								NominaIndividualTypeDevengadosLicenciasLicenciaMP licencia = new NominaIndividualTypeDevengadosLicenciasLicenciaMP();

								licencia.Pago = item.Pago;
								licencia.Cantidad = item.Cantidad.ToString();
								licencia.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
								licencia.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
								list_MP.Add(licencia);								

								break;
							case 2:
								NominaIndividualTypeDevengadosLicenciasLicenciaR licenciar = new NominaIndividualTypeDevengadosLicenciasLicenciaR();

								licenciar.Pago = item.Pago;
								licenciar.Cantidad = item.Cantidad.ToString();
								licenciar.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
								licenciar.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
								list_R.Add(licenciar);
								break;
							case 3:
								NominaIndividualTypeDevengadosLicenciasLicenciaNR licencianr = new NominaIndividualTypeDevengadosLicenciasLicenciaNR();

								licencianr.Cantidad = item.Cantidad.ToString();
								licencianr.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
								licencianr.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
								list_NR.Add(licencianr);
								break;

							default:
								break;
						}

					}

					if (list_MP.Count > 0)
					{
						novedad.LicenciaMP = list_MP.ToArray();
					}

					if (list_R.Count > 0)
					{
						novedad.LicenciaR = list_R.ToArray();
					}

					if (list_NR.Count > 0)
					{
						novedad.LicenciaNR = list_NR.ToArray();
					}

					devengado.Licencias = novedad;

				}

				if (devengados_doc.Bonificaciones != null && devengados_doc.Bonificaciones.Count() > 0)
				{

					List<NominaIndividualTypeDevengadosBonificacion> list_bonificacion = new List<NominaIndividualTypeDevengadosBonificacion>();

					foreach (NovedadSalNoSal item in devengados_doc.Bonificaciones)
					{
						NominaIndividualTypeDevengadosBonificacion bonificacion = new NominaIndividualTypeDevengadosBonificacion();
						if (item.Pago > 0)
						{
							bonificacion.BonificacionSSpecified = true;
							bonificacion.BonificacionS = item.Pago;
						}
						
						if (item.PagoNS > 0)
						{
							bonificacion.BonificacionNSSpecified = true;
							bonificacion.BonificacionNS = item.PagoNS;
						}

						list_bonificacion.Add(bonificacion);

					}

					devengado.Bonificaciones = list_bonificacion.ToArray();

				}

				if (devengados_doc.Auxilios != null && devengados_doc.Auxilios.Count() > 0)
				{

					List<NominaIndividualTypeDevengadosAuxilio> lista = new List<NominaIndividualTypeDevengadosAuxilio>();

					foreach (NovedadSalNoSal item in devengados_doc.Auxilios)
					{
						NominaIndividualTypeDevengadosAuxilio novedad = new NominaIndividualTypeDevengadosAuxilio();
						if (item.Pago > 0)
						{
							novedad.AuxilioSSpecified = true;
							novedad.AuxilioS = item.Pago;
						}
						

						if (item.PagoNS > 0)
						{
							novedad.AuxilioNSSpecified = true;
							novedad.AuxilioNS = item.PagoNS;
						}
						

						lista.Add(novedad);

					}

					devengado.Auxilios = lista.ToArray();

				}

				if (devengados_doc.HuelgaLegal != null && devengados_doc.HuelgaLegal.Count() > 0)
				{
					List<NominaIndividualTypeDevengadosHuelgaLegal> lista = new List<NominaIndividualTypeDevengadosHuelgaLegal>();

					foreach (NovedadGeneral item in devengados_doc.HuelgaLegal)
					{

						NominaIndividualTypeDevengadosHuelgaLegal novedad = new NominaIndividualTypeDevengadosHuelgaLegal();
						novedad.Cantidad = item.Cantidad.ToString();
						novedad.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
						novedad.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
						lista.Add(novedad);

					}

					devengado.HuelgasLegales = lista.ToArray();
				}

				if (devengados_doc.OtrosConceptos != null && devengados_doc.OtrosConceptos.Count() > 0)
				{
					List<NominaIndividualTypeDevengadosOtroConcepto> lista = new List<NominaIndividualTypeDevengadosOtroConcepto>();

					foreach (OtroConcepto item in devengados_doc.OtrosConceptos)
					{

						NominaIndividualTypeDevengadosOtroConcepto novedad = new NominaIndividualTypeDevengadosOtroConcepto();
						if (item.PagoConcepto.Pago > 0)
						{
							novedad.ConceptoSSpecified = true;
							novedad.ConceptoS = item.PagoConcepto.Pago;
						}
						
						if (item.PagoConcepto.PagoNS > 0)
						{
							novedad.ConceptoNSSpecified = true;
							novedad.ConceptoNS = item.PagoConcepto.PagoNS;
						}
						
						novedad.DescripcionConcepto = item.DescripcionConcepto;
						lista.Add(novedad);

					}

					devengado.OtrosConceptos = lista.ToArray();
				}

				if (devengados_doc.Compensaciones != null && devengados_doc.Compensaciones.Count() > 0)
				{
					List<NominaIndividualTypeDevengadosCompensacion> lista = new List<NominaIndividualTypeDevengadosCompensacion>();

					foreach (NovedadSalNoSal item in devengados_doc.Compensaciones)
					{

						NominaIndividualTypeDevengadosCompensacion novedad = new NominaIndividualTypeDevengadosCompensacion();
						novedad.CompensacionO = item.Pago;
						novedad.CompensacionE = item.PagoNS;
						lista.Add(novedad);

					}

					devengado.Compensaciones = lista.ToArray();
				}

				List<NominaIndividualTypeDevengadosBonoEPCTV> lista_BonAlim = new List<NominaIndividualTypeDevengadosBonoEPCTV>();

				if (devengados_doc.BonoEPCTV != null && devengados_doc.BonoEPCTV.Count() > 0)
				{
					foreach (NovedadSalNoSal item in devengados_doc.BonoEPCTV)
					{

						NominaIndividualTypeDevengadosBonoEPCTV novedad = new NominaIndividualTypeDevengadosBonoEPCTV();
						if (item.Pago > 0)
						{
							novedad.PagoSSpecified = true;
							novedad.PagoS = item.Pago;
						}
						if (item.PagoNS > 0)
						{
							novedad.PagoNSSpecified = true;
							novedad.PagoNS = item.PagoNS;
						}
						
						lista_BonAlim.Add(novedad);

					}
				}

				if (devengados_doc.BonoAlimentacion != null && devengados_doc.BonoAlimentacion.Count() > 0)
				{
					List<NominaIndividualTypeDevengadosBonoEPCTV> lista = new List<NominaIndividualTypeDevengadosBonoEPCTV>();

					foreach (NovedadSalNoSal item in devengados_doc.BonoAlimentacion)
					{

						NominaIndividualTypeDevengadosBonoEPCTV novedad = new NominaIndividualTypeDevengadosBonoEPCTV();
						if (item.Pago > 0)
						{
							novedad.PagoAlimentacionSSpecified = true;
							novedad.PagoAlimentacionS = item.Pago;
						}
						
						if (item.PagoNS > 0)
						{
							novedad.PagoAlimentacionNSSpecified = true;
							novedad.PagoAlimentacionNS = item.PagoNS;
						}
						
						lista.Add(novedad);

					}

					lista_BonAlim.AddRange(lista);
				}

				if (lista_BonAlim.Count() > 0)
				{
					devengado.BonoEPCTVs = lista_BonAlim.ToArray();
				}

				if (devengados_doc.Comisiones != null && devengados_doc.Comisiones.Count() > 0)
				{
					devengado.Comisiones = devengados_doc.Comisiones.ToArray();
				}

				if (devengados_doc.PagosTerceros != null && devengados_doc.PagosTerceros.Count() > 0)
				{
					devengado.PagosTerceros = devengados_doc.PagosTerceros.ToArray();
				}

				if (devengados_doc.Anticipos != null && devengados_doc.Anticipos.Count() > 0)
				{
					devengado.Anticipos = devengados_doc.Anticipos.ToArray();
				}

				devengado.Dotacion = devengados_doc.Dotacion;

				if (devengados_doc.ApoyoSostenimiento > 0)
				{
					devengado.ApoyoSostSpecified = true;
					devengado.ApoyoSost = devengados_doc.ApoyoSostenimiento;
				}

				//if (practicante == true && devengados_doc.ApoyoSostenimiento == 0)
				//{
				//	devengado.ApoyoSostSpecified = true;
				//	devengado.ApoyoSost = devengados_doc.SueldoTrabajado;
				//	devengado.Basico.SueldoTrabajado = 0;
				//}
				//else if (devengados_doc.ApoyoSostenimiento > 0)
				//{
				//	devengado.ApoyoSostSpecified = true;
				//	devengado.ApoyoSost = devengados_doc.ApoyoSostenimiento;
				//	//devengado.Basico.SueldoTrabajado = 0;
				//}
				devengado.Teletrabajo = devengados_doc.Teletrabajo;
				if (devengados_doc.Teletrabajo > 0)
					devengado.TeletrabajoSpecified = true;

				devengado.BonifRetiro = devengados_doc.BonifRetiro;
				if (devengados_doc.BonifRetiro > 0)
					devengado.BonifRetiroSpecified = true;

				devengado.Indemnizacion = devengados_doc.Indemnizacion;
				if (devengados_doc.Indemnizacion > 0)
					devengado.IndemnizacionSpecified = true;

				devengado.Reintegro = devengados_doc.Reintegro;
				if (devengados_doc.Reintegro > 0)
					devengado.ReintegroSpecified = true;

				return devengado;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Devengados";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		public static NominaIndividualTypeDeducciones ObtenerDeducciones(Deducciones deduccion_doc)
		{

			try
			{
				if (deduccion_doc == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				NominaIndividualTypeDeducciones deduccion = new NominaIndividualTypeDeducciones();

				//Si o si debe enviarse este tag aunque sea en cero
				deduccion.Salud = new NominaIndividualTypeDeduccionesSalud();
				if (deduccion_doc.Salud != null)
				{
					deduccion.Salud.Deduccion = deduccion_doc.Salud.Deduccion;
					deduccion.Salud.Porcentaje = deduccion_doc.Salud.Porcentaje;
				}

				//Si o si debe enviarse este tag aunque sea en cero
				deduccion.FondoPension = new NominaIndividualTypeDeduccionesFondoPension();
				if (deduccion_doc.Pension != null)
				{
					deduccion.FondoPension.Deduccion = deduccion_doc.Pension.Deduccion;
					deduccion.FondoPension.Porcentaje = deduccion_doc.Pension.Porcentaje;
				}
				

				if (deduccion_doc.DatosFondoSP != null)
				{
					deduccion.FondoSP = new NominaIndividualTypeDeduccionesFondoSP();
					deduccion.FondoSP.Porcentaje = decimal.Round(deduccion_doc.DatosFondoSP.Porcentaje, 2);
					if (deduccion_doc.DatosFondoSP.Porcentaje > 0)
						deduccion.FondoSP.PorcentajeSpecified = true;
					deduccion.FondoSP.Deduccion = deduccion_doc.DatosFondoSP.DeduccionFSP;
					if (deduccion_doc.DatosFondoSP.DeduccionFSP > 0)
						deduccion.FondoSP.DeduccionSpecified = true;

					deduccion.FondoSP.PorcentajeSub = decimal.Round(deduccion_doc.DatosFondoSP.PorcentajeSub, 2);
					if (deduccion_doc.DatosFondoSP.PorcentajeSub > 0)
						deduccion.FondoSP.PorcentajeSubSpecified = true;
					deduccion.FondoSP.DeduccionSub = deduccion_doc.DatosFondoSP.DeduccionSub;
					if (deduccion_doc.DatosFondoSP.DeduccionSub > 0)
						deduccion.FondoSP.DeduccionSubSpecified = true;
				}

				if (deduccion_doc.DatosSindicatos != null && deduccion_doc.DatosSindicatos.Count() > 0)
				{

					List<NominaIndividualTypeDeduccionesSindicato> lista = new List<NominaIndividualTypeDeduccionesSindicato>();

					foreach (NovedadDeduccion item in deduccion_doc.DatosSindicatos)
					{

						NominaIndividualTypeDeduccionesSindicato novedad = new NominaIndividualTypeDeduccionesSindicato();
						novedad.Deduccion = item.Deduccion;
						novedad.Porcentaje = item.Porcentaje;
						lista.Add(novedad);

					}

					deduccion.Sindicatos = lista.ToArray();


				}

				if (deduccion_doc.DatosSanciones != null && deduccion_doc.DatosSanciones.Count() > 0)
				{

					List<NominaIndividualTypeDeduccionesSancion> lista = new List<NominaIndividualTypeDeduccionesSancion>();

					foreach (Sancion item in deduccion_doc.DatosSanciones)
					{

						NominaIndividualTypeDeduccionesSancion novedad = new NominaIndividualTypeDeduccionesSancion();
						novedad.SancionPriv = item.SancionPriv;
						novedad.SancionPublic = item.SancionPublic;
						lista.Add(novedad);

					}

					deduccion.Sanciones = lista.ToArray();
				}

				if (deduccion_doc.DatosLibranzas != null && deduccion_doc.DatosLibranzas.Count() > 0)
				{

					List<NominaIndividualTypeDeduccionesLibranza> lista = new List<NominaIndividualTypeDeduccionesLibranza>();

					foreach (Libranza item in deduccion_doc.DatosLibranzas)
					{

						NominaIndividualTypeDeduccionesLibranza novedad = new NominaIndividualTypeDeduccionesLibranza();
						novedad.Deduccion = item.Deduccion;
						novedad.Descripcion = item.Descripcion;
						lista.Add(novedad);

					}

					deduccion.Libranzas = lista.ToArray();


				}

				if (deduccion_doc.PagosTerceros != null && deduccion_doc.PagosTerceros.Count() > 0)
				{
					deduccion.PagosTerceros = deduccion_doc.PagosTerceros.ToArray();

				}

				if (deduccion_doc.Anticipos != null && deduccion_doc.Anticipos.Count() > 0)
				{
					deduccion.Anticipos = deduccion_doc.Anticipos.ToArray();
				}

				if (deduccion_doc.OtrasDeducciones != null && deduccion_doc.OtrasDeducciones.Count() > 0)
				{
					deduccion.OtrasDeducciones = deduccion_doc.OtrasDeducciones.ToArray();
				}

				deduccion.PensionVoluntaria = deduccion_doc.PensionVoluntaria;
				if (deduccion_doc.PensionVoluntaria > 0)
					deduccion.PensionVoluntariaSpecified = true;

				deduccion.RetencionFuente = deduccion_doc.RetencionFuente;
				if (deduccion_doc.RetencionFuente > 0)
					deduccion.RetencionFuenteSpecified = true;

				deduccion.AFC = deduccion_doc.AFC;
				if (deduccion_doc.AFC > 0)
					deduccion.AFCSpecified = true;

				deduccion.Cooperativa = deduccion_doc.Cooperativa;
				if (deduccion_doc.Cooperativa > 0)
					deduccion.CooperativaSpecified = true;

				deduccion.EmbargoFiscal = deduccion_doc.EmbargoFiscal;
				if (deduccion_doc.EmbargoFiscal > 0)
					deduccion.EmbargoFiscalSpecified = true;

				deduccion.PlanComplementarios = deduccion_doc.PlanComplementarios;
				if (deduccion_doc.PlanComplementarios > 0)
					deduccion.PlanComplementariosSpecified = true;

				deduccion.Educacion = deduccion_doc.Educacion;
				if (deduccion_doc.Educacion > 0)
					deduccion.EducacionSpecified = true;

				deduccion.Reintegro = deduccion_doc.Reintegro;
				if (deduccion_doc.Reintegro > 0)
					deduccion.ReintegroSpecified = true;

				deduccion.Deuda = deduccion_doc.Deuda;
				if (deduccion_doc.Deuda > 0)
					deduccion.DeudaSpecified = true;

				return deduccion;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Deduccion";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		public static NominaIndividualDeAjusteTypeReemplazarDevengados ObtenerDevengadosAjuste(Devengados devengados_doc, bool practicante)
		{

			try
			{
				if (devengados_doc == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				NominaIndividualDeAjusteTypeReemplazarDevengados devengado = new NominaIndividualDeAjusteTypeReemplazarDevengados();

				devengado.Basico = new NominaIndividualDeAjusteTypeReemplazarDevengadosBasico();
				devengado.Basico.SueldoTrabajado = devengados_doc.SueldoTrabajado;
				devengado.Basico.DiasTrabajados = devengados_doc.DiasTrabajados.ToString();

				if (devengados_doc.DatosTransporte != null && devengados_doc.DatosTransporte.Count() > 0)
				{
					NominaIndividualDeAjusteTypeReemplazarDevengadosTransporte[] transporte_doc = new NominaIndividualDeAjusteTypeReemplazarDevengadosTransporte[devengados_doc.DatosTransporte.Count()];
					NominaIndividualDeAjusteTypeReemplazarDevengadosTransporte transporte = new NominaIndividualDeAjusteTypeReemplazarDevengadosTransporte();
					transporte.AuxilioTransporte = devengados_doc.DatosTransporte.Sum(x => x.AuxilioTransporte);
					if (transporte.AuxilioTransporte > 0)
						transporte.AuxilioTransporteSpecified = true;
					transporte.ViaticoManuAlojNS = devengados_doc.DatosTransporte.Sum(x => x.ViaticoManuAlojNS);
					if (transporte.ViaticoManuAlojNS > 0)
						transporte.ViaticoManuAlojNSSpecified = true;
					transporte.ViaticoManuAlojS = devengados_doc.DatosTransporte.Sum(x => x.ViaticoManuAlojS);
					if (transporte.ViaticoManuAlojS > 0)
						transporte.ViaticoManuAlojSSpecified = true;
					transporte_doc[0] = transporte;

					devengado.Transporte = transporte_doc;
				}

				if (devengados_doc.DatosHoras != null && devengados_doc.DatosHoras.Count > 0)
				{

					List<NominaIndividualDeAjusteTypeReemplazarDevengadosHED> list_HED = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosHED>();
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosHEN> list_HEN = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosHEN>();
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosHRN> list_HRN = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosHRN>();
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosHEDDF> list_HEDDF = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosHEDDF>();
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosHRDDF> list_HRDDF = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosHRDDF>();
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosHENDF> list_HENDF = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosHENDF>();
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosHRNDF> list_HRNDF = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosHRNDF>();

					foreach (Hora item in devengados_doc.DatosHoras)
					{

						TipoHoraNomina Hora = Enumeracion.GetEnumObjectByValue<TipoHoraNomina>(item.TipoHora);

						switch (Hora)
						{
							case TipoHoraNomina.ExtraDiurna:
								NominaIndividualDeAjusteTypeReemplazarDevengadosHED HED = new NominaIndividualDeAjusteTypeReemplazarDevengadosHED();

								HED.Cantidad = item.Cantidad;
								HED.Pago = item.Valor;
								HED.Porcentaje = item.Porcentaje;
								HED.HoraInicioSpecified = false;
								HED.HoraFinSpecified = false;

								list_HED.Add(HED);

								//if (devengado.HEDs == null)
								//{
								//	int cant_tipo = devengados_doc.DatosHoras.Where(x => x.TipoHora.Equals(TipoHoraNomina.ExtraDiurna.GetHashCode())).Count();
								//	devengado.HEDs = new NominaIndividualDeAjusteTypeReemplazarDevengadosHED[cant_tipo];
								//}
								//else
								//{
								//	contador = devengado.HEDs.Count() + 1;
								//}

								//devengado.HEDs[contador] = HED;

								break;

							case TipoHoraNomina.ExtraNocturna:
								NominaIndividualDeAjusteTypeReemplazarDevengadosHEN HEN = new NominaIndividualDeAjusteTypeReemplazarDevengadosHEN();

								HEN.Cantidad = item.Cantidad;
								HEN.Pago = item.Valor;
								HEN.Porcentaje = item.Porcentaje;
								HEN.HoraInicioSpecified = false;
								HEN.HoraFinSpecified = false;

								list_HEN.Add(HEN);

								break;
							case TipoHoraNomina.RecargoNocturno:
								NominaIndividualDeAjusteTypeReemplazarDevengadosHRN HRN = new NominaIndividualDeAjusteTypeReemplazarDevengadosHRN();

								HRN.Cantidad = item.Cantidad;
								HRN.Pago = item.Valor;
								HRN.Porcentaje = item.Porcentaje;
								HRN.HoraInicioSpecified = false;
								HRN.HoraFinSpecified = false;

								list_HRN.Add(HRN);

								break;
							case TipoHoraNomina.DominicalesFestivas:
								NominaIndividualDeAjusteTypeReemplazarDevengadosHEDDF HEDDF = new NominaIndividualDeAjusteTypeReemplazarDevengadosHEDDF();

								HEDDF.Cantidad = item.Cantidad;
								HEDDF.Pago = item.Valor;
								HEDDF.Porcentaje = item.Porcentaje;
								HEDDF.HoraInicioSpecified = false;
								HEDDF.HoraFinSpecified = false;

								list_HEDDF.Add(HEDDF);

								break;
							case TipoHoraNomina.RecargoDominicalesFestivas:
								NominaIndividualDeAjusteTypeReemplazarDevengadosHRDDF HRDDF = new NominaIndividualDeAjusteTypeReemplazarDevengadosHRDDF();

								HRDDF.Cantidad = item.Cantidad;
								HRDDF.Pago = item.Valor;
								HRDDF.Porcentaje = item.Porcentaje;
								HRDDF.HoraInicioSpecified = false;
								HRDDF.HoraFinSpecified = false;

								list_HRDDF.Add(HRDDF);

								break;
							case TipoHoraNomina.ExtraDominicalesFestivasNocturnas:
								NominaIndividualDeAjusteTypeReemplazarDevengadosHENDF HENDF = new NominaIndividualDeAjusteTypeReemplazarDevengadosHENDF();

								HENDF.Cantidad = item.Cantidad;
								HENDF.Pago = item.Valor;
								HENDF.Porcentaje = item.Porcentaje;
								HENDF.HoraInicioSpecified = false;
								HENDF.HoraFinSpecified = false;

								list_HENDF.Add(HENDF);

								break;
							case TipoHoraNomina.RecargoDominicalesFestivasNocturnas:
								NominaIndividualDeAjusteTypeReemplazarDevengadosHRNDF HRNDF = new NominaIndividualDeAjusteTypeReemplazarDevengadosHRNDF();

								HRNDF.Cantidad = item.Cantidad;
								HRNDF.Pago = item.Valor;
								HRNDF.Porcentaje = item.Porcentaje;
								HRNDF.HoraInicioSpecified = false;
								HRNDF.HoraFinSpecified = false;

								list_HRNDF.Add(HRNDF);

								break;
							default:
								break;
						}

					}

					if (list_HED.Count() > 0)
						devengado.HEDs = list_HED.ToArray();

					if (list_HEN.Count() > 0)
						devengado.HENs = list_HEN.ToArray();

					if (list_HRN.Count() > 0)
						devengado.HRNs = list_HRN.ToArray();

					if (list_HEDDF.Count() > 0)
						devengado.HEDDFs = list_HEDDF.ToArray();

					if (list_HRDDF.Count() > 0)
						devengado.HRDDFs = list_HRDDF.ToArray();

					if (list_HENDF.Count() > 0)
						devengado.HENDFs = list_HENDF.ToArray();

					if (list_HRNDF.Count() > 0)
						devengado.HRNDFs = list_HRNDF.ToArray();

				}


				if (devengados_doc.Vacaciones != null && devengados_doc.Vacaciones.Count() > 0)
				{

					devengado.Vacaciones = new NominaIndividualDeAjusteTypeReemplazarDevengadosVacaciones();
					int cont = 0;
					int cant_vac = 0;

					if (devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)) != null)
					{
						cant_vac = devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)).Count();

						if (cant_vac > 0)
						{
							devengado.Vacaciones.VacacionesComunes = new NominaIndividualDeAjusteTypeReemplazarDevengadosVacacionesVacacionesComunes[cant_vac];
							foreach (NovedadGeneral item in devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)))
							{
								NominaIndividualDeAjusteTypeReemplazarDevengadosVacacionesVacacionesComunes vacacion = new NominaIndividualDeAjusteTypeReemplazarDevengadosVacacionesVacacionesComunes();
								vacacion.Cantidad = item.Cantidad.ToString();
								vacacion.Pago = item.Pago;
								vacacion.FechaInicioSpecified = false;
								vacacion.FechaFinSpecified = false;
								devengado.Vacaciones.VacacionesComunes[cont] = vacacion;
								cont++;
							}

						}

						if (devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(2)) != null)
						{
							cont = 0;
							cant_vac = devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)).Count();

							if (cant_vac > 0)
							{
								devengado.Vacaciones.VacacionesCompensadas = new NominaIndividualDeAjusteTypeReemplazarDevengadosVacacionesVacacionesCompensadas[cant_vac];
								foreach (NovedadGeneral item in devengados_doc.Vacaciones.Where(x => x.Tipo.Equals(1)))
								{
									NominaIndividualDeAjusteTypeReemplazarDevengadosVacacionesVacacionesCompensadas vacacion = new NominaIndividualDeAjusteTypeReemplazarDevengadosVacacionesVacacionesCompensadas();
									vacacion.Cantidad = item.Cantidad.ToString();
									vacacion.Pago = item.Pago;
									devengado.Vacaciones.VacacionesCompensadas[cont] = vacacion;
									cont++;
								}

							}
						}

					}


				}

				if (devengados_doc.PagoPrima != null && devengados_doc.PagoPrima.Pago != null)
				{

					devengado.Primas = new NominaIndividualDeAjusteTypeReemplazarDevengadosPrimas();

					devengado.Primas.Cantidad = devengados_doc.PagoPrima.Cantidad.ToString();
					devengado.Primas.Pago = devengados_doc.PagoPrima.Pago.Pago;
					devengado.Primas.PagoNS = devengados_doc.PagoPrima.Pago.PagoNS;
					if (devengados_doc.PagoPrima.Pago.PagoNS > 0)
						devengado.Primas.PagoNSSpecified = true;

				}

				if (devengados_doc.PagoCesantias != null)
				{
					devengado.Cesantias = new NominaIndividualDeAjusteTypeReemplazarDevengadosCesantias();

					devengado.Cesantias.Pago = devengados_doc.PagoCesantias.Pago;
					devengado.Cesantias.PagoIntereses = devengados_doc.PagoCesantias.PagoIntereses;
					devengado.Cesantias.Porcentaje = devengados_doc.PagoCesantias.Porcentaje;

				}

				if (devengados_doc.Incapacidades != null && devengados_doc.Incapacidades.Count() > 0)
				{

					List<NominaIndividualDeAjusteTypeReemplazarDevengadosIncapacidad> list_incapacidad = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosIncapacidad>();

					foreach (NovedadGeneral item in devengados_doc.Incapacidades)
					{
						NominaIndividualDeAjusteTypeReemplazarDevengadosIncapacidad incapacidad = new NominaIndividualDeAjusteTypeReemplazarDevengadosIncapacidad();
						incapacidad.Tipo = item.Tipo.ToString();
						incapacidad.Pago = item.Pago;
						incapacidad.Cantidad = item.Cantidad.ToString();
						incapacidad.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
						incapacidad.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
						list_incapacidad.Add(incapacidad);

					}

					devengado.Incapacidades = list_incapacidad.ToArray();

				}

				if (devengados_doc.Licencias != null && devengados_doc.Licencias.Count() > 0)
				{

					NominaIndividualDeAjusteTypeReemplazarDevengadosLicencias novedad = new NominaIndividualDeAjusteTypeReemplazarDevengadosLicencias();
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaMP> list_MP = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaMP>();
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaR> list_R = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaR>();
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaNR> list_NR = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaNR>();

					foreach (NovedadGeneral item in devengados_doc.Licencias)
					{

						switch (item.Tipo)
						{
							case 1:

								NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaMP licencia = new NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaMP();

								licencia.Pago = item.Pago;
								licencia.Cantidad = item.Cantidad.ToString();
								licencia.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
								licencia.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
								list_MP.Add(licencia);

								break;
							case 2:
								NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaR licenciar = new NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaR();

								licenciar.Pago = item.Pago;
								licenciar.Cantidad = item.Cantidad.ToString();
								licenciar.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
								licenciar.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
								list_R.Add(licenciar);
								break;
							case 3:
								NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaNR licencianr = new NominaIndividualDeAjusteTypeReemplazarDevengadosLicenciasLicenciaNR();

								licencianr.Cantidad = item.Cantidad.ToString();
								licencianr.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
								licencianr.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
								list_NR.Add(licencianr);
								break;

							default:
								break;
						}

					}

					if (list_MP.Count > 0)
					{
						novedad.LicenciaMP = list_MP.ToArray();
					}

					if (list_R.Count > 0)
					{
						novedad.LicenciaR = list_R.ToArray();
					}

					if (list_NR.Count > 0)
					{
						novedad.LicenciaNR = list_NR.ToArray();
					}

					devengado.Licencias = novedad;

				}

				if (devengados_doc.Bonificaciones != null && devengados_doc.Bonificaciones.Count() > 0)
				{

					List<NominaIndividualDeAjusteTypeReemplazarDevengadosBonificacion> list_bonificacion = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosBonificacion>();

					foreach (NovedadSalNoSal item in devengados_doc.Bonificaciones)
					{
						NominaIndividualDeAjusteTypeReemplazarDevengadosBonificacion bonificacion = new NominaIndividualDeAjusteTypeReemplazarDevengadosBonificacion();
						bonificacion.BonificacionS = item.Pago;
						if (item.Pago > 0)
							bonificacion.BonificacionSSpecified = true;

						bonificacion.BonificacionNS = item.PagoNS;
						if (item.PagoNS > 0)
							bonificacion.BonificacionNSSpecified = true;

						list_bonificacion.Add(bonificacion);

					}

					devengado.Bonificaciones = list_bonificacion.ToArray();

				}

				if (devengados_doc.Auxilios != null && devengados_doc.Auxilios.Count() > 0)
				{

					List<NominaIndividualDeAjusteTypeReemplazarDevengadosAuxilio> lista = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosAuxilio>();

					foreach (NovedadSalNoSal item in devengados_doc.Auxilios)
					{
						NominaIndividualDeAjusteTypeReemplazarDevengadosAuxilio novedad = new NominaIndividualDeAjusteTypeReemplazarDevengadosAuxilio();
						novedad.AuxilioS = item.Pago;
						if (item.Pago > 0)
							novedad.AuxilioSSpecified = true;

						novedad.AuxilioNS = item.PagoNS;
						if (item.PagoNS > 0)
							novedad.AuxilioNSSpecified = true;

						lista.Add(novedad);

					}

					devengado.Auxilios = lista.ToArray();

				}

				if (devengados_doc.HuelgaLegal != null && devengados_doc.HuelgaLegal.Count() > 0)
				{
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosHuelgaLegal> lista = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosHuelgaLegal>();

					foreach (NovedadGeneral item in devengados_doc.HuelgaLegal)
					{

						NominaIndividualDeAjusteTypeReemplazarDevengadosHuelgaLegal novedad = new NominaIndividualDeAjusteTypeReemplazarDevengadosHuelgaLegal();
						novedad.Cantidad = item.Cantidad.ToString();
						novedad.FechaInicio = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
						novedad.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
						lista.Add(novedad);

					}

					devengado.HuelgasLegales = lista.ToArray();
				}

				if (devengados_doc.OtrosConceptos != null && devengados_doc.OtrosConceptos.Count() > 0)
				{
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosOtroConcepto> lista = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosOtroConcepto>();

					foreach (OtroConcepto item in devengados_doc.OtrosConceptos)
					{

						NominaIndividualDeAjusteTypeReemplazarDevengadosOtroConcepto novedad = new NominaIndividualDeAjusteTypeReemplazarDevengadosOtroConcepto();
						novedad.ConceptoS = item.PagoConcepto.Pago;
						if (item.PagoConcepto.Pago > 0)
							novedad.ConceptoSSpecified = true;

						novedad.ConceptoNS = item.PagoConcepto.PagoNS;
						if (item.PagoConcepto.PagoNS > 0)
							novedad.ConceptoNSSpecified = true;
						 
						novedad.DescripcionConcepto = item.DescripcionConcepto;
						lista.Add(novedad);

					}

					devengado.OtrosConceptos = lista.ToArray();
				}

				if (devengados_doc.Compensaciones != null && devengados_doc.Compensaciones.Count() > 0)
				{
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosCompensacion> lista = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosCompensacion>();

					foreach (NovedadSalNoSal item in devengados_doc.Compensaciones)
					{

						NominaIndividualDeAjusteTypeReemplazarDevengadosCompensacion novedad = new NominaIndividualDeAjusteTypeReemplazarDevengadosCompensacion();
						novedad.CompensacionO = item.Pago;
						novedad.CompensacionE = item.PagoNS;
						lista.Add(novedad);

					}

					devengado.Compensaciones = lista.ToArray();
				}

				List<NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV> lista_BonAlim = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV>();

				if (devengados_doc.BonoEPCTV != null && devengados_doc.BonoEPCTV.Count() > 0)
				{
					foreach (NovedadSalNoSal item in devengados_doc.BonoEPCTV)
					{

						NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV novedad = new NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV();
						novedad.PagoS = item.Pago;
						if (item.Pago > 0)
							novedad.PagoSSpecified = true;

						novedad.PagoNS = item.PagoNS;
						if (item.PagoNS > 0)
							novedad.PagoNSSpecified = true;

						lista_BonAlim.Add(novedad);

					}
				}

				if (devengados_doc.BonoAlimentacion != null && devengados_doc.BonoAlimentacion.Count() > 0)
				{
					List<NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV> lista = new List<NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV>();

					foreach (NovedadSalNoSal item in devengados_doc.BonoAlimentacion)
					{

						NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV novedad = new NominaIndividualDeAjusteTypeReemplazarDevengadosBonoEPCTV();
						novedad.PagoAlimentacionS = item.Pago;
						if (item.Pago > 0)
							novedad.PagoAlimentacionSSpecified = true;

						novedad.PagoNS = item.PagoNS;
						if (item.PagoNS > 0)
							novedad.PagoAlimentacionNSSpecified = true;

						lista.Add(novedad);

					}

					lista_BonAlim.AddRange(lista);
				}

				if (lista_BonAlim.Count() > 0)
				{
					devengado.BonoEPCTVs = lista_BonAlim.ToArray();
				}

				if (devengados_doc.Comisiones != null && devengados_doc.Comisiones.Count() > 0)
				{
					devengado.Comisiones = devengados_doc.Comisiones.ToArray();
				}

				if (devengados_doc.PagosTerceros != null && devengados_doc.PagosTerceros.Count() > 0)
				{
					devengado.PagosTerceros = devengados_doc.PagosTerceros.ToArray();
				}

				if (devengados_doc.Anticipos != null && devengados_doc.Anticipos.Count() > 0)
				{
					devengado.Anticipos = devengados_doc.Anticipos.ToArray();
				}

				devengado.Dotacion = devengados_doc.Dotacion;
				if (devengados_doc.Dotacion > 0)
					devengado.DotacionSpecified = true;

				if (practicante == true && devengados_doc.ApoyoSostenimiento == 0)
				{
					devengado.ApoyoSostSpecified = true;
					devengado.ApoyoSost = devengados_doc.SueldoTrabajado;
					devengado.Basico.SueldoTrabajado = 0;
				}
				else if (devengados_doc.ApoyoSostenimiento > 0)
				{
					devengado.ApoyoSostSpecified = true;
					devengado.ApoyoSost = devengados_doc.ApoyoSostenimiento;
					devengado.Basico.SueldoTrabajado = 0;
				}

				devengado.Teletrabajo = devengados_doc.Teletrabajo;
				devengado.BonifRetiro = devengados_doc.BonifRetiro;
				if (devengados_doc.BonifRetiro > 0)
					devengado.BonifRetiroSpecified = true;

				devengado.Indemnizacion = devengados_doc.Indemnizacion;
				if (devengados_doc.Indemnizacion > 0)
					devengado.IndemnizacionSpecified = true;

				devengado.Reintegro = devengados_doc.Reintegro;
				if (devengados_doc.Reintegro > 0)
					devengado.ReintegroSpecified = true;

				return devengado;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Devengados en Reemplazo";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}

		public static NominaIndividualDeAjusteTypeReemplazarDeducciones ObtenerDeduccionesAjuste(Deducciones deduccion_doc)
		{

			try
			{
				if (deduccion_doc == null)
					throw new Exception("Los datos de la empresa son inválidos.");

				NominaIndividualDeAjusteTypeReemplazarDeducciones deduccion = new NominaIndividualDeAjusteTypeReemplazarDeducciones();

				//Si o si debe enviarse este tag aunque sea en cero
				deduccion.Salud = new NominaIndividualDeAjusteTypeReemplazarDeduccionesSalud();
				if (deduccion_doc.Salud != null)
				{
					deduccion.Salud.Deduccion = deduccion_doc.Salud.Deduccion;
					deduccion.Salud.Porcentaje = deduccion_doc.Salud.Porcentaje;
				}

				//Si o si debe enviarse este tag aunque sea en cero
				deduccion.FondoPension = new NominaIndividualDeAjusteTypeReemplazarDeduccionesFondoPension();
				if (deduccion_doc.Pension != null)
				{
					deduccion.FondoPension.Deduccion = deduccion_doc.Pension.Deduccion;
					deduccion.FondoPension.Porcentaje = deduccion_doc.Pension.Porcentaje;
				}


				if (deduccion_doc.DatosFondoSP != null)
				{
					deduccion.FondoSP = new NominaIndividualDeAjusteTypeReemplazarDeduccionesFondoSP();
					deduccion.FondoSP.Porcentaje = decimal.Round(deduccion_doc.DatosFondoSP.Porcentaje,2);
					if (deduccion_doc.DatosFondoSP.Porcentaje > 0)
						deduccion.FondoSP.PorcentajeSpecified = true;

					deduccion.FondoSP.Deduccion = deduccion_doc.DatosFondoSP.DeduccionFSP;
					if (deduccion_doc.DatosFondoSP.DeduccionFSP > 0)
						deduccion.FondoSP.DeduccionSpecified = true;

					deduccion.FondoSP.PorcentajeSub = decimal.Round(deduccion_doc.DatosFondoSP.PorcentajeSub,2);
					if (deduccion_doc.DatosFondoSP.PorcentajeSub > 0)
						deduccion.FondoSP.PorcentajeSubSpecified = true;

					deduccion.FondoSP.DeduccionSub = deduccion_doc.DatosFondoSP.DeduccionSub;
					if (deduccion_doc.DatosFondoSP.DeduccionSub > 0)
						deduccion.FondoSP.DeduccionSubSpecified = true;
				}

				if (deduccion_doc.DatosSindicatos != null && deduccion_doc.DatosSindicatos.Count() > 0)
				{

					List<NominaIndividualDeAjusteTypeReemplazarDeduccionesSindicato> lista = new List<NominaIndividualDeAjusteTypeReemplazarDeduccionesSindicato>();

					foreach (NovedadDeduccion item in deduccion_doc.DatosSindicatos)
					{

						NominaIndividualDeAjusteTypeReemplazarDeduccionesSindicato novedad = new NominaIndividualDeAjusteTypeReemplazarDeduccionesSindicato();
						novedad.Deduccion = item.Deduccion;
						novedad.Porcentaje = item.Porcentaje;
						lista.Add(novedad);

					}

					deduccion.Sindicatos = lista.ToArray();


				}

				if (deduccion_doc.DatosSanciones != null && deduccion_doc.DatosSanciones.Count() > 0)
				{

					List<NominaIndividualDeAjusteTypeReemplazarDeduccionesSancion> lista = new List<NominaIndividualDeAjusteTypeReemplazarDeduccionesSancion>();

					foreach (Sancion item in deduccion_doc.DatosSanciones)
					{

						NominaIndividualDeAjusteTypeReemplazarDeduccionesSancion novedad = new NominaIndividualDeAjusteTypeReemplazarDeduccionesSancion();
						novedad.SancionPriv = item.SancionPriv;
						novedad.SancionPublic = item.SancionPublic;
						lista.Add(novedad);

					}

					deduccion.Sanciones = lista.ToArray();
				}

				if (deduccion_doc.DatosLibranzas != null && deduccion_doc.DatosLibranzas.Count() > 0)
				{

					List<NominaIndividualDeAjusteTypeReemplazarDeduccionesLibranza> lista = new List<NominaIndividualDeAjusteTypeReemplazarDeduccionesLibranza>();

					foreach (Libranza item in deduccion_doc.DatosLibranzas)
					{

						NominaIndividualDeAjusteTypeReemplazarDeduccionesLibranza novedad = new NominaIndividualDeAjusteTypeReemplazarDeduccionesLibranza();
						novedad.Deduccion = item.Deduccion;
						novedad.Descripcion = item.Descripcion;
						lista.Add(novedad);

					}

					deduccion.Libranzas = lista.ToArray();


				}

				if (deduccion_doc.PagosTerceros != null && deduccion_doc.PagosTerceros.Count() > 0)
				{
					deduccion.PagosTerceros = deduccion_doc.PagosTerceros.ToArray();

				}

				if (deduccion_doc.Anticipos != null && deduccion_doc.Anticipos.Count() > 0)
				{
					deduccion.Anticipos = deduccion_doc.Anticipos.ToArray();
				}

				if (deduccion_doc.OtrasDeducciones != null && deduccion_doc.OtrasDeducciones.Count() > 0)
				{
					deduccion.OtrasDeducciones = deduccion_doc.OtrasDeducciones.ToArray();
				}

				deduccion.PensionVoluntaria = deduccion_doc.PensionVoluntaria;
				if (deduccion_doc.PensionVoluntaria > 0)
					deduccion.PensionVoluntariaSpecified = true;

				deduccion.RetencionFuente = deduccion_doc.RetencionFuente;
				if (deduccion_doc.RetencionFuente > 0)
					deduccion.RetencionFuenteSpecified = true;

				deduccion.AFC = deduccion_doc.AFC;
				if (deduccion_doc.AFC > 0)
					deduccion.AFCSpecified = true;

				deduccion.Cooperativa = deduccion_doc.Cooperativa;
				if (deduccion_doc.Cooperativa > 0)
					deduccion.CooperativaSpecified = true;

				deduccion.EmbargoFiscal = deduccion_doc.EmbargoFiscal;
				if (deduccion_doc.EmbargoFiscal > 0)
					deduccion.EmbargoFiscalSpecified = true;

				deduccion.PlanComplementarios = deduccion_doc.PlanComplementarios;
				if (deduccion_doc.PlanComplementarios > 0)
					deduccion.PlanComplementariosSpecified = true;

				deduccion.Educacion = deduccion_doc.Educacion;
				if (deduccion_doc.Educacion > 0)
					deduccion.EducacionSpecified = true;

				deduccion.Reintegro = deduccion_doc.Reintegro;
				if (deduccion_doc.Reintegro > 0)
					deduccion.ReintegroSpecified = true;

				deduccion.Deuda = deduccion_doc.Deuda;
				if (deduccion_doc.Deuda > 0)
					deduccion.DeudaSpecified = true;


				return deduccion;
			}
			catch (Exception excepcion)
			{
				string msg_custom = "Generacion de Deduccion en Reemplazo";

				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Convertir, MensajeTipo.Error, MensajeAccion.escritura, msg_custom);

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}


	}
}
