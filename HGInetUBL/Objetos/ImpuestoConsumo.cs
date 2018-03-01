using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetUBL.Objetos
{
    internal class ImpuestoConsumo
    {
        #region Propiedades

        private string _codigo;
        private string _descripcion;
        private decimal _valor;
        private string _tipo_impuesto;
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
        /// Descripción
        /// </summary>
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        /// <summary>
        /// Valor Iva
        /// </summary>
        public decimal Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }

        /// <summary>
        /// Valor Iva
        /// </summary>
        public string TipoImpuesto
        {
            get { return _tipo_impuesto; }
            set { _tipo_impuesto = value; }
        }

        /// <summary>
        /// Nombre (En este caso seria Iva)
        /// </summary>
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        #endregion
    }    
}
