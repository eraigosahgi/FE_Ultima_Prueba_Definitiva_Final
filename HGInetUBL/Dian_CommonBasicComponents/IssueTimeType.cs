using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HGInetUBL
{
    /// <comentarios/>
    /*Se cambia el tipo de dato de DateTime a String
     *Se cambia el tipo de Serializacion de DataType="time" a DataType = "language"
     Esto debido a que que la Dian solo necesita la hora en este campo*/
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
    [System.Xml.Serialization.XmlRootAttribute("IssueTime", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", IsNullable = false)]
    public partial class IssueTimeType
    {

        private string valueField;

        /// <comentarios/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "language")]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}