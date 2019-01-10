<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaPlanesTransacciones.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaPlanesTransacciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
    <script src="../../Scripts/Services/MaestrosEnum.js?vjs201910"></script>
    <script src="../../Scripts/Pages/PlanesTransacciones.js?vjs201910"></script>

    <div data-ng-app="GestionPlanesApp" data-ng-controller="ConsultaPlanesController">
                <%--//Panel Grid--%>
        <div class="col-md-12">
            <div class="panel panel-white">
                <div>
                    <br />
                    
                    <div class="col-md-12">
                        <div class="col-md-10">
                            <h6 class="panel-title">Datos</h6>
                        </div>
                        <div class="col-md-12 text-right">
                            <a class="btn btn-primary" style="background: #337ab7" href="GestionPlanesTransacciones.aspx">Crear</a>
                        </div>
                    </div>

                </div>

                <br />
                <div class="panel-body" style="margin-top: 2%">
                    <div class="demo-container">
                        <div id="grid"></div>
                    </div>

                </div>

            </div>
        </div>
    </div>

</asp:Content>
