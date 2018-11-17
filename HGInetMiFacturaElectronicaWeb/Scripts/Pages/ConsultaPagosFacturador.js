DevExpress.localization.locale('es-ES');

var path = window.location.pathname;
var ruta = window.location.href;
ruta = ruta.replace(path, "/");
//imprimir MaestrosEnum.js para tener acceso a sus funciones
document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/MaestrosEnum.js"></scr' + 'ipt>');
//imprimir SrvDocumentos.js para tener acceso a sus funciones
document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/SrvDocumentos.js"></scr' + 'ipt>');


var email_destino = "";
var id_seguridad = "";
var items_recibo = [];
var PagosFacturadorApp = angular.module('PagosFacturadorApp', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento']);
PagosFacturadorApp.controller('PagosFacturadorController', function PagosFacturadorController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento) {

	var now = new Date();
	var Estado;
	var ResolucionesPrefijo;

	var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";
	resolucion = "";
	Filtro_fecha = 2;

	Estado_Documento = "";

	$scope.documentosPendientes = false;

	$scope.Verificando = false;

	//Objecto json con documentos pendientes
	var datos = [];
	var objeto = {};

	$scope.CantidaddocPendientes = 0;

	SrvMaestrosEnum.ObtenerSesion().then(function (data) {
		codigo_facturador = data[0].Identificacion;


		SrvMaestrosEnum.ObtenerEnum(0, "publico").then(function (data) {
			SrvMaestrosEnum.ObtenerEnum(4).then(function (dataacuse) {
				Estado = data;
				items_recibo = dataacuse;

				$http.get('/api/ObtenerResPrefijo?codigo_facturador=' + codigo_facturador).then(function (response) {
					ResolucionesPrefijo = response.data;
					cargarFiltros();
				}, function (response) {
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

			});
		});

	});



	function cargarFiltros() {
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


		var DataCheckBox = function (Datos) {
			return new DevExpress.data.CustomStore({
				loadMode: "raw",
				key: "ID",
				load: function () {
					return JSON.parse(JSON.stringify(Datos));
				}
			});
		};




		//Resolucion - Prefijo
		$("#filtrosResolucion").dxDropDownBox({
			valueExpr: "ID",
			placeholder: "Seleccione un Item",
			displayExpr: "Descripcion",
			showClearButton: true,
			dataSource: DataCheckBox(ResolucionesPrefijo),
			contentTemplate: function (e) {
				var value = e.component.option("value"),
                    $dataGrid = $("<div>").dxDataGrid({
                    	dataSource: e.component.option("dataSource"),
                    	allowColumnResizing: true,
                    	columns:
                            [
                                 {
                                 	caption: "Descripción",
                                 	dataField: "Descripcion",
                                 	title: "Descripcion",
                                 	width: 500

                                 }],
                    	hoverStateEnabled: true,
                    	paging: { enabled: true, pageSize: 10 },
                    	filterRow: { visible: true },
                    	scrolling: { mode: "infinite" },
                    	height: 240,
                    	selection: { mode: "multiple" },
                    	selectedRowKeys: value,
                    	onSelectionChanged: function (selectedItems) {
                    		var keys = selectedItems.selectedRowKeys;
                    		e.component.option("value", keys);
                    		resolucion = keys;
                    	}
                    });

				dataGrid = $dataGrid.dxDataGrid("instance");

				e.component.on("valueChanged", function (args) {
					var value = args.value;
					dataGrid.selectRows(value, false);
				});

				return $dataGrid;
			}
		});

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
            			numero_documento = data.value;
            		}
            	},
            	Adquiriente: {
            		placeholder: "Ingrese Identificación del Adquiriente",
            		onValueChanged: function (data) {
            			codigo_adquiriente = data.value;
            		}
            	}
            }

		$("#FechaFinal").dxDateBox({ min: now });
		$("#FechaInicial").dxDateBox({ max: now });
		validarEstado();
		consultar();
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





	function validarEstado() {

		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();


		$('#wait').show();
		$http.get('/api/ObtenerPagosFacturador?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&resolucion=' + resolucion + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
		    $('#wait').hide();
		    console.log("Se validaron los documentos con estado pendiente");
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
				$scope.documentosPendientes = true;
			} else {
				$scope.documentosPendientes = false;
			}


			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data,
				keyExpr: "NumeroDocumento",
				paging: {
					pageSize: 20
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
        , columns: [
             {
             	cssClass: "col-md-1 col-xs-1",
             	caption: 'Estado',
             	dataField: 'EstadoFactura',
             	cellTemplate: function (container, options) {
             		var clase = "";
             		if (options.data.EstadoFactura == 'Aprobado')
             			clase = " style='color:green; cursor:default;' title='Aprobado'";
             		else if (options.data.EstadoFactura == 'Rechazado')
             			clase = " style='color:red; cursor:default;' title='Rechazado'";
             		else
             			clase = " style='color:gray; cursor:default;' title='Pendiente'";

             		$("<div style='text-align:center; cursor:default;'>")
						.append($("<a taget=_self class='icon-circle2'" + clase + ">"))
						.appendTo(container);
             	}
             }, {
             	caption: "Adquiriente",
             	dataField: "StrEmpresaAdquiriente"
             },

              {
              	caption: "Nombre Adquiriente",
              	dataField: "NombreAdquiriente"
              },
              {
              	caption: "Fecha Pago",
              	dataField: "DatAdquirienteFechaRecibo",
              	dataType: "date",
              	format: "yyyy-MM-dd HH:mm"

              },
            {
            	caption: "Documento",
            	dataField: "NumeroDocumento",

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

				summary: {
					groupItems: [{
						column: "PagoFactura",
						summaryType: "sum",
						displayFormat: " {0} Total ",
						valueFormat: "currency"
					}]
                    , totalItems: [{
                    	name: "Suma",
                    	showInColumn: "PagoFactura",
                    	displayFormat: "{0}",
                    	valueFormat: "currency",
                    	summaryType: "custom"

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


PagosFacturadorApp.controller('PagosAdquirienteController', function PagosAdquirienteController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento) {

	var now = new Date();
	var Estado;
	var ResolucionesPrefijo;

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


	SrvMaestrosEnum.ObtenerSesion().then(function (data) {
		codigo_facturador = data[0].Identificacion;

		SrvMaestrosEnum.ObtenerEnum(4).then(function (dataacuse) {
			items_recibo = dataacuse;
			cargarFiltros();
		});
	});




	function cargarFiltros() {
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
            			numero_documento = data.value;
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
		consultar();
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
		$http.get('/api/ObtenerPagosAdquiriente?codigo_facturador=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_facturador + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
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
		$http.get('/api/ObtenerPagosAdquiriente?codigo_facturador=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_facturador + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
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
				keyExpr: "NumeroDocumento",
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
        , columns: [
             {
             	cssClass: "col-md-1 col-xs-1",
             	caption: 'Estado',
             	dataField: 'EstadoFactura',
             	cellTemplate: function (container, options) {
             		var clase = "";
             		//Personalización  columna estado del pago
             		if (options.data.EstadoFactura == 'Aprobado')
             			clase = " style='color:green; cursor:default;' title='Aprobado'";
             		else if (options.data.EstadoFactura == 'Rechazado')
             			clase = " style='color:red; cursor:default;' title='Rechazado'";
             		else
             			clase = " style='color:gray; cursor:default;' title='Pendiente'";

             		$("<div style='text-align:center; cursor:default;'>")
						.append($("<a taget=_self class='icon-circle2'" + clase + ">"))
						.appendTo(container);
             	}
             },
             {
             	caption: "Facturador",
             	dataField: "StrEmpresaAdquiriente"
             },

              {
              	caption: "Nombre Facturador",
              	dataField: "NombreAdquiriente"
              },
              {
              	caption: "Fecha Pago",
              	dataField: "DatAdquirienteFechaRecibo",
              	dataType: "date",
              	format: "yyyy-MM-dd HH:mm"

              },
            {
            	caption: "Documento",
            	dataField: "NumeroDocumento",

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

				summary: {
					groupItems: [{
						column: "PagoFactura",
						summaryType: "sum",
						displayFormat: " {0} Total ",
						valueFormat: "currency"
					}]
                    , totalItems: [{
                    	name: "Suma",
                    	showInColumn: "PagoFactura",
                    	displayFormat: "{0}",
                    	valueFormat: "currency",
                    	summaryType: "custom"

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


var items_Fecha =
    [
    { ID: "1", Texto: 'Fecha-Documento' },
    { ID: "2", Texto: 'Fecha-Pago' }
    ];