using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData
{
    public class PasarelaPagos : ConfigurationElement
    {

        [ConfigurationProperty("IdComercio", DefaultValue = "", IsKey = true, IsRequired = true)]
        public int IdComercio
        {
            get { return (int)this["IdComercio"]; }
            set { this["IdComercio"] = value; }
        }

        [ConfigurationProperty("ClaveComercio", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ClaveComercio
        {
            get { return (string)this["ClaveComercio"]; }
            set { this["ClaveComercio"] = value; }
        }

        [ConfigurationProperty("RutaComercio", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string RutaComercio
        {
            get { return (string)this["RutaComercio"]; }
            set { this["RutaComercio"] = value; }
        }

        [ConfigurationProperty("CodigoServicio", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string CodigoServicio
        {
            get { return (string)this["CodigoServicio"]; }
            set { this["CodigoServicio"] = value; }
        }

    }
}
