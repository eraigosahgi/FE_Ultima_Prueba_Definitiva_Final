using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaData.Enumerables
{
    /// <summary>
    /// Enumerable de Códigos de documentos de acuerdo a los valores definidos en Guia de Interoperabilidad en la ​Tabla No. 5.2.1: Códigos de respuesta - Capítulo No. 5.2.  
    /// </summary>
    public enum DocumentType
    {
        [Description("FV")]
        FacturaNacional = 1,

        [Description("ND")]
        NotaDebito = 2,

        [Description("NC")]
        NotaCredito = 3,

        [Description("FC")]
        FacturaContingencia = 4,

        [Description("FE")]
        FacturaExportacion = 5,

        [Description("Acuse de Recibo")]
        AcuseDeRecibo = 6,

        [Description("Aceptacion")]
        Aceptacion = 7,

        [Description("Rechazo")]
        Rechazo = 8,

        [Description("Otro")]
        Otro = 9,
    }

    /// <summary>
    /// Código de respuesta de acuerdo a los valores definidos en Guia de Interoperabilidad en la ​Tabla No. 5.2.1: Códigos de respuesta - Capítulo No. 5.2. 
    /// </summary>
    public enum ResponseCode
    {
        [Description("RECEIVED")]
        Pendiente = 0,

        [Description("ACCEPTED")]
        Aceptado = 1,

        [Description("REJECTED")]
        Rechazado = 2,

        [Description("TACITLY_ACCEPTED")]
        AprobadoTacito = 3,

        [Description("PAID")]
        Pagado = 4
    }

    public enum RespuestaInterOperabilidad
    {
        [Description("El zip se radicó exitosamente")]
        ZipRadicado = 200,

        [Description("Documento encolado para procesamiento")]
        PendienteProcesamiento = 201,

        [Description("Documento no encontrado en el archivo ZIP ")]
        DocumentoNoEncontrado = 404,

        [Description("El cliente destinatario de la factura electrónica no tiene convenio con el receptor ")]
        ClienteNoEncontrado = 406,

        [Description("Error interno del receptor del documento electrónico ")]
        ErrorInternoReceptor = 500,

        [Description("Usuario no autenticado ")]
        UsuarioNoAutenticado = 401,

        [Description("El documento especificado en el campo nombre no existe en el servidor SFTP")]
        DocumentoNoEncontradoZip = 412,

        [Description("El archivo comprimido contiene más de 100 documentos electrónicos ")]
        ZipSuperaMaximo = 414,

        [Description("El zip especificado no contiene documentos electrónicos ")]
        Zipvacio = 415,

        [Description("El zip se procesó parcialmente ")]
        ProcesamientoParcial = 409,

    }

    /// <summary>
    /// Enumerable para el manejo de las respuestas de la consulta de documento por uuid Global del Documento
    /// </summary>
    public enum RespuestaInterOperabilidadUUID
    {
        [Description("El documento electrónico existe en el sistema  se procede a dar el estado de procesamiento ")]
        ConsultaExitosa = 201,

        [Description("El documento electrónico asociado al UUID consultado no existe en el sistema")]
        Noexiste = 409,

        [Description("El documento electrónico asociado al UUID consultado existe en el sistema pero fue registrado por un proveedor de factura diferente ")]
        Noexisteparaproveedorcosnsultado = 406,

        [Description("Error interno del receptor del documento electrónico ")]
        ErrorInterno = 500,        

        [Description("Usuario no autenticado ")]
        UsuarioNoAutenticado = 400,
        
    }


    /// <summary>
    /// Codigos de respuesta del Historial de documento detallado por estado
    /// </summary>
    public enum RespuestaUUIDDocumento
    {
        [Description("Radicado")]
        Radicado = 0,

        [Description("Aceptado")]
        Aceptado = 1,

        [Description("Rechazado")]
        Rechazado = 2,

        [Description("Tácitamente aceptado")]
        AprobadoTacito = 3,

        [Description("Pagado")]
        Pagado = 4
    }




    /// <summary>
    /// Codigos de respuesta del Historial de documento detallado por estado
    /// </summary>
    public enum RespuestaUUIDMensaje
    {
        [Description("Documento radicado en el Operador del receptor")]
        Pendiente = 0,

        [Description("Documento aceptado por el cliente")]
        Aceptado = 1,

        [Description("Documento rechazado por el cliente")]
        Rechazado = 2,

        [Description("Documento Tácitamente aceptado por el cliente")]
        AprobadoTacito = 3,

        [Description("Documento Pagado por el cliente")]
        Pagado = 4
    }

}
