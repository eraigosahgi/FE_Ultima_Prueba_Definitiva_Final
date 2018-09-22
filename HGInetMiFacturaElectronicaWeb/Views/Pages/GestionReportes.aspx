<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionReportes.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionReportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Pages/ReportDesignerWeb.js"></script>

	<!-- CONTENEDOR PRINCIPAL -->
	<div data-ng-app="GestionReportesApp" data-ng-controller="GestionReportesController">

		<div class="col-md-12 text-right">
			<a class="btn btn-primary" style="background: #337ab7" href="../ReportDesigner/ReportDesignerWeb.aspx">Crear Nuevo Formato</a>
		</div>


		<div class="col-md-12">
			<div data-ng-repeat="datos in FormatosPdfEmpresa">
				<div class="col-md-2">
					<span class="no-margin text-size-large text-semibold">Código: </span>
					<span class="no-margin text-size-large">{{datos.CodigoFormato}}</span><br />
					<span class="no-margin text-size-large text-semibold">Fecha Creación: </span>
					<span class="no-margin text-size-large">{{datos.FechaRegistro}}</span>


				</div>
			</div>
		</div>

	</div>

</asp:Content>
