using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGInetMiFacturaElectronicaWeb.Seguridad
{
    public class PaginaPermiso
    {
        private string codigo = "";
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        private string titulo = "";
        public string Titulo
        {
            get { return titulo; }
            set { titulo = value; }
        }

        private string grupo_pagina = "";
        public string GrupoPagina
        {
            get { return grupo_pagina; }
            set { grupo_pagina = value; }
        }

        private string ruta_pagina_inicio_url = "";
        public string RutaPaginaInicioUrl
        {
            get { return ruta_pagina_inicio_url; }
            set { ruta_pagina_inicio_url = value; }
        }

        private List<string> codigos_asociados = new List<string>();
        public List<string> CodigosAsociados
        {
            get { return codigos_asociados; }
            set { codigos_asociados = value; }
        }

        public PaginaPermiso()
        {
        }

    }
}