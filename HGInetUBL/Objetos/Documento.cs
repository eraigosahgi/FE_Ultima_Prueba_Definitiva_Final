using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HGInetUBL.Objetos
{
    internal class xDocumento
    {
        #region Propiedades

        private int _codigo;
        private short _codigo_empresa;
        private string _codigo_transaccion;
        private string _codigo_tercero;
       
        private DateTime _fecha;
        private DateTime _fecha_graba;
        private decimal _iva;
        private decimal _valor_impuesto1;
        private decimal _rete_fte;
        private decimal _rete_cree;
        private decimal _rete_iva;
        private decimal _rete_ica;
        private decimal _porcentaje_descuento;
        private decimal _valor;
        private decimal _subtotal;
        private decimal _neto;
        private decimal _d_descuento;
        private decimal _total;
        private string _moneda;

        private List<xDocumentoDetalle> _documento_detalle;
        private xTransaccion _datos_transaccion;
        private xTercero _datos_tercero;
        private xEmpresa _datos_empresa;

        // private string _transportador;

        //private System.Nullable<int> _documento_referencia;
        // private decimal _porcentaje_descuento_fin; informativo
        //private short _ano;
        //private short _periodo;
        //private DateTime _vencimiento;
        //private string _vinculado;
        //private string _tercero_auxiliar;
        //private string _d_vendedor;
        //private string _bodega_des;
        //private string _bodega;
        //private string _clase;
        //private string _sucursal;
        //private string _centro_costo;
        //private string _subcentro_costo;
        //private string _producto_p;
        //private decimal _cantidad_p;
        //private decimal _base_p;
        //private string _tipo_evento;
        //private string _tran_auxiliar;
        //private string _local;
        //private string _referencia;
        //private string _referencia1;
        //private string _referencia2;
        //private string _referencia3;
        //private string _plazo;
        //private System.Nullable<short> _tipo_doc;
        //private decimal _fletes;
        //private decimal _intereses;
        //private decimal _otros_cobros;
        //private decimal _costo_agregado;
        //private decimal _pago;
        //private decimal _cambio;
        //private string _observaciones;
        //private int _paso_cupo;
        //private short _estado;
        //private short _gestion;
        //private short _totales_manual;
        //private int _impresiones;
        //private int _pedido;
        //private decimal _valor_descuento_fin;
        //private System.Nullable<System.DateTime> _fecha_descuento_fin;
        //private short _registros;
        //private short _cartera;
        //private int _cuotas;
        //private int _intervalo_cuotas;
        //private decimal _puntos;
        //private string _caja;
        //private int _turno;
        //  private decimal _t_cantidad;
        //private decimal _valor1;
        //private decimal _valor2;
        //private decimal _valor3;
        //private decimal _valor4;
        //private System.Nullable<System.DateTime> _fecha1;
        //private System.Nullable<System.DateTime> _fecha2;
        //private System.Nullable<System.DateTime> _fecha3;
        //private System.Nullable<System.DateTime> _fecha4;
        //private decimal _valor_parametro1;
        //private decimal _valor_parametro2;
        //private decimal _valor_parametro3;
        //private decimal _valor_parametro4;
        //private string _usuario_graba;
        //private decimal por_intereses;
        //   private Error _error;

        //private string _nombre_vendedor;

        /// <summary>
        /// Empresa
        /// </summary>
        public short CodigoEmpresa
        {
            get { return _codigo_empresa; }
            set { _codigo_empresa = value; }
        }

        /// <summary>
        /// Transaccion
        /// </summary>
        public string CodigoTransaccion
        {
            get { return _codigo_transaccion; }
            set { _codigo_transaccion = value; }
        }

        /// <summary>
        /// Tercero
        /// </summary>
        public string CodigoTercero
        {
            get { return _codigo_tercero; }
            set { _codigo_tercero = value; }
        }

        /// <summary>
        /// Número de Documento
        /// </summary>
        public int Numero
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// Fecha
        /// </summary>
        public System.DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        /// <summary>
        /// Fecha en que se graba documento
        /// </summary>
        public DateTime FechaGraba
        {
            get { return _fecha_graba; }
            set { _fecha_graba = value; }
        }

        /// <summary>
        /// Iva
        /// </summary>
        public decimal Iva
        {
            get { return _iva; }
            set { _iva = value; }
        }

        /// <summary>
        /// Valor Impuesto al consumo
        /// </summary>
        public decimal ImpuestoConsumo
        {
            get { return _valor_impuesto1; }
            set { _valor_impuesto1 = value; }
        }

        /// <summary>
        /// Retención en la Fuente
        /// </summary>
        public decimal ReteFte
        {
            get { return _rete_fte; }
            set { _rete_fte = value; }
        }

        /// <summary>
        /// Retención CREE
        /// </summary>
        public decimal ReteCREE
        {
            get { return _rete_cree; }
            set { _rete_cree = value; }
        }

        /// <summary>
        /// Retención IVA
        /// </summary>
        public decimal ReteIVA
        {
            get { return _rete_iva; }
            set { _rete_iva = value; }
        }

        /// <summary>
        /// Retención ICA
        /// </summary>
        public decimal ReteICA
        {
            get { return _rete_ica; }
            set { _rete_ica = value; }
        }

        /// <summary>
        /// Porcentaje Descuento
        /// </summary>
        public decimal PorcentajeDescuento
        {
            get { return _porcentaje_descuento; }
            set { _porcentaje_descuento = value; }
        }

        /// <summary>
        /// Valor (ImporteBruto)
        /// </summary>
        public decimal Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }

        /// <summary>
        /// Subtotal (BaseImponible)
        /// </summary>
        public decimal Subtotal
        {
            get { return _subtotal; }
            set { _subtotal = value; }
        }

        /// <summary>
        /// Neto (Total)
        /// </summary>
        public decimal Neto
        {
            get { return _neto; }
            set { _neto = value; }
        }

        /// <summary>
        /// Descuento Total del Documento
        /// </summary>
        public decimal DescuentoTotalDocumento
        {
            get { return _d_descuento; }
            set { _d_descuento = value; }
        }

        /// <summary>
        /// Total
        /// </summary>
        public decimal Total
        {
            get { return _total; }
            set { _total = value; }
        }

        /// <summary>
        /// Documento Detalle
        /// </summary>
        public List<xDocumentoDetalle> DocumentoDetalle
        {
            get { return _documento_detalle; }
            set { _documento_detalle = value; }
        }

        /// <summary>
        /// Moneda
        /// </summary>
        public string Moneda
        {
            get { return _moneda; }
            set { _moneda = value; }
        }

        /// <summary>
        /// Datos de la transaccion
        /// </summary>
        public xTransaccion DatosTransaccion
        {
            get { return _datos_transaccion; }
            set { _datos_transaccion = value; }
        }

        /// <summary>
        /// Datos del tercero
        /// </summary>
        public xTercero DatosTercero
        {
            get { return _datos_tercero; }
            set { _datos_tercero = value; }
        }

        /// <summary>
        /// Datos empresa
        /// </summary>
        public xEmpresa DatosEmpresa
        {
            get { return _datos_empresa; }
            set { _datos_empresa = value; }
        }

        ///// <summary>
        ///// Año
        ///// </summary>
        //public short Ano
        //{
        //    get { return _ano; }
        //    set { _ano = value; }
        //}

        ///// <summary>
        ///// Período
        ///// </summary>
        //public short Periodo
        //{
        //    get { return _periodo; }
        //    set { _periodo = value; }
        //}

        ///// <summary>
        ///// Vencimiento
        ///// </summary>
        //public System.DateTime Vencimiento
        //{
        //    get { return _vencimiento; }
        //    set { _vencimiento = value; }
        //}

        ///// <summary>
        ///// Vinculado
        ///// </summary>
        //public string Vinculado
        //{
        //    get { return _vinculado; }
        //    set { _vinculado = value; }
        //}

        ///// <summary>
        ///// Tercero Auxiliar
        ///// </summary>
        //public string TerceroAuxiliar
        //{
        //    get { return _tercero_auxiliar; }
        //    set { _tercero_auxiliar = value; }
        //}

        ///// <summary>
        ///// Código Vendedor
        ///// </summary>
        //public string Vendedor
        //{
        //    get { return _d_vendedor; }
        //    set { _d_vendedor = value; }
        //}

        ///// <summary>
        ///// Transportador
        ///// </summary>
        //public string Transportador
        //{
        //    get { return _transportador; }
        //    set { _transportador = value; }
        //}

        ///// <summary>
        ///// Bodega de Destino
        ///// </summary>
        //public string BodegaDestino
        //{
        //    get { return _bodega_des; }
        //    set { _bodega_des = value; }
        //}

        ///// <summary>
        ///// Bodega
        ///// </summary>
        //public string Bodega
        //{
        //    get { return _bodega; }
        //    set { _bodega = value; }
        //}

        ///// <summary>
        ///// Clase
        ///// </summary>
        //public string Clase
        //{
        //    get { return _clase; }
        //    set { _clase = value; }
        //}



        ///// <summary>
        ///// Sucursal
        ///// </summary>
        //public string Sucursal
        //{
        //    get { return _sucursal; }
        //    set { _sucursal = value; }
        //}

        ///// <summary>
        ///// Centro de Costo
        ///// </summary>
        //public string CentroCosto
        //{
        //    get { return _centro_costo; }
        //    set { _centro_costo = value; }
        //}

        ///// <summary>
        ///// Subcentro de Costo
        ///// </summary>
        //public string SubcentroCosto
        //{
        //    get { return _subcentro_costo; }
        //    set { _subcentro_costo = value; }
        //}

        ///// <summary>
        ///// Producto que se usa en el encabezado del documento
        ///// </summary>
        //public string ProductoP
        //{
        //    get { return _producto_p; }
        //    set { _producto_p = value; }
        //}

        ///// <summary>
        ///// valor de cantidad para el proceso de prducción
        ///// </summary>
        //public decimal CantidadP
        //{
        //    get { return _cantidad_p; }
        //    set { _cantidad_p = value; }
        //}

        ///// <summary>
        ///// valor de base para el proceso de prducción
        ///// </summary>
        //public decimal BaseP
        //{
        //    get { return _base_p; }
        //    set { _base_p = value; }
        //}

        ///// <summary>
        ///// Tipo Evento
        ///// </summary>
        //public string TipoEvento
        //{
        //    get { return _tipo_evento; }
        //    set { _tipo_evento = value; }
        //}

        ///// <summary>
        ///// Transacción Auxiliar
        ///// </summary>
        //public string TransaccionAuxiliar
        //{
        //    get { return _tran_auxiliar; }
        //    set { _tran_auxiliar = value; }
        //}

        ///// <summary>
        ///// Documento Referencia
        ///// </summary>
        //public System.Nullable<int> DocumentoReferencia
        //{
        //    get { return _documento_referencia; }
        //    set { _documento_referencia = value; }
        //}

        ///// <summary>
        ///// Local
        ///// </summary>
        //public string Local
        //{
        //    get { return _local; }
        //    set { _local = value; }
        //}

        ///// <summary>
        ///// Referencia
        ///// </summary>
        //public string Referencia
        //{
        //    get { return _referencia; }
        //    set { _referencia = value; }
        //}

        ///// <summary>
        ///// Referencia1
        ///// </summary>
        //public string Referencia1
        //{
        //    get { return _referencia1; }
        //    set { _referencia1 = value; }
        //}

        ///// <summary>
        ///// Referencia2
        ///// </summary>
        //public string Referencia2
        //{
        //    get { return _referencia2; }
        //    set { _referencia2 = value; }
        //}

        ///// <summary>
        ///// Referencia3
        ///// </summary>
        //public string Referencia3
        //{
        //    get { return _referencia3; }
        //    set { _referencia3 = value; }
        //}

        ///// <summary>
        ///// Plazo
        ///// </summary>
        //public string Plazo
        //{
        //    get { return _plazo; }
        //    set { _plazo = value; }
        //}

        ///// <summary>
        ///// Tipo Documento
        ///// </summary>
        //public System.Nullable<short> TipoDocumento
        //{
        //    get { return _tipo_doc; }
        //    set { _tipo_doc = value; }
        //}  

        ///// <summary>
        ///// Fletes
        ///// </summary>
        //public decimal Fletes
        //{
        //    get { return _fletes; }
        //    set { _fletes = value; }
        //}

        ///// <summary>
        ///// Intereses
        ///// </summary>
        //public decimal Intereses
        //{
        //    get { return _intereses; }
        //    set { _intereses = value; }
        //}

        ///// <summary>
        ///// Otros Cobros
        ///// </summary>
        //public decimal OtrosCobros
        //{
        //    get { return _otros_cobros; }
        //    set { _otros_cobros = value; }
        //}

        ///// <summary>
        ///// Costo Agregado
        ///// </summary>
        //public decimal CostoAgregado
        //{
        //    get { return _costo_agregado; }
        //    set { _costo_agregado = value; }
        //}

        ///// <summary>
        ///// Pago
        ///// </summary>
        //public decimal Pago
        //{
        //    get { return _pago; }
        //    set { _pago = value; }
        //}

        ///// <summary>
        ///// Cambio
        ///// </summary>
        //public decimal Cambio
        //{
        //    get { return _cambio; }
        //    set { _cambio = value; }
        //}

        ///// <summary>
        ///// Observaciones
        ///// </summary>
        //public string Observaciones
        //{
        //    get { return _observaciones; }
        //    set { _observaciones = value; }
        //}

        ///// <summary>
        ///// Paso Cupo
        ///// </summary>
        //public int PasoCupo
        //{
        //    get { return _paso_cupo; }
        //    set { _paso_cupo = value; }
        //}

        ///// <summary>
        ///// Estado
        ///// </summary>
        //public short Estado
        //{
        //    get { return _estado; }
        //    set { _estado = value; }
        //}

        ///// <summary>
        ///// Gestion
        ///// </summary>
        //public short Gestion
        //{
        //    get { return _gestion; }
        //    set { _gestion = value; }
        //}

        ///// <summary>
        ///// Totales Manual
        ///// </summary>
        //public short TotalesManual
        //{
        //    get { return _totales_manual; }
        //    set { _totales_manual = value; }
        //}

        ///// <summary>
        ///// Impresiones
        ///// </summary>
        //public int Impresiones
        //{
        //    get { return _impresiones; }
        //    set { _impresiones = value; }
        //}

        ///// <summary>
        ///// Pedido
        ///// </summary>
        //public int Pedido
        //{
        //    get { return _pedido; }
        //    set { _pedido = value; }
        //}

        ///// <summary>
        ///// Porcentaje Descuento Financiero
        ///// </summary>
        //public decimal PorcentajeDescuentoFinanciero
        //{
        //    get { return _porcentaje_descuento_fin; }
        //    set { _porcentaje_descuento_fin = value; }
        //}

        ///// <summary>
        ///// Valor Descuento Financiero
        ///// </summary>
        //public decimal ValorDescuentoFinanciero
        //{
        //    get { return _valor_descuento_fin; }
        //    set { _valor_descuento_fin = value; }
        //}

        ///// <summary>
        ///// Fecha Descuento Financiero
        ///// </summary>
        //public System.Nullable<System.DateTime> FechaDescuentoFinanciero
        //{
        //    get { return _fecha_descuento_fin; }
        //    set { _fecha_descuento_fin = value; }
        //}

        ///// <summary>
        ///// Registros
        ///// </summary>
        //public short Registros
        //{
        //    get { return _registros; }
        //    set { _registros = value; }
        //}

        ///// <summary>
        ///// Cartera
        ///// </summary>
        //public short Cartera
        //{
        //    get { return _cartera; }
        //    set { _cartera = value; }
        //}

        ///// <summary>
        ///// Cuotas
        ///// </summary>
        //public int Cuotas
        //{
        //    get { return _cuotas; }
        //    set { _cuotas = value; }
        //}

        ///// <summary>
        ///// Intervalo Cuotas
        ///// </summary>
        //public int IntervaloCuotas
        //{
        //    get { return _intervalo_cuotas; }
        //    set { _intervalo_cuotas = value; }
        //}

        ///// <summary>
        ///// Puntos
        ///// </summary>
        //public decimal Puntos
        //{
        //    get { return _puntos; }
        //    set { _puntos = value; }
        //}

        ///// <summary>
        ///// Caja
        ///// </summary>
        //public string Caja
        //{
        //    get { return _caja; }
        //    set { _caja = value; }
        //}

        ///// <summary>
        ///// Turno
        ///// </summary>
        //public int Turno
        //{
        //    get { return _turno; }
        //    set { _turno = value; }
        //}

        ///// <summary>
        ///// TCantidad
        ///// </summary>
        //public decimal TCantidad
        //{
        //    get { return _t_cantidad; }
        //    set { _t_cantidad = value; }
        //}

        ///// <summary>
        ///// Valor1
        ///// </summary>
        //public decimal Valor1
        //{
        //    get { return _valor1; }
        //    set { _valor1 = value; }
        //}

        ///// <summary>
        ///// Valor2
        ///// </summary>
        //public decimal Valor2
        //{
        //    get { return _valor2; }
        //    set { _valor2 = value; }
        //}

        ///// <summary>
        ///// Valor3
        ///// </summary>
        //public decimal Valor3
        //{
        //    get { return _valor3; }
        //    set { _valor3 = value; }
        //}

        ///// <summary>
        ///// Valor4
        ///// </summary>
        //public decimal Valor4
        //{
        //    get { return _valor4; }
        //    set { _valor4 = value; }
        //}

        ///// <summary>
        ///// Fecha1
        ///// </summary>
        //public System.Nullable<System.DateTime> Fecha1
        //{
        //    get { return _fecha1; }
        //    set { _fecha1 = value; }
        //}

        ///// <summary>
        ///// Fecha2
        ///// </summary>
        //public System.Nullable<System.DateTime> Fecha2
        //{
        //    get { return _fecha2; }
        //    set { _fecha2 = value; }
        //}

        ///// <summary>
        ///// Fecha3
        ///// </summary>
        //public System.Nullable<System.DateTime> Fecha3
        //{
        //    get { return _fecha3; }
        //    set { _fecha3 = value; }
        //}

        ///// <summary>
        ///// Fecha4
        ///// </summary>
        //public System.Nullable<System.DateTime> Fecha4
        //{
        //    get { return _fecha4; }
        //    set { _fecha4 = value; }
        //}

        ///// <summary>
        ///// Valor Parametro1
        ///// </summary>
        //public decimal ValorParametro1
        //{
        //    get { return _valor_parametro1; }
        //    set { _valor_parametro1 = value; }
        //}

        ///// <summary>
        ///// Valor Parametro2
        ///// </summary>
        //public decimal ValorParametro2
        //{
        //    get { return _valor_parametro2; }
        //    set { _valor_parametro2 = value; }
        //}

        ///// <summary>
        ///// Valor Parametro3
        ///// </summary>
        //public decimal ValorParametro3
        //{
        //    get { return _valor_parametro3; }
        //    set { _valor_parametro3 = value; }
        //}

        ///// <summary>
        ///// Valor Parametro4
        ///// </summary>
        //public decimal ValorParametro4
        //{
        //    get { return _valor_parametro4; }
        //    set { _valor_parametro4 = value; }
        //}

        ///// <summary>
        ///// Usuario que graba documento
        ///// </summary>
        //public string UsuarioGraba
        //{
        //    get { return _usuario_graba; }
        //    set { _usuario_graba = value; }
        //}

        ///// <summary>
        ///// Error
        ///// </summary>
        //public Error Error
        //{
        //    get { return _error; }
        //    set { _error = value; }
        //}



        ///// <summary>
        ///// Nombre del vendedor
        ///// </summary>
        //public string NombreVendedor
        //{
        //    get { return _nombre_vendedor; }
        //    set { _nombre_vendedor = value; }
        //}

        ///// <summary>
        ///// Por Intereses
        ///// </summary>
        //public decimal PorIntereses
        //{
        //    get { return por_intereses; }
        //    set { por_intereses = value; }
        //}
        #endregion
    }
}
