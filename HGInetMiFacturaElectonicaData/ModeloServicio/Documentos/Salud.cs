using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.ModeloServicio
{

	public class Salud
	{
		/// <summary>
		///Debe ser una lista de 21 elementos en donde la descripcion debe ser del enumerable(lista) CamposSalud y el valor que corresponda.
		/// </summary>
		public List<CampoValor> CamposSector { get; set; }

		/// <summary>
		/// Corresponde a una direccion web donde el emisor dispone  de la informacion complementaria a los documentos electronicos que el adquiriente puede ingresar y descargar
		/// </summary>
		public string URLDescargaAdjuntos { get; set; }

		/// <summary>
		///Grupo de informacion para indicar caracteristicas adicionales a la URL ya informada.
		/// </summary>
		public List<CampoValor> ParametrosDescargaAdjuntos { get; set; }

		/// <summary>
		/// El Web Service es utilizado para la recepcion de los eventos que se genere por parte del Adquiriente. Corresponde a un acuerdo o formalidad entre las partes(Emisor y Receptor)
		/// </summary>
		public string URLWebService { get; set; }

		/// <summary>
		///Grupo de informacion para indicar caracteristicas adicionales a la conexion al Webservices ya informado (Nombre parametros, claves o nombre de archivos etc).
		/// </summary>
		public List<CampoValor> ParametrosWebService { get; set; }

		/// <summary>
		/// Corresponde a la futura operacion de acreditación se llena en campo @schemeID del CustomizationID
		/// </summary>
		//public string FuturaOperacionRecaudo { get; set; }

		/// <summary>
		/// Los documentos referenciados en estos fragmentos comprueban el recaudo de cuotas moderadoras, copagos o cuotas de recuperacion, como recursos a favor del Adquiriente
		/// </summary>
		public List<ReferenciaDocumento> Documentosreferencia { get; set; }

		/// <summary>
		/// Descripcion de la Modalidad de contratacion y Pago Tabla 18.4.3 resolucion 084 pagina 63
		/// </summary>
		//public string ModalidadesContratacionPago { get; set; }

	}
}
