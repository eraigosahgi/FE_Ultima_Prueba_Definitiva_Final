﻿using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;

namespace HGInetMiFacturaElectonicaController.Registros
{
    public class Ctl_NotaDebito : BaseObject<TblDocumentos>
    {
        /// <summary>
        /// Obtiene los documentos de tipo nota débito a nombre del adquiriente que se consulta
        /// </summary>
        /// <param name="identificacion_adquiriente">identificación del adquiriente</param>
        /// <param name="fecha_inicio">fecha inicial de consulta</param>
        /// <param name="fecha_fin">fecha final de consulta</param>
        /// <returns>documentos de tipo nota débito entre fechas por adquiriente</returns>
        public List<NotaDebitoConsulta> ObtenerPorFechasAdquiriente(string identificacion_adquiriente, DateTime FechaInicial, DateTime FechaFinal)
        {
            try
            {
                int tipo_doc = TipoDocumento.NotaDebito.GetHashCode();

                // valida que los parametros sean correctos.
                if (string.IsNullOrWhiteSpace(identificacion_adquiriente) || identificacion_adquiriente.Equals("*"))
                    throw new ApplicationException("Número de identificación del adquiriente inválido.");

                // valida los parámetros de fechas
                if (FechaInicial == null)
                    throw new ApplicationException("Fecha inicial inválida.");
                if (FechaFinal == null)
                    throw new ApplicationException("Fecha final inválida.");

                if (FechaFinal < FechaInicial)
                    throw new ApplicationException("Fecha final debe ser mayor o igual que la fecha inicial.");

                // Valida los estados de visibilidad pública para el adquiriente
                var estado_dian = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "publico"));

                FechaInicial = FechaInicial.Date;
                FechaFinal = FechaFinal.Date.AddDays(1);

                FechaFinal = new DateTime(FechaFinal.Year, FechaFinal.Month, FechaFinal.Day, 23, 59, 59, 999);

                // obtiene los documentos de acuerdo con los filtros de fechas y con los estados de visibilidad pública
                var respuesta = (from datos in context.TblDocumentos
                                 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
                                 where (empresa.StrIdentificacion.Equals(identificacion_adquiriente))
                                 && (datos.IntDocTipo == tipo_doc)
                                 && (datos.DatFechaDocumento >= FechaInicial && datos.DatFechaDocumento < FechaFinal)
                                 && (estado_dian.Contains(datos.IntIdEstado.ToString()))
                                 orderby datos.IntNumero descending
                                 select datos).ToList();

                List<NotaDebitoConsulta> lista_respuesta = new List<NotaDebitoConsulta>();

                // convierte los registros de base de datos a objeto de servicio Nota Débito y los añade a la lista de retorno
                foreach (TblDocumentos item in respuesta)
                {
                    var objeto = (dynamic)null;

                    try
                    {
                        if (item != null)
                        {
                            //Envia el objeto de Bd a convertir a objeto de servicio
                            objeto = Ctl_Documento.ConvertirServicio(item, false, true);

                        }
                    }
                    catch (Exception excepcion)
                    {
                        ProcesoEstado proceso_estado = Enumeracion.ParseToEnum<ProcesoEstado>((int)item.IntIdEstado);
						Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta);
						objeto = new NotaDebitoConsulta()
                        {
                            Aceptacion = item.IntAdquirienteRecibo,
                            CodigoRegistro = item.StrObligadoIdRegistro.ToString(),
                            DatosNotaDebito = null,
                            DescripcionProceso = Enumeracion.GetDescription(proceso_estado),
                            Documento = item.IntNumero,
                            Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
                            FechaUltimoProceso = item.DatFechaActualizaEstado,
                            IdDocumento = item.StrIdSeguridad.ToString(),
                            IdentificacionFacturador = item.StrEmpresaFacturador,
                            IdProceso = item.IntIdEstado,
                            MotivoRechazo = item.StrAdquirienteMvoRechazo,
                            ProcesoFinalizado = (proceso_estado == ProcesoEstado.Finalizacion || proceso_estado == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
                            UrlPdf = item.StrUrlArchivoPdf,
                            UrlXmlUbl = item.StrUrlArchivoUbl,
                            UrlAcuse = null,
                            EstadoDian = null,

                        };
                    }

                    lista_respuesta.Add(objeto);

                }

                return lista_respuesta;
            }
            catch (Exception exec)
            {
                Error error = new Error(CodigoError.VALIDACION, exec);
                throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
            }
        }


		public List<NotaDebitoConsulta> ObtenerHisPorFechasAdquiriente(string identificacion_adquiriente, DateTime FechaInicio, DateTime FechaFinal, int Procesados_ERP = 0)
		{
			List<NotaDebitoConsulta> lista_respuesta = new List<NotaDebitoConsulta>();

			try
			{

				string UrlWs = "https://historico.hgidocs.co";

				UrlWs = string.Format("{0}/Api/Notacredito/ObtenerHisPorFechasAdquiriente", UrlWs);

				// Construir la URL de la API con los parámetros
				//ObtenerHisPorFechasAdquiriente(string Identificacion, DateTime FechaInicio, DateTime FechaFinal, int Procesados_ERP = 0)
				UrlWs += $"?Identificacion={identificacion_adquiriente}&FechaInicio={FechaInicio.ToString("yyyy-MM-dd")}&FechaFinal={FechaFinal.ToString("yyyy-MM-dd")}&Procesados_ERP={Procesados_ERP}";

				// Crear una solicitud HTTP utilizando la URL de la API
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlWs);
				request.Method = "GET";

				// Enviar la solicitud y obtener la respuesta
				try
				{
					using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
					{
						// Verificar el código de estado de la respuesta
						if (response.StatusCode == HttpStatusCode.OK)
						{
							// Leer la respuesta
							using (StreamReader reader = new StreamReader(response.GetResponseStream()))
							{
								string responseData = reader.ReadToEnd();

								// Deserializar la respuesta JSON en un objeto MiObjeto
								lista_respuesta = JsonConvert.DeserializeObject<List<NotaDebitoConsulta>>(responseData);
							}
						}
						else
						{
							//Console.WriteLine("Error al llamar a la API. Código de estado: " + response.StatusCode);
							//throw new Exception("Error al obtener los datos con los parámetros indicados. Código de estado:" + response.StatusCode);
						}
					}
				}
				catch (WebException ex)
				{
					//string ex_message = string.Empty;
					//// Manejar excepciones de WebException
					//if (ex.Response != null)
					//{
					//	using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
					//	{
					//		ex_message = ("Error de la API. Código de estado: " + errorResponse.StatusCode);
					//		using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
					//		{
					//			string errorText = reader.ReadToEnd();
					//			ex_message = string.Format("{0} - {1} - Error_Message: {2}", ex_message, ("Detalle del error: " + errorText), ex.Message);
					//		}
					//	}
					//}
					//else
					//{
					//	ex_message = ("Error: " + ex.Message);
					//}

					//throw new Exception(ex_message, ex);
				}
			}
			catch (Exception exec)
			{
				//Error error = new Error(CodigoError.VALIDACION, exec);
				//throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}

			return lista_respuesta;
		}


		/// <summary>
		/// Obtiene los documentos de tipo Nota Credito a nombre del adquiriente que se consulta
		/// </summary>
		/// <param name="identificacion_adquiriente">identificación del adquiriente</param>
		/// <param name="fecha_inicio">fecha inicial de consulta</param>
		/// <param name="fecha_fin">fecha final de consulta</param>
		/// <returns>documentos de tipo Nota Debito por adquiriente</returns>
		public List<NotaDebitoConsulta> ObtenerPorIdSeguridadAdquiriente(string identificacion_adquiriente, string CodigosRegistros, bool Facturador = false)
        {
            try
            {
                int tipo_doc = TipoDocumento.NotaDebito.GetHashCode();

                // valida que los parametros sean correctos.
                if (string.IsNullOrWhiteSpace(identificacion_adquiriente) || identificacion_adquiriente.Equals("*"))
                    throw new ApplicationException("Número de identificación del adquiriente inválido.");

                if (string.IsNullOrWhiteSpace(CodigosRegistros))
                    throw new ApplicationException("Filtro por números inválido.");

                // Valida los estados de visibilidad pública para el adquiriente
                var estado_dian = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "publico"));

                //Convierte CodigoRegistros en una lista.
                List<string> lista_documentos = Coleccion.ConvertirLista(CodigosRegistros);

				List<TblDocumentos> respuesta = new List<TblDocumentos>();

				if (Facturador == false)
				{

					// obtiene los documentos de acuerdo al id seguridad y con los estados de visibilidad pública
					respuesta = (from datos in context.TblDocumentos
								 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
								 where (empresa.StrIdentificacion.Equals(identificacion_adquiriente))
								 && (lista_documentos.Contains(datos.StrIdSeguridad.ToString()))
								 && (estado_dian.Contains(datos.IntIdEstado.ToString()))
								 orderby datos.IntNumero descending
								 select datos).ToList();
				}
				else
				{
					// obtiene los documentos de acuerdo al id seguridad y con los estados de visibilidad pública
					respuesta = (from datos in context.TblDocumentos
								 join empresa in context.TblEmpresas on datos.StrEmpresaFacturador equals empresa.StrIdentificacion
								 where (empresa.StrIdentificacion.Equals(identificacion_adquiriente))
								 && (lista_documentos.Contains(datos.StrIdSeguridad.ToString()))
								 && (estado_dian.Contains(datos.IntIdEstado.ToString()))
								 orderby datos.IntNumero descending
								 select datos).ToList();
				}

				List<NotaDebitoConsulta> lista_respuesta = new List<NotaDebitoConsulta>();

                // convierte los registros de base de datos a objeto de servicio Factura y los añade a la lista de retorno
                foreach (TblDocumentos item in respuesta)
                {

                    var objeto = (dynamic)null;

                    try
                    {
                        if (item != null)
                        {
							//Envia el objeto de Bd a convertir a objeto de servicio
							objeto = Ctl_Documento.ConvertirServicio(item, false, true);

						}
                    }
                    catch (Exception excepcion)
                    {

                        ProcesoEstado proceso_estado = Enumeracion.ParseToEnum<ProcesoEstado>((int)item.IntIdEstado);
						Ctl_Log.Guardar(excepcion, MensajeCategoria.BaseDatos, MensajeTipo.Error, MensajeAccion.consulta);
						objeto = new NotaDebitoConsulta
                        {
                            Aceptacion = item.IntAdquirienteRecibo,
                            CodigoRegistro = item.StrObligadoIdRegistro.ToString(),
                            DatosNotaDebito = null,
                            DescripcionProceso = Enumeracion.GetDescription(proceso_estado),
                            Documento = item.IntNumero,
                            Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
                            FechaUltimoProceso = item.DatFechaActualizaEstado,
                            IdDocumento = item.StrIdSeguridad.ToString(),
                            IdentificacionFacturador = item.StrEmpresaFacturador,
                            IdProceso = item.IntIdEstado,
                            MotivoRechazo = item.StrAdquirienteMvoRechazo,
                            ProcesoFinalizado = (proceso_estado == ProcesoEstado.Finalizacion || proceso_estado == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
                            UrlPdf = item.StrUrlArchivoPdf,
                            UrlXmlUbl = item.StrUrlArchivoUbl,
                            UrlAcuse = null,
                            EstadoDian = null,

                        };
                    }

                    lista_respuesta.Add(objeto);
                }

                return lista_respuesta;
            }
            catch (Exception exec)
            {
                Error error = new Error(CodigoError.VALIDACION, exec);
                throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
            }
        }

		public List<NotaDebitoConsulta> ObtenerHisPorIdSeguridadAdquiriente(string identificacion_adquiriente, string CodigosRegistros, bool Facturador = false)
		{

			List<NotaDebitoConsulta> lista_respuesta = new List<NotaDebitoConsulta>();

			try
			{
				string UrlWs = "https://historico.hgidocs.co";

				UrlWs = string.Format("{0}/Api/Notacredito/ObtenerHisPorIdSeguridadAdquiriente", UrlWs);

				// Construir la URL de la API con los parámetros
				//ObtenerHisDocumentosIdseguridad(System.Guid id_seguridad)
				UrlWs += $"?identificacion_adquiriente={identificacion_adquiriente}&CodigosRegistros={CodigosRegistros}&Facturador={Facturador}";

				// Crear una solicitud HTTP utilizando la URL de la API
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlWs);
				request.Method = "GET";

				// Enviar la solicitud y obtener la respuesta
				try
				{
					using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
					{
						// Verificar el código de estado de la respuesta
						if (response.StatusCode == HttpStatusCode.OK)
						{
							// Leer la respuesta
							using (StreamReader reader = new StreamReader(response.GetResponseStream()))
							{
								string responseData = reader.ReadToEnd();

								// Deserializar la respuesta JSON en un objeto MiObjeto
								lista_respuesta = JsonConvert.DeserializeObject<List<NotaDebitoConsulta>>(responseData);
							}
						}
						else
						{
							//Console.WriteLine("Error al llamar a la API. Código de estado: " + response.StatusCode);
							//throw new Exception("Error al obtener los datos con los parámetros indicados. Código de estado:" + response.StatusCode);
						}
					}
				}
				catch (WebException ex)
				{
					//string ex_message = string.Empty;
					//// Manejar excepciones de WebException
					//if (ex.Response != null)
					//{
					//	using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
					//	{
					//		ex_message = ("Error de la API. Código de estado: " + errorResponse.StatusCode);
					//		using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
					//		{
					//			string errorText = reader.ReadToEnd();
					//			ex_message = string.Format("{0} - {1} - Error_Message: {2}", ex_message, ("Detalle del error: " + errorText), ex.Message);
					//		}
					//	}
					//}
					//else
					//{
					//	ex_message = ("Error: " + ex.Message);
					//}

					//throw new Exception(ex_message, ex);
				}

				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}
	}
}
