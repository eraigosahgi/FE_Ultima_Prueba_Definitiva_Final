<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestablecerClavePagos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Login.RestablecerClavePagos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />

	<title>HGI Facturación Electrónica</title>
	<%--Se debe copiar el archivo al proyecto--%>
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
	<%--<script type="text/javascript" src="../../Scripts/assets/js/pages/login.js"></script>--%>

	<%--<script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/ripple.min.js"></script>--%>
	<!-- /theme JS files -->


	<!-- Estilos CSS -->
	<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.spa.css" />
	<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.common.css" />
	<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />

	<!-- DevExtreme -->
	<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
	<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.16/angular.min.js"></script>
	<script src="https://cdn3.devexpress.com/jslib/17.2.7/js/dx.all.js"></script>
	<script src="https://unpkg.com/devextreme-aspnet-data@1.3.0"></script>
	<link href="../../Content/dx.hgi.css" rel="stylesheet" />
	<!-- /DevExtreme -->

	<script src="../../Scripts/Pages/RestablecerClavePagos.js?vjs20201019"></script>


	<style>
		div.dx-progressbar-range {
			background-color: red;
			position: relative;
			border: 1px solid red;
		}
	</style>
</head>
<body class="login-container" style="background-color: #eeeded">

	<div class="demo-container" data-ng-app="RestablecerClavePagosApp" data-ng-controller="RestablecerClavePagosController">
		<!-- Contenedor de Página -->
		<div class="page-container">

			<!-- Contenido de Página -->
			<div class="page-content">

				<!-- Main content -->
				<div class="content-wrapper">

					<!-- Área de Contenido -->
					<div class="content">
						<form action="your-action" data-ng-submit="onFormSubmit($event)" class="ng-pristine ng-valid">

							<div class="panel panel-body login-form widget-container" id="divcontenido">
								<div class="text-center">
									<img id="img_cliente" class="img-responsive" />
									<h5 class="content-group-lg">Restablecimiento de Contraseña</h5>
								</div>

								<div class="widget-container">

									<div id="pswd_info" style="text-align: justify;">
										<h4>La contraseña debe cumplir con el 60% de seguridad de acuerdo con los siguientes criterios:</h4>
										<ul>
											<li id="length" class="invalid"><strong>Ocho caracteres</strong>
											</li>
											<li id="letter" class="invalid"><strong>Una letra minúscula</strong>
											</li>
											<li id="capital" class="invalid"><strong>Una letra mayúscula</strong>
											</li>
											<li id="simbolo" class="invalid"><strong>Un caracter especial</strong>
											</li>
											<li id="number" class="invalid"><strong>Un número</strong>
											</li>

										</ul>
									</div>

									<div id="form" data-dx-form="formOptions">
									</div>
								</div>
								<br />

								<div id="PanelControles" runat="server">

									<div id="progress">
										<div id="progressBarStatus"></div>
									</div>
									<div data-dx-button="buttonAceptar"></div>
									<br />
									<br />
								</div>

							</div>

							<div class="panel panel-body login-form widget-container" id="divexito" style="display: none">
								<div class="text-center">
									<div class="icon-object border-primary-800 text-primary-800"><i class="icon-lock2"></i></div>
									<h5 class="content-group-lg">Cambio de Contraseña exitoso<small class="display-block"><a href="Default.aspx">Por favor ingrese al siguiente link para iniciar sesión</a></small></h5>
								</div>


							</div>

							<!-- Pie de Página -->
							<div class="footer text-muted text-center" style="font-size: 13px">
								Copyright &copy; 2022 <a href="http://www.hgi.com.co" target="_blank" style="color: #166dba;">HGI S.A.S - HGI Facturación Electrónica</a>
							</div>
							<!-- /Pie de Página -->

						</form>
						<!-- /Formulario Autenticación -->
					</div>
					<!-- /Área de Contenido -->

				</div>
				<!-- /main content -->

			</div>
			<!-- /Contenido de Página -->

		</div>
		<!-- /Contenedor de Página -->
	</div>
</body>
</html>
