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
    [System.Xml.Serialization.XmlRootAttribute("ObjectIdentifier", Namespace = "http://uri.etsi.org/01903/v1.3.2#", IsNullable = false)]
    public partial class ObjectIdentifierType
    {

        private IdentifierType identifierField;

        private string descriptionField;

        private DocumentationReferencesType documentationReferencesField;

        /// <comentarios/>
        public IdentifierType Identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }

        /// <comentarios/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <comentarios/>
        public DocumentationReferencesType DocumentationReferences
        {
            get
            {
                return this.documentationReferencesField;
            }
            set
            {
                this.documentationReferencesField = value;
            }
        }
    }
}
