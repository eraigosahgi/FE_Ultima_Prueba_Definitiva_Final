//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HGInetMiFacturaElectonicaData.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class TblEmpresas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TblEmpresas()
        {
            this.TblEmpresasAdquiriente = new HashSet<TblDocumentos>();
            this.TblEmpresasFacturador = new HashSet<TblDocumentos>();
            this.TblFormatos = new HashSet<TblFormatos>();
            this.TblOpcionesUsuario = new HashSet<TblOpcionesUsuario>();
            this.TblUsuarios = new HashSet<TblUsuarios>();
            this.TblEmpresasResoluciones = new HashSet<TblEmpresasResoluciones>();
            this.TblPagosElectronicos = new HashSet<TblPagosElectronicos>();
            this.TblPagosElectronicos1 = new HashSet<TblPagosElectronicos>();
            this.TblPlanesTransacciones = new HashSet<TblPlanesTransacciones>();
            this.TblEmpresaSucursal = new HashSet<TblEmpresaSucursal>();
            this.TblEmpresaIntegradores = new HashSet<TblEmpresaIntegradores>();
        }
    
        public string StrIdentificacion { get; set; }
        public short IntIdentificacionDv { get; set; }
        public string StrTipoIdentificacion { get; set; }
        public string StrRazonSocial { get; set; }
        public string StrMailAdmin { get; set; }
        public System.DateTime DatFechaIngreso { get; set; }
        public string StrObservaciones { get; set; }
        public string StrSerial { get; set; }
        public bool IntAdquiriente { get; set; }
        public bool IntObligado { get; set; }
        public bool IntAdministrador { get; set; }
        public bool IntIntegrador { get; set; }
        public string StrResolucionDian { get; set; }
        public System.DateTime DatFechaActualizacion { get; set; }
        public System.Guid StrIdSeguridad { get; set; }
        public Nullable<byte> IntHabilitacion { get; set; }
        public string StrEmpresaAsociada { get; set; }
        public int IntNumUsuarios { get; set; }
        public string StrTelefono { get; set; }
        public Nullable<short> IntAcuseTacito { get; set; }
        public bool IntManejaAnexos { get; set; }
        public bool IntEnvioMailRecepcion { get; set; }
        public string StrEmpresaDescuento { get; set; }
        public short IntIdEstado { get; set; }
        public short IntCobroPostPago { get; set; }
        public string StrMailEnvio { get; set; }
        public string StrMailRecepcion { get; set; }
        public string StrMailAcuse { get; set; }
        public string StrMailPagos { get; set; }
        public Nullable<short> IntTimeout { get; set; }
        public short IntVersionDian { get; set; }
        public Nullable<bool> IntCertResponsableHGI { get; set; }
        public Nullable<bool> IntCertNotificar { get; set; }
        public string StrCertRuta { get; set; }
        public string StrCertClave { get; set; }
        public Nullable<short> IntCertProveedor { get; set; }
        public Nullable<System.DateTime> DatCertVence { get; set; }
        public short IntMailEnvioVerificado { get; set; }
        public short IntMailAdminVerificado { get; set; }
        public short IntMailRecepcionVerificado { get; set; }
        public short IntMailAcuseVerificado { get; set; }
        public short IntMailPagosVerificado { get; set; }
        public short IntCertFirma { get; set; }
        public string StrSerialCloudServices { get; set; }
        public Nullable<bool> IntDebug { get; set; }
        public bool IntPdfCampoDian { get; set; }
        public decimal IntPdfCampoDianPosX { get; set; }
        public decimal IntPdfCampoDianPosY { get; set; }
        public bool IntManejaPagoE { get; set; }
        public bool IntPagoEParcial { get; set; }
        public Nullable<byte> IntHabilitacionNomina { get; set; }
        public bool IntPagosPermiteConsTodos { get; set; }
        public Nullable<System.Guid> ComercioConfigId { get; set; }
        public string ComercioConfigDescrip { get; set; }
        public bool IntInteroperabilidad { get; set; }
        public bool IntRadian { get; set; }
        public bool IntEnvioNominaMail { get; set; }
        public short IntTipoPlan { get; set; }
        public bool IntCompraPlan { get; set; }
        public bool IntFacturaE { get; set; }
        public bool IntNominaE { get; set; }
        public bool IntDocSoporte { get; set; }
        public string StrNombresRep { get; set; }
        public string StrApellidosRep { get; set; }
        public string StrIdentificacionRep { get; set; }
        public string StrTipoIdentificacionRep { get; set; }
        public string StrCargo { get; set; }
        public int IntTipoOperador { get; set; }
        public bool IntEnvioSms { get; set; }
        public int IntValidacionVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblDocumentos> TblEmpresasAdquiriente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblDocumentos> TblEmpresasFacturador { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblFormatos> TblFormatos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblOpcionesUsuario> TblOpcionesUsuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblUsuarios> TblUsuarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblEmpresasResoluciones> TblEmpresasResoluciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblPagosElectronicos> TblPagosElectronicos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblPagosElectronicos> TblPagosElectronicos1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblPlanesTransacciones> TblPlanesTransacciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblEmpresaSucursal> TblEmpresaSucursal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblEmpresaIntegradores> TblEmpresaIntegradores { get; set; }
        public virtual TblIntegradores TblIntegradores { get; set; }
    }
}
