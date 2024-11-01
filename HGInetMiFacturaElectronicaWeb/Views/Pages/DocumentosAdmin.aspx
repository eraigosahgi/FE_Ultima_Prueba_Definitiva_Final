﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="DocumentosAdmin.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.DocumentosAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20221212"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs20221212"></script>
	<script src="../../Scripts/Services/SrvDocumentos.js?vjs20221212"></script>
	<script src="../../Scripts/Pages/DocumentosAdmin.js?vjs20221212"></script>
	<%--<script src="../../Scripts/Pages/ModalConsultaEmpresas.js?vjs20221212"></script>
	<script src="../../Scripts/Pages/ModalAuditoria.js?vjs20221212"></script>
	<script src="../../Scripts/Pages/EventosRadian.js?vjs20221212"></script>--%>

	<div data-ng-app="App">

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


								<div id="filterBuilder"></div>
								<div id="apply"></div>


								<div class="dx-fieldset">

									
									<div class="col-md-4" style="margin-top: 1%">
										<i class=" icon-calendar"></i>
										<label>Fecha Inicial:</label>
										<div id="FechaInicial"></div>
									</div>


									<div class="col-md-4" style="margin-top: 1%">
										<i class=" icon-calendar"></i>
										<label>Fecha Final:</label>
										<div id="FechaFinal"></div>
									</div>


									<div class="col-md-4" style="margin-top: 1%; display: none">
										<i class="icon-file-text"></i>
										<label>Estado Acuse:</label>
										<div data-dx-select-box="filtros.EstadoRecibo"></div>
									</div>
									<div class="col-md-4" style="margin-top: 1%">
										<i class="icon-files-empty"></i>
										<label>Número Documento:</label>
										<div data-dx-autocomplete="filtros.NumeroDocumento"></div>
									</div>

									<div class="col-md-3" style="margin-top: 1%; display: none">
										<i class="icon-file-text"></i>
										<label>Estado:</label>
										<div id="filtrosEstadoRecibo"></div>
									</div>

									<div class="col-md-6" style="margin-top: 1%">
										<i class="icon-user"></i>
										<label>Facturador:</label>
										<div id="facturador"></div>
									</div>

									
									<div class="col-md-3" style="margin-top: 1%; display: none">
										<div data-hgi-filtro="Adquiriente"></div>
									</div>


									<div class="col-md-3" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Tìpo:</label>
										<div id="TipoDocumento"></div>
									</div>

									<div class="col-md-3 text-right" style="margin-top: 2%">
										
										<div data-dx-button="ButtonOptionsConsultar" style="margin-right: 0px"></div>
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
					<div id="pivotgrid-chart"></div>					
					<br />

						<div class="demo-container">
							<div id="gridDocumentos"></div>
						</div>
						<%--<div data-ng-include="'Partials/LoadingRegistros.Html'"></div>--%>
					</div>

				</div>
			</div>
			<!-- /DATOS -->
			<%--<div data-ng-include="'ModalConsultaEmpresas.aspx'"></div>--%>
		</div>
		<!-- /CONTENEDOR PRINCIPAL -->
		<div id="modal_enviar_email" class="modal fade" style="display: none; margin-top: 15%;" modal="showModal" data-ng-controller="EnvioEmailController">
			<div class="modal-dialog modal-sm">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" id="btncerrarModal" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px" class="modal-title">Envío E-mail Adquiriente</h5>
					</div>

					<div class="modal-body text-center">
						<h6>El siguiente E-mail corresponde al destinatario de este mensaje.
						</h6>

						<div id="formEmailEnvio" data-dx-form="formOptionsEmailEnvio">
						</div>

					</div>
					<div id="divsombra" class="modal-footer">
						<div data-dx-button="buttonCerrarModal" data-dismiss="modal"></div>
						<div data-dx-button="buttonEnviarEmail"></div>
					</div>

				</div>
			</div>
		</div>
		<%--<div data-ng-include="'AuditoriaDocumento.aspx'"></div>
		<div data-ng-include="'Partials/EventosRadian.html'"></div>--%>
	</div>
</asp:Content>
