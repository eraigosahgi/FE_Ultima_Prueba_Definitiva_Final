
DevExpress.localization.locale(navigator.language);

var AcuseReciboApp = angular.module('AcuseReciboApp', ['dx']);
AcuseReciboApp.controller('AcuseReciboController', function AcuseReciboController($scope, $http, $timeout) {


	$(document).ready(function () {

		var IdSeguridad = location.search.split('id_seguridad=')[1];
		//Almacena el parametro Zpago para ver si debe enviarlo a la pantalla de pago
		var Zpago = location.search.split('Zpago=')[1];
		$scope.DetalleAcuse = true;
		var estado = "";
		var motivo_rechazo = "";
		//Id unico de registro de pago
		$scope.Idregistro;
		$scope.IdSeguridad = IdSeguridad;

		//Valor pendiente del documento.
		$scope.ValorPagoDoc = 0;
		//Valor ingresado por el usuario.
		$scope.VlrTxtValorPago = 0;

		//Indica si se esta verificando un pago
		$scope.EnProceso = false;


		$scope.habilitar = function () {
			//-------------------------------------------------------------------
			var id = IdSeguridad.split('&')[0];
			$http.get('/api/ConsultaSaldoDocumento?StrIdSeguridadDoc=' + id).then(function (response) {
				if (response.data != "PagoPendiente" && response.data != "DocumentoCancelado" && response.data != "ErrorDian") {
					var Vpago1 = window.open("", "Pagos", "width=10,height=10");
					if (Vpago1 == null || Vpago1 == undefined) {
						DevExpress.ui.notify({ message: "Las ventanas emergentes estan bloqueadas, para realizar pagos, debe habilitarlas", position: { my: "center top", at: "center top" } }, "error", 6000);

					} else {
						//--------------------------					
						$("#summary").dxValidationSummary({});

						$scope.ValorPagoDoc = parseInt(response.data);
						$scope.VlrTxtValorPago = $scope.ValorPagoDoc;

						//Campo valor del pago de modal_inicio_pago
						$("#TxtValorPago").dxNumberBox({
							value: $scope.ValorPagoDoc,
							format: "$ #,##0.##",
							validationGroup: "ValidarPago",
							onValueChanged: function (data) {
								$scope.VlrTxtValorPago = data.value;
							}
						}).dxValidator({
							validationRules: [{
								type: "required",
								message: "Debe Indicar el monto a pagar"
							},
							{
								type: 'custom', validationCallback: function (options) {
									if ($scope.VlrTxtValorPago > $scope.ValorPagoDoc) {
										options.rule.message = "El monto a pagar no puede ser mayor al monto pendiente.";
										return false;
									} else { return true; }
								}
							},
							{
								type: 'custom', validationCallback: function (options) {

									if ($scope.VlrTxtValorPago < 1) {
										options.rule.message = "El monto de ser mayor a cero (0).";
										return false;
									} else
										return true;
								}
							}, {
								type: 'pattern',
								pattern: '^[0-9-.]+$',
								message: 'No debe Incluir puntos(.) ni caracteres especiales.'
							}
							, {
								type: "numeric",
								message: "El monto a pagar debe ser numérico."
							}]
						});


						var datos_doc = $scope.RespuestaAcuse;

						//Carga información del documento
						$scope.tipodoc = datos_doc[0].tipodoc;
						$scope.NumeroDocumento = datos_doc[0].NumeroDocumento;
						$scope.FechaDocumento = datos_doc[0].FechaDocumento;
						$scope.ValorDoc = datos_doc[0].ValorDoc;

						//Valida si se permiten pagos parciales, según el número de resolución del documento.
						if (datos_doc[0].PermiteParciales) {
							$scope.CampoValorPago = true;
							$scope.InfoValorPago = false;
						}
						else {
							$scope.InfoValorPago = true;
							$scope.CampoValorPago = false;
							$scope.ValorPago = $scope.ValorPagoDoc;
						}


						//Botón continuar de modal_inicio_pago
						$("#BtnContinuarPago").dxButton({
							text: "continuar",
							type: "default",
							useSubmitBehavior: true,
						});

						$("#formInicioPago").on("submit", function (e) {

							$http.get('/api/Documentos?strIdSeguridad=' + id + '&tipo_pago = 0 &registrar_pago=true&valor_pago=' + $scope.VlrTxtValorPago).then(function (response) {

								var RutaServicio = $('#Hdf_RutaPagos').val() + "?IdSeguridad=";

								window.open(RutaServicio + response.data.Ruta, "_blank");

								$('#cmdpago').hide();
								VerificarEstado();
								//Si lo envia a la pantalla de pago, cierra la pantalla actual
								//if (Zpago)
								// window.close();
								$("#modal_inicio_pago").modal('hide');
							}, function (error) {
								DevExpress.ui.notify("Problemas con la plataforma de pago", 'error', 7000);
								$scope.DetalleAcuse = false;
							});

							e.preventDefault();

						});




						//Abre modal
						$("#modal_inicio_pago").modal('show');
						//-------------------------
					}

				} else {
					if (response.data == "PagoPendiente") {
						DevExpress.ui.notify("No puede hacer pagos mientras tenga pagos pendientes", 'error', 7000);
					}
					if (response.data == "DocumentoCancelado") {
						DevExpress.ui.notify("Este documento ya fue pagado", 'error', 7000);
					}

					if (response.data == "ErrorDian") {
						DevExpress.ui.notify("El estado actual del documento, no permite hacer pagos", 'error', 7000);
					}


				}
			}, function (error) {
				$scope.DetalleAcuse = false;
				DevExpress.ui.notify("Problemas con la plataforma de pago", 'error', 7000);

			});
			//----------------------------------------------------------------

		};

		//Realiza la redirección para dar inicio al pago.
		function RedireccionPago(id_seguridad, valor_pago) {
			$http.get('/api/Documentos?strIdSeguridad=' + id_seguridad + '&tipo_pago = 0 &registrar_pago=true&valor_pago=' + valor_pago).then(function (response) {

				var RutaServicio = $('#Hdf_RutaPagos').val() + "?IdSeguridad=";

				window.open(RutaServicio + response.data.Ruta, "_blank");
				$('#cmdpago').hide();
				VerificarEstado();
				//Si lo envia a la pantalla de pago, cierra la pantalla actual
				//if (Zpago)
				// window.close();
				$("#modal_inicio_pago").modal('hide');
			}, function (error) {
				DevExpress.ui.notify("Problemas con la plataforma de pago", 'error', 7000);
				$scope.DetalleAcuse = false;
			});
		}

		function consultar() {
			//Obtiene los datos del web api
			//ControladorApi: /Api/Documentos/
			//Datos GET: id_seguridad
			$http.get('/api/ConsultarAcuse?id_seguridad=' + IdSeguridad + '&usuario=' + $scope.Usuario).then(function (response) {
				$scope.RespuestaAcuse = response.data;

				$('#plugin').attr('src', $scope.RespuestaAcuse[0].Pdf);

				//Si estatus es igual a 2, entonces asigno los valores a las variables para ejecutar la consulta de saldo
				if (response.data[0].Estatus == 2) {
					$scope.Idregistro = response.data[0].pago[0].StrIdRegistro
					VerificarEstado();
				}

				VerHistorialPagos();

			});


			$scope.TextAreaObservaciones = {

				readOnly: false,
				showColonAfterLabel: true,
				showValidationSummary: true,
				validationGroup: "ObservacionesAcuse",
				onInitialized: function (e) {
					formInstance = e.component;
				},
				items: [{
					itemType: "group",
					items: [
						{
							dataField: "Observaciones",
							editorType: "dxTextArea",
							label: {
								text: "Observaciones"
							},
							validationRules: [{
								type: "required",
								message: "Las observaciones son requeridas."
							}]
						}
					]
				}]
			};

		}
		//ValidarEstado(1);
		$scope.ValidarEstado = function (Estado) {
			if (Estado == 1) {
				$scope.AceptarVar = true;
				$scope.RechazarVar = false;
			} else {
				$scope.AceptarVar = false;
				$scope.RechazarVar = true;
			}
		}

		//Botón Rechazar.
		$scope.ButtonOptionsAceptar = {
			text: "Enviar",
			type: "success",
			onClick: function (e) {
				estado = 1;
				motivo_rechazo = $('textarea[name=Observaciones]').val();
				ActualizarDatos();
			}
		};

		//Botón Aceptar.
		$scope.ButtonOptionsRechazar = {
			text: "Enviar",
			type: "success",
			validationGroup: "ObservacionesAcuse",
			useSubmitBehavior: true,
		};

		$scope.onFormSubmit = function (e) {
			motivo_rechazo = $('textarea[name=Observaciones]').val();
			estado = 2;
			ActualizarDatos();
		}


		//Función para actualizar los datos
		function ActualizarDatos() {


			var id = IdSeguridad.split('&')[0];

			var data = $.param({
				id_seguridad: id,
				estado: estado,
				motivo_rechazo: motivo_rechazo,
				usuario: $scope.Usuario
			});


			$('#wait').show();

			$http.post('/api/Documentos?' + data).then(function (data, response) {
				$scope.ServerResponse = data;
				consultar();
				var id = id;
				$('#wait').hide();
			}, function errorCallback(response) {
				$('#wait').hide();
			});
		}

		$scope.Usuario = "";
		$('#btnautenticar').show();
		$http.get('/api/SesionDatosUsuario/').then(function (response) {
			$scope.Usuario = response.data[0].IdSeguridad;
			consultar();
			$('#btnautenticar').hide();

		}, function errorCallback(response) {
			$scope.Usuario = "";
			consultar();
			$('#btnautenticar').show();
		});



		////Verificación de pago pendiente
		function VerificarEstado() {
			$scope.EnProceso = true;
			$scope.buttonProcesar = {
				type: "success",
				onClick: function (e) {
					DevExpress.ui.notify("Aun no ha terminado el proceso actual", 'error', 3000);
				}
			};

			$timeout(function callAtTimeout() {
				if ($scope.Idregistro != undefined) {
					$http.get('/Api/ActualizarEstado?IdSeguridad=' + $scope.IdSeguridad + "&StrIdSeguridadRegistro=" + $scope.Idregistro).then(function (response) {
						$scope.EnProceso = false;
						consultar();
					}), function (response) {
						$scope.EnProceso = false;
						Mensaje(response.data.ExceptionMessage, "error");
					};
				} else {
					$scope.EnProceso = false;
					consultar();
				}
			}, 9000);

		}

		if (Zpago == 'true') {
			//Si parametro Zpago = true entonces lo envia a la pantalla de pago
			//-------------------------
			var Vpago = window.open("", "Pagos", "width=10,height=10");
			if (Vpago == null || Vpago == undefined) {
				// $("#button").dxButton({ visible: false });
				DevExpress.ui.notify({ message: "Las ventanas emergentes estan bloqueadas, para realizar pagos, debe habilitarlas", position: { my: "center top", at: "center top" } }, "error", 6000);

				$timeout(function callAtTimeout() {
					$('#cmdpago').hide();
				}, 1000);
			} else {
				$scope.PanelInformacion = false;
				$scope.habilitar();
			}

		}


		//Carga el historial de pagos del documento.
		function VerHistorialPagos() {

			$http.get('/Api/ConsultarPagos?StrIdSeguridadDoc=' + IdSeguridad).then(function (response) {

				if (response.data[0].Pagos.length == 0) {
					$scope.BtnVerPagos = false;
				} else {

					$scope.BtnVerPagos = true;
					$("#gridPagosDocumento").dxDataGrid({
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
				   	} catch (err) {
				   		DevExpress.ui.notify(err.message, 'error', 3000);
				   	}
				   }, loadPanel: {
				   	enabled: true
				   },
						allowColumnResizing: true,
						allowColumnReordering: true,
						columns: [
							{
								caption: "Fecha Pago",
								dataField: "FechaRegistro",
								dataType: "date",
								format: "yyyy-MM-dd HH:mm"
							},
							{
								caption: "Cód.Transacción",
								dataField: "StrIdSeguridadPago"

							},
							{
								caption: "Valor",
								dataField: "Monto"
							},
							{
								caption: "Estado",
								dataField: "Estado"
							}
						],
						masterDetail: {
							enabled: true,
							template: function (container, options) {
								var ruta_redireccion = $('#Hdf_RutaPlataformaServicios').val() + "Views/DetallesPagoE.aspx?IdSeguridadPago=" + IdSeguridad + "&IdSeguridadRegistro=" + options.data.IdRegistro;
								$("<div>").append($('<object data="' + ruta_redireccion + '" style="width: 100%; height: 600px" />')).appendTo(container);
							}
						}
					});
				}

			}, function (response) {
				DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				return $q.reject(response.data);
			});

		}

		//Abre la modal de los pagos realizados.
		$scope.ConsultarDetallesPago = function () {
			$("#modal_pagos_documento").modal('show');
		}

	});



});


