using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HGInetZonaPagos
{
	/// <summary>
	/// Información del cliente
	/// </summary>
	public class Cliente
	{
		/// <summary>
		/// Cédula, Nit o código de identificación
		/// Tamaño máximo: 30
		/// </summary>
		[StringLength(30, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string id_cliente;

		/// <summary>
		/// Tipo identificación del cliente
		/// Los Valores que se deben enviar son:
		/// 0 No se usa o Tipo no Identificado
		/// 1 CC Cedula de Ciudadanía
		/// 2 CE Cedula de Extranjería
		/// 3 NIT Nit Empresa
		/// 4 NUIP Número Único de Identificación
		/// 5 TI Tarjeta de Identidad
		/// 6 PP Pasaporte
		/// 7 IDC Identificador Único del Cliente
		/// 8 CEL En caso de que el identificador sea un número móvil o celular
		/// 9 RC Registro Civil de Nacimiento
		/// 10 DE Documento de Identificación Extranjero
		/// 11 Otro no tipificado
		/// Tamaño máximo: 5
		/// </summary>
		[StringLength(5, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string tipo_id;

		/// <summary>
		/// Nombre
		/// Tamaño máximo: 50
		/// </summary>
		[StringLength(50, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string nombre;

		/// <summary>
		/// Apellido
		/// Tamaño máximo: 50
		/// </summary>
		[StringLength(50, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string apellido;

		/// <summary>
		/// Teléfono
		/// Tamaño máximo: 50
		/// </summary>
		[StringLength(50, ErrorMessage = "La propiedad {0} debe tener {1} caracteres máximo.")]
		public string telefono;

		/// <summary>
		/// Correo electrónico del cliente
		/// Tamaño máximo: 70
		/// </summary>
		[StringLength(70, ErrorMessage="La propiedad {0} debe tener {1} caracteres máximo.")]
		public string email { get; set; }

	}
}
