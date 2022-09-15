
var id_seguridad;
var numero_documento;
var obligado;
var idEndosatario;

var GrupoValidarEndoso = "GrupoValidarEndoso";
var formRegistrarEmpresa = "formRegistrarEmpresa";

App.controller('EventosRadianController', function EventosRadianController($scope, $http, $location, $rootScope) {

	$rootScope.ConsultarEventosRadian = function (IdSeguridad, NumeroDocumento, Obligado, idEndosatario) {
		$scope.IdSeguridad = IdSeguridad;
		id_seguridad = IdSeguridad;
		$("#IdSeguridad").text(IdSeguridad);
		$scope.NumeroDocumento = $("#NumeroDocumento").text(NumeroDocumento);
		numero_documento = NumeroDocumento;
		$scope.Obligado = $("#Obligado").text(Obligado);

		obligado = Obligado;

		$('#panelEndoso').hide();
		$("#tiposEndoso").dxSelectBox({ value: '' });
		$("#tiposOperacionEvento").dxSelectBox({ value: '' });
		$("#idEndosatario").dxTextBox({ value: '' });
		$("#tasaDescuentoEndoso").dxNumberBox({ value: '' });
		$("#razonSocialEmpresa").text('');

		$http.get('/api/ObtenerEventosRadian?id_seguridad=' + IdSeguridad).then(function (response) {

			$("#GridEventosRadianDocumento").dxDataGrid({
				dataSource: response.data,
				allowColumnResizing: true,
				paging: {
					pageSize: 10
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				},
				onContentReady: function (e) {
					try {
						$("#cmdenviar").dxButton({ visible: response.data[0].Inscribir_Documento });
						$("#cmdenviar1").dxButton({ visible: response.data[0].otros_eventos });
						$("#cmdenviar2").dxButton({ visible: response.data[0].otros_eventos });
						$("#cmdenviar3").dxButton({ visible: response.data[0].otros_eventos });
						$("#cmdenviar4").dxButton({ visible: response.data[0].otros_eventos });
						$("#cmdenviar5").dxButton({ visible: response.data[0].otros_eventos });

					}
					catch (err) {
						$("#cmdenviar").dxButton({ visible: false });
						$("#cmdenviar1").dxButton({ visible: false });
						$("#cmdenviar2").dxButton({ visible: false });
						$("#cmdenviar3").dxButton({ visible: false });
						$("#cmdenviar4").dxButton({ visible: false });
						$("#cmdenviar5").dxButton({ visible: false });
					}
				},
				columns: [
				    {
				    	caption: "Fecha",
				    	dataField: "DatFechaEvento",
				    	cssClass: "col-md-2",
				    	dataType: "date",
				    	format: "yyyy-MM-dd HH:mm:ss",
				    }, {
				    	caption: "Estado",
				    	dataField: "EstadoEvento",
				    	cssClass: "col-md-4",
				    }, {
				    	caption: "Numero Evento",
				    	dataField: "IntNumeroEvento",
				    	cssClass: "col-md-1",
				    },
				   {
				   	caption: "StrUrlEvento",
				   	dataField: "StrUrlEvento",
				   	visible: false
				   },
				    {
				    	caption: "respuesta_evento",
				    	dataField: "respuesta_evento",
				    	visible: false
				    },
				   {
				   	caption: "Archivo",
				   	cssClass: "col-md-1",
				   	cellTemplate: function (container, options) {
				   		var visible_xml = "href='" + options.data.StrUrlEvento + "' class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
				   		$("<div>")
							.append(
							   $("<a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + ">"))
							.appendTo(container);
				   	}
				   },
				   {
				   	caption: "Respuesta",
				   	cssClass: "col-md-1",
				   	cellTemplate: function (container, options) {
				   		var visible_xml_resp = "href='" + options.data.respuesta_evento + "' class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
				   		$("<div>")
							.append(
							   $("<a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml_resp + ">"))
							.appendTo(container);
				   	}
				   },
				]

			});





			$("#cmdenviar").dxButton({
				text: "Inscripción TV",
				type: "default",
				visible: false,
				onClick: function () {
					$('#wait2').show();
					$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
						//alert(response.data);
						$('#wait2').show();
						$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
						$('#wait2').hide();
					}, function errorCallback(response) {
						$('#wait2').hide();
						//Carga notificación de creación con opción de editar formato.
						var myDialog = DevExpress.ui.dialog.custom({
							title: "Proceso Falló",
							message: response.data.ExceptionMessage,
							buttons: [{
								text: "Aceptar",
								onClick: function (e) {
									myDialog.hide();
									$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
								}
							}]
						});
						myDialog.show().done(function (dialogResult) {
						});

						$('#wait2').hide();
					});
				}
			});

			// Función para mostrar el panel Tipo Endoso
			$("#cmdenviar1").dxButton({
				text: "Endoso",
				type: "default",
				visible: false,
				onClick: function () {
					$("#panelEndoso").toggle();
				}
			});

			$("#cmdenviar2").dxButton({
				text: "Aval",
				type: "default",
				visible: false,
				onClick: function () {

					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar3").dxButton({
				text: "Mandato",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar4").dxButton({
				text: "Limitación",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar5").dxButton({
				text: "Transferencia Derecho",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar6").dxButton({
				text: "Informe Pago",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar7").dxButton({
				text: "Pago",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});




		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}

	function MensajeEventoRadian() {
		DevExpress.ui.notify("Evento no disponible temporalmente", 'error', 3000);
	}

	// Cargar datos filtro Tipo Endoso
	$("#tiposEndoso").dxSelectBox({
		placeholder: "Seleccionar...",
		displayExpr: "Texto",
		dataSource: tiposEndoso,
		onValueChanged: function (data) {
			tiposEndoso = data.value.ID;
			console.log(tiposEndoso);

			// Activar botón Enviar si todos los campos están llenos
			var camposLLenos = validarCamposFiltrosEndoso();
			console.log(camposLLenos);
			if (camposLLenos === true) {
				// Activar botón de Enviar
				$('#btnRealizarEndoso').dxButton({
					disabled: false,
					elementAttr: {
						title: "Realizar Endoso.",
						style: "cursor: pointer; pointer-events: initial;",
					},
				});
			}

		},
		elementAttr: {
			id: "tiposEndoso",
			title: "Elige el tipo de endoso",
			//  class: "class-name"
		}
	}).dxValidator({
		validationGroup: GrupoValidarEndoso,
		validationRules: [{
			type: 'required',
			message: 'Campo requerido',
		}],
	});

	// Cargar datos filtro Tipo Operación Endoso
	$("#tiposOperacionEvento").dxSelectBox({
		placeholder: "Seleccionar...",
		displayExpr: "Texto",
		dataSource: tiposOperacionEvento,
		onValueChanged: function (data) {
			tiposOperacionEvento = data.value.ID;
			console.log(tiposOperacionEvento);

			// Activar botón Enviar si todos los campos están llenos
			var camposLLenos = validarCamposFiltrosEndoso();
			console.log(camposLLenos);
			if (camposLLenos === true) {
				// Activar botón de Enviar
				$('#btnRealizarEndoso').dxButton({
					disabled: false,
					elementAttr: {
						title: "Realizar Endoso.",
						style: "cursor: pointer; pointer-events: initial;",
					},
				});
			}
		},
		elementAttr: {
			id: "tiposOperacionEvento",
			title: "Elige el tipo de operación del evento",
			//    class: "class-name"
		}
	}).dxValidator({
		validationGroup: GrupoValidarEndoso,
		validationRules: [{
			type: 'required',
			message: 'Campo requerido',
		}],
	});

	// Fltro Tasa de Descuento Endoso
	$("#tasaDescuentoEndoso").dxNumberBox({
		placeholder: "0.0%",
		format: '#0.00%',
		value: null,
		min: 0.01,
		step: 0.01,
		//  max: 10,
		elementAttr: {
			id: "tasaDescuentoEndoso",
			title: "Ingrese el valor del descuento. Permite decimales. Ej: 7.9",
			// class: "class-name"
		},
		onValueChanged: function (data) {
			tasaDescuentoEndoso = data.value;
			tasaDescuentoEndoso = tasaDescuentoEndoso * 100;
			console.log(tasaDescuentoEndoso);

			// Activar botón Enviar si todos los campos están llenos
			var camposLLenos = validarCamposFiltrosEndoso();
			console.log(camposLLenos);
			if (camposLLenos === true) {
				// Activar botón de Enviar
				$('#btnRealizarEndoso').dxButton({
					disabled: false,
					elementAttr: {
						title: "Realizar Endoso.",
						style: "cursor: pointer; pointer-events: initial;",
					},
				});
			}
		},

	}).dxValidator({
		validationGroup: GrupoValidarEndoso,
		validationRules: [{
			type: 'required',
			message: 'Campo requerido',
		}],
	});

	// Campo ID Endosatario
	$("#idEndosatario").dxTextBox({
		placeholder: "Ingresar ID Endosatario",
		validationGroup: GrupoValidarEndoso,
		elementAttr: {
			id: "idEndosatario",
			title: "Ingrese el NIT de la empresa que hará de Endosatario",
			//     class: "class-name"
		},
		onValueChanged: function (data) {
			idEndosatario = data.value;
			//console.log(idEndosatario);

			// Validar que el campo ID Endosatario esté lleno.
			if (idEndosatario === '') {
				//console.log("No se pudo consultar la empresa porque el campo ID Endotatario está vacío.");

				// Mostrar mensaje
			    $("#razonSocialEmpresa").text("No se pudo consultar la empresa porque el campo ID Endotatario está vacío.");

			    // Ocultar panel para registrar empresa
			    $('#panelCrearEmpresa').css('display', 'none');

				// Ajustar botón de Enviar
				$('#btnRealizarEndoso').dxButton({
					disabled: true,
					elementAttr: {
						title: "Para realizar el proceso de Endoso primero debe de validar si la empresa existe rellenando el campo ID Endosatario.",
						style: "cursor: not-allowed; pointer-events: initial;",
					},
				});

				return;
			}

			// Consultar empresa
			if (!idEndosatario || idEndosatario.trim().length >= 6) {
				$http.get('/api/ObtenerOperador?identificacion=' + idEndosatario).then(function (response) {

					// Validar si la empresa existe
					TipoOperador = response["data"]["TipoOperador"];            // Obtener tipo de operador
					if (TipoOperador != 0) {
					//	console.log("La empresa con NIT " + idEndosatario + " existe. Realizar Endoso...");

						// Mostrar razón social empresa
						razonSocialEmpresa = response["data"]["RazonSocial"];  // Obtener razón social de la empresa
						$("#razonSocialEmpresa").text("Razón social empresa: " + razonSocialEmpresa);

					    // Ocultar panel para registrar empresa
						$('#panelCrearEmpresa').css('display', 'none');

						// Activar botón Enviar si todos los campos están llenos
						var camposLLenos = validarCamposFiltrosEndoso();
						console.log(camposLLenos);
						if (camposLLenos === true) {
							// Activar botón de Enviar
						    $('#btnRealizarEndoso').dxButton({
						        text: "Realizar Endoso",
						        type: "default",
								disabled: false,
								elementAttr: {
									title: "Realizar Endoso",
									style: "cursor: pointer; pointer-events: initial;",
								},
							});
						}
					} else {
					//	console.log("La empresa con NIT " + idEndosatario + " existe pero no puede realizar Endoso.");

						// Mostrar mensaje
					    $("#razonSocialEmpresa").text("La empresa con NIT " + idEndosatario + " existe pero no puede realizar Endoso.");

					    // Ocultar panel para registrar empresa
					    $('#panelCrearEmpresa').css('display', 'none');

						// Deshabilitar botón Enviar
						$('#btnRealizarEndoso').dxButton({
						    text: "Realizar Endoso",
						    type: "default",
							disabled: true,
							elementAttr: {
								title: "La empresa existe pero no puede realizar Endoso.",
								style: "cursor: not-allowed; pointer-events: initial;",
							},
						});
					}
					//debugger;
				}, function errorCallback(response) {

				    // Cargar campo Número de Identificación
				    $("#numIdentificacionEmp").dxNumberBox({
				        placeholder: "Ingresar número de identificación",
				        displayExpr: "Texto",
				        value: idEndosatario,
				        readOnly: true,
				        disabled: true,
				        dataSource: numIdentificacionEmp,
				        onValueChanged: function (data) {
				            numIdentificacionEmp = data.value;
				            console.log(numIdentificacionEmp);

				            // Activar botón si todos los campos están llenos
				            var camposLLenosFormEmp = validarCamposRegistroEmpresa();
				            console.log(camposLLenosFormEmp);
				            if (camposLLenosFormEmp === true) {
				                // Activar botón Registrar Empresa
				                $('#btnCrearEmpresa').dxButton({
				                    disabled: false,
				                    elementAttr: {
				                        title: "Registrar empresa",
				                        style: "cursor: pointer; pointer-events: initial;",
				                    },
				                });
				            } else {
				                // Desactivar botón Registrar Empresa
				                $('#btnCrearEmpresa').dxButton({
				                    disabled: true,
				                    elementAttr: {
				                        title: "Rellene todos los campos para poder registrar la empresa",
				                        style: "cursor: not-allowed; pointer-events: initial;",
				                    },
				                });
				            }
				        },
				        elementAttr: {
				            id: "numIdentificacionEmp",
				            title: "Ingresa el número de identificación",
				            //  class: "class-name"
				        }
				    }).dxValidator({
				        validationGroup: formRegistrarEmpresa,
				        validationRules: [{
				            type: 'required',
				            message: 'Campo requerido',
				        }],
				    });

				    $("#razonSocialEmpresa").text("");
				    $("#razonSocialEmpresa").append("La empresa con NIT " + idEndosatario + " no existe. <br>Regístrela a continuación, para poder realizar el Endoso.");
				    //DevExpress.ui.notify("La empresa con NIT " + idEndosatario + " no existe. <br>Regístrala a continuación, para poder realizar el Endoso.", 'error', 7000);

				    // Mostrar formulario para crear/registrar empresa
				    $('#panelCrearEmpresa').css('display', 'block');

				    // Deshabilitar botón Enviar
					$('#btnRealizarEndoso').dxButton({
						disabled: true,
						elementAttr: {
						    title: "La empresa con NIT " + idEndosatario + " no existe.",
						    style: "cursor: not-allowed; pointer-events: initial;",
						},
					});

					$('#wait2').hide();
				});
			}
		}
	}).dxValidator({
		validationGroup: GrupoValidarEndoso,
		validationRules: [{
			type: 'required',
			message: 'ID Endosatario requerido',
		},{
		    type: 'stringLength',
		    min: 6,
		    message: 'El ID del Endosatario debe tener al menos 6 digitos',
		}],
	});

	$("#summaryEndoso").dxValidationSummary(
	{
		validationGroup: GrupoValidarEndoso
	});

	$scope.btnRealizarEndoso = {
	    text: 'Realizar Endoso',
		type: 'default',
		visible: true,
		disabled: true,
		validationGroup: GrupoValidarEndoso,
		elementAttr: {
			id: "btnRealizarEndoso",
			class: "btnRealizarEndoso",
			title: "Para realizar el proceso de Endoso primero debe de validar si la empresa existe rellenando el campo ID Endosatario.",
			style: "cursor: not-allowed; pointer-events: initial;",
		},
		onClick: function (e) {
			var result = e.validationGroup.validate();
			if (result.isValid) {

				$('#btnRealizarEndoso').dxButton({
					disabled: true,
				});
				//$('#wait').show();
				//if (tiposEndoso == undefined || tiposEndoso == "") {
				//	DevExpress.ui.notify('Seleccione el tipo Endoso', 'error', 7000);

				//	return;
				//}

				//if (tiposOperacionEvento == undefined || tiposOperacionEvento == "") {
				//	DevExpress.ui.notify('Seleccione el tipo de operación', 'error', 7000);

				//	return;
				//}

				//if (tasaDescuentoEndoso == undefined || tasaDescuentoEndoso == "" || tasaDescuentoEndoso == 0.00) {
				//	DevExpress.ui.notify('Ingrese la tasa de descuento', 'error', 7000);

				//	return;
				//}

				$('#wait2').show();
				$http.post('/api/GenerarEventoRadian?id_seguridad=' + id_seguridad + '&tipo_evento=' + tiposEndoso + '&operacion_evento=' + tiposOperacionEvento + '&id_receptor_evento=' + idEndosatario + '&tasa_descuento=' + tasaDescuentoEndoso + '&usuario=').then(function (response) {
					//alert(response.data);
					$('#wait2').hide();
					$rootScope.ConsultarEventosRadian(id_seguridad, numero_documento, obligado);
				}, function errorCallback(response) {

					//Carga notificación de creación con opción de editar formato.
					var myDialog = DevExpress.ui.dialog.custom({
						title: "Proceso fallido",
						message: response.data.ExceptionMessage,
						buttons: [{
							text: "Aceptar",
							onClick: function (e) {
								myDialog.hide();
								$rootScope.ConsultarEventosRadian(id_seguridad, numero_documento, obligado);
							}
						}]
					});
					myDialog.show().done(function (dialogResult) {
					});

					$('#wait2').hide();
					$('#panelEndoso').hide();
				});
			}
		}
	};

	// Activar botón
	// Validar campos formulario para realizar Endoso
	function validarCamposFiltrosEndoso() {

		// Obtener valores actuales de los campos
		var tipoEndoso = $('#tiposEndoso').dxSelectBox('instance').option('value');
		var tipoOperacionEvento = $('#tiposOperacionEvento').dxSelectBox('instance').option('value');
		var tasaDescuentoEndoso = $('#tasaDescuentoEndoso').dxNumberBox('instance').option('value');
		var idEndosatario = $('#idEndosatario').dxTextBox('instance').option('value');

		if (!tipoEndoso || !tipoOperacionEvento || !tasaDescuentoEndoso || !idEndosatario) {
			return false;
		} else {
			return true;
		}
	}


    /**
    * Panel para crear empresa
    */
    // Cargar campo Razón Social
    $("#razonSocialEmp").dxTextBox({
        placeholder: "Ingresar razón social",
	    displayExpr: "Texto",
	    dataSource: razonSocialEmp,
	    onValueChanged: function (data) {
	        razonSocialEmp = data.value;
	        console.log(razonSocialEmp);

	        // Activar botón si todos los campos están llenos
	        var camposLLenosFormEmp = validarCamposRegistroEmpresa();
	        console.log(camposLLenosFormEmp);
	        if (camposLLenosFormEmp === true) {
	            // Activar botón Registrar Empresa
	            $('#btnCrearEmpresa').dxButton({
	                disabled: false,
	                elementAttr: {
	                    title: "Registrar empresa",
	                    style: "cursor: pointer; pointer-events: initial;",
	                },
	            });
	        } else {
	            // Desactivar botón Registrar Empresa
	            $('#btnCrearEmpresa').dxButton({
	                disabled: true,
	                elementAttr: {
	                    title: "Rellene todos los campos para poder registrar la empresa",
	                    style: "cursor: not-allowed; pointer-events: initial;",
	                },
	            });
	        }
	    },
	    elementAttr: {
	        id: "razonSocialEmp",
	        title: "Ingresa la razón social de la empresa",
	        //  class: "class-name"
	    }
	}).dxValidator({
	    validationGroup: formRegistrarEmpresa,
	    validationRules: [{
	        type: 'required',
	        message: 'Razón social requerida',
	    }],
	});

    // Cargar campo Tipo de Identificación
    $("#tipoIdentificacionEmp").dxSelectBox({
        placeholder: "Seleccionar...",
        displayExpr: "Texto",
        dataSource: tipoIdentificacionEmp,
        onValueChanged: function (data) {
            tipoIdentificacionEmp = data.value.ID;
            console.log(tipoIdentificacionEmp);

            // Activar botón si todos los campos están llenos
            var camposLLenosFormEmp = validarCamposRegistroEmpresa();
            console.log(camposLLenosFormEmp);
            if (camposLLenosFormEmp === true) {
                // Activar botón Registrar Empresa
                $('#btnCrearEmpresa').dxButton({
                    disabled: false,
                    elementAttr: {
                        title: "Registrar empresa",
                        style: "cursor: pointer; pointer-events: initial;",
                    },
                });
            } else {
                // Desactivar botón Registrar Empresa
                $('#btnCrearEmpresa').dxButton({
                    disabled: true,
                    elementAttr: {
                        title: "Rellene todos los campos para poder registrar la empresa",
                        style: "cursor: not-allowed; pointer-events: initial;",
                    },
                });
            }
        },
        elementAttr: {
            id: "tipoIdentificacionEmp",
            title: "Selecciona el tipo de identificación",
            //  class: "class-name"
        }
    }).dxValidator({
        validationGroup: formRegistrarEmpresa,
        validationRules: [{
            type: 'required',
            message: 'Tipo de identificación requerida',
        }],
    });

    // Cargar campo Número de Identificación
    $("#numIdentificacionEmp").dxNumberBox({
        placeholder: "Ingresar número de identificación",
        displayExpr: "Texto",
        value: null,
        dataSource: numIdentificacionEmp,
        onValueChanged: function (data) {
            numIdentificacionEmp = data.value;
            console.log(numIdentificacionEmp);

            // Activar botón si todos los campos están llenos
            var camposLLenosFormEmp = validarCamposRegistroEmpresa();
            console.log(camposLLenosFormEmp);
            if (camposLLenosFormEmp === true) {
                // Activar botón Registrar Empresa
                $('#btnCrearEmpresa').dxButton({
                    disabled: false,
                    elementAttr: {
                        title: "Registrar empresa",
                        style: "cursor: pointer; pointer-events: initial;",
                    },
                });
            } else {
                // Desactivar botón Registrar Empresa
                $('#btnCrearEmpresa').dxButton({
                    disabled: true,
                    elementAttr: {
                        title: "Rellene todos los campos para poder registrar la empresa",
                        style: "cursor: not-allowed; pointer-events: initial;",
                    },
                });
            }
        },
        elementAttr: {
            id: "numIdentificacionEmp",
            title: "Ingresa el número de identificación",
            //  class: "class-name"
        }
    }).dxValidator({
        validationGroup: formRegistrarEmpresa,
        validationRules: [{
            type: 'required',
            message: 'Número de identificación requerido',
        }],
    });

    // Validar si el correo existe
    //const sendRequest = function (value) {
    //    const invalidEmail = 'test@dx-email.com';
    //    const d = $.Deferred();
    //    setTimeout(() => {
    //        d.resolve(value !== invalidEmail);
    //    }, 1000);
    //    return d.promise();
    //};

    // Cargar campo Correo Electrónico
    $("#correoEmp").dxTextBox({
        placeholder: "Ingresar correo electrónico",
        displayExpr: "Texto",
        dataSource: correoEmp,
        onValueChanged: function (data) {
            correoEmp = data.value;
            console.log(correoEmp);

            // Activar botón si todos los campos están llenos
            var camposLLenosFormEmp = validarCamposRegistroEmpresa();
            console.log(camposLLenosFormEmp);
            if (camposLLenosFormEmp === true) {
                // Activar botón Registrar Empresa
                $('#btnCrearEmpresa').dxButton({
                    disabled: false,
                    elementAttr: {
                        title: "Registrar empresa",
                        style: "cursor: pointer; pointer-events: initial;",
                    },
                });
            } else {
                // Desactivar botón Registrar Empresa
                $('#btnCrearEmpresa').dxButton({
                    disabled: true,
                    elementAttr: {
                        title: "Rellene todos los campos para poder registrar la empresa",
                        style: "cursor: not-allowed; pointer-events: initial;",
                    },
                });
            }
        },
        elementAttr: {
            id: "correoEmp",
            title: "Ingresar correo electrónico",
            //  class: "class-name"
        }
    }).dxValidator({
        name: "Correo electrónico",
        validationGroup: formRegistrarEmpresa,
        validationRules: [{
            type: 'required',
            message: 'Correo electrónico requerido',
        },{
            type: 'email',
            message: 'Correo inválido',
        }
        //, { // Valida si el correo existe
        //    type: 'async',
        //    message: 'El correo ya se encuentra registrado',
        //    validationCallback(params) {
        //        return sendRequest(params.value);
        //    },
        //}
        ],
    });

    // Cargar campo Tipo de Operador
    $("#tipoOperadorEmp").dxSelectBox({
        placeholder: "Seleccionar...",
        displayExpr: "Texto",
        dataSource: tipoOperadorEmp,
        onValueChanged: function (data) {
            tipoOperadorEmp = data.value.ID;
            console.log(tipoOperadorEmp);

            // Activar botón si todos los campos están llenos
            var camposLLenosFormEmp = validarCamposRegistroEmpresa();
            console.log(camposLLenosFormEmp);
            if (camposLLenosFormEmp === true) {
                // Activar botón Registrar Empresa
                $('#btnCrearEmpresa').dxButton({
                    disabled: false,
                    elementAttr: {
                        title: "Registrar empresa",
                        style: "cursor: pointer; pointer-events: initial;",
                    },
                });
            } else {
                // Desactivar botón Registrar Empresa
                $('#btnCrearEmpresa').dxButton({
                    disabled: true,
                    elementAttr: {
                        title: "Rellene todos los campos para poder registrar la empresa",
                        style: "cursor: not-allowed; pointer-events: initial;",
                    },
                });
            }
        },
        elementAttr: {
            id: "tipoOperadorEmp",
            title: "Elige el tipo de operador",
            //    class: "class-name"
        }
    }).dxValidator({
        validationGroup: formRegistrarEmpresa,
        validationRules: [{
            type: 'required',
            message: 'Tipo de operador requerido',
        }],
    });

    $("#summaryCrearEmpresa").dxValidationSummary({
        validationGroup: formRegistrarEmpresa
    });

    $scope.btnCrearEmpresa = {
        text: 'Registrar empresa',
        type: 'default',
        visible: true,
        disabled: true,
        validationGroup: formRegistrarEmpresa,
        elementAttr: {
            id: "btnCrearEmpresa",
            class: "btnCrearEmpresa",
            title: "Rellene todos los campos para poder registrar la empresa",
            style: "cursor: not-allowed; pointer-events: initial;",
        },
        onClick: function (e) {
            var result = e.validationGroup.validate();
            if (result.isValid) {

                // Deshabilitar botón
                $('#btnCrearEmpresa').dxButton({
                    disabled: true,
                    elementAttr: {
                        title: "Registrando empresa",
                        style: "cursor: not-allowed; pointer-events: initial;",
                    },
                });

                $('#wait2').show();

                // Crear/Registrar empresa
                $http.post('/api/CrearEditarOperador?Idseguridad=' + null + '&RazonSocial=' + razonSocialEmp + '&TipoIdentificacion=' + tipoIdentificacionEmp + '&Identificacion=' + numIdentificacionEmp + '&Email=' + correoEmp + '&TipoOperador=' + tipoOperadorEmp).then(function (response) {
                    console.log(response.data);

                    $('#wait2').hide();
                    //$rootScope.ConsultarEventosRadian(id_seguridad, numero_documento, obligado);

                    // Ocultar formulario para crear/registrar empresa
                    $('#panelCrearEmpresa').hide();

                    // Limpiar campos formulario
                    $('#razonSocialEmp').dxTextBox({ value: '' });
                    $('#tipoIdentificacionEmp').dxSelectBox({ value: '' });
                    $('#numIdentificacionEmp').dxNumberBox({ value: '' });
                    $('#tipoOperadorEmp').dxSelectBox({ value: '' });
                    $('#correoEmp').dxTextBox({ value: '' });

                    // Mostrar notificación
                    DevExpress.ui.notify('Empresa registrada exitosamente', 'success', 7000);

                    // Mostrar razón social empresa
                    razonSocialEmpresa = response["data"]["RazonSocial"];  // Obtener razón social de la empresa
                    $("#razonSocialEmpresa").text("");
                    $("#razonSocialEmpresa").text("Razón social empresa: " + razonSocialEmpresa);

                    // Habilitar botón Realizar Endoso
                    $('#btnRealizarEndoso').dxButton({
                        disabled: false,
                        elementAttr: {
                            title: "Realizar Endoso",
                            style: "cursor: pointer; pointer-events: initial;",
                        },
                    });

                }, function errorCallback(response) {

                    // Mostrar notificación de proceso fallido
                    var myDialog = DevExpress.ui.dialog.custom({
                        title: "Proceso fallido",
                        message: response.data.ExceptionMessage,
                        buttons: [{
                            text: "Aceptar",
                            onClick: function (e) {
                                myDialog.hide();
                                //$rootScope.ConsultarEventosRadian(id_seguridad, numero_documento, obligado);
                            }
                        }]
                    });
                    myDialog.show().done(function (dialogResult) {
                    });

                    $('#wait2').hide();                 // Ocultar loading
                    //$('#panelCrearEmpresa').hide();     // Ocultar formulario para crear/registrar empresa
                });
            }
        }
    };

    // Validar campos formulario para Registrar Empresa
    function validarCamposRegistroEmpresa() {

        // Obtener valor campos
        var razonSocialEmp = $('#razonSocialEmp').dxTextBox('instance').option('value');
        var tipoIdentificacionEmp = $('#tipoIdentificacionEmp').dxSelectBox('instance').option('value');
        var numIdentificacionEmp = $('#numIdentificacionEmp').dxNumberBox('instance').option('value');
        var tipoOperadorEmp = $('#tipoOperadorEmp').dxSelectBox('instance').option('value');
        var correoEmp = $('#correoEmp').dxTextBox('instance').option('value');

        if (!razonSocialEmp || !tipoIdentificacionEmp || !numIdentificacionEmp || !tipoOperadorEmp || !correoEmp) {
            return false;
        } else {
            return true;
        }
    }

});

// Datos para el filtro Tipo Endoso
var tiposEndoso = [
    { ID: "7", Texto: 'Propiedad' },
    //{ ID: "8", Texto: 'Garantía' },
    //{ ID: "15", Texto: 'Procuración' }
];

// Datos para el filtro Tipo Operación Evento
var tiposOperacionEvento = [
    { ID: "0", Texto: 'Con responsabilidad' },
    { ID: "1", Texto: 'Sin responsabilidad' }
];

// Tipos de Identificación
var tipoIdentificacionEmp = [
    { ID: "13", Texto: 'Cédula de ciudadanía' },
    { ID: "31", Texto: 'NIT' }
];

// Tipos de Operador
var tipoOperadorEmp = [
    { ID: "1", Texto: 'Factoring' },
    { ID: "2", Texto: 'Confirming' }
];