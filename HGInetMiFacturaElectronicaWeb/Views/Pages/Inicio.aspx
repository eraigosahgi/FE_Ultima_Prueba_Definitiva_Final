<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <script src="../../Scripts/Pages/Indicadores.js"></script>

    <div class="col-md-12" data-ng-app="IndicadoresApp" data-ng-controller="IndicadoresController">

        <div class="panel-body">

            <ul class="nav nav-tabs">
                <li class="active"><a href="#TabAdministrador" data-toggle="tab">Administrador</a></li>
                <li><a href="#TabFacturador" data-toggle="tab">Facturador</a></li>
                <li><a href="#TabAdquiriente" data-toggle="tab" aria-expanded="false">Adquiriente</a></li>
            </ul>

            <div class="tab-content">

                <!-- TAB ADMINISTRADOR -->
                <div class="tab-pane active" id="TabAdministrador">

                    <div class="row">

                        <div class="col-md-12">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Estado</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosEstadoAdmin">
                                        <div class="content-group-sm svg-center position-relative col-md-2 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarDocumentosEstadoAdmin()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- REPORTE ESTADO ACUSE MENSUAL-->
                        <div class="col-md-6">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Respuesta de Acuse Mensual</h4>
                                    <div data-ng-repeat="c in ReporteAcuseMensualAdmin">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarAcuseMensualAdmin()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE MENSUAL -->

                        <!-- REPORTE ESTADO ACUSE ACUMULADO -->
                        <div class="col-md-6">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Respuesta de Acuse Acumulado</h4>
                                    <div data-ng-repeat="c in ReporteAcuseAcumuladoAdmin">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarAcuseAcumuladoAdmin()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE ACUMULADO -->

                        <!-- REPORTE TIPO DOCUMENTO ANUAL -->
                        <div class="col-md-8">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Tipo</h4>
                                    <label class="text-muted">Indica la cantidad de documentos generados por tipo durante los últimos 12 meses.</label>
                                    <div id="ReporteTipoDocumentoAnualAdmin"></div>
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
                                    <h4 class="text-bold" style="margin-top: -10px;">Acumulado Documentos por Tipo</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosTipoAdmin">
                                        <div class="content-group-sm svg-center position-relative col-md-12 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarDocumentosTipoAdmin()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->

                    </div>
                </div>
                <!-- /TAB ADMINISTRADOR -->

                <!-- TAB FACTURADOR -->
                <div class="tab-pane" id="TabFacturador">

                    <div class="row">

                        <!-- REPORTE SALDOS TRANSACCIONALES -->
                        <div class="col-md-12">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Resumen Transaccional</h4>
                                    <div class="table-responsive">


                                        <div class="col-md-1"></div>

                                        <div class="col-md-3">

                                            <div class="content-group">
                                                <h4 class="text-semibold no-margin"><i class="icon-copy4 position-left text-slate"></i>
                                                    <label>{{SaldoPlanActual}}</label></h4>
                                                <span class="no-margin text-size-small text-muted">Total Plan Actual</span>
                                            </div>

                                            <div class="content-group">
                                                <h4 class="text-semibold no-margin"><i class="icon-cart-remove position-left text-danger"></i>
                                                    <label>{{SaldoConsumoPlanActual}}</label></h4>
                                                <span class="no-margin text-size-small text-muted">Consumo Plan Actual</span>
                                            </div>

                                            <div class="content-group">
                                                <h4 class="text-semibold no-margin"><i class="icon-cart-add2 position-left text-success"></i>
                                                    <label>{{SaldoCompras}}</label></h4>
                                                <span class="no-margin text-size-small text-muted">Saldo Nuevas Compras</span>
                                            </div>

                                            <div class="content-group">
                                                <h4 class="text-semibold no-margin"><i class="icon-file-check2 position-left text-success"></i>
                                                    <label>{{SaldoDisponible}}</label></h4>
                                                <span class="no-margin text-size-small text-muted">Saldo Disponible</span>
                                            </div>

                                        </div>

                                        <div class="col-md-7">
                                            <h4 class="text-semibold">Planes Adquiridos</h4>
                                            <div id="ResumenPlanesAdquiridosFacturador"></div>
                                        </div>

                                        <div class="col-md-1"></div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /SALDOS TRANSACCIONALES -->

                        <!-- REPORTE ESTADOS DOCUMENTO -->
                        <div class="col-md-12">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Estado</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosEstadoFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-2 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarDocumentosEstadoFacturador()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADOS DOCUMENTO -->

                        <!-- REPORTE ESTADO ACUSE MENSUAL -->
                        <div class="col-md-6">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Respuesta de Acuse Mensual</h4>
                                    <div data-ng-repeat="c in ReporteAcuseMensualFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarAcuseMensualFacturador()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE MENSUAL -->

                        <!-- REPORTE ESTADO ACUSE ACUMULADO-->
                        <div class="col-md-6">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Respuesta de Acuse Acumulado</h4>
                                    <div data-ng-repeat="c in ReporteAcuseAcumuladoFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarAcuseAcumuladoFacturador()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE ACUMULADO -->

                        <!-- REPORTE TIPO DOCUMENTO ANUAL -->
                        <div class="col-md-8">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Tipo</h4>
                                    <label class="text-muted">Indica la cantidad de documentos generados por tipo durante los últimos 12 meses.</label>
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
                                        <div class="content-group-sm svg-center position-relative col-md-12 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarDocumentosTipoFacturador()"></div>
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

                        <!-- REPORTE ESTADO ACUSE ACUMULADO-->
                        <div class="col-md-6">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Respuesta de Acuse Acumulado</h4>
                                    <div data-ng-repeat="c in ReporteAcuseAcumuladoAdquiriente">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarAcuseAcumuladoAdquiriente()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE ACUMULADO-->

                        <!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
                        <div class="col-md-4">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Acumulado Documentos por Tipo</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosTipoAdquiriente">
                                        <div class="content-group-sm svg-center position-relative col-md-12 text-center" id="{{c.IdControl}}"></div>
                                        <div ng-if="$last" data-ng-init="CargarDocumentosTipoAdquiriente()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->

                        <!-- REPORTE TIPO DOCUMENTO ANUAL -->
                        <div class="col-md-6">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px;">Documentos por Tipo</h4>
                                    <label class="text-muted">Indica la cantidad de documentos generados por tipo durante los últimos 12 meses.</label>
                                    <div id="ReporteTipoDocumentoAnualAdquiriente"></div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE TIPO DOCUMENTO ANUAL -->


                    </div>

                </div>
                <!-- TAB ADQUIRIENTE -->

            </div>
        </div>



        <div id="content"></div>


    </div>



</asp:Content>
