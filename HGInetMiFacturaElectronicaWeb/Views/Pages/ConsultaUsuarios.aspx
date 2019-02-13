<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaUsuarios.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">


    <!-- JS DocumentosAdquiriente-->
    <script src="../../Scripts/Pages/Usuarios.js?vjs201913"></script>

    <!-- CONTENEDOR PRINCIPAL -->
    <div data-ng-app="ConsultaUsuarioApp" data-ng-controller="ConsultaUsuarioController">

        <!-- DATOS -->
        <div class="col-md-12">
            <div class="panel panel-white">
                <div>
                    <br />

                    <div class="col-md-12">
                        <div class="col-md-10">
                            <h6 class="panel-title">Datos</h6>
                        </div>
                        <div class="col-md-12 text-right">
                            <a class="btn btn-primary" style="background: #337ab7" href="GestionUsuarios.aspx">Crear</a>
                            <br />
                        </div>
                    </div>

                </div>
                <br />
                <div class="panel-body" style="margin-top: 2%">
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
