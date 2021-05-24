using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBLv2_1
{
	public partial class NominaXML
	{

		public static FacturaE_Documento CrearDocumento(Guid id_documento, Nomina documento, HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion, TipoDocumento tipo, string ambiente_dian, ref string cadena_cufe)
		{
			try
			{
				if (documento == null)
					throw new Exception("La documento es inválido.");

				//Obtiene el nombre del archivo XML
				string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosEmpleador.Identificacion, tipo, documento.Prefijo);


				NominaIndividualType nomina = new NominaIndividualType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.ObtenerNamespaces(tipo);

				//******validar esta variable
				nomina.Novedad = new NominaIndividualTypeNovedad();
				nomina.Novedad.Value = false;
				nomina.Novedad.CUNENov = "";

				#region nnomina.NumeroSecuenciaXML //Número de documento: Número de documento nomina.
				string numero_documento = "";
				if (!string.IsNullOrEmpty(documento.Prefijo))
					numero_documento = string.Format("{0}", documento.Prefijo);

				numero_documento = string.Format("{0}{1}", numero_documento, documento.Documento.ToString());
				nomina.NumeroSecuenciaXML = new NominaIndividualTypeNumeroSecuenciaXML();
				nomina.NumeroSecuenciaXML.Numero = numero_documento;
				nomina.NumeroSecuenciaXML.Prefijo = documento.Prefijo;
				nomina.NumeroSecuenciaXML.Consecutivo = documento.DatosTrabajador.Identificacion;

				nomina.NumeroSecuenciaXML.CodigoTrabajador = documento.DatosTrabajador.CodigoTrabajador;

				#endregion

				nomina.Periodo = new NominaIndividualTypePeriodo();
				nomina.Periodo.FechaGen = Convert.ToDateTime(documento.FechaGen.ToString(Fecha.formato_fecha_hginet));
				nomina.Periodo.FechaIngreso = Convert.ToDateTime(documento.DatosPeriodo.FechaIngreso.ToString(Fecha.formato_fecha_hginet));
				nomina.Periodo.FechaRetiro = Convert.ToDateTime(documento.DatosPeriodo.FechaRetiro.ToString(Fecha.formato_fecha_hginet));
				nomina.Periodo.FechaLiquidacionInicio = Convert.ToDateTime(documento.DatosPeriodo.FechaLiquidacionInicio.ToString(Fecha.formato_fecha_hginet));
				nomina.Periodo.FechaLiquidacionFin = Convert.ToDateTime(documento.DatosPeriodo.FechaLiquidacionFin.ToString(Fecha.formato_fecha_hginet));
				nomina.Periodo.FechaRetiroSpecified = false;
				nomina.Periodo.TiempoLaborado = documento.DatosPeriodo.TiempoLaborado.ToString();

				nomina.LugarGeneracionXML = new NominaIndividualTypeLugarGeneracionXML();
				nomina.LugarGeneracionXML.Idioma = "es";
				nomina.LugarGeneracionXML.Pais = documento.DatosEmpleador.Pais;
				nomina.LugarGeneracionXML.DepartamentoEstado = documento.DatosEmpleador.DepartamentoEstado;
				nomina.LugarGeneracionXML.MunicipioCiudad = documento.DatosEmpleador.MunicipioCiudad;

				nomina.InformacionGeneral = new NominaIndividualTypeInformacionGeneral();
				nomina.InformacionGeneral.Ambiente = ambiente_dian;
				nomina.InformacionGeneral.FechaGen = Convert.ToDateTime(documento.FechaGen.ToString(Fecha.formato_fecha_hginet));
				nomina.InformacionGeneral.HoraGen = Convert.ToDateTime(documento.FechaGen.ToString(Fecha.formato_hora_zona));
				nomina.InformacionGeneral.PeriodoNomina = documento.PeriodoNomina.ToString();
				nomina.InformacionGeneral.TipoMoneda = documento.Moneda;
				nomina.InformacionGeneral.TipoXML = "102";
				nomina.InformacionGeneral.Version = "V1.0: Documento Soporte de Pago de Nómina Electrónica";
				if (!documento.Moneda.Equals("COP") && documento.Trm != null)
					nomina.InformacionGeneral.TRM = documento.Trm.Valor;
				
				if (documento.Notas != null && documento.Notas.Count > 0)
					nomina.Notas = documento.Notas.ToArray();

				nomina.Empleador = ParticipantesNominaXML.ObtenerEmpleador(documento.DatosEmpleador);

				nomina.Trabajador = ParticipantesNominaXML.ObtenerTrabajador(documento.DatosTrabajador);

				nomina.Pago = new NominaIndividualTypePago();
				nomina.Pago.Metodo = documento.DatosPago.Metodo.ToString();
				nomina.Pago.Forma = documento.DatosPago.Forma.ToString();
				nomina.Pago.NumeroCuenta = documento.DatosPago.NumeroCuenta;
				nomina.Pago.TipoCuenta = documento.DatosPago.TipoCuenta;
				nomina.Pago.Banco = documento.DatosPago.Banco;

				nomina.FechasPagos = documento.FechasPagos.ToArray();

				//Se valida si es practicante para llenar la novedad de apoyo sostenimiento
				bool practicante = false;
				if (nomina.Trabajador.TipoContrato.Equals("4") || nomina.Trabajador.TipoContrato.Equals("5"))
					practicante = true;

				nomina.Devengados = NovedadesNominaXML.ObtenerDevengados(documento.DatosDevengados, practicante);

				nomina.Deducciones = NovedadesNominaXML.ObtenerDeducciones(documento.DatosDeducciones);

				nomina.DevengadosTotal = documento.DevengadosTotal;

				nomina.DeduccionesTotal = documento.DeduccionesTotal;

				nomina.ComprobanteTotal = documento.ComprobanteTotal;

				if (string.IsNullOrEmpty(resolucion.ClaveTecnicaDIAN))
					throw new Exception("La clave técnica en la resolución de la DIAN es inválida para el documento.");

				string CUNE = CalcularCUNE(nomina, resolucion.PinSoftware, ambiente_dian, ref cadena_cufe);
				nomina.InformacionGeneral.CUNE = CUNE;
				nomina.InformacionGeneral.EncripCUNE = "CUNE-SHA384";

				string ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe.dian.gov.co/document/searchqr?documentkey=", CUNE);

				string software_security_code = string.Format("{0}{1}{2}", resolucion.IdSoftware, resolucion.PinSoftware, numero_documento);
				string software_security_code_encriptado = Encriptar.Encriptar_SHA384(software_security_code);

				nomina.ProveedorXML = new NominaIndividualTypeProveedorXML();
				nomina.ProveedorXML.NIT = resolucion.NitProveedor;
				nomina.ProveedorXML.DV = "4";
				nomina.ProveedorXML.RazonSocial = "HERRAMIENTAS DE GESTIÓN INFORMÁTICA SAS";
				nomina.ProveedorXML.SoftwareID = resolucion.IdSoftware;
				nomina.ProveedorXML.SoftwareSC = software_security_code_encriptado;
				nomina.CodigoQR = ruta_qr_Dian;

				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<firma></firma>");

				List<UBLExtensionType> UBLExtensions = new List<UBLExtensionType>();

				//// Extension de la Dian
				//UBLExtensionType UBLExtensionDian = new UBLExtensionType();
				//UBLExtensionDian.ExtensionContent = HGInetUBLv2_1.ExtensionDian.Obtener(resolucion, TipoDocumento.Nomina, nomina.NumeroSecuenciaXML.Numero, ruta_qr_Dian);
				//UBLExtensions.Add(UBLExtensionDian);

				// Extension de la firma
				UBLExtensionType UBLExtensionFirma = new UBLExtensionType();
				UBLExtensionFirma.ExtensionContent = doc.DocumentElement;
				UBLExtensions.Add(UBLExtensionFirma);

				nomina.UBLExtensions = UBLExtensions.ToArray();

				// convierte los datos del objeto en texto XML 
				StringBuilder txt_xml = ConvertirXML.Convertir(nomina, namespaces_xml, TipoDocumento.Nomina);

				FacturaE_Documento xml_sin_firma = new FacturaE_Documento();
				xml_sin_firma.Documento = documento;
				xml_sin_firma.NombreXml = nombre_archivo_xml;
				xml_sin_firma.DocumentoXml = txt_xml;
				xml_sin_firma.CUFE = CUNE;

				return xml_sin_firma;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		public static string CalcularCUNE(NominaIndividualType nomina, string pin_software, string ambiente, ref string cadena_cufe)
		{
			try
			{
				if (nomina == null)
					throw new Exception("Los datos de la factura son inválidos.");
				if (string.IsNullOrWhiteSpace(ambiente))
					throw new Exception("El ambiente es inválido.");

				#region Documentación de la creación código CUNE
				/*
				NumNIE: /NominaIndividual/NumeroSecuenciaXML/@Numero
				FecNIE: /NominaIndividual/InformacionGeneral/@FechaGen
				HorNIE: /NominaIndividual/InformacionGeneral/@HoraGen
				ValDev: /NominaIndividual/DevengadosTotal
				ValDed: /NominaIndividual/DeduccionesTotal
				ValTol: /NominaIndividual/ComprobanteTotal
				NitNIE: /NominaIndividual/Empleador/@NIT
				DocEmp: /NominaIndividual/Trabajador/@NumeroDocumento
				TipoXML: /NominaIndividual/InformacionGeneral/@TipoXML
				Software-Pin: - No está incluido dentro del documento XML. 
							  - Valor reservado, de circulación restringida, asignado por quien obtuvo el Código de Activación 
							  del software en la plataforma del Documento Soporte de Pago de Nómina Electrónica - DIAN 
				TipAmb: /NominaIndividual/InformacionGeneral/@Ambiente
				 			 
				*/
				#endregion

				#region Creación Código CUFE

				string NumCr = nomina.NumeroSecuenciaXML.Numero;
				string FecCr = string.Format("{0}{1}", nomina.InformacionGeneral.FechaGen.ToString("yyyy-MM-dd"), nomina.InformacionGeneral.HoraGen); //fecha.ToString(Fecha.formato_fecha_java);
				string ValDev = nomina.DevengadosTotal.ToString();
				string ValDed = nomina.DeduccionesTotal.ToString();
				string ValTol = nomina.ComprobanteTotal.ToString();
				string NitNIE = nomina.Empleador.NIT;
				string DocEmp = nomina.Trabajador.NumeroDocumento;
				string TipoXML = nomina.InformacionGeneral.TipoXML;



				string cufe = NumCr
					+ FecCr
					+ ValDev.Replace(",", ".")
					+ ValDed.ToString().Replace(",", ".")
					+ ValTol.Replace(",", ".")
					+ NitNIE
					+ DocEmp
					+ TipoXML
					+ pin_software
					+ ambiente
				;

				cadena_cufe = "NumDoc: " + NumCr + ", "
										 + "FechaDoc: " + FecCr + ", "
										 + "ValorDev:" + ValDev.Replace(",", ".") + ", "
										 + "ValorDed: " + ValDed.ToString().Replace(",", ".") + ", "
										 + "TotalDoc: " + ValTol.Replace(",", ".") + ", "
										 + "IdEmpleador: " + NitNIE + ", "
										 + "IdAdquiriente: " + DocEmp + ", "
										 + "TipoNom: " + TipoXML + ", "
										 + "PinSW " + pin_software + ", "
										 + "Ambiente: " + ambiente + ", "
					;

				string cufe_encriptado = Encriptar.Encriptar_SHA384(cufe);
				return cufe_encriptado;
				#endregion
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



	}
}
