using HGInetMiFacturaElectonicaData.ModeloServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Formatos
{
	public class Planilla
	{
		public short Año { get; set; }
		public short Mes { get; set; }
		public string Prefijo { get; set; }
		public long Documento { get; set; }
		public string Empleado { get; set; }
		public string EmpleadoEmail { get; set; }
		public string EmpleadoCuenta { get; set; }
		public string EmpleadoDir { get; set; }
		public string EmpleadoTel { get; set; }
		public string EmpleadoIdentificacionAlt { get; set; }
		public string EmpleadoNombre { get; set; }
		public decimal EmpleadoSalario { get; set; }
		public string Empresa { get; set; }
		public string EmpresaDireccion { get; set; }
		public string EmpresaNit { get; set; }
		public string EmpresaNombre { get; set; }
		public string EmpresaTelefono { get; set; }
		public byte[] EmpresaLogo { get; set; }
		public string Entidad { get; set; }
		public string EntidadDes { get; set; }
		public DateTime Fecha { get; set; }
		public string Observacion { get; set; }
		public string CodigoQR { get; set; }
		public string CUNE { get; set; }
		public DateTime PeriodoFechaF { get; set; }
		public DateTime PeriodoFechaI { get; set; }
		//public short TipoConcepto { get; set; }
		//public short TipoLiquida { get; set; }
		//public string Unidad { get; set; }
		//public string Usuario { get; set; }
		//public string UsuarioNom { get; set; }
		public List<Novedad> Novedades { get; set; }
		public decimal DevengadosTotal { get; set; }
		public decimal DeduccionesTotal { get; set; }
		public decimal ComprobanteTotal { get; set; }
		public Formato DocumentoFormato { get; set; }
	}
}
