using HGInetZonaPagos.ZonaVirtualServicioPagos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetZonaPagos
{
	/// <summary>
	/// Información del pago
	/// </summary>
	public class Pago
	{
		/// <summary>
		/// Identificador del pago, puede ser el número de factura o algún valor con el cual identificar el pago o transacción en el futuro. 
		/// Tamaño máximo: 30
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio.")]
		[StringLength(30, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string id_pago;

		/// <summary>
		/// Concepto o descripción del pago que realizará el cliente
		/// Tamaño máximo: 70
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio.")]
		[StringLength(70, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string descripcion_pago;

		/// <summary>
		/// Total a pagar con IVA incluido Valor numérico, máximo 2 decimales
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio.")]
		public double total_con_iva;

		/// <summary>
		/// Total IVA Valor numérico, máximo 2 decimales. Si no se utiliza IVA, se debe enviar cero.
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio.")]
		public double valor_iva;

		/// <summary>
		/// Campo con información opcional 1
		/// Tamaño máximo: 70
		/// </summary>
		[StringLength(70, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string info_opcional1;

		/// <summary>
		/// Campo con información opcional 2
		/// Tamaño máximo: 70
		/// </summary>
		[StringLength(70, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string info_opcional2;

		/// <summary>
		/// Campo con información opcional 3
		/// Tamaño máximo: 70
		/// </summary>
		[StringLength(70, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string info_opcional3;

		/// <summary>
		/// Caso Multicrédito: es el código de servicio principal o padre.
		/// Caso Normal: código de servicio estándar.
		/// Importante: este código de servicio debe estar previamente creado tanto en PSE como en Zona Pagos. 
		/// </summary>
		[Required(ErrorMessage = "{0} es un campo obligatorio.")]
		public string codigo_servicio_principal;

		/// <summary>
		/// Caso Multicrédito: contiene la lista de todos los códigos de servicio multicrédito hijos del código de servicio principal.
		///	Caso Normal: No se llena, se envía nulo este parámetro.
		///	Importante: estos códigos de servicio debe estar previamente creados tanto en PSE como en Zona Pagos
		///	Es obligatorio en caso de ser multicrédito. 
		/// </summary>
		public string[] lista_codigos_servicio_multicredito;

		/// <summary>
		/// Caso Multicrédito: Contiene la lista de todos los Nit correspondientes a los códigos de servicio multicrédito Enviados
		///	Caso Normal: no se llena, se envía nulo este parámetro.
		///	Importante: estos códigos de servicio debe estar previamente creados tanto en PSE como en Zona Pagos
		///	Es obligatorio en caso de ser multicrédito. 
		/// </summary>
		public string[] lista_nit_codigos_servicio_multicredito;

		/// <summary>
		/// Caso Multicrédito: Contiene la lista de todos los valores a pagar de cada servicio multicrédito enviado.
		/// Las suma de todos los valores enviado debe ser igual al parametro enviado “total_con_iva”
		///	Caso Normal: no se llena, se envía nulo este parámetro.
		///	Es obligatorio en caso de ser multicrédito. 
		/// </summary>
		public double[] lista_valores_con_iva;

		/// <summary>
		/// Caso Multicrédito: Contiene la lista de los valores del IVA a pagar de cada servicio multicrédito enviado.
		/// Las suma de todos los valores enviado debe ser igual al parámetro enviado “valor_iva”. Si no se maneja IVA se debe enviar cero.
		///	Caso Normal: no se llena, se envía nulo este parámetro.
		///	Es obligatorio en caso de ser multicrédito. 
		/// </summary>
		public double[] lista_valores_iva;


		/// <summary>
		/// Caso Multicrédito: Es la suma del número de códigos de servicio multicrédito que se enviaron (Sin contar el código de servicio principal)
		///	Caso Normal: Se envía 0 (cero) este parámetro.
		///	Es obligatorio en caso de ser multicrédito. 
		/// </summary>
		public int total_codigos_servicio;

		/// <summary>
		/// Conversion de datos lista de string al objeto del servicio
		/// </summary>
		/// <param name="datos">lista</param>
		/// <returns>ArrayOfString</returns>
		public static ArrayOfString ConvertirArrayString(string[] datos)
		{
			try
			{
				if (datos == null)
					return null;

				ArrayOfString datos_conversion = new ArrayOfString();
				datos_conversion.AddRange(datos);
				return datos_conversion;
			}
			catch (Exception excepcion)
			{
				throw;
			}
		}

		/// <summary>
		/// Conversion de datos lista de double al objeto del servicio
		/// </summary>
		/// <param name="datos">lista</param>
		/// <returns>ArrayOfDouble</returns>
		public static ArrayOfDouble ConvertirArrayDouble(double[] datos)
		{
			try
			{
				if (datos == null)
					return null;

				ArrayOfDouble datos_conversion = new ArrayOfDouble();
				datos_conversion.AddRange(datos);
				return datos_conversion;
			}
			catch (Exception excepcion)
			{
				throw;
			}
		}

	}
}