DevExpress.localization.locale('es-ES');

var email_destino = "";
var id_seguridad = "*";
var UsuarioSession = "";
var Procedencia = [];
var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var AudAdminApp = angular.module('AudAdminApp', ['dx', 'AppMaestrosEnum', 'AppSrvAuditoria', 'ModalEmpresasApp']);
AudAdminApp.controller('AudAdminController', function AudAdminController($scope, $http, $location, SrvMaestrosEnum, SrvAuditoria, $rootScope) {

	var now = new Date();
	var Estado;
	var Proceso;

	var codigo_facturador = "",
           numero_documento = "*",
           estado_dian = "*",
		   proceso_doc = "*",
		   procedencia_proceso = "",
		   tipo_registro = "",
           fecha_inicio = "",
           fecha_fin = "",
           cod_facturador = "*",
            tipo_filtro_fecha = 1;

	SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {
		codigo_facturador = data[0].IdentificacionEmpresa;
		UsuarioSession = data[0].IdSeguridad;
		//consultar();
	});

	SrvMaestrosEnum.ObtenerEnum(5).then(function (data) {
		Estado = data;
		var DatosEstados = function () {
			return new DevExpress.data.CustomStore({
				loadMode: "raw",
				key: "ID",
				load: function () {
					return JSON.parse(JSON.stringify(Estado));
				}
			});
		};
		$("#filtrosEstado").dxDropDownBox({
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
	});

	SrvMaestrosEnum.ObtenerEnum(6).then(function (dataproc) {
		Proceso = dataproc;
		console.log("Enum 6", dataproc);
		var DatosProceso = function () {
			return new DevExpress.data.CustomStore({
				loadMode: "raw",
				key: "ID",
				load: function () {
					return JSON.parse(JSON.stringify(Proceso));
				}
			});
		};
		$("#filtrosProceso").dxDropDownBox({
			valueExpr: "ID",
			placeholder: "Seleccione un Item",
			displayExpr: "Descripcion",
			showClearButton: true,
			dataSource: DatosProceso(),
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
                    		proceso_doc = keys;
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
	});

	SrvMaestrosEnum.ObtenerEnum(7).then(function (datatipo) {
		SrvMaestrosEnum.ObtenerEnum(8).then(function (dataproced) {
			TipoRegistro = datatipo;
			Procedencia = dataproced;
			cargarFiltros();
			consultar();
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
            	tipo_registro: {
            		searchEnabled: true,
            		//Carga la data del control
            		dataSource: new DevExpress.data.ArrayStore({
            			data: TipoRegistro,
            			key: "ID"
            		}),
            		displayExpr: "Descripcion",
            		Enabled: true,
            		placeholder: "Seleccione un Item",
            		onValueChanged: function (data) {
            			tipo_registro = data.value.ID;
            		}
            	},

            	Procedencia: {
            		searchEnabled: true,
            		//Carga la data del control
            		dataSource: new DevExpress.data.ArrayStore({
            			data: Procedencia,
            			key: "ID"
            		}),
            		displayExpr: "Descripcion",
            		Enabled: true,
            		placeholder: "Seleccione un Item",
            		onValueChanged: function (data) {
            			procedencia_proceso = data.value.ID;
            		}
            	},

            	NumeroDocumento: {
            		placeholder: "Ingrese Número Documento",
            		showClearButton: true,
            		onValueChanged: function (data) {
            			numero_documento = data.value;
            		}
            	},

            }

		$("#txtempresaasociada").dxTextBox({
			placeholder: "Ingrese Identificación del Facturador",
			title: 'Prueba',
			onValueChanged: function (data) {
				empresa = data.value.split(' -- ');
				cod_facturador = empresa[0];
				$("#txtempresaasociada").dxTextBox({ value: cod_facturador });
			},
			onKeyDown: function (data) {
				if (data.event.key == 'F4')
					$('#modal_Buscar_empresa').modal('show');
			}
		});

		$("#FechaFinal").dxDateBox({ min: now });
		$("#FechaInicial").dxDateBox({ max: now });
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

		SrvAuditoria.ConsultaAuditoria(fecha_inicio, fecha_fin, cod_facturador, numero_documento, estado_dian, proceso_doc, tipo_registro, procedencia_proceso).then(function (response) {

			console.log(response);
			$("#gridDocumentos").dxDataGrid({
				dataSource: response,
				paging: {
					pageSize: 20
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				}
				, loadPanel: {
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
					caption: "Fecha",
					dataField: "DatFecha",
					dataType: "date",
					format: "yyyy-MM-dd",

					validationRules: [{
						type: "required",
						message: "El campo Fecha es obligatorio."
					}]
				},

				{
					caption: "Documento",
					dataField: "StrNumero",

				},

				{
					caption: "IdFacturador",

					dataField: "StrObligado"
				},
				{
					caption: "Estado",

					dataField: "StrDesEstado"
				},
				{
					caption: "Proceso",

					dataField: "StrDesProceso"
				},
				{
					caption: "Tipo Registro",

					dataField: "StrDescTipoReg"
				},
				{
					caption: "Procedencia",

					dataField: "StrDescProcePor"
				},
				{
					caption: "Mensaje",

					dataField: "StrMensaje"
				},
				{
					caption: "Realizado Por",

					dataField: "StrRealizadoPor"
				}

				],

				//**************************************************************
				masterDetail: {
					enabled: true,
					template: function (container, options) {

						container.append($('<h4 class="form-control">MENSAJE:</h4><pre style="width:100%"> ' + options.data.StrMensaje + '</pre><br/>'));

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

		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});


	}

});
