<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionUsuarios.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
    <script src="../../Scripts/Pages/Usuarios.js"></script>
    <script src="../../Scripts/Pages/ModalConsultaEmpresas.js"></script>
    <div ng-app="ConsultaUsuarioApp" ng-controller="GestionUsuarioController">
        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-heading">
                    <h6 class="panel-title">Gestión de Usuario<a class="heading-elements-toggle"><i class="icon-more"></i></a></h6>
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a data-action="collapse"></a></li>
                        </ul>
                    </div>
                </div>

                <div class="panel-body">

                    <div class="col-lg-12">

                        <div class="row">
                            <form id="form1">

                                <div class="dx-fieldset">
                                    <div class="col-md-6 col-xs-12">
                                        <label>Nombres:<strom style="color: red;">*</label>
                                        <div id="txtnombres"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">

                                        <label>Apellidos:<strom style="color: red;">*</label>
                                        <div id="txtapellidos"></div>

                                    </div>
                                </div>

                                <div class="dx-fieldset">
                                    <div class="col-md-6 col-xs-12">
                                        <label>Usuario:<strom style="color: red;">*</label>
                                        <div id="txtusuario"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label>Clave:<strom style="color: red;">*</label>
                                        <div id="txtclave"></div>
                                    </div>
                                </div>

                                <div class="dx-fieldset">
                                    <div class="col-md-6 col-xs-12">
                                        <label>Teléfono:<strom style="color: red;">*</label>
                                        <div id="txttelefono"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label>Extensión:<strom style="color: red;">*</label>
                                        <div id="txtextension"></div>
                                    </div>
                                </div>

                                <div class="dx-fieldset">
                                    <div class="col-md-6 col-xs-12">
                                        <label>Celular:<strom style="color: red;">*</label>
                                        <div id="txtcelular"></div>
                                    </div>

                                    <div class="col-md-6 col-xs-12">
                                        <label>Email:<strom style="color: red;">*</label>
                                        <div id="txtemail"></div>
                                    </div>
                                </div>


                                <div class="dx-fieldset">

                                    <div class="col-md-5 col-xs-10">
                                        <label>Empresa:<strom style="color: red;">*</label>
                                        <div id="txtempresaasociada"></div>

                                    </div>
                                    <div class="col-md-1 col-xs-2">
                                        <a data-toggle="modal" data-target="#modal_Buscar_empresa" data-popup="tooltip" title="Consulta Empresa" style="color: #166dba">
                                            <h6 id="SelecionarEmpresa">Empresa</h6>
                                        </a>
                                    </div>
                                    <div class="col-md-6 col-xs-12">
                                        <label>Cargo:<strom style="color: red;">*</label>
                                        <div id="txtcargo"></div>
                                    </div>
                                </div>


                                <div class="dx-fieldset">
                                    <div class="col-md-6 col-xs-12">
                                        <label>Estado:<strom style="color: red;">*</label>
                                        <div id="cboestado"></div>
                                    </div>


                                    <div class="col-lg-12 text-right">
                                        <br />
                                        <br />
                                        <div id="button"></div>
                                    </div>

                                </div>
                        </div>
                        <div class="dx-fieldset">
                            <div id="summary"></div>
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
