using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class EventosRadianController : ApiController
	{

		[HttpGet]
		[Route("api/ObtenerEventosRadian")]
		public IHttpActionResult ObtenerEventosRadian(Guid id_seguridad)
		{
			try
			{
				Sesion.ValidarSesion();

				Ctl_EventosRadian Ctrl_Eventos = new Ctl_EventosRadian();

				List<TblEventosRadian> eventos = Ctrl_Eventos.Obtener(id_seguridad);

				//variables que indican que boton de evento va estar activo para hacer en el documento
				bool inscribir_documento = true;
				bool endoso = true;
				bool informe_pago = true;
				bool pago = true;
				bool tacito = false;
				//bool otros_eventos = false;

				decimal valor_factura = 0.00M;
				decimal valor_pagado = 0.00M;

				TblEventosRadian evento_reciboM = eventos.Where(x => x.IntEstadoEvento == CodigoResponseV2.Aceptado.GetHashCode()).FirstOrDefault();

				TblEventosRadian evento_tacito = eventos.Where(x => x.IntEstadoEvento == CodigoResponseV2.AprobadoTacito.GetHashCode()).FirstOrDefault();

				TblEventosRadian evento_expresa = eventos.Where(x => x.IntEstadoEvento == CodigoResponseV2.Expresa.GetHashCode()).FirstOrDefault();

				TblEventosRadian evento_inscripcion = eventos.Where(x => x.IntEstadoEvento == CodigoResponseV2.Inscripcion.GetHashCode()).FirstOrDefault();

				TblEventosRadian evento_endoso = eventos.Where(x => x.IntEstadoEvento == CodigoResponseV2.EndosoPp.GetHashCode()).FirstOrDefault();

				TblEventosRadian evento_informeP = eventos.Where(x => x.IntEstadoEvento == CodigoResponseV2.InformePago.GetHashCode()).FirstOrDefault();

				TblEventosRadian evento_pago = eventos.Where(x => x.IntEstadoEvento == CodigoResponseV2.PagoFvTV.GetHashCode()).FirstOrDefault();

				if ((evento_tacito != null || evento_expresa != null) && evento_inscripcion == null)
				{
					inscribir_documento = false;
				}

				if (evento_tacito == null && evento_expresa == null && evento_reciboM != null)
				{
					tacito = true;
				}

				if (evento_inscripcion != null && (evento_endoso == null || evento_informeP == null || evento_pago == null))
				{
					Ctl_Documento ctl_documento = new Ctl_Documento();

					TblDocumentos docbd = ctl_documento.ObtenerDocumento(id_seguridad);

					if (evento_endoso == null && evento_pago == null)
					{
						endoso = false;
						if (evento_informeP == null)
						{
							//consulto que tiempo por vencer tiene la factura respecto a la fecha actual
							TimeSpan porvencer = Fecha.GetFecha().Subtract(docbd.DatFechaVencDocumento);
							//Obtengo el dia que vence la factura para identificar si cumple con 3 dias habiles 
							int dia_vence = Convert.ToInt16(docbd.DatFechaVencDocumento.DayOfWeek.ToString("d"));
							//**Pruebas de la fecha de vencimiento y posibilidad de generar evento
							//DateTime fecha = new DateTime(2022, 09, 29);
							//dia_vence = Convert.ToInt16(fecha.DayOfWeek.ToString("d"));
							//porvencer = Fecha.GetFecha().Date.Subtract(fecha);
							switch (dia_vence)
							{
								//Domingo-Lunes-Martes
								case 0:
								case 1:
								case 2:
									if (porvencer.TotalHours == -72)
										informe_pago = false;
									break;
								//Miercoles-Jueves-Viernes
								case 3:
								case 4:
								case 5:
									if (porvencer.TotalHours == -120)
										informe_pago = false;
									break;
								//Sabado
								case 6:
									if (porvencer.TotalHours == -96)
										informe_pago = false;
									break;
								default:
									break;
							}

							//Si le falta 3 o mas dias por vencer habilito el boton del informe
							//*****Debo agregar validacion del dia habil segun dia de la fecha de vencimiento como en el acuse tacito
							//if (porvencer.Days == -3)
							//{
							//	informe_pago = false;
							//}

							pago = false;
							valor_factura = docbd.IntVlrTotal;
							valor_pagado = docbd.IntValorPagar;
							if (evento_pago != null && (valor_factura - valor_pagado == 0))
							{
								pago = true;
							}
						}
						else
						{
							endoso = false;
							pago = false;
						}
					}
					else if (evento_pago != null)
					{
						valor_factura = docbd.IntVlrTotal;
						valor_pagado = docbd.IntValorPagar;
						if (evento_pago != null && (valor_factura - valor_pagado == 0))
						{
							pago = true;
						}
					}

				}

				if (eventos == null)
				{
					return NotFound();
				}

				var retorno = eventos.Select(d => new
				{
					Tacito = tacito,
					Inscribir_Documento = inscribir_documento,
					Endoso = endoso,
					Informe = informe_pago,
					Pago = pago,
					d.DatFechaEvento,
					d.IntEstadoEvento,
					EstadoEvento = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CodigoResponseV2>(d.IntEstadoEvento)),
					d.IntNumeroEvento,
					StrUrlEvento = d.StrUrlEvento.Replace(string.Format("-{0}.xml", d.IntEstadoEvento), ".xml"),
					respuesta_evento = d.StrUrlEvento,
					ValorF = valor_factura,
					valPago = valor_pagado,
				}).OrderBy(x => x.DatFechaEvento);

				return Ok(retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
