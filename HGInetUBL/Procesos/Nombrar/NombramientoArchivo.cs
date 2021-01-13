using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetUBL
{
    public static class NombramientoArchivo
    {
        /// <summary>
        /// Construye el nombre del archivo xml con el estándar dispuesto por la DIAN
        /// </summary>
        /// <param name="consecutivo_documento" type="string">Consecutivo del documento</param>
        /// <param name="identificacion" type="string">Identificación del obligado a facturar</param>
        /// <param name="tipo_documento" type="TipoDocumento">Tipo de documento XML que generar (Factura-NotaCredito-NotaDebido)</param>
        /// <returns>Nombre del archivo</returns>
        public static string ObtenerXml(string consecutivo_documento, string identificacion, TipoDocumento tipo_documento, string prefijo)
        {
            string nombre_archivo = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(consecutivo_documento))
                    throw new Exception("El código del documento es inválido.");

                if (string.IsNullOrWhiteSpace(identificacion))
                    throw new Exception("El NIT es inválido.");

                string hex_id_factura = string.Empty;

				try
				{
					int id_factura = Convert.ToInt32(consecutivo_documento);
					hex_id_factura = id_factura.ToString("X10");//Convierte el consecutivo de la factura en hexadecimal
				}
				catch (Exception)
				{
					long id_fac = Convert.ToInt64(consecutivo_documento);
					hex_id_factura = id_fac.ToString("X10");
				}

                identificacion = identificacion.PadLeft(10, '0');//La identificación del obligado a facturar debe contener 10 caracteres, si contiene menos lo rellena con 0 a la izquierda 

                if (tipo_documento == TipoDocumento.Factura)
                {
                    nombre_archivo = Recursos.NombreArchivos.nombre_xml_factura;
                }
                else if (tipo_documento == TipoDocumento.NotaDebito)
                {
                    nombre_archivo = Recursos.NombreArchivos.nombre_xml_nota_debito;
                }
                else if (tipo_documento == TipoDocumento.NotaCredito)
                {
                    nombre_archivo = Recursos.NombreArchivos.nombre_xml_nota_credito;
                }
                else if (tipo_documento == TipoDocumento.AcuseRecibo)
                {
                    nombre_archivo = Recursos.NombreArchivos.nombre_xml_acuse_recibo;
                }

                nombre_archivo = string.Format("{0}{1}{2}{3}", nombre_archivo, identificacion, prefijo , hex_id_factura);

                return nombre_archivo;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Construye el nombre del archivo zip con el estándar dispuesto por la DIAN
        /// </summary>
        /// <param name="consecutivo_documento" type="string">Consecutivo del documento</param>
        /// <param name="identificacion" type="string">Identificación del obligado a facturar</param>
        /// <param name="tipo_documento" type="TipoDocumento">Tipo de documento (Factura-NotaCredito-NotaDebido)</param>
        /// <returns>Nombre del archivo</returns>
        public static string ObtenerZip(string consecutivo_documento, string identificacion, TipoDocumento tipo_documento, string prefijo)
        {
            string nombre_archivo = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(consecutivo_documento))
                    throw new Exception("El código del documento es inválido.");

                if (string.IsNullOrWhiteSpace(identificacion))
                    throw new Exception("El NIT es inválido.");

                int codigo_factura = Convert.ToInt32(consecutivo_documento);

                string hex_id_factura = codigo_factura.ToString("X10");//Convierte el consecutivo de la factura en hexadecimal

                identificacion = identificacion.PadLeft(10, '0');//La identificación del obligado a facturar debe contener 10 caracteres, si contiene menos lo rellena con 0 a la izquierda 

                if (tipo_documento == TipoDocumento.Factura)
                {
                    nombre_archivo = Recursos.NombreArchivos.nombre_zip_factura;
                }
                else if (tipo_documento == TipoDocumento.NotaDebito)
                {
                    nombre_archivo = Recursos.NombreArchivos.nombre_zip_nota_debito;
                }
                else if (tipo_documento == TipoDocumento.NotaCredito)
                {
                    nombre_archivo = Recursos.NombreArchivos.nombre_zip_nota_credito;
                }

                nombre_archivo = string.Format("{0}{1}{2}{3}", nombre_archivo, identificacion, prefijo, hex_id_factura);

                return nombre_archivo;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }
    }
}
