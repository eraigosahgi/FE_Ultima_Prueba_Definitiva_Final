<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaEmpresas.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaEmpresas1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
    <!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/SrvEmpresa.js"></script>
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs201913"></script>
    <script src="../../Scripts/Pages/Empresas.js?vjs201913"></script>

    <!-- CONTENEDOR PRINCIPAL -->
    <div data-ng-app="EmpresasApp" data-ng-controller="ConsultaEmpresasController" data-ng-init="Admin=false">

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
                            <a class="btn btn-primary" style="background: #337ab7" href="GestionEmpresas.aspx">Crear</a>
                        </div>
                    </div>

                </div>

                <br />
                <div class="panel-body" style="margin-top: 3%">
                    <div class="demo-container">
                        <div id="gridEmpresas"></div>
                    </div>

                </div>

            </div>
        </div>
    </div>
</asp:Content>
