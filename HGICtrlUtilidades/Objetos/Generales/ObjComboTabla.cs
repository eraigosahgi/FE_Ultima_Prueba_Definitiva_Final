using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Objetos.Generales
{
	public class ObjComboTabla
	{
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
        public string EAN { get; set; }
        public Guid IdSeguridad { get; set; }
		public decimal Precio { get; set; }
        public short ManejaLote { get; set; }

    }
}
