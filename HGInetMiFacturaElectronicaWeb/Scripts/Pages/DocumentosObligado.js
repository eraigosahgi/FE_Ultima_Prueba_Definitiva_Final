DevExpress.localization.locale('es-ES');

var email_destino = "";
var id_seguridad = "";
var items_recibo = [];
var App = angular.module('App', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento', 'AppSrvFiltro']);
var UsuarioSession = "";
App.controller('DocObligadoController', function DocObligadoController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento, $rootScope, SrvFiltro) {
	var now = new Date();
	var Estado;
	var ResolucionesPrefijo;

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

	SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {
		codigo_facturador = data[0].IdentificacionEmpresa;
		UsuarioSession = data[0].IdSeguridad;
	});

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
            			data: items_recibo,
            			key: "ID"
            		}),
            		displayExpr: "Descripcion",
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
		consultar();
	}

	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultar();
		}
	};


	function consultar() {
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
			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data,
				keyExpr: "StrIdSeguridad",
				paging: {
					pageSize: 20
				}

				/*, stateStoring: {
		        enabled: true,
		        type: "localStorage",
		        storageKey: "storage"
				}*/
                , focusedRowEnabled: true
                , hoverStateEnabled: true
                , selection: {
                	mode: "single"
                }
				, pager: {
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
				//"export": {
				//	enabled: true,
				//	fileName: "Documentos",
				//	allowExportSelectedData: true
				//},
				allowColumnResizing: true
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
                    	width: "10%",
                    	alignment: "left",
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
                    {
                    	caption: "Documento",
                    	cssClass: "col-xs-3 col-md-1",
                    	dataField: "NumeroDocumento",

                    },
                    {
                    	caption: "Fecha Documento",
                    	dataField: "DatFechaDocumento",
                    	dataType: "date",
                    	format: "yyyy-MM-dd",
                    	cssClass: "col-xs-3 col-md-1",
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
                    	cssClass: "hidden-xs col-md-1",
                    	validationRules: [{
                    		type: "required",
                    		message: "El campo Fecha es obligatorio."
                    	}]
                    },
                    {
                    	caption: "Valor Total",
                    	cssClass: "col-xs-2 col-md-1",
                    	dataField: "IntVlrTotal"
                    },
					 {
					 	caption: "SubTotal",
					 	cssClass: "col-xs-2 col-md-1",
					 	dataField: "IntSubTotal"
					 },
					  {
					  	caption: "Neto",
					  	cssClass: "col-xs-2 col-md-1",
					  	dataField: "IntNeto"
					  },
                     {
                     	caption: "Adquiriente",
                     	cssClass: "hidden-xs col-md-1",
                     	dataField: "IdentificacionAdquiriente"
                     },
                     {
                     	caption: "Tipo Documento",
                     	cssClass: "hidden-xs col-md-1",
                     	dataField: "tipodoc"
                     },
                      {
                      	caption: "Nombre Adquiriente",
                      	cssClass: "hidden-xs col-md-1",
                      	dataField: "NombreAdquiriente"
                      },
                       {
                       	dataField: "EstadoFactura",
                       	caption: "Estado",
                       	cssClass: "hidden-xs col-md-1",
                       	lookup: {
                       		dataSource: ProcesoEstado,
                       		displayExpr: "Name",
                       		valueExpr: "ID"
                       	},
                       	cellTemplate: function (container, options) {

                       		$("<div>")
								.append($(ColocarEstado(options.data.Estado, options.data.EstadoFactura)))
								.appendTo(container);
                       	}
                       },
                    
					   {
					   	caption: "Acuse",					   	
					   	dataField: "EstadoAcuse",
					   	cssClass: "hidden-xs col-md-1",
					   	lookup: {
					   		dataSource: AdquirienteRecibo,
					   		displayExpr: "Name",
					   		valueExpr: "ID"
					   	},
					   	cellTemplate: function (container, options) {

					   		$("<div>")
								.append($(ColocarEstadoAcuse(options.data.IntAdquirienteRecibo, options.data.EstadoAcuse)))
								.appendTo(container);
					   	}
					   },
                      {
                      	caption: "Motivo Rechazo",
                      	cssClass: "hidden-xs col-md-1",
                      	dataField: "MotivoRechazo",
                      },
					{
						caption: "Estado Email",
						cssClass: "hidden-xs col-md-1",
						dataField: "EstadoEnvioMail",
						cellTemplate: function (container, options) {

							$("<a>")
		                        .append($(ColocarEstadoEmail(options.data.EnvioMail, options.data.MensajeEnvio, options.data.EstadoEnvioMail, options.data.StrIdSeguridad)))
		                        .appendTo(container);
						}
					}

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

		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});


	}


	ConsultarAuditoria = function (IdSeguridad, IdFacturador, NumeroDocumento) {
		$rootScope.ConsultarAuditDoc(IdSeguridad, IdFacturador, NumeroDocumento);
	};

	SrvFiltro.ObtenerFiltro('Documento Adquiriente', 'Adquiriente', 'icon-user-tie', 115, '/api/ObtenerAdquirientes?Facturador=' + $('#Hdf_Facturador').val(), 'ID', 'Texto', false, 4).then(function (Datos) {
		$scope.Adquiriente = Datos;
	});


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
					var respuesta = responseEnvio.data;

					if (respuesta) {
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
							title: 'Error',
							text: 'Ocurrió un error en el envío del e-mail.',
							type: 'Error',
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




