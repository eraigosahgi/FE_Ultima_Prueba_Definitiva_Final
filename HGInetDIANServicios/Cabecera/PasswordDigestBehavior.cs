using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace HGInetDIANServicios
{
	public class PasswordDigestBehavior : IEndpointBehavior
	{
		public Guid IdRequest { get; set; }
		public string Usuario { get; set; }
		public string Password { get; set; }

		public PasswordDigestMessageInspector Inspector { get; set; }

		public PasswordDigestBehavior(string username, string password, Guid id_request)
		{
			this.Usuario = username;
			this.Password = password;
			this.IdRequest = id_request;
		}

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
			return;
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			this.Inspector = new PasswordDigestMessageInspector(Usuario, Password, IdRequest);

			clientRuntime.MessageInspectors.Add(this.Inspector);
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
			return;
		}

		public void Validate(ServiceEndpoint endpoint)
		{
			// Todo bien.
			return;
		}
	}
}
