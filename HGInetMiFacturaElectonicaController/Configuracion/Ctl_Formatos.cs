using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_Formatos : BaseObject<TblFormatos>
	{

		#region Agregar
		/// <summary>
		/// Almacena el formato en la base de datos
		/// </summary>
		/// <param name="formato"></param>
		/// <returns></returns>
		public TblFormatos Crear(TblFormatos formato)
		{
			formato = this.Add(formato);

			return formato;
		}

		public TblFormatos AlmacenarFormato(string identificacion_empresa, byte[] byte_formato, int tipo_formato)
		{
			try
			{
				System.Guid id_seguridad = System.Guid.NewGuid();

				//Construye el objeto de almacenamiento
				TblFormatos datos_formato = new TblFormatos();
				datos_formato.StrEmpresa = identificacion_empresa;
				datos_formato.IntCodigoFormato = ObtenerIdFormato(identificacion_empresa);
				datos_formato.StrIdSeguridad = id_seguridad;
				datos_formato.IntTipo = tipo_formato;
				datos_formato.FechaRegistro = Fecha.GetFecha();
				datos_formato.IntEstado = true;
				datos_formato.IntGenerico = false;
				datos_formato.Formato = byte_formato;

				//Crea el registro en base de datos.
				TblFormatos datos_respueta = Crear(datos_formato);

				return datos_respueta;
			}
			catch (Exception excepcion)
			{
				return null;
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene el código consecutivo del formato
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <returns></returns>
		public int ObtenerIdFormato(string identificacion_empresa)
		{
			try
			{
				int formato_resultado = 0;

				IEnumerable<TblFormatos> datos = (from formato in context.TblFormatos
												  where formato.StrEmpresa.Equals(identificacion_empresa)
												  select formato);

				if (datos.Count() > 0)
					formato_resultado = datos.Max(x => x.IntCodigoFormato);

				formato_resultado = formato_resultado + 1;

				return formato_resultado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Obtener

		/// <summary>
		/// Obtiene el formato por código e identificación de la empresa
		/// </summary>
		/// <param name="id_formato">código del formato</param>
		/// <param name="identificacion_empresa">número de identificación de la empresa</param>
		/// <returns></returns>
		public TblFormatos Obtener(int id_formato, string identificacion_empresa)
		{
			try
			{
				TblFormatos formato_resultado = (from formato in context.TblFormatos
												 where formato.IntCodigoFormato == id_formato
												 && formato.StrEmpresa.Equals(identificacion_empresa)
												 select formato).First();
				return formato_resultado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		///  Obtiene los formatos de la empresa
		/// </summary>
		/// <param name="identificacion_empresa"></param>
		/// <param name="tipo_formato">indica si es plantilla pdf (1) -- plantilla html (2)</param>
		/// <returns></returns>
		public TblFormatos ObtenerFormatosEmpresa(string identificacion_empresa, int tipo_formato)
		{
			try
			{
				TblFormatos formato_resultado = (from formato in context.TblFormatos
												 where formato.StrEmpresa.Equals(identificacion_empresa)
												 select formato).First();
				return formato_resultado;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

	}
}
