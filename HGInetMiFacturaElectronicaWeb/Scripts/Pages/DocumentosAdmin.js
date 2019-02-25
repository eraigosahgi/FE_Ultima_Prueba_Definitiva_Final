DevExpress.localization.locale('es-ES');
var email_destino = "";
var id_seguridad = "*";
var items_recibo = [];
var UsuarioSession = "";
var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var DocObligadoApp = angular.module('DocObligadoApp', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento', 'ModalEmpresasApp', 'AppSrvFiltro']);
DocObligadoApp.controller('DocObligadoController', function DocObligadoController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento, $rootScope, SrvFiltro) {

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

	

	SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {
		codigo_facturador = data[0].IdentificacionEmpresa;
		UsuarioSession = data[0].IdSeguridad;
		consultar();
	});

	SrvMaestrosEnum.ObtenerEnum(5).then(function (data) {
		SrvMaestrosEnum.ObtenerEnum(1).then(function (dataacuse) {
			Estado = data;
			items_recibo = dataacuse;
			cargarFiltros();
		});
	});


	SrvFiltro.ObtenerFiltro('Documento Facturador', 'Facturador', 'icon-user-tie', 115, '/api/Empresas?Facturador=true', 'Identificacion', 'RazonSocial', false,7).then(function (Datos) {
		$scope.Facturador = Datos;
	});
	
	SrvFiltro.ObtenerFiltro('Documento Adquiriente', 'Adquiriente', 'icon-user-tie', 115, '/api/ObtenerAdquirientes?Facturador=' + $('#Hdf_Facturador').val(), 'ID', 'Texto', false,10).then(function (Datos) {
		$scope.Adquiriente = Datos;
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


		var DatosEstados = function () {
			return new DevExpress.data.CustomStore({
				loadMode: "raw",
				key: "ID",
				load: function () {
					return JSON.parse(JSON.stringify(Estado));
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
            		showClearButton: true,
            		onValueChanged: function (data) {
            			numero_documento = data.value;
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
			consultar2();
		}
	};


	function consultar2() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();
		var documentoFacturador = (txt_hgi_Facturador == undefined || txt_hgi_Facturador == '') ? '' : txt_hgi_Facturador;
		var codigo_adquiriente = (txt_hgi_Adquiriente == undefined || txt_hgi_Adquiriente == '') ? '' : txt_hgi_Adquiriente;
		SrvDocumento.ObtenerDocumentosAdmin(documentoFacturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, Datos_Tipo, tipo_filtro_fecha).then(function (data) {
			$("#gridDocumentos").dxDataGrid({
				dataSource: data
			});
		});
	}





	function consultar() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();


		var documentoFacturador = (txt_hgi_Facturador == undefined || txt_hgi_Facturador == '') ? '' : txt_hgi_Facturador;
		var codigo_adquiriente = (txt_hgi_Adquiriente == undefined || txt_hgi_Adquiriente == '') ? '' : txt_hgi_Adquiriente;
		SrvDocumento.ObtenerDocumentosAdmin(documentoFacturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, Datos_Tipo, tipo_filtro_fecha).then(function (data) {
			$("#gridDocumentos").dxDataGrid({
				dataSource: data,
				keyExpr: "NumeroDocumento",
				paging: {
					pageSize: 20
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
						cellTemplate: function (container, options) {

							var permite_envio = "class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:5%; font-size:19px'";

							var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

							var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

							var visible_acuse = " title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";
							console.log(options.data.EstadoAcuse);

							if (options.data.Pdf)
								visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer;'";
							else
								options.data.Pdf = "#";

							if (options.data.Xml)
								visible_xml = "href='" + options.data.Xml + "'  class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
							else
								options.data.Xml = "#";

							if (options.data.EstadoAcuse == 'Aprobado' || options.data.EstadoAcuse == 'Rechazado' || options.data.EstadoAcuse == 'Aprobado Tácito')
								visible_acuse = "href='" + options.data.RutaAcuse + "' title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
							else
								visible_acuse = "#";

							$("<div>")
								.append(
									$("<a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "><a style='margin-left:5%;' target='_blank'  " + visible_xml + ">"),
									$("<a " + permite_envio + "></a>").dxButton({
										onClick: function () {
											$scope.showModal = true;
											email_destino = options.data.MailAdquiriente;
											id_seguridad = options.data.StrIdSeguridad;
											$('input:text[name=EmailDestino]').val("");
											SrvDocumento.ConsultarEmailUbl(options.data.StrIdSeguridad).then(function (data) {
												$('input:text[name=EmailDestino]').val(data);
											});
										}
									}).removeClass("dx-button dx-button-normal dx-widget")

							)
								.appendTo(container);

						}
					},
					{
						caption: "Documento",

						dataField: "NumeroDocumento",

					},
					{
						caption: "Fecha Documento",
						dataField: "DatFechaDocumento",
						dataType: "date",
						format: "yyyy-MM-dd",

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
					{
						caption: "Valor Total",
						visible: false,
						dataField: "IntVlrTotal"
					},
					{
						caption: "SubTotal",
						visible: false,
						dataField: "IntSubTotal"
					},
					{
						caption: "Neto",
						visible: false,
						dataField: "IntNeto"
					},
					 {
					 	caption: "Adquiriente",

					 	dataField: "IdentificacionAdquiriente"
					 },
					 {
					 	caption: "Tipo Documento",

					 	dataField: "tipodoc"
					 },
					  {
					  	caption: "Nombre Adquiriente",

					  	dataField: "NombreAdquiriente"
					  },

					   {
					   	dataField: "EstadoCategoria",
					   	caption: "Estado",
					   	cssClass: "hidden-xs col-md-1",
					   	cellTemplate: function (container, options) {

					   		$("<div>")

								.append($(ColocarEstado(options.data.Estado, options.data.EstadoCategoria)))
								.appendTo(container);
					   	}
					   },
					  {
					  	caption: "Proceso",
					  	visible: false,
					  	dataField: "EstadoFactura",
					  },

					  {
					  	caption: "Estado Acuse",
					  	visible: false,
					  	dataField: "EstadoAcuse",
					  },
					  {
					  	caption: "Motivo Rechazo",
					  	visible: false,
					  	dataField: "MotivoRechazo",
					  }
				],

				//**************************************************************
				masterDetail: {
					enabled: true,
					template: function (container, options) {

						var visible_zip = "";

						var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

						var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

						var visible_xml_acuse = "style='pointer-events:auto;cursor: not-allowed;'";

						var visible_acuse = "   title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

						var visible_Servicio_DIAN = "style='pointer-events:auto;cursor: not-allowed;'";

						if (options.data.Pdf)
							visible_pdf = "href='" + options.data.Pdf + "' title='ver PDF' style='pointer-events:auto;cursor: pointer;'";
						else
							visible_pdf = "#";

						if (options.data.Xml)
							visible_xml = "href='" + options.data.Xml + "' class='icon-file-xml' title='ver XML' style='pointer-events:auto;cursor: pointer;'";
						else
							visible_xml = "#";

						if (options.data.EstadoAcuse == 'Aprobado' || options.data.EstadoAcuse == 'Rechazado' || options.data.EstadoAcuse == 'Aprobado Tácito')
							visible_acuse = "href='" + options.data.RutaAcuse + "' class='icon-file-eye2'  title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
						else
							visible_acuse = "#";

						if (options.data.XmlAcuse != null)
							visible_xml_acuse = "href='" + options.data.XmlAcuse + "' class='icon-file-xml' title='ver XML Respuesta acuse' style='pointer-events:auto;cursor: pointer'";
						else
							visible_xml_acuse = "#";


						if (options.data.zip)
							visible_zip = "href='" + options.data.zip + "' class='icon-file-zip' title='ver anexo' style='pointer-events:auto;cursor: pointer'";
						else
							visible_zip = "#";

						if (options.data.RutaServDian)
							visible_Servicio_DIAN = "class='icon-file-xml' href='" + options.data.RutaServDian + "' title='ver XML' style='pointer-events:auto;cursor: pointer;'";
						else
							visible_Servicio_DIAN = "#";

						container.append($("<td aria-selected='false' role='gridcell' aria-colindex='1' class='dx-cell-focus-disabled dx-master-detail-cell' colspan='6' style='text-align: center;'><div class='master-detail-caption'>Lista de Archivos:</div><div class='dx-widget dx-visibility-change-handler' role='presentation'><div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>PDF Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='2' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='3' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Ver Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Anexo</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Respuesta DIAN</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Auditoría</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='2' style='text-align: center;'><div> <a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='3' style='text-align: center;'><div> <a target='_blank'  " + visible_xml_acuse + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank'   " + visible_acuse + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_zip + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='5' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_Servicio_DIAN + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'><div><a style='margin-left:5%;' class='icon-file-eye' onClick=ConsultarAuditoria('" + options.data.StrIdSeguridad + "','" + options.data.IdFacturador + "','" + options.data.NumeroDocumento + "') target='_blank' data-toggle='modal' data-target='#modal_audit_documento' title='ver Auditoría'></a></div></td></tr><tr class='dx-row dx-column-lines dx-freespace-row' role='row' style='height: 0px; display: none;'><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td></tr></tbody></table></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-horizontal dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='width: 831px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-vertical dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='height: 35px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div></div></div><span class='dx-datagrid-nodata dx-hidden'></span></div><div class='dx-hidden' style='padding-right: 0px;'></div><div></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-drag-header dx-datagrid-text-content dx-widget' style='display: none;'></div><div class='dx-context-menu dx-has-context-menu dx-widget dx-visibility-change-handler dx-collection dx-datagrid'></div><div class='dx-header-filter-menu'></div><div></div></div></div></td>"
                        ));
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
                    	showInColumn: "IntVlrTotal",
                    	displayFormat: "{0}",
                    	valueFormat: "currency",
                    	summaryType: "custom"

                    },
					{
						name: "SumaSubTotal",
						showInColumn: "IntSubTotal",
						displayFormat: "{0}",
						valueFormat: "currency",
						summaryType: "custom"

					},
					{
						name: "SumaNeto",
						showInColumn: "IntNeto",
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


	ConsultarAuditoria = function (IdSeguridad, IdFacturador, NumeroDocumento) {
		$rootScope.ConsultarAuditDoc(IdSeguridad, IdFacturador, NumeroDocumento);
	};

});


DocObligadoApp.controller('EnvioEmailController', function EnvioEmailController($scope, $http, $location) {

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


//Controlador para gestionar la consulta de Auditoría de Documento 
DocObligadoApp.controller('ModalAuditDocumentoController', function ModalAuditDocumentoController($http, $scope, $location, $rootScope) {

	$rootScope.ConsultarAuditDoc = function (IdSeguridad, IdFacturador, NumeroDocumento) {

		$http.get('/api/AuditoriaDocumento?id_seguridad_doc=' + IdSeguridad).then(function (response) {

			$scope.IdSeguridad = IdSeguridad;
			$scope.NumeroDocumento = NumeroDocumento;
			$scope.Obligado = IdFacturador;

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

});

var items_Tipo =
    [
        { ID: "0", Texto: 'Todos' },
        { ID: "1", Texto: 'Factura' },
        { ID: "2", Texto: 'Nota Debito' },
        { ID: "3", Texto: 'Nota Crédito' }
    ];

var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Fecha Recepción' },
    { ID: "2", Texto: 'Fecha Documento' }
    ];


