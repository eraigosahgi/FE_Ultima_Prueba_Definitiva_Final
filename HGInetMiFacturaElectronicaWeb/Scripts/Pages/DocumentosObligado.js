DevExpress.localization.locale('es-ES');

var email_destino = "";
var id_seguridad = "";
var items_recibo = [];
var list_doc_idSeguridad = "";
var array_lis_doc_rad = [];
var App = angular.module('App', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento', 'AppSrvFiltro']);
var UsuarioSession = "";
App.controller('DocObligadoController', function DocObligadoController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento, $rootScope, SrvFiltro) {
	var now = new Date();
	var Estado;
	var ResolucionesPrefijo;
	var Radian = false;

	var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
		   tipo_filtro_fecha = 1,
           codigo_adquiriente = "",
			prefijo = "",
			resolucion = "";




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

	cargarFiltros();

	function cargarFiltros() {
		$("#filtrosEstadoRecibo").dxDropDownBox({
			valueExpr: "ID",
			placeholder: "Seleccione un Item",
			displayExpr: "Descripcion",
			showClearButton: true,
			dataSource: DataCheckBox(Estado),
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
            		onValueChanged: function (data) {
            			numero_documento = data.value;
            		}
            	}
            }

		$("#FechaFinal").dxDateBox({ min: now });
		$("#FechaInicial").dxDateBox({ max: now });

		//Se desactivan las consultas Automaticas
		//consultar();
	}

	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultarDatos();
		}
	};

	consultar();

	function consultar() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		if (resolucion == "")
			resolucion = "*";

		var Muestradetalle = true;


		$("#gridDocumentos").dxDataGrid({
			keyExpr: "StrIdSeguridad",
			paging: {
				pageSize: 20,
				enabled: true
			}
			, focusedRowEnabled: true
			, hoverStateEnabled: true
			, selection: {
				mode: "single"
			}
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
			}
	,
			stateStoring: {
				enabled: false,
				type: 'localStorage',
				storageKey: 'storageObligado',
			},
			onInitialized(e) {
				e.component.option("stateStoring.ignoreColumnOptionNames", ["filterValues"]);
			}
			//Formatos personalizados a las columnas en este caso para el monto
			, onCellPrepared: function (options) {
				var fieldData = options.value,
					fieldHtml = "";
				try {
					if (options.column.caption == "Valor Total" || options.column.caption == "SubTotal" || options.column.caption == "Neto") {
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
									BotonConsultaEvento(true)
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
					caption: "Archivos",
					//width: "10%",
					columnMinWidth: 80,
					alignment: "left",
					columnAutoWidth: false,
					cellTemplate: function (container, options) {

						var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

						var visible_xml = " style='pointer-events:auto;cursor: not-allowed;'";

						var visible_xml_acuse = "style='pointer-events:auto;cursor: not-allowed;'";

						var visible_acuse = "  title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

						var permite_envio = "class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:5%; font-size:19px'";

						if (options.data.Pdf)
							visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer;'";
						else
							options.data.Pdf = "#";

						if (options.data.Xml)
							visible_xml = "href='" + options.data.Xml + "' class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
						else
							options.data.Xml = "#";

						if (options.data.EstadoAcuse != 0)
							visible_acuse = "href='" + options.data.RutaAcuse + "' title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
						else
							options.data.RutaAcuse = "#";

						if (options.data.XmlAcuse)
							visible_xml_acuse = "href='" + options.data.XmlAcuse + "' title='ver XML Respuesta acuse' style='pointer-events:auto;cursor: pointer'";
						else
							options.data.XmlAcuse = "#";

						$("<div>")
							.append(
							   $("<a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "><a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + ">"),
								$("<a " + permite_envio + "></a>").dxButton({
									onClick: function () {
										$scope.showModal = true;
										email_destino = options.data.MailAdquiriente;
										id_seguridad = options.data.StrIdSeguridad;
										$('input:text[name=EmailDestino]').val("");
										if (permite_envio != "") {
											SrvDocumento.ConsultarEmailUbl(options.data.StrIdSeguridad).then(function (data) {
												$('input:text[name=EmailDestino]').val(data);
											});
										}
									}
								}).removeClass("dx-button dx-button-normal dx-widget")
						)
							.appendTo(container);
					}
				},
				//{
				//	caption: "Consulta Radian",
				//	//cssClass: "col-xs-3 col-md-1",
				//	alignment: "center",
				//	//width: "10%",
				//	//dataField: "NumeroDocumento",
				//	visible: Radian,
				//	//disable: ((response.data.IntAdquirienteRecibo > 5) && (response.data.EstadoFactura > 7)) ? true : false,
				//	cellTemplate: function (container, options) {
				//		if (options.data.tipodoc != 'Nota Crédito') {
				//			$('<div id="chkdoc_evento_' + options.data.StrIdSeguridad + '"></div>').dxCheckBox({
				//				name: "chkdoc_evento_" + options.data.StrIdSeguridad,
				//				disabled: ((options.data.IntAdquirienteRecibo > 5) || (options.data.EstadoFactura < 8)) ? true : (Radian == false) ? true : false,
				//				onValueChanged: function (data) {
				//					validarSeleccion();
				//				}
				//			})
				//			.appendTo(container);
				//		}
				//	}

				//},
				{
					caption: "Documento",
					//cssClass: "col-xs-3 col-md-1",
					dataField: "NumeroDocumento",

				},
				{
					caption: "Fecha Documento",
					dataField: "DatFechaDocumento",
					dataType: "date",
					format: "yyyy-MM-dd",
					//cssClass: "col-xs-3 col-md-1",
					validationRules: [{
						type: "required",
						message: "El campo Fecha es obligatorio."
					}]
				},
				{
					caption: "Fecha Vencimiento",
					dataField: "DatFechaVencDocumento",
					dataType: "date",
					format: "yyyy-MM-dd",
					//cssClass: "hidden-xs col-md-1",
					validationRules: [{
						type: "required",
						message: "El campo Fecha es obligatorio."
					}]
				},
				{
					caption: "Valor Total",
					//cssClass: "col-xs-2 col-md-1",
					dataField: "IntVlrTotal"
				},
				 {
				 	caption: "SubTotal",
				 	//cssClass: "col-xs-2 col-md-1",
				 	dataField: "IntSubTotal",
				 	visible: false
				 },
				  {
				  	caption: "Neto",
				  	//cssClass: "col-xs-2 col-md-1",
				  	dataField: "IntNeto",
				  	visible: false
				  },
				 {
				 	caption: "Adquiriente",
				 	//cssClass: "hidden-xs col-md-1",
				 	dataField: "IdentificacionAdquiriente"
				 },
				 {
				 	caption: "Nombre Adquiriente",
				 	//cssClass: "hidden-xs col-md-1",
				 	dataField: "NombreAdquiriente"
				 },
				 {
				 	caption: "Tipo Documento",
				 	//cssClass: "hidden-xs col-md-1",
				 	dataField: "tipodoc"
				 },				
				   {
				   	dataField: "EstadoFactura",
				   	caption: "Estado",				  
				   	lookup: {
				   		dataSource: CategoriaEstado,
				   		displayExpr: "Name",
				   		valueExpr: "ID"
				   	},				   
				   	cellTemplate: function (container, options) {				   		
				   		$("<div>")
				   			.append($(ColocarEstado(options.data.Estado, options.data.EstadoFactura)))
				   			.appendTo(container);
				   	}
					,
				   	calculateCellValue: function (e, a) {
				   		return e.Estado;
				   	}
				   },
				   {
				   	caption: "Radian",
				   	dataField: "TituloValor",
				   	//cssClass: "hidden-xs col-md-1",
				   	visible: Radian,
				   	//lookup: {
				   	//	dataSource: AdquirienteRecibo,
				   	//	displayExpr: "Name",
				   	//	valueExpr: "ID"
				   	//},
				   	cellTemplate: function (container, options) {
				   		//(d.IntAdquirienteRecibo > 5) ? "Titulo Valor" : (d.IntAdquirienteRecibo == 5) || (d.IntAdquirienteRecibo == 3) ? "Aceptado" : "Documento Electrónico"
				   		var estado = (options.data.IntAdquirienteRecibo > 5) ? 0 : (options.data.IntAdquirienteRecibo == 5) || (options.data.IntAdquirienteRecibo == 3) ? 1 : 2;
				   		$("<a>")
							.append($(ColocarColorRadian(estado, options.data.TituloValor, options.data.StrIdSeguridad, options.data.NumeroDocumento, codigo_facturador)))
							.appendTo(container);
				   	},

				   },

				   {
				   	caption: "Acuse",
				   	dataField: "IntAdquirienteRecibo",
				   	//cssClass: "hidden-xs col-md-1",
				   	lookup: {
				   		dataSource: AdquirienteRecibo,
				   		displayExpr: "Name",
				   		valueExpr: "ID"
				   	},
				   	cellTemplate: function (container, options) {

				   		if (options.data.IntAdquirienteRecibo != 4)
				   		{
				   			$("<div>")
						   .append($(ColocarEstadoAcuseObligado(options.data.IntAdquirienteRecibo, options.data.IntAdquirienteRecibo)))
						   .appendTo(container);
				   		}
				   		else
				   		{
				   			$("<a>")
								.append($(ColocarColorRadian(options.data.IntAdquirienteRecibo, options.data.TituloValor, options.data.StrIdSeguridad, options.data.NumeroDocumento, codigo_facturador)))
								.appendTo(container);
				   		}
				   		
				   	}
				   },
				  {
				  	caption: "Motivo Rechazo",
				  	//cssClass: "hidden-xs col-md-1",
				  	dataField: "MotivoRechazo",
				  },
				{
					caption: "Estado Email",
					//cssClass: "hidden-xs col-md-1",
					dataField: "EstadoEnvioMail",
					lookup: {
						dataSource: EstadoEnvio,
						displayExpr: "Name",
						valueExpr: "ID"
					},
					cellTemplate: function (container, options) {

						$("<a>")
							.append($(ColocarEstadoEmail(options.data.EnvioMail, options.data.MensajeEnvio, options.data.EstadoEnvioMail, options.data.StrIdSeguridad)))
							.appendTo(container);
					}
				},
				 {
				 	caption: "Forma Pago",
				 	visible: false,
				 	dataField: "FormaPago",
				 },

			],

			//**************************************************************
			masterDetail: {
				enabled: true,
				template: function (container, options) {



					container.append(ObtenerDetallle(options.data.Pdf, options.data.Xml, options.data.EstadoAcuse, options.data.RutaAcuse, options.data.XmlAcuse, options.data.zip, options.data.RutaServDian, options.data.StrIdSeguridad, options.data.StrEmpresaFacturador, options.data.NumeroDocumento));

				}
			},
			//****************************************************************
			summary: {
				groupItems: [{
					column: "IntVlrTotal",
					summaryType: "sum",
					displayFormat: " {0} Total ",
					valueFormat: "currency"
				}, {
					column: "IntSubTotal",
					summaryType: "sum",
					displayFormat: " {0} Neto ",
					valueFormat: "currency"
				}, {
					column: "IntNeto",
					summaryType: "sum",
					displayFormat: " {0} Neto ",
					valueFormat: "currency"
				}]
				, totalItems: [{
					name: "Suma",
					displayFormat: "{0}",
					valueFormat: "currency",
					summaryType: "custom"

				},
				{
					column: "IntVlrTotal",
					showInColumn: "IntVlrTotal",
					summaryType: "sum",
					customizeText: function (data) {
						return fNumber.go(data.value).replace("$-", "-$");
					}
				},
				{
					name: "SumaSubTotal",
					summaryType: "sum",
					column: "IntSubTotal",
					customizeText: function (data) {
						return fNumber.go(data.value).replace("$-", "-$");
					}

				},
				{
					name: "SumaNeto",
					displayFormat: "{0}",
					valueFormat: "currency",
					summaryType: "custom"

				},
				{
					column: "IntNeto",
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
							options.totalValue = options.totalValue + options.value.IntVlrTotal;
							$('#Total').text("Total: " + fNumber.go(options.totalValue).replace("$-", "-$"));
						}
					}

					if (options.name === "SumaSubTotal") {
						if (options.summaryProcess === "start") {
							options.totalValue = 0;
							$('#SubTotal').text("");
						}
						if (options.summaryProcess === "calculate") {
							options.totalValue = options.totalValue + options.value.IntSubTotal;
							$('#SubTotal').text("SubTotal: " + fNumber.go(options.totalValue).replace("$-", "-$"));
						}
					}


					if (options.name === "SumaNeto") {
						if (options.summaryProcess === "start") {
							options.totalValue = 0;
							$('#Neto').text("");
						}
						if (options.summaryProcess === "calculate") {
							options.totalValue = options.totalValue + options.value.IntNeto;
							$('#Neto').text("Neto: " + fNumber.go(options.totalValue).replace("$-", "-$"));
						}
					}
				}
			}
			,
			filterRow: {
				visible: true
			}

		});






		function validarSeleccion() {
			var data = $("#gridDocumentos").dxDataGrid("instance").option().dataSource;
			var lista = '';
			var total_seleccionados = 0;
			for (var i = 0; i < data.length; i++) {

				valor = false;

				try {
					var valor = $("#chkdoc_evento_" + data[i].StrIdSeguridad).dxCheckBox("instance").option().value;
				} catch (e) {

				}
				if (valor) {

					try {
						if (lista == "") {
							lista = data[i].StrIdSeguridad;
						}
						else {
							lista += ',' + data[i].StrIdSeguridad;
						}
						//lista += (lista) ? ',' + data[i].StrIdSeguridad : data[i].StrIdSeguridad;//'';
						//lista += "{'" + data[i].StrIdSeguridad + "'}";
					} catch (e) {

					}

					total_seleccionados += 1;
				}

			}

			if (total_seleccionados > 0) {
				//lista = "[" + lista + "]"
				$scope.documentos = lista;

				BotonConsultaEvento(true);
				//$("#consultaeventos").dxButton({ visible: true });
				//$("#gridDocumentos").dxDataGrid({
				//	onToolbarPreparing: function (e) {
				//		e.toolbarOptions.items.unshift({
				//			location: "after",
				//			widget: "dxButton",
				//			visible: true,
				//			options: {
				//				icon: "find", elementAttr: { title: "Consultar" },
				//				onClick: function () {
				//					//ObtenerProductos();
				//				}
				//			}
				//		})
				//	}
				//});


			} else {

				//$("#consultaeventos").dxButton({ visible: false });
				BotonConsultaEvento(false);
				$scope.documentos = '';
				//$("#gridDocumentos").dxDataGrid({
				//	onToolbarPreparing: function (e) {
				//		e.toolbarOptions.items.unshift({
				//			location: "after",
				//			widget: "dxButton",
				//			visible: false,
				//			options: {
				//				icon: "find", elementAttr: { title: "Consultar" },
				//				onClick: function () {
				//					//ObtenerProductos();
				//				}
				//			}
				//		})
				//	}
				//});
			}

		}

		function BotonConsultaEvento(Visible) {

			$("#gridDocumentos").dxDataGrid({
				onToolbarPreparing: function (e) {
					e.toolbarOptions.items.unshift({
						location: "after",
						widget: "dxButton",
						visible: Visible,
						options: {
							icon: "find", elementAttr: { title: "Consultar" },
							onClick: function () {
								$http.get('/api/ConsultarEventosRadian?List_IdSeguridad=' + $scope.documentos).then(function (response) {
								})
							}
						}
					})
				}
			});

		}




		SrvFiltro.ObtenerFiltro('Documento Adquiriente', 'Adquiriente', 'icon-user-tie', 115, '/api/ObtenerAdquirientes?Facturador=' + $('#Hdf_Facturador').val(), 'ID', 'Texto', false, 4).then(function (Datos) {
			$scope.Adquiriente = Datos;
		});

		SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {
			codigo_facturador = data[0].IdentificacionEmpresa;
			UsuarioSession = data[0].IdSeguridad;


			SrvMaestrosEnum.ObtenerEnum(5, '').then(function (data) {
				SrvMaestrosEnum.ObtenerEnum(1).then(function (dataacuse) {
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
		ConsultarAuditoria = function (IdSeguridad, IdFacturador, NumeroDocumento) {
			$rootScope.ConsultarAuditDoc(IdSeguridad, IdFacturador, NumeroDocumento);
		};


		ConsultarEventosRadian = function (IdSeguridad, IdFacturador, NumeroDocumento) {
			$rootScope.ConsultarEventosRadian(IdSeguridad, IdFacturador, NumeroDocumento);
		}


	}

	function consultarDatos() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		if (resolucion == "")
			resolucion = "*";

		var Muestradetalle = true;

		$('#wait').show();
		var codigo_adquiriente = (txt_hgi_Adquiriente == undefined || txt_hgi_Adquiriente == '') ? '' : txt_hgi_Adquiriente;
		$http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&estado_dian=' + estado_dian + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&resolucion=' + resolucion + '&tipo_filtro_fecha=' + tipo_filtro_fecha).then(function (response) {
			$('#wait').hide();
			Radian = response.data[0].Radian;
			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data,
				paging: {
					pageSize: 20,
					enabled: true
				}
			});

		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});

	}

});


App.controller('EnvioEmailController', function EnvioEmailController($scope, $http, $location) {

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

		onClick: function (e) {

			try {

				email_destino = $('input:text[name=EmailDestino]').val();

				if (email_destino == "") {
					throw new DOMException("El e-mail de destino es obligatorio.");
				}
				try {
					//Google Analytics							
					ga('send', 'event', 'Reenvio Email', 'Email : ' + email_destino + ' Id:' + id_seguridad, sessionStorage.getItem("Usuario"));
				} catch (e) { }

				$('#wait').show();
				$http.get('/api/Documentos?id_seguridad=' + id_seguridad + '&email=' + email_destino + '&Usuario=' + UsuarioSession).then(function (responseEnvio) {
					$('#wait').hide();

					var mensaje = 'Ocurrió un error en el envío del e-mail.';
					var envio_exitoso = true;

					if (responseEnvio.data[0].Error != null)
					{
						mensaje = responseEnvio.data[0].Error.ErrorMessage;
						envio_exitoso = false;
					}

					//var respuesta = responseEnvio.data;

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


});




var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Fecha Recepción' },
    { ID: "2", Texto: 'Fecha Documento' }
    ];




