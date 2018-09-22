using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using HGInetUBL.Objetos;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectonicaData.Enumerables;

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
        public static FacturaE_Documento Generar(Guid id_documento, Factura documento, TipoDocumento tipo_doc, TblEmpresas empresa, TblEmpresasResoluciones resolucion)
        {

            // resolución del documento
            HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();

            // obtiene los datos del proveedor tecnológico de la DIAN
            DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

            // obtiene los datos de prueba del proveedor tecnológico de la DIAN
            DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

            string IdSoftware = data_dian.IdSoftware;
            string PinSoftware = data_dian.Pin;
            string NitProveedor = data_dian.NitProveedor;

            // sobre escribe los datos de la resolución si se encuentra en estado de habilitación
            if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
            {   IdSoftware = data_dian_habilitacion.IdSoftware;
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
            
            // convierte el documento 
            FacturaE_Documento resultado = FacturaXML.CrearDocumento(id_documento, documento, extension_documento, tipo_doc);
            resultado.DocumentoTipo = tipo_doc;
			resultado.IdSeguridadDocumento = id_documento;
            resultado.IdSeguridadTercero = empresa.StrIdSeguridad;

            // genera el nombre del archivo ZIP
            resultado.NombreZip = NombramientoArchivo.ObtenerZip(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo_doc);

            return resultado;
        }

        /// <summary>
        /// Genera la información del documento en formato UBL
        /// </summary>
        /// <param name="id_seguridad">id único de identificación de la plataforma</param>
        /// <param name="documento">datos del documento</param>
        /// <param name="pruebas">indica si el documento es de pruebas (true)</param>
        /// <returns>datos del documento</returns>
        public static FacturaE_Documento Generar(Guid id_seguridad, NotaCredito documento, TipoDocumento tipo_doc, TblEmpresas empresa, TblEmpresasResoluciones resolucion)
        {
            // resolución del documento
            HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();

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


            // convierte el documento 
            FacturaE_Documento resultado = NotaCreditoXML.CrearDocumento(id_seguridad, documento, extension_documento, tipo_doc);
            resultado.DocumentoTipo = tipo_doc;
            resultado.IdSeguridadTercero = empresa.StrIdSeguridad;

            // genera el nombre del archivo ZIP
            resultado.NombreZip = NombramientoArchivo.ObtenerZip(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo_doc);

            return resultado;
        }

        /// <summary>
        /// Genera la información del documento en formato UBL
        /// </summary>
        /// <param name="id_documento">id único de identificación de la plataforma</param>
        /// <param name="documento">datos del documento</param>
        /// <param name="pruebas">indica si el documento es de pruebas (true)</param>
        /// <returns>datos del documento</returns>
        public static FacturaE_Documento Generar(Guid id_documento, NotaDebito documento, TipoDocumento tipo_doc, TblEmpresas empresa, TblEmpresasResoluciones resolucion)
        {
			// resolución del documento
			HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian();

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


			// convierte el documento 
			FacturaE_Documento resultado = NotaDebitoXML.CrearDocumento(id_documento, documento, extension_documento, tipo_doc);
			resultado.DocumentoTipo = tipo_doc;
			resultado.IdSeguridadTercero = empresa.StrIdSeguridad;

			// genera el nombre del archivo ZIP
			resultado.NombreZip = NombramientoArchivo.ObtenerZip(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo_doc);

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
                // valida el nodo de ExtensionContent
                documento.DocumentoXml = HGInetUBL.ExtensionDian.ValidarNodo(documento.DocumentoXml);

                string id_obligado = string.Empty;

                switch (documento.DocumentoTipo)
                {
                    case TipoDocumento.Factura:
                        Factura doc_factura = ((Factura)documento.Documento);
                        id_obligado = documento.IdSeguridadTercero.ToString();
                        break;
                    case TipoDocumento.NotaCredito:
                        NotaCredito doc_nota_credito = ((NotaCredito)documento.Documento);
						id_obligado = documento.IdSeguridadTercero.ToString();
                        break;
                    case TipoDocumento.NotaDebito:
                        NotaDebito doc_nota_debito = ((NotaDebito)documento.Documento);
						id_obligado = documento.IdSeguridadTercero.ToString();
						
                        break;
                    case TipoDocumento.AcuseRecibo:
                        Acuse doc_acuse_recibo = ((Acuse)documento.Documento);
                        id_obligado = documento.IdSeguridadTercero.ToString();

                        break;

                    default:
                        break;
                }


                // carpeta del xml
                string carpeta_xml = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), id_obligado);
                carpeta_xml = string.Format(@"{0}{1}", carpeta_xml, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaXmlFacturaE);

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
                string carpeta_zip = LibreriaGlobalHGInet.Dms.ObtenerCarpetaPrincipal(Directorio.ObtenerDirectorioRaiz(), id_obligado);
                carpeta_zip = string.Format(@"{0}{1}", carpeta_zip, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian);

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

    }
}
