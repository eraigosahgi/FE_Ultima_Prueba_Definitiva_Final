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
		[Description("Código del prestador de servicios de salud")]
		prestadorservicios = 1,

		[Description("Tipo de documento de identificación del usuario")]
		tipodocumento = 2,

		[Description("Número de documento de identificación del usuario")]
		numerodocumento = 3,

		[Description("Primer apellido del usuario")]
		primerapellidousuario = 4,

		[Description("Segundo apellido del usuario")]
		segundoapellidousuario = 5,

		[Description("Primer nombre del usuario")]
		primernombreusuario = 6,

		[Description("Otros Nombres")]
		segundonombreusuario = 7,

		[Description("Tipo de usuario")]
		tipousuario = 8,

		[Description("Modalidades de contratación y de pago")]
		RecepcionAcuse = 9,

		[Description("Cobertura o plan de beneficios")]
		coberturaplanbeneficios = 10,

		[Description("Número de autorización")]
		numeroautorizacion = 11,

		[Description("Número de mi prescripción (MIPRES)")]
		numeromipres = 12,

		[Description("Número de ID de suministro mi prescripción (MIPRES)")]
		numeroidmipres = 13,

		[Description("Número de contrato")]
		numerocontrato = 14,

		[Description("Número de póliza")]
		numeropoliza = 15,

		[Description("Fecha de inicio")]
		fechainicio = 16,

		[Description("Fecha final")]
		fechafin = 17,

		[Description("Copago")]
		copago = 18,

		[Description("Cuota moderadora")]
		cuotamoderadora = 19,

		[Description("Cuota de recuperación")]
		cuotarecuperacion = 20,

		[Description("Pagos compartidos en planes voluntarios de salud")]
		PrevalidacionErrorDian = 21
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


}
