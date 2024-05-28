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
App.controller('MonitorDeRechazosController', function MonitorDeRechazosController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento, $rootScope, SrvFiltro) {
	//Google Analytics
	ga('send', 'event', 'Pages_DocumentosAdmin', 'Consulta', 'Consulta Documentos');

	//Declaramos el array
	var Documentos = [];
	var AlmacenDocumentos = new DevExpress.data.ArrayStore({
		key: "StrIdSeguridad",
		data: Documentos
	});
	//****************

	var now = new Date();
	var Estado;

	var fecha_inicio = new Date(now).toISOString();
	var fecha_fin = new Date(now).toISOString();

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

		$("#gridDocumentos").dxDataGrid({
			dataSource: {
				store: AlmacenDocumentos,
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
							localStorage.removeItem('storageConsultarDocumentosAdmin');
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
			//stateStoring: {
			//	enabled: false,
			//	type: 'localStorage',
			//	storageKey: 'storageConsultarDocumentosAdmin',
			//},
			//onInitialized(e) {
			//	e.component.option("stateStoring.ignoreColumnOptionNames", ["filterValues"]);
			//},
			//*********************

			keyExpr: "StrIdSeguridad",
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
					try {
						if (options.column.caption == "Valor Total" || options.column.caption == "SubTotal" || options.column.caption == "Neto") {
							if (fieldData) {
								var inicial = fNumber.go(fieldData);
								options.cellElement.html(inicial);
							}
						}
					} catch (err) {
					}

				}, loadPanel: {
					enabled: true
				}
				, focusedRowEnabled: true
				, hoverStateEnabled: true
				, headerFilter: {
					visible: true
				}
				, showRowLines: true
				, rowAlternationEnabled: true
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
						caption: "Archivos",
						width: 100,
						cellTemplate: function (container, options) {

							var permite_envio = "class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:5%; font-size:19px'";

							var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

							var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

							var visible_acuse = " title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

							if (options.data.Pdf)
								visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer;'";
							else
								options.data.Pdf = "#";

							if (options.data.Xml)
								visible_xml = "href='" + options.data.Xml + "'  class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
							else
								options.data.Xml = "#";


							var visible_DIAN = "href='" + options.data.RutaServDian + "' class='icon-file-xml' title='ver respuesta DIAN'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";

							$("<div>")
								.append(
									$("<a style='margin-left:10%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "><a style='margin-left:10%;' target='_blank'  " + visible_xml + "><a style='margin-left:10%;' target='_blank'  " + visible_DIAN + ">")).appendTo(container);

						}
					},
					 {
					 	caption: "Mensaje",
					 	visible: true,
					 	dataField: "MensajeError",
					 	cellTemplate: function (container, options) {
					 		try {
					 			const datos = options.data.MensajeError;

					 			for (let i = 0; i < datos.length; i++) {


					 				$('<div>')
									  .addClass('master-detail-caption')
									  .text(`${datos[i]}`)
									  .appendTo(container);
					 			}
					 		} catch (e) {

					 		}
					 	}
					 },
					{
						caption: "Documento",

						dataField: "NumeroDocumento",

					},

				{
					caption: "Fecha Recepción",
					dataField: "DatFechaIngreso",
					dataType: "date",
					format: "yyyy-MM-dd HH:mm:ss",

				},
					{
						caption: "Fecha Documento",
						dataField: "DatFechaDocumento",
						dataType: "date",
						format: "yyyy-MM-dd ",

						validationRules: [{
							type: "required",
							message: "El campo Fecha es obligatorio."
						}]
					},
					{
						caption: "Fecha Vencimiento",
						dataField: "DatFechaVencDocumento",
						visible: false,
						dataType: "date",
						format: "yyyy-MM-dd",

						validationRules: [{
							type: "required",
							message: "El campo Fecha es obligatorio."
						}]
					},
					{
						caption: "IdFacturador",

						dataField: "IdFacturador"
					}, {
						caption: "Facturador",

						dataField: "Facturador"
					},
					//{
					//	caption: "Valor Total",
					//	visible: false,
					//	dataField: "IntVlrTotal"
					//},
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
					 	dataField: "tipodoc",
					 	lookup: {
					 		dataSource: items_Tipo,
					 		displayExpr: "Texto",
					 		valueExpr: "ID"
					 	},
					 },
					 {
					 	caption: "Adquiriente",

					 	dataField: "IdentificacionAdquiriente"
					 },
					  {
					  	caption: "Nombre Adquiriente",

					  	dataField: "NombreAdquiriente"
					  },

					  // {
					  // 	dataField: "EstadoCategoria",
					  // 	caption: "Estado",
					  // 	cssClass: "hidden-xs col-md-1",
					  // 	lookup: {
					  // 		dataSource: CategoriaEstado,
					  // 		displayExpr: "Name",
					  // 		valueExpr: "ID"
					  // 	},
					  // 	cellTemplate: function (container, options) {

					  // 		$("<div>")

					  //  		.append($(ColocarEstado(options.data.Estado, options.data.EstadoCategoria)))
					  //  		.appendTo(container);
					  // 	}
					  // },
					  //{
					  //	caption: "Proceso",
					  //	visible: true,
					  //	dataField: "EstadoFactura",
					  //	lookup: {
					  //		dataSource: ProcesoEstado,
					  //		displayExpr: "Name",
					  //		valueExpr: "ID"
					  //	},

					  //},



					//{
					//	caption: "Estado Email",
					//	visible: false,
					//	dataField: "EstadoEnvioMail",
					//	lookup: {
					//		dataSource: EstadoEnvio,
					//		displayExpr: "Name",
					//		valueExpr: "ID"
					//	},
					//	cellTemplate: function (container, options) {

					//		$("<a>")
					//			.append($(ColocarEstadoEmail(options.data.EnvioMail, options.data.MensajeEnvio, options.data.EstadoEnvioMail, options.data.StrIdSeguridad)))
					//			.appendTo(container);
					//	}
					//}
				],
			filterRow: {
				visible: true
			}

		});



	}





	function consultar2() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();
		$('#waitRegistros').show();
		SrvDocumento.ObtenerDocumentosRechazado(fecha_inicio, fecha_fin).then(function (data) {

			Documentos = [];
			AlmacenDocumentos = new DevExpress.data.ArrayStore({
				key: "StrIdSeguridad",
				data: Documentos
			});

			cargarDocumentos(data);

			$("#gridDocumentos").dxDataGrid({
				dataSource: {
					store: AlmacenDocumentos,
					reshapeOnPush: true
				},
				paging: {
					pageSize: 20,
					enabled: true
				}
			});
			$('#wait').hide();
			$('#waitRegistros').hide();
			//*******		
		}, function errorCallback(response) {
			$('#wait').hide();
			$('#waitRegistros').hide();
			DevExpress.ui.notify(response, 'error', 3000);
		});


	}


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





});


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



