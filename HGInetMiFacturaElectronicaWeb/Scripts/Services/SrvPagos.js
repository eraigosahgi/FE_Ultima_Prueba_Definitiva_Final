//var ruta = 'http://localhost:61436/';
//var ruta_servicios = 'https://pruebascloudservices.hginet.co';

var ruta = 'https://portal.mifacturaenlinea.com.co/';
var ruta_servicios = 'https://cloudservices.hginet.co/';


var apphgi = angular.module('myApp', []);
apphgi.controller('myCtrl', function ($scope, SrvPagos) {
	$scope.serial = "";
	$scope.Identificacion = "";
	$scope.Documento = "";
	$scope.Prefijo = "";
	$scope.Monto = 0;
	$scope.IdSeguridad = '';
	$scope.EtiquetaIdentificacion = 'Identificación';
	$scope.EtiquetaDocumento = 'Documento Scope';
	$scope.PagosParciales = false;
	$scope.MuestraDocumento = true;
	$scope.PnlConsulta = true;

	$scope.lista_pagos = [];



	SrvPagos.Cargar().then(function (Datos) {
		$scope.PanelPagos = Datos;

	});

	$scope.ConsultarDocumento = function () {
		//Consultar($scope.Identificacion, $scope.Documento);
		ConsultarDocumentos();
	}
	//Sin Angularjs
	PagarDocumento = function (documento, monto) {
		$scope.Monto = $("#txtSaldo_" + documento).dxNumberBox("instance").option().value;
		InicicarPago(documento, monto);
	}
	//Con Angularjs
	$scope.PagarDocumento = function (documento, monto) {
		InicicarPago(documento, monto);
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

		var forma_pago = 0;
		var UsuarioSession = '';
		var alto_pantalla = $(window).height() - 10;
		var ancho_pantalla = $(window).width() - 10;

		var Vpago = window.open("", "Pagos", "top:10px, width=" + ancho_pantalla + "px,height=" + alto_pantalla + "px;");
		if (Vpago == null || Vpago == undefined) {
			DevExpress.ui.notify({ message: "Las ventanas emergentes estan bloqueadas, para realizar pagos, debe habilitarlas", position: { my: "center top", at: "center top" } }, "error", 6000);
		} else {
			//DevExpress.ui.notify({ message: "Se Inicia el pago", position: { my: "center top", at: "center top" } }, "success", 6000);
			if (idseg != null && idseg != undefined) {
				$scope.EnProceso = true;
				$('#btnpagar').hide();
				$.ajax({
					url: ruta + '/api/Documentos?strIdSeguridad=' + idseg + '&tipo_pago=0&registrar_pago=true&valor_pago=' + $scope.Monto + '&usuario=' + UsuarioSession + '&IntPagoFormaPago=' + forma_pago,
					success: function (response) {

						var alto_pantalla = $(window).height() - 10;
						var ancho_pantalla = $(window).width() - 10;

						//Inicializo la variable en uno(1) cuando guardo el pago ya que luego debo consultar unas tres veces al servidor
						$scope.NumVerificacion = 1;
						//Ruta servicio
						var RutaServicio = ruta_servicios + "/Views/Pago.aspx?IdSeguridad=";
						$scope.Idregistro = response.IdRegistro;
						//$("#modal_PagoEmbebido").modal("show");

						//$("#Pago_Embed").attr("src", RutaServicio + response.data.Ruta);
						var Vpago2 = window.open(RutaServicio + response.Ruta, "Pagos", "top:10px, width=" + ancho_pantalla + "px,height=" + alto_pantalla + "px;");

					}, error: function () {
						console.log("No se ha podido obtener la información");
						$('#btnpagar').show();
					}
				});
			}
		}

	}



	$scope.ConsultarDocumentoFueraCtrl = function (Identificacion, Documento) {
		//Consultar(Identificacion, Documento);
		$scope.Identificacion = Identificacion;
		$scope.Documento = Documento;
		ConsultarDocumentos();
	}






	function ConsultarDocumentos() {
		var ValidarGestionPagos = "ValidarGestionPagos";
		$("#summary_Pagos").dxValidationSummary(
		{
			validationGroup: ValidarGestionPagos
		});

		//Limpiar Errores
		$("#mensajeError").text("");

		$.ajax({
			url: ruta + '/api/ConsultarPagosFueraPlataforma?IdSeguridad=' + $scope.serial + '&identificacion=' + $scope.Identificacion + '&documento=' + $scope.Documento + '&prefijo=' + $scope.Prefijo,
			success: function (respuesta) {

				$('#wait').hide();
				$("#gridDocumentos").dxDataGrid({
					dataSource: respuesta,
					paging: {
						enabled: false,
					},
					keyExpr: "StrIdSeguridad",
					onContentReady: function (options) {
						try {
							if (options.component._options.dataSource.length == 1) {
								$("#gridDocumentos").dxDataGrid("columnOption", "Seleccionar", "visible", false);
							} else {
								$("#gridDocumentos").dxDataGrid("columnOption", "Seleccionar", "visible", true);
							}
						} catch (e) {
						}
					}
					, onCellPrepared: function (options) {
						var fieldData = options.value,
							fieldHtml = "";
						try {
							if (options.column.caption == "Valor Total" || options.column.caption == "SubTotal" || options.column.caption == "Neto") {
								if (fieldData) {
									var inicial = fNumber.go(fieldData).replace("$-", "-$");
									options.cellElement.html(inicial);
								}
							}
						} catch (err) {
							DevExpress.ui.notify(err.message, 'error', 3000);
						}
					}, loadPanel: {
						enabled: true
					},

					columns: [
				   {
				   	caption: "Seleccionar",
				   	cssClass: "col-md-1 col-xs-2",
				   	cellTemplate: function (container, options) {
				   		$('<div id="chkSaldo_' + options.data.StrIdSeguridad + '"></div>').dxCheckBox({
				   			name: "chkSaldo_" + options.data.StrIdSeguridad,
				   			onValueChanged: function (data) {
				   				validarSeleccion();
				   			}
				   		})
						.appendTo(container);
				   	},
				   },
					   {
					   	caption: "Prefijo",
					   	dataField: "StrPrefijo",
					   },
					   {
					   	caption: "Fecha Documento",
					   	dataField: "FechaDocumento",
					   },
					   {
					   	caption: $scope.EtiquetaDocumento,//"Documento",
					   	dataField: "IntNumero",
					   },

					   {
					   	caption: "Valor Total",
					   	dataField: "IntVlrTotal",
					   	Type: Number,
					   },

				   {
				   	caption: "Saldo",
				   	dataField: "Saldo",
				   	visible: true,
				   	//disabled: !data.habilitar_documento,
				   	cellTemplate: function (container, options) {

				   		$('<div id="txtSaldo_' + options.data.StrIdSeguridad + '"></div>').dxNumberBox({
				   			value: options.data.Saldo,
				   			name: "txtSaldo_" + options.data.StrIdSeguridad,
				   			disabled: (options.data.PagosParciales == 1) ? false : true,
				   			inputAttr: {
				   				id: "txtSaldo_cursor_" + options.data.StrIdSeguridad,
				   				style: "width: 100%; text-align:right;",
				   			},

				   			//tabIndex: Documento_Indice,
				   			format: {
				   				type: "fixedPoint",
				   				precision: 2
				   			},
				   			onValueChanged: function (data) {
				   				validarSeleccion();
				   			},
				   		})
						.dxValidator({
							validationGroup: ValidarGestionPagos,
							validationRules: [{
								type: "numeric",
								message: "El campo debe ser numerico"
							}, {
								type: "range",
								min: 1,
								max: options.data.Saldo,
								message: "Monto incorrecto"
							}]
						})
						.appendTo(container);
				   	},


				   },
					   {
					   	///Opción de pago
					   	cssClass: "col-md-1 ",
					   	caption: "Pago",
					   	width: "120px",
					   	alignment: "center",

					   	cellTemplate: function (container, options) {

					   		if (options.data.Saldo > 0) {
					   			if (options.data.Estado != 400) {
					   				var click = " onClick=PagarDocumento('" + options.data.StrIdSeguridad + "','" + options.data.Saldo + "')";
					   				var boton_pagar = '<div  id="btnpagar" ' + click + ' target="_blank" data-toggle="modal" data-target="#modal_Pagos_Electronicos" class="dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-icon dx-button-has-text" role="button" aria-label="Pagar" tabindex="10012"><div class="dx-button-content"><i class="dx-icon dx-icon-money"></i><span class="dx-button-text">Pagar</span></div></div>'
					   				var imagen = "";
					   				imagen = boton_pagar;
					   				$("<div>")
									.append($(imagen))
									 .appendTo(container);
					   			}
					   		} else {
					   			var click = " onClick=ConsultarDetallesPago(" + options.data.IdsPago + ")";
					   			var boton_pagar = '<div  ' + click + ' target="_blank" data-toggle="modal" data-target="#modal_Pagos_Electronicos" class="dx-button dx-button-default dx-button-mode-contained dx-widget dx-button-has-icon dx-button-has-text" role="button" aria-label="Ver" tabindex="10012"><div class="dx-button-content"><i class="dx-icon dx-icon-money"></i><span class="dx-button-text">Ver</span></div></div>'
					   			var imagen = "";
					   			imagen = boton_pagar;
					   			$("<div>")
								.append($(imagen))
								 .appendTo(container);
					   		}
					   	}
					   },

					],

				});
			}, error: function (error) {
				$("#mensajeError").text(error.responseText);
			}
		});


		ConsultarDetallesPago = function (IdRegistroPago, IdSeguridadDoc) {

			var ruta_redireccion = ruta_servicios + "/Views/DetallesPagoE.aspx?IdSeguridadPago=" + IdSeguridadDoc + "&IdSeguridadRegistro=" + IdRegistroPago;
			var alto_pantalla = $(window).height();
			var ancho_pantalla = $(window).width();

			var x = (screen.width / 2) - (ancho_pantalla / 2);
			//Ajustar verticalmente
			var y = (screen.height / 2) - (alto_pantalla / 2);
			window.open(ruta_redireccion, 'Pagos', 'left=' + x + ', top=' + y + '');
		};



		$("#multipagos").dxButton({
			text: "Pagar",
			type: "success",
			icon: 'money',
			visible: false,
			validationGroup: ValidarGestionPagos,
			onClick: function (e) {
				var result = e.validationGroup.validate();
				if (result.isValid) {
					var alto_pantalla = $(window).height() - 10;
					var ancho_pantalla = $(window).width() - 10;

					var Vpago = window.open("", "Pagos", "top:10px, width=" + ancho_pantalla + "px,height=" + alto_pantalla + "px;");
					if (Vpago == null || Vpago == undefined) {
						DevExpress.ui.notify({ message: "Las ventanas emergentes estan bloqueadas, para realizar pagos, debe habilitarlas", position: { my: "center top", at: "center top" } }, "error", 6000);
					} else {

						///Pago Multiple
						$.ajax({
							url: ruta + '/api/PagoMultiple?lista_documentos=' + $scope.documentos + '&valor_pago=' + $scope.total,
							success: function (respuesta) {



								//Inicializo la variable en uno(1) cuando guardo el pago ya que luego debo consultar unas tres veces al servidor
								$scope.NumVerificacion = 1;
								//Ruta servicio
								var RutaServicio = ruta_servicios + "/Views/Pago.aspx?IdSeguridad=";
								$scope.Idregistro = respuesta.IdRegistro;
								var Vpago2 = window.open(RutaServicio + respuesta.Ruta, "Pagos", "top:10px, width=" + ancho_pantalla + "px,height=" + alto_pantalla + "px;");



							},
							error: function (error) {
								if (error != undefined) {
									DevExpress.ui.notify(error.responseJSON.ExceptionMessage, 'error', 6000);
									$("#button").dxButton({ visible: true });


									if (error.responseJSON.ExceptionMessage == 'Documento ya no esta disponible') {
										$('#modal_Pagos_Electronicos').modal('hide')
									}

								}
							}
						});

					}
				}
			}
		});

	}

	function validarSeleccion() {
		var data = $("#gridDocumentos").dxDataGrid("instance").option().dataSource;
		var lista = '';
		var total_a_pagar = 0;
		var total_seleccionados = 0;
		var comercios_direfentes = false;

		var comercio_actual = "";
		for (var i = 0; i < data.length; i++) {

			var valor = $("#chkSaldo_" + data[i].StrIdSeguridad).dxCheckBox("instance").option().value;
			if (valor) {
				if (comercio_actual != "") {
					if (comercio_actual != data[i].IdComercio) {
						comercios_direfentes = true;
						swal({
							title: 'Comercios Diferentes',
							text: 'Para realizar pagos de multiples documentos, estos deben ser del mismo comercio',
							icon: 'error',
							confirmButtonColor: '#66BB6A',
							confirmButtonText: 'Aceptar',
							animation: 'pop',
							html: true,
						});
					}
				}
				total_seleccionados += 1;
				comercio_actual = data[i].IdComercio;
			}
		}

		if (total_seleccionados > 0) {
			$("#gridDocumentos").dxDataGrid("columnOption", "Pago", "visible", false);
			//$("#gridDocumentos").dxDataGrid("columnOption", "Saldo", "visible", true);


			for (var i = 0; i < data.length; i++) {

				var valor_seleccionado = 0;
				var seleccion = $("#chkSaldo_" + data[i].StrIdSeguridad).dxCheckBox("instance").option().value;
				if (seleccion) {
					lista += (lista) ? ',' : '';
					valor_seleccionado = $("#txtSaldo_" + data[i].StrIdSeguridad).dxNumberBox("instance").option().value;
					lista += "{Documento: '" + data[i].StrIdSeguridad + "',Valor: '" + valor_seleccionado.toString().replace(",", ".") + "'}";
					total_a_pagar += valor_seleccionado;
				}
			}
			lista = "[" + lista + "]"
			$scope.documentos = lista;
			$scope.total = total_a_pagar;

		} else {
			$scope.documentos = "Ningun Documento Por Procesar";
			$scope.total = 0;
			$("#gridDocumentos").dxDataGrid("columnOption", "Pago", "visible", true);
			//$("#gridDocumentos").dxDataGrid("columnOption", "Saldo", "visible", false);
			$("#multipagos").dxButton({ visible: false });
		}



		if (total_a_pagar > 0) {
			if (comercios_direfentes) {
				$('#Total_a_Pagar').text("");
				$("#multipagos").dxButton({ visible: false });
			} else {
				$('#Total_a_Pagar').text("Total: " + fNumber.go(total_a_pagar).replace("$-", "-$"));
				$("#multipagos").dxButton({ visible: true });
			}
		} else {
			$('#Total_a_Pagar').text("");
			$("#multipagos").dxButton({ visible: false });
		}

	}


});

function HgiConsultarPorIdentificacion(identificacion, documento) {
	angular.element('#myCtrl').scope().ConsultarDocumentoFueraCtrl(identificacion, documento);
}


apphgi.service('SrvPagos', function ($location, $q) {
	this.Cargar = function () {

		var panel_pago = '<link href="' + ruta + '/Content/dx.hgi.css" rel="stylesheet" />\
			<script src="'+ ruta + '/Scripts/config.js"></script>\
			<script src="https://cdn3.devexpress.com/jslib/17.2.7/js/dx.all.js"></script>\
			<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">\
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>\
			<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.common.css" />\
			<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />\
			<script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>\
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
					<table style="padding: 5px;" id="pnlConsulta" ng-show="PnlConsulta">\
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
							<td style="padding: 0px; text-align: right; width: 100px;" ><input type="text" style="width: 100%; text-align: right;  padding-right: 10px;"  id="txtvalorPago_{{x.StrIdSeguridad}}" ng-disabled="!PagosParciales" value="{{x.Saldo | number }}" class="Texto" /></td>\
							<td style="padding: 0px; text-align: right;">\
								<input type="button" class="BtnConsulta" value="Pagar" ng-show="x.Saldo>0" ng-click="PagarDocumento(x.StrIdSeguridad,x.IntVlrTotal)" /></td>\
						</tr>\
					</table>\
					<div class="col-md-12" style="padding: 5px; text-align: right;">\
					<label id="Total_a_Pagar" class="text-semibold text-right" style="font-size: medium; margin-right: 20px;"></label>\
					<div id="multipagos"></div>\
					</div>\
					<div id="gridDocumentos"></div>\
				</div>';


		var ventana_modal = '<div id="modal_detalles_pago" class="modal fade" style="display: none;">\
								<div class="modal-dialog modal-lg">\
									<div class="modal-content">\
										<div id="EncabezadoModal" class="modal-header">\
										<h5 style="margin-bottom: 10px;" class="modal-title">Detalles Pago Electrónico</h5>\
											<button type="button" class="close" data-dismiss="modal">×</button>\
										</div>\
										<div class="modal-body">\
											<div id="ContenidoDetallesPago">\
											</div>\
										</div>\
									</div>\
								</div>\
							</div>';

		//Impresión de campos
		return $.when().then(function () {
			return panel_pago + ventana_modal;
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


