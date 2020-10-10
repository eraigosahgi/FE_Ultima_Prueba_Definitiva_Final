DevExpress.localization.locale(navigator.language);
var opc_pagina = "1332";
var TiposHabilitacion = [];
var Lista_Proveedores_Firma = [];
var Regresa_Listado = false;//Indica true o false si puede regresar a la pagina con la lista de empresas
var id_seguridad
//Estas variables son para tener la copia de cada correo y guardar si tienen algun cambio
var Copia_Email_Administracion = "";
var Copia_Email_Recepcion = "";
var Copia_Email_Acuse = "";
var Copia_Email_Envio = "";
var Copia_Email_Pagos = "";


//Estas variables son para identificar el proceso de validación de cada correo
var Proc_Email = 0,
Proc_MailEnvio = 0,
Proc_MailRecepcion = 0,
Proc_MailAcuse = 0,
Proc_MailPagos = 0

var Copia_Proc_Email = 0,
Copia_Proc_MailEnvio = 0,
Copia_Proc_MailRecepcion = 0,
Copia_Proc_MailAcuse = 0,
Copia_Proc_MailPagos = 0

var codigo_facturador = "",
numero_documento = "",
estado_dian = "",
estado_recibo = "",
fecha_inicio = "",
fecha_fin = "",
Habilitacion = "",
Datos_estado = "",
Datos_postpago = 0,
Datos_debug = 0,
Datos_Email_Recepcion = "",
Datos_Email_Acuse = "",
Datos_Email_Envio = "",
Datos_Email_Pagos = "",
codigo_adquiriente = "",
Datos_VersionDIAN = "",
Datos_CertFirma = "",
Datos_Hgi_Responsable = "",
Datos_Hgi_Notifica = "",
Datos_FechaCert = "",
id_seguridad = "",
Datos_ClaveCert = "",
Datos_Serial = "",
Datos_Serial_Cloud = "",
Datos_proveedores = "";
//Desde hasta en la consulta de la grid
var Desde = 0;
var Hasta = 20;
var CantRegCargados = 0;

var ModalDetalleEmpresasApp = angular.module('ModalDetalleEmpresasApp', []);

var EmpresasApp = angular.module('EmpresasApp', ['ModalDetalleEmpresasApp', 'dx', 'AppSrvFiltro', 'AppMaestrosEnum', 'AppSrvEmpresas']);
//Controlador para la gestion de Empresas(Editar, Nueva Empresa)
EmpresasApp.controller('GestionEmpresasController', function GestionEmpresasController($scope, $http, $location, SrvFiltro, SrvMaestrosEnum, SrvEmpresas) {

	$("#Recargar").dxButton({
		icon: "refresh",
		onClick: function (e) {
			consultar();
		}
	});

	$("#InfCert").dxButton({
		name: "InformacionCert",
		icon: "search",
		onClick: function (e) {
			if (Datos_ClaveCert == "" || Datos_ClaveCert == undefined) {
				DevExpress.ui.notify("Debe ingresar la contraseña del certificado", 'error', 3000);
				return false;
			}
			SrvEmpresas.ObtenerInfCert(id_seguridad, Datos_ClaveCert, Datos_proveedores).then(function (data) {
				Datos_FechaCert = data.FechaVencimiento;
				$("#VenceCert").dxTextBox({ value: Datos_FechaCert });
				showInfo(data);
			});
		}
	}).removeClass("dx-icon");

	//Modal Certificado
	//*******************************************************************************************
	var Certificado = {},
	   popup = null,
	   popupOptions = {
	   	width: 500,
	   	height: 250,
	   	contentTemplate: function () {
	   		return $("<div />").append(
				  $("<p>Descripción:     <span>" + Certificado.Descripcion + "</span></p>"),
                  $("<p>Fecha de vencimiento:      <span>" + Certificado.FechaVencimiento + "</span></p>"),
				  $("<p>Serial:      <span>" + Certificado.Serial + "</span></p>"),
				  $("<p>Emisor:      <span>" + Certificado.Emisor + "</span></p>")
			);
	   	},
	   	showTitle: true,
	   	title: "Información del Certificado",
	   	visible: false,
	   	dragEnabled: false,
	   	closeOnOutsideClick: true
	   };

	var showInfo = function (data) {
		Certificado = data;
		if (popup) {
			$(".popup").remove();
		}
		var $popupContainer = $("<div />")
                                .addClass("popup")
                                .appendTo($("#popup"));
		popup = $popupContainer.dxPopup(popupOptions).dxPopup("instance");
		popup.show();
	};
	//*******************************************************************************************

	$('#modal_Buscar_empresa').modal('show');

	//Consultar por el id de seguridad para obtener los datos de la empresa a modificar
	id_seguridad = location.search.split('IdSeguridad=')[1];

	var now = new Date();

	try {
		SrvFiltro.ObtenerFiltro('Empresa Asociada', 'EmpresaAsociada', 'icon-user-tie', 115, '/api/ConsultarBolsaAdmin', 'ID', 'Texto', true, 14).then(function (Datos) {
			$scope.EmpresaAsociada = Datos;
		});
	} catch (e) {
		//cuando el usuario no es administrador esta consulta no es necesario
	}

	try {
		SrvFiltro.ObtenerFiltro('Empresa Descuenta', 'EmpresaDescuenta', 'icon-user-tie', 115, '/api/ConsultarBolsaAdmin', 'ID', 'Texto', true, 17).then(function (Datos) {
			$scope.EmpresaDescuenta = Datos;
		});
	} catch (e) {
		//cuando el usuario no es administrador esta consulta no es necesario
	}

	$http.get('/api/DatosSesion/').then(function (response) {

		$scope.Integrador = response.data[0].Integrador

		codigo_facturador = response.data[0].Identificacion;
		Habilitacion = response.data[0].Habilitacion;
		var tipo = response.data[0].Admin;
		if (tipo) {
			$scope.Admin = true;
		} else {
			$scope.Admin = false;
			//Bloquear_EmpresaDescuenta();
			//Bloquear_EmpresaAsociada();
		};
		$scope.AdminIntegrador = $scope.Admin;

		//Aqui valido si regreso al Listado o me quedo en la misma vista de empresa
		if ($scope.Integrador || $scope.Admin) {
			Regresa_Listado = true;
		} else {
			Regresa_Listado = false;
		}

		//Obtiene el usuario autenticado.
		$http.get('/api/Usuario/').then(function (response) {
			//Obtiene el código del permiso.
			$http.get('/api/Permisos?codigo_usuario=' + response.data[0].CodigoUsuario + '&identificacion_empresa=' + codigo_facturador + '&codigo_opcion=' + opc_pagina).then(function (response) {
				$("#wait").hide();
				try {
					var respuesta;

					//Valida si el id_seguridad contiene datos
					if (id_seguridad) {
						$scope.id_seguridad = "      -   Id de seguridad : " + id_seguridad;
						respuesta = response.data[0].Editar;
					} else {
						$scope.id_seguridad = "";
						respuesta = response.data[0].Agregar
					}
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
		Datos_telefono = "",
	Datos_IdentificacionDv = "",
	Datos_Tipo = "1",
	Datos_Observaciones = "",
	Datos_empresa_Asociada = "",
	Datos_Integrador = false,
	Datos_Anexo = false,
	Datos_EmailRecepcion = false,
	Datos_Numero_usuarios = 1,
	Datos_Horas_Acuse = 0;


	//Define los campos del Formulario  
	function CargarFormulario() {
		$("#summary").dxValidationSummary({});

		$("#TipoIndentificacion").dxSelectBox({
			placeholder: "Seleccione el tipo de Idetificación",
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
				if ($scope.Admin) {
					Set_EmpresaAsociada(Datos_Idententificacion);
					Set_EmpresaDescuenta(Datos_Idententificacion);
				}
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


		$("#txttelefono").dxTextBox({
			onValueChanged: function (data) {
				Datos_telefono = data.value;
			}
		})
		.dxValidator({
			validationRules: [
			 {
			 	type: "stringLength",
			 	max: 50,
			 	message: "El teléfono no puede ser mayor a 50 caracteres"
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
				if (Copia_Email_Administracion != Datos_Email) {
					AsigEstado("Proc_Email", 0);
					Proc_Email = 0;
				} else {
					AsigEstado("Proc_Email", Copia_Proc_Email);
				}
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






		$("#txtHorasAcuse").dxNumberBox({
			value: Datos_Horas_Acuse,
			onValueChanged: function (data) {
				Datos_Horas_Acuse = data.value;
			}
		}).dxValidator({
			validationRules: [
				{
					type: "required",
					message: "Debe introducir el número días para el acuse"
				},
			{
				type: 'custom', validationCallback: function (options) {
					if (!validarHorasAcuse()) {
						options.rule.message = "Las horas de acuse deben ser mayor a 71 o cero(0) para no tomar en cuenta este parametro";
						return false;
					} else { return true; }
				}

			}]
		});



		$("#txtMailEnvio").dxTextBox({
			onValueChanged: function (data) {
				Datos_Email_Envio = data.value;
				if (Copia_Email_Envio != Datos_Email_Envio) {
					AsigEstado("Proc_MailEnvio", 0);
					Proc_MailEnvio = 0;
				} else {
					AsigEstado("Proc_MailEnvio", Copia_Proc_MailEnvio);
				}
				//Validación adicional
				$("#txtMailEnvio").dxValidator({
					validationRules: [{
						type: "email",
						message: "El campo Email de envío no tiene el formato correcto"
					}]
				});
			}
		}).dxValidator({
			validationRules: [{
				type: "stringLength",
				max: 200,
				message: "El Email de envío no puede ser mayor a 200 caracteres"
			},
			{
				type: "email",
				message: "El campo Email de envío no tiene el formato correcto"
			},
			{
				type: 'custom', validationCallback: function (options) {
					if (!validarEmailEnvio()) {
						options.rule.message = "Debe introducir el Email de Envio";
						return false;
					} else { return true; }
				}

			}

			]
		});

		$("#txtMailRecepcion").dxTextBox({
			onValueChanged: function (data) {
				Datos_Email_Recepcion = data.value;
				if (Copia_Email_Recepcion != Datos_Email_Recepcion) {
					AsigEstado("Proc_MailRecepcion", 0);
					Proc_MailRecepcion = 0;
				} else {
					AsigEstado("Proc_MailRecepcion", Copia_Proc_MailRecepcion);
				}

				//Validación adicional
				$("#txtMailRecepcion").dxValidator({
					validationRules: [{
						type: "email",
						message: "El campo Email de Recepción no tiene el formato correcto"
					}]
				});
			}
		}).dxValidator({
			validationRules: [{
				type: "stringLength",
				max: 200,
				message: "El Email de Recepción no puede ser mayor a 200 caracteres"
			}, {
				type: 'custom', validationCallback: function (options) {
					if (!validarEmailRecepcion()) {
						options.rule.message = "Debe introducir el Email de Recepción";
						return false;
					} else { return true; }
				}

			}, {
				type: "email",
				message: "El campo Email de Recepción no tiene el formato correcto"
			}]
		});



		$("#txtMailAcuse").dxTextBox({
			onValueChanged: function (data) {
				Datos_Email_Acuse = data.value;
				if (Copia_Email_Acuse != Datos_Email_Acuse) {
					AsigEstado("Proc_MailAcuse", 0);
					Proc_MailAcuse = 0;
				} else {
					AsigEstado("Proc_MailAcuse", Copia_Proc_MailAcuse);
				}
				//Validación adicional
				$("#txtMailAcuse").dxValidator({
					validationRules: [{
						type: "email",
						message: "El campo Email de Acuse no tiene el formato correcto"
					}]
				});
			}
		}).dxValidator({
			validationRules: [{
				type: "stringLength",
				max: 200,
				message: "El Email de Acuse no puede ser mayor a 200 caracteres"
			}, {

				type: 'custom', validationCallback: function (options) {
					if (!validarEmailAcuse()) {
						options.rule.message = "Debe introducir el Email de Acuse";
						return false;
					} else { return true; }
				}
			}, {
				type: "email",
				message: "El campo Email de Acuse no tiene el formato correcto"
			}]
		});

		$("#txtMailPagos").dxTextBox({
			onValueChanged: function (data) {
				Datos_Email_Pagos = data.value;
				if (Copia_Email_Pagos != Datos_Email_Pagos) {
					AsigEstado("Proc_MailPagos", 0);
					Proc_MailPagos = 0;
				} else {
					AsigEstado("Proc_MailPagos", Copia_Proc_MailPagos);
				}
				//Validación adicional
				$("#txtMailPagos").dxValidator({
					validationRules: [{
						type: "email",
						message: "El campo Email de Pagos no tiene el formato correcto"
					}]
				});
			}
		}).dxValidator({
			validationRules: [{
				type: "stringLength",
				max: 200,
				message: "El Email de Pagos no puede ser mayor a 200 caracteres"
			}, {

				type: 'custom', validationCallback: function (options) {
					if (!validarEmailPagos()) {
						options.rule.message = "Debe introducir el Email de Pagos";
						return false;
					} else { return true; }
				}
			}, {
				type: "email",
				message: "El campo Email de Pagos no tiene el formato correcto"
			}]
		});



		//****************En caso de Ser Administrador, muestro estos puntos********************************************************
		if ($scope.Admin) {

			$("#Integradora").dxCheckBox({
				name: "Empresa_Integradora",
				text: "Integrador",
				value: false,
				onValueChanged: function (data) {
					Datos_Integrador = data.value;
					//Si es verdadero, entonces inabilito el modal popop de selección de empresa
					if (data.value) {
						//$scope.Admin = false;
						Set_EmpresaAsociada(Datos_Idententificacion);
						Bloquear_EmpresaAsociada();
					} else {
						//Si No es verdadero, entonces primero pregunto si no es empresa Administradora ya
						//que no debe tener permisos para el modal popop de selección de empresa
						if ($scope.AdminIntegrador) {
							//$scope.Admin = true;
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
					if (Datos_estado == 3) {
						//Si es proceso de registro, entonces todos los campos de Email se colocan blanco ""
						$("#txtEmail").dxTextBox({ value: "" });
						$("#txtMailEnvio").dxTextBox({ value: "" });
						$("#txtMailRecepcion").dxTextBox({ value: "" });
						$("#txtMailAcuse").dxTextBox({ value: "" });
						$("#txtMailPagos").dxTextBox({ value: "" });
					} else {
						//Si Se activa nuevamente la empresa, entonces se valida si ya existen datos de esta empresa en Emails
						Proc_Email = Copia_Proc_Email;
						Proc_MailEnvio = Copia_Proc_MailEnvio;
						Proc_MailRecepcion = Copia_Proc_MailRecepcion;
						Proc_MailAcuse = Copia_Proc_MailAcuse,
						Proc_MailPagos = Copia_Proc_MailPagos;

						$("#txtEmail").dxTextBox({ value: Copia_Email_Administracion });
						$("#txtMailRecepcion").dxTextBox({ value: Copia_Email_Recepcion });
						$("#txtMailEnvio").dxTextBox({ value: Copia_Email_Envio });
						$("#txtMailAcuse").dxTextBox({ value: Copia_Email_Acuse });
						$("#txtMailPagos").dxTextBox({ value: Copia_Email_Pagos });

					}


					//Validar cuando cambia de Activo a Registro******************************************************
					$("#txtMailEnvio").dxValidator({
						validationRules: [
						{
							type: 'custom', validationCallback: function (options) {
								if (!validarEmailEnvio()) {
									options.rule.message = "Debe introducir el Email de Envio";
									return false;
								} else { return true; }
							}

						}
						]
					});

					$("#txtMailRecepcion").dxValidator({
						validationRules: [
						{
							type: 'custom', validationCallback: function (options) {
								if (!validarEmailRecepcion()) {
									options.rule.message = "Debe introducir el Email de Recepción";
									return false;
								} else { return true; }
							}

						}]
					});



					$("#txtMailAcuse").dxValidator({
						validationRules: [{
							type: 'custom', validationCallback: function (options) {
								if (!validarEmailAcuse()) {
									options.rule.message = "Debe introducir el Email de Acuse";
									return false;
								} else { return true; }
							}
						}]
					});

					$("#txtMailPagos").dxValidator({
						validationRules: [{
							type: 'custom', validationCallback: function (options) {
								if (!validarEmailPagos()) {
									options.rule.message = "Debe introducir el Email de Pagos";
									return false;
								} else { return true; }
							}
						}]
					});




					//Validar cuando cambia de Activo a Registro******************************************************

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

			$("#debug").dxCheckBox({
				name: "debug",
				onValueChanged: function (data) {
					Datos_debug = (data.value == true) ? 1 : 0;
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


			$("#cboVersionDIAM").dxSelectBox({
				placeholder: "Versión",
				displayExpr: "Texto",
				dataSource: VersionDIAN,
				onValueChanged: function (data) {
					Datos_VersionDIAN = data.value.ID;
				}
			}).dxValidator({
				validationRules: [{
					type: "required",
					message: "Debe indicar la versión DIAN"
				}]
			});

			$("#txtobservaciones").dxTextArea({
				height: "100px",
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


			$("#txtSerial").dxTextBox({

				onValueChanged: function (data) {
					Datos_Serial = data.value;
				}
			})
			.dxValidator({
				validationRules: [
				{
					type: "stringLength",
					max: 50,
					message: "El campo Serial no puede ser mayor a 50 caracteres"
				}, {
					type: "required",
					message: "Debe indicar el Serial"
				}]
			});


			$("#txtSerialCloud").dxTextBox({

				onValueChanged: function (data) {
					Datos_Serial_Cloud = data.value;
				}
			})


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
				value: true,
				readOnly: true,
				onValueChanged: function (data) {
					Datos_EmailRecepcion = data.value;
				}
			});

			//****************En caso de Ser Administración o Integrador, muestro estos puntos


			//***********************Datos del Cerificado
			//Certificado Digital

		} else {
			//Aqui consultamos el registro de empresa como Facturador común o integrador
			if (id_seguridad) {
				consultar();
				//Si es Facturador Común o Integrador, entonces no debe tener activa la opción de manejar anexos
				$("#Anexo").dxCheckBox({ readOnly: true });
				$("#txtRasonSocial").dxTextBox({ readOnly: true });
			}
		}

		$http.get('/api/empresas?tipo=' + Habilitacion).then(function (response) {
			TiposHabilitacion = response.data;
			$("#Habilitacion").dxRadioGroup({
				searchEnabled: true,
				caption: 'Habilitación',
				dataSource: TiposHabilitacion,
				displayExpr: "Texto",
				Enabled: true,
				onValueChanged: function (data) {
					Datos_Habilitacion = data.value.ID;
				}
			});

			//*********************************
			SrvMaestrosEnum.ObtenerEnum(9, 'publico').then(function (data) {
				Lista_Proveedores_Firma = data;
				$("#cboProveedor").dxSelectBox({
					placeholder: "Proveedor del Certificado",
					displayExpr: "Descripcion",
					dataSource: data,
					onValueChanged: function (data) {
						Datos_proveedores = data.value.ID;
						$("#Certificado").dxFileUploader({
							multiple: false,
							allowedFileExtensions: [".pfx", ".p12"],
							uploadMode: "instantly",
							readyToUploadMessage: "Certificado Digital subido exitosamente",
							uploadUrl: "/api/SubirArchivo?StrIdSeguridad=" + id_seguridad + "&Clave=" + Datos_ClaveCert + "&Certificadora=" + Datos_proveedores,
						});
					}
				})
				/*.dxValidator({
					validationRules: [{
						type: "required",
						message: "Debe seleccionar el Proveedor del Certificado"
					}]
				})*/
				;

				if (id_seguridad) { consultar(); }
			});
			//*********************************

		});




		if ($scope.Admin) {
			$("#Habilitacion").dxRadioGroup().dxValidator({
				validationRules: [{
					type: "required",
					message: "Debe indicar el tipo de Habilitacion"
				}]
			});
		}




		$("#CerFirma").dxRadioGroup({
			searchEnabled: true,
			caption: 'Firma',
			dataSource: TiposCertFirma,
			displayExpr: "Texto",
			Enabled: true,
			onValueChanged: function (data) {
				Datos_CertFirma = data.value.ID;

				if (Datos_CertFirma == 0) {
					$('#PanelFirmaFacturador').hide();
					//Coloco como NO requerido el proveedor del certificado
					$("#cboProveedor").dxValidator({ validationRules: [] });
					//Coloco como NO requerida la clave del certificado						
					$("#ClaveCert").dxValidator({ validationRules: [] });

					try {
						//Coloco como NO requerida la clave del certificado						
						//Se coloca dentro de try porque este campo no existe cuando la empresa es nueva
						$("#VenceCert").dxTextBox({ validationRules: [] });
					} catch (e) { }

				} else {
					$('#PanelFirmaFacturador').show();
					//Coloco como requerido el proveedor del certificado
					//$("#cboProveedor").dxValidator({
					//	validationRules: [{
					//		type: "required",
					//		message: "Debe seleccionar el proveedor del certificado"
					//	}]
					//});
					////Coloco como requerida la clave del certificado
					//$("#ClaveCert").dxValidator({
					//	validationRules: [{
					//		type: "required",
					//		message: "Debe ingresar la clave del certificado"
					//	}]
					//});

					//if (id_seguridad != "") {
					//	$("#VenceCert").dxTextBox(
					//	{
					//		validationRules: [{
					//			type: "required",
					//			message: "Debe consultar los datos del certificado para obtener la fecha de vencimiento"
					//		}]
					//	});
					//}

				}
			}
		});
		//.dxValidator({
		//	validationRules: [{
		//		type: "required",
		//		message: "Debe indicar quien firma con el certificado"
		//	}]
		//});


		$("#Certificado").dxFileUploader({
			multiple: false,
			allowedFileExtensions: [".pfx", ".p12"],
			uploadMode: "instantly",
			readyToUploadMessage: "Certificado Digital subido exitosamente",
			uploadUrl: "/api/SubirArchivo?StrIdSeguridad=" + id_seguridad + "&Clave=" + Datos_ClaveCert + "&Certificadora=" + Datos_proveedores,
			//onValueChanged: function (e) {
			//	console.log(e);
			//	var files = e.value;
			//	if (files.length > 0) {
			//		SrvEmpresas.ObtenerInfCert(id_seguridad, Datos_ClaveCert).then(function (data) {
			//			Datos_FechaCert = data.FechaVencimiento;
			//			$("#VenceCert").dxTextBox({ value: Datos_FechaCert });
			//			showInfo(data);
			//		});
			//	}
			//	else
			//		$("#selected-files").hide();
			//}
		}
		);


		$("#Hgi_Responsable").dxCheckBox({
			name: "Hgi_Responsable",
			onValueChanged: function (data) {
				Datos_Hgi_Responsable = data.value;
			}
		});

		$("#Hgi_Notifica").dxCheckBox({
			name: "Hgi_Notifica",
			onValueChanged: function (data) {
				Datos_Hgi_Notifica = data.value;
			}
		});


		$("#ClaveCert").dxTextBox({
			mode: "password",
			placeholder: "Contraseña",
			showClearButton: true,
			onValueChanged: function (data) {
				Datos_ClaveCert = data.value;
				$("#Certificado").dxFileUploader({
					multiple: false,
					allowedFileExtensions: [".pfx", ".p12"],
					uploadMode: "instantly",
					selectButtonText: "Seleccione el Certificado Digital",
					uploadedMessage: "Certificado guardado exitosamente",
					uploadUrl: "/api/SubirArchivo?StrIdSeguridad=" + id_seguridad + "&Clave=" + Datos_ClaveCert + "&Certificadora=" + Datos_proveedores,
					onUploaded: function (e) {
						SrvEmpresas.ObtenerInfCert(id_seguridad, Datos_ClaveCert, Datos_proveedores).then(function (data) {
							Datos_FechaCert = data.FechaVencimiento;
							$("#VenceCert").dxTextBox({ value: Datos_FechaCert });
							showInfo(data);
							//Se valida si la empresa no es administradora ya que entonces se coloca como responsable la empresa que sube el certificado y no hgi
							if (!$scope.Admin) {
								Datos_Hgi_Responsable = false;
								Datos_Hgi_Notifica = true;
								Datos_CertFirma = 1;
							}
						});
					}
					,
					onUploadError: function (e) {
						var datos = JSON.parse(e.request.response);
						DevExpress.ui.notify(datos.ExceptionMessage, 'error', 6000);
					}
				});
				//Si se coloco una clave en el certificado, entonces 
				//if (Datos_ClaveCert != undefined && Datos_ClaveCert != "") {
				//	$("#cboProveedor").dxValidator({
				//		validationRules: [{
				//			type: "required",
				//			message: "Debe seleccionar el proveedor del certificado"
				//		}]
				//	});
				//} else {
				//	$("#cboProveedor").dxValidator({});
				//}
			}
		});

		$("#VenceCert").dxTextBox({
			placeholder: "Fecha de vencimiento",
			format: "yyyy-MM-dd HH:mm",
			readOnly: true,
			onValueChanged: function (data) {
				Datos_FechaCert = data.value;
			}
		});





		/////////////////////////////////////Tooltip
		$("#ttEmail").dxPopover({
			target: "#txtEmail",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Email Administrativo:",
			contentTemplate: function (data) {
				data.html("<br/>Se enviarán las notificaciones <br/> propias de la plataforma como registro, recargas, saldos, boletines, etc.");
			}
		});
		$("#ttMailEnvio").dxPopover({
			target: "#txtMailEnvio",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Email Envío Documentos:",
			contentTemplate: function (data) {
				data.html("<br/>Se utilizará para el envío de los correos electrónicos a los Adquirientes.");
			}
		});
		$("#ttMailRecepcion").dxPopover({
			target: "#txtMailRecepcion",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Email Recepción Documentos:",
			contentTemplate: function (data) {
				data.html("<br/>Se utilizará para la recepción de correos electrónicos como Adquiriente.");
			}
		});
		$("#ttMailAcuse").dxPopover({
			target: "#txtMailAcuse",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Email Recepción Acuse de Recibo:",
			contentTemplate: function (data) {
				data.html("<br/>Se enviarán las respuestas realizadas por los Adquirientes al realizar el acuse de recibo.");
			}
		});
		$("#ttMailPagos").dxPopover({
			target: "#txtMailPagos",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Email de Pagos:",
			contentTemplate: function (data) {
				data.html("<br/>Se enviarán las notificaciones al recibir un pago electrónico por la Plataforma de Servicios.");
			}
		});



		$("#tooltip_cboVersionDIAM").dxPopover({
			target: "#cboVersionDIAM",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});


		$("#tooltip_Integradora").dxPopover({
			target: "#Integradora",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});

		$("#tooltip_Facturador").dxPopover({
			target: "#Facturador",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});

		$("#tooltip_Adquiriente").dxPopover({
			target: "#Adquiriente",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});

		$("#tooltip_cboestado").dxPopover({
			target: "#cboestado",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});

		$("#tooltip_txtobservaciones").dxPopover({
			target: "#txtobservaciones",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});


		$("#tooltip_Habilitacion").dxPopover({
			target: "#Habilitacion",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});

		$("#tooltip_txtUsuarios").dxPopover({
			target: "#txtUsuarios",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});


		$("#tooltip_txtHorasAcuse").dxPopover({
			target: "#txtHorasAcuse",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});


		$("#tooltip_Anexo").dxPopover({
			target: "#Anexo",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});


		$("#tooltip_EmailRecepcion").dxPopover({
			target: "#EmailRecepcion",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});


		$("#tooltip_postpagoaut").dxPopover({
			target: "#postpagoaut",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});


		$("#tooltip_debug").dxPopover({
			target: "#debug",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});


		$("#tooltip_EmpresaAsociada").dxPopover({
			target: "#EmpresaAsociada",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
		});


		$("#tooltip_EmpresaDescuenta").dxPopover({
			target: "#EmpresaDescuenta",
			showEvent: {
				name: "mouseenter",
				delay: 500
			},
			hideEvent: "mouseleave",
			position: "bottom",
			width: 300,
			showTitle: true,
			title: "Detalle:"
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


		$("#form1").on("submit", function (e) {
			guardarEmpresa();
			e.preventDefault();
		});

		$("#button").dxButton({
			text: "Guardar",
			type: "default",
			onClick: function (e) {
				//Validamos si es administrador
				if (Datos_CertFirma == "1" || Datos_CertFirma == 1) {
					if (Datos_ClaveCert != undefined && Datos_ClaveCert != "") {
						//Si es administrador, validamos si la clave del certificado es correcta
						if (Datos_proveedores != undefined && Datos_proveedores != "") {
							SrvEmpresas.ObtenerInfCert(id_seguridad, Datos_ClaveCert, Datos_proveedores).then(function (data) {
								//Si la clave es correcta, habilitamos el viaje del formulario al servidor
								$("#button").dxButton({ useSubmitBehavior: true });
								$("#button").click();
							}, function (response) {
								//Si no es correcta la clave, debe salir un mensaje del interceptor indicando el error, e imprimimos en consola el error.
								console.log(response);
							});
						} else {
							DevExpress.ui.notify('Debe seleccionar el proveedor del certificado', 'error', 7000);
						}
					} else {
						$("#button").dxButton({ useSubmitBehavior: true });
						$("#button").click();
					}
				} else {
					//Si no es administrador, no validamos esos datos y simplemente habilitamos el viaje de los datos del formulario, al servidor
					$("#button").dxButton({ useSubmitBehavior: true });
					$("#button").click();
				}
			}
		});

	};




	//Funciones
	function validar() {
		if (Datos_Adquiriente == true || Datos_Obligado == true) {
			return false;
		}
		return true;
	}

	//Validación de Email
	function validarEmailEnvio() {
		if (Datos_estado != 3) {
			if (Datos_Email_Envio == "" || Datos_Email_Envio == undefined) {
				return false;
			}
		} else {
			return true;
		}

		return true;
	}

	function validarEmailRecepcion() {
		if (Datos_estado != 3) {
			if (Datos_Email_Recepcion == "" || Datos_Email_Recepcion == undefined) {
				return false;
			}
		} else {
			return true;
		}

		return true;
	}

	function validarEmailAcuse() {
		if (Datos_estado != 3) {
			if (Datos_Email_Acuse == "" || Datos_Email_Acuse == undefined) {
				return false;
			}
		} else {
			return true;
		}

		return true;
	}

	function validarEmailPagos() {
		if (Datos_estado != 3) {
			if (Datos_Email_Pagos == "" || Datos_Email_Pagos == undefined) {
				return false;
			}
		} else {
			return true;
		}

		return true;
	}

	function validarHorasAcuse() {
		if (Datos_Horas_Acuse == 0) {
			return true;
		}
		if (Datos_Horas_Acuse != 0 && Datos_Horas_Acuse < 72) {
			return false;
		}
		return true;
	}


	//Si es una nueva empresa, por defecto debe tener 
	if (id_seguridad == '' || id_seguridad == undefined) {
		$("#CerFirma").dxRadioGroup({ value: TiposCertFirma[BuscarID(TiposCertFirma, 0)] });
		$("#CerFirma").dxRadioGroup({ readOnly: true });

	}



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
				Datos_Serial = response.data[0].Serial;
				Datos_empresa_Asociada = response.data[0].StrEmpresaAsociada;
				Datos_Integrador = response.data[0].IntIntegrador;
				Datos_Numero_usuarios = response.data[0].IntNumUsuarios;
				Datos_Horas_Acuse = response.data[0].IntAcuseTacito;
				Datos_Anexo = response.data[0].IntAnexo;
				Datos_EmailRecepcion = response.data[0].IntEmailRecepcion;
				Datos_estado = response.data[0].Estado;
				Datos_postpago = response.data[0].Postpago;
				Datos_Email_Envio = response.data[0].StrMailEnvio;
				Datos_Email_Recepcion = response.data[0].StrMailRecepcion;
				Datos_Email_Acuse = response.data[0].StrMailAcuse;
				Datos_Email_Pagos = response.data[0].StrMailPagos;
				Datos_telefono = response.data[0].telefono;
				Datos_VersionDIAN = response.data[0].VersionDIAN;

				Datos_Serial_Cloud = response.data[0].SerialCloudServices;
				Datos_debug = response.data[0].Debug;

				//Certificado				
				Datos_CertFirma = response.data[0].IntCertFirma;
				Datos_proveedores = response.data[0].IntCertProveedor;
				Datos_Hgi_Responsable = response.data[0].IntCertResponsableHGI;
				Datos_Hgi_Notifica = response.data[0].IntCertNotificar;
				Datos_ClaveCert = response.data[0].StrCertClave;
				Datos_FechaCert = (response.data[0].DatCertVence == "0001-01-01" || response.data[0].DatCertVence == "") ? "" : response.data[0].DatCertVence;
				//Certificado

				//Se guardan variables para poder identifcar si algún correo tiene un cambio
				Copia_Email_Administracion = Datos_Email;
				Copia_Email_Recepcion = Datos_Email_Recepcion;
				Copia_Email_Acuse = Datos_Email_Acuse;
				Copia_Email_Envio = Datos_Email_Envio;
				Copia_Email_Pagos = Datos_Email_Pagos;

				$("#NumeroIdentificacion").dxTextBox({ value: Datos_Idententificacion });
				$("#NumeroIdentificacion").dxTextBox({ readOnly: true });
				$("#txtRasonSocial").dxTextBox({ value: Datos_Razon_Social });

				$("#TipoIndentificacion").dxSelectBox({ value: TiposIdentificacion[BuscarID(TiposIdentificacion, Datos_Tipoidentificacion)] });
				$("#TipoIndentificacion").dxSelectBox({ readOnly: true });

				$("#txttelefono").dxTextBox({ value: Datos_telefono });
				$("#txtHorasAcuse").dxNumberBox({ value: Datos_Horas_Acuse });
				if (Datos_Anexo == 1) {
					$("#Anexo").dxCheckBox({ value: true });
				}

				//Proceso de validación de Correo****************************
				//Si la Empresa esta activa, no hago validaciones de ningún tipo

				Proc_Email = response.data[0].Proc_Email;
				Proc_MailEnvio = response.data[0].Proc_MailEnvio;
				Proc_MailRecepcion = response.data[0].Proc_MailRecepcion;
				Proc_MailAcuse = response.data[0].Proc_MailAcuse;
				Proc_MailPagos = response.data[0].Proc_MailPagos;
				//Guardo Copia en caso de cambios en la vista
				Copia_Proc_Email = Proc_Email;
				Copia_Proc_MailEnvio = Proc_MailEnvio;
				Copia_Proc_MailRecepcion = Proc_MailRecepcion;
				Copia_Proc_MailAcuse = Proc_MailAcuse;
				Copia_Proc_MailPagos = Proc_MailPagos;

				//***********************************************************				

				//Si es administrador o integrador
				if ($scope.Admin) {
					Set_EmpresaAsociada((Datos_empresa_Asociada) ? Datos_empresa_Asociada : '');
					Set_EmpresaDescuenta((response.data[0].StrEmpresaDescuenta) ? response.data[0].StrEmpresaDescuenta : Datos_Idententificacion)
					if (Datos_Observaciones != null) {
						$("#txtobservaciones").dxTextArea({ value: Datos_Observaciones });
					}

					if (Datos_Serial != null) {
						$("#txtSerial").dxTextBox({ value: Datos_Serial });
					}

					if (Datos_Adquiriente == 1) {
						$("#Adquiriente").dxCheckBox({ value: 1 });
					}
					if (Datos_Integrador == 1)
						$("#Integradora").dxCheckBox({ value: true });

					$("#txtUsuarios").dxNumberBox({ value: Datos_Numero_usuarios });

					if (Datos_Obligado == 1) {
						$("#Facturador").dxCheckBox({ value: 1 });
						$("#Habilitacion").dxRadioGroup({ value: TiposHabilitacion[BuscarID(TiposHabilitacion, response.data[0].Habilitacion)] });
					}

					Datos_EmailRecepcion = true;//Se coloca como true la variable ya que este campo debe estar en True y de solo lectura, solicitud: mparamo
					if (Datos_EmailRecepcion == true) {
						$("#EmailRecepcion").dxCheckBox({ value: true });
					} else {
						$("#EmailRecepcion").dxCheckBox({ value: false });
					}

					$("#cboestado").dxSelectBox({ value: TiposEstado[BuscarID(TiposEstado, Datos_estado)] });

					$("#cboVersionDIAM").dxSelectBox({ value: VersionDIAN[BuscarID(VersionDIAN, Datos_VersionDIAN)] });


					if (Datos_postpago == 1) {
						$("#postpagoaut").dxCheckBox({ value: true });
					}


					//Serial Cloud
					try {
						$("#txtSerialCloud").dxTextBox({ value: Datos_Serial_Cloud });
					} catch (e) { }


					try {
						if (Datos_debug == 1) {
							$("#debug").dxCheckBox({ value: true });
						}
					} catch (e) { }


					//Certificado
					try {
						$("#CerFirma").dxRadioGroup({ value: TiposCertFirma[BuscarID(TiposCertFirma, Datos_CertFirma)] });
					} catch (e) { }

					try {
						$("#Hgi_Notifica").dxCheckBox({ value: Datos_Hgi_Notifica });
					} catch (e) { }
					try {
						$("#Hgi_Responsable").dxCheckBox({ value: Datos_Hgi_Responsable });
					} catch (e) { }
				}


				if (Datos_CertFirma == "1") {
					$scope.ReponsableFacturadorCertificado = true;
				}

				try {
					$("#cboProveedor").dxSelectBox({ value: Lista_Proveedores_Firma[BuscarID(Lista_Proveedores_Firma, Datos_proveedores)] });
				} catch (e) { }
				try {
					$("#ClaveCert").dxTextBox({ value: Datos_ClaveCert });
				} catch (e) { }
				try {
					$("#VenceCert").dxTextBox({ value: Datos_FechaCert });
				} catch (e) { }


				$("#txtEmail").dxTextBox({ value: Datos_Email });
				$("#txtMailEnvio").dxTextBox({ value: Datos_Email_Envio });
				$("#txtMailRecepcion").dxTextBox({ value: Datos_Email_Recepcion });
				$("#txtMailAcuse").dxTextBox({ value: Datos_Email_Acuse });
				$("#txtMailPagos").dxTextBox({ value: Datos_Email_Pagos });

				AsigEstado("Proc_Email", Proc_Email);
				AsigEstado("Proc_MailEnvio", Proc_MailEnvio);
				AsigEstado("Proc_MailRecepcion", Proc_MailRecepcion);
				AsigEstado("Proc_MailAcuse", Proc_MailAcuse);
				AsigEstado("Proc_MailPagos", Proc_MailPagos);


			} catch (err) {
				DevExpress.ui.notify(err.message + ' Validar Estado Producción', 'error', 7000);
			}
		});
	}

	function AsigEstado(mail, Proceso) {
		var Icono_Registro = "icon-cross2 text-danger-400";
		var Icono_Verificíon = "icon-circles text-orange";
		var Icono_Activo = "icon-checkmark3 text-success";
		var Icono = "";
		var Titulo = "";

		if (Proceso == 0) {
			Titulo = "En proceso de Registro";
			Icono = Icono_Registro;
		}

		if (Proceso == 1) {
			Titulo = "En proceso de Verificación";
			Icono = Icono_Verificíon;
		}

		if (Proceso == 2) {
			Titulo = "Correo Verificado";
			Icono = Icono_Activo;
		}

		$('#Html' + mail).attr("title", Titulo);
		$('#Html' + mail).removeClass();
		$('#Html' + mail).addClass(Icono);

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
		//Si es administrador o integrador
		if ($scope.Admin) {
			if (txt_hgi_EmpresaAsociada == undefined || txt_hgi_EmpresaAsociada == '') {
				Campo_Invalido_EmpresaAsociada();
				return false;
			}
			if (txt_hgi_EmpresaDescuenta == undefined || txt_hgi_EmpresaDescuenta == '') {
				Campo_Invalido_EmpresaDescuenta();
				return false;
			}
		}
		return true;
	}


	function guardarEmpresa() {
		if (ValidarAsociadaDescuenta()) {
			var empresa = null;
			var Asociada = "";
			var empresaDescuenta = 0;
			//Si es administrador o integrador
			if ($scope.Admin) {
				if (txt_hgi_EmpresaAsociada != null && txt_hgi_EmpresaAsociada != "") {
					Asociada = txt_hgi_EmpresaAsociada;
				} else {
					empresa = Datos_Idententificacion;
					Asociada = empresa;
				}
				empresaDescuenta = txt_hgi_EmpresaDescuenta;
			}

			var ObjEmpresa = ({
				StrIdSeguridad: id_seguridad,
				StrTipoIdentificacion: Datos_Tipoidentificacion,
				StrIdentificacion: Datos_Idententificacion,
				StrRazonSocial: Datos_Razon_Social,
				StrMailAdmin: Datos_Email,
				IntAdquiriente: (Datos_Adquiriente) ? true : false,
				IntObligado: (Datos_Obligado) ? true : false,
				IntHabilitacion: Datos_Habilitacion,
				StrEmpresaAsociada: Asociada,
				StrObservaciones: Datos_Observaciones,
				StrSerial: Datos_Serial,
				IntIntegrador: Datos_Integrador,
				IntNumUsuarios: Datos_Numero_usuarios,
				IntAcuseTacito: Datos_Horas_Acuse,
				IntManejaAnexos: Datos_Anexo,
				IntEnvioMailRecepcion: Datos_EmailRecepcion,
				StrEmpresaDescuento: empresaDescuenta,
				IntIdEstado: Datos_estado,
				IntCobroPostPago: Datos_postpago,
				StrMailEnvio: Datos_Email_Envio,
				StrMailRecepcion: Datos_Email_Recepcion,
				StrMailAcuse: Datos_Email_Acuse,
				StrMailPagos: Datos_Email_Pagos,
				StrTelefono: Datos_telefono,
				IntVersionDian: Datos_VersionDIAN,
				IntMailAdminVerificado: Proc_Email,
				IntMailEnvioVerificado: Proc_MailEnvio,
				IntMailRecepcionVerificado: Proc_MailRecepcion,
				IntMailAcuseVerificado: Proc_MailAcuse,
				IntMailPagosVerificado: Proc_MailPagos,
				IntCertFirma: Datos_CertFirma,
				IntCertProveedor: Datos_proveedores,
				IntCertResponsableHGI: Datos_Hgi_Responsable,
				IntCertNotificar: Datos_Hgi_Notifica,
				StrCertClave: Datos_ClaveCert,
				DatCertVence: Datos_FechaCert,
				StrSerialCloudServices: Datos_Serial_Cloud,
				IntDebug: Datos_debug
			});
			var tipo = Datos_Tipo;

			$http({ url: '/api/GuardarEmpresa/', data: ObjEmpresa, method: 'Post' }).then(function (response) {
				//$http.post('/api/Empresas?' + data).then(function (response) {
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
EmpresasApp.controller('ConsultaEmpresasController', function ConsultaEmpresasController($scope, $http, $rootScope, $location, SrvEmpresas, SrvFiltro) {

	var Empresas = [];


	var AlmacenEmpresas = new DevExpress.data.ArrayStore({
		key: "Identificacion",
		data: Empresas
	});

	var estado;
	var Item_TipoTercero = 0;

	$scope.Admin = false;

	$scope.filtros =
    {
    	TipoTercero: {
    		searchEnabled: true,
    		//Carga la data del control
    		dataSource: new DevExpress.data.ArrayStore({
    			data: items_TipoTercero,
    			key: "ID"
    		}),
    		displayExpr: "Texto",
    		Enabled: true,
    		placeholder: "Facturador",
    		onValueChanged: function (data) {
    			Item_TipoTercero = data.value.ID;
    		}
    	}
    }

	SrvFiltro.ObtenerFiltro('Documento Facturador', 'Facturador', 'icon-user-tie', 115, '/api/Empresas?Facturador=true', 'Identificacion', 'RazonSocial', false, 2).then(function (Datos) {
		$scope.Facturador = Datos;
		txt_hgi_Facturador = "";
	});

	$("#razon_social").dxTextBox({
		value: ""
	});




	$http.get('/api/DatosSesion/').then(function (response) {
		$('#wait').hide();
		if (response.data[0].Admin == false && response.data[0].Integrador == false) {
			window.location.assign("../Pages/GestionEmpresas.aspx?IdSeguridad=" + response.data[0].IdSeguridad);
		}

		codigo_facturador = response.data[0].Identificacion;
		var tipo = response.data[0].Admin;
		if (tipo) {
			$scope.Admin = true;
		} else {
			consultar();
		}


	}, function errorCallback(response) {
		$('#wait').hide();
		DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
	});




	//consultar();
	function consultar() {
		$('#wait').hide();

		SrvEmpresas.ObtenerEmpresas(codigo_facturador, Desde, Hasta, Item_TipoTercero, txt_hgi_Facturador, $("#razon_social").dxTextBox("instance").option().value).then(function (data) {
			$('#wait').hide();
			$('#waitRegistros').show();
			//*******Consulta de los primeros 20 registros
			Empresas = [];
			AlmacenEmpresas = new DevExpress.data.ArrayStore({
				key: "Identificacion",
				data: Empresas
			});

			cargarEmpresas(data);

			$("#gridEmpresas").dxDataGrid({
				dataSource: {
					store: AlmacenEmpresas,
					reshapeOnPush: true
				},
				keyExpr: "Identificacion",
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
						  		}
						  		if (options.data.Estado == 2) {
						  			estado = " style='color:red; cursor:default;' title='Inactivo'";
						  		}
						  		if (options.data.Estado == 3) {
						  			estado = " style='color:orange; cursor:default;' title='En proceso de registro'";
						  		}

						  	} catch (err) {

						  	}

						  }, allowColumnResizing: true
				, onToolbarPreparing: function (e) {
					var dataGrid = e.component;

					e.toolbarOptions.items.unshift({

						location: "after",
						widget: "dxButton",
						options: {
							icon: "refresh",
							onClick: function () {
								consultar();
							}
						}
					})
				}
				   , columns: [
					   {
					   	cssClass: "col-md-1 col-xs-2",
					   	cellTemplate: function (container, options) {
					   		var ver = "class='icon-user-tie' style='margin-left:5%; font-size:19px;color: #1E88E5;'";
					   		$("<div title='Detalle : " + options.data.RazonSocial + "' >")
					   		$("<div style='text-align:center'>")
								.append($("<a taget=_self class='icon-pencil3' title='Editar' href='GestionEmpresas.aspx?IdSeguridad=" + options.data.IdSeguridad + "'>"))




								.append(
									$("<i " + ver + "></i>").dxButton({
										onClick: function () {
											$rootScope.ConsultaDetalleEmpresa(options.data.IdSeguridad);
										}
									}).removeClass("dx-button dx-button-normal dx-widget")
							)
								.appendTo(container);
					   	}
					   }

					   ,

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
					   }

				   ],
				filterRow: {
					visible: true
				}
			});

			//*************************************************************************				
			CantRegCargados = AlmacenEmpresas._array.length;

			CargarAsyn();
			function CargarAsyn() {
				SrvEmpresas.ObtenerEmpresas(codigo_facturador, CantRegCargados, CantidadRegEmpresa, Item_TipoTercero, txt_hgi_Facturador, $("#razon_social").dxTextBox("instance").option().value).then(function (data) {
					CantRegCargados += data.length;
					if (data.length > 0) {
						cargarEmpresas(data);
						CargarAsyn();
					} else {
						$('#waitRegistros').hide();
					}

				});
			}
			//*************************************************************************


		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});




	}


	//Boton Consultar
	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultar();
		}
	};


	//Carga las empresas al array
	function cargarEmpresas(data) {
		if (data != "") {
			data.forEach(function (d) {
				Empresas = d;
				AlmacenEmpresas.push([{ type: "insert", data: Empresas }]);
			});
		}
	}

});







//Esta funcion es para ir a la pagina de consulta
function IrAConsulta() {
	if (Regresa_Listado) {
		window.location.assign("../Pages/ConsultaEmpresas.aspx");
	} else {
		window.location.assign("../Pages/GestionEmpresas.aspx?IdSeguridad=" + id_seguridad);
	}


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
	{ ID: "2", Texto: 'INACTIVO' },
	{ ID: "3", Texto: 'EN PROCESO DE REGISTRO' }
];


var VersionDIAN =
[
	{ ID: "1", Texto: 'Versión 1' },
	{ ID: "2", Texto: 'Versión Validación Previa' }
];


var TiposCertFirma =
[
	{ ID: "0", Texto: 'HGI SAS' },
	{ ID: "1", Texto: 'FACTURADOR' }
];


var items_TipoTercero =
[
	{ ID: "2", Texto: 'TODOS' },
	{ ID: "0", Texto: 'FACTURADOR' },
	{ ID: "1", Texto: 'ADQUIRIENTE' }
];