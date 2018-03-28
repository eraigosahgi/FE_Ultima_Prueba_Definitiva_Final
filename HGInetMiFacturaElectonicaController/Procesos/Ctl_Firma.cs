using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGInetFirmaDigital;
using HGInetUBL;
using HGInetUBL.Objetos;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetMiFacturaElectonicaController.Procesos
{
	public class Ctl_Firma
	{

		public static FacturaE_Documento Generar(string certificado_ruta, string certificado_serial, string certificado_clave, EnumCertificadoras empresa_certificadora, FacturaE_Documento informacion)
		{
			List<FacturaE_Documento> datos = new List<FacturaE_Documento>();
			datos.Add(informacion);

            // firma el archivo fìsico xml
            datos = Ctl_FirmadoXml.FirmarDocumentos(certificado_ruta, certificado_serial, certificado_clave, empresa_certificadora, datos);


            // firma el texto xml
            //datos = Ctl_FirmadoStringBuilder.FirmarDocumentos(certificado_ruta, certificado_serial, certificado_clave, empresa_certificadora, datos);


			return datos[0];
		}



	}
}
