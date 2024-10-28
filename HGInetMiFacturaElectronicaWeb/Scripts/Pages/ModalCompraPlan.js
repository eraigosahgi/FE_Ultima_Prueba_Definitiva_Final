DevExpress.localization.locale('es-ES');

//var ModalComprarPlanApp = angular.module('ModalComprarPlanApp', ['dx'])

GestionCompraPlanesApp.controller('ModalComprarPlanController', function ModalComprarPlanController($scope, $http, $rootScope) {

	$("#BtnComprar").dxButton({
		text: "Activar",
		type: "default",
		useSubmitBehavior: true
	});

	$("#FormularioCompraPlan").on("submit", function (e) {
		GuardarCompra();
		e.preventDefault();
	});


	function GuardarCompra() {
		var sucursal = $('#IdSucursal').dxLookup("instance").option().value;
		if (!sucursal) {
			sucursal = 0;
		}
		//Caso 718725
		//$http.post('/api/ComprarPlan?cantidad=' + $scope.cantidad + '&valor_unit=' + $scope.Valor_Unit + '&valor_total=' + $scope.Valor_Total + '&tipo_doc=' + $scope.tipo_documento + '&codigo_plan=' + $scope.codigo_Plan).then(function (response) {
		$http.post('/api/ComprarPlan?cantidad=' + $scope.cantidad + '&valor_unit=' + $scope.Valor_Unit + '&valor_total=' + $scope.Valor_Total + '&tipo_doc=' + 0 + '&codigo_plan=' + $scope.codigo_Plan + '&codigo_sucursal=' + sucursal).then(function (response) {
			$("#wait").hide();
			try {

				$('#modal_comprar_plan').modal('hide');

				//Resetea los campos del formulario.
				//document.getElementById("FormularioCompraPlan").reset();

				if (response.data == "") {
					//Carga notificación de creación con opción de editar formato.
					//var myDialog = DevExpress.ui.dialog.custom({
					//	title: "Proceso Éxitoso",
					//	message: "Tú plan ha sido activado correctamente, pronto será enviada la factura electrónica correspondiente.",
					//	buttons: [{
					//		text: "Aceptar",
					//		onClick: function (e) {
					//			myDialog.hide();
					//			$scope.CargarPlanes();
					//		}
					//	}]
					//});
					//myDialog.show().done(function (dialogResult) {
					//});

					swal({
						title: "Proceso Éxitoso",
						text: "Tú plan ha sido activado correctamente, pronto será enviada la factura electrónica correspondiente.",
						type: 'success',
						confirmButtonColor: '#4CAF50',
						confirmButtonText: 'Aceptar',
						animation: 'pop',
						html: true,
					}, function () {
						$scope.CargarPlanes();
						//window.location = "/Views/Pages/GestionReportes.aspx";
					});
				}
				else {
					//Carga notificación de creación con opción de editar formato.
					//var myDialog = DevExpress.ui.dialog.custom({
					//	title: "Proceso Falló",
					//	message: response.data,
					//	buttons: [{
					//		text: "Aceptar",
					//		onClick: function (e) {
					//			myDialog.hide();
					//			$scope.CargarPlanes();
					//		}
					//	}]
					//});
					//myDialog.show().done(function (dialogResult) {
					//});


					swal({
						title: 'Notificación',
						text: response.data,
						type: 'warning',
						confirmButtonColor: '#FF5722',
						confirmButtonText: 'Aceptar',
						animation: 'pop',
						html: true,
					}, function () {
						$scope.CargarPlanes();
						//window.location = "/Views/Pages/GestionReportes.aspx";
					});

				}






			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response, 'error', 6000);
		});
	}

	function validarCantidad() {
		if (cantidad > $scope.Min && cantidad < $scope.Max) {
			return true;
		}
		else {
			return false;
		}
	}

});
