﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HGInetDIANServicios.DianResolucion {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.dian.gov.co/servicios/facturaelectronica/ConsultaResolucionFacturacion" +
        "", ConfigurationName="DianResolucion.resolucionFacturacionPortName")]
    public interface resolucionFacturacionPortName {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionRespuesta ConsultaResolucionesFacturacion(HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionPeticion request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="", ReplyAction="*")]
        System.IAsyncResult BeginConsultaResolucionesFacturacion(HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionPeticion request, System.AsyncCallback callback, object asyncState);
        
        HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionRespuesta EndConsultaResolucionesFacturacion(System.IAsyncResult result);
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/servicios/facturaelectronica/ConsultaResolucionFacturacion" +
        "")]
    public partial class ConsultaResoluciones : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string nITObligadoFacturarElectronicamenteField;
        
        private string nITProveedorTecnologicoField;
        
        private string identificadorSoftwareField;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string NITObligadoFacturarElectronicamente {
            get {
                return this.nITObligadoFacturarElectronicamenteField;
            }
            set {
                this.nITObligadoFacturarElectronicamenteField = value;
                this.RaisePropertyChanged("NITObligadoFacturarElectronicamente");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string NITProveedorTecnologico {
            get {
                return this.nITProveedorTecnologicoField;
            }
            set {
                this.nITProveedorTecnologicoField = value;
                this.RaisePropertyChanged("NITProveedorTecnologico");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string IdentificadorSoftware {
            get {
                return this.identificadorSoftwareField;
            }
            set {
                this.identificadorSoftwareField = value;
                this.RaisePropertyChanged("IdentificadorSoftware");
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
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/servicios/facturaelectronica/ConsultaResolucionFacturacion" +
        "")]
    public partial class RangoFacturacion : object, System.ComponentModel.INotifyPropertyChanged {
        
        private long numeroResolucionField;
        
        private System.DateTime fechaResolucionField;
        
        private string prefijoField;
        
        private long rangoInicialField;
        
        private long rangoFinalField;
        
        private System.DateTime fechaVigenciaDesdeField;
        
        private System.DateTime fechaVigenciaHastaField;
        
        private string claveTecnicaField;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public long NumeroResolucion {
            get {
                return this.numeroResolucionField;
            }
            set {
                this.numeroResolucionField = value;
                this.RaisePropertyChanged("NumeroResolucion");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=1)]
        public System.DateTime FechaResolucion {
            get {
                return this.fechaResolucionField;
            }
            set {
                this.fechaResolucionField = value;
                this.RaisePropertyChanged("FechaResolucion");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Prefijo {
            get {
                return this.prefijoField;
            }
            set {
                this.prefijoField = value;
                this.RaisePropertyChanged("Prefijo");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public long RangoInicial {
            get {
                return this.rangoInicialField;
            }
            set {
                this.rangoInicialField = value;
                this.RaisePropertyChanged("RangoInicial");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public long RangoFinal {
            get {
                return this.rangoFinalField;
            }
            set {
                this.rangoFinalField = value;
                this.RaisePropertyChanged("RangoFinal");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=5)]
        public System.DateTime FechaVigenciaDesde {
            get {
                return this.fechaVigenciaDesdeField;
            }
            set {
                this.fechaVigenciaDesdeField = value;
                this.RaisePropertyChanged("FechaVigenciaDesde");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", Order=6)]
        public System.DateTime FechaVigenciaHasta {
            get {
                return this.fechaVigenciaHastaField;
            }
            set {
                this.fechaVigenciaHastaField = value;
                this.RaisePropertyChanged("FechaVigenciaHasta");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string ClaveTecnica {
            get {
                return this.claveTecnicaField;
            }
            set {
                this.claveTecnicaField = value;
                this.RaisePropertyChanged("ClaveTecnica");
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
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/servicios/facturaelectronica/ConsultaResolucionFacturacion" +
        "")]
    public partial class ResolucionesFacturacion : object, System.ComponentModel.INotifyPropertyChanged {
        
        private CodigoType codigoOperacionField;
        
        private string descripcionOperacionField;
        
        private decimal identificadorOperacionField;
        
        private RangoFacturacion[] rangoFacturacionField;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public CodigoType CodigoOperacion {
            get {
                return this.codigoOperacionField;
            }
            set {
                this.codigoOperacionField = value;
                this.RaisePropertyChanged("CodigoOperacion");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string DescripcionOperacion {
            get {
                return this.descripcionOperacionField;
            }
            set {
                this.descripcionOperacionField = value;
                this.RaisePropertyChanged("DescripcionOperacion");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public decimal IdentificadorOperacion {
            get {
                return this.identificadorOperacionField;
            }
            set {
                this.identificadorOperacionField = value;
                this.RaisePropertyChanged("IdentificadorOperacion");
            }
        }
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("RangoFacturacion", Order=3)]
        public RangoFacturacion[] RangoFacturacion {
            get {
                return this.rangoFacturacionField;
            }
            set {
                this.rangoFacturacionField = value;
                this.RaisePropertyChanged("RangoFacturacion");
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
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.dian.gov.co/servicios/facturaelectronica/ConsultaResolucionFacturacion" +
        "")]
    public enum CodigoType {
        
        /// <comentarios/>
        OK,
        
        /// <comentarios/>
        ND,
        
        /// <comentarios/>
        EP,
        
        /// <comentarios/>
        ES,
        
        /// <comentarios/>
        EH,
        
        /// <comentarios/>
        EA,
        
        /// <comentarios/>
        ER,
        
        /// <comentarios/>
        EF,
        
        /// <comentarios/>
        EN,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultaResolucionesFacturacionPeticion {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultaResolucionesFacturacionPeticion", Namespace="http://www.dian.gov.co/servicios/facturaelectronica/ConsultaResolucionFacturacion" +
            "", Order=0)]
        public HGInetDIANServicios.DianResolucion.ConsultaResoluciones ConsultaResolucionesFacturacionPeticion1;
        
        public ConsultaResolucionesFacturacionPeticion() {
        }
        
        public ConsultaResolucionesFacturacionPeticion(HGInetDIANServicios.DianResolucion.ConsultaResoluciones ConsultaResolucionesFacturacionPeticion1) {
            this.ConsultaResolucionesFacturacionPeticion1 = ConsultaResolucionesFacturacionPeticion1;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultaResolucionesFacturacionRespuesta {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultaResolucionesFacturacionRespuesta", Namespace="http://www.dian.gov.co/servicios/facturaelectronica/ConsultaResolucionFacturacion" +
            "", Order=0)]
        public HGInetDIANServicios.DianResolucion.ResolucionesFacturacion ConsultaResolucionesFacturacionRespuesta1;
        
        public ConsultaResolucionesFacturacionRespuesta() {
        }
        
        public ConsultaResolucionesFacturacionRespuesta(HGInetDIANServicios.DianResolucion.ResolucionesFacturacion ConsultaResolucionesFacturacionRespuesta1) {
            this.ConsultaResolucionesFacturacionRespuesta1 = ConsultaResolucionesFacturacionRespuesta1;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface resolucionFacturacionPortNameChannel : HGInetDIANServicios.DianResolucion.resolucionFacturacionPortName, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ConsultaResolucionesFacturacionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public ConsultaResolucionesFacturacionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionRespuesta Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionRespuesta)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class resolucionFacturacionPortNameClient : System.ServiceModel.ClientBase<HGInetDIANServicios.DianResolucion.resolucionFacturacionPortName>, HGInetDIANServicios.DianResolucion.resolucionFacturacionPortName {
        
        private BeginOperationDelegate onBeginConsultaResolucionesFacturacionDelegate;
        
        private EndOperationDelegate onEndConsultaResolucionesFacturacionDelegate;
        
        private System.Threading.SendOrPostCallback onConsultaResolucionesFacturacionCompletedDelegate;
        
        public resolucionFacturacionPortNameClient() {
        }
        
        public resolucionFacturacionPortNameClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public resolucionFacturacionPortNameClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public resolucionFacturacionPortNameClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public resolucionFacturacionPortNameClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<ConsultaResolucionesFacturacionCompletedEventArgs> ConsultaResolucionesFacturacionCompleted;
        
        public HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionRespuesta ConsultaResolucionesFacturacion(HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionPeticion request) {
            return base.Channel.ConsultaResolucionesFacturacion(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginConsultaResolucionesFacturacion(HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionPeticion request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginConsultaResolucionesFacturacion(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionRespuesta EndConsultaResolucionesFacturacion(System.IAsyncResult result) {
            return base.Channel.EndConsultaResolucionesFacturacion(result);
        }
        
        private System.IAsyncResult OnBeginConsultaResolucionesFacturacion(object[] inValues, System.AsyncCallback callback, object asyncState) {
            HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionPeticion request = ((HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionPeticion)(inValues[0]));
            return this.BeginConsultaResolucionesFacturacion(request, callback, asyncState);
        }
        
        private object[] OnEndConsultaResolucionesFacturacion(System.IAsyncResult result) {
            HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionRespuesta retVal = this.EndConsultaResolucionesFacturacion(result);
            return new object[] {
                    retVal};
        }
        
        private void OnConsultaResolucionesFacturacionCompleted(object state) {
            if ((this.ConsultaResolucionesFacturacionCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.ConsultaResolucionesFacturacionCompleted(this, new ConsultaResolucionesFacturacionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void ConsultaResolucionesFacturacionAsync(HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionPeticion request) {
            this.ConsultaResolucionesFacturacionAsync(request, null);
        }
        
        public void ConsultaResolucionesFacturacionAsync(HGInetDIANServicios.DianResolucion.ConsultaResolucionesFacturacionPeticion request, object userState) {
            if ((this.onBeginConsultaResolucionesFacturacionDelegate == null)) {
                this.onBeginConsultaResolucionesFacturacionDelegate = new BeginOperationDelegate(this.OnBeginConsultaResolucionesFacturacion);
            }
            if ((this.onEndConsultaResolucionesFacturacionDelegate == null)) {
                this.onEndConsultaResolucionesFacturacionDelegate = new EndOperationDelegate(this.OnEndConsultaResolucionesFacturacion);
            }
            if ((this.onConsultaResolucionesFacturacionCompletedDelegate == null)) {
                this.onConsultaResolucionesFacturacionCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnConsultaResolucionesFacturacionCompleted);
            }
            base.InvokeAsync(this.onBeginConsultaResolucionesFacturacionDelegate, new object[] {
                        request}, this.onEndConsultaResolucionesFacturacionDelegate, this.onConsultaResolucionesFacturacionCompletedDelegate, userState);
        }
    }
}
