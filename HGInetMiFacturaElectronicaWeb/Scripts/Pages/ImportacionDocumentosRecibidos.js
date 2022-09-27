DevExpress.localization.locale('es-ES');

var App = angular.module('App', ['dx']);

var id_seguridad_empresa = "";

App.controller('ImportarDocumentosRecibidosController', function ImportarDocumentosRecibidosController($scope, $http, $location,  $rootScope) {
	
	var total_seleccionados = 0;


	$("#procesar").dxButton({
		text: "Procesar Documentos",
		visible: false,
		type: "default",
		onClick: function (e) {
			ProcesarDocumentos();
		} 
	});

	$http.get('/api/DatosSesion/').then(function (response) {
		
		id_seguridad_empresa = response.data[0].IdSeguridad;
		$("#Archivo").dxFileUploader({
			multiple: false,
			allowedFileExtensions: [".xlsx", ".xls"],
			uploadMode: "instantly",
			selectButtonText: "Seleccione el Archivo de excel",
			uploadedMessage: "Archivo guardado exitosamente",
			uploadUrl: "/api/ImportarArchivo?StrIdSeguridad=" + id_seguridad_empresa + "&Operacion=" + 1,
			onUploaded: function (e) {
				$('#wait2').show();
				consultarDatos();
				//$('#wait2').hide();
				if (e.request.response == true) {
					//Carga notificación de creación con opción de editar formato.
					var myDialog = DevExpress.ui.dialog.custom({
						title: "Importación terminada con éxito",
						message: response.data.ExceptionMessage,
						buttons: [{
							text: "Aceptar",
							onClick: function (e) {
								myDialog.hide();
							}
						}]
					});
					myDialog.show().done(function (dialogResult) {
					});
				}
				//$('#wait2').hide();
			}
				   ,
			onUploadError: function (e) {
				var datos = JSON.parse(e.request.response);
				//DevExpress.ui.notify(datos.ExceptionMessage, 'error', 6000);
				//Carga notificación de creación con opción de editar formato.
				var myDialog = DevExpress.ui.dialog.custom({
					title: "Proceso de importación falló",
					message: datos.ExceptionMessage,
					buttons: [{
						text: "Aceptar",
						onClick: function (e) {
							myDialog.hide();
						}
					}]
				});
				myDialog.show().done(function (dialogResult) {
				});
			}
		});



	}, function errorCallback(response) {
		$('#wait').hide();
		DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
	});

	
	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultarDatos();
		}
	};

	consultar();

	function consultar() {
		//$('#Total').text("");
		//if (fecha_inicio == "")
		//	fecha_inicio = now.toISOString();

		//if (fecha_fin == "")
		//	fecha_fin = now.toISOString();

		//if (resolucion == "")
		//	resolucion = "*";

		//var Muestradetalle = true;


		$("#gridDocumentos").dxDataGrid({
			keyExpr: "IdSeguridad",
			paging: {
				pageSize: 20,
				enabled: true
			}
			, focusedRowEnabled: true
			, hoverStateEnabled: true
			//, selection: {
			//	mode: "multiple",
			//	width: 10
			//}
			, pager: {
				showPageSizeSelector: true,
				allowedPageSizes: [5, 10, 20],
				showInfo: true
			},
			columnAutoWidth: true,
			scrolling: {
				columnRenderingMode: "virtual",
				//mode: "virtual",
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
			//	storageKey: 'storageObligado',
			//},
			onInitialized(e) {
				e.component.option("stateStoring.ignoreColumnOptionNames", ["filterValues"]);
			}
			//Formatos personalizados a las columnas en este caso para el monto
			, onCellPrepared: function (options) {
				var fieldData = options.value,
					fieldHtml = "";
				try {
					if (options.column.caption == "IVA" || options.column.caption == "ICA" || options.column.caption == "IPC" || options.column.caption == "Total") {
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
			},
			filterRow: {
				visible: true
			},
			"export": {
				enabled: true,
				fileName: "Documentos",
			},

			allowColumnResizing: true
			, allowColumnReordering: true
			, columnChooser: {
				enabled: true,
				mode: "select",
				title: "Selector de Columnas"
			},
			onContentReady: function (e) {
				$("#gridDocumentos").dxDataGrid({
					onToolbarPreparing: function (e) {
						e.toolbarOptions.items.unshift({
							location: "after",
							widget: "dxButton",
							visible: false,
							options: {
								icon: "find", elementAttr: { title: "Consultar" },
								onClick: function () {
									//$http.get('/api/ConsultarEventosRadian?List_IdSeguridad=' + $scope.documentos).then(function (response) {
									//})
									//BotonConsultaEvento(true)
								}
							}
						},
						{
							location: "after",
							widget: "dxButton",
							options: {
								icon: "clear", elementAttr: { title: "Reordenar Columnas" },
								onClick: function () {
									localStorage.removeItem('storageObligado');
									consultar();
									consultarDatos();
								}
							}
						})
					}
				});
			},
			groupPanel: {
				allowColumnDragging: true,
				visible: true
			}
			, columns: [
			{
				caption: "Seleccionar",
				width: "auto",
				cellTemplate: function (container, options) {
					if (options.data.IdProceso == 1) {
						$('<div id="chkImportar_' + options.data.IdSeguridad + '"></div>').dxCheckBox({
							name: "chkImportar_" + options.data.IdSeguridad,
							onValueChanged: function (data) {
								validarSeleccion();
							}
						})
						.appendTo(container);
					}
				},
			},
				{
					caption: "Prefijo",
					//cssClass: "col-xs-3 col-md-1",
					dataField: "StrPrefijo",

				},
				{
					caption: "Documento",
					//cssClass: "col-xs-3 col-md-1",
					dataField: "IntNumero",

				},
				{
					caption: "Tipo Documento",
					//cssClass: "hidden-xs col-md-1",
					dataField: "StrTipodoc"
				},
				{
					caption: "Proceso",
					dataField: "IdProceso",
					lookup: {
						dataSource: ProcesoImportacion,
						displayExpr: "Name",
						valueExpr: "ID"
					},
					cellTemplate: function (container, options) {

						$("<div>")
							.append($(ColocarProcesoImportacion(options.data.IdProceso)))
							.appendTo(container);
					}
				},
				 {
				 	caption: "Nit Facturador",
				 	//cssClass: "hidden-xs col-md-1",
				 	dataField: "StrEmpresaFacturador"
				 },
				 {
				 	caption: "Nombre Facturador",
				 	//cssClass: "hidden-xs col-md-1",
				 	dataField: "StrNombreFacturador"
				 },
				{
					caption: "Resultado",
					dataField: "StrObservaciones"
				},
				{
					caption: "Fecha Recepcion",
					dataField: "DatFechaRecepcion",
					dataType: "date",
					format: "yyyy-MM-dd",
				},
				{
					caption: "Fecha Emision",
					dataField: "DatFechaEmision",
					dataType: "date",
					format: "yyyy-MM-dd",
				},
				   {
				   	caption: "IVA",
				   	//cssClass: "col-xs-2 col-md-1",
				   	dataField: "IntVlrIva",
				   	//visible: false
				   },
				   {
				   	caption: "ICA",
				   	//cssClass: "col-xs-2 col-md-1",
				   	dataField: "IntVlrIca",
				   	//visible: false
				   },
				   {
				   	caption: "IPC",
				   	//cssClass: "col-xs-2 col-md-1",
				   	dataField: "IntVlrIPC",
				   	//visible: false
				   },
				  {
				  	caption: "Total",
				  	//cssClass: "col-xs-2 col-md-1",
				  	dataField: "IntVlrTotal",
				  	//visible: false
				  },
				{
					caption: "CUFE",
					//cssClass: "col-xs-2 col-md-1",
					dataField: "StrCufe"
				},
				   {
				   	dataField: "StrEstadoDian",
				   	caption: "Estado Dian",
				   	//cssClass: "hidden-xs col-md-1",
				   },
				   {
				   	caption: "Nit Adquiriente",
				   	//cssClass: "hidden-xs col-md-1",
				   	dataField: "StrEmpresaAdquiriente"
				   },
				 {
				 	caption: "Nombre Adquiriente",
				 	//cssClass: "hidden-xs col-md-1",
				 	dataField: "StrNombreAdquiriente"
				 },


			],
		});


		function validarSeleccion() {
			var data = $("#gridDocumentos").dxDataGrid("instance").option().dataSource;
			var lista = '';
			total_seleccionados = 0;
			for (var i = 0; i < data.length; i++) {

				valor = false;

				try {
					var valor = $("#chkImportar_" + data[i].IdSeguridad).dxCheckBox("instance").option().value;
				} catch (e) {

				}
				if (valor) {

					try {
						if (lista == "") {
							lista = data[i].IdSeguridad;
						}
						else {
							lista += ',' + data[i].IdSeguridad;
						}
						//lista += (lista) ? ',' + data[i].StrIdSeguridad : data[i].StrIdSeguridad;//'';
						//lista += "{'" + data[i].StrIdSeguridad + "'}";
					} catch (e) {

					}

					total_seleccionados += 1;
				}

			}

			if (total_seleccionados > 0) {
				
				$scope.documentos = lista;
				$("#procesar").dxButton({ visible: true });

			} else {

				
				$scope.documentos = '';
				$("#procesar").dxButton({ visible: false });
				
			}

		}

	}

	function consultarDatos() {
		$('#wait2').show();
		$http.get('/api/ObtenerDocRecibidos?StrIdSeguridad_Empresa=' + id_seguridad_empresa).then(function (response) {
			$('#wait2').hide();

			//if (response.data.length > 0) {
			//	Radian = response.data[0].Radian;
			//}
			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data,
				paging: {
					pageSize: 20,
					enabled: true
				}
			});

		}, function errorCallback(response) {
			$('#wait2').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});

	}

	function ProcesarDocumentos() {
		if (total_seleccionados > 0) {

			var myDialog = DevExpress.ui.dialog.custom({
				title: "¿Desea procesar los documentos?",
				messageHtml: "Señor(a) Usuario, debe tener en cuenta que procesar los documentos de este listado no contaran con los archivos XML y PDF en nuestra Plataforma, no podra validar que contiene el documento para posterior conversión a compra." + "</br></br>Si desea continuar con el proceso presione 'Aceptar'",
				position: { my: "center", at: "center", align: "center" },
				buttons: [{
					text: "Aceptar",
					onClick: function (e) {
						$('#wait2').show();
						$http.post('/api/ProcesarDocs?lista_documentos=' + $scope.documentos + '&Operacion=1').then(function (response) {

							$("#procesar").dxButton({ visible: false });
							$('#wait2').show();
							consultarDatos();
							//$('#wait2').hide();
							//if (response.data.length > 0) {
							//	Radian = response.data[0].Radian;
							//}
							//$("#gridDocumentos").dxDataGrid({
							//	dataSource: response.data,
							//	paging: {
							//		pageSize: 20,
							//		enabled: true
							//	}
							//});

						}, function errorCallback(response) {
							$('#wait2').hide();
							DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
						});
					}
				},
				{
					text: "Cancelar",
					onClick: function (e) {
						$('#wait2').hide();
						myDialog.hide();
					}
				}]
			});
			myDialog.show().done(function (dialogResult) {
			});
			
		}

	}

});
