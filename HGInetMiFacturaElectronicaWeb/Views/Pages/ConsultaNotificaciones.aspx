<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaNotificaciones.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaNotificaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<script src="../../Scripts/Services/SrvAlertas.js?vjs20201005"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs20201005"></script>
	<script src="../../Scripts/Pages/ConsultaNotificaciones.js?vjs20201005"></script>
	<script src="../../Scripts/Pages/ModalDetalleEmpresa.js?vjs20201005"></script>


	<div data-ng-app="ConsultaNotificacionApp" data-ng-controller="ConsultaNotificacionController">
			
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
				<div class="panel-body" style="margin-top: 2%">
					<div class="demo-container">
						<div id="grid"></div>
					</div>

				</div>

			</div>
		</div>
		<div data-ng-include="'ModalDetalleEmpresa.aspx'"></div>
	</div>
</asp:Content>
