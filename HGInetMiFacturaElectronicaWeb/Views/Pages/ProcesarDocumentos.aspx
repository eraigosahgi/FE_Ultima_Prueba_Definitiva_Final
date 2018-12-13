<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ProcesarDocumentos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ProcesarDocumentos1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
    <!-- JS DocumentosAdquiriente-->

    <script src="../../Scripts/Services/SrvUsuario.js"></script>
    <script src="../../Scripts/Services/MaestrosEnum.js"></script>
    <script src="../../Scripts/Pages/ProcesarDocumentos.js"></script>
    <script src="../../Scripts/Services/Loading.js"></script>


    <!-- CONTENEDOR PRINCIPAL -->
    <div data-ng-app="ProcesarDocumentosApp" data-ng-controller="ProcesarDocumentosController" data-ng-init="total=0">
        <!-- DATOS -->
        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-heading">
                    <h6 class="panel-title" id="divtotal">Datos : 
                        <input type="text" id="lbltotal" style="border-style: none; width: 100%; font-size: small;" readonly></input>
                        <input type="text" id="lbltotaldocumentos" style="border-style: none; width: 100%; font-size: small;" readonly></input></h6>



                    <div class="col md-12">

                        <div class="row">

                            <div class="dx-fieldset" style="margin-top: -5%;">

                                <div class="col-md-8 ">
                                </div>





                                <div class="col-md-4 text-right">

                                    <div data-dx-button="ButtonOptionsConsultar" style="margin-top: 5%;"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>


                <div class="panel-body">
                    <div class="demo-container">
                        <div id="gridDocumentos"></div>
                        <div class="col-lg-12 text-right">
                            <br />
                            <div data-ng-show="total>0">
                                <div class="col-md-7 text-right">
                                    <i class="icon-file-text"></i>
                                    <label style="margin-top: -10%;">Consulta DIAN:</label>
                                </div>
                                <div class="col-md-2 text-right">
                                    <div data-dx-select-box="filtros.TipoFiltro"></div>
                                </div>
                                <div class="col-md-3 text-right">
                                    <div id="btnProcesar" style="margin-right: 20px;"></div>
                                </div>
                            </div>
                        </div>
                        <div class="demo-container" style="padding-top: 80px;">
                            <div id="panelresultado">
                                <div class="panel-heading">
                                    <h6 class="panel-title">Resultado de documentos procesados : </h6>
                                </div>
                                <hr />

                                <div id="gridDocumentosProcesados"></div>
                            </div>
                        </div>

                    </div>

                </div>

            </div>
        </div>


    </div>
    <!-- /CONTENEDOR PRINCIPAL -->

</asp:Content>
