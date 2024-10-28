DevExpress.localization.locale(navigator.language);
var opc_pagina = "1338";

var FechaRecepcionDianApp = angular.module('FechaRecepcionDianApp', ['dx', 'AppSrvFiltro', 'AppSrvEmpresas', 'AppSrvDocumento']);

FechaRecepcionDianApp.controller('FechaRecepcionDianController', function FechaRecepcionDianController($scope, $http, $location, SrvFiltro, SrvEmpresas, SrvDocumento) {
	var codigo_empresa = "", id_empresa = "", posicion_x = 0, posicion_y = 0;
	var tipo_documento, numero_documento, numero_resolucion;

	var summary_FormCamposDian = "summary_FormCamposDian";
	$("#summary_FormCamposDian").dxValidationSummary({ validationGroup: summary_FormCamposDian });

	var summary_CamposVisualizar = "summary_CamposVisualizar";
	$("#summary_CamposVisualizar").dxValidationSummary({ validationGroup: summary_CamposVisualizar });

	//SrvFiltro.ObtenerFiltro('Empresa', 'Facturador', 'icon-user-tie', 115, '/api/Empresas?Facturador=true', 'Identificacion', 'RazonSocial', false, 1).then(function (Datos) {
	//	$scope.Facturador = Datos;
	//	txt_hgi_Facturador = "";
	//});

	$("#txtEmpresa").dxTextBox({
		readOnly: true,
		name: txtEmpresa,
		onValueChanged: function (data) {

		},
		onFocusIn: function (data) {
			$('#modal_Buscar_empresa').modal('show');

			SrvEmpresas.ObtenerFacturadores().then(function (data) {

				$("#gridEmpresas").dxDataGrid({
					dataSource: data,
					paging: {
						pageSize: 10
					},
					pager: {
						showPageSizeSelector: true,
						allowedPageSizes: [5, 10, 20],
						showInfo: true
					},
					columns: [
					   {
					   	caption: "Identificación",
					   	dataField: "Identificacion",
					   	cssClass: "col-md-3",
					   	cellTemplate: function (container, options) {
					   		$("<div title='Detalle : " + options.data.RazonSocial + "' >")
					   		$("<div style='text-align:center'>")
								.append($("<a></a>").dxButton({
									text: options.data.Identificacion,
									onClick: function () {
										ObtenerEmpresa(options.data);
									}
								}).removeClass("dx-button dx-button-normal dx-widget")
							)
								.appendTo(container);
					   	}
					   },
						   {
						   	caption: "Razón Social",
						   	dataField: "RazonSocial",
						   	cssClass: "col-md-9"
						   },
					],
					filterRow: {
						visible: true,
					}

				});
			});
		}
	}).dxValidator({
		validationGroup: summary_FormCamposDian,
		validationRules: [{
			type: 'required',
			message: "Debe seleccionar la empresa"
		}]
	});

	$("#txtIntPdfCampoDianPosX").dxNumberBox({
		format: "#0.##",
		showSpinButtons: true,
		showClearButton: true,
		onValueChanged: function (data) {
			posicion_x = data.value;
		}
	});

	$("#txtIntPdfCampoDianPosY").dxNumberBox({
		format: "#0.##",
		showSpinButtons: true,
		showClearButton: true,
		onValueChanged: function (data) {
			posicion_y = data.value;
		}
	});

	$("#txtTipoDocumento").dxSelectBox({
		placeholder: "Seleccione el Tipo de Documento",
		displayExpr: "Texto",
		dataSource: TiposDocumento,
		onValueChanged: function (data) {
			tipo_documento = data.value.ID;
		}
	}).dxValidator({
		validationGroup: summary_CamposVisualizar,
		validationRules: [{
			type: "required",
			message: "Debe seleccionar el Tipo de Documento"
		}]
	});

	$("#txtNumeroDocumento").dxTextBox({
		placeholder: "Ingrese Número Documento",
		showSpinButtons: true,
		showClearButton: true,
		onValueChanged: function (data) {
			numero_documento = data.value;
		}
	}).dxValidator({
		validationGroup: summary_CamposVisualizar,
		validationRules: [{
			type: "required",
			message: "Debe ingresar el número de documento"
		}]
	});

	$("#txtNumeroResolucion").dxSelectBox({
		placeholder: "Debe seleccionar una Empresa para continuar",
		disabled: true,
	});

	//Almacenar Datos 
	$("#BtnAlmacenarDatos").dxButton({
		text: "Guardar",
		type: "default",
		validationGroup: summary_FormCamposDian,
		onClick: function (e) {
			var result = e.validationGroup.validate();
			if (result.isValid) {
				guardarDatos();
			}
		}
	});

	//Visualizar Documento
	$("#BtnvisualizarDocumento").dxButton({
		text: "Visualizar",
		type: "default",
		validationGroup: summary_CamposVisualizar,
		onClick: function (e) {
			var result = e.validationGroup.validate();
			if (result.isValid) {
				visualizarDocumento();
			}
		}
	});

	function guardarDatos() {

		SrvEmpresas.EditarCamposDian(codigo_empresa, posicion_x, posicion_y).then(function (Datos) {

			DevExpress.ui.notify({ message: "Datos actualizados con exito", position: { my: "center top", at: "center top" } }, "success", 1500);

		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response, 'error', 6000);
		});
	}

	function visualizarDocumento() {

		if (codigo_empresa == "") {
			DevExpress.ui.notify({ message: "Debe seleccionar una empresa para continuar.", position: { my: "center top", at: "center top" } }, "error", 1500);
		} else {
			SrvDocumento.ValidarTextoXYPDF(codigo_empresa, id_empresa, tipo_documento, numero_documento, numero_resolucion, posicion_x, posicion_y).then(function (Datos) {

				console.log(Datos);
				window.open(Datos, "_blank")

			}, function errorCallback(response) {
				$('#wait').hide();
				DevExpress.ui.notify(response, 'error', 6000);
			});
		}



	}

	function CargarResoluciones(codigo_facturador) {
		$http.get('/api/ObtenerResPrefijo?codigo_facturador=' + codigo_facturador).then(function (response) {

			$("#txtNumeroResolucion").dxSelectBox({
				dataSource: response.data,
				valueExpr: "ID",
				displayExpr: "Descripcion",
				searchEnabled: true,
				disabled: false,
				placeholder: "Seleccione un Item",
				onValueChanged: function (data) {
					var selectedItem = data.component.option('selectedItem');
					numero_resolucion = selectedItem.ID;
				}
			}).dxValidator({
				validationGroup: summary_CamposVisualizar,
				validationRules: [{
					type: "required",
					message: "Debe seleccionar una resolución"
				}]
			});

		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}

	function ObtenerEmpresa(Empresa) {
		$('#modal_Buscar_empresa').modal('hide');
		codigo_empresa = Empresa.IdSeguridad;
		id_empresa = Empresa.Identificacion;
		$("#txtEmpresa").dxTextBox({ value: Empresa.Identificacion + ' --  ' + Empresa.RazonSocial });
		$("#txtIntPdfCampoDianPosX").dxNumberBox({ value: Empresa.IntPdfCampoDianPosX });
		$("#txtIntPdfCampoDianPosY").dxNumberBox({ value: Empresa.IntPdfCampoDianPosY });
		CargarResoluciones(Empresa.Identificacion);
	}


});

var TiposDocumento = [
{ ID: "1", Texto: 'Factura' },
{ ID: "2", Texto: 'Nota Débito' },
{ ID: "3", Texto: 'Nota Crédito' }
];