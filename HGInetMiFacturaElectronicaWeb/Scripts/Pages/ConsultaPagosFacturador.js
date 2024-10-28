﻿DevExpress.localization.locale('es-ES');

var email_destino = "";
var id_seguridad = "";
var items_recibo = [];
var PagosFacturadorApp = angular.module('PagosFacturadorApp', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento', 'AppSrvFiltro']);
PagosFacturadorApp.controller('PagosFacturadorController', function PagosFacturadorController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento, SrvFiltro) {

	var now = new Date();
	var Estado;
	var ResolucionesPrefijo = [];

	var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";
	resolucion = "";
	Filtro_fecha = 2,
	tipo_consulta = 1;

	Estado_Documento = "";

	$scope.documentosPendientes = false;

	$scope.Verificando = false;

	//Objecto json con documentos pendientes
	var datos = [];
	var objeto = {};

	$scope.CantidaddocPendientes = 0;

	$("#FechaInicial").dxDateBox({
		value: now,
		width: '100%',
		displayFormat: "yyyy-MM-dd",
		onValueChanged: function (data) {
			fecha_inicio = new Date(data.value).toISOString();
			$("#FechaFinal").dxDateBox({ min: fecha_inicio });
		}

	});

	$("#FechaFinal").dxDateBox({
		value: now,
		width: '100%',
		displayFormat: "yyyy-MM-dd",
		onValueChanged: function (data) {
			fecha_fin = new Date(data.value).toISOString();
			$("#FechaInicial").dxDateBox({ max: fecha_fin });
		}

	});
	//SrvMaestrosEnum.ObtenerSesion().then(function (data) {
	//	codigo_facturador = data[0].Identificacion;
	var codigo_facturador = "";



	//SrvMaestrosEnum.ObtenerEnum(0, "publico").then(function (data) {
	//	console.log("publico: ", data);

	//});

	//});

	var items_recibo = [];

	$("#EstadoRecibo").dxSelectBox({
		placeholder: "Seleccione el Estado",
		displayExpr: "Descripcion",
		dataSource: items_recibo,
		showClearButton: true,
		onValueChanged: function (data) {
			estado_recibo = (data.value == null) ? "*" : data.value.ID;
		},
		onOpened: function (data) {
			if (items_recibo.length == 0) {
				SrvMaestrosEnum.ObtenerEnum(4).then(function (dataacuse) {
					items_recibo = dataacuse;
					$("#EstadoRecibo").dxSelectBox({ dataSource: items_recibo });
				});
			}
		}
	});

	$("#txtTipoConsulta").dxSelectBox({
		placeholder: "Seleccione el Tipo de Consulta",
		displayExpr: "Texto",
		valueExpr: "ID",
		value: items_tipo_consulta[0].ID,
		dataSource: items_tipo_consulta,
		showClearButton: true,
		onValueChanged: function (data) {
			console.log(data.value);
			tipo_consulta = data.value;
		},
	});


	SrvFiltro.ObtenerFiltro('Documento Adquiriente', 'Adquiriente', 'icon-user-tie', 115, '/api/ObtenerAdquirientes?Facturador=' + $('#Hdf_Facturador').val(), 'ID', 'Texto', false).then(function (Datos) {
		$scope.Adquiriente = Datos;
	});



	var DataCheckBox = function (Datos) {
		return new DevExpress.data.CustomStore({
			loadMode: "raw",
			key: "ID",
			load: function () {
				return JSON.parse(JSON.stringify(Datos));
			}
		});
	};

	////Resolucion - Prefijo
	//$("#filtrosResolucion").dxDropDownBox({
	//	valueExpr: "ID",
	//	placeholder: "Seleccione una Resolución",
	//	displayExpr: "Descripcion",
	//	showClearButton: true,
	//	//dataSource: DataCheckBox(ResolucionesPrefijo),		
	//	onOpened: function (data) {
	//		if (ResolucionesPrefijo.length == 0) {
	//			$http.get('/api/ObtenerResPrefijo?codigo_facturador=').then(function (response) {
	//				console.log("Prefijo: ", response);
	//				ResolucionesPrefijo = response.data;
	//				$("#filtrosResolucion").dxDropDownBox({
	//					dataSource: DataCheckBox(ResolucionesPrefijo),
	//					contentTemplate: function (e) {
	//						var value = e.component.option("value"),
	//							$dataGrid = $("<div>").dxDataGrid({
	//								dataSource: e.component.option("dataSource"),
	//								allowColumnResizing: true,
	//								columns:
	//									[
	//										 {
	//										 	caption: "Descripción",
	//										 	dataField: "Descripcion",
	//										 	title: "Descripcion",
	//										 	width: 500

	//										 }],
	//								hoverStateEnabled: true,
	//								paging: { enabled: true, pageSize: 10 },
	//								filterRow: { visible: true },
	//								scrolling: { mode: "infinite" },
	//								height: 240,
	//								selection: { mode: "multiple" },
	//								selectedRowKeys: value,
	//								onSelectionChanged: function (selectedItems) {
	//									var keys = selectedItems.selectedRowKeys;
	//									e.component.option("value", keys);
	//									resolucion = keys;
	//								}
	//							});

	//						dataGrid = $dataGrid.dxDataGrid("instance");

	//						e.component.on("valueChanged", function (args) {
	//							var value = args.value;
	//							dataGrid.selectRows(value, false);
	//						});

	//						return $dataGrid;
	//					},
	//				});
	//			}, function (response) {
	//				DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
	//			});
	//		}
	//	}
	//});

	//Define los campos y las opciones
	$scope.filtros =
		{
			//Fecha: {
			//	//Carga la data del control
			//	dataSource: new DevExpress.data.ArrayStore({
			//		data: items_Fecha,
			//		key: "ID"
			//	}),
			//	displayExpr: "Texto",
			//	value: items_Fecha[1],

			//	onValueChanged: function (data) {
			//		if (data.value != null) {
			//			Filtro_fecha = data.value.ID;
			//		} else {
			//			Filtro_fecha = 1;
			//		}
			//	}
			//},

			NumeroDocumento: {
				placeholder: "Ingrese Número Documento",
				onValueChanged: function (data) {
					numero_documento = (data.value == null) ? "" : data.value;
				}
			}
		}

	$("#FechaFinal").dxDateBox({ min: now });
	$("#FechaInicial").dxDateBox({ max: now });
	//validarEstado();
	//consultar();


	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			validarEstado();
			consultar();
		}
	};



	$scope.buttonProcesar = {
		type: "success",
		onClick: function (e) {
			validarEstado();
			consultar();
		}
	};





	function validarEstado() {

		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();


		$('#wait').show();
		var codigo_adquiriente = (txt_hgi_Adquiriente == undefined || txt_hgi_Adquiriente == '') ? '' : txt_hgi_Adquiriente;
		$http.get('/api/ObtenerPagosFacturador?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&resolucion=' + resolucion + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
			$('#wait').hide();
			datos = [];
			//Recorro la data para ver la cantidad de documentos pendientes por procesar
			response.data.forEach(function (valor, indice, array) {
				if (valor.CodEstado == 888 || valor.CodEstado == 999) {
					datos.push({ "StrIdRegistro": valor.StrIdRegistro, "StrIdSeguridadDoc": valor.StrIdSeguridadDoc });
				}
			});

			objeto.datos = datos;

			if (objeto.datos.length > 0) {
				//var RutaServicio = $('#Hdf_RutaSrvPagos').val();

				objeto.datos.forEach(function (valor, indice, array) {
					//Conuslto el estado del documento en PI
					//SrvDocumento.ActualizaEstatusPago(RutaServicio, valor.StrIdSeguridadDoc, valor.StrIdRegistro).then(function (data1) {
					//Actualizado datos de plataforma intermedia, en la plataforma de FE
					SrvDocumento.ActualizaEstatusPagoInterno(valor.StrIdSeguridadDoc, valor.StrIdRegistro).then(function (data2) {
						consultar();
					});
					//});

				});
			}
		});
		/*
        $scope.$apply(function () {
            $scope.Verificando = false;
        });
        */
	}


	function consultar() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		if (resolucion == "")
			resolucion = "*";

		var estado = "";

		//Obtiene los datos del web api        
		$('#wait').show();
		var codigo_adquiriente = (txt_hgi_Adquiriente == undefined || txt_hgi_Adquiriente == '') ? '' : txt_hgi_Adquiriente;
		$http.get('/api/ObtenerPagosFacturador?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&resolucion=' + resolucion + '&tipo_fecha=' + Filtro_fecha + '&tipo_consulta=' + tipo_consulta).then(function (response) {
			$('#wait').hide();

			datos = [];
			//Recorro la data para ver la cantidad de documentos pendientes por procesar
			response.data.forEach(function (valor, indice, array) {
				if (valor.CodEstado == 888 || valor.CodEstado == 999) {
					datos.push({ "StrIdRegistro": valor.StrIdRegistro, "StrIdSeguridadDoc": valor.StrIdSeguridadDoc });
				}
			});

			objeto.datos = datos;

			if (objeto.datos.length > 0) {
				$scope.documentosPendientes = true;
			} else {
				$scope.documentosPendientes = false;
			}

			if (tipo_consulta == 1) {
				$("#gridDocumentos").dxDataGrid({
					dataSource: response.data,
					keyExpr: "StrIdRegistro",
					paging: {
						pageSize: 20
					},
					export: {
						enabled: true,
						filename:"Pagos Recibidos"
					},
					/*,stateStoring: {
						enabled: true,
						type: "localStorage",
						storageKey: "storage"
					},*/
					pager: {
						showPageSizeSelector: true,
						allowedPageSizes: [5, 10, 20],
						showInfo: true
					}
					//Formatos personalizados a las columnas en este caso para el monto
					, onCellPrepared: function (options) {
						var fieldData = options.value,
							fieldHtml = "";
						try {
							if (options.column.caption == "Valor") {
								if (fieldData) {
									var inicial = fNumber.go(fieldData).replace("$-", "-$");
									options.cellElement.html(inicial);
								}
							}


						} catch (err) {
						}

					}, loadPanel: {
						enabled: true
					},
					headerFilter: {
						visible: true
					}
			, allowColumnResizing: true
			, allowColumnReordering: true
			, columnChooser: {
				enabled: true,
				mode: "select",
				title: "Selector de Columnas"
			},
					groupPanel: {
						allowColumnDragging: true,
						visible: true
					}
			, columns: [{
				caption: 'Detalles',
				cssClass: "col-md-1 col-xs-1",
				cellTemplate: function (container, options) {
					$("<div style='text-align:center;'><a style='margin-left:5%;' class='icon-file-eye' onClick=ConsultarDetallesPago('" + options.data.StrIdRegistro + "','" + options.data.StrIdSeguridadDoc + "') title='Ver Detalles Pago'></a></div>")
							.appendTo(container);

				}
			}, {
				cssClass: "col-md-1 col-xs-1",
				caption: 'Estado',
				dataField: 'EstadoFactura',
				cellTemplate: function (container, options) {
					$("<div>").append($(ControlEstadoPago(options.data.CodEstado, options.data.EstadoFactura))).appendTo(container);
				}
			},
			{
				caption: "Forma Pago",
				dataField: "Franquicia",
				width: '90px',
				alignment: "center",
				cellTemplate: function (container, options) {

					if (options.data.Franquicia != "") {
						var titulo = (options.data.Franquicia != "") ? "title = " + options.data.Franquicia : "";

						$('<img src="/Scripts/Images/' + options.data.Franquicia + '.png"  ' + titulo + ' />')
								.appendTo(container);
					}
				}
			},
			  {
			  	caption: "Ticket",
			  	dataField: "Ticket"
			  },
			{
				caption: "Ciclo",
				dataField: "Ciclo"
			},
			{
				caption: "CUS o Aprobación",
				dataField: "Cus"
			},
			{
				caption: "Adquiriente",
				dataField: "StrEmpresaAdquiriente"
			},

				  {
				  	caption: "Adquiriente",
				  	dataField: "NombreAdquiriente"
				  },
				  {
				  	caption: "Fecha Pago",
				  	dataField: "DatAdquirienteFechaRecibo",
				  	dataType: "date",
				  	format: "yyyy-MM-dd HH:mm"

				  },

				{
					caption: "Fecha Verificación",
					dataField: "DatFechaVencDocumento",
					dataType: "date",
					format: "yyyy-MM-dd HH:mm"
				},
				  {
				  	caption: "Valor",
				  	dataField: "PagoFactura"
				  }


			],
					masterDetail: {
						enabled: true,
						template: function (container, options) {

							var currentEmployeeData = options.data;
							console.log(options.data);
							//$("<div>")
							//	.addClass("master-detail-caption")
							//	.text(currentEmployeeData.FirstName + " " + currentEmployeeData.LastName + "'s Tasks:")
							//	.appendTo(container);

							$("<div>")
								.dxDataGrid({
									columnAutoWidth: true,
									showBorders: true,
									onCellPrepared: function (options) {
										var fieldData = options.value,
											fieldHtml = "";
										try {
											if (options.column.caption == "Monto") {
												if (fieldData) {
													var inicial = fNumber.go(fieldData).replace("$-", "-$");
													options.cellElement.html(inicial);
												}
											}

										} catch (err) {
										}

									},
									columns: [
									{
										caption: "Prefijo",
										dataField: "Prefijo",


									},
									{
										caption: "Documento",
										dataField: "Documento"
									},
									{
										caption: "Monto",
										dataField: "Monto",

									}],
									dataSource: options.data.Pagos

								}).appendTo(container);
							//container.append(ObtenerDetallle(options.data.Pdf, options.data.Xml, options.data.EstadoAcuse, options.data.RutaAcuse, options.data.XmlAcuse, options.data.zip, options.data.RutaServDian, options.data.StrIdSeguridad, options.data.IdentificacionFacturador, options.data.NumeroDocumento, "Adquiriente"));
						}
					},

					summary: {
						groupItems: [{
							column: "PagoFactura",
							summaryType: "sum",
							displayFormat: " {0} Total ",
							valueFormat: "currency"
						}]
						, totalItems: [{
							name: "Suma",
							displayFormat: "{0}",
							valueFormat: "currency",
							summaryType: "custom"

						},
						{
							column: "PagoFactura",
							showInColumn: "PagoFactura",
							summaryType: "sum",
							customizeText: function (data) {
								return fNumber.go(data.value).replace("$-", "-$");
							}

						},
						{
							showInColumn: "DatFechaVencDocumento",
							displayFormat: "Total : ",
							alignment: "right"
						}
						],
						calculateCustomSummary: function (options) {
							if (options.name === "Suma") {
								if (options.summaryProcess === "start") {
									options.totalValue = 0;
									$('#Total').text("");
								}
								if (options.summaryProcess === "calculate") {
									options.totalValue = options.totalValue + options.value.PagoFactura;
									$('#Total').text("Total: " + fNumber.go(options.totalValue).replace("$-", "-$"));
								}
							}
						}
					}
			,
					filterRow: {
						visible: true
					}

				});
			} else {
				$("#gridDocumentos").dxDataGrid({
					dataSource: response.data,
					paging: {
						pageSize: 20
					},
					export: {
						enabled: true,
					},
					pager: {
						showPageSizeSelector: true,
						allowedPageSizes: [5, 10, 20],
						showInfo: true
					},
					onCellPrepared: function (options) {
						//Formatos personalizados a las columnas en este caso para el monto
						var fieldData = options.value,
							fieldHtml = "";
						try {
							if (options.column.caption == "Valor") {
								if (fieldData) {
									var inicial = fNumber.go(fieldData).replace("$-", "-$");
									options.cellElement.html(inicial);
								}
							}
						} catch (err) { }
					}, loadPanel: {
						enabled: true
					},
					headerFilter: {
						visible: true
					},
					allowColumnResizing: true,
					allowColumnReordering: true,
					columnChooser: {
						enabled: true,
						mode: "select",
						title: "Selector de Columnas"
					},
					columns: [
					{
						cssClass: "col-md-1 col-xs-1",
						caption: "Prefijo",
						dataField: "Prefijo"
					},
					{
						caption: "Documento",
						dataField: "Documento"
					},
					{
						cssClass: "col-md-1 col-xs-1",
						caption: 'Estado',
						dataField: 'EstadoFactura',
						cellTemplate: function (container, options) {
							$("<div>").append($(ControlEstadoPago(options.data.CodEstado, options.data.EstadoFactura))).appendTo(container);
						}
					},
					{
						caption: "Forma Pago",
						dataField: "Franquicia",
						width: '90px',
						alignment: "center",
						cellTemplate: function (container, options) {

							if (options.data.Franquicia != "") {
								var titulo = (options.data.Franquicia != "") ? "title = " + options.data.Franquicia : "";

								$('<img src="/Scripts/Images/' + options.data.Franquicia + '.png"  ' + titulo + ' />')
										.appendTo(container);
							}
						}
					},
					{
						caption: "Ticket",
						dataField: "Ticket"
					},
					{
						caption: "Ciclo",
						dataField: "Ciclo"
					}, {
						caption: "CUS o Aprobación",
						dataField: "Cus"
					},
					{
						caption: "Adquiriente",
						dataField: "StrEmpresaAdquiriente"
					},
					{
						caption: "Adquiriente",
						dataField: "NombreAdquiriente"
					},
					{
						caption: "Fecha Pago",
						dataField: "DatAdquirienteFechaRecibo",
						dataType: "date",
						format: "yyyy-MM-dd HH:mm"
					},
					{
						caption: "Fecha Verificación",
						dataField: "DatFechaVencDocumento",
						dataType: "date",
						format: "yyyy-MM-dd HH:mm"
					},
					{
						caption: "Valor",
						dataField: "PagoFactura"
					}],
					masterDetail: {
						enabled: false,
					},
					summary: {
						groupItems: [{
							column: "PagoFactura",
							summaryType: "sum",
							displayFormat: " {0} Total ",
							valueFormat: "currency"
						}]
						, totalItems: [{
							name: "Suma",
							displayFormat: "{0}",
							valueFormat: "currency",
							summaryType: "custom"

						},
						{
							column: "PagoFactura",
							showInColumn: "PagoFactura",
							summaryType: "sum",
							customizeText: function (data) {
								return fNumber.go(data.value).replace("$-", "-$");
							}

						},
						{
							showInColumn: "DatFechaVencDocumento",
							displayFormat: "Total : ",
							alignment: "right"
						}
						],
						calculateCustomSummary: function (options) {
							if (options.name === "Suma") {
								if (options.summaryProcess === "start") {
									options.totalValue = 0;
									$('#Total').text("");
								}
								if (options.summaryProcess === "calculate") {
									options.totalValue = options.totalValue + options.value.PagoFactura;
									$('#Total').text("Total: " + fNumber.go(options.totalValue).replace("$-", "-$"));
								}
							}
						}
					},
					filterRow: {
						visible: true
					}
				});
			}

		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});


	}

});


PagosFacturadorApp.controller('PagosAdquirienteController', function PagosAdquirienteController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento, SrvFiltro) {

	var now = new Date();
	var Estado;
	var ResolucionesPrefijo = [];

	var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "",
    resolucion = "",
    Filtro_fecha = 2
	Estado_Documento = "";

	$scope.documentosPendientes = false;

	$scope.Verificando = false;

	//Objecto json con documentos pendientes
	var datos = [];
	var objeto = {};

	$scope.CantidaddocPendientes = 0;

	$("#FechaInicial").dxDateBox({
		value: now,
		width: '100%',
		displayFormat: "yyyy-MM-dd",
		onValueChanged: function (data) {
			fecha_inicio = new Date(data.value).toISOString();
			$("#FechaFinal").dxDateBox({ min: fecha_inicio });
		}

	});

	$("#FechaFinal").dxDateBox({
		value: now,
		width: '100%',
		displayFormat: "yyyy-MM-dd",
		onValueChanged: function (data) {
			fecha_fin = new Date(data.value).toISOString();
			$("#FechaInicial").dxDateBox({ max: fecha_fin });
		}

	});


	//SrvMaestrosEnum.ObtenerSesion().then(function (data) {
	//	codigo_facturador = data[0].Identificacion;		
	//});

	SrvMaestrosEnum.ObtenerEnum(4).then(function (dataacuse) {
		items_recibo = dataacuse;
		cargarFiltros();
	});


	SrvFiltro.ObtenerFiltro('Documento Facturador', 'Facturador', 'icon-user-tie', 115, '/api/Empresas?Facturador=true', 'Identificacion', 'RazonSocial', false).then(function (Datos) {
		$scope.Facturador = Datos;
	});

	function cargarFiltros() {



		//Define los campos y las opciones
		$scope.filtros =
            {
            	Fecha: {
            		//Carga la data del control
            		dataSource: new DevExpress.data.ArrayStore({
            			data: items_Fecha,
            			key: "ID"
            		}),
            		displayExpr: "Texto",
            		value: items_Fecha[1],

            		onValueChanged: function (data) {
            			if (data.value != null) {
            				Filtro_fecha = data.value.ID;
            			} else {
            				Filtro_fecha = 1;
            			}
            		}
            	},
            	EstadoRecibo: {
            		searchEnabled: true,
            		//Carga la data del control
            		dataSource: new DevExpress.data.ArrayStore({
            			data: items_recibo,
            			key: "ID"
            		}),
            		displayExpr: "Descripcion",
            		Enabled: true,
            		placeholder: "Seleccione un Item",
            		onValueChanged: function (data) {
            			estado_recibo = (data.value == null) ? "*" : data.value.ID;
            		}

            	},
            	NumeroDocumento: {
            		placeholder: "Ingrese Número Documento",
            		onValueChanged: function (data) {
            			numero_documento = (data.value == null) ? "" : data.value;
            		}
            	},
            	Adquiriente: {
            		placeholder: "Ingrese Identificación del Facturador",
            		onValueChanged: function (data) {
            			codigo_adquiriente = data.value;
            		}
            	}
            }

		$("#FechaFinal").dxDateBox({ min: now });
		$("#FechaInicial").dxDateBox({ max: now });

		validarEstado();
		//consultar();
	}

	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			validarEstado();
			consultar();
		}
	};


	$scope.buttonProcesar = {
		type: "success",
		onClick: function (e) {
			validarEstado();
			consultar();
		}
	};

	$scope.buttonVerificando = {
		type: "success",
		onClick: function (e) {
			DevExpress.ui.notify("Aun no ha terminado el proceso actual", 'error', 3000);
		}
	};


	function validarEstado() {

		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();


		$('#wait').show();
		var codigo_Facturador_consulta = (txt_hgi_Facturador == undefined || txt_hgi_Facturador == '') ? '' : txt_hgi_Facturador;
		$http.get('/api/ObtenerPagosAdquiriente?codigo_facturador=' + codigo_Facturador_consulta + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_facturador + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
			$('#wait').hide();
			datos = [];
			//Recorro la data para ver la cantidad de documentos pendientes por procesar
			response.data.forEach(function (valor, indice, array) {
				if (valor.CodEstado == 888 || valor.CodEstado == 999) {
					datos.push({ "StrIdRegistro": valor.StrIdRegistro, "StrIdSeguridadDoc": valor.StrIdSeguridadDoc });
				}
			});

			objeto.datos = datos;

			if (objeto.datos.length > 0) {
				//var RutaServicio = $('#Hdf_RutaSrvPagos').val();

				objeto.datos.forEach(function (valor, indice, array) {
					//Conuslto el estado del documento en PI
					//SrvDocumento.ActualizaEstatusPago(RutaServicio, valor.StrIdSeguridadDoc, valor.StrIdRegistro).then(function (data1) {
					//Actualizado datos de plataforma intermedia, en la plataforma de FE
					SrvDocumento.ActualizaEstatusPagoInterno(valor.StrIdSeguridadDoc, valor.StrIdRegistro).then(function (data2) {
						consultar();
					});


					//});

				});
			}
		});
		$scope.$apply(function () {
			$scope.Verificando = false;
		});
	}


	function consultar() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		//Obtiene los datos del web api        
		$('#wait').show();
		var codigo_Facturador_consulta = (txt_hgi_Facturador == undefined || txt_hgi_Facturador == '') ? '' : txt_hgi_Facturador;
		$http.get('/api/ObtenerPagosAdquiriente?codigo_facturador=' + codigo_Facturador_consulta + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_facturador + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
			$('#wait').hide();


			datos = [];
			//Recorro la data para ver la cantidad de documentos pendientes por procesar
			response.data.forEach(function (valor, indice, array) {
				if (valor.CodEstado == 888 || valor.CodEstado == 999) {
					datos.push({ "StrIdRegistro": valor.StrIdRegistro, "StrIdSeguridadDoc": valor.StrIdSeguridadDoc });
				}
			});


			objeto.datos = datos;

			if (objeto.datos.length > 0) {
				$scope.documentosPendientes = true;
			} else {
				$scope.documentosPendientes = false;
			}

			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data,
				keyExpr: "StrIdRegistro",
				paging: {
					pageSize: 20
				},
				stateStoring: {
					enabled: true,
					type: "localStorage",
					storageKey: "storage"
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				}
				//Formatos personalizados a las columnas en este caso para el monto
                , onCellPrepared: function (options) {
                	var fieldData = options.value,
                        fieldHtml = "";
                	try {
                		if (options.column.caption == "Valor") {
                			if (fieldData) {
                				var inicial = fNumber.go(fieldData).replace("$-", "-$");
                				options.cellElement.html(inicial);
                			}
                		}

                	} catch (err) {
                	}

                }, loadPanel: {
                	enabled: true
                },
				headerFilter: {
					visible: true
				}
        , allowColumnResizing: true
        , allowColumnReordering: true
        , columnChooser: {
        	enabled: true,
        	mode: "select",
        	title: "Selector de Columnas"
        },
				groupPanel: {
					allowColumnDragging: true,
					visible: true
				}
        , columns: [{
        	caption: 'Detalles',
        	cssClass: "col-md-1 col-xs-1",
        	cellTemplate: function (container, options) {
        		$("<div style='text-align:center;'><a style='margin-left:5%;' class='icon-file-eye' onClick=ConsultarDetallesPago('" + options.data.StrIdRegistro + "','" + options.data.StrIdSeguridadDoc + "') title='Ver Detalles Pago'></a></div>")
						.appendTo(container);

        	}
        }, {
        	cssClass: "col-md-1 col-xs-1",
        	caption: 'Estado',
        	dataField: 'EstadoFactura',
        	cellTemplate: function (container, options) {
        		$("<div>").append($(ControlEstadoPago(options.data.CodEstado, options.data.EstadoFactura))).appendTo(container);
        	}
        },
		{
			caption: "Forma Pago",
			dataField: "Franquicia",
			width: '90px',
			alignment: "center",
			cellTemplate: function (container, options) {

				if (options.data.Franquicia != "") {
					var titulo = (options.data.Franquicia != "") ? "title = " + options.data.Franquicia : "";

					$('<img src="/Scripts/Images/' + options.data.Franquicia + '.png"  ' + titulo + ' />')
							.appendTo(container);
				}
			}
		},
		 {
		 	caption: "Ticket",
		 	dataField: "Ticket"
		 },
        {
        	caption: "Ciclo",
        	dataField: "Ciclo"
        },
		{
			caption: "CUS o Aprobación",
			dataField: "Cus"
		},

             {
             	caption: "Facturador",
             	dataField: "StrEmpresaFacturador"
             },

              {
              	caption: "Nombre Facturador",
              	dataField: "NombreFacturador"
              },
              {
              	caption: "Fecha Pago",
              	dataField: "DatAdquirienteFechaRecibo",
              	dataType: "date",
              	format: "yyyy-MM-dd HH:mm"

              },

             {
             	caption: "Cod. Transacción",
             	dataField: "idseguridadpago",
             },
              {
              	caption: "Fecha Verificación",
              	dataField: "DatFechaVencDocumento",
              	dataType: "date",
              	format: "yyyy-MM-dd HH:mm"
              },
              {
              	caption: "Valor",
              	dataField: "PagoFactura"
              }



        ],
				masterDetail: {
					enabled: true,
					template: function (container, options) {

						var currentEmployeeData = options.data;
						$("<div>")
							.dxDataGrid({
								columnAutoWidth: true,
								showBorders: true,
								onCellPrepared: function (options) {
									var fieldData = options.value,
										fieldHtml = "";
									try {
										if (options.column.caption == "Monto") {
											if (fieldData) {
												var inicial = fNumber.go(fieldData).replace("$-", "-$");
												options.cellElement.html(inicial);
											}
										}

									} catch (err) {
									}

								},
								columns: [
								{
									caption: "Prefijo",
									dataField: "Prefijo",


								},
								{
									caption: "Documento",
									dataField: "Documento"
								},
								{
									caption: "Monto",
									dataField: "Monto",

								}],
								dataSource: options.data.Pagos

							}).appendTo(container);
						//container.append(ObtenerDetallle(options.data.Pdf, options.data.Xml, options.data.EstadoAcuse, options.data.RutaAcuse, options.data.XmlAcuse, options.data.zip, options.data.RutaServDian, options.data.StrIdSeguridad, options.data.IdentificacionFacturador, options.data.NumeroDocumento, "Adquiriente"));
					}
				},
				summary: {
					groupItems: [{
						column: "PagoFactura",
						summaryType: "sum",
						displayFormat: " {0} Total ",
						valueFormat: "currency"
					}]
                    , totalItems: [{
                    	name: "Suma",
                    	displayFormat: "{0}",
                    	valueFormat: "currency",
                    	summaryType: "custom"

                    },
					{
						column: "PagoFactura",
						showInColumn: "PagoFactura",
						summaryType: "sum",
						customizeText: function (data) {
							return fNumber.go(data.value).replace("$-", "-$");
						}

					},
                    {
                    	showInColumn: "DatFechaVencDocumento",
                    	displayFormat: "Total : ",
                    	alignment: "right"
                    }
                    ],
					calculateCustomSummary: function (options) {
						if (options.name === "Suma") {
							if (options.summaryProcess === "start") {
								options.totalValue = 0;
								$('#Total').text("");
							}
							if (options.summaryProcess === "calculate") {
								options.totalValue = options.totalValue + options.value.PagoFactura;
								$('#Total').text("Total: " + fNumber.go(options.totalValue).replace("$-", "-$"));
							}
						}
					}
				}
        ,
				filterRow: {
					visible: true
				}

			});

		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});


	}


});


ConsultarDetallesPago = function (IdRegistroPago, IdSeguridadDoc) {

	var ruta_redireccion = $('#Hdf_RutaPlataformaServicios').val() + "Views/DetallesPagoE.aspx?IdSeguridadPago=" + IdSeguridadDoc + "&IdSeguridadRegistro=" + IdRegistroPago;


	$("#modal_detalles_pago").modal('show');
	$("#ContenidoDetallesPago").html('<object data="' + ruta_redireccion + '" style="width: 100%; height: 600px" />');
};

var items_Fecha =
    [
    { ID: "1", Texto: 'Fecha-Documento' },
    { ID: "2", Texto: 'Fecha-Pago' }
    ];

var items_tipo_consulta = [{ ID: "1", Texto: 'General' }, { ID: "2", Texto: 'Por Documento' }];