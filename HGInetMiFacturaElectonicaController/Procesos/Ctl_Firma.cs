using HGInetFirmaDigital;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public class Ctl_Firma
	{
		
		public static FacturaE_Documento Generar(string NitCertificado, string certificado_ruta, string certificado_serial, string certificado_clave, EnumCertificadoras empresa_certificadora, FacturaE_Documento informacion, bool firma_proveedor)
		{

			// firma el archivo fìsico xml
			informacion = Ctl_FirmadoXml.FirmarDocumentos(NitCertificado, certificado_ruta, certificado_serial, certificado_clave, empresa_certificadora, informacion, firma_proveedor);
			
			return informacion;
		}



	}
}
