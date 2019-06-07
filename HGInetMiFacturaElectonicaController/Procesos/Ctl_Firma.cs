using HGInetFirmaDigital;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public class Ctl_Firma
	{
		
		public static FacturaE_Documento Generar(string certificado_ruta, string certificado_serial, string certificado_clave, EnumCertificadoras empresa_certificadora, FacturaE_Documento informacion)
		{

			// firma el archivo fìsico xml
			informacion = Ctl_FirmadoXml.FirmarDocumentos(certificado_ruta, certificado_serial, certificado_clave, empresa_certificadora, informacion);
			
			return informacion;
		}



	}
}
