namespace HGInetUBLv2_1
{
	public class DocumentoImpuestos
	{
		#region Propiedades

		private string _codigo;
		private decimal _porcentaje;
		private decimal _valor_impuesto;
		private string _tipo_impuesto;
		private decimal _base_imponible;
		private string _nombre;

		/// <summary>
		/// Código 
		/// </summary>
		public string Codigo
		{
			get { return _codigo; }
			set
			{ _codigo = value; }
		}

		/// <summary>
		/// Porcentaje 
		/// </summary>
		public decimal Porcentaje
		{
			get { return _porcentaje; }
			set { _porcentaje = value; }
		}

		/// <summary>
		/// Valor total del impuesto retenido
		/// </summary>
		public decimal ValorImpuesto
		{
			get { return _valor_impuesto; }
			set { _valor_impuesto = value; }
		}

		/// <summary>
		/// TipoImpuesto
		/// </summary>
		public string TipoImpuesto
		{
			get { return _tipo_impuesto; }
			set { _tipo_impuesto = value; }
		}

		/// <summary>
		/// //Base Imponible = Importe bruto + cargos - descuentos
		/// </summary>
		public decimal BaseImponible
		{
			get { return _base_imponible; }
			set { _base_imponible = value; }
		}

		/// <summary>
		/// Nombre 
		/// </summary>
		public string Nombre
		{
			get { return _nombre; }
			set { _nombre = value; }
		}

		#endregion

	}
}