DevExpress.localization.locale(navigator.language);
var Lista_Resoluciones;
var Datos_Resolucion = "*";



var App = angular.module('App', ['dx', 'AppSrvResoluciones', 'AppMaestrosEnum', 'AppSrvFiltro', 'AppSrvEmpresas']);
App.controller('ConsultaResolucionesController', function ConsultaResolucionesController($scope, $http, $rootScope, $location, SrvResoluciones, SrvMaestrosEnum, SrvFiltro, SrvEmpresas) {

	$scope.MuestraFiltros = false;
	$scope.FiltroResolucion = false;
	$scope.MuestraActualizarGrid = false;


	SrvMaestrosEnum.ObtenerSesion().then(function (data) {
		if (data[0].admin || data[0].Integrador) {
			$scope.MuestraFiltros = true;
			//Creamos el botón Consultar Con la DIAN
			$scope.ButtonActualizar = {
				icon: "refresh",
				onClick: function (e) {
					SrvResoluciones.ActualizarConDIAN(txt_hgi_Facturador).then(function (data) {
						consultar(txt_hgi_Facturador);
					});
				}
			};
		} else {
			$scope.MuestraActualizarGrid = true;
		}

		consultar(data[0].Identificacion);
		txt_hgi_Facturador = data[0].Identificacion;
		//Obtenemos los facturadores para el filtro
		SrvEmpresas.ObtenerEmpresas(data[0].Identificacion, 1, 10000, 0).then(function (data) {
			Datos_hgi_Facturador = data;
			$('#grid_Facturador').dxDataGrid({ dataSource: data });
		});

	});

	//Aqui creamos el control generico de empresas para poder filtrar
	try {
		SrvFiltro.ObtenerFiltro2('Facturador', 'Facturador', 'icon-user-tie', 115, '', 'Identificacion', 'RazonSocial', false, 14, "BuscarResoluciones();").then(function (Datos) {
			//SrvFiltro.ObtenerFiltro('Facturador', 'Facturador', 'icon-user-tie', 115, '', 'Identificacion', 'RazonSocial', false, 14).then(function (Datos) {
			$scope.Facturador = Datos;
		});
	} catch (e) {
		//cuando el usuario no es administrador esta consulta no es necesario
	}

	//Aqui cargamos la resoluciones del facturador
	function cargarFiltro(Facturador) {
		//Obtiene las resoluciones asociadas para cargarlas en el filtro
		SrvResoluciones.ObtenerAsociadas(Facturador).then(function (data) {
			cargarResolucion(JSON.parse(JSON.stringify(data)));
		});
	}

	//Aqui creamos el control de resoluciones ya con los datos
	function cargarResolucion(Lista) {
		$("#Listaresolucion").dxSelectBox({
			placeholder: "seleccione el código de resolución",
			displayExpr: "Descripcion",
			dataSource: Lista,
			onValueChanged: function (data) {
				Datos_Resolucion = data.value.ID;
			}
		});
	}
	//Creamos el botón Consultar
	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultar(txt_hgi_Facturador);
		}
	};




	//Función consultar
	function consultar(Facturador) {

		//Aqui obtenemos la lista de resoluciones
		$('#wait').hide();
		SrvResoluciones.Obtener(Facturador, Datos_Resolucion).then(function (data) {

			Lista_Resoluciones = data;

			$("#gridResoluciones").dxDataGrid({
				dataSource: Lista_Resoluciones,
				columnHidingEnabled: true,
				paging: {
					pageSize: 20
				},
				keyExpr: "IdSeguridad",
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
					mode: "select"
				},
				groupPanel: {
					allowColumnDragging: true,
					visible: true
				},
				onToolbarPreparing: function (e) {
					var dataGrid = e.component;

					e.toolbarOptions.items.unshift({

						location: "after",
						widget: "dxButton",
						options: {
							icon: "refresh",
							visible: $scope.MuestraActualizarGrid,
							onClick: function () {
								SrvResoluciones.ActualizarConDIAN(txt_hgi_Facturador).then(function (data) {
									consultar(txt_hgi_Facturador);
								});
							}
						}
					})
				}
				, columns: [{

					cssClass: "col-md-1 col-xs-2",
					cellTemplate: function (container, options) {
						var EditarResolucion = "";
						//La empresa debe manejar pagos y tener serial de cloudservices para poder configurar un comercio
						if (options.data.SerialCloud) {
							EditarResolucion = "<a class='icon-pencil3' data-toggle='modal' data-target='#modal_Resoluciones' style='margin-left:12%; font-size:19px'></a>";
						}

						$("<div style='text-align:center'>")

						 .append(

						 $(EditarResolucion).dxButton({
						 	onClick: function () {
						 		//linea 149
						 		var Datos_Pagos_Parciales = false;
						 		var Datos_Id_Comercio = "";
						 		var Datos_Descripcion_Comercio = "";


						 		//var Datos_Id_ComercioTC = "";
						 		//var Datos_Descripcion_ComercioTC = "";


						 		//Servicio para obtener la lista de comercios en plataforma intermedia
						 		SrvResoluciones.ObtenerComercios(options.data.Empresa, options.data.SerialCloud).then(function (data) {
						 			var Datos = data;

						 			Datos_Id_Comercio = (options.data.ComercioConfigId == null) ? "" : options.data.ComercioConfigId;
						 			Datos_Descripcion_Comercio = (options.data.ComercioConfigDescrip == null) ? "" : options.data.ComercioConfigDescrip;
						 			//Datos_Id_ComercioTC = (options.data.ComercioConfigIdTC == null) ? "" : options.data.ComercioConfigIdTC;
						 			//Datos_Descripcion_ComercioTC = (options.data.ComercioConfigDescripTC == null) ? "" : options.data.ComercioConfigDescripTC;
						 			Datos_Pagos_Parciales = (options.data.PermiteParciales == 1) ? true : false;

						 			//**************************************PSE
						 			$("#IddeComercio").dxTextBox({
						 				value: options.data.ComercioConfigId,
						 				enabled: false,
						 				onValueChanged: function (data) {
						 					Datos_Id_Comercio = (data.value == null) ? "" : data.value;
						 				}
						 			});



						 			$("#DescripcionComercio").dxTextBox({
						 				value: options.data.ComercioConfigDescrip,
						 				onValueChanged: function (data) {
						 					Datos_Descripcion_Comercio = (data.value == null) ? "" : data.value;
						 				}
						 			});

						 			$("#PermiteParciales").dxCheckBox({
						 				value: options.data.PermiteParciales,
						 				onValueChanged: function (data) {
						 					Datos_Pagos_Parciales = (data.value == null) ? "" : data.value;
						 				}
						 			});


						 			//Crear Lista de comercios
						 			$("#lstComercios").dxSelectBox({
						 				dataSource: new DevExpress.data.ArrayStore({
						 					data: data,
						 					key: "Valor"
						 				}),
						 				displayExpr: "Descripcion",
						 				valueExpr: "Valor",
						 				onValueChanged: function (e) {

						 					$("#IddeComercio").dxTextBox({ value: e.value });
						 					Datos_Id_Comercio = e.value;

						 					$("#PermiteParciales").dxCheckBox({ value: options.data.PermiteParciales });
						 					Datos_Pagos_Parciales = options.data.PermiteParciales;

						 					var Id = e.value;
						 					//Recorremos la lista de configuraciones para sacar la descripción.
						 					for (var i = 0; i < Datos.length; i += 1) {
						 						if (Id == Datos[i].Valor) {
						 							//Asignar Descripción
						 							$("#DescripcionComercio").dxTextBox({ value: Datos[i].Descripcion.replace('- ' + e.value, '') });
						 							Datos_Descripcion_Comercio = Datos[i].Descripcion.replace('- ' + e.value, '');
						 						}
						 					}
						 				}
						 			});
						 			//**************************************PSE

						 			////**************************************TDC
						 			//$("#IddeComercioTC").dxTextBox({
						 			//	value: options.data.ComercioConfigIdTC,
						 			//	onValueChanged: function (data) {
						 			//		Datos_Id_ComercioTC = (data.value == null) ? "" : data.value;						 					
						 			//	}
						 			//});


						 			//$("#DescripcionComercioTC").dxTextBox({
						 			//	value: options.data.ComercioConfigDescripTC,
						 			//	onValueChanged: function (data) {
						 			//		Datos_Descripcion_ComercioTC = (data.value == null) ? "" : data.value;
						 			//	}
						 			//});


						 			////Crear Lista de comercios
						 			//$("#lstComerciosTC").dxSelectBox({
						 			//	dataSource: new DevExpress.data.ArrayStore({
						 			//		data: data,
						 			//		key: "Valor"
						 			//	}),
						 			//	displayExpr: "Descripcion",
						 			//	valueExpr: "Valor",
						 			//	onValueChanged: function (e) {

						 			//		$("#IddeComercioTC").dxTextBox({ value: e.value });
						 			//		Datos_Id_ComercioTC = e.value;


						 			//		var Id = e.value;
						 			//		//Recorremos la lista de configuraciones para sacar la descripción.
						 			//		for (var i = 0; i < Datos.length; i += 1) {
						 			//			if (Id == Datos[i].Valor) {
						 			//				//Asignar Descripción
						 			//				$("#DescripcionComercioTC").dxTextBox({ value: Datos[i].Descripcion.replace('- ' + e.value, '') });
						 			//				Datos_Descripcion_ComercioTC = Datos[i].Descripcion.replace('- ' + e.value, '');
						 			//			}
						 			//		}
						 			//	}
						 			//});

						 			////**************************************TDC




						 			// Botón Guardar
						 			$("#buttonGuardar").dxButton({
						 				text: "Guardar",
						 				type: "default",
						 				onClick: function (e) {
						 					//Servicio para guardar la configuración
						 					SrvResoluciones.EditarConfigPago(options.data.IdSeguridad, (Datos_Pagos_Parciales == 1 || Datos_Pagos_Parciales == true) ? true : false, Datos_Id_Comercio, Datos_Descripcion_Comercio).then(function (data) {
						 						DevExpress.ui.notify("Datos guardados con exito", 'success', 6000);
						 						//Actualiza los datos en el grid para que no tenga que ir nuevamente a base de datos
						 						//for (var i = 0; i < Lista_Resoluciones.length; i += 1) {
						 						//	if (options.data.IdSeguridad == Lista_Resoluciones[i].IdSeguridad) {
						 						//		Lista_Resoluciones[i].ComercioConfigId = Datos_Id_Comercio;
						 						//		Lista_Resoluciones[i].ComercioConfigDescrip = Datos_Descripcion_Comercio;
						 						//		Lista_Resoluciones[i].PermiteParciales = Datos_Pagos_Parciales;
						 						//	}
						 						//}
						 						//Hacemos click en el botón cancelar para cerrar el modal
						 						$('#btncancelar').click();
						 						consultar(txt_hgi_Facturador);
						 					});
						 				}
						 			});
						 		});
						 	}
						 }).removeClass("dx-button dx-button-normal dx-widget")

							).appendTo(container);
					}
				},
				{
					caption: "Facturador",
					dataField: "Empresa",
				},

				{
					caption: "Numero Resolución",
					dataField: "NumResolucion",
				}
					,
					{
						caption: "Tipo Documento",
						dataField: "TipoDoc",
						lookup: {
							dataSource: TipoDocumento,
							displayExpr: "Texto",
							valueExpr: "ID"
						}
					},
					{
						caption: "Prefijo",
						dataField: "Prefijo",
					},
					{
						caption: "Fecha Vigencia Hasta",
						dataField: "FechaVigenciaHasta",
						dataType: "date",
						format: "yyyy-MM-dd ",
					},
					{
						caption: "RangoFinal",
						dataField: "RangoFinal",
					},

					{
						caption: "Descripcion",
						dataField: "Descripcion",
						hidingPriority: 0,
					},
					{
						caption: "Id Set Dian",
						dataField: "IdSetDian",
						hidingPriority: 1,
					},
					{
						caption: "Fecha Ingreso",
						dataField: "FechaIngreso",
						dataType: "date",
						format: "yyyy-MM-dd ",
						hidingPriority: 2,
					},
					{
						caption: "Fecha Vigencia Desde",
						dataField: "FechaVigenciaDesde",
						dataType: "date",
						format: "yyyy-MM-dd ",
						hidingPriority: 3,
					},
					{
						caption: "Fecha Actualización",
						dataField: "FechaActualizacion",
						dataType: "date",
						format: "yyyy-MM-dd ",
						hidingPriority: 4,

					},
					{
						caption: "Observaciones",
						dataField: "Observaciones",
						hidingPriority: 5,
					}
					,

					{
						dataField: "RangoInicial",
						caption: "RangoInicial",
						hidingPriority: 6,
					},

					{
						caption: "Respuesta ServicioWeb",
						dataField: "RespuestaServicioWeb",
						hidingPriority: 7,
					},
					{
						caption: "Versión Dian",
						dataField: "VersionDian",
						hidingPriority: 8,
					},
					{
						caption: "Id de Comercio",
						dataField: "ComercioConfigId",
						hidingPriority: 9,
					},
					{
						caption: "Descripción del Comercio",
						dataField: "ComercioConfigDescrip",
						hidingPriority: 10,
					},

				],
				filterRow: {
					visible: true
				},
			});
		});
	}

	$scope.BuscarResolucionesControlador = function (Facturador) {
		//Consulto las resoluciones
		cargarFiltro(Facturador);
		//Muestro el filtro de resoluciones
		$scope.FiltroResolucion = true;
	}

});

//Tipos de documentos
var TipoDocumento =
    [
    { ID: 1, Texto: 'Factura' },
    { ID: 2, Texto: 'Nota Debito' },
	{ ID: 3, Texto: 'Nota Credito' }
    ];

//Esta función es la que llama el control generico fuera del controlador de angular, para luego entrar nuevamente al controlador
function BuscarResoluciones() {
	angular.element('#ConsultaResolucionesController').scope().BuscarResolucionesControlador(txt_hgi_Facturador);
}

