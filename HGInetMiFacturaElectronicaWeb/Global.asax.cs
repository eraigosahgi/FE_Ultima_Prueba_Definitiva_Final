using System;
using DevExpress.XtraReports.Web;
using DevExpress.XtraReports.Native;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Newtonsoft.Json;
using HGInetMiFacturaElectronicaWeb.Controllers.Services;
using System.Configuration;
using System.IO;
using System.Text;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.RegistroLog;

namespace HGInetMiFacturaElectronicaWeb
{
	public class Global : HttpApplication
	{

		protected static string fileNameBase;
		protected static string ext = "log";

		// One file name per day
		protected string FileName
		{
			get
			{
				return String.Format("{0}{1}.{2}", fileNameBase, DateTime.Now.ToString("yyyy-MM-dd"), ext);
			}
		}

		void Application_Start(object sender, EventArgs e)
		{
			// Código que se ejecuta al iniciar la aplicación

			DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Expressions;
			DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Default;
			DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Default;
			DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Default;
			ASPxReportDesigner.StaticInitialize();

			DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(Application_Error);			

			HttpConfiguration configuration = GlobalConfiguration.Configuration;

			var jsonFormatter = configuration.Formatters.JsonFormatter;
			jsonFormatter.SerializerSettings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				TypeNameHandling = TypeNameHandling.Objects,
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
			};

			configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

			GlobalConfiguration.Configure(WebApiConfig.Register);

			RegisterRoutes(RouteTable.Routes);

		}

		/// <summary>
		/// Evento al empezar una solicitud web
		/// </summary>
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			var context = HttpContext.Current;
			var response = context.Response;

			// enable CORS
			response.AddHeader("X-Frame-Options", "ALLOW-FROM *");

			if (context.Request.HttpMethod == "OPTIONS")
			{
				response.End();
			}
			RegisterRoutes(RouteTable.Routes);

			//Proceso para guardar la peticion cuando sea un servicio
			string servicio = context.Request.FilePath.ToLowerInvariant();

			string id = string.Empty;

			try
			{
				if (servicio.Contains("wfc"))
				{
					//Proceso tomado de https://blogs.msdn.microsoft.com/rodneyviana/2014/02/06/logging-incoming-requests-and-responses-in-an-asp-net-or-wcf-application-in-compatibility-mode/
					// Crea un guid por cada peticion
					id = Guid.NewGuid().ToString();
					//guarda la peticion
					PeticionHttp input = new PeticionHttp(context, Request.Filter, id, servicio);
					Request.Filter = input;
					input.SetFilter(false);
					//guarda la respuesta
					PeticionHttp output = new PeticionHttp(context, Response.Filter, id, servicio);
					output.SetFilter(true);
					Response.Filter = output;
				}
			}
			catch (Exception excepcion)
			{
				//LogExcepcion.Guardar(new Exception(string.Format("Error al procesar la peticion {0}", excepcion), excepcion));
				RegistroLog.EscribirLog(excepcion, MensajeCategoria.Servicio, MensajeTipo.Error, MensajeAccion.creacion);
			}
		}

		void RegisterRoutes(RouteCollection routes)
		{
			// routes.MapPageRoute("", "HGI/Login", "~/Views/Login/Default.aspx");
			// routes.MapPageRoute("", "AcuseRecibo/{id_seguridad}", "~/Views/Pages/AcuseRecibo.aspx");
		}

		void Application_Error(object sender, EventArgs e)
		{
			// Code that runs when an unhandled error occurs
		}

	}
}