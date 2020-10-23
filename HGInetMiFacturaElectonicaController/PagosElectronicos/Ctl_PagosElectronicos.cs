using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System.Globalization;
using HGInetMiFacturaElectonicaData.Enumerables;
using LibreriaGlobalHGInet.ObjetosComunes.PagosEnLinea;
using HGInetMiFacturaElectonicaController.Auditorias;

namespace HGInetMiFacturaElectonicaController.PagosElectronicos
{
	public class Ctl_PagosElectronicos : BaseObject<TblPagosElectronicos>
	{

		#region Crear 

		/// <summary>
		/// almacena los datos del pago electrónico en base de datos.
		/// </summary>
		/// <param name="datos_pago"></param>
		/// <returns></returns>
		public TblPagosElectronicos Crear(TblPagosElectronicos datos_pago)
		{
			try
			{
				datos_pago = this.Add(datos_pago);

				return datos_pago;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Almacena los pagos en la base de datos, construyendo el registro con los datos de la pasarela
		/// </summary>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
		/// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
		/// <param name="id_seguridad">ID de seguridad del documento o el plan a cancelar.</param>
		/// <param name="tipo_pago">indica si el pago es para un documento o una compra de planes (0: Documento - 1: Plan).</param>
		/// <param name="valor"></param>
		/// <returns></returns>
		public TblPagosElectronicos CrearPago(Guid StrIdRegistro, System.Guid id_seguridad, int tipo_pago, decimal valor)
		{
			try
			{
				TblPagosElectronicos datos_registro = new TblPagosElectronicos();
				/*
                if (string.IsNullOrWhiteSpace(id_plataforma))
                    throw new ApplicationException("El ID de seguridad de la plataforma no puede estar vacío.");
                if (id_seguridad == null)
                    throw new ApplicationException("El ID de seguridad no puede estar vacío.");
                    */
				if (valor <= 0)
					throw new ApplicationException("El valor del pago no puede ");

				datos_registro.StrIdRegistro = StrIdRegistro;

				//Valida que tipo de pago se realiza (0: Documento - 1: Planes)
				if (tipo_pago == 0)
					datos_registro.StrIdSeguridadDoc = id_seguridad;
				else if (tipo_pago == 1)
					datos_registro.StrIdSeguridadPlanes = id_seguridad;

				//Fecha de registro.
				datos_registro.DatFechaRegistro = Fecha.GetFecha();
				//valor del documento.
				datos_registro.IntValorPago = valor;

				datos_registro.IntEstadoPago = EstadoPago.Pendiente.GetHashCode();

				Crear(datos_registro);

				return datos_registro;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		#endregion

		#region Actualizar

		/// <summary>
		/// Actualiza los datos del pago electrónico en base de datos
		/// </summary>
		/// <param name="datos_pago"></param>
		/// <returns></returns>
		public TblPagosElectronicos Actualizar(TblPagosElectronicos datos_pago)
		{
			try
			{
				datos_pago = this.Edit(datos_pago);

				return datos_pago;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Actualiza los datos del pago según la respuesta
		/// </summary>        
		/// <param name="datos_pago">Objeto de pago de la plataforma intermedia</param>
		/// <returns></returns>
		public TblPagosElectronicos ActualizarPago(TblPasarelaPagosPI datos_pago)
		{
			try
			{
				TblPagosElectronicos datos_registro = new TblPagosElectronicos();

				datos_registro = Obtener(datos_pago.StrIdSeguridadDoc, datos_pago.StrIdSeguridadRegistro);

				if (datos_registro == null)
					throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el pago", datos_pago.StrIdSeguridad));

				if (datos_pago != null)
				{
					if (datos_pago.IntPagoEstado != EstadoPago.Pendiente.GetHashCode())
					{
						datos_registro.StrCodigoBanco = datos_pago.StrPagoCodBanco;
						datos_registro.StrCodigoFranquicia = datos_pago.StrPagoCodFranquicia;
						datos_registro.IntClicloTransaccion = datos_pago.IntPagoClicloTransaccion;
						//datos_registro.IntCodigoServicio = Convert.ToInt32(datos_pago.StrCodigoServicio);
						datos_registro.IntEstadoPago = datos_pago.IntPagoEstado;
						datos_registro.IntFormaPago = datos_pago.IntPagoFormaPago;
						datos_registro.StrTicketID = datos_pago.StrPagoTicketID;
						datos_registro.StrTransaccionCUS = datos_pago.StrPagoTransaccionCUS;
						datos_registro.StrIdSeguridadPago = datos_pago.StrIdSeguridadPago;
						datos_registro.StrIdPlataforma = datos_pago.StrIdPlataforma;
					}
					else
					{
						datos_registro.IntEstadoPago = datos_pago.IntPagoEstado;
						datos_registro.StrIdSeguridadPago = datos_pago.StrIdSeguridadPago;
						datos_registro.StrIdPlataforma = datos_pago.StrIdPlataforma;
					}

					if (datos_pago.DatFechaVerificacion != null)
						datos_registro.DatFechaVerificacion = Fecha.GetFecha();

					datos_registro.StrMensaje = datos_pago.StrMensajeVerificacion;
					datos_registro = Actualizar(datos_registro);

					try
					{
						Ctl_DocumentosAudit _auditoria = new Ctl_DocumentosAudit();
						_auditoria.Crear(datos_registro.TblDocumentos.StrIdSeguridad, Guid.Empty, datos_registro.TblDocumentos.StrEmpresaFacturador, ProcesoEstado.PagoDocumento, TipoRegistro.Actualizacion, Procedencia.Usuario, string.Empty,string.Format("Actualizando estado del pago"), datos_pago.StrMensajeVerificacion, datos_registro.TblDocumentos.StrPrefijo, datos_registro.TblDocumentos.IntNumero.ToString());
					}
					catch (Exception) { }
				}
				return datos_registro;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		#endregion

		#region Obtener

		/// <summary>
		/// Obtiene el pago de la base de datos por códigos principales (StrIdSeguridadPago y StrIdPlataforma).
		/// </summary>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
		/// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
		/// <returns></returns>
		public TblPagosElectronicos Obtener(Guid id_seguridad_doc, Guid id_Seguridad_Registro)
		{
			try
			{

				TblPagosElectronicos datos_pago = (from pago in context.TblPagosElectronicos
												   where pago.StrIdSeguridadDoc == id_seguridad_doc
												   && pago.StrIdRegistro == id_Seguridad_Registro
												   select pago).FirstOrDefault();

				return datos_pago;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene el pago de la base de datos por códigos principales (StrIdSeguridadPago y StrIdPlataforma).
		/// </summary>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
		/// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
		/// <returns></returns>
		public List<TblDocumentos> Obtener(System.Guid StrIdSeguridadDoc)
		{
			try
			{

				List<TblDocumentos> datos_pago = (from documentos in context.TblDocumentos
												  where documentos.StrIdSeguridad == StrIdSeguridadDoc
												  select documentos).ToList();

				return datos_pago;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene el Saldo Pendiente de un Documento
		/// </summary>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>        
		/// <returns></returns>
		public String ConsultaSaldoDocumento(System.Guid StrIdSeguridadDoc)
		{
			try
			{
				TblDocumentos Valor_Documento = (from Doc in context.TblDocumentos
												 where Doc.StrIdSeguridad == StrIdSeguridadDoc
												 select Doc).FirstOrDefault();



				int Pendiente1 = EstadoPago.Pendiente.GetHashCode();//Pendiente por iniciar
				int Pendiente2 = EstadoPago.Pendiente2.GetHashCode();//Pendiente por Finalizar
				int Aprobado = EstadoPago.Aprobado.GetHashCode();



				if (Valor_Documento.IntIdEstado == ProcesoEstado.FinalizacionErrorDian.GetHashCode())
				{
					return "ErrorDian";
				}


				int Pago_pendiente = (from pagos in context.TblPagosElectronicos
									  where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
									  && (pagos.IntEstadoPago == Pendiente1 || pagos.IntEstadoPago == Pendiente2)
									  select pagos.IntValorPago).Count();
				if (Pago_pendiente > 0)
				{
					return "PagoPendiente";
				}

				var Pagos = (from pagos in context.TblPagosElectronicos
							 where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
							 && pagos.IntEstadoPago == Aprobado
							 select pagos.IntValorPago).FirstOrDefault();

				decimal Datos_pago = 0;
				if (Pagos > 0)
				{
				Datos_pago = (from pagos in context.TblPagosElectronicos
							  where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
							   && pagos.IntEstadoPago == Aprobado
							  select pagos.IntValorPago).Sum();
				}


				if (Datos_pago >= Valor_Documento.IntVlrTotal)
				{
					return "DocumentoCancelado";
				}

				return (Valor_Documento.IntVlrTotal - Datos_pago).ToString();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Retorna uno de los siguientes valores: 
		/// 1 si el documento esta totalmente pagado
		/// 2 si tiene un pago pendiente por verificar 
		/// 3 No pagado totalmente el documento(Es decir que no tiene ningun pago sin verificar, pero si tiene aun saldo pendiente en la factura)
		/// </summary>
		/// <param name="id_seguridad_pago">ID de seguridad del documento.</param>        
		/// <returns></returns>
		public int VerificarSaldo(System.Guid StrIdSeguridadDoc)
		{
			try
			{
				//Obtengo el documento
				TblDocumentos Valor_Documento = (from Doc in context.TblDocumentos
												 where Doc.StrIdSeguridad == StrIdSeguridadDoc
												 select Doc).FirstOrDefault();

				int Pendiente1 = EstadoPago.Pendiente.GetHashCode();//Pendiente por iniciar
				int Pendiente2 = EstadoPago.Pendiente2.GetHashCode();//Pendiente por Finalizar
				int Aprobado = EstadoPago.Aprobado.GetHashCode();

				//Luego valido si tiene algún pago pendiente (estatus 888,999)
				int Pago_pendiente = (from pagos in context.TblPagosElectronicos
									  where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
									  && (pagos.IntEstadoPago == Pendiente1 || pagos.IntEstadoPago == Pendiente2)
									  select pagos.IntValorPago).Count();

				if (Pago_pendiente > 0)
				{
					return 2;
				}

				//Luego Valido si tiene pagos
				var Pagos = (from pagos in context.TblPagosElectronicos
							 where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
							 && pagos.IntEstadoPago == Aprobado
							 select pagos.IntValorPago).FirstOrDefault();

				decimal Datos_pago = 0;
				if (Pagos > 0)
				{
					Datos_pago = (from pagos in context.TblPagosElectronicos
								  where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
								   && pagos.IntEstadoPago == Aprobado
								  select pagos.IntValorPago).Sum();
				}

				//Pago totalmente cancelado
				if (Datos_pago >= Valor_Documento.IntVlrTotal)
				{
					return 1;
				}
				//no ha pagado totalmente el documento
				return 3;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene la lista de pagos del Facturador
		/// </summary>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
		/// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
		/// <returns></returns>
		public List<TblPagosElectronicos> ObtenerPagosFacturador(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, string Resolucion, int tipo_fecha)
		{

			fecha_inicio = fecha_inicio.Date;

			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			long num_doc = -1;
			long.TryParse(numero_documento, out num_doc);


			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);

			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";
			if (string.IsNullOrWhiteSpace(codigo_adquiriente))
				codigo_adquiriente = "*";

			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";

			List<string> LstResolucion = new List<string>();

			if (string.IsNullOrWhiteSpace(Resolucion))
			{
				Resolucion = "*";
			}
			else
			{
				LstResolucion = Coleccion.ConvertirLista(Resolucion);
			}



			var documentos = (from Pagos in context.TblPagosElectronicos
							  where Pagos.TblDocumentos.StrEmpresaFacturador.Equals(codigo_facturador)
							  //Valida si la fecha es documento o fecha pago
							  && ((Pagos.TblDocumentos.DatFechaDocumento >= fecha_inicio && Pagos.TblDocumentos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 2)
							  //&& ((Pagos.DatFechaRegistro >= fecha_inicio && Pagos.DatFechaRegistro <= fecha_fin) || tipo_fecha == 1)
							  && ((Pagos.DatFechaRegistro >= fecha_inicio && Pagos.DatFechaRegistro <= fecha_fin) || tipo_fecha == 1)
							  && (LstResolucion.Contains(Pagos.TblDocumentos.StrNumResolucion.ToString()) || Resolucion.Equals("*"))
							  && (Pagos.IntEstadoPago.Equals(cod_estado_recibo) || estado_recibo.Equals("*"))
							  && (Pagos.TblDocumentos.IntNumero == num_doc || numero_documento.Equals("*"))
							  && (Pagos.TblDocumentos.StrEmpresaAdquiriente.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
							  orderby Pagos.DatFechaRegistro descending
							  select Pagos).ToList();


			return documentos;

		}


		/// <summary>
		/// Obtiene la lista de pagos del Adquiriente
		/// </summary>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
		/// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
		/// <returns></returns>
		public List<TblPagosElectronicos> ObtenerPagosAdquiriente(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, int tipo_fecha)
		{

			fecha_inicio = fecha_inicio.Date;

			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			int num_doc = -1;
			int.TryParse(numero_documento, out num_doc);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);

			if (string.IsNullOrWhiteSpace(numero_documento))
				numero_documento = "*";
			if (string.IsNullOrWhiteSpace(codigo_adquiriente))
				codigo_adquiriente = "*";

			if (string.IsNullOrWhiteSpace(estado_recibo))
				estado_recibo = "*";


			var documentos = (from Pagos in context.TblPagosElectronicos
							  where Pagos.TblDocumentos.StrEmpresaAdquiriente.Equals(codigo_adquiriente)
							  && ((Pagos.TblDocumentos.DatFechaDocumento >= fecha_inicio && Pagos.TblDocumentos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 2)
							  && ((Pagos.DatFechaRegistro >= fecha_inicio && Pagos.DatFechaRegistro <= fecha_fin) || tipo_fecha == 1)
							  && (Pagos.TblDocumentos.StrEmpresaAdquiriente.Equals(codigo_facturador))
							  && (Pagos.IntEstadoPago == cod_estado_recibo || estado_recibo.Equals("*"))
							  && (Pagos.TblDocumentos.IntNumero == num_doc || numero_documento.Equals("*"))
							  orderby Pagos.DatFechaRegistro descending
							  select Pagos).ToList();

			return documentos;

		}

		/// <summary>
		/// obtiene los pagos por estado de verificación (consulta por DatFechaVerificacion, si esta null no se encuentra verificado). ¿FECHAS?
		/// </summary>
		/// <param name="fecha_inicio">fecha inicial del rango para el filtro fechas (aplica sobre la fecha de registro del pago).</param>
		/// <param name="fecha_fin">fecha final del rango para el filtro por fechas (aplica sobre la fecha de registro del pago).</param>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
		/// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
		/// <param name="estado_verificacion">(0:por verificar - 1: verificado)</param>
		/// <returns></returns>
		public List<TblPagosElectronicos> ObtenerPorVerificacion(DateTime fecha_inicio, DateTime fecha_fin, string id_seguridad_pago, string id_plataforma, int estado_verificacion)
		{
			try
			{
				List<TblPagosElectronicos> pagos = (from pago in context.TblPagosElectronicos

													where (pago.DatFechaRegistro.Date >= fecha_inicio && pago.DatFechaRegistro.Date <= fecha_fin)
													&& pago.StrIdSeguridadPago.Equals(id_seguridad_pago) || id_seguridad_pago.Equals("*")
													&& pago.StrIdPlataforma.Equals(id_plataforma) || id_plataforma.Equals("*")
													&& (estado_verificacion == 0) ? pago.DatFechaVerificacion.Value == null : pago.DatFechaVerificacion.Value != null
													select pago).ToList();

				return pagos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Plataforma intermedia

		/// <summary>
		/// Realiza el registro del pago en la plataforma electrónica.
		/// Consulta el documento por id de seguridad para la construcción del objeto de envío de los datos del pago.
		/// Retorna la url con el id virtual para la redirección a la página de inicio del proceso de pago.
		/// Si el pago es de un documento los datos de la pasarela son tomados desde el comercio asociado en la resolución del mismo, de lo contrario lso datos serán obtenidos del web.config
		/// </summary>
		/// <param name="id_seguridad">Id de seguridad del documento o plan transaccional</param>
		/// <param name="tipo_pago">indica si el pago es para un documento o una compra de planes (0: Documento - 1: Plan).</param>
		/// <param name="registrar_pago">indica si se registra el pago en base de datos.</param>
		/// <param name="valor_pago">valor a pagar.</param>
		/// <returns></returns>
		public dynamic ReportePagoElectronicoPI(System.Guid id_seguridad, int tipo_pago = 0, bool registrar_pago = true, double valor_pago = 0, string usuario = "")
		{
			try
			{
				//Ruta retornada por el servicio, redirecciona a la  página de inicio del pago (Selección de tipo persona, forma pago y banco).
				string ruta_inicio = string.Empty;
				//Datos de la pasarela electrónica.
				int comercio_id = 0;
				string comercio_clave = string.Empty;
				string comercio_ruta = string.Empty;
				string codigo_servicio = string.Empty;
				string identificacion_empresa = string.Empty;
				string prefijo = string.Empty;
				string documento = string.Empty;
				string FacturadorAut = string.Empty;

				//Objetos para reportar el pago
				Cliente datos_cliente = new Cliente();
				Pago datos_pago = new Pago();


				TblPasarelaPagosPI ObjPago = new TblPasarelaPagosPI();

				ObjPago.StrIdSeguridadDoc = id_seguridad;

				//ObjPago.StrIdSeguridadRegistro = Guid.NewGuid();
				ObjPago.StrIdSeguridadRegistro = Guid.NewGuid();

				ObjPago.DatFechaRegistro = Fecha.GetFecha();

				ObjPago.DatFechaSync = Fecha.GetFecha();
				ObjPago.DatFechaVerificacion = Fecha.GetFecha();

				ObjPago.StrAuthIdEmpresa = "";
				ObjPago.IntSincronizacion = true;

				ObjPago.StrAuthToken = "";

				PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
				ObjPago.StrRutaSync = plataforma.RutaPublica.ToString() + "/Api/SrcActualizaEstadoP";

				ObjPago.StrRutaDestino = "";

				ObjPago.StrRutaProcedencia = "";

				ObjPago.StrPagoIdPlataforma = "";

				decimal Monto_Pendiente = 0;

				//Valida si el pago es de un documento o una compra de planes.
				if (tipo_pago == 0)
				{
					Ctl_Documento clase_documento = new Ctl_Documento();
					TblDocumentos datos_documento = clase_documento.ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

					//Valido si este documento tiene algún pago
					var Pagos = (from pagos in context.TblPagosElectronicos
								 where pagos.StrIdSeguridadDoc == datos_documento.StrIdSeguridad
								 && pagos.IntEstadoPago == 1
								 select pagos.IntValorPago).FirstOrDefault();

					//Si tiene algún pago, entonces valido cuanto es el total de todos los pagos
					if (Pagos > 0)
					{
						//Le asigno la suma de los pagos a la siguiente variable
						Monto_Pendiente = (from pagos in context.TblPagosElectronicos
										   where pagos.StrIdSeguridadDoc == datos_documento.StrIdSeguridad
											&& pagos.IntEstadoPago == 1
										   select pagos.IntValorPago).Sum();

						//Luego Resto, el total del documento menos la suma de pagos
						Monto_Pendiente = datos_documento.IntVlrTotal - Monto_Pendiente;
					}

					//Id del erp que envia el pago
					ObjPago.StrIdDocumento = datos_documento.StrObligadoIdRegistro;

					identificacion_empresa = datos_documento.StrEmpresaAdquiriente;

					//valisa que el documento no sea null.
					if (datos_documento != null)
					{
						ObjPago.DatFechaDocumento = datos_documento.DatFechaDocumento;
						ObjPago.StrPrefijoDocumento = datos_documento.StrPrefijo;
						ObjPago.IntNumeroDocumento = datos_documento.IntNumero;
						ObjPago.DatFechaVencDocumento = datos_documento.DatFechaVencDocumento;
						ObjPago.IntVlrTotalDocumento = datos_documento.IntVlrTotal;

						//consulta la resolución para obtener el comercio que tiene asociado.
						Ctl_EmpresaResolucion clase_resoluciones = new Ctl_EmpresaResolucion();
						TblEmpresasResoluciones datos_resolucion = clase_resoluciones.Obtener(datos_documento.StrEmpresaFacturador, datos_documento.StrNumResolucion, datos_documento.StrPrefijo);
						
						//Valida que la resolución no sea null.
						if (datos_resolucion != null)
						{

							ObjPago.StrIdSeguridadComercio = datos_resolucion.StrComercioConfigId;
							ObjPago.StrAuthToken = datos_resolucion.StrEmpresa;

							/*if (datos_resolucion.IntComercioId.Value <= 0)
								throw new ApplicationException(string.Format("La Resuloción N.{0} para el Facturador {1}, no tiene configurado un comercio.", datos_documento.StrNumResolucion, datos_documento.StrEmpresaFacturador));

							//Obtiene los datos de la pasarela.
							Ctl_EmpresasPasarela clase_pasarela = new Ctl_EmpresasPasarela();
							TblEmpresasPasarela datos_pasarela = clase_pasarela.Obtener(datos_documento.StrEmpresaFacturador, datos_resolucion.IntComercioId.Value);

							if (datos_pasarela != null)
							{
								ObjPago.IntComercioId = datos_pasarela.IntComercioId;
								ObjPago.StrComercioClave = datos_pasarela.StrComercioClave;
								ObjPago.StrComercioIdRuta = datos_pasarela.StrComercioIdRuta;
								ObjPago.StrCodigoServicio = datos_pasarela.StrCodigoServicio;
								ObjPago.IntPasarela = datos_pasarela.IntPasarela;
							}*/
							//Asigna valores a prefijo y documento para la auditoria
							try
							{
								var Doc = datos_resolucion.TblDocumentos.Where(x => x.StrIdSeguridad == id_seguridad).FirstOrDefault();
								prefijo = Doc.StrPrefijo;
								documento = Doc.IntNumero.ToString();
								FacturadorAut = Doc.StrEmpresaFacturador;
							}
							catch (Exception) { }
						}
						else
						{
							throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el número de Resolución", datos_documento.StrNumResolucion));
						}

						datos_pago.descripcion_pago = string.Format("{0}", datos_pago.id_pago);
						//Si la variable de pago viene sin valor
						if (valor_pago <= 0)
							if (Monto_Pendiente > 0) //Pregunto si el pago pendiente este presente
							{
								valor_pago = Convert.ToDouble(Monto_Pendiente);//Si esta presente le asigno la variable pendiente al pago
							}
							else
							{
								valor_pago = Convert.ToInt32(datos_documento.IntVlrTotal);//Si no, entonces busco el monto total del pago
							}


						else
						{
							if (valor_pago > Convert.ToDouble(datos_documento.IntVlrTotal))
								throw new ApplicationException("El valor a pagar no puede ser superior al valor total del documento.");
						}
					}
					else
					{
						//PasarelaPagos pasarela = HgiConfiguracion.GetConfiguration().PasarelaPagos;

						////Datos de la pasarela electrónica.
						//ObjPago.IntComercioId = Convert.ToInt32(pasarela.IdComercio);
						//ObjPago.StrComercioClave = pasarela.ClaveComercio;
						//ObjPago.StrComercioIdRuta = pasarela.RutaComercio;
						//ObjPago.StrCodigoServicio = pasarela.CodigoServicio;
						//ObjPago.IntPasarela = 1;//Pasarela por defecto es 1 Zona de pagos
						
						//Obtiene el plan de transacciones.
						Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();
						TblPlanesTransacciones datos_plan = clase_planes.ObtenerIdSeguridad(id_seguridad);

						identificacion_empresa = datos_plan.StrEmpresaFacturador;

						if (datos_plan == null)
							throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el Plan", id_seguridad));

						datos_pago.descripcion_pago = string.Format("{0}", datos_plan.StrObservaciones);

						if (valor_pago <= 0)
							valor_pago = Convert.ToDouble(datos_plan.IntValor);
						else
						{
							if (valor_pago > Convert.ToDouble(datos_plan.IntValor))
								throw new ApplicationException("El valor a pagar no puede ser superior al valor total de la compra.");
						}
					}

					ObjPago.IntValor = decimal.Parse(valor_pago.ToString());
					ObjPago.IntValorIva = 0;

					//Obtiene los datos del cliente.
					Ctl_Empresa clase_empresa = new Ctl_Empresa();
					TblEmpresas datos_empresa = clase_empresa.Obtener(identificacion_empresa);

					if (datos_empresa == null)
						throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "la empresa", datos_empresa.StrIdentificacion));


					ObjPago.StrClienteTipoId = datos_empresa.StrTipoIdentificacion.ToString();

					ObjPago.StrClienteIdentificacion = datos_empresa.StrIdentificacion;

					ObjPago.StrClienteNombre = datos_empresa.StrRazonSocial;

					ObjPago.StrClienteTelefono = datos_empresa.StrTelefono;

					ObjPago.StrClienteEmail = datos_empresa.StrMailAdmin;

					//Encriptar datos de seguridad secundaria                    
					ObjPago.StrAuthIdEmpresa = Encriptar.Encriptar_SHA256(ObjPago.StrIdSeguridadRegistro.ToString() + "-" + ObjPago.StrClienteIdentificacion + "-" + ObjPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ObjPago.StrIdSeguridadComercio.ToString() + "-" + ObjPago.IntValor.ToString("0.##"));

					//Registro el Pago Local                    
					TblPagosElectronicos pago = CrearPago(ObjPago.StrIdSeguridadRegistro, id_seguridad, tipo_pago, Convert.ToDecimal(valor_pago));

					//convierto el Objeto de pago en Json
					var ObjetoPago = JsonConvert.SerializeObject(ObjPago);


					try
					{
						Ctl_DocumentosAudit _auditoria = new Ctl_DocumentosAudit();
						_auditoria.Crear(id_seguridad, Guid.Empty, FacturadorAut, ProcesoEstado.PagoDocumento, TipoRegistro.Creacion, Procedencia.Usuario, usuario, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(ProcesoEstado.PagoDocumento.GetHashCode())), ObjPago.StrIdSeguridadRegistro.ToString(), prefijo, documento);
					}
					catch (Exception) { }

					//Retorno el Objeto Json Cifrado 
					return new { Ruta = EncriptarObjeto(ObjetoPago.ToString()), IdRegistro = ObjPago.StrIdSeguridadRegistro };


				}
				else
					throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el documento", id_seguridad));

				return ruta_inicio;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Cifra el Objeto de pago Json para enviarlo a la pagina intermedia de pago
		/// </summary>
		/// <param name="_cadenaAencriptar"></param>
		/// <returns></returns>
		private static string EncriptarObjeto(string _cadenaAencriptar)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_cadenaAencriptar);
			return System.Convert.ToBase64String(plainTextBytes);
		}




		#endregion
	}
}
