<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaProveedores.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaProveedores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">


    <script src="../../Scripts/Services/MaestrosEnum.js?vjs201926"></script>
    <script src="../../Scripts/Services/SrvProveedor.js?vjs201926"></script>
     <!-- JS Proveedores-->
    <script src="../../Scripts/Pages/Proveedores.js?vjs201926"></script>


    <!-- CONTENEDOR PRINCIPAL -->
    <div data-ng-app="ProveedoresApp" data-ng-controller="ProveedoresController" data-ng-init="Admin=false">

        <%--//Panel Grid--%>
        <div class="col-md-12">
            <div class="panel panel-white">
                <div>
                    <br />
                    
                    <div class="col-md-12">
                        <div class="col-md-10">
                            <h6 class="panel-title">Datos</h6>
                        </div>
                        <div class="col-md-12 text-right" data-ng-show="Admin">
                            <a class="btn btn-primary" style="background: #337ab7" href="GestionProveedores.aspx">Crear</a>
                        </div>
                    </div>

                </div>

                <br />
                <div class="panel-body" style="margin-top: 2%">
                    <div class="demo-container">
                        <div id="gridProveedores"></div>
                    </div>

                </div>

            </div>
        </div>
    </div>
</asp:Content>
