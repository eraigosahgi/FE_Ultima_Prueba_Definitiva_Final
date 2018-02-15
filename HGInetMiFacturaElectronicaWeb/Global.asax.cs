using System;
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
		}
    }
}