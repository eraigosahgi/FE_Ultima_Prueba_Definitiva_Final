<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="DocumentosObligado.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.DocumentosObligado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs201912"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs201912"></script>
	<script src="../../Scripts/Services/SrvDocumentos.js?vjs201912"></script>
	<script src="../../Scripts/Pages/DocumentosObligado.js?vjs201912"></script>
	<div data-ng-app="DocObligadoApp">

		<!-- CONTENEDOR PRINCIPAL -->
		<div data-ng-controller="DocObligadoController">

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
										<div data-hgi-filtro="Adquiriente"></div>
									</div>
									<div class="col-md-2" style="margin-top: 1%">
										<i class="icon-files-empty"></i>
										<label>Nº Documento:</label>
										<div data-dx-autocomplete="filtros.NumeroDocumento"></div>
									</div>


									<div class="col-md-3" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Estado Acuse:</label>
										<div data-dx-select-box="filtros.EstadoRecibo"></div>
									</div>

									<div class="col-md-4" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Estado:</label>
										<div id="filtrosEstadoRecibo"></div>
									</div>


									<div class="col-md-5" style="margin-top: 1%">
										<i class="icon-files-empty"></i>
										<label>Resolución:</label>
										<div id="filtrosResolucion"></div>
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

		</div>
		<!-- /CONTENEDOR PRINCIPAL -->



		<div id="modal_enviar_email" class="modal fade" style="display: none; margin-top: 15%;" modal="showModal" ng-controller="EnvioEmailController">
			<div class="modal-dialog modal-sm">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" id="btncerrarModal" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px" class="modal-title">Envío E-mail Adquiriente</h5>
					</div>

					<div class="modal-body text-center">
						<h6>El siguiente E-mail corresponde al destinatario de este mensaje.
						</h6>

						<div id="formEmailEnvio" dx-form="formOptionsEmailEnvio">
						</div>

					</div>
					<div id="divsombra" class="modal-footer">
						<div dx-button="buttonCerrarModal" data-dismiss="modal"></div>
						<div dx-button="buttonEnviarEmail"></div>
					</div>

				</div>
			</div>
		</div>

		<div data-ng-include="'AuditoriaDocumento.aspx'"></div>

	</div>

</asp:Content>
