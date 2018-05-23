<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionEmpresas.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionEmpresas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
    <script src="../../Scripts/Pages/Empresas.js"></script>
    <script src="../../Scripts/Pages/ModalConsultaEmpresas.js"></script>

    <div ng-app="EmpresasApp" ng-controller="GestionEmpresasController">
        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-heading">
                    <h6 class="panel-title">Gestión de Empresa</h6>                    
                </div>

                <div class="panel-body">

                    <div class="col-lg-12">

                        <form id="form1" action="your-action">
                            <div class="row">

                                <div class="dx-fieldset">

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Tipo de Indentificación:<strom style="color: red;">*</strom></label>
                                        <div id="TipoIndentificacion"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Numero de Indentificación:<strom style="color: red;">*</strom></label>
                                        <div id="NumeroIdentificacion"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Razón Social:<strom style="color: red;">*</strom></label>
                                        <div id="txtRasonSocial"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email:<strom style="color: red;">*</label>
                                        <div id="txtEmail"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                        <div class="dx-field-label" style="font-size: 14px;">Perfil:<strom style="color: red;">*</strom></div>
                                        <div id="Facturador" style="margin-top: 2.15%"></div>
                                        <br />
                                        <div id="Adquiriente"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                        <div class="dx-field-label" style="font-size: 14px;">Habilitación:<strom style="color: red;">*</strom></div>
                                        <div class="dx-field-value">
                                            <div id="Habilitacion"></div>
                                        </div>
                                    </div>

                                    <div class="col-md-6 col-xs-10">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Empresa Asociada:</label>
                                        <div id="txtempresaasociada"></div>

                                    </div>

                                    <div class="col-md-6 col-xs-2" style="margin-top: 2%">
                                        <a data-toggle="modal" data-target="#modal_Buscar_empresa" data-popup="tooltip" title="Consulta Empresa" style="color: #166dba;">
                                            <h6 id="SelecionarEmpresa">Seleccionar Empresa</h6>
                                        </a>
                                    </div>

                                    
                                    <div class="col-md-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Observacaciones:</label>
                                        <div id="txtobservaciones"></div>
                                    </div>

                                </div>

                                <div class="dx-fieldset">
                                    <div id="summary"></div>
                                </div>

                            </div>

                            <div class="col-lg-12 text-right">
                                <br />
                                <br />
                                <a id="btncancelar" class="btn btn-default" style="text-transform: initial !important; margin-right: 1%" href="/Views/Pages/ConsultaEmpresas.aspx" style="font-size: 14px; text-align: center;">Cancelar</a>
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
