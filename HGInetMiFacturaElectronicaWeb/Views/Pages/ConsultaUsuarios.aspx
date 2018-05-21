<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaUsuarios.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">


    <!-- JS DocumentosAdquiriente-->
    <script src="../../Scripts/Pages/Usuarios.js"></script>

    <!-- CONTENEDOR PRINCIPAL -->
    <div ng-app="ConsultaUsuarioApp" ng-controller="ConsultaUsuarioController">

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
                                    <i class="icon-files-empty"></i>
                                    <label>Código Usuario:</label>
                                    <div dx-autocomplete="filtros.CodigoUsuario"></div>
                                </div>

                                <div class="col-md-3">
                                    <i class="icon-files-empty"></i>
                                    <label>Código Empresa:</label>
                                    <div dx-autocomplete="filtros.CodigoEmpresa"></div>
                                </div>

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

        <!--/FILTROS DE BÚSQUEDA -->



        <!-- DATOS -->
        <div class="col-md-12">
            <div class="panel panel-white">
                 <div>
                    <br />
                    
                    <div class="col-md-12">
                        <div class="col-md-10">
                            <h6 class="panel-title">Lista de Usuarios</h6>
                        </div>
                        <div class="col-md-2">
                            <a class="btn btn-primary" href="GestionUsuarios.aspx">Nuevo Usuario</a>
                        </div>
                    </div>

                </div>
                    <br />
                <div class="panel-body">
                    <div class="demo-container">
                        <div id="gridUsuarios"></div>
                    </div>

                </div>

            </div>
        </div>
        <!-- /DATOS -->

    </div>
    <!-- /CONTENEDOR PRINCIPAL -->


</asp:Content>
