DevExpress.localization.locale('es-ES');

var GestionReportesApp = angular.module('GestionReportesApp', ['dx']);
GestionReportesApp.controller('GestionReportesController', function GestionReportesController($scope, $sce, $http, $location) {

	var identificacion_empresa_autenticada = "";

	$http.get('/api/DatosSesion/').then(function (response) {

		identificacion_empresa_autenticada = response.data[0].Identificacion;

		CargarFormatos();

	}), function errorCallback(response) {
		Mensaje(response.data.ExceptionMessage, "error");
	};


	function CargarFormatos() {
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



});


function reportDesigner_CustomizeMenuActions(s, e) {

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

function reportDesigner_EndCallback(s, e) {

	if (isReportSavingCallback) {
		isReportSavingCallback = false;

		console.log(isReportSavingCallback);

		var ruta_regreso = s.cpRutaRegreso;


		var myDialog = DevExpress.ui.dialog.custom({
			title: s.cpTituloNotificacion,
			message: s.cpMensajeNotificacion,
			buttons: [{
				text: s.cpTextoBtnNotificacion,
				onClick: function (e) {
					window.location.href = '/Views/Pages/GestionReportes.aspx';
				}
			},
			]
		});
		myDialog.show().done(function (dialogResult) {
			console.log(dialogResult.buttonText);
		});

	}
}

