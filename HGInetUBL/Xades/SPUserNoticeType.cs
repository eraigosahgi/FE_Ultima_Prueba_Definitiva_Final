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
    [System.Xml.Serialization.XmlRootAttribute("SPUserNotice", Namespace = "http://uri.etsi.org/01903/v1.3.2#", IsNullable = false)]
    public partial class SPUserNoticeType
    {

        private NoticeReferenceType noticeRefField;

        private string explicitTextField;

        /// <comentarios/>
        public NoticeReferenceType NoticeRef
        {
            get
            {
                return this.noticeRefField;
            }
            set
            {
                this.noticeRefField = value;
            }
        }

        /// <comentarios/>
        public string ExplicitText
        {
            get
            {
                return this.explicitTextField;
            }
            set
            {
                this.explicitTextField = value;
            }
        }
    }
    
}
