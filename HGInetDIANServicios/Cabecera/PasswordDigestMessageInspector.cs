using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Web.Services3.Security.Tokens;

namespace HGInetDIANServicios
{
	public class PasswordDigestMessageInspector : IClientMessageInspector
	{
		public Guid IdRequest { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public System.Xml.Linq.XDocument XmlResponse { get; set; }

		public Message Response { get; set; }

		public PasswordDigestMessageInspector(string username, string password, Guid id_request)
		{
			this.Username = username;
			this.Password = password;
			this.IdRequest = id_request;
		}

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
			Response = reply;

			string strRequestXML = reply.ToString();
			XmlResponse = System.Xml.Linq.XDocument.Parse(strRequestXML);

			return;
		}

		public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
		{
			var token = new UsernameToken(Username, Password, PasswordOption.SendPlainText);
			var securityToken = token.GetXml(new XmlDocument());
			var securityHeader = MessageHeader.CreateHeader("Security",
				"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd",
				 securityToken, false);
			request.Headers.Add(securityHeader);

			return Convert.DBNull;
		}

	}
}
