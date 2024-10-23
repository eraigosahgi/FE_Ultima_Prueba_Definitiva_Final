using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Trabajador
	{

		/// <summary>
		/// Corresponde a la clasificación de PILA para conocer en qué calidad se realizan las cotizaciones a la seguridad social. Se debe colocar el Código de la tabla 5.5.1.3. (ERP - contratos tipo cotizante)
		/// </summary>
		public string TipoTrabajador { get; set; }

		/// <summary>
		/// Corresponde a una sub clasificación de PILA para conocer en qué calidad se realizan las cotizaciones a la seguridad social. Se debe colocar el Código de la tabla 5.5.1.4. (ERP - Empleado subtipo cot)
		/// </summary>
		public string SubTipoTrabajador { get; set; }

		/// <summary>
		/// Si el trabajador desarrollo durante el presente periodo alguna de las actividades descritas en el Decreto 2090 de 2003. Se debe colocar \"true\" o \"false\".  (Riesgo afp maestro de empleados)
		/// </summary>
		public bool AltoRiesgoPension { get; set; }

		/// <summary>
		/// Tipo de identificación: 13-Cedula, 22-Cedula Extranjeria, 31-NIT
		/// Tipo de documento de identificación que actualmente tiene el trabajador o aprendiz. Código de la tabla 5.2.1.
		/// </summary>
		public int TipoDocumento { get; set; }

		/// <summary>
		/// Debe ir el Numero de documento del trabajador, sin puntos ni comas ni espacios.
		/// </summary>
		public string Identificacion { get; set; }

		/// <summary>
		/// Debe ir el Primer Apellido del trabajador.
		/// </summary>
		public string PrimerApellido { get; set; }

		/// <summary>
		/// Debe ir el Segundo Apellido del trabajador.
		/// </summary>
		public string SegundoApellido { get; set; }

		/// <summary>
		/// Debe ir el Primer Nombre del trabajador.
		/// </summary>
		public string PrimerNombre { get; set; }

		/// <summary>
		/// Deben ir los Otros Nombres del trabajador. Ocurrencia 0-1.
		/// </summary>
		public string OtrosNombres { get; set; }

		/// <summary>
		/// Telefono
		/// </summary>
		public string Telefono { get; set; }

		/// <summary>
		/// Correo 
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Código del país actual donde se encontraba ubicado el trabajador o aprendiz en el mes reportado. Alfa-2 de la tabla 5.4.1. (ej: CO = Colombia, etc.)
		/// </summary>
		public string LugarTrabajoPais { get; set; }

		/// <summary>
		/// Nombre del país, lo llena la plataforma y para uso de formatos (ej: CO = Colombia, etc.)
		/// </summary>
		public string PaisNombre { get; set; }

		/// <summary>
		/// Código del departamento actual donde se encontraba ubicado el trabajador o aprendiz en el mes reportado. De la tabla 5.4.2.
		/// </summary>
		public string LugarTrabajoDepartamentoEstado { get; set; }

		/// <summary>
		/// Nombre del departamento,  lo llena la plataforma y para uso de formatos. El Código de la tabla 5.4.2.
		/// </summary>
		public string DepartamentoNombre { get; set; }

		/// <summary>
		/// Código del municipio o ciudad actual donde se encontraba ubicado el trabajador o aprendiz en el mes reportado. De la tabla 5.4.3.
		/// </summary>
		public string LugarTrabajoMunicipioCiudad { get; set; }

		/// <summary>
		/// Nombre del municipio o ciudad,  lo llena la plataforma y para uso de formatos, referencia tabla 5.4.3.
		/// </summary>
		public string CiudadNombre { get; set; }

		/// <summary>
		/// Debe corresponder a la dirección del lugar físico donde vive el empleado.
		/// </summary>
		public string LugarTrabajoDireccion { get; set; }

		/// <summary>
		/// Si el trabajador tiene un salario integral, el cual es el tipo de remuneración que incluye todos los conceptos que puedan constituir salario en un solo monto o pago (prestaciones sociales y recargos nocturno, dominical y festivo, y el trabajo extra) y que sea superior a 10 SMLMV más un 30% correspondiente a factor prestacional. Se debe colocar \"true\" o \"false\".
		/// </summary>
		public bool SalarioIntegral { get; set; }

		/// <summary>
		/// Tipo de Contrato que posee el empleado con el Empleador. Código de la tabla 5.5.1.2.
		/// 1 Termino Fijo - 2 Término Indefinido - 3 Obra o Labor - 4 Aprendizaje - 5 Prácticas o Pasantías
		/// </summary>
		public int TipoContrato { get; set; }

		/// <summary>
		/// Corresponde al valor que el empleador paga de forma periódica al trabajador como contraprestación por el trabajo realizado, este puede ser fijo o variable de acuerdo a la unidad de tiempo en que las partes hayan acordado el pago, teniendo como base el día o la hora trabajada. Se debe colocar el Sueldo Base que el Trabajador tiene en la empresa.
		/// </summary>
		public decimal Sueldo { get; set; }


		/// <summary>
		/// Código del Trabajador. Campo Opcional queda a manejo Interno del Empleador. Ocurrencia 0-1. (ERP - Validación parámetro general código alterno)
		/// </summary>
		public string CodigoTrabajador { get; set; }


	}
}
