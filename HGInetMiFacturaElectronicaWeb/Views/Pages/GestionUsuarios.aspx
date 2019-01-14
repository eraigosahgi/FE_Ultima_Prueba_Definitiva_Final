<%@ Page Title=""  EnableEventValidation="false" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionUsuarios.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <script src="../../Scripts/Pages/Usuarios.js?vjs201911"></script>
    <script src="../../Scripts/Pages/ModalConsultaEmpresas.js?vjs201911"></script>

    <div data-ng-app="ConsultaUsuarioApp" data-ng-controller="GestionUsuarioController">
        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-heading">
                    <h6 class="panel-title">Gestión de Usuario</h6>
                </div>

                <div class="panel-body">

                    <div class="col-lg-12">

                        <form id="form1">
                            <div class="row">

                                <div class="dx-fieldset">

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nombres:<strom style="color: red;">*</strom></label>
                                        <div id="txtnombres"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label id="lblapellido" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Apellidos:<strom style="color: red;">*</strom></label>
                                        <div id="txtapellidos"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Usuario:<strom style="color: red;">*</strom></label>
                                        <div id="txtusuario"></div>
                                    </div>


                                    <div class="col-md-6 col-xs-12">
                                        <label id="lbltelefono" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Teléfono:<strom style="color: red;">*</strom></label>
                                        <div id="txttelefono"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Extensión:</label>
                                        <div id="txtextension"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Celular:</label>
                                        <div id="txtcelular"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email:<strom style="color: red;">*</strom></label>
                                        <div id="txtemail"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Cargo:</label>
                                        <div id="txtcargo"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Estado:<strom style="color: red;">*</strom></label>
                                        <div id="cboestado"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Empresa Asociada:</label>
                                        <div id="txtempresaasociada"></div>

                                    </div>

                                    <!--<div class="col-md-2 col-xs-2" style="margin-top: 2%">
                                        <a data-toggle="modal" data-target="#modal_Buscar_empresa" data-popup="tooltip" title="Consulta Empresa" style="color: #166dba;">
                                            <h6 id="SelecionarEmpresa">Seleccionar Empresa</h6>
                                        </a>
                                    </div>-->

                                </div>

                                <div class="dx-fieldset">
                                    <div id="summary"></div>
                                </div>

                            </div>


                            <div class="col-md-12" style="margin-top: 3%">
                                <label>Permisos de Usuario:</label>
                                <div id="treeListOpcionesPermisos" dx-tree-list="treeListOpcionesPermisos">
                                </div>
                            </div>


                            <div class="col-md-12 text-right">
                                <br />
                                <br />
                                <a id="btncancelar" class="btn btn-default" style="text-transform: initial !important; margin-right: 1%" href="/Views/Pages/ConsultaUsuarios.aspx" style="font-size: 14px; text-align: center;">Cancelar</a>
                                <div id="button"></div>
                            </div>


                        </form>
                    </div>

                </div>


            </div>
        </div>
        <!--<div ng-if="Admin">-->
        <div >
            <div data-ng-include="'ModalConsultaEmpresas.aspx'"></div>
        </div>

    </div>

</asp:Content>
