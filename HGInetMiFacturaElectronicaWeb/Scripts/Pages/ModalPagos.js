//Controlador de Pagos
App.controller('ModalPagosController', function ($scope, $rootScope, $location, $http, $timeout) {

	$("#summary").dxValidationSummary({});
	$scope.valoraPendiente = 0;

	/////Parametros de la tabla
	$scope.fechadoc = "";
	$scope.documento = "";
	$scope.tipoDoc = "";
	$scope.email = "";
	$scope.telefono = "";
	$scope.nit = "";
	$scope.razonsocial = "";
	$scope.SinpagosPendiente = true;

	//Variable contadora para verificar pagos
	$scope.NumVerificacion = 1;

	//Esta variable es para marcarla como true si hay un pago pendiente y asi poder seguir consultando su estado
	$scope.pagoenVerificacion = false;

	//Este es el new Guid con el que se guarda el documento
	//Se utiliza aqui solo si se va a verificar el estado de un pago
	$scope.Idregistro = "";
	$scope.PermitePagosParciales = false;
	$rootScope.ConsultarPago = function (IdSeguridad, Monto, PagosParciales, poseeIdComercioPSE, poseeIdComercioTC) {
		$("#PanelPago").show();
		//$("#MontoPago").dxNumberBox({ readOnly: (PagosParciales == "1") ? false : true });
		$("#Detallepagos").hide();
		$("#panelPagoPendiente").show();
		$("#PanelVerificacion").hide();

		$scope.PermitePagosParciales = PagosParciales;

		$scope.pagoenVerificacion = false;



		///Id de seguridad del Documento        
		try {
			$scope.$apply(function () {
				$scope.IdSeguridad = IdSeguridad;
			});
		} catch (e) {

		}

		///Monto total cancelado hasta la fecha
		$scope.TotalPago = 0;

		///Valor a Cancelar
		$scope.valoraPagar = 0;

		$scope.EnProceso = false;


		

		
		//Consulto los pagos de este documento        
		$http.get('/api/ConsultarPagos?StrIdSeguridadDoc=' + IdSeguridad).then(function (response) {
			///Este es el monto total de la factura
			$scope.montoFactura = response.data[0].Monto;
			$scope.valoraPendiente = response.data[0].Monto;
			$scope.valoraPagar = response.data[0].Monto;
			$("#divValorPendiente").text(fNumber.go($scope.valoraPendiente));
			$("#MontoPago").dxNumberBox({ value: $scope.valoraPendiente });


			/////Parametros de la tabla
			$scope.fechadoc = response.data[0].FechaDocumento;
			$scope.documento = response.data[0].IntNumero;
			$scope.tipoDoc = response.data[0].DocTipo;
			$scope.email = response.data[0].Mail;//(Email != "null" && Email != "undefined") ? Email : "";
			$scope.telefono = response.data[0].Telefono;//(Telefono != "null" && Telefono != "undefined") ? Telefono : "";
			$scope.nit = response.data[0].NitFacturador;
			$scope.razonsocial = response.data[0].RazonSocialFacturador;

			
			$("#MontoPago").dxNumberBox({ readOnly: !response.data[0].PagosParciales });


			$("#button").dxButton({ visible: true });

			$("#grid").dxDataGrid({
				dataSource: response.data[0].Pagos,
				paging: {
					pageSize: 20
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				}
				//Formatos personalizados a las columnas en este caso para el monto
               , onCellPrepared: function (options) {
               	var fieldData = options.value,
					fieldHtml = "";
               	try {
               		if (options.column.caption == "Valor") {
               			if (fieldData) {
               				var inicial = fNumber.go(fieldData).replace("$-", "-$");
               				options.cellElement.html(inicial);
               			}
               		}

               		if (options.column.caption == "Estado") {
               			if (fieldData) {
               				if (fieldData != "Aprobado" && fieldData != "Rechazado" && fieldData != "No Iniciado") {
               					if (response.data[0].Pagos[0].Monto > 0) {
               						$("#panelPagoPendiente").hide();
               						$("#PanelVerificacion").show();
               						if ($scope.NumVerificacion > 1) {
               							$("#mensaje").text("El estado del pago aun se encuentra pendiente, por favor intente mas tarde");
               						}
               						$scope.Idregistro = options.data.IdRegistro;
               						$scope.pagoenVerificacion = true;
               						if ($scope.NumVerificacion <= 4) {
               							EsperayValida();
               						}
               					}

               				}
               			}
               		}

               	} catch (err) {
               		DevExpress.ui.notify(err.message, 'error', 3000);
               	}
               }, loadPanel: {
               	enabled: true
               }
               , allowColumnResizing: true
               , allowColumnReordering: true

               , columns: [
                   {
                   	caption: "Cód.Transacción",
                   	dataField: "StrIdSeguridadPago"

                   },
				   {
				   	caption: "Forma Pago",
				   	dataField: "Franquicia",
				   	width: '90px',
				   	alignment: "center",
				   	cellTemplate: function (container, options) {

				   		if (options.data.Franquicia != "") {
				   			var titulo = (options.data.Franquicia != "") ? "title = " + options.data.Franquicia : "";

				   			$('<img src="/Scripts/Images/' + options.data.Franquicia + '.png"  ' + titulo + ' />')
									.appendTo(container);
				   		}
				   	}
				   },
                   {
                   	caption: "Fecha Pago",
                   	dataField: "FechaRegistro",
                   	dataType: "date",
                   	format: "yyyy-MM-dd HH:mm"
                   },
                   {
                   	caption: "Valor",
                   	dataField: "Monto"
                   },
                   {
                   	caption: "Estado",
                   	dataField: "Estado"
                   },
                   {
                   	caption: "Fecha Verificación",
                   	dataField: "FechaVerificacion",
                   	dataType: "date",
                   	format: "yyyy-MM-dd HH:mm"
                   }
               ],

				summary: {
					totalItems: [
					{
						name: "MontoSum",
						showInColumn: "Monto",
						displayFormat: "$ {0}",
						//valueFormat: "$ #,##0.##",
						summaryType: "custom"
					}

					],
					calculateCustomSummary: function (options) {


						if (options.name === "MontoSum") {
							if (options.summaryProcess === "start") {
								options.totalValue = 0;
								$scope.TotalPago = 0;
							}
							if (options.summaryProcess === "calculate") {
								if (options.value.Estado == "Aprobado") {
									options.totalValue = options.totalValue + options.value.Monto;
									$scope.TotalPago = $scope.TotalPago + options.totalValue;
									///Monto pendiente por pagar
									$scope.valoraPendiente = $scope.montoFactura - options.totalValue;
									$("#MontoPago").dxNumberBox({ value: $scope.valoraPendiente });
									$("#divValorPendiente").text(fNumber.go($scope.valoraPendiente));
								}
								if ($scope.valoraPendiente == 0) {
									$("#button").dxButton({ visible: false });
									$("#summary").dxValidationSummary({ visible: false });
									$("#MontoPago").hide();
									$("#PanelPago").hide();
									$("#Pagar").hide();
								} else {
									$("#button").dxButton({ visible: true });
									$("#summary").dxValidationSummary({ visible: true });
									$("#PanelPago").show();
									$("#MontoPago").show();
									$("#Pagar").show();

								}
							}
						}
					}
				}
			});


			try {
				if (response.data[0].Pagos[0].Monto > 0) {
					$("#Detallepagos").show();
				} else {
					$("#panelPagoPendiente").show();
				}
			} catch (e) {

			}

			//-------------------------
			//Esta validación es solo la primera vez
			if ($scope.NumVerificacion == 1) {
				var Vpago = window.open("", "Pagos", "width=10,height=10");
				if (Vpago == null || Vpago == undefined) {
					$("#button").dxButton({ visible: false });
					DevExpress.ui.notify({ message: "Las ventanas emergentes estan bloqueadas, para realizar pagos, debe habilitarlas", position: { my: "center top", at: "center top" } }, "error", 6000);
				} else {
					$("#button").dxButton({
						text: "Pagar",
						type: "success",
						icon: 'money',
						//useSubmitBehavior: true
						onClick: function (e) {							
							InicicarPago(0);
							//console.log("poseeIdComercioPSE", poseeIdComercioPSE);
							//console.log("poseeIdComercioTC", poseeIdComercioTC);

							//if (poseeIdComercioPSE && poseeIdComercioTC) {

							//	$("#btnModalFormaPagoPSE").dxButton({
							//		text: "PSE",
							//		width: "300px",
							//		height: "50px",
							//		elementAttr: { title: "Pagar con PSE" },
							//		icon: "../../scripts/Images/LogoPSE90x90.png",
							//		onClick: function (e) {
							//			$("#modal_FormasDePago").modal("hide");
							//			InicicarPago(29);
							//		}
							//	});

							//	$("#btnModalFormaPagoTDC").dxButton({
							//		text: "Tarjeta de Crédito",
							//		icon: "../../scripts/Images/TarjetasdeCredito.png",
							//		width: "300px",
							//		height: "50px",
							//		elementAttr: { title: "Pagar con Tarjeta de Crédito" },
							//		onClick: function (e) {
							//			$("#modal_FormasDePago").modal("hide");
							//			InicicarPago(31);
							//		},
							//		onContentReady: function (e) {
							//			$("#btnModalFormaPagoTDC img").attr("style", "width:auto;");
							//		}
							//	});

							//	$("#modal_FormasDePago").modal("show");
							//} else {
							//	if (poseeIdComercioPSE) {
							//		InicicarPago(29);
							//	} else {
							//		if (poseeIdComercioTC) {
							//			InicicarPago(31);
							//		}
							//	}
							//}
						}
					});

				}
				Vpago.close();

				try {
					//Limpiamos la seleccion de mas de un doumento en la grid de pagos
					var dataGrid = $("#gridDocumentos").dxDataGrid("instance");
					dataGrid.deselectAll();
					dataGrid.clearSelection();
				} catch (e) {

				}
			}
			//-------------------------

		}), function errorCallback(response) {
			Mensaje(response.data.ExceptionMessage, "error");
		};


		//Valido el monto Total menos el monto pagado

		$("#MontoPago").dxNumberBox({
			format: "$ #,##0.##",
			validationGroup: "ValidarPago",
			onValueChanged: function (data) {
				$scope.valoraPagar = data.value;
			}
		})
		.dxValidator({
			validationRules: [{
				type: "required",
				message: "Debe Indicar el monto a pagar"
			},
			{
				type: 'custom', validationCallback: function (options) {
					if (validar()) {
						options.rule.message = "El monto a pagar no puede ser mayor al monto pendiente";
						return false;
					} else { return true; }
				}
			},
			{
				type: 'custom', validationCallback: function (options) {
					if (validarMayorACero()) {
						options.rule.message = "El monto de ser mayor a cero(0)";
						return false;
					} else { return true; }
				}
			}


			, {
				type: 'pattern',
				pattern: '^[0-9-.]+$',
				message: 'No debe Incluir puntos(.) ni caracteres especiales'
			}
			, {
				type: "numeric",
				message: "El monto a pagar debe ser numérico"
			}]
		});
		//Valida que el Monto a Pagar no sea Mayor al monto pendiente
		function validar() {
			if ($scope.valoraPagar > $scope.valoraPendiente) {
				return true;
			}
			return false;
		}

		//Valida que el Monto a Pagar no sea Mayor al monto pendiente
		function validarMayorACero() {
			if ($scope.valoraPagar < 1) {
				return true;
			}
			return false;
		}




		$("#PagoTotal").dxCheckBox({
			name: "chkpagoTotal",
			text: "",
			value: false,
			onValueChanged: function (data) {
				if (data.value) {
					$("#MontoPago").dxNumberBox({ value: $scope.valoraPendiente });
				} else {
					$("#MontoPago").dxNumberBox({ value: "" });
				}

			}
		})
	}


	$scope.buttonProcesar = {
		text: 'Pagar',
		type: "success",
		onClick: function (e) {
			DevExpress.ui.notify("Aun no ha terminado el proceso actual", 'error', 3000);
		}
	};


	$scope.buttonVerificar = {
		text: 'Verificar',
		type: "success",
		onClick: function (e) {
			VerificarEstado();
		}
	}


	$("#form").on("submit", function (e) {
		

		//e.preventDefault();
	});


	function InicicarPago(forma_pago) {

		var idseg = $scope.IdSeguridad;
		var valor_pago = $scope.valoraPagar;
		if (idseg != null && idseg != undefined) {
			$scope.EnProceso = true;
			$("#button").dxButton({ visible: false });
			$http.get('/api/Documentos?strIdSeguridad=' + idseg + '&tipo_pago=0&registrar_pago=true&valor_pago=' + valor_pago + '&usuario=' + UsuarioSession + '&IntPagoFormaPago=' + forma_pago).then(function (response) {

				var alto_pantalla = $(window).height() - 10;
				var ancho_pantalla = $(window).width() - 10;


				//Inicializo la variable en uno(1) cuando guardo el pago ya que luego debo consultar unas tres veces al servidor
				$scope.NumVerificacion = 1;
				//Ruta servicio
				var RutaServicio = $('#Hdf_RutaPagos').val() + "?IdSeguridad=";
				$scope.Idregistro = response.data.IdRegistro;

				//$("#modal_PagoEmbebido").modal("show");

				//$("#Pago_Embed").attr("src", RutaServicio + response.data.Ruta);
				var Vpago2 = window.open(RutaServicio + response.data.Ruta, "Pagos", "top:10px, width=" + ancho_pantalla + "px,height=" + alto_pantalla + "px;");
				$timeout(function callAtTimeout() {
					VerificarEstado();
				}, 90000);



			}, function (error) {

				if (error != undefined) {
					DevExpress.ui.notify(error.data.ExceptionMessage, 'error', 6000);
					$("#button").dxButton({ visible: true });
					$scope.EnProceso = false;

					if (error.data.ExceptionMessage == 'Documento ya no esta disponible') {
						$('#modal_Pagos_Electronicos').modal('hide')
					}

				}

				$scope.$apply(function () {
					$scope.EnProceso = false;
				});


			});
		}
	}




	function EsperayValida() {
		///Se va a ejecutar automaticamente la sonda de consulta mientras el pago este pendiente
		//if ($scope.pagoenVerificacion) {
		//	$timeout(function callAtTimeout() {
		//		VerificarEstado();
		//	}, 90000);
		//}
	}


	function VerificarEstado() {

		var RutaServicio = $('#Hdf_RutaSrvPagos').val();

		// $http.get(RutaServicio + '?IdSeguridadPago=' + $scope.IdSeguridad + "&StrIdSeguridadRegistro=" + $scope.Idregistro).then(function (response) {
		//Esto retorna un objeto  de la plataforma intermedia que sirve para actualizar el pago local
		//   var ObjeRespuestaPI = response.data;
		//////////////////////////////////////////////////////////////////////
		//$http.get('/Api/ActualizarEstado?IdSeguridad=' + $scope.IdSeguridad + '&StrIdSeguridadRegistro=' + $scope.Idregistro + '&Pago=' + ObjeRespuestaPI + "'").then(function (response) {
		$http.get('/Api/ActualizarEstado?IdSeguridad=' + $scope.IdSeguridad + '&StrIdSeguridadRegistro=' + $scope.Idregistro).then(function (response) {
			$('#wait').hide();

			//Incremento la variable consulta para llegar a un maximo de tres consultar al servicio de zona virtual
			$scope.NumVerificacion = $scope.NumVerificacion + 1;
			
			$rootScope.ConsultarPago($scope.IdSeguridad, $scope.montoFactura, $scope.PermitePagosParciales);

			


			$scope.EnProceso = false;

			/////////////////////////////////////////////////////////////////////////////////////////

		}), function (response) {

			$scope.EnProceso = false;
			Mensaje(response.data.ExceptionMessage, "error");
		};
		/*
		}), function (response) {
	
			$scope.EnProceso = false;
			Mensaje(response.data.ExceptionMessage, "error");
		}
		*/

	}

});