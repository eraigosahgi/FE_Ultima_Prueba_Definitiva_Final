using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibreriaGlobalHGInet.ObjetosComunes.ParametrosHGI
{
	public class ObjParametrosAnyo
	{
		public int Anyo { get; set; }
		public short DiasCalendario { get; set; }
		public short PeriodosAdmin { get; set; }
		public short PeriodosCont { get; set; }
		public int SalarioMinimo { get; set; }
		public int SubsidioTransporte { get; set; }
		public decimal Uvt { get; set; }
		public decimal ValPensionE { get; set; }
		public decimal ValSaludE { get; set; }
		public decimal ValSalud { get; set; }
		public decimal ValPension { get; set; }
		public decimal ValCC { get; set; }
		public decimal ValICBF { get; set; }
		public decimal ValSena { get; set; }
		public decimal ValEsap { get; set; }
		public decimal ValMinE { get; set; }
		public string Borrar { get; set; }
		public short PeriodosNom1 { get; set; }
		public decimal Ipc1 { get; set; }
	}
}
