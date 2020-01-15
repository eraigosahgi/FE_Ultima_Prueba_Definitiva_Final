using HGInetMiFacturaElectonicaData.ModeloServicio;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using HGInetMiFacturaElectonicaController.Configuracion;
using LibreriaGlobalHGInet.Error;

namespace HGInetMiFacturaElectronicaWeb.wcf
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class listaFe : IlistaFe
	{
		public string DoWork()
		{
			return "¡Prueba correcta!";
		}

		/// <summary>
		/// Permite Obtener las Listas que se utilizan en la Factura Electronica
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>
		/// <param name="CodigoLista">números de codigos para consulta separados por el caracter coma (,) ó enviando el caracter (*) para obtenerlas todas</param>
		/// <returns>Objeto con las Lista de FE</returns>
		public List<ListaFE> Obtener(string DataKey, string Identificacion, string CodigoLista)
		{
			try
			{

				if (string.IsNullOrEmpty(DataKey))
					throw new ApplicationException("Parámetro DataKey de tipo string inválido.");

				if (string.IsNullOrEmpty(Identificacion))
					throw new ApplicationException("Parámetro Identificacion de tipo string inválido.");

				bool todas = false;

				if (string.IsNullOrEmpty(CodigoLista) || CodigoLista.Equals("*"))
					todas = true;


				List<ListaFE> respuesta = new List<ListaFE>();

				//Válida que la key sea correcta.
				Peticion.Validar(DataKey, Identificacion);

				Ctl_ListaFe ctl_lista = new Ctl_ListaFe();

				// obtiene los datos
				respuesta = ctl_lista.Obtener(CodigoLista, todas);

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
