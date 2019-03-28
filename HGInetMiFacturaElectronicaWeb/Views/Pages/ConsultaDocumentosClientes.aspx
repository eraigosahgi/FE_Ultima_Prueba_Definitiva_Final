<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaDocumentosClientes.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaDocumentosClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/SrvDocumentos.js?vjs201915></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs201915></script>
    <script src="../../Scripts/Pages/ConsultaDocumentosClientes.js?vjs201915></script>
    <script src="../../Scripts/Pages/ModalConsultaEmpresas.js?vjs201915></script>

    <div data-ng-app="DocObligadoApp">

        <!-- CONTENEDOR PRINCIPAL -->
        <div data-ng-controller="DocObligadoController">
            <div data-ng-include="'ModalConsultaEmpresas.aspx'"></div>
            <!-- FILTROS DE BÚSQUEDA -->
            <form id="form1" >
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

                                        <div class="col-md-12">
                                            <div class="col-md-6">
                                                <label style="margin-left: -10px; margin-top: 16px; margin-bottom: 1%">Empresa Asociada:</label>
                                                <div id="txtempresaasociada" style="margin-left: -10px; margin-bottom: 1%"></div>

                                            </div>
                                            
                                            <div class="col-md-6" >
                                                <i class="icon-file-text"></i>
                                                <label style="margin-left: -10px; margin-top: 16px; margin-bottom: 1%">Tipo de Busqueda:</label>
                                                <div id="Tipo" style="margin-left: -10px; margin-bottom: 1%"></div>
                                            </div>
                                        </div>


                                    </div>
                                    <div class="dx-fieldset">
                                        <div class="col-md-12" id="divcodigoplataforma" hidden="hidden">
                                            <label style="margin-bottom: 1%">Código de la plataforma:</label>
                                            <div id="txtcodigoplataforma" style="margin-bottom: 1%; margin-right: 10px;"></div>
                                        </div>
                                        <div class="col-md-12 " id="divdocumento" hidden="hidden">
                                            <div class="col-md-6 ">
                                                <label style="margin-top: 16px; margin-bottom: 1%">Resolución:</label>
                                                <div id="Listaresolucion" style="margin-left: -10px; margin-bottom: 1%"></div>
                                            </div>
                                            <div class="col-md-6 ">
                                                <label style="margin-left: -10px; margin-top: 16px; margin-bottom: 1%">Número de Documento:</label>
                                                <div id="txtDocumento" style="margin-left: -10px; margin-bottom: 1%"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <div class="col-lg-12 text-right">
                                <br />
                                <br />
                                <!--<div dx-button="ButtonOptionsConsultar" style="margin-top: -30px; margin-right: 30px"></div>-->
                                <div id="summary"></div>
                                <div id="button"></div>
                            </div>

                            <div class="dx-fieldset">
                                
                            </div>


                        </div>

                    </div>
                </div>
            </form>
            <!--/FILTROS DE BÚSQUEDA -->

            <!-- DATOS -->
            <div class="col-md-12">
                <div class="panel panel-white">
                    <div class="panel-heading">
                        <h6 class="panel-title ">Datos</h6>
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
            <!-- /DATOS -->

        </div>
        <!-- /CONTENEDOR PRINCIPAL -->



        <div id="modal_enviar_email" class="modal fade" style="display: none; margin-top: 15%;" modal="showModal" ng-controller="EnvioEmailController">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div id="EncabezadoModal" class="modal-header">
                        <button type="button" id="btncerrarModal" class="close" data-dismiss="modal">×</button>
                        <h5 style="margin-bottom: 10px" class="modal-title">Envío E-mail Adquiriente</h5>
                    </div>

                    <div class="modal-body text-center">
                        <h6>El siguiente E-mail corresponde al destinatario de este mensaje.
                        </h6>

                        <div id="formEmailEnvio" data-dx-form="formOptionsEmailEnvio">
                        </div>

                    </div>
                    <div id="divsombra" class="modal-footer">
                        <div data-dx-button="buttonCerrarModal" data-dismiss="modal"></div>
                        <div data-dx-button="buttonEnviarEmail"></div>
                    </div>

                </div>
            </div>
        </div>

		<div data-ng-include="'AuditoriaDocumento.aspx'"></div>

    </div>

</asp:Content>

