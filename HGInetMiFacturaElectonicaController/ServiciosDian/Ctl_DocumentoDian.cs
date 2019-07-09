using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetDIANServicios;
using HGInetDIANServicios.DianFactura;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetFirmaDigital;
using LibreriaGlobalHGInet.Funciones;

namespace HGInetMiFacturaElectonicaController.ServiciosDian
{
	public class Ctl_DocumentoDian
	{

		public static AcuseRecibo Enviar(FacturaE_Documento documento, TblEmpresas empresa)
		{

			string IdSoftware = null;
			string PinSoftware = null;
			string clave = null;
			string UrlServicioWeb = null;


			string ruta_certificado = string.Empty;
			CertificadoDigital certificado = null;

			if (empresa.IntVersionDian == 2)
			{
				// obtiene la información de configuración del certificado digital
				certificado = HgiConfiguracion.GetConfiguration().CertificadoDigitalData;

				// obtiene la empresa certificadora
				EnumCertificadoras empresa_certificadora = EnumCertificadoras.Andes;

				if (certificado.Certificadora.Equals("andes"))
					empresa_certificadora = EnumCertificadoras.Andes;
				else if (certificado.Certificadora.Equals("gse"))
					empresa_certificadora = EnumCertificadoras.Gse;

				// información del certificado digital
				ruta_certificado = string.Format("{0}{1}", Directorio.ObtenerDirectorioRaiz(), certificado.RutaLocal);

				// obtiene los datos del proveedor tecnológico de la DIAN
				DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

				IdSoftware = data_dian.IdSoftware;
				PinSoftware = data_dian.Pin;
				clave = data_dian.ClaveAmbiente;
				UrlServicioWeb = data_dian.UrlServicioWeb;
			}
			else
			{

				// sobrescribe los datos de la resolución si se encuentra en estado de habilitación
				if (empresa.IntHabilitacion < Habilitacion.Produccion.GetHashCode())
				{
					// obtiene los datos de prueba del proveedor tecnológico de la DIAN
					DianProveedorTest data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedorTest;

					IdSoftware = data_dian_habilitacion.IdSoftware;
					PinSoftware = data_dian_habilitacion.Pin;
					clave = data_dian_habilitacion.ClaveAmbiente;
					UrlServicioWeb = data_dian_habilitacion.UrlServicioWeb;
				}
				else
				{
					// obtiene los datos del proveedor tecnológico de la DIAN
					DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

					IdSoftware = data_dian.IdSoftware;
					PinSoftware = data_dian.Pin;
					clave = data_dian.ClaveAmbiente;
					UrlServicioWeb = data_dian.UrlServicioWeb;
				}
			}

			string prefijo = string.Empty;

			string numero = string.Empty;

			string nit_obligado = string.Empty;

			DateTime fecha = new DateTime();

			switch (documento.DocumentoTipo)
			{
				case TipoDocumento.Factura:
					Factura doc_factura= ((Factura)documento.Documento);
					prefijo = doc_factura.Prefijo;
					numero = doc_factura.Documento.ToString();
					nit_obligado = doc_factura.DatosObligado.Identificacion;
					fecha = doc_factura.Fecha;
					break;
				case TipoDocumento.NotaCredito:
					NotaCredito doc_nota_credito = ((NotaCredito)documento.Documento);
					prefijo = doc_nota_credito.Prefijo;
					numero = doc_nota_credito.Documento.ToString();
					nit_obligado = doc_nota_credito.DatosObligado.Identificacion;
					fecha = doc_nota_credito.Fecha;
					break;
				case TipoDocumento.NotaDebito:
					NotaDebito doc_nota_debito = ((NotaDebito)documento.Documento);
					prefijo = doc_nota_debito.Prefijo;
					numero = doc_nota_debito.Documento.ToString();
					nit_obligado = doc_nota_debito.DatosObligado.Identificacion;
					fecha = doc_nota_debito.Fecha;
					break;
				default:
					break;
			}
			
			// ruta del zip
			string ruta_zip = string.Format(@"{0}\{1}.zip", documento.RutaArchivosEnvio, documento.NombreZip);
			
			// valida el archivo zip si existe
			if (!Archivo.ValidarExistencia(ruta_zip))
				throw new ApplicationException(string.Format("No se encuentra la ruta del archivo zip: {0}", ruta_zip));


			AcuseRecibo acuse = null;

			switch (empresa.IntVersionDian)
			{
				// envía el documento a través de los servicios web de la DIAN
				case 1:  
					acuse = Ctl_Factura.Enviar(documento.IdSeguridadDocumento, IdSoftware, clave, prefijo, numero, fecha, nit_obligado, ruta_zip, UrlServicioWeb);
					break;

				case 2:
					acuse = new AcuseRecibo();
					acuse = Ctl_Factura.Enviar_v2(ruta_zip, documento.NombreZip, ruta_certificado, certificado.Clave, clave, UrlServicioWeb);
					break;

				default:
					acuse = Ctl_Factura.Enviar(documento.IdSeguridadDocumento, IdSoftware, clave, prefijo, numero, fecha, nit_obligado, ruta_zip, UrlServicioWeb);
					break;
			}
			 
			return acuse;
		}

	}
}
