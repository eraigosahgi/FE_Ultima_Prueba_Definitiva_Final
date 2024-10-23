using System.Collections.Generic;
using System.Xml.Serialization;

namespace LibreriaGlobalHGInet.Parametros
{
	[XmlRoot(ElementName = "Configuracion")]
	public class ParametroEstructura
	{
		[XmlArrayItem(ElementName = "Registro")]
		private List<ParametroRegistro> registros;

		public List<ParametroRegistro> Registros
		{
			get
			{
				if (registros == null)
					registros = new List<ParametroRegistro>();
				return registros;
			}
			set
			{
				registros = value;
			}
		}
	}
}
