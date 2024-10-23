using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Pago
	{

		/// <summary>
		/// Formas de Pago del Documento. Código de la tabla 5.3.3.1. (1 - Contado)
		/// </summary>
		public int Forma { get; set; }

		/// <summary>
		/// Métodos de Pago del Documento. Código de la tabla 5.3.3.2. (ListaMediosPago de la DIAN FacturaE)
		/// </summary>
		public int Metodo { get; set; }

		/// <summary>
		/// Descripcion del Medio de Pago segun el codigo recibido - Es llenado por la plataforma para la generacion de Formatos
		/// </summary>
		public string TerminoPago_Descripcion { get; set; }

		/// <summary>
		/// Se debe colocar el nombre de la entidad bancaria donde el trabajador tiene su cuenta para pago de nómina. Si el Método de Pago se realiza de forma Bancaria, este campo es obligatorio. Ocurrencia 0-1. (ERP - Entidad financiera maestro de empleados)
		/// </summary>
		public string Banco { get; set; }

		/// <summary>
		/// Se debe colocar el tipo de cuenta que el trabajador tiene para pago de nómina. Si el Método de Pago se realiza de forma Bancaria, este campo es obligatorio. Ocurrencia 0-1. (string ahorros o corriente)
		/// </summary>
		public string TipoCuenta { get; set; }

		/// <summary>
		/// Se debe colocar el número de la cuenta que el trabajador tiene para pago de nómina. Si el Método de Pago se realiza de forma Bancaria, este campo es obligatorio. Ocurrencia 0-1.
		/// </summary>
		public string NumeroCuenta { get; set; }

	}
}
