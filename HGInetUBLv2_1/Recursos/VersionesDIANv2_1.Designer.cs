﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HGInetUBLv2_1.Recursos {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class VersionesDIANv2_1 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal VersionesDIANv2_1() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HGInetUBLv2_1.Recursos.VersionesDIANv2_1", typeof(VersionesDIANv2_1).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a CUDE-SHA384.
        /// </summary>
        internal static string CUDE {
            get {
                return ResourceManager.GetString("CUDE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a CUFE-SHA384.
        /// </summary>
        internal static string CUFE {
            get {
                return ResourceManager.GetString("CUFE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Factura Electrónica de Venta.
        /// </summary>
        internal static string ProfileID {
            get {
                return ResourceManager.GetString("ProfileID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a DIAN 2.1: ApplicationResponse de la Factura Electrónica de Venta.
        /// </summary>
        internal static string ProfileIDAR {
            get {
                return ResourceManager.GetString("ProfileIDAR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a UBL 2.1.
        /// </summary>
        internal static string UBLVersionID {
            get {
                return ResourceManager.GetString("UBLVersionID", resourceCulture);
            }
        }
    }
}