﻿using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectronicaAudit.Modelo;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria;
using LibreriaGlobalHGInet.ObjetosComunes.Mensajeria.Mail;
using LibreriaGlobalHGInet.RegistroLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HGInetMiFacturaElectronicaAudit.Controladores
{
	public class Srv_DocumentosAudit
	{
		FEHGIAuditoria db = new FEHGIAuditoria();

		/// <summary>
		/// Crea el registro de auditoria de un documento.
		/// </summary>
		/// <param name="datos">datos del registro.</param>
		/// <returns></returns>
		public TblAuditDocumentos Crear(TblAuditDocumentos datos)
		{
			try
			{
				db.Set<TblAuditDocumentos>().Add(datos);
				db.SaveChanges();

				return datos;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
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
				var datos = db.TblAuditDocumentos.ToList();
				return datos;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
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

				Guid Id = Guid.Parse(id_seguridad_doc);
				List<TblAuditDocumentos> registros_audit = db.TblAuditDocumentos.Where(x => (x.StrIdSeguridad == Id || id_seguridad_doc.Equals("*")) && (x.StrObligado.Equals(identificacion_obligado) || identificacion_obligado.Equals("*"))).ToList();

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		public List<TblAuditDocumentos> ObtenerAudit(string id_seguridad_doc)
		{
			try
			{

				Guid Id = Guid.Parse(id_seguridad_doc);
				List<TblAuditDocumentos> registros_audit = db.TblAuditDocumentos.Where(x => (x.StrIdSeguridad == Id) && (x.IntIdProceso == 93 || x.IntIdProceso == 2)).OrderBy(x => x.DatFecha).ToList();

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
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
		public List<TblAuditDocumentos> Obtener(string numero_documento, string identificacion_obligado, DateTime fecha_inicio, DateTime fecha_fin, string estado, string proceso, string tipo_registro, string procedencia, int Desde, int Hasta)
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
				else if (tipo_registro != "*")
					ListTipoReg = Coleccion.ConvertirStringInt(tipo_registro);



				List<TblAuditDocumentos> registros_audit = db.TblAuditDocumentos.Where(x => (x.StrNumero.Equals(numero_documento) || numero_documento.Equals("*"))
															&& (x.StrObligado.Equals(identificacion_obligado) || identificacion_obligado.Equals("*"))
															&& (x.DatFecha >= fecha_inicio.Date && x.DatFecha <= fecha_fin)
															&& (ListIntEstado.Contains(x.IntIdEstado) || estado.Equals("*"))
															&& (ListProceso.Contains(x.IntIdProceso) || proceso.Equals("*"))
															&& (ListTipoReg.Contains(x.IntTipoRegistro) || tipo_registro.Equals("*"))
															&& (ListProcedencia.Contains(x.IntIdProcesadoPor) || procedencia.Equals("*"))
															).OrderByDescending(x => x.DatFecha).Skip(Desde).Take(Hasta).ToList();

				return registros_audit;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}




		//#region Sonda de Email
		///// <summary>
		///// Retorna la respuesta de email de un documento
		///// </summary>
		///// <param name="IdSeguridad">Id de seguridad del documento</param>
		///// <returns></returns>
		//public MensajeValidarEmail ObtenerResultadoEmail(Guid IdSeguridad)
		//{
		//	try
		//	{
		//		List<TblAuditDocumentos> ListaEmail = db.TblAuditDocumentos.Where(x => (x.StrIdSeguridad.Equals(IdSeguridad)) && (x.IntIdProceso.Equals(8))).OrderBy(x => x.DatFecha).ToList();
		//		MensajeResumen datos_retorno = new MensajeResumen();

		//		MensajeValidarEmail MailPlataforma = new MensajeValidarEmail();

		//		Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
		//		foreach (TblAuditDocumentos Email in ListaEmail)
		//		{
		//			try
		//			{
		//				if (Email.StrResultadoProceso.Contains("MessageID"))
		//				{
		//					email = new Ctl_EnvioCorreos();
		//					Email.StrResultadoProceso = Email.StrResultadoProceso.Replace("[", "").Replace("]", "");
		//					dynamic datos = JsonConvert.DeserializeObject(Email.StrResultadoProceso);

		//					datos_retorno = email.ConsultarCorreo((long)datos.MessageID);
		//					//Si ya tengo una respuesta se la asigno a este 
		//					if (MailPlataforma.EmailEnviado == null && datos_retorno.Estado != null)
		//					{
		//						MailPlataforma.EmailEnviado = (string)datos.Email;
		//						MailPlataforma.Estado = datos_retorno.Estado;
		//						MailPlataforma.IdResultado = datos_retorno.IdResultado;
		//						MailPlataforma.Recibido = datos_retorno.Recibido;
		//						if (MailPlataforma.IdResultado == MensajeIdResultado.Entregado.GetHashCode())
		//						{
		//							return MailPlataforma;
		//						}
		//					}
		//					else if (datos_retorno.Estado != null)
		//					{
		//						MailPlataforma.EmailEnviado = (string)datos.Email;
		//						MailPlataforma.Estado = datos_retorno.Estado;
		//						MailPlataforma.IdResultado = datos_retorno.IdResultado;
		//						MailPlataforma.Recibido = datos_retorno.Recibido;
		//						if (MailPlataforma.IdResultado == MensajeIdResultado.Entregado.GetHashCode())
		//						{
		//							return MailPlataforma;
		//						}
		//					}
		//				}
		//			}
		//			catch (Exception)
		//			{ }
		//		}

		//		return MailPlataforma;
		//	}
		//	catch (Exception excepcion)
		//	{
		//		throw new ApplicationException(excepcion.Message, excepcion.InnerException);
		//	}
		//}

		//#endregion


		/// <summary>
		/// Obtiene todos los email asociados a un documento
		/// </summary>
		/// <param name="IdSeguridad">identificador unico del documento en la plataforma</param>
		/// <returns></returns>
		public List<TblAuditDocumentos> ObtenerDocumentoMail(Guid IdSeguridad)
		{

			try
			{
				List<TblAuditDocumentos> ListaEmail = db.TblAuditDocumentos.Where(x => (x.StrIdSeguridad == IdSeguridad) && (x.IntIdProceso.Equals(8))).OrderBy(x => x.DatFecha).ToList();

				return ListaEmail;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

	}
}
