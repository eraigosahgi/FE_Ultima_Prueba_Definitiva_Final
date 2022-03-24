var opc_pagina = "1335";

var ModalAsignarFormatoApp = angular.module('ModalAsignarFormatoApp', []);
var GestionCompraPlanesApp = angular.module('GestionCompraPlanesApp', ['ModalAsignarFormatoApp', 'dx', 'AppSrvFormatos']);
GestionCompraPlanesApp.controller('GestionCompraPlanesController', function GestionCompraPlanesController($scope, $sce, $http, $location, SrvFormatos) {

	//$('#modal_asignar_formato').modal('show');

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

						$scope.CargarFormatos();
					}

				} catch (err) {
					DevExpress.ui.notify(err.message, 'error', 3000);
				}
			}, function errorCallback(response) {
				$('#wait').hide();
				DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			});

		});

	}), function errorCallback(response) {
		Mensaje(response.data.ExceptionMessage, "error");
	};

	

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

});





function reportDesigner_CustomizeMenuActions(s, e) {

	/*e.Actions.push({
		text: 'Exportar Diseño',
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

