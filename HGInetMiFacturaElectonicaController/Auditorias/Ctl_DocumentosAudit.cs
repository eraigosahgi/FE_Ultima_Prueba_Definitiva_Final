using HGInetEmailServicios.ServicioEnvio;
using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos;
using LibreriaGlobalHGInet.Formato;
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
		public TblAuditDocumentos Crear(Guid id_seguridad_doc, Guid id_peticion, string facturador, ProcesoEstado proceso, TipoRegistro tipo_registro, Procedencia procesado_por, string realizado_por, string mensaje, string resultado_proceso, string prefijo, string numero,int estado = 0)
		{
			try
			{
				TblAuditDocumentos datos = new HGInetMiFacturaElectonicaData.ModeloAuditoria.Objetos.TblAuditDocumentos()
				{
					StrIdSeguridad = id_seguridad_doc.ToString(),
					StrIdPeticion = id_peticion.ToString(),
					DatFecha = Fecha.GetFecha(),
					StrObligado = facturador,
					IntIdEstado = (estado == 0) ? Ctl_Documento.ObtenerCategoria(proceso.GetHashCode()): estado,
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
		public List<TblAuditDocumentos> Crear(Guid id_seguridad_doc, Guid id_peticion, string facturador, ProcesoEstado proceso, TipoRegistro tipo_registro, Procedencia procesado_por, string realizado_por, string mensaje, string resultado_proceso, List<LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail.Respuesta.MensajeEnvio> respuestas_email, string prefijo, string numero, int estado = 0)
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
						datos.IntIdEstado = (estado == 0) ? Ctl_Documento.ObtenerCategoria(proceso.GetHashCode()) : estado;
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
				List<TblAuditDocumentos> registros_audit = this.GetFilter(x => (x.StrIdSeguridad.Equals(id_seguridad_doc) || id_seguridad_doc.Equals("*")) && (x.StrObligado.Equals(identificacion_obligado) || identificacion_obligado.Equals("*")));

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
		/// <param name="estado">Estado por el cual paso el documento en plataforma</param>
		/// <param name="proceso">Proceso por el cual paso el documento en plataforma</param>
		/// <param name="numero_documento">Busqueda por un Numero de documento especifico</param>
		/// <param name="procedencia">Indica la procedencia de la Auditoria: 1 - Plataforma, 2 - Usuario, 3 - Sonda, 4 - DIAN, 5 - Mail, 6 - Sms</param>
		/// <param name="tipo_registro">Indica que tipo de registro se hizo: 1 - Proceso, 2 - Creacion, 3 - Actualizacion</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> Obtener(string numero_documento, string identificacion_obligado, DateTime fecha_inicio, DateTime fecha_fin, string estado, string proceso, string tipo_registro, string procedencia)
		{
			try
			{

				fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

				if (string.IsNullOrEmpty(numero_documento))
					numero_documento = "*";

				if (string.IsNullOrEmpty(identificacion_obligado))
					identificacion_obligado = "*";

				List<int> ListIntEstado = new List<int>();
				if (string.IsNullOrEmpty(estado))
					estado = "*";

				else if (estado != "*")
					ListIntEstado = Coleccion.ConvertirStringInt(estado);

				List<int> ListProceso = new List<int>();
				if (string.IsNullOrEmpty(proceso))
					proceso = "*";
				else if (proceso != "*")
					ListProceso = Coleccion.ConvertirStringInt(proceso);

				List<int> ListProcedencia = new List<int>();
				if (string.IsNullOrEmpty(procedencia))
					procedencia = "*";
				else if (procedencia != "*")
					ListProcedencia = Coleccion.ConvertirStringInt(procedencia);

				List<int> ListTipoReg = new List<int>();
				if (string.IsNullOrEmpty(tipo_registro))
					tipo_registro = "*";
				else if(tipo_registro != "*")
					ListTipoReg = Coleccion.ConvertirStringInt(tipo_registro);
				


				List<TblAuditDocumentos> registros_audit = this.GetFilter(x => (x.StrNumero.Equals(numero_documento) || numero_documento.Equals("*"))
															&& (x.StrObligado.Equals(identificacion_obligado) || identificacion_obligado.Equals("*"))
															&& (x.DatFecha >= fecha_inicio.Date && x.DatFecha <= fecha_fin)
															&& (ListIntEstado.Contains(x.IntIdEstado) || estado.Equals("*"))
															&& (ListProceso.Contains(x.IntIdProceso) || proceso.Equals("*"))
															&& (ListTipoReg.Contains(x.IntTipoRegistro) || tipo_registro.Equals("*"))
															&& (ListProcedencia.Contains(x.IntIdProcesadoPor) || procedencia.Equals("*"))
															);

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
