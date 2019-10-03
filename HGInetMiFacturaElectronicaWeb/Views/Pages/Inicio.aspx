<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Pages/Indicadores.js?vjs201924"></script>

	<style>
		.nav-tabs:before {
			content: 'Indicadores' !important;
		}

		.nav-tabs > li.active > a,
		.nav-tabs > li.active > a:hover,
		.nav-tabs > li.active > a:focus {
			border-left-color: #337ab7;
		}
	</style>



	<!-- CONTENEDOR PRINCIPAL -->
	<div class="col-md-12" data-ng-app="IndicadoresApp" data-ng-controller="IndicadoresController">

		<!-- FILTROS DE BÚSQUEDA -->
		<div class="panel panel-white" id="panel_filtros">
			<div class="panel-body">

				<div class="col-md-12" style="margin-bottom: 1%">
					<label>Tipo Filtro:</label>
					<div id="OpcionesFiltroFechas"></div>
				</div>

				<div class="col-md-3" id="VerFiltroInicio">
					<label>Fecha Inicial:</label>
					<div id="FiltroFechaInicio"></div>
				</div>

				<div class="col-md-3" id="VerFiltroFin">
					<label>Fecha Final:</label>
					<div id="FiltroFechaFin"></div>
				</div>

				<div class="col-md-3" style="margin-top: 2%">
					<div id="BtnFiltroIndicadores"></div>
				</div>
			</div>

		</div>
		<!-- /FILTROS DE BÚSQUEDA -->

		<!-- CONTENIDO PANEL-->
		<div class="panel-body">
			<!-- MENÚ TABS -->
			<ul class="nav nav-tabs">
				<li id="LiTabAdministrador" data-ng-show="IndicadoresAdmin"><a id="LinkTabAdministrador" data-ng-if="LinkTabAdministrador" data-ng-init="LinkTabAdministrador=true" href="#TabAdministrador" data-toggle="tab">Administrador</a></li>
				<li id="LiTabFacturador" data-ng-class="{'active':!IndicadoresAdmin}" data-ng-show="IndicadoresFacturador"><a id="LinkTabFacturador" data-ng-if="LinkTabFacturador" data-ng-init="LinkTabFacturador=true" href="#TabFacturador" data-toggle="tab">Facturador</a></li>
				<%--<li id="LiTabAdquiriente" data-ng-class="{'active':!IndicadoresAdmin && !IndicadoresFacturador }" data-ng-show="IndicadoresAdquiriente"><a id="LinkTabAdquiriente" data-ng-if="LinkTabAdquiriente" data-ng-init="LinkTabAdquiriente=true" href="#TabAdquiriente" data-toggle="tab">Adquiriente</a></li>--%>
				<li id="LiTabAdquiriente" data-ng-class="{'active':!IndicadoresAdmin && !IndicadoresFacturador }" data-ng-show="IndicadoresAdquiriente"><a id="LinkTabAdquiriente" href="#TabAdquiriente" data-toggle="tab" data-ng-click="validarActivo(3);">Adquiriente</a></li>
			</ul>
			<!--  /MENÚ TABS -->

			<!-- CONTENIDOS TABS -->
			<div class="tab-content">

				<!-- TAB ADMINISTRADOR -->
				<div class="tab-pane active" id="TabAdministrador">
					<div class="row">

						<!-- REPORTE ESTADOS DOCUMENTO CATEGORIA -->
						<div class="col-md-12 col-lg-12" id="Panel13519" data-ng-if="Panel13519" data-ng-init="Panel13519=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px" id="totaldocestado">Documentos por Estado</h4>
									<div data-ng-repeat="c in ReporteDocumentosEstadoCategoriaAdmin">
										<div class="content-group-sm svg-center position-relative col-xs-12 col-md-6 col-lg-3 text-center" data-ng-show="c.Estado!=-1" id="{{c.IdControl}}">
										</div>

										<div data-ng-if="$last" data-ng-init="CargarDocumentosEstadoCategoriaAdmin()"></div>

									</div>
								</div>
							</div>
						</div>

						<!-- REPORTE ESTADOS DOCUMENTO -->
						<div class="col-md-12 col-lg-12" id="Panel13511" data-ng-if="Panel13511" data-ng-init="Panel13511=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 40px" id="totaldocproceso">Documentos por Proceso</h4>
									<div data-ng-repeat="c in ReporteDocumentosEstadoAdmin">
										<div class="content-group-sm svg-center position-relative col-md-3 text-center" data-ng-show="c.Estado!=0" id="{{c.IdControl}}"></div>
										<div data-ng-if="$last" data-ng-init="CargarDocumentosEstadoAdmin()"></div>
									</div>
								</div>
							</div>
						</div>
						<!-- /REPORTE ESTADOS DOCUMENTO -->

						<!-- REPORTE ESTADO ACUSE MENSUAL-->
						<div class="col-md-6 col-lg-6" id="Panel13512" data-ng-if="Panel13512" data-ng-init="Panel13512=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px" id="TotalAcuseAdmin">Acuse de Respuesta</h4>
									<div data-ng-repeat="c in ReporteAcuseMensualAdmin">
										<div class="content-group-sm svg-center position-relative col-md-3 text-center" id="{{c.IdControl}}"></div>
										<div data-ng-if="$last" data-ng-init="CargarAcuseMensualAdmin()"></div>
									</div>
								</div>
							</div>
						</div>
						<!-- /REPORTE ESTADO ACUSE MENSUAL -->

						<!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
						<div class="col-md-6 col-lg-6" id="Panel13515" data-ng-if="Panel13515" data-ng-init="Panel13515=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px" id="TotalTiposDocAdmin">Tipos de Documento</h4>
									<div data-ng-repeat="c in ReporteDocumentosTipoAdmin">
										<div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
										<div data-ng-if="$last" data-ng-init="CargarDocumentosTipoAdmin()"></div>
									</div>
								</div>
							</div>
						</div>
						<!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->

						<!-- REPORTE TIPO DOCUMENTO ANUAL -->
						<div class="col-md-12 col-lg-12" id="Panel13514" data-ng-if="Panel13514" data-ng-init="Panel13514=false">
							<div class="panel panel-default">

								<div class="panel-body">

									<div class="heading-elements panel-nav">
										<ul class="nav nav-tabs nav-tabs-bottom">
											<li class="dropdown">
												<a class="dropdown-toggle legitRipple btn-default" data-toggle="dropdown" aria-expanded="false">
													<i class="icon-cog5"></i>
													<span class="position-right">visualización</span>
													<span class="caret"></span>
												</a>
												<ul class="dropdown-menu dropdown-menu-right" style="background-color: #fcfcfc">
													<li><a data-ng-click="CambiarGrafico13514('bar')"><i class="icon-stats-bars2"></i>Barras</a></li>
													<li><a data-ng-click="CambiarGrafico13514('line')"><i class="icon-stats-dots"></i>Líneas</a></li>
												</ul>
											</li>
										</ul>
									</div>

									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px" id="TotalTiposDocGraficoAdmin">Tipos de Documento</h4>
									<label class="text-muted" style="margin-bottom: 20px">Indica la cantidad de documentos generados en el rango de tiempo seleccionado.</label>

									<div id="ReporteTipoDocumentoAnualAdmin"></div>

								</div>
							</div>
						</div>
						<!-- /REPORTE TIPO DOCUMENTO ANUAL -->

						<!-- REPORTE VENTAS -->
						<div class="col-md-6 col-lg-6" id="Panel13516" data-ng-if="Panel13516" data-ng-init="Panel13516=false">
							<div class="panel panel-default">

								<div class="panel-body">

									<div class="heading-elements panel-nav">
										<ul class="nav nav-tabs nav-tabs-bottom">
											<li class="dropdown">
												<a class="dropdown-toggle legitRipple btn-default" data-toggle="dropdown" aria-expanded="false">
													<i class="icon-cog5"></i>
													<span class="position-right">visualización</span>
													<span class="caret"></span>
												</a>
												<ul class="dropdown-menu dropdown-menu-right" style="background-color: #fcfcfc">
													<li><a data-ng-click="CambiarGrafico13516('bar')"><i class="icon-stats-bars2"></i>Barras</a></li>
													<li><a data-ng-click="CambiarGrafico13516('Line')"><i class="icon-stats-dots"></i>Líneas</a></li>
												</ul>
											</li>
										</ul>
									</div>

									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Ventas</h4>
									<label class="text-muted" style="margin-bottom: 20px">Indica el nivel de ventas en el rango de tiempo seleccionado.</label>
									<div id="ReporteVentas"></div>
								</div>
							</div>
						</div>
						<!-- /REPORTE VENTAS -->

						<!-- REPORTE TOP COMPRADORES -->
						<div class="col-md-6" id="Panel13517" data-ng-if="Panel13517" data-ng-init="Panel13517=false">
							<div class="panel panel-default">

								<div class="panel-body">

									<div class="col-md-12">
										<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px">Top Compradores</h4>
										<label class="text-muted" style="margin-bottom: 20px">Indica las empresas con mayor nivel de compras.</label>
									</div>

									<div class="col-md-8" style="margin-bottom: 1%">
										Cantidad Top: <i class="icon-info22" id="InfoPanelInfoPanel1351713518"></i>
										<div id="ToolTipPanel13517"></div>
										<div id="CantTopCompradoresAdmin"></div>
									</div>

									<div class="col-md-12">
										<div id="ReporteTopCompradores"></div>
									</div>

								</div>
							</div>
						</div>
						<!-- /REPORTE TOP COMPRADORES -->

						<!-- REPORTE FLUJO TRANSACCIONAL -->
						<div class="col-md-12" id="Panel13518" data-ng-if="Panel13518" data-ng-init="Panel13518=false">
							<div class="panel panel-default">

								<div class="panel-body">

									<div class="col-md-12">
										<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px" id="TotalFlujoTransAdmin">Flujo Transaccional</h4>
										<label class="text-muted" style="margin-bottom: 20px">Informa el flujo de transacciones de las empresas con mayor movimiento.</label>
									</div>

									<div class="col-md-4" style="margin-bottom: 1%">
										Cantidad Top: <i class="icon-info22" id="InfoPanel13518"></i>
										<div id="ToolTipPanel13518"></div>
										<div id="CantTopTransaccionalAdmin"></div>
									</div>

									<div class="col-md-12">
										<div id="ReporteTopMovimiento"></div>
									</div>

								</div>
							</div>
						</div>
						<!-- /REPORTE FLUJO TRANSACCIONAL -->

					</div>

				</div>
				<!-- /TAB ADMINISTRADOR -->

				<!-- TAB FACTURADOR -->
				<div class="tab-pane" id="TabFacturador">
					<div class="row">

						<!-- REPORTE ESTADOS DOCUMENTO CATEGORIA -->
						<div class="col-md-12 col-lg-12" id="Panel13527" data-ng-if="Panel13527" data-ng-init="Panel13527=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px" id="totaldocestadoFacturador">Documentos por Estado</h4>
									<div data-ng-repeat="c in ReporteDocumentosEstadoCategoriaFacturador">
										<div class="content-group-sm svg-center position-relative col-xs-12 col-md-6 col-lg-3 text-center" data-ng-show="c.Estado!=-1" id="{{c.IdControl}}"></div>
										<div data-ng-if="$last" data-ng-init="CargarDocumentosEstadoCategoriaFacturador()"></div>
									</div>
								</div>
							</div>
						</div>

						<%-- <!-- REPORTE ESTADOS DOCUMENTO -->
                        <div class="col-md-12" id="Panel13521" data-ng-if="Panel13521" data-ng-init="Panel13521=false">
                            <div class="panel">
                                <div class="panel-body">
                                    <h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Documentos por Proceso</h4>
                                    <div data-ng-repeat="c in ReporteDocumentosEstadoFacturador">
                                        <div class="content-group-sm svg-center position-relative col-md-2 text-center" id="{{c.IdControl}}"></div>
                                        <div data-ng-if="$last" data-ng-init="CargarDocumentosEstadoFacturador()"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /REPORTE ESTADOS DOCUMENTO -->--%>

						<!-- REPORTE SALDOS TRANSACCIONALES -->
						<div class="col-md-12 col-lg-12" id="Panel13522" data-ng-if="Panel13522" data-ng-init="Panel13522=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px">Resumen Documentos</h4>
									<div class="table-responsive">

										<div class="col-md-3">

											<div class="content-group">
												<h4 class="text-semibold no-margin"><i class="icon-copy4 position-left text-slate"></i>
													<label>{{TransaccionesAdquiridas}}</label></h4>
												<span class="no-margin text-size-small text-muted">Transacciones Adquiridas</span>
											</div>

											<div class="content-group">
												<h4 class="text-semibold no-margin"><i class="icon-cart-remove position-left text-danger"></i>
													<label>{{TransaccionesProcesadas}}</label></h4>
												<span class="no-margin text-size-small text-muted">Transacciones Procesadas</span>
											</div>

											<div class="content-group">
												<h4 class="text-semibold no-margin"><i class="icon-file-check2 position-left text-success"></i>
													<label>{{TransaccionesDisponibles}}</label></h4>
												<span class="no-margin text-size-small text-muted">Transacciones Disponibles</span>
											</div>

										</div>

										<div class="col-md-9">
											<div class="content-group">
												<h4 class="text-semibold no-margin">Planes Adquiridos</h4>
												<div id="ResumenPlanesAdquiridosFacturador"></div>
											</div>
										</div>

										<div class="col-md-1"></div>

									</div>
								</div>
							</div>
						</div>
						<!-- /SALDOS TRANSACCIONALES -->

						<!-- REPORTE ESTADO ACUSE MENSUAL -->
						<div class="col-md-6 col-lg-6" id="Panel13523" data-ng-if="Panel13523" data-ng-init="Panel13523=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px" id="TotalAcuseFacturador">Acuse de Respuesta</h4>
									<div data-ng-repeat="c in ReporteAcuseMensualFacturador">
										<div class="content-group-sm svg-center position-relative col-md-3 text-center" id="{{c.IdControl}}"></div>
										<div data-ng-if="$last" data-ng-init="CargarAcuseMensualFacturador()"></div>
									</div>
								</div>
							</div>
						</div>
						<!-- /REPORTE ESTADO ACUSE MENSUAL -->

						<!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
						<div class="col-md-6 col-lg-6" id="Panel13526" data-ng-if="Panel13526" data-ng-init="Panel13526=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px" id="TotalTiposDocFacturador">Tipos de Documento</h4>
									<div data-ng-repeat="c in ReporteDocumentosTipoFacturador">
										<div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
										<div data-ng-if="$last" data-ng-init="CargarDocumentosTipoFacturador()"></div>
									</div>
								</div>
							</div>
						</div>
						<!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->

						<!-- REPORTE TIPO DOCUMENTO ANUAL -->
						<div class="col-md-12 col-lg-12 " id="Panel13525" data-ng-if="Panel13525" data-ng-init="Panel13525=false">
							<div class="panel panel-default">

								<div class="panel-body">
									<div class="heading-elements panel-nav">
										<ul class="nav nav-tabs nav-tabs-bottom">
											<li class="dropdown">
												<a class="dropdown-toggle legitRipple btn-default" data-toggle="dropdown" aria-expanded="false">
													<i class="icon-cog5"></i>
													<span class="position-right">visualización</span>
													<span class="caret"></span>
												</a>
												<ul class="dropdown-menu dropdown-menu-right" style="background-color: #fcfcfc">
													<li><a data-ng-click="CambiarGrafico13525('bar')"><i class="icon-stats-bars2"></i>Barras</a></li>
													<li><a data-ng-click="CambiarGrafico13525('line')"><i class="icon-stats-dots"></i>Líneas</a></li>
												</ul>
											</li>
										</ul>
									</div>
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px" id="TotalTiposDocGraficoFacturador">Tipos de Documento</h4>
									<label class="text-muted" style="margin-bottom: 20px">Indica la cantidad de documentos generados en el rango de tiempo seleccionado.</label>
									<div id="ReporteTipoDocumentoAnualFacturador"></div>
								</div>
							</div>
						</div>
						<!-- /REPORTE TIPO DOCUMENTO ANUAL -->

						<!-- REPORTE FLUJO TRANSACCIONAL -->
						<div class="col-md-12" id="Panel13524" data-ng-if="Panel13524" data-ng-init="Panel13524=false">
							<div class="panel panel-default">

								<div class="panel-body">
									<div class="heading-elements panel-nav">
										<ul class="nav nav-tabs nav-tabs-bottom">
											<li class="dropdown active">
												<div class="dx-field">
													<div class="dx-field-label">
														Cantidad Top: <i class="icon-info22" id="InfoPanel13524" style="margin-left: 2%"></i>
														<div id="ToolTipPanel13524"></div>
													</div>
													<div class="dx-field-value">
														<div id="CantTopTransaccionalFacturador"></div>
													</div>
												</div>
											</li>
										</ul>
									</div>
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px" id="TotalFlujoTransFacturador">Flujo Transaccional</h4>
									<label class="text-muted" style="margin-bottom: 20px">Informa el flujo de transacciones de las empresas con mayor movimiento.</label>

									<div id="ReporteTopMovimientoFacturador"></div>
								</div>
							</div>
						</div>
						<!-- /REPORTE FLUJO TRANSACCIONAL -->

					</div>

				</div>
				<!-- /TAB FACTURADOR -->

				<!-- TAB ADQUIRIENTE -->
				<div class="tab-pane" id="TabAdquiriente">

					<div class="row">

						<!-- REPORTE ESTADO ACUSE ACUMULADO-->
						<div class="col-md-6 col-lg-6" id="Panel13531" data-ng-if="Panel13531" data-ng-init="Panel13531=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px" id="TotalAcuseAdquiriente">Acuse de Respuesta</h4>
									<div data-ng-repeat="c in ReporteAcuseAcumuladoAdquiriente">
										<div class="content-group-sm svg-center position-relative col-md-3 text-center" id="{{c.IdControl}}"></div>
										<div data-ng-if="$last" data-ng-init="CargarAcuseAcumuladoAdquiriente()"></div>
									</div>
								</div>
							</div>
						</div>
						<!-- /REPORTE ESTADO ACUSE ACUMULADO-->

						<!-- REPORTE ACUMULADO TIPO DOCUMENTO -->
						<div class="col-md-6 col-lg-6" id="Panel13532" data-ng-if="Panel13532" data-ng-init="Panel13532=false">
							<div class="panel">
								<div class="panel-body">
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px" id="TotalTiposDocAdquiriente">Tipos de Documento</h4>
									<div data-ng-repeat="c in ReporteDocumentosTipoAdquiriente">
										<div class="content-group-sm svg-center position-relative col-md-4 text-center" id="{{c.IdControl}}"></div>
										<div data-ng-if="$last" data-ng-init="CargarDocumentosTipoAdquiriente()"></div>
									</div>
								</div>
							</div>
						</div>
						<!-- /REPORTE ACUMULADO TIPO DOCUMENTO -->

						<!-- REPORTE TIPO DOCUMENTO ANUAL -->
						<div class="col-md-12 col-lg-12" id="Panel13533" data-ng-if="Panel13533" data-ng-init="Panel13533=false">
							<div class="panel panel-default">

								<div class="panel-body">
									<div class="heading-elements panel-nav">
										<ul class="nav nav-tabs nav-tabs-bottom">
											<li class="dropdown">
												<a class="dropdown-toggle legitRipple btn-default" data-toggle="dropdown" aria-expanded="false">
													<i class="icon-cog5"></i>
													<span class="position-right">visualización</span>
													<span class="caret"></span>
												</a>
												<ul class="dropdown-menu dropdown-menu-right" style="background-color: #fcfcfc">
													<li><a data-ng-click="CambiarGrafico13533('bar')"><i class="icon-stats-bars2"></i>Barras</a></li>
													<li><a data-ng-click="CambiarGrafico13533('line')"><i class="icon-stats-dots"></i>Líneas</a></li>
												</ul>
											</li>
										</ul>
									</div>
									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px" id="TotalTiposDocGraficoAdquiriente">Tipos de Documento</h4>
									<label class="text-muted" style="margin-bottom: 20px">Indica la cantidad de documentos generados en el rango de tiempo seleccionado.</label>
									<div id="ReporteTipoDocumentoAnualAdquiriente"></div>
								</div>
							</div>
						</div>
						<!-- /REPORTE TIPO DOCUMENTO ANUAL -->

						<!-- REPORTE FLUJO TRANSACCIONAL -->
						<div class="col-md-12" id="Panel13534" data-ng-if="Panel13534" data-ng-init="Panel13534=false">
							<div class="panel panel-default">

								<div class="panel-body">
									<div class="heading-elements panel-nav">
										<ul class="nav nav-tabs nav-tabs-bottom">
											<li class="dropdown active">
												<div class="dx-field">
													<div class="dx-field-label">
														Cantidad Top: <i class="icon-info22" id="InfoPanel13534" style="margin-left: 2%"></i>
														<div id="ToolTipPanel13534"></div>
													</div>
													<div class="dx-field-value">
														<div id="CantTopTransaccionalAdquiriente"></div>
													</div>
												</div>
											</li>
										</ul>
									</div>

									<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 2px" id="TotalFlujoTransAdquiriente">Flujo Transaccional</h4>
									<label class="text-muted" style="margin-bottom: 20px">Informa el flujo de transacciones de las empresas con mayor movimiento.</label>

									<div id="ReporteTopMovimientoAdquiriente"></div>
								</div>
							</div>
						</div>
						<!-- /REPORTE FLUJO TRANSACCIONAL -->

					</div>

				</div>
				<!-- TAB ADQUIRIENTE -->

			</div>
			<!-- /CONTENIDOS TABS -->
			<div>
				<img src="../../Scripts/Images/ImgIndexLogos.PNG" class="img-responsive" data-ng-hide="IndicadoresFacturador || IndicadoresAdquiriente || IndicadoresAdmin " />
			</div>
		</div>
		<!-- /CONTENIDO PANEL-->



	</div>
	<!-- /CONTENEDOR PRINCIPAL -->


</asp:Content>
