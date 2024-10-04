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
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaData.ModeloServicio.Respuestas;
using LibreriaGlobalHGInet.Error;
using System.ServiceModel;
using LibreriaGlobalHGInet.RegistroLog;

namespace HGInetMiFacturaElectonicaController.PagosElectronicos
{
	public class Ctl_PagosElectronicos : BaseObject<TblPagosElectronicos>
	{

		#region HGIpay
		/// <summary>
		/// Obtiene la lista de pagos del Adquiriente
		/// </summary>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
		/// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
		/// <returns></returns>
		//public List<TblPagosElectronicos> HGIpayObtenerPagosAdquiriente(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, int tipo_fecha)
		//{

		//	fecha_inicio = fecha_inicio.Date;

		//	fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

		//	int num_doc = -1;
		//	int.TryParse(numero_documento, out num_doc);

		//	short cod_estado_recibo = -1;
		//	short.TryParse(estado_recibo, out cod_estado_recibo);

		//	if (string.IsNullOrWhiteSpace(numero_documento))
		//		numero_documento = "*";
		//	if (string.IsNullOrWhiteSpace(codigo_adquiriente))
		//		codigo_adquiriente = "*";

		//	if (string.IsNullOrWhiteSpace(estado_recibo))
		//		estado_recibo = "*";

		//	if (string.IsNullOrEmpty(codigo_facturador) || codigo_facturador == "null")
		//	{
		//		codigo_facturador = "*";
		//	}


		//	var documentos = (from Pagos in context.TblPagosElectronicos
		//					  where Pagos.TblDocumentos.StrEmpresaAdquiriente.Equals(codigo_adquiriente)
		//					  && ((Pagos.TblDocumentos.DatFechaDocumento >= fecha_inicio && Pagos.TblDocumentos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 2)
		//					  && ((Pagos.DatFechaRegistro >= fecha_inicio && Pagos.DatFechaRegistro <= fecha_fin) || tipo_fecha == 1)
		//					  && (Pagos.TblDocumentos.StrEmpresaFacturador.Equals(codigo_facturador))
		//					  && (Pagos.IntEstadoPago == cod_estado_recibo || estado_recibo.Equals("*"))
		//					  && (Pagos.TblDocumentos.IntNumero == num_doc || numero_documento.Equals("*"))
		//					  orderby Pagos.DatFechaRegistro descending
		//					  select Pagos).ToList();

		//	return documentos;

		//}
		public List<TblPagosElectronicos> HGIpayObtenerPagosAdquiriente(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, int tipo_fecha)
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

			if (string.IsNullOrEmpty(codigo_facturador) || codigo_facturador == "null")
			{
				codigo_facturador = "*";
			}

			//var documentos = (from Pagos in context.TblPagosDetalles
			//				  where Pagos.TblDocumentos.StrEmpresaAdquiriente.Equals(codigo_adquiriente)
			//				  && ((Pagos.TblDocumentos.DatFechaDocumento >= fecha_inicio && Pagos.TblDocumentos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 2)
			//				  && ((Pagos.TblPagosElectronicos.DatFechaRegistro >= fecha_inicio && Pagos.TblPagosElectronicos.DatFechaRegistro <= fecha_fin) || tipo_fecha == 1)
			//				  && (Pagos.TblDocumentos.StrEmpresaFacturador.Equals(codigo_facturador))
			//				  && (Pagos.TblPagosElectronicos.IntEstadoPago == cod_estado_recibo || estado_recibo.Equals("*"))
			//				  && (Pagos.TblDocumentos.IntNumero == num_doc || numero_documento.Equals("*"))
			//				  orderby Pagos.TblPagosElectronicos.DatFechaRegistro descending
			//				  select Pagos).ToList();

			var documentos = (from Pagos in context.TblPagosElectronicos
							  where (Pagos.DatFechaRegistro >= fecha_inicio && Pagos.DatFechaRegistro <= fecha_fin)
							  && (Pagos.IntEstadoPago == cod_estado_recibo || estado_recibo.Equals("*"))
							  orderby Pagos.DatFechaRegistro descending
							  select Pagos).ToList();

			return documentos;

		}

		#endregion


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
		public TblPagosElectronicos CrearPago(Guid StrIdRegistro, System.Guid id_seguridad, int tipo_pago, decimal valor, int forma_pago, string empresa, string adquiriente)
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
					//datos_registro.StrIdSeguridadDoc = id_seguridad;
					datos_registro.StrIdRegistro2 = id_seguridad;
				else if (tipo_pago == 1)
					datos_registro.StrIdSeguridadPlanes = id_seguridad;

				//Fecha de registro.
				datos_registro.DatFechaRegistro = Fecha.GetFecha();
				//valor del documento.
				datos_registro.IntValorPago = valor;

				datos_registro.IntEstadoPago = EstadoPago.Pendiente.GetHashCode();

				datos_registro.IntFormaPago = forma_pago;

				datos_registro.StrEmpresaFacturador = empresa;

				datos_registro.StrEmpresaAdquiriente = adquiriente;

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


					try
					{
						datos_registro.StrCampo1 = datos_pago.StrCampo1;
					}
					catch (Exception)
					{
					}

					if (datos_pago.DatFechaVerificacion != null)
						datos_registro.DatFechaVerificacion = Fecha.GetFecha();

					datos_registro.StrMensaje = datos_pago.StrMensajeVerificacion;
					datos_registro = Actualizar(datos_registro);

					try
					{
						Ctl_DocumentosAudit _auditoria = new Ctl_DocumentosAudit();
						_auditoria.Crear(datos_registro.StrIdRegistro, Guid.Empty, datos_registro.TblPagosDetalles.FirstOrDefault().TblDocumentos.StrEmpresaFacturador, ProcesoEstado.PagoDocumento, TipoRegistro.Actualizacion, Procedencia.Usuario, string.Empty, string.Format("Actualizando estado del pago"), datos_pago.StrMensajeVerificacion, datos_registro.TblPagosDetalles.FirstOrDefault().TblDocumentos.StrPrefijo, datos_registro.TblPagosDetalles.FirstOrDefault().TblDocumentos.IntNumero.ToString());
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

		public string ObtenerPorRegistroPrincipal(Guid registro_principal)
		{
			try
			{

				TblPagosElectronicos datos_pago = (from pago in context.TblPagosElectronicos
												   where pago.StrIdRegistro == registro_principal
												   select pago).FirstOrDefault();

				if (datos_pago != null)
				{
					return datos_pago.StrIdRegistro2.ToString();
				}
				return "";
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public TblPagosElectronicos ObtenerPagoPorRegistroPrincipal(Guid registro_principal , bool lazyloading = true)
		{
			try
			{
				context.Configuration.LazyLoadingEnabled = lazyloading;

				TblPagosElectronicos datos_pago = (from pago in context.TblPagosElectronicos
												   where pago.StrIdRegistro == registro_principal
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
		public TblPagosElectronicos Obtener(Guid id_seguridad_doc, Guid id_Seguridad_Registro)
		{
			try
			{

				TblPagosElectronicos datos_pago = (from pago in context.TblPagosElectronicos
												   where pago.StrIdRegistro2 == id_seguridad_doc
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
		/// Obtiene todos los pagos de un documento
		/// </summary>
		/// <param name="id_seguridad_doc">Id de seguridad del documento</param>		
		/// <returns></returns>
		public List<TblPagosElectronicos> ObtenerPagos(Guid id_seguridad_doc)
		{
			try
			{
				List<TblPagosElectronicos> lista_pagos = (from pago in context.TblPagosElectronicos
														  where pago.StrIdRegistro2 == id_seguridad_doc
														  select pago).ToList();

				return lista_pagos;
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
				//context.Configuration.LazyLoadingEnabled = false;

				//List<TblDocumentos> datos_pago = (from documentos in context.TblDocumentos.Include("TblEmpresasFacturador").Include("TblPagosElectronicos")
				List<TblDocumentos> datos_pago = (from documentos in context.TblDocumentos//.Include("TblEmpresasFacturador").Include("TblPagosDetalles")
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
									  where pagos.StrIdRegistro2 == StrIdSeguridadDoc
									  && (pagos.IntEstadoPago == Pendiente1 || pagos.IntEstadoPago == Pendiente2)
									  select pagos.IntValorPago).Count();
				if (Pago_pendiente > 0)
				{
					return "PagoPendiente";
				}

				var Pagos = (from pagos in context.TblPagosElectronicos
							 where pagos.StrIdRegistro2 == StrIdSeguridadDoc
							 && pagos.IntEstadoPago == Aprobado
							 select pagos.IntValorPago).FirstOrDefault();

				decimal Datos_pago = 0;
				if (Pagos > 0)
				{
					Datos_pago = (from pagos in context.TblPagosElectronicos
								  where pagos.StrIdRegistro2 == StrIdSeguridadDoc
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
									  where pagos.StrIdRegistro2 == StrIdSeguridadDoc
									  && (pagos.IntEstadoPago == Pendiente1 || pagos.IntEstadoPago == Pendiente2)
									  select pagos.IntValorPago).Count();

				if (Pago_pendiente > 0)
				{
					return 2;
				}

				//Luego Valido si tiene pagos
				var Pagos = (from pagos in context.TblPagosElectronicos
							 where pagos.StrIdRegistro2 == StrIdSeguridadDoc
							 && pagos.IntEstadoPago == Aprobado
							 select pagos.IntValorPago).FirstOrDefault();

				decimal Datos_pago = 0;
				if (Pagos > 0)
				{
					Datos_pago = (from pagos in context.TblPagosElectronicos
								  where pagos.StrIdRegistro2 == StrIdSeguridadDoc
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
		public List<TblPagosElectronicos> ObtenerPagos(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, string Resolucion, int tipo_fecha)
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

			if (string.IsNullOrEmpty(codigo_facturador))
				codigo_facturador = "*";


			//var documentos = (from Pagos in context.TblPagosDetalles
			//				  where (Pagos.TblDocumentos.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
			//				  //Valida si la fecha es documento o fecha pago
			//				  && ((Pagos.TblDocumentos.DatFechaDocumento >= fecha_inicio && Pagos.TblDocumentos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 2)
			//				  //&& ((Pagos.DatFechaRegistro >= fecha_inicio && Pagos.DatFechaRegistro <= fecha_fin) || tipo_fecha == 1)
			//				  && ((Pagos.TblPagosElectronicos.DatFechaRegistro >= fecha_inicio && Pagos.TblPagosElectronicos.DatFechaRegistro <= fecha_fin) || tipo_fecha == 1)
			//				  && (LstResolucion.Contains(Pagos.TblDocumentos.StrNumResolucion.ToString()) || Resolucion.Equals("*"))
			//				  && (Pagos.TblPagosElectronicos.IntEstadoPago.Equals(cod_estado_recibo) || estado_recibo.Equals("*"))
			//				  && (Pagos.TblDocumentos.IntNumero == num_doc || numero_documento.Equals("*"))
			//				  && (Pagos.TblDocumentos.StrEmpresaAdquiriente.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
			//				  orderby Pagos.TblPagosElectronicos.DatFechaRegistro descending
			//				  select Pagos).ToList();

			List<TblPagosElectronicos> documentos = new List<TblPagosElectronicos>();

			if (num_doc > 0)
			{

				List<TblDocumentos> datos = new List<TblDocumentos>();

				datos = (from d in context.TblDocumentos
						 where d.IntNumero == num_doc
						 && (d.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
						 && (d.StrEmpresaAdquiriente.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
						 select d).ToList();

				if (datos != null)
				{
					List<Guid> lista_documentos = new List<Guid>();
					foreach (var item in datos)
					{
						lista_documentos.Add(item.StrIdSeguridad);
					}

					var detalles = (from det in context.TblPagosDetalles
									where lista_documentos.Contains(det.StrIdSeguridadDoc)
									select det).ToList();

					if (detalles != null)
					{
						lista_documentos = new List<Guid>();
						foreach (var item in detalles)
						{
							lista_documentos.Add(item.StrIdPagoPrincipal);
						}

						documentos = (from Pagos in context.TblPagosElectronicos
									  where lista_documentos.Contains(Pagos.StrIdRegistro)
									  select Pagos).ToList();
					}
				}


			}
			else
			{
				documentos = (from Pagos in context.TblPagosElectronicos
							  where (Pagos.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
							  //Valida si la fecha es documento o fecha pago
							  //&& ((Pagos.TblPagosDetalles.TblDocumentos.DatFechaDocumento >= fecha_inicio && Pagos.TblDocumentos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 2)							  
							  && ((Pagos.DatFechaRegistro >= fecha_inicio && Pagos.DatFechaRegistro <= fecha_fin))
							  && (Pagos.IntEstadoPago.Equals(cod_estado_recibo) || estado_recibo.Equals("*"))
							  && (Pagos.StrEmpresaAdquiriente.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
							  orderby Pagos.DatFechaRegistro descending
							  select Pagos).ToList();
			}



			return documentos;

		}


		public List<TblPagosDetalles> ObtenerPagosDetalle(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, string Resolucion, int tipo_fecha, bool lazyloading = true)
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

			if (string.IsNullOrEmpty(codigo_facturador))
				codigo_facturador = "*";

			List<TblPagosDetalles> documentos = new List<TblPagosDetalles>();

			if (num_doc > 0)
			{

				List<TblDocumentos> datos = new List<TblDocumentos>();

				datos = (from d in context.TblDocumentos
						 where d.IntNumero == num_doc
						 && (d.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
						 && (d.StrEmpresaAdquiriente.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
						 select d).ToList();

				if (datos != null)
				{
					List<Guid> lista_documentos = new List<Guid>();
					foreach (var item in datos)
					{
						lista_documentos.Add(item.StrIdSeguridad);
					}

					var detalles = (from det in context.TblPagosDetalles
									where lista_documentos.Contains(det.StrIdSeguridadDoc)
									select det).ToList();

					if (detalles != null)
					{
						lista_documentos = new List<Guid>();
						foreach (var item in detalles)
						{
							lista_documentos.Add(item.StrIdPagoPrincipal);
						}

						documentos = (from Pagos in context.TblPagosDetalles
									  where lista_documentos.Contains(Pagos.StrIdPagoPrincipal)
									  select Pagos).ToList();
					}
				}


			}
			else
			{
				context.Configuration.LazyLoadingEnabled = lazyloading;

				documentos = (from Pagos in context.TblPagosDetalles
							  join enc_pago in context.TblPagosElectronicos on Pagos.StrIdPagoPrincipal equals enc_pago.StrIdRegistro
							  where (enc_pago.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
							  && ((enc_pago.DatFechaRegistro >= fecha_inicio && enc_pago.DatFechaRegistro <= fecha_fin))
							  && (enc_pago.IntEstadoPago.Equals(cod_estado_recibo) || estado_recibo.Equals("*"))
							  && (enc_pago.StrEmpresaAdquiriente.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
							  orderby enc_pago.DatFechaRegistro descending
							  select Pagos).ToList();
			
			}



			return documentos;

		}

		/// <summary>
		/// Obtiene la lista de pagos del Adquiriente
		/// </summary>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
		/// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
		/// <returns></returns>
		//public List<TblPagosDetalles> ObtenerPagosAdquiriente(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, int tipo_fecha)
		//{

		//	fecha_inicio = fecha_inicio.Date;

		//	fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

		//	int num_doc = -1;
		//	int.TryParse(numero_documento, out num_doc);

		//	short cod_estado_recibo = -1;
		//	short.TryParse(estado_recibo, out cod_estado_recibo);

		//	if (string.IsNullOrWhiteSpace(numero_documento))
		//		numero_documento = "*";
		//	if (string.IsNullOrWhiteSpace(codigo_adquiriente))
		//		codigo_adquiriente = "*";

		//	if (string.IsNullOrWhiteSpace(estado_recibo))
		//		estado_recibo = "*";

		//	if (string.IsNullOrEmpty(codigo_facturador) || codigo_facturador == "null")
		//	{
		//		codigo_facturador = "*";
		//	}


		//	var documentos = (from Pagos in context.TblPagosDetalles
		//					  where Pagos.TblDocumentos.StrEmpresaAdquiriente.Equals(codigo_adquiriente)
		//					  && ((Pagos.TblDocumentos.DatFechaDocumento >= fecha_inicio && Pagos.TblDocumentos.DatFechaDocumento <= fecha_fin) || tipo_fecha == 2)
		//					  && ((Pagos.TblPagosElectronicos.DatFechaRegistro >= fecha_inicio && Pagos.TblPagosElectronicos.DatFechaRegistro <= fecha_fin) || tipo_fecha == 1)
		//					  && (Pagos.TblDocumentos.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
		//					  && (Pagos.TblPagosElectronicos.IntEstadoPago == cod_estado_recibo || estado_recibo.Equals("*"))
		//					  && (Pagos.TblDocumentos.IntNumero == num_doc || numero_documento.Equals("*"))
		//					  orderby Pagos.TblPagosElectronicos.DatFechaRegistro descending
		//					  select Pagos).ToList();

		//	return documentos;

		//}

		/// <summary>
		/// obtiene los pagos por estado de verificación (consulta por DatFechaVerificacion, si esta null no se encuentra verificado). ¿FECHAS?
		/// </summary>
		/// <param name="fecha_inicio">fecha inicial del rango para el filtro fechas (aplica sobre la fecha de registro del pago).</param>
		/// <param name="fecha_fin">fecha final del rango para el filtro por fechas (aplica sobre la fecha de registro del pago).</param>
		/// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
		/// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
		/// <param name="estado_verificacion">(0:por verificar - 1: verificado)</param>
		/// <returns></returns>
		//public List<TblPagosElectronicos> ObtenerPorVerificacion(DateTime fecha_inicio, DateTime fecha_fin, string id_seguridad_pago, string id_plataforma, int estado_verificacion)
		//{
		//	try
		//	{
		//		List<TblPagosElectronicos> pagos = (from pago in context.TblPagosElectronicos

		//											where (pago.DatFechaRegistro.Date >= fecha_inicio && pago.DatFechaRegistro.Date <= fecha_fin)
		//											&& pago.StrIdSeguridadPago.Equals(id_seguridad_pago) || id_seguridad_pago.Equals("*")
		//											&& pago.StrIdPlataforma.Equals(id_plataforma) || id_plataforma.Equals("*")
		//											&& (estado_verificacion == 0) ? pago.DatFechaVerificacion.Value == null : pago.DatFechaVerificacion.Value != null
		//											select pago).ToList();

		//		return pagos;
		//	}
		//	catch (Exception excepcion)
		//	{
		//		throw new ApplicationException(excepcion.Message, excepcion.InnerException);
		//	}
		//}

		#endregion

		#region Plataforma intermedia

		///// <summary>
		///// Realiza el registro del pago en la plataforma electrónica.
		///// Consulta el documento por id de seguridad para la construcción del objeto de envío de los datos del pago.
		///// Retorna la url con el id virtual para la redirección a la página de inicio del proceso de pago.
		///// Si el pago es de un documento los datos de la pasarela son tomados desde el comercio asociado en la resolución del mismo, de lo contrario lso datos serán obtenidos del web.config
		///// </summary>
		///// <param name="id_seguridad">Id de seguridad del documento o plan transaccional</param>
		///// <param name="tipo_pago">indica si el pago es para un documento o una compra de planes (0: Documento - 1: Plan).</param>
		///// <param name="registrar_pago">indica si se registra el pago en base de datos.</param>
		///// <param name="valor_pago">valor a pagar.</param>
		///// <param name="usuario">Guid de seguridad del usuario para guardar en la auditoria</param>
		///// /// <param name="Metodo">Metodo de Pago, 0 = No Definida, 29 = PSE, 31 = Tarjeta de Crédito</param>
		///// <returns></returns>
		//public dynamic ReportePagoElectronicoPI(System.Guid id_seguridad, int tipo_pago = 0, bool registrar_pago = true, double valor_pago = 0, string usuario = "", int IntPagoFormaPago = 0, string lista_documentos = "")
		//{
		//	try
		//	{
		//		//Ruta retornada por el servicio, redirecciona a la  página de inicio del pago (Selección de tipo persona, forma pago y banco).
		//		string ruta_inicio = string.Empty;
		//		//Datos de la pasarela electrónica.				
		//		string comercio_clave = string.Empty;
		//		string comercio_ruta = string.Empty;
		//		string codigo_servicio = string.Empty;
		//		string identificacion_empresa = string.Empty;
		//		string prefijo = string.Empty;
		//		string documento = string.Empty;
		//		string FacturadorAut = string.Empty;

		//		//Objetos para reportar el pago
		//		Cliente datos_cliente = new Cliente();
		//		Pago datos_pago = new Pago();


		//		TblPasarelaPagosPI ObjPago = new TblPasarelaPagosPI();

		//		ObjPago.StrIdSeguridadDoc = id_seguridad;

		//		//ObjPago.StrIdSeguridadRegistro = Guid.NewGuid();
		//		ObjPago.StrIdSeguridadRegistro = Guid.NewGuid();

		//		ObjPago.DatFechaRegistro = Fecha.GetFecha();

		//		ObjPago.DatFechaSync = Fecha.GetFecha();
		//		ObjPago.DatFechaVerificacion = Fecha.GetFecha();

		//		ObjPago.StrAuthIdEmpresa = "";
		//		ObjPago.IntSincronizacion = true;

		//		ObjPago.StrAuthToken = "";

		//		PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
		//		ObjPago.StrRutaSync = plataforma.RutaPublica.ToString() + "/Api/SrcActualizaEstadoP";

		//		ObjPago.StrRutaDestino = "";

		//		ObjPago.StrRutaProcedencia = "";

		//		ObjPago.StrPagoIdPlataforma = "";

		//		decimal Monto_Pendiente = 0;

		//		ObjPago.IntPagoFormaPago = IntPagoFormaPago;

		//		//Valida si el pago es de un documento o una compra de planes.
		//		if (tipo_pago == 0)
		//		{
		//			Ctl_Documento clase_documento = new Ctl_Documento();
		//			TblDocumentos datos_documento = clase_documento.ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

		//			if (datos_documento.IntIdEstadoPago == 1000)//Documento pagado por fuera o manualmente
		//			{
		//				throw new ApplicationException("Documento ya no esta disponible");
		//			}

		//			//Valido si este documento tiene algún pago
		//			var Pagos = (from pagos in context.TblPagosElectronicos
		//						 where pagos.StrIdRegistro2 == datos_documento.StrIdSeguridad
		//						 && pagos.IntEstadoPago == 1
		//						 select pagos.IntValorPago).FirstOrDefault();

		//			//Si tiene algún pago, entonces valido cuanto es el total de todos los pagos
		//			if (Pagos > 0)
		//			{
		//				//Le asigno la suma de los pagos a la siguiente variable
		//				Monto_Pendiente = (from pagos in context.TblPagosElectronicos
		//								   where pagos.StrIdRegistro2 == datos_documento.StrIdSeguridad
		//									&& pagos.IntEstadoPago == 1
		//								   select pagos.IntValorPago).Sum();

		//				//Luego Resto, el total del documento menos la suma de pagos
		//				Monto_Pendiente = datos_documento.IntVlrTotal - Monto_Pendiente;
		//			}

		//			//Id del erp que envia el pago
		//			ObjPago.StrIdDocumento = datos_documento.StrObligadoIdRegistro;

		//			identificacion_empresa = datos_documento.StrEmpresaAdquiriente;

		//			//valida que el documento no sea null.
		//			if (datos_documento != null)
		//			{
		//				ObjPago.DatFechaDocumento = datos_documento.DatFechaDocumento;
		//				ObjPago.StrPrefijoDocumento = datos_documento.StrPrefijo;
		//				ObjPago.IntNumeroDocumento = datos_documento.IntNumero;
		//				ObjPago.DatFechaVencDocumento = datos_documento.DatFechaVencDocumento;
		//				ObjPago.IntVlrTotalDocumento = datos_documento.IntVlrTotal;

		//				//consulta la resolución para obtener el comercio que tiene asociado.
		//				Ctl_EmpresaResolucion clase_resoluciones = new Ctl_EmpresaResolucion();
		//				TblEmpresasResoluciones datos_resolucion = clase_resoluciones.Obtener(datos_documento.StrEmpresaFacturador, datos_documento.StrNumResolucion, datos_documento.StrPrefijo, false);

		//				//Validamos si existe resolucion con los parametros indicados
		//				if (datos_resolucion != null)
		//				{
		//					//Si la resolucion no tiene configuración de pago, entonces tiene que tomar la configuracion de la empresa
		//					if (datos_resolucion.ComercioConfigId != null)
		//					{
		//						ObjPago.StrIdSeguridadComercio = datos_resolucion.ComercioConfigId;
		//						ObjPago.StrAuthToken = datos_resolucion.StrEmpresa;
		//					}
		//					else
		//					{
		//						//Comercio de la empresa
		//						ObjPago.StrIdSeguridadComercio = datos_documento.TblEmpresasFacturador.ComercioConfigId;
		//					}
		//				}
		//				else
		//				{
		//					//Comercio de la empresa
		//					ObjPago.StrIdSeguridadComercio = datos_documento.TblEmpresasFacturador.ComercioConfigId;
		//				}

		//				//Valida que la resolución no sea null.
		//				if (!datos_documento.TblEmpresasFacturador.IntManejaPagoE)
		//				{
		//					throw new ApplicationException("Empresa no maneja Pagos");
		//				}

		//				ObjPago.StrIdTercero = datos_documento.StrEmpresaFacturador;

		//				ObjPago.StrAuthToken = datos_documento.StrEmpresaFacturador;

		//				ObjPago.IntIdAplicativo = Convert.ToInt32(Constantes.IdAplicativoPagosE.ToString());

		//				datos_pago.descripcion_pago = string.Format("{0}", datos_pago.id_pago);
		//				//Si la variable de pago viene sin valor
		//				if (valor_pago <= 0)
		//					if (Monto_Pendiente > 0) //Pregunto si el pago pendiente este presente
		//					{
		//						valor_pago = Convert.ToDouble(Monto_Pendiente);//Si esta presente le asigno la variable pendiente al pago
		//					}
		//					else
		//					{
		//						valor_pago = Convert.ToInt32(datos_documento.IntVlrTotal);//Si no, entonces busco el monto total del pago
		//					}


		//				else
		//				{
		//					if (valor_pago > Convert.ToDouble(datos_documento.IntVlrTotal))
		//						throw new ApplicationException("El valor a pagar no puede ser superior al valor total del documento.");
		//				}
		//			}
		//			else
		//			{

		//				//Obtiene el plan de transacciones.
		//				Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();
		//				TblPlanesTransacciones datos_plan = clase_planes.ObtenerIdSeguridad(id_seguridad);

		//				identificacion_empresa = datos_plan.StrEmpresaFacturador;

		//				if (datos_plan == null)
		//					throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el Plan", id_seguridad));

		//				datos_pago.descripcion_pago = string.Format("{0}", datos_plan.StrObservaciones);

		//				if (valor_pago <= 0)
		//					valor_pago = Convert.ToDouble(datos_plan.IntValor);
		//				else
		//				{
		//					if (valor_pago > Convert.ToDouble(datos_plan.IntValor))
		//						throw new ApplicationException("El valor a pagar no puede ser superior al valor total de la compra.");
		//				}
		//			}

		//			ObjPago.IntValor = decimal.Parse(valor_pago.ToString());
		//			ObjPago.IntValorIva = 0;

		//			//Obtiene los datos del cliente.
		//			Ctl_Empresa clase_empresa = new Ctl_Empresa();
		//			TblEmpresas datos_empresa = clase_empresa.Obtener(identificacion_empresa);

		//			if (datos_empresa == null)
		//				throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "la empresa", datos_empresa.StrIdentificacion));


		//			ObjPago.StrClienteTipoId = datos_empresa.StrTipoIdentificacion.ToString();

		//			ObjPago.StrClienteIdentificacion = datos_empresa.StrIdentificacion;

		//			ObjPago.StrClienteNombre = datos_empresa.StrRazonSocial;

		//			ObjPago.StrClienteTelefono = datos_empresa.StrTelefono;

		//			ObjPago.StrClienteEmail = datos_empresa.StrMailAdmin;

		//			//Encriptar datos de seguridad secundaria                    
		//			ObjPago.StrAuthIdEmpresa = Encriptar.Encriptar_SHA256(ObjPago.StrIdSeguridadRegistro.ToString() + "-" + ObjPago.StrClienteIdentificacion + "-" + ObjPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ObjPago.StrIdSeguridadComercio.ToString() + "-" + ObjPago.IntValor.ToString("0.##"));

		//			//Registro el Pago Local                    
		//			TblPagosElectronicos pago = CrearPago(ObjPago.StrIdSeguridadRegistro, id_seguridad, tipo_pago, Convert.ToDecimal(valor_pago), IntPagoFormaPago, datos_documento.StrEmpresaFacturador, datos_documento.StrEmpresaAdquiriente);

		//			//convierto el Objeto de pago en Json
		//			var ObjetoPago = JsonConvert.SerializeObject(ObjPago);


		//			try
		//			{
		//				Ctl_DocumentosAudit _auditoria = new Ctl_DocumentosAudit();
		//				_auditoria.Crear(id_seguridad, Guid.Empty, FacturadorAut, ProcesoEstado.PagoDocumento, TipoRegistro.Creacion, Procedencia.Usuario, usuario, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(ProcesoEstado.PagoDocumento.GetHashCode())), ObjPago.StrIdSeguridadRegistro.ToString(), prefijo, documento);
		//			}
		//			catch (Exception) { }

		//			//Retorno el Objeto Json Cifrado 
		//			return new { Ruta = EncriptarObjeto(ObjetoPago.ToString()), IdRegistro = ObjPago.StrIdSeguridadRegistro, PasarelaHgi = true };


		//		}
		//		else
		//			throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el documento", id_seguridad));

		//		return ruta_inicio;
		//	}
		//	catch (Exception excepcion)
		//	{
		//		throw new ApplicationException(excepcion.Message, excepcion.InnerException);
		//	}
		//}


		public dynamic ReportePagoElectronicoPIMultiple(string lista_documentos, double valor_pago, string usuario = "")
		{
			try
			{

				List<PagosMultiples> lista_pagos_multiples = new List<PagosMultiples>();

				lista_pagos_multiples = JsonConvert.DeserializeObject<List<PagosMultiples>>(lista_documentos);


				//Ruta retornada por el servicio, redirecciona a la  página de inicio del pago (Selección de tipo persona, forma pago y banco).
				string ruta_inicio = string.Empty;
				//Datos de la pasarela electrónica.				
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

				ObjPago.StrIdSeguridadDoc = Guid.NewGuid(); //PM id_seguridad;				
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

				ObjPago.IntPagoFormaPago = 0;

				//Valida si el pago es de un documento o una compra de planes.

				Ctl_Documento clase_documento = new Ctl_Documento();

				List<TblPagosDetalles> lista_pagos_detalle = new List<TblPagosDetalles>();

				TblDocumentos datos_documento = new TblDocumentos();


				string facturador_actual = string.Empty;

				foreach (var item in lista_pagos_multiples)
				{

					datos_documento = clase_documento.ObtenerPorIdSeguridad(item.Documento).FirstOrDefault();

					//valida que el documento no sea null.
					if (datos_documento != null)
					{
						//Se hace una validacion para garantizar que el pago multiple, sea a documentos del mismo facturador.
						if (!string.IsNullOrEmpty(facturador_actual))
						{
							if (facturador_actual != datos_documento.StrEmpresaFacturador)
							{
								throw new ApplicationException("No se puede hacer hacer pagos multiples a diferentes facturadores");
							}
						}

						//Valida que la resolución no sea null.
						if (!datos_documento.TblEmpresasFacturador.IntManejaPagoE)
						{
							throw new ApplicationException("Empresa no maneja Pagos");
						}

						//Documento pagado por fuera o manualmente
						if (datos_documento.IntIdEstadoPago == 1000)
						{
							throw new ApplicationException(string.Format("Documento {0} ya no esta disponible", datos_documento.IntNumero));
						}

						//Validamos Saldo Pendiente del documento
						Monto_Pendiente = ConsultaSaldoDocumentoPM(datos_documento.StrIdSeguridad, datos_documento.IntValorPagar);

						//Si la variable de pago viene sin valor
						if (item.Valor <= 0)
						{
							if (Monto_Pendiente > 0) //Pregunto si el pago pendiente este presente
							{
								//Validamos si el valor a pagar es mayor al saldo pendiente
								if (item.Valor > Monto_Pendiente)
								{
									//Si es mayor, entonces retornamos el error
									throw new ApplicationException(string.Format("El saldo pendiente del documento {0} ({1}), es mayor al valor que desea pagar", datos_documento.IntNumero, Monto_Pendiente));
								}
								else
								{
									//Si el valor a pagar es menor o igual al saldo, entonces lo asignamos al objeto de pago
									ObjPago.IntValor = decimal.Parse(item.Valor.ToString());
								}
							}
							else
							{
								throw new ApplicationException(string.Format("El monto a pagar del documento {0} no puede ser menor o igual a cero(0)", datos_documento.IntNumero));
							}

						}
						//else
						//{
						//	if (valor_pago > Convert.ToDouble(datos_documento.IntVlrTotal))
						//		throw new ApplicationException("El valor a pagar no puede ser superior al valor total del documento.");
						//}

						//consulta la resolución para obtener el comercio que tiene asociado.
						Ctl_EmpresaResolucion clase_resoluciones = new Ctl_EmpresaResolucion();
						TblEmpresasResoluciones datos_resolucion = clase_resoluciones.Obtener(datos_documento.StrEmpresaFacturador, datos_documento.StrNumResolucion, datos_documento.StrPrefijo, false);

						//Validamos si existe resolucion con los parametros indicados
						if (datos_resolucion != null)
						{
							//Si la resolucion no tiene configuración de pago, entonces tiene que tomar la configuracion de la empresa
							if (datos_resolucion.ComercioConfigId != null)
							{
								ObjPago.StrIdSeguridadComercio = datos_resolucion.ComercioConfigId;
								ObjPago.StrAuthToken = datos_resolucion.StrEmpresa;
							}
							else
							{
								//Comercio de la empresa
								ObjPago.StrIdSeguridadComercio = datos_documento.TblEmpresasFacturador.ComercioConfigId;
							}
						}
						else
						{
							//Comercio de la empresa
							ObjPago.StrIdSeguridadComercio = datos_documento.TblEmpresasFacturador.ComercioConfigId;
						}




						//Retorno el Objeto Json Cifrado 

						datos_pago.descripcion_pago = string.Format("{0}", datos_pago.id_pago);
						//Id del erp que envia el pago
						ObjPago.StrIdDocumento = datos_documento.StrObligadoIdRegistro;
						identificacion_empresa = datos_documento.StrEmpresaAdquiriente;
						ObjPago.DatFechaDocumento = datos_documento.DatFechaDocumento;
						ObjPago.StrPrefijoDocumento = datos_documento.StrPrefijo;
						ObjPago.IntNumeroDocumento = datos_documento.IntNumero;
						ObjPago.DatFechaVencDocumento = datos_documento.DatFechaVencDocumento;
						ObjPago.IntVlrTotalDocumento = datos_documento.IntVlrTotal;
						ObjPago.StrIdTercero = datos_documento.StrEmpresaFacturador;
						ObjPago.StrAuthToken = datos_documento.StrEmpresaFacturador;
						ObjPago.IntIdAplicativo = Convert.ToInt32(Constantes.IdAplicativoPagosE.ToString());
						ObjPago.IntValorIva = 0;

						TblPagosDetalles pago_detalle = new TblPagosDetalles();
						pago_detalle.StrIdPagoPrincipal = ObjPago.StrIdSeguridadRegistro;
						pago_detalle.StrIdSeguridadDoc = datos_documento.StrIdSeguridad;
						pago_detalle.IntValorPago = item.Valor;
						lista_pagos_detalle.Add(pago_detalle);

						facturador_actual = datos_documento.StrEmpresaFacturador;
					}
					else
						throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el documento", ObjPago.StrIdSeguridadDoc));

				}

				//Registrar el pago unico

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
				ObjPago.IntValor = decimal.Parse(valor_pago.ToString());

				try
				{
					ObjPago.IntValor = Convert.ToInt32(Math.Floor(valor_pago));
				}
				catch (Exception)
				{

				}

				//Encriptar datos de seguridad secundaria                    
				ObjPago.StrAuthIdEmpresa = Encriptar.Encriptar_SHA256(ObjPago.StrIdSeguridadRegistro.ToString() + "-" + ObjPago.StrClienteIdentificacion + "-" + ObjPago.DatFechaRegistro.ToString("dd/MM/yyyy h:m:s.F t", CultureInfo.InvariantCulture) + ObjPago.StrIdSeguridadComercio.ToString() + "-" + ObjPago.IntValor.ToString("0.##"));


				//Registro el Pago Local                    
				TblPagosElectronicos pago = CrearPago(ObjPago.StrIdSeguridadRegistro, ObjPago.StrIdSeguridadDoc, 0, ObjPago.IntValor, 0, datos_documento.StrEmpresaFacturador, datos_documento.StrEmpresaAdquiriente);

				Ctl_PagosDetalles _PagosDetalles = new Ctl_PagosDetalles();

				foreach (TblPagosDetalles detalle in lista_pagos_detalle)
				{
					_PagosDetalles.Crear(detalle);
				}
				//convierto el Objeto de pago en Json
				var ObjetoPago = JsonConvert.SerializeObject(ObjPago);

				try
				{
					Ctl_DocumentosAudit _auditoria = new Ctl_DocumentosAudit();
					_auditoria.Crear(ObjPago.StrIdSeguridadDoc, Guid.Empty, FacturadorAut, ProcesoEstado.PagoDocumento, TipoRegistro.Creacion, Procedencia.Usuario, usuario, Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(ProcesoEstado.PagoDocumento.GetHashCode())), ObjPago.StrIdSeguridadRegistro.ToString(), prefijo, documento);
				}
				catch (Exception) { }

				return new { Ruta = EncriptarObjeto(ObjetoPago.ToString()), IdRegistro = ObjPago.StrIdSeguridadRegistro, PasarelaHgi = true };

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

		#region Metodos para los WCF
		/// <summary>
		/// Obtiene los pagos entre un rango de fechas especifica
		/// </summary>
		/// <param name="identificacion_obligado">Identificación del Facturador</param>
		/// <param name="FechaInicial">Fecha Inicial</param>
		/// <param name="FechaFinal">Fecha Final</param>
		/// <param name="Procesados">0 Todos Los Pagos, 1 Pagos Procesados, 2 Pagos que aun no se han Procesado</param>
		/// <returns>List<PagoElectronicoRespuesta></returns>
		public List<PagoElectronicoRespuestaPorFecha> ConsultaPorFechaElaboracion(string identificacion_obligado, DateTime FechaInicial, DateTime FechaFinal, int Procesados = 0)
		{
			try
			{
				//Valida que los parametros sean correctos.
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");


				List<PagoElectronicoRespuestaPorFecha> lista_respuesta = new List<PagoElectronicoRespuestaPorFecha>();

				//Creamos variable para saber si la consulta en con pagos aprobados o no aprobados.
				bool estado_pago = (Procesados == 1) ? true : false;			

				var lista_pagos = (from pagos in context.TblPagosElectronicos
									   //join documento in context.TblDocumentos on pagos.StrIdRegistro2 equals documento.StrIdSeguridad
								   where pagos.StrEmpresaFacturador.Equals(identificacion_obligado)
										   && pagos.DatFechaRegistro >= FechaInicial
										   && pagos.DatFechaRegistro <= FechaFinal
										   && (pagos.IntProcesado == estado_pago || Procesados == 0)
								   select pagos).ToList();


				List<PagoElectronicoRespuestaPorFecha> Lista_Resultado = new List<PagoElectronicoRespuestaPorFecha>();

				PagoElectronicoRespuestaPorFecha resultado = new PagoElectronicoRespuestaPorFecha();

				Ctl_Documento _doc = new Ctl_Documento();

				foreach (var pago in lista_pagos)
				{

					foreach (var detalle in pago.TblPagosDetalles)
					{

						TblDocumentos documento = _doc.ObtenerDocumento(detalle.StrIdSeguridadDoc);
						resultado = new PagoElectronicoRespuestaPorFecha();

						resultado.Documento = documento.IntNumero;
						resultado.Cufe = documento.StrCufe;
						resultado.Identificacion = documento.StrEmpresaAdquiriente;
						resultado.IdDocumento = documento.StrIdSeguridad.ToString();
						resultado.IdRegistro = pago.StrIdRegistro.ToString();
						resultado.DocumentoTipo = documento.IntDocTipo;
						resultado.Fecha = pago.DatFechaRegistro;
						resultado.IdPago = pago.StrIdSeguridadPago;
						resultado.ReferenciaCUS = pago.StrTransaccionCUS;
						resultado.TicketID = pago.StrTicketID;
						resultado.PagoEstadoDescripcion = pago.StrMensaje;
						resultado.PagoEstado = pago.IntEstadoPago;
						resultado.Valor = detalle.IntValorPago;
						resultado.FormaPago = pago.IntFormaPago.ToString();
						resultado.Franquicia = pago.StrCodigoFranquicia;

						Lista_Resultado.Add(resultado);

					}

				}

				var datos = JsonConvert.SerializeObject(Lista_Resultado);

				lista_respuesta = JsonConvert.DeserializeObject<List<PagoElectronicoRespuestaPorFecha>>(datos);


				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}


		/// <summary>
		/// Obtiene los pagos entre un rango de fechas especifica
		/// </summary>
		/// <param name="identificacion_obligado">Identificación del Facturador</param>
		/// <param name="FechaInicial">Fecha Inicial</param>
		/// <param name="FechaFinal">Fecha Final</param>
		/// <param name="Procesados">0 Todos Los Pagos, 1 Pagos Procesados, 2 Pagos que aun no se han Procesado</param>
		/// <returns>List<PagoElectronicoRespuesta></returns>
		/// List<PagoElectronicoRespuestaAgrupadoPorFecha>
		public List<PagoElectronicoRespuestaAgrupadoPorFecha> ConsultaAgrupadosPorFechaElaboracion(string identificacion_obligado, DateTime FechaInicial, DateTime FechaFinal, int Procesados = 0)
		{
			try
			{
				//Valida que los parametros sean correctos.
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");

				FechaInicial = FechaInicial.Date;

				FechaFinal = new DateTime(FechaFinal.Year, FechaFinal.Month, FechaFinal.Day, 23, 59, 59, 999);

				List<PagoElectronicoRespuestaAgrupadoPorFecha> lista_respuesta = new List<PagoElectronicoRespuestaAgrupadoPorFecha>();

				//Creamos variable para saber si la consulta en con pagos aprobados o no aprobados.
				bool estado_pago = (Procesados == 1) ? true : false;


				var lista_pagos = (from pagos in context.TblPagosElectronicos
								   where pagos.StrEmpresaFacturador.Equals(identificacion_obligado)
										   && pagos.DatFechaRegistro >= FechaInicial
										   && pagos.DatFechaRegistro <= FechaFinal
										   && (pagos.IntProcesado == estado_pago || Procesados == 0)
										   && pagos.IntFormaPago == 31//Tarjeta de Credito
								   select pagos).ToList();


				List<PagoElectronicoRespuestaAgrupadoPorFecha> Lista_Resultado = new List<PagoElectronicoRespuestaAgrupadoPorFecha>();
				List<PagoElectronicoRespuestaPorFecha> Lista_Resultado_Detalle = new List<PagoElectronicoRespuestaPorFecha>();

				PagoElectronicoRespuestaPorFecha resultado = new PagoElectronicoRespuestaPorFecha();

				Ctl_Documento _doc = new Ctl_Documento();

				foreach (var pago in lista_pagos)
				{

					var data = new PagoElectronicoRespuestaAgrupadoPorFecha();
					data.Fecha = pago.DatFechaRegistro;
					data.Ciclo = pago.IntClicloTransaccion.Value;
					data.FormaPago = pago.IntFormaPago.Value;
					data.Valor = pago.IntValorPago;

					Lista_Resultado_Detalle = new List<PagoElectronicoRespuestaPorFecha>();
					foreach (var detalle in pago.TblPagosDetalles)
					{

						TblDocumentos documento = _doc.ObtenerDocumento(detalle.StrIdSeguridadDoc);
						resultado = new PagoElectronicoRespuestaPorFecha();

						resultado.Documento = documento.IntNumero;
						resultado.Cufe = documento.StrCufe;
						resultado.Identificacion = documento.StrEmpresaAdquiriente;
						resultado.IdDocumento = documento.StrIdSeguridad.ToString();
						resultado.IdRegistro = pago.StrIdRegistro.ToString();
						resultado.DocumentoTipo = documento.IntDocTipo;
						resultado.Fecha = pago.DatFechaRegistro;
						resultado.IdPago = pago.StrIdSeguridadPago;
						resultado.ReferenciaCUS = pago.StrTransaccionCUS;
						resultado.TicketID = pago.StrTicketID;
						resultado.PagoEstadoDescripcion = pago.StrMensaje;
						resultado.PagoEstado = pago.IntEstadoPago;
						resultado.Valor = detalle.IntValorPago;
						resultado.FormaPago = pago.IntFormaPago.ToString();
						resultado.Franquicia = pago.StrCodigoFranquicia;

						Lista_Resultado_Detalle.Add(resultado);

					}
					data.Pago_Electronico_Respuesta_Por_Fecha = Lista_Resultado_Detalle;

					//
					Lista_Resultado.Add(data);
				}



				var lista_pagos_pse = (from pagos in context.TblPagosElectronicos
									   where pagos.StrEmpresaFacturador.Equals(identificacion_obligado)
											   && pagos.DatFechaRegistro >= FechaInicial
											   && pagos.DatFechaRegistro <= FechaFinal
											   && (pagos.IntProcesado == estado_pago || Procesados == 0)
											   && pagos.IntFormaPago == 29 //PSE
									   group pagos by new { pagos.StrEmpresaFacturador, pagos.IntEstadoPago, pagos.IntFormaPago, pagos.IntClicloTransaccion, pagos.DatFechaRegistro.Year, pagos.DatFechaRegistro.Month, pagos.DatFechaRegistro.Day } into pago
									   select pago).OrderByDescending(x => x.FirstOrDefault().StrTicketID).ToList();



				PagoElectronicoRespuestaPorFecha PagoAgrupado = new PagoElectronicoRespuestaPorFecha();
				foreach (var item in lista_pagos_pse)
				{
					var data = new PagoElectronicoRespuestaAgrupadoPorFecha();
					data.Fecha = item.FirstOrDefault().DatFechaRegistro;
					data.Ciclo = item.FirstOrDefault().IntClicloTransaccion.Value;
					data.FormaPago = item.FirstOrDefault().IntFormaPago.Value;
					data.Valor = item.Select(d => d.IntValorPago).Sum();

					Lista_Resultado_Detalle = new List<PagoElectronicoRespuestaPorFecha>();
					foreach (var item2 in item)
					{
						

						var pagos2 = (from pagos in context.TblPagosElectronicos
									  where pagos.StrIdRegistro == item2.StrIdRegistro
									  select pagos).FirstOrDefault();

						

						foreach (var detalle in pagos2.TblPagosDetalles)
						{

							
							TblDocumentos documento = _doc.ObtenerDocumento(detalle.StrIdSeguridadDoc);
							resultado = new PagoElectronicoRespuestaPorFecha();

							resultado.Documento = documento.IntNumero;
							resultado.Cufe = documento.StrCufe;
							resultado.Identificacion = documento.StrEmpresaAdquiriente;
							resultado.IdDocumento = documento.StrIdSeguridad.ToString();
							resultado.IdRegistro = pagos2.StrIdRegistro.ToString();
							resultado.DocumentoTipo = documento.IntDocTipo;
							resultado.Fecha = pagos2.DatFechaRegistro;
							resultado.IdPago = pagos2.StrIdSeguridadPago;
							resultado.ReferenciaCUS = pagos2.StrTransaccionCUS;
							resultado.TicketID = pagos2.StrTicketID;
							resultado.PagoEstadoDescripcion = pagos2.StrMensaje;
							resultado.PagoEstado = pagos2.IntEstadoPago;
							resultado.Valor = detalle.IntValorPago;
							resultado.FormaPago = pagos2.IntFormaPago.ToString();
							resultado.Franquicia = pagos2.StrCodigoFranquicia;

							Lista_Resultado_Detalle.Add(resultado);

						}
												
					}
					data.Pago_Electronico_Respuesta_Por_Fecha = Lista_Resultado_Detalle;

					Lista_Resultado.Add(data);

				}


				var datos = JsonConvert.SerializeObject(Lista_Resultado);

				lista_respuesta = JsonConvert.DeserializeObject<List<PagoElectronicoRespuestaAgrupadoPorFecha>>(datos);


				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

		/// <summary>
		/// Obtiene los documentos por Código de Registros
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>

		/// <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
		/// <returns></returns>
		public List<PagoElectronicoRespuestaDetalle> ActualizarEstadoPago(string identificacion_obligado, string CodigosRegistros)
		{
			try
			{
				//Valida que los parametros sean correctos.
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");
				if (string.IsNullOrWhiteSpace(CodigosRegistros))
					throw new ApplicationException("Filtro por números inválido.");

				List<PagoElectronicoRespuestaDetalle> lista_respuesta = new List<PagoElectronicoRespuestaDetalle>();

				//Convierte CodigoRegistros en una lista.
				List<string> lista_pagos = Coleccion.ConvertirLista(CodigosRegistros);

				//Se restringe la consulta a un número de documentos específicos  NOVIEMBRE 16
				if (lista_pagos.Count > 100)
					throw new ApplicationException("Supera el número máximo de 100 registros por consulta.");

				context.Configuration.LazyLoadingEnabled = false;

				var pagos = (from p in context.TblPagosElectronicos
							 where (lista_pagos.Contains(p.StrIdRegistro.ToString()))
							 select p).ToList();

				foreach (TblPagosElectronicos p in pagos)
				{
					try
					{
						p.IntProcesado = true;
						this.Edit(p);


						PagoElectronicoRespuestaDetalle detalle_pago = new PagoElectronicoRespuestaDetalle();

						detalle_pago.IdRegistro = p.StrIdRegistro.ToString();
						detalle_pago.Fecha = p.DatFechaRegistro;
						detalle_pago.IdPago = p.StrIdSeguridadPago;
						detalle_pago.ReferenciaCUS = p.StrTransaccionCUS;
						detalle_pago.TicketID = p.StrTicketID;
						detalle_pago.PagoEstadoDescripcion = p.StrMensaje;
						detalle_pago.PagoEstado = p.IntEstadoPago;
						detalle_pago.Valor = p.IntValorPago;
						detalle_pago.FormaPago = p.IntFormaPago.ToString();
						detalle_pago.Franquicia = p.StrCodigoFranquicia;

						lista_respuesta.Add(detalle_pago);

					}
					catch (Exception)
					{

					}
				}

				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}



		#endregion

		#region Generación de Nota Crédito como pago


		/// <summary>
		/// Proceso asyncronico para generar una nota credito como pago en la factura
		/// </summary>
		/// <param name="cufe_factura">identificador de la factura afectada por la nota</param>
		/// <param name="documentoBd">resgitro de la nota credito en bd para tomar el valor a registrar como pago</param>
		/// <returns></returns>
		public async Task GenerarNotaPago(string cufe_factura, TblDocumentos documentoBd)
		{
			try
			{
				var Tarea = TareaGenerarNotaPagoAsync(cufe_factura, documentoBd);
				await Task.WhenAny(Tarea);
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio);
			}
		}

		public async Task TareaGenerarNotaPagoAsync(string cufe_factura, TblDocumentos documentoBd)
		{
			await Task.Factory.StartNew(() =>
			{
				ProcesarNotaPago(cufe_factura, documentoBd);
			});
		}

		/// <summary>
		/// Crea el pago de una nota credito para que al consultar el saldo, este ya descuente el valor de la nota
		/// </summary>
		/// <param name="cufe_factura">CUFE de la Factura</param>
		/// <param name="documentoBd">Tabla TblDocumentos</param>
		public void ProcesarNotaPago(string cufe_factura, TblDocumentos documentoBd)
		{
			try
			{
				TblDocumentos factura = (from Doc in context.TblDocumentos
										 where Doc.StrCufe == cufe_factura && Doc.IntDocTipo == 1
										 select Doc).FirstOrDefault();

				if (factura != null)
				{
					//Crea el pago en la tabla principal
					TblPagosElectronicos datos_registro = new TblPagosElectronicos();
					datos_registro.StrIdRegistro = Guid.NewGuid();
					datos_registro.StrIdRegistro2 = factura.StrIdSeguridad;					
					datos_registro.DatFechaRegistro = Fecha.GetFecha();					
					datos_registro.IntValorPago = documentoBd.IntVlrTotal;
					datos_registro.StrMensaje = "Pago Automatico Nota";
					datos_registro.IntEstadoPago = EstadoPago.Aprobado.GetHashCode();
					datos_registro.IntFormaPago = 0;
					datos_registro.StrEmpresaFacturador = factura.StrEmpresaFacturador;
					datos_registro.StrEmpresaAdquiriente = factura.StrEmpresaAdquiriente;
					datos_registro.StrCodigoFranquicia = "NC";
					Crear(datos_registro);

					//Crea el detalle del pago
					TblPagosDetalles pago_detalle = new TblPagosDetalles();
					pago_detalle.StrIdSeguridadDoc = factura.StrIdSeguridad;
					pago_detalle.StrIdPagoPrincipal = datos_registro.StrIdRegistro;
					pago_detalle.IntValorPago = datos_registro.IntValorPago;

					Ctl_PagosDetalles _PagosDetalles = new Ctl_PagosDetalles();

					_PagosDetalles.Crear(pago_detalle);

				}
			}
			catch (Exception excepcion)
			{
				Ctl_Log.Guardar(excepcion, MensajeCategoria.Sonda, MensajeTipo.Error, MensajeAccion.envio);
			}

		}

		#endregion



		//Pagos Multiples
		/// <summary>
		/// Obtener Saldo de un documento
		/// </summary>
		/// <param name="StrIdSeguridadDoc">Id de seguridad del documento</param>
		/// <param name="valor">Valor del Documento</param>
		/// <returns>Saldo Pendiente</returns>
		public decimal ConsultaSaldoDocumentoPM(System.Guid StrIdSeguridadDoc, decimal valor)
		{
			try
			{
				decimal total = valor;
				decimal valor_pagado_o_pendiente = 0;
				int Pendiente1 = EstadoPago.Pendiente.GetHashCode();//Pendiente por iniciar
				int Pendiente2 = EstadoPago.Pendiente2.GetHashCode();//Pendiente por Finalizar
				int Aprobado = EstadoPago.Aprobado.GetHashCode();


				try
				{
					valor_pagado_o_pendiente = (from pagos in context.TblPagosDetalles
												where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
												&& (pagos.TblPagosElectronicos.IntEstadoPago == Aprobado || pagos.TblPagosElectronicos.IntEstadoPago == Pendiente1 || pagos.TblPagosElectronicos.IntEstadoPago == Pendiente2)
												select pagos.IntValorPago).Sum();
				}
				catch (Exception)
				{
				}

				try
				{
					total = total - valor_pagado_o_pendiente;
				}
				catch (Exception)
				{

				}

				return total;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Retorna, los id de seguridad con los que se debe consultar 
		/// el estado del pago en plataforma intermedia
		/// </summary>
		/// <param name="StrIdSeguridadDoc">Id de seguridad del Documento</param>
		/// <returns>guid de seguridad de los documentos de pago</returns>
		public string ConsultaIDSeguridadPagos(System.Guid StrIdSeguridadDoc)
		{

			try
			{
				var datos = (from pagos in context.TblPagosDetalles
							 where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
							 select pagos).FirstOrDefault();

				return string.Format("'{0}','{1}'", datos.TblPagosElectronicos.StrIdRegistro, datos.TblPagosElectronicos.StrIdRegistro2);
			}
			catch (Exception)
			{
				return "";
			}



		}

		public class PagosMultiples
		{
			public Guid Documento { get; set; }
			public decimal Valor { get; set; }
		}

	}
}
