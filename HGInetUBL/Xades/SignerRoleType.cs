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
    [System.Xml.Serialization.XmlRootAttribute("SignerRole", Namespace = "http://uri.etsi.org/01903/v1.3.2#", IsNullable = false)]
    public partial class SignerRoleType
    {

        private string[] claimedRolesField;

        private EncapsulatedPKIDataType[] certifiedRolesField;

        /// <comentarios/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ClaimedRole", IsNullable = false)]
        public string[] ClaimedRoles
        {
            get
            {
                return this.claimedRolesField;
            }
            set
            {
                this.claimedRolesField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CertifiedRole", IsNullable = false)]
        public EncapsulatedPKIDataType[] CertifiedRoles
        {
            get
            {
                return this.certifiedRolesField;
            }
            set
            {
                this.certifiedRolesField = value;
            }
        }
    }
}
