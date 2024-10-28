using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBLv2_1
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

				//La gerencia de la compañia solicita que no sea en hexadecimal si no los primeros 8 numero del documento 2021-11-05
				//int id_factura = Convert.ToInt32(consecutivo_documento);
				//string hex_id_factura = id_factura.ToString("X10");//Convierte el consecutivo de la factura en hexadecimal

				string numero_documento = consecutivo_documento.PadLeft(8, '0');

				identificacion = identificacion.PadLeft(10, '0');//La identificación del obligado a facturar debe contener 10 caracteres, si contiene menos lo rellena con 0 a la izquierda 

				//Anexo Tecnico V1.8 Seccion 6.9. Códigos de asignación
				string codigo_dian_asignado_PT = "028";

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
				else if (tipo_documento == TipoDocumento.Nomina)
				{
					nombre_archivo = "nie";
				}
				else if (tipo_documento == TipoDocumento.NominaAjuste)
				{
					nombre_archivo = "niae";
				}
				else if (tipo_documento == TipoDocumento.Attached)
				{
					nombre_archivo = "ad";
				}

				nombre_archivo = string.Format("{0}{1}{2}{3}{4}{5}", nombre_archivo, identificacion, codigo_dian_asignado_PT, Fecha.GetFecha().ToString("yy"), prefijo, numero_documento);

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

				//La gerencia de la compañia solicita que no sea en hexadecimal si no los primeros 8 numero del documento 2021-11-05
				//int id_factura = Convert.ToInt32(consecutivo_documento);
				//string hex_id_factura = id_factura.ToString("X10");//Convierte el consecutivo de la factura en hexadecimal

				string numero_documento = consecutivo_documento.PadLeft(8, '0');

				identificacion = identificacion.PadLeft(10, '0');//La identificación del obligado a facturar debe contener 10 caracteres, si contiene menos lo rellena con 0 a la izquierda 

				//Anexo Tecnico V1.8 Seccion 6.9. Códigos de asignación
				string codigo_dian_asignado_PT = "028";

				nombre_archivo = Recursos.NombreArchivos.nombre_zip_factura;

				//if (tipo_documento == TipoDocumento.Factura)
				//{
				//	nombre_archivo = Recursos.NombreArchivos.nombre_zip_factura;
				//}
				//else if (tipo_documento == TipoDocumento.NotaDebito)
				//{
				//	nombre_archivo = Recursos.NombreArchivos.nombre_zip_nota_debito;
				//}
				//else if (tipo_documento == TipoDocumento.NotaCredito)
				//{
				//	nombre_archivo = Recursos.NombreArchivos.nombre_zip_nota_credito;
				//}
				//else if (tipo_documento == TipoDocumento.Nomina || tipo_documento == TipoDocumento.NominaAjuste)
				//{
				//	nombre_archivo = Recursos.NombreArchivos.nombre_zip_nomina;
				//}

				nombre_archivo = string.Format("{0}{1}{2}{3}{4}{5}", nombre_archivo, identificacion, codigo_dian_asignado_PT, Fecha.GetFecha().ToString("yy"), prefijo, numero_documento);

				return nombre_archivo;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}
