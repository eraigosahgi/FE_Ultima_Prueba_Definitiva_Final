<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <script src="../../Scripts/Pages/Indicadores.js"></script>

    <!-- CONTENEDOR PRINCIPAL -->
    <div class="col-md-12" data-ng-app="IndicadoresApp" data-ng-controller="IndicadoresController">

        <!-- CONTENIDO PANEL-->
        <div class="panel-body">

            <!-- MENÚ TABS -->
            <ul class="nav nav-tabs">
                <li id="LiTabAdministrador"><a id="LinkTabAdministrador" data-ng-if="LinkTabAdministrador" data-ng-init="LinkTabAdministrador=true" href="#TabAdministrador" data-toggle="tab">Administrador</a></li>
                <li id="LiTabFacturador"><a id="LinkTabFacturador" data-ng-if="LinkTabFacturador" data-ng-init="LinkTabFacturador=true" href="#TabFacturador" data-toggle="tab">Facturador</a></li>
                <li id="LiTabAdquiriente"><a id="LinkTabAdquiriente" data-ng-if="LinkTabAdquiriente" data-ng-init="LinkTabAdquiriente=true" href="#TabAdquiriente" data-toggle="tab">Adquiriente</a></li>
            </ul>
            <!--  /MENÚ TABS -->

            <!-- CONTENIDOS TABS -->
            <div class="tab-content">

                <!-- TAB ADMINISTRADOR -->
                <div class="tab-pane active" id="TabAdministrador">

                    <div class="row">

                        <!-- REPORTE ESTADOS DOCUMENTO -->
                        <div class="col-md-12" id="Panel13511" data-ng-if="Panel13511" data-ng-init="Panel13511=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Documentos por Estado</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosEstadoAdmin">
                                        <div class="content-group-sm svg-center position-relative col-md-2 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarDocumentosEstadoAdmin()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADOS DOCUMENTO -->

                        <!-- REPORTE ESTADO ACUSE MENSUAL-->
                        <div class="col-md-6" id="Panel13512" data-ng-if="Panel13512" data-ng-init="Panel13512=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Acuse de Respuesta Mes Actual</h4>
                                    <div data-ng-repeat="c in ReporteAcuseMensualAdmin">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarAcuseMensualAdmin()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE MENSUAL -->

                        <!-- REPORTE ESTADO ACUSE ACUMULADO -->
                        <div class="col-md-6" id="Panel13513" data-ng-if="Panel13513" data-ng-init="Panel13513=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Acuse de Respuesta Acumulado</h4>
                                    <div data-ng-repeat="c in ReporteAcuseAcumuladoAdmin">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarAcuseAcumuladoAdmin()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE ACUMULADO -->

                        <!-- REPORTE TIPO DOCUMENTO ANUAL -->
                        <div class="col-md-8" id="Panel13514" data-ng-if="Panel13514" data-ng-init="Panel13514=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Tipos de Documento por Mes</h4>
                                    <label class="text-muted" style="margin-bottom: 20px">Indica la cantidad de documentos generados por tipo durante los últimos 12 meses.</label>
                                    <div id="ReporteTipoDocumentoAnualAdmin"></div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE TIPO DOCUMENTO ANUAL -->

                        <!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
                        <div class="col-md-4" id="Panel13515" data-ng-if="Panel13515" data-ng-init="Panel13515=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Tipos de Documento Acumulado</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosTipoAdmin">
                                        <div class="content-group-sm svg-center position-relative col-md-12 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarDocumentosTipoAdmin()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->

                        <!-- REPORTE VENTAS -->
                        <div class="col-md-12" id="Panel13516" data-ng-if="Panel13516" data-ng-init="Panel13516=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Ventas</h4>
                                    <label class="text-muted" style="margin-bottom: 20px">Indica el nivel de ventas durante los últimos 12 meses.</label>
                                    <div id="ReporteVentas"></div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE VENTAS -->

                        <!-- REPORTE TOP COMPRADORES -->
                        <div class="col-md-6" id="Panel13517" data-ng-if="Panel13517" data-ng-init="Panel13517=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Top Compradores</h4>
                                    <label class="text-muted" style="margin-bottom: 20px">Indica las empresas con mayor nivel de compras.</label>
                                    <div id="ReporteTopCompradores"></div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE TOP COMPRADORES -->

                        <!-- REPORTE TOP MOVIMIENTOS -->
                        <div class="col-md-6" id="Panel13518" data-ng-if="Panel13518" data-ng-init="Panel13518=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Flujo Transaccional</h4>
                                    <label class="text-muted" style="margin-bottom: 20px">Informa el flujo de transacciones de las empresas con mayor movimiento.</label>
                                    <div id="ReporteTopMovimiento"></div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE TOP MOVIMIENTOS -->

                    </div>

                </div>
                <!-- /TAB ADMINISTRADOR -->

                <!-- TAB FACTURADOR -->
                <div class="tab-pane" id="TabFacturador">

                    <div class="row">

                        <!-- REPORTE ESTADOS DOCUMENTO -->
                        <div class="col-md-12" id="Panel13521" data-ng-if="Panel13521" data-ng-init="Panel13521=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Documentos por Estado</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosEstadoFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-2 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarDocumentosEstadoFacturador()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADOS DOCUMENTO -->

                        <!-- REPORTE SALDOS TRANSACCIONALES -->
                        <div class="col-md-12" id="Panel13522" data-ng-if="Panel13522" data-ng-init="Panel13522=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Resumen Transaccional</h4>
                                    <div class="table-responsive">


                                        <div class="col-md-1"></div>

                                        <div class="col-md-3">

                                            <div class="content-group">
                                                <h4 class="text-semibold no-margin"><i class="icon-copy4 position-left text-slate"></i>
                                                    <label>{{TransaccionesAdquiridas}}</label></h4>
                                                <span class="no-margin text-size-small text-muted">Transacciones Adquiridas</span>
                                            </div>

                                            <div class="content-group">
                                                <h4 class="text-semibold no-margin"><i class="icon-cart-remove position-left text-danger"></i>
                                                    <label>{{TransaccionesProcesadas}}</label></h4>
                                                <span class="no-margin text-size-small text-muted">Transacciones Procesadas</span>
                                            </div>

                                            <div class="content-group">
                                                <h4 class="text-semibold no-margin"><i class="icon-file-check2 position-left text-success"></i>
                                                    <label>{{TransaccionesDisponibles}}</label></h4>
                                                <span class="no-margin text-size-small text-muted">Transacciones Disponibles</span>
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

                        <!-- REPORTE ESTADO ACUSE MENSUAL -->
                        <div class="col-md-6" id="Panel13523" data-ng-if="Panel13523" data-ng-init="Panel13523=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Acuse de Respuesta Mes Actual</h4>
                                    <div data-ng-repeat="c in ReporteAcuseMensualFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarAcuseMensualFacturador()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE MENSUAL -->

                        <!-- REPORTE ESTADO ACUSE ACUMULADO-->
                        <div class="col-md-6" id="Panel13524" data-ng-if="Panel13524" data-ng-init="Panel13524=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Acuse de Respuesta Acumulado</h4>
                                    <div data-ng-repeat="c in ReporteAcuseAcumuladoFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarAcuseAcumuladoFacturador()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE ACUMULADO -->

                        <!-- REPORTE TIPO DOCUMENTO ANUAL -->
                        <div class="col-md-8" id="Panel13525" data-ng-if="Panel13525" data-ng-init="Panel13525=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Tipos de Documento por Mes</h4>
                                    <label class="text-muted" style="margin-bottom: 20px">Indica la cantidad de documentos generados por tipo durante los últimos 12 meses.</label>
                                    <div id="ReporteTipoDocumentoAnualFacturador"></div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE TIPO DOCUMENTO ANUAL -->

                        <!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
                        <div class="col-md-4" id="Panel13526" data-ng-if="Panel13526" data-ng-init="Panel13526=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Tipos de Documento Acumulado</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosTipoFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-12 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarDocumentosTipoFacturador()"></div>
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
                        <div class="col-md-6" id="Panel13531" data-ng-if="Panel13531" data-ng-init="Panel13531=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Acuse de Respuesta Acumulado</h4>
                                    <div data-ng-repeat="c in ReporteAcuseAcumuladoAdquiriente">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarAcuseAcumuladoAdquiriente()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADO ACUSE ACUMULADO-->

                        <!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
                        <div class="col-md-6" id="Panel13532" data-ng-if="Panel13532" data-ng-init="Panel13532=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Tipos de Documento Acumulado</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosTipoAdquiriente">
                                        <div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarDocumentosTipoAdquiriente()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->

                        <!-- REPORTE TIPO DOCUMENTO ANUAL -->
                        <div class="col-md-6" id="Panel13533" data-ng-if="Panel13533" data-ng-init="Panel13533=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Tipos de Documento por Mes</h4>
                                    <label class="text-muted" style="margin-bottom: 20px">Indica la cantidad de documentos generados por tipo durante los últimos 12 meses.</label>
                                    <div id="ReporteTipoDocumentoAnualAdquiriente"></div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE TIPO DOCUMENTO ANUAL -->

                    </div>

                </div>
                <!-- TAB ADQUIRIENTE -->

            </div>
            <!-- /CONTENIDOS TABS -->

        </div>
        <!-- /CONTENIDO PANEL-->

    </div>
    <!-- /CONTENEDOR PRINCIPAL -->


</asp:Content>
