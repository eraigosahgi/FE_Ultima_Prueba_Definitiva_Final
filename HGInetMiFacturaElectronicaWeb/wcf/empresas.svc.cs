using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using LibreriaGlobalHGInet.Error;
using System.ServiceModel.Activation;
using System.Text;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaController.Properties;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class empresas : Iempresas
	{
		public string DoWork()
		{
			return "¡Prueba correcta!";
		}

		/// <summary>
		/// Obtiene la información de la empresa
		/// </summary>
		/// <param name="DataKey"></param>
		/// <param name="Identificacion"></param>
		/// <returns></returns>
		public Empresa Obtener(string DataKey, string Identificacion)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(DataKey))
					throw new ApplicationException("DataKey de la empresa inválido.");

				Empresa respuesta = new Empresa();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				//Obtiene los datos de la empresa.
				TblEmpresas datos_tbl = ctl_empresa.Obtener(Identificacion);

				//Valida si se obtuvieron datos y convierte la tbl a Empresa.
				if (datos_tbl != null)
				{
					respuesta = Ctl_Empresa.ConvertirEmpresa(datos_tbl,true);
					DianProveedorV2 data_dian = HgiConfiguracion.GetConfiguration().DianProveedorV2;

					respuesta.PinSoftware = data_dian.Pin;

					//if (datos_tbl.IntHabilitacion < 99 && datos_tbl.StrIdentificacion.Equals(data_dian.NitProveedor))
					//{
					//	//Para el ambiente de habilitacion a nombre de HGI se cambia informacion del pin e id del SW
					//	DianProveedor data_dian_habilitacion = HgiConfiguracion.GetConfiguration().DianProveedor;
					//	respuesta.PinSoftware = data_dian_habilitacion.Pin;
					//}
					
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
		/// Obtiene la información de la empresa
		/// </summary>
		/// <param name="DataKey"></param>
		/// <param name="Identificacion"></param>
		/// <returns></returns>
		public Empresa ConsultarAdquiriente(string DataKey, string IdentificacionEmisor, string IdentificacionAdquiriente)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(DataKey))
					throw new ApplicationException("DataKey de la empresa inválido.");

				Empresa respuesta = new Empresa();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, IdentificacionEmisor);

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				//Obtiene los datos de la empresa.
				TblEmpresas datos_tbl = ctl_empresa.Obtener(IdentificacionAdquiriente);

				//Valida si se obtuvieron datos y convierte la tbl a Empresa.
				if (datos_tbl != null)
				{
					respuesta = Ctl_Empresa.ConvertirEmpresa(datos_tbl, false);				
					
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
		/// Crea o actualiza la Empresa Desde el ERP de HGI	en la plataforma.
		/// </summary>
		/// <param name="empresa_nueva">Objeto con la In</param>
		/// <returns></returns>
		public bool Crear(Empresa empresa_nueva)
		{
			try
			{
				if (empresa_nueva == null)
					throw new ApplicationException("Informacion no encontrada");

				if (string.IsNullOrWhiteSpace(empresa_nueva.idseguridad_EmpresaEmisor))
					throw new ApplicationException("idseguridad de la empresa inválido.");

				if (!empresa_nueva.Identificacion_EmpresaEmisor.Equals(Constantes.NitResolucionconPrefijo))
					throw new ApplicationException("Servicio no disponible");

				bool respuesta = true;

				//Válida que si sea HGI el que utiliza este metodo.
				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				TblEmpresas hgi = ctl_empresa.Obtener(empresa_nueva.Identificacion_EmpresaEmisor, false);

				if (!hgi.StrIdSeguridad.Equals(Guid.Parse(empresa_nueva.idseguridad_EmpresaEmisor)))
					throw new ApplicationException("idseguridad de la empresa inválido.");

				//Obtiene los datos de la empresa.
				try
				{
					TblEmpresas datos_tbl = ctl_empresa.ConvertirEmpresaErP(empresa_nueva);
				}
				catch (Exception exec)
				{
					Error error = new Error(CodigoError.ERROR_AGREGAR, exec);
					throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
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
