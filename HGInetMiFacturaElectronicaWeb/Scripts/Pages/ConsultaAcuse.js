DevExpress.localization.locale(navigator.language);

var Usuariosession = "";
var App = angular.module('App', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento', 'AppSrvFiltro']);
App.controller('AcuseConsultaController', function AcuseConsultaController($scope, $http, $location, SrvDocumento, SrvMaestrosEnum, SrvFiltro) {

	var now = new Date();

	var codigo_facturador = "",
               codigo_adquiriente = "",
               numero_documento = "",
               fecha_inicio = "",
               fecha_fin = "",
               estado_acuse = "";
	Filtro_fecha = 2;

	SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {
		codigo_facturador = data[0].IdentificacionEmpresa;
		Usuariosession = data[0].IdSeguridad;
		consultar();
	});

	SrvFiltro.ObtenerFiltro('Documento Adquiriente', 'Adquiriente', 'icon-user-tie', 115, '/api/ObtenerAdquirientes?Facturador=' + $('#Hdf_Facturador').val(), 'ID', 'Texto', false, 6).then(function (Datos) {
		$scope.Adquiriente = Datos;
	});

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

	$scope.filtros =
           {
           	Fecha: {
           		//Carga la data del control
           		dataSource: new DevExpress.data.ArrayStore({
           			data: items_Fecha,
           			key: "ID"
           		}),
           		displayExpr: "Texto",
           		value: items_Fecha[1],

           		onValueChanged: function (data) {
           			if (data.value != null) {
           				Filtro_fecha = data.value.ID;
           			} else {
           				Filtro_fecha = 1;
           			}
           		}
           	},
           	EstadoRecibo: {
           		//Carga la data del control
           		dataSource: new DevExpress.data.ArrayStore({
           			data: items_recibo,
           			key: "ID"
           		}),
           		displayExpr: "Texto",
           		placeholder: "Seleccione un Item",
           		onValueChanged: function (data) {
           			estado_acuse = data.value.ID;
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

	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultar2();
		}
	};


	function consultar2() {
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		$('#wait').show();
		var codigo_adquiriente = (txt_hgi_Adquiriente == undefined || txt_hgi_Adquiriente == '') ? '' : txt_hgi_Adquiriente;
		$http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&codigo_adquiriente=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_acuse + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
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

	function consultar() {
		if (fecha_inicio == "")
			fecha_inicio = now.toISOString();

		if (fecha_fin == "")
			fecha_fin = now.toISOString();

		//Obtiene los datos del web api
		//ControladorApi: /Api/Documentos/
		//Datos GET: codigo_facturador, codigo_adquiriente, numero_documento, estado_recibo, fecha_inicio, fecha_fin
		$('#wait').show();
		$http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&codigo_adquiriente=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_acuse + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
			$('#wait').hide();
			$("#gridDocumentos").dxDataGrid({
				dataSource: response.data,
				paging: {
					pageSize: 20,
					enabled: true
				},
				keyExpr: "StrIdSeguridad",
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				}
            , loadPanel: {
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
				//**********************************************							
				onToolbarPreparing: function (e) {
					e.toolbarOptions.items.unshift(				
					{
						location: "after",
						widget: "dxButton",
						options: {
							icon: "clear", elementAttr: { title: "Reordenar Columnas" },
							onClick: function () {
								localStorage.removeItem('storageConsultarAcuse');
								consultar();
							}
						}
					})
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
					storageKey: 'storageConsultarAcuse',
				},
				onInitialized(e) {
					e.component.option("stateStoring.ignoreColumnOptionNames", ["filterValues"]);
				}
				//*********************


				, columns: [
                    {
                    	caption: "Archivos",
                    	cellTemplate: function (container, options) {

                    		var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

                    		var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

                    		if (options.data.Pdf)
                    			visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer'";
                    		else
                    			options.data.Pdf = "#";

                    		if (options.data.Xml)
                    			visible_xml = "href='" + options.data.Xml + "' style='pointer-events:auto;cursor: pointer'";
                    		else
                    			options.data.Xml = "#";

                    		$("<div>")
                                .append(
                                    $("<a target='_blank' class='icon-file-pdf'  " + visible_pdf + ">&nbsp;&nbsp;<a target='_blank' class='icon-file-xml' " + visible_xml + ">&nbsp;&nbsp;"),
                                    $("<a class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:12%; font-size:19px'></a>&nbsp;&nbsp;").dxButton({
                                    	onClick: function () {
                                    		$scope.showModal = true;
                                    		email_destino = options.data.MailAdquiriente;
                                    		id_seguridad = options.data.StrIdSeguridad;
                                    		$('input:text[name=EmailDestino]').val(email_destino);
                                    		SrvDocumento.ConsultarEmailUbl(options.data.StrIdSeguridad).then(function (data) {
                                    			$('input:text[name=EmailDestino]').val(data);
                                    		});
                                    	}
                                    }).removeClass("dx-button dx-button-normal dx-widget"))
                                        .append($(""))
                                        .appendTo(container);
                    	}
                    },
                     {
                     	caption: "Identificación Adquiriente",
                     	dataField: "IdentificacionAdquiriente"
                     },
                      {
                      	caption: "Nombre Adquiriente",
                      	dataField: "RazonSocial"
                      },
                      {
                      	caption: "Número Documento",
                      	dataField: "NumeroDocumento",
                      },
                    {
                    	caption: "Fecha Respuesta",
                    	dataField: "FechaRespuesta",
                    	dataType: "date",
                    	format: "yyyy-MM-dd HH:mm",
                    	validationRules: [{
                    		type: "required",
                    		message: "El campo Fecha es obligatorio."
                    	}]
                    },
                       {
                       	caption: "Estado Acuse",
                       	dataField: "Estado",
                       	cellTemplate: function (container, options) {

                       		$("<div>")
								.append($(ColocarEstadoAcuse(options.data.IntAdquirienteRecibo, options.data.Estado)))
								.appendTo(container);
                       	}
                       },
                      {
                      	caption: "Motivo Rechazo",
                      	dataField: "MotivoRechazo",
                      }
				],
				//**************************************************************
				masterDetail: {
					enabled: true,
					template: function (container, options) {
						container.append(ObtenerDetallle(options.data.Pdf, options.data.Xml, options.data.Estado, options.data.RutaAcuse, options.data.XmlAcuse, options.data.zip, options.data.RutaServDian, options.data.StrIdSeguridad, codigo_facturador, options.data.NumeroDocumento));
					}
				},
				//****************************************************************
				filterRow: {
					visible: true
				},
			});
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});

	}


});

App.controller('EnvioEmailController', function App($scope, $http, $location) {

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
				$http.post('/api/Documentos?id_seguridad=' + id_seguridad + '&mail=' + email_destino + '&Usuario=' + Usuariosession).then(function (responseEnvio) {
					$('#wait').hide();

					swal({
						title: 'Proceso Éxitoso',
						text: 'El e-mail ha sido enviado con éxito.',
						type: 'success',
						confirmButtonColor: '#66BB6A',
						confirmButtonTex: 'Aceptar',
						animation: 'pop',
						html: true,
					});

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

var items_recibo =
    [
    { ID: "*", Texto: 'Obtener Todos' },
    { ID: "1", Texto: 'Aprobado' },
    { ID: "2", Texto: 'Rechazado' },
    { ID: "3", Texto: 'Aprobado Tácito' }
    ];


var items_Fecha =
    [
    { ID: "1", Texto: 'Fecha-Documento' },
    { ID: "2", Texto: 'Fecha-Acuse' }
    ];