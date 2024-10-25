DevExpress.localization.locale('es-ES');

var id_seguridad = "*";
var items_recibo = [];
var UsuarioSession = "";

//Desde hasta en la consulta de la grid
var Desde = 0;
var Hasta = 20;
var CantRegCargados = 0;
var CantidadRegCorreosRecibidos = 1000;
//*********************************


var App = angular.module('App', ['dx', 'AppMaestrosEnum', 'AppSrvFiltro']);
App.controller('RecepcionCorreoController', function RecepcionCorreoController($scope, $http, $location, SrvMaestrosEnum, $rootScope, SrvFiltro) {
	////Google Analytics
	//ga('send', 'event', 'Pages_DocumentosAdmin', 'Consulta', 'Consulta Documentos');

	//Declaramos el array
	var Correos = [];
	var AlmacenCorreos = new DevExpress.data.ArrayStore({
		key: "StrIdSeguridad",
		data: Correos
	});
	//****************

	var now = new Date();
	var Estado;

	var    estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
            tipo_filtro_fecha = 1;



	cargarFiltros();

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


		var DatosEstados = function () {
			return new DevExpress.data.CustomStore({
				loadMode: "raw",
				key: "ID",
				load: function () {
					//try {
					//	return JSON.parse(JSON.stringify(Estado));
					//} catch (e) {
					var datos = [{ ID: '1', Descripcion: '1 - Rechazado' },
					{ ID: '2', Descripcion: '2 - Recibido' },
					{ ID: '3', Descripcion: '3 - Procesado' },];
					return JSON.parse(JSON.stringify(datos));
					//}
				}
			});
		};


		$("#filtrosEstadoRecibo").dxDropDownBox({
			valueExpr: "ID",
			placeholder: "Seleccione un Item",
			displayExpr: "Descripcion",
			showClearButton: true,
			dataSource: DatosEstados(),
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
                    	height: 250,
                    	selection: { mode: "multiple" },
                    	selectedRowKeys: value,
                    	onSelectionChanged: function (selectedItems) {
                    		var keys = selectedItems.selectedRowKeys;
                    		e.component.option("value", keys);
                    		estado_recibo = keys;
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
            	TipoFiltroFecha: {
            		//Carga la data del control
            		dataSource: new DevExpress.data.ArrayStore({
            			data: TiposFiltroFecha,
            			key: "ID"
            		}),
            		displayExpr: "Texto",
            		value: TiposFiltroFecha[0],

            		onValueChanged: function (data) {
            			if (data.value != null) {
            				tipo_filtro_fecha = data.value.ID;
            			} else {
            				tipo_filtro_fecha = 1;
            			}
            		}
            	}
            }


		$("#FechaFinal").dxDateBox({ min: now });
		$("#FechaInicial").dxDateBox({ max: now });


	}

	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultar2();
		}
	};


	consultar();
	function consultar() {

		$("#gridCorreosRecibidos").dxDataGrid({
			dataSource: {
				store: AlmacenCorreos,
				reshapeOnPush: true
			},
			onToolbarPreparing: function (e) {
				e.toolbarOptions.items.unshift(
				{
					location: "after",
					widget: "dxButton",
					options: {
						icon: "clear", elementAttr: { title: "Reordenar Columnas" },
						onClick: function () {
							localStorage.removeItem('storageConsultarCorreosRecibidos');
							consultar();
						}
					}
				})
			},
			columnAutoWidth: true,
			scrolling: {
				columnRenderingMode: "virtual",
				//mode: "infinite",
				preloadEnabled: true,
				renderAsync: undefined,
				rowRenderingMode: "virtual",
				scrollByContent: true,
				scrollByThumb: true,
				showScrollbar: "always",
				useNative: "auto"
			},
			stateStoring: {
				enabled: false,
				type: 'localStorage',
				storageKey: 'storageConsultarCorreosRecibidos',
			},
			onInitialized(e) {
				e.component.option("stateStoring.ignoreColumnOptionNames", ["filterValues"]);
			},
			//*********************

			keyExpr: "StrId",
			paging: {
				pageSize: 20,
				enabled: true
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

				}, loadPanel: {
					enabled: true
				}
				, focusedRowEnabled: true
				, hoverStateEnabled: true
				, headerFilter: {
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
                    caption: "Id Seguridad",
                    dataField: "StrId",
                    visible: false
                },
                {
					caption: "Fecha Recepción",
					dataField: "DatFechaRegistro",
					dataType: "date",
					format: "yyyy-MM-dd HH:mm:ss",

				},            				
					{
						caption: "Fecha Correo",
						dataField: "DatFechaCorreo",
						dataType: "date",
						format: "yyyy-MM-dd ",

						validationRules: [{
							type: "required",
							message: "El campo Fecha es obligatorio."
						}]
					},
					{
						caption: "Proceso",
						dataField: "IntProceso",
						cssClass: "hidden-xs col-md-1",
						lookup: {
							dataSource: TipoProcesoRecepcion,
							displayExpr: "Name",
							valueExpr: "ID"
						},
						
					},
					{
						caption: "Remitente",
						dataField: "StrRemitente"
					}, {
						caption: "Asunto",
						dataField: "StrAsunto"
					},
					{
						caption: "Estado",
						dataField: "IntEstado",
						cssClass: "hidden-xs col-md-1",
					   	lookup: {
					   		dataSource: EstadoProcesoRecepcion,
					   		displayExpr: "Name",
					   		valueExpr: "ID"
					   	},
					   	cellTemplate: function (container, options) {

					   		$("<div>")

								.append($(ColocarEstadoRecepcion(options.data.IntEstado)))
								.appendTo(container);
					   	}
					},
					{
						caption: "Observaciones",
						dataField: "StrObservaciones"
					},	 
				],

			filterRow: {
				visible: true
			}

		});

	}


	function consultar2() {
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		$http.get('/api/ObtenerRecepcionDocumentos?estado=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&Desde=' + Desde + '&Hasta=' + Hasta).then(function (datos) {
			$('#waitRegistros').show();
			Correos = [];
			AlmacenCorreos = new DevExpress.data.ArrayStore({
				key: "StrId",
				data: Correos
			});

			cargarDocumentos(datos.data);

			$("#gridCorreosRecibidos").dxDataGrid({
				dataSource: {
					store: AlmacenCorreos,
					reshapeOnPush: true
				},
				paging: {
					pageSize: 20,
					enabled: true
				}
			});
			//*******

			//*************************************************************************				
			CantRegCargados = AlmacenCorreos._array.length;
			CargarAsyn();
			function CargarAsyn() {
				$http.get('/api/ObtenerRecepcionDocumentos?estado=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&Desde=' + CantRegCargados + '&Hasta=' + CantidadRegCorreosRecibidos).then(function (datos) {
					CantRegCargados += datos.data.length;
					//CantidadRegCorreosRecibidos += hasta;
					if (datos.data.length > 0) {
						cargarDocumentos(datos.data);
						CargarAsyn();
					} else {
						$('#waitRegistros').hide();
					}

				});
			}
			//*************************************************************************
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 10000);
		});

	}


	//Carga las empresas al array
	function cargarDocumentos(data) {
		if (data != "") {
			data.forEach(function (d) {
				Correos = d;
				try {
					AlmacenCorreos.push([{ type: "insert", data: Correos }]);
				} catch (e) {
				}
			});
		}
	}

});


var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Fecha Recepción' },
    { ID: "2", Texto: 'Fecha Documento' }
    ];

var TipoProcesoRecepcion =
[
{ "ID": 0, "Name": "Recepción" },
{ "ID": 1, "Name": "Emisión" },
];

var EstadoProcesoRecepcion =
[
{ "ID": 1, "Name": "Rechazado" },
{ "ID": 2, "Name": "Recibido" },
{ "ID": 3, "Name": "Procesado" },
];


