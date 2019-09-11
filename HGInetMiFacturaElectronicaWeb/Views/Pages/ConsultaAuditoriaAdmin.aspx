<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaAuditoriaAdmin.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaAuditoriaAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<!-- JS DocumentosAdquiriente-->

	<script src="../../Scripts/Services/FiltroGenerico.js?vjs201921"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs201921"></script>
	<script src="../../Scripts/Services/SrvAuditoria.js?vjs201921"></script>
	<script src="../../Scripts/Pages/ConsultaAuditoriaAdmin.js?vjs201921"></script>
	<script src="../../Scripts/Pages/ModalConsultaEmpresas.js?vjs201921"></script>

	<div data-ng-app="AudAdminApp">

		<!-- CONTENEDOR PRINCIPAL -->
		<div data-ng-controller="AudAdminController">

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
										<i class="icon-file-text"></i>
										<label>Procedencia:</label>
										<div data-dx-select-box="filtros.Procedencia"></div>
									</div>

									<div class="col-md-3" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Tipo Registro:</label>
										<div data-dx-select-box="filtros.tipo_registro"></div>
									</div>

									<div class="col-md-3" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Proceso:</label>
										<div id="filtrosProceso"></div>
									</div>

									<div class="col-md-3" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Estado:</label>
										<div id="filtrosEstado"></div>
									</div>

									<div class="col-md-3" style="margin-top: 1%">
										<div data-hgi-filtro="Facturador"></div>
									</div>

									<div class="col-md-3" style="margin-top: 1%">
										<i class="icon-files-empty"></i>
										<label>Número Documento:</label>
										<div data-dx-autocomplete="filtros.NumeroDocumento"></div>
									</div>

								</div>

							</div>


						</div>
						<div class="col-lg-12 text-right">
							<br />
							<br />
							<div dx-button="ButtonOptionsConsultar" style="margin-right: 20px"></div>
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
							<div id="gridDocumentos"></div>
						</div>
					</div>

				</div>
			</div>
			<!-- /DATOS -->
			<div data-ng-include="'ModalConsultaEmpresas.aspx'"></div>

		</div>
		<!-- /CONTENEDOR PRINCIPAL -->


	</div>
</asp:Content>
