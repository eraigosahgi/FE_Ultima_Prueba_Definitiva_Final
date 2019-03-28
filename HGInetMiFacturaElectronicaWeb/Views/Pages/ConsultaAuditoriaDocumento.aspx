<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaAuditoriaDocumento.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaAuditoriaDocumento" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />

	<meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
	<meta http-equiv="Pragma" content="no-cache" />
	<meta http-equiv="Expires" content="0" />

	<title>HGInet Factura Electrónica</title>

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

	<style>
		.divImg {
			border-radius: 40px;
			-moz-border-radius: 40px;
			-webkit-border-radius: 50px;
			border-style: solid;
			border-color: white;
			border-right-width: 15px;
			border-left-width: 15px;
			background-color: white;
		}
	</style>

	<script src="../../Scripts/Pages/ConsultaAuditoriaDocumento.js?vjs201915"></script>
	<script src="../../Scripts/config.js?vjs201915"></script>

</head>
<body class="login-container" style="background-color: #eeeded">

	<%--Panel carga o Loading--%>
	<div id="wait" style="display: none; z-index: 9999;">
		<div class="modal" style="background-color: white; opacity: 0.6; display: block;"></div>
		<div>
			<img class="divImg" style="position: absolute; left: 43%; top: 30%; z-index: 9999; width: 20%; height: 20%;" src="../../Content/icons/Loading.gif" />
		</div>
	</div>

	<div runat="server" class="form-horizontal" data-ng-app="AuditDocumentoApp" data-ng-controller="AuditDocumentoController">

		<div style="margin: 1%;" runat="server">

			<!-- Visualización Información Documento -->
			<div class="col-md-6">
				<div class="panel panel-flat form-horizontal">

					<img class="img-responsive" alt="" title="" src="http://tz37.mjt.lu/tplimg/tz37/b/6572/x8s6k.png" style="float: right;" />
					<br />
					<br />
					<br />
					<div class="panel-body">

						<div style="font-size: 15px" class="dx-fieldset">

							<h4 class="panel-title text-bold text-center" style="margin-bottom: 5%">Información de {{TipoDocumento}}</h4>

							<div>
								<label id="Label2" class="text-bold">Número Documento: </label>
								<label id="LblNumeroDocumento"></label>
								<span>{{StrNumero}}</span>
							</div>

							<div style="margin-bottom: 0%">
								<label class="text-bold">Radicado: </label>
								<span>{{StrIdSeguridad}}</span>
							</div>

							<div style="margin-bottom: 0%">
								<label class="text-bold">Facturador: </label>
								<span>{{StrObligado}}</span>
							</div>

							<div style="margin-bottom: 0%">
								<label class="text-bold">Estado Actual: </label>
								<span>{{StrDesEstado}}</span>
							</div>

							<div style="margin-bottom: 0%">
								<label class="text-bold">Último Proceso: </label>
								<span>{{StrDesProceso}}</span>
							</div>

							<div style="margin-bottom: 0%">
								<label class="text-bold">Fecha Último Proceso: </label>
								<span>{{DatFecha}}</span>
							</div>
						</div>

						<div id="PanelInformacionArchivos" style="font-size: 15px" class="dx-fieldset">

							<h4 class="panel-title text-bold text-center" style="margin-bottom: 5%">Archivos</h4>

							<div style="margin-top: 1%; text-align: center;">

								<a href="{{RutaPdf}}" target="_blank" data-ng-show="VerPdf" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px;">Pdf</a>

								<a href="{{RutaXml}}" target="_blank" data-ng-show="VerXml" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px;">Xml</a>

								<a href="{{RutaXmlAcuse}}" target="_blank" data-ng-show="VerXmlAcuse" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px;">Xml Acuse</a>
							</div>
							<br />

							<div style="margin-top: 1%; text-align: justify;">
								Para visualizar los archivo en el navegador presione clic o si desea descargarlos presione clic derecho sobre el link y seleccione la opción Guardar como.
							</div>

						</div>
						<br />
						<br />
						<div class="footer text-muted" style="font-size: 14px; text-align: center;">
							Copyright © 2018 <a href="http://www.hgi.com.co" target="_blank" style="color: rgb(22, 109, 186);">HGI S.A.S - HGInet Factura Electrónica</a>
						</div>

					</div>


				</div>

			</div>
			<!-- /Visualización Información Documento -->
		</div>

		<div class="col-md-6">
			<div class="panel panel-flat form-horizontal">
				<div class="panel-body" style="display: block">

					<!-- MENÚ TABS -->
					<ul class="nav nav-tabs">
						<li class="active" id="LiTabAuditoriaDoc"><a id="LinkTabAuditoriaDoc" href="#TabAuditoriaDoc" data-toggle="tab">Auditoría</a></li>
						<li id="LiTabVerPdf" data-ng-show="VerPdf"><a id="LinkVerPdf" href="#TabVerPdf" data-toggle="tab">Pdf</a></li>
					</ul>
					<!--  /MENÚ TABS -->

					<!-- CONTENIDOS TABS -->
					<div class="tab-content">

						<!-- TAB AUDITORÍA -->
						<div class="tab-pane active" id="TabAuditoriaDoc">

							<div id="PanelAuditoriaDocumento" class="dx-fieldset">
								<div class="demo-container">
									<div id="gridAuditDocumento"></div>
								</div>
							</div>
						</div>
						<!-- /TAB AUDITORÍA -->

						<!-- TAB VISUALIZACIÓN PDF -->
						<div class="tab-pane" id="TabVerPdf">

							<div id="PanelPdfDocumento" class="dx-fieldset">
								<iframe width="100%" height="850px" id="PanelVisorPdf" type="application/pdf"></iframe>
							</div>

						</div>
						<!-- /TAB VISUALIZACIÓN PDF -->
					</div>

				</div>
			</div>
		</div>

	</div>

</body>
</html>
