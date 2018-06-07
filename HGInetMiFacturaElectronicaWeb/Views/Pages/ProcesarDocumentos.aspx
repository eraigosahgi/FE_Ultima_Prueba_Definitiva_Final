<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ProcesarDocumentos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ProcesarDocumentos1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
      <!-- JS DocumentosAdquiriente-->
    <script src="../../Scripts/Pages/ProcesarDocumentos.js"></script>

    <!-- CONTENEDOR PRINCIPAL -->
    <div ng-app="ProcesarDocumentosApp" ng-controller="ProcesarDocumentosController">

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
                                    <label>Fecha Inicial:</label>
                                    <div id="FechaInicial"></div>                                    
                                </div>


                                <div class="col-md-3">
                                    <i class=" icon-calendar"></i>
                                    <label>Fecha Final:</label>
                                     <div id="FechaFinal"></div> 
                                </div>

                                <div class="col-md-3">
                                    <i class="icon-file-text"></i>
                                    <label>Estado:</label>
                                    <div dx-select-box="filtros.EstadoRecibo"></div>
                                </div>

                                <div class="col-md-5">
                                    <i class="icon-files-empty"></i>
                                    <label>Identificador Documento:</label>
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
                    <h6 class="panel-title">Datos :  <input type="text" id="lbltotaldocumentos" style="border-style:none; width:100%; font-size:small;" readonly ></input></h6>                    
                </div>

                <div class="panel-body">               
                     <div class="demo-container">
                        <div id="gridDocumentos"></div>  
                         <div class="col-lg-12 text-right">
                            <br />
                            <br />
                            <div id="btnProcesar"></div>
                        </div>
                         
                    </div>

                  
                               
                </div>

            </div>
        </div>

    </div>
    <!-- /CONTENEDOR PRINCIPAL -->

</asp:Content>
