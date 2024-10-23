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
		///Debe ser una lista de 5 elementos en donde la descripcion debe ser del enumerable(lista) CamposSalud y el valor que corresponda.
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
		/// Indica si la operacion del documento 0 - Factura Eelctronica, 1 - Recaudo, 2 - Factura Talonario, 3 - Reporte, 4 - Sin Aporte 
		/// </summary>
		public int TipoOperacion { get; set; }

		/// <summary>
		/// Fecha Inicio del periodo de Facturacion
		/// </summary>
		public DateTime FechaIni { get; set; }

		/// <summary>
		/// Fecha Fin del periodo de Facturacion
		/// </summary>
		public DateTime FechaFin { get; set; }

		/// <summary>
		/// Informacion del usuario beneficiario del servicio de salud
		/// </summary>
		public Tercero DatosBeneficiario { get; set; }

	}
}
