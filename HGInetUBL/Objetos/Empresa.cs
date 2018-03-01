using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HGInetUBL.Objetos
{
    /// <summary>
    /// Información del obligado a facturar
    /// </summary>
    internal class xEmpresa
    {
        private short _codigo;
        private string _identificacion;
        private string _tipo_identificacion;
        private int _regimen;
        private int _tipo_persona;
        private string _nombre_comercial;
        private string _pais;
        private string _departamento;
        private string _ciudad;
        private string _direccion;
        private string _razon_social;

        /// <summary>
        /// Código empresa
        /// </summary>
        public short Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        /// <summary>
        /// Identificación
        /// Campo: StrNit      
        /// </summary>
        public string Identificacion
        {
            get { return _identificacion; }
            set { _identificacion = value; }
        }

        /// <summary>
        /// Tipo de identificación
        /// Campo: StrTipoId
        /// </summary>
        public string TipoIdentificacion
        {
            get { return _tipo_identificacion; }
            set { _tipo_identificacion = value; }
        }

        /// <summary>
        /// Regimen
        /// Campo: IntRegimen
        /// Codigo segun la Dian
        /// </summary>
        public int Regimen
        {
            get { return _regimen; }
            set { _regimen = value; }
        }

        /// <summary>
        /// Tipo de persona: natural o jurídica
        /// Campo: IntTipoPersona
        /// Codigo segun la Dian
        /// </summary>
        public int TipoPersona
        {
            get { return _tipo_persona; }
            set { _tipo_persona = value; }
        }

        /// <summary>
        /// Nombre comercial
        /// Campo: StrNombreComercial
        /// Codigo segun la Dian
        /// </summary>
        public string NombreComercial
        {
            get { return _nombre_comercial; }
            set { _nombre_comercial = value; }
        }

        /// <summary>
        /// Pais
        /// Campo: 
        /// Codigo segun la Dian
        /// </summary>
        public string Pais
        {
            get { return _pais; }
            set { _pais = value; }
        }

        /// <summary>
        /// Departamento
        /// Campo:
        /// </summary>
        public string Departamento
        {
            get { return _departamento; }
            set { _departamento = value; }
        }

        /// <summary>
        /// Ciudad
        /// Campo:
        /// </summary>
        public string Ciudad
        {
            get { return _ciudad; }
            set { _ciudad = value; }
        }

        /// <summary>
        /// Dirección
        /// Campo: StrDireccion
        /// </summary>
        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; }
        }

        /// <summary>
        /// Razón social
        /// Campo: StrEmpresa
        /// </summary>
        public string RazonSocial
        {
            get { return _razon_social; }
            set { _razon_social = value; }
        }
    }
}
