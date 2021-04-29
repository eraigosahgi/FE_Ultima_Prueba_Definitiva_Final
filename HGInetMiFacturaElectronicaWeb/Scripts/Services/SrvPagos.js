var ruta = 'https://serv7.mifacturaenlinea.com.co/';
var ruta_servicios = 'https://cloudservices.hginet.co/Views/IniciarPago.aspx';

var apphgi = angular.module('myApp', []);
apphgi.controller('myCtrl', function ($scope, SrvPagos) {
	$scope.serial = "";
	$scope.Identificacion = "";
	$scope.Documento = "";
	$scope.Prefijo = "";
	$scope.Monto = 0;
	$scope.IdSeguridad = '';
	$scope.EtiquetaIdentificacion = 'Identificación';
	$scope.EtiquetaDocumento = 'Documento';
	$scope.PagosParciales = false;
	$scope.MuestraDocumento = true;
	$scope.PnlConsulta = true;

	$scope.lista_pagos = [];



	SrvPagos.Cargar().then(function (Datos) {
		$scope.PanelPagos = Datos;

	});

	$scope.ConsultarDocumento = function () {
		Consultar($scope.Identificacion, $scope.Documento);
	}


	$scope.PagarDocumento = function (documento, monto) {
		InicicarPago(documento, monto)
	}

	function Consultar(ValorIdentificacion, ValorDocumento) {

		$scope.lista_pagos = [];
		$('#tblPagos').hide();
		$.ajax({
			url: ruta + '/api/ConsultarPagosFueraPlataforma?IdSeguridad=' + $scope.serial + '&identificacion=' + ValorIdentificacion + '&documento=' + ValorDocumento + '&prefijo=' + $scope.Prefijo,
			success: function (respuesta) {

				$scope.lista_pagos = respuesta;
				$scope.PagosParciales = respuesta[0].PagosParciales;

				if (!$scope.$$phase) $scope.$apply();

				$('#tblPagos').show();
				$("#mensajeError").text("");

			},
			error: function (error) {
				$("#mensajeError").text(error.responseText);
			}
		});

	}


	function InicicarPago(idseg, valor_pago) {
	
		valor_pago = $("#txtvalorPago_" + idseg).val();
		var forma_pago = 0;
		var UsuarioSession = '';
		if (idseg != null && idseg != undefined) {
			$scope.EnProceso = true;

			$.ajax({
				url: ruta + '/api/Documentos?strIdSeguridad=' + idseg + '&tipo_pago=0&registrar_pago=true&valor_pago=' + $scope.Monto + '&usuario=' + UsuarioSession + '&IntPagoFormaPago=' + forma_pago,
				success: function (response) {



					var alto_pantalla = $(window).height() - 10;
					var ancho_pantalla = $(window).width() - 10;

					//Inicializo la variable en uno(1) cuando guardo el pago ya que luego debo consultar unas tres veces al servidor
					$scope.NumVerificacion = 1;
					//Ruta servicio
					var RutaServicio = ruta_servicios + "?IdSeguridad=";
					$scope.Idregistro = response.IdRegistro;
					//$("#modal_PagoEmbebido").modal("show");

					//$("#Pago_Embed").attr("src", RutaServicio + response.data.Ruta);
					var Vpago2 = window.open(RutaServicio + response.Ruta, "Pagos", "top:10px, width=" + ancho_pantalla + "px,height=" + alto_pantalla + "px;");


				}, error: function () {
					console.log("No se ha podido obtener la información");
				}
			});
		}

	}



	$scope.ConsultarDocumentoFueraCtrl = function (Identificacion, Documento) {
		Consultar(Identificacion, Documento);
	}


});

function HgiConsultarPorIdentificacion(identificacion, documento) {
	angular.element('#myCtrl').scope().ConsultarDocumentoFueraCtrl(identificacion, documento);
}


apphgi.service('SrvPagos', function ($location, $q) {
	this.Cargar = function () {

		var panel_pago = '<link href="' + ruta + '/Content/dx.hgi.css" rel="stylesheet" />\
			<script src="'+ ruta + '/Scripts/config.js"></script>\
			<div style="margin-top: 1%; min-width: 400px;">\
				<div class="panelPago" >\
					<div id="wait" style="display: none; z-index: 9999;">\
						<div class="modal" style="background-color: lightslategray; opacity: 0.6; display: block;"></div>\
						<div>\
							<img class="divImg" style="position: absolute; left: 43%; top: 50px; z-index: 9999; max-width: 25%; max-height: 25%;" src="../../Content/icons/Loading.gif" />\
						</div>\
					</div>\
					<h4>Pagos Electrónicos</h4>\
					<hr />\
					<table style="padding: 5px; width: 100%;" id="pnlConsulta" ng-show="PnlConsulta">\
						<tr style="padding: 5px;">\
							<td style="padding: 5px; text-align: right;">{{EtiquetaIdentificacion}}</td>\
							<td style="padding: 5px; text-align: right; width: 80px;">\
								<input type="text" id="txtIdentificacion" ng-model="Identificacion" class="Texto" />\
							</td>\
						</tr>\
						<tr style="padding: 5px;" ng-show="MuestraDocumento">\
							<td style="padding: 5px; text-align: right;">{{EtiquetaDocumento}}</td>\
							<td style="padding: 5px; text-align: right;">\
								<input type="text" id="txtDocumento" ng-model="Documento" class="Texto" />\
							</td>\
						</tr>\
						<tr style="padding: 5px;">\
							<td style="padding: 5px;"></td>\
							<td style="padding: 5px; text-align: right;">\
								<input type="button" class="BtnConsulta" value="Consultar" id="BtnConsultar" ng-click="ConsultarDocumento()" />\
							</td>\
						</tr>\
					</table>\
					<label id="mensajeError" style="color: red;"></label>\
					<table style="padding: 0px; width: 100%; display: none;" id="tblPagos">\
						<tr style="padding: 10px;">\
							<td style="padding: 10px; text-align: center;">Prefijo</td>\
							<td style="padding: 10px; text-align: center;">{{EtiquetaDocumento}}</td>\
							<td style="padding: 10px; text-align: center;">Fecha</td>\
							<td style="padding: 10px; text-align: right;">Valor</td>\
							<td style="padding: 10px; text-align: right;">Pago</td>\
							<td style="padding: 10px; width: 100px;"></td>\
						</tr>\
						<tr style="padding: 0px;" ng-repeat="x in lista_pagos">\
							<td style="padding: 0px; text-align: center;">{{x.StrPrefijo}}</td>\
							<td style="padding: 0px; text-align: center;">{{x.IntNumero}}</td>\
							<td style="padding: 0px; text-align: center;">{{x.FechaDocumento}}</td>\
							<td style="padding: 0px; text-align: right; width: 100px; padding-right: 10px; padding-left: 10px;">{{x.IntVlrTotal | number }}</td>\
							<td style="padding: 0px; text-align: right; width: 100px;" ><input type="text" style="width: 100%; text-align: right;  padding-right: 10px;"  id="txtvalorPago_{{x.StrIdSeguridad}}" ng-disabled="!PagosParciales" value="{{x.IntVlrTotal | number }}" class="Texto" /></td>\
							<td style="padding: 0px; text-align: right;">\
								<input type="button" class="BtnConsulta" value="Pagar" ng-click="PagarDocumento(x.StrIdSeguridad,x.IntVlrTotal)" /></td>\
						</tr>\
					</table>\
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


