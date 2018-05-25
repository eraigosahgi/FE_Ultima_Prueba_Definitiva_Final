using HGInetDIANServicios.DianFactura;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaController.ServiciosDian;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public partial class Ctl_Documentos
	{

		/// <summary>
		/// Genera el xml con la información del documento en formato UBL
		/// </summary>        
		/// <param name="documentoBd">información del documento en base de datos</param>
		/// <param name="empresa">información del facturador electrónico en base de datos</param>
		/// <param name="respuesta">datos de respuesta del documento</param>
		/// <param name="documento_result">información del proceso interno del documento</param>
		/// <returns>información adicional de respuesta del documento</returns>
		public static AcuseRecibo EnviarDian(TblDocumentos documentoBd, TblEmpresas empresa, ref DocumentoRespuesta respuesta, ref FacturaE_Documento documento_result)
		{
			HGInetDIANServicios.DianFactura.AcuseRecibo acuse = new HGInetDIANServicios.DianFactura.AcuseRecibo();
			try
			{
				respuesta.DescripcionProceso = "Envío del archivo ZIP con el XML firmado a la DIAN.";
				respuesta.FechaUltimoProceso = Fecha.GetFecha();
				respuesta.IdProceso = ProcesoEstado.EnvioZip.GetHashCode();

				acuse = Ctl_DocumentoDian.Enviar(documento_result, empresa);
			}
			catch (Exception excepcion)
			{
				respuesta.Error = new LibreriaGlobalHGInet.Error.Error(string.Format("Error en el envío del archivo ZIP con el XML firmado a la DIAN. Detalle: {0}", excepcion.Message), LibreriaGlobalHGInet.Error.CodigoError.VALIDACION, excepcion.InnerException);
				return null;
			}

			respuesta.Cufe = documento_result.CUFE;

			// url pública del xml
			string url_ppal = LibreriaGlobalHGInet.Dms.ObtenerUrlPrincipal("", documento_result.IdSeguridadTercero.ToString());
			respuesta.UrlXmlUbl = string.Format(@"{0}{1}/{2}.xml", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreXml);

			// url pública del zip
			string url_ppal_zip = string.Format(@"{0}{1}/{2}.zip", url_ppal, LibreriaGlobalHGInet.Properties.RecursoDms.CarpetaFacturaEDian, documento_result.NombreZip);

			documentoBd.StrCufe = respuesta.Cufe;
			documentoBd.StrUrlArchivoUbl = respuesta.UrlXmlUbl;
			documentoBd.StrUrlArchivoZip = url_ppal_zip;
			documentoBd.DatFechaActualizaEstado = respuesta.FechaUltimoProceso;
			documentoBd.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);

			Ctl_Documento documento_tmp = new Ctl_Documento();
			documento_tmp.Actualizar(documentoBd);

			//Se da una pausa en proceso para que el servicio de la DIAN termine la validacion del documento
			System.Threading.Thread.Sleep(5000);

			return acuse;
		}

		public static void ConsultarDian()
		{

		}



	}
}
