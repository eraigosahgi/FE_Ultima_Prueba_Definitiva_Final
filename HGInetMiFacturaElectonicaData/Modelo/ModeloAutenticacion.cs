using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
	public enum Motores
	{
		[Description("Sql")]
		Sql = 0,

		[Description("MongoDB")]
		MongoDB = 1,
	}

	public class ModeloAutenticacion
	{
		private string servidor;
		private string basedatos;
		private string usuario;
		private string clave;
		private int motor;

		public string Servidor
		{
			get
			{
				return servidor;
			}
		}

		public int Motor
		{
			get
			{
				return motor;
			}
		}

		public string Basedatos
		{
			get
			{
				return basedatos;
			}
		}

		public string Usuario
		{
			get
			{
				return usuario;
			}

		}

		public string Clave
		{
			get
			{
				return clave;
			}
		}

		public ModeloAutenticacion(Motores motor = Motores.Sql)
		{
			dynamic server_bd;

			switch (motor)
			{
				case Motores.Sql:
					server_bd = HgiConfiguracion.GetConfiguration().DataBaseServer;
					this.servidor = server_bd.Servidor;
					this.basedatos = server_bd.BaseDatos;
					this.usuario = server_bd.Usuario;
					this.clave = server_bd.Clave;
					this.motor = motor.GetHashCode();
					break;

				case Motores.MongoDB:
					server_bd = HgiConfiguracion.GetConfiguration().DbAuditoria;
					this.servidor = server_bd.Servidor;
					this.basedatos = server_bd.BaseDatos;
					this.usuario = server_bd.Usuario;
					this.clave = server_bd.Clave;
					this.motor = motor.GetHashCode();
					break;
			}
		}

		public ModeloAutenticacion(string servidor, string basedatos, string usuario, string clave, int motor)
		{
			this.servidor = servidor;
			this.basedatos = basedatos;
			this.usuario = usuario;
			this.clave = clave;
			this.motor = motor;
		}

	}
}
