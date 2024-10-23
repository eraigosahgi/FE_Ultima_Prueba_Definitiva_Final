<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaLog.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<script src="../../Scripts/Services/Loading.js?vjs201926"></script>
	<script src="../../Scripts/Services/SrvLog.js?vjs201926"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs201926"></script>
	<script src="../../Scripts/Pages/ConsultaLog.js?vjs201926"></script>

	<div data-ng-app="App">

		<!-- CONTENEDOR PRINCIPAL -->
		<div data-ng-controller="LogController">

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

							<div class="row">

								<div class="dx-fieldset">

									<div class="col-md-2">
										<i class=" icon-calendar"></i>
										<label>Fecha:</label>
										<div id="Fecha"></div>
									</div>


									<div class="col-md-3">
										<i class="icon-file-text"></i>
										<label>Categoria:</label>
										<div data-dx-select-box="filtros.Categoria"></div>
									</div>

									<div class="col-md-2">
										<i class="icon-file-text"></i>
										<label>Tipo:</label>
										<div data-dx-select-box="filtros.Tipo"></div>
									</div>

									<div class="col-md-3">
										<i class="icon-file-text"></i>
										<label>Acción:</label>
										<div data-dx-select-box="filtros.Accion"></div>
									</div>
									<div class="col-md-2 text-right" style="margin-top: 3%">

										<div data-dx-button="ButtonOptionsConsultar" style="margin-right: 20px"></div>

									</div>


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
						<h6 class="panel-title ">Datos</h6>
						<div style="float: right; margin-right: 2%; margin-top: -20px;">
							<label id="Total" class="text-semibold text-right" style="font-size: medium;"></label>
						</div>
					</div>

					<div class="panel-body">
						<div class="demo-container">
							<div id="gridLog"></div>
						</div>

					</div>

				</div>
			</div>
			<!-- /DATOS -->


		</div>

	</div>
</asp:Content>
