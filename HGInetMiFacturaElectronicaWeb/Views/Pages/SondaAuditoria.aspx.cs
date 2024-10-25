using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.RegistroLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
	public partial class SondaAuditoria : System.Web.UI.Page
	{

		long Cantidad = 0;
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void BtnDocumentos_Click(object sender, EventArgs e)
		{
			try
			{

				//lblResultado.Text = string.Empty;
				Gif.Visible = true;
				DateTime HoraEjecucion = Fecha.GetFecha();

				lblResultado.Text += string.Format("{0}{1} Fecha Inicio: {2} -- Fecha Fin: {3}", System.Environment.NewLine,System.Environment.NewLine,TxtFechaInicio.Text.ToString(),TxtFechaFin.Text.ToString());				

				//Controlador Auditoria mongoDB
				HGInetMiFacturaElectonicaController.Auditorias.MigracionAuditoria.Ctl_AuditoriaDocumentos ControladorSonda = new HGInetMiFacturaElectonicaController.Auditorias.MigracionAuditoria.Ctl_AuditoriaDocumentos();

				var Datos = ControladorSonda.Obtener(DateTime.Parse(TxtFechaInicio.Text), DateTime.Parse(TxtFechaFin.Text));

				//Controlador Auditoria SQL Server
				HGInetMiFacturaElectonicaController.Auditorias.Ctl_DocumentosAudit Controlador_Auditoria = new HGInetMiFacturaElectonicaController.Auditorias.Ctl_DocumentosAudit();

				//Objeto de auditoria SQL Server
				HGInetMiFacturaElectronicaAudit.Modelo.TblAuditDocumentos Objeto = new HGInetMiFacturaElectronicaAudit.Modelo.TblAuditDocumentos();

				Cantidad = 0;
				Datos.Count();
				
				foreach (var item in Datos)
				{

					try
					{
						Objeto = new HGInetMiFacturaElectronicaAudit.Modelo.TblAuditDocumentos();

						Objeto.Id = Guid.NewGuid();
						Objeto.DatFecha = item.DatFecha.AddHours(-5);
						Objeto.IntIdEstado = item.IntIdEstado;
						Objeto.IntIdProcesadoPor = item.IntIdProcesadoPor;
						Objeto.IntIdProceso = item.IntIdProceso;
						Objeto.IntTipoRegistro = item.IntTipoRegistro;
						Objeto.StrIdPeticion = item.StrIdPeticion;
						Objeto.StrIdSeguridad = Guid.Parse(item.StrIdSeguridad);
						Objeto.StrMensaje = item.StrMensaje;
						Objeto.StrNumero = item.StrNumero;
						Objeto.StrObligado = item.StrObligado;
						Objeto.StrPrefijo = item.StrPrefijo;
						Objeto.StrRealizadoPor = item.StrRealizadoPor;
						Objeto.StrResultadoProceso = item.StrResultadoProceso;

						Controlador_Auditoria.Crear(Objeto);
						Cantidad++;
					}


					catch (Exception excepcion)
					{
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
						lblResultado.Text += excepcion.Message;
					}

				}
				lblResultado.Text += string.Format("{0}Cantidad de registros:{1}", System.Environment.NewLine, Cantidad);
				lblResultado.Text += string.Concat(System.Environment.NewLine, "Hora Inicio de Ejecución:  ", HoraEjecucion.ToString(Fecha.formato_fecha_hora_completa));
				lblResultado.Text += string.Concat(System.Environment.NewLine, "Hora Fin de la Ejecución:  ", Fecha.GetFecha().ToString(Fecha.formato_fecha_hora_completa), System.Environment.NewLine);
				lblResultado.Text += string.Concat("Tiempo: ", Fecha.GetFecha().Subtract(HoraEjecucion).ToString(), System.Environment.NewLine, System.Environment.NewLine);

				Gif.Visible = false;
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
				lblResultado.Text += excepcion.Message;
				Gif.Visible = false;
			}
		}

		protected void BtnFormatos_Click(object sender, EventArgs e)
		{
			try
			{
				//Controlador Auditoria mongoDB
				HGInetMiFacturaElectonicaController.Auditorias.MigracionAuditoria.Ctl_AuditoriaFormatos ControladorSonda = new HGInetMiFacturaElectonicaController.Auditorias.MigracionAuditoria.Ctl_AuditoriaFormatos();

				var Datos = ControladorSonda.Obtener(DateTime.Parse(TxtFechaInicio.Text), DateTime.Parse(TxtFechaFin.Text));

				//Controlador Auditoria SQL Server
				HGInetMiFacturaElectonicaController.Auditorias.Ctl_FormatosAudit Controlador_Auditoria = new HGInetMiFacturaElectonicaController.Auditorias.Ctl_FormatosAudit();

				//Objeto de auditoria SQL Server
				HGInetMiFacturaElectronicaAudit.Modelo.TblAuditFormatos Objeto = new HGInetMiFacturaElectronicaAudit.Modelo.TblAuditFormatos();

				Cantidad = 0;
				foreach (var item in Datos)
				{

					try
					{
						Objeto = new HGInetMiFacturaElectronicaAudit.Modelo.TblAuditFormatos();

						Objeto.Id = Guid.NewGuid();
						Objeto.DatFechaProceso = item.DatFechaProceso.AddHours(-5);
						Objeto.IntCodigoFormato = item.IntCodigoFormato;
						Objeto.IntTipoProceso = item.IntTipoProceso;
						Objeto.StrEmpresa = item.StrEmpresa;
						Objeto.StrIdSeguridad = Guid.Parse(item.StrIdSeguridad);
						Objeto.StrObservaciones = item.StrObservaciones;
						Objeto.StrUsuarioProceso = item.StrUsuarioProceso;						

						Controlador_Auditoria.Crear(Objeto);
						Cantidad++;
					}


					catch (Exception excepcion)
					{
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
						lblResultado.Text += excepcion.Message;
					}

				}
				lblResultado.Text += string.Format("Proceso finalizado, Cantidad de registros:{0}", Cantidad);
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
				lblResultado.Text += excepcion.Message;
			}
		}

		protected void BtnNotificaciones_Click(object sender, EventArgs e)
		{
			try
			{
				//Controlador Auditoria mongoDB
				HGInetMiFacturaElectonicaController.Auditorias.MigracionAuditoria.Ctl_AuditoriaNotificaciones ControladorSonda = new HGInetMiFacturaElectonicaController.Auditorias.MigracionAuditoria.Ctl_AuditoriaNotificaciones();

				var Datos = ControladorSonda.Obtener(DateTime.Parse(TxtFechaInicio.Text), DateTime.Parse(TxtFechaFin.Text));

				//Controlador Auditoria SQL Server
				HGInetMiFacturaElectonicaController.Auditorias.Ctl_AlertasHistAudit Controlador_Auditoria = new HGInetMiFacturaElectonicaController.Auditorias.Ctl_AlertasHistAudit();

				//Objeto de auditoria SQL Server
				HGInetMiFacturaElectronicaAudit.Modelo.TblSeguimientoAlertas Objeto = new HGInetMiFacturaElectronicaAudit.Modelo.TblSeguimientoAlertas();

				Cantidad = 0;
				foreach (var item in Datos)
				{

					try
					{
						Objeto = new HGInetMiFacturaElectronicaAudit.Modelo.TblSeguimientoAlertas();

						Objeto.Id = Guid.NewGuid();
						Objeto.DatFecha = item.DatFecha.AddHours(-5);
						Objeto.IntIdAlerta = item.IntIdAlerta;
						Objeto.IntIdEstado = item.IntIdEstado;
						Objeto.IntIdTipo = item.IntIdTipo;
						Objeto.StrIdentificacion = item.StrIdentificacion;
						Objeto.StrIdSeguridadEmpresa = Guid.Parse(item.StrIdSeguridadEmpresa);
						Objeto.StrIdSeguridadPlan = Guid.Parse(item.StrIdSeguridadPlan);
						Objeto.StrMensaje = item.StrMensaje;
						Objeto.StrResultadoProceso = item.StrResultadoProceso;						

						Controlador_Auditoria.Crear(Objeto);
						Cantidad++;
					}

					catch (Exception excepcion)
					{
						RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.creacion);
						lblResultado.Text += excepcion.Message;
					}

				}
				lblResultado.Text += string.Format("Proceso finalizado, Cantidad de registros:{0}", Cantidad);
			}
			catch (Exception excepcion)
			{
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Auditoria, MensajeTipo.Error, MensajeAccion.consulta);
				lblResultado.Text += excepcion.Message;
			}
		}
	}
}