using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HGInetUBL.Xades
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://uri.etsi.org/01903/v1.3.2#")]
    [System.Xml.Serialization.XmlRootAttribute("Include", Namespace = "http://uri.etsi.org/01903/v1.3.2#", IsNullable = false)]
    public partial class IncludeType
    {

        private string uRIField;

        private bool referencedDataField;

        private bool referencedDataFieldSpecified;

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string URI
        {
            get
            {
                return this.uRIField;
            }
            set
            {
                this.uRIField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool referencedData
        {
            get
            {
                return this.referencedDataField;
            }
            set
            {
                this.referencedDataField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool referencedDataSpecified
        {
            get
            {
                return this.referencedDataFieldSpecified;
            }
            set
            {
                this.referencedDataFieldSpecified = value;
            }
        }
    }
}
