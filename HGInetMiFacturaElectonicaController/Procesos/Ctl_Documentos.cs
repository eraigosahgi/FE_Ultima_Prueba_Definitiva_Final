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
using LibreriaGlobalHGInet.Formato;
using System.Text.RegularExpressions;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData;
using HGInetDIANServicios;
using HGInetDIANServicios.DianResolucion;
using HGInetMiFacturaElectonicaController.Properties;
using LibreriaGlobalHGInet.Enumerables;
using HGInetUBL;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaController.Auditorias;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	/// <summary>
	/// Controlador para gestionar los documentos
	/// </summary>
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Realiza el proceso en la plataforma para un documento
		/// 1. Generar UBL - 2. Firmar - 3. Almacenar XML - 4. Comprimir - 5. Enviar DIAN - 6. Envío Adquiriente
		/// </summary>
		/// <param name="id_peticion">id único de identificación de la plataforma</param>
		/// <param name="documento">datos del documento</param>
		/// <param name="tipo_doc">tipo de documento a procesar</param>
		/// <param name="resolucion">resolución del documento</param>
		/// <param name="empresa">facturador electrónico del documento</param>
		/// <returns>resultado del proceso</returns>
		public static DocumentoRespuesta Procesar(Guid id_peticion, Guid id_radicado, object documento, TipoDocumento tipo_doc, TblEmpresasResoluciones resolucion, TblEmpresas empresa)
		{
			Ctl_DocumentosAudit clase_auditoria = new Ctl_DocumentosAudit();

			string numero_resolucion = string.Empty;
			string prefijo = string.Empty;

			var documento_obj = (dynamic)null;
			documento_obj = documento;

			//Si fecha trae hora la setea a las 12:00
			documento_obj.Fecha = documento_obj.Fecha.Date;

			if (documento_obj != null)
			{

				DateTime fecha_actual = Fecha.GetFecha();

				FacturaE_Documento documento_result = new FacturaE_Documento();

				var datos_plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;

				DocumentoRespuesta respuesta = new DocumentoRespuesta()
				{
					Aceptacion = 0,
					CodigoRegistro = documento_obj.CodigoRegistro,
					Cufe = "",
					DescripcionProceso = "Recepción - Información del documento.",
					DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido),
					DocumentoTipo = tipo_doc.GetHashCode(),
					Documento = documento_obj.Documento,
					Error = null,
					FechaRecepcion = fecha_actual,
					FechaUltimoProceso = fecha_actual,
					IdDocumento = id_radicado.ToString(),
					Identificacion = documento_obj.DatosAdquiriente.Identificacion,
					IdProceso = ProcesoEstado.Recepcion.GetHashCode(),
					MotivoRechazo = "",
					NumeroResolucion = documento_obj.NumeroResolucion,
					Prefijo = documento_obj.Prefijo,
					ProcesoFinalizado = 0,
					UrlPdf = "",
					UrlXmlUbl = "",
					IdPeticion = id_peticion,
					IdentificacionObligado = documento_obj.DatosObligado.Identificacion,
					UrlAuditoria = string.Format("{0}{1}", datos_plataforma.RutaPublica, Constantes.PaginaConsultaAuditoriaDoc.Replace("{id_seguridad_doc}", id_radicado.ToString())),
				};

				try
				{

					// valida la información del documento
					respuesta = Validar(documento_obj, tipo_doc, resolucion, ref respuesta);
					ValidarRespuesta(respuesta);


					if (empresa.IntHabilitacion > Habilitacion.Valida_Objeto.GetHashCode())
					{

						//Guarda la id de la Peticion con la que se esta haciendo el proceso
						documento_result.IdSeguridadPeticion = id_peticion;

						//Guarda el Id del documento generado por la plataforma
						documento_result.IdSeguridadDocumento = Guid.Parse(respuesta.IdDocumento);

						//Valida que el Proveedor Receptor enviado exista en Bd
						if (documento_obj.IdentificacionProveedor != null)
						{

							if (!documento_obj.IdentificacionProveedor.Equals(Constantes.NitResolucionsinPrefijo))
							{
								TblConfiguracionInteroperabilidad proveedor_receptor = new TblConfiguracionInteroperabilidad();

								Ctl_ConfiguracionInteroperabilidad proveedor = new Ctl_ConfiguracionInteroperabilidad();

								proveedor_receptor = proveedor.Obtener(documento_obj.IdentificacionProveedor);

								//si no existe asigna a HGI como Proveedor Receptor
								if (proveedor_receptor == null)
								{
									documento_obj.IdentificacionProveedor = null;
								}
							}
						}

						Ctl_Documento documento_tmp = new Ctl_Documento();

						//convierte documento para BD
						TblDocumentos documentoBd = Ctl_Documento.Convertir(respuesta, documento_obj, resolucion, empresa, tipo_doc);
						documentoBd.StrIdPlanTransaccion = documento_obj.IdPlan;

						// genera el xml en ubl
						respuesta = UblGenerar(documento_obj, tipo_doc, resolucion, documentoBd, empresa, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						// almacena el xml en ubl
						respuesta = UblGuardar(documentoBd, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						//Asignación de Cufe a documento_obj 
						documento_obj.Cufe = documento_result.CUFE;

						//Asignacion del CUFE a la respuesta
						respuesta.Cufe = documento_result.CUFE;

						// almacena Formato
						respuesta = GuardarFormato(documento_obj, documentoBd, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta, respuesta.UrlPdf);

						// almacena Anexos enviados
						if (documento_obj.ArchivoAnexos != null)
						{
							if (empresa.IntManejaAnexos)
							{
								respuesta = GuardarAnexo(documento_obj.ArchivoAnexos, documentoBd, ref respuesta, ref documento_result);
								ValidarRespuesta(respuesta, respuesta.UrlAnexo);
							}
							else
							{
								respuesta.IdEstado = CategoriaEstado.NoRecibido.GetHashCode();
								respuesta.DescripcionEstado = Enumeracion.GetDescription(CategoriaEstado.NoRecibido);
								respuesta.IdProceso = ProcesoEstado.Validacion.GetHashCode();
								respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.Validacion);
								respuesta.IdDocumento = null;
								respuesta.UrlPdf = null;
								respuesta.UrlXmlUbl = null;
								respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("El Facturador Electrónico {0} no se encuentra habilitado para el procesamiento de anexos", documentoBd.StrEmpresaFacturador), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION);
								ValidarRespuesta(respuesta, "El Facturador Electrónico no se encuentra habilitado para el procesamiento de anexos");
							}
						}


						Ctl_Empresa empresa_config = new Ctl_Empresa();

						TblEmpresas adquirienteBd = null;

						//Validacion de Adquiriente
						try
						{

							//Obtiene la informacion del Adquiriente que se tiene en BD
							adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);
							try
							{

								//Si no existe Adquiriente se crea en BD y se crea Usuario
								if (adquirienteBd == null)
								{
									empresa_config = new Ctl_Empresa();
									//Creacion del Adquiriente
									adquirienteBd = empresa_config.Crear(documento_obj.DatosAdquiriente);

								}
							}
							catch (Exception excepcion)
							{
								string msg_excepcion = Excepcion.Mensaje(excepcion);

								if (!msg_excepcion.ToLowerInvariant().Contains("insert duplicate key"))
									throw excepcion;
								else
									adquirienteBd = empresa_config.Obtener(documento_obj.DatosAdquiriente.Identificacion);
							}
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al obtener el Adquiriente Detalle. Detalle: ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
							LogExcepcion.Guardar(excepcion);
							throw excepcion;
						}

						//Crea el documento en BD
						try
						{
							documentoBd = documento_tmp.Crear(documentoBd);
							documentoBd.TblEmpresasResoluciones = resolucion;
							respuesta.DescuentaSaldo = true;
							respuesta.IdPlan = documentoBd.StrIdPlanTransaccion.Value;
						}
						catch (Exception excepcion)
						{
							respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al guardar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_LICENCIA, excepcion.InnerException);
							LogExcepcion.Guardar(excepcion);
							throw excepcion;
						}

						// firma el xml
						respuesta = UblFirmar(documentoBd, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						// comprime el archivo xml firmado                        
						respuesta = UblComprimir(documentoBd, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta);

						if (documentoBd.IntEnvioMail == true && empresa.IntEnvioMailRecepcion == true)
						{
							respuesta = Envio(documento_obj, documentoBd, empresa, ref respuesta, ref documento_result, true);
							documento_tmp = new Ctl_Documento();
							documentoBd = documento_tmp.Actualizar(documentoBd);
							//ValidarRespuesta(respuesta);
						}

						// envía el archivo zip con el xml firmado a la DIAN
						HGInetDIANServicios.DianFactura.AcuseRecibo acuse = EnviarDian(documentoBd, empresa, ref respuesta, ref documento_result);

						if (documentoBd.IntEnvioMail == true && empresa.IntEnvioMailRecepcion == true)
						{
							respuesta = Envio(documento_obj, documentoBd, empresa, ref respuesta, ref documento_result, true);
							documento_tmp = new Ctl_Documento();
							documentoBd = documento_tmp.Actualizar(documentoBd);
						}
						ValidarRespuesta(respuesta, (acuse != null) ? string.Format("{0} - {1}", acuse.Response, acuse.Comments) : "");


						//Valida estado del documento en la Plataforma de la DIAN
						respuesta = Consultar(documentoBd, empresa, ref respuesta);

						// envía el mail de documentos al adquiriente
						if (respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Aceptado.GetHashCode())
						{

							if ((documentoBd.StrProveedorReceptor == null) || documentoBd.StrProveedorReceptor.Equals(Constantes.NitResolucionsinPrefijo))
							{
								respuesta = Envio(documento_obj, documentoBd, empresa, ref respuesta, ref documento_result);
								//ValidarRespuesta(respuesta);
							}
							else
							{
								//Se actualiza respuesta	
								respuesta.DescripcionProceso = Enumeracion.GetDescription(ProcesoEstado.PendienteEnvioProveedorDoc);
								respuesta.FechaUltimoProceso = Fecha.GetFecha();
								respuesta.IdProceso = ProcesoEstado.PendienteEnvioProveedorDoc.GetHashCode();

								//Actualiza Documento en Base de Datos
								documentoBd.DatFechaActualizaEstado = Fecha.GetFecha();
								documentoBd.IntIdEstado = (short)respuesta.IdProceso;

								//Actualizo el estado del documento para enviar al proveedor receptor
								documento_tmp = new Ctl_Documento();
								documento_tmp.Actualizar(documentoBd);

								//Actualiza la categoria con el nuevo estado
								respuesta.IdEstado = documentoBd.IdCategoriaEstado;
								respuesta.DescripcionEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(documentoBd.IdCategoriaEstado));
							}
						}
						else if ((respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Pendiente.GetHashCode() || respuesta.EstadoDian.EstadoDocumento == EstadoDocumentoDian.Recibido.GetHashCode()) && documentoBd.IntEnvioMail == null)
						{
							respuesta = Envio(documento_obj, documentoBd, empresa, ref respuesta, ref documento_result, true);
							documento_tmp = new Ctl_Documento();
							documentoBd.IntEnvioMail = true;
							documento_tmp.Actualizar(documentoBd);
							//ValidarRespuesta(respuesta);
						}

					}


				}
				catch (Exception excepcion)
				{
					LogExcepcion.Guardar(excepcion);
					// no se controla excepción
				}

				return respuesta;
			}
			throw new ArgumentException("No se recibieron datos para realizar el proceso");
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

			//valida que la identificacion no contenga caracteres especiales
			//Regex isnumber = new Regex("[^0-9]");
			if (!string.IsNullOrEmpty(tercero.Identificacion))
			{
				if (!Texto.ValidarExpresion(TipoExpresion.Numero, tercero.Identificacion) && !Texto.ValidarExpresion(TipoExpresion.Alfanumerico, tercero.Identificacion))
					throw new ArgumentException(string.Format("El parámetro {0} del {1} no puede contener caracteres especiales", "Identificacion", tipo));

				// valida los ceros al inicio de la identificación
				if (!Texto.ValidarExpresion(TipoExpresion.NumeroNotStartZero, tercero.Identificacion))
					throw new ArgumentException(string.Format("El parámetro {0} del {1} no puede contener ceros al inicio", "Identificacion", tipo));
			}
			else
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Identificacion", tipo).Replace("de tipo", "del"));


			if ((tercero.IdentificacionDv < 0) || (tercero.IdentificacionDv > 9))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "IdentificacionDv", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Ciudad))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Ciudad", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Departamento))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Departamento", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Direccion))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Direccion", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Telefono))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Telefono", tipo).Replace("de tipo", "del"));

			//Regex ismail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");

			if (tercero.Email.Contains(";"))
			{
				foreach (var item_mail in Coleccion.ConvertirLista(tercero.Email, ';'))
				{
					if (!Texto.ValidarExpresion(TipoExpresion.Email, item_mail))
						throw new ArgumentException(string.Format("El Email {0} no esta bien formado", item_mail));
				}
			}
			else
			{
				if (!Texto.ValidarExpresion(TipoExpresion.Email, tercero.Email))
					throw new ArgumentException(string.Format("El parámetro {0} del {1} no esta bien formado", "Email", tipo));
			}

			//Regex isweb = new Regex("([\\w-]+\\.)+(/[\\w- ./?%&=]*)?");
			if (tercero.PaginaWeb == null)
			{
				tercero.PaginaWeb = string.Empty;
			}
			else if (!Texto.ValidarExpresion(TipoExpresion.PaginaWeb, tercero.PaginaWeb))
				tercero.PaginaWeb = string.Empty;

			if (string.IsNullOrEmpty(tercero.CodigoPais))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoPais", tipo).Replace("de tipo", "del"));

			if (!ConfiguracionRegional.ValidarCodigoPais(tercero.CodigoPais))
				throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 3166-1 alfa-2 en el {2} ", "CodigoPais", tercero.CodigoPais, tipo));

			if (string.IsNullOrEmpty(tercero.RazonSocial))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "RazonSocial", tipo).Replace("de tipo", "del"));

			if ((tercero.TipoPersona < 1) || (tercero.TipoPersona > 2))
				throw new ArgumentException(string.Format("El parámetro {0} con valor {1} del {2} no esta bien formado", "TipoPersona", tercero.TipoPersona, tipo));

			if (tercero.TipoPersona == 2)
			{
				if (string.IsNullOrEmpty(tercero.PrimerApellido))
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerApellido", tipo).Replace("de tipo", "del"));

				if (string.IsNullOrEmpty(tercero.PrimerNombre))
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "PrimerNombre", tipo).Replace("de tipo", "del"));
			}

		}

		/// <summary>
		/// Valida los totales del objeto
		/// </summary>
		/// <param name="documento_fac">Documento Factura</param>
		/// <param name="documento_nc">Documento Nota Credito</param>
		/// <param name="documento_nd">Documento Nota Debito</param>
		/// <param name="tipo_doc">Tipo de Documento enviado</param>
		public static void ValidarTotales(Factura documento_fac, NotaCredito documento_nc, NotaDebito documento_nd, TipoDocumento tipo_doc)
		{

			var documento = (dynamic)null;

			if (tipo_doc == TipoDocumento.Factura)
				documento = documento_fac;
			else if (tipo_doc == TipoDocumento.NotaCredito)
				documento = documento_nc;
			else if (tipo_doc == TipoDocumento.NotaDebito)
				documento = documento_nd;

			if (documento != null)
			{

				//Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

				//Valida el Iva 
				if (documento.ValorIva == 0)
				{
					documento.ValorIva = Convert.ToDecimal(0.00M);
				}
				if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorIva).Replace(",", ".")))
					//if (!isnumber.IsMatch(Convert.ToString(documento.ValorIva).Replace(",", ".")))
					throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Iva", documento.ValorIva));

				//Valida el Descuento 
				if (documento.ValorDescuento == 0)
				{
					documento.ValorDescuento = Convert.ToDecimal(0.00M);
				}

				if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorDescuento).Replace(",", ".")))
					throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Descuento", documento.ValorDescuento));

				//Valida el Subtotal 
				if (documento.ValorSubtotal == 0)
				{
					documento.ValorSubtotal = Convert.ToDecimal(0.00M);
				}
				if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorSubtotal).Replace(",", ".")))
					throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Subtotal", documento.ValorSubtotal));

				//Valida el Impuesto al consumo 
				if (documento.ValorImpuestoConsumo == 0)
				{
					documento.ValorImpuestoConsumo = Convert.ToDecimal(0.00M);
				}
				if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorImpuestoConsumo).Replace(",", ".")))
					throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Impuesto al Consumo", documento.ValorImpuestoConsumo));

				//Valida la Retencion en la fuente
				if (documento.ValorReteFuente == 0)
				{
					documento.ValorReteFuente = Convert.ToDecimal(0.00M);
				}
				if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorReteFuente).Replace(",", ".")))
					throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ReteFuente", documento.ValorReteFuente));

				//Valida el ReteIca 
				if (documento.ValorReteIca == 0)
				{
					documento.ValorReteIca = Convert.ToDecimal(0.00M);
				}
				if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorReteIca).Replace(",", ".")))
					throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ReteIca", documento.ValorReteIca));

				//Calculo del total con los campos enviados en el objeto
				if (documento.Total == 0)
				{
					documento.Total = Convert.ToDecimal(0.00M);
				}
				if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.Total).Replace(",", ".")))
					throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Total", documento.Total));

				//Validacion del Neto calculado con el que es enviado en el documento
				if (documento.Neto == 0)
				{
					documento.Neto = Convert.ToDecimal(0.00M);
				}
				if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.Neto).Replace(",", ".")))
					throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Valor Neto", documento.Neto));

				//Validacion del ReteIva calculado con el que es enviado en el documento
				if (documento.ValorReteIva == 0)
				{
					documento.ValorReteIva = Convert.ToDecimal(0.00M);
				}

				if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorReteIva).Replace(",", ".")))
					throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ReteIva", documento.ValorReteIva));

				//Se valida el detalle del documento
				ValidarDetalleDocumento(documento.DocumentoDetalles);

			}
		}

		/// <summary>
		/// Valida los totales enviados en el detalle
		/// </summary>
		/// <param name="documentoDetalle"></param>
		/// <returns></returns>
		public static void ValidarDetalleDocumento(List<DocumentoDetalle> documentoDetalle)
		{

			if (documentoDetalle == null || !documentoDetalle.Any())
				throw new Exception("El detalle del documento es inválido.");


			foreach (DocumentoDetalle Docdet in documentoDetalle)
			{
				try
				{
					//Validacion del valor unitario
					//Regex isnumber = new Regex(@"^(0|([1-9][0-9]*))(\.\d\d$)$");

					if (Docdet == null)
						throw new ApplicationException("Se encontro un detalle del documento vacio");

					if (string.IsNullOrEmpty(Docdet.Bodega))
						Docdet.Bodega = string.Empty;

					if (Docdet.ValorUnitario == 0)
					{
						Docdet.ValorUnitario = 0.00M;
					}
					if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ValorUnitario).Replace(",", ".")))
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "Valor Unitario", Docdet.ValorUnitario));

					if (Docdet.DescuentoPorcentaje < 0 || Docdet.DescuentoPorcentaje > 100)
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "Porcentaje Descuento", Docdet.DescuentoPorcentaje));

					if (Docdet.DescuentoValor == 0)
					{
						Docdet.DescuentoValor = Convert.ToDecimal(0.00M);
					}

					//Validacion del valor IVA
					if (Docdet.IvaValor == 0)
					{
						Docdet.IvaValor = Convert.ToDecimal(0.00M);
					}
					if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.IvaValor).Replace(",", ".")))
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "Iva", Docdet.IvaValor));

					//Validacion del Valor Subtotal
					if (Docdet.ValorSubtotal == 0)
					{
						Docdet.ValorSubtotal = Convert.ToDecimal(0.00M);
					}

					if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ValorSubtotal).Replace(",", ".")))
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "Subtotal", Docdet.ValorSubtotal));

					//Validacion del Valor del Impuesto al Consumo
					if (Docdet.ValorImpuestoConsumo == 0)
					{
						Docdet.ImpoConsumoPorcentaje = 0.00M;
						Docdet.ValorImpuestoConsumo = 0.00M;
					}
					if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ValorImpuestoConsumo).Replace(",", ".")))
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "Impuesto al Consumo", Docdet.ValorImpuestoConsumo));


					//Validacion del Valor del ReteICA
					if (Docdet.ReteIcaValor == 0)
					{
						Docdet.ReteIcaValor = 0.00M;
					}
					if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ReteIcaValor).Replace(",", ".")))
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "ReteIca", Docdet.ReteIcaValor));

					//Validacion del Valor del ReteFte
					if (Docdet.ReteFuenteValor == 0)
					{
						Docdet.ReteFuenteValor = 0.00M;
					}
					if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(Docdet.ReteFuenteValor).Replace(",", ".")))
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "ReteFuente", Docdet.ReteFuenteValor));
				}

				catch (Exception ex)
				{
					LogExcepcion.Guardar(ex);
					throw ex;
				}
			}


		}

		/// <summary>
		/// Procesa una lista de documentos tipo Documento Archivo
		/// </summary>
		/// <param name="documentos">documentos Documento Archivo</param>
		/// <returns>objeto tipo Documento Respuesta</returns>
		public static List<DocumentoRespuesta> Procesar(List<DocumentoArchivo> documentos)
		{

			if (documentos == null)
				throw new ApplicationException("No se encontraron datos");
			if (documentos.FirstOrDefault() == null)
				throw new ApplicationException("No se encontraron datos en el primer registro");

			Ctl_Empresa Peticion = new Ctl_Empresa();

			//Válida que la key sea correcta.
			TblEmpresas facturador_electronico = Peticion.Validar(documentos.FirstOrDefault().DataKey, documentos.FirstOrDefault().Identificacion);

			if (!facturador_electronico.IntObligado)
				throw new ApplicationException(string.Format("Licencia inválida para la identificación {0}.", facturador_electronico.StrIdentificacion));

			// genera un id único de la plataforma
			Guid id_peticion = Guid.NewGuid();

			DateTime fecha_actual = Fecha.GetFecha();
			List<TblEmpresasResoluciones> lista_resolucion = new List<TblEmpresasResoluciones>();

			// sobre escribe los datos del facturador electrónico si se encuentra en estado de habilitación
			if (facturador_electronico.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
			{

				//Valida que Resolucion tomar, con Prefijo o sin Prefijo
				string resolucion_pruebas = string.Empty;
				string nit_resolucion = string.Empty;
				string prefijo_pruebas = string.Empty;
				if (documentos.FirstOrDefault().Prefijo.Equals(string.Empty))
				{
					resolucion_pruebas = Constantes.ResolucionPruebas;
					nit_resolucion = Constantes.NitResolucionsinPrefijo;

				}
				else
				{
					resolucion_pruebas = Constantes.ResolucionPruebas;
					nit_resolucion = Constantes.NitResolucionconPrefijo;
					prefijo_pruebas = Constantes.PrefijoResolucionPruebas;
				}



				Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();
				lista_resolucion.Add(_resolucion.Obtener(nit_resolucion, resolucion_pruebas, prefijo_pruebas));

			}
			else
			{
				// actualiza las resoluciones de los servicios web de la DIAN en la base de datos
				lista_resolucion = Ctl_Resoluciones.Actualizar(id_peticion, facturador_electronico);
			}


			if (lista_resolucion == null)
				throw new ApplicationException(string.Format("No se encontraron resoluciones para el Facturador Electrónico {0}", facturador_electronico.StrIdentificacion));
			else if (!lista_resolucion.Any())
				throw new ApplicationException(string.Format("No se encontraron resoluciones para el Facturador Electrónico {0}", facturador_electronico.StrIdentificacion));


			List<DocumentoRespuesta> respuesta = new List<DocumentoRespuesta>();

			foreach (DocumentoArchivo objeto in documentos)
			{


				DocumentoRespuesta item_respuesta = new DocumentoRespuesta();

				//Si el documento enviado ya existe retorna la informacion que se tiene almacenada
				bool doc_existe = false;

				try
				{

					if (string.IsNullOrEmpty(objeto.NumeroResolucion))
						throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "NumeroResolucion", "string"));


					Ctl_Documento num_doc = new Ctl_Documento();

					//valida si el Documento ya existe en Base de Datos
					TblDocumentos numero_documento = num_doc.Obtener(objeto.Identificacion, objeto.Documento, objeto.Prefijo);

					if (numero_documento != null)
					{
						item_respuesta = Ctl_Documento.Convertir(numero_documento);
						doc_existe = true;
						throw new ApplicationException(string.Format("El documento número {0} con prefijo {1} ya xiste para el Facturador Electrónico {2}", objeto.Documento, objeto.Prefijo, facturador_electronico.StrIdentificacion));
					}

					TblEmpresasResoluciones resolucion = null;

					try
					{
						ApplicationException exTMP = new ApplicationException(string.Format("DataRes: {0}", lista_resolucion.FirstOrDefault().StrIdSeguridad));

						LogExcepcion.Guardar(exTMP);

						// filtra la resolución del documento
						resolucion = lista_resolucion.Where(_resolucion => _resolucion.StrNumResolucion.Equals(objeto.NumeroResolucion)).FirstOrDefault();
					}
					catch (Exception excepcion)
					{
						throw new ApplicationException(string.Format("No se encontró la resolución {0} para el Facturador Electrónico {1}", objeto.NumeroResolucion, facturador_electronico.StrIdentificacion));
					}


					// procesa el documento
					item_respuesta = Procesar(id_peticion, objeto, facturador_electronico, resolucion);
				}
				catch (Exception excepcion)
				{

					ProcesoEstado proceso_actual = ProcesoEstado.Recepcion;
					LogExcepcion.Guardar(excepcion);
					if (!doc_existe)
					{
						item_respuesta = new DocumentoRespuesta()
						{
							Aceptacion = 0,
							CodigoRegistro = objeto.CodigoRegistro,
							Cufe = "",
							DescripcionProceso = Enumeracion.GetDescription(proceso_actual),
							DocumentoTipo = objeto.TipoDocumento,
							Documento = objeto.Documento,
							Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.ERROR_NO_CONTROLADO, excepcion.InnerException),
							EstadoDian = null,
							FechaRecepcion = fecha_actual,
							FechaUltimoProceso = fecha_actual,
							IdDocumento = "",
							Identificacion = "",
							IdProceso = proceso_actual.GetHashCode(),
							MotivoRechazo = "",
							NumeroResolucion = objeto.NumeroResolucion,
							Prefijo = objeto.Prefijo,
							ProcesoFinalizado = (proceso_actual == ProcesoEstado.Finalizacion || proceso_actual == ProcesoEstado.FinalizacionErrorDian) ? (1) : 0,
							UrlPdf = "",
							UrlXmlUbl = ""
						};
					}
					else
					{
						item_respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error al procesar el documento. Detalle: {0} ", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
					}

				}
				respuesta.Add(item_respuesta);
			}

			return respuesta;
		}

	}
}




