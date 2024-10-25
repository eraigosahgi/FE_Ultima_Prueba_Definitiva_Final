using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBL
{
    /// <comentarios/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.mifacturaenlinea.com.co/v1/Structures")]
    //[System.Xml.Serialization.XmlRootAttribute("HgiSasExtensions", Namespace = "http://www.mifacturaenlinea.com.co/v1/Structures", IsNullable = false)]
    public partial class HGInet
    {
        private DescriptionType application;

        private DescriptionType descriptionField;

        private CodeType idSeguridad;

        private DescriptionType pdfFormat;


        /// <comentarios/>
        //[System.Xml.Serialization.XmlElementAttribute("Description", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public DescriptionType Aplicacion
        {
            get
            {
                return this.application;
            }
            set
            {
                this.application = value;
            }
        }


        /// <comentarios/>
        //[System.Xml.Serialization.XmlElementAttribute("Description", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public DescriptionType ProveedorTecnologico
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
        //[System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public CodeType IdSeguridad
        {
            get
            {
                return this.idSeguridad;
            }
            set
            {
                this.idSeguridad = value;
            }
        }


        //[System.Xml.Serialization.XmlElementAttribute("PdfFormat", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2")]
        public DescriptionType PdfFormat
        {
            get
            {
                return this.pdfFormat;
            }
            set
            {
                this.pdfFormat = value;
            }
        }

    }
}
