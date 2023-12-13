using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public enum CamposSalud
	{
		[Description("CODIGO_PRESTADOR")]
		prestadorservicios = 1,

		//[Description("TIPO_DOCUMENTO_IDENTIFICACION")]
		//tipodocumento = 2,

		//[Description("NUMERO_DOCUMENTO_IDENTIFICACION")]
		//numerodocumento = 3,

		//[Description("PRIMER_APELLIDO")]
		//primerapellidousuario = 4,

		//[Description("SEGUNDO_APELLIDO")]
		//segundoapellidousuario = 5,

		//[Description("PRIMER_NOMBRE")]
		//primernombreusuario = 6,

		//[Description("SEGUNDO_NOMBRE")]
		//segundonombreusuario = 7,

		//[Description("TIPO_USUARIO")]
		//tipousuario = 8,

		[Description("MODALIDAD_PAGO")]
		RecepcionAcuse = 2,

		[Description("COBERTURA_PLAN_BENEFICIOS")]
		coberturaplanbeneficios = 3,

		[Description("NUMERO_AUTORIZACION")]
		numeroautorizacion = 4,

		//[Description("NUMERO_MIPRES")]
		//numeromipres = 12,

		//[Description("NUMERO_ENTREGA_MIPRES")]
		//numeroidmipres = 13,

		//[Description("NUMERO_CONTRATO")]
		//numerocontrato = 14,

		[Description("NUMERO_POLIZA")]
		numeropoliza = 5,

		[Description("COPAGO")]
		copago = 6,

		[Description("CUOTA_MODERADORA")]
		cuotamoderadora = 7,

		[Description("CUOTA_RECUPERACION")]
		cuotarecuperacion = 8,

		[Description("PAGOS_COMPARTIDOS")]
		pagoscompartidos = 9,

		[Description("FECHA_INICIO")]
		fechainicio = 10,

		[Description("FECHA_FIN")]
		fechafin = 11
	}

	public enum TipoIdentificacionSalud
	{
		[Description("Cédula de ciudadanía")]
		CC = 1,

		[Description("Cédula de extranjería")]
		CE = 2,

		[Description("Carné diplomático")]
		CD = 3,

		[Description("Pasaporte")]
		PA = 4,

		[Description("Salvoconducto")]
		SC = 5,

		[Description("Permiso especial de permanencia")]
		PE = 6,

		[Description("Registro civil de nacimiento")]
		RC = 7,

		[Description("Tarjeta de identidad")]
		TI = 8,

		[Description("Certificado de nacido vivo")]
		CN = 9,

		[Description("Adulto sin identificar")]
		AS = 10,

		[Description("Menor sin identificar")]
		MS = 11,

		[Description("Documento extranjero")]
		DE = 12,

		[Description("Sin identificación")]
		SI = 13,
	}

	public enum TipoUsuarioSalud
	{
		[Description("Contributivo cotizante")]
		[AmbientValue("01")]
		ContributivoC = 01,

		[Description("Contributivo beneficiario")]
		[AmbientValue("02")]
		ContributivoB = 2,

		[Description("Contributivo adicional")]
		[AmbientValue("03")]
		ContributivoA = 3,

		[Description("Subsidiado")]
		[AmbientValue("04")]
		Subsidiado = 4,

		[Description("Sin régimen")]
		[AmbientValue("05")]
		SinRegimen = 5,

		[Description("Especiales o de Excepción cotizante")]
		[AmbientValue("06")]
		EspecialesC = 6,

		[Description("Especiales o de Excepción beneficiario")]
		[AmbientValue("07")]
		EspecialesB = 7,

		[Description("Particular")]
		[AmbientValue("08")]
		Particular = 8,

		[Description("Tomador/Amparado ARL")]
		[AmbientValue("09")]
		TomadorA = 9,

		[Description("Tomador/Amparado SOAT")]
		[AmbientValue("10")]
		TomadorS = 10,

		[Description("Tomador/Amparado Planes voluntarios de salud")]
		[AmbientValue("11")]
		TomadorP = 11,
	}

	public enum CoberturaSalud
	{
		[Description("Plan de beneficios en salud financiado con UPC")]
		[AmbientValue("01")]
		Plan = 01,

		[Description("Presupuesto máximo")]
		[AmbientValue("02")]
		Presupuesto = 2,

		[Description("Prima EPS/EOC, no asegurados SOAT")]
		[AmbientValue("03")]
		Prima = 3,

		[Description("Cobertura Póliza SOAT")]
		[AmbientValue("04")]
		CoberturaP = 4,

		[Description("Cobertura ARL")]
		[AmbientValue("05")]
		CoberturaA = 5,

		[Description("Cobertura ADRES")]
		[AmbientValue("06")]
		CoberturaAD = 6,

		[Description("Cobertura Salud Pública")]
		[AmbientValue("07")]
		CoberturaS = 7,

		[Description("Cobertura entidad territorial, recursos de oferta")]
		[AmbientValue("08")]
		CoberturaE = 8,

		[Description("Urgencias población migrante")]
		[AmbientValue("09")]
		Urgencias = 9,

		[Description("Plan complementario en salud")]
		[AmbientValue("10")]
		PlanC = 10,

		[Description("Plan medicina prepagada")]
		[AmbientValue("11")]
		PlanM = 11,

		[Description("Otras pólizas en salud")]
		[AmbientValue("12")]
		Otras = 12,

		[Description("Cobertura Régimen Especial o Excepción")]
		[AmbientValue("13")]
		CoberturaR = 13,

		[Description("Cobertura Fondo Nacional de Salud de las Personas Privadas de la Libertad")]
		[AmbientValue("14")]
		CoberturaF = 14,

		[Description("Particular")]
		[AmbientValue("15")]
		Particular = 15
	}

	public enum ModalidadDePago
	{
		[Description("Pago individual por caso / Conjunto integral de atenciones / Paquete / Canasta")]
		[AmbientValue("01")]
		ContributivoC = 1,

		[Description("Pago global prospectivo")]
		[AmbientValue("02")]
		ContributivoB = 2,

		[Description("Pago por capitación")]
		[AmbientValue("03")]
		ContributivoA = 3,

		[Description("Pago por evento")]
		[AmbientValue("04")]
		Subsidiado = 4,

		[Description("Otra modalidad (específica)")]
		[AmbientValue("05")]
		SinRegimen = 5
	}


}
