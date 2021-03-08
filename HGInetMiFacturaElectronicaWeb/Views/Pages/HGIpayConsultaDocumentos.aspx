<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HGIpayConsultaDocumentos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.HGIpayConsultaDocumentos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Consulta de Documentos</title>

	<script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
	<script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.17.1/moment.min.js"></script>



	<link href="../../Scripts/assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css" />
	<link href="../../Scripts/assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
	<link href="../../Scripts/assets/css/core.css" rel="stylesheet" type="text/css" />
	<link href="../../Scripts/assets/css/components.css" rel="stylesheet" type="text/css" />
	<link href="../../Scripts/assets/css/colors.css" rel="stylesheet" type="text/css" />
	<!-- /global stylesheets -->

	<!-- Core JS files -->
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/loaders/pace.min.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/core/libraries/jquery.min.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/core/libraries/bootstrap.min.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/loaders/blockui.min.js"></script>
	<!-- /core JS files -->

	<!-- Theme JS files -->
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/visualization/d3/d3.min.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/visualization/d3/d3_tooltip.js"></script>

	<script type="text/javascript" src="../../Scripts/assets/js/plugins/forms/styling/switchery.min.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/forms/styling/uniform.min.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/forms/selects/bootstrap_multiselect.js"></script>

	<script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/moment/moment.min.js"></script>

	<script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/headroom/headroom.min.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/headroom/headroom_jquery.min.js"></script>


	<script type="text/javascript" src="../../Scripts/assets/js/plugins/notifications/bootbox.min.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/notifications/pnotify.min.js"></script>
	<%--<script type="text/javascript" src="../../Scripts/assets/js/plugins/notifications/sweet_alert.min.js"></script>--%>

	<script type="text/javascript" src="../../Scripts/assets/js/plugins/media/fancybox.min.js"></script>

	<script type="text/javascript" src="../../Scripts/assets/js/core/app.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/pages/user_pages_team.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/pages/components_popups.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/pages/components_modals.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/pages/layout_navbar_hideable.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/extensions/session_timeout.min.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/pages/gallery.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/ripple.min.js"></script>

	<!-- /theme JS files -->


	<!-- Estilos CSS -->
	<%-- <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.spa.css" />--%>
	<link href="../../Scripts/Scripts-descarga/dx.spa.css" rel="stylesheet" />
	<%--<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.common.css" />
	<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />--%>
	<link href="../../Scripts/Scripts-descarga/dx.common.css" rel="stylesheet" />
	<link href="../../Scripts/Scripts-descarga/dx.light.css" rel="stylesheet" />


	<!-- Scripts Requeridos
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>-->

	<%--<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.16/angular.min.js"></script>--%>
	<script src="../../Scripts/Scripts-descarga/angular.min.js"></script>

	<!--<script src="https://cdn3.devexpress.com/jslib/17.2.7/js/dx.all.js"></script>    -->


	<%--<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>--%>
	<script src="../../Scripts/Scripts-descarga/jszip.min.js"></script>


	<%--<script src="../../Scripts/devall.js"></script>--%>
	<%--<script src="https://cdn3.devexpress.com/jslib/19.1.6/js/dx.all.js"></script>--%>
	<script src="../../Scripts/Scripts-descarga/dx.all.js"></script>

	<%--<script src="https://unpkg.com/devextreme-intl/dist/devextreme-intl.min.js"></script>--%>
	<script src="../../Scripts/Scripts-descarga/devextreme-intl.min.js"></script>


	<%--<script src="https://cdn3.devexpress.com/jslib/19.1.6/js/localization/dx.messages.es.js"></script>--%>
	<script src="../../Scripts/Scripts-descarga/dx.messages.es.js"></script>

	<%--<script src="https://unpkg.com/devextreme-aspnet-data@1.3.0"></script>--%>

	<!-- JS Localización -->
	<%--<script src="../../Scripts/devextreme-localization/dx.messages.es.js"></script>--%>
	
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20201019"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs20201019"></script>		
	<script src="../../Scripts/Services/SrvDocumentos.js"></script>
	<script src="../../Scripts/WebConfig.js?vjs20201019"></script>
	<script src="../../Scripts/config.js?vjs20201019"></script>
	<script src="../../Scripts/Services/Loading.js?vjs20201019"></script>
	<script src="../../Scripts/Pages/HGIpayConsultaDocumentos.js?vjs20201019"></script>
</head>
<body data-ng-app="App">
	<form id="form1" runat="server">
		<div style="padding-top: 20px; padding-left: 40px; margin-bottom: -20px;" class="row">
			<div class="col-md-3">
				<img src="/../Scripts/Images/LogoHGI.png" id="img_cliente" style="align-content: center; max-width: 300px" class="img-responsive" />
				<br />
			</div>
			<div class="col-md-4" style="padding-top: 40px;">
				<asp:Label runat="server" ID="lblNombre_Facturador"></asp:Label>
				<br />
				<asp:Label runat="server" ID="lblNit_Facturador"></asp:Label>
			</div>
			<div class="col-md-5" style="padding-top: 20px; text-align: right; margin-left: -10px;">
				<asp:Label runat="server" ID="lblUsuario"></asp:Label>

				<div id="BtnCerrar" style="margin: 50px; background-color: #a6a0a0; color: white;"></div>

			</div>
			<input type="hidden" runat="server" id="Hdf_RutaSrvPagos" />
			<input type="hidden" runat="server" id="Hdf_RutaPagos" />

		</div>
		<hr />
		<script src="../../Scripts/Services/MaestrosEnum.js?vjs20201019"></script>
		<script src="../../Scripts/Pages/HGIpayConsultaDocumentos.js?vjs20201019"></script>
		<script src="../../Scripts/Pages/ModalPagos.js?vjs20201019"></script>
		<!-- CONTENEDOR PRINCIPAL -->
		<div>
			<div data-ng-app="App">

				<div class="menu dropdown-content wmin-xl- show">

					<ul class="nav nav-tabs ">
						<li class="nav-item"><a href="!#tabs_content_documentos" id="tabs_documentos" class="nav-link border-left-0 active" data-toggle="tab">Documentos</a></li>
						<li class="nav-item"><a href="!#tabs_content_pagos" id="tabs_pagos" class="nav-link border-right-0" data-toggle="tab">Pagos</a></li>
					</ul>

					<div class="tab-content">
						<!--General-->
						<div class="tab-pane p-0 active" id="tabs_content_documentos">


							<div class="row" data-ng-controller="HGIpayConsultaDocumentosController">

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
														<div class="col-md-3">
															<i class="icon-file-text"></i>
															<label>Filtro Fecha:</label>
															<div data-dx-select-box="filtros.TipoFiltroFecha"></div>
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

														<%--<div class="col-md-4">
                                        <i class="icon-file-text"></i>
                                        <label>Estado Recibo:</label>
                                        <div data-dx-select-box="filtros.EstadoRecibo"></div>
                                    </div>--%>

														<div class="col-md-3">
															<i class="icon-files-empty"></i>
															<label>Número Documento:</label>
															<div data-dx-autocomplete="filtros.NumeroDocumento"></div>
														</div>

														<div class="col-md-2 text-right">
															<br />
															<div data-dx-button="ButtonOptionsConsultar"></div>

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
											<h6 class="panel-title">Datos</h6>
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
								<!--/DATOS -->

							</div>

						</div>

						<div class="tab-pane" id="tabs_content_pagos">

							<!-- CONTENEDOR PRINCIPAL -->
							<div data-ng-controller="HGIpayPagosAdquirienteController">

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

														<div class="col-md-4">
															<i class=" icon-calendar"></i>
															<label>Fecha Inicial:</label>
															<div id="FechaInicialPagos"></div>
														</div>


														<div class="col-md-4">
															<i class=" icon-calendar"></i>
															<label>Fecha Final:</label>
															<div id="FechaFinalPagos"></div>
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
														

														<div class="col-md-4 text-right" style="margin-top: 1%">
															<br />
															<div data-dx-button="ButtonOptionsConsultarPagos"></div>
														</div>


													</div>

												</div>


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
												<label id="TotalPagos" class="text-semibold text-right" style="font-size: medium;"></label>
											</div>
										</div>

										<div class="panel-body">
											<div class="demo-container">
												<div id="gridPagos"></div>
											</div>
										</div>

									</div>
								</div>
								<!-- /DATOS -->

							</div>
							<!-- /CONTENEDOR PRINCIPAL -->

							<div id="modal_detalles_pago" class="modal fade" style="display: none;">
								<div class="modal-dialog modal-lg">
									<div class="modal-content">
										<div id="EncabezadoModal" class="modal-header">
											<button type="button" class="close" data-dismiss="modal">×</button>
											<h5 style="margin-bottom: 10px;" class="modal-title">Detalles Pago Electrónico</h5>
										</div>
										<div class="modal-body">

											<div id="ContenidoDetallesPago">
											</div>

										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>


			</div>
			<!--Aplicacion de Pagos-->
			<div data-ng-include="'ModalPagos.aspx'"></div>
			<div data-ng-include="'Partials/ModalFormasDePago.html'"></div>
			<div data-ng-include="'Partials/ModalPagosEmbebida.html'"></div>
			<!--Aplicacion de Pagos-->
		</div>
	</form>
</body>
</html>
