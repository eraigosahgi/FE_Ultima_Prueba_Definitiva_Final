using HGInetMiFacturaElectonicaData.ModeloServicio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBL
{
	public class FormatoNotas
	{

		/// <summary>
		/// Agrega los campos campos del formato adicionales en el XML
		/// </summary>
		/// <param name="campos">Campos adicionales que lleva el PDF</param>
		/// <returns>lista de notas</returns>
		public static List<string> CamposPredeterminados(Formato campos)
		{
			List<string> notas_documento = new List<string>();

			if (campos == null)
			{
				campos = new Formato();

				if (campos.CamposPredeterminados == null)
					campos.CamposPredeterminados = new List<FormatoCampo>();
			}

			JsonSerializerSettings config = new JsonSerializerSettings()
			{
				Formatting = Newtonsoft.Json.Formatting.None,
				DateTimeZoneHandling = DateTimeZoneHandling.Utc
			};

			//añade el documento a una lista, para ser recorrida.
			List<Formato> datos = new List<Formato>();
			datos.Add(campos);

			//Construye el objeto a serializar.
			var datos_json = datos.Select(d => new
			{
				Codigo = d.Codigo,
				CamposPredeterminados = d.CamposPredeterminados,
				Titulo = d.Titulo
			});

			// convierte el objeto en json
			string formato_json = JsonConvert.SerializeObject(datos_json.FirstOrDefault(), typeof(Formato), config);

			//Si envian alguno de estos saltos de lineas, se reemplazan por el que toma el diseñador del formato
			//el campo donde se va mostrar debe tener habilitada la propiedad Multilinea
			formato_json = formato_json.Replace("<\br>","\r\n").Replace("\\n", "\r\n");

			// agrega los campos del formato del documento en la 2da posición
			notas_documento.Add(formato_json);

			return notas_documento;


		}



	}
}
