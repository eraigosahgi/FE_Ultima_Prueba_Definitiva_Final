﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Indicadores.Objetos
{
	public class TopTransaccional
	{
		public int Consecutivo { get; set; }
		public string Identificacion { get; set; }
		public decimal CantidadAnterior { get; set; }
		public string DescripcionAnterior { get; set; }
		public decimal CantidadActual { get; set; }
		public string DescripcionActual { get; set; }
		public string RazonSocial { get; set; }
		public decimal TotalDocumentos { get; set; }
		public decimal ValorTotalDocumentos { get; set; }
		public string ProveedorEmisor { get; set; }
	}

}
