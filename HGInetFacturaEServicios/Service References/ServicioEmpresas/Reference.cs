﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HGInetFacturaEServicios.ServicioEmpresas {
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
        private short HorasAcuseTacitoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdentificacionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdentificacionDvField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ManejaAnexoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PinSoftwareField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RazonSocialField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TelefonoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int VersionDianField;
        
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
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", ConfigurationName="ServicioEmpresas.ServicioEmpresas")]
    public interface ServicioEmpresas {
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Test", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/TestResponse")]
        HGInetFacturaEServicios.ServicioEmpresas.TestResponse Test(HGInetFacturaEServicios.ServicioEmpresas.TestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Test", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/TestResponse")]
        System.IAsyncResult BeginTest(HGInetFacturaEServicios.ServicioEmpresas.TestRequest request, System.AsyncCallback callback, object asyncState);
        
        HGInetFacturaEServicios.ServicioEmpresas.TestResponse EndTest(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Obtener", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/ObtenerResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(LibreriaGlobalHGInet.Error.Error), Action="Obtener", Name="Error")]
        HGInetFacturaEServicios.ServicioEmpresas.ObtenerResponse Obtener(HGInetFacturaEServicios.ServicioEmpresas.ObtenerRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/Obtener", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioEmpresas/ObtenerResponse")]
        System.IAsyncResult BeginObtener(HGInetFacturaEServicios.ServicioEmpresas.ObtenerRequest request, System.AsyncCallback callback, object asyncState);
        
        HGInetFacturaEServicios.ServicioEmpresas.ObtenerResponse EndObtener(System.IAsyncResult result);
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
        public HGInetFacturaEServicios.ServicioEmpresas.Empresa ObtenerResult;
        
        public ObtenerResponse() {
        }
        
        public ObtenerResponse(HGInetFacturaEServicios.ServicioEmpresas.Empresa ObtenerResult) {
            this.ObtenerResult = ObtenerResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServicioEmpresasChannel : HGInetFacturaEServicios.ServicioEmpresas.ServicioEmpresas, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public TestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public HGInetFacturaEServicios.ServicioEmpresas.TestResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((HGInetFacturaEServicios.ServicioEmpresas.TestResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ObtenerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public ObtenerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public HGInetFacturaEServicios.ServicioEmpresas.ObtenerResponse Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((HGInetFacturaEServicios.ServicioEmpresas.ObtenerResponse)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioEmpresasClient : System.ServiceModel.ClientBase<HGInetFacturaEServicios.ServicioEmpresas.ServicioEmpresas>, HGInetFacturaEServicios.ServicioEmpresas.ServicioEmpresas {
        
        private BeginOperationDelegate onBeginTestDelegate;
        
        private EndOperationDelegate onEndTestDelegate;
        
        private System.Threading.SendOrPostCallback onTestCompletedDelegate;
        
        private BeginOperationDelegate onBeginObtenerDelegate;
        
        private EndOperationDelegate onEndObtenerDelegate;
        
        private System.Threading.SendOrPostCallback onObtenerCompletedDelegate;
        
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
        
        public event System.EventHandler<TestCompletedEventArgs> TestCompleted;
        
        public event System.EventHandler<ObtenerCompletedEventArgs> ObtenerCompleted;
        
        public HGInetFacturaEServicios.ServicioEmpresas.TestResponse Test(HGInetFacturaEServicios.ServicioEmpresas.TestRequest request) {
            return base.Channel.Test(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginTest(HGInetFacturaEServicios.ServicioEmpresas.TestRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginTest(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public HGInetFacturaEServicios.ServicioEmpresas.TestResponse EndTest(System.IAsyncResult result) {
            return base.Channel.EndTest(result);
        }
        
        private System.IAsyncResult OnBeginTest(object[] inValues, System.AsyncCallback callback, object asyncState) {
            HGInetFacturaEServicios.ServicioEmpresas.TestRequest request = ((HGInetFacturaEServicios.ServicioEmpresas.TestRequest)(inValues[0]));
            return this.BeginTest(request, callback, asyncState);
        }
        
        private object[] OnEndTest(System.IAsyncResult result) {
            HGInetFacturaEServicios.ServicioEmpresas.TestResponse retVal = this.EndTest(result);
            return new object[] {
                    retVal};
        }
        
        private void OnTestCompleted(object state) {
            if ((this.TestCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.TestCompleted(this, new TestCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void TestAsync(HGInetFacturaEServicios.ServicioEmpresas.TestRequest request) {
            this.TestAsync(request, null);
        }
        
        public void TestAsync(HGInetFacturaEServicios.ServicioEmpresas.TestRequest request, object userState) {
            if ((this.onBeginTestDelegate == null)) {
                this.onBeginTestDelegate = new BeginOperationDelegate(this.OnBeginTest);
            }
            if ((this.onEndTestDelegate == null)) {
                this.onEndTestDelegate = new EndOperationDelegate(this.OnEndTest);
            }
            if ((this.onTestCompletedDelegate == null)) {
                this.onTestCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnTestCompleted);
            }
            base.InvokeAsync(this.onBeginTestDelegate, new object[] {
                        request}, this.onEndTestDelegate, this.onTestCompletedDelegate, userState);
        }
        
        public HGInetFacturaEServicios.ServicioEmpresas.ObtenerResponse Obtener(HGInetFacturaEServicios.ServicioEmpresas.ObtenerRequest request) {
            return base.Channel.Obtener(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginObtener(HGInetFacturaEServicios.ServicioEmpresas.ObtenerRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginObtener(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public HGInetFacturaEServicios.ServicioEmpresas.ObtenerResponse EndObtener(System.IAsyncResult result) {
            return base.Channel.EndObtener(result);
        }
        
        private System.IAsyncResult OnBeginObtener(object[] inValues, System.AsyncCallback callback, object asyncState) {
            HGInetFacturaEServicios.ServicioEmpresas.ObtenerRequest request = ((HGInetFacturaEServicios.ServicioEmpresas.ObtenerRequest)(inValues[0]));
            return this.BeginObtener(request, callback, asyncState);
        }
        
        private object[] OnEndObtener(System.IAsyncResult result) {
            HGInetFacturaEServicios.ServicioEmpresas.ObtenerResponse retVal = this.EndObtener(result);
            return new object[] {
                    retVal};
        }
        
        private void OnObtenerCompleted(object state) {
            if ((this.ObtenerCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.ObtenerCompleted(this, new ObtenerCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void ObtenerAsync(HGInetFacturaEServicios.ServicioEmpresas.ObtenerRequest request) {
            this.ObtenerAsync(request, null);
        }
        
        public void ObtenerAsync(HGInetFacturaEServicios.ServicioEmpresas.ObtenerRequest request, object userState) {
            if ((this.onBeginObtenerDelegate == null)) {
                this.onBeginObtenerDelegate = new BeginOperationDelegate(this.OnBeginObtener);
            }
            if ((this.onEndObtenerDelegate == null)) {
                this.onEndObtenerDelegate = new EndOperationDelegate(this.OnEndObtener);
            }
            if ((this.onObtenerCompletedDelegate == null)) {
                this.onObtenerCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnObtenerCompleted);
            }
            base.InvokeAsync(this.onBeginObtenerDelegate, new object[] {
                        request}, this.onEndObtenerDelegate, this.onObtenerCompletedDelegate, userState);
        }
    }
}