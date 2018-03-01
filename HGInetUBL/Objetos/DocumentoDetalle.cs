using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HGInetUBL.Objetos
{
    internal class xDocumentoDetalle
    {
        #region Propiedades

        private int _codigo;
        private int _codigo_documento;
        private short _codigo_empresa;
        private string _codigo_transaccion;       
        private string _codigo_producto;
        //private string _descripcion1;
        private decimal _valor_unitario;
        private decimal _valor_iva;
        private decimal _cantidad_documento;
        private xProducto _datos_producto;

        // private string _tercero;
        //private decimal _valor_costo;
        //private decimal _valor_total;
        //private decimal _porcentaje_descuento;
        //private decimal _valor_descuento;
       
        //private decimal _valor_impuesto1;
        //private decimal _rete_fte;

        //private string _unidad;
        //private decimal _factor;
        //private string _talla;
        //private string _color;
        //private string _serie2;
        
        //private string _lote;
        //private string _bodega;
        //private string _serie;
        //private string _serie1;   
        //private int _documento_referenciaD;
        //private decimal _costo_agregado;       
        //private decimal _valor_unitario_saldoI;
        //private decimal _valor_unitario_saldoF;
        //private decimal _saldoI;
        //private decimal _SaldoF;
        //private System.Nullable<System.DateTime> _fecha1;
        //private System.Nullable<System.DateTime> _fecha2;
        //private string _sucursal;
        //private string _centro_costo;
        //private string _subcentro_costo;          
        //private string _vinculado;
        //private string _vendedor;
        //private short _tipo;
        //private Error _error;
        // private decimal _cantidad; SOLO AFECTA INVENTARIO

        /// <summary>
        /// Código Empresa
        /// </summary>		
        public short CodigoEmpresa
        {
            get { return _codigo_empresa; }
            set { _codigo_empresa = value; }
        }

        /// <summary>
        /// Código Empresa
        /// </summary>
        public string CodigoProducto
        {
            get { return _codigo_producto; }
            set { _codigo_producto = value; }
        }

        /// <summary>
        /// Transacción
        /// </summary>		
        public string CodigoTransaccion
        {
            get { return _codigo_transaccion; }
            set { _codigo_transaccion = value; }
        }

        /// <summary>
        /// Documento
        /// </summary>		
        public int CodigoDocumento
        {
            get { return _codigo_documento; }
            set { _codigo_documento = value; }
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// Descripción1
        /// </summary>
        //public string Descripcion1
        //{
        //    get { return _descripcion1; }
        //    set { _descripcion1 = value; }
        //}

        /// <summary>
        /// Valor Unitario
        /// </summary>
        public decimal ValorUnitario
        {
            get { return _valor_unitario; }
            set { _valor_unitario = value; }
        }

        /// <summary>
        /// Cantidad Documento
        /// </summary>
        public decimal CantidadDocumento
        {
            get { return _cantidad_documento; }
            set { _cantidad_documento = value; }
        }


        /// <summary>
        /// Prodcutos
        /// </summary>
        public xProducto DatosProducto
        {
            get { return _datos_producto; }
            set { _datos_producto = value; }
        }

        /// <summary>
        /// Valor Iva
        /// </summary>
        public decimal ValorIva
        {
            get { return _valor_iva; }
            set { _valor_iva = value; }
        }

        ///// <summary>
        ///// Valor Costo
        ///// </summary>
        //public decimal ValorCosto
        //{
        //    get { return _valor_costo; }
        //    set { _valor_costo = value; }
        //}

        ///// <summary>
        ///// Valor Total
        ///// </summary>
        //public decimal ValorTotal
        //{
        //    get { return _valor_total; }
        //    set { _valor_total = value; }
        //}

        ///// <summary>
        ///// Porcentaje Descuento
        ///// </summary>
        //public decimal PorcentajeDescuento
        //{
        //    get { return _porcentaje_descuento; }
        //    set { _porcentaje_descuento = value; }
        //}

        ///// <summary>
        ///// Valor Descuento
        ///// </summary>
        //public decimal ValorDescuento
        //{
        //    get { return _valor_descuento; }
        //    set { _valor_descuento = value; }
        //}

     

        ///// <summary>
        ///// Valor Impuesto1
        ///// </summary>
        //public decimal ValorImpuesto1
        //{
        //    get { return _valor_impuesto1; }
        //    set { _valor_impuesto1 = value; }
        //}

        ///// <summary>
        ///// Retención en la fuente
        ///// </summary>
        //public decimal ReteFte
        //{
        //    get { return _rete_fte; }
        //    set { _rete_fte = value; }
        //}


        ///// <summary>
        ///// Talla
        ///// </summary>
        //public string Talla
        //{
        //    get { return _talla; }
        //    set { _talla = value; }
        //}

        ///// <summary>
        ///// Color
        ///// </summary>
        //public string Color
        //{
        //    get { return _color; }
        //    set { _color = value; }
        //}

        ///// <summary>
        ///// Serie2
        ///// </summary>
        //public string Serie2
        //{
        //    get { return _serie2; }
        //    set { _serie2 = value; }
        //}

        ///// <summary>
        ///// Producto
        ///// </summary>		
        //public string Producto
        //{
        //    get { return _producto; }
        //    set { _producto = value; }
        //}

        ///// <summary>
        ///// Lote
        ///// </summary>
        //public string Lote
        //{
        //    get { return _lote; }
        //    set { _lote = value; }
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
        ///// Serie
        ///// </summary>
        //public string Serie
        //{
        //    get { return _serie; }
        //    set { _serie = value; }
        //}

        ///// <summary>
        ///// Serie1
        ///// </summary>
        //public string Serie1
        //{
        //    get { return _serie1; }
        //    set { _serie1 = value; }
        //}

        ///// <summary>
        ///// Cantidad
        ///// </summary>
        //public decimal Cantidad
        //{
        //    get { return _cantidad; }
        //    set { _cantidad = value; }
        //}

        ///// <summary>
        ///// Unidad
        ///// </summary>
        //public string Unidad
        //{
        //    get { return _unidad; }
        //    set { _unidad = value; }
        //}

        ///// <summary>
        ///// Factor
        ///// </summary>
        //public decimal Factor
        //{
        //    get { return _factor; }
        //    set { _factor = value; }
        //}

        ///// <summary>
        ///// Documento RerenciaD
        ///// </summary>
        //public int DocumentoRerenciaD
        //{
        //    get { return _documento_referenciaD; }
        //    set { _documento_referenciaD = value; }
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
        ///// Saldo Inicial
        ///// </summary>
        //public decimal SaldoInicial
        //{
        //    get { return _saldoI; }
        //    set { _saldoI = value; }
        //}

        ///// <summary>
        ///// Valor Unitario Saldo Inicial
        ///// </summary>
        //public decimal ValorUnitarioSaldoI
        //{
        //    get { return _valor_unitario_saldoI; }
        //    set { _valor_unitario_saldoI = value; }
        //}

        ///// <summary>
        ///// Saldo Final
        ///// </summary>
        //public decimal SaldoFinal
        //{
        //    get { return _SaldoF; }
        //    set { _SaldoF = value; }
        //}

        ///// <summary>
        ///// Valor Unitario Saldo Final
        ///// </summary>
        //public decimal ValorUnitarioSaldoF
        //{
        //    get { return _valor_unitario_saldoF; }
        //    set { _valor_unitario_saldoF = value; }
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
        ///// Sucursal
        ///// </summary>
        //public string Sucursal
        //{
        //    get { return _sucursal; }
        //    set { _sucursal = value; }
        //}

        ///// <summary>
        ///// Centro Costo
        ///// </summary>
        //public string CentroCosto
        //{
        //    get { return _centro_costo; }
        //    set { _centro_costo = value; }
        //}

        ///// <summary>
        ///// Subcentro Costo
        ///// </summary>
        //public string SubcentroCosto
        //{
        //    get { return _subcentro_costo; }
        //    set { _subcentro_costo = value; }
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
        ///// Vendedor
        ///// </summary>
        //public string Vendedor
        //{
        //    get { return _vendedor; }
        //    set { _vendedor = value; }
        //}

        ///// <summary>
        ///// Tipo
        ///// </summary>
        //public short Tipo
        //{
        //    get { return _tipo; }
        //    set { _tipo = value; }
        //}

        ///// <summary>
        ///// Error
        ///// </summary>
        //public Error Error
        //{
        //    get { return _error; }
        //    set { _error = value; }
        //}

        #endregion
    }
}
