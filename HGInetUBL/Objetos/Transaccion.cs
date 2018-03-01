using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HGInetUBL.Objetos
{
    internal class xTransaccion
    {
        #region Propiedades
        private short _empresa;
        private string _codigo;
        private string _prefijo;
        private string _resolucion;
        private DateTime _fecha_final_resolucion;
        private DateTime _fecha_inicial_resolucion;
        private string _identificador_dian;
        private string _pin_dian;
        private short _iva_incluido;
        private short _tipo_factura;//F
        private short _tipo_transaccion;
        private int _documento_inicial;
        private int _documento_final;
        private string _clave_tecnica;
        private string _clave_ambiente;
        private string ruta_servicio;

        //private short _estado;
        //private string _descripcion;

        //private short _descuento;
        //private short _Imprime_Cantidad_0;
        //private short _captura_descuento;
        //private short _captura_por_descuento;
        //private short _valida_por_descuento;
        //private string _resolucion;
        //private string _observaciones;
        //private int _observacion_tercero;
        //private short _maneja_consecutivo;
        //private short _visible;
        //private short _fletes;
        //private short _flete_No_Bases;
        //private string _bodega_defecto;
        //private string _tercero_defecto;
        //private short _controla_fecha;
        //private int _cuadre;
        //private short _bloqueo_fecha;     

        //private short _control_rangos;
        //private int _control_existencias;
        //private int _plazo_documento;
        //private TransaccionOpcionExtra _permiso_transaccion;
        //private short _cuenta_por_cobrar;
        //private short _cartera_predeterminada;
        //private short precio_unitario;
        //private int _calculo_iva;
        //private string _tipo_recibo_pago;
        //private short origen_concepto_pago;
        //private short _tercero_ref_cia;
        //private string _nombre_tercero_ref;
        //private string _factor_descripcion;
        //private short _factor_habilita;
        //private short _factor_valida_cantidad;
        //private short _factor_total_cantidad;
        //private short _factor_no_cantidad;
        //private short _ajuste;
        //private short _punto_venta;
        //private short _valida_doc_ref;
        //private short _maneja_puntos;
        //private short _bodega_detalle;
        //private short _vinculado_detalle;

        /// <summary>
        /// codigo de la empresa
        /// </summary>
        public short CodigoEmpresa
        {
            get { return _empresa; }
            set { _empresa = value; }
        }

        /// <summary>
        /// codigo transacción
        /// </summary>
        public string Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// prefijo de la resoucion de facturación
        /// </summary>
        public string Prefijo
        {
            get { return _prefijo; }
            set { _prefijo = value; }
        }

        /// <summary>
        /// resolucion de facturación
        /// </summary>
        public string Resolucion
        {
            get { return _resolucion; }
            set { _resolucion = value; }
        }

        /// <summary>
        /// Fecha inicial de resolucion de la resolución
        /// </summary>      
        public DateTime FechaIniResolucion
        {
            get { return _fecha_inicial_resolucion; }
            set { _fecha_inicial_resolucion = value; }
        }

        /// <summary>
        /// Fecha final de resolucion de la resolución
        /// </summary>
        public DateTime FechaFinResolucion
        {
            get { return _fecha_final_resolucion; }
            set { _fecha_final_resolucion = value; }
        }

        /// <summary>
        /// Identificador del software proporcionado en la plataforma de la Dian
        /// </summary>
        public string IdentificadorDian
        {
            get { return _identificador_dian; }
            set { _identificador_dian = value; }
        }

        /// <summary>
        /// Pin del software asignado en la plataforma Dian
        /// </summary>
        public string PinDian
        {
            get { return _pin_dian; }
            set { _pin_dian = value; }
        }

         /// <summary>
        /// Ruta del servcio de la Dian
        /// </summary>
        public string RutaServicioDian
        {
            get { return ruta_servicio; }
            set { ruta_servicio = value; }
        }
        

        /// <summary>
        /// Clave tecnica asignada en la plataforma Dian
        /// </summary>
        public string ClaveTecnicaDian
        {
            get { return _clave_tecnica; }
            set { _clave_tecnica = value; }
        }

        /// <summary>
        /// Clave del ambiente de pruebas Dian
        /// </summary>
        public string ClaveAmbienteDian
        {
            get { return _clave_ambiente; }
            set { _clave_ambiente = value; }
        }

        /// <summary>
        /// iva incluido
        /// </summary>
        public short IvaIncluido
        {
            get { return _iva_incluido; }
            set { _iva_incluido = value; }
        }

        /// <summary>
        /// Indica si la transaccion es de tipo factura
        /// </summary>
        public short Factura
        {
            get { return _tipo_factura; }
            set { _tipo_factura = value; }
        }

        /// <summary>
        /// tipo transaccion 
        /// </summary>
        public short TipoTransaccion
        {
            get { return _tipo_transaccion; }
            set { _tipo_transaccion = value; }
        }

        /// <summary>
        ///(DESDE) Rango de resolucion de la Dian
        /// </summary>
        public int DocumentoInicial
        {
            get { return _documento_inicial; }
            set { _documento_inicial = value; }
        }

        /// <summary>
        ///(HASTA) Rango de resolucion de la Dian
        /// </summary>
        public int DocumentoFinal
        {
            get { return _documento_final; }
            set { _documento_final = value; }
        }

        ///// <summary>
        ///// estado
        ///// </summary>
        //public short Estado
        //{
        //    get { return _estado; }
        //    set { _estado = value; }
        //}

        ///// <summary>
        ///// descripción
        ///// </summary>
        //public string Descripcion
        //{
        //    get { return _descripcion; }
        //    set { _descripcion = value; }
        //}



        ///// <summary>
        ///// descuento
        ///// </summary>
        //public short Descuento
        //{
        //    get { return _descuento; }
        //    set { _descuento = value; }
        //}

        ///// <summary>
        ///// imprimer catidad en cero
        ///// </summary>
        //public short ImprimeCantidad0
        //{
        //    get { return _Imprime_Cantidad_0; }
        //    set { _Imprime_Cantidad_0 = value; }
        //}

        ///// <summary>
        ///// captura descuento
        ///// </summary>
        //public short CapturaDescuento
        //{
        //    get { return _captura_descuento; }
        //    set { _captura_descuento = value; }
        //}

        ///// <summary>
        ///// captura por descuento
        ///// </summary>
        //public short CapturaPorDescuento
        //{
        //    get { return _captura_por_descuento; }
        //    set { _captura_por_descuento = value; }
        //}

        ///// <summary>
        ///// validar por descuento
        ///// </summary>
        //public short ValidaPorDescuento
        //{
        //    get { return _valida_por_descuento; }
        //    set { _valida_por_descuento = value; }
        //}

        ///// <summary>
        ///// observaciones
        ///// </summary>
        //public string Observaciones
        //{
        //    get { return _observaciones; }
        //    set { _observaciones = value; }
        //}

        ///// <summary>
        ///// observación del tercero
        ///// </summary>
        //public int ObservacionTercero
        //{
        //    get { return _observacion_tercero; }
        //    set { _observacion_tercero = value; }
        //}

        ///// <summary>
        ///// maneja consecutivo
        ///// </summary>
        //public short ManejaConsecutivo
        //{
        //    get { return _maneja_consecutivo; }
        //    set { _maneja_consecutivo = value; }
        //}

        ///// <summary>
        ///// visibilidad
        ///// </summary>
        //public short Visible
        //{
        //    get { return _visible; }
        //    set { _visible = value; }
        //}

        ///// <summary>
        ///// valor del flete
        ///// </summary>
        //public short Fletes
        //{
        //    get { return _fletes; }
        //    set { _fletes = value; }
        //}

        ///// <summary>
        ///// maneja el valor del flete con iva
        ///// </summary>
        //public short IvaFlete
        //{
        //    get { return _flete_No_Bases; }
        //    set { _flete_No_Bases = value; }
        //}

        ///// <summary>
        ///// bodega por defeceto
        ///// </summary>
        //public string BodegaDefecto
        //{
        //    get { return _bodega_defecto; }
        //    set { _bodega_defecto = value; }
        //}

        ///// <summary>
        ///// tercero por defecto
        ///// </summary>
        //public string TerceroDefecto
        //{
        //    get { return _tercero_defecto; }
        //    set { _tercero_defecto = value; }
        //}

        ///// <summary>
        ///// controla fecha
        ///// </summary>
        //public short ControlaFecha
        //{
        //    get { return _controla_fecha; }
        //    set { _controla_fecha = value; }
        //}

        ///// <summary>
        ///// consulidación de ingresos y salidas
        ///// </summary>
        //public int Cuadre
        //{
        //    get { return _cuadre; }
        //    set { _cuadre = value; }
        //}

        ///// <summary>
        ///// bloqueo de fechas
        ///// </summary>
        //public short BloqueoFecha
        //{
        //    get { return _bloqueo_fecha; }
        //    set { _bloqueo_fecha = value; }
        //}



        ///// <summary>
        ///// control de rangos
        ///// </summary>
        //public short ControlRangos
        //{
        //    get { return _control_rangos; }
        //    set { _control_rangos = value; }
        //}

        ///// <summary>
        ///// control de existencias
        ///// </summary>
        //public int ControlExistencias
        //{
        //    get { return _control_existencias; }
        //    set { _control_existencias = value; }
        //}

        ///// <summary>
        ///// plazo en el documento
        ///// </summary>
        //public int PlazoDocumento
        //{
        //    get { return _plazo_documento; }
        //    set { _plazo_documento = value; }
        //}

        ///// <summary>
        ///// Objeto de tipo TransaccionOpcionExtra
        ///// </summary>
        //public TransaccionOpcionExtra PermisoTransaccion
        //{
        //    get { return _permiso_transaccion; }
        //    set { _permiso_transaccion = value; }
        //}

        ///// <summary>
        ///// Cuenta por Cobrar
        ///// </summary>
        //public short CuentaPorCobrar
        //{
        //    get { return _cuenta_por_cobrar; }
        //    set { _cuenta_por_cobrar = value; }
        //}

        ///// <summary>
        ///// Cartera Predeterminada
        ///// </summary>
        //public short CarteraPredeterminada
        //{
        //    get { return _cartera_predeterminada; }
        //    set { _cartera_predeterminada = value; }
        //}

        ///// <summary>
        ///// Precio Unitario
        ///// </summary>
        //public short PrecioUnitario
        //{
        //    get { return precio_unitario; }
        //    set { precio_unitario = value; }
        //}

        ///// <summary>
        ///// Obtiene la forma para el cálculo del IVA en la transacción.
        ///// 0=No realiza cálculo.
        ///// 1=Cálcula el IVA de los productos según la tarifa asiganada a cada uno.
        ///// 2=Cálcula el 16% sobre el subtotal de la transacción.
        ///// 3=Cálcula el 16% sobre el campo valor 1.
        ///// </summary>
        //public int CalculoIva
        //{
        //    get { return _calculo_iva; }
        //    set { _calculo_iva = value; }
        //}

        ///// <summary>
        ///// transacción del recibo de pago
        ///// </summary>
        //public string TipoReciboPago
        //{
        //    get { return _tipo_recibo_pago; }
        //    set { _tipo_recibo_pago = value; }
        //}

        ///// <summary>
        ///// origen de concepto de la transaccion
        ///// </summary>
        //public short OrigenConceptoPago
        //{
        //    get { return origen_concepto_pago; }
        //    set { origen_concepto_pago = value; }
        //}

        ///// <summary>
        ///// tercero de referencia de la compañía
        ///// </summary>
        //public short TerceroRefCia
        //{
        //    get { return _tercero_ref_cia; }
        //    set { _tercero_ref_cia = value; }
        //}

        ///// <summary>
        ///// nombre del tercero de referencia
        ///// </summary>
        //public string NombreTerceroRef
        //{
        //    get { return _nombre_tercero_ref; }
        //    set { _nombre_tercero_ref = value; }
        //}

        ///// <summary>
        ///// Texto factor
        ///// </summary>
        //public string FactorDescripcion
        //{
        //    get { return _factor_descripcion; }
        //    set { _factor_descripcion = value; }
        //}

        ///// <summary>
        ///// Maneja Factor
        ///// </summary>
        //public short FactorHabilita
        //{
        //    get { return _factor_habilita; }
        //    set { _factor_habilita = value; }
        //}

        ///// <summary>
        ///// Valida cantidad factor
        ///// </summary>
        //public short FactorValidaCantidad
        //{
        //    get { return _factor_valida_cantidad; }
        //    set { _factor_valida_cantidad = value; }
        //}

        ///// <summary>
        ///// el total del documento es afectada por el factor
        ///// </summary>
        //public short FactorTotalCantidad
        //{
        //    get { return _factor_total_cantidad; }
        //    set { _factor_total_cantidad = value; }
        //}

        ///// <summary>
        ///// el factor ingresado no es multiplicado por la cantidad y el Valor unitario para hallar el total
        ///// </summary>
        //public short FactorNoCantidad
        //{
        //    get { return _factor_no_cantidad; }
        //    set { _factor_no_cantidad = value; }
        //}

        ///// <summary>
        ///// Ajuste
        ///// </summary>
        //public short Ajuste
        //{
        //    get { return _ajuste; }
        //    set { _ajuste = value; }
        //}

        ///// <summary>
        ///// Punto venta
        ///// </summary>
        //public short PuntoVenta
        //{
        //    get { return _punto_venta; }
        //    set { _punto_venta = value; }
        //}

        ///// <summary>
        ///// Valida documento referencia
        ///// </summary>
        //public short ValidaDocRef
        //{
        //    get { return _valida_doc_ref; }
        //    set { _valida_doc_ref = value; }
        //}

        ///// <summary>
        ///// Maneja Puntos
        ///// </summary>
        //public short ManejaPuntos
        //{
        //    get { return _maneja_puntos; }
        //    set { _maneja_puntos = value; }
        //}

        ///// <summary>
        ///// Bodega Detalle
        ///// </summary>
        //public short BodegaDetalle
        //{
        //    get { return _bodega_detalle; }
        //    set { _bodega_detalle = value; }
        //}

        ///// <summary>
        ///// vinculado Detalle
        ///// </summary>
        //public short VinculadoDetalle
        //{
        //    get { return _vinculado_detalle; }
        //    set { _vinculado_detalle = value; }
        //}

        #endregion
    }
}
