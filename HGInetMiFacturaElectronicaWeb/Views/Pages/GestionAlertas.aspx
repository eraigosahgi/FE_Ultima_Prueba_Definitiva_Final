<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionAlertas.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Configuracion.GestionAlertas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Services/Loading.js?vjs2020401"></script>
	<script src="../../Scripts/Services/SrvPermisos.js?vjs2020401"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs2020401"></script>
	<script src="../../Scripts/Services/SrvAlertas.js?vjs2020401"></script>
	<script src="../../../Scripts/Pages/Alertas.js?vjs2020401"></script>
	
    <div data-ng-app="AlertasApp" data-ng-controller="AlertasController" >
    
        <div class="col-md-12">
            <div class="panel panel-white">
                <div class="panel-heading">
						<h6 class="panel-title ">Datos</h6>						
					</div>
                <div class="panel-body" >
                    <div class="demo-container">
                        <div id="gridAlertas"></div>
                    </div>

                </div>

            </div>
        </div>
    </div>
</asp:Content>
