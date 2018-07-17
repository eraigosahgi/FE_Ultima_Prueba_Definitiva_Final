<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionEmpresas.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionEmpresas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
    <script src="../../Scripts/Pages/Empresas.js"></script>
    <script src="../../Scripts/Pages/ModalConsultaEmpresas.js"></script>

    <div data-ng-app="EmpresasApp" data-ng-controller="GestionEmpresasController">
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

                                    <div class="col-md-12" style="z-index: 9;">
                                        <div class="col-md-6 col-xs-12" style="z-index: 9;">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Tipo de Indentificación:<strom style="color: red;">*</strom></label>
                                            <div id="TipoIndentificacion"></div>
                                        </div>


                                        <div class="col-md-6 col-xs-12" style="z-index: 9;">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Numero de Indentificación:<strom style="color: red;">*</strom></label>
                                            <div id="NumeroIdentificacion"></div>
                                        </div>
                                    </div>

                                    <div class="col-md-12" style="z-index: 9;">
                                        <div class="col-md-6 col-xs-12" style="z-index: 9;">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Razón Social:<strom style="color: red;">*</strom></label>
                                            <div id="txtRasonSocial"></div>
                                        </div>

                                        <div class="col-md-6 col-xs-12" style="z-index: 9;">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email:<strom style="color: red;">*</label>
                                            <div id="txtEmail"></div>
                                        </div>
                                    </div>

                                    <div class="col-md-12" style="z-index: 9;">
                                        <div class="col-md-6 col-xs-12" style="margin: 0px; margin-top: 16px; margin-bottom: 1%; z-index: 9;">
                                            <div class="dx-field-label" style="font-size: 14px;">Perfil:<strom style="color: red;">*</strom></div>
                                            <div id="Facturador" style="margin-top: 2.15%"></div>
                                            <br />
                                            <div id="Adquiriente"></div>
                                        </div>

                                        <div class="col-md-6 col-xs-12" style="z-index: 9; margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                            <div class="dx-field-label" style="font-size: 14px;" id="idHabilitacion">Habilitación:<strom style="color: red;">*</strom></div>
                                            <div class="dx-field-value">
                                                <div id="Habilitacion"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12" >
                                        <div class="col-md-6 col-xs-12" style="z-index: 9;">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Empresa Asociada:</label>
                                            <div id="txtempresaasociada"></div>

                                        </div>

                                        <div class="col-md-2 col-xs-4 " style="z-index: 9; margin-left: 0px">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nª Usuarios:</label>
                                            <div id="txtUsuarios"></div>

                                        </div>

                                        <div class="col-md-4 col-xs-8 text-left" style="z-index: 9; margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                            <div id="Integradora" style="margin-top: 30px"></div>

                                        </div>

                                        <!--<div class="col-md-6 col-xs-2" style="margin-top: 2%">
                                            <a data-toggle="modal" data-target="#modal_Buscar_empresa" data-popup="tooltip" title="Consulta Empresa" style="color: #166dba;">
                                                <h6 id="SelecionarEmpresa">Seleccionar Empresa</h6>
                                            </a>
                                        </div>-->
                                    </div>

                                    <div class="col-md-12 text-left" style="z-index: 8; margin: 10px; ">
                                        <label style=" margin-top: 16px;" >Observacaciones:</label>
                                        <div id="txtobservaciones"></div>
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
                            </div>
                        </form>

                    </div>

                </div>
            </div>
        </div>
        <div data-ng-if="Admin">
            <div data-ng-include="'ModalConsultaEmpresas.aspx'"></div>
        </div>
    </div>

</asp:Content>
