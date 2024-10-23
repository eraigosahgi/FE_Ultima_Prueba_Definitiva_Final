using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace LibreriaGlobalHGInet
{
	public class IpAddress
	{
		public static string ObtenerIpPublica()
		{
			try
			{
				string externalIP;
				externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
				externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
							 .Matches(externalIP)[0].ToString();
				return externalIP;
			}
			catch { return null; }
		}
	}
}
