using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace HGInetMiFacturaElectonicaController.Registros
{
	public class Ctl_NotaDebito : BaseObject<TblDocumentos>
	{
		/// <summary>
		/// Obtiene los documentos de tipo nota débito a nombre del adquiriente que se consulta
		/// </summary>
		/// <param name="identificacion_adquiriente">identificación del adquiriente</param>
		/// <param name="fecha_inicio">fecha inicial de consulta</param>
		/// <param name="fecha_fin">fecha final de consulta</param>
		/// <returns>documentos de tipo nota débito entre fechas por adquiriente</returns>
		public List<NotaDebitoConsulta> ObtenerPorFechasAdquiriente(string identificacion_adquiriente, DateTime FechaInicial, DateTime FechaFinal)
		{
			try
			{
				int tipo_doc = TipoDocumento.NotaDebito.GetHashCode();

				// valida que los parametros sean correctos.
				if (string.IsNullOrWhiteSpace(identificacion_adquiriente) || identificacion_adquiriente.Equals("*"))
					throw new ApplicationException("Número de identificación del adquiriente inválido.");

				// Valida los estados de visibilidad pública para el adquiriente
				var estado_dian = Coleccion.ConvertirString(Ctl_MaestrosEnum.ListaEnum(0, "publico"));

				FechaInicial = FechaInicial.Date;
				FechaFinal = FechaFinal.Date.AddDays(1);

				FechaFinal = new DateTime(FechaFinal.Year, FechaFinal.Month, FechaFinal.Day, 23, 59, 59, 999);

				// obtiene los documentos de acuerdo con los filtros de fechas y con los estados de visibilidad pública
				var respuesta = (from datos in context.TblDocumentos
								 join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
								 where (empresa.StrIdentificacion.Equals(identificacion_adquiriente))
								 && (datos.IntDocTipo == tipo_doc)
								 && (datos.DatFechaDocumento >= FechaInicial && datos.DatFechaDocumento < FechaFinal)
								 && (estado_dian.Contains(datos.IntIdEstado.ToString()))
								 orderby datos.IntNumero descending
								 select datos).ToList();

				List<NotaDebitoConsulta> lista_respuesta = new List<NotaDebitoConsulta>();

				// convierte los registros de base de datos a objeto de servicio Nota Débito y los añade a la lista de retorno
				foreach (TblDocumentos item in respuesta)
				{
					var objeto = (dynamic)null;

					try
					{
						if (item != null)
						{
							//Envia el objeto de Bd a convertir a objeto de servicio
							objeto = Ctl_Documento.ConvertirServicio(item);

							lista_respuesta.Add(objeto);
						}
					}
					catch (Exception)
					{

					}

				}

				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}
	}
}
