<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaFacturadorPagos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaFacturadorPagos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/SrvDocumentos.js?vjs201912"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs201912"></script>
	<script src="../../Scripts/Pages/ConsultaPagosFacturador.js?vjs201912"></script>

	<div data-ng-app="PagosFacturadorApp">

		<!-- CONTENEDOR PRINCIPAL -->
		<div data-ng-controller="PagosFacturadorController">

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

									<div class="col-md-4">
										<i class="icon-file-text"></i>
										<label>Filtro Fecha</label>
										<div data-dx-select-box="filtros.Fecha"></div>
									</div>

									<div class="col-md-2">
										<i class=" icon-calendar"></i>
										<label>Fecha Inicial:</label>
										<div id="FechaInicial"></div>
									</div>


									<div class="col-md-2">
										<i class=" icon-calendar"></i>
										<label>Fecha Final:</label>
										<div id="FechaFinal"></div>
									</div>


									<div class="col-md-4">
										<i class="icon-file-text"></i>
										<label>Estado Pago:</label>
										<div data-dx-select-box="filtros.EstadoRecibo"></div>
									</div>
									<div class="col-md-4" style="margin-top: 1%">
										<i class="icon-files-empty"></i>
										<label>Número Documento:</label>
										<div data-dx-autocomplete="filtros.NumeroDocumento"></div>
									</div>



									<div class="col-md-4" style="margin-top: 1%">
										<i class="icon-files-empty"></i>
										<label>Código Adquiriente:</label>
										<div data-dx-autocomplete="filtros.Adquiriente"></div>
									</div>

									<div class="col-md-4" style="margin-top: 1%">
										<i class="icon-files-empty"></i>
										<label>Resolución-Prefijo:</label>
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
					<div class="panel-heading" style="height: 60px">
						<div class="col-md-2">
							<h6 class="panel-title ">Datos</h6>
						</div>

						<div id="pnl_Verificar_Pago" class="col-md-7 text-right">

							<div data-ng-show="documentosPendientes">
								<div data-dx-button="buttonProcesar">
									<i class="icon-spinner11"></i>
								</div>
							</div>
						</div>

						<div class="col-md-3">
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

		<div data-ng-include="'ModalDetallePago.aspx'"></div>


	</div>

</asp:Content>
