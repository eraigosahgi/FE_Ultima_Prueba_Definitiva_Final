var opc_pagina = "138";

//var ModalComprarPlanApp = angular.module('ModalComprarPlanApp', []);
var GestionCompraPlanesApp = angular.module('GestionCompraPlanesApp', ['dx']);
GestionCompraPlanesApp.controller('GestionCompraPlanesController', function GestionCompraPlanesController($scope, $sce, $http, $location) {

	$('#modal_comprar_plan').modal('show');

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

						

						$scope.CargarPlanes();
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

	//Ejecuta la consulta de formatos de la empresa autenticada.
	$scope.CargarPlanes = function () {
		$http.get('/Api/ObtenerPlanesCompra').then(function (response) {
			$("#wait").hide();
			try {

				$scope.ListadoPlanes = response.data;

			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}

	//Coloca la clase del panel según el estado.
	$scope.ClasePanel = function (estado) {
		if (estado == 0)
			return "panel-danger"
		else if (estado == 1)
			return "panel-success";
		else
			return "panel-default";
	}

	

	//Evento para la carga del modal de creación/asignación de formatos.
	$scope.AbrirModal = function (Plan, Desde, Hasta, Valor_Unit) {

		$scope.Valor_Unit = Valor_Unit;
		$scope.cantidad = Desde;
		$scope.tipo_documento = 0;
		$scope.Valor_Total = Desde * Valor_Unit;
		$scope.codigo_Plan = Plan;

		$("#txtPlan").dxTextBox({
			value: Plan,
			readOnly: true
		});

		$("#txtMin").dxNumberBox({
			value: Desde,
			readOnly: true
		});

		$("#txtMax").dxNumberBox({
			value: Hasta,
			readOnly: true
		});

		$("#txtValUni").dxNumberBox({
			format: "$ #,##0",
			value: Valor_Unit,
			readOnly: true
		});

		$("#txtTotal").dxNumberBox({
			format: "$ #,##0",
			value: Desde * Valor_Unit,
			readOnly: true
		});

		$("#txtCantidad").dxNumberBox({
			value: Desde,
			onValueChanged: function (data) {
				cantidad = data.value;
				$("#txtTotal").dxNumberBox({ value: cantidad * Valor_Unit });
				$scope.cantidad = cantidad;
				$scope.Valor_Total = cantidad * Valor_Unit;
			}
		}).dxValidator({
			//validationGroup: ValidarGestionDocumento,
			validationRules: [{
				type: "range",
				max: Hasta,
				min: Desde,
				message: "La cantidad de documentos debe estar en el rango seleccionado"
			}]
			
		});

		//Caso 718725
		//TipoDocumento
		//$("#SelectTipoDoc").dxSelectBox({
		//	placeholder: "Seleccione el Tipo de Documento",
		//	displayExpr: "Texto",
		//	dataSource: TiposDocumento,
		//	onValueChanged: function (data) {
		//		$scope.tipo_documento = data.value.ID;
		//	}
		//}).dxValidator({
		//	validationRules: [{
		//		type: "required",
		//		message: "Debe seleccionar el Tipo de Documento"
		//	}]
		//});

		$('#modal_comprar_plan').modal('show');
	};

	var TiposDocumento =
		[
			{ ID: "0", Texto: 'Mixto' },
			//{ ID: "1", Texto: 'Factura Electrónica' },
			//{ ID: "2", Texto: 'Nomina Electrónica' }
		];

});


