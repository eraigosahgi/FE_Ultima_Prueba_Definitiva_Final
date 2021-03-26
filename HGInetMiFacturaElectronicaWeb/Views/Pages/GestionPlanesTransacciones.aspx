<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionPlanesTransacciones.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionPlanesTransacciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20201019"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs20201019"></script>
	<script src="../../Scripts/Pages/PlanesTransacciones.js?vjs20210326"></script>
	<script src="../../Scripts/Pages/ModalConsultaEmpresas.js?vjs20201019"></script>

	<div data-ng-app="GestionPlanesApp" data-ng-controller="GestionPlanesController" data-ng-init="consumido=false" data-ng-cloak>

		<div class="col-md-12" data-ng-init="PanelNotificacion=true">
			<div class="panel panel-white">
				<div class="panel-heading">
					<div class="col-md-6" style="margin-right: 2%; margin-top: -10px;">
						<h6 class="panel-title" title="Prueba">Gestión de Planes Transaccionales</h6>
					</div>
					<div id="Leyenda" style="float: right; margin-right: 2%; margin-top: -10px;">
						<label id="FechaInicio" class="text-right" style="font-size: medium; margin-right: 40px;" title="Fecha de rececpción del primer documento de este plan">{{FechaInicio}}</label>
						<label id="Total" class="text-semibold text-right" style="font-size: medium;">Documentos:</label>
						<label id="lblProcesados" class="text-semibold text-right" style="font-size: medium; color: red; z-index: 999;" title="Procesados">{{Tprocesados}}</label>
						<label class="text-semibold text-right" style="font-size: medium;">| </label>
						<label id="lblSaldo" class="text-semibold text-right" style="font-size: medium; color: green;" title="Disponibles">{{Tdisponibles}}</label>
					</div>
				</div>

				<div class="panel-body">

					<div class="col-lg-12">

						<form id="form1" action="your-action">
							<div class="row">

								<div class="col-md-12">

									<div class="col-md-6 col-xs-12" style="margin-top: 16px; z-index: 9;">
										<div class="dx-field-label" style="font-size: 14px; width: auto;">Tipo Proceso:<strom style="color: red;">*</strom></div>
										<div class="dx-field-value">
											<div id="TipoProceso" style="margin-left: -4%;"></div>
										</div>
									</div>

									<div class="col-md-6 col-xs-12" style="margin-top: 16px">
										<div data-hgi-filtro="Facturador"></div>
									</div>


									<div class="col-md-3 col-xs-12" style="z-index: 9;" id="divCantTransacciones">
										<label style="margin-top: 16px;">Cantidad Transacciones:<strom style="color: red;">*</strom></label>
										<div id="CantidadTransacciones"></div>
									</div>

									<div class="col-md-3 col-xs-12" style="z-index: 9;" id="divValorPlan">
										<label style="margin-top: 16px;">Valor Plan:<strom style="color: red;">*</strom></label>
										<div id="ValorPlan"></div>
									</div>
									
									<div class="col-md-1" style="margin-top: 16px; z-index: 9;">
										<div class="col-md-12 text-center">
											<label>Vence?</label>
										</div>
										<div class="col-md-12 text-center" style="margin-top: 10px; z-index: 9;">
											<div id="Vence"></div>
										</div>
									</div>

									<div id="panelfechaVencimiento" class="col-md-5">
									<div class="col-md-6 " style="z-index: 9;" >
										<label style="margin-top: 16px;">Meses vence:</label>
										<div id="MesesVence"></div>
									</div>

									<div class="col-md-6"  style="margin-top: 16px; z-index: 9;">
										<label id="TituloFecVenc" style="display:none">Fecha Vencimiento:</label>
										<div id="FechaVence"></div>
									</div>
									</div>
									<div class="col-md-12" style="margin-top: 16px; z-index: 9;">
										<div class="col-md-6">
											<div class="dx-field-label" style="font-size: 14px;">Estado:<strom style="color: red;">*</strom></div>

											<div class="dx-field-value" data-ng-hide="consumido">
												<div id="EstadoPlan" style="margin-left: -4%;"></div>
											</div>

											<div class="dx-field-value" data-ng-show="consumido">
												<label style="margin-top: 9px; margin-bottom: 1%; font-size: 14px; color: grey;"><b>Procesado</b></label>
											</div>
										</div>
										<div class="col-md-2" style="margin-top:-8px">
											<label style="margin-top: 16px;">Documento Ref:</label>
											<div id="DocRef"></div>
										</div>

									</div>



									<div class="col-md-12" style="margin-top: -10px; z-index: 0;">

										<label style="margin: 0px; margin-top: 16px; font-size: 14px;">Observaciones: </label>



										<div id="txtObservaciones"></div>

									</div>
									<div class="col-md-12 text-right" data-ng-show="PanelNotificacion">
										<label style="margin-top: 10px;">Enviar notificación de recarga:</label>
										<div id="Email"></div>
									</div>
								</div>


								<div class="dx-fieldset">
									<div id="summary"></div>
								</div>
							</div>

							<div class="col-lg-12 text-right">
								<br />
								<br />
								<a id="btncancelar" class="btn btn-default" style="text-transform: initial !important; margin-right: 1%" href="/Views/Pages/ConsultaPlanesTransacciones.aspx" style="font-size: 14px; text-align: center;">Cancelar</a>
								<div id="button"></div>
							</div>

						</form>

					</div>

				</div>
			</div>
		</div>

	</div>

</asp:Content>
