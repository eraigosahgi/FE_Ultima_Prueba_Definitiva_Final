<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndicadoresAdquiriente.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Indicadores.IndicadoresAdquiriente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div class="row">

        <!-- REPORTE ESTADO ACUSE -->
        <div class="col-md-4">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Documentos por Respuesta de Acuse</h4>
                    <label class="text-muted">Indica el porcentaje de documentos según el estado del acuse.</label>
                    <div class="col-md-12">
                        <div id="ReporteAcumEstadoAcuseAdquiriente"></div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /REPORTE MES ACTUAL ESTADO ACUSE -->

        <!-- REPORTE TIPO DOCUMENTO ANUAL -->
        <div class="col-md-4">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Documentos por Tipo</h4>
                    <label class="text-muted">Indica la cantidad de documentos generados por tipo.</label>
                    <div id="ReporteTipoDocumentoAnualAdquiriente"></div>
                </div>
            </div>
        </div>
        <!-- /REPORTE TIPO DOCUMENTO ANUAL -->

        <!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
        <div class="col-md-4">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Acumulado Documentos por Tipo</h4>
                    <label class="text-muted">Informa el porcentaje acumulado de documentos generados por tipo.</label>
                    <div id="ReporteAcumuladoTipoDocumentoAdquiriente"></div>
                </div>
            </div>
        </div>
        <!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->

    </div>
</body>
</html>
