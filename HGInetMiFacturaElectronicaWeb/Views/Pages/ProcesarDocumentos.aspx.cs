﻿using HGInetMiFacturaElectonicaData.Enumerables;
using HGInetMiFacturaElectronicaWeb.Properties;
using HGInetMiFacturaElectronicaWeb.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGInetMiFacturaElectronicaWeb.Views.Pages
{
    public partial class ProcesarDocumentos1 : PaginaContenido
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.CodigoOpcion = OpcionesPermisos.ProcesarDocumentos;


            this.ProcesoPagina = OperacionesBD.IntGestion;

            this.ProcesoPagina = OperacionesBD.IntConsultar;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }
    }
}