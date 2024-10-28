using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HGInetUBLv2_1
{
	public partial class NominaAjusteXML
	{

		public static FacturaE_Documento CrearDocumento(Guid id_documento, NominaAjuste documento, HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian resolucion, TipoDocumento tipo, string ambiente_dian, ref string cadena_cufe)
		{
			try
			{
				if (documento == null)
					throw new Exception("La documento es inválido.");

				//Obtiene el nombre del archivo XML
				string nombre_archivo_xml = NombramientoArchivo.ObtenerXml(documento.Documento.ToString(), documento.DatosEmpleador.Identificacion, tipo, documento.Prefijo);


				NominaIndividualDeAjusteType nomina = new NominaIndividualDeAjusteType();
				XmlSerializerNamespaces namespaces_xml = NamespacesXML.ObtenerNamespaces(tipo);
				nomina.TipoNota = documento.TipoNota.ToString();
				string CUNE = string.Empty;
				string numero_documento = "";

				//1 - Reemplazar y 2 - Eliminar
				if (documento.TipoNota.Equals(1))
				{

					NominaIndividualDeAjusteTypeReemplazar reemplazo_doc = new NominaIndividualDeAjusteTypeReemplazar();

					reemplazo_doc.ReemplazandoPredecesor = new NominaIndividualDeAjusteTypeReemplazarReemplazandoPredecesor();
					reemplazo_doc.ReemplazandoPredecesor.CUNEPred = documento.CUNEPred;
					reemplazo_doc.ReemplazandoPredecesor.FechaGenPred = documento.FechaGenPred;
					reemplazo_doc.ReemplazandoPredecesor.NumeroPred = documento.NumeroPred;

					#region nnomina.NumeroSecuenciaXML //Número de documento: Número de documento nomina.

					if (!string.IsNullOrEmpty(documento.Prefijo))
						numero_documento = string.Format("{0}", documento.Prefijo);

					numero_documento = string.Format("{0}{1}", numero_documento, documento.Documento.ToString());
					reemplazo_doc.NumeroSecuenciaXML = new NominaIndividualDeAjusteTypeReemplazarNumeroSecuenciaXML();
					reemplazo_doc.NumeroSecuenciaXML.Numero = numero_documento;
					reemplazo_doc.NumeroSecuenciaXML.Prefijo = documento.Prefijo;
					reemplazo_doc.NumeroSecuenciaXML.Consecutivo = documento.Documento.ToString();

					reemplazo_doc.NumeroSecuenciaXML.CodigoTrabajador = documento.DatosTrabajador.CodigoTrabajador;

					#endregion

					reemplazo_doc.Periodo = new NominaIndividualDeAjusteTypeReemplazarPeriodo();
					reemplazo_doc.Periodo.FechaGen = Convert.ToDateTime(documento.FechaGen.ToString(Fecha.formato_fecha_hginet));
					reemplazo_doc.Periodo.FechaIngreso = Convert.ToDateTime(documento.DatosPeriodo.FechaIngreso.ToString(Fecha.formato_fecha_hginet));
					reemplazo_doc.Periodo.FechaRetiro = Convert.ToDateTime(documento.DatosPeriodo.FechaRetiro.ToString(Fecha.formato_fecha_hginet));
					reemplazo_doc.Periodo.FechaLiquidacionInicio = Convert.ToDateTime(documento.DatosPeriodo.FechaLiquidacionInicio.ToString(Fecha.formato_fecha_hginet));
					reemplazo_doc.Periodo.FechaLiquidacionFin = Convert.ToDateTime(documento.DatosPeriodo.FechaLiquidacionFin.ToString(Fecha.formato_fecha_hginet));
					reemplazo_doc.Periodo.FechaRetiroSpecified = false;
					reemplazo_doc.Periodo.TiempoLaborado = documento.DatosPeriodo.TiempoLaborado.ToString();

					reemplazo_doc.LugarGeneracionXML = new NominaIndividualDeAjusteTypeReemplazarLugarGeneracionXML();
					reemplazo_doc.LugarGeneracionXML.Idioma = "es";
					reemplazo_doc.LugarGeneracionXML.Pais = documento.DatosEmpleador.Pais;
					reemplazo_doc.LugarGeneracionXML.DepartamentoEstado = documento.DatosEmpleador.DepartamentoEstado;
					reemplazo_doc.LugarGeneracionXML.MunicipioCiudad = documento.DatosEmpleador.MunicipioCiudad;

					reemplazo_doc.InformacionGeneral = new NominaIndividualDeAjusteTypeReemplazarInformacionGeneral();
					reemplazo_doc.InformacionGeneral.Ambiente = ambiente_dian;
					reemplazo_doc.InformacionGeneral.FechaGen = Convert.ToDateTime(documento.FechaGen.ToString(Fecha.formato_fecha_hginet));
					reemplazo_doc.InformacionGeneral.HoraGen = documento.FechaGen.ToString(Fecha.formato_hora_zona);
					reemplazo_doc.InformacionGeneral.PeriodoNomina = documento.PeriodoNomina.ToString();
					reemplazo_doc.InformacionGeneral.TipoMoneda = documento.Moneda;
					reemplazo_doc.InformacionGeneral.TipoXML = "103";
					reemplazo_doc.InformacionGeneral.Version = "V1.0: Nota de Ajuste de Documento Soporte de Pago de Nómina Electrónica";
					if (!documento.Moneda.Equals("COP") && documento.Trm != null)
						reemplazo_doc.InformacionGeneral.TRM = documento.Trm.Valor;

					if (documento.Notas != null && documento.Notas.Count > 0)
						reemplazo_doc.Notas = documento.Notas.ToArray();

					reemplazo_doc.Empleador = ParticipantesNominaXML.ObtenerEmpleadorAjusteR(documento.DatosEmpleador);

					reemplazo_doc.Trabajador = ParticipantesNominaXML.ObtenerTrabajadorAjusteR(documento.DatosTrabajador);

					reemplazo_doc.Pago = new NominaIndividualDeAjusteTypeReemplazarPago();
					reemplazo_doc.Pago.Metodo = documento.DatosPago.Metodo.ToString();
					reemplazo_doc.Pago.Forma = documento.DatosPago.Forma.ToString();
					reemplazo_doc.Pago.NumeroCuenta = documento.DatosPago.NumeroCuenta;
					reemplazo_doc.Pago.TipoCuenta = documento.DatosPago.TipoCuenta;
					reemplazo_doc.Pago.Banco = documento.DatosPago.Banco;

					reemplazo_doc.FechasPagos = documento.FechasPagos.ToArray();

					//Se valida si es practicante para llenar la novedad de apoyo sostenimiento
					bool practicante = false;
					if (reemplazo_doc.Trabajador.TipoContrato.Equals("4") || reemplazo_doc.Trabajador.TipoContrato.Equals("5"))
						practicante = true;

					reemplazo_doc.Devengados = NovedadesNominaXML.ObtenerDevengadosAjuste(documento.DatosDevengados, practicante);

					reemplazo_doc.Deducciones = NovedadesNominaXML.ObtenerDeduccionesAjuste(documento.DatosDeducciones);

					reemplazo_doc.DevengadosTotal = documento.DevengadosTotal;

					reemplazo_doc.DeduccionesTotal = documento.DeduccionesTotal;

					reemplazo_doc.ComprobanteTotal = documento.ComprobanteTotal;

					reemplazo_doc.ProveedorXML = new NominaIndividualDeAjusteTypeReemplazarProveedorXML();
					reemplazo_doc.ProveedorXML.NIT = resolucion.NitProveedor;
					reemplazo_doc.ProveedorXML.DV = "4";
					reemplazo_doc.ProveedorXML.RazonSocial = "HERRAMIENTAS DE GESTIÓN INFORMÁTICA SAS";
					reemplazo_doc.ProveedorXML.SoftwareID = resolucion.IdSoftware;
					

					nomina.Reemplazar = new NominaIndividualDeAjusteTypeReemplazar();
					nomina.Reemplazar = reemplazo_doc;

				}
				else
				{
					NominaIndividualDeAjusteTypeEliminar eliminar_doc = new NominaIndividualDeAjusteTypeEliminar();
																							  
					eliminar_doc.EliminandoPredecesor = new NominaIndividualDeAjusteTypeEliminarEliminandoPredecesor();
					eliminar_doc.EliminandoPredecesor.CUNEPred = documento.CUNEPred;
					eliminar_doc.EliminandoPredecesor.FechaGenPred = documento.FechaGenPred;
					eliminar_doc.EliminandoPredecesor.NumeroPred = documento.NumeroPred;
					
					#region nnomina.NumeroSecuenciaXML //Número de documento: Número de documento nomina.
					if (!string.IsNullOrEmpty(documento.Prefijo))
						numero_documento = string.Format("{0}", documento.Prefijo);

					numero_documento = string.Format("{0}{1}", numero_documento, documento.Documento.ToString());
					eliminar_doc.NumeroSecuenciaXML = new NominaIndividualDeAjusteTypeEliminarNumeroSecuenciaXML();
					eliminar_doc.NumeroSecuenciaXML.Numero = numero_documento;
					eliminar_doc.NumeroSecuenciaXML.Prefijo = documento.Prefijo;
					eliminar_doc.NumeroSecuenciaXML.Consecutivo = documento.Documento.ToString();

					#endregion

					eliminar_doc.LugarGeneracionXML = new NominaIndividualDeAjusteTypeEliminarLugarGeneracionXML();
					eliminar_doc.LugarGeneracionXML.Idioma = "es";
					eliminar_doc.LugarGeneracionXML.Pais = documento.DatosEmpleador.Pais;
					eliminar_doc.LugarGeneracionXML.DepartamentoEstado = documento.DatosEmpleador.DepartamentoEstado;
					eliminar_doc.LugarGeneracionXML.MunicipioCiudad = documento.DatosEmpleador.MunicipioCiudad;

					eliminar_doc.InformacionGeneral = new NominaIndividualDeAjusteTypeEliminarInformacionGeneral();
					eliminar_doc.InformacionGeneral.Ambiente = ambiente_dian;
					eliminar_doc.InformacionGeneral.FechaGen = Convert.ToDateTime(documento.FechaGen.ToString(Fecha.formato_fecha_hginet));
					eliminar_doc.InformacionGeneral.HoraGen = documento.FechaGen.ToString(Fecha.formato_hora_zona);
					eliminar_doc.InformacionGeneral.TipoXML = "103";
					eliminar_doc.InformacionGeneral.Version = "V1.0: Nota de Ajuste de Documento Soporte de Pago de Nómina Electrónica";

					if (documento.Notas != null && documento.Notas.Count > 0)
						eliminar_doc.Notas = documento.Notas.ToArray();

					eliminar_doc.Empleador = ParticipantesNominaXML.ObtenerEmpleadorAjusteE(documento.DatosEmpleador);

					eliminar_doc.ProveedorXML = new NominaIndividualDeAjusteTypeEliminarProveedorXML();
					eliminar_doc.ProveedorXML.NIT = resolucion.NitProveedor;
					eliminar_doc.ProveedorXML.DV = "4";
					eliminar_doc.ProveedorXML.RazonSocial = "HERRAMIENTAS DE GESTIÓN INFORMÁTICA SAS";
					eliminar_doc.ProveedorXML.SoftwareID = resolucion.IdSoftware;

					nomina.Eliminar = new NominaIndividualDeAjusteTypeEliminar();
					nomina.Eliminar = eliminar_doc;

				}

				CUNE = CalcularCUNE(nomina, resolucion.PinSoftware, ambiente_dian, ref cadena_cufe, documento.TipoNota);

				string software_security_code = string.Format("{0}{1}{2}", resolucion.IdSoftware, resolucion.PinSoftware, numero_documento);
				string software_security_code_encriptado = Encriptar.Encriptar_SHA384(software_security_code);

				string ruta_qr_Dian = string.Empty;
				if (ambiente_dian.Equals("1"))
				{
					ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe.dian.gov.co/document/searchqr?documentkey=", CUNE);
				}
				else
				{
					ruta_qr_Dian = string.Format("{0}{1}", "https://catalogo-vpfe-hab.dian.gov.co/document/searchqr?documentkey=", CUNE);
				}

				//1 - Reemplazar y 2 - Eliminar
				if (documento.TipoNota.Equals(1))
				{
					nomina.Reemplazar.InformacionGeneral.CUNE = CUNE;
					nomina.Reemplazar.InformacionGeneral.EncripCUNE = "CUNE-SHA384";
					nomina.Reemplazar.ProveedorXML.SoftwareSC = software_security_code_encriptado;
					nomina.Reemplazar.CodigoQR = ruta_qr_Dian;
				}
				else
				{
					nomina.Eliminar.InformacionGeneral.CUNE = CUNE;
					nomina.Eliminar.InformacionGeneral.EncripCUNE = "CUNE-SHA384";
					nomina.Eliminar.ProveedorXML.SoftwareSC = software_security_code_encriptado;
					nomina.Eliminar.CodigoQR = ruta_qr_Dian;
				}


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
				StringBuilder txt_xml = ConvertirXML.Convertir(nomina, namespaces_xml, tipo);

				// valida el namespace xmlns:schemaLocation y lo reemplaza para Google Chrome
				TextReader textReader = new StringReader(txt_xml.ToString());
				string texto_xml = textReader.ReadToEnd();

				if (texto_xml.Contains("xmlns:schemaLocation"))
				{
					texto_xml = texto_xml.Replace("xmlns:schemaLocation", "xsi:schemaLocation");
					texto_xml = texto_xml.Replace("xmlns:xs=\"http://www.w3.org/2001/XMLSchema-instance\"", "xmlns:xs=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" SchemaLocation=\"\" ");
					txt_xml = new StringBuilder(texto_xml);
				}

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


		public static string CalcularCUNE(NominaIndividualDeAjusteType nomina, string pin_software, string ambiente, ref string cadena_cufe, int tipo_nota)
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

				string NumCr = string.Empty;
				string FecCr = string.Empty;
				string ValDev = string.Empty;
				string ValDed = string.Empty;
				string ValTol = string.Empty;
				string NitNIE = string.Empty;
				string DocEmp = string.Empty;
				string TipoXML = string.Empty;

				if (tipo_nota.Equals(1))
				{
					NumCr = nomina.Reemplazar.NumeroSecuenciaXML.Numero;
					FecCr = string.Format("{0}{1}", nomina.Reemplazar.InformacionGeneral.FechaGen.ToString("yyyy-MM-dd"), nomina.Reemplazar.InformacionGeneral.HoraGen); //fecha.ToString(Fecha.formato_fecha_java);
					ValDev = decimal.Round(nomina.Reemplazar.DevengadosTotal + 0.00M,2).ToString();
					ValDed = decimal.Round(nomina.Reemplazar.DeduccionesTotal + 0.00M,2).ToString();
					ValTol = decimal.Round(nomina.Reemplazar.ComprobanteTotal + 0.00M,2).ToString();
					NitNIE = nomina.Reemplazar.Empleador.NIT;
					DocEmp = nomina.Reemplazar.Trabajador.NumeroDocumento;
					TipoXML = nomina.Reemplazar.InformacionGeneral.TipoXML;
				}
				else
				{
					NumCr = nomina.Eliminar.NumeroSecuenciaXML.Numero;
					FecCr = string.Format("{0}{1}", nomina.Eliminar.InformacionGeneral.FechaGen.ToString("yyyy-MM-dd"), nomina.Eliminar.InformacionGeneral.HoraGen); //fecha.ToString(Fecha.formato_fecha_java);
					ValDev = "0.00";
					ValDed = "0.00";
					ValTol = "0.00";
					NitNIE = nomina.Eliminar.Empleador.NIT;
					DocEmp = "0";
					TipoXML = nomina.Eliminar.InformacionGeneral.TipoXML;
				}

				



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
