using HGInetMiFacturaElectonicaController.PagosElectronicos;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Modelo;
using LibreriaGlobalHGInet.Enumerables.PagosEnLinea;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.ObjetosComunes.PagosEnLinea;
using LibreriaGlobalHGInet.Peticiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class PagosController : ApiController
	{
		/// <summary>
		/// Valida el estatus de una lista de pagos
		/// </summary>
		/// <param name="strIdSeguridad"></param>        
		/// <returns></returns>

		[HttpPost]
		[Route("Api/ListaEstadoPagos")]
		public IHttpActionResult ListaEstadoPagos(object ListaPagos)
		{

			try
			{
				var jobjeto = (dynamic)ListaPagos;

				string ListaDoc = jobjeto.ListaPagos;

				List<ObjetoConsultaPago> ConfigPago = new JavaScriptSerializer().Deserialize<List<ObjetoConsultaPago>>(ListaDoc);
				foreach (var item in ConfigPago)
				{
					//

				}
				return Ok();
			}
			catch (Exception excepcion)
			{

				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		[HttpGet]
		[Route("Api/ObtenerPagoE")]
		public IHttpActionResult ObtenerPagoE(Guid id_seguridad_doc, Guid id_Seguridad_Registro)
		{
			try
			{
				PasarelaPagos pasarela_pagos = HgiConfiguracion.GetConfiguration().PasarelaPagos;

				List<TblPasarelaPagosPI> respuesta = new List<TblPasarelaPagosPI>();
				string datos_get = string.Format("?IdSeguridadPago={0}&StrIdSeguridadRegistro={1}", id_seguridad_doc, id_Seguridad_Registro);

				ClienteRest<TblPasarelaPagosPI> cliente = new ClienteRest<TblPasarelaPagosPI>(pasarela_pagos.RutaPlataforma + "Api/ObtenerPago" + datos_get, TipoContenido.Applicationjson.GetHashCode(), "");
				try
				{
					TblPasarelaPagosPI data = cliente.GET();

					if (data != null)
						respuesta.Add(data);
				}
				catch (Exception ex)
				{
					var cod = cliente.CodHttp;
				}

				Ctl_Documento ctl_documento = new Ctl_Documento();

				//Obtiene los datos del documento en la base de datos.
				TblDocumentos datos_doc = new TblDocumentos();
				if (respuesta.Count > 0)
					datos_doc = ctl_documento.ObtenerPorIdSeguridad(respuesta.FirstOrDefault().StrIdSeguridadDoc).FirstOrDefault();

				var datos_retorno = respuesta.Select(d => new
				{
					DatFechaEmisionDoc = (datos_doc != null) ? datos_doc.DatFechaIngreso : new DateTime?(),
					DatFechaVenceDoc = (datos_doc != null) ? datos_doc.DatFechaVencDocumento : new DateTime?(),
					StrNumerDoc = string.Format("{0}{1}", (datos_doc.StrPrefijo == null) ? "" : (!datos_doc.StrPrefijo.Equals("0")) ? datos_doc.StrPrefijo : "", datos_doc.IntNumero),
					d.DatFechaRegistro,
					d.DatFechaSync,
					d.DatFechaVerificacion,
					d.IntAuthCompania,
					d.IntAuthEmpresa,
					//d.IntComercioId,
					d.IntIdAplicativo,
					d.IntPagoClicloTransaccion,
					d.IntPagoCodServicio,
					d.IntPagoEstado,
					d.IntPagoFormaPago,
					StrPagoDesFormaPago = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<FormasPago>(d.IntPagoFormaPago)),
					d.IntSincronizacion,
					d.IntValor,
					d.IntValorIva,
					d.StrAuthIdEmpresa,
					d.StrAuthToken,
					d.StrCampo1,
					d.StrCampo2,
					d.StrCampo3,
					d.StrClienteEmail,
					d.StrClienteIdentificacion,
					d.StrClienteNombre,
					d.StrClienteTelefono,
					d.StrClienteTipoId,
					//d.StrCodigoServicio,
					//d.StrComercioClave,
					d.StrComercioData,
					//d.StrComercioIdRuta,
					d.StrDescripcionPago,
					d.StrIdDocumento,
					d.StrIdPlataforma,
					d.StrIdSeguridad,
					d.StrIdSeguridadDoc,
					d.StrIdSeguridadPago,
					d.StrIdSeguridadRegistro,
					d.StrMensajeVerificacion,
					d.StrPagoCodBanco,
					d.StrPagoCodFranquicia,
					d.StrPagoDesBanco,
					d.StrPagoIdPlataforma,
					d.StrPagoTicketID,
					d.StrPagoTransaccionCUS,
					d.StrRutaDestino,
					d.StrRutaProcedencia,
					d.StrRutaSync
				});

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		public class ObjetoConsultaPago
		{
			public string StrIdRegistro { get; set; }
			public string StrIdSeguridadDoc { get; set; }
		}

	}
}