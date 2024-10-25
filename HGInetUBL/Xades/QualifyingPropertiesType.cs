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
    [System.Xml.Serialization.XmlRootAttribute("QualifyingProperties", Namespace = "http://uri.etsi.org/01903/v1.3.2#", IsNullable = false)]
    public partial class QualifyingPropertiesType
    {

        private SignedPropertiesType signedPropertiesField;

        private UnsignedPropertiesType unsignedPropertiesField;

        private string targetField;

        private string idField;

        /// <comentarios/>
        public SignedPropertiesType SignedProperties
        {
            get
            {
                return this.signedPropertiesField;
            }
            set
            {
                this.signedPropertiesField = value;
            }
        }

        /// <comentarios/>
        public UnsignedPropertiesType UnsignedProperties
        {
            get
            {
                return this.unsignedPropertiesField;
            }
            set
            {
                this.unsignedPropertiesField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string Target
        {
            get
            {
                return this.targetField;
            }
            set
            {
                this.targetField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }
}
