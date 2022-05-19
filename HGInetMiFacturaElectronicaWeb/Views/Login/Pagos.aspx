<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pagos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Login.Pagos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
	<meta http-equiv="Pragma" content="no-cache" />
	<meta http-equiv="Expires" content="0" />

	<title>HGIDocs: Facturación Electrónica</title>

	<script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
	<!-- Global stylesheets -->
	<link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css" />
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
	<script type="text/javascript" src="../../Scripts/assets/js/plugins/forms/styling/uniform.min.js"></script>

	<script type="text/javascript" src="../../Scripts/assets/js/core/app.js"></script>
	<script type="text/javascript" src="../../Scripts/assets/js/pages/login.js"></script>

	<!-- /theme JS files -->


	<!-- Estilos CSS -->
	<%--<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.spa.css" />--%>
	<link href="../../Scripts/Scripts-descarga/dx.spa.css" rel="stylesheet" />

	<%--<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />--%>
	<link href="../../Scripts/Scripts-descarga/dx.common.css" rel="stylesheet" />
	<link href="../../Scripts/Scripts-descarga/dx.light.css" rel="stylesheet" />

	<!-- DevExtreme -->
	<%--<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>--%>
	<script src="../../Scripts/Scripts-descarga/jszip.min.js"></script>

	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
	<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.16/angular.min.js"></script>
	<%--<script src="https://cdn3.devexpress.com/jslib/17.2.7/js/dx.all.js"></script>--%>
	<script src="../../Scripts/Scripts-descarga/dx.all.js"></script>

	<script src="https://unpkg.com/devextreme-aspnet-data@1.3.0"></script>

	<!-- /DevExtreme -->


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

	<script src="../../Scripts/WebConfig.js?vjs20210308r2"></script>
	<script src="../../Scripts/Pages/AutenticacionPagos.js?vjs20210308r2"></script>
	<script src="../../Scripts/Pages/RegistroUsuarioPagos.js"></script>


</head>
<body class="login-container" style="background-color: #eeeded">
	<%--Panel carga o Loading--%>
	<div id="wait" style="display: none; z-index: 9999;">
		<div class="modal" style="background-color: white; opacity: 0.6; display: block;"></div>
		<div>
			<img class="divImg" style="position: absolute; left: 43%; top: 30%; z-index: 9999; width: 20%; height: 20%;" src="../../Content/icons/Loading.gif" />
		</div>
	</div>
	<div class="row" style="height: 106%; background-color: white; display: none" id="pnl_general">

		<div class="col-md-4 col-md-offset-4 " style="background-color: white; height: 106%;" id="pnl_autenticacion">
			<div data-ng-app="AutenticacionPagosApp">
				<!-- Contenedor de Página -->
				<div class="page-container" data-ng-controller="AutenticacionPagosController">

					<!-- Contenido de Página -->
					<div>

						<!-- Main content -->
						<div class="text-center">

							<!-- Área de Contenido -->
							<div class="content">

								<!-- Formulario Autenticación -->
								<form data-ng-submit="onFormSubmit($event)">


									<div class="text-center">
										<img id="img_cliente" style="align-content: center; width: 350px" class="img-responsive center-block" />
										<br />
										<%--<div class="icon-object border-primary-800 text-primary-800"><i class="icon-user"></i></div>--%>
										<h5 class="content-group-lg"><small class="display-block">Ingrese los datos de autenticación</small></h5>
									</div>

									<div class="widget-container">
										<div id="form" data-dx-form="formOptions">
										</div>
									</div>
									<br />

									<table style="width: 100%;" class="text-right">
										<tbody>
											<tr>
												<td>
													<div  data-dx-button="buttonOptions"></div>
												</td>
												<td></td>
											</tr>
										</tbody>
									</table>
									<br />
									<br />
									<div class="text-center">
										<a data-toggle="modal" data-target="#modal_restablecer_clave" data-popup="tooltip" title="Restablecer contraseña" style="color: #166dba">
											<h6>Restablecer contraseña</h6>
										</a>
										<br />
										<a data-toggle="modal" data-target="#modal_registrar_usuario_pagos" data-popup="tooltip" style="color: #166dba" title="Registrarme">Registrarme										
										</a>



									</div>
									<br />
									<div style="font-size: small; margin-bottom: 3%; margin-top: 3%; display: none" class="text-left">
										<a href="https://www.hgi.com.co/protecciondatospersonales/" target="_blank" style="color: #001d85 !important; text-decoration: underline #001d85 !important; line-height: 2;">Política de Protección de Datos Personales</a><br />
										<a href="https://www.hgi.com.co/politicadeseguridaddelainformacion/" target="_blank" style="color: #001d85 !important; text-decoration: underline #001d85 !important; line-height: 2;">Política de Seguridad de la Información</a><br />
										<a href="https://www.hgi.com.co/politicadeusodelportalwebfacturaelectronica/" target="_blank" style="color: #001d85 !important; text-decoration: underline #001d85 !important; line-height: 2;">Política de Uso del Portal Web Factura Electrónica</a>
									</div>

								</form>
								<!-- /Formulario Autenticación -->

								<span class="help-block text-center no-margin">
									<br />
									Copyright © 2022 <a href="http://www.hgi.com.co" target="_blank">HGI S.A.S.</a>
									<br />
								</span>

							</div>
							<!-- /Área de Contenido -->



						</div>
						<!-- /main content -->

					</div>
					<!-- /Contenido de Página -->

					<div ng-include="'RegistroUsuarioPagos.html'"></div>
				</div>
				<!-- /Contenedor de Página -->
				<%--Modal Restablecer Contraseña--%>
				<form data-ng-submit="onFormSubmit($event)" data-ng-controller="RestablecerPagosController">
					<div id="modal_restablecer_clave" class="modal fade" style="display: none;">
						<div class="modal-dialog">
							<div class="modal-content">
								<div id="EncabezadoModal" class="modal-header">
									<button type="button" class="close" data-dismiss="modal" data-ng-click="cerrarmodal();">×</button>
									<h5 style="margin-bottom: 10px;" class="modal-title">Restablecer Contraseña</h5>
								</div>

								<div class="modal-body">

									<div class="col-md-8 col-md-offset-2">

										<div id="formulario" class="row">


											<div id="formvalidar" data-dx-form="formOptions2">
											</div>



										</div>

									</div>
								</div>

								<div id="divsombra" class="modal-footer" style="margin-top: 12%">
									<div data-dx-button="buttonCerrarRestablecer" data-dismiss="modal"></div>
									<div data-dx-button="buttonRestablecer"></div>
								</div>

							</div>
						</div>
					</div>

				</form>
			</div>
		</div>


	</div>
	<div id="panelfondo"></div>
	<%--/ Modal Restablecer Contraseña--%>



	<script>
		(function (i, s, o, g, r, a, m) {
			i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () { (i[r].q = i[r].q || []).push(arguments) }, i[r].l = 1 * new Date(); a = s.createElement(o),
			m = s.getElementsByTagName(o)
			[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
		})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
	</script>

</body>
</html>
