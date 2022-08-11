<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaRecepcionDocumentos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaRecepcionDocumentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20220811"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs20220811"></script>
	<script src="../../Scripts/Pages/ConsultaRecepcionDocs.js?vjs20220811"></script>

	<div data-ng-app="App">

		<!-- CONTENEDOR PRINCIPAL -->
		<div data-ng-controller="RecepcionCorreoController">

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

									<div class="col-md-3" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Filtro Fecha:</label>
										<div data-dx-select-box="filtros.TipoFiltroFecha"></div>
									</div>

									<div class="col-md-2" style="margin-top: 1%">
										<i class=" icon-calendar"></i>
										<label>Fecha Inicial:</label>
										<div id="FechaInicial"></div>
									</div>


									<div class="col-md-2" style="margin-top: 1%">
										<i class=" icon-calendar"></i>
										<label>Fecha Final:</label>
										<div id="FechaFinal"></div>
									</div>


									<div class="col-md-3" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Estado:</label>
										<div id="filtrosEstadoRecibo"></div>
									</div>
									
								</div>
							</div>
						</div>
						<div class="col-lg-12 text-right">
							<br />
							<br />
							<div data-dx-button="ButtonOptionsConsultar" style="margin-right: 20px"></div>
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
							<div id="gridCorreosRecibidos"></div>
						</div>
						<div data-ng-include="'Partials/LoadingRegistros.Html'"></div>
					</div>

				</div>
			</div>
			
		</div>
		
	
	</div>
</asp:Content>
