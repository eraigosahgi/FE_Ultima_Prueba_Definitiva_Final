DevExpress.localization.locale('es-ES');

//var path = window.location.pathname;
//var ruta = window.location.href;
//ruta = ruta.replace(path, "/");
//document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/MaestrosEnum.js"></script>');

var Estado;
var UsuarioSession = "";
var App = angular.module('App', ['dx', 'AppMaestrosEnum', 'AppSrvFiltro']);
//Controlador para la consulta de documentos Adquiriente
App.controller('DocAdquirienteController', function ($scope, $rootScope, $http, $location, SrvMaestrosEnum, SrvFiltro) {

	var ValidarGestionPagos = "ValidarGestionPagos";
	$("#summary_Pagos").dxValidationSummary(
	{
		validationGroup: ValidarGestionPagos
	});

	SrvFiltro.ObtenerFiltro('Documento Facturador', 'Facturador', 'icon-user-tie', 115, '/api/ObtenerMisFacturadores', 'Identificacion', 'RazonSocial', false, 7).then(function (Datos) {
		$scope.Facturador = Datos;
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

				var alto_pantalla = $(window).height() - 10;
				var ancho_pantalla = $(window).width() - 10;

				var Vpago = window.open("", "Pagos", "top:10px, width=" + ancho_pantalla + "px,height=" + alto_pantalla + "px;");
				if (Vpago == null || Vpago == undefined) {
					DevExpress.ui.notify({ message: "Las ventanas emergentes estan bloqueadas, para realizar pagos, debe habilitarlas", position: { my: "center top", at: "center top" } }, "error", 6000);
				} else {

					if ($scope.total > 0) {
						///Pago Multiple						
						$http.get('/api/PagoMultiple?lista_documentos=' + $scope.documentos + '&valor_pago=' + $scope.total).then(function (response) {



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
					} else {
						DevExpress.ui.notify('El monto a pagar debe ser mayor a cero(0)', 'error', 6000);
					}

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
			consultar2();
		}
	};


	function consultar2() {
		$('#Total').text("");
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();


		var codigo_facturador = (txt_hgi_Facturador != undefined) ? txt_hgi_Facturador : '';

		$('#wait').show();
		$http.get('/api/ObtenerDocumentosAdquirientes?codigo_facturador=' + codigo_facturador + '&codigo_adquiente=' + codigo_adquiente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_filtro_fecha=' + tipo_filtro_fecha).then(function (response) {
			$('#wait').hide();
			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data,
				paging: {
					pageSize: 20,
					enabled: true
				}
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


		var codigo_facturador = (txt_hgi_Facturador != undefined) ? txt_hgi_Facturador : '';
		//Obtiene los datos del web api
		//ControladorApi: /Api/Documentos/
		//Datos GET: codigo_adquiente - numero_documento - estado_recibo - fecha_inicio - fecha_fin
		$('#wait').show();
		$http.get('/api/ObtenerDocumentosAdquirientes?codigo_facturador=' + codigo_facturador + '&codigo_adquiente=' + codigo_adquiente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_filtro_fecha=' + tipo_filtro_fecha).then(function (response) {
			$('#wait').hide();
			$("#gridDocumentos").dxDataGrid({
				onToolbarPreparing: function (e) {
					e.toolbarOptions.items.unshift(
					//**********************************************							
					{
						location: "after",
						widget: "dxButton",
						options: {
							icon: "clear", elementAttr: { title: "Reordenar Columnas" },
							onClick: function () {
								localStorage.removeItem('storageConsultarDocumentosAdquirientes');
								consultar();
							}
						}
					})
				},
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
				columnAutoWidth: true,
				stateStoring: {
					enabled: false,
					type: 'localStorage',
					storageKey: 'storageConsultarDocumentosAdquirientes',
				},
				onInitialized(e) {
					e.component.option("stateStoring.ignoreColumnOptionNames", ["filterValues"]);
				},
				//*********************
				dataSource: response.data,
				paging: {
					enabled: true
				},
				keyExpr: "StrIdSeguridad",
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				},
				//Formatos personalizados a las columnas en este caso para el monto
				onCellPrepared: function (options) {
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
					width: "auto",
					cellTemplate: function (container, options) {
						if (options.data.tipodoc != 'Nota Crédito' && options.data.poseeIdComercio == 1 && options.data.Saldo > 0) {
							$('<div id="chkSaldo_' + options.data.StrIdSeguridad + '"></div>').dxCheckBox({
								name: "chkSaldo_" + options.data.StrIdSeguridad,
								onValueChanged: function (data) {
									validarSeleccion();
								}
							})
							.appendTo(container);
						}
					},
				},
                    {
                    	caption: "Archivos",
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
                    	visible: false,
                    	validationRules: [{
                    		type: "required",
                    		message: "El campo Fecha es obligatorio."
                    	}]
                    },

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
                      	caption: "Estado Evento",
                      	cssClass: "hidden-xs col-md-1",
                      	dataField: "EstadoAcuse",
                      	cellTemplate: function (container, options) {

                      		$("<div>")
								.append($(ColocarEstadoAcuse(options.data.IntAdquirienteRecibo, options.data.EstadoAcuse, options.data.RutaAcuse)))
								.appendTo(container);
                      	}
                      },
                      {
                      	caption: "Motivo Rechazo",
                      	cssClass: "hidden-xs col-md-1",
                      	dataField: "MotivoRechazo",
                      },
                      //{
                      //	dataField: "",
                      //	caption: "Acuse",
                      //	cssClass: "col-md-1 col-xs-2",
                      //	cellTemplate: function (container, options) {

                      //		var Mostrar_Acuse;
                      //		if (options.data.Estado != 400)
                      //			Mostrar_Acuse = "<a target='_blank'  href='" + options.data.RutaAcuse + "'>Acuse</a>";
                      //		else
                      //			Mostrar_Acuse = "";

                      //		$("<div>")
					  //  		.append($(Mostrar_Acuse))
					  //  		.appendTo(container);
                      //	}
                      //},
					   {
					   	caption: "Saldo",
					   	dataField: "Saldo",
					   	//visible: false,
					   	//cssClass: "col-md-1 col-xs-3",
					   	//disabled: !data.habilitar_documento,
					   	cellTemplate: function (container, options) {

					   		if (options.data.tipodoc != 'Nota Crédito' && options.data.poseeIdComercio == 1 && options.data.Saldo > 0) {
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
					    				//min: 0,
					    				max: options.data.Saldo,
					    				message: "Monto incorrecto"
					    			}]
					    		})
					    		.appendTo(container);
					   		}
					   	},



					   },
					     {
					     	caption: "Valor Total",
					     	dataField: "IntVlrTotal",
					     	//cssClass: "col-md-1 col-xs-1",
					     	//width: '12%',
					     	Type: Number,
					     }
                    ,
					{
						caption: "SubTotal",
						dataField: "IntSubTotal",
						//cssClass: "col-md-1 col-xs-1",
						//width: '12%',
						Type: Number,
						visible: false
					}
                    ,
					{
						caption: "Neto",
						dataField: "IntNeto",
						//cssClass: "col-md-1 col-xs-1",
						//width: '12%',
						Type: Number,
						visible: false
					}
                    ,
                {
                	///Opción de pago
                	//cssClass: "col-md-1 ",
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

                			if (options.data.tipodoc != 'Nota Crédito' && options.data.poseeIdComercio == 1 && options.data.Saldo <= 0) {//aqui se debe colocar el status que indica el pago de la factura                            
                				imagen = "<a " + click + " target='_blank' data-toggle='modal' data-target='#modal_Pagos_Electronicos' class='btn btn-default' >Ver</a>"
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

	function validarSeleccion() {
		var data = $("#gridDocumentos").dxDataGrid("instance").option().dataSource;
		var lista = '';
		var total_a_pagar = 0;
		var total_seleccionados = 0;
		var comercios_direfentes = false;
		//Validamos si el comercio es diferente
		var comercio_actual = "";
		var facturador_actual = "";
		for (var i = 0; i < data.length; i++) {

			valor = false;

			try {
				var valor = $("#chkSaldo_" + data[i].StrIdSeguridad).dxCheckBox("instance").option().value;
			} catch (e) {

			}
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
				if (facturador_actual != "") {
					if (facturador_actual != data[i].Facturador) {
						comercios_direfentes = true;
						swal({
							title: 'Facturador Diferente',
							text: 'Para realizar pagos de multiples documentos, deben ser al mismo Facturador',
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
				facturador_actual = data[i].Facturador;
			}


		}



		if (total_seleccionados > 0) {
			$("#gridDocumentos").dxDataGrid("columnOption", "Pago", "visible", false);
			$("#gridDocumentos").dxDataGrid("columnOption", "Saldo", "visible", true);


			for (var i = 0; i < data.length; i++) {

				var valor_seleccionado = "0";
				try {
					var seleccion = $("#chkSaldo_" + data[i].StrIdSeguridad).dxCheckBox("instance").option().value;
				} catch (e) {

				}
				if (seleccion) {

					try {

						valor_seleccionado = $("#txtSaldo_" + data[i].StrIdSeguridad).dxNumberBox("instance").option().value;
						lista += (lista) ? ',' : '';
						lista += "{Documento: '" + data[i].StrIdSeguridad + "',Valor: '" + valor_seleccionado.toString().replace(",", ".") + "'}";
						total_a_pagar += valor_seleccionado;
					} catch (e) {

					}
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

	txt_hgi_Facturador = "";
	consultar();
});



var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Fecha Recepción' },
    { ID: "2", Texto: 'Fecha Documento' }
    ];