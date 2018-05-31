<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionPlanesTransacciones.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionPlanesTransacciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <script src="../../Scripts/Pages/PlanesTransacciones.js"></script>
        <script src="../../Scripts/Pages/ModalConsultaEmpresas.js"></script>

    <div ng-app="GestionPlanesApp" ng-controller="GestionPlanesController">


        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-heading">
                    <h6 class="panel-title">Gestión de Planes Transaccionales</h6>
                    
                </div>

                <div class="panel-body">

                    <div class="col-lg-12">

                        <form id="form1" action="your-action">
                            <div class="row">

                                <div class="dx-fieldset">

                                    <div class="col-md-6 col-xs-12" style=" margin-top: 16px; ">
                                        <div class="dx-field-label" style="font-size: 14px;">Tipo Proceso:<strom style="color: red;">*</strom></div>
                                        <div class="dx-field-value">
                                            <div id="TipoProceso"></div>
                                        </div>
                                    </div>

                                    <div class="col-md-4 col-xs-10">
                                        <label style=" margin-top: 16px;">Empresa:<strom style="color: red;">*</strom></label>
                                        <div id="txtempresaasociada"></div>
                                    </div>

                                    <div class="col-md-2 col-xs-2">
                                        <a data-toggle="modal" data-target="#modal_Buscar_empresa" data-popup="tooltip" title="Consulta Empresa" style="color: #166dba;">
                                            <h6 id="SelecionarEmpresa">Seleccionar Empresa</h6>
                                        </a>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin-top: 16px; ">Cantidad Transacciones:<strom style="color: red;">*</strom></label>
                                        <div id="CantidadTransacciones"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style=" margin-top: 16px; ">Valor Plan:<strom style="color: red;">*</strom></label>
                                        <div id="ValorPlan"></div>
                                    </div>

                                   <div class="col-md-6 col-xs-12" style=" margin-top: 16px;">
                                        <div class="dx-field-label" style="font-size: 14px;">Estado:<strom style="color: red;">*</strom></div>
                                        <div class="dx-field-value">
                                            <div id="EstadoPlan"></div>
                                        </div>
                                    </div>

                                    <div class="col-md-12" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Observaciones:</label>                                        
                                            <div id="txtObservaciones"></div>                                        
                                    </div>

                                </div>

                                <div class="dx-fieldset">
                                    <div id="summary"></div>
                                </div>

                            </div>

                            <div class="col-lg-12 text-right">
                                <br />
                                <br />
                                <a id="btncancelar" class="btn btn-default" style="text-transform: initial !important; margin-right: 1%" href="/Views/Pages/ConsultaPlanesTransacciones.aspx" style="font-size: 14px; text-align: center;">Cancelar</a>
                                <div id="button"></div>
                            </div>

                        </form>

                    </div>

                </div>
            </div>
        </div>
        <div ng-if="Admin">
            <div ng-include="'ModalConsultaEmpresas.aspx'"></div>
        </div>
    </div>

</asp:Content>
