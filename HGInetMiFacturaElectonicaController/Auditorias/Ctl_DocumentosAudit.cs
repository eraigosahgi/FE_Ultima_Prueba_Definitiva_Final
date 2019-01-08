using HGInetEmailServicios.ServicioEnvio;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Auditorias
{

	public class Ctl_DocumentosAudit : MongoDBContext<TblAuditDocumentos>
	{
		#region Constructores 

		public Ctl_DocumentosAudit() : base(new ModeloAutenticacion(Motores.MongoDB)) { }

		#endregion


		/// <summary>
		/// Crea el registro de auditoria de un documento.
		/// </summary>
		/// <param name="datos">datos del registro.</param>
		/// <returns></returns>
		private TblAuditDocumentos Crear(TblAuditDocumentos datos)
		{
			try
			{
				var data = this.Insert(datos);

				if (data.Exception != null)
					throw new ApplicationException(data.Exception.Message, data.Exception.InnerException);

				return datos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}



		/// <summary>
		/// construye el objeto para el registro de auditoria de un documento.
		/// </summary>
		/// <param name="id_seguridad_doc">id se seguridad del documento.</param>
		/// <param name="id_peticion">id de la petición.</param>
		/// <param name="facturador">Identificación del obligado a facturar</param>
		/// <param name="proceso">Proceso del documento.</param>
		/// <param name="estado">Estado actual del documento.</param>
		/// <param name="tipo_registro">Tipo del registro de auditoría</param>
		/// <param name="procesado_por">Procedencia de la petición del log</param>
		/// <param name="realizado_por">usuario que ejecuta el proceso.</param>
		/// <param name="mensaje">mensaje.</param>
		/// <param name="resultado_proceso">Código Dian, Id Mailjet...entre otros</param>
		/// <returns></returns>
		public TblAuditDocumentos Crear(Guid id_seguridad_doc, Guid id_peticion, string facturador, ProcesoEstado proceso, TipoRegistro tipo_registro, Procedencia procesado_por, string realizado_por, string mensaje, string resultado_proceso,string prefijo,string numero)
		{
			try
			{
				TblAuditDocumentos datos = new HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos.TblAuditDocumentos()
				{
					StrIdSeguridad = id_seguridad_doc.ToString(),
					StrIdPeticion = id_peticion.ToString(),
					DatFecha = Fecha.GetFecha(),
					StrObligado = facturador,
					IntIdEstado = Ctl_Documento.ObtenerCategoria(proceso.GetHashCode()),
					IntIdProceso = proceso.GetHashCode(),
					IntTipoRegistro = tipo_registro.GetHashCode(),
					IntIdProcesadoPor = procesado_por.GetHashCode(),
					StrRealizadoPor = realizado_por,
					StrMensaje = mensaje,
					StrResultadoProceso = resultado_proceso,
					StrPrefijo = prefijo,
					StrNumero = numero
				};

				datos = Crear(datos);

				return datos;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// construye el objeto para el registro de auditoria de un documento.
		/// </summary>
		/// <param name="id_seguridad_doc">id se seguridad del documento.</param>
		/// <param name="id_peticion">id de la petición.</param>
		/// <param name="facturador">Identificación del obligado a facturar</param>
		/// <param name="proceso">Proceso del documento.</param>
		/// <param name="estado">Estado actual del documento.</param>
		/// <param name="tipo_registro">Tipo del registro de auditoría</param>
		/// <param name="procesado_por">Procedencia de la petición del log</param>
		/// <param name="realizado_por">usuario que ejecuta el proceso.</param>
		/// <param name="mensaje">mensaje.</param>
		/// <param name="resultado_proceso">Código Dian, Id Mailjet...entre otros</param>
		/// <param name="respuestas_email">Lista de la respuesta de envios de correos</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> Crear(Guid id_seguridad_doc, Guid id_peticion, string facturador, ProcesoEstado proceso, TipoRegistro tipo_registro, Procedencia procesado_por, string realizado_por, string mensaje, string resultado_proceso, List<LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta.MensajeEnvio> respuestas_email, string prefijo, string numero)
		{
			try
			{
				List<TblAuditDocumentos> respuesta_auditoria = new List<TblAuditDocumentos>();

				TblAuditDocumentos datos = new HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos.TblAuditDocumentos();

				foreach (var item in respuestas_email)
				{
					foreach (var data in item.Data)
					{
						datos = new HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos.TblAuditDocumentos();
						datos.StrIdSeguridad = id_seguridad_doc.ToString();
						datos.StrIdPeticion = id_peticion.ToString();
						datos.DatFecha = Fecha.GetFecha();
						datos.StrObligado = facturador;
						datos.IntIdEstado = Ctl_Documento.ObtenerCategoria(proceso.GetHashCode());
						datos.IntIdProceso = proceso.GetHashCode();
						datos.IntTipoRegistro = tipo_registro.GetHashCode();
						datos.IntIdProcesadoPor = procesado_por.GetHashCode();
						datos.StrRealizadoPor = realizado_por;						
						datos.StrResultadoProceso = resultado_proceso;
						datos.StrPrefijo = prefijo;
						datos.StrNumero = numero;
						datos.StrResultadoProceso = Newtonsoft.Json.JsonConvert.SerializeObject(data); 
						datos.StrMensaje = string.Format("{0} : {1}", mensaje, data.Email);
						datos = Crear(datos);						
					}
				}


				return respuesta_auditoria;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene todos los registros
		/// </summary>
		/// <returns></returns>
		public List<TblAuditDocumentos> ObtenerTodos()
		{
			try
			{
				List<TblAuditDocumentos> registros_audit = this.GetAllToList;

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene los registros por id de seguridad del documento e identificación del obligado
		/// </summary>
		/// <param name="id_seguridad_doc">ID de seguridad del documento</param>
		/// <param name="identificacion_obligado">Número de identificación del obligado.</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> Obtener(string id_seguridad_doc, string identificacion_obligado)
		{
			try
			{
				List<TblAuditDocumentos> registros_audit = this.GetFilter(x => (x.StrIdSeguridad.Equals(id_seguridad_doc) || id_seguridad_doc.Equals("*")) && x.StrObligado.Equals(identificacion_obligado));

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Obtiene los registros por rangos de fecha.
		/// las fechas aplican sobre el campo DatFecha, correspondiente a la fecha de registro de la auditoria
		/// </summary>
		/// <param name="id_seguridad_doc">ID de seguridad del documento</param>
		/// <param name="identificacion_obligado">Número de identificación del obligado.</param>
		/// <param name="fecha_inicio">Fecha inicial del rango de búsqueda</param>
		/// <param name="fecha_fin">Fecha final del rango de búsqueda</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> Obtener(string id_seguridad_doc, string identificacion_obligado, DateTime fecha_inicio, DateTime fecha_fin)
		{
			try
			{
				List<TblAuditDocumentos> registros_audit = this.GetFilter(x => x.StrIdSeguridad.Equals(id_seguridad_doc) && x.StrObligado.Equals(identificacion_obligado) && (x.DatFecha >= fecha_inicio.Date && x.DatFecha <= fecha_fin.Date));

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
