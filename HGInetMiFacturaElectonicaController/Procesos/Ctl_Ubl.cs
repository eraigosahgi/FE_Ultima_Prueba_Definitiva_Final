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
		/// <param name="tipo"></param>
		/// <returns></returns>
		public ResultadoXml Generar(Factura documento, TipoDocumento tipo)
		{
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
			ResultadoXml resultado = FacturaXML.CrearDocumento(documento, extension_documento);

			return resultado;
		}


	}
}
