<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionProveedores.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionProveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <script src="../../Scripts/Services/MaestrosEnum.js?vjs201922"></script>
    <script src="../../Scripts/Services/SrvProveedor.js?vjs201922"></script>
    <script src="../../Scripts/Pages/Proveedores.js?vjs201922"></script>

    <div data-ng-app="ProveedoresApp" data-ng-controller="GestionProveedoresController">
        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-heading">
                    <h6 class="panel-title">Gestión de Proveedor</h6>
                </div>

                <div class="panel-body">

                    <div class="col-lg-12">

                        <form id="form1" action="your-action">
                            <div class="row">

                                <div class="dx-fieldset">

                                    <div class="col-md-12" style="z-index: 9;">


                                        <div class="col-md-6 col-xs-12" style="z-index: 9;">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Indentificación:<strom style="color: red;">*</strom></label>
                                            <div id="Identificacion"></div>
                                        </div>

                                        <div class="col-md-6 col-xs-12" style="z-index: 9;">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Razón Social:<strom style="color: red;">*</strom></label>
                                            <div id="RazonSocial"></div>
                                        </div>



                                        <div class="col-md-6 col-xs-12" style="z-index: 9;">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email:<strom style="color: red;">*</label>
                                            <div id="Email"></div>
                                        </div>


                                        <div class="col-md-3 col-xs-6" style="z-index: 9;">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Teléfono:<strom style="color: red;">*</strom></label>
                                            <div id="Telefono"></div>
                                        </div>

                                        <div class="col-md-3 col-xs-6" style="z-index: 9;">
                                            <br />
                                            <br />

                                            <div id="estado"></div>
                                        </div>


                                        <hr />
                                        <div class="col-md-12" style="margin: 10px; padding: 10px;">
                                            <div class="col-md-12 ">
                                                <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Datos usuario Receptor:</label>
                                            </div>

                                            <div class="col-md-4 col-xs-12" style="z-index: 9;">
                                                <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Usuario:<strom style="color: red;">*</strom></label>
                                                <div id="UsuarioProveedor" style="margin: 1%"></div>
                                            </div>

                                            <div class="col-md-4 col-xs-12" style="z-index: 9;">
                                                <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Clave:<strom style="color: red;">*</strom></label>
                                                <div id="PasswordProveedor" style="margin: 1%"></div>
                                            </div>

                                            <div class="col-md-4 col-xs-12" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                                <i class=" icon-calendar"></i>
                                                <label>Fecha Expiración Proveedor:</label>
                                                <div id="FechaExpiracionP"></div>
                                            </div>

                                        </div>
                                        <hr />




                                        <div class="col-md-12" style="margin: 10px; padding: 10px;">

                                            <div class="col-md-12 ">
                                                <hr />
                                                <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Datos usuario Hgi:</label>

                                            </div>

                                            <div class="col-md-4 col-xs-12" style="z-index: 9;">
                                                <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Usuario:<strom style="color: red;">*</strom></label>
                                                <div id="Usuariohgi" style="margin: 1%"></div>
                                            </div>

                                            <div class="col-md-4 col-xs-12" style="z-index: 9;">
                                                <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Clave:<strom style="color: red;">*</strom></label>
                                                <div id="Passwordhgi" style="margin: 1%"></div>
                                            </div>

                                            <div class="col-md-4 col-xs-12" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                                <i class=" icon-calendar"></i>
                                                <label>Fecha Expiración HGI:</label>
                                                <div id="FechaExpiracionHgi"></div>
                                            </div>



                                        </div>


                                        <div class="col-md-6 col-xs-12" style="z-index: 9;">
                                            <div class=""></div>
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                                Url Servicio Rest:  -  Validado<strom style="color: red;">                                            
                                                    <div id="btnValidarapi"></div>                                            
                                            </strom></label>
                                            <div id="urlapi"></div>
                                        </div>

                                        <div class="col-md-6 col-xs-12 " style="z-index: 9; margin-left: 0px">
                                            <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">
                                                Servidor Ftp:   -    Validado <strom style="color: red;">
                                                     <div id="btnValidarsftp"></div>                                            
                                         </strom></label>
                                            <div id="ftp"></div>

                                        </div>


                                        <hr />


                                        <div class="col-md-12 text-left" style="z-index: 8;">
                                            <label style="margin-top: 16px;">Observaciones:</label>
                                            <div id="observaciones"></div>
                                        </div>



                                    </div>





                                    <div class="dx-fieldset">
                                        <div id="summary"></div>
                                    </div>

                                </div>

                                <div class="col-lg-12 text-right">
                                    <br />
                                    <br />
                                    <a id="btncancelar" class="btn btn-default" style="text-transform: initial !important; margin-right: 1%" href="/Views/Pages/ConsultaProveedores.aspx" style="font-size: 14px; text-align: center;">Cancelar</a>
                                    <div id="button"></div>
                                </div>
                            </div>
                        </form>

                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
