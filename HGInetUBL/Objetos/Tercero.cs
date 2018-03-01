using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HGInetUBL.Objetos
{
    /// <summary>
    /// Información del adquiriente
    /// </summary>
    internal class xTercero
    {
        private string _identificacion;
        private string _tipo_identificacion;
        private int _regimen;
        private int _tipo_persona;
        private string _pais;
        private string _departamento;
        private string _ciudad;
        private string _direccion;
        private string _razon_social;
        private string _primer_nombre;
        private string _segundo_nombre;
        private string _primer_apellido;
        private string _segundo_apellido;

        /// <summary>
        /// Identificación
        /// Campo: StrIdTercero      
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
        /// Campo: StrApellido1
        /// </summary>
        public string RazonSocial
        {
            get { return _razon_social; }
            set { _razon_social = value; }
        }

        /// <summary>
        /// Primer Nombre
        /// Campo: StrNombre1
        /// </summary>
        public string PrimerNombre
        {
            get { return _primer_nombre; }
            set { _primer_nombre = value; }
        }

        /// <summary>
        /// Segundo Nombre
        /// Campo: StrNombre2
        /// </summary>
        public string SegundoNombre
        {
            get { return _segundo_nombre; }
            set { _segundo_nombre = value; }
        }

        /// <summary>
        /// Primer Apellido
        /// Campo: StrApellido1 
        /// </summary>
        public string PrimerApellido
        {
            get { return _primer_apellido; }
            set { _primer_apellido = value; }
        }

        /// <summary>
        /// Segundo Apellido
        /// Campo: StrApellido2
        /// </summary>
        public string SegundoApellido
        {
            get { return _segundo_apellido; }
            set { _segundo_apellido = value; }
        }

    }
}
