using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades
{
    public enum TipoDocumentoElectronico
    {
        [Description("No Aplica")]
        NoAplica = 0,

        [Description("Factura")]
        Factura = 1,

        [Description("Nota Débito")]
        NotaDebito = 2,

        [Description("Nota Crédito")]
        NotaCredito = 3,

        [Description("Contingencia Factura")]
        ContingenciaFactura = 4,

        [Description("Factura Exportación")]
        FacturaExportacion = 5,

        [Description("Compra Electrónica")]
        CompraElectronica = 6,

        [Description("Documento Soporte")]
        DocumentoSoporte = 7,

        [Description("Nota Documento Soporte")]
        NotaDocumetoSoporte = 8,

        [Description("Nota Credito No Referenciada")]
        NotaCreditoNoReferenciada = 9,

        [Description("POS electronico")]
        POSelectronico = 10,

        [Description("Nota POS electronico")]
        NotaPOSelectronico = 11,

    }
}
