DevExpress.localization.locale(navigator.language);
var opc_pagina = "1334";

var Datos_DocRef = "";
var Datos_Meses_Vence = 12;

var GestionPlanesApp = angular.module('GestionPlanesApp', ['dx', 'AppMaestrosEnum', 'AppSrvFiltro']);

//Controlador para la gestion planes transaccionales
GestionPlanesApp.controller('GestionPlanesController', function GestionPlanesController($scope, $http, $location, SrvMaestrosEnum, SrvFiltro) {

	var TiposProceso = [];
	var TiposDoc = [];
	var now = new Date();

	var StrIdSeguridad = location.search.split('IdSeguridad=')[1];

	var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           Datos_Email = true,
		   Datos_Vence = true,
		   Datos_FechaVence = "",
           codigo_adquiriente = "";

	$scope.Vence = true;

	$scope.Tprocesados = "";
	$scope.Tdisponibles = "";
	$scope.FechaInicio = "";

	SrvFiltro.ObtenerFiltro('Empresa', 'Facturador', 'icon-user-tie', 115, '/api/Empresas?Facturador=true', 'Identificacion', 'RazonSocial', true, 1).then(function (Datos) {
		$scope.Facturador = Datos;
	});


	$http.get('/api/DatosSesion/').then(function (response) {
		codigo_facturador = response.data[0].Identificacion;
		var tipo = response.data[0].Admin;
		if (tipo) {
			$scope.Admin = true;
		};
		$http.get('/api/Usuario/').then(function (response) {
			//Obtiene el código del permiso.
			$http.get('/api/Permisos?codigo_usuario=' + response.data[0].CodigoUsuario + '&identificacion_empresa=' + codigo_facturador + '&codigo_opcion=' + opc_pagina).then(function (response) {
				$("#wait").hide();
				try {
					var respuesta;

					//Valida si el id_seguridad contiene datos
					if (StrIdSeguridad) {
						respuesta = response.data[0].Editar;
						$scope.PanelNotificacion = false;
						$('#Leyenda').show();
						try {
							//Google Analytics							
							ga('send', 'event', 'Consulta_Plan', 'Plan : ' + StrIdSeguridad, sessionStorage.getItem("Usuario"));
						} catch (e) { }
					} else {
						respuesta = response.data[0].Agregar
						$scope.PanelNotificacion = true;
						$('#Leyenda').hide();
						try {
							//Google Analytics
							ga('send', 'event', 'Consulta_Plan', 'Nuevo Plan', sessionStorage.getItem("Usuario"));
						} catch (e) { }
					}

					//Valida la visibilidad del control según los permisos.
					if (respuesta)
						$('#button').show();
					else
						$('#button').hide();
				} catch (err) {
					DevExpress.ui.notify(err.message, 'error', 3000);
				}
			}, function errorCallback(response) {
				$('#wait').hide();
				DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			});
		});

	});

	var Datos_TiposProceso = "",
        codigo_empresa = "",
        Datos_T_compra = "",
        Datos_valor_plan = "",
        Datos_E_Plan = "",
		datos_empresa_asociada = "",
		Datos_obsrvaciones = "",
    	Datos_TipoDoc = 0;


	//Define los campos del Formulario  
	$(function () {
		$("#summary").dxValidationSummary({});
		SrvMaestrosEnum.ObtenerEnum(3).then(function (data) {
			TiposProceso = data;
			//Selección Tipo de Proceso
			$("#TipoProceso").dxRadioGroup({
				searchEnabled: true,
				caption: 'TipoProceso',
				layout: "horizontal",
				dataSource: TiposProceso,
				displayExpr: "Descripcion",
				Enabled: true,
				onValueChanged: function (data) {
					Datos_TiposProceso = data.value.ID;
					//Post-Pago
					if (Datos_TiposProceso == 3) {
						$('#divValorPlan').hide();
						$('#divCantTransacciones').hide();
						$("#CantidadTransacciones").dxNumberBox({ value: 0 });
						$("#ValorPlan").dxNumberBox({ value: 0 });
						ValidarCantTransacciones();
					}
					//Compra
					if (Datos_TiposProceso == 2) {
						$('#divValorPlan').show();
						$('#divCantTransacciones').show();
						ValidarCantTransacciones();
					}
					//Cortesía
					if (Datos_TiposProceso == 1) {
						$('#divValorPlan').hide();
						$('#divCantTransacciones').show();
						$("#ValorPlan").dxNumberBox({ value: 0 });
						ValidarCantTransacciones();
					}
				}
			}
            ).dxValidator({
            	validationRules: [{
            		type: "required",
            		message: "Debe indicar el tipo de proceso."
            	}]
            });



			if (StrIdSeguridad != '' && StrIdSeguridad != null) {
				Consultar(StrIdSeguridad);
			}
		});

		SrvMaestrosEnum.ObtenerEnum(11).then(function (data) {
			TiposDoc = data;
			//Selección Tipo de Proceso
			$("#TipoDoc").dxRadioGroup({
				searchEnabled: true,
				caption: 'TipoDoc',
				layout: "horizontal",
				dataSource: TiposDoc,
				displayExpr: "Descripcion",
				Enabled: true,
				onValueChanged: function (data) {
					Datos_TiposDoc = data.value.ID;
				}
			}
            ).dxValidator({
            	validationRules: [{
            		type: "required",
            		message: "Debe indicar el tipo de documento que descontara la plataforma."
            	}]
            });



			if (StrIdSeguridad != '' && StrIdSeguridad != null) {
				Consultar(StrIdSeguridad);
			}
		});

		function ValidarCantTransacciones() {
			$("#CantidadTransacciones").dxValidator({
				validationRules: [{
					type: 'custom', validationCallback: function (options) {
						if ((Datos_TiposProceso == 1 || Datos_TiposProceso == 2) && Datos_T_compra < 1) {
							options.rule.message = "Debe Indicar la cantidad de transacciones";
							return false;
						} else {
							return true;
						}
					}
				}]
			});

			$("#ValorPlan").dxValidator({
				validationRules: [{
					type: 'custom', validationCallback: function (options) {
						if ((Datos_TiposProceso == 2) && Datos_valor_plan < 1) {
							options.rule.message = "Debe Indicar el valor de las transacciones";
							return false;
						} else {
							return true;
						}
					}
				}]
			});






		}

		$("#DocRef").dxNumberBox({
			onValueChanged: function (data) {
				Datos_DocRef = data.value;
			}
		});


		//Campo cantidad de transacciones del plan
		$("#CantidadTransacciones").dxNumberBox({
			format: "#,##0",
			validationGroup: "ValidacionDatosPlan",
			onValueChanged: function (data) {
				Datos_T_compra = data.value;
			}
		})
        .dxValidator({
        	validationRules: [{
        		type: "stringLength",
        		max: 10,
        		message: "El número de transacciones no puede mayor a 10 digitos"
        	}, {
        		type: "required",
        		message: "Debe indicar la cantidad de transacciones del plan."
        	}, {
        		type: "numeric",
        		message: "El campo sólo debe contener números."
        	}, {
        		type: 'pattern',
        		pattern: '^[0-9]+$',
        		message: 'No debe Incluir puntos(.) ni caracteres especiales'
        	},
            {
            	type: 'custom', validationCallback: function (options) {
            		if ((Datos_TiposProceso == 1 || Datos_TiposProceso == 2) && Datos_T_compra < 1) {
            			options.rule.message = "Debe Indicar la cantidad de transacciones";
            			return false;
            		} else {
            			return true;
            		}
            	}
            }

        	]
        });


		//Campo Valor plan
		$("#ValorPlan").dxNumberBox({
			format: "$ #,##0",
			onValueChanged: function (data) {
				Datos_valor_plan = data.value;
			}
		})
        .dxValidator({
        	validationRules: [{
        		type: "required",
        		message: "Debe indicar el valor del plan."
        	}, {
        		type: "numeric",
        		message: "El campo sólo debe contener números."
        	}, {
        		type: 'pattern',
        		pattern: '^[0-9]+$',
        		message: 'No debe Incluir puntos(.) ni caracteres especiales'
        	}]
        });


		//Selección Estado de plan
		$("#EstadoPlan").dxRadioGroup({
			searchEnabled: true,
			caption: 'EstadoPlan',
			layout: "horizontal",
			dataSource: EstadosPlanes,
			displayExpr: "Texto",
			Enabled: true,
			onValueChanged: function (data) {
				Datos_E_Plan = data.value.ID;
			}
		}).dxValidator({
			validationRules: [{
				type: "required",
				message: "Debe indicar el estado del plan."
			}]
		});

		//Observaciones
		$("#txtObservaciones").dxTextArea({
			onValueChanged: function (data) {
				Datos_obsrvaciones = data.value.toUpperCase();
			}
		});


		$("#button").dxButton({
			text: "Guardar",
			type: "default",
			useSubmitBehavior: true
		});


		$("#form1").on("submit", function (e) {
			GuardarPlan();
			e.preventDefault
                ();
		});

	});



	$("#Vence").dxCheckBox({
		name: "Vence",
		value: true,
		onValueChanged: function (data) {
			Datos_Vence = data.value;
			if (Datos_Vence) {
				$scope.Vence = true;
				$('#panelfechaVencimiento').show();
			} else {
				$scope.Vence = false;
				$('#panelfechaVencimiento').hide();
			}
		}
	});

	var FVence = new Date(now);
	FVence.setFullYear(FVence.getFullYear() + 1);
	Datos_FechaVence = FVence.toISOString();


	$("#FechaVence").dxDateBox({
		value: FVence,
		width: '100%',
		readOnly: true,
		visible: false,
		displayFormat: "yyyy-MM-dd",
		onValueChanged: function (data) {
			Datos_FechaVence = new Date(data.value).toISOString();
		}

	});




	$("#Email").dxCheckBox({
		name: "Email",
		value: true,
		onValueChanged: function (data) {
			Datos_Email = data.value;
		}
	});

	$("#MesesVence").dxNumberBox({
		value: Datos_Meses_Vence,
		onValueChanged: function (data) {
			Datos_Meses_Vence = data.value;
		}
	});

	$scope.ButtonGuardar = {
		text: 'Guardar',
		type: 'default',
		validationGroup: "ValidacionDatosPlan",
		onClick: function (e) {
			GuardarPlan();
		}
	};

	function GuardarPlan() {
		if (txt_hgi_Facturador != null && txt_hgi_Facturador != "") {

			try {
				var empresaFactura = txt_hgi_Facturador.split(' -- ');
				empresa = empresaFactura[0];
			} catch (e) {
				empresa = txt_hgi_Facturador;
			}

		} else {
			DevExpress.ui.notify("Debe seleccionar el Facturador", 'error', 3000);
			return false;
		}

		var Meses = Datos_Meses_Vence + "";
		var n = Meses.includes(".");
		if (n) {
			DevExpress.ui.notify("El numero de meses debe ser un numero entero", 'error', 3000);
			return false;
		}

		if (Datos_T_compra == "") { Datos_T_compra = "0" }

		if (Datos_valor_plan == "") { Datos_valor_plan = "0" }

		if (Datos_Vence && Datos_Meses_Vence == 0) {
			DevExpress.ui.notify("Debe indicar la cantidad de meses de Vencimiento del Plan", 'error', 3000);
			return false;
		}


		var data = $.param({
			IntTipoProceso: Datos_TiposProceso,
			StrEmpresa: codigo_facturador,
			StrUsuario: $('#LblCodigoUsuario').text(),
			IntNumTransaccCompra: Datos_T_compra,
			IntNumTransaccProcesadas: 0,
			IntValor: Datos_valor_plan,
			Estado: Datos_E_Plan,
			StrObservaciones: Datos_obsrvaciones,
			StrEmpresaFacturador: empresa,
			Envia_email: Datos_Email,
			Vence: Datos_Vence,
			FechaVence: Datos_FechaVence,
			MesesVence: Datos_Meses_Vence,
			DocRef: Datos_DocRef,
			TipoDoc: Datos_TiposDoc
		});

		var IdActualizar = (StrIdSeguridad) ? '&' + $.param({ StrIdSeguridad: StrIdSeguridad, Editar: true }) : '';

		$("#wait").show();
		$http.post('/api/PlanesTransacciones?' + data + IdActualizar).then(function (response) {
			$("#wait").hide();
			try {
				
				if (response.data == "")
				{
					DevExpress.ui.notify({ message: "El plan ha sido registrado con exito.", position: { my: "center top", at: "center top" } }, "success", 1500);
					$("#button").hide();
					$("#btncancelar").hide();
					setTimeout(IrAConsulta, 1000);

					try {
						//Google Analytics
						if (StrIdSeguridad) {
							ga('send', 'event', 'Editando_Plan', 'Plan : ' + StrIdSeguridad, sessionStorage.getItem("Usuario"));
						} else {
							ga('send', 'event', 'Creando_Plan', 'Nuevo Plan', sessionStorage.getItem("Usuario"));
						}
					} catch (e) { }
				}
				else {
					DevExpress.ui.notify(response.data, 'error', 5000);
				}
				



			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}


	//Consultar por el id de seguridad para obtener los datos de la empresa a modificar


	function Consultar(StrIdSeguridad) {
		$("#wait").show();
		$http.get('/api/PlanesTransacciones?IdSeguridad=' + StrIdSeguridad).then(function (response) {
			$("#wait").hide();
			try {
				Datos_TiposProceso = response.data[0].Tipo;
				codigo_empresa = response.data[0].CodigoEmpresaFacturador;
				Datos_T_compra = response.data[0].TCompra;
				Datos_T_Procesadas = response.data[0].TProcesadas;
				Datos_valor_plan = response.data[0].Valor;
				Datos_obsrvaciones = response.data[0].Observaciones;
				Datos_E_Plan = response.data[0].Estado;
				Datos_Nombre_facturador = response.data[0].EmpresaFacturador;
				Datos_T_Disponibles = response.data[0].TDisponibles;
				Datos_FechaInicio = response.data[0].FechaInicio;
				Datos_DocRef = response.data[0].DocRef;
				Datos_Meses_Vence = response.data[0].MesesVence;
				Datos_TiposDoc = response.data[0].TipoDoc;

				if (Datos_TiposProceso == 1 || Datos_TiposProceso == 2 || Datos_TiposProceso == 3) {
					$("#TipoProceso").dxRadioGroup({ value: TiposProceso[BuscarID(TiposProceso, Datos_TiposProceso)] });
				}

				if (Datos_TiposDoc == 0 || Datos_TiposDoc == 1 || Datos_TiposDoc == 2) {
					$("#TipoDoc").dxRadioGroup({ value: TiposDoc[BuscarID(TiposDoc, Datos_TiposDoc)] });
				}

				//$("#txtempresaasociada").dxTextBox({ value: codigo_empresa + ' -- ' + Datos_Nombre_facturador });
				Set_Facturador(codigo_empresa + ' -- ' + Datos_Nombre_facturador);
				Bloquear_Facturador();
				$("#CantidadTransacciones").dxNumberBox({ value: Datos_T_compra });

				$("#ValorPlan").dxNumberBox({ value: Datos_valor_plan });
				if (Datos_E_Plan == 2) {
					$scope.consumido = true;
				} else {
					$("#EstadoPlan").dxRadioGroup({ value: EstadosPlanes[BuscarID(EstadosPlanes, Datos_E_Plan)] });
				}

				if (Datos_obsrvaciones != null) {
					$("#txtObservaciones").dxTextArea({ value: Datos_obsrvaciones });
				}

				$("#CantidadTransacciones").dxNumberBox({ readOnly: true });
				$("#ValorPlan").dxNumberBox({ readOnly: true });
				$('#SelecionarEmpresa').hide();
				$("#TipoProceso").dxRadioGroup({ readOnly: true });
				$("#DocRef").dxNumberBox({ value: Datos_DocRef });

				if (Datos_Meses_Vence == 0) {
					$("#Vence").dxCheckBox({ value: false });
					$('#panelfechaVencimiento').hide();
					$('#TituloFecVenc').hide();
				} else {
					$("#Vence").dxCheckBox({ value: true });
					$("#FechaVence").dxDateBox({ value: response.data[0].FechaVence });
					$("#FechaVence").dxDateBox({ visible: true });
					$('#TituloFecVenc').show();
				}

				if (response.data[0].FechaVence == null || Datos_T_Procesadas == 0) {
					$('#TituloFecVenc').hide();
					$("#FechaVence").dxDateBox({ visible: false });
				} else {
					$("#FechaVence").dxDateBox({ value: response.data[0].FechaVence });
					$("#FechaVence").dxDateBox({ visible: true });
					$('#TituloFecVenc').show();
				}

				$("#MesesVence").dxNumberBox({ value: Datos_Meses_Vence });

				$scope.Tprocesados = Datos_T_Procesadas;
				$scope.Tdisponibles = Datos_T_Disponibles;
				if (Datos_FechaInicio != undefined && Datos_FechaInicio != "") {
					$scope.FechaInicio = "Inicio: " + Datos_FechaInicio;
				} else {
					$scope.FechaInicio = "";
				}



			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 7000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
		});
	}





	$(document).ready(function () {
		$("#FechaVence").dxDateBox({ min: now });
	});

});

GestionPlanesApp.controller('ConsultaPlanesController', function ConsultaPlanesController($scope, $http, $location, SrvMaestrosEnum, SrvFiltro) {


	var now = new Date();

	var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";

	var estado = "";

	CargarSession();

	function CargarSession() {
		$http.get('/api/DatosSesion/').then(function (response) {
			codigo_facturador = response.data[0].Identificacion;
			CargarConsulta();
		}, function errorCallback(response) {

			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
		});
	}

	SrvFiltro.ObtenerFiltro('Empresa', 'Facturador', 'icon-user-tie', 115, '/api/Empresas?Facturador=true', 'Identificacion', 'RazonSocial', false).then(function (Datos) {
		$scope.Facturador = Datos;
	});

	function CargarConsulta() {
		$("#wait").show();
		$http.get('/api/PlanesTransacciones?Identificacion=' + codigo_facturador).then(function (response) {
			$("#wait").hide();
			console.log(response.data);
			try {
				$("#grid").dxDataGrid({
					dataSource: response.data,
					keyExpr: "id",
					paging: {
						pageSize: 20

					},
					pager: {
						showPageSizeSelector: true,
						allowedPageSizes: [5, 10, 20],
						showInfo: true
					},

					focusedRowEnabled: true
					, hoverStateEnabled: true,

					groupPanel: {
						allowColumnDragging: true,
						visible: true
					},
					masterDetail: {
						enabled: true,
						template: function (container, options) {
							var currentEmployeeData = options.data.Observaciones;
							container.append($('<div> <h4 class="form-control">OBSERVACIONES:</h4> <p > ' + currentEmployeeData + '</p> </div>'));
						}
					}
					, onToolbarPreparing: function (e) {
						var dataGrid = e.component;

						e.toolbarOptions.items.unshift({

							location: "after",
							widget: "dxButton",
							options: {
								icon: "refresh",
								onClick: function () {
									CargarConsulta();
								}
							}
						})
					},

					//Formatos personalizados a las columnas en este caso para el monto
					onCellPrepared: function (options) {
						var fieldData = options.value,
                            fieldHtml = "";
						try {
							if (options.columnIndex == 6) {//Columna de valor Total
								if (fieldData) {
									var inicial = fNumber.go(fieldData);
									options.cellElement.html(inicial);
								}
							}
							if (options.columnIndex == 5 || options.columnIndex == 7 || options.columnIndex == 8) {
								if (fieldData) {
									var inicial = FormatoNumber.go(fieldData);
									options.cellElement.html(inicial);
								}
							}

							if (options.data.CodigoEstado == 0) {
								estado = " style='color:green; cursor:default;' title='Habilitado'";
							}
							if (options.data.CodigoEstado == 1) {
								estado = " style='color:red; cursor:default;' title='Inhabilitado'";
							}
							if (options.data.CodigoEstado == 2) {
								estado = " style='color:grey; cursor:default;' title='Procesado'";
							}


						} catch (err) {

						}
					}
                    , loadPanel: {
                    	enabled: true
                    }
                      , allowColumnResizing: true
                 , columns: [
                     {
                     	width: 50,
                     	cellTemplate: function (container, options) {
                     		$("<div style='text-align:center'>")
								.append($("<a target='_blank' class='icon-file-eye' onClick=ConsultarDetalle('" + options.data.id + "')  ><a taget=_self style='margin-left:20%;' class='icon-pencil3' title='Editar' href='GestionPlanesTransacciones.aspx?IdSeguridad=" + options.data.id + "'>"))
								.appendTo(container);
                     	}
                     },
                     {
                     	caption: "Fecha",
                     	dataField: "Fecha",
                     	dataType: "date",
                     	format: "yyyy-MM-dd HH:mm"
                     },

                 {
                 	caption: "Doc. Facturador",
                 	dataField: "Facturador"
                 },
					 ,
                      {

                      	caption: "Empresa Compra",
                      	dataField: "EmpresaFacturador"
                      },
                     {

                     	caption: "Transacciones",
                     	dataField: "TCompra"
                     },
                      {

                      	caption: "Valor",
                      	dataField: "Valor"
                      },
                     {

                     	caption: "Procesadas",
                     	dataField: "TProcesadas"
                     }
                     ,
                     {

                     	caption: "Saldo",
                     	dataField: "Saldo"
                     }
					 //, {
                     //	caption: "Porcentaje %",
                     //	width: 100,
                     //	cellTemplate: function (container, options) {

                     //		$("<div style='text-align:center'>")
					 //   		.append($("<div class='bullet hgi_" + options.data.id + "'></div>"))
					 //   		.appendTo(container);
                     //	}

					 //}

                 , {
                 	dataField: "Porcentaje",
                 	caption: "Porcentaje %",
                 	alignment: "right",
                 	width: 100,
                 	cellTemplate: CrearGraficoBarra,
                 	cssClass: "bullet"
                 },
					 , {

					 	caption: "Empresa",
					 	dataField: "Empresa",
					 	visible: false
					 },
                     {

                     	caption: "Usuario",
                     	dataField: "Usuario",
                     	visible: false
                     }
                     ,
                     {

                     	caption: "Tipo",
                     	dataField: "Tipoproceso"
                     }
					 ,
                     {

                     	caption: "TipoDoc",
                     	dataField: "TipoDoc"
                     }
                     ,
                      {

                      	caption: 'Estado',
                      	dataField: 'Estado',
                      	cellTemplate: function (container, options) {
                      		$("<div style='text-align:center'>")
								.append($("<a taget=_self class='icon-circle2'" + estado + ">"))
								.appendTo(container);
                      	}
                      }
                 ], summary: {
                 	groupItems: [{
                 		column: "Valor",
                 		summaryType: "sum",
                 		displayFormat: " {0} Total ",
                 		valueFormat: "currency"
                 	}]

                    , totalItems: [{
                    	column: "Valor",
                    	summaryType: "sum",
                    	customizeText: function (data) {
                    		return fNumber.go(data.value).replace("$-", "-$");
                    	}
                    }, {
                    	column: "TCompra",
                    	summaryType: "sum",
                    	valueFormat: 'fixedPoint',
                    	displayFormat: '{0}'

                    }
                    ]
                 }
                    , filterRow: {
                    	visible: true
                    }
				});


			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 7000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
		});


		ConsultarDetalle = function (IdSeguridad) {
			$('#modal_detalle_plan').modal('show');
			$("#wait").show();
			$http.get('/api/PlanesTransacciones?IdSeguridad=' + IdSeguridad).then(function (response) {
				$("#wait").hide();
				$scope.Empresa = response.data[0].Empresa;
				$scope.Usuario = response.data[0].Usuario;
				$scope.Valor = response.data[0].Valor;
				$scope.TCompra = response.data[0].TCompra;
				$scope.TProcesadas = response.data[0].TProcesadas;
				$scope.TDisponibles = response.data[0].TDisponibles;
				$scope.id = response.data[0].id;
				$scope.Fecha = response.data[0].Fecha;
				$scope.CodigoEmpresaFacturador = response.data[0].CodigoEmpresaFacturador;
				$scope.EmpresaFacturador = response.data[0].EmpresaFacturador;
				$scope.Tipo = response.data[0].Tipo;
				$scope.TipoDoc = response.data[0].TipoDoc;
				$scope.Observaciones = response.data[0].Observaciones;
				$scope.Estado = response.data[0].Estado;
				$scope.FechaVence = response.data[0].FechaVence;


				if ($scope.Tipo == 3) {
					nivelPlanes(100, $scope.Tipo);
					$('.Hgi_plan').dxBullet(CrearGrafico(100, 'Consumo Actual ', ' %', nivelPlanes(porcentaje, $scope.Tipo)));
					$('#hgi_porcentaje_plan').html("100%");
				} else {
					var porcentaje = ($scope.TProcesadas.toString().replace(',', '.') / $scope.TCompra.toString().replace(',', '.')) * 100;
					nivelPlanes(porcentaje, $scope.Tipo);
					$('.Hgi_plan').dxBullet(CrearGrafico(porcentaje, 'Consumo Actual ', ' %', nivelPlanes(porcentaje, $scope.Tipo)));
					$('#hgi_porcentaje_plan').html(Number(porcentaje).toFixed(2) + "%");
				}

				///////////////////////////////////////////////////////////////////////

			}, function errorCallback(response) {
				$('#wait').hide();
				DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
			});
		}




	}

	function nivel(nivel, tipo) {
		if (tipo == 3) {
			nivel = 100;
			color = '#5cb85c'; //Verde
		} else {
			var color = '#FE2E2E';
			if (nivel >= 71 && nivel <= 90) {
				color = '#E8BE0C';
			}

			if (nivel > 90) {
				color = '#FE2E2E';
			}

			if (nivel <= 70) {
				color = '#5cb85c';
			}
		}

		$("#progressBarStatus").dxProgressBar({ value: nivel });
		$('div.dx-progressbar-range').css('background-color', color);
		$('div.dx-progressbar-range').css('border', '1px solid  ' + color);

		return color;
	}




	var discountCellTemplate = function (container, options) {

		var porcentaje = (options.data.CodCompra != 3) ? ((options.data.TProcesadas / options.data.TCompra) * 100) : 100;
		var color = nivel(porcentaje, options.data.CodCompra);

		$("<div/>").dxBullet({
			onIncidentOccurred: null,
			size: {
				width: 60,
				height: 35
			},
			margin: {
				top: 5,
				bottom: 0,
				left: 5
			},
			showTarget: false,
			showZeroLevel: true,
			value: porcentaje,
			startScaleValue: 0,
			endScaleValue: 100,
			color: color,
			tooltip: {
				enabled: true,
				font: {
					size: 18
				},
				paddingTopBottom: 2,
				customizeTooltip: function () {
					return { text: Number(porcentaje).toFixed(2) + '%' };
				},
				zIndex: 5
			}
		}).appendTo(container);
	};

	var collapsed = false;


});

//Esta funcion es para ir a la pagina de consulta
function IrAConsulta() {
	window.location.assign("../Pages/ConsultaPlanesTransacciones.aspx");
}

//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
	for (var i = 0; i < miArray.length; i += 1) {
		if (ID == miArray[i].ID) {
			return i;
		}
	}
}

var EstadosPlanes =
    [
        { ID: 1, Texto: 'Inhabilitar' },
        { ID: 0, Texto: 'Habilitar' },

    ];
