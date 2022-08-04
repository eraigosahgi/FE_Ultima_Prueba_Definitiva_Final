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
		
		$http.post('/api/ComprarPlan?cantidad=' + $scope.cantidad + '&valor_unit=' + $scope.Valor_Unit + '&valor_total=' + $scope.Valor_Total + '&tipo_doc=' + $scope.tipo_documento).then(function (response) {
			$("#wait").hide();
			try {

				$('#modal_comprar_plan').modal('hide');

				//Resetea los campos del formulario.
				//document.getElementById("FormularioCompraPlan").reset();

				if (response.data == "")
				{
					//Carga notificación de creación con opción de editar formato.
					var myDialog = DevExpress.ui.dialog.custom({
						title: "Proceso Éxitoso",
						message: "Tú plan ha sido activado correctamente, pronto será enviada la factura electrónica correspondiente.",
						buttons: [{
							text: "Aceptar",
							onClick: function (e) {
								myDialog.hide();
								$scope.CargarPlanes();
							}
						}]
					});
					myDialog.show().done(function (dialogResult) {
					});
				}
				else {
					//Carga notificación de creación con opción de editar formato.
					var myDialog = DevExpress.ui.dialog.custom({
						title: "Proceso Falló",
						message: response.data,
						buttons: [{
							text: "Aceptar",
							onClick: function (e) {
								myDialog.hide();
								$scope.CargarPlanes();
							}
						}]
					});
					myDialog.show().done(function (dialogResult) {
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
