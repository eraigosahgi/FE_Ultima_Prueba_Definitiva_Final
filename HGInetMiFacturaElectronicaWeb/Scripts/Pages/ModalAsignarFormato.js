DevExpress.localization.locale('es-ES');

var ModalAsignarFormatoApp = angular.module('ModalAsignarFormatoApp', ['dx'])

ModalAsignarFormatoApp.controller('ModalAsignarFormatoController', function ModalAsignarFormatoController($scope, $location, $http) {

	var seleccion_empresa = "",
	seleccion_estado = ""
	seleccion_categoria = "",
	observaciones = "",
	tipo_documento = "";

	$http.get('/api/Empresas?Facturador=true').then(function (response) {
		$("#wait").hide();
		try {
			var stopTaskStatusDataSet = "";
			$http.get('/api/DatosSesion/').then(function (response) {
				stopTaskStatusDataSet = response.data[0].RazonSocial;
			});

			$("#SelectEmpresa").dxSelectBox({
				value: stopTaskStatusDataSet,
				placeholder: "Seleccione la Empresa",
				displayExpr: "RazonSocial",
				valueExpr: "Identificacion",
				dataSource: response.data,
				searchExpr: ["RazonSocial", "Identificacion"],
				searchEnabled: true,
				onValueChanged: function (data) {
					seleccion_empresa = data.value;
				}
			}).dxValidator({
				validationRules: [{
					type: "required",
					message: "Debe seleccionar la Empresa"
				}]
			});

		} catch (err) {
			DevExpress.ui.notify(err.message, 'error', 3000);
		}
	}, function errorCallback(response) {
		$('#wait').hide();
		DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
	});

	//Estado del formato
	$("#SelectEstado").dxSelectBox({
		placeholder: "Seleccione el Estado",
		displayExpr: "Texto",
		dataSource: Estados,
		onValueChanged: function (data) {
			seleccion_estado = data.value.ID;
		}
	}).dxValidator({
		validationRules: [{
			type: "required",
			message: "Debe seleccionar el Estado"
		}]
	});

	//Categoría (Prediseñado, personalizado)
	$("#SelectGenerico").dxSelectBox({
		placeholder: "Seleccione la Categoría",
		displayExpr: "Texto",
		dataSource: CategoriasFormato,
		onValueChanged: function (data) {
			seleccion_categoria = data.value.ID;
		}
	}).dxValidator({
		validationRules: [{
			type: "required",
			message: "Debe seleccionar la Categoría"
		}]
	});

	//TipoDocumento
	$("#SelectTipoDoc").dxSelectBox({
		placeholder: "Seleccione el Tipo de Documento",
		displayExpr: "Texto",
		dataSource: TiposDocumento,
		onValueChanged: function (data) {
			tipo_documento = data.value.ID;
		}
	}).dxValidator({
		validationRules: [{
			type: "required",
			message: "Debe seleccionar el Tipo de Documento"
		}]
	});

	//Campo de observaciones
	$("#TxtObservaciones").dxTextArea({
		onValueChanged: function (data) {
			observaciones = data.value.toUpperCase();
		}
	}).dxValidator({
		validationRules: [
		{
			type: "stringLength",
			max: 200,
			message: "El campo Observación no puede ser mayor a 200 caracteres"
		}]
	});

	$("#BtnContinuar").dxButton({
		text: "Continuar",
		type: "default",
		useSubmitBehavior: true
	});

	$("#FormularioFormato").on("submit", function (e) {
		GuardarFormato();
		e.preventDefault();
	});


	function GuardarFormato() {
		var formato_base = $scope.CodigoFormato;
		var empresa_base = $scope.EmpresaFormato;

		//AlmacenarFormatoPdf(string identificacion_empresa, bool estado, bool categoria, string observaciones, int formato_base, string empresa_base)
		$http.post('/api/AlmacenarFormatoPdf?identificacion_empresa=' + seleccion_empresa + '&estado=' + seleccion_estado + '&categoria=' + seleccion_categoria + '&observaciones=' + observaciones + '&formato_base=' + formato_base + '&empresa_base=' + empresa_base + '&tipo_documento=' + tipo_documento).then(function (response) {
			$("#wait").hide();
			try {
				console.log(response.data.IntDocTipo);
				DevExpress.ui.notify({ message: "El formato ha sido asignado correctamente. Código Identificador:" + response.data.IntCodigoFormato, position: { my: "center top", at: "center top" } }, "success", 1500);
				window.location.href = '/Views/ReportDesigner/ReportDesignerWeb.aspx?ID=' + response.data.IntCodigoFormato + '&Nit=' + response.data.StrEmpresa + '&TipoDoc=' + response.data.IntDocTipo;


			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response, 'error', 6000);
		});
	}

});


var Estados = [
{ ID: "0", Texto: 'INACTIVO' },
{ ID: "1", Texto: 'ACTIVO' }
];

var CategoriasFormato = [
{ ID: "0", Texto: 'PERSONALIZADO' },
{ ID: "1", Texto: 'PREDISEÑADO' }
];

var TiposDocumento =
    [
        { ID: "0", Texto: 'Todos' },
        { ID: "1", Texto: 'Factura' },
        { ID: "2", Texto: 'Nota Débito' },
        { ID: "3", Texto: 'Nota Crédito' }
    ];