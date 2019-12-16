<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaResoluciones.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaResoluciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<script src="../../Scripts/Services/SrvEmpresa.js"></script>
	<script src="../../Scripts/Services/Loading.js"></script>
	<script src="../../Scripts/Services/FiltroGenerico.js"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js"></script>
	<script src="../../Scripts/config.js?vjs201926"></script>
	<script src="../../Scripts/Services/SrvResoluciones.js?vjs201926"></script>
	<script src="../../Scripts/Pages/Resoluciones.js?vjs201926"></script>

	<div data-ng-app="App" id="ConsultaResolucionesController" data-ng-controller="ConsultaResolucionesController">

		<div data-ng-include="'Partials/ModalResoluciones.html'"></div>
		<!-- FILTROS DE BÚSQUEDA -->
		<div class="col-md-12" data-ng-show="MuestraFiltros">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Filtros de Búsqueda<a class="heading-elements-toggle"><i class="icon-more"></i></a></h6>
					<div class="heading-elements">
						<ul class="icons-list">
							<li><a data-action="collapse"></a></li>
						</ul>
					</div>
				</div>
				<div class="panel-body">

					<div class="col-lg-12">

						<div class="row">
							<div class="col-md-4 ">
								<div data-hgi-filtro="Facturador"></div>
							</div>
							<div class="col-md-6 " data-ng-show="FiltroResolucion">
								<label style="margin-top: 5px; margin-bottom: 1%">Resolución:</label>
								<div id="Listaresolucion" style="margin-left: -10px; margin-bottom: 1%"></div>
							</div>

							<div class="col-md-2 text-right" style="margin-top: 8px; margin-bottom: 1%">
								<br />
								<div data-dx-button="ButtonOptionsConsultar"></div>
							</div>

						</div>

					</div>


				</div>

			</div>
		</div>
		<!--/FILTROS DE BÚSQUEDA -->

		<!-- DATOS -->
		<div class="col-md-12">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Datos</h6>
				</div>

				<div class="panel-body">
					<div class="demo-container">
						<div id="gridResoluciones"></div>
					</div>

				</div>

			</div>
		</div>
		<!--/DATOS -->
	</div>
</asp:Content>
