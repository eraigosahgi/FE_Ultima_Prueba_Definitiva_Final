using System;
using System.Collections.Generic;
using System.Linq;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Properties;
using LibreriaGlobalHGInet.Formato;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData;
using HGInetDIANServicios;
using HGInetMiFacturaElectonicaController.Properties;
using LibreriaGlobalHGInet.Enumerables;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaData.ModeloServicio.Documentos;
using HGInetUBLv2_1.DianListas;
using LibreriaGlobalHGInet.HgiNet;
using LibreriaGlobalHGInet.RegistroLog;

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
					IdEstadoEnvioMail = 0,
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
					IdVersionDian = empresa.IntVersionDian
				};

				try
				{

					// valida la información del documento
					respuesta = Validar(documento_obj, tipo_doc, resolucion, ref respuesta, empresa);
					ValidarRespuesta(respuesta);


					if (empresa.IntHabilitacion > Habilitacion.Valida_Objeto.GetHashCode())
					{

						//Guarda la id de la Peticion con la que se esta haciendo el proceso
						documento_result.IdSeguridadPeticion = id_peticion;

						//Guarda el Id del documento generado por la plataforma
						documento_result.IdSeguridadDocumento = Guid.Parse(respuesta.IdDocumento);

						// Establece la versión de la DIAN
						documento_result.VersionDian = empresa.IntVersionDian;


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

						string cadena_cufe = String.Empty;

						// genera el xml en ubl
						respuesta = UblGenerar(documento_obj, tipo_doc, resolucion, documentoBd, empresa, ref respuesta, ref documento_result, ref cadena_cufe);
						ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						// Establece la versión de la DIAN segun el Facturador
						documento_result.VersionDian = empresa.IntVersionDian;

						// almacena el xml en ubl
						respuesta = UblGuardar(documentoBd, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						//Asignación de Cufe a documento_obj 
						documento_obj.Cufe = documento_result.CUFE;

						//Asignacion del CUFE a la respuesta
						respuesta.Cufe = documento_result.CUFE;

						// almacena Formato
						respuesta = GuardarFormato(documento_obj, documentoBd, ref respuesta, ref documento_result, empresa);
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
							Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
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
							Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
							throw excepcion;
						}

						// firma el xml
						respuesta = UblFirmar(empresa, documentoBd, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta, respuesta.UrlXmlUbl);

						// comprime el archivo xml firmado                        
						respuesta = UblComprimir(documentoBd, ref respuesta, ref documento_result);
						ValidarRespuesta(respuesta, "", null, false);

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
					Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
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
		public static void ValidarTercero(Tercero tercero, string tipo, TblEmpresas Facturador)
		{

			if (tercero == null)
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Tercero", tipo).Replace("de tipo", "del"));

			//valida que la identificacion no contenga caracteres especiales
			//Regex isnumber = new Regex("[^0-9]");
			if (!string.IsNullOrEmpty(tercero.Identificacion))
			{
				if (tercero.TipoIdentificacion.Equals(31) || tercero.TipoIdentificacion.Equals(13))
				{
					if (!Texto.ValidarExpresion(TipoExpresion.Numero, tercero.Identificacion) && !Texto.ValidarExpresion(TipoExpresion.Alfanumerico, tercero.Identificacion))
						throw new ArgumentException(string.Format("El parámetro {0} del {1} no puede contener caracteres especiales", "Identificacion",tipo));

					// valida los ceros al inicio de la identificación
					if (!Texto.ValidarExpresion(TipoExpresion.NumeroNotStartZero, tercero.Identificacion))
						throw new ArgumentException(string.Format("El parámetro {0} del {1} no puede contener ceros al inicio", "Identificacion", tipo));
				}
			}
			else
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Identificacion", tipo).Replace("de tipo", "del"));


			if ((tercero.IdentificacionDv < 0) || (tercero.IdentificacionDv > 9) && tipo.Equals("Obligado"))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "IdentificacionDv", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.CodigoPais))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "CodigoPais", tipo).Replace("de tipo", "del"));

			if (!ConfiguracionRegional.ValidarCodigoPais(tercero.CodigoPais))
				throw new ArgumentException(string.Format("No se encuentra registrado {0} con valor {1} según ISO 3166-1 alfa-2 en el {2} ", "CodigoPais", tercero.CodigoPais, tipo));


			if (string.IsNullOrEmpty(tercero.Ciudad) && tercero.CodigoPais.Equals("CO"))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Ciudad", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Departamento) && tercero.CodigoPais.Equals("CO"))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Departamento", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Direccion))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Direccion", tipo).Replace("de tipo", "del"));

			if (string.IsNullOrEmpty(tercero.Telefono))
				throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Telefono", tipo).Replace("de tipo", "del"));

			//Regex ismail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");

			if (tercero.Email.Contains(";"))
			{
				string tercero_mail = string.Empty;
				if (Coleccion.ConvertirLista(tercero.Email, ';').Count > 0)
				{
					foreach (var item_mail in Coleccion.ConvertirLista(tercero.Email, ';'))
					{
						if (!Texto.ValidarExpresion(TipoExpresion.Email, item_mail.Trim()))
							throw new ArgumentException(string.Format("El Email {0} no esta bien formado", item_mail));
						else
							tercero_mail = (string.IsNullOrEmpty(tercero_mail) ? item_mail.Trim() : string.Format("{0};{1}", tercero_mail, item_mail.Trim()));
					}

					tercero.Email = tercero_mail;
				}
				else
				{
					throw new ArgumentException(string.Format("El parámetro {0} del {1} no esta bien formado", "Email", tipo));
				}
			}
			else
			{
				tercero.Email = tercero.Email.Trim();
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

			if (Facturador.IntVersionDian == 2)
			{
				ListaTipoIdFiscal list_tipoId = new ListaTipoIdFiscal();
				ListaItem identi = list_tipoId.Items.Where(d => d.Codigo.Equals(tercero.TipoIdentificacion.ToString())).FirstOrDefault();
				if (identi == null)
					throw new ArgumentException(string.Format("El Tipo de Identificacion {0} no esta bien formado del {1}", tercero.TipoIdentificacion, tipo));

				if (tipo.Equals("Obligado") && !identi.Codigo.Equals("31"))
					throw new ArgumentException(string.Format("El Tipo de Identificacion {0} para el {1} a facturar no es correcto", tercero.TipoIdentificacion, tipo));

				if (identi.Codigo.Equals("31"))
				{
					//Validacion del digito de verificacion enviado
					short tercero_dv = FuncionesIdentificacion.Dv(tercero.Identificacion);

					if (tercero_dv != tercero.IdentificacionDv)
						throw new ArgumentException(string.Format("El digito de verificacion {0} no esta bien calculado del {1}", tercero.IdentificacionDv, tipo));
				}
				/*
				ListaTipoRegimen list_fiscal = new ListaTipoRegimen();
				ListaItem regimenfiscal = list_fiscal.Items.Where(d => d.Codigo.Equals(tercero.RegimenFiscal)).FirstOrDefault();
				if (regimenfiscal == null)
					throw new ArgumentException(string.Format("El Código del Regimen Fiscal {0} no esta bien formado del {1}", tercero.RegimenFiscal, tipo));
				*/
				ListaPaises list_paises = new ListaPaises();
				ListaItem pais = list_paises.Items.Where(d => d.Codigo.Equals(tercero.CodigoPais)).FirstOrDefault();
				if (pais == null)
					throw new ArgumentException(string.Format("El Codigo Pais {0} no esta bien formado del {1}", tercero.CodigoPais, tipo));

				if (tercero.CodigoPais.Equals("CO"))
				{

					ListaMunicipio list_municipio = new ListaMunicipio();
					ListaItem municipio = list_municipio.Items.Where(d => d.Codigo.Equals(tercero.CodigoCiudad)).FirstOrDefault();
					if (municipio == null)
						throw new ArgumentException(string.Format("El Código Ciudad {0} no esta bien formado del {1}", tercero.CodigoCiudad, tipo));
					else
						tercero.Ciudad = municipio.Nombre;


					ListaDepartamentos list_depart = new ListaDepartamentos();
					ListaItem departamento = list_depart.Items.Where(d => d.Codigo.Equals(tercero.CodigoDepartamento)).FirstOrDefault();
					if (departamento == null)
						throw new ArgumentException(string.Format("El Codigo Departamento {0} no esta bien formado del {1}", tercero.CodigoDepartamento, tipo));
					else
						tercero.Departamento = departamento.Nombre;

					//if (!tercero.CodigoPostal.StartsWith(tercero.CodigoDepartamento))
					//	throw new ArgumentException(string.Format("El Codigo Postal {0} no esta bien formado del {1}", tercero.CodigoPostal, tipo));
					ListaCodigoPostal list_codpostal = new ListaCodigoPostal();
					ListaItem codpostal = list_codpostal.Items.Where(d => d.Codigo.Equals(tercero.CodigoPostal)).FirstOrDefault();
					if (codpostal == null)
						throw new ArgumentException(string.Format("El Codigo Postal {0} del {1} no cumplen con el listado de la DIAN", tercero.CodigoPostal, tipo));
				}

				//Se agrega validacion, se tiene en el listado de la DIAN estas opciones pero presenta notificacion en la rececpion.
				if (tercero.CodigoTributo.Equals("ZZ") || tercero.CodigoTributo.Equals("ZA"))
					tercero.CodigoTributo = "01";

				ListaTipoImpuestoTercero list_tipoImp = new ListaTipoImpuestoTercero();
				ListaItem tipoimp = list_tipoImp.Items.Where(d => d.Codigo.Equals(tercero.CodigoTributo)).FirstOrDefault();
				if (tipoimp == null)
				{
					tercero.CodigoTributo = "01";
					//throw new ArgumentException(string.Format("El Codigo Tributo {0} no esta bien formado del {1}", tercero.CodigoTributo, tipo));
				}

				if (string.IsNullOrEmpty(tercero.RegimenFiscal))
					tercero.RegimenFiscal = "48";
				//	throw new ArgumentException(string.Format("El Código del Regimen Fiscal {0} no esta bien formado del {1}", tercero.RegimenFiscal, tipo));

				bool validar_regimen = false;
				bool regimen_validado = false;
				if (!(tercero.RegimenFiscal.Equals("48") || tercero.RegimenFiscal.Equals("49")))
					validar_regimen = true;


				if (tercero.Responsabilidades == null && tipo.Equals("Obligado"))
				{
					throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Responsabilidades", tipo).Replace("de tipo", "del"));
				}
				else if (tercero.Responsabilidades.Count == 1 && String.IsNullOrWhiteSpace(tercero.Responsabilidades[0]))
				{
					if (tipo.Equals("Obligado"))
						throw new ArgumentException(string.Format(RecursoMensajes.ArgumentNullError, "Responsabilidades", tipo).Replace("de tipo", "del"));
				}
				else
				{
					if (tercero.Responsabilidades.Count == 1 && tercero.Responsabilidades[0].Equals("O-00"))
					{
						throw new ArgumentException(string.Format("Responsabilidad {0} Invalida del {1}", tercero.Responsabilidades[0], tipo));
					}
					else
					{
						if (validar_regimen == true)
						{
							if (tercero.Responsabilidades.Contains("O-16") || tercero.Responsabilidades.Contains("O-49"))
							{
								tercero.RegimenFiscal = "49";
								regimen_validado = true;
							}
							else if (tercero.Responsabilidades.Contains("O-48"))
							{
								tercero.RegimenFiscal = "48";
								regimen_validado = true;
							}
							else if (tipo.Equals("Obligado"))
							{
								tercero.RegimenFiscal = "48";
								regimen_validado = true;
							}
						}


						List<string> responsabilidades = new List<string>();
						foreach (string item in tercero.Responsabilidades)
						{
							ListaTipoResponsabilidad list_resp = new ListaTipoResponsabilidad();
							ListaItem responsabilidad = list_resp.Items.Where(r => r.Codigo.Equals(item)).FirstOrDefault();
							if (responsabilidad != null)
								responsabilidades.Add(item);
							//throw new ArgumentException(string.Format("Responsabilidad {0} Invalida del {1}", item, tipo));

						}
						if (responsabilidades.Count == 0 && tipo.Equals("Obligado"))
						{
							throw new ArgumentException(string.Format(" Las Responsabilidades enviadas del {0} no cumplen con el listado de la DIAN", tipo));
						}

						tercero.Responsabilidades = responsabilidades;

					}

				}

				if (regimen_validado == false && !tipo.Equals("Obligado"))
				{
					if (tercero.TipoPersona.Equals(1))
					{
						tercero.RegimenFiscal = "48";
					}
					else
					{
						tercero.RegimenFiscal = "49";
					}

				}

			}

		}

		/// <summary>
		/// Valida los totales del objeto
		/// </summary>
		/// <param name="documento_fac">Documento Factura</param>
		/// <param name="documento_nc">Documento Nota Credito</param>
		/// <param name="documento_nd">Documento Nota Debito</param>
		/// <param name="tipo_doc">Tipo de Documento enviado</param>
		public static void ValidarTotales(Factura documento_fac, NotaCredito documento_nc, NotaDebito documento_nd, TipoDocumento tipo_doc, TblEmpresas facturador)
		{

			var documento = (dynamic)null;
			bool autoretenedor = false;

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

				if (facturador.IntVersionDian == 2)
				{
					string resp = LibreriaGlobalHGInet.Formato.Coleccion.ConvertListToString(documento.DatosObligado.Responsabilidades, ";");
					if (resp.Contains("O-15") == true)
						autoretenedor = true;

					//if (decimal.Floor(documento.ValorSubtotal) > documento.Total)
					//	throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no puede ser mayor al Total del documento", "Subtotal", documento.ValorSubtotal));

					//Validacion Del valor bruto con respecto al detalle
					List<DocumentoDetalle> detalle = new List<DocumentoDetalle>();
					detalle = documento.DocumentoDetalles;
					if (detalle.Sum(v => (v.ValorSubtotal)) != documento.ValorSubtotal)
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según sumatoria del detalle con valor {2}", "Subtotal", documento.ValorSubtotal, detalle.Sum(v => (v.ValorSubtotal))));

					if (!Numero.Tolerancia(documento.ValorIva, detalle.Sum(v => (v.IvaValor)), 10))
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según sumatoria del detalle con valor {2}", "IVA", documento.ValorIva, detalle.Sum(v => (v.IvaValor))));

					//if (decimal.Floor(detalle.Sum(v => (v.IvaValor))) != decimal.Floor(documento.ValorIva))
					//	throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "IVA", documento.ValorIva));

					if (!Numero.Tolerancia(documento.ValorReteFuente, detalle.Sum(v => (v.ReteFuenteValor)), 10) && autoretenedor == true)
						//if ((detalle.Sum(v => (v.ReteFuenteValor)) != documento.ValorReteFuente) && autoretenedor == true)
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado,según sumatoria del detalle con valor {2}", "ReteFuente", documento.ValorReteFuente, detalle.Sum(v => (v.ReteFuenteValor))));

					if (!Numero.Tolerancia(documento.ValorImpuestoConsumo, detalle.Sum(v => (v.ValorImpuestoConsumo)), 10))
						//if (detalle.Sum(v => (v.ValorImpuestoConsumo)) != documento.ValorImpuestoConsumo)
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según sumatoria del detalle con valor {2}", "ImpuestoConsumo", documento.ValorImpuestoConsumo, detalle.Sum(v => (v.ValorImpuestoConsumo))));

					//Validacion del Anticipo calculado con el que es enviado en el documento
					if (documento.Anticipos == null)
					{

						if (documento.ValorAnticipo == 0)
						{
							documento.ValorAnticipo = Convert.ToDecimal(0.00M);

							if (!Texto.ValidarExpresion(TipoExpresion.Decimal,
								Convert.ToString(documento.ValorAnticipo).Replace(",", ".")))
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Anticipo", documento.ValorAnticipo));
						}
						else
						{
							throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Anticipo", documento.ValorAnticipo));
						}
					}
					else
					{
						if (documento.Anticipos.Count > 0 && documento.ValorAnticipo != 0)
						{
							List<Anticipo> list_ant = documento.Anticipos;
							Anticipo anticipo = new Anticipo();
							if (!list_ant.Sum(v => v.Valor).Equals(documento.ValorAnticipo))
							{
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según sumatoria de los Anticipos con valor {2}", "ValorAnticipo", documento.ValorAnticipo, list_ant.Sum(v => v.Valor)));
							}
							else
							{
								anticipo = list_ant.Where(d => string.IsNullOrEmpty(d.Codigo)).FirstOrDefault();
								if (anticipo != null)
								{
									throw new ApplicationException(string.Format("El identificador del {0} no está bien formado", "Anticipo"));
								}

								anticipo = list_ant.Where(c => c.Valor > documento.ValorSubtotal).FirstOrDefault();
								if (anticipo != null)
								{
									throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no puede ser mayor al Total del documento", "ValorAnticipo", anticipo.Valor));
								}
							}
						}
						else if (documento.Anticipos.Count >= 0 && documento.ValorAnticipo == 0)
						{
							List<Anticipo> list_ant = documento.Anticipos;
							if (!list_ant.Sum(v1 => v1.Valor).Equals(documento.ValorAnticipo))
							{
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según sumatoria de los Anticipos con valor {2}", "ValorAnticipo", documento.ValorAnticipo));
							}
							else
							{
								documento.ValorAnticipo = Convert.ToDecimal(0.00M);

								if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorAnticipo).Replace(",", ".")))
									throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Anticipo", documento.ValorAnticipo));

							}
						}
						else
						{
							throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ValorAnticipo", documento.ValorAnticipo));
						}

					}

					//Validacion de los Cargos calculados enviados en el documento
					if (documento.Cargos == null)
					{
						if (documento.ValorCargo == 0)
						{
							documento.ValorCargo = Convert.ToDecimal(0.00M);

							if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorCargo).Replace(",", ".")))
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ValorCargo", documento.ValorCargo));
						}
						else
						{
							throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ValorCargo", documento.ValorCargo));
						}
					}
					else
					{
						if (documento.Cargos.Count > 0 && documento.ValorCargo != 0)
						{

							List<Cargo> list_cargo = documento.Cargos;
							if (!list_cargo.Sum(v => v.Valor).Equals(documento.ValorCargo))
							{
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según sumatoria de los Cargos con valor {2}", "ValorCargo", documento.ValorCargo, list_cargo.Sum(v => v.Valor)));
							}
							else
							{
								Cargo cargo = new Cargo();
								cargo = list_cargo.Where(d => string.IsNullOrEmpty(d.Descripcion)).FirstOrDefault();
								if (cargo != null)
								{
									throw new ApplicationException("La Descripción del campo Cargo no está bien formado");
								}

								cargo = list_cargo.Where(d => d.Porcentaje > 100).FirstOrDefault();
								if (cargo != null)
								{
									throw new ApplicationException("El porcentaje del campo Cargo no puede ser mayor a 100");
								}

								cargo = list_cargo.Where(d => d.Valor > documento.ValorSubtotal).FirstOrDefault();
								if (cargo != null)
								{
									throw new ApplicationException("El Valor del campo Cargo no puede ser mayor al Subtotal del documento");
								}
							}
						}
						else if (documento.Cargos.Count >= 0 && documento.ValorCargo == 0)
						{
							List<Cargo> list_cargo = documento.Cargos;
							if (!list_cargo.Sum(c => c.Valor).Equals(documento.ValorCargo))
							{
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según sumatoria de los Cargos con valor {2}", "ValorCargo", documento.ValorCargo, list_cargo.Sum(c => c.Valor)));
							}
							else
							{
								documento.ValorCargo = Convert.ToDecimal(0.00M);

								if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorCargo).Replace(",", ".")))
									throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ValorCargo", documento.ValorCargo));
							}
						}
						else
						{
							throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ValorCargo", documento.ValorCargo));
						}

					}

					//Validacion de los Descuentos calculados enviados en el documento
					if (documento.Descuentos == null)
					{
						if (documento.ValorDescuento == 0)
						{
							documento.ValorDescuento = Convert.ToDecimal(0.00M);

							if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorDescuento).Replace(",", ".")))
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ValorDescuento", documento.ValorDescuento));
						}
						else
						{
							throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "ValorDescuento", documento.ValorDescuento));
						}
					}
					else
					{

						if (documento.Descuentos.Count > 0 && documento.ValorDescuento != 0)
						{

							List<Descuento> list_descuento = documento.Descuentos;
							if (!list_descuento.Sum(v => v.Valor).Equals(documento.ValorDescuento))
							{
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según sumatoria de los Descuentos con valor {2}", "ValorDescuento",documento.ValorDescuento, list_descuento.Sum(v => v.Valor)));
							}
							else
							{
								foreach (Descuento item in list_descuento)
								{
									
									if (string.IsNullOrEmpty(item.Codigo))
									{
										throw new ApplicationException("El Código del campo Descuento no está bien formado");
									}
									ListaCodigoDescuento list_razon_desc = new ListaCodigoDescuento();
									ListaItem razon_desc = list_razon_desc.Items.Where(d => d.Codigo.Equals(item.Codigo)).FirstOrDefault();
									if(razon_desc == null)
										throw new ApplicationException("El Código del campo Descuento no está bien formado");

									if (item.Porcentaje > 100)
									{
										throw new ApplicationException("El porcentaje del campo Descuento no puede ser mayor a 100");
									}

									if (item.Valor > documento.ValorSubtotal)
									{
										throw new ApplicationException("El Valor del campo Descuento no puede ser mayor al Subtotal del documento");
									}
								}
							}
						}
						else if (documento.Descuentos.Count >= 0 && documento.ValorDescuento == 0)
						{
							List<Descuento> list_descuento = documento.Descuentos;
							if (!list_descuento.Sum(v => v.Valor).Equals(documento.ValorDescuento))
							{
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según sumatoria de los Descuentos con valor {2}", "ValorDescuento",documento.ValorDescuento, list_descuento.Sum(v => v.Valor)));
							}
							else
							{
								documento.ValorDescuento = Convert.ToDecimal(0.00M);

								if (!Texto.ValidarExpresion(TipoExpresion.Decimal, Convert.ToString(documento.ValorDescuento).Replace(",", ".")))
									throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Descuento", documento.ValorDescuento));
							}
						}
						else
						{
							throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado", "Descuento", documento.ValorDescuento));
						}
					}

					if (!string.IsNullOrEmpty(documento.DocumentoRef) || documento.OrderReference != null)
					{
						if (documento.OrderReference != null)
						{
							if (string.IsNullOrEmpty(documento.OrderReference.Documento))
								documento.OrderReference.Documento = "0";

						}
						else if (!string.IsNullOrEmpty(documento.DocumentoRef))
						{
							documento.OrderReference = new ReferenciaAdicional();
						}

						if (!string.IsNullOrEmpty(documento.DocumentoRef))
						{
							documento.OrderReference.Documento = documento.DocumentoRef;
						}

					}

					if (documento.DocumentosReferencia != null)
					{
						foreach (ReferenciaAdicional docref in documento.DocumentosReferencia)
						{
							ListaTipoReferenciaAdicional list_refAd = new ListaTipoReferenciaAdicional();
							ListaItem refad = list_refAd.Items.Where(d => d.Codigo.Equals(docref.CodigoReferencia)).FirstOrDefault();
							if (refad == null)
								throw new ApplicationException(string.Format("El tipo de documento referenciado '{0}' no es válido según Estandar DIAN", docref.CodigoReferencia));
						}
					}

					if (!string.IsNullOrEmpty(documento.PedidoRef) || documento.DespatchDocument != null)
					{
						if (documento.DespatchDocument != null)
						{
							foreach (ReferenciaAdicional docref in documento.DespatchDocument)
							{
								if (string.IsNullOrEmpty(docref.Documento))
									docref.Documento = "0";
							}
						}
						else if (!string.IsNullOrEmpty(documento.PedidoRef))
						{
							documento.DespatchDocument = new List<ReferenciaAdicional>();
						}

						if (!string.IsNullOrEmpty(documento.PedidoRef))
						{
							ReferenciaAdicional doc = new ReferenciaAdicional();
							doc.Documento = documento.PedidoRef;
							documento.DespatchDocument.Add(doc);
						}

					}

					if (documento.ReceiptDocument != null)
					{
						foreach (ReferenciaAdicional docref in documento.ReceiptDocument)
						{
							if (string.IsNullOrEmpty(docref.Documento))
								docref.Documento = "0";
						}
					}

					//Validacion del total
					decimal total_calculado = decimal.Round(documento.ValorSubtotal + documento.ValorIva + documento.ValorImpuestoConsumo + documento.ValorCargo - documento.ValorDescuento - documento.ValorAnticipo,MidpointRounding.AwayFromZero);
					if (total_calculado != decimal.Round(documento.Total, MidpointRounding.AwayFromZero))
					{
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según calculos de la informacion enviada por valor {2}", "Total", documento.Total, total_calculado));
					}

					//Validacion del Valor Neto
					decimal neto_calculado = decimal.Round(documento.Total - documento.ValorReteFuente - documento.ValorReteIva -documento.ValorReteIca, MidpointRounding.AwayFromZero);
					if (neto_calculado != decimal.Round(documento.Neto, MidpointRounding.AwayFromZero) && autoretenedor == false)
					{
						throw new ApplicationException(string.Format("El campo {0} con valor {1} del encabezado no está bien formado, según calculos de la informacion enviada por valor {2}", "Neto", documento.Neto, neto_calculado));
					}
				}

				//Se valida el detalle del documento
				ValidarDetalleDocumento(documento.DocumentoDetalles, facturador,autoretenedor, documento.TipoOperacion);

			}
		}

		/// <summary>
		/// Valida los totales enviados en el detalle
		/// </summary>
		/// <param name="documentoDetalle"></param>
		/// <returns></returns>
		public static void ValidarDetalleDocumento(List<DocumentoDetalle> documentoDetalle, TblEmpresas facturador, bool autorretenedor, int tipo_operacion)
		{

			if (documentoDetalle == null || !documentoDetalle.Any())
				throw new Exception("El detalle del documento es inválido.");

			bool validacion_Aiu = true;

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

					if (facturador.IntVersionDian == 2)
					{
						//Se valida si llega informacion con AIU que tenga como minimo un item con descripcion
						if (validacion_Aiu == true)
						{
							validacion_Aiu = false;
							List<DocumentoDetalle> lis_detalle = new List<DocumentoDetalle>();
							lis_detalle = documentoDetalle.Where(v => v.Aiu > 0).ToList();
							if (lis_detalle != null && lis_detalle.Any())
							{
								DocumentoDetalle detalle = new DocumentoDetalle();
								detalle = lis_detalle.Where(d => !string.IsNullOrEmpty(d.ProductoDescripcion)).FirstOrDefault();
								if (detalle == null)
									throw new ApplicationException("No se encontro descripción del contrato AIU en el campo ProductoDescripcion en el detalle del documento");
							}
						}

						if (Docdet.ValorUnitario == 0 && Docdet.ProductoGratis == false)
							throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "Valor Unitario", Docdet.ValorUnitario));

						if (Docdet.ProductoGratis == true)
						{
							if (Docdet.ValorUnitario == 0 && Docdet.ValorImpuestoConsumo == 0)
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "Valor Unitario", Docdet.ValorUnitario));

							if (Docdet.ValorSubtotal > 0)
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del producto {2} no está bien formado", "Valor Subtotal", Docdet.ValorSubtotal,Docdet.ProductoCodigo));

							ListaCodigoPrecioReferencia list_precioref = new ListaCodigoPrecioReferencia();
							ListaItem precioref = list_precioref.Items.Where(d => d.Codigo.Equals(Docdet.ProductoGratisPrecioRef)).FirstOrDefault();
							if (precioref == null)
								throw new ApplicationException(string.Format("El campo {0} con valor {1} para informar muestra y/o regalo del detalle no está bien formado", "PrecioReferencia", Docdet.ProductoGratisPrecioRef));
						}

						decimal subtotal_calculado = decimal.Round((Docdet.Cantidad * Docdet.ValorUnitario) - Docdet.DescuentoValor, 2,MidpointRounding.AwayFromZero);
						if ((subtotal_calculado != Docdet.ValorSubtotal) && Docdet.ProductoGratis == false)
							throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado, según calculos generados por valor{2}", "ValorSubTotal", Docdet.ValorSubtotal, subtotal_calculado));

						if (Docdet.CalculaIVA == 0)
						{
							//Se redondea en el valor medio hacia arriba ej: 121.5 redondeado = 122
							//if (decimal.Round((Docdet.ValorSubtotal * (Docdet.IvaPorcentaje / 100)), 2) == Docdet.IvaValor)

							//Se agrega Validacion de la Base para calcular el IVA
							if (Docdet.BaseImpuestoIva == 0 && Docdet.IvaPorcentaje > 0)
								Docdet.BaseImpuestoIva = Docdet.ValorSubtotal;
							else
								Docdet.BaseImpuestoIva += 0.00M;

							decimal iva_cal = decimal.Round((Docdet.BaseImpuestoIva * (Docdet.IvaPorcentaje / 100)),2, MidpointRounding.AwayFromZero);
							if ((iva_cal == Docdet.IvaValor) && Docdet.ProductoGratis == false)
							{
								Docdet.IvaPorcentaje += 0.00M;
								ListaTarifaImpuestoIVA lista_iva = new ListaTarifaImpuestoIVA();
								ListaItem iva = lista_iva.Items.Where(d => d.Codigo.Equals(Docdet.IvaPorcentaje.ToString().Replace(",", "."))).FirstOrDefault();

								if (iva == null)
									throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "IvaPorcentaje", Docdet.IvaPorcentaje));
							}
							else if (Docdet.ProductoGratis == true)
							{

								Docdet.IvaPorcentaje += 0.00M;

								ListaTarifaImpuestoIVA lista_iva = new ListaTarifaImpuestoIVA();
								ListaItem iva = lista_iva.Items.Where(d => d.Codigo.Equals(Docdet.IvaPorcentaje.ToString().Replace(",", "."))).FirstOrDefault();

								if (iva == null)
									throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado para producto gratis o de muestra", "IvaPorcentaje", Docdet.IvaPorcentaje));

								//Calculo el valor del IVA para los regalos o muestras para validarlo con el que envian
								decimal iva_cal_mues = decimal.Round(((Docdet.Cantidad * Docdet.ValorUnitario) - Docdet.DescuentoValor) * (Docdet.IvaPorcentaje / 100), 2, MidpointRounding.AwayFromZero);
								if (iva_cal_mues != Docdet.IvaValor)
								{
									throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado para producto gratis o de muestra, según calculos generados por valor {2}", "Iva", Docdet.IvaValor,iva_cal_mues));
								}
							}
							else
							{
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado, según calculos generados por valor {2}", "ValorIva", Docdet.IvaValor, iva_cal));
							}
						}
						else
						{
							if (Docdet.IvaValor > 0)
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "CalculaIVA", Docdet.CalculaIVA));
						}

						if (!string.IsNullOrEmpty(Docdet.UnidadCodigo))
						{
							ListaUnidadesMedida list_unidad = new ListaUnidadesMedida();
							ListaItem unidad = list_unidad.Items.Where(d => d.Codigo.Equals(Docdet.UnidadCodigo)).FirstOrDefault();
							if (unidad == null)
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no corresponde al estandar de la DIAN", "UnidadCodigo", Docdet.UnidadCodigo));
						}
						else
						{
							throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado", "UnidadCodigo", Docdet.UnidadCodigo));
						}


						if (Docdet.ReteFuenteValor > 0 && autorretenedor == true)
						{
							//if (decimal.Round((Docdet.ValorSubtotal * (Docdet.ReteFuentePorcentaje / 100)), MidpointRounding.AwayFromZero) == Docdet.ReteFuenteValor)
							decimal retefte_cal = decimal.Round((Docdet.ValorSubtotal * (Docdet.ReteFuentePorcentaje / 100)), 2,MidpointRounding.AwayFromZero);
							if (retefte_cal == Docdet.ReteFuenteValor)
							{
								ListaTarifaImpuestoReteFuente list_retefte = new ListaTarifaImpuestoReteFuente();
								ListaItem retfte = list_retefte.Items.Where(d => d.Codigo.Equals(Docdet.ReteFuentePorcentaje.ToString().Replace(",", "."))).FirstOrDefault();
								if (retfte == null)
									throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no corresponde al estandar de la DIAN", "ReteFuentePorcentaje", Docdet.ReteFuentePorcentaje));
							}
							else
							{
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado, según calculos generados por valor {2}", "ReteFuente",Docdet.ReteFuenteValor,retefte_cal));
							}

						}


						if (Docdet.DescuentoValor > 0)
						{
							decimal desc_cal = decimal.Round(((Docdet.ValorUnitario * Docdet.Cantidad) * (Docdet.DescuentoPorcentaje / 100)), 2, MidpointRounding.AwayFromZero);
							if (desc_cal != Docdet.DescuentoValor)
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado, según calculos generados por valor {2}", "DescuentoValor", Docdet.DescuentoValor,desc_cal));
						}

						if ((Docdet.ValorImpuestoConsumo > 0) && Docdet.ProductoGratis == false)
						{
							decimal impconsumo_cal = decimal.Round(Docdet.ValorSubtotal * (Docdet.ImpoConsumoPorcentaje / 100), 2, MidpointRounding.AwayFromZero);
							if ( impconsumo_cal == Docdet.ValorImpuestoConsumo)
							{
								ListaTarifaImpuestoINC list_consumo = new ListaTarifaImpuestoINC();
								ListaItem consumo = list_consumo.Items.Where(d => d.Codigo.Equals(Docdet.ImpoConsumoPorcentaje.ToString().Replace(",", "."))).FirstOrDefault();
								if (consumo == null)
									throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no corresponde al estandar de la DIAN", "ImpoConsumoPorcentaje", Docdet.ImpoConsumoPorcentaje));
							}
							else
							{
								throw new ApplicationException(string.Format("El campo {0} con valor {1} del detalle no está bien formado, según calculos generados por valor {2}", "ValorImpuestoConsumo", Docdet.ValorImpuestoConsumo,impconsumo_cal));
							}

						}

						if (Docdet.DatosMandatario != null)
						{
							ValidarTercero(Docdet.DatosMandatario, "Mandatario", facturador);

						}


						#region Campos Adicionales
						
						bool llenar_marca = false;
						bool llenar_modelo = false;
						if (Docdet.CamposAdicionales != null)
						{
							bool marca = Docdet.CamposAdicionales.Exists(d => d.Descripcion.Equals("MARCA") && string.IsNullOrEmpty(d.Valor));
							if (marca == false && tipo_operacion == 2)
								llenar_marca = true;
							bool modelo = Docdet.CamposAdicionales.Exists(d => d.Descripcion.Equals("MODELO") && string.IsNullOrEmpty(d.Valor));
							if (modelo == false && tipo_operacion == 2)
								llenar_modelo = true;
							
							//Valida que los campos Adicioanles en Valor no este vacio
							CampoValor campo = Docdet.CamposAdicionales.Where(c => string.IsNullOrEmpty(c.Valor)).FirstOrDefault();
							if (campo != null)
								throw new ApplicationException(string.Format("El valor del campo {0} de los Campos Adicionales del detalle no puede estar vacio", campo.Descripcion));
						}
						else if (tipo_operacion == 2)//Si es exportacion debe llevar estos valores segun Anexo de la DIAN
						{
							llenar_marca = true;
							llenar_modelo = true;
							Docdet.CamposAdicionales = new List<CampoValor>();
						}

						if (llenar_marca == true)
						{
							CampoValor valorMar = new CampoValor();
							valorMar.Descripcion = "MARCA";
							valorMar.Valor = "0";
							Docdet.CamposAdicionales.Add(valorMar);
						}

						if (llenar_modelo == true)
						{
							CampoValor valorMod = new CampoValor();
							valorMod.Descripcion = "MODELO";
							valorMod.Valor = "0";
							Docdet.CamposAdicionales.Add(valorMod);
						}

						#endregion

					}
				}
				catch (Exception ex)
				{
					Ctl_Log.Guardar(ex, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.consulta);
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
			Ctl_EmpresaResolucion resolucion_bd = new Ctl_EmpresaResolucion();

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


				lista_resolucion.Add(resolucion_bd.Obtener(nit_resolucion, resolucion_pruebas, prefijo_pruebas));

			}
			else
			{

				lista_resolucion = resolucion_bd.ObtenerResoluciones(facturador_electronico.StrIdentificacion, "*");

				if (lista_resolucion == null || lista_resolucion.Count < 1)
				{
					// actualiza las resoluciones de los servicios web de la DIAN en la base de datos
					lista_resolucion = Ctl_Resoluciones.Actualizar(id_peticion, facturador_electronico);
				}

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

						Ctl_Log.Guardar(exTMP, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);

						// filtra la resolución del documento
						resolucion = lista_resolucion.Where(_resolucion => _resolucion.StrNumResolucion.Equals(objeto.NumeroResolucion) && _resolucion.StrPrefijo.Equals(objeto.Prefijo)).FirstOrDefault();
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
					Ctl_Log.Guardar(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
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




