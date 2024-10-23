using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Objetos
{


    public class PermisoUsuario : Permiso
    {

        private string usuario;

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }


    }

    public class PermisoPerfil : Permiso
    {

        private string perfil;

        public string Perfil
        {
            get { return perfil; }
            set { perfil = value; }
        }

    }

    public class Permiso
    {
        private string _aplicacion;
        private string _opcion;
        private bool _imprimir;
        private bool _gestion;
        private bool _anular;
        private bool _eliminar;
        private bool _ingresar;
        public bool _editar;
        private bool _activado;
        private bool _servicios;
        private string _descripcion;
        private string _strurl;
        private string _strclasecss;
        private string _opcionpadre;
        private int _tipo;

        /// <summary>
        /// Código del aplicativo
        /// </summary>
        public string Aplicacion
        {
            get { return _aplicacion; }
            set { _aplicacion = value; }
        }

        /// <summary>
        /// Código de opción
        /// </summary>
        public string Opcion
        {
            get { return _opcion; }
            set { _opcion = value; }
        }

        /// <summary>
        /// activado
        /// </summary>
        public bool Activado
        {
            get { return _activado; }
            set { _activado = value; }
        }

        /// <summary>
        /// editar
        /// </summary>
        public bool Editar
        {
            get { return _editar; }
            set { _editar = value; }
        }

        /// <summary>
        /// ingresar
        /// </summary>
        public bool Ingresar
        {
            get { return _ingresar; }
            set { _ingresar = value; }
        }

        /// <summary>
        /// eliminar
        /// </summary>
        public bool Eliminar
        {
            get { return _eliminar; }
            set { _eliminar = value; }
        }

        /// <summary>
        /// anular
        /// </summary>
        public bool Anular
        {
            get { return _anular; }
            set { _anular = value; }
        }

        /// <summary>
        /// imprimir
        /// </summary>
        public bool Imprimir
        {
            get { return _imprimir; }
            set { _imprimir = value; }
        }

        /// <summary>
        /// gestion
        /// </summary>
        public bool Gestion
        {
            get { return _gestion; }
            set { _gestion = value; }
        }

        /// <summary>
        /// gestion
        /// </summary>
        public bool Servicios
        {
            get { return _servicios; }
            set { _servicios = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public string StrClaseCss
        {
            get { return _strclasecss; }
            set { _strclasecss = value; }
        }

        public string StrUrl
        {
            get { return _strurl; }
            set { _strurl = value; }
        }

        public string OpcionPadre
        {
            get { return _opcionpadre; }
            set { _opcionpadre = value; }
        }

        public int Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
    }

    public class objetoPermisosUsuario
    {
        public string Aplicacion { get; set; }
        public string Codigo { get; set; }
        public bool Imprimir { get; set; }
        public bool Gestion { get; set; }
        public bool Anular { get; set; }
        public bool Eliminar { get; set; }
        public bool Ingresar { get; set; }
        public bool Editar { get; set; }
        public bool Activado { get; set; }
        public bool Servicios { get; set; }
        public string Descripcion { get; set; }
        public string Strurl { get; set; }
        public string Strclasecss { get; set; }
        public string Dependencia { get; set; }
        public int Tipo { get; set; }
    }
}
