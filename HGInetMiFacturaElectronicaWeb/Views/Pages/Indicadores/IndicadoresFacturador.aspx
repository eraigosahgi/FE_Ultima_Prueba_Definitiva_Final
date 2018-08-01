<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndicadoresFacturador.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Indicadores.IndicadoresFacturador" %>

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

        <div class="panel">

            <div class="panel-body">
                <h4 class="text-bold" style="margin-top: -10px;">Documentos por Estado</h4>
                <div data-ng-repeat="c in ReporteDocumentosEstadoFacturador">
                    <div class="content-group-sm svg-center position-relative col-md-2 text-center" id="{{c.IdControl}}" data-ng-bind-html="htmlTrusted(c.IdControl,c.Color, c.Porcentaje,c.Titulo, c.Observaciones)"></div>
                </div>
            </div>
        </div>

        <!-- /REPORTE ESTADOS DOCUMENTO -->

        <!-- REPORTE ESTADO ACUSE MENSUAL-->
        <div class="col-md-6">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Respuesta de Acuse Mensual</h4>
                    <div data-ng-repeat="c in ReporteAcuseMensualFacturador">
                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}" data-ng-bind-html="htmlTrusted(c.IdControl,c.Color, c.Porcentaje,c.Titulo, c.Observaciones)"></div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /REPORTE ESTADO ACUSE -->

        <!-- REPORTE ESTADO ACUSE ACUMULADO-->
        <div class="col-md-6">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Respuesta de Acuse Acumulado</h4>
                    <div data-ng-repeat="c in ReporteAcuseAcumuladoFacturador">
                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}" data-ng-bind-html="htmlTrusted(c.IdControl,c.Color, c.Porcentaje,c.Titulo, c.Observaciones)"></div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /REPORTE ESTADO ACUSE -->

        <!-- REPORTE TIPO DOCUMENTO ANUAL -->
        <div class="col-md-8">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Tipo Anual</h4>
                    <label class="text-muted">Indica la cantidad de documentos generados por tipo, durante los últimos 12 meses.</label>
                    <div id="ReporteTipoDocumentoAnualFacturador"></div>
                </div>
            </div>
        </div>
        <!-- /REPORTE TIPO DOCUMENTO ANUAL -->

        <!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
        <div class="col-md-4">
            <div class="panel">
                <div class="panel-body">
                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Tipo Acumulado</h4>
                    <div data-ng-repeat="c in ReporteDocumentosTipoFacturador">
                        <div class="content-group-sm svg-center position-relative col-md-12 text-center" id="{{c.IdControl}}" data-ng-bind-html="htmlTrusted(c.IdControl,c.Color, c.Porcentaje,c.Titulo, c.Observaciones)"></div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->


    </div>


</body>
</html>
