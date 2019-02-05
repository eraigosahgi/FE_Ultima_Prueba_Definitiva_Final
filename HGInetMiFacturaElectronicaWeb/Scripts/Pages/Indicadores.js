DevExpress.localization.locale('es-ES');

var IndicadoresApp = angular.module('IndicadoresApp', ['dx']);
IndicadoresApp.controller('IndicadoresController', function IndicadoresController($scope, $sce, $http, $location) {

	function Circulo() {
		$(".d3-progress-background").attr("d", "M0,38A38,38 0 1,1 0,-38A38,38 0 1,1 0,38M0,36A36,36 0 1,0 0,-36A36,36 0 1,0 0,36Z");
	}

	//Opciones Adicionales de las gráficas.
	var chartOptions = {
		gamaAzul: ["rgb(0,22,245,1)", "rgb(0,22,245,0.7)", "rgb(0,22,245,0.4)"]
	};

	var identificacion_empresa_autenticada = "";
	var usuario_autenticado = "";

	//Inicializa las fechas para el filtro de los indicadores.
	var fecha_base = new Date(),
	fecha_inicio = new Date(fecha_base.getFullYear(), fecha_base.getMonth(), 1).toISOString(),
	fecha_fin = new Date(fecha_base.getFullYear(), fecha_base.getMonth() + 1, 1).toISOString(),
	cantidad_top = 10;

	var datos_Panel13514 = "";
	var datos_Panel13516 = "";
	var datos_Panel13525 = "";
	var datos_Panel13533 = "";

	CargarIndicadores();

	//Botón de consulta de indicadores
	$("#BtnFiltroIndicadores").dxButton({
		text: "Consultar",
		type: "default",
		onClick: function () {
			CargarIndicadores();
		}
	});

	$("#FiltroCantidadTop").dxNumberBox({
		value: cantidad_top,
		width: '60%',
		showSpinButtons: true,
		onValueChanged: function (e) {
			cantidad_top = e.value;
		}
	});

	//Evento para cambio de gráfico del panel 13514
	$scope.CambiarGrafico13514 = function (tipo_grafico) {
		CargarTipoDoc13514(datos_Panel13514, tipo_grafico);
	}
	//Evento para cambio de gráfico del panel 13516
	$scope.CambiarGrafico13516 = function (tipo_grafico) {
		CargarVentas13516(datos_Panel13516, tipo_grafico);
	}
	//Evento para cambio de gráfico del panel 13525
	$scope.CambiarGrafico13525 = function (tipo_grafico) {
		CargarTiposDocs13525(datos_Panel13525, tipo_grafico)
	}
	//Evento para cambio de gráfico del panel 13533
	$scope.CambiarGrafico13533 = function (tipo_grafico) {
		CargarTiposDocs13533(datos_Panel13533, tipo_grafico);
	}


	//Controla el tipo de filtro para la búsqueda por fechas.
	var RadioGroup = $("#OpcionesFiltroFechas").dxRadioGroup({
		dataSource: TiposFiltro,
		displayExpr: "Descripcion",
		valueExpr: "Codigo",
		layout: "horizontal",
		onValueChanged: function (e) {

			$("#VerFiltroFin").hide();
			$("#VerFiltroInicio").show();

			//Según la opción seleccionada, se realiza la carga del control de selección.
			switch (e.value) {

				//Día actual
				case "1":
					fecha_inicio = new Date(fecha_base).toISOString();


					var set_date = new Date(fecha_base);
					set_date.setDate(set_date.getDate() + 1);
					fecha_fin = set_date.toISOString();

					$("#VerFiltroInicio").hide();
					break;

					//Fecha Especifica.
				case "2":
					fecha_inicio = new Date(fecha_base).toISOString();

					var set_date = new Date(fecha_inicio);
					set_date.setDate(set_date.getDate() + 1);
					fecha_fin = set_date.toISOString();

					$("#FiltroFechaInicio").dxDateBox({
						value: fecha_base,
						width: '80%',
						displayFormat: "yyyy-MM-dd",
						maxZoomLevel: 'month',
						minZoomLevel: 'century',
						min: undefined,
						max: undefined,
						disabled: false,
						onValueChanged: function (data) {
							fecha_inicio = new Date(data.value).toISOString();

							var set_date = new Date(fecha_inicio);
							set_date.setDate(set_date.getDate() + 1);
							fecha_fin = set_date.toISOString();
						}
					});

					break;

					//Rango por mes
				case "3":
					fecha_inicio = new Date(fecha_base.getFullYear(), fecha_base.getMonth(), 1).toISOString();
					fecha_fin = new Date(fecha_base.getFullYear(), fecha_base.getMonth() + 1, 0).toISOString();

					$("#FiltroFechaInicio").dxDateBox({
						value: fecha_base,
						getCellTemplate: "cell",
						width: '80%',
						displayFormat: 'monthAndYear',
						maxZoomLevel: 'year',
						minZoomLevel: 'century',
						min: undefined,
						max: undefined,
						disabled: false,
						onValueChanged: function (data) {
							fecha_inicio = new Date(data.value.getFullYear(), data.value.getMonth(), 1).toISOString();
							fecha_fin = new Date(data.value.getFullYear(), data.value.getMonth() + 1, 1).toISOString();
						}
					});

					break;

					//Rango inicio-fin
				case "4":
					$("#VerFiltroFin").show();

					fecha_inicio = new Date(fecha_base).toISOString();

					var set_date = new Date(fecha_base);
					set_date.setDate(set_date.getDate() + 1);
					fecha_fin = set_date.toISOString();

					//Filtro de fecha inicial del rango
					$("#FiltroFechaInicio").dxDateBox({
						value: fecha_base,
						width: '80%',
						displayFormat: "yyyy-MM-dd",
						maxZoomLevel: 'month',
						minZoomLevel: 'century',
						disabled: false,
						onValueChanged: function (data) {
							fecha_inicio = new Date(data.value).toISOString();
							$("#FiltroFechaFin").dxDateBox({ min: fecha_inicio });
						}
					});

					//Filtro de fecha final del rango
					$("#FiltroFechaFin").dxDateBox({
						value: fecha_base,
						width: '80%',
						displayFormat: "yyyy-MM-dd",
						maxZoomLevel: 'month',
						minZoomLevel: 'century',
						disabled: false,
						onValueChanged: function (data) {
							var set_date = new Date(data.value);
							set_date.setDate(set_date.getDate() + 1);
							fecha_fin = set_date.toISOString();
							$("#FiltroFechaInicio").dxDateBox({ max: fecha_fin });
						}
					});

					break;


					//Selección de año.
				case "5":
					fecha_inicio = new Date(fecha_base.getFullYear(), 0, 1).toISOString();
					fecha_fin = new Date(fecha_base.getFullYear() + 1, 0, 1).toISOString();

					$("#FiltroFechaInicio").dxDateBox({
						value: fecha_base,
						width: '80%',
						displayFormat: 'Year',
						maxZoomLevel: 'decade',
						minZoomLevel: 'century',
						min: undefined,
						max: undefined,
						disabled: false,
						onValueChanged: function (data) {
							fecha_inicio = new Date(data.value.getFullYear(), 0, 1).toISOString();
							fecha_fin = new Date(data.value.getFullYear() + 1, 0, 1).toISOString();
						}
					});

					break;

					/*case "6":
						fecha_inicio = fecha_base.toISOString();
	
						var set_date = new Date(fecha_base);
						set_date.setDate(set_date.getDate() + 6);
						fecha_fin = set_date.toISOString();
	
						$("#FiltroFechaInicio").dxDateBox({
							value: fecha_base,
							width: '80%',
							displayFormat: "yyyy-MM-dd",
							maxZoomLevel: 'month',
							minZoomLevel: 'century',
							min: fecha_inicio,
							max: fecha_fin,
							disabled: false,
							onValueChanged: function (data) {
	
								fecha_inicio = data.value.toISOString();
	
								var set_date = new Date(data.value);
								set_date.setDate(set_date.getDate() + 6);
								fecha_fin = set_date.toISOString();
	
								$("#FiltroFechaInicio").dxDateBox({ min: data.value });
								$("#FiltroFechaInicio").dxDateBox({ max: fecha_fin });
	
								console.log();
	
							}
						});
						
						break;*/

			}
		}
	}).dxRadioGroup("instance");

	RadioGroup.option("value", TiposFiltro[2].Codigo);

	//Realiza la consulta de indicadores según los rangos de fecha
	function CargarIndicadores() {

		$http.get('/api/SesionDatosUsuario/').then(function (response) {

			identificacion_empresa_autenticada = response.data[0].IdentificacionEmpresa;
			usuario_autenticado = response.data[0].Usuario;

			$http.get('/api/DatosSesion/').then(function (response) {

				if (response.data[0].Administrador)
					PerfilAdministrador();

				if (response.data[0].Obligado)
					PerfilFacturador();

				if (response.data[0].Adquiriente)
					PerfilAdquiriente();

				if (response.data[0].Administrador) {
					$('#LiTabAdministrador').addClass('active');
					$('#TabAdministrador').addClass('active');
				}
				else if (!response.data[0].Administrador && response.data[0].Obligado) {
					$('#LiTabFacturador').addClass('active');
					$('#TabFacturador').addClass('active');
				}

				else if (!response.data[0].Administrador && !response.data[0].Obligado) {
					$('#LiTabAdquiriente').addClass('active');
					$('#TabAdquiriente').addClass('active');
				}

			}), function errorCallback(response) {
				Mensaje(response.data.ExceptionMessage, "error");
			};


		}), function errorCallback(response) {
			Mensaje(response.data.ExceptionMessage, "error");
		};


		function PerfilAdministrador() {

			$scope.LinkTabAdministrador = true;

			//PermisosIndicadores string codigo_usuario, string identificacion_empresa, int tipo_perfil
			$http.get('/api/PermisosIndicadores?codigo_usuario=' + usuario_autenticado + '&identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_perfil=' + 1).then(function (response) {

				var opciones_permisos = response.data;
				if (opciones_permisos.length < 1) {
					$scope.IndicadoresAdmin = false;
				} else {
					$scope.IndicadoresAdmin = true;
				}
				for (var i = 0; i < opciones_permisos.length; i++) {
					var cod_div = 'Panel' + opciones_permisos[i].Codigo;
					$scope[cod_div] = opciones_permisos[i].Consultar;
				}

				/*************************************************************************** ADMINISTRADOR ***************************************************************************/

				//REPORTE DOCUMENTOS POR ESTADO ADMINISTRADOR
				$http.get('/Api/ReporteDocumentosPorEstadoCategoria?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {
						if (response.data.length == 0)
							$scope.Panel13519 = false;
						else {
							$scope.ReporteDocumentosEstadoCategoriaAdmin = response.data;
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

				//REPORTE DOCUMENTOS POR ESTADO ADMINISTRADOR
				$http.get('/Api/ReporteDocumentosPorEstado?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {
						if (response.data.length == 0)
							$scope.Panel13511 = false;
						else {
							$scope.ReporteDocumentosEstadoAdmin = response.data;
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}

				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

				//REPORTE ACUSE MENSUAL ADMINISTRADOR
				$http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {

						if (response.data.length == 0)
							$scope.Panel13512 = false;
						else {
							$scope.ReporteAcuseMensualAdmin = response.data;
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

				//REPORTE TIPO DOCUMENTO ACUMULADO ADMINISTRADOR
				$http.get('/Api/ReporteDocumentosPorTipo?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {
						if (response.data.length == 0)
							$scope.Panel13515 = false;
						else {
							$scope.ReporteDocumentosTipoAdmin = response.data;
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

				//REPORTE TIPO DOCUMENTO ANUAL ADMINISTRADOR
				ConsultarTiposDocumentosAdmin("Bar");

				//REPORTE VENTAS ANUALES ADMINISTRADOR
				ConsultarVentasAdmin("Bar");

				//REPORTE TOP COMPRADORES ADMINISTRADOR
				$http.get('/Api/ReporteTopCompradores?cantidad_top=' + cantidad_top + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					if (response.data.Detalles.length == 0)
						$scope.Panel13517 = false;
					else {

						$scope.TotalTopCompradores = response.data.TotalTop;
						$scope.TotalOtrosCompradores = response.data.TotalOtros;
						$scope.TotalCompradores = response.data.Total;

						$("#ToolTipPanel13517").dxTooltip({
							target: "#InfoPanel13517",
							showEvent: "mouseenter",
							hideEvent: "mouseleave",
							position: "right",
							contentTemplate: function (data) {
								data.html("<label>El top es aplicado sobre el filtro de fecha seleccionado. </br> Para el calculo Total, no se tienen en cuenta los filtros aplicados a la búsqueda.</label>");
							}
						});

						try {
							$("#ReporteTopCompradores").dxDataGrid({
								dataSource: response.data.Detalles,
								paging: {
									pageSize: 10
								},
								pager: {
									showPageSizeSelector: true,
									allowedPageSizes: [10, 20, 30],
									showInfo: true
								},
								showRowLines: true,
								showBorders: true,
								showColumnLines: false,
								loadPanel: {
									enabled: true
								},
								scrolling: {
									mode: "virtual"
								},
								sorting: {
									mode: "none"
								},
								onCellPrepared: function (options) {
									var fieldData = options.value,
										fieldHtml = "";
									try {
										if (options.columnIndex == 2) {
											if (fieldData) {
												var inicial = FormatoNumber.go(fieldData);
												options.cellElement.html(inicial);
											}
										}
										if (options.columnIndex == 3) {
											if (fieldData) {
												var inicial = fNumber.go(fieldData);
												options.cellElement.html(inicial);
											}
										}
									} catch (err) {
										DevExpress.ui.notify(err.message, 'error', 3000);
									}
								},
								onContentReady: function (e) {
									e.component.option("loadPanel.enabled", false);
								}, columns: [
									/*{
										caption: "",
										cellTemplate: function (container, options) {
			
											if (options.data.Posicion) {
			
												var opacidad = 0;
												switch (options.data.Posicion) {
													case 1:
														opacidad = "1"
														break;
													case 2:
														opacidad = "0.9"
														break;
													case 3:
														opacidad = "0.8"
														break;
													case 4:
														opacidad = "0.7"
														break;
													case 5:
														opacidad = "0.6"
														break;
													case 6:
														opacidad = "0.5"
														break;
													case 7:
														opacidad = "0.4"
														break;
													case 8:
														opacidad = "0.3"
														break;
													case 9:
														opacidad = "0.2"
														break;
													case 10:
														opacidad = "0.1"
														break;
												}
			
												var color = "style='margin-left:5%; color: rgba(0, 22, 245, " + opacidad + ")'";
											}
			
											$("<div>")
												.append($("<label class='icon-coin-dollar' " + color + " />"))
												.appendTo(container);
										}
									},*/
									{
										caption: "Identificación",
										dataField: "Identificacion",
									},
									{
										caption: "Razón Social",
										dataField: "RazonSocial",
									},
									{
										caption: "Cantidad Transacciones",
										dataField: "CantidadTransacciones",
									},
									{
										caption: "Valor Compras",
										dataField: "ValorCompras",
									}
								]
							});
						} catch (err) {
							DevExpress.ui.notify(err.message, 'error', 3000);
						}
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

				//REPORTE TOP TRANSACCIONAL ADMINISTRADOR
				$http.get('/Api/ReporteTopTransaccional?cantidad_top=' + cantidad_top + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					if (response.data.Detalles.length == 0)
						$scope.Panel13518 = false;
					else {
						try {
							$scope.TotalTopMovimiento = response.data.TotalTop;
							$scope.TotalOtrosMovimiento = response.data.TotalOtros;
							$scope.TotalMovimiento = response.data.Total;

							$("#ToolTipPanel13518").dxTooltip({
								target: "#InfoPanel13518",
								showEvent: "mouseenter",
								hideEvent: "mouseleave",
								position: "right",
								contentTemplate: function (data) {
									data.html("<label>El top es aplicado sobre el filtro de fecha seleccionado. </br> Para el calculo Total, no se tienen en cuenta los filtros aplicados a la búsqueda.</label>");
								}
							});

							$("#ReporteTopMovimiento").dxDataGrid({
								dataSource: response.data.Detalles,
								paging: {
									pageSize: 10
								},
								pager: {
									showPageSizeSelector: true,
									allowedPageSizes: [10, 20, 30],
									showInfo: true
								},
								showRowLines: true,
								showBorders: true,
								showColumnLines: false,
								customizeColumns: function (columns) {
									columns[0].width = 70;
								},
								loadPanel: {
									enabled: true
								},
								scrolling: {
									mode: "virtual"
								},
								sorting: {
									mode: "none"
								},
								onCellPrepared: function (options) {
									var fieldData = options.value,
										fieldHtml = "";
									try {
										if (options.columnIndex == 2) {
											if (fieldData) {
												var inicial = FormatoNumber.go(fieldData);
												options.cellElement.html(inicial);
											}
										}
										if (options.columnIndex == 3) {
											if (fieldData) {
												var inicial = fNumber.go(fieldData);
												options.cellElement.html(inicial);
											}
										}
									} catch (err) {
										DevExpress.ui.notify(err.message, 'error', 3000);
									}
								},
								onContentReady: function (e) {
									e.component.option("loadPanel.enabled", false);
								}, columns: [
									/*{
										caption: "",
										cellTemplate: function (container, options) {
			
											var indicador = "";
			
										//Decreciente
											if (options.data.CantidadMesActual < options.data.CantidadMesAnterior)
												indicador = "class='icon-stats-decline' style='margin-left:5%; color:#E70000 '";
												//Creciente
											else if (options.data.CantidadMesActual > options.data.CantidadMesAnterior)
												indicador = "class='icon-stats-growth' style='margin-left:5%; color:#62C415'";
												//Estable
											else if (options.data.CantidadMesActual == options.data.CantidadMesAnterior)
												indicador = "class='icon-sort' style='margin-left:5%; color: #2196f3'";
												
											$("<div>")
												.append($("<label " + indicador + " />"))
												.appendTo(container);
										}
									},*/
									{
										caption: "Identificación",
										dataField: "Identificacion",
										cssClass: "col-md-3",
									},
									{
										caption: "Razón Social",
										dataField: "RazonSocial",
										cssClass: "col-md-3",
									},
									{
										caption: "Total Documentos",
										dataField: "TotalDocumentos",
										cssClass: "col-md-3",
									},
									{
										caption: "Valor Documentos",
										dataField: "ValorTotalDocumentos",
										cssClass: "col-md-3",
									}
									/*{
										caption: "Mes Anterior",
										dataField: "CantidadMesAnterior",
									},
									{
										caption: "Mes Actual",
										dataField: "CantidadMesActual",
									}*/

								]
							});
						} catch (err) {
							DevExpress.ui.notify(err.message, 'error', 3000);
						}
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});


			}), function errorCallback(response) {
				Mensaje(response.data.ExceptionMessage, "error");
			};

		}

		function PerfilFacturador() {

			$scope.LinkTabFacturador = true;

			//PermisosIndicadores string codigo_usuario, string identificacion_empresa
			$http.get('/api/PermisosIndicadores?codigo_usuario=' + usuario_autenticado + '&identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_perfil=' + 2).then(function (response) {

				var opciones_permisos = response.data;
				if (opciones_permisos.length < 1) {
					$scope.IndicadoresFacturador = false;
				} else {
					$scope.IndicadoresFacturador = true;
				}
				for (var i = 0; i < opciones_permisos.length; i++) {
					var cod_div = 'Panel' + opciones_permisos[i].Codigo;
					$scope[cod_div] = opciones_permisos[i].Consultar;
				}

				/*************************************************************************** FACTURADOR ***************************************************************************/

				//REPORTE DOCUMENTOS POR ESTADO FACTURADOR
				$http.get('/Api/ReporteDocumentosPorEstadoCategoria?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {
						if (response.data.length == 0)
							$scope.Panel13527 = false;
						else {
							$scope.ReporteDocumentosEstadoCategoriaFacturador = response.data;
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

				/*
				//REPORTE DOCUMENTOS POR ESTADO FACTURADOR
				$http.get('/Api/ReporteDocumentosPorEstado?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2').then(function (response) {
					$("#wait").hide();
					try {
						if (response.data.length == 0)
							$scope.Panel13521 = false;
						else {
							$scope.ReporteDocumentosEstadoFacturador = response.data;
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});
				*/

				//REPORTE ACUSE MENSUAL FACTURADOR
				$http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {
						if (response.data.length == 0)
							$scope.Panel13523 = false;
						else {
							$scope.ReporteAcuseMensualFacturador = response.data;
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

				//REPORTE TIPO DOCUMENTO ACUMULADO FACTURADOR
				$http.get('/Api/ReporteDocumentosPorTipo?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {
						if (response.data.length == 0)
							$scope.Panel13526 = false;
						else {
							$scope.ReporteDocumentosTipoFacturador = response.data;
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

				//REPORTE TIPO DOCUMENTO ANUAL FACTURADOR
				ConsultarTiposDocFacturador("bar");

				//REPORTE RESUMEN TRANSACCIONAL
				$http.get('/Api/ReporteResumenPlanesAdquiridos?identificacion_empresa=' + identificacion_empresa_autenticada + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {
						if (response.data.length == 0)
							$scope.Panel13522 = false;
						else {
							$scope.TransaccionesAdquiridas = response.data[0].TransaccionesAdquiridas;
							$scope.TransaccionesProcesadas = response.data[0].TransaccionesProcesadas;
							$scope.TransaccionesDisponibles = response.data[0].TransaccionesDisponibles;
							$("#ResumenPlanesAdquiridosFacturador").dxDataGrid({
								dataSource: response.data[0].PlanesAdquiridos,
								showBorders: false,
								showRowLines: false,
								showColumnLines: false,
								rowAlternationEnabled: true,
								noDataText: "No se encontraron Planes Transaccionales Registrados.",
								paging: {
									pageSize: 5
								},
								pager: {
									showInfo: true
								},
								columns: [
									{
										caption: "Fecha Compra",
										dataField: "DatFecha",
										dataType: "date",
										format: "yyyy-MM-dd",
									},
									{
										caption: "Transacciones Plan",
										dataField: "IntNumTransaccCompra",
									},
									{
										caption: "Transacciones Procesadas",
										dataField: "IntNumTransaccProcesadas",
									},
									{
										caption: "Fecha Vencimiento",
										dataField: "DatFechaVencimiento",
										dataType: "date",
										format: "yyyy-MM-dd",
									}
								]
							});
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

			}), function errorCallback(response) {
				Mensaje(response.data.ExceptionMessage, "error");
			};
		}

		function PerfilAdquiriente() {

			$scope.LinkTabAdquiriente = true;

			//PermisosIndicadores string codigo_usuario, string identificacion_empresa
			$http.get('/api/PermisosIndicadores?codigo_usuario=' + usuario_autenticado + '&identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_perfil=' + 3).then(function (response) {

				var opciones_permisos = response.data;
				if (opciones_permisos.length < 1) {
					$scope.IndicadoresAdquiriente = false;
				} else {
					$scope.IndicadoresAdquiriente = true;
				}
				for (var i = 0; i < opciones_permisos.length; i++) {
					var cod_div = 'Panel' + opciones_permisos[i].Codigo;
					$scope[cod_div] = opciones_permisos[i].Consultar;
				}

				/*************************************************************************** ADQUIRIENTE ***************************************************************************/
				//REPORTE ACUSE ACUMULADO ADQUIRIENTE
				$http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '3' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {

						if (response.data.length == 0)
							$scope.Panel13531 = false;
						else {
							$scope.ReporteAcuseAcumuladoAdquiriente = response.data;
						}
					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

				//REPORTE TIPO DOCUMENTO ANUAL ADQUIRIENTE
				ConsultarTiposDocAdquiriente("bar");

				//REPORTE TIPO DOCUMENTO ACUMULADO ADQUIRIENTE
				$http.get('/Api/ReporteDocumentosPorTipo?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '3' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
					$("#wait").hide();
					try {
						if (response.data.length == 0)
							$scope.Panel13532 = false;
						else {
							$scope.ReporteDocumentosTipoAdquiriente = response.data;
						}

					} catch (err) {
						DevExpress.ui.notify(err.message, 'error', 3000);
					}
				}, function errorCallback(response) {
					$('#wait').hide();
					DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
				});

			}), function errorCallback(response) {
				Mensaje(response.data.ExceptionMessage, "error");
			};
		}


		//d="M0,38A38,38 0 1,1 0,-38A38,38 0 1,1 0,38M0,36A36,36 0 1,0 0,-36A36,36 0 1,0 0,36Z"
		/*************************************************************************** CARGA PORCENTAJES ADMINISTRADOR ***************************************************************************/

		$scope.CargarDocumentosEstadoCategoriaAdmin = function () {
			var Indicador = $scope.ReporteDocumentosEstadoCategoriaAdmin
			$('#totaldocestado').text("Documentos por Estado: " + Indicador[0].Cantidad);
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}

		$scope.CargarDocumentosEstadoAdmin = function () {
			var Indicador = $scope.ReporteDocumentosEstadoAdmin
			$('#totaldocproceso').text("Documentos por Proceso: " + Indicador[0].Cantidad);
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}

		$scope.CargarAcuseMensualAdmin = function () {
			var Indicador = $scope.ReporteAcuseMensualAdmin
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}

		$scope.CargarDocumentosTipoAdmin = function () {
			var Indicador = $scope.ReporteDocumentosTipoAdmin
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}

		/*************************************************************************** CARGA PORCENTAJES FACTURADOR ***************************************************************************/

		$scope.CargarDocumentosEstadoCategoriaFacturador = function () {
			var Indicador = $scope.ReporteDocumentosEstadoCategoriaFacturador
			$('#totaldocestadoFacturador').text("Documentos por Estado: " + Indicador[0].Cantidad);
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}

		/*
		$scope.CargarDocumentosEstadoFacturador = function () {
			var Indicador = $scope.ReporteDocumentosEstadoFacturador
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}
		*/

		$scope.CargarAcuseMensualFacturador = function () {
			var Indicador = $scope.ReporteAcuseMensualFacturador
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}

		$scope.CargarDocumentosTipoFacturador = function () {
			var Indicador = $scope.ReporteDocumentosTipoFacturador
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}

		/*************************************************************************** CARGA PORCENTAJES ADQUIRIENTE ***************************************************************************/

		$scope.CargarAcuseAcumuladoAdquiriente = function () {
			var Indicador = $scope.ReporteAcuseAcumuladoAdquiriente
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}

		$scope.CargarDocumentosTipoAdquiriente = function () {
			var Indicador = $scope.ReporteDocumentosTipoAdquiriente
			for (var i = 0; i < Indicador.length; i++) {
				PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
			}
		}

		setTimeout(Circulo, 4000);
	};


	//Consulta los datos para el indicador 13514
	function ConsultarTiposDocumentosAdmin(tipo_grafico) {
		$http.get('/Api/ReporteDocumentosPorTipoAnual?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
			$("#wait").hide();
			try {
				if (response.data.length == 0)
					$scope.Panel13514 = false;
				else {
					$scope.Panel13514 = true;
					datos_Panel13514 = response;
					CargarTipoDoc13514(response, tipo_grafico);
				}

			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}
	//Carga la gráfica del panel 13514
	function CargarTipoDoc13514(response, tipo_grafico) {
		$("#ReporteTipoDocumentoAnualAdmin").dxChart({
			palette: chartOptions.gamaAzul,
			dataSource: response.data,
			tooltip: {
				enabled: true,
				format: "largeNumber",
				customizeTooltip: function (arg) {

					var datos = response.data.filter(x => x.DescripcionMes == arg.argument);

					var mensaje = "";

					if (datos.length > 0) {
						mensaje = "Mes: " + arg.argument + "<br/>" + "Año: " + datos[0].Anyo + "<br/>" + "Cant.Documentos: " + datos[0].CantidadDocumentos + "<br/>" + "ValorFacturas: " +
						fNumber.go(datos[0].ValorFacturas) + "<br/>" + "ValorNotasCredito: " + fNumber.go(datos[0].ValorNotasCredito) + "<br/>" + "ValorNotasDebito: " + fNumber.go(datos[0].ValorNotasDebito);
					}
					return {
						text: mensaje
					};
				}
			},
			commonSeriesSettings: {
				ignoreEmptyPoints: true,
				argumentField: "DescripcionMes",
				type: tipo_grafico
			},
			series: [
				 {
				 	valueField: "CantidadDocumentos",
				 	name: "Cant.Documentos"
				 }
			],
			legend: {
				verticalAlignment: "bottom",
				horizontalAlignment: "center"
			},
			title: ""
		});
	}

	//Consulta los datos para el indicador 13516
	function ConsultarVentasAdmin(tipo_grafico) {
		$http.get('/Api/ReporteVentasAnuales?fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
			$("#wait").hide();
			try {
				if (response.data.length == 0)
					$scope.Panel13516 = false;
				else {
					$scope.Panel13516 = true;
					datos_Panel13516 = response;
					CargarVentas13516(response, tipo_grafico);
				}
			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}
	//Carga la gráfica del panel 13516
	function CargarVentas13516(response, tipo_grafico) {
		$("#ReporteVentas").dxChart({
			palette: chartOptions.gamaAzul,
			dataSource: response.data,
			tooltip: {
				enabled: true,
				format: "largeNumber",
				customizeTooltip: function (arg) {

					var datos_reporte = response.data.filter(x => x.DescripcionMes == arg.argument);

					var mensaje = "";

					if (datos_reporte.length > 0) {

						if (arg.seriesName == "Cortesias")
							mensaje = "Cortesías" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos_reporte[0].CantidadTransaccionesCortesias;
						else if (arg.seriesName == "Ventas")
							mensaje = "Ventas" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos_reporte[0].CantidadTransaccionesVentas + "<br/>" + "Valor: " + fNumber.go(datos_reporte[0].ValorVentas);
						else if (arg.seriesName == "Post-Venta")
							mensaje = "Post-Venta" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos_reporte[0].CantidadTransaccionesPostVenta;
					}
					return {
						text: mensaje
					};
				}
			},
			commonSeriesSettings: {
				ignoreEmptyPoints: true,
				argumentField: "DescripcionMes",
				type: tipo_grafico
			},
			series: [
				{ valueField: "CantidadTransaccionesVentas", name: "Ventas" },
				{ valueField: "CantidadTransaccionesCortesias", name: "Cortesias" },
				{ valueField: "CantidadTransaccionesPostVenta", name: "Post-Venta" }
			],
			legend: {
				verticalAlignment: "bottom",
				horizontalAlignment: "center"
			},
			title: "",
		});
	}

	//Consulta los datos para el indicador 13525
	function ConsultarTiposDocFacturador(tipo_grafico) {
		$http.get('/Api/ReporteDocumentosPorTipoAnual?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
			$("#wait").hide();
			try {
				if (response.data.length == 0)
					$scope.Panel13525 = false;
				else {
					$scope.Panel13525 = true;
					datos_Panel13525 = response;
					CargarTiposDocs13525(response, tipo_grafico);
				}
			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});

	}
	//Carga la gráfica del panel 13525
	function CargarTiposDocs13525(response, tipo_grafico) {
		$("#ReporteTipoDocumentoAnualFacturador").dxChart({
			palette: chartOptions.gamaAzul,
			dataSource: response.data,
			tooltip: {
				enabled: true,
				format: "largeNumber",
				customizeTooltip: function (arg) {

					var datos = response.data.filter(x => x.DescripcionMes == arg.argument);

					var mensaje = "";

					if (datos.length > 0) {
						mensaje = "Mes: " + arg.argument + "<br/>" + "Año: " + datos[0].Anyo + "<br/>" + "Cant.Documentos: " + datos[0].CantidadDocumentos + "<br/>" + "ValorFacturas: " +
						fNumber.go(datos[0].ValorFacturas) + "<br/>" + "ValorNotasCredito: " + fNumber.go(datos[0].ValorNotasCredito) + "<br/>" + "ValorNotasDebito: " + fNumber.go(datos[0].ValorNotasDebito);
					}
					return {
						text: mensaje
					};
				}
			},
			commonSeriesSettings: {
				ignoreEmptyPoints: true,
				argumentField: "DescripcionMes",
				type: tipo_grafico
			},
			series: [
				 {
				 	valueField: "CantidadDocumentos",
				 	name: "Cant.Documentos"
				 }
			],
			legend: {
				verticalAlignment: "bottom",
				horizontalAlignment: "center"
			},
			title: ""
		});
	}

	//Consulta los datos para el indicador 13533
	function ConsultarTiposDocAdquiriente(tipo_grafico) {
		$http.get('/Api/ReporteDocumentosPorTipoAnual?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '3' + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
			$("#wait").hide();
			try {
				if (response.data.length == 0)
					$scope.Panel13533 = false;
				else {
					$scope.Panel13533 = true;
					datos_Panel13533 = response;
					CargarTiposDocs13533(response, tipo_grafico);
				}
			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}
	//Carga la gráfica del panel 13533
	function CargarTiposDocs13533(response, tipo_grafico) {
		$("#ReporteTipoDocumentoAnualAdquiriente").dxChart({
			palette: chartOptions.gamaAzul,
			dataSource: response.data,
			tooltip: {
				enabled: true,
				format: "largeNumber",
				customizeTooltip: function (arg) {

					var datos = response.data.filter(x => x.DescripcionMes == arg.argument);

					var mensaje = "";

					if (datos.length > 0) {
						mensaje = "Mes: " + arg.argument + "<br/>" + "Año: " + datos[0].Anyo + "<br/>" + "Cant.Documentos: " + datos[0].CantidadDocumentos + "<br/>" + "ValorFacturas: " +
						fNumber.go(datos[0].ValorFacturas) + "<br/>" + "ValorNotasCredito: " + fNumber.go(datos[0].ValorNotasCredito) + "<br/>" + "ValorNotasDebito: " + fNumber.go(datos[0].ValorNotasDebito);
					}
					return {
						text: mensaje
					};
				}
			},
			commonSeriesSettings: {
				ignoreEmptyPoints: true,
				argumentField: "DescripcionMes",
				type: tipo_grafico
			},
			series: [
				 {
				 	valueField: "CantidadDocumentos",
				 	name: "Cant.Documentos"
				 }
			],
			legend: {
				verticalAlignment: "bottom",
				horizontalAlignment: "center"
			},
			title: ""
		});
	}
});


TiposFiltro = [

 {
 	Codigo: "1",
 	Descripcion: "Hoy"
 },
 {
 	Codigo: "2",
 	Descripcion: "Fecha"
 },
{
	Codigo: "3",
	Descripcion: "Mes"
},
{
	Codigo: "4",
	Descripcion: "Rango"
},
{
	Codigo: "5",
	Descripcion: "Año"
},
];
