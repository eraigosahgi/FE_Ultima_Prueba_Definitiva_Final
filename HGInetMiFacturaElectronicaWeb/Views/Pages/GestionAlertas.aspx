<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionAlertas.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Configuracion.GestionAlertas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Services/MaestrosEnum.js"></script>
	<script src="../../Scripts/Services/SrvAlertas.js"></script>
	<script src="../../../Scripts/Pages/Alertas.js"></script>
	
    <div data-ng-app="AlertasApp" data-ng-controller="AlertasController" >
    
        <div class="col-md-12">
            <div class="panel panel-white">
                <div>
                    <br />
                    
                    <div class="col-md-12">
                        <div class="col-md-10">
                            <h6 class="panel-title">Datos</h6>
                        </div>                        
                    </div>

                </div>

                <br />
                <div class="panel-body" style="margin-top: 3%">
                    <div class="demo-container">
                        <div id="gridAlertas"></div>
                    </div>

                </div>

            </div>
        </div>
    </div>
</asp:Content>
