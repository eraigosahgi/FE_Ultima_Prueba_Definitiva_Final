using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetUBL;
using HGInetUBL.Objetos;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	/// <summary>
	/// Controlador para gestionar la generación del estándar UBL
	/// </summary>
	public class Ctl_Ubl
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="documento"></param>
		/// <returns></returns>
		public static FacturaE_Documento Generar(Factura documento)
		{
			TipoDocumento tipo = TipoDocumento.Factura;

			// obtiene la resolución del documento
			Ctl_EmpresaResolucion _resolucion = new Ctl_EmpresaResolucion();
			TblEmpresasResoluciones resolucion_documento = _resolucion.Obtener(documento.DatosObligado.Identificacion, documento.NumeroResolucion);

			// obtiene los datos del proveedor tecnológico de la DIAN
			DianProveedor data_dian = HgiConfiguracion.GetConfiguration().DianProveedor;

			// convierte la información de la resolución a la extensión DIAN
			HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian extension_documento = new HGInetMiFacturaElectonicaData.ModeloServicio.ExtensionDian()
			{
				TipoDocumento = tipo.GetHashCode(),
				NumResolucion = resolucion_documento.StrNumResolucion,
				Prefijo = resolucion_documento.StrPrefijo,
				FechaResIni = resolucion_documento.DatFechaVigenciaDesde,
				FechaResFin = resolucion_documento.DatFechaVigenciaHasta,
				RangoIni = resolucion_documento.IntRangoInicial,
				RangoFin = resolucion_documento.IntRangoFinal,
				IdSoftware = data_dian.IdSoftware,
				NitProveedor = data_dian.NitProveedor,
				ClaveTecnicaDIAN = data_dian.ClaveAmbiente,
				PinSoftware = data_dian.Pin
			};

			// convierte el documento 
			FacturaE_Documento resultado = FacturaXML.CrearDocumento(documento, extension_documento);

			// Obtiene el nombre del archivo ZIP
			resultado.NombreZip = NombramientoArchivo.ObtenerZip(documento.Documento.ToString(), documento.DatosObligado.Identificacion, tipo);
			
			return resultado;
		}


	}
}
