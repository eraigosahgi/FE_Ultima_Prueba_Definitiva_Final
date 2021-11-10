using System;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaController.Properties;
using HGInetMiFacturaElectonicaController.Registros;
using LibreriaGlobalHGInet.Funciones;
using System.Collections.Generic;
using System.Linq;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	/// <summary>
	/// Controlador para gestionar la generación del estándar UBL
	/// </summary>
	public class Ctl_Ubl
	{

		/// <summary>
		/// Genera la información del documento en formato UBL
		/// </summary>
		/// <param name="id_documento">id único de identificación de la plataforma</param>
		/// <param name="documento">datos del documento</param>
		/// <param name="pruebas">indica si el documento es de pruebas (true)</param>
		/// <returns>datos del documento</returns>
		public static FacturaE_Documento Generar(Guid id_documento, Factura documento, TipoDocumento tipo_doc, TblEmpresas empresa, TblEmpresasResoluciones resolucion, ref string cadena_cufe)
		{

			// resolución del documento
			HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();


			if (empresa.IntVersionDian != 2)
			{

				// obtiene los datos del proveedor tecnológico de la DIAN
				DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

				// obtiene los datos de prueba del proveedor tecnológico de la DIAN
				DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

				string IdSoftware = data_dian.IdSoftware;
				string PinSoftware = data_dian.Pin;
				string NitProveedor = data_dian.NitProveedor;

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
				if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
					NitProveedor = data_dian_habilitacion.NitProveedor;
				}

				// convierte la información de la resolución a la extensión DIAN
				extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
				{
					TipoDocumento = tipo_doc.GetHashCode(),
					NumResolucion = resolucion.StrNumResolucion,
					Prefijo = (!string.IsNullOrEmpty(resolucion.StrPrefijo)) ? resolucion.StrPrefijo : "",
					FechaResIni = resolucion.DatFechaVigenciaDesde,
					FechaResFin = resolucion.DatFechaVigenciaHasta,
					RangoIni = resolucion.IntRangoInicial,
					RangoFin = resolucion.IntRangoFinal,
					IdSoftware = IdSoftware,
					NitProveedor = NitProveedor,
					ClaveTecnicaDIAN = resolucion.StrClaveTecnica,
					PinSoftware = PinSoftware
				};
			}
			else
			{
				DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

				//Para el ambiente de habilitacion a nombre de HGI se cambia informacion del pin e id del SW
				DianProveedor data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedor;

				string IdSoftware = data_dian.IdSoftware;
				string PinSoftware = data_dian.Pin;
				string NitProveedor = data_dian.NitProveedor;

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación y es HGI
				if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode() && empresa.StrIdentificacion.Equals(data_dian_habilitacion.NitProveedor))
				{
					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
					NitProveedor = data_dian_habilitacion.NitProveedor;
				}

				// convierte la información de la resolución a la extensión DIAN
				extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
				{
					TipoDocumento = tipo_doc.GetHashCode(),
					NumResolucion = resolucion.StrNumResolucion,
					Prefijo = (!string.IsNullOrEmpty(resolucion.StrPrefijo)) ? resolucion.StrPrefijo : "",
					FechaResIni = resolucion.DatFechaVigenciaDesde,
					FechaResFin = resolucion.DatFechaVigenciaHasta,
					RangoIni = resolucion.IntRangoInicial,
					RangoFin = resolucion.IntRangoFinal,
					IdSoftware = IdSoftware,
					NitProveedor = NitProveedor,
					ClaveTecnicaDIAN = resolucion.StrClaveTecnica,
					PinSoftware = PinSoftware
				};
			}

			FacturaE_Documento resultado = null;

			// convierte el documento 
			switch (empresa.IntVersionDian)
			{
				case 1:
					resultado = HGInetUBL.FacturaXML.CrearDocumento(id_documento, documento, extension_documento, tipo_doc);
					break;

				case 2:
					PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

					//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
					string ambiente_dian = string.Empty;

					if (plataforma_datos.RutaPublica.Contains("app"))
						ambiente_dian = "1";
					else
						ambiente_dian = "2";

					resultado = HGInetUBLv2_1.FacturaXMLv2_1.CrearDocumento(id_documento, documento, extension_documento, ambiente_dian, ref cadena_cufe);
					break;

				default:
					resultado = HGInetUBL.FacturaXML.CrearDocumento(id_documento, documento, extension_documento, tipo_doc);
					break;
			}

			resultado.DocumentoTipo = tipo_doc;
			resultado.IdSeguridadDocumento = id_documento;
			resultado.IdSeguridadTercero = empresa.StrIdSeguridad;

			// genera el nombre del archivo ZIP
			resultado.NombreZip = HGInetUBLv2_1.NombramientoArchivo.ObtenerZip(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo_doc, documento.Prefijo);

			return resultado;
		}

		/// <summary>
		/// Genera la información del documento en formato UBL
		/// </summary>
		/// <param name="id_seguridad">id único de identificación de la plataforma</param>
		/// <param name="documento">datos del documento</param>
		/// <param name="pruebas">indica si el documento es de pruebas (true)</param>
		/// <returns>datos del documento</returns>
		public static FacturaE_Documento Generar(Guid id_seguridad, NotaCredito documento, TipoDocumento tipo_doc, TblEmpresas empresa, TblEmpresasResoluciones resolucion, ref string cadena_cufe)
		{
			// resolución del documento
			HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();

			if (empresa.IntVersionDian != 2)
			{

				// obtiene los datos del proveedor tecnológico de la DIAN
				DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

				// obtiene los datos de prueba del proveedor tecnológico de la DIAN
				DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

				string IdSoftware = data_dian.IdSoftware;
				string PinSoftware = data_dian.Pin;
				string NitProveedor = data_dian.NitProveedor;

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
				if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
					NitProveedor = data_dian_habilitacion.NitProveedor;
				}

				// convierte la información de la resolución a la extensión DIAN
				extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
				{
					TipoDocumento = tipo_doc.GetHashCode(),
					Prefijo = documento.Prefijo,
					IdSoftware = IdSoftware,
					NitProveedor = NitProveedor,
					ClaveTecnicaDIAN = resolucion.StrClaveTecnica,
					PinSoftware = PinSoftware
				};
			}
			else
			{
				DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

				//Para el ambiente de habilitacion a nombre de HGI se cambia informacion del pin e id del SW
				DianProveedor data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedor;

				string IdSoftware = data_dian.IdSoftware;
				string PinSoftware = data_dian.Pin;
				string NitProveedor = data_dian.NitProveedor;

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación y es HGI
				if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode() && empresa.StrIdentificacion.Equals(data_dian_habilitacion.NitProveedor))
				{
					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
					NitProveedor = data_dian_habilitacion.NitProveedor;
				}

				// convierte la información de la resolución a la extensión DIAN
				extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
				{
					TipoDocumento = tipo_doc.GetHashCode(),
					NumResolucion = resolucion.StrNumResolucion,
					Prefijo = (!string.IsNullOrEmpty(resolucion.StrPrefijo)) ? resolucion.StrPrefijo : "",
					FechaResIni = resolucion.DatFechaVigenciaDesde,
					FechaResFin = resolucion.DatFechaVigenciaHasta,
					RangoIni = resolucion.IntRangoInicial,
					RangoFin = resolucion.IntRangoFinal,
					IdSoftware = IdSoftware,
					NitProveedor = NitProveedor,
					ClaveTecnicaDIAN = resolucion.StrClaveTecnica,
					PinSoftware = PinSoftware
				};
			}

			FacturaE_Documento resultado = null;

			// convierte el documento 
			switch (empresa.IntVersionDian)
			{
				case 1:
					resultado = HGInetUBL.NotaCreditoXML.CrearDocumento(id_seguridad, documento, extension_documento, tipo_doc);
					break;

				case 2:
					PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

					//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
					string ambiente_dian = string.Empty;

					if (plataforma_datos.RutaPublica.Contains("app"))
						ambiente_dian = "1";
					else
						ambiente_dian = "2";

					resultado = HGInetUBLv2_1.NotaCreditoXMLv2_1.CrearDocumento(id_seguridad, documento, extension_documento, tipo_doc, ambiente_dian, ref cadena_cufe);
					break;

				default:
					resultado = HGInetUBL.NotaCreditoXML.CrearDocumento(id_seguridad, documento, extension_documento, tipo_doc);
					break;

			}

			resultado.DocumentoTipo = tipo_doc;
			resultado.IdSeguridadTercero = empresa.StrIdSeguridad;

			// genera el nombre del archivo ZIP
			resultado.NombreZip = HGInetUBLv2_1.NombramientoArchivo.ObtenerZip(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo_doc, documento.Prefijo);

			return resultado;
		}

		/// <summary>
		/// Genera la información del documento en formato UBL
		/// </summary>
		/// <param name="id_documento">id único de identificación de la plataforma</param>
		/// <param name="documento">datos del documento</param>
		/// <param name="pruebas">indica si el documento es de pruebas (true)</param>
		/// <returns>datos del documento</returns>
		public static FacturaE_Documento Generar(Guid id_documento, NotaDebito documento, TipoDocumento tipo_doc, TblEmpresas empresa, TblEmpresasResoluciones resolucion, ref string cadena_cufe)
		{
			// resolución del documento
			HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();

			if (empresa.IntVersionDian != 2)
			{

				// obtiene los datos del proveedor tecnológico de la DIAN
				DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

				// obtiene los datos de prueba del proveedor tecnológico de la DIAN
				DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

				string IdSoftware = data_dian.IdSoftware;
				string PinSoftware = data_dian.Pin;
				string NitProveedor = data_dian.NitProveedor;

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación
				if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
					NitProveedor = data_dian_habilitacion.NitProveedor;
				}

				// convierte la información de la resolución a la extensión DIAN
				extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
				{
					TipoDocumento = tipo_doc.GetHashCode(),
					Prefijo = documento.Prefijo,
					IdSoftware = IdSoftware,
					NitProveedor = NitProveedor,
					ClaveTecnicaDIAN = resolucion.StrClaveTecnica,
					PinSoftware = PinSoftware
				};
			}
			else
			{
				DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

				//Para el ambiente de habilitacion a nombre de HGI se cambia informacion del pin e id del SW
				DianProveedor data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedor;

				string IdSoftware = data_dian.IdSoftware;
				string PinSoftware = data_dian.Pin;
				string NitProveedor = data_dian.NitProveedor;

				// sobre escribe los datos de la resolución si se encuentra en estado de habilitación y es HGI
				if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode() && empresa.StrIdentificacion.Equals(data_dian_habilitacion.NitProveedor))
				{
					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
					NitProveedor = data_dian_habilitacion.NitProveedor;
				}

				// convierte la información de la resolución a la extensión DIAN
				extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
				{
					TipoDocumento = tipo_doc.GetHashCode(),
					NumResolucion = resolucion.StrNumResolucion,
					Prefijo = (!string.IsNullOrEmpty(resolucion.StrPrefijo)) ? resolucion.StrPrefijo : "",
					FechaResIni = resolucion.DatFechaVigenciaDesde,
					FechaResFin = resolucion.DatFechaVigenciaHasta,
					RangoIni = resolucion.IntRangoInicial,
					RangoFin = resolucion.IntRangoFinal,
					IdSoftware = IdSoftware,
					NitProveedor = NitProveedor,
					ClaveTecnicaDIAN = resolucion.StrClaveTecnica,
					PinSoftware = PinSoftware
				};
			}


			// convierte el documento 
			FacturaE_Documento resultado = null;

			switch (empresa.IntVersionDian)
			{
				case 1:
					resultado = HGInetUBL.NotaDebitoXML.CrearDocumento(id_documento, documento, extension_documento, tipo_doc);
					break;

				case 2:
					PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

					//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
					string ambiente_dian = string.Empty;

					if (plataforma_datos.RutaPublica.Contains("app"))
						ambiente_dian = "1";
					else
						ambiente_dian = "2";

					resultado = HGInetUBLv2_1.NotaDebitoXML2_1.CrearDocumento(id_documento, documento, extension_documento, tipo_doc, ambiente_dian, ref cadena_cufe);
					break;

				default:
					resultado = HGInetUBL.NotaDebitoXML.CrearDocumento(id_documento, documento, extension_documento, tipo_doc);
					break;

			}

			resultado.DocumentoTipo = tipo_doc;
			resultado.IdSeguridadTercero = empresa.StrIdSeguridad;

			// genera el nombre del archivo ZIP
			resultado.NombreZip = HGInetUBLv2_1.NombramientoArchivo.ObtenerZip(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo_doc, documento.Prefijo);

			return resultado;
		}

		/// <summary>
		/// Almacena el archivo Xml físicamente
		/// </summary>
		/// <param name="documento">datos del documento</param>
		/// <returns>datos del documento</returns>
		public static FacturaE_Documento Almacenar(FacturaE_Documento documento)
		{
			System.IO.StreamWriter file = null;

			try
			{
				int extension_sector = 0;

				string id_obligado = string.Empty;

				switch (documento.DocumentoTipo)
				{
					case TipoDocumento.Factura:
						Factura doc_factura = ((Factura)documento.Documento);
						id_obligado = documento.IdSeguridadTercero.ToString();
						if (doc_factura.SectorSalud != null && doc_factura.SectorSalud.CamposSector.Count > 0)
							extension_sector = 1;
						break;
					case TipoDocumento.NotaCredito:
						NotaCredito doc_nota_credito = ((NotaCredito)documento.Documento);
						id_obligado = documento.IdSeguridadTercero.ToString();
						break;
					case TipoDocumento.NotaDebito:
						NotaDebito doc_nota_debito = ((NotaDebito)documento.Documento);
						id_obligado = documento.IdSeguridadTercero.ToString();
						break;
					case TipoDocumento.Nomina:
						Nomina doc_nomina = ((Nomina)documento.Documento);
						id_obligado = documento.IdSeguridadTercero.ToString();
						break;
					case TipoDocumento.NominaAjuste:
						NominaAjuste doc_nomina_ajus = ((NominaAjuste)documento.Documento);
						id_obligado = documento.IdSeguridadTercero.ToString();
						break;
					case TipoDocumento.AcuseRecibo:
						Acuse doc_acuse_recibo = ((Acuse)documento.Documento);
						id_obligado = documento.IdSeguridadTercero.ToString();

						break;

					default:
						break;
				}

				// valida el nodo de ExtensionContent
				documento.DocumentoXml = HGInetUBL.ExtensionDian.ValidarNodo(documento.DocumentoXml, extension_sector);


				PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

				// ruta física del xml
				string carpeta_xml = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, id_obligado);
				carpeta_xml = string.Format(@"{0}\{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

				// valida la existencia de la carpeta
				carpeta_xml = Directorio.CrearDirectorio(carpeta_xml);

				// ruta del xml
				string archivo_xml = string.Format(@"{0}.xml", documento.NombreXml);

				// ruta del xml
				string ruta_xml = string.Format(@"{0}\{1}", carpeta_xml, archivo_xml);

				// elimina el archivo xml si existe
				if (Archivo.ValidarExistencia(ruta_xml))
					Archivo.Borrar(ruta_xml);

				// almacena el archivo xml
				string ruta_save = Xml.Guardar(documento.DocumentoXml, carpeta_xml, archivo_xml);

				// asigna la ruta del directorio para los archivos
				documento.RutaArchivosProceso = carpeta_xml;

				// carpeta del zip
				string carpeta_zip = string.Format("{0}\\{1}\\{2}", plataforma_datos.RutaDmsFisica, Constantes.CarpetaFacturaElectronica, id_obligado);
				carpeta_zip = string.Format(@"{0}\{1}", carpeta_zip, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

				// directorio para el zip y xml firmado
				documento.RutaArchivosEnvio = Directorio.CrearDirectorio(carpeta_zip);

				return documento;
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
			finally
			{
				if (file != null)
					file.Close();
			}
		}

		public static FacturaE_Documento Generar(Guid id_seguridad, object documento_obj, TipoDocumento tipo_doc, TblEmpresas empresa, TblEmpresasResoluciones resolucion, ref string cadena_cufe)
		{

			var documento = (dynamic)null;
			documento = documento_obj;

			// resolución del documento
			HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();

			DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

			//Para el ambiente de habilitacion a nombre de HGI se cambia informacion del pin e id del SW
			DianProveedor data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedor;

			DianProveedorTest data_dian_test = HgiConfiguracion.GetConfiguration().DianProveedorTest;

			string IdSoftware = data_dian_test.IdSoftware;
			string PinSoftware = data_dian.Pin;
			string NitProveedor = data_dian.NitProveedor;

			// sobre escribe los datos de la resolución si se encuentra en estado de habilitación y es HGI
			if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode() && empresa.StrIdentificacion.Equals(data_dian_habilitacion.NitProveedor))
			{
				//IdSoftware = data_dian_habilitacion.IdSoftware;
				PinSoftware = data_dian_habilitacion.Pin;
				NitProveedor = data_dian_habilitacion.NitProveedor;
			}

			// convierte la información de la resolución a la extensión DIAN
			extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
			{
				TipoDocumento = tipo_doc.GetHashCode(),
				NumResolucion = resolucion.StrNumResolucion,
				Prefijo = (!string.IsNullOrEmpty(resolucion.StrPrefijo)) ? resolucion.StrPrefijo : "",
				FechaResIni = resolucion.DatFechaVigenciaDesde,
				FechaResFin = resolucion.DatFechaVigenciaHasta,
				RangoIni = resolucion.IntRangoInicial,
				RangoFin = resolucion.IntRangoFinal,
				IdSoftware = IdSoftware,
				NitProveedor = NitProveedor,
				ClaveTecnicaDIAN = resolucion.StrClaveTecnica,
				PinSoftware = PinSoftware
			};

			FacturaE_Documento resultado = null;

			PlataformaData plataforma_datos = HgiConfiguracion.GetConfiguration().PlataformaData;

			//---Ambiente de la DIAN al que se va enviar el documento: 1 - Produccion, 2 - Pruebas
			string ambiente_dian = string.Empty;

			if (plataforma_datos.RutaPublica.Contains("app"))
				ambiente_dian = "1";
			else
				ambiente_dian = "2";

			if (TipoDocumento.Nomina.Equals(tipo_doc))
			{
				//Se valida si se es un documento con variacion para que obtenga el documento principal
				string cune_nov = string.Empty;
				if (documento.VariacionNomina == true)
				{
					Ctl_Documento ctl = new Ctl_Documento();
					List<TblDocumentos> lista_doc = ctl.ObtenerPorMes(documento.DatosEmpleador.Identificacion, Fecha.GetFecha().Month, documento.DatosTrabajador.Identificacion);

					if (lista_doc != null && lista_doc.Count > 0)
					{
						cune_nov = lista_doc.Where(x => x.IntDocTipo == TipoDocumento.Nomina.GetHashCode()).FirstOrDefault().StrCufe;
					}

				}
				resultado = HGInetUBLv2_1.NominaXML.CrearDocumento(id_seguridad, (Nomina)documento, extension_documento, tipo_doc, ambiente_dian, ref cadena_cufe, cune_nov);
			}
			else if (TipoDocumento.NominaAjuste.Equals(tipo_doc))
			{
				resultado = HGInetUBLv2_1.NominaAjusteXML.CrearDocumento(id_seguridad, (NominaAjuste)documento, extension_documento, tipo_doc, ambiente_dian, ref cadena_cufe);
			}

				

			resultado.DocumentoTipo = tipo_doc;
			resultado.IdSeguridadTercero = empresa.StrIdSeguridad;

			// genera el nombre del archivo ZIP
			resultado.NombreZip = HGInetUBLv2_1.NombramientoArchivo.ObtenerZip(documento.Documento.ToString(), documento.DatosEmpleador.Identificacion, tipo_doc, documento.Prefijo);

			return resultado;
		}

	}
}
