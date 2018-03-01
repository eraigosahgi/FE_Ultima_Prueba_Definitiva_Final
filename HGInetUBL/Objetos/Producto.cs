using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBL.Objetos
{
    internal class xProducto
    {
        #region Propiedades

        private string _codigo;
        private string _descripcion;
        private string _codigo_iva;
        private xImpuestoIva _datos_impuesto_iva;


        //private string _codigo_linea;
        //private string _codigo_grupo;
        //private string _codigo_tipo;
        //private string _codigo_clase;
        //private string _codigo_unidad;
        //private string _codigo_unidad_compra;
        //private string _codigo_tarifa_iva;
        //private string _codigo_tarifa_impto1;
        //private string _codigo_tarifa_rtefte;
        //private string _codigo_moneda;
        //private string _codigo_ean;
        //private string _codigo_alterno;
        //private decimal _porcentaje;
        //private string _orden;
        //private decimal _descuento;
        //private decimal _precio_1;
        //private decimal _precio_2;
        //private decimal _precio_3;
        //private decimal _precio_4;
        //private decimal _precio_5;
        //private decimal _precio_6;
        //private decimal _precio_7;
        //private decimal _precio_8;
        //private decimal _impuesto_consumo;
        //private short _validar_kardex;
        //private short _marcado;
        //private System.Nullable<short> _puntos;
        //private short _aplica_ica;
        //private short _aiu;
        //private short _maneja_lote;
        //private short _actualiza_precio;
        //private decimal _descuento_promocion;
        //private System.Nullable<System.DateTime> _fecha_inicio_promocion;
        //private System.Nullable<System.DateTime> _fecha_fin_promocion;
        //private short _compuesto;
        //private byte[] _foto;
        //private string _texto_parametro1;
        //private string _texto_parametro2;
        //private string _texto_parametro3;
        //private string _codigo_parametro1;
        //private List<MaximoMinimo> _maximo_minimo;
        //private short _vigente;
        //private short _ecommerce;
        //private short _movil;
        //private List<ProductoImagen> _fotos;
        //private string _foto_ecommerce;
        //private decimal _valor_iva;
        //private DateTime _fecha_actualizacion;
        //private DateTime _fecha_consulta;
        //private bool actualiza;
        //private List<string> _fotos_url;
        //private string _foto_ecommerce_B64;
        //private List<Lote> _lotes;


        /// <summary>
        /// Código del producto
        /// </summary>
        public string Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// Descripción
        /// </summary>
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        /// <summary>
        /// Datos del impuesto iva
        /// </summary>
        public xImpuestoIva DatosImpuestoIva
        {
            get { return _datos_impuesto_iva; }
            set { _datos_impuesto_iva = value; }
        }

        public string CodigoIva
        {
            get { return _codigo_iva; }
            set { _codigo_iva = value; }
        }
        ///// <summary>
        ///// Código EAN
        ///// </summary>
        //public string CodigoEAN
        //{
        //    get { return _codigo_ean; }
        //    set
        //    {
        //        RaisePropertyChanging("Strauxiliar");
        //        _codigo_ean = value;
        //        RaisePropertyChanged("Strauxiliar");
        //    }
        //}

        ///// <summary>
        ///// Código Alterno
        ///// </summary>
        //public string CodigoAlterno
        //{
        //    get { return _codigo_alterno; }
        //    set
        //    {
        //        RaisePropertyChanging("StrCodAlterno");
        //        _codigo_alterno = value;
        //        RaisePropertyChanged("StrCodAlterno");
        //    }
        //}

        ///// <summary>
        ///// Código Línea
        ///// </summary>
        //public string CodigoLinea
        //{
        //    get { return _codigo_linea; }
        //    set
        //    {
        //        RaisePropertyChanging("StrLinea");
        //        _codigo_linea = value;
        //        RaisePropertyChanged("StrLinea");
        //    }
        //}

        ///// <summary>
        ///// Código Grupo
        ///// </summary>
        //public string CodigoGrupo
        //{
        //    get { return _codigo_grupo; }
        //    set
        //    {
        //        RaisePropertyChanging("StrLinea");
        //        _codigo_grupo = value;
        //        RaisePropertyChanged("StrLinea");
        //    }
        //}

        ///// <summary>
        ///// Código Clase
        ///// </summary>
        //public string CodigoClase
        //{
        //    get { return _codigo_clase; }
        //    set
        //    {
        //        RaisePropertyChanging("StrClase");
        //        _codigo_clase = value;
        //        RaisePropertyChanged("StrClase");
        //    }
        //}

        ///// <summary>
        ///// Código Tipo
        ///// </summary>
        //public string CodigoTipo
        //{
        //    get { return _codigo_tipo; }
        //    set
        //    {
        //        RaisePropertyChanging("StrTipo");
        //        _codigo_tipo = value;
        //        RaisePropertyChanged("StrTipo");
        //    }
        //}

        ///// <summary>
        ///// Código Unidad
        ///// </summary>
        //public string CodigoUnidad
        //{
        //    get { return _codigo_unidad; }
        //    set
        //    {
        //        RaisePropertyChanging("StrUnidad");
        //        _codigo_unidad = value;
        //        RaisePropertyChanged("StrUnidad");
        //    }
        //}

        ///// <summary>
        ///// Código Unidad de Compra
        ///// </summary>
        //public string CodigoUnidadCompra
        //{
        //    get { return _codigo_unidad_compra; }
        //    set
        //    {
        //        RaisePropertyChanging("StrUndCompra");
        //        _codigo_unidad_compra = value;
        //        RaisePropertyChanged("StrUndCompra");
        //    }
        //}

        ///// <summary>
        ///// Precio 1
        ///// </summary>
        //public decimal Precio1
        //{
        //    get { return _precio_1; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPrecio1");
        //        _precio_1 = value;
        //        RaisePropertyChanged("IntPrecio1");
        //    }
        //}

        ///// <summary>
        ///// Precio 2
        ///// </summary>
        //public decimal Precio2
        //{
        //    get { return _precio_2; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPrecio2");
        //        _precio_2 = value;
        //        RaisePropertyChanged("IntPrecio2");
        //    }
        //}

        ///// <summary>
        ///// Precio 3
        ///// </summary>
        //public decimal Precio3
        //{
        //    get { return _precio_3; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPrecio3");
        //        _precio_3 = value;
        //        RaisePropertyChanged("IntPrecio3");
        //    }
        //}

        ///// <summary>
        ///// Precio 4
        ///// </summary>
        //public decimal Precio4
        //{
        //    get { return _precio_4; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPrecio4");
        //        _precio_4 = value;
        //        RaisePropertyChanged("IntPrecio4");
        //    }
        //}

        ///// <summary>
        ///// Precio 5
        ///// </summary>
        //public decimal Precio5
        //{
        //    get { return _precio_5; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPrecio5");
        //        _precio_5 = value;
        //        RaisePropertyChanged("IntPrecio5");
        //    }
        //}

        ///// <summary>
        ///// Precio 6
        ///// </summary>
        //public decimal Precio6
        //{
        //    get { return _precio_6; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPrecio6");
        //        _precio_6 = value;
        //        RaisePropertyChanged("IntPrecio6");
        //    }
        //}

        ///// <summary>
        ///// Precio 7
        ///// </summary>
        //public decimal Precio7
        //{
        //    get { return _precio_7; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPrecio7");
        //        _precio_7 = value;
        //        RaisePropertyChanged("IntPrecio7");
        //    }
        //}

        ///// <summary>
        ///// Precio 8
        ///// </summary>
        //public decimal Precio8
        //{
        //    get { return _precio_8; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPrecio8");
        //        _precio_8 = value;
        //        RaisePropertyChanged("IntPrecio8");
        //    }
        //}

        ///// <summary>
        ///// Impuesto de Consumo
        ///// </summary>
        //public decimal ImpuestoConsumo
        //{
        //    get { return _impuesto_consumo; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPrecio8");
        //        _impuesto_consumo = value;
        //        RaisePropertyChanged("IntPrecio8");
        //    }
        //}

        ///// <summary>
        ///// Validar Inventario
        ///// </summary>
        //public short ValidarKardex
        //{
        //    get { return _validar_kardex; }
        //    set { _validar_kardex = value; }
        //}

        ///// <summary>
        ///// Maneja Lote
        ///// </summary>
        //public short ManejaLote
        //{
        //    get { return _maneja_lote; }
        //    set { _maneja_lote = value; }
        //}

        ///// <summary>
        ///// Código Tarifa de IVA
        ///// </summary>
        //public string CodigoTarifaIVA
        //{
        //    get { return _codigo_tarifa_iva; }
        //    set
        //    {
        //        RaisePropertyChanging("IntIva");
        //        _codigo_tarifa_iva = value;
        //        RaisePropertyChanged("IntIva");
        //    }
        //}

        ///// <summary>
        ///// Código Tarifa de Retención en la Fuente
        ///// </summary>
        //public string CodigoTarifaRteFte
        //{
        //    get { return _codigo_tarifa_rtefte; }
        //    set
        //    {
        //        RaisePropertyChanging("IntRetencion");
        //        _codigo_tarifa_rtefte = value;
        //        RaisePropertyChanged("IntRetencion");
        //    }
        //}

        ///// <summary>
        ///// Código Tarifa de Impuesto 1
        ///// </summary>
        //public string CodigoTarifaImpuesto1
        //{
        //    get { return _codigo_tarifa_impto1; }
        //    set
        //    {
        //        RaisePropertyChanging("IntImpuesto1");
        //        _codigo_tarifa_impto1 = value;
        //        RaisePropertyChanged("IntImpuesto1");
        //    }
        //}

        ///// <summary>
        ///// Aplica ICA
        ///// </summary>
        //public short AplicaICA
        //{
        //    get { return _aplica_ica; }
        //    set { _aplica_ica = value; }
        //}

        ///// <summary>
        ///// Vigente
        ///// </summary>
        //public short Vigente
        //{
        //    get { return _vigente; }
        //    set { _vigente = value; }
        //}

        ///// <summary>
        ///// Marcado
        ///// </summary>
        //public short Marcado
        //{
        //    get { return _marcado; }
        //    set { _marcado = value; }
        //}

        ///// <summary>
        ///// AIU - Administración, Imprevistos y utilidad
        ///// </summary>
        //public short AIU
        //{
        //    get { return _aiu; }
        //    set { _aiu = value; }
        //}

        ///// <summary>
        ///// Código Moneda
        ///// </summary>
        //public string CodigoMoneda
        //{
        //    get { return _codigo_moneda; }
        //    set
        //    {
        //        RaisePropertyChanging("StrMoneda");
        //        _codigo_moneda = value;
        //        RaisePropertyChanged("StrMoneda");
        //    }
        //}

        ///// <summary>
        ///// Porcentaje
        ///// </summary>
        //public decimal Porcentaje
        //{
        //    get { return _porcentaje; }
        //    set
        //    {
        //        RaisePropertyChanging("IntPorcentaje");
        //        _porcentaje = value;
        //        RaisePropertyChanged("IntPorcentaje");
        //    }
        //}

        ///// <summary>
        ///// Actualiza precio
        ///// </summary>
        //public short ActualizaPrecio
        //{
        //    get { return _actualiza_precio; }
        //    set { _actualiza_precio = value; }
        //}

        ///// <summary>
        ///// Orden
        ///// </summary>
        //public string Orden
        //{
        //    get { return _orden; }
        //    set
        //    {
        //        RaisePropertyChanging("StrOrden");
        //        _orden = value;
        //        RaisePropertyChanged("StrOrden");
        //    }
        //}

        ///// <summary>
        ///// Compuesto
        ///// </summary>
        //public short Compuesto
        //{
        //    get { return _compuesto; }
        //    set { _compuesto = value; }
        //}

        ///// <summary>
        ///// Texto parametro 1
        ///// </summary>
        //public string ParametroTexto1
        //{
        //    get { return _texto_parametro1; }
        //    set
        //    {
        //        RaisePropertyChanging("StrParam1");
        //        _texto_parametro1 = value;
        //        RaisePropertyChanged("StrParam1");
        //    }
        //}

        ///// <summary>
        ///// Texto parametro 2
        ///// </summary>
        //public string ParametroTexto2
        //{
        //    get { return _texto_parametro2; }
        //    set
        //    {
        //        RaisePropertyChanging("StrParam2");
        //        _texto_parametro2 = value;
        //        RaisePropertyChanged("StrParam2");
        //    }
        //}

        ///// <summary>
        ///// Texto parametro 3
        ///// </summary>
        //public string ParametroTexto3
        //{
        //    get { return _texto_parametro3; }
        //    set
        //    {
        //        RaisePropertyChanging("StrParam3");
        //        _texto_parametro3 = value;
        //        RaisePropertyChanged("StrParam3");
        //    }
        //}

        ///// <summary>
        ///// Código parametro 1
        ///// </summary>
        //public string CodigoParametro1
        //{
        //    get { return _codigo_parametro1; }
        //    set
        //    {
        //        RaisePropertyChanging("StrPParametro1");
        //        _codigo_parametro1 = value;
        //        RaisePropertyChanged("StrPParametro1");
        //    }
        //}

        ///// <summary>
        ///// Puntos
        ///// </summary>
        //public System.Nullable<short> Puntos
        //{
        //    get { return _puntos; }
        //    set { _puntos = value; }
        //}

        ///// <summary>
        ///// Total Coef
        ///// </summary>
        //public decimal Descuento
        //{
        //    get { return _descuento; }
        //    set
        //    {
        //        RaisePropertyChanging("IntDescuento");
        //        _descuento = value;
        //        RaisePropertyChanged("IntDescuento");
        //    }
        //}

        ///// <summary>
        ///// Descuento Promoción
        ///// </summary>
        //public decimal DescuentoPromocion
        //{
        //    get { return _descuento_promocion; }
        //    set
        //    {
        //        RaisePropertyChanging("IntDescPromocion");
        //        _descuento_promocion = value;
        //        RaisePropertyChanged("IntDescPromocion");
        //    }
        //}

        ///// <summary>
        ///// Fecha Inicio Promoción
        ///// </summary>
        //public System.Nullable<System.DateTime> FechaInicioPromocion
        //{
        //    get { return _fecha_inicio_promocion; }
        //    set { _fecha_inicio_promocion = value; }
        //}

        ///// <summary>
        ///// Fecha Fin Promoción
        ///// </summary>
        //public System.Nullable<System.DateTime> FechaFinPromocion
        //{
        //    get { return _fecha_fin_promocion; }
        //    set { _fecha_fin_promocion = value; }
        //}

        ///// <summary>
        ///// Foto 
        ///// </summary>
        //public byte[] Foto
        //{
        //    get { return _foto; }
        //    set
        //    {
        //        RaisePropertyChanging("Foto");
        //        _foto = value;
        //        RaisePropertyChanged("Foto");
        //    }
        //}

        ///// <summary>
        ///// Lista de fotos
        ///// </summary>
        //public List<ProductoImagen> Fotos
        //{
        //    get { return _fotos; }
        //    set { _fotos = value; }
        //}

        ///// <summary>
        ///// Url de la foto
        ///// </summary>
        //public string FotoEcommerce
        //{
        //    get { return _foto_ecommerce; }
        //    set { _foto_ecommerce = value; }
        //}

        ///// <summary>
        ///// Valor del iva
        ///// </summary>
        //public decimal ValorIva
        //{
        //    get { return _valor_iva; }
        //    set { _valor_iva = value; }
        //}

        ///// <summary>
        ///// Lista de maximo y minimo
        ///// </summary>
        //public List<MaximoMinimo> MaximoMinimo
        //{
        //    get { return _maximo_minimo; }
        //    set { _maximo_minimo = value; }
        //}

        ///// <summary>
        ///// producto maracado como Ecommerce
        ///// </summary>
        //public short Ecommerce
        //{
        //    get { return _ecommerce; }
        //    set { _ecommerce = value; }
        //}

        ///// <summary>
        ///// producto marcado como móvil.
        ///// </summary>
        //public short Movil
        //{
        //    get { return _movil; }
        //    set { _movil = value; }
        //}

        ///// <summary>
        ///// fecha última actualización
        ///// </summary>
        //public DateTime FechaActualizacion
        //{
        //    get { return _fecha_actualizacion; }
        //    set { _fecha_actualizacion = value; }
        //}

        ///// <summary>
        ///// Fecha ultima consulta
        ///// </summary>
        //public DateTime FechaConsulta
        //{
        //    get { return _fecha_consulta; }
        //    set { _fecha_consulta = value; }
        //}

        ///// <summary>
        ///// Actualiza información.
        ///// </summary>
        //public bool Actualiza
        //{
        //    get { return actualiza; }
        //    set { actualiza = value; }
        //}

        ///// <summary>
        ///// Lista de url de fotos
        ///// </summary>
        //public List<string> FotosUrl
        //{
        //    get { return _fotos_url; }
        //    set { _fotos_url = value; }
        //}

        ///// <summary>
        ///// Foto en ToBase64String
        ///// </summary>
        //public string FotoEcommerceB64
        //{
        //    get { return _foto_ecommerce_B64; }
        //    set { _foto_ecommerce_B64 = value; }
        //}

        ///// <summary>
        ///// Lotes del producto
        ///// </summary>
        //public List<Lote> Lotes
        //{
        //    get { return _lotes; }
        //    set { _lotes = value; }
        //}

        #endregion
    }
}
