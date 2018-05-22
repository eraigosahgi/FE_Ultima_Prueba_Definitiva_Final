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
                    <h6 class="panel-title">Gestión de Empresa<a class="heading-elements-toggle"><i class="icon-more"></i></a></h6>
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a data-action="collapse"></a></li>
                        </ul>
                    </div>
                </div>

                <div class="panel-body">

                    <div class="col-lg-12">

                        <form id="form1" action="your-action">
                            <div class="row">

                                <div class="dx-fieldset">

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Tipo de Indentificación:</label>
                                        <div id="TipoIndentificacion"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Numero de Indentificación:<strom style="color: red;">*</label>
                                        <div id="NumeroIdentificacion"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Razón Social:</label>
                                        <div id="txtRasonSocial"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email:<strom style="color: red;">*</label>
                                        <div id="txtEmail"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                        <div class="dx-field-label" style="font-size: 14px;">Perfil:</div>
                                        <div id="Facturador" style="margin-top: 2.15%"></div>
                                        <br />
                                        <div id="Adquiriente"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                        <div class="dx-field-label" style="font-size: 14px;">Habilitación:</div>
                                        <div class="dx-field-value">
                                            <div id="Habilitacion"></div>
                                        </div>
                                    </div>

                                    <div class="col-md-4 col-xs-10">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Empresa Asociada:<strom style="color: red;">*</label>
                                        <div id="txtempresaasociada"></div>

                                    </div>

                                    <div class="col-md-2 col-xs-2" style="margin-top: 2%">
                                        <a data-toggle="modal" data-target="#modal_Buscar_empresa" data-popup="tooltip" title="Consulta Empresa" style="color: #166dba;">
                                            <h6 id="SelecionarEmpresa">Seleccionar Empresa</h6>
                                        </a>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nª Resolución:<strom style="color: red;">*</label>
                                        <div id="txtResolucion"></div>
                                    </div>

                                </div>

                                <div class="dx-fieldset">
                                    <div id="summary"></div>
                                </div>

                            </div>

                            <div class="col-lg-12 text-right">
                                <br />
                                <br />
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
