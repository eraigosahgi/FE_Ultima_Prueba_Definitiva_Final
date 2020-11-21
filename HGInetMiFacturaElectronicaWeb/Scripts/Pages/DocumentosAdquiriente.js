DevExpress.localization.locale('es-ES');

//var path = window.location.pathname;
//var ruta = window.location.href;
//ruta = ruta.replace(path, "/");
//document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/MaestrosEnum.js"></script>');

var Estado;
var UsuarioSession = "";
var App = angular.module('App', ['dx', 'AppMaestrosEnum']);
//Controlador para la consulta de documentos Adquiriente
App.controller('DocAdquirienteController', function ($scope, $rootScope, $http, $location, SrvMaestrosEnum) {

	var now = new Date();

	var codigo_adquiente = "",
           numero_documento = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           tipo_filtro_fecha = 1;

	SrvMaestrosEnum.ObtenerEnum(5).then(function (data) {
		Estado = data;
		cargarFiltros();
	});


	SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {
		codigo_adquiente = data[0].IdentificacionEmpresa;
		UsuarioSession = data[0].IdSeguridad;
		//consultar();
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
                , NumeroDocumento: {
                	placeholder: "Ingrese Número Documento",
                	onValueChanged: function (data) {
                		numero_documento = data.value;
                	}
                }

            }
	}
	var mensaje_acuse = "";
	$("#FechaFinal").dxDateBox({ min: now });
	$("#FechaInicial").dxDateBox({ max: now });

	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultar();
		}
	};


	function consultar2() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		$('#wait').show();
		$http.get('/api/Documentos?codigo_adquiente=' + codigo_adquiente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_filtro_fecha=' + tipo_filtro_fecha).then(function (response) {
			$('#wait').hide();
			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data
			});
		});
	}



	//Consultar DOcumentos
	function consultar() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		//Obtiene los datos del web api
		//ControladorApi: /Api/Documentos/
		//Datos GET: codigo_adquiente - numero_documento - estado_recibo - fecha_inicio - fecha_fin
		$('#wait').show();
		$http.get('/api/Documentos?codigo_adquiente=' + codigo_adquiente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_filtro_fecha=' + tipo_filtro_fecha).then(function (response) {
			$('#wait').hide();
			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data,
				paging: {
					pageSize: 20
				},
				keyExpr: "StrIdSeguridad",
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
                				var inicial = fNumber.go(fieldData).replace("$-", "-$");
                				options.cellElement.html(inicial);
                			}
                		}
                	} catch (err) {
                		DevExpress.ui.notify(err.message, 'error', 3000);
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
                	mode: "select"
                },
				groupPanel: {
					allowColumnDragging: true,
					visible: true
				}
                , columns: [
                    {
                    	caption: "  Lista de Archivos",
                    	cssClass: "col-md-1 col-xs-2",
                    	width: "auto",
                    	cellTemplate: function (container, options) {
                    		var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

                    		var visible_xml = " style='pointer-events:auto;cursor: not-allowed;'";

                    		var visible_xml_acuse = "style='pointer-events:auto;cursor: not-allowed;'";

                    		var visible_acuse = "  title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

                    		var permite_envio = "class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:5%; font-size:19px'";

                    		if (options.data.Pdf)
                    			visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer;'";
                    		else
                    			visible_pdf = "#";

                    		if (options.data.Xml)
                    			visible_xml = "href='" + options.data.Xml + "' class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
                    		else
                    			visible_xml = "#";

                    		if (options.data.EstadoAcuse == 'Aprobado' || options.data.EstadoAcuse == 'Rechazado' || options.data.EstadoAcuse == 'Aprobado Tácito')
                    			visible_acuse = "href='" + options.data.RutaAcuse + "' title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
                    		else
                    			visible_acuse = "#";

                    		if (options.data.XmlAcuse != null)
                    			visible_xml_acuse = "href='" + options.data.XmlAcuse + "' title='ver XML Respuesta acuse' style='pointer-events:auto;cursor: pointer'";
                    		else
                    			visible_xml_acuse = "#";



                    		$("<div>")
                                .append(
                                   $("<a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "><a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + ">"))
                                .append($(""))
                                .appendTo(container);
                    	}
                    },
                    {
                    	caption: "Documento",
                    	dataField: "NumeroDocumento",
                    	cssClass: "col-md-1 col-xs-3",
                    },
                    {
                    	caption: "Fecha Documento",
                    	dataField: "DatFechaDocumento",
                    	dataType: "date",
                    	format: "yyyy-MM-dd ",
                    	cssClass: "col-md-1 col-xs-3",
                    	validationRules: [{
                    		type: "required",
                    		message: "El campo Fecha es obligatorio."
                    	}]
                    },
                    {
                    	caption: "Fecha Vencimiento",
                    	dataField: "DatFechaVencDocumento",
                    	dataType: "date",
                    	format: "yyyy-MM-dd ",
                    	cssClass: "hidden-xs col-md-1",
                    	validationRules: [{
                    		type: "required",
                    		message: "El campo Fecha es obligatorio."
                    	}]
                    },
                    {
                    	caption: "Valor Total",
                    	dataField: "IntVlrTotal",
                    	cssClass: "col-md-1 col-xs-1",
                    	width: '12%',
                    	Type: Number,
                    }
                    ,
					{
						caption: "SubTotal",
						dataField: "IntSubTotal",
						cssClass: "col-md-1 col-xs-1",
						width: '12%',
						Type: Number,
					}
                    ,
					{
						caption: "Neto",
						dataField: "IntNeto",
						cssClass: "col-md-1 col-xs-1",
						width: '12%',
						Type: Number,
					}
                    ,
                     {
                     	caption: "Identificación Facturador",
                     	cssClass: "hidden-xs col-md-1",
                     	dataField: "IdFacturador"

                     },
                      {
                      	caption: "Nombre Facturador",
                      	cssClass: "hidden-xs col-md-1",
                      	dataField: "Facturador"
                      },

                        {
                        	dataField: "EstadoFactura",
                        	caption: "Estado",
                        	cssClass: "hidden-xs col-md-1",
                        	cellTemplate: function (container, options) {

                        		$("<div>")
                                    .append($(ColocarEstado(options.data.Estado, options.data.EstadoFactura)))
                                    .appendTo(container);
                        	}
                        },

                       {
                       	caption: "Tipo Documento",
                       	cssClass: "hidden-xs col-md-1",
                       	dataField: "tipodoc"
                       },
                      {
                      	caption: "Estado Acuse",
                      	cssClass: "hidden-xs col-md-1",
                      	dataField: "EstadoAcuse",
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
                      	dataField: "",
                      	caption: "Acuse",
                      	cssClass: "col-md-1 col-xs-2",
                      	cellTemplate: function (container, options) {

                      		var Mostrar_Acuse;
                      		if (options.data.Estado != 400)
                      			Mostrar_Acuse = "<a target='_blank'  href='" + options.data.RutaAcuse + "'>Acuse</a>";
                      		else
                      			Mostrar_Acuse = "";

                      		$("<div>")
								.append($(Mostrar_Acuse))
								.appendTo(container);
                      	}
                      },
                {
                	///Opción de pago
                	cssClass: "col-md-1 ",
                	caption: "Pago",
                	width: "120px",
                	alignment: "center",

                	cellTemplate: function (container, options) {
                		if (options.data.Estado != 400) {
                			var RazonSocial = options.data.Facturador.replace(" ", "_%%_");
                			var click = " onClick=ConsultarPago1('" + options.data.StrIdSeguridad + "','" + options.data.IntVlrTotal + "','" + options.data.PagosParciales + "'," + options.data.poseeIdComercioPSE + "," + options.data.poseeIdComercioTC + ")";


                			var boton_pagar = '<div  ' + click + ' target="_blank" data-toggle="modal" data-target="#modal_Pagos_Electronicos" class="dx-button dx-button-success dx-button-mode-contained dx-widget dx-button-has-icon dx-button-has-text" role="button" aria-label="Pagar" tabindex="10012"><div class="dx-button-content"><i class="dx-icon dx-icon-money"></i><span class="dx-button-text">Pagar</span></div></div>'


                			var imagen = "";
                			if (options.data.tipodoc != 'Nota Crédito' && options.data.poseeIdComercio == 1) {
                				imagen = boton_pagar;//  "<a " + click + " style='font-size: 20px !important; color: #4caf50;' class='icon dx-icon-money' title='Pagar' target='_blank' data-toggle='modal' data-target='#modal_Pagos_Electronicos' ></a><a " + click + " style='font-size: 16px !important; color: black;' title='Pagar' target='_blank' data-toggle='modal' data-target='#modal_Pagos_Electronicos' >Pagar</a>";
                			} else {
                				imagen = "";

                			}

                			if (options.data.tipodoc != 'Nota Crédito' && options.data.poseeIdComercio == 1 && options.data.FacturaCancelada == 100) {//aqui se debe colocar el status que indica el pago de la factura                            
                				imagen = "<a " + click + " target='_blank' data-toggle='modal' data-target='#modal_Pagos_Electronicos' >Ver</a>"
                			}

                			$("<div>")
							.append($(imagen))

							 .appendTo(container);
                		}
                	}
                },

                ],
				//**************************************************************
				masterDetail: {
					enabled: true,
					template: function (container, options) {
						container.append(ObtenerDetallle(options.data.Pdf, options.data.Xml, options.data.EstadoAcuse, options.data.RutaAcuse, options.data.XmlAcuse, options.data.zip, options.data.RutaServDian, options.data.StrIdSeguridad, options.data.IdentificacionFacturador, options.data.NumeroDocumento, "Adquiriente"));
					}
				},
				//****************************************************************
				//Totales y agrupaciones
				summary: {
					groupItems: [
					{
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
				},
				filterRow: {
					visible: true
				},
			});
		});

	}

	//Redirecciona el pago interno al metodo del controlador de pago    
	ConsultarPago1 = function (IdSeguridad, Monto, PagosParciales, poseeIdComercioPSE, poseeIdComercioTC) {
		$rootScope.ConsultarPago(IdSeguridad, Monto, PagosParciales, poseeIdComercioPSE, poseeIdComercioTC);
	};


});



var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Fecha Recepción' },
    { ID: "2", Texto: 'Fecha Documento' }
    ];