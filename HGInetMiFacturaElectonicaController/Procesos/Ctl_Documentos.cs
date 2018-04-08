using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetFirmaDigital;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Properties;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	/// <summary>
	/// Controlador para gestionar los documentos
	/// </summary>
	public class Ctl_Documentos
	{

		/// <summary>
		/// Realiza el proceso de la plataforma para el documento
		/// 1. Generar UBL - 2. Firmar - 3. Almacenar XML - 4. Comprimir - 5. Enviar DIAN
		/// </summary>
		/// <param name="id_peticion">id único de identificación de la plataforma</param>
		/// <param name="documento_obj">datos del documento</param>
		/// <param name="pruebas">indica si el documento es de pruebas (true)</param>
		/// <returns>datos de resultado para el documento</returns>
		public static DocumentoRespuesta Procesar(Guid id_peticion, Factura documento_obj, bool pruebas = false, bool solo_validar = false)
		{
			bool error = false;

			DateTime fecha_actual = Fecha.GetFecha();

			FacturaE_Documento documento_result = new FacturaE_Documento();

			DocumentoRespuesta respuesta = new DocumentoRespuesta()
			{
				Aceptacion = 0,
				CodigoRegistro = documento_obj.CodigoRegistro,
				Cufe = "",
				DescripcionProceso = "Recepción - Información del documento.",
				Documento = documento_obj.Documento,
				Error = new LibreriaGlobalHGInet.Error.Error() { Codigo = LibreriaGlobalHGInet.Error.CodigoError.OK, Fecha = fecha_actual, Mensaje = "" },
				FechaRecepcion = fecha_actual,
				FechaUltimoProceso = fecha_actual,
				IdDocumento = Guid.NewGuid().ToString(),
				Identificacion = documento_obj.DatosObligado.Identificacion,
				IdProceso = 1,
				MotivoRechazo = "",
				NumeroResolucion = documento_obj.NumeroResolucion,
				Prefijo = documento_obj.Prefijo,
				ProcesoFinalizado = 0,
				UrlPdf = "",
				UrlXmlUbl = ""
			};

			try
			{

				// valida la información del documento
				try
				{
					fecha_actual = Fecha.GetFecha();
					respuesta.DescripcionProceso = "Valida la información del documento.";
					respuesta.FechaUltimoProceso = fecha_actual;
					respuesta.IdProceso = 2;

					documento_obj = Validar(documento_obj);
				}
				catch (Exception excepcion)
				{
					respuesta.Error = new LibreriaGlobalHGInet.Error.Error()
					{
						Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION,
						Fecha = fecha_actual,
						Mensaje = string.Format("Error en la validación del documento. Detalle: {0}", excepcion.Message)
					};

					throw excepcion;
				}

				if (!solo_validar)
				{

					// genera el xml en ubl
					try
					{
						fecha_actual = Fecha.GetFecha();
						respuesta.DescripcionProceso = "Genera información en estandar UBL.";
						respuesta.FechaUltimoProceso = fecha_actual;
						respuesta.IdProceso = 3;

						documento_result = Ctl_Ubl.Generar(id_peticion, documento_obj, pruebas);
					}
					catch (Exception excepcion)
					{
						respuesta.Error = new LibreriaGlobalHGInet.Error.Error()
						{
							Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION,
							Fecha = fecha_actual,
							Mensaje = string.Format("Error en la generación del estandar UBL del documento. Detalle: {0}", excepcion.Message)
						};

						throw excepcion;
					}

					// almacena el xml en ubl
					try
					{
						fecha_actual = Fecha.GetFecha();
						respuesta.DescripcionProceso = "Almacena el archivo XML con la información en estandar UBL.";
						respuesta.FechaUltimoProceso = fecha_actual;
						respuesta.IdProceso = 4;

						// almacena el xml
						documento_result = Ctl_Ubl.Almacenar(documento_result);
					}
					catch (Exception excepcion)
					{
						respuesta.Error = new LibreriaGlobalHGInet.Error.Error()
						{
							Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION,
							Fecha = fecha_actual,
							Mensaje = string.Format("Error en el almacenamiento del documento UBL en XML. Detalle: {0}", excepcion.Message)
						};

						throw excepcion;
					}

					// firma el xml
					try
					{
						fecha_actual = Fecha.GetFecha();
						respuesta.DescripcionProceso = "Firma el archivo XML con la información en estandar UBL.";
						respuesta.FechaUltimoProceso = fecha_actual;
						respuesta.IdProceso = 5;

						string ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), "certificado_test.p12");
						documento_result = Ctl_Firma.Generar(ruta_certificado, "6c 0b 07 62 62 6d a0 e2", "persona_juridica_pruebas1", EnumCertificadoras.Andes, documento_result);

					}
					catch (Exception excepcion)
					{
						respuesta.Error = new LibreriaGlobalHGInet.Error.Error()
						{
							Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION,
							Fecha = fecha_actual,
							Mensaje = string.Format("Error en el firmado del documento UBL en XML. Detalle: {0}", excepcion.Message)
						};

						throw excepcion;
					}

					// comprime el archivo xml firmado
					try
					{
						fecha_actual = Fecha.GetFecha();
						respuesta.DescripcionProceso = "Compresión del archivo XML firmado con la información en estandar UBL.";
						respuesta.FechaUltimoProceso = fecha_actual;
						respuesta.IdProceso = 6;

						Ctl_Compresion.Comprimir(documento_result);
					}
					catch (Exception excepcion)
					{
						respuesta.Error = new LibreriaGlobalHGInet.Error.Error()
						{
							Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION,
							Fecha = fecha_actual,
							Mensaje = string.Format("Error en la compresión del documento UBL en XML firmado. Detalle: {0}", excepcion.Message)
						};

						throw excepcion;
					}

					// envía el archivo zip con el xml firmado a la DIAN
					HGInetDIANServicios.DianFactura.AcuseRecibo acuse = new HGInetDIANServicios.DianFactura.AcuseRecibo();
					try
					{
						fecha_actual = Fecha.GetFecha();
						respuesta.DescripcionProceso = "Envío del archivo ZIP con el XML firmado a la DIAN.";
						respuesta.FechaUltimoProceso = fecha_actual;
						respuesta.IdProceso = 7;

						acuse = Ctl_DocumentoDian.Enviar(documento_result);
					}
					catch (Exception excepcion)
					{
						respuesta.Error = new LibreriaGlobalHGInet.Error.Error()
						{
							Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION,
							Fecha = fecha_actual,
							Mensaje = string.Format("Error en el envío del archivo ZIP con el XML firmado a la DIAN. Detalle: {0}", excepcion.Message)
						};

						throw excepcion;
					}

					respuesta.Cufe = documento_result.CUFE;

					// url pública del xml
					string url_ppal = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_obj.DatosObligado.Identificacion);
					respuesta.UrlXmlUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);

					// se indica la respuesta de la DIAN
					respuesta.Error.Codigo = LibreriaGlobalHGInet.Error.CodigoError.VALIDACION;
					respuesta.Error.Fecha = fecha_actual;
					respuesta.Error.Mensaje = string.Format("Respuesta DIAN: {0} - {1} - {2}", acuse.Response, acuse.Comments, acuse.ResponseDateTime);

				}
			}
			catch (Exception excepcion)
			{
				// no se controla excepción
			}

			return respuesta;
		}

		/// <summary>
		/// Procesa una lista de documentos
		/// </summary>
		/// <param name="documentos"></param>
		/// <returns></returns>
		public static List<DocumentoRespuesta> Procesar(List<Factura> documentos)
		{
			// genera un id único de la plataforma
			Guid id_peticion = Guid.NewGuid();

			Factura _documento = new Factura();

			DateTime fecha_actual = Fecha.GetFecha();

			List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

			foreach (Factura item in documentos)
			{

				DocumentoRespuesta respuesta_tmp = Procesar(id_peticion, item, true, true);

				respuesta.Add(respuesta_tmp);
			}

			return respuesta;

		}


		public static Factura Validar(Factura documento)
		{
			// valida objeto recibido
			if (documento == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "documento", "Factura"));

			//valida que no este vacio y existencia
			if (string.IsNullOrEmpty(documento.CodigoRegistro))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoRegistro", "string"));

			//valida que no este vacio y existencia
			if (string.IsNullOrEmpty(documento.DataKey))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "DataKey", "string"));

			// validar el número del documento y validez con resolucion
			if (documento.Documento < 0)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Documento", "int").Replace("no puede ser nulo", "no puede ser menor a 0"));

			//Validar que no este vacio y si esta correcta
			if (string.IsNullOrEmpty(documento.NumeroResolucion))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));

			//Validar que no este vacia la fecha
			if (documento.Fecha == null)
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Fecha", "DateTime"));

			//Valida que la fecha este en los terminos
			if (documento.Fecha.Date < Fecha.GetFecha().AddDays(-2).Date || documento.Fecha.Date > Fecha.GetFecha().Date)
				throw new ApplicationException(string.Format("La fecha {0} no esta dentro los terminos.", documento.Fecha));

			//Valida que no este vacio y Formato
			if (string.IsNullOrEmpty(documento.Moneda))
				throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "Moneda", "string"));

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosObligado, "Obligado");

			//Valida que no este vacio y este bien formado 
			ValidarTercero(documento.DatosAdquiriente, "Adquiriente");

			ValidarTotales(documento);

			return documento;
		}

		/// <summary>
		/// Validacion del objeto tercero
		/// </summary>
		/// <param name="tercero">Objeto</param>
		/// <param name="tipo">Tipo de Tercero: Adquiriente - Obligado</param>
		public static void ValidarTercero(Tercero tercero, string tipo)
		{

			if (tercero == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Tercero", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Identificacion))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Identificacion", tipo).Replace("de tipo", "del"));

			if (tercero.IdentificacionDv < 0)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "IdentificacionDv", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Ciudad))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Ciudad", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Departamento))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Departamento", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Direccion))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Direccion", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Telefono))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Telefono", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Email))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Email", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.CodigoPais))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoPais", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.RazonSocial))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "RazonSocial", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.RazonSocial))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "RazonSocial", tipo).Replace("de tipo", "del"));

			if (tercero.TipoIdentificacion == 13)
			{
				if (string.IsNullOrEmpty(tercero.PrimerApellido))
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerApellido", tipo).Replace("de tipo", "del"));

				if (string.IsNullOrEmpty(tercero.SegundoApellido))
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "SegundoApellido", tipo).Replace("de tipo", "del"));

				if (string.IsNullOrEmpty(tercero.PrimerNombre))
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerNombre", tipo).Replace("de tipo", "del"));

				if (string.IsNullOrEmpty(tercero.SegundoNombre))
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "SegundoNombre", tipo).Replace("de tipo", "del"));
			}
		}


		public static void ValidarTotales(Factura documento)
		{

			DocumentoDetalle retorno = ValidarDetalleDocumento(documento.DocumentoDetalles);

			//valida el 
			if (documento.ValorIva != retorno.IvaValor)
				throw new ApplicationException(string.Format("El valor Iva {0} no es correcto.", documento.ValorIva));

			if (documento.ValorDescuento != retorno.DescuentoValor)
				throw new ApplicationException(string.Format("El valor Descuento {0} no es correcto", documento.ValorDescuento));

			if (documento.ValorSubtotal != retorno.ValorSubtotal)
				throw new ApplicationException(string.Format("El subtotal {0} no es correcto.", documento.ValorSubtotal));

			if (documento.ValorImpuestoConsumo != retorno.ValorImpuestoConsumo)
				throw new ApplicationException(string.Format("El Impuesto al Consumo {0} no es correcto.", documento.ValorImpuestoConsumo));

			if (documento.ValorReteFuente != retorno.ReteFuenteValor)
				throw new ApplicationException(string.Format("El valor ReteFuente {0} no es correcto.", documento.ValorReteFuente));

			if (documento.ValorReteIca != retorno.ReteIcaValor)
				throw new ApplicationException(string.Format("El valor ReteIca {0} no es correcto.", documento.ValorReteIca));

			//Calculo del total con los campos enviados en el objeto
			decimal total_cal = documento.ValorSubtotal + documento.ValorIva;
			//Validacion del total del objeto con el calculado
			if (documento.Total != total_cal)
				throw new ApplicationException(string.Format("El valor Total {0} no es correcto.", documento.Total));

			decimal retencion_doc = (documento.ValorReteFuente + documento.ValorReteIca + documento.ValorReteIva);
			if ((documento.Total - retencion_doc) != documento.Neto)
				throw new ApplicationException(string.Format("El valor Neto {0} no es correcto.", documento.Neto));

			if ((documento.Total - documento.Neto - documento.ValorReteFuente - documento.ValorReteIca) != documento.ValorReteIva)
				throw new ApplicationException(string.Format("El valor ReteIva {0} no es correcto.", documento.ValorReteIva));


		}

		public static DocumentoDetalle ValidarDetalleDocumento(List<DocumentoDetalle> documentoDetalle)
		{

			DocumentoDetalle retorno = new DocumentoDetalle();

			decimal Iva_total = 0;
			decimal Desc_total = 0;
			decimal Subtotal = 0;
			decimal Impcon = 0;
			decimal RetIca = 0;
			decimal ReteFte = 0;

			if (documentoDetalle == null || !documentoDetalle.Any())
				throw new Exception("El detalle del documento es inválido.");

			foreach (DocumentoDetalle Docdet in documentoDetalle)
			{
				decimal cantidad = (Docdet.ValorSubtotal + Docdet.DescuentoValor) / Docdet.ValorUnitario;
				if (Docdet.Cantidad != cantidad)
					throw new ApplicationException(string.Format("La cantidad {0} no es correcto.", Docdet.Cantidad));

				decimal val_unitario = (Docdet.ValorSubtotal + Docdet.DescuentoValor) / Docdet.Cantidad;
				if (Docdet.ValorUnitario != val_unitario)
					throw new ApplicationException(string.Format("El valor Unitario {0} no es correcto", Docdet.ValorUnitario));

				decimal val_iva = (Docdet.ValorUnitario * Docdet.IvaPorcentaje) * Docdet.Cantidad;
				if (Docdet.IvaValor != val_iva)
					throw new ApplicationException(string.Format("El valor Iva {0} no es correcto", Docdet.IvaValor));

				if (Docdet.DescuentoPorcentaje > 1 || Docdet.DescuentoPorcentaje < 0)
					throw new ApplicationException(string.Format("El porcentaje Descuento {0} no es correcto", Docdet.DescuentoValor));
				decimal val_descuento = (Docdet.ValorUnitario * Docdet.DescuentoPorcentaje) * Docdet.Cantidad;
				if (Docdet.DescuentoValor != val_descuento)
					throw new ApplicationException(string.Format("El valor Descuento {0} no es correcto", Docdet.DescuentoValor));

				decimal val_subtotal = (Docdet.Cantidad * Docdet.ValorUnitario) - val_descuento;
				if (Docdet.ValorSubtotal != val_subtotal)
					throw new ApplicationException(string.Format("El subtotal {0} no es correcto.", Docdet.ValorSubtotal));

				decimal val_ImpConsumo = Docdet.ValorImpuestoConsumo * Docdet.Cantidad;
				if (Docdet.ValorImpuestoConsumo != val_ImpConsumo)
					throw new ApplicationException(string.Format("El Impuesto al Consumo {0} no es correcto.", Docdet.ValorImpuestoConsumo));

				decimal val_reteica = (Docdet.ValorUnitario * Docdet.ReteIcaPorcentaje) * Docdet.Cantidad;
				if (Docdet.ReteIcaValor != val_reteica)
					throw new ApplicationException(string.Format("El valor ReteIca {0} no es correcto.", Docdet.ReteIcaValor));

				decimal val_retefte = (Docdet.ValorSubtotal * Docdet.ReteFuenteProcentaje);
				if (Docdet.ReteFuenteValor != val_retefte)
					throw new ApplicationException(string.Format("El valor ReteFuente {0} no es correcto.", Docdet.ReteFuenteValor));

				Iva_total += Docdet.IvaValor;
				Desc_total += Docdet.DescuentoValor;
				Subtotal += Docdet.ValorSubtotal;
				Impcon += Docdet.ValorImpuestoConsumo;
				RetIca += Docdet.ReteIcaValor;
				ReteFte += Docdet.ReteFuenteValor;

			}

			retorno.IvaValor = Iva_total;
			retorno.DescuentoValor = Desc_total;
			retorno.ValorSubtotal = Subtotal;
			retorno.ValorImpuestoConsumo = Impcon;
			retorno.ReteIcaValor = RetIca;
			retorno.ReteFuenteValor = ReteFte;
			return retorno;
		}



	}
}


