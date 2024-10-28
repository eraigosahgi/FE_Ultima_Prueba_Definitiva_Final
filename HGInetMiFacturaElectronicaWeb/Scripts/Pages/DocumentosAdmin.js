DevExpress.localization.locale('es-ES');
var email_destino = "";
var id_seguridad = "*";
var items_recibo = [];
var UsuarioSession = "";
var numero_documento_val = "";

//Desde hasta en la consulta de la grid
var Desde = 0;
var Hasta = 20;
var CantRegCargados = 0;
//*********************************

var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var App = angular.module('App', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento', 'ModalEmpresasApp', 'AppSrvFiltro']);
App.controller('DocObligadoController', function DocObligadoController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento, $rootScope, SrvFiltro) {
	//Google Analytics
	ga('send', 'event', 'Pages_DocumentosAdmin', 'Consulta', 'Consulta Documentos');

	//Declaramos el array
	var Documentos = [];
	var AlmacenDocumentos = new DevExpress.data.ArrayStore({
		key: "StrIdSeguridad",
		data: Documentos
	});
	//****************
	var codigo = 'StrIdentificacion';
	var descripcion = 'StrRazonSocial';
	$("#facturador").dxLookup({
		showClearButton: true,
		clearButtonText: 'Limpiar',
		tabIndex: 1,
		dropDownOptions: {
			hideOnOutsideClick: true,
			showTitle: true,
		},
		dataSource: DevExpress.data.AspNet.createStore({
			key: codigo,
			loadUrl: '../../Api/EmpresasApi/Consultar',
			loadParams: { select: "[ '" + codigo + "','" + descripcion + "']" },
			onBeforeSend: function (method, ajaxOptions) {
				//	ajaxOptions.data.sort = '[{"selector":' + "'" + descripcion + "'" + ',"desc":false}]'
				ajaxOptions.headers = {
					//		'Authorization': 'Bearer ' + sessionStorage.getItem('Token')
				};
			}
		}),
		onValueChanged: function (data) {
			txt_hgi_Facturador = data.value;
		},
		searchExpr: [codigo, descripcion],
		valueExpr: codigo,
		displayExpr: descripcion,
		fieldTemplate: function (datos) {
			try {
				return datos[codigo] + ' - ' + datos[descripcion];
			} catch (e) {

			}
		},
		itemTemplate: function (datos) {

			return datos[codigo] + ' - ' + datos[descripcion];

		},
	});

	var now = new Date();
	var Estado;

	var codigo_facturador = "",
		   numero_documento = "",
		   estado_dian = "",
		   estado_recibo = "",
		   fecha_inicio = "",
		   fecha_fin = "",
		   cod_facturador = "*",
			tipo_filtro_fecha = 1,
		   Datos_Tipo = "0";



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
					var datos = [{ ID: '0', Descripcion: 'No Recibido' },
					{ ID: '100', Descripcion: 'Recibido Plataforma' },
					{ ID: '200', Descripcion: 'Envío DIAN' },
					{ ID: '300', Descripcion: 'Validado DIAN' },
					{ ID: '400', Descripcion: 'Fallido DIAN' }];
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
							estado_dian = keys;
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
				},
				EstadoRecibo: {
					searchEnabled: true,
					//Carga la data del control
					dataSource: new DevExpress.data.ArrayStore({
						data: AdquirienteRecibo,
						key: "ID"
					}),
					displayExpr: "Name",
					Enabled: true,
					placeholder: "Seleccione un Item",
					onValueChanged: function (data) {
						estado_recibo = data.value.ID;
					}
				},
				NumeroDocumento: {
					placeholder: "Ingrese Número Documento",
					showClearButton: true,
					onValueChanged: function (data) {
						numero_documento = (data.value == null) ? "" : data.value;
					}
				}
			}


		$("#FechaFinal").dxDateBox({ min: now });
		$("#FechaInicial").dxDateBox({ max: now });



		$("#TipoDocumento").dxSelectBox({
			placeholder: "Seleccione el Tipo",
			displayExpr: "Texto",
			dataSource: items_Tipo,
			onValueChanged: function (data) {
				Datos_Tipo = data.value.ID;
			}
		});


	}

	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultarnew();
		}
	};




	function consultarnew() {
		$("#gridDocumentos").dxDataGrid({
			dataSource: DevExpress.data.AspNet.createStore({
				key: 'StrIdSeguridad',
				loadUrl: '/Api/Documentos/Consultar',
				onBeforeSend: function (method, ajaxOptions) {

					ajaxOptions.data.Select = "['IntAdquirienteRecibo','StrCufe','StrIdSeguridad','DatFechaIngreso','StrEmpresaFacturador','StrNumResolucion','StrPrefijo','IntNumero','IdCategoriaEstado','IntIdEstado','DatFechaVencDocumento','IntVlrTotal','IntDocTipo','StrEmpresaAdquiriente','StrUrlArchivoPdf', 'StrUrlArchivoUbl', 'IntAdquirienteRecibo', 'StrUrlAcuseUbl', 'StrUrlAnexo']"

					ajaxOptions.data.sort = "[{ 'selector': 'DatFechaIngreso', 'desc': true }]";

					var fecha_desde = new Date($("#FechaInicial").dxDateBox("instance").option().value);
					fecha_desde = fecha_desde.getFullYear() + '-' + (fecha_desde.getMonth() + 1) + '-' + fecha_desde.getDate() + 'T00:00:00.000'

					var fecha_hasta = new Date($("#FechaFinal").dxDateBox("instance").option().value);
					fecha_hasta = fecha_hasta.getFullYear() + '-' + (fecha_hasta.getMonth() + 1) + '-' + (fecha_hasta.getDate() + 1) + 'T00:00:00.000'

					if (ajaxOptions.data.filter == undefined) {
						ajaxOptions.data.filter = '[["DatFechaIngreso", ">=", "' + fecha_desde + '"], "and", ["DatFechaIngreso", "<", "' + fecha_hasta + '"]]';
					} else {
						ajaxOptions.data.filter = '[' + ajaxOptions.data.filter + ' , "and",  [["DatFechaIngreso", ">=", "' + fecha_desde + '"], "and", ["DatFechaIngreso", "<", "' + fecha_hasta + '"]]]';
					}

					let estado = $("#filtrosEstadoRecibo").dxDropDownBox("instance").option().value;

					if (estado != null && estado != undefined && estado != '[]') {
						ajaxOptions.data.filter = '[' + ajaxOptions.data.filter + ',"and", ["IdCategoriaEstado","=",' + estado + ']]'
					}

					let tipodoc = $("#TipoDocumento").dxSelectBox("instance").option().value;
					if (tipodoc != null) {
						if (tipodoc.ID != "0") {
							ajaxOptions.data.filter = '[' + ajaxOptions.data.filter + ',"and", ["IntDocTipo","=",' + tipodoc.ID + ']]'
						}
					}

					let tercero = $('#facturador').dxLookup("instance").option().value;

					if (tercero != null && tercero != undefined) {
						ajaxOptions.data.filter = '[' + ajaxOptions.data.filter + ',"and", ["StrEmpresaFacturador","contains",' + tercero + ']]'
					}

					if (numero_documento != "") {
						ajaxOptions.data.filter = '[["IntNumero", "=", ' + numero_documento + ']]';
					}

					ajaxOptions.headers = {						
					};
				}
			}),
			focusedRowEnabled: true
			, hoverStateEnabled: true
			, headerFilter: {
				visible: true
			}
			, allowColumnResizing: true
			, allowColumnReordering: true
			, columnAutoWidth: true
			, columnChooser: {
				enabled: true,
				mode: "select",
				title: "Selector de Columnas"
			},
			sorting: {
				mode: 'multiple',
			},
			rowAlternationEnabled: true,
			showRowLines: true,
			showColumnLines: true,
			showBorders: true,
			focusedRowEnabled: true,
			filterRow: {
				visible: true,
			},
			headerFilter: {
				visible: true
			},
			remoteOperations: true,
			scrolling: {
				rowRenderingMode: 'virtual',
				scrollByContent: true,
				scrollByThumb: true,
				showScrollbar: "always",
				useNative: "auto"
			},
			paging: {
				pageSize: 20,
			},
			pager: {
				visible: true,
				allowedPageSizes: [20, 1000, 50000],
				showPageSizeSelector: true,
				showInfo: true,
				showNavigationButtons: true,
			},
			columns: [
			{
				caption: "StrIdSeguridad",
				dataField: "StrIdSeguridad",
				visible: false
			},
			{
				caption: "Fecha De Ingreso",
				dataField: "DatFechaIngreso",
				dataType: "date",
				format: "yyyy-MM-dd HH:mm:ss",
			},
			{
				caption: "Facturador",
				dataField: "StrEmpresaFacturador",
			},
			{
				caption: "Nombre o Razon Social",
				dataField: "StrEmpresaFacturador",
				filterRow: false
				,
				lookup: {
					dataSource: DevExpress.data.AspNet.createStore({
						key: codigo,
						loadUrl: '../../Api/EmpresasApi/Consultar',
						loadParams: { select: "[ 'StrIdentificacion','StrRazonSocial']" },
						onBeforeSend: function (method, ajaxOptions) {

							ajaxOptions.headers = {

							};
						}
					}),
					valueExpr: 'StrIdentificacion',
					displayExpr: 'StrRazonSocial'
				}
			},
			{
				caption: "Resolución",
				dataField: "StrNumResolucion",
			},
			{
				caption: "Prefijo",
				dataField: "StrPrefijo",
			},
			{
				caption: "Documento",
				dataField: "IntNumero",
			},
			{
				caption: "Fecha Vencimiento",
				dataField: "DatFechaVencDocumento",
				visible: false,
				dataType: "date",
				format: "yyyy-MM-dd",

			},
						//{
						//	caption: "IdFacturador",

						//	dataField: "IdFacturador"
						//}, {
						//	caption: "Facturador",

						//	dataField: "Facturador"
						//},
						{
							caption: "Valor Total",
							visible: true,
							dataField: "IntVlrTotal"
						},
						//{
						//	caption: "SubTotal",
						//	visible: false,
						//	dataField: "IntSubTotal"
						//},
						//{
						//	caption: "Neto",
						//	visible: false,
						//	dataField: "IntNeto"
						//},
						 {
						 	caption: "Tipo Documento",
						 	dataField: "IntDocTipo",
						 	lookup: {
						 		dataSource: items_Tipo,
						 		displayExpr: "Texto",
						 		valueExpr: "ID"

						 	},
						 },
						 {
						 	caption: "Adquiriente",

						 	dataField: "StrEmpresaAdquiriente"
						 },
						  //{
						  //	caption: "Nombre Adquiriente",

						  //	dataField: "NombreAdquiriente"
						  //},
			////
			 {
			 	dataField: "IdCategoriaEstado",
			 	caption: "Estado",
			 	cssClass: "hidden-xs col-md-1",
			 	lookup: {
			 		dataSource: CategoriaEstado,
			 		displayExpr: "Name",
			 		valueExpr: "ID"
			 	},
			 	cellTemplate: function (container, options) {

			 		$("<div>")

						.append($(ColocarEstado(options.data.IdCategoriaEstado, options.data.IdCategoriaEstado)))
						.appendTo(container);
			 	}
			 },
								  {
								  	caption: "Proceso",
								  	visible: true,
								  	dataField: "IntIdEstado",
								  	lookup: {
								  		dataSource: ProcesoEstado,
								  		displayExpr: "Name",
								  		valueExpr: "ID"
								  	},

								  },

			 					  {
			 					  	caption: "Estado Acuse",
			 					  	dataField: "IntAdquirienteRecibo",
			 					  	cssClass: "hidden-xs col-md-1",
			 					  	lookup: {
			 					  		dataSource: AdquirienteRecibo,
			 					  		displayExpr: "Name",
			 					  		valueExpr: "ID"
			 					  	},
			 					  	cellTemplate: function (container, options) {

			 					  		numero_documento_val = options.data.NumeroDocumento;
			 					  		$("<div>")
			 								.append($(ColocarEstadoAcuseAdmin(options.data.IntAdquirienteRecibo, options.data.TituloValor, options.data.StrIdSeguridad, options.data.NumeroDocumento, codigo_facturador)))
			 					  			.appendTo(container);
			 					  	}
			 					  },
			// 					  {
			// 					  	caption: "Motivo Rechazo",
			// 					  	visible: false,
			// 					  	dataField: "MotivoRechazo",
			// 					  },
			// 					{
			// 						caption: "Estado Email",
			// 						visible: false,
			// 						dataField: "EstadoEnvioMail",
			// 						lookup: {
			// 							dataSource: EstadoEnvio,
			// 							displayExpr: "Name",
			// 							valueExpr: "ID"
			// 						},
			// 						cellTemplate: function (container, options) {

			// 							$("<a>")
			// 								.append($(ColocarEstadoEmail(options.data.EnvioMail, options.data.MensajeEnvio, options.data.EstadoEnvioMail, options.data.StrIdSeguridad)))
			// 								.appendTo(container);
			// 						}
			// 					}
			],
			masterDetail: {
				enabled: true,
				template: function (container, options) {
					let Respuesta_DIAN = "";

					try {
						options.data.StrUrlArchivoUbl.replace("FacturaEDian", "FacturaEConsultaDian");
					} catch (e) {

					}

					container.append(ObtenerDetallle(options.data.StrUrlArchivoPdf, options.data.StrUrlArchivoUbl, options.data.IntAdquirienteRecibo, "options.data.RutaAcuse", options.data.StrUrlAcuseUbl, options.data.StrUrlAnexo, Respuesta_DIAN, options.data.StrIdSeguridad, options.data.IdFacturador, options.data.IntNumero, "Admin", options.data.IntIdEstado));
				}
			}
		});
	}

	function consultarPivot() {

		const pivotGridChart = $('#pivotgrid-chart').dxChart({
			commonSeriesSettings: {
				type: 'bar',
			},			
			size: {
				height: 200,
			},
			adaptiveLayout: {
				width: 450,
			},
		}).dxChart('instance');







		const pivotGrid = $("#gridDocumentos").dxPivotGrid({
			allowSortingBySummary: true,
			allowFiltering: true,
			showBorders: true,
			showColumnGrandTotals: false,
			showRowGrandTotals: false,
			showRowTotals: false,
			showColumnTotals: false,
			fieldChooser: {
				enabled: true,
				height: 400,
			},
			dataSource: {
				store: DevExpress.data.AspNet.createStore({
					key: 'StrIdSeguridad',
					loadUrl: '/Api/Documentos/Consultar',
					onBeforeSend: function (method, ajaxOptions) {



						//ajaxOptions.data.Select = "['IntAdquirienteRecibo','StrCufe','StrIdSeguridad','DatFechaIngreso','StrEmpresaFacturador','StrNumResolucion','StrPrefijo','IntNumero','IdCategoriaEstado','IntIdEstado','DatFechaVencDocumento','IntVlrTotal','IntDocTipo','StrEmpresaAdquiriente','StrUrlArchivoPdf', 'StrUrlArchivoUbl', 'IntAdquirienteRecibo', 'StrUrlAcuseUbl', 'StrUrlAnexo']"
						ajaxOptions.data.Select = "['DatFechaIngreso','StrEmpresaFacturador','IntNumero']"

						ajaxOptions.data.sort = "[{ 'selector': 'DatFechaIngreso', 'desc': true }]";

						var fecha_desde = new Date($("#FechaInicial").dxDateBox("instance").option().value);
						fecha_desde = fecha_desde.getFullYear() + '-' + (fecha_desde.getMonth() + 1) + '-' + fecha_desde.getDate() + 'T00:00:00.000'

						var fecha_hasta = new Date($("#FechaFinal").dxDateBox("instance").option().value);
						fecha_hasta = fecha_hasta.getFullYear() + '-' + (fecha_hasta.getMonth() + 1) + '-' + (fecha_hasta.getDate() + 1) + 'T00:00:00.000'


						if (ajaxOptions.data.filter == undefined) {
							ajaxOptions.data.filter = '[["DatFechaIngreso", ">=", "' + fecha_desde + '"], "and", ["DatFechaIngreso", "<", "' + fecha_hasta + '"]]';
						} else {
							ajaxOptions.data.filter = '[' + ajaxOptions.data.filter + ' , "and",  [["DatFechaIngreso", ">=", "' + fecha_desde + '"], "and", ["DatFechaIngreso", "<", "' + fecha_hasta + '"]]]';
						}

						let estado = $("#filtrosEstadoRecibo").dxDropDownBox("instance").option().value;

						if (estado != null && estado != undefined && estado != '[]') {
							ajaxOptions.data.filter = '[' + ajaxOptions.data.filter + ',"and", ["IdCategoriaEstado","=",' + estado + ']]'
						}

						let tipodoc = $("#TipoDocumento").dxSelectBox("instance").option().value;
						if (tipodoc != null) {
							if (tipodoc.ID != "0") {
								ajaxOptions.data.filter = '[' + ajaxOptions.data.filter + ',"and", ["IntDocTipo","=",' + tipodoc.ID + ']]'
							}

						}


						let tercero = $('#facturador').dxLookup("instance").option().value;

						if (tercero != null && tercero != undefined) {
							ajaxOptions.data.filter = '[' + ajaxOptions.data.filter + ',"and", ["StrEmpresaFacturador","contains",' + tercero + ']]'
						}

						if (numero_documento != "") {
							ajaxOptions.data.filter = '[["IntNumero", "=", ' + numero_documento + ']]';
						}

						ajaxOptions.headers = {
							//	'Authorization': 'Bearer ' + sessionStorage.getItem('Token')
						};
					},
				}),


				fields: [
			//{
			//	caption: "StrIdSeguridad",
			//	dataField: "StrIdSeguridad",
			//	visible: false
			//},			
			{
				caption: "Fecha De Ingreso",
				dataField: "DatFechaIngreso",
				dataType: "date",				
				area: 'column',
			},
			{
				caption: "Mes",
				dataField: "DatFechaIngreso",
				dataType: "month",				
				area: 'column',
			},
			{
				caption: "Facturador",
				dataField: "StrEmpresaFacturador",
				area: 'row',
			},
			//{
			//	caption: "Resolución",
			//	dataField: "StrNumResolucion",
			//},
			//{
			//	caption: "Prefijo",
			//	dataField: "StrPrefijo",
			//},
			{
				caption: "Documento",
				dataField: "IntNumero",
				summaryType: 'count',
				sortOrder: 'desc',
				area: 'data',
			},
			//{
			//	caption: "Fecha Vencimiento",
			//	dataField: "DatFechaVencDocumento",
			//	visible: false,
			//	dataType: "date",
			//	format: "yyyy-MM-dd",

			//},
			//			{
			//				caption: "Valor Total",
			//				visible: true,
			//				dataField: "IntVlrTotal",
			//				format: 'currency',
			//			},
			//			 {
			//			 	caption: "Tipo Documento",
			//			 	dataField: "IntDocTipo",
			//			 	lookup: {
			//			 		dataSource: items_Tipo,
			//			 		displayExpr: "Texto",
			//			 		valueExpr: "ID"

			//			 	},
			//			 },
			//			 {
			//			 	caption: "Adquiriente",

			//			 	dataField: "StrEmpresaAdquiriente"
			//			 },
			// {
			// 	dataField: "IdCategoriaEstado",
			// 	caption: "Estado",
			// 	cssClass: "hidden-xs col-md-1",
			// 	lookup: {
			// 		dataSource: CategoriaEstado,
			// 		displayExpr: "Name",
			// 		valueExpr: "ID"
			// 	},
			// 	cellTemplate: function (container, options) {

			// 		$("<div>")

			//			.append($(ColocarEstado(options.data.IdCategoriaEstado, options.data.IdCategoriaEstado)))
			//			.appendTo(container);
			// 	}
			// },
			//					  {
			//					  	caption: "Proceso",
			//					  	visible: true,
			//					  	dataField: "IntIdEstado",
			//					  	lookup: {
			//					  		dataSource: ProcesoEstado,
			//					  		displayExpr: "Name",
			//					  		valueExpr: "ID"
			//					  	},

			//					  },

			// 					  {
			// 					  	caption: "Estado Acuse",
			// 					  	dataField: "IntAdquirienteRecibo",
			// 					  	cssClass: "hidden-xs col-md-1",
			// 					  	lookup: {
			// 					  		dataSource: AdquirienteRecibo,
			// 					  		displayExpr: "Name",
			// 					  		valueExpr: "ID"
			// 					  	},
			// 					  	cellTemplate: function (container, options) {

			// 					  		numero_documento_val = options.data.NumeroDocumento;
			// 					  		$("<div>")
			// 								.append($(ColocarEstadoAcuseAdmin(options.data.IntAdquirienteRecibo, options.data.TituloValor, options.data.StrIdSeguridad, options.data.NumeroDocumento, codigo_facturador)))
			// 					  			.appendTo(container);
			// 					  	}
			// 					  }
				]
			}
		}).dxPivotGrid('instance');



		function expand() {
			const dataSource = pivotGrid.getDataSource();
			dataSource.expandHeaderItem('row', ['StrEmpresaFacturador']);
			dataSource.expandHeaderItem('column', [2024]);
		}

		setTimeout(expand, 0);
	}

	//consultar();
	//function consultar() {

	//	$("#gridDocumentos").dxDataGrid({
	//		dataSource: {
	//			store: AlmacenDocumentos,
	//			reshapeOnPush: true
	//		},
	//		onToolbarPreparing: function (e) {
	//			e.toolbarOptions.items.unshift(
	//			{
	//				location: "after",
	//				widget: "dxButton",
	//				options: {
	//					icon: "clear", elementAttr: { title: "Reordenar Columnas" },
	//					onClick: function () {
	//						localStorage.removeItem('storageConsultarDocumentosAdmin');
	//						consultar();
	//					}
	//				}
	//			})
	//		},
	//		columnAutoWidth: true,
	//		scrolling: {
	//			columnRenderingMode: "virtual",
	//			//mode: "infinite",
	//			preloadEnabled: true,
	//			renderAsync: undefined,
	//			rowRenderingMode: "virtual",
	//			scrollByContent: true,
	//			scrollByThumb: true,
	//			showScrollbar: "always",
	//			useNative: "auto"
	//		},
	//		stateStoring: {
	//			enabled: false,
	//			type: 'localStorage',
	//			storageKey: 'storageConsultarDocumentosAdmin',
	//		},
	//		onInitialized(e) {
	//			e.component.option("stateStoring.ignoreColumnOptionNames", ["filterValues"]);
	//		},
	//		//*********************

	//		keyExpr: "StrIdSeguridad",
	//		paging: {
	//			pageSize: 20,
	//			enabled: true
	//		},
	//		pager: {
	//			showPageSizeSelector: true,
	//			allowedPageSizes: [5, 10, 20],
	//			showInfo: true
	//		}
	//		//Formatos personalizados a las columnas en este caso para el monto
	//			, onCellPrepared: function (options) {
	//				var fieldData = options.value,
	//					fieldHtml = "";
	//				try {
	//					if (options.column.caption == "Valor Total" || options.column.caption == "SubTotal" || options.column.caption == "Neto") {
	//						if (fieldData) {
	//							var inicial = fNumber.go(fieldData);
	//							options.cellElement.html(inicial);
	//						}
	//					}
	//				} catch (err) {
	//				}

	//			}, loadPanel: {
	//				enabled: true
	//			}
	//			, focusedRowEnabled: true
	//			, hoverStateEnabled: true
	//			, headerFilter: {
	//				visible: true
	//			}
	//			, allowColumnResizing: true
	//			, allowColumnReordering: true
	//			, columnChooser: {
	//				enabled: true,
	//				mode: "select",
	//				title: "Selector de Columnas"
	//			},
	//		groupPanel: {
	//			allowColumnDragging: true,
	//			visible: true
	//		}
	//			, columns: [
	//				{
	//					caption: "Archivos",
	//					cellTemplate: function (container, options) {

	//						var permite_envio = "class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:5%; font-size:19px'";

	//						var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

	//						var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

	//						var visible_acuse = " title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

	//						if (options.data.Pdf)
	//							visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer;'";
	//						else
	//							options.data.Pdf = "#";

	//						if (options.data.Xml)
	//							visible_xml = "href='" + options.data.Xml + "'  class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
	//						else
	//							options.data.Xml = "#";

	//						if (options.data.EstadoAcuse == 1 || options.data.EstadoAcuse == 2 || options.data.EstadoAcuse == 3)
	//							visible_acuse = "href='" + options.data.RutaAcuse + "' title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
	//						else
	//							visible_acuse = "#";

	//						$("<div>")
	//							.append(
	//								$("<a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "><a style='margin-left:5%;' target='_blank'  " + visible_xml + ">"),
	//								$("<a " + permite_envio + "></a>").dxButton({
	//									onClick: function () {
	//										$scope.showModal = true;
	//										email_destino = options.data.MailAdquiriente;
	//										id_seguridad = options.data.StrIdSeguridad;
	//										$('input:text[name=EmailDestino]').val("");
	//										SrvDocumento.ConsultarEmailUbl(options.data.StrIdSeguridad).then(function (data) {
	//											$('input:text[name=EmailDestino]').val(data);
	//										});
	//									}
	//								}).removeClass("dx-button dx-button-normal dx-widget")

	//						)
	//							.appendTo(container);

	//					}
	//				},
	//				{
	//					caption: "Documento",

	//					dataField: "NumeroDocumento",

	//				},				
	//				{
	//					caption: "Fecha Documento",
	//					dataField: "DatFechaDocumento",
	//					dataType: "date",
	//					format: "yyyy-MM-dd ",

	//					validationRules: [{
	//						type: "required",
	//						message: "El campo Fecha es obligatorio."
	//					}]
	//				},
	//				{
	//					caption: "Fecha Vencimiento",
	//					dataField: "DatFechaVencDocumento",
	//					visible: false,
	//					dataType: "date",
	//					format: "yyyy-MM-dd",

	//					validationRules: [{
	//						type: "required",
	//						message: "El campo Fecha es obligatorio."
	//					}]
	//				},
	//				{
	//					caption: "IdFacturador",

	//					dataField: "IdFacturador"
	//				}, {
	//					caption: "Facturador",

	//					dataField: "Facturador"
	//				},
	//				{
	//					caption: "Valor Total",
	//					visible: false,
	//					dataField: "IntVlrTotal"
	//				},
	//				{
	//					caption: "SubTotal",
	//					visible: false,
	//					dataField: "IntSubTotal"
	//				},
	//				{
	//					caption: "Neto",
	//					visible: false,
	//					dataField: "IntNeto"
	//				},
	//				 {
	//				 	caption: "Tipo Documento",

	//				 	dataField: "tipodoc"
	//				 },
	//				 {
	//				 	caption: "Adquiriente",

	//				 	dataField: "IdentificacionAdquiriente"
	//				 },
	//				  {
	//				  	caption: "Nombre Adquiriente",

	//				  	dataField: "NombreAdquiriente"
	//				  },

	//				   {
	//				   	dataField: "EstadoCategoria",
	//				   	caption: "Estado",
	//				   	cssClass: "hidden-xs col-md-1",
	//				   	lookup: {
	//				   		dataSource: CategoriaEstado,
	//				   		displayExpr: "Name",
	//				   		valueExpr: "ID"
	//				   	},
	//				   	cellTemplate: function (container, options) {

	//				   		$("<div>")

	//							.append($(ColocarEstado(options.data.Estado, options.data.EstadoCategoria)))
	//							.appendTo(container);
	//				   	}
	//				   },
	//				  {
	//				  	caption: "Proceso",
	//				  	visible: false,
	//				  	dataField: "EstadoFactura",
	//				  	lookup: {
	//				  		dataSource: ProcesoEstado,
	//				  		displayExpr: "Name",
	//				  		valueExpr: "ID"
	//				  	},

	//				  },

	//				  {
	//				  	caption: "Estado Acuse",
	//				  	dataField: "IntAdquirienteRecibo",
	//				  	cssClass: "hidden-xs col-md-1",
	//				  	lookup: {
	//				  		dataSource: AdquirienteRecibo,
	//				  		displayExpr: "Name",
	//				  		valueExpr: "ID"
	//				  	},
	//				  	cellTemplate: function (container, options) {

	//				  		numero_documento_val = options.data.NumeroDocumento;
	//				  		$("<div>")
	//							.append($(ColocarEstadoAcuseAdmin(options.data.IntAdquirienteRecibo, options.data.TituloValor, options.data.StrIdSeguridad, options.data.NumeroDocumento, codigo_facturador)))
	//				  			.appendTo(container);
	//				  	}
	//				  },
	//				  {
	//				  	caption: "Motivo Rechazo",
	//				  	visible: false,
	//				  	dataField: "MotivoRechazo",
	//				  },
	//				{
	//					caption: "Estado Email",
	//					visible: false,
	//					dataField: "EstadoEnvioMail",
	//					lookup: {
	//						dataSource: EstadoEnvio,
	//						displayExpr: "Name",
	//						valueExpr: "ID"
	//					},
	//					cellTemplate: function (container, options) {

	//						$("<a>")
	//							.append($(ColocarEstadoEmail(options.data.EnvioMail, options.data.MensajeEnvio, options.data.EstadoEnvioMail, options.data.StrIdSeguridad)))
	//							.appendTo(container);
	//					}
	//				}
	//			],

	//		masterDetail: {
	//			enabled: true,
	//			template: function (container, options) {
	//				container.append(ObtenerDetallle(options.data.Pdf, options.data.Xml, options.data.EstadoAcuse, options.data.RutaAcuse, options.data.XmlAcuse, options.data.zip, options.data.RutaServDian, options.data.StrIdSeguridad, options.data.IdFacturador, options.data.NumeroDocumento, "Admin", options.data.EstadoFactura));
	//			}
	//		},

	//		summary: {
	//			groupItems: [{
	//				column: "IntVlrTotal",
	//				summaryType: "sum",
	//				displayFormat: " {0} Total ",
	//				valueFormat: "currency"
	//			}, {
	//				column: "IntSubTotal",
	//				summaryType: "sum",
	//				displayFormat: " {0} Neto ",
	//				valueFormat: "currency"
	//			}, {
	//				column: "IntNeto",
	//				summaryType: "sum",
	//				displayFormat: " {0} Neto ",
	//				valueFormat: "currency"
	//			}]
	//			, totalItems: [{
	//				name: "Suma",
	//				showInColumn: "IntVlrTotal",
	//				displayFormat: "{0}",
	//				valueFormat: "currency",
	//				summaryType: "custom"

	//			},
	//			{
	//				name: "SumaSubTotal",
	//				showInColumn: "IntSubTotal",
	//				displayFormat: "{0}",
	//				valueFormat: "currency",
	//				summaryType: "custom"

	//			},
	//			{
	//				name: "SumaNeto",
	//				showInColumn: "IntNeto",
	//				displayFormat: "{0}",
	//				valueFormat: "currency",
	//				summaryType: "custom"

	//			},
	//			{
	//				showInColumn: "DatFechaVencDocumento",
	//				displayFormat: "Total : ",
	//				alignment: "right"
	//			}
	//			],
	//			calculateCustomSummary: function (options) {
	//				if (options.name === "Suma") {
	//					if (options.summaryProcess === "start") {
	//						options.totalValue = 0;
	//						$('#Total').text("");
	//					}
	//					if (options.summaryProcess === "calculate") {
	//						options.totalValue = options.totalValue + options.value.IntVlrTotal;
	//						$('#Total').text("Total: " + fNumber.go(options.totalValue).replace("$-", "-$"));
	//					}
	//				}

	//				if (options.name === "SumaSubTotal") {
	//					if (options.summaryProcess === "start") {
	//						options.totalValue = 0;
	//						$('#SubTotal').text("");
	//					}
	//					if (options.summaryProcess === "calculate") {
	//						options.totalValue = options.totalValue + options.value.IntSubTotal;
	//						$('#SubTotal').text("SubTotal: " + fNumber.go(options.totalValue).replace("$-", "-$"));
	//					}
	//				}


	//				if (options.name === "SumaNeto") {
	//					if (options.summaryProcess === "start") {
	//						options.totalValue = 0;
	//						$('#Neto').text("");
	//					}
	//					if (options.summaryProcess === "calculate") {
	//						options.totalValue = options.totalValue + options.value.IntNeto;
	//						$('#Neto').text("Neto: " + fNumber.go(options.totalValue).replace("$-", "-$"));
	//					}
	//				}
	//			}
	//		},
	//		filterRow: {
	//			visible: true
	//		}

	//	});

	//	ConsultarEventosRadian = function (id_seguridad, codigo_facturador, numero_documento_val) {
	//		$rootScope.ConsultarEventosRadian(id_seguridad, codigo_facturador, numero_documento_val);
	//	}

	//}





	//function consultar2() {
	//	$('#Total').text("");
	//	if (fecha_inicio == "")
	//		fecha_inicio = now.toISOString();

	//	if (fecha_fin == "")
	//		fecha_fin = now.toISOString();


	//	var documentoFacturador = (txt_hgi_Facturador == undefined || txt_hgi_Facturador == '') ? '' : txt_hgi_Facturador;
	//	var codigo_adquiriente = (txt_hgi_Adquiriente == undefined || txt_hgi_Adquiriente == '') ? '' : txt_hgi_Adquiriente;
	//	SrvDocumento.ObtenerDocumentosAdmin(documentoFacturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, Datos_Tipo, tipo_filtro_fecha, Desde, Hasta).then(function (data) {
	//		$('#waitRegistros').show();
	//		Documentos = [];
	//		AlmacenDocumentos = new DevExpress.data.ArrayStore({
	//			key: "StrIdSeguridad",
	//			data: Documentos
	//		});

	//		cargarDocumentos(data);

	//		$("#gridDocumentos").dxDataGrid({
	//			dataSource: {
	//				store: AlmacenDocumentos,
	//				reshapeOnPush: true
	//			},
	//			paging: {
	//				pageSize: 20,
	//				enabled: true
	//			}
	//		});
	//		//*******

	//		//*************************************************************************				
	//		CantRegCargados = AlmacenDocumentos._array.length;
	//		CargarAsyn();
	//		function CargarAsyn() {
	//			SrvDocumento.ObtenerDocumentosAdmin(documentoFacturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, Datos_Tipo, tipo_filtro_fecha, CantRegCargados, CantidadRegDocumentosAdmin).then(function (data) {
	//				CantRegCargados += data.length;
	//				if (data.length > 0) {
	//					cargarDocumentos(data);
	//					CargarAsyn();
	//				} else {
	//					$('#waitRegistros').hide();
	//				}

	//			});
	//		}
	//		//*************************************************************************

	//	}, function errorCallback(response) {
	//		$('#wait').hide();
	//		DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
	//	});


	//}


	//Carga las empresas al array
	function cargarDocumentos(data) {
		if (data != "") {
			data.forEach(function (d) {
				Documentos = d;
				try {
					AlmacenDocumentos.push([{ type: "insert", data: Documentos }]);
				} catch (e) {
				}
			});
		}
	}


	//SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {
	//	codigo_facturador = data[0].IdentificacionEmpresa;
	//	UsuarioSession = data[0].IdSeguridad;
	//	//consultar();
	//});

	//SrvMaestrosEnum.ObtenerEnum(5).then(function (data) {
	//	SrvMaestrosEnum.ObtenerEnum(1).then(function (dataacuse) {
	//		Estado = data;
	//		items_recibo = dataacuse;
	//		cargarFiltros();
	//	});
	//});


	////SrvFiltro.ObtenerFiltro('Documento Facturador', 'Facturador', 'icon-user-tie', 115, '/api/Empresas?Facturador=true', 'Identificacion', 'RazonSocial', false, 7).then(function (Datos) {
	////	$scope.Facturador = Datos;
	////});

	//SrvFiltro.ObtenerFiltro('Documento Adquiriente', 'Adquiriente', 'icon-user-tie', 115, '/api/ObtenerTodosAdquirientes', 'ID', 'Texto', false, 10).then(function (Datos) {
	//	$scope.Adquiriente = Datos;
	//});

});


App.controller('EnvioEmailController', function EnvioEmailController($scope, $http, $location) {

	//Formulario.
	$scope.formOptionsEmailEnvio = {

		readOnly: false,
		showColonAfterLabel: true,
		showValidationSummary: true,
		validationGroup: "DatosEmail",
		onInitialized: function (e) {
			formInstance = e.component;
		},
		items: [{
			itemType: "group",
			items: [
                {
                	dataField: "EmailDestino",
                	editorType: "dxTextBox",
                	label: {
                		text: "E-mail"
                	},
                	validationRules: [{
                		type: "required",
                		message: "El e-mail de destino es obligatorio."
                	}, {
                		//Valida que el campo solo contenga números
                		type: "pattern",
                		pattern: "[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$",
                		message: "El campo no contiene el formato requerido."
                	}]
                }
			]
		}
		]
	};

	//botón Cerrar Modal
	$scope.buttonCerrarModal = {
		text: "CERRAR"
	};

	//Botón Enviar email
	$scope.buttonEnviarEmail = {
		text: "ENVIAR",
		type: "success",
		//useSubmitBehavior: true,
		//validationGroup: "DatosEmail",

		onClick: function (e) {

			try {

				email_destino = $('input:text[name=EmailDestino]').val();

				if (email_destino == "") {
					throw new DOMException("El e-mail de destino es obligatorio.");
				}
				$('#wait').show();
				$http.get('/api/Documentos?id_seguridad=' + id_seguridad + '&email=' + email_destino + '&Usuario=' + UsuarioSession).then(function (responseEnvio) {
					$('#wait').hide();

					var mensaje = 'Ocurrió un error en el envío del e-mail.';
					var envio_exitoso = true;

					if (responseEnvio.data[0].Error != null) {
						mensaje = responseEnvio.data[0].Error.ErrorMessage;
						envio_exitoso = false;
					}

					if (envio_exitoso == true) {
						swal({
							title: 'Proceso Éxitoso',
							text: 'El e-mail ha sido enviado con éxito.',
							type: 'success',
							confirmButtonColor: '#66BB6A',
							confirmButtonTex: 'Aceptar',
							animation: 'pop',
							html: true,
						});
					} else {
						swal({
							title: 'Proceso Fallido',
							text: mensaje,
							type: 'error',
							confirmButtonColor: '#66BB6A',
							confirmButtonTex: 'Aceptar',
							animation: 'pop',
							html: true,
						});
					}

					$('input:text[name=EmailDestino]').val("");
					$('#btncerrarModal').click();
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 10000);
				});

			} catch (e) {
				$('#wait').hide();
				DevExpress.ui.notify(e.message, 'error', 10000);
			}
		}
	};



	ConsultarAuditDoc = function (IdSeguridad, IdFacturador, NumeroDocumento, EstadoFactura) {

		if ((EstadoFactura >= 8 && EstadoFactura < 92) || EstadoFactura == 99) {
			$("#modal_audit_documento").modal('hide');
			$http.get('/api/AuditoriaDocumento?id_seguridad_doc=' + IdSeguridad).then(function (response) {

				if (location.href.includes('habilitacion')) {
					window.open("https://catalogo-vpfe-hab.dian.gov.co/document/searchqr?documentkey=" + response.data, '_blank');
				} else {
					window.open("https://catalogo-vpfe.dian.gov.co/document/searchqr?documentkey=" + response.data, '_blank');
				}

				$("#modal_audit_documento").modal('hide');

			}, function (response) {
				$('#wait').hide();
				$("#modal_audit_documento").modal('hide');
				DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			});
		}
		else {
			ConsultarAuditDoc_old(IdSeguridad, IdFacturador, NumeroDocumento);
		}

	}

	ConsultarAuditDoc_old = function (IdSeguridad, IdFacturador, NumeroDocumento) {
		$http.get('/api/AuditoriaDocumento_old?id_seguridad_doc=' + IdSeguridad).then(function (response) {

			$scope.IdSeguridad = IdSeguridad;
			$scope.NumeroDocumento = NumeroDocumento;
			$scope.Obligado = IdFacturador;

			$("#modal_audit_documento").modal('show');

			$("#gridAuditDocumento").dxDataGrid({
				dataSource: response.data,
				allowColumnResizing: true,
				allowColumnReordering: true,
				paging: {
					pageSize: 10
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				}, loadPanel: {
					enabled: true
				},
				columns: [{
					caption: "Fecha",
					dataField: "DatFecha",
					cssClass: "col-md-2",
				}, {
					caption: "Estado",
					dataField: "StrDesEstado",
					cssClass: "col-md-2"
				}, {
					caption: "Proceso",
					dataField: "StrDesProceso",
					cssClass: "col-md-2"
				}, {
					caption: "Procesado Por",
					dataField: "StrDesProcesadoPor",
					cssClass: "col-md-2"
				}, {
					caption: "Realizado Por",
					dataField: "StrDesRealizadoPor",
					cssClass: "col-md-2"
				}
				],
				masterDetail: {
					enabled: true,
					template: function (container, options) {

						container.append($('<h4 class="form-control">MENSAJE:</h4><p style="width:10%"> ' + options.data.StrMensaje + '</p><br/>'));

						if (options.data.IntIdProceso == 8 || options.data.IntIdProceso == 10) {

							if (options.data.StrResultadoProceso)
								container.append($('<h4 class="form-control">RESPUESTA:</h4><span><p style="width:10%"> ' + options.data.StrResultadoProceso + '</p></span>'));

							if (options.data.StrResultadoProceso) {
								$http.get('/api/DetallesRespuesta?id_proceso=' + options.data.IntIdProceso + '&respuesta=' + options.data.StrResultadoProceso).then(function (response) {

									if (response.data != null) {
										container.append($('<h4 class="form-control">DETALLES RESPUESTA:</h4></br><label><b>Fecha Envío: </b> ' + response.data.Recibido + '</label></br><label><b>ID Remitente : </b> '
										+ response.data.IdRemitente + '</label></br><label><b>ID Contacto : </b> ' + response.data.IdContacto + '</label></br><label><b>Cantidad Adjuntos: </b> '
										+ response.data.Adjuntos + '</label></br><div id="json"></div>'));

										$("#json").dxDataGrid({
											dataSource: response.data.Seguimiento,
											allowColumnResizing: true,
											allowColumnReordering: true,
											paging: {
												pageSize: 10
											},
											pager: {
												showPageSizeSelector: true,
												allowedPageSizes: [5, 10, 20],
												showInfo: true
											}, loadPanel: {
												enabled: true
											},
											columns: [{
												caption: "Fecha Proceso",
												dataField: "FechaEvento",
												dataType: "date",
												format: "yyyy-MM-dd HH:mm:ss",
											},
											{
												dataField: "Tipo Proceso",
												caption: "TipoEvento",
												cellTemplate: function (container, options) {
													$("<div>").append($(ControlTipoEventoMail(options.data.TipoEvento))).appendTo(container);
												}
											},
											]
										}, function (response) {
											$('#wait').hide();
											DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
										});
									}
								});
							}
						} else {
							var cadena_inicio = options.data.StrResultadoProceso.substring(0, 4);

							//Valida si el mensaje de respuesta inicia con http y lo añade como link.
							if (cadena_inicio == "http") {
								container.append($('<h4 class="form-control">RESPUESTA:</h4><pre><a style="margin-left:5%;" target="_blank" href="' + options.data.StrResultadoProceso + '">' + options.data.StrResultadoProceso + '</a></pre>'));
							}
								//Valida si la cadena inicia con {
							else if (options.data.StrResultadoProceso.substring(0, 1) == "{") {
								container.append($('<h4 class="form-control">DETALLES RESPUESTA:</h4>'));

								var datos = angular.fromJson(options.data.StrResultadoProceso)

								var code_html = "";
								//Recorre una cadena json y la carga en código html propiedad por propiedad.
								for (var prop in datos) {

									code_html = code_html + '<label><b>' + prop + ':</b></label>';

									cadena_inicio = datos[prop].toString().substring(0, 4)

									//Valida si el valor de la propiedad es una ruta.
									if (cadena_inicio == "http")
										code_html = code_html + '<a style="margin-left:5%;" target="_blank" href="' + datos[prop] + '">' + datos[prop] + '</a></br>';
									else
										code_html = code_html + '<label>' + datos[prop] + '</label></br>';
								}

								container.append($('<pre>' + code_html + '</pre>'));

							}
							else if (options.data.StrResultadoProceso) {
								container.append($('<h4 class="form-control">RESPUESTA:</h4><span><p style="width:10%"> ' + options.data.StrResultadoProceso + '</p></span>'));
							}

						}
					}
				},
				filterRow: {
					visible: true
				}
			});
		}, function (response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});

	}


})

var items_Tipo =
    [
        { ID: "0", Texto: 'Todos' },
        { ID: "1", Texto: 'Factura Electrónica de Venta' },
        { ID: "2", Texto: 'Nota Debito' },
        { ID: "3", Texto: 'Nota Crédito' },
		{ ID: "4", Texto: 'Acuse de Recibo' },
		{ ID: "5", Texto: 'Attached de Recibo' },
		{ ID: "10", Texto: 'Documento Soporte de Pago de Nómina Electrónica' },
		{ ID: "11", Texto: 'Nota de Ajuste de Documento Soporte de Pago de Nómina Electrónica' },
		{ ID: "15", Texto: 'Documento Soporte Adquisiciones' },
		{ ID: "16", Texto: 'Nota de Ajuste de Adquisiciones' },

    ];

var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Fecha Recepción' },
    { ID: "2", Texto: 'Fecha Documento' }
    ];


