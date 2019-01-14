<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="DocumentosAdquiriente.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.DocumentosAdquiriente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs201911"></script>
    <script src="../../Scripts/Pages/DocumentosAdquiriente.js?vjs201911"></script>

    <!-- CONTENEDOR PRINCIPAL -->
    <div data-ng-app="DocAdquirienteApp">


        <!--        <div data-ng-include="'ModalPagos.aspx'"></div>-->
        <div data-ng-controller="DocAdquirienteController">
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
                                    <div class="col-md-4">
                                        <i class="icon-file-text"></i>
                                        <label>Filtro Fecha:</label>
                                        <div data-dx-select-box="filtros.TipoFiltroFecha"></div>
                                    </div>

                                    <div class="col-md-4">
                                        <i class=" icon-calendar"></i>
                                        <label>Fecha Inicial:</label>
                                        <div id="FechaInicial"></div>
                                    </div>


                                    <div class="col-md-4">
                                        <i class=" icon-calendar"></i>
                                        <label>Fecha Final:</label>
                                        <div id="FechaFinal"></div>
                                    </div>

                                    <%--<div class="col-md-4">
                                        <i class="icon-file-text"></i>
                                        <label>Estado Recibo:</label>
                                        <div data-dx-select-box="filtros.EstadoRecibo"></div>
                                    </div>--%>

                                    <div class="col-md-8">
                                        <i class="icon-files-empty"></i>
                                        <label>Número Documento:</label>
                                        <div data-dx-autocomplete="filtros.NumeroDocumento"></div>
                                    </div>

                                    <div class="col-md-4 text-right">
                                        <br />                                        
                                        <div data-dx-button="ButtonOptionsConsultar" ></div>
                                    </div>

                                </div>

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
                        <h6 class="panel-title">Datos</h6>
                        <div style="float: right; margin-right: 2%; margin-top: -20px;">
                            <label id="Total" class="text-semibold text-right" style="font-size: medium;"></label>
                        </div>
                    </div>

                    <div class="panel-body">
                        <div class="demo-container">
                            <div id="gridDocumentos"></div>
                        </div>

                    </div>

                </div>
            </div>
            <!--/DATOS -->
        </div>
        <!--Aplicacion de Pagos-->
        <div data-ng-include="'ModalPagos.aspx'"></div>
        <!--Aplicacion de Pagos-->
    </div>
</asp:Content>
