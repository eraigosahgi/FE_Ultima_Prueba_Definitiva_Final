﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión del motor en tiempo de ejecución:2.0.50727.8669
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// Este código fuente fue generado automáticamente por xsd, Versión=2.0.50727.3038.
// 


/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/ContextValueAssociation/1.0/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://docs.oasis-open.org/codelist/ns/ContextValueAssociation/1.0/", IsNullable=false)]
public partial class ContextValueAssociation {
    
    private Annotation annotationField;
    
    private Title titleField;
    
    private Include[] includeField;
    
    private ValueTests valueTestsField;
    
    private ValueLists valueListsField;
    
    private InstanceMetadataSets instanceMetadataSetsField;
    
    private Contexts contextsField;
    
    private string idField;
    
    private string nameField;
    
    private string versionField;
    
    private string queryBindingField;
    
    private string baseField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Annotation Annotation {
        get {
            return this.annotationField;
        }
        set {
            this.annotationField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Title Title {
        get {
            return this.titleField;
        }
        set {
            this.titleField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("Include", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Include[] Include {
        get {
            return this.includeField;
        }
        set {
            this.includeField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ValueTests ValueTests {
        get {
            return this.valueTestsField;
        }
        set {
            this.valueTestsField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ValueLists ValueLists {
        get {
            return this.valueListsField;
        }
        set {
            this.valueListsField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public InstanceMetadataSets InstanceMetadataSets {
        get {
            return this.instanceMetadataSetsField;
        }
        set {
            this.instanceMetadataSetsField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Contexts Contexts {
        get {
            return this.contextsField;
        }
        set {
            this.contextsField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="NMTOKEN")]
    public string name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string version {
        get {
            return this.versionField;
        }
        set {
            this.versionField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="NMTOKEN")]
    public string queryBinding {
        get {
            return this.queryBindingField;
        }
        set {
            this.queryBindingField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
    public string @base {
        get {
            return this.baseField;
        }
        set {
            this.baseField = value;
        }
    }
}