<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaEmpresas.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaEmpresas1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
    <!-- JS DocumentosAdquiriente-->
    <script src="../../Scripts/Pages/Empresas.js"></script>

    <!-- CONTENEDOR PRINCIPAL -->
    <div ng-app="EmpresasApp" ng-controller="ConsultaEmpresasController">

        <%--//Panel Grid--%>
        <div class="col-md-12">
            <div class="panel panel-white">
                <div>
                    <br />
                    
                    <div class="col-md-12">
                        <div class="col-md-10">
                            <h6 class="panel-title">Datos</h6>
                        </div>
                        <div class="col-md-2">
                            <a class="btn btn-primary" style="background: #337ab7" href="GestionEmpresas.aspx">Nueva Empresa </a>
                        </div>
                    </div>

                </div>

                <br />
                <div class="panel-body">
                    <div class="demo-container">
                        <div id="gridEmpresas"></div>
                    </div>

                </div>

            </div>
        </div>
    </div>
</asp:Content>
