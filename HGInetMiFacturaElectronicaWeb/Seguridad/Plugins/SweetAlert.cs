using HGInetMiFacturaElectronicaWeb.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HGInetMiFacturaElectronicaWeb.Seguridad.Plugins
{
    public class SweetAlert
    {
        protected static string tab = "\t";
        protected static string line = "\n";

        /// <summary>
        /// Título que se mostrará la notificación
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Mensaje que se mostrará la notificación
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Tiempo en que se mostrará la notificación en milisegundos
        /// </summary>
        public int Tiempo { get; set; }

        /// <summary>
        /// Se redirecciona o no  a otra pagina
        /// </summary>
        public bool Redireccionar { get; set; }

        /// <summary>
        /// Ruta a la que se va a redireccionar la pagina
        /// </summary>
        public string Ruta { get; set; }

        /// <summary>
        /// Notificación 
        /// </summary>
        public SweetAlert()
        {
        }

        /// <summary>
        /// Notificación 
        /// </summary>
        /// <param name="titulo">título</param>
        /// <param name="mensaje">mensaje</param>
        public SweetAlert(string titulo, string mensaje)
        {
            this.Titulo = titulo;
            this.Mensaje = mensaje;
        }

        /// <summary>
        /// Color de fondo de la notificación
        /// </summary>
        private string ColorConfirmacion
        {
            get
            {
                if (Tipo == TipoMensaje.warning)
                    return "#FF7043";
                else if (Tipo == TipoMensaje.info)
                    return "#2196F3";
                else if (Tipo == TipoMensaje.success)
                    return "#66BB6A";
                else if (Tipo == TipoMensaje.error)
                    return "#EF5350";
                else
                    return "#2196F3";
            }
        }

        /// <summary>
        /// Tipo notificación (enum TipoMensaje)
        /// </summary>
        public TipoMensaje Tipo { get; set; }

        /// <summary>
        /// Tipo de mensaje para la notificación (PNotify)
        /// </summary>
        public enum TipoMensaje
        {
            warning = 1,
            info = 2,
            success = 3,
            error = 4
        }

        /// <summary>
        /// Tipo animación (enum TipoAnimacion)
        /// </summary>
        public TipoAnimacion Animacion { get; set; }

        /// <summary>
        /// Tipo de mensaje para la notificación (PNotify)
        /// </summary>
        public enum TipoAnimacion
        {
            none = 1,
            pop = 2,
            slide_from_top = 3,
            slide_from_bottom = 4
        }


        /// <summary>
        /// Notificación 
        /// </summary>
        /// <param name="titulo">título</param>
        /// <param name="mensaje">mensaje (string, Exception)</param>
        /// <param name="tipo">tipo notificación (enum TipoMensaje)</param>
        /// <param name="animacion">tipo animación (enum TipoAnimacion)</param>
        /// <param name="tiempo">tiempo en que se mostrará la notificación en milisegundos</param>
        public SweetAlert(string titulo, object mensaje, TipoMensaje tipo = TipoMensaje.info, TipoAnimacion animacion = TipoAnimacion.pop, int tiempo = 0)
        {
            this.Titulo = titulo;

            Type mensajeObjeto = mensaje.GetType();
            Type mensajeObjetoHerencia = mensaje.GetType().BaseType;

            if (mensajeObjeto == typeof(string))
                this.Mensaje = mensaje.ToString();
            else if (mensajeObjeto == typeof(System.Exception) || mensajeObjeto == typeof(System.ApplicationException) || mensajeObjetoHerencia == typeof(System.Exception) || mensajeObjetoHerencia == typeof(System.ApplicationException))
            {
                string text = (mensaje as Exception).Message;

                if ((mensaje as Exception).InnerException != null)
                    text = string.Format("{0}<br/>{1}", text, (mensaje as Exception).InnerException.Message);

                this.Mensaje = text.ToString();
            }

            if (tiempo <= 0)
                this.Tiempo = Convert.ToInt32(Constantes.SweetAlert_delay);
            else
                this.Tiempo = tiempo;
            this.Tipo = tipo;
            this.Animacion = TipoAnimacion.pop;
        }

        /// <summary>
        /// Notificación 
        /// </summary>
        /// <param name="titulo">título</param>
        /// <param name="mensaje">mensaje (string, Exception)</param>
        /// <param name="tipo">tipo notificación (enum TipoMensaje)</param>
        /// <param name="animacion">tipo animación (enum TipoAnimacion)</param>
        /// <param name="tiempo">tiempo en que se mostrará la notificación en milisegundos</param>
        /// /// <param name="redireccionar">Si va a redireccionar o no</param>
        /// /// <param name="ruta">ruta a la que va a redireccionar</param>
        public SweetAlert(string titulo, object mensaje, bool redireccionar = false, string ruta = "", TipoMensaje tipo = TipoMensaje.info, TipoAnimacion animacion = TipoAnimacion.pop, int tiempo = 0)
        {
            this.Titulo = titulo;

            Type mensajeObjeto = mensaje.GetType();
            Type mensajeObjetoHerencia = mensaje.GetType().BaseType;

            if (mensajeObjeto == typeof(string))
                this.Mensaje = mensaje.ToString();
            else if (mensajeObjeto == typeof(System.Exception) || mensajeObjeto == typeof(System.ApplicationException) || mensajeObjetoHerencia == typeof(System.Exception) || mensajeObjetoHerencia == typeof(System.ApplicationException))
            {
                string text = (mensaje as Exception).Message;

                if ((mensaje as Exception).InnerException != null)
                    text = string.Format("{0}<br/>{1}", text, (mensaje as Exception).InnerException.Message);

                this.Mensaje = text.ToString();
            }

            if (tiempo <= 0)
                this.Tiempo = Convert.ToInt32(Constantes.SweetAlert_delay);
            else
                this.Tiempo = tiempo;

            this.Redireccionar = redireccionar;
            this.Ruta = ruta;
            this.Tipo = tipo;
            this.Animacion = TipoAnimacion.pop;
        }



        /// <summary>
        /// Genera el script para mostrar en la página web un mensaje de notificación
        /// Alerta, Información, Éxito o Error
        /// </summary>
        /// <returns>texto javascript</returns>
        public StringBuilder ObtenerScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();

            string notificacionTitulo = this.Titulo.Replace("'", "\'");

            string notificacionMensaje = this.Mensaje.Replace("'", "").Replace("\"", "");

            script.Append("$(function () {");
            script.Append("new swal({\n");
            script.AppendFormat("{0}title: '{2}', {1}", tab, line, notificacionTitulo);
            script.AppendFormat("{0}text: '{2}', {1}", tab, line, notificacionMensaje);
            script.AppendFormat("{0}type: '{2}', {1}", tab, line, this.Tipo.ToString());
            script.AppendFormat("{0}confirmButtonColor: '{2}', {1}", tab, line, this.ColorConfirmacion);
            script.AppendFormat("{0}confirmButtonText: '{2}', {1}", tab, line, Constantes.SweetAlert_confirmButtonText);
            script.AppendFormat("{0}animation: '{2}', {1}", tab, line, this.Animacion.ToString().Replace("_", "-"));
            script.AppendFormat("{0}html: true, {1}", tab, line);

            if (Convert.ToInt32(Constantes.SweetAlert_delay) > 0)
                script.AppendFormat("{0}timer: '{2}', {1}", tab, line, this.Tipo.ToString());

            if (this.Redireccionar == true)
            {
                script.AppendFormat("{0}closeOnConfirm: false, {1}", tab, line);
                script.Append("},\n");
                script.Append("function(isConfirm) {");
                script.Append("if(isConfirm) {");
                script.AppendFormat("{0}location.href= '{2}' {1}", tab, line, Ruta);
                script.Append("}\n");
                script.Append("else{\n");
                script.Append("new swal({\n");
                script.AppendFormat("{0}title: '{2}' {1}", tab, line, notificacionTitulo);
                script.Append("});\n");
                script.Append("}\n");
                script.Append("});\n");
            }
            else
            {
                script.Append("\t});\n");
            }
            script.Append("});\n");
            return script;
        }



    }
}