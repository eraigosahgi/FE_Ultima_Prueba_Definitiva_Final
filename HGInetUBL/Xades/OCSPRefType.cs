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
    public partial class OCSPRefType
    {

        private OCSPIdentifierType oCSPIdentifierField;

        private DigestAlgAndValueType digestAlgAndValueField;

        /// <comentarios/>
        public OCSPIdentifierType OCSPIdentifier
        {
            get
            {
                return this.oCSPIdentifierField;
            }
            set
            {
                this.oCSPIdentifierField = value;
            }
        }

        /// <comentarios/>
        public DigestAlgAndValueType DigestAlgAndValue
        {
            get
            {
                return this.digestAlgAndValueField;
            }
            set
            {
                this.digestAlgAndValueField = value;
            }
        }
    }
}
