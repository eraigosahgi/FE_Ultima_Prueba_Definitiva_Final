<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="DocumentosAdquiriente.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.DocumentosAdquiriente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <!-- JS DocumentosAdquiriente-->
    <script src="../../Scripts/Pages/DocumentosAdquiriente.js"></script>

    <!-- CONTENEDOR PRINCIPAL -->
    <div ng-app="DocAdquirienteApp" ng-controller="DocAdquirienteController">

        <!-- FILTROS DE BÚSQUEDA -->

        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-heading">
                    <h6 class="panel-title">Filtros de Búsqueda<a class="heading-elements-toggle"><i class="icon-more"></i></a></h6>
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a data-action="collapse"></a></li>
                        </ul>
                    </div>
                </div>

                <div class="panel-body">

                    <div class="col-lg-12">

                        <div class="row">

                            <div class="dx-fieldset">

                                <div class="col-md-3">
                                    <i class=" icon-calendar"></i>
                                    <label>Fecha Inicial</label>
                                    <div dx-date-box="filtros.FechaInicial"></div>
                                </div>

                                <div class="col-md-3">
                                    <i class=" icon-calendar"></i>
                                    <label>Fecha Final</label>
                                    <div dx-date-box="filtros.FechaFinal"></div>
                                </div>

                                <div class="col-md-3">
                                    <i class="icon-file-text"></i>
                                    <label>Estado Recibo</label>
                                    <div dx-select-box="filtros.EstadoRecibo"></div>
                                </div>

                                <div class="col-md-3">
                                    <i class="icon-files-empty"></i>
                                    <label>Número Documento:</label>
                                    <div dx-autocomplete="filtros.NumeroDocumento"></div>
                                </div>

                                <div class="col-md-4">
                                </div>

                            </div>

                        </div>

                        <div class="col-lg-12 text-right">
                            <br />
                            <br />
                            <div dx-button="ButtonOptionsConsultar"></div>
                        </div>

                    </div>

                </div>

            </div>
        </div>

        <!--/FILTROS DE BÚSQUEDA -->

        <!-- DATOS -->
        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-heading">
                    <h6 class="panel-title">Datos<a class="heading-elements-toggle"><i class="icon-more"></i></a></h6>
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a data-action="collapse"></a></li>
                        </ul>
                    </div>
                </div>

                <div class="panel-body">
                    <div id="gridContainer" dx-data-grid="dataGridOptions">
                    </div>
                </div>

            </div>
        </div>
        <!-- /DATOS -->

    </div>
    <!-- /CONTENEDOR PRINCIPAL -->

</asp:Content>
