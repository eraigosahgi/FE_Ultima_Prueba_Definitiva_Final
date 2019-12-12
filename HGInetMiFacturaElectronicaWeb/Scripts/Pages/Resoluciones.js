
DevExpress.localization.locale(navigator.language);
var App = angular.module('App', ['dx', 'AppSrvResoluciones']);
App.controller('ConsultaResolucionesController', function ConsultaResolucionesController($scope, $http, $rootScope, $location, SrvResoluciones) {



	SrvResoluciones.Obtener('811021438').then(function (data) {
		consultar(data);
	});

	function consultar(data) {
		$('#wait').hide();
		$("#gridResoluciones").dxDataGrid({
			dataSource: data,
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
			//Formatos personalizados a las columnas en este caso para el monto
			//, onCellPrepared: function (options) {
			//	var fieldData = options.value,
			//		fieldHtml = "";
			//	try {
			//		if (options.column.caption == "Valor Total" || options.column.caption == "SubTotal" || options.column.caption == "Neto") {
			//			if (fieldData) {
			//				var inicial = fNumber.go(fieldData).replace("$-", "-$");
			//				options.cellElement.html(inicial);
			//			}
			//		}
			//	} catch (err) {
			//		DevExpress.ui.notify(err.message, 'error', 3000);
			//	}
			//}
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
			}
			, columns: [

				{
					caption: "Descripcion",
					dataField: "Descripcion",
				},
				{
					caption: "Id Set Dian",
					dataField: "IdSetDian",
				},
				{
					caption: "Fecha Ingreso",
					dataField: "FechaIngreso",
					dataType: "date",
					format: "yyyy-MM-dd ",
				},
				{
					caption: "Fecha Vigencia Desde",
					dataField: "FechaVigenciaDesde",
					dataType: "date",
					format: "yyyy-MM-dd "
				},
				{
					caption: "Fecha Vigencia Hasta",
					dataField: "FechaVigenciaHasta",
					dataType: "date",
					format: "yyyy-MM-dd ",
					hidingPriority: 0
				},
				{
					caption: "Fecha Actualización",
					dataField: "FechaActualizacion",
					dataType: "date",
					format: "yyyy-MM-dd ",
					hidingPriority: 1
				}
				,
				{
					caption: "Numero Resolución",
					dataField: "NumResolucion",
					hidingPriority: 2
				}
				,
				{
					caption: "Observaciones",
					dataField: "Observaciones",
					hidingPriority: 3
				}
				,
				{
					caption: "Permite Pagos Parciales",
					dataField: "PermiteParciales",
					hidingPriority: 4

				},
				{
					caption: "Prefijo",
					dataField: "Prefijo",
					hidingPriority: 5
				},
				{
					dataField: "RangoInicial",
					caption: "RangoInicial",
					hidingPriority: 6

				},
				{
					caption: "RangoFinal",
					dataField: "RangoFinal",
					hidingPriority: 7
				},

				{
					caption: "Respuesta ServicioWeb",
					dataField: "RespuestaServicioWeb",
					hidingPriority: 8

				},
				{
					caption: "Tipo Documento",
					dataField: "TipoDoc",
					hidingPriority: 9
				},
				{
					caption: "Versión Dian",
					dataField: "VersionDian",
					hidingPriority: 10
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
				  //}
			],
			//**************************************************************

			filterRow: {
				visible: true
			},
		});
	}

});
