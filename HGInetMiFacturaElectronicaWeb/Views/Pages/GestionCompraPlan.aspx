<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionCompraPlan.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionCompraPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<%--<script src="../../Scripts/Pages/ReportDesignerWeb.js?vjs20201019"></script>--%>
	<script src="../../Scripts/Pages/CompraPlanes.js?vjs20201019"></script>
	<script src="../../Scripts/Pages/ModalCompraPlan.js?vjs20220401"></script>
	<style>
		.BtnStyleNone {
			background-color: transparent;
			border-color: transparent;
		}
	</style>

	<!-- CONTENEDOR PRINCIPAL -->
	<div data-ng-app="GestionCompraPlanesApp" data-ng-controller="GestionCompraPlanesController">

		<div class="col-md-12">

			<div data-ng-repeat="datos in ListadoPlanes | filter:filtrar">
				<div class=" col-sm-6 col-md-2" id="{{datos.Idplan}}">
					<div class="panel" data-ng-class="ClasePanel(datos.Estado)" id="{{datos.ClavePrimaria}}">
						<div class="panel-heading text-center" style="padding: 5px 10px !important; height: 90px;">
							<label style="font-size: medium;">{{datos.Titulo}}</label><br />
							<label title="(Desde {{datos.Desde}})" style="font-size: smaller; white-space: nowrap; text-overflow: ellipsis; overflow: hidden;">
								(Desde {{datos.Desde}}</label>
							<label title="(Hasta {{datos.Hasta}})" style="font-size: smaller; white-space: nowrap; text-overflow: ellipsis; overflow: hidden;">
								Hasta {{datos.Hasta}})</label>
						</div>

						<div style="margin: 5%; height: 150px;">

							<div style="margin-bottom: 1%;">
								<img src="../../Scripts/Images/FormatoXML.png" class="img-responsive" style="width: 40%; display: block; margin-left: auto; margin-right: auto;" />
							</div>
							<div style="margin-top: 13%">
								<label class="no-margin text-size-large text-semibold">Valor Und: </label>
								<label class="no-margin text-size-large">{{datos.Valorund}}</label><br />
							</div>
						</div>

						<div class="text-right" style="margin-top: 1%">

							<ul class="nav nav-tabs nav-tabs-bottom" style="text-align: center;">
							<li><a data-ng-click="AbrirModal(datos.Idplan, datos.Desde, datos.Hasta, datos.Valorund)" style="font-weight: bold;"><i class="icon-cart"></i>Activar</a></li>
							</ul>
						</div>

					</div>
				</div>
			</div>

		</div>

		<div data-ng-include="'ModalComprarPlan.aspx'"></div>

		

	</div>

</asp:Content>
