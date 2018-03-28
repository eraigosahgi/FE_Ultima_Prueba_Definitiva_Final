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

namespace HGInetMiFacturaElectonicaController.ServiciosDian
{
	public class Ctl_DocumentoDian
	{

		public static AcuseRecibo Enviar(FacturaE_Documento documento)
		{
			// obtiene los datos del proveedor tecnológico de la DIAN
			DianProveedorTest data_dian = HgiConfiguracion.GetConfiguration().DianProveedorTest;
			
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
					numero = doc_nota_credito.Documento.ToString();
					nit_obligado = doc_nota_credito.DatosObligado.Identificacion;
					fecha = doc_nota_credito.Fecha;
					break;
				case TipoDocumento.NotaDebito:
					/*NotaDebito doc_nota_debito = ((NotaDebito)documento.Documento);
					numero = doc_nota_debito.Documento.ToString();
					nit_obligado = doc_nota_debito.DatosObligado.Identificacion;
					fecha = doc_nota_debito.Fecha;*/
					break;
				default:
					break;
			}
			
			// ruta del zip
			string ruta_zip = string.Format(@"{0}\{1}.zip", documento.RutaArchivosEnvio, documento.NombreZip);
			
			// valida el archivo zip si existe
			if (!Archivo.ValidarExistencia(ruta_zip))
				throw new ApplicationException(string.Format("No se encuentra la ruta del archivo zip: {0}", ruta_zip));
				
			// envía el documento a través de los servicios web de la DIAN
			AcuseRecibo acuse = Ctl_Factura.Enviar(documento.ID, data_dian.IdSoftware, data_dian.ClaveAmbiente, prefijo, numero, fecha, nit_obligado, ruta_zip, data_dian.UrlServicioWeb);

			return acuse;
		}

	}
}
