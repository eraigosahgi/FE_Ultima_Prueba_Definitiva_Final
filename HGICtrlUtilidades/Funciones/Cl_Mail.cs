using HGICtrlUtilidades.ManejoDatos;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Funciones
{
	public class Cl_Mail
	{
		public partial struct CamposMail
		{
			public int Envio;
			public string Cuenta;
			public string Asunto;
			public string Cuerpo;
			public string[] Adjunto;
			public string Tercero;
		}

		public static void enviar(CamposMail PCamposMail, bool PHtml, bool PMensaje, string Pcuenta, string PDeDir, string PDeNombre, ref string PPwd, bool PSSl, string PHostEnvio, int PPuerto, string PCC, string PCampania, string IdNitEmpresa, string Usuario, int IdAplicativo, string ServidorSQL, string BaseDatosSql, string UsrSql, string PwdSql, bool PMuestraMensaje = true, bool mensaje_sql = true)
		{
			System.Net.Mail.SmtpClient Cliente = new System.Net.Mail.SmtpClient();
			System.Net.Mail.MailMessage Correo = new System.Net.Mail.MailMessage();
			DataTable dt = new DataTable();
			string Sql = "";
			string Fecha;
			string EmailEnvio = "notificaciones@hgi.com.co";
			// valida el correo origen
			try
			{
				Correo.From = new System.Net.Mail.MailAddress(PDeDir, PDeNombre);
			}
			catch (Exception ex)
			{
				if (PMuestraMensaje == true)
					//AMTR Cl_FuncionesForms.MensajeCritico("Cuenta Para envío de correo Inválida");
					throw new ApplicationException("Cuenta Para envío de correo Inválida");
				else
					GrabaEnvio(PCamposMail, "Cuenta Para envío de correo Inválida", Cl_Fecha.GetFecha().ToString(), "2", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);

				return;
			}
			try
			{
				if (string.IsNullOrWhiteSpace(PCamposMail.Cuenta))
				{
					if (PMuestraMensaje)
						//AMTR Cl_FuncionesForms.MensajeCritico("No existe cuenta de Destino");
						throw new ApplicationException("No existe cuenta de Destino");
					else
						GrabaEnvio(PCamposMail, "No existe cuenta de Destino", Cl_Fecha.GetFecha().ToString(), "2", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
				}
			}
			catch (Exception ex)
			{
			}

			// valida que el correo no este en la lista de bloqueados
			if (PCamposMail.Cuenta.IndexOf("@") > 0)
			{
				try
				{
					Correo.To.Add(PCamposMail.Cuenta);
				}
				catch (Exception ex)
				{
					GrabaEnvio(PCamposMail, ex.Message.ToString(), Cl_Fecha.GetFecha().ToString(), "3", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
					return;
				}
			}
			else
			{
				GrabaEnvio(PCamposMail, "error en  cuenta de Destino", Cl_Fecha.GetFecha().ToString(), "3", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
				return;
			}
			if (Cl_FuncionesBD.TblCorreosBloqueadosValida(PCamposMail.Cuenta))
			{
				Fecha = (DateTime)DateTime.Now + ":" + DateTime.Now.Second;
				GrabaEnvio(PCamposMail, "Email Bloqueado " + Cl_FuncionesBD.TblCorreosBloqueadosDescripcion(PCamposMail.Cuenta), Fecha, "2", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
				return;
			}

			if (PPuerto == 100 | PPuerto == 0)
			{
				try
				{
					List<MensajeContenido> Mensajes = new List<MensajeContenido>();
					List<MensajeEnvio> ListMensajeEnvio = new List<MensajeEnvio>();
					MensajeContenido Mensajedet = new MensajeContenido();
					List<DestinatarioEmail> ListaCorreos = new List<DestinatarioEmail>();
					DestinatarioEmail Remitente = new DestinatarioEmail();
					DestinatarioEmail Destinatario = new DestinatarioEmail();
					DestinatarioEmail CC = new DestinatarioEmail();
					DestinatarioEmail RePly = new DestinatarioEmail();
					List<Adjunto> Listadjunto = new List<Adjunto>();
					Adjunto Padjunto = new Adjunto();
					Remitente.Nombre = PDeNombre;
					Remitente.Email = EmailEnvio;
					Remitente.Tipo = TipoDestinatario.Desde;
					ListaCorreos.Add(Remitente);
					Destinatario.Nombre = PDeNombre;
					Destinatario.Email = PCamposMail.Cuenta;
					Destinatario.Tipo = TipoDestinatario.Para;
					ListaCorreos.Add(Destinatario);
					RePly.Nombre = PDeNombre;
					RePly.Email = PDeDir;
					RePly.Tipo = TipoDestinatario.Responder;
					ListaCorreos.Add(RePly);

					if (PCC.IndexOf("@") > 0)
					{
						CC.Nombre = "";
						CC.Email = PCC;
						CC.Tipo = TipoDestinatario.Copia;
						ListaCorreos.Add(CC);
					}
					if (PCamposMail.Asunto == "")
						Mensajedet.Asunto = "Sin Asunto";
					else
						Mensajedet.Asunto = PCamposMail.Asunto;

					int j;
					int I;
					try
					{
						I = Information.UBound(PCamposMail.Adjunto);
						for (j = 0; j <= I; j++)
						{
							Padjunto = new Adjunto();
							Padjunto.ContenidoB64 = Convert.ToBase64String(Cl_Funciones.ConvertirArchvoByte(PCamposMail.Adjunto[j]));
							Padjunto.Nombre = Path.GetFileName(PCamposMail.Adjunto[j]);
							Listadjunto.Add(Padjunto);
						}
					}
					catch (Exception ex)
					{
						I = 0;
					}
					Mensajedet.Adjuntos = Listadjunto;
					if (PHtml)
						Mensajedet.ContenidoHtml = PCamposMail.Cuerpo;
					else
						Mensajedet.ContenidoTexto = PCamposMail.Cuerpo;
					Mensajedet.UnoaUno = true;
					Mensajedet.Emails = ListaCorreos;
					Mensajes.Add(Mensajedet);

					ListMensajeEnvio = Cl_CloudMensajeria.Enviar(PHostEnvio, Pcuenta, IdNitEmpresa, Mensajes);

					foreach (MensajeEnvio item in ListMensajeEnvio)
					{
						foreach (MensajeEnvioItem respuesta in item.Data)
							GrabaEnvio(PCamposMail, "Entregado al servidor de correo", Cl_Fecha.GetFecha().ToString(), "1", PCampania, respuesta.MessageID.ToString(), Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
					}
				}
				catch (Exception ex)
				{
					if (PMuestraMensaje)
					{
						//AMTR Cl_Funciones.DesplegarError(ex);
						GrabaEnvio(PCamposMail, ex.Message.ToString(), Cl_Fecha.GetFecha().ToString(), "3", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
						throw new ApplicationException(ex.Message, ex.InnerException);
					}
					else
						GrabaEnvio(PCamposMail, ex.Message.ToString(), Cl_Fecha.GetFecha().ToString(), "3", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
				}
			}
			else
				try
				{
					if (Strings.InStr(PCC, "@") > 0)
						Correo.Bcc.Add(PCC);
					int j;
					int I;
					try
					{
						I = Information.UBound(PCamposMail.Adjunto);
						for (j = 0; j <= I; j++)
						{
							System.Net.Mail.Attachment att1 = new System.Net.Mail.Attachment(PCamposMail.Adjunto[j]);
							Correo.Attachments.Add(att1);
						}
					}
					catch (Exception ex)
					{
						I = 0;
					}

					// If Len(PCamposMail.Adjunto) > 0 Then
					// Dim att1 As New System.Net.Mail.Attachment(PCamposMail.Adjunto)
					// Correo.Attachments.Add(att1)
					// End If

					Correo.Subject = PCamposMail.Asunto;
					Correo.Body = PCamposMail.Cuerpo;
					// Correo.IsBodyHtml = False
					Correo.IsBodyHtml = PHtml;
					Correo.Priority = System.Net.Mail.MailPriority.High;

					// Cliente.Host = "209.239.124.177"
					try
					{
						Cliente.Host = PHostEnvio;
					}
					catch (Exception ex)
					{
						if (PMuestraMensaje == true)
							//AMTR Cl_FuncionesForms.MensajeCritico("Host Para envío de correo Inválido");
							throw new ApplicationException("Host Para envío de correo Inválido");
						else
							GrabaEnvio(PCamposMail, "Host Para envío de correo Inválido", Cl_Fecha.GetFecha().ToString(), "2", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);

						return;
					}
					Cliente.UseDefaultCredentials = true;

					Cliente.Credentials = new System.Net.NetworkCredential(Pcuenta, PPwd);
					if (PSSl)
						Cliente.EnableSsl = true;

					Cliente.Port = PPuerto;

					// Dim j As Integer
					// For j = 0 To Correo.To.Count - 1
					// MessageBox.Show(Correo.To(j).Address)
					// Next j
					// objCorreo = New System.Net.Mail.MailMessage("jbedoya@hgi.com.co", "jbedoya@hgi.com.co", "ASUNTO", "CONTENIDO")
					// smtpCliente.UseDefaultCredentials = True
					// Cliente.Credentials = New System.Net.NetworkCredential("jbedoya@hgi.com.co", "hgi1234")
					try
					{
						// Cl_Funciones.IniciarBarrarProgreso("Generando Correo a " & Correo.To.ToString, True, False)
						/* AMTR if (PMuestraMensaje)
							Cl_VariablesGlobales.BarraProgresoMensaje2 = "Generando Correo a " + Correo.To.ToString();*/

						Cliente.Send(Correo);
						// Dim Env As String = "Envio"
						// Cliente.SendAsync(Correo, Env)
						string Pme = "";

						Fecha = (DateTime)DateTime.Now + ":" + DateTime.Now.Second;
						Pme = "Entregado al servidor de correo";
						GrabaEnvio(PCamposMail, Pme, Fecha, "1", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
					}
					catch (SmtpException ex)
					{
						Fecha = (DateTime)DateTime.Now + ":" + DateTime.Now.Second;
						GrabaEnvio(PCamposMail, ex.Message.ToString(), Fecha, "3", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
					}
					// Cl_Funciones.DesplegarError(ex)

					catch (Exception ex)
					{
						Fecha = (DateTime)DateTime.Now + ":" + DateTime.Now.Second;
						GrabaEnvio(PCamposMail, ex.Message.ToString(), Fecha, "3", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
					}
				}
				// Cl_Funciones.PararBarraProgreso()
				catch (Exception ex)
				{
					Fecha = (DateTime)DateTime.Now + ":" + DateTime.Now.Second;
					GrabaEnvio(PCamposMail, ex.Message.ToString(), Fecha, "3", PCampania, "", Usuario, IdAplicativo, ServidorSQL, BaseDatosSql, UsrSql, PwdSql, mensaje_sql);
					if (PMensaje)
						//AMTR Cl_Funciones.DesplegarError(ex);
						throw new ApplicationException(ex.Message, ex.InnerException);
				}
		}

		/// <summary>
		/// Graba el envío de un correo electrónico en la base de datos
		/// </summary>
		/// <param name="PCamposMail"></param>
		/// <param name="PResultado"></param>
		/// <param name="PFecha"></param>
		/// <param name="PrIntesultado"></param>
		/// <param name="PCampania"></param>
		/// <param name="PIdServidor"></param>
		private static void GrabaEnvio(CamposMail PCamposMail, string PResultado, string PFecha, string PrIntesultado, string PCampania, string PIdServidor, string Usuario, int IdAplicativo, string ServidorSQL, string BaseDatosSql, string UsrSql, string PwdSql, bool mensaje_sql = true)
		{
			string StrSql;
			string Anexo;

			int j;
			int I;
			try
			{
				I = Information.UBound(PCamposMail.Adjunto) + 1;
			}
			catch (Exception ex)
			{
				I = 0;
			}
			Anexo = "";
			for (j = 1; j <= I; j++)
			{
				Anexo += PCamposMail.Adjunto[j - 1];
				if (I > j)
					Anexo += ",";
			}

			try
			{
				string conexion_sql = Cl_EjecutarSQL.StringConeccionSQL(ServidorSQL, BaseDatosSql, 1, UsrSql, PwdSql, 1, Usuario);

				StrSql = "Insert into TblCorreosEnviados(StrUsuario,DatFecha,IntEnvio,StrMail,StrAsunto,StrAdjunto,StrEstacion,IntAplicativo,StrResultado,StrTercero, StrIdServidor)" + Environment.NewLine;
				StrSql += "Select ";
				StrSql += "'" + Usuario + "'" + Environment.NewLine;
				StrSql += ",'" + Cl_Fecha.GetFecha().ToString(Cl_Fecha.formato_fecha_hora_completa_sql) + "'" + Environment.NewLine;
				StrSql += "," + PCamposMail.Envio + Environment.NewLine;
				StrSql += ",'" + PCamposMail.Cuenta + "'" + Environment.NewLine;
				StrSql += ",'" + PCamposMail.Asunto + "'" + Environment.NewLine;
				StrSql += ",'" + Strings.Left(Anexo, 2000) + "'" + Environment.NewLine;
				StrSql += ",'" + Cl_Enumeracion.GetDescription(Cl_Enumeracion.GetEnumObjectByValue<CodigosAplicativo>(IdAplicativo)) + "'" + Environment.NewLine;
				StrSql += "," + IdAplicativo;
				StrSql += ",'" + Strings.Left(PResultado, 200) + "'" + Environment.NewLine;
				StrSql += ",'" + PCamposMail.Tercero + "'" + Environment.NewLine;
				StrSql += ",'" + PIdServidor + "'" + Environment.NewLine;
				Cl_Funciones.EjecutaInstruccion(StrSql, 0, conexion_sql);

				StrSql = "Update TblCorreosEnvio" + Environment.NewLine;
				StrSql += "Set StrResultado = '" + Strings.Left(PResultado, 200) + "'" + Environment.NewLine;
				StrSql += ",IntResultado = '" + PrIntesultado + "'" + Environment.NewLine;
				StrSql += ", DatFecha = '" + PFecha + "'" + Environment.NewLine;
				StrSql += ", StrIdServidor = '" + PIdServidor + "'" + Environment.NewLine;
				StrSql += "Where StridMail = '" + PCamposMail.Cuenta + "'" + Environment.NewLine;
				StrSql += "And StrCampania = '" + PCampania + "'" + Environment.NewLine;
				Cl_Funciones.EjecutaInstruccion(StrSql, 0, conexion_sql);
			}
			catch (Exception exception)
			{
				if (mensaje_sql)
					throw new ApplicationException(exception.Message, exception.InnerException);
			}
			
		}



	}
}
