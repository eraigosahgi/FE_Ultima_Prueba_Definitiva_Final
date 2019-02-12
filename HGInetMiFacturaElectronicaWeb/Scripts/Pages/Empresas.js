DevExpress.localization.locale(navigator.language);
var opc_pagina = "1332";
var TiposHabilitacion = [];

//var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var EmpresasApp = angular.module('EmpresasApp', [ 'dx', 'AppSrvFiltro']);
//Controlador para la gestion de Empresas(Editar, Nueva Empresa)
EmpresasApp.controller('GestionEmpresasController', function GestionEmpresasController($scope, $http, $location, SrvFiltro) {

	$('#modal_Buscar_empresa').modal('show');

	//Consultar por el id de seguridad para obtener los datos de la empresa a modificar
	var id_seguridad = location.search.split('IdSeguridad=')[1];

	var now = new Date();

	var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           Habilitacion = "",
		   Datos_estado = "",
		   Datos_postpago = 0,
		   Datos_Email_Recepcion = "",
			Datos_Email_Acuse = "",
			Datos_Email_Envio = "",
			Datos_Email_Pagos = "",
           codigo_adquiriente = "";

	SrvFiltro.ObtenerFiltro('Empresa Asociada', 'EmpresaAsociada', 'icon-user-tie', 115, '/api/ConsultarBolsaAdmin', 'ID', 'Texto', true).then(function (Datos) {
		$scope.EmpresaAsociada = Datos;
	});


	SrvFiltro.ObtenerFiltro('Empresa Descuenta Planes', 'EmpresaDescuenta', 'icon-user-tie', 115, '/api/ConsultarBolsaAdmin', 'ID', 'Texto', true).then(function (Datos) {
		$scope.EmpresaDescuenta = Datos;
	});


	$http.get('/api/DatosSesion/').then(function (response) {
		codigo_facturador = response.data[0].Identificacion;
		Habilitacion = response.data[0].Habilitacion;
		var tipo = response.data[0].Admin;
		if (tipo) {
			$scope.Admin = true;
		} else {			
			$scope.Admin = false;
			Bloquear_EmpresaDescuenta();
			Bloquear_EmpresaAsociada();
		};
		$scope.AdminIntegrador = $scope.Admin;

		//Obtiene el usuario autenticado.
		$http.get('/api/Usuario/').then(function (response) {
			//Obtiene el código del permiso.
			$http.get('/api/Permisos?codigo_usuario=' + response.data[0].CodigoUsuario + '&identificacion_empresa=' + codigo_facturador + '&codigo_opcion=' + opc_pagina).then(function (response) {
				$("#wait").hide();
				try {
					var respuesta;

					//Valida si el id_seguridad contiene datos
					if (id_seguridad)
						respuesta = response.data[0].Editar;
					else
						respuesta = response.data[0].Agregar

					//Valida la visibilidad del control según los permisos.
					if (respuesta)
						$('#button').show();
					else
						$('#button').hide();
				} catch (err) {
					DevExpress.ui.notify(err.message, 'error', 3000);
				}
			}, function errorCallback(response) {
				$('#wait').hide();
				DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			});
		});

		CargarFormulario();
	});

	var Datos_Tipoidentificacion = "",
        Datos_Idententificacion = "",
        Datos_Razon_Social = "",
        Datos_Email = "",
        Datos_Adquiriente = "",
        Datos_Obligado = "",
        Datos_Habilitacion = "",
    Datos_IdentificacionDv = "",
    Datos_Tipo = "1",
    Datos_Observaciones = "",
    Datos_empresa_Asociada = "",
    Datos_Integrador = false,
	Datos_Anexo = false,
	Datos_EmailRecepcion = false,
    Datos_Numero_usuarios = 1,
    Datos_Dias_Acuse = 10;


	//Define los campos del Formulario  
	function CargarFormulario() {
		$("#summary").dxValidationSummary({});

		$("#TipoIndentificacion").dxSelectBox({
			placeholder: "Seleccione el tipo de Indetificación",
			displayExpr: "Texto",
			dataSource: TiposIdentificacion,
			onValueChanged: function (data) {
				Datos_Tipoidentificacion = data.value.ID;
			}
		}).dxValidator({
			validationRules: [{
				type: "required",
				message: "Debe seleccionar el tipo de documento"
			}]
		});

		$("#NumeroIdentificacion").dxTextBox({
			onValueChanged: function (data) {
				Datos_Idententificacion = data.value;
				Set_EmpresaAsociada(Datos_Idententificacion);
				Set_EmpresaDescuenta(Datos_Idententificacion);
			}
		})
        .dxValidator({
        	validationRules: [{
        		type: "required",
        		message: "Debe Indicar el numero de Identificación"
        	}, {
        		type: "stringLength",
        		max: 50,
        		min: 6,
        		message: "El numero de Identificación no puede ser mayor a 50 digitos ni menor a 6"
        	}, {
        		type: "numeric",
        		message: "El numero de Identificación debe ser numérico"
        	}]
        });

		$("#txtRasonSocial").dxTextBox({
			onValueChanged: function (data) {
				Datos_Razon_Social = data.value.toUpperCase();
			}
		})
            .dxValidator({
            	validationRules: [{
            		type: "stringLength",
            		max: 200,
            		message: "La razón Social no puede ser mayor a 200 caracteres"
            	}, {
            		type: "required",
            		message: "Debe introducir la Razón Social"
            	}]
            });

		$("#txtEmail").dxTextBox({
			onValueChanged: function (data) {
				Datos_Email = data.value;
			}
		}).dxValidator({
			validationRules: [{
				type: "stringLength",
				max: 200,
				message: "El Email no puede ser mayor a 200 caracteres"
			}, {
				type: "required",
				message: "Debe introducir el Email"
			}, {
				type: "email",
				message: "El campo Email no tiene el formato correcto"
			}]
		});

		$("#Facturador").dxCheckBox({
			name: "PerfilFacturador",
			text: "Facturador Electrónico",
			value: false,
			onValueChanged: function (data) {
				Datos_Obligado = data.value;
				ValidarSeleccionPerfil();
				validarHabilitacion();				
				if (Datos_Obligado === true) {
					$("#txtEmail").dxTextBox({ value: "" });
				}

			}
		}).dxValidator({
			validationRules: [
                {
                	type: 'custom', validationCallback: function (options) {
                		if (validar()) {
                			options.rule.message = "Debe Indicar si es Facturador o Adquiriente";
                			return false;
                		} else { return true; }
                	}
                }
			]
		});


		$("#Adquiriente").dxCheckBox({
			name: "PerfilAdquiriente",
			text: "Adquiriente",
			value: false,
			onValueChanged: function (data) {
				Datos_Adquiriente = data.value;
				ValidarSeleccionPerfil();
				validarHabilitacion();

			}
		}).dxValidator({
			validationRules: [
                {
                	type: 'custom', validationCallback: function (options) {
                		if (validar()) {
                			options.rule.message = "Debe Indicar si es Facturador o Adquiriente";
                			return false;
                		} else { return true; }
                	}
                }
			]
		});


		$("#Anexo").dxCheckBox({
			name: "Anexo",
			value: false,
			onValueChanged: function (data) {
				Datos_Anexo = data.value;
			}
		});

		$("#EmailRecepcion").dxCheckBox({
			name: "EmailRecepcion",
			onValueChanged: function (data) {
				Datos_EmailRecepcion = data.value;
			}
		});

		$("#txtUsuarios").dxNumberBox({
			value: Datos_Numero_usuarios,
			onValueChanged: function (data) {
				Datos_Numero_usuarios = data.value;
			}
		}).dxValidator({
			validationRules: [
                {
                	type: "required",
                	message: "Debe introducir el número de usuarios"
                }]
		});




		$("#txtDiasAcuse").dxNumberBox({
			value: Datos_Dias_Acuse,
			onValueChanged: function (data) {
				Datos_Dias_Acuse = data.value;
			}
		}).dxValidator({
			validationRules: [
                {
                	type: "required",
                	message: "Debe introducir el número días para el acuse"
                }]
		});

		$("#Integradora").dxCheckBox({
			name: "Empresa_Integradora",
			text: "Integrador",
			value: false,
			onValueChanged: function (data) {
				Datos_Integrador = data.value;
				//Si es verdadero, entonces inabilito el modal popop de selección de empresa
				if (data.value) {
					$scope.Admin = false;
					//$("#txtempresaasociada").dxTextBox({ value: Datos_Idententificacion });
					//$('#txt_filtro_EmpresaAsociada').val(Datos_Idententificacion);
					Set_EmpresaAsociada(Datos_Idententificacion);
					Bloquear_EmpresaAsociada();
				} else {
					//Si No es verdadero, entonces primero pregunto si no es empresa Administradora ya
					//que no debe tener permisos para el modal popop de selección de empresa
					if ($scope.AdminIntegrador) {
						$scope.Admin = true;
						Desbloquear_EmpresaAsociada();
					}
				}
			}
		});


		$("#cboestado").dxSelectBox({
			placeholder: "Estado",
			displayExpr: "Texto",
			dataSource: TiposEstado,
			onValueChanged: function (data) {
				Datos_estado = data.value.ID;
			}
		}).dxValidator({
			validationRules: [{
				type: "required",
				message: "Debe seleccionar el Estado"
			}]
		});

		$("#postpagoaut").dxCheckBox({
			name: "postpagoaut",
			onValueChanged: function (data) {
				Datos_postpago = (data.value == true) ? 1 : 0;
			}
		});



		$("#txtMailEnvio").dxTextBox({
			onValueChanged: function (data) {
				Datos_Email_Envio = data.value;
			}
		}).dxValidator({
			validationRules: [{
				type: "stringLength",
				max: 200,
				message: "El Email de envío no puede ser mayor a 200 caracteres"
			}, {
				type: "required",
				message: "Debe introducir el Email de envío"
			}, {
				type: "email",
				message: "El campo Email de envío no tiene el formato correcto"
			}]
		});

		$("#txtMailRecepcion").dxTextBox({
			onValueChanged: function (data) {
				Datos_Email_Recepcion = data.value;
			}
		}).dxValidator({
			validationRules: [{
				type: "stringLength",
				max: 200,
				message: "El Email de Recepción no puede ser mayor a 200 caracteres"
			}, {
				type: "required",
				message: "Debe introducir el Email de Recepción"
			}, {
				type: "email",
				message: "El campo Email de Recepción no tiene el formato correcto"
			}]
		});



		$("#txtMailAcuse").dxTextBox({
			onValueChanged: function (data) {
				Datos_Email_Acuse = data.value;
			}
		}).dxValidator({
			validationRules: [{
				type: "stringLength",
				max: 200,
				message: "El Email de Acuse no puede ser mayor a 200 caracteres"
			}, {
				type: "required",
				message: "Debe introducir el Email de Acuse"
			}, {
				type: "email",
				message: "El campo Email de Acuse no tiene el formato correcto"
			}]
		});

		$("#txtMailPagos").dxTextBox({
			onValueChanged: function (data) {
				Datos_Email_Pagos = data.value;
			}
		}).dxValidator({
			validationRules: [{
				type: "stringLength",
				max: 200,
				message: "El Email de Pagos no puede ser mayor a 200 caracteres"
			}, {
				type: "required",
				message: "Debe introducir el Email de Pagos"
			}, {
				type: "email",
				message: "El campo Email de Pagos no tiene el formato correcto."
			}]
		});

		/////////////////////////////////////Tooltip
		$("#ttEmail").dxTooltip({
			target: "#txtEmail",
			showEvent: "mouseenter",
			hideEvent: "mouseleave",
			position: "top",
			animation: {
				show: {
					type: "slide",
					from: {
						top: -100,
						opacity: 0
					},
					to: {
						opacity: 1,
						top: 0
					}
				},
				hide: {
					type: "pop",
					from: {
						scale: 1,
						opacity: 1
					},
					to: {
						opacity: 0,
						scale: 0.1
					}
				}
			},
			contentTemplate: function (data) {
				data.html("<br/><b>Email Administrativo</b><br/>Se enviarán las notificaciones <br/> propias de la plataforma como registro, recargas, saldos, boletines, etc.");
			}
		});
		$("#ttMailEnvio").dxTooltip({
			target: "#txtMailEnvio",
			showEvent: "mouseenter",
			hideEvent: "mouseleave",
			position: "top",
			animation: {
				show: {
					type: "slide",
					from: {
						top: -100,
						opacity: 0
					},
					to: {
						opacity: 1,
						top: 0
					}
				},
				hide: {
					type: "pop",
					from: {
						scale: 1,
						opacity: 1
					},
					to: {
						opacity: 0,
						scale: 0.1
					}
				}
			},
			contentTemplate: function (data) {
				data.html("<br/><b>Email Envío Documentos</b><br/>Se enviarán las notificaciones <br/>Se utilizará para el envío de los correos electrónicos a los Adquirientes.");
			}
		});
		$("#ttMailRecepcion").dxTooltip({
			target: "#txtMailRecepcion",
			showEvent: "mouseenter",
			hideEvent: "mouseleave",
			position: "top",
			animation: {
				show: {
					type: "slide",
					from: {
						top: -100,
						opacity: 0
					},
					to: {
						opacity: 1,
						top: 0
					}
				},
				hide: {
					type: "pop",
					from: {
						scale: 1,
						opacity: 1
					},
					to: {
						opacity: 0,
						scale: 0.1
					}
				}
			},
			contentTemplate: function (data) {
				data.html("<br/><b>Email Recepción Documentos</b><br/>Se utilizará para la recepción de correos electrónicos como Adquirientes.");
			}
		});
		$("#ttMailAcuse").dxTooltip({
			target: "#txtMailAcuse",
			showEvent: "mouseenter",
			hideEvent: "mouseleave",
			position: "top",
			animation: {
				show: {
					type: "slide",
					from: {
						top: -100,
						opacity: 0
					},
					to: {
						opacity: 1,
						top: 0
					}
				},
				hide: {
					type: "pop",
					from: {
						scale: 1,
						opacity: 1
					},
					to: {
						opacity: 0,
						scale: 0.1
					}
				}
			},
			contentTemplate: function (data) {
				data.html("<br/><b>Email Recepción Acuse de Recibo</b><br/>Se enviarán las respuestas realizadas por los Adquirientes al realizar el acuse de recibo.");
			}
		});
		$("#ttMailPagos").dxTooltip({
			target: "#txtMailPagos",
			showEvent: "mouseenter",
			hideEvent: "mouseleave",
			position: "top",
			animation: {
				show: {
					type: "slide",
					from: {
						top: -100,
						opacity: 0
					},
					to: {
						opacity: 1,
						top: 0
					}
				},
				hide: {
					type: "pop",
					from: {
						scale: 1,
						opacity: 1
					},
					to: {
						opacity: 0,
						scale: 0.1
					}
				}
			},
			contentTemplate: function (data) {
				data.html("<br/><b>Email Pagos Electrónicos</b><br/>Se enviarán las notificaciones al recibir un pago electrónico por la Plataforma de Servicios.");
			}
		});
		/////////////////////////////////////Tooltip

		function validarHabilitacion() {
			var caso = "";


			if (Datos_Adquiriente) {
				caso = "Adquiriente";
			}
			if (Datos_Obligado) {
				caso = "Facturador";
			}

			switch (caso) {
				case "Facturador":
					Datos_Habilitacion = Habilitacion;
					$('#idHabilitacion').show();
					$("#Habilitacion").dxRadioGroup({ visible: true });
					$("#Habilitacion").dxRadioGroup({ value: TiposHabilitacion[BuscarID(TiposHabilitacion, Datos_Habilitacion)] });
					$("#divEmailFacturador").show();
					break;
				case "Adquiriente":
					$('#idHabilitacion').hide();
					$("#Habilitacion").dxRadioGroup({ visible: false });
					$("#Habilitacion").dxRadioGroup({ value: "0" });
					Datos_Habilitacion = "0";
					$("#divEmailFacturador").hide();
					break;
				case "":
					$('#idHabilitacion').hide();
					$("#Habilitacion").dxRadioGroup({ visible: false });
					$("#Habilitacion").dxRadioGroup({ value: "" });
					Datos_Habilitacion = "0";
					$("#divEmailFacturador").hide();
			}


		}



		//Valido Si esta seleccionado alguno de los dos perfiles, Facturador o adquiriente
		function ValidarSeleccionPerfil() {
			$("#Adquiriente").dxValidator({
				validationRules: [{
					type: 'custom', validationCallback: function (options) {
						if (!Datos_Adquiriente && !Datos_Obligado) {
							options.rule.message = "Debe Indicar si es adquiriente o Facturador";
							return false;
						} else {
							return true;
						}
					}
				}]
			});
			$("#Facturador").dxValidator({
				validationRules: [{
					type: 'custom', validationCallback: function (options) {
						if (!Datos_Adquiriente && !Datos_Obligado) {
							options.rule.message = "Debe Indicar si es adquiriente o Facturador";
							return false;
						} else {
							return true;
						}
					}
				}]
			});
		}




		$http.get('/api/empresas?tipo=' + Habilitacion).then(function (response) {			
			TiposHabilitacion = response.data;
			$("#Habilitacion").dxRadioGroup({
				searchEnabled: true,
				caption: 'Habilitación',
				dataSource: new DevExpress.data.ArrayStore({
					data: TiposHabilitacion,
					key: "ID"
				}),
				displayExpr: "Texto",
				Enabled: true,
				onValueChanged: function (data) {
					Datos_Habilitacion = data.value.ID;
				}
			}).dxValidator({
				validationRules: [{
					type: "required",
					message: "Debe indicar el tipo de Habilitacion"
				}]
			});


			if (id_seguridad) { consultar(); }
		});


		$("#txtobservaciones").dxTextArea({
			onValueChanged: function (data) {
				Datos_Observaciones = data.value.toUpperCase();
			}
		})
         .dxValidator({
         	validationRules: [
			{
				type: "stringLength",
				max: 150,
				message: "El campo Observación no puede ser mayor a 150 caracteres"
			}]
         });


		$("#button").dxButton({
			text: "Guardar",
			type: "default",
			useSubmitBehavior: true
		});


		$("#form1").on("submit", function (e) {
			guardarEmpresa();
			e.preventDefault();
		});

		function validar() {
			if (Datos_Adquiriente == true || Datos_Obligado == true) {
				return false;
			}
			return true;
		}

	};

	function consultar() {
		Datos_Tipo = "2";
		//SrvEmpresas.ObtenerEmpresa(id_seguridad).then(function (response) {
		$http.get('/api/Empresas?IdSeguridad=' + id_seguridad).then(function (response) {
			try {

				Datos_Tipoidentificacion = response.data[0].TipoIdentificacion;
				Datos_Idententificacion = response.data[0].Identificacion;
				Datos_Razon_Social = response.data[0].RazonSocial;
				Datos_Email = response.data[0].Email;
				Datos_Adquiriente = (response.data[0].Intadquiriente) ? 1 : 0;
				Datos_Obligado = (response.data[0].intObligado) ? 1 : 0;
				Datos_Habilitacion = response.data[0].Habilitacion;
				Datos_IdentificacionDv = response.data[0].IntIdentificacionDv;
				Datos_Observaciones = response.data[0].StrObservaciones;
				Datos_empresa_Asociada = response.data[0].StrEmpresaAsociada;
				Datos_Integrador = response.data[0].IntIntegrador;
				Datos_Numero_usuarios = response.data[0].IntNumUsuarios;
				Datos_Dias_Acuse = response.data[0].IntAcuseTacito;
				Datos_Anexo = response.data[0].IntAnexo;
				Datos_EmailRecepcion = response.data[0].IntEmailRecepcion;
				Datos_estado = response.data[0].Estado;
				Datos_postpago = response.data[0].Postpago;
				Datos_Email_Envio = response.data[0].StrMailEnvio;
				Datos_Email_Recepcion = response.data[0].StrMailRecepcion;
				Datos_Email_Acuse = response.data[0].StrMailAcuse;
				Datos_Email_Pagos = response.data[0].StrMailPagos;

				$("#NumeroIdentificacion").dxTextBox({ value: Datos_Idententificacion });
				$("#NumeroIdentificacion").dxTextBox({ readOnly: true });
				$("#txtRasonSocial").dxTextBox({ value: Datos_Razon_Social });
				$("#txtEmail").dxTextBox({ value: Datos_Email });
				$("#txtMailEnvio").dxTextBox({ value: Datos_Email_Envio });
				$("#txtMailRecepcion").dxTextBox({ value: Datos_Email_Recepcion });
				$("#txtMailAcuse").dxTextBox({ value: Datos_Email_Acuse });
				$("#txtMailPagos").dxTextBox({ value: Datos_Email_Pagos });
				$("#TipoIndentificacion").dxSelectBox({ value: TiposIdentificacion[BuscarID(TiposIdentificacion, Datos_Tipoidentificacion)] });
				$("#TipoIndentificacion").dxSelectBox({ readOnly: true });
				Set_EmpresaAsociada((Datos_empresa_Asociada) ? Datos_empresa_Asociada : '');
				Set_EmpresaDescuenta((response.data[0].StrEmpresaDescuenta) ? response.data[0].StrEmpresaDescuenta : Datos_Idententificacion)
				if (Datos_Observaciones != null) {
					$("#txtobservaciones").dxTextArea({ value: Datos_Observaciones });
				}
				if (Datos_Adquiriente == 1) {
					$("#Adquiriente").dxCheckBox({ value: 1 });
				}
				if (Datos_Integrador == 1)
					$("#Integradora").dxCheckBox({ value: true });

				$("#txtUsuarios").dxNumberBox({ value: Datos_Numero_usuarios });
				$("#txtDiasAcuse").dxNumberBox({ value: Datos_Dias_Acuse });
				if (Datos_Obligado == 1) {
					$("#Facturador").dxCheckBox({ value: 1 });
					$("#Habilitacion").dxRadioGroup({ value: TiposHabilitacion[BuscarID(TiposHabilitacion, response.data[0].Habilitacion)] });
				}

				if (Datos_Anexo == 1)
					$("#Anexo").dxCheckBox({ value: true });

				if (Datos_EmailRecepcion == 1)
					$("#EmailRecepcion").dxCheckBox({ value: true });

				$("#cboestado").dxSelectBox({ value: TiposEstado[BuscarID(TiposEstado, Datos_estado)] });

				if (Datos_postpago == 1)
					$("#postpagoaut").dxCheckBox({ value: true });


			} catch (err) {
				DevExpress.ui.notify(err.message + ' Validar Estado Producción', 'error', 7000);
			}
		});
	}



	$scope.ButtonGuardar = {
		text: 'Guardar',
		type: 'default',
		validationGroup: "ValidacionDatosEmpresa",
		onClick: function (e) {
			if (ValidarAsociadaDescuenta()) {
				guardarEmpresa();
			}
		}
	};
	//Valida los campos asociadas y descuenta
	function ValidarAsociadaDescuenta() {
		if (txt_hgi_EmpresaAsociada == undefined || txt_hgi_EmpresaAsociada == '') {
			Campo_Invalido_EmpresaAsociada();
			return false;
		}
		if (txt_hgi_EmpresaDescuenta == undefined || txt_hgi_EmpresaDescuenta == '') {
			Campo_Invalido_EmpresaDescuenta();
			return false;
		}
		return true;
	}

	function guardarEmpresa() {		
		if (ValidarAsociadaDescuenta()) {
			var empresa = null;
			var Asociada = "";
			if (txt_hgi_EmpresaAsociada != null && txt_hgi_EmpresaAsociada != "") {
				Asociada = txt_hgi_EmpresaAsociada;
			} else {
				empresa = Datos_Idententificacion;
				Asociada = empresa;
			}

			var data = $.param({
				TipoIdentificacion: Datos_Tipoidentificacion,
				Identificacion: Datos_Idententificacion,
				RazonSocial: Datos_Razon_Social,
				Email: Datos_Email,
				Intadquiriente: (Datos_Adquiriente) ? true : false,
				IntObligado: (Datos_Obligado) ? true : false,
				IntHabilitacion: Datos_Habilitacion,
				StrEmpresaAsociada: Asociada,
				tipo: Datos_Tipo,
				StrObservaciones: Datos_Observaciones,
				IntIntegrador: Datos_Integrador,
				IntNumUsuarios: Datos_Numero_usuarios,
				IntAcuseTacito: Datos_Dias_Acuse,
				IntAnexo: Datos_Anexo,
				IntEmailRecepcion: Datos_EmailRecepcion,
				StrEmpresaDescuento: txt_hgi_EmpresaDescuenta,
				intestado: Datos_estado,
				intpostpago: Datos_postpago,
				StrMailEnvio: Datos_Email_Envio,
				StrMailRecepcion: Datos_Email_Recepcion,
				StrMailAcuse: Datos_Email_Acuse,
				StrMailPagos: Datos_Email_Pagos
			});


			//SrvEmpresas.GuardarEmpresa(data).then(function (response) {
				$http.post('/api/Empresas?' + data).then(function (response) {
				try {
					//Aqui se debe colocar los pasos a seguir
					DevExpress.ui.notify({ message: "Empresa Guardada con exito", position: { my: "center top", at: "center top" } }, "success", 1500);
					$("#button").hide();
					$("#btncancelar").hide();
					setTimeout(IrAConsulta, 2000);
				} catch (err) {
					DevExpress.ui.notify(err.message, 'error', 3000);
				}
			});
		}
	}

});

//Controlador para gestionar la consulta de empresas
EmpresasApp.controller('ConsultaEmpresasController', function ConsultaEmpresasController($scope, $http, $location) {
	$scope.Admin = false;
	$("#wait").show();
	$http.get('/api/DatosSesion/').then(function (response) {
		codigo_facturador = response.data[0].Identificacion;
		var tipo = response.data[0].Admin;
		if (tipo) {
			$scope.Admin = true;
		};
		$http.get('/api/ObtenerEmpresas?IdentificacionEmpresa=' + codigo_facturador).then(function (response) {
			$('#wait').hide();
			$("#gridEmpresas").dxDataGrid({
				dataSource: response.data,

				paging: {
					pageSize: 20
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				}
                     , loadPanel: {
                     	enabled: true
                     }
                          , onCellPrepared: function (options) {
                          	var fieldData = options.value,
								fieldHtml = "";
                          	try {
                          		if (options.data.Estado == 1) {
                          			estado = " style='color:green; cursor:default;' title='Activo'";
                          		} else {
                          			estado = " style='color:red; cursor:default;' title='Inactivo'";
                          		}

                          	} catch (err) {

                          	}

                          }, allowColumnResizing: true

                   , columns: [
                       {
                       	cssClass: "col-md-1 col-xs-2",
                       	cellTemplate: function (container, options) {
                       		$("<div style='text-align:center'>")
								.append($("<a taget=_self class='icon-pencil3' title='Editar' href='GestionEmpresas.aspx?IdSeguridad=" + options.data.IdSeguridad + "'>"))
								.appendTo(container);
                       	}
                       },

                       {
                       	caption: "Identificacion",
                       	dataField: "Identificacion"
                       },
                       {
                       	caption: "Razón social",
                       	dataField: "RazonSocial",                       
                       },
                       {
                       	caption: "Email",
                       	dataField: "Email"
                       },
                       {
                       	caption: "Serial",
                       	dataField: "Serial"
                       },
                       {
                       	dataField: "Perfil"
                       },
					   {
					   	cssClass: "col-md-1 col-xs-1",
					   	caption: 'Estado',
					   	dataField: 'Estado',
					   	cellTemplate: function (container, options) {
					   		$("<div style='text-align:center; cursor:default;'>")
								.append($("<a taget=_self class='icon-circle2'" + estado + ">"))
								.appendTo(container);
					   	}
					   },
					   {
					   	caption: "Asociado",
					   	dataField: "Asociado",
					   	hidingPriority: 0
					   },
					   {
					   	caption: "Empresa descuento",
					   	dataField: "EmpresaDescuento",
					   	hidingPriority: 1
					   },

					   {
					   	caption: "Post-Pago automatico",
					   	dataField: "Postpago",
					   	hidingPriority: 2
					   },
					   {
					   	caption: "Nº Usuarios activos",
					   	dataField: "Nusuaurios",
					   	hidingPriority: 3
					   },
					   {
					   	caption: "Nº Horas para acuse tacito",
					   	dataField: "HorasAcuse",
					   	hidingPriority: 4
					   },
					   {
					   	caption: "Notificación en recepción",
					   	dataField: "NotificacionMail",
					   	hidingPriority: 5
					   },
					    {
					    	caption: "Correo de Recepción de Documentos",
					    	dataField: "StrMailRecepcion",
					    	hidingPriority: 6
					    }
						,
					    {
					    	caption: "Correo de Recepción de Acuse",
					    	dataField: "StrMailAcuse",
					    	hidingPriority: 7
					    }
						,
					    {
					    	caption: "Correo para Envios",
					    	dataField: "StrMailEnvio",
					    	hidingPriority: 8
					    }
						,
					    {
					    	caption: "Correo de Recepción de Pagos",
					    	dataField: "StrMailPagos",
					    	hidingPriority: 9
					    }





                       //,
                       //{
                       //    dataField: "Habilitacion"
                       //}                   
                   ],
				filterRow: {
					visible: true
				}
			});
			
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});


	}, function errorCallback(response) {
		$('#wait').hide();
		DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
	});

	
});

//Esta funcion es para ir a la pagina de consulta
function IrAConsulta() {
	window.location.assign("../Pages/ConsultaEmpresas.aspx");
}

//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
	for (var i = 0; i < miArray.length; i += 1) {
		if (ID == miArray[i].ID) {
			return i;
		}
	}
}

var TiposIdentificacion =
    [
        { ID: "11", Texto: 'Registro civil' },
        { ID: "12", Texto: 'Tarjeta de identidad' },
        { ID: "13", Texto: 'Cédula de ciudadanía' },
        { ID: "21", Texto: 'Tarjeta de extranjería' },
        { ID: "22", Texto: 'Cédula de extranjería' },
        { ID: "31", Texto: 'NIT' },
        { ID: "41", Texto: 'Pasaporte' },
        { ID: "42", Texto: 'Documento de identificación extranjero' }
    ];


var TiposEstado =
[
{ ID: "1", Texto: 'ACTIVO' },
{ ID: "2", Texto: 'INACTIVO' }
];