<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="MonitorDeRechazos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.MonitorDeRechazos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20221212"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs20221212"></script>
	<script src="../../Scripts/Services/SrvDocumentos.js?vjs20221212"></script>
	<script src="../../Scripts/Pages/MonitorDeRechazos.js?vjs20221212"></script>
	<script src="../../Scripts/Pages/ModalConsultaEmpresas.js?vjs20221212"></script>
	<script src="../../Scripts/Pages/ModalAuditoria.js?vjs20221212"></script>
	<script src="../../Scripts/Pages/EventosRadian.js?vjs20221212"></script>

	<div data-ng-app="App">

		<!-- CONTENEDOR PRINCIPAL -->
		<div data-ng-controller="MonitorDeRechazosController">

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
										<i class=" icon-calendar"></i>
										<label>Fecha Inicial:</label>
										<div id="FechaInicial"></div>
									</div>


									<div class="col-md-3" style="margin-top: 1%">
										<i class=" icon-calendar"></i>
										<label>Fecha Final:</label>
										<div id="FechaFinal"></div>
									</div>

									<div class="col-md-3" style="margin-top: 1%">
										<i class=" icon-calendar"></i>
										<label>Tipo de Rechazo:</label>
										<div id="TipoRechazo"></div>
									</div>


									<div class="col-md-3 text-right">
										<br />
										<br />
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
						
					</div>

					<div class="panel-body">
						<div class="demo-container">
							<div id="gridDocumentos"></div>
						</div>
						<div data-ng-include="'Partials/LoadingRegistros.Html'"></div>
					</div>

				</div>
			</div>
			<!-- /DATOS -->
			<div data-ng-include="'ModalConsultaEmpresas.aspx'"></div>

		</div>
		
		<div data-ng-include="'AuditoriaDocumento.aspx'"></div>
		
	</div>
</asp:Content>
