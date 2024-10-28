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
    [System.Xml.Serialization.XmlRootAttribute("CommitmentTypeIndication", Namespace = "http://uri.etsi.org/01903/v1.3.2#", IsNullable = false)]
    public partial class CommitmentTypeIndicationType
    {

        private ObjectIdentifierType commitmentTypeIdField;

        private object[] itemsField;

        private AnyType[] commitmentTypeQualifiersField;

        /// <comentarios/>
        public ObjectIdentifierType CommitmentTypeId
        {
            get
            {
                return this.commitmentTypeIdField;
            }
            set
            {
                this.commitmentTypeIdField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("AllSignedDataObjects", typeof(object))]
        [System.Xml.Serialization.XmlElementAttribute("ObjectReference", typeof(string), DataType = "anyURI")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CommitmentTypeQualifier", IsNullable = false)]
        public AnyType[] CommitmentTypeQualifiers
        {
            get
            {
                return this.commitmentTypeQualifiersField;
            }
            set
            {
                this.commitmentTypeQualifiersField = value;
            }
        }
    }
}
