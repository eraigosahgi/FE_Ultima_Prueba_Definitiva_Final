<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcuseRecibo.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.AcuseRecibo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />

	<meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
	<meta http-equiv="Pragma" content="no-cache" />
	<meta http-equiv="Expires" content="0" />

	<title>HGI Facturación Electrónica</title>

	<!-- Global stylesheets -->
	<link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css" />
	<!-- /Global stylesheets -->

	<script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
	<script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.17.1/moment.min.js"></script>
	<!-- Global stylesheets -->
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
	<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.common.css" />
	<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />

	<!-- Scripts Requeridos
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>-->

	<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.16/angular.min.js"></script>
	<!--<script src="https://cdn3.devexpress.com/jslib/17.2.7/js/dx.all.js"></script>    -->
	<script src="../../Scripts/devall.js"></script>
	<script src="https://unpkg.com/devextreme-aspnet-data@1.3.0"></script>


	<link rel="stylesheet" type="text/css" href="https://www.w3schools.com/w3css/4/w3.css" />


	<script src="../../Scripts/devextreme-localization/dx.messages.es.js"></script>

	<!-- JS AcuseRecibo -->
	<script src="../../Scripts/WebConfig.js?vjs20220621"></script>
	<script src="../../Scripts/config.js?vjs20220621"></script>
	<script src="../../Scripts/Services/SrvDocumentos.js?vjs20220621"></script>
	<script src="../../Scripts/config.js?vjs20220621"></script>
	<script src="../../Scripts/Pages/DocumentosAdquiriente.js?vjs20220621"></script>
	<script src="../../Scripts/Pages/AcuseRecibo.js?vjs20220621"></script>
	<script src="../../Scripts/Pages/ModalPagos.js?vjs20220621"></script>

</head>

<body class="login-container" style="background-color: #eeeded">
	<input type="hidden" id="Hdf_RutaPlataformaServicios" runat="server" />

	<div runat="server" class="form-horizontal" ng-app="App" ng-controller="AcuseReciboController" data-ng-cloak="" data-ng-init="DetalleAcuse=true">

		<!--Aplicacion de Pagos-->
		<div data-ng-include="'ModalPagos.aspx'"></div>
		<div data-ng-include="'Partials/ModalFormasDePago.html'"></div>
		<div data-ng-include="'Partials/ModalPagosEmbebida.html'"></div>
		<!--Aplicacion de Pagos-->

		<div style="margin: 1%;" runat="server" id="PanelInformacion" ng-repeat="datos in RespuestaAcuse">
			<!--Ruta de servicios de plataforma de pago-->
			<input type="hidden" id="Hdf_RutaPagos" runat="server" />
			<input type="hidden" id="Hdf_RutaSrvPagos" runat="server" />
			<!-- Visualización Información Factura -->
			<div class="col-md-6">
				<div class="panel panel-flat form-horizontal">


					<img class="img-responsive" alt="" title="" src="https://tz37.mjt.lu/tplimg/tz37/b/1k4nu/v7gni.png" style="float: right;" />
					<br />
					<br />
					<br />
					<div class="panel-body" style="display: block">

						<div id="PanelInformacionFactura" style="font-size: 15px" class="dx-fieldset">

							<h4 class="panel-title text-bold text-center" style="margin-bottom: 5%">Información de {{datos.tipodoc}}</h4>

							<div>
								<label id="Label2" class="text-bold">Número Documento: </label>
								<label id="LblNumeroDocumento"></label>
								<span>{{datos.NumeroDocumento}}</span>
							</div>

							<div style="margin-top: 0%">
								<label id="Label5" class="text-bold">Identificación Tercero: </label>
								<span>{{datos.IdAdquiriente}}</span>
							</div>

							<div style="margin-top: 0%">
								<label id="Label9" class="text-bold">Nombre Tercero: </label>
								<span>{{datos.NombreAdquiriente}}</span>
							</div>

							<div style="margin-top: 0%; width: 90%; word-wrap: break-word;">
								<label class="text-bold">CUFE: </label>
								<span>{{datos.Cufe}}</span>
								<br />
							</div>

							<div style="margin-bottom: 0%">
								<label class="text-bold">Radicado: </label>
								<span>{{datos.IdSeguridad}}</span>
							</div>

							<div style="margin-bottom: 0%">
								<label class="text-bold">Estado: </label>
								<span>{{datos.EstadoFactura}}</span>
							</div>


						</div>


						<!-- PANEL CONTIENE LA RESPUESTA SI YA LA TIENE-->
						<div id="PanelRespuestaAdquiriente" style="margin-top: -25px; font-size: 15px" ng-show="{{datos.RespuestaVisible}}" class="dx-fieldset">

							<h4 class="panel-title text-bold text-center">Respuesta Adquiriente</h4>

							<div style="margin-top: 1%;" runat="server" id="DivEstadoRespuesta">
								<label class="text-bold">Estado: </label>
								<span>{{datos.EstadoAcuse}}</span>
							</div>


							<div style="margin-top: 1%;" runat="server" id="DivObservaciones">
								<label class="text-bold">Observaciones: </label>
								<span>{{datos.MotivoRechazo}}</span>
							</div>

							<div style="margin-top: 1%;" runat="server" id="Div1">
								<label class="text-bold">Fecha: </label>
								<span>{{datos.FechaRespuesta | date: "yyyy-MM-dd HH:mm"}}</span>
							</div>

						</div>

						<!--data-ng-if="DetalleAcuse"-->
						<div>
							<!-- PANEL CONTIENE LAS OPCIONES DE RESPUESTA APROBAR/RECHAZAR Y MOTIVO -->
							<div id="PanelOpcionesAdquiriente" style="margin-top: -25px" data-ng-show="{{datos.CamposVisibles && datos.EstadoCat !=400}}" class="dx-fieldset">
												   
								<form data-ng-submit="onFormSubmit($event)">

									<div>
										<h4 class="panel-title text-bold text-center">Respuesta Acuse</h4>
										<br />
										<div class="col-md-12">

											<input type="radio" style="cursor: pointer" class="w3-radio" value="3" data-ng-model="value" id="rb_acuse" name="group2" ng-change="ValidarEstado(value)" data-ng-show="{{datos.IntAdquirienteRecibo != 4}}"/>
											<label><strong>Recibo del bien o prestación del servicio</strong></label>
											 <label class="text-danger" data-ng-show="{{datos.IntAdquirienteRecibo > 3}}">Ejecutado</label>
										</div>
										<br />
										<div class="col-md-12 ">

											<input type="radio" style="cursor: pointer" class="w3-radio" value="1" data-ng-model="value" id="rb_expresa" name="group1" ng-change="ValidarEstado(value)" />
											<label><strong>Aceptación Expresa:</strong></label>
										</div>
										<br />
										<div class="col-md-12 ">

											<input type="radio" style="cursor: pointer" class="w3-radio" value="2" data-ng-model="value" id="rb_rechazo" name="group1" ng-change="ValidarEstado(value)" />
											<label><strong>Reclamos a la Venta:</strong></label>
										</div>
										<br />
										<br />
									</div>

									<div id="form" data-dx-form="TextAreaObservaciones" style="padding-left: 2%; padding-top: 5%;"></div>
									<br />
									<div class="col-lg-12 text-right" style="margin-top: -15px">
										<div data-dx-button="ButtonOptionsRechazar" data-ng-if="RechazarVar"></div>
										<div data-dx-button="ButtonOptionsAceptar" data-ng-if="AceptarVar"></div>
										<div data-dx-button="ButtonOptionsAcuse" data-ng-if="AcuseVar" data-ng-show="{{datos.IntAdquirienteRecibo != 4}}"></div>
										&nbsp;&nbsp;&nbsp;
                                    
									</div>

								</form>
							</div>
						</div>
						<div id="PanelInformacionArchivos" style="font-size: 15px" class="dx-fieldset">

							<h4 class="panel-title text-bold text-center">Archivos</h4>

							<div style="margin-top: 1%; text-align: center;">

								<a href="{{datos.Pdf}}" target="_blank" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px;">Pdf</a>

								<a href="{{datos.Xml}}" target="_blank" data-ng-show="{{datos.EstadoCat !=400}}" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px;">Xml</a>

								<a href="{{datos.XmlAcuse}}" target="_blank" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px;" data-ng-show="datos.XmlAcuse != null">Xml Acuse</a>

								<a id="btnautenticar" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important" href="../Login/Default.aspx" style="font-size: 14px; text-align: center;">Autenticar</a>
							</div>
							<br />


							<div style="margin-top: 1%; text-align: justify;">
								Para visualizar los archivo en el navegador presione clic o si desea descargarlos presione clic derecho sobre el link y seleccione la opción Guardar como.
							</div>

							<div>

								<div style="text-align: center; margin-top: 5%;">
									<div style="display: inline-block; margin-bottom: 5%">
										<a class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px; text-align: center;" data-ng-hide="datos.tipodoc=='Nota Crédito' || datos.poseeIdComercio==false || datos.Estatus !=1" data-ng-click="ConsultarDetallesPago()">Historial Pagos</a>
									</div>
									<div id="cmdpago" style="display: inline-block; margin-bottom: 5%">
										<a class="btn btn-default" id="btnpago" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px; text-align: center;" target="_blank" data-toggle="modal" data-target="#modal_Pagos_Electronicos" data-ng-click="ConsultarPago1('','1',true,true)" data-ng-show="datos.tipodoc!='Nota Crédito' && datos.poseeIdComercio==true && datos.Estatus ==3">Realizar Pago</a>
									</div>
								</div>

								<div id="pnl_Verificar_Pago" data-ng-if="EnProceso" class="text-center">
									<hr />
									<div class="col-md-12">Verificando estado del pago</div>
									<div data-dx-button="buttonProcesar">
										<i class="icon-spinner6 spinner mr-2"></i>
									</div>
								</div>
							</div>
						</div>

					</div>
					<div class="footer text-muted" style="font-size: 14px; text-align: center;">
						Copyright © 2022 <a href="https://www.hgidocs.co" target="_blank" style="color: rgb(22, 109, 186);">HGI S.A.S - HGI Facturación Electrónica</a>
					</div>

				</div>

			</div>

			<%-- <!-- Visualización PDF -->
            <div class="col-md-6">
                <embed width="100%" height="850px" id="plugin"  type="application/pdf"/>                
            </div>
            <!-- /Visualización PDF -->--%>
		</div>
		<!-- Visualización PDF -->
		<div class="col-md-6">
			<iframe width="100%" height="850px" id="plugin" type="application/pdf"></iframe>
		</div>
		<!-- /Visualización PDF -->

		<div id="modal_pagos_documento" class="modal fade" style="display: none; z-index: 999999;">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title">Pagos Realizados</h5>
					</div>
					<div class="modal-body">
						<div class="col-md-12 ">
							<!-- CONTENEDOR PRINCIPAL -->

							<div class="col-md-12">
								<div class="panel panel-white">

									<div class="panel-body">

										<div class="demo-container">
											<div id="gridPagosDocumento"></div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>

					<div id="divsombra" class="modal-footer" style="margin-top: 22%">
					</div>

				</div>
			</div>
		</div>

		<%--<form id="formInicioPago" target="_blank">
			<div id="modal_inicio_pago" class="modal fade" style="display: none; z-index: 999999;">
				<div class="modal-dialog">
					<div class="modal-content">
						<div class="modal-header">
							<button type="button" class="close" data-dismiss="modal">×</button>
							<h5 style="margin-bottom: 5px;" class="modal-title">Efectuar Pago</h5>
						</div>
						<div class="modal-body">
							

							<div class="row" style="margin-bottom: 5%; margin-left: 3%; margin-right: 3%" runat="server" id="PanelInfoRecaudador">

								<div class="col-sm-12" style="background-color: #F3F3F3; height: 40px; margin-bottom: 2%;">
									<div style="margin-top: 1.5%">
										<asp:Label runat="server" Style="vertical-align: middle;" Font-Size="Medium"><b>Información Documento</b></asp:Label>
									</div>
								</div>

								<div class="col-sm-4" style="background-color: #F3F3F3; margin-bottom: 2%; height: 30px">
									<asp:Label runat="server" Style="vertical-align: middle; font-size: small"><b>Tipo:</b></asp:Label>
								</div>
								<div class="col-sm-8" style="margin-bottom: 2%; height: 30px">
									<asp:Label runat="server" Style="vertical-align: middle; font-size: small">{{tipodoc}}</asp:Label>
								</div>

								<div class="col-sm-4" style="background-color: #F3F3F3; margin-bottom: 2%; height: 30px">
									<asp:Label runat="server" Style="vertical-align: middle; font-size: small"><b>Número:</b></asp:Label>
								</div>
								<div class="col-sm-8" style="margin-bottom: 2%; height: 30px">
									<asp:Label runat="server" Style="vertical-align: middle; font-size: small">{{NumeroDocumento}}</asp:Label>
								</div>

								<div class="col-sm-4" style="background-color: #F3F3F3; margin-bottom: 2%; height: 30px">
									<asp:Label runat="server" Style="vertical-align: middle; font-size: small"><b>Fecha:</b></asp:Label>
								</div>
								<div class="col-sm-8" style="margin-bottom: 2%; height: 30px">
									<asp:Label runat="server" Style="vertical-align: middle; font-size: small">{{FechaDocumento}}</asp:Label>
								</div>

								<div class="col-sm-4" style="background-color: #F3F3F3; margin-bottom: 2%; height: 30px">
									<asp:Label runat="server" Style="vertical-align: middle; font-size: small"><b>Valor:</b></asp:Label>
								</div>
								<div class="col-sm-8" style="margin-bottom: 2%; height: 30px">
									<asp:Label runat="server" Style="vertical-align: middle; font-size: small">{{ValorDoc | currency:"$ " }}</asp:Label>
								</div>

								<div data-ng-show="InfoValorPago">
									<div class="col-sm-4" style="background-color: #F3F3F3; margin-bottom: 2%; height: 30px">
										<asp:Label runat="server" Style="vertical-align: middle; font-size: small"><b>Pago Pendiente:</b></asp:Label>
									</div>
									<div class="col-sm-8" style="margin-bottom: 2%; height: 30px">
										<asp:Label runat="server" Style="vertical-align: middle; font-size: small">{{ValorPago | currency:"$ " }}</asp:Label>
									</div>
								</div>

								<div data-ng-show="CampoValorPago">
									<div class="col-sm-4" style="background-color: #F3F3F3; margin-bottom: 2%; height: 36px">
										<asp:Label runat="server" Style="vertical-align: middle; font-size: small"><b>Monto a Pagar:</b></asp:Label>
									</div>
									<div class="col-sm-8" style="margin-bottom: 2%; height: 30px">
										<div id="TxtValorPago"></div>
									</div>
								</div>

							</div>

							<div id="summary"></div>

						</div>
						<div class="modal-footer">
							<div id="BtnContinuarPago"></div>
						</div>
					</div>
				</div>
			</div>
		</form>--%>
	</div>

	<%--Panel carga o Loading--%>
	<div id="wait" style="display: none; z-index: 9999;">
		<div class="modal" style="background-color: white; opacity: 0.6; display: block;"></div>
		<div>
			<img class="divImg" style="position: absolute; left: 43%; top: 30%; z-index: 9999; width: 20%; height: 20%;" src="../../Content/icons/Loading.gif" />
		</div>
	</div>




	<script>
		try {
			(function (i, s, o, g, r, a, m) {
				i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () { (i[r].q = i[r].q || []).push(arguments) }, i[r].l = 1 * new Date(); a = s.createElement(o),
				m = s.getElementsByTagName(o)
				[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
			})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
		} catch (e) { }
	</script>
</body>


</html>
