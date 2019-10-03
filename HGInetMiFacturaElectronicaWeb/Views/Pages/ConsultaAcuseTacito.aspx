<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaAcuseTacito.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaAcuseTacito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
    <!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs201924"></script>
	<script src="../../Scripts/Services/SrvDocumentos.js?vjs201924"></script>
    <script src="../../Scripts/Services/MaestrosEnum.js?vjs201924"></script>
    <script src="../../Scripts/Pages/ConsultaAcuseTacito.js?vjs201924"></script>
    <!-- CONTENEDOR PRINCIPAL -->
    <div data-ng-app="AcuseConsultaApp" data-ng-controller="AcuseConsultaController"  data-ng-init="total=0">

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

                              
                                <div class="col-md-3" style="margin-top: 2%">
										<div data-hgi-filtro="Facturador"></div>
									</div>

                               <div class="col-md-3" style="margin-top: 2%">
										<div data-hgi-filtro="Adquiriente"></div>
									</div>

                                <div class="col-md-3" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                    <i class="icon-files-empty"></i>
                                    <label>Número Documento:</label>
                                    <div data-dx-autocomplete="filtros.NumeroDocumento"></div>
                                </div>

                                <div class="col-md-3 text-right" style="margin-top: 0px;">
                                    <br />
                                    <br />
                                    <div data-dx-button="ButtonOptionsConsultar"></div>
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
                    <h6 class="panel-title">Datos<input type="text" id="lbltotaldocumentos" style="border-style: none; width: 100%; font-size: small;" readonly></input></h6>
                </div>

                <div class="panel-body">
                    <div class="demo-container">
                        <div id="gridDocumentos"></div>
                        <div class="col-lg-12 text-right" data-ng-show="total>0">
                            <br />
                            <br />
                            <div id="btnProcesar" style="margin-right: 20px;"></div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <!-- /DATOS -->
    </div>
    <!-- /CONTENEDOR PRINCIPAL -->
</asp:Content>
