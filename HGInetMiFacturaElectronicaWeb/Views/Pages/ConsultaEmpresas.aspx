<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaEmpresas.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaEmpresas1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	
	<script src="../../Scripts/Services/SrvEmpresa.js?vjs201921"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs201921"></script>
	<script src="../../Scripts/Services/SrvEmpresa.js?vjs201921"></script>
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs201921"></script>
	<script src="../../Scripts/Pages/Empresas.js?vjs201921"></script>
	<script src="../../Scripts/Pages/ModalDetalleEmpresa.js?vjs201921"></script>
	<div data-ng-app="EmpresasApp" data-ng-controller="ConsultaEmpresasController" data-ng-init="Admin=false">

		<div class="col-md-12">
			<div class="panel panel-white">
				<div>
					<br />

					<div class="col-md-12">
						<div class="col-md-10">
							<h6 class="panel-title">Datos</h6>
						</div>
						<div class="col-md-12 text-right" data-ng-show="Admin" style="margin-top: -2%">
							<a class="btn btn-primary" style="background: #337ab7" href="GestionEmpresas.aspx">Crear</a>
							<br />
						</div>
					</div>

				</div>
				<br />
				<div class="panel-body" style="margin-top: 2%">
					<div class="demo-container">
						<div id="gridEmpresas"></div>
					</div>

					<div data-ng-include="'Partials/LoadingRegistros.Html'"></div>

				</div>
			</div>
		</div>
		<div data-ng-include="'ModalDetalleEmpresa.aspx'"></div>
	</div>
</asp:Content>
