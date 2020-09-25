var contenido_archivo_xml = "", opc_pagina = "1335";

var ModalAsignarFormatoApp = angular.module('ModalAsignarFormatoApp', []);
var GestionReportesApp = angular.module('GestionReportesApp', ['ModalAsignarFormatoApp', 'dx', 'AppSrvFormatos']);
GestionReportesApp.controller('GestionReportesController', function GestionReportesController($scope, $sce, $http, $location, SrvFormatos) {

	$('#modal_asignar_formato').modal('show');

	var identificacion_empresa_autenticada = "";
	var opc_crear = false, opc_editar = false, opc_gestion = false;
	$http.get('/api/DatosSesion/').then(function (response) {

		identificacion_empresa_autenticada = response.data[0].Identificacion;

		$http.get('/api/SesionDatosUsuario/').then(function (response) {

			$http.get('/api/Permisos?codigo_usuario=' + response.data[0].Usuario + '&identificacion_empresa=' + identificacion_empresa_autenticada + '&codigo_opcion=' + opc_pagina).then(function (response) {
				$("#wait").hide();
				try {

					if (response.data.length > 0) {
						opc_crear = response.data[0].Agregar;
						opc_editar = response.data[0].Editar;
						opc_gestion = response.data[0].Gestion;

						if (!opc_crear)
							$("#BtnCrearFormato").hide();
					}

				} catch (err) {
					DevExpress.ui.notify(err.message, 'error', 3000);
				}
			}, function errorCallback(response) {
				$('#wait').hide();
				DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			});

		});


		$scope.CargarFormatos();

	}), function errorCallback(response) {
		Mensaje(response.data.ExceptionMessage, "error");
	};

	//Ejecuta la consulta de formatos de la empresa autenticada.
	$scope.CargarFormatos = function () {
		$http.get('/Api/FormatosPdfEmpresa?identificacion_empresa=' + identificacion_empresa_autenticada).then(function (response) {
			$("#wait").hide();
			try {

				$scope.FormatosPdfEmpresa = response.data;

			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}

	//Evento para al edición del formato.
	$scope.BtnEditar = function (CodFormato, NitEmpresa, Estado) {

		var mensaje_notificacion = "";

		if (Estado == 3 || Estado == 4 || Estado == 5)
			mensaje_notificacion = "Si edita el formato deberá realizar nuevamente el proceso de solicitud de aprobación.";
		else
			mensaje_notificacion = "Si edita el formato deberá realizar el proceso de solicitud de aprobación.";

		var myDialog = DevExpress.ui.dialog.custom({
			title: "¿Desea Editar el Formato?",
			message: mensaje_notificacion,
			buttons: [{
				text: "Aceptar",
				onClick: function (e) {
					window.location.href = '/Views/ReportDesigner/ReportDesignerWeb.aspx?ID=' + CodFormato + '&Nit=' + NitEmpresa;
				}
			},
			{
				text: "Cancelar",
				onClick: function (e) {
					myDialog.hide();
				}
			}]
		});
		myDialog.show().done(function (dialogResult) {
		});

	};

	//Evento para el cambio de estado Activo/Inactivo
	$scope.BtnCambioEstado = function (estado, nit, codigo) {

		//int id_formato, string identificacion_empresa, bool estado_actual
		$http.post('/api/ActualizarEstadoFormato?id_formato=' + codigo + '&identificacion_empresa=' + nit + '&estado_actual=' + estado + '&tipo_proceso=3' + '&observaciones=' + observaciones).then(function (response) {
			$("#wait").hide();
			try {

				DevExpress.ui.notify({ message: "Estado Actualizado Correctamente.", position: { my: "center top", at: "center top" } }, "success", 1500);

				$scope.CargarFormatos();

			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response, 'error', 6000);
		});

		return false;
	};

	//Coloca la clase del panel según el estado.
	$scope.ClasePanel = function (estado) {
		if (estado == 0)
			return "panel-danger"
		else if (estado == 1)
			return "panel-success";
		else
			return "panel-default";
	}

	//Carga opción Asignar
	$scope.OpcionAsignar = function (estado) {
		if (!opc_crear)
			return "ng-hide"
		else {
			if (estado == 0 || estado == 2 || estado == 3 || estado == 4 || estado == 5 || estado == 6)
				return "ng-hide"
		}
	}

	//Carga opción Cambio Estado
	$scope.OpcionCambioEstado = function (estado) {
		if (!opc_editar)
			return "ng-hide"
		else {
			if (estado == 2 || estado == 3 || estado == 4 || estado == 5 || estado == 6)
				return "ng-hide"
		}
	}

	//Carga opción Editar
	$scope.OpcionEditar = function (generico) {
		if (!opc_editar)
			return "ng-hide"
		else if (generico)
			return "ng-hide"
	}

	//Carga opción Aprobar
	$scope.OpcionAprobar = function (estado, admin) {
		if (!opc_gestion)
			return "ng-hide"
		else if (!admin)
			return "ng-hide"
		else if (estado != 2 && estado != 3)
			return "ng-hide"
	}

	//Carga opción publicar formato.
	$scope.OpcionPublicar = function (estado) {
		if (!opc_gestion)
			return "ng-hide"
		else if (estado != 5)
			return "ng-hide"
	}

	$scope.filtros = {
		FiltrarFormatos: {
			placeholder: "Ingrese Identificación de la Empresa",
		}
	};

	//Evento para la carga del modal de creación/asignación de formatos.
	$scope.AbrirModal = function (cod, nit) {

		$scope.CodigoFormato = cod;
		$scope.EmpresaFormato = nit;

		$('#modal_asignar_formato').modal('show');
	};

	//Modal de solicitud de aprobación del formato.
	$scope.SolicitarAprobacion = function (codigo, nit, estado) {
		$("#SolicitudAprobacion").show();
		$("#AprobacionFormato").hide();
		$("#EnvioMailPrueba").hide();

		$("#CampoObservaciones").show();
		$("#CampoMailPrueba").hide();

		$('#LblTituloModal').text("Solicitud Aprobación de Formato");

		$scope.ObservacionesSolicitud = "";
		//Campo de observaciones
		$("#TxtObservacionesSolicitud").dxTextArea({
			value: "",
			onValueChanged: function (data) {
				$scope.ObservacionesSolicitud = data.value.toUpperCase();
			}
		}).dxValidator({
			validationRules: [
			{
				type: "stringLength",
				max: 200,
				message: "El campo Observación no puede ser mayor a 500 caracteres"
			}]
		});


		$("#BtnSolicitarAprobacion").dxButton({
			text: "Solicitar",
			type: "default",
			onClick: function (e) {
				ActualizarFormato(codigo, nit, estado, 4, $scope.ObservacionesSolicitud);

				$('#modal_solicitar_aprobacion').modal('hide');
			}
		});

		$('#modal_solicitar_aprobacion').modal('show');
	}

	//Aprobación del diseño del formato.
	$scope.AprobarFormato = function (codigo, nit, estado) {

		$("#SolicitudAprobacion").hide();
		$("#AprobacionFormato").show();

		$("#CampoObservaciones").show();
		$("#CampoMailPrueba").hide();

		$('#LblTituloModal').text("Aprobación de Formato");

		$("#TxtObservacionesSolicitud").dxTextBox({
			value: "",
			validationGroup: "ObservacionesRespuesta",
			onValueChanged: function (data) {
				console.log(data.value);
				$scope.ObservacionesSolicitud = data.value.toUpperCase();
			}
		}).dxValidator({
			validationRules: [
               {
               	type: "stringLength",
               	max: 200,
               	message: "El campo Observación no puede ser mayor a 500 caracteres"
               }, {
               	type: "required",
               	message: "Las observaciones son requeridas."
               }
			],
			validationGroup: "ObservacionesRespuesta",
		});

		$("#summary").dxValidationSummary({
			validationGroup: "ObservacionesRespuesta"
		});

		$("#BtnRechazarFormato").dxButton({
			text: "Rechazar",
			type: "danger",
			validationGroup: "ObservacionesRespuesta",
			onClick: function (params) {
				var continua_proceso = params.validationGroup.validate().isValid;
				console.log(continua_proceso);
				if (continua_proceso) {
					ActualizarFormato(codigo, nit, estado, 6, $scope.ObservacionesSolicitud);
					$('#modal_solicitar_aprobacion').modal('hide');
				}
			}
		});

		$("#BtnAprobarFormato").dxButton({
			text: "Aprobar",
			type: "success",
			onClick: function (e) {
				ActualizarFormato(codigo, nit, estado, 5, $scope.ObservacionesSolicitud);

				$('#modal_solicitar_aprobacion').modal('hide');
			}
		});


		$('#modal_solicitar_aprobacion').modal('show');
	}

	//Publicación del formato.
	$scope.PublicarFormato = function (codigo, nit, estado) {

		var myDialog = DevExpress.ui.dialog.custom({
			title: "Publicación de Formato",
			message: "¿Desea publicar el diseño del formato número " + codigo + " de la empresa " + nit + "?",
			buttons: [{
				text: "Aceptar",
				onClick: function (e) {
					ActualizarFormato(codigo, nit, estado, 7, "");
				}
			},
			{
				text: "Cancelar",
				onClick: function (e) {
					myDialog.hide();
				}
			}]
		});
		myDialog.show().done(function (dialogResult) {
		});

	}

	//Realiza el envío de un email con el formato de prueba.
	$scope.MailPrueba = function (codigo, nit) {

		$("#SolicitudAprobacion").show();
		$("#AprobacionFormato").hide();

		$("#CampoObservaciones").hide();
		$("#CampoMailPrueba").show();

		$('#LblTituloModal').text("Envío Formato de Prueba");

		$("#TxtNitEmpresa").dxTextBox({
			value: "",
			onValueChanged: function (data) {
				$scope.TxtNitEmpresa = data.value;
			}
		}).dxValidator({
			validationRules: [{
				type: "required",
				message: "Debe ingresar el nit de la empresa."
			}, {
				//Valida que el campo solo contenga números
				type: "pattern",
				pattern: "^[0-9]+$",
				message: "El campo no debe contener letras ni caracteres especiales."
			}],
			validationGroup: "FrmMailPruebaFormato",
		});

		$("#TxtPrefijoDoc").dxTextBox({
			value: "",
			onValueChanged: function (data) {
				$scope.TxtPrefijoDoc = data.value;
			}
		});

		$("#TxtNumeroDoc").dxTextBox({
			value: "",
			onValueChanged: function (data) {
				$scope.TxtNumeroDoc = data.value;
			}
		});

		$("#TxtMailPrueba").dxTextBox({
			value: "",
			onValueChanged: function (data) {
				$scope.TxtMailPrueba = data.value;
			}
		}).dxValidator({
			validationRules: [{
				type: "required",
				message: "Debe introducir el Email"
			}, {
				type: "stringLength",
				max: 200,
				message: "El email no puede ser mayor a 200 caracteres"
			}, {
				type: "email",
				message: "El email no tiene el formato correcto"
			}],
			validationGroup: "FrmMailPruebaFormato",
		});

		$("#summary").dxValidationSummary({
			validationGroup: "FrmMailPruebaFormato"
		});

		$("#BtnSolicitarAprobacion").dxButton({
			text: "Enviar",
			type: "default",
			validationGroup: "FrmMailPruebaFormato",
			onClick: function (params) {
				var continua_proceso = params.validationGroup.validate().isValid;
				if (continua_proceso) {
					EnviarMail(codigo, nit, $scope.TxtMailPrueba, $scope.TxtNitEmpresa, $scope.TxtPrefijoDoc, $scope.TxtNumeroDoc);
					$('#modal_solicitar_aprobacion').modal('hide');
				}
			}
		});

		$('#modal_solicitar_aprobacion').modal('show');

	}

	function EnviarMail(codigo, nit, mail, empresa_documento, prefijo, numero_documento) {

		prefijo = (prefijo == undefined) ? "" : prefijo;
		numero_documento = (numero_documento == undefined) ? "" : numero_documento;

		SrvFormatos.EnviarFormatoPrueba(codigo, nit, mail, empresa_documento, prefijo, numero_documento).then(function (data) {

			DevExpress.ui.notify({ message: "El mensaje ha sido enviado con éxito.", position: { my: "center top", at: "center top" } }, "success", 1500);

		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.ExceptionMessage, 'error', 6000);
		});
	}

	//Visualización de la auditoría del formato.
	$scope.VerAuditoria = function (codigo, nit) {

		$http.get('/api/AuditoriaFormatos?codigo_formato=' + codigo + '&identificacion_empresa=' + nit).then(function (response) {
			$("#wait").hide();
			try {
				$("#gridAuditoriaFormato").dxDataGrid({
					dataSource: response.data,
					paging: {
						pageSize: 10
					},
					pager: {
						showPageSizeSelector: true,
						allowedPageSizes: [5, 10, 20],
						showInfo: true
					}
			   , loadPanel: {
			   	enabled: true
			   },
					allowColumnResizing: true,
					allowColumnReordering: true,
					columns: [
						{
							caption: "Fecha Proceso",
							dataField: "DatFechaProceso",
							cssClass: "col-xs-3 col-md-3",
						},
						{
							caption: "Proceso",
							dataField: "StrProceso",
							cssClass: "col-xs-3 col-md-2",
						},
						{
							caption: "Usuario",
							dataField: "NombreUsuario",
							cssClass: "col-xs-3 col-md-3",
						}
						,
						{
							caption: "Observaciones",
							dataField: "StrObservaciones",
							cssClass: "col-xs-3 col-md-4",
						}
					]
				});

				$('#modal_auditoria_formato').modal('show');


			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response, 'error', 6000);
		});

	}

	//Función que ejecuta la actualización del formato.
	function ActualizarFormato(codigo, nit, estado, proceso, observaciones) {

		$http.post('/api/ActualizarEstadoFormato?id_formato=' + codigo + '&identificacion_empresa=' + nit + '&estado_actual=' + estado + '&tipo_proceso=' + proceso + '&observaciones=' + observaciones).then(function (response) {
			$("#wait").hide();
			try {

				switch (proceso) {
					case 4:
						DevExpress.ui.notify({ message: "Se ha enviado la solicitud de aprobación para el formato número" + codigo + " de la empresa " + nit + " con éxito.", position: { my: "center top", at: "center top" } }, "success", 1500);
						break;

					case 5:
						DevExpress.ui.notify({ message: "El formato número " + codigo + " de la empresa " + nit + " , ha sido aprobado.", position: { my: "center top", at: "center top" } }, "success", 1500);
						break;

					case 6:
						DevExpress.ui.notify({ message: "El formato número " + codigo + " de la empresa " + nit + " , ha sido rechazado.", position: { my: "center top", at: "center top" } }, "success", 1500);
						break;

					case 7:
						DevExpress.ui.notify({ message: "El formato número " + codigo + " de la empresa " + nit + ", ha sido publicado con éxito.", position: { my: "center top", at: "center top" } }, "success", 1500);
						break;
				}

				$scope.CargarFormatos();

			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response, 'error', 6000);
		});
	}

	$scope.ExportarFormato = function (id_formato, identificacion_empresa) {
		console.log(datos);

		$http.get('/Api/ObtenerFormato?id_formato=' + id_formato + '&identificacion_empresa=' + identificacion_empresa).then(function (response) {

			console.log(response.data);

			var nombre_archivo = "Formato" + response.data.NitEmpresa + "-" + response.data.CodigoFormato + ".xml";

			var pom = document.createElement('a');
			pom.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(response.data.FormatoXml));
			pom.setAttribute('download', nombre_archivo);

			if (document.createEvent) {
				var event = document.createEvent('MouseEvents');
				event.initEvent('click', true, true);
				pom.dispatchEvent(event);
			}
			else {
				pom.click();
			}

		}), function errorCallback(response) {
			Mensaje(response.data.ExceptionMessage, "error");
		};
	}

	$scope.ImportarFormato = function (datos) {

		$scope.CodFormatoImportar = datos.CodigoFormato;
		$scope.NitEmpresaImportar = datos.NitEmpresa;
		$scope.NombreEmpresaImportar = datos.RazonSocial;

		var summaryImportarFormato = "summaryImportarFormato";
		$("#summaryImportarFormato").dxValidationSummary({ validationGroup: summaryImportarFormato });

		$("#UploaderFormato").dxFileUploader({
			selectButtonText: "Seleccione el archivo XML",
			labelText: "",
			accept: "text/xml",
			uploadMode: "useForm",
			onValueChanged: function (e) {

				var fReader = new FileReader();

				if (e.value && e.value[0]) {
					fReader.readAsDataURL(e.value[0]);
					fReader.onloadend = function (event) {
						contenido_archivo_xml = event.target.result;
						contenido_archivo_xml = contenido_archivo_xml.replace('data:text/xml;base64,', '');
					}
				}
			}
		}).dxValidator({
			validationGroup: summaryImportarFormato,
			validationRules: [{ type: "required", message: "Debe seleccionar un archivo." }, {
				type: "custom", validationCallback: function (options) {
					if (options.value.length > 0) {
						var file = options.value[0];
						if (file.type != "text/xml") {
							options.rule.message = "Tipo de archivo inválido, solo se permite formato .xml";
							return false;
						}
					}
					return true;
				}
			}]
		});

		$("#BtnImportarFormato").dxButton({
			text: "Cargar Diseño",
			type: "default",
			validationGroup: summaryImportarFormato,
			onClick: function (e) {
				var result = e.validationGroup.validate();
				if (result.isValid) {

					var ObjFormato = ({
						CodigoFormato: datos.CodigoFormato,
						NitEmpresa: datos.NitEmpresa,
						FormatoB64: contenido_archivo_xml,
						IdSeguridad: datos.IdSeguridad
					});

					SrvFormatos.ImportarFormato(ObjFormato).then(function (data) {
						$('#modal_importar_formato').modal('hide');

						var myDialog = DevExpress.ui.dialog.custom({
							title: "Proceso éxitoso",
							message: "Se ha importado correctamente el diseño para el formato número " + datos.CodigoFormato + " de la empresa " + datos.NitEmpresa,
							buttons: [{
								text: "Editar",
								onClick: function (e) {
									window.location.href = '/Views/ReportDesigner/ReportDesignerWeb.aspx?ID=' + datos.CodigoFormato + '&Nit=' + datos.NitEmpresa;
								}
							},
							{
								text: "Cancelar",
								onClick: function (e) {
									myDialog.hide();
									$scope.CargarFormatos();
								}
							}]
						});
						myDialog.show().done(function (dialogResult) {
						});

					}, function errorCallback(response) {
						$('#wait').hide();
						DevExpress.ui.notify(response.ExceptionMessage, 'error', 6000);
					});

				}
			}
		});
		$('#modal_importar_formato').modal('show');
	}

});





function reportDesigner_CustomizeMenuActions(s, e) {

	/*e.Actions.push({
		text: 'Importar Diseño',
		container: 'menu',
		visible: true,
		disabled: false,
		clickAction: function () {
		},
		imageClassName: 'dxrd-image-export-to'
	});*/

	//Elimina la opción del diseñador mediante el asistente
	var Wizard = e.Actions.filter(function (x) { return x.imageClassName === 'dxrd-image-run-wizard' })[0];
	if (Wizard) {
		Wizard.visible = false;
	}
}



//Evento para la captura de excepciones ocurridas durante la construcción del formato
function reportDesigner_OnServerError(s, e) {
	var errorMessage = e.Error.data.error;
	DevExpress.ui.notify("Ha Ocurrido un Error Durante el Proceso: " + errorMessage, 'error', 20000);
}


//FUNCIÓN PARA EJECUTAR ALERTA DEL LADO DEL CLIENTE DESPUÉS DE UNA EJECUCIÓN SAVE.
var isReportSavingCallback = false;

function reportDesigner_SaveCommandExecute(s, e) {
	isReportSavingCallback = true;
}

function CargarAlerta(mensaje) {
	swal({
		title: 'Notificación',
		text: mensaje,
		type: 'warning',
		confirmButtonColor: '#FF5722',
		confirmButtonText: 'Aceptar',
		animation: 'pop',
		html: true,
	}, function () {
		//window.location = "/Views/Pages/GestionReportes.aspx";
	});
}

function reportDesigner_EndCallback(s, e) {

	if (isReportSavingCallback) {
		isReportSavingCallback = false;

		var ruta_regreso = s.cpRutaRegreso;


		var myDialog = DevExpress.ui.dialog.custom({
			title: s.cpTituloNotificacion,
			message: s.cpMensajeNotificacion,
			buttons: [{
				text: s.cpTextoBtnNotificacion,
				onClick: function (e) {
					window.location.href = '/Views/Pages/GestionReportes.aspx';
				}
			}, {
				text: 'Continuar Edición',
				visible: s.cpCargaContinuarEdicion,
				onClick: function (e) {
					myDialog.hide();
				}
			}
			]
		});
		myDialog.show().done(function (dialogResult) {
		});

	}
}



//Función del botón salir del diseñador
function ExitDesignerfunction(s, e) {
	window.location.href = '/Views/Pages/GestionReportes.aspx';
}

