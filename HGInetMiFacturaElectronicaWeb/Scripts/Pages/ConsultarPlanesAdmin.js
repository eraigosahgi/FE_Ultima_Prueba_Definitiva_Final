DevExpress.localization.locale(navigator.language);
var opc_pagina = "1314";
var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);



//var path = window.location.pathname;
//var ruta = window.location.href;
//ruta = ruta.replace(path, "/");

//document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/FiltroGenerico.js"></script>');

var GestionPlanesApp = angular.module('GestionPlanesApp', ['ModalEmpresasApp', 'dx', 'AppMaestrosEnum', 'AppSrvFiltro']);
GestionPlanesApp.controller('ConsultaPlanesController', function ConsultaPlanesController($scope, $http, $location, SrvFiltro) {
	var now = new Date();
	var date = new Date();
	date.setMonth(date.getMonth() - 12);

	var fecha_inicio = date.toISOString();

	var codigo_facturador = "",
           Estado_plan = "",
           Tipo_plan = "",
           tipo_filtro_fecha = 1,
           fecha_fin = "";

	var estado = "";

	SrvFiltro.ObtenerFiltro('Facturador','Facturador', 'icon-user-tie', 115, '/api/ConsultarBolsaAdmin', 'ID', 'Texto',false).then(function (Datos) {
		$scope.Facturador = Datos;
	});

	$http.get('/api/DatosSesion/').then(function (response) {
		codigo_facturador = response.data[0].Identificacion;

		//Parametros
		//Titulo
		//Icono
		CargarSession();

	}, function errorCallback(response) {

		DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
	});





	function CargarSession() {

		$scope.filtros =
  {
  	TipoFiltroFecha: {
  		//Carga la data del control
  		dataSource: new DevExpress.data.ArrayStore({
  			data: TiposFiltroFecha,
  			key: "ID"
  		}),
  		displayExpr: "Texto",
  		value: TiposFiltroFecha[0],

  		onValueChanged: function (data) {
  			tipo_filtro_fecha = data.value.ID;
  		}
  	}
  }

		$("#FechaInicial").dxDateBox({
			value: fecha_inicio,
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



		$("#FechaFinal").dxDateBox({ min: now });
		$("#FechaInicial").dxDateBox({ max: now });

		var DataCheckBox = function (Datos) {
			return new DevExpress.data.CustomStore({
				loadMode: "raw",
				key: "ID",
				load: function () {
					return JSON.parse(JSON.stringify(Datos));
				}
			});
		};

		var DataCheckBox1 = function (Datos) {
			return new DevExpress.data.CustomStore({
				loadMode: "raw",
				key: "ID",
				load: function () {
					return JSON.parse(JSON.stringify(Datos));
				}
			});
		};

		$("#Tipoplan").dxDropDownBox({
			valueExpr: "ID",
			placeholder: "Seleccione un Item",
			displayExpr: "Texto",
			showClearButton: true,
			dataSource: DataCheckBox1(Lista_Tipo_plan),
			contentTemplate: function (e) {
				var value = e.component.option("value"),
                    $dataGrid1 = $("<div>").dxDataGrid({
                    	dataSource: e.component.option("dataSource"),
                    	allowColumnResizing: true,
                    	columns:
                            [
                                 {
                                 	caption: "Descripción",
                                 	dataField: "Texto",
                                 	title: "Descripción",
                                 	width: 300

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
                    		Tipo_plan = keys;
                    	}
                    });

				dataGrid1 = $dataGrid1.dxDataGrid("instance");

				e.component.on("valueChanged", function (args) {
					var value = args.value;
					dataGrid1.selectRows(value, false);
				});

				return $dataGrid1;
			}
		});


		$("#Estadoplan").dxDropDownBox({
			valueExpr: "ID",
			placeholder: "Seleccione un Item",
			displayExpr: "Texto",
			showClearButton: true,
			dataSource: DataCheckBox(Lista_Estado_plan),
			contentTemplate: function (e) {
				var value = e.component.option("value"),
                    $dataGrid = $("<div>").dxDataGrid({
                    	dataSource: e.component.option("dataSource"),
                    	allowColumnResizing: true,
                    	columns:
                            [
                                 {
                                 	caption: "Descripción",
                                 	dataField: "Texto",
                                 	title: "Descripción",
                                 	width: 300

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
                    		Estado_plan = keys;
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



		$scope.ButtonOptionsConsultar = {
			text: 'Consultar',
			type: 'default',
			onClick: function (e) {
				CargarConsulta((txt_hgi_Facturador == undefined || txt_hgi_Facturador == '') ? codigo_facturador : txt_hgi_Facturador);
			}
		};


		CargarConsulta((txt_hgi_Facturador == undefined || txt_hgi_Facturador == '') ? codigo_facturador : txt_hgi_Facturador);

	}

	function CargarConsulta(Facturador) {

		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		$("#wait").show();
		$http.get('/api/ConsultarPlanesAdministrador?Identificacion=' + Facturador + '&TipoPlan=' + Tipo_plan + '&Estado=' + Estado_plan + '&TipoFecha=' + tipo_filtro_fecha + '&FechaInicio=' + fecha_inicio + '&FechaFin=' + fecha_fin).then(function (response) {
			$("#wait").hide();
			try {
				$("#grid").dxDataGrid({
					dataSource: response.data,
					paging: {
						pageSize: 20

					},
					headerFilter: {
						visible: true
					},
					pager: {
						showPageSizeSelector: true,
						allowedPageSizes: [5, 10, 20],
						showInfo: true
					},
					groupPanel: {
						allowColumnDragging: true,
						visible: true
					},
					masterDetail: {
						enabled: true,
						template: function (container, options) {

							//************************************************************************************
							$('#Total').text("");
							if (fecha_inicio == "")
								fecha_inicio = now.toISOString();

							if (fecha_fin == "")
								fecha_fin = now.toISOString();


							var resolucion = "*";
							var tipo_filtro_fecha = 1;
							var Muestradetalle = true;

							$http.get('/api/DocumentosPlanes?IdPlan=' + options.data.id).then(function (response) {
								if (response.data.length > 0) {


									$("<div style='padding:20px'>").addClass("demo-container").dxDataGrid({
										dataSource: response.data,
										keyExpr: "NumeroDocumento",
										paging: {
											pageSize: 10
										}

                                        , pager: {
                                        	showPageSizeSelector: true,
                                        	allowedPageSizes: [5, 10, 20],
                                        	showInfo: true
                                        }
                                      , loadPanel: {
                                      	enabled: true
                                      },
										headerFilter: {
											visible: true
										}



                                        , columns: [
                                            {
                                            	caption: "Fecha",
                                            	dataField: "DatFechaIngreso",
                                            	dataType: "date",
                                            	format: "yyyy-MM-dd HH:mm"

                                            },

                                            {
                                            	caption: "Documento",
                                            	dataField: "NumeroDocumento",

                                            },
                                            {
                                            	caption: "Facturador",
                                            	dataField: "Cod_Facturador",

                                            },
                                            {
                                            	caption: "Nombre Facturador",
                                            	dataField: "NombreFacturador",

                                            },


                                        ]

									}).appendTo(container);
									$('div[class="dx-datagrid"]').attr("style", "padding:5px;");
								}
							}, function errorCallback(response) {
								DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
							});


							//*************************************************************************************
						}
					},

					//Formatos personalizados a las columnas en este caso para el monto
					onCellPrepared: function (options) {
						var fieldData = options.value,
                            fieldHtml = "";
						try {
							if (options.columnIndex == 5) {//Columna de valor Total
								if (fieldData) {
									var inicial = fNumber.go(fieldData);
									options.cellElement.html(inicial);
								}
							}

							if (options.data.CodigoEstado == 0) {
								estado = " style='color:green; cursor:default;' title='Habilitado'";
							}
							if (options.data.CodigoEstado == 1) {
								estado = " style='color:red; cursor:default;' title='Inhabilitado'";
							}
							if (options.data.CodigoEstado == 2) {
								estado = " style='color:grey; cursor:default;' title='Procesado'";
							}


						} catch (err) {

						}
					}
                    , loadPanel: {
                    	enabled: true
                    }
                      , allowColumnResizing: true
                 , columns: [
                     {                     	
                     	width: 50,
                     	cellTemplate: function (container, options) {
                     		$("<div style='text-align:center'>")
								.append($("<a target='_blank' class='icon-file-eye' onClick=ConsultarDetalle('" + options.data.id + "')  >"))
								.appendTo(container);
                     	}
                     },
                     {
                     	
                     	caption: "Fecha",
                     	dataField: "Fecha",
                     	dataType: "date",
                     	format: "yyyy-MM-dd HH:mm"
                     },
					
                 {					
                 	caption: "Doc. Facturador",
                 	dataField: "Facturador"
                 },

                      {                      	
                      	caption: "Empresa Compra",
                      	dataField: "EmpresaFacturador"
                      },

                      {                      	
                      	caption: "Valor",
                      	dataField: "Valor"
                      },
					   {					   
					   	caption: "Transacciones",
					   	dataField: "TCompra"
					   }
                      ,
                     {                     	
                     	caption: "Procesadas",
                     	dataField: "TProcesadas"
                     }
                     ,
                     {                     
                     	caption: "Saldo",
                     	dataField: "Saldo"
                     },
					 
                 {
                 	dataField: "porcentaje",
                 	caption: "Porcentaje %",
                 	dataType: "number",
                 	format: "percent",
                 	alignment: "right",
                 	allowGrouping: false,
                 	cellTemplate: discountCellTemplate,
                 	cssClass: "bullet"
                 },
                 {                 	
                 	caption: "Empresa",
                 	dataField: "Empresa",

                 },
                     {
                  
                     	caption: "Usuario",
                     	dataField: "Usuario"
                     }
                     ,
                     {
                  
                     	caption: "Tipo",
                     	dataField: "Tipoproceso"
                     }

                     ,
                      {
                  
                      	caption: 'Estado',
                      	dataField: 'Estado',
                      	cellTemplate: function (container, options) {
                      		$("<div style='text-align:center'>")
								.append($("<a taget=_self class='icon-circle2'" + estado + ">"))
								.appendTo(container);
                      	}
                      }
                 ], summary: {
                 	groupItems: [{
                 		column: "Valor",
                 		summaryType: "sum",
                 		displayFormat: " {0} Total ",
                 		valueFormat: "currency"
                 	}]

                    , totalItems: [{
                    	column: "Valor",
                    	displayFormat: "{0}",
                    	valueFormat: "currency",
                    	summaryType: "sum"
                    }, {
                    	column: "TCompra",
                    	summaryType: "sum",
                    	valueFormat: 'fixedPoint',
                    	displayFormat: '{0}'

                    }
                    ]
                 }
                    , filterRow: {
                    	visible: true
                    }
				});


			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 7000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
		});
	}


	ConsultarDetalle = function (IdSeguridad) {
		$('#modal_detalle_plan').modal('show');
		$("#wait").show();
		$http.get('/api/PlanesTransacciones?IdSeguridad=' + IdSeguridad).then(function (response) {
			$("#wait").hide();
			$scope.Empresa = response.data[0].Empresa;
			$scope.Usuario = response.data[0].Usuario;
			$scope.Valor = response.data[0].Valor;
			$scope.TCompra = response.data[0].TCompra;
			$scope.TProcesadas = response.data[0].TProcesadas;
			$scope.TDisponibles = response.data[0].TDisponibles;
			$scope.id = response.data[0].id;
			$scope.Fecha = response.data[0].Fecha;
			$scope.CodigoEmpresaFacturador = response.data[0].CodigoEmpresaFacturador;
			$scope.EmpresaFacturador = response.data[0].EmpresaFacturador;
			$scope.Tipo = response.data[0].Tipo;
			$scope.Observaciones = response.data[0].Observaciones;
			$scope.Estado = response.data[0].Estado;
			$scope.FechaVence = response.data[0].FechaVence;

			///////////////////////////////////////////////////////////////////////				
			$("#progressBarStatus").dxProgressBar({
				min: 0,
				max: 100,
				width: "100%",
				statusFormat: function (value) {
					return value * 100 + "%";
				}
			});

			if ($scope.Tipo == 3) {
				nivel(100, $scope.Tipo);
			} else {
				var porcentaje = ($scope.TProcesadas.toString().replace(',', '.') / $scope.TCompra.toString().replace(',', '.')) * 100;
				nivel(porcentaje, $scope.Tipo);
			}
			///////////////////////////////////////////////////////////////////////

		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
		});
	}



	function nivel(nivel, tipo) {
		if (tipo == 3) {
			nivel = 100;
			color = '#5cb85c'; //Verde
		} else {
			var color = '#FE2E2E';
			if (nivel >= 71 && nivel <= 90) {
				color = '#E8BE0C';
			}

			if (nivel > 90) {
				color = '#FE2E2E';
			}

			if (nivel <= 70) {
				color = '#5cb85c';
			}
		}

		$("#progressBarStatus").dxProgressBar({ value: nivel });
		$('div.dx-progressbar-range').css('background-color', color);
		$('div.dx-progressbar-range').css('border', '1px solid  ' + color);
		return color;
	}

	var discountCellTemplate = function (container, options) {

		var porcentaje = (options.data.TProcesadas > 0) ? (options.data.TProcesadas / options.data.TCompra) * 100 : 100;
		var color = nivel(porcentaje, options.data.CodCompra);
		$("<div/>").dxBullet({
			onIncidentOccurred: null,
			size: {
				width: 60,
				height: 35
			},
			margin: {
				top: 5,
				bottom: 0,
				left: 5
			},
			showTarget: false,
			showZeroLevel: true,
			value: porcentaje,
			startScaleValue: 0,
			endScaleValue: 100,
			color: color,
			tooltip: {
				enabled: true,
				font: {
					size: 18
				},
				paddingTopBottom: 2,
				customizeTooltip: function () {
					return { text: porcentaje + '%' };
				},
				zIndex: 5
			}
		}).appendTo(container);
	};

	var collapsed = false;

});


//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
	for (var i = 0; i < miArray.length; i += 1) {
		if (ID == miArray[i].ID) {
			return i;
		}
	}
}

var EstadosPlanes =
    [
        { ID: 1, Texto: 'Inhabilitar' },
        { ID: 0, Texto: 'Habilitar' },

    ];
var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Recarga' },
    { ID: "2", Texto: 'Vencimiento' }
    ];


var Lista_Tipo_plan =
    [
    { ID: "1", Texto: 'Cortesía' },
    { ID: "2", Texto: 'Compra' },
    { ID: "3", Texto: 'Post-Pago' }
    ];

var Lista_Estado_plan =
    [
    { ID: "0", Texto: 'Habilitado' },
    { ID: "1", Texto: 'Inhabilitado' },
    { ID: "2", Texto: 'Procesado' }
    ];