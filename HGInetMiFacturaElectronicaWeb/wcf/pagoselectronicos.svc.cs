using HGInetMiFacturaElectonicaController.PagosElectronicos;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio.Respuestas;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using LibreriaGlobalHGInet.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class pagoselectronicos : Ipagoselectronicos
	{
		public string DoWork()
		{
			return "¡Prueba correcta!";
		}

		/// <summary>
		/// Obtiene los pagos electronicos de documentos por Código de Registro
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>		
		/// <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
		/// <returns></returns>
		public List<PagoElectronicoRespuesta> ConsultaPorCodigoRegistro(string DataKey, string Identificacion, string CodigosRegistros)
		{
			try
			{

				//Se agrega esta a linea a solicitud de don Jorge ya que el servicio de HGIDocs se esta viendo afectado porque estan haciendo muchas consultas por este servicio
				//al parecer una especie de consulta programada o robot
				throw new ApplicationException(string.Format("Error Transaction (Process ID 61) was deadlocked on lock resources with another process and has been chosen as the deadlock victim. Rerun the transaction {0}", Identificacion));

				List<PagoElectronicoRespuesta> respuesta = new List<PagoElectronicoRespuesta>();

				//Válida que la key sea correcta.
				TblEmpresas empresa = Peticion.Validar(DataKey, Identificacion);

				//Se valida si la empresa maneja Pagos
				if (!empresa.IntManejaPagoE)
				{
					throw new ApplicationException(string.Format("El Facturador con la identificación {0} no maneja Pagos Electrónicos.", Identificacion));
				}

				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos
				respuesta = ctl_documento.ConsultaPorCodigoRegistro(Identificacion, CodigosRegistros);

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("PagoConsultaPorCodigoRegistro", DataKey, Identificacion, CodigosRegistros.ToString(), respuesta.Count.ToString());
				}
				catch (Exception)
				{
				}

				return respuesta;

			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

		/// <summary>
		///  Obtiene los pagos entre un rango de fechas especifica
		/// </summary>
		/// <param name="DataKey">DataKey</param>
		/// <param name="Identificacion">Identificacion</param>
		/// <param name="FechaInicial">Fecha Inicial</param>
		/// <param name="FechaFinal">Fecha Final</param>
		/// <param name="Procesados"> Procesados</param>
		/// <returns>List<PagoElectronicoRespuesta></returns>
		public List<PagoElectronicoRespuestaPorFecha> ConsultaPorFechaElaboracion(string DataKey, string Identificacion, DateTime FechaInicial, DateTime FechaFinal, int Procesados = 0)
		{
			try
			{
				List<PagoElectronicoRespuestaPorFecha> respuesta = new List<PagoElectronicoRespuestaPorFecha>();

				//Válida que la key sea correcta.
				TblEmpresas empresa = Peticion.Validar(DataKey, Identificacion);

				//Se valida si la empresa maneja Pagos
				if (!empresa.IntManejaPagoE)
				{
					throw new ApplicationException(string.Format("El Facturador con la identificación {0} no maneja Pagos Electrónicos.", Identificacion));
				}

				Ctl_PagosElectronicos controlador = new Ctl_PagosElectronicos();

				var datos = controlador.ConsultaPorFechaElaboracion(Identificacion, FechaInicial, FechaFinal, Procesados);

				//DateTime fecha_corte = new DateTime(2024, 01, 01, 00, 00, 00);

				//bool obtener_historico = true;

				//if (FechaInicial >= fecha_corte)
				//{
				//	obtener_historico = false;
				//}

				//if (obtener_historico == true)
				//{
				//	List<PagoElectronicoRespuestaPorFecha> datosH = new List<PagoElectronicoRespuestaPorFecha>();

				//	datosH = controlador.ConsultaHisPorFechaElaboracion(Identificacion, FechaInicial, FechaFinal, Procesados);

				//	if (datosH != null && datosH.Count > 0)
				//	{
				//		if (datos != null && datos.Count > 0)
				//		{
				//			datos.AddRange(datosH);
				//		}
				//		else
				//		{
				//			datos = datosH;
				//		}

				//	}
				//}

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ConsultaPagoPorFechaElaboracion", DataKey, Identificacion, FechaInicial.ToString(), FechaFinal.ToString(), respuesta.Count.ToString());
				}
				catch (Exception)
				{
				}

				return datos;

			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}


		/// <summary>
		///  Obtiene los pagos entre un rango de fechas especifica
		/// </summary>
		/// <param name="DataKey">DataKey</param>
		/// <param name="Identificacion">Identificacion</param>
		/// <param name="FechaInicial">Fecha Inicial</param>
		/// <param name="FechaFinal">Fecha Final</param>
		/// <param name="Procesados"> Procesados</param>
		/// <returns>List<PagoElectronicoRespuesta></returns>
		public List<PagoElectronicoRespuestaAgrupadoPorFecha> ConsultaAgrupadosPorFechaElaboracion(string DataKey, string Identificacion, DateTime FechaInicial, DateTime FechaFinal, int Procesados = 0)
		{
			try
			{
				List<PagoElectronicoRespuestaAgrupadoPorFecha> respuesta = new List<PagoElectronicoRespuestaAgrupadoPorFecha>();

				//Válida que la key sea correcta.
				TblEmpresas empresa = Peticion.Validar(DataKey, Identificacion);

				//Se valida si la empresa maneja Pagos
				if (!empresa.IntManejaPagoE)
				{
					throw new ApplicationException(string.Format("El Facturador con la identificación {0} no maneja Pagos Electrónicos.", Identificacion));
				}

				Ctl_PagosElectronicos controlador = new Ctl_PagosElectronicos();

				var datos = controlador.ConsultaAgrupadosPorFechaElaboracion(Identificacion, FechaInicial, FechaFinal, Procesados);

				//DateTime fecha_corte = new DateTime(2024, 01, 01, 00, 00, 00);

				//bool obtener_historico = true;

				//if (FechaInicial >= fecha_corte)
				//{
				//	obtener_historico = false;
				//}

				//if (obtener_historico == true)
				//{
				//	List<PagoElectronicoRespuestaAgrupadoPorFecha> datosH = new List<PagoElectronicoRespuestaAgrupadoPorFecha>();

				//	datosH = controlador.ConsultaAgrupadosHisPorFechaElaboracion(Identificacion, FechaInicial, FechaFinal, Procesados);

				//	if (datosH != null && datosH.Count > 0)
				//	{
				//		if (datos != null && datos.Count > 0)
				//		{
				//			datos.AddRange(datosH);
				//		}
				//		else
				//		{
				//			datos = datosH;
				//		}

				//	}
				//}

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("ConsultaPorFechaElaboracion", DataKey, Identificacion, FechaInicial.ToString(), FechaFinal.ToString(), respuesta.Count.ToString());
				}
				catch (Exception)
				{
				}

				return datos;

			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}



		/// <summary>
		/// Obtiene los pagos electronicos de documentos por Código de Registro
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>		
		/// <param name="CodigosRegistros">código de registro de los Pagos (recibe varios códigos separados por coma)</param>
		/// <returns></returns>
		public List<PagoElectronicoRespuestaDetalle> ActualizarEstadoPago(string DataKey, string Identificacion, string CodigosRegistros)
		{
			try
			{
				List<PagoElectronicoRespuestaDetalle> respuesta = new List<PagoElectronicoRespuestaDetalle>();

				//Válida que la key sea correcta.
				TblEmpresas empresa = Peticion.Validar(DataKey, Identificacion);

				//Se valida si la empresa maneja Pagos
				if (!empresa.IntManejaPagoE)
				{
					throw new ApplicationException(string.Format("El Facturador con la identificación {0} no maneja Pagos Electrónicos.", Identificacion));
				}

				Ctl_PagosElectronicos ctl_pagos = new Ctl_PagosElectronicos();

				//Obtiene los datos
				respuesta = ctl_pagos.ActualizarEstadoPago(Identificacion, CodigosRegistros);

				//Almacena la petición
				try
				{
					Task tarea = Peticion.GuardarPeticionAsync("PagoConsultaPorCodigoRegistro", DataKey, Identificacion, CodigosRegistros.ToString(), respuesta.Count.ToString());
				}
				catch (Exception)
				{
				}

				return respuesta;

			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}
	}
}
