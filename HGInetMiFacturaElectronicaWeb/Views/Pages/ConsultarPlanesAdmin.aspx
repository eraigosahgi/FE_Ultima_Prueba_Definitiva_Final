<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultarPlanesAdmin.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultarPlanesAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Services/FiltroGenerico.js?vjs201922"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs201922"></script>
	<script src="../../Scripts/Pages/ConsultarPlanesAdmin.js?vjs201922"></script>

	<div data-ng-app="GestionPlanesApp" data-ng-controller="ConsultaPlanesController">

		<!-- FILTROS DE BÚSQUEDA -->

		<div class="col-md-12">
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

						<div class="row" style="margin-left: -2%;">

							<div class="dx-fieldset" style="margin-top: -1%;">

								<div class="col-md-3" style="margin-top: 1%">
									<i class="icon-file-text"></i>
									<label>Tipo de Plan:</label>
									<div id="Tipoplan"></div>
								</div>

								<div class="col-md-3" style="margin-top: 1%">
									<i class="icon-file-text"></i>
									<label>Filtro Fecha:</label>
									<div data-dx-select-box="filtros.TipoFiltroFecha"></div>
								</div>

								<div class="col-md-3" style="margin-top: 1%">
									<i class=" icon-calendar"></i>
									<label>Fecha Inicial:</label>
									<div id="FechaInicial"></div>
								</div>


								<div class="col-md-3" style="margin-top: 1%">
									<i class=" icon-calendar"></i>
									<label>Fecha Final:</label>
									<div id="FechaFinal"></div>
								</div>

								<div class="col-md-6" style="margin-top: 1%">
									<i class="icon-file-text"></i>
									<label>Estado del Plan:</label>
									<div id="Estadoplan"></div>
								</div>

								<div class="col-md-6" style="margin-top: 1%">
									<div data-hgi-filtro="Facturador"></div>
								</div>
							</div>

						</div>


					</div>
					<div class="col-lg-12 text-right">
						<br />
						<br />
						<div data-dx-button="ButtonOptionsConsultar" style="margin-right: 20px"></div>
					</div>

					<p data-ng-bind-html="message"></p>

				</div>

			</div>
		</div>

		<!--/FILTROS DE BÚSQUEDA -->

		<%--//Panel Grid--%>
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
		<div data-ng-include="'ModalDetallePlan.html'"></div>
	</div>
</asp:Content>
