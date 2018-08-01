<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <script src="../../Scripts/Pages/Indicadores.js"></script>

    <div class="col-md-12" data-ng-app="IndicadoresApp" data-ng-controller="IndicadoresController">

        <div class="panel-body">

            <%--            <ul class="nav nav-tabs">
                <li class="active"><a href="#TabAdministrador" data-toggle="tab">Administrador</a></li>
                <li><a href="#TabFacturador" data-toggle="tab">Facturador</a></li>
                <li><a href="#TabAdquiriente" data-toggle="tab" aria-expanded="false">Adquiriente</a></li>
            </ul>

            <div class="tab-content">

                <!-- TAB ADMINISTRADOR -->
                <div class="tab-pane active" id="TabAdministrador">

                    <div class="row">

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

                        <!-- REPORTE ESTADOS DOCUMENTO -->
                        <div class="col-md-12">
                            <div class="panel">

                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Estado</h4>
                                    <div class="col-md-3 text-center">
                                        <div class="content-group-sm svg-center position-relative" id="ReporteDocumentosRecibidos"></div>
                                    </div>
                                    <div class="col-md-3 text-center">
                                        <div class="content-group-sm svg-center position-relative" id="ReporteDocumentosEnviados"></div>
                                    </div>
                                    <div class="col-md-3 text-center">
                                        <div class="content-group-sm svg-center position-relative" id="ReporteDocumentosPendientes"></div>
                                    </div>
                                    <div class="col-md-3 text-center">
                                        <div class="content-group-sm svg-center position-relative" id="ReporteDocumentosFinalizados"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADOS DOCUMENTO -->
                    </div>

                </div>
                <!-- /TAB ADMINISTRADOR -->

                <!-- TAB FACTURADOR -->
                <div class="tab-pane" id="TabFacturador">

                    <div class="row">

                        <!-- REPORTE ESTADOS DOCUMENTO -->
                        <div class="col-md-12">
                            <div class="panel">

                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Estado</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosEstadoFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-2 text-center" id="{{c.IdControl}}" data-ng-bind-html="htmlTrusted(c.IdControl,c.Color, c.Porcentaje,c.Titulo, c.Observaciones)"></div>
                                    </div>
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
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Tipo</h4>
                                    <label class="text-muted">Indica la cantidad de documentos generados por tipo.</label>
                                    <div id="ReporteTipoDocumentoAnualFacturador"></div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE TIPO DOCUMENTO ANUAL -->

                        <!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
                        <div class="col-md-4">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Acumulado Documentos por Tipo</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosTipoFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-12 text-center" id="{{c.IdControl}}" data-ng-bind-html="htmlTrusted(c.IdControl,c.Color, c.Porcentaje,c.Titulo, c.Observaciones)"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->


                    </div>
                </div>
                <!-- /TAB FACTURADOR -->

                <!-- TAB ADQUIRIENTE -->
                <div class="tab-pane" id="TabAdquiriente">

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

                </div>
                <!-- TAB ADQUIRIENTE -->

            </div>--%>

            <div class="btn-group btn-group-justified">
                <a href="#" class="btn btn-default btn-raised" data-ng-click="cambiaInclude(1)">Administrador</a>
                <a href="#" class="btn btn-default btn-raised" data-ng-click="cambiaInclude(2)">Facturador</a>
                <a href="#" class="btn btn-default btn-raised" data-ng-click="cambiaInclude(3)">Adquiriente</a>
            </div>
            <br />
            <div data-ng-include="include"></div>

        </div>
    </div>

</asp:Content>
