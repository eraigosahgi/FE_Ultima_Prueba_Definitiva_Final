﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HGInetFeAPI.ServicioEmpresas {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Empresa", Namespace="http://schemas.datacontract.org/2004/07/HGInetMiFacturaElectonicaData.ModeloServi" +
        "cio")]
    [System.SerializableAttribute()]
    public partial class Empresa : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAcuseField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailAdminField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailEnvioField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailPagosField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailRecepcionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailRecepcionDianField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool FacturaEField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short HorasAcuseTacitoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdentificacionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdentificacionDvField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Identificacion_EmpresaEmisorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ManejaAnexoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool NominaEField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PinSoftwareField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RazonSocialField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TelefonoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int TipoIdentificacionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ValidacionVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int VersionDianField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string idseguridad_EmpresaEmisorField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailAcuse {
            get {
                return this.EmailAcuseField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailAcuseField, value) != true)) {
                    this.EmailAcuseField = value;
                    this.RaisePropertyChanged("EmailAcuse");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailAdmin {
            get {
                return this.EmailAdminField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailAdminField, value) != true)) {
                    this.EmailAdminField = value;
                    this.RaisePropertyChanged("EmailAdmin");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailEnvio {
            get {
                return this.EmailEnvioField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailEnvioField, value) != true)) {
                    this.EmailEnvioField = value;
                    this.RaisePropertyChanged("EmailEnvio");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailPagos {
            get {
                return this.EmailPagosField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailPagosField, value) != true)) {
                    this.EmailPagosField = value;
                    this.RaisePropertyChanged("EmailPagos");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailRecepcion {
            get {
                return this.EmailRecepcionField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailRecepcionField, value) != true)) {
                    this.EmailRecepcionField = value;
                    this.RaisePropertyChanged("EmailRecepcion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailRecepcionDian {
            get {
                return this.EmailRecepcionDianField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailRecepcionDianField, value) != true)) {
                    this.EmailRecepcionDianField = value;
                    this.RaisePropertyChanged("EmailRecepcionDian");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool FacturaE {
            get {
                return this.FacturaEField;
            }
            set {
                if ((this.FacturaEField.Equals(value) != true)) {
                    this.FacturaEField = value;
                    this.RaisePropertyChanged("FacturaE");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short HorasAcuseTacito {
            get {
                return this.HorasAcuseTacitoField;
            }
            set {
                if ((this.HorasAcuseTacitoField.Equals(value) != true)) {
                    this.HorasAcuseTacitoField = value;
                    this.RaisePropertyChanged("HorasAcuseTacito");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Identificacion {
            get {
                return this.IdentificacionField;
            }
            set {
                if ((object.ReferenceEquals(this.IdentificacionField, value) != true)) {
                    this.IdentificacionField = value;
                    this.RaisePropertyChanged("Identificacion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IdentificacionDv {
            get {
                return this.IdentificacionDvField;
            }
            set {
                if ((this.IdentificacionDvField.Equals(value) != true)) {
                    this.IdentificacionDvField = value;
                    this.RaisePropertyChanged("IdentificacionDv");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Identificacion_EmpresaEmisor {
            get {
                return this.Identificacion_EmpresaEmisorField;
            }
            set {
                if ((object.ReferenceEquals(this.Identificacion_EmpresaEmisorField, value) != true)) {
                    this.Identificacion_EmpresaEmisorField = value;
                    this.RaisePropertyChanged("Identificacion_EmpresaEmisor");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ManejaAnexo {
            get {
                return this.ManejaAnexoField;
            }
            set {
                if ((this.ManejaAnexoField.Equals(value) != true)) {
                    this.ManejaAnexoField = value;
                    this.RaisePropertyChanged("ManejaAnexo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool NominaE {
            get {
                return this.NominaEField;
            }
            set {
                if ((this.NominaEField.Equals(value) != true)) {
                    this.NominaEField = value;
                    this.RaisePropertyChanged("NominaE");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PinSoftware {
            get {
                return this.PinSoftwareField;
            }
            set {
                if ((object.ReferenceEquals(this.PinSoftwareField, value) != true)) {
                    this.PinSoftwareField = value;
                    this.RaisePropertyChanged("PinSoftware");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RazonSocial {
            get {
                return this.RazonSocialField;
            }
            set {
                if ((object.ReferenceEquals(this.RazonSocialField, value) != true)) {
                    this.RazonSocialField = value;
                    this.RaisePropertyChanged("RazonSocial");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Telefono {
            get {
                return this.TelefonoField;
            }
            set {
                if ((object.ReferenceEquals(this.TelefonoField, value) != true)) {
                    this.TelefonoField = value;
                    this.RaisePropertyChanged("Telefono");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TipoIdentificacion {
            get {
                return this.TipoIdentificacionField;
            }
            set {
                if ((this.TipoIdentificacionField.Equals(value) != true)) {
                    this.TipoIdentificacionField = value;
                    this.RaisePropertyChanged("TipoIdentificacion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ValidacionVersion {
            get {
                return this.ValidacionVersionField;
            }
            set {
                if ((this.ValidacionVersionField.Equals(value) != true)) {
                    this.ValidacionVersionField = value;
                    this.RaisePropertyChanged("ValidacionVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int VersionDian {
            get {
                return this.VersionDianField;
            }
            set {
                if ((this.VersionDianField.Equals(value) != true)) {
                    this.VersionDianField = value;
                    this.RaisePropertyChanged("VersionDian");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string idseguridad_EmpresaEmisor {
            get {
                return this.idseguridad_EmpresaEmisorField;
            }
            set {
                if ((object.ReferenceEquals(this.idseguridad_EmpresaEmisorField, value) != true)) {
                    this.idseguridad_EmpresaEmisorField = value;
                    this.RaisePropertyChanged("idseguridad_EmpresaEmisor");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Error", Namespace="http://schemas.datacontract.org/2004/07/LibreriaGlobalHGInet.Error")]
    [System.SerializableAttribute()]
    public partial class Error : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private HGInetFeAPI.ServicioEmpresas.CodigoError CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MensajeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public HGInetFeAPI.ServicioEmpresas.CodigoError Codigo {
            get {
                return this.CodigoField;
            }
            set {
                if ((this.CodigoField.Equals(value) != true)) {
                    this.CodigoField = value;
                    this.RaisePropertyChanged("Codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Fecha {
            get {
                return this.FechaField;
            }
            set {
                if ((this.FechaField.Equals(value) != true)) {
                    this.FechaField = value;
                    this.RaisePropertyChanged("Fecha");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Mensaje {
            get {
                return this.MensajeField;
            }
            set {
                if ((object.ReferenceEquals(this.MensajeField, value) != true)) {
                    this.MensajeField = value;
                    this.RaisePropertyChanged("Mensaje");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CodigoError", Namespace="http://schemas.datacontract.org/2004/07/LibreriaGlobalHGInet.Error")]
    public enum CodigoError : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OK = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ERROR_NO_CONTROLADO = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ERROR_EN_SERVIDOR = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        VALIDACION = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ERROR_AGREGAR = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ERROR_EDITAR = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ERROR_ELIMINAR = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ERROR_LICENCIA = 98,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NINGUNO = 99,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", ConfigurationName="ServicioEmpresas.ServicioEmpresas")]
    public interface ServicioEmpresas {
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Test", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/TestResponse")]
        HGInetFeAPI.ServicioEmpresas.TestResponse Test(HGInetFeAPI.ServicioEmpresas.TestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Test", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/TestResponse")]
        System.Threading.Tasks.Task<HGInetFeAPI.ServicioEmpresas.TestResponse> TestAsync(HGInetFeAPI.ServicioEmpresas.TestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Obtener", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/ObtenerResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(HGInetFeAPI.ServicioEmpresas.Error), Action="Obtener", Name="Error")]
        HGInetFeAPI.ServicioEmpresas.ObtenerResponse Obtener(HGInetFeAPI.ServicioEmpresas.ObtenerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Obtener", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/ObtenerResponse")]
        System.Threading.Tasks.Task<HGInetFeAPI.ServicioEmpresas.ObtenerResponse> ObtenerAsync(HGInetFeAPI.ServicioEmpresas.ObtenerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/ConsultarAdquiriente", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/ConsultarAdquirienteRespon" +
            "se")]
        [System.ServiceModel.FaultContractAttribute(typeof(HGInetFeAPI.ServicioEmpresas.Error), Action="ConsultarAdquiriente", Name="Error")]
        HGInetFeAPI.ServicioEmpresas.ConsultarAdquirienteResponse ConsultarAdquiriente(HGInetFeAPI.ServicioEmpresas.ConsultarAdquirienteRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/ConsultarAdquiriente", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/ConsultarAdquirienteRespon" +
            "se")]
        System.Threading.Tasks.Task<HGInetFeAPI.ServicioEmpresas.ConsultarAdquirienteResponse> ConsultarAdquirienteAsync(HGInetFeAPI.ServicioEmpresas.ConsultarAdquirienteRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Crear", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/CrearResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(HGInetFeAPI.ServicioEmpresas.Error), Action="Crear", Name="Error")]
        HGInetFeAPI.ServicioEmpresas.CrearResponse Crear(HGInetFeAPI.ServicioEmpresas.CrearRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Crear", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/CrearResponse")]
        System.Threading.Tasks.Task<HGInetFeAPI.ServicioEmpresas.CrearResponse> CrearAsync(HGInetFeAPI.ServicioEmpresas.CrearRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Test", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class TestRequest {
        
        public TestRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="TestResponse", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class TestResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public string TestResult;
        
        public TestResponse() {
        }
        
        public TestResponse(string TestResult) {
            this.TestResult = TestResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Obtener", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class ObtenerRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public string DataKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=1)]
        public string Identificacion;
        
        public ObtenerRequest() {
        }
        
        public ObtenerRequest(string DataKey, string Identificacion) {
            this.DataKey = DataKey;
            this.Identificacion = Identificacion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ObtenerResponse", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class ObtenerResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public HGInetFeAPI.ServicioEmpresas.Empresa ObtenerResult;
        
        public ObtenerResponse() {
        }
        
        public ObtenerResponse(HGInetFeAPI.ServicioEmpresas.Empresa ObtenerResult) {
            this.ObtenerResult = ObtenerResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ConsultarAdquiriente", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class ConsultarAdquirienteRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public string DataKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=1)]
        public string IdentificacionEmisor;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=2)]
        public string IdentificacionAdquiriente;
        
        public ConsultarAdquirienteRequest() {
        }
        
        public ConsultarAdquirienteRequest(string DataKey, string IdentificacionEmisor, string IdentificacionAdquiriente) {
            this.DataKey = DataKey;
            this.IdentificacionEmisor = IdentificacionEmisor;
            this.IdentificacionAdquiriente = IdentificacionAdquiriente;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ConsultarAdquirienteResponse", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class ConsultarAdquirienteResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public HGInetFeAPI.ServicioEmpresas.Empresa ConsultarAdquirienteResult;
        
        public ConsultarAdquirienteResponse() {
        }
        
        public ConsultarAdquirienteResponse(HGInetFeAPI.ServicioEmpresas.Empresa ConsultarAdquirienteResult) {
            this.ConsultarAdquirienteResult = ConsultarAdquirienteResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Crear", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class CrearRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public HGInetFeAPI.ServicioEmpresas.Empresa empresa_nueva;
        
        public CrearRequest() {
        }
        
        public CrearRequest(HGInetFeAPI.ServicioEmpresas.Empresa empresa_nueva) {
            this.empresa_nueva = empresa_nueva;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CrearResponse", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class CrearResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public bool CrearResult;
        
        public CrearResponse() {
        }
        
        public CrearResponse(bool CrearResult) {
            this.CrearResult = CrearResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServicioEmpresasChannel : HGInetFeAPI.ServicioEmpresas.ServicioEmpresas, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioEmpresasClient : System.ServiceModel.ClientBase<HGInetFeAPI.ServicioEmpresas.ServicioEmpresas>, HGInetFeAPI.ServicioEmpresas.ServicioEmpresas {
        
        public ServicioEmpresasClient() {
        }
        
        public ServicioEmpresasClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicioEmpresasClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioEmpresasClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioEmpresasClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public HGInetFeAPI.ServicioEmpresas.TestResponse Test(HGInetFeAPI.ServicioEmpresas.TestRequest request) {
            return base.Channel.Test(request);
        }
        
        public System.Threading.Tasks.Task<HGInetFeAPI.ServicioEmpresas.TestResponse> TestAsync(HGInetFeAPI.ServicioEmpresas.TestRequest request) {
            return base.Channel.TestAsync(request);
        }
        
        public HGInetFeAPI.ServicioEmpresas.ObtenerResponse Obtener(HGInetFeAPI.ServicioEmpresas.ObtenerRequest request) {
            return base.Channel.Obtener(request);
        }
        
        public System.Threading.Tasks.Task<HGInetFeAPI.ServicioEmpresas.ObtenerResponse> ObtenerAsync(HGInetFeAPI.ServicioEmpresas.ObtenerRequest request) {
            return base.Channel.ObtenerAsync(request);
        }
        
        public HGInetFeAPI.ServicioEmpresas.ConsultarAdquirienteResponse ConsultarAdquiriente(HGInetFeAPI.ServicioEmpresas.ConsultarAdquirienteRequest request) {
            return base.Channel.ConsultarAdquiriente(request);
        }
        
        public System.Threading.Tasks.Task<HGInetFeAPI.ServicioEmpresas.ConsultarAdquirienteResponse> ConsultarAdquirienteAsync(HGInetFeAPI.ServicioEmpresas.ConsultarAdquirienteRequest request) {
            return base.Channel.ConsultarAdquirienteAsync(request);
        }
        
        public HGInetFeAPI.ServicioEmpresas.CrearResponse Crear(HGInetFeAPI.ServicioEmpresas.CrearRequest request) {
            return base.Channel.Crear(request);
        }
        
        public System.Threading.Tasks.Task<HGInetFeAPI.ServicioEmpresas.CrearResponse> CrearAsync(HGInetFeAPI.ServicioEmpresas.CrearRequest request) {
            return base.Channel.CrearAsync(request);
        }
    }
}