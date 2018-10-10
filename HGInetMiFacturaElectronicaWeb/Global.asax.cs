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

namespace HGInetMiFacturaElectronicaWeb
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			// Código que se ejecuta al iniciar la aplicación

			DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Expressions;
			DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Default;
			DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Default;
			DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Default;
			ASPxReportDesigner.StaticInitialize();

			DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(Application_Error);

			SerializationService.RegisterSerializer(SerializeReport.Name, new SerializeReport());

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