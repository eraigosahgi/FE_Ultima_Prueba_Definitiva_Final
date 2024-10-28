﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{
	public class Nomina
	{

		/// <summary>
		/// Código de seguridad (autenticación)
		/// </summary>
		public string DataKey { get; set; }

		/// <summary>
		/// Identificador del Documento asigando por el Facturador Electrónico
		/// </summary>
		public string CodigoRegistro { get; set; }

		/// <summary>
		/// Indica si esa nomina presenta variacion(False - Es compleata por el Mes, True - No esta completa por el mes y solo representa una parte de lo liquidado al empleado)
		/// </summary>
		public bool VariacionNomina { get; set; }

		/// <summary>
		/// Prefijo de la Nómina, depende de las sucursales que posea el Empleador y elegido por este.
		/// </summary>
		public string Prefijo { get; set; }

		/// <summary>
		/// Número del Documento
		/// </summary>
		public long Documento { get; set; }

		/// <summary>
		/// Número de Resolución del Documento generada por la plataforma.
		/// </summary>
		public string NumeroResolucion { get; set; }

		/// <summary>
		/// Código Único de Documento Soporte de Pago de Nómina Electrónica. Elemento que verifica la integridad de la información recibida</summary>
		public string Cune { get; set; }

		/// <summary>
		/// Debe ir la fecha de emision del documento. Considerando zona horaria de Colombia (-5), en formato yyyy-MM-dd HH:mm:ss.
		/// </summary>
		public DateTime FechaGen { get; set; }

		/// <summary>
		/// Corresponde al Código de Periodo de Nómina de la tabla 5.5.1.1.(Semanal, quincenal...)
		/// </summary>
		public int PeriodoNomina { get; set; }

		/// <summary>
		/// Código de la moneda según tabla ISO 4217 (ej: COP = Pesos Colombianos).
		/// </summary>
		public string Moneda { get; set; }

		/// <summary>
		/// Notas (observaciones) del documento
		/// </summary>
		public List<string> Notas { get; set; }

		/// <summary>
		/// valor Total de Todos los Devengados del Trabajador
		/// </summary>
		public decimal DevengadosTotal { get; set; }


		/// <summary>
		/// valor Total de Todos las Deducciones del Trabajador
		/// </summary>
		public decimal DeduccionesTotal { get; set; }

		/// <summary>
		/// la Diferencia entre DevengadosTotal - DeduccionesTotal
		/// </summary>
		public decimal ComprobanteTotal { get; set; }

		/// <summary>
		/// Lista de objeto tipo Datetime para informar las fechas de pago del documento. Considerando zona horaria de Colombia (‐5), en formato AAAA‐MM‐DD  Ocurrencia 1-N
		/// </summary>
		public List<DateTime> FechasPagos { get; set; }

		/// <summary>
		/// Datos del periodo liquidado Ocurrencia 1-1
		/// </summary>
		public Periodo DatosPeriodo { get; set; }

		/// <summary>
		/// Datos del Empleador Ocurrencia 1-1
		/// </summary>
		public Empleador DatosEmpleador { get; set; }

		/// <summary>
		/// Datos del Empleador Ocurrencia 1-1
		/// </summary>
		public Trabajador DatosTrabajador { get; set; }

		/// <summary>
		/// Datos de la forma de Pago del documento. Ocurrencia 1-1
		/// </summary>
		public Pago DatosPago { get; set; }

		/// <summary>
		/// Datos de los devengados del trabajador liquidados en el documento. Ocurrencia 1-1
		/// </summary>
		public Devengados DatosDevengados { get; set; }

		/// <summary>
		/// Datos de las Deducciones del trabajador liquidados en el documento. Ocurrencia 1-1
		/// </summary>
		public Deducciones DatosDeducciones { get; set; }

		/// <summary>
		/// Tasa de cambio de moneda extranjera
		/// </summary>
		public TasaCambio Trm { get; set; }

		/// <summary>
		/// Id de seguridad del plan de donde se va a descontar el presente documento
		/// </summary>
		public Guid IdPlan { get; set; }

		/// <summary>
		/// Datos del formato del documento
		/// </summary>
		public Formato DocumentoFormato { get; set; }

		/// <summary>
		/// Version de la aplicacion donde fue generado el documento
		/// </summary>
		public string VersionAplicativo { get; set; }

		/// <summary>
		/// Identificacion de la empresa generadora (Integrador) del documento
		/// </summary>
		public string IdentificacionIntegrador { get; set; }



	}
}
