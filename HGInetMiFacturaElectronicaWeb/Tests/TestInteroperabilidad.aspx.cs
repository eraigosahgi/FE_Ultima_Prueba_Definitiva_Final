﻿using HGInetInteroperabilidad.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HGInetInteroperabilidad.Configuracion;

namespace HGInetMiFacturaElectronicaWeb.Tests
{
	public partial class TestInteroperabilidad : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				Ctl_MailRecepcion.Procesar();
				//Ctl_Descomprimir.ProcesarArchivosCorreo();
				txt_resultado.Text = "";
			}
			catch (Exception excepcion)
			{
				
			}
		}
	}
}