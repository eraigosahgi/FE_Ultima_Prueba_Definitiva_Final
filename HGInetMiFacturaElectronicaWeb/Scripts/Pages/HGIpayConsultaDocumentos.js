DevExpress.localization.locale('es-ES');

var Estado;
var UsuarioSession = "";
var App = angular.module('App', ['dx', 'AppMaestrosEnum', 'AppSrvFiltro', 'AppSrvDocumento']);
//Controlador para la consulta de documentos Adquiriente de pagos
var serial = "";
App.controller('HGIpayConsultaDocumentosController', function ($scope, $rootScope, $http, $location, SrvMaestrosEnum) {


	var ValidarGestionPagos = "ValidarGestionPagos";
	$("#summary_Pagos").dxValidationSummary(
	{
		validationGroup: ValidarGestionPagos
	});


	$("#BtnCerrar").dxButton({
		text: "Cerrar",
		type: "danger",
		icon: ' icon-switch2',
		onClick: function (e) {
			window.location.assign("../Login/Pagos.aspx?serial=" + serial);
		}
	});

	$("#multipagos").dxButton({
		text: "Pagar",
		type: "success",
		icon: 'money',
		visible: false,
		validationGroup: ValidarGestionPagos,
		onClick: function (e) {
			var result = e.validationGroup.validate();
			if (result.isValid) {

				var Vpago = window.open("", "Pagos", "width=10,height=10");
				if (Vpago == null || Vpago == undefined) {
					DevExpress.ui.notify({ message: "Las ventanas emergentes estan bloqueadas, para realizar pagos, debe habilitarlas", position: { my: "center top", at: "center top" } }, "error", 6000);
				} else {
					console.log("Generar Pago: ", $scope.documentos);
					///Pago Multiple
					$http.get('/api/PagoMultiple?lista_documentos=' + $scope.documentos + '&valor_pago=' + $scope.total).then(function (response) {

						var alto_pantalla = $(window).height() - 10;
						var ancho_pantalla = $(window).width() - 10;
						
						//Ruta servicio
						var RutaServicio = $('#Hdf_RutaPagos').val() + "?IdSeguridad=";
						$scope.Idregistro = response.data.IdRegistro;
						
						var Vpago2 = window.open(RutaServicio + response.data.Ruta, "Pagos", "top:10px, width=" + ancho_pantalla + "px,height=" + alto_pantalla + "px;");
						//$timeout(function callAtTimeout() {
						//	VerificarEstado();
						//}, 90000);

					}, function (error) {

						if (error != undefined) {
							DevExpress.ui.notify(error.data.ExceptionMessage, 'error', 6000);
							$("#button").dxButton({ visible: true });


							if (error.data.ExceptionMessage == 'Documento ya no esta disponible') {
								$('#modal_Pagos_Electronicos').modal('hide')
							}

						}


					});

					///
				}
			}
		}
	});


	var now = new Date();

	var codigo_adquiente = "",
           numero_documento = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           tipo_filtro_fecha = 1;

	//Validación de Serial de Cliente de Pagos**********************************
	serial = location.search.split('Serial=')[1];
	//Validamos si tiene un serial en la url
	if (serial != undefined) {
		//Si es asi, entonces asignamos la imagen del tercero
		$('#img_cliente').attr("src", "/../Scripts/Images/Terceros/" + serial + ".png");
	} else {
		console.log("Retornar a Index");
	}

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
		$http.get('/api/HGIpayConsultaDocumentos?IdSeguridad=' + serial + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_filtro_fecha=' + tipo_filtro_fecha).then(function (response) {
			$('#wait').hide();
			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data
			});
		});
	}



	//Consultar DOcumentos
	function consultar() {
		$('#Total').text("");

		$('#Total_a_Pagar').text("");
		$("#multipagos").dxButton({ visible: false });


		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		$('#wait').show();
		$http.get('/api/HGIpayConsultaDocumentos?IdSeguridad=' + serial + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_filtro_fecha=' + tipo_filtro_fecha).then(function (response) {
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
					caption: "Seleccionar",
					cssClass: "col-md-1 col-xs-2",					
					cellTemplate: function (container, options) {
						$('<div id="chkSaldo_' + options.data.StrIdSeguridad + '"></div>').dxCheckBox({
							name: "chkSaldo_" + options.data.StrIdSeguridad,
							onValueChanged: function (data) {
								validarSeleccion();
							}
						})
						.appendTo(container);
					},
				},
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
						caption: "IdComercio",
						dataField: "IdComercio",
						cssClass: "col-md-1 col-xs-3",
						visible: false
					},
					{
						caption: "DescripComercio",
						dataField: "DescripComercio",
						cssClass: "col-md-1 col-xs-3",
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
                    //{
                    //	caption: "Fecha Vencimiento",
                    //	dataField: "DatFechaVencDocumento",
                    //	dataType: "date",
                    //	format: "yyyy-MM-dd ",
                    //	cssClass: "hidden-xs col-md-1",
                    //	validationRules: [{
                    //		type: "required",
                    //		message: "El campo Fecha es obligatorio."
                    //	}]
                    //},
                    {
                    	caption: "Valor Total",
                    	dataField: "IntVlrTotal",
                    	cssClass: "col-md-1 col-xs-1",
                    	width: '12%',
                    	Type: Number,
                    }
                    //,
					//{
					//	caption: "SubTotal",
					//	dataField: "IntSubTotal",
					//	cssClass: "col-md-1 col-xs-1",
					//	width: '12%',
					//	Type: Number,
					//}
                    //,
					//{
					//	caption: "Neto",
					//	dataField: "IntNeto",
					//	cssClass: "col-md-1 col-xs-1",
					//	width: '12%',
					//	Type: Number,
					//}
                    ,
                     //{
                     //	caption: "Identificación Facturador",
                     //	cssClass: "hidden-xs col-md-1",
                     //	dataField: "IdFacturador"

                     //},
                     // {
                     // 	caption: "Nombre Facturador",
                     // 	cssClass: "hidden-xs col-md-1",
                     // 	dataField: "Facturador"
                     // },

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
                      //{
                      //	caption: "Motivo Rechazo",
                      //	cssClass: "hidden-xs col-md-1",
                      //	dataField: "MotivoRechazo",
                      //},
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
                	caption: "Saldo",
                	dataField: "Saldo",
                	visible: false,
                	cssClass: "col-md-1 col-xs-2",
                	//disabled: !data.habilitar_documento,
                	cellTemplate: function (container, options) {

                		$('<div id="txtSaldo_' + options.data.StrIdSeguridad + '"></div>').dxNumberBox({
                			value: options.data.Saldo,
                			name: "txtSaldo_" + options.data.StrIdSeguridad,
                			disabled: (options.data.PagosParciales == 1) ? false : true,
                			inputAttr: {
                				id: "txtSaldo_cursor_" + options.data.StrIdSeguridad,
                				style: "width: 100%; text-align:right;",
                			},

                			//tabIndex: Documento_Indice,
                			format: {
                				type: "fixedPoint",
                				precision: 2
                			},
                			onValueChanged: function (data) {
                				validarSeleccion();
                			},
                		})
						.dxValidator({
							validationGroup: ValidarGestionPagos,
							validationRules: [{
								type: "numeric",
								message: "El campo debe ser numerico"
							}, {
								type: "range",
								min: 1,
								max: options.data.Saldo,
								message: "Monto incorrecto"
							}]
						})
						.appendTo(container);
                	},


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
					},
					//{
					//	column: "IntSubTotal",
					//	summaryType: "sum",
					//	displayFormat: " {0} Neto ",
					//	valueFormat: "currency"
					//},
					//{
					//	column: "IntNeto",
					//	summaryType: "sum",
					//	displayFormat: " {0} Neto ",
					//	valueFormat: "currency"
					//}
					]
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
					//{
					//	name: "SumaSubTotal",
					//	summaryType: "sum",
					//	column: "IntSubTotal",
					//	customizeText: function (data) {
					//		return fNumber.go(data.value).replace("$-", "-$");
					//	}

					//},
					//{
					//	name: "SumaNeto",
					//	displayFormat: "{0}",
					//	valueFormat: "currency",
					//	summaryType: "custom"

					//},
					//{
					//	column: "IntNeto",
					//	summaryType: "sum",
					//	customizeText: function (data) {
					//		return fNumber.go(data.value).replace("$-", "-$");
					//	}

					//},
                    {
                    	showInColumn: "DatFechaVencDocumento",
                    	displayFormat: "Total : ",
                    	alignment: "right"
                    }
                    ],
					calculateCustomSummary: function (options) {
						//	if (options.name === "Suma") {
						//		if (options.summaryProcess === "start") {
						//			options.totalValue = 0;
						//			$('#Total').text("");
						//		}
						//		if (options.summaryProcess === "calculate") {
						//			options.totalValue = options.totalValue + options.value.IntVlrTotal;
						//			$('#Total').text("Total: " + fNumber.go(options.totalValue).replace("$-", "-$"));
						//		}
						//	}

						//	//if (options.name === "SumaSubTotal") {
						//	//	if (options.summaryProcess === "start") {
						//	//		options.totalValue = 0;
						//	//		$('#SubTotal').text("");
						//	//	}
						//	//	if (options.summaryProcess === "calculate") {
						//	//		options.totalValue = options.totalValue + options.value.IntSubTotal;
						//	//		$('#SubTotal').text("SubTotal: " + fNumber.go(options.totalValue).replace("$-", "-$"));
						//	//	}
						//	//}


						//	//if (options.name === "SumaNeto") {
						//	//	if (options.summaryProcess === "start") {
						//	//		options.totalValue = 0;
						//	//		$('#Neto').text("");
						//	//	}
						//	//	if (options.summaryProcess === "calculate") {
						//	//		options.totalValue = options.totalValue + options.value.IntNeto;
						//	//		$('#Neto').text("Neto: " + fNumber.go(options.totalValue).replace("$-", "-$"));
						//	//	}
						//	//}
					}
				},
				filterRow: {
					visible: true
				},
			});
		})

		//*******************
		.catch(function (data) {
			swal({
				title: 'Alerta',
				text: data.data.ExceptionMessage,
				type: 'warning',
				confirmButtonColor: '#FF7043',
				confirmButtonText: 'Aceptar',
				animation: 'pop',
				html: true,
				closeOnConfirm: false
			});

		});
	}






	function validarSeleccion() {
		var data = $("#gridDocumentos").dxDataGrid("instance").option().dataSource;		
		var lista = '';
		var total_a_pagar = 0;
		var total_seleccionados = 0;		
		var comercios_direfentes = false;

		var comercio_actual = "";
		for (var i = 0; i < data.length; i++) {

			var valor = $("#chkSaldo_" + data[i].StrIdSeguridad).dxCheckBox("instance").option().value;
			if (valor) {
				if (comercio_actual != "") {
					if (comercio_actual != data[i].IdComercio) {
						comercios_direfentes = true;
						swal({
							title: 'Comercios Diferentes',
							text: 'Para realizar pagos de multiples documentos, estos deben ser del mismo comercio',
							icon: 'error',
							confirmButtonColor: '#66BB6A',
							confirmButtonText: 'Aceptar',
							animation: 'pop',
							html: true,
						});
					}
				}
				total_seleccionados += 1;
				comercio_actual = data[i].IdComercio;
			}
		}

		if (total_seleccionados > 0) {
			$("#gridDocumentos").dxDataGrid("columnOption", "Pago", "visible", false);
			$("#gridDocumentos").dxDataGrid("columnOption", "Saldo", "visible", true);


			for (var i = 0; i < data.length; i++) {

				var valor_seleccionado = 0;
				var seleccion = $("#chkSaldo_" + data[i].StrIdSeguridad).dxCheckBox("instance").option().value;
				if (seleccion) {
					lista += (lista) ? ',' : '';
					valor_seleccionado = $("#txtSaldo_" + data[i].StrIdSeguridad).dxNumberBox("instance").option().value;
					lista += "{Documento: '" + data[i].StrIdSeguridad + "',Valor: '" + valor_seleccionado + "'}";
					total_a_pagar += valor_seleccionado;
				}
			}
			lista = "[" + lista + "]"
			$scope.documentos = lista;			
			$scope.total = total_a_pagar;

		} else {
			$scope.documentos = "Ningun Documento Por Procesar";
			$scope.total = 0;
			$("#gridDocumentos").dxDataGrid("columnOption", "Pago", "visible", true);
			$("#gridDocumentos").dxDataGrid("columnOption", "Saldo", "visible", false);
			$("#multipagos").dxButton({ visible: false });
		}

		if (total_a_pagar > 0) {


			if (comercios_direfentes) {
				$('#Total_a_Pagar').text("");
				$("#multipagos").dxButton({ visible: false });
			} else {
				$('#Total_a_Pagar').text("Total: " + fNumber.go(total_a_pagar).replace("$-", "-$"));
				$("#multipagos").dxButton({ visible: true });
			}
		} else {
			$('#Total_a_Pagar').text("");
			$("#multipagos").dxButton({ visible: false });
		}
	
	}


	//Redirecciona el pago interno al metodo del controlador de pago    
	ConsultarPago1 = function (IdSeguridad, Monto, PagosParciales, poseeIdComercioPSE, poseeIdComercioTC) {
		$rootScope.ConsultarPago(IdSeguridad, Monto, PagosParciales, poseeIdComercioPSE, poseeIdComercioTC);
	};


});



App.controller('HGIpayPagosAdquirienteController', function ($scope, $http, $location, SrvMaestrosEnum, SrvFiltro, SrvDocumento) {

	var now = new Date();
	var Estado;
	var ResolucionesPrefijo = [];

	var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "",
    resolucion = "",
    Filtro_fecha = 2
	Estado_Documento = "";

	$scope.documentosPendientes = false;

	$scope.Verificando = false;

	//Objecto json con documentos pendientes
	var datos = [];
	var objeto = {};

	$scope.CantidaddocPendientes = 0;

	$("#FechaInicialPagos").dxDateBox({
		value: now,
		width: '100%',
		displayFormat: "yyyy-MM-dd",
		onValueChanged: function (data) {
			fecha_inicio = new Date(data.value).toISOString();
			$("#FechaFinalPagos").dxDateBox({ min: fecha_inicio });
		}

	});

	$("#FechaFinalPagos").dxDateBox({
		value: now,
		width: '100%',
		displayFormat: "yyyy-MM-dd",
		onValueChanged: function (data) {
			fecha_fin = new Date(data.value).toISOString();
			$("#FechaInicialPagos").dxDateBox({ max: fecha_fin });
		}

	});




	SrvMaestrosEnum.ObtenerEnum(4).then(function (dataacuse) {
		items_recibo = dataacuse;
		cargarFiltros();
	});




	function cargarFiltros() {



		//Define los campos y las opciones
		$scope.filtros =
            {
            	Fecha: {
            		//Carga la data del control
            		dataSource: new DevExpress.data.ArrayStore({
            			data: TiposFiltroFecha,
            			key: "ID"
            		}),
            		displayExpr: "Texto",
            		value: TiposFiltroFecha[1],

            		onValueChanged: function (data) {
            			if (data.value != null) {
            				Filtro_fecha = data.value.ID;
            			} else {
            				Filtro_fecha = 1;
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
            			estado_recibo = (data.value == null) ? "*" : data.value.ID;
            		}

            	},
            	NumeroDocumento: {
            		placeholder: "Ingrese Número Documento",
            		onValueChanged: function (data) {
            			numero_documento = (data.value == null) ? "" : data.value;
            		}
            	},
            	Adquiriente: {
            		placeholder: "Ingrese Identificación del Facturador",
            		onValueChanged: function (data) {
            			codigo_adquiriente = data.value;
            		}
            	}
            }

		$("#FechaFinalPagos").dxDateBox({ min: now });
		$("#FechaInicialPagos").dxDateBox({ max: now });

		//validarEstado();
		//consultar();
	}

	$scope.ButtonOptionsConsultarPagos = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			validarEstado();
			consultar();
		}
	};


	$scope.ButtonProcesarPagos = {
		type: "success",
		onClick: function (e) {
			validarEstado();
			consultar();
		}
	};

	$scope.buttonVerificando = {
		type: "success",
		onClick: function (e) {
			DevExpress.ui.notify("Aun no ha terminado el proceso actual", 'error', 3000);
		}
	};


	function validarEstado() {

		$('#TotalPagos').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();


		$('#wait').show();
		//var codigo_Facturador_consulta = (txt_hgi_Facturador == undefined || txt_hgi_Facturador == '') ? '' : txt_hgi_Facturador;
		$http.get('/api/HGIpayObtenerPagosAdquiriente?IdSeguridad=' + serial + '&numero_documento=' + numero_documento + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
			$('#wait').hide();
			datos = [];
			//Recorro la data para ver la cantidad de documentos pendientes por procesar
			response.data.forEach(function (valor, indice, array) {
				if (valor.CodEstado == 888 || valor.CodEstado == 999) {
					datos.push({ "StrIdRegistro": valor.StrIdRegistro, "StrIdSeguridadDoc": valor.StrIdSeguridadDoc });
				}
			});

			objeto.datos = datos;

			if (objeto.datos.length > 0) {
				//var RutaServicio = $('#Hdf_RutaSrvPagos').val();

				objeto.datos.forEach(function (valor, indice, array) {
					//Conuslto el estado del documento en PI
					//SrvDocumento.ActualizaEstatusPago(RutaServicio, valor.StrIdSeguridadDoc, valor.StrIdRegistro).then(function (data1) {
					//Actualizado datos de plataforma intermedia, en la plataforma de FE
					SrvDocumento.ActualizaEstatusPagoInterno(valor.StrIdSeguridadDoc, valor.StrIdRegistro).then(function (data2) {
						consultar();
					});


					//});

				});
			}
		});

		try {
			if (!$scope.$$phase) {
				$scope.$apply(function () {
					$scope.Verificando = false;
				});
			}
		} catch (e) {

		}
	}


	function consultar() {
		$('#TotalPagos').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		//Obtiene los datos del web api        
		$('#wait').show();
		//var codigo_Facturador_consulta = (txt_hgi_Facturador == undefined || txt_hgi_Facturador == '') ? '' : txt_hgi_Facturador;
		$http.get('/api/HGIpayObtenerPagosAdquiriente?IdSeguridad=' + serial + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_facturador + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
			$('#wait').hide();


			datos = [];
			//Recorro la data para ver la cantidad de documentos pendientes por procesar
			response.data.forEach(function (valor, indice, array) {
				if (valor.CodEstado == 888 || valor.CodEstado == 999) {
					datos.push({ "StrIdRegistro": valor.StrIdRegistro, "StrIdSeguridadDoc": valor.StrIdSeguridadDoc });
				}
			});


			objeto.datos = datos;

			if (objeto.datos.length > 0) {
				$scope.documentosPendientes = true;
			} else {
				$scope.documentosPendientes = false;
			}

			$("#gridPagos").dxDataGrid({
				dataSource: response.data,
				keyExpr: "StrIdSeguridadDoc",
				paging: {
					pageSize: 20
				},
				stateStoring: {
					enabled: true,
					type: "localStorage",
					storageKey: "storage"
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
                		if (options.column.caption == "Valor") {
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
        , columns: [{
        	caption: 'Detalles',
        	cssClass: "col-md-1 col-xs-1",
        	cellTemplate: function (container, options) {
        		$("<div style='text-align:center;'><a style='margin-left:5%;' class='icon-file-eye' onClick=ConsultarDetallesPago('" + options.data.StrIdRegistro + "','" + options.data.StrIdSeguridadDoc + "') title='Ver Detalles Pago'></a></div>")
						.appendTo(container);

        	}
        },
		{
			cssClass: "col-md-1 col-xs-1",
			caption: 'Estado',
			dataField: 'EstadoFactura',
			cellTemplate: function (container, options) {
				$("<div>").append($(ControlEstadoPago(options.data.CodEstado, options.data.EstadoFactura))).appendTo(container);
			}
		},

        {
        	cssClass: "col-md-1 col-xs-1",
        	caption: 'Ticket',
        	dataField: 'Ticket',

        },
		 {
		 	cssClass: "col-md-1 col-xs-1",
		 	caption: 'Codigo CUS',
		 	dataField: 'Cus',

		 },
		{
			caption: "Forma Pago",
			dataField: "Franquicia",
			width: '90px',
			alignment: "center",
			cellTemplate: function (container, options) {

				if (options.data.Franquicia != "") {
					var titulo = (options.data.Franquicia != "") ? "title = " + options.data.Franquicia : "";

					$('<img src="/Scripts/Images/' + options.data.Franquicia + '.png"  ' + titulo + ' />')
							.appendTo(container);
				}
			}
		},
              {
              	caption: "Fecha Pago",
              	dataField: "DatAdquirienteFechaRecibo",
              	dataType: "date",
              	format: "yyyy-MM-dd HH:mm"

              },
            //{
            //	caption: "Documento",
            //	dataField: "NumeroDocumento",

            //},
             {
             	caption: "Cod. Transacción",
             	dataField: "idseguridadpago",
             },
              {
              	caption: "Fecha Verificación",
              	dataField: "DatFechaVencDocumento",
              	dataType: "date",
              	format: "yyyy-MM-dd HH:mm"
              },
              {
              	caption: "Valor",
              	dataField: "PagoFactura"
              }



        ],
				//**************************************************************
        masterDetail: {
        	enabled: true,
        	template: function (container, options) {

        		var currentEmployeeData = options.data;
        		$("<div>")
					.dxDataGrid({
						columnAutoWidth: true,
						showBorders: true,
						onCellPrepared: function (options) {
							var fieldData = options.value,
								fieldHtml = "";
							try {
								if (options.column.caption == "Monto") {
									if (fieldData) {
										var inicial = fNumber.go(fieldData).replace("$-", "-$");
										options.cellElement.html(inicial);
									}
								}

							} catch (err) {
							}

						},
						columns: [
						{
							caption: "Prefijo",
							dataField: "Prefijo",


						},
						{
							caption: "Documento",
							dataField: "Documento"
						},
						{
							caption: "Monto",
							dataField: "Monto",

						}],
						dataSource: options.data.Pagos

					}).appendTo(container);
        		//container.append(ObtenerDetallle(options.data.Pdf, options.data.Xml, options.data.EstadoAcuse, options.data.RutaAcuse, options.data.XmlAcuse, options.data.zip, options.data.RutaServDian, options.data.StrIdSeguridad, options.data.IdentificacionFacturador, options.data.NumeroDocumento, "Adquiriente"));
        	}
        },
				//****************************************************************

				summary: {
					groupItems: [{
						column: "PagoFactura",
						summaryType: "sum",
						displayFormat: " {0} TotalPagos ",
						valueFormat: "currency"
					}]
                    , TotalPagosItems: [{
                    	name: "Suma",
                    	displayFormat: "{0}",
                    	valueFormat: "currency",
                    	summaryType: "custom"

                    },
					{
						column: "PagoFactura",
						showInColumn: "PagoFactura",
						summaryType: "sum",
						customizeText: function (data) {
							return fNumber.go(data.value).replace("$-", "-$");
						}

					},
                    {
                    	showInColumn: "DatFechaVencDocumento",
                    	displayFormat: "TotalPagos : ",
                    	alignment: "right"
                    }
                    ],
					calculateCustomSummary: function (options) {
						if (options.name === "Suma") {
							if (options.summaryProcess === "start") {
								options.TotalPagosValue = 0;
								$('#TotalPagos').text("");
							}
							if (options.summaryProcess === "calculate") {
								options.TotalPagosValue = options.TotalPagosValue + options.value.PagoFactura;
								$('#TotalPagos').text("TotalPagos: " + fNumber.go(options.TotalPagosValue).replace("$-", "-$"));
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


});


ConsultarDetallesPago = function (IdRegistroPago, IdSeguridadDoc) {

	var ruta_redireccion = $('#Hdf_RutaSrvPagos').val() + "/Views/DetallesPagoE.aspx?IdSeguridadPago=" + IdSeguridadDoc + "&IdSeguridadRegistro=" + IdRegistroPago;

	console.log(ruta_redireccion);

	$("#modal_detalles_pago").modal('show');
	$("#ContenidoDetallesPago").html('<object data="' + ruta_redireccion + '" style="width: 100%; height: 600px" />');
};


var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Fecha Recepción' },
    { ID: "2", Texto: 'Fecha Documento' }
    ];