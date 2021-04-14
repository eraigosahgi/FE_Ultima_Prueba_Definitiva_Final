/// <reference path="../config.js" />
//Documentación
//****************************************************************************************
//**  **
//**  **
//**  **
//**  **
//**  **
//**  **
//**  **
//****************************************************************************************

var ruta = 'http://localhost:61437';
var ruta_servicios = 'https://pruebascloudservices.hginet.co/Views/IniciarPago.aspx';

var apphgi = angular.module('myApp', []);
apphgi.controller('myCtrl', function ($scope, SrvPagos) {
	$scope.serial = "";
	$scope.Identificacion = "";
	$scope.Documento = "";
	$scope.Prefijo = "";
	$scope.Monto = 0;
	$scope.IdSeguridad = '';

	$scope.lista_pagos = [];



	SrvPagos.Cargar().then(function (Datos) {
		$scope.PanelPagos = Datos;
	});

	$scope.ConsultarDocumento = function () {
		Consultar();
	}


	$scope.PagarDocumento = function (documento, monto) {
		InicicarPago(documento, monto)
	}

	function Consultar() {

		$scope.lista_pagos = [];
		$('#tblPagos').hide();
		$.ajax({
			url: ruta + '/api/ConsultarPagosFueraPlataforma?IdSeguridad=' + $scope.serial + '&identificacion=' + $scope.Identificacion + '&documento=' + $scope.Documento + '&prefijo=' + $scope.Prefijo,
			success: function (respuesta) {

				$scope.lista_pagos = respuesta;

				if (!$scope.$$phase) $scope.$apply();

				console.log($scope.lista_pagos);

				//$scope.Monto = respuesta.valor;
				//$scope.IdSeguridad = respuesta.IdSeguridad;
				//$("#lblpago").text(fNumber.go($scope.Monto));
				//console.log("Datos de la Identificacion: ", $scope.Identificacion);
				//console.log("Datos del Documento", $scope.Documento);
				//console.log("Datos del Prefijo", $scope.Prefijo);
				//$('#BtnPagar').show();
				//$('#BtnConsultar').hide();
				//$('#pnltextomonto').show();
				$('#tblPagos').show();
				$("#mensajeError").text("");

			},
			error: function (error) {
				$("#mensajeError").text(error.responseText);
			}
		});

	}


	function InicicarPago(idseg, valor_pago) {
		console.log(idseg);

		console.log(valor_pago);

		var forma_pago = 0;
		var UsuarioSession = '';
		//if (idseg != null && idseg != undefined) {
		//	$scope.EnProceso = true;

		//	$.ajax({
		//		url: ruta + '/api/Documentos?strIdSeguridad=' + idseg + '&tipo_pago=0&registrar_pago=true&valor_pago=' + $scope.Monto + '&usuario=' + UsuarioSession + '&IntPagoFormaPago=' + forma_pago,
		//		success: function (response) {



		//			var alto_pantalla = $(window).height() - 10;
		//			var ancho_pantalla = $(window).width() - 10;


		//			//Inicializo la variable en uno(1) cuando guardo el pago ya que luego debo consultar unas tres veces al servidor
		//			$scope.NumVerificacion = 1;
		//			//Ruta servicio
		//			var RutaServicio = ruta_servicios + "?IdSeguridad=";
		//			$scope.Idregistro = response.IdRegistro;

		//			//$("#modal_PagoEmbebido").modal("show");

		//			//$("#Pago_Embed").attr("src", RutaServicio + response.data.Ruta);
		//			var Vpago2 = window.open(RutaServicio + response.Ruta, "Pagos", "top:10px, width=" + ancho_pantalla + "px,height=" + alto_pantalla + "px;");


		//		}, error: function () {
		//			console.log("No se ha podido obtener la información");
		//		}
		//	});
		//}
	}



});

apphgi.service('SrvPagos', function ($location, $q) {
	this.Cargar = function () {

		var panel_pago = '<link href="../../Content/dx.hgi.css" rel="stylesheet" />\
		<script src="../../Scripts/config.js"></script>\
		<div class="col-md-3" style="width: 25%; margin-top: 1%;  min-width: 400px;">\
			<div class="panelPago">\
				<div id="wait" style="display: none; z-index: 9999;">\
					<div class="modal" style="background-color: lightslategray; opacity: 0.6; display: block;"></div>\
					<div>\
						<img class="divImg" style="position: absolute; left: 43%; top: 50px; z-index: 9999; max-width: 25%; max-height: 25%;" src="../../Content/icons/Loading.gif" />\
					</div>\
				</div>\
				<h4>Pagos Electronicos</h4>\
				<hr />\
				<div style="padding-top:12px; width:10%; float:left; min-width: 150px;">\
					<div style="padding-top:10px;" class="Etiquetas">\
						Identificacion\
					</div>\
					<div style="padding-top:50px;" class="Etiquetas">\
						Documento:\
					</div>\
					<div style="padding-top:50px;" class="Etiquetas">\
						Prefijo:\
					</div>\
					<div style="display:none;" id="pnltextomonto">\
						<div style="padding-top:40px;" class="Etiquetas">\
							Monto:\
						</div>\
					</div>\
				</div>\
				<div style="padding-top:10px; ">\
					<div style="padding-top:10px; text-align:right !important;">\
						<input type="text" ng-model="Identificacion" class="Texto">\
					</div>\
					<div style="padding-top:20px; text-align:right !important;">\
						<input type="text" ng-model="Documento" class="Texto">\
					</div>\
					<div style="padding-top:20px; text-align:right !important;">\
						<input type="text" ng-model="Prefijo" class="Texto">\
					</div>\
					<div style="display:none;" id="pnlvalormonto">\
						<div style="padding-top:20px; text-align: right !important;">\
							<label ng-model="Monto" id="lblpago">$0</label>\
						</div>\
					</div>\
					<div style="padding-top:10px;text-align: center;color: red;">\
				<label id="mensajeError"></label>\
				</div>\
					<div style=" padding-top:20px; text-align: right; ">\
						<input type="button" class="BtnConsulta" value="Consultar" id="BtnConsultar" ng-click="ConsultarDocumento()" />\
						<input type="button" class="BtnConsulta" value="Pagar" id="BtnPagar" ng-click="PagarDocumento()" style="display:none;" ng-click="PagarDocumento()" />\
					</div>\
				</div>\
			</div>\
		</div>';

		//Impresión de campos
		return $.when().then(function () {
			return panel_pago;
		});
	}


});


apphgi.directive('hgiPagos', function ($compile) {
	return {
		compile: function compile(tElement, tAttrs, transclude) {
			return {
				post: function preLink(scope, elem, iAttrs, controller) {
					var scopePropName = iAttrs['hgiPagos'];
					var linkingFunc = $compile(scope[scopePropName]);
					linkingFunc(scope, function (newElem) {
						elem.replaceWith(newElem);
					});
				}
			};
		}
	}
});



/*<div class="panel panel-white"  style="width: 25%;">

		<div data-ng-app="myApp" data-ng-controller="myCtrl" data-ng-init="serial='eb821fbe-02ba-4cfc-a7a9-248711513591'">
			<%--<div data-hgi-pagos="PanelPagos"></div>--%>
			<div style="margin-top: 1%; min-width: 400px;">
				<div class="panelPago">
					<div id="wait" style="display: none; z-index: 9999;">
						<div class="modal" style="background-color: lightslategray; opacity: 0.6; display: block;"></div>
						<div>
							<img class="divImg" style="position: absolute; left: 43%; top: 50px; z-index: 9999; max-width: 25%; max-height: 25%;" src="../../Content/icons/Loading.gif" />
						</div>
					</div>
					<h4>Pagos Electronicos</h4>
					<hr />
					<table style="padding: 10px; width: 100%;">
						<tr style="padding: 10px;">
							<td style="padding: 10px;">Identificacion</td>
							<td style="padding: 10px; text-align: right;">
								<input type="text" ng-model="Identificacion" class="Texto" />
							</td>
						</tr>
						<tr style="padding: 10px;">
							<td style="padding: 10px;">Documento</td>
							<td style="padding: 10px; text-align: right;">
								<input type="text" ng-model="Documento" class="Texto" />
							</td>
						</tr>
						<tr style="padding: 10px;">
							<td style="padding: 10px;">Prefijo</td>
							<td style="padding: 10px; text-align: right;">
								<input type="text" ng-model="Prefijo" class="Texto" />
							</td>
						</tr>
						<tr style="padding: 10px;">

							<td style="padding: 10px;"></td>
							<td style="padding: 10px; text-align: right;">
								<input type="button" class="BtnConsulta" value="Consultar" id="BtnConsultar" ng-click="ConsultarDocumento()" />
							</td>
						</tr>
					</table>
					<label id="mensajeError" style="color: red;"></label>
					<table style="padding: 10px; width: 100%; display: none;" id="tblPagos">
						<tr style="padding: 10px;">
							<td style="padding: 10px;">Documento</td>
							<td style="padding: 10px;">Prefijo</td>
							<td style="padding: 10px;">Monto</td>
							<td style="padding: 10px;"></td>
						</tr>
						<tr style="padding: 10px;" ng-repeat="x in lista_pagos">
							<td style="padding: 10px;">{{x.IntNumero}}</td>
							<td style="padding: 10px;">{{x.StrPrefijo}}</td>

							<td style="padding: 10px;">{{x.IntVlrTotal | number }}</td>
							<td style="padding: 10px; text-align: right;">
								<input type="button" class="BtnConsulta" value="Pagar" ng-click="PagarDocumento(x.StrIdSeguridad,x.IntVlrTotal)" /></td>
						</tr>
					</table>

				</div>

			</div>http://localhost:61437/../../obj
		</div>

	</div>
	*/