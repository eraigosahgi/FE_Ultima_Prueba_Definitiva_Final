using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Registros
{
	public class Ctl_Documento : BaseObject<TblDocumentos>
	{
		#region Constructores 

		public Ctl_Documento() : base(new ModeloAutenticacion()) { }
		public Ctl_Documento(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_Documento(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion


		public TblDocumentos Crear(TblDocumentos documento)
		{
			documento = this.Add(documento);

			return documento;
		}


		public void Validar(TblDocumentos documento)
		{
			

		}

	}


}
