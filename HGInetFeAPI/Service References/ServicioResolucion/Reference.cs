﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HGInetFeAPI.ServicioResolucion {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Resolucion", Namespace="http://schemas.datacontract.org/2004/07/HGInetMiFacturaElectonicaData.ModeloServi" +
        "cio")]
    [System.SerializableAttribute()]
    public partial class Resolucion : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClaveTecnicaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DataKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaResolucionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaVigenciaFinalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime FechaVigenciaInicialField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdentificacionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NumeroResolucionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PrefijoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int RangoFinalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int RangoInicialField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SetIdDianField;
        
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
        public string ClaveTecnica {
            get {
                return this.ClaveTecnicaField;
            }
            set {
                if ((object.ReferenceEquals(this.ClaveTecnicaField, value) != true)) {
                    this.ClaveTecnicaField = value;
                    this.RaisePropertyChanged("ClaveTecnica");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DataKey {
            get {
                return this.DataKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.DataKeyField, value) != true)) {
                    this.DataKeyField = value;
                    this.RaisePropertyChanged("DataKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime FechaResolucion {
            get {
                return this.FechaResolucionField;
            }
            set {
                if ((this.FechaResolucionField.Equals(value) != true)) {
                    this.FechaResolucionField = value;
                    this.RaisePropertyChanged("FechaResolucion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime FechaVigenciaFinal {
            get {
                return this.FechaVigenciaFinalField;
            }
            set {
                if ((this.FechaVigenciaFinalField.Equals(value) != true)) {
                    this.FechaVigenciaFinalField = value;
                    this.RaisePropertyChanged("FechaVigenciaFinal");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime FechaVigenciaInicial {
            get {
                return this.FechaVigenciaInicialField;
            }
            set {
                if ((this.FechaVigenciaInicialField.Equals(value) != true)) {
                    this.FechaVigenciaInicialField = value;
                    this.RaisePropertyChanged("FechaVigenciaInicial");
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
        public string NumeroResolucion {
            get {
                return this.NumeroResolucionField;
            }
            set {
                if ((object.ReferenceEquals(this.NumeroResolucionField, value) != true)) {
                    this.NumeroResolucionField = value;
                    this.RaisePropertyChanged("NumeroResolucion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Prefijo {
            get {
                return this.PrefijoField;
            }
            set {
                if ((object.ReferenceEquals(this.PrefijoField, value) != true)) {
                    this.PrefijoField = value;
                    this.RaisePropertyChanged("Prefijo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RangoFinal {
            get {
                return this.RangoFinalField;
            }
            set {
                if ((this.RangoFinalField.Equals(value) != true)) {
                    this.RangoFinalField = value;
                    this.RaisePropertyChanged("RangoFinal");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RangoInicial {
            get {
                return this.RangoInicialField;
            }
            set {
                if ((this.RangoInicialField.Equals(value) != true)) {
                    this.RangoInicialField = value;
                    this.RaisePropertyChanged("RangoInicial");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SetIdDian {
            get {
                return this.SetIdDianField;
            }
            set {
                if ((object.ReferenceEquals(this.SetIdDianField, value) != true)) {
                    this.SetIdDianField = value;
                    this.RaisePropertyChanged("SetIdDian");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Error", Namespace="http://schemas.datacontract.org/2004/07/LibreriaGlobalHGInet.Error")]
    [System.SerializableAttribute()]
    public partial class Error : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private HGInetFeAPI.ServicioResolucion.CodigoError CodigoField;
        
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
        public HGInetFeAPI.ServicioResolucion.CodigoError Codigo {
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", ConfigurationName="ServicioResolucion.ServicioResolucion")]
    public interface ServicioResolucion {
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/Test", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/TestResponse")]
        HGInetFeAPI.ServicioResolucion.TestResponse Test(HGInetFeAPI.ServicioResolucion.TestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/Test", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/TestResponse")]
        System.Threading.Tasks.Task<HGInetFeAPI.ServicioResolucion.TestResponse> TestAsync(HGInetFeAPI.ServicioResolucion.TestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/Consultar", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/ConsultarResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(HGInetFeAPI.ServicioResolucion.Error), Action="Consultar", Name="Error")]
        HGInetFeAPI.ServicioResolucion.ConsultarResponse Consultar(HGInetFeAPI.ServicioResolucion.ConsultarRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/Consultar", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/ConsultarResponse")]
        System.Threading.Tasks.Task<HGInetFeAPI.ServicioResolucion.ConsultarResponse> ConsultarAsync(HGInetFeAPI.ServicioResolucion.ConsultarRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/ConsultarResolucion", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/ConsultarResolucionRespo" +
            "nse")]
        [System.ServiceModel.FaultContractAttribute(typeof(HGInetFeAPI.ServicioResolucion.Error), Action="ConsultarResolucion", Name="Error")]
        HGInetFeAPI.ServicioResolucion.ConsultarResolucionResponse ConsultarResolucion(HGInetFeAPI.ServicioResolucion.ConsultarResolucionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/ConsultarResolucion", ReplyAction="HGInetFacturaElectronica.ServiciosWcf/ServicioResolucion/ConsultarResolucionRespo" +
            "nse")]
        System.Threading.Tasks.Task<HGInetFeAPI.ServicioResolucion.ConsultarResolucionResponse> ConsultarResolucionAsync(HGInetFeAPI.ServicioResolucion.ConsultarResolucionRequest request);
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
    [System.ServiceModel.MessageContractAttribute(WrapperName="Consultar", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class ConsultarRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public string DataKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=1)]
        public string Identificacion;
        
        public ConsultarRequest() {
        }
        
        public ConsultarRequest(string DataKey, string Identificacion) {
            this.DataKey = DataKey;
            this.Identificacion = Identificacion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ConsultarResponse", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class ConsultarResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public System.Collections.Generic.List<HGInetFeAPI.ServicioResolucion.Resolucion> ConsultarResult;
        
        public ConsultarResponse() {
        }
        
        public ConsultarResponse(System.Collections.Generic.List<HGInetFeAPI.ServicioResolucion.Resolucion> ConsultarResult) {
            this.ConsultarResult = ConsultarResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ConsultarResolucion", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class ConsultarResolucionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public HGInetFeAPI.ServicioResolucion.Resolucion Resolucion;
        
        public ConsultarResolucionRequest() {
        }
        
        public ConsultarResolucionRequest(HGInetFeAPI.ServicioResolucion.Resolucion Resolucion) {
            this.Resolucion = Resolucion;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ConsultarResolucionResponse", WrapperNamespace="HGInetFacturaElectronica.ServiciosWcf", IsWrapped=true)]
    public partial class ConsultarResolucionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="HGInetFacturaElectronica.ServiciosWcf", Order=0)]
        public System.Collections.Generic.List<HGInetFeAPI.ServicioResolucion.Resolucion> ConsultarResolucionResult;
        
        public ConsultarResolucionResponse() {
        }
        
        public ConsultarResolucionResponse(System.Collections.Generic.List<HGInetFeAPI.ServicioResolucion.Resolucion> ConsultarResolucionResult) {
            this.ConsultarResolucionResult = ConsultarResolucionResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServicioResolucionChannel : HGInetFeAPI.ServicioResolucion.ServicioResolucion, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioResolucionClient : System.ServiceModel.ClientBase<HGInetFeAPI.ServicioResolucion.ServicioResolucion>, HGInetFeAPI.ServicioResolucion.ServicioResolucion {
        
        public ServicioResolucionClient() {
        }
        
        public ServicioResolucionClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicioResolucionClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioResolucionClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioResolucionClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public HGInetFeAPI.ServicioResolucion.TestResponse Test(HGInetFeAPI.ServicioResolucion.TestRequest request) {
            return base.Channel.Test(request);
        }
        
        public System.Threading.Tasks.Task<HGInetFeAPI.ServicioResolucion.TestResponse> TestAsync(HGInetFeAPI.ServicioResolucion.TestRequest request) {
            return base.Channel.TestAsync(request);
        }
        
        public HGInetFeAPI.ServicioResolucion.ConsultarResponse Consultar(HGInetFeAPI.ServicioResolucion.ConsultarRequest request) {
            return base.Channel.Consultar(request);
        }
        
        public System.Threading.Tasks.Task<HGInetFeAPI.ServicioResolucion.ConsultarResponse> ConsultarAsync(HGInetFeAPI.ServicioResolucion.ConsultarRequest request) {
            return base.Channel.ConsultarAsync(request);
        }
        
        public HGInetFeAPI.ServicioResolucion.ConsultarResolucionResponse ConsultarResolucion(HGInetFeAPI.ServicioResolucion.ConsultarResolucionRequest request) {
            return base.Channel.ConsultarResolucion(request);
        }
        
        public System.Threading.Tasks.Task<HGInetFeAPI.ServicioResolucion.ConsultarResolucionResponse> ConsultarResolucionAsync(HGInetFeAPI.ServicioResolucion.ConsultarResolucionRequest request) {
            return base.Channel.ConsultarResolucionAsync(request);
        }
    }
}
