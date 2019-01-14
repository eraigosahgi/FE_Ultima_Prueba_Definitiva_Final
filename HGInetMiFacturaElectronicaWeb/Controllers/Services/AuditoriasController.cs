using HGInetMiFacturaElectonicaController.Auditorias;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class AuditoriasController : ApiController
	{
		[HttpGet]
		[Route("Api/AuditoriaDocumento")]
		public IHttpActionResult AuditoriaDocumento(string id_seguridad_doc)
		{
			try
			{
				//Valida los datos de la sesión.
				Sesion.ValidarSesion();

				Ctl_DocumentosAudit clase_audit_doc = new Ctl_DocumentosAudit();

				//Realiza la consulta de los datos en la base de datos.
				List<TblAuditDocumentos> datos_audit = clase_audit_doc.Obtener(id_seguridad_doc, "*").OrderByDescending(x => x.DatFecha).ToList();

				if (datos_audit == null)
				{
					return NotFound();
				}

				var datos_retorno = datos_audit.Select(d => new
				{
					StrIdSeguridad = d.StrIdSeguridad,
					StrIdPeticion = d.StrIdPeticion,
					DatFecha = d.DatFecha.AddHours(-5).ToString("yyyy-MM-dd HH:mm:ss"),
					StrObligado = d.StrObligado,
					IntIdEstado = d.IntIdEstado,
					StrDesEstado = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<CategoriaEstado>(d.IntIdEstado)),
					IntIdProceso = d.IntIdProceso,
					StrDesProceso = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<ProcesoEstado>(d.IntIdProceso)),
					IntTipoRegistro = d.IntTipoRegistro,
					IntIdProcesadoPor = d.IntIdProcesadoPor,
					StrDesProcesadoPor = Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<Procedencia>(d.IntIdProcesadoPor)),
					StrRealizadoPor = d.StrRealizadoPor,
					StrMensaje = d.StrMensaje,
					StrResultadoProceso = d.StrResultadoProceso,
					StrPrefijo = d.StrPrefijo,
					StrNumero = string.Format("{0}{1}", (d.StrPrefijo == null) ? "" : (!d.StrPrefijo.Equals("0")) ? d.StrPrefijo : "", d.StrNumero),
				}).ToList();

				return Ok(datos_retorno);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
