using HGInetFacturaEReports.ReportDesigner;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using HGInetMiFacturaElectonicaData.Enumerables;
using Telerik.Reporting;
using HGInetUBLv2_1.DianListas;
using HGInetMiFacturaElectonicaData.Formatos;
using Newtonsoft.Json;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Guarda el formato PDF del documento
		/// </summary>
		/// <param name="documento_obj">información del documento</param>
		/// <param name="documentoBd">información del documento en base de datos</param>
		/// <param name="respuesta">datos de respuesta del documento</param>
		/// <param name="documento_result">información del proceso interno del documento</param>
		/// <returns>información adicional de respuesta del documento</returns>
		public static DocumentoRespuesta GuardarFormato(object documento, TblDocumentos documentoBd, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result, TblEmpresas facturador)
		{
			respuesta.DescripcionProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(ProcesoEstado.PDFGeneracion.GetHashCode()));
			respuesta.FechaUltimoProceso = Fecha.GetFecha();
			respuesta.IdProceso = ProcesoEstado.PDFGeneracion.GetHashCode();
			respuesta.IdEstado = Ctl_Documento.ObtenerCategoria(respuesta.IdProceso);

			try
			{
				var documento_obj = (dynamic)null;
				documento_obj = documento;

				bool generar_pdf = true;

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				string carpeta_formato = string.Empty;
				string ruta_archivo_formato = string.Empty;

				// valida formato en base 64 del objeto
				Formato formato_documento = (Formato)(dynamic)documento_obj.DocumentoFormato;
				if (formato_documento != null)
				{
					if (!string.IsNullOrEmpty(formato_documento.ArchivoPdf))
					{
						// almacena archivo en base64
						documento_result.NombrePdf = Ctl_Formato.GuardarArchivo(formato_documento.ArchivoPdf, documento_result);
						respuesta.UrlPdf = string.Empty;

						if (!string.IsNullOrWhiteSpace(documento_result.NombrePdf))
						{

							// url pública del pdf
							string url_ppal_pdf = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());

							respuesta.UrlPdf = string.Format(@"{0}/{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombrePdf);

							documentoBd.StrUrlArchivoPdf = respuesta.UrlPdf;
						}

						generar_pdf = false;
					}
				}

				if (generar_pdf)
				{

					TipoDocumento tipo_doc = Enumeracion.GetEnumObjectByValue<TipoDocumento>(documentoBd.IntDocTipo);

					string url_ppal_pdf = string.Format("{0}/{1}/{2}", plataforma_datos.RutaDmsPublica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
					string ruta = string.Format(@"{0}/{1}/", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);
					Report reporte_pdf = new Report();
					bool reporte_archivado = true;
					/*
                     Para la contrucción de formatos se debe tener en cuenta el envío del parametro TipoDocumento de caracter obligatorio
                     para el reporte pdf, ya que este valor es implementado para la carga de información desde cada formato.
                     */

					try
					{
						string ruta_formato = string.Format(@"{0}\{1}\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, documento_result.IdSeguridadTercero.ToString());
						carpeta_formato = string.Format(@"{0}\{1}\", ruta_formato, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

						ruta_archivo_formato = string.Format(@"{0}\{1}.json", carpeta_formato, documento_result.NombreXml);
					}
					catch (Exception)
					{
					}

					//Obtiene el diseño del formato en la base de datos y realiza el proceso de creación del pdf.
					//sino hay un formato en base de datos con el formato especificado, toma los formatos existentes en el proyecto
					Ctl_Formatos clase_formatos = new Ctl_Formatos();

					string facturador_formato = documentoBd.StrEmpresaFacturador;

					if ((tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste || documento_obj.TipoOperacion == 3))
					{
						facturador_formato = Constantes.NitResolucionconPrefijo;
					}

					TblFormatos datos_formato = clase_formatos.ObtenerFormato(formato_documento.Codigo, facturador_formato, TipoFormato.FormatoPDF.GetHashCode(), tipo_doc);

					if (tipo_doc.GetHashCode() < TipoDocumento.AcuseRecibo.GetHashCode())
					{
						List<DocumentoDetalle> detalles_formato = new List<DocumentoDetalle>();
						//Recorre los detalles y agrega los items visibles.
						foreach (var item in documento_obj.DocumentoDetalles)
						{
							if (item.OcultarItem == 0)
								detalles_formato.Add(item);
							documento_obj.ValorDescuentoDet += item.DescuentoValor;
						}

						documento_obj.DocumentoDetalles = detalles_formato;
					}

					//Se llena propiedad del medio de pago para poder mostrarla en la representacion grafica
					if (tipo_doc == TipoDocumento.Factura)
					{
						ListaMediosPago list_medio = new ListaMediosPago();
						ListaItem medio = list_medio.Items.Where(d => d.Codigo.Equals(documento_obj.TerminoPago.ToString())).FirstOrDefault();
						documento_obj.TerminoPago_Descripcion = medio.Descripcion;

						Ctl_EmpresaResolucion empresa_resolucion = new Ctl_EmpresaResolucion();
						TblEmpresasResoluciones resoluciones_bd = empresa_resolucion.ObtenerResoluciones(facturador_formato, documentoBd.StrNumResolucion, false).FirstOrDefault();

						int meses_vigencia = Math.Abs((resoluciones_bd.DatFechaVigenciaDesde.Month - resoluciones_bd.DatFechaVigenciaHasta.Month) + 12 * (resoluciones_bd.DatFechaVigenciaDesde.Year - resoluciones_bd.DatFechaVigenciaHasta.Year));
						documento_obj.ResolucionCompleta = string.Format("Resolución No {0}, Fecha: {1}, del No.{2} {3} al {4} {5}, vigencia: {6} meses", resoluciones_bd.StrNumResolucion, resoluciones_bd.DatFechaVigenciaDesde.ToString("yyyy-MM-dd"), resoluciones_bd.StrPrefijo, resoluciones_bd.IntRangoInicial, resoluciones_bd.StrPrefijo, resoluciones_bd.IntRangoFinal, meses_vigencia);
					}

					if ((tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste) && (documento_obj.DatosPago != null))
					{
						//Se agrega estos metodos que son los mas normales en la nomina para el tema de impresion
						if (documento_obj.DatosPago.Metodo != 1 && documento_obj.DatosPago.Metodo != 10 && documento_obj.DatosPago.Metodo != 42 && documento_obj.DatosPago.Metodo != 47)
						{
							documento_obj.DatosPago.Metodo = 1;
						}	
						ListaMediosPago list_medio = new ListaMediosPago();
						ListaItem medio = list_medio.Items.Where(d => d.Codigo.Equals(documento_obj.DatosPago.Metodo.ToString())).FirstOrDefault();
						documento_obj.DatosPago.TerminoPago_Descripcion = medio.Descripcion;
					}



					if (datos_formato != null)
					{
						reporte_archivado = false;
						XtraReportDesigner rep = new XtraReportDesigner();

						MemoryStream datos = new MemoryStream(datos_formato.Formato);
						rep.LoadLayoutFromXml(datos);


						if (tipo_doc == TipoDocumento.Nomina || tipo_doc == TipoDocumento.NominaAjuste)
						{
							Planilla objeto_planilla_nomina = new Planilla();
							objeto_planilla_nomina = convertirObjetoPlanilla(documento_obj, tipo_doc);
							rep.DataSource = objeto_planilla_nomina;
						}
						else
						{
							rep.DataSource = documento_obj;
						}
						
						HGInetFacturaEReports.Reporte x = new HGInetFacturaEReports.Reporte(documento_result.NombreXml, documento_result.RutaArchivosEnvio);
						x.GenerarPdfDev(rep, documentoBd.StrEmpresaFacturador);

						// almacena el objeto en archivo json
						try
						{
							File.WriteAllText(ruta_archivo_formato, JsonConvert.SerializeObject(formato_documento));
						}
						catch (Exception)
						{}

					}
					else if(facturador.IntHabilitacion == Habilitacion.Produccion.GetHashCode())
					{
						switch (formato_documento.Codigo)
						{
							case 1:
								switch (documento_result.DocumentoTipo)
								{
									case TipoDocumento.Factura:
										reporte_pdf = new HGInetFacturaEReports.Facturas.Formato1();
										break;
									case TipoDocumento.NotaDebito:
										reporte_pdf = new HGInetFacturaEReports.NotasDebito.Formato1();
										break;
									case TipoDocumento.NotaCredito:
										reporte_pdf = new HGInetFacturaEReports.NotasCredito.Formato1();
										break;
								}
								break;
							case 2:
								reporte_pdf = new HGInetFacturaEReports.Facturas.Formato2();
								reporte_pdf.ReportParameters["TipoDocumento"].Value = documento_result.DocumentoTipo.GetHashCode();
								break;
							case 3:
								reporte_pdf = new HGInetFacturaEReports.Facturas.Formato3();
								reporte_pdf.ReportParameters["TipoDocumento"].Value = documento_result.DocumentoTipo.GetHashCode();
								break;
							case 4:
								reporte_pdf = new HGInetFacturaEReports.Facturas.Formato4();
								reporte_pdf.ReportParameters["TipoDocumento"].Value = documento_result.DocumentoTipo.GetHashCode();
								break;
							case 5:
								reporte_pdf = new HGInetFacturaEReports.Facturas.Formato5();
								reporte_pdf.ReportParameters["TipoDocumento"].Value = documento_result.DocumentoTipo.GetHashCode();
								break;

							default:
								switch (documento_result.DocumentoTipo)
								{
									case TipoDocumento.Factura:
										reporte_pdf = new HGInetFacturaEReports.Facturas.Formato1();
										break;
									case TipoDocumento.NotaDebito:
										reporte_pdf = new HGInetFacturaEReports.NotasDebito.Formato1();
										break;
									case TipoDocumento.NotaCredito:
										reporte_pdf = new HGInetFacturaEReports.NotasCredito.Formato1();
										break;
								}
								break;
						}
					}
					else
					{
						throw new ArgumentException(string.Format("No se encuentra registrado el codigo del formato {0} del Facturador {1}", formato_documento.Codigo,facturador.StrIdentificacion));
					}

					if (reporte_archivado)
					{
						//Asigna los datos al reporte y genera el pdf
						reporte_pdf.DataSource = documento_obj;
						HGInetFacturaEReports.Reporte x = new HGInetFacturaEReports.Reporte(documento_result.NombreXml, documento_result.RutaArchivosEnvio);
						x.GenerarPdf(reporte_pdf);

						// almacena el objeto en archivo json
						try
						{
							File.WriteAllText(ruta_archivo_formato, JsonConvert.SerializeObject(formato_documento));
						}
						catch (Exception)
						{ }

					}

					respuesta.UrlPdf = string.Format(@"{0}/{1}/{2}.pdf", url_ppal_pdf, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);
					respuesta.IdEstado = CategoriaEstado.Recibido.GetHashCode();
					respuesta.DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.Recibido);

					//Actualiza el registro en la base de datos.
					documentoBd.StrUrlArchivoPdf = respuesta.UrlPdf;
					documentoBd.IdCategoriaEstado = CategoriaEstado.Recibido.GetHashCode();
				}



			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el almacenamiento del documento PDF. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
			}

			return respuesta;
		}

		public static Planilla convertirObjetoPlanilla(object documento_obj, TipoDocumento tipo_doc)
		{
			try
			{

				Planilla objeto_result = new Planilla();

				var documento = (dynamic)null;
				documento = documento_obj;

				//Informacion Fecha y periodo
				
				objeto_result.Año = (documento.DatosPeriodo != null) ? (short)documento.DatosPeriodo.FechaLiquidacionInicio.Year : (short)documento.FechaGen.Year;
				objeto_result.Mes = (documento.DatosPeriodo != null) ? (short)documento.DatosPeriodo.FechaLiquidacionInicio.Month : (short)documento.FechaGen.Month;
				objeto_result.Fecha = documento.FechaGen;
				objeto_result.PeriodoFechaI = (documento.DatosPeriodo != null) ? documento.DatosPeriodo.FechaLiquidacionInicio : documento.FechaGen;
				objeto_result.PeriodoFechaF = (documento.DatosPeriodo != null) ? documento.DatosPeriodo.FechaLiquidacionFin : documento.FechaGen;

				//Informacion de Empresa
				objeto_result.EmpresaNombre = documento.DatosEmpleador.RazonSocial;
				objeto_result.EmpresaNit = documento.DatosEmpleador.Identificacion;
				objeto_result.EmpresaTelefono = documento.DatosEmpleador.Telefono;
				objeto_result.EmpresaDireccion = documento.DatosEmpleador.Direccion;

				objeto_result.DocumentoFormato = new Formato();
				objeto_result.DocumentoFormato.Titulo = Enumeracion.GetDescription(tipo_doc);

				if (documento.DocumentoFormato != null && documento.DocumentoFormato.CamposPredeterminados != null)
				{
					try
					{
						List<FormatoCampo> campos_predeterminados = documento.DocumentoFormato.CamposPredeterminados;
						FormatoCampo campo_logo = campos_predeterminados.Where(x => x.Ubicacion.Equals("campo1")).FirstOrDefault();
						objeto_result.EmpresaLogo = Convert.FromBase64String(campo_logo.Valor);
					}
					catch (Exception)
					{
					   
					}
				}

				//Informacion de Empleado
				objeto_result.Empleado = documento.DatosTrabajador.Identificacion;
				objeto_result.EmpleadoNombre = string.Format("{0} {1} {2} {3}",documento.DatosTrabajador.PrimerApellido, documento.DatosTrabajador.SegundoApellido, documento.DatosTrabajador.PrimerNombre, documento.DatosTrabajador.OtrosNombres);
				objeto_result.EmpleadoDir = documento.DatosTrabajador.LugarTrabajoDireccion;
				objeto_result.EmpleadoTel = documento.DatosTrabajador.Telefono;
				objeto_result.EmpleadoEmail = documento.DatosTrabajador.Email;
				objeto_result.EmpleadoSalario = documento.DatosTrabajador.Sueldo;
				objeto_result.EmpleadoCuenta = (documento.DatosPago != null) ? documento.DatosPago.NumeroCuenta : "";

				objeto_result.MedioPago = (documento.DatosPago != null) ? documento.DatosPago.TerminoPago_Descripcion : "Instrumento no definido";

				//Informacion del documento
				objeto_result.Documento = documento.Documento;
				objeto_result.Prefijo = documento.Prefijo;
				objeto_result.CUNE = documento.Cune;

				//Informacion Totales
				objeto_result.DevengadosTotal = documento.DevengadosTotal;
				objeto_result.DeduccionesTotal = documento.DeduccionesTotal;
				objeto_result.ComprobanteTotal = documento.ComprobanteTotal;

				objeto_result.CodigoQR = ObtenerQR(tipo_doc, objeto_result.Prefijo, objeto_result.Documento, objeto_result.Fecha,objeto_result.EmpresaNit,objeto_result.Empleado,objeto_result.DevengadosTotal, objeto_result.DeduccionesTotal, objeto_result.ComprobanteTotal);

				short orden_novedades = 0;
				Novedad novedad = new Novedad();

				//Informacion Novedades(Detalles)
				if (documento.DatosDevengados != null)
				{
					objeto_result.Novedades = new List<Novedad>();
					Devengados devengado = new Devengados();
					devengado = documento.DatosDevengados;

					if (devengado.SueldoTrabajado > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Salario";
						novedad.ConceptoDes = "Salario";
						novedad.Cantidad = devengado.DiasTrabajados;
						novedad.Dev = devengado.SueldoTrabajado;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (devengado.ApoyoSostenimiento > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "ApoyoSostenimiento";
						novedad.ConceptoDes = "Apoyo de Sostenimiento";
						novedad.Cantidad = devengado.DiasTrabajados;
						novedad.Dev = devengado.ApoyoSostenimiento;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (devengado.DatosTransporte != null && devengado.DatosTransporte.Count() > 0)
					{
						Transporte transporte = new Transporte();
						transporte.AuxilioTransporte = devengado.DatosTransporte.Sum(x => x.AuxilioTransporte);
						if (transporte.AuxilioTransporte > 0)
						{
							novedad = new Novedad();
							novedad.Concepto = "AuxTransporte";
							novedad.ConceptoDes = "AuxilioTransporte";
							novedad.Dev = transporte.AuxilioTransporte;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

						transporte.ViaticoManuAlojNS = devengado.DatosTransporte.Sum(x => x.ViaticoManuAlojNS);
						if (transporte.ViaticoManuAlojNS > 0)
						{
							novedad = new Novedad();
							novedad.Concepto = "ViaticoManuAlojNS";
							novedad.ConceptoDes = "Viatico Manutencion Alojamiento No Salarial";
							novedad.Dev = transporte.ViaticoManuAlojNS;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

						transporte.ViaticoManuAlojS = devengado.DatosTransporte.Sum(x => x.ViaticoManuAlojS);
						if (transporte.ViaticoManuAlojS > 0)
						{
							novedad = new Novedad();
							novedad.Concepto = "ViaticoManuAlojS";
							novedad.ConceptoDes = "Viatico Manutencion Alojamiento Salarial";
							novedad.Dev = transporte.ViaticoManuAlojNS;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

					}

					//Se valida Horas extras, festivas, nocturnas y recargos
					if (devengado.DatosHoras != null && devengado.DatosHoras.Count > 0)
					{

						foreach (Hora item in devengado.DatosHoras)
						{

							TipoHoraNomina Hora = Enumeracion.GetEnumObjectByValue<TipoHoraNomina>(item.TipoHora);

							switch (Hora)
							{
								case TipoHoraNomina.ExtraDiurna:

									novedad = new Novedad();
									novedad.Concepto = item.CodigoConcepto;
									novedad.ConceptoDes = "Horas Extras Diarias";
									novedad.Cantidad = item.Cantidad;
									novedad.Dev = item.Valor;
									novedad.Porcentaje = item.Porcentaje;
									novedad.orden = orden_novedades;
									objeto_result.Novedades.Add(novedad);
									orden_novedades += 1;

									break;

								case TipoHoraNomina.ExtraNocturna:

									novedad = new Novedad();
									novedad.Concepto = item.CodigoConcepto;
									novedad.ConceptoDes = "Horas Extras Nocturnas";
									novedad.Cantidad = item.Cantidad;
									novedad.Dev = item.Valor;
									novedad.Porcentaje = item.Porcentaje;
									novedad.orden = orden_novedades;
									objeto_result.Novedades.Add(novedad);
									orden_novedades += 1;

									break;
								case TipoHoraNomina.RecargoNocturno:

									novedad = new Novedad();
									novedad.Concepto = item.CodigoConcepto;
									novedad.ConceptoDes = "Horas Recargo Nocturno";
									novedad.Cantidad = item.Cantidad;
									novedad.Dev = item.Valor;
									novedad.Porcentaje = item.Porcentaje;
									novedad.orden = orden_novedades;
									objeto_result.Novedades.Add(novedad);
									orden_novedades += 1;

									break;
								case TipoHoraNomina.DominicalesFestivas:

									novedad = new Novedad();
									novedad.Concepto = item.CodigoConcepto;
									novedad.ConceptoDes = "Horas Extras Diarias Dominicales y Festivas";
									novedad.Cantidad = item.Cantidad;
									novedad.Dev = item.Valor;
									novedad.Porcentaje = item.Porcentaje;
									novedad.orden = orden_novedades;
									objeto_result.Novedades.Add(novedad);
									orden_novedades += 1;

									break;
								case TipoHoraNomina.RecargoDominicalesFestivas:

									novedad = new Novedad();
									novedad.Concepto = item.CodigoConcepto;
									novedad.ConceptoDes = "Horas Recargo Diarias Dominicales y Festivas";
									novedad.Cantidad = item.Cantidad;
									novedad.Dev = item.Valor;
									novedad.Porcentaje = item.Porcentaje;
									novedad.orden = orden_novedades;
									objeto_result.Novedades.Add(novedad);
									orden_novedades += 1;

									break;
								case TipoHoraNomina.ExtraDominicalesFestivasNocturnas:

									novedad = new Novedad();
									novedad.Concepto = item.CodigoConcepto;
									novedad.ConceptoDes = "Horas Extras Nocturnas Dominicales y Festivas";
									novedad.Cantidad = item.Cantidad;
									novedad.Dev = item.Valor;
									novedad.Porcentaje = item.Porcentaje;
									novedad.orden = orden_novedades;
									objeto_result.Novedades.Add(novedad);
									orden_novedades += 1;

									break;
								case TipoHoraNomina.RecargoDominicalesFestivasNocturnas:

									novedad = new Novedad();
									novedad.Concepto = item.CodigoConcepto;
									novedad.ConceptoDes = "Horas Recargo Nocturno Dominicales y Festivas";
									novedad.Cantidad = item.Cantidad;
									novedad.Dev = item.Valor;
									novedad.Porcentaje = item.Porcentaje;
									novedad.orden = orden_novedades;
									objeto_result.Novedades.Add(novedad);
									orden_novedades += 1;

									break;
								default:
									break;
							}

						}

					}

					if (devengado.Vacaciones != null && devengado.Vacaciones.Count() > 0)
					{


						foreach (NovedadGeneral item in devengado.Vacaciones)
						{
							

							novedad = new Novedad();
							if (item.Tipo.Equals(1))
							{
								novedad.Concepto = "VacacionesComunes";
								novedad.ConceptoDes = "Vacaciones Comunes";
							}	
							else
							{
								novedad.Concepto = "VacacionesCompensadas";
								novedad.ConceptoDes = "VacacionesCompensadas";
							}
							
							novedad.Cantidad = item.Cantidad;
							novedad.Dev = item.Pago;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}


					}

					if (devengado.PagoPrima != null && devengado.PagoPrima.Pago != null)
					{

						if (devengado.PagoPrima.Pago.Pago > 0)
						{
							novedad = new Novedad();
							novedad.Concepto = "Prima";
							novedad.ConceptoDes = "Prima";
							novedad.Cantidad = devengado.PagoPrima.Cantidad;
							novedad.Dev = devengado.PagoPrima.Pago.Pago;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

						if (devengado.PagoPrima.Pago.PagoNS > 0)
						{
							novedad = new Novedad();
							novedad.Concepto = "PrimaNS";
							novedad.ConceptoDes = "Prima No Salarial";
							novedad.Cantidad = devengado.PagoPrima.Cantidad;
							novedad.Dev = devengado.PagoPrima.Pago.PagoNS;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

					}

					if (devengado.PagoCesantias != null)
					{
						if (devengado.PagoCesantias.Pago > 0)
						{
							novedad = new Novedad();
							novedad.Concepto = "Cesantias";
							novedad.ConceptoDes = "Cesantias";
							novedad.Dev = devengado.PagoCesantias.Pago;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

						if (devengado.PagoCesantias.PagoIntereses > 0)
						{
							novedad = new Novedad();
							novedad.Concepto = "IntCesantias";
							novedad.ConceptoDes = "Intereses a las Cesantias";
							novedad.Dev = devengado.PagoCesantias.PagoIntereses;
							novedad.Porcentaje = devengado.PagoCesantias.Porcentaje;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

					}

					if (devengado.Incapacidades != null && devengado.Incapacidades.Count() > 0)
					{

						foreach (NovedadGeneral item in devengado.Incapacidades)
						{

							novedad = new Novedad();
							if (item.Tipo.Equals(1))
							{
								novedad.Concepto = "IncapacidadC";
								novedad.ConceptoDes = "Incapacidad Común";
							}
							else if (item.Tipo.Equals(2))
							{
								novedad.Concepto = "IncapacidadP";
								novedad.ConceptoDes = "Incapacidad Profesional";
							}
							else if (item.Tipo.Equals(3))
							{
								novedad.Concepto = "IncapacidadL";
								novedad.ConceptoDes = "Incapacidad Laboral";
							}
							novedad.Cantidad = item.Cantidad;
							novedad.Dev = item.Pago;
							novedad.FechaIni = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
							novedad.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;

						}

					}

					if (devengado.Licencias != null && devengado.Licencias.Count() > 0)
					{

						foreach (NovedadGeneral item in devengado.Licencias)
						{

							novedad = new Novedad();
							if (item.Tipo.Equals(1))
							{
								novedad.Concepto = "LicenciaMP";
								novedad.ConceptoDes = "Licencia de Maternidad o Paternidad";
							}
							else if (item.Tipo.Equals(2))
							{
								novedad.Concepto = "LicenciaR";
								novedad.ConceptoDes = "Licencia Remunerada";
							}
							else if (item.Tipo.Equals(3))
							{
								novedad.Concepto = "LicenciaNR";
								novedad.ConceptoDes = "Licencia No Remunerada";
							}
							novedad.Cantidad = item.Cantidad;
							novedad.Dev = item.Pago;
							novedad.FechaIni = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
							novedad.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;

						}

					}

					if (devengado.Bonificaciones != null && devengado.Bonificaciones.Count() > 0)
					{
						foreach (NovedadSalNoSal item in devengado.Bonificaciones)
						{

							if (item.Pago > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "Bonificacion";
								novedad.ConceptoDes = "Bonificacion Salarial";
								novedad.Dev = item.Pago;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;

							}
							else if (item.PagoNS > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "BonificacionNS";
								novedad.ConceptoDes = "Bonificacion No Salarial";
								novedad.Dev = item.PagoNS;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;
							}

						}

					}

					if (devengado.Auxilios != null && devengado.Auxilios.Count() > 0)
					{

						
						foreach (NovedadSalNoSal item in devengado.Auxilios)
						{

							if (item.Pago > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "Auxilio";
								novedad.ConceptoDes = "Auxilio Salarial";
								novedad.Dev = item.Pago;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;

							}
							else if (item.PagoNS > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "AuxilioNS";
								novedad.ConceptoDes = "Auxilio No Salarial";
								novedad.Dev = item.PagoNS;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;
							}

						}
					}

					if (devengado.HuelgaLegal != null && devengado.HuelgaLegal.Count() > 0)
					{
						foreach (NovedadGeneral item in devengado.HuelgaLegal)
						{

							novedad.Concepto = "HuelgaLegal";
							novedad.ConceptoDes = "Huelga Legal";
							novedad.Cantidad = item.Cantidad;
							novedad.FechaIni = Convert.ToDateTime(item.FechaInicio.ToString(Fecha.formato_fecha_hginet));
							novedad.FechaFin = Convert.ToDateTime(item.FechaFin.ToString(Fecha.formato_fecha_hginet));
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;

						}
					}

					if (devengado.OtrosConceptos != null && devengado.OtrosConceptos.Count() > 0)
					{
						
						foreach (OtroConcepto item in devengado.OtrosConceptos)
						{

							if (item.PagoConcepto.Pago > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "OtroConceptoS";
								novedad.ConceptoDes = item.DescripcionConcepto;
								novedad.Dev = item.PagoConcepto.Pago;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;

							}
							else if (item.PagoConcepto.PagoNS > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "OtroConceptoNS";
								novedad.ConceptoDes = item.DescripcionConcepto;
								novedad.Dev = item.PagoConcepto.PagoNS;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;
							}

						}
					}

					if (devengado.Compensaciones != null && devengado.Compensaciones.Count() > 0)
					{
						foreach (NovedadSalNoSal item in devengado.Compensaciones)
						{

							if (item.Pago > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "CompensacionO";
								novedad.ConceptoDes = "Compensaciones Ordinarias";
								novedad.Dev = item.Pago;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;

							}
							else if (item.PagoNS > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "CompensacionE";
								novedad.ConceptoDes = "Compensaciones Extraordinarias";
								novedad.Dev = item.PagoNS;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;
							}

						}

					}

					if (devengado.BonoEPCTV != null && devengado.BonoEPCTV.Count() > 0)
					{
						foreach (NovedadSalNoSal item in devengado.BonoEPCTV)
						{

							if (item.Pago > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "BonoEPCTVS";
								novedad.ConceptoDes = "Bonos Electronicos o de Papel de Servicio, Cheques, Tarjetas, Vales Salarial";
								novedad.Dev = item.Pago;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;

							}
							else if (item.PagoNS > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "BonoEPCTVNS";
								novedad.ConceptoDes = "Bonos Electronicos o de Papel de Servicio, Cheques, Tarjetas, Vales No Salarial";
								novedad.Dev = item.PagoNS;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;
							}

						}
					}

					if (devengado.BonoAlimentacion != null && devengado.BonoAlimentacion.Count() > 0)
					{
						foreach (NovedadSalNoSal item in devengado.BonoAlimentacion)
						{

							if (item.Pago > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "BonoAlimentacionS";
								novedad.ConceptoDes = "Bono Alimentacion Salarial";
								novedad.Dev = item.Pago;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;

							}
							else if (item.PagoNS > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "BonoAlimentacionNS";
								novedad.ConceptoDes = "Bono Alimentacion No Salarial";
								novedad.Dev = item.PagoNS;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;
							}

						}
					}

					if (devengado.Comisiones != null && devengado.Comisiones.Count() > 0)
					{

						foreach (decimal item in devengado.Comisiones)
						{
							novedad = new Novedad();
							novedad.Concepto = "Comision";
							novedad.ConceptoDes = "Comision";
							novedad.Dev = item;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}
	
					}

					if (devengado.PagosTerceros != null && devengado.PagosTerceros.Count() > 0)
					{
						foreach (decimal item in devengado.PagosTerceros)
						{
							novedad = new Novedad();
							novedad.Concepto = "PagosTerceros";
							novedad.ConceptoDes = "Pagos a Terceros";
							novedad.Dev = item;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}
					}

					if (devengado.Anticipos != null && devengado.Anticipos.Count() > 0)
					{
						foreach (decimal item in devengado.Anticipos)
						{
							novedad = new Novedad();
							novedad.Concepto = "Anticipos";
							novedad.ConceptoDes = "Anticipos";
							novedad.Dev = item;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}
					}

					if (devengado.Dotacion > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Dotacion";
						novedad.ConceptoDes = "Dotacion";
						novedad.Dev = devengado.Dotacion;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (devengado.Teletrabajo > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Teletrabajo";
						novedad.ConceptoDes = "Teletrabajo";
						novedad.Dev = devengado.Teletrabajo;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (devengado.BonifRetiro > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "BonifRetiro";
						novedad.ConceptoDes = "Bonificacion Retiro";
						novedad.Dev = devengado.BonifRetiro;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (devengado.Indemnizacion > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Indemnizacion";
						novedad.ConceptoDes = "Indemnizacion";
						novedad.Dev = devengado.Indemnizacion;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}
					if (devengado.Reintegro > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Reintegro";
						novedad.ConceptoDes = "Reintegro";
						novedad.Dev = devengado.Reintegro;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					
				}

				if (documento.DatosDeducciones != null)
				{
					if (objeto_result.Novedades == null)
						objeto_result.Novedades = new List<Novedad>();

					Deducciones deduccion = new Deducciones();
					deduccion = documento.DatosDeducciones;

					if (deduccion.Salud != null && deduccion.Salud.Deduccion > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Salud";
						novedad.ConceptoDes = "Salud";
						novedad.Ded = deduccion.Salud.Deduccion;
						novedad.Porcentaje = deduccion.Salud.Porcentaje;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;

					}

					if (deduccion.Pension != null && deduccion.Pension.Deduccion > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Pension";
						novedad.ConceptoDes = "Pension";
						novedad.Ded = deduccion.Pension.Deduccion;
						novedad.Porcentaje = deduccion.Pension.Porcentaje;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;

					}


					if (deduccion.DatosFondoSP != null)
					{

						if (deduccion.DatosFondoSP.DeduccionFSP > 0)
						{
							novedad = new Novedad();
							novedad.Concepto = "DeduccionSP";
							novedad.ConceptoDes = "Fondo de Seguridad Pensional";
							novedad.Ded = deduccion.DatosFondoSP.DeduccionFSP;
							novedad.Porcentaje = deduccion.DatosFondoSP.Porcentaje;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

						if (deduccion.DatosFondoSP.DeduccionSub > 0)
						{
							novedad = new Novedad();
							novedad.Concepto = "DeduccionSub";
							novedad.ConceptoDes = "Fondo de Subsistencia";
							novedad.Ded = deduccion.DatosFondoSP.DeduccionSub;
							novedad.Porcentaje = deduccion.DatosFondoSP.PorcentajeSub;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

						
					}

					if (deduccion.DatosSindicatos != null && deduccion.DatosSindicatos.Count() > 0)
					{
						foreach (NovedadDeduccion item in deduccion.DatosSindicatos)
						{
							novedad = new Novedad();
							novedad.Concepto = "Sindicato";
							novedad.ConceptoDes = "Sindicato";
							novedad.Ded = item.Deduccion;
							novedad.Porcentaje = item.Porcentaje;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;

						}

					}

					if (deduccion.DatosSanciones != null && deduccion.DatosSanciones.Count() > 0)
					{

						List<NominaIndividualTypeDeduccionesSancion> lista = new List<NominaIndividualTypeDeduccionesSancion>();

						foreach (Sancion item in deduccion.DatosSanciones)
						{

							if (item.SancionPriv > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "SancionPriv";
								novedad.ConceptoDes = "Sancion Privada";
								novedad.Ded = item.SancionPriv;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;
							}

							if (item.SancionPublic > 0)
							{
								novedad = new Novedad();
								novedad.Concepto = "SancionPublic";
								novedad.ConceptoDes = "Sancion Publica";
								novedad.Ded = item.SancionPublic;
								novedad.orden = orden_novedades;
								objeto_result.Novedades.Add(novedad);
								orden_novedades += 1;
							}
						}
					}

					if (deduccion.DatosLibranzas != null && deduccion.DatosLibranzas.Count() > 0)
					{

						List<NominaIndividualTypeDeduccionesLibranza> lista = new List<NominaIndividualTypeDeduccionesLibranza>();

						foreach (Libranza item in deduccion.DatosLibranzas)
						{
							novedad = new Novedad();
							novedad.Concepto = "Libranza";
							novedad.ConceptoDes = item.Descripcion;
							novedad.Ded = item.Deduccion;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;

						}

					}

					if (deduccion.PagosTerceros != null && deduccion.PagosTerceros.Count() > 0)
					{
						foreach (decimal item in deduccion.PagosTerceros)
						{
							novedad = new Novedad();
							novedad.Concepto = "PagosTerceros";
							novedad.ConceptoDes = "Pagos a Terceros";
							novedad.Ded = item;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}

					}

					if (deduccion.Anticipos != null && deduccion.Anticipos.Count() > 0)
					{
						foreach (decimal item in deduccion.Anticipos)
						{
							novedad = new Novedad();
							novedad.Concepto = "Anticipo";
							novedad.ConceptoDes = "Anticipo";
							novedad.Ded = item;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}
					}

					if (deduccion.OtrasDeducciones != null && deduccion.OtrasDeducciones.Count() > 0)
					{
						foreach (decimal item in deduccion.OtrasDeducciones)
						{
							novedad = new Novedad();
							novedad.Concepto = "OtrasDeducciones";
							novedad.ConceptoDes = "Otras Deducciones";
							novedad.Ded = item;
							novedad.orden = orden_novedades;
							objeto_result.Novedades.Add(novedad);
							orden_novedades += 1;
						}
					}

					if (deduccion.PensionVoluntaria > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "PensionVoluntaria";
						novedad.ConceptoDes = "Pension Voluntaria";
						novedad.Ded = deduccion.PensionVoluntaria;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (deduccion.RetencionFuente > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "RetencionFuente";
						novedad.ConceptoDes = "Retencion en la Fuente";
						novedad.Ded = deduccion.RetencionFuente;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (deduccion.AFC > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "AFC";
						novedad.ConceptoDes = "Ahorro Fomento a la contruccion";
						novedad.Ded = deduccion.AFC;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (deduccion.Cooperativa > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Cooperativa";
						novedad.ConceptoDes = "Cooperativa";
						novedad.Ded = deduccion.Cooperativa;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (deduccion.EmbargoFiscal > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "EmbargoFiscal";
						novedad.ConceptoDes = "Embargo Fiscal";
						novedad.Ded = deduccion.EmbargoFiscal;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (deduccion.PlanComplementarios > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "PlanComplementarios";
						novedad.ConceptoDes = "Plan Complementarios";
						novedad.Ded = deduccion.PlanComplementarios;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (deduccion.Educacion > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Educacion";
						novedad.ConceptoDes = "Educacion";
						novedad.Ded = deduccion.Educacion;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (deduccion.Reintegro > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Reintegro";
						novedad.ConceptoDes = "Deduccion Reintegro";
						novedad.Ded = deduccion.Reintegro;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}

					if (deduccion.Deuda > 0)
					{
						novedad = new Novedad();
						novedad.Concepto = "Deuda";
						novedad.ConceptoDes = "Deuda";
						novedad.Ded = deduccion.Deuda;
						novedad.orden = orden_novedades;
						objeto_result.Novedades.Add(novedad);
						orden_novedades += 1;
					}



				}
				
				return objeto_result;

			}
			catch (Exception excepcion)
			{
				string mensaje = string.Format("inconsistencia conviertiendo objeto a planilla de Pago - {0}", excepcion.Message);
				throw new ApplicationException(mensaje, excepcion.InnerException);
			}
		}

		public static string ObtenerQR(TipoDocumento DocumentoTipo, string Prefijo, long NumDocumento, DateTime FechaDocumento, string IdentificacionEmpleador, string IdentificacionTrabajador, decimal DevengadoTotal, decimal DeduccionTotal, decimal Total)
		{
			string QR = "";

			try
			{
				string DescripcionDoc = "NumNE:";
				string DescripcionFecha = "FecNE:";
				string DescripcionHora = "HorNE:";
				string DescripcionDevengado = "ValDev:";
				string DescripcionDeduccion = "ValDed:";
				string DescripcionTotal = "ValTolNE:";
				string DescripcionTipoDoc = "";

				if (DocumentoTipo == TipoDocumento.Nomina)
				{
					DescripcionTipoDoc = "102";
				}
				else if (DocumentoTipo == TipoDocumento.NominaAjuste)
				{
					DescripcionTipoDoc = "103";
				}

				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
				string ambiente_dian = string.Empty;
				//

				if (plataforma_datos.RutaPublica.Contains("habilitacion") || plataforma_datos.RutaPublica.Contains("localhost"))
					ambiente_dian = "2";
				else
					ambiente_dian = "1";

				DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

				string software_pin = data_dian.Pin;


				//@DescripcionDoc + cast(TblTransacciones.StrPrefijo as varchar) + cast(intDocumento as varchar) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", DescripcionDoc, Prefijo, NumDocumento);

				//(@DescripcionFecha + CAST((CAST(year(DatFecha)AS varchar) + '-' + RIGHT('00' + convert(varchar, Month(DatFecha)), 2) + '-' + RIGHT('00' + convert(varchar, day(DatFecha)), 2)) as varchar)) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, DescripcionFecha, FechaDocumento.ToString(Fecha.formato_fecha_hginet));

				//(@DescripcionHora + RIGHT('00' + convert(varchar, DATEPART(hour, DATEADD(hour, 5, tbldocumentos.DatFechaGra))), 2) + ':' + RIGHT('00' + convert(varchar, DATEPART(minute, tbldocumentos.DatFechaGra)), 2) + ':' + RIGHT('00' + convert(varchar, DATEPART(second, tbldocumentos.DatFechaGra)), 2) + '-5:00') + CHAR(10) +
				QR = string.Format("{0}{1}{2}{3}\n", QR, DescripcionHora, FechaDocumento.ToString(Fecha.formato_hora_completa), "-05:00");

				//(@DescripcionValor + CAST(CAST(tbldocumentos.IntSubTotal As numeric(17, 2)) as varchar)) + Char(10) +
				QR = string.Format("{0}{1}{2}\n", QR, DescripcionDevengado, DevengadoTotal.ToString());

				//('ValIva:' + CAST(CAST(tbldocumentos.IntIva AS numeric(17, 2)) as varchar)) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, DescripcionDeduccion, DeduccionTotal.ToString());

				//(@DescripcionTotal + CAST(CAST(tbldocumentos.IntTotal AS numeric(17, 2)) as varchar)) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, DescripcionTotal, Total.ToString());

				//('NitFac:' + TblEmpresas.StrNit) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, "NitNE:", IdentificacionEmpleador);

				//('DocAdq:' + tbldocumentos.strtercero) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, "DocEmp:", IdentificacionTrabajador);

				//('ValOtroIm:' + CAST(CAST(tbldocumentos.IntVrImpuesto1 AS numeric(17, 2)) as varchar)) + CHAR(10) +
				QR = string.Format("{0}{1}{2}\n", QR, "TipoXML:", DescripcionTipoDoc);

				//(@DescripcionCufe + ':' + tbldocumentos.strFacturaECufe)
				QR = string.Format("{0}{1}{2}\n", QR, "Software-Pin:", software_pin);

				//(@DescripcionCufe + ':' + tbldocumentos.strFacturaECufe)
				QR = string.Format("{0}{1}{2}\n", QR, "TipAmb:", ambiente_dian);

				//QR = string.Format("{0}{1}\n", QR, ruta_validar_doc);

				return QR;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
