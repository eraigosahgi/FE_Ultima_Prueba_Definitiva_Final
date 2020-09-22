<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ProcesarInteroperabilidad.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ProcesarInteroperabilidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <script src="../../Scripts/Pages/ProcesarInteroperabilidad.js?vjs20200921"></script>
    <script src="../../Scripts/Pages/ModalConsultaProveedores.js?vjs20200921"></script>

    <div data-ng-app="ProcesarInteroperabilidadApp" data-ng-controller="ProcesarInteroperabilidadController">

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
                    <div class="row">
                        <div class="col-md-8">
                            <br />                            
                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%;">Proveedor Tecnológico:</label>
                            <div id="txtProveedorTecnologico" style="width: 80%"></div>
                        </div>

                        <div class="col-md-4 text-right">
                            <br />
                            <br />
                            <br />
                            <div data-dx-button="ButtonOptionsConsultar" style="margin-top: -10px;"></div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <!--/FILTROS DE BÚSQUEDA -->      
        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-body">
                    <div class="demo-container">
                        <div id="gridDocumentos"></div>
                        <div class="col-lg-12 text-right" style="z-index: 999">
                            <br />
                            <div data-ng-show="total>0">
                                <div id="btnProcesar" style="margin-right: 20px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Panel de resultado--%>
                <div class="panel panel-white">
                    <div class="panel-body">
                        <div class="demo-container">
                            <div id="panelresultado">
                                <div class="panel-heading">
                                    <label id="Lblresultado">Resultado</label>

                                </div>
                                <hr />
                                <div id="gridDocumentosProcesados"></div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>


        <div>
            <div data-ng-include="'ModalConsultaProveedores.aspx'"></div>
        </div>


    </div>
    <!-- /CONTENEDOR PRINCIPAL -->

</asp:Content>
