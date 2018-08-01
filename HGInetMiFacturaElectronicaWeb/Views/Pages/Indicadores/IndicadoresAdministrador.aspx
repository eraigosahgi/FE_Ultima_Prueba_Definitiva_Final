<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndicadoresAdministrador.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Indicadores.IndicadoresAdministrador" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Mi Factura en Línea</title>
</head>
<body>
    <div class="row">

        <!-- REPORTE ESTADOS DOCUMENTO -->
        <div class="col-md-12">
            <div class="panel">

                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Estado</h4>
                    <div data-ng-repeat="c in ReporteDocumentosEstadoAdmin">
                        <div class="content-group-sm svg-center position-relative col-md-2 text-center" id="{{c.IdControl}}" data-ng-bind-html="htmlTrusted(c.IdControl,c.Color, c.Porcentaje,c.Titulo, c.Observaciones)"></div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /REPORTE ESTADOS DOCUMENTO -->


        <!-- REPORTE ESTADO ACUSE -->
        <div class="col-md-4">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Documentos por Respuesta de Acuse</h4>
                    <label class="text-muted">Indica el porcentaje de documentos según el estado del acuse.</label>
                    <div class="col-md-6" style="margin-top: -40px;">
                        <div id="ReporteMesActualEstadoAcuse"></div>
                    </div>
                    <div class="col-md-6" style="margin-top: -40px;">
                        <div id="ReporteAcumuladoEstadoAcuse"></div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /REPORTE ESTADO ACUSE -->

        <!-- REPORTE TIPO DOCUMENTO ANUAL -->
        <div class="col-md-5">
            <div class="panel">

                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Documentos por Tipo</h4>
                    <label class="text-muted">Indica la cantidad de documentos generados por tipo.</label>
                    <div id="ReporteTipoDocumentoAnual"></div>
                </div>

            </div>
        </div>
        <!-- /REPORTE TIPO DOCUMENTO ANUAL -->

        <!-- REPORTE FLUJO DOCUMENTOS -->
        <div class="col-md-3">
            <div class="panel">

                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Flujo Documentos Diario</h4>
                    <div class="col-md-6 text-center">
                        <div class="content-group-sm svg-center position-relative" id="ReporteFlujoDiarioPendientes"></div>
                    </div>
                    <div class="col-md-6 text-center">
                        <div class="content-group-sm svg-center position-relative" id="ReporteFlujoDiarioProcesados"></div>
                    </div>
                </div>

            </div>
        </div>
        <!-- /REPORTE FLUJO DOCUMENTOS -->

        <!-- REPORTE ESTADOS RESPUESTA -->
        <div class="col-md-3">
            <div class="panel">

                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Respuesta Documentos</h4>
                    <div class="col-md-6 text-center">
                        <div class="content-group-sm svg-center position-relative" id="ReporteDocumentosRechazados"></div>
                    </div>
                    <div class="col-md-6 text-center">
                        <div class="content-group-sm svg-center position-relative" id="ReporteDocumentosAprobados"></div>
                    </div>
                </div>

            </div>
        </div>
        <!-- /REPORTE ESTADOS RESPUESTA -->

        <!-- REPORTE VENTAS -->
        <div class="col-md-5">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Ventas</h4>
                    <label class="text-muted">Indica el nivel de ventas durante los últimos 12 meses.</label>
                    <div id="ReporteVentas"></div>
                </div>
            </div>
        </div>
        <!-- /REPORTE VENTAS -->

        <!-- REPORTE TOP COMPRADORES -->
        <div class="col-md-7">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Top Compradores</h4>
                    <label class="text-muted">Indica las empresas con mayor nivel de compras.</label>
                    <div id="ReporteTopCompradores"></div>
                </div>
            </div>
        </div>
        <!-- /REPORTE TOP COMPRADORES -->

        <!-- REPORTE TOP MOVIMIENTOS -->
        <div class="col-md-8">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Flujo Transaccional</h4>
                    <label class="text-muted">Informa el flujo de transacciones de las empresas con mayor movimiento.</label>
                    <div id="ReporteTopMovimiento"></div>
                </div>
            </div>
        </div>
        <!-- /REPORTE TOP MOVIMIENTOS -->

        <!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
        <div class="col-md-4">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Acumulado Documentos por Tipo</h4>
                    <label class="text-muted">Informa el porcentaje acumulado de documentos generados por tipo.</label>
                    <div id="ReporteAcumuladoTipoDocumento"></div>
                </div>
            </div>
        </div>
        <!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->


    </div>
</body>
</html>
