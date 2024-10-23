DevExpress.localization.locale(navigator.language);

var ModalDetalleEmpresasApp = angular.module('ModalDetalleEmpresasApp', []);

var ConsultaNotificacionApp = angular.module('ConsultaNotificacionApp', ['ModalDetalleEmpresasApp', 'dx', 'AppMaestrosEnum', 'AppSrvAlertas']);
ConsultaNotificacionApp.controller('ConsultaNotificacionController', function ConsultaNotificacionController($scope, $rootScope, SrvAlertas) {

	CargarConsulta();

	function CambiarIcono() {
		$('.dx-datagrid-adaptive-more').addClass("icon-file-eye").removeClass("dx-datagrid-adaptive-more").attr("style", "margin-left:5%; font-size:19px; color: #1E88E5; cursor: pointer; title:'Detalle Notificación';");
	}
	function CargarConsulta() {
		SrvAlertas.ObtenerAlertas().then(function (data) {
			$("#grid").dxDataGrid({
				dataSource: data,

				headerFilter: {
					visible: true
				},
				groupPanel: {
					allowColumnDragging: true,
					visible: true
				},
				columnChooser: {
					enabled: true,
					mode: "select",
					title: "Selector de Columnas"
				},

				loadPanel: {
					enabled: true
				}
				  , allowColumnResizing: true

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
				}
			 , columns: [
			 {
			 	caption: "Detalle",
			 	alignment: "center",
			 	width: "6%",
			 	cellTemplate: function (container, options) {
			 		var ver = "class='icon-user-tie' style='margin-left:5%; font-size:19px;color: #1E88E5;'";
			 		$("<div title='Detalle : " + options.data.facturador + "' >")
						.append(
							$("<i " + ver + "></i>").dxButton({
								onClick: function () {
									$rootScope.ConsultaDetalleEmpresa(options.data.idseguridadEmpresa);
								}
							}).removeClass("dx-button dx-button-normal dx-widget")
					)
						.appendTo(container);
			 	}
			 },
			{

				caption: "Fecha de Notificación",
				dataField: "DatFecha",
				dataType: "date",
				format: "yyyy-MM-dd HH:mm",
				width: "12%"
			},

			 {
			 	caption: "Doc. Facturador",
			 	dataField: "identificacion",
			 	width: "12%"

			 },

			 {
			 	caption: "Facturador",
			 	dataField: "facturador",
			 	width: "25%"
			 },


				 {
				 	caption: "Notificado",
				 	dataField: "notificado",
				 	width: "5%"
				 },
				 {
				 	caption: "Alerta",
				 	dataField: "alerta",
				 	width: "25%"
				 },

				{
					caption: "Notifica A",
					dataField: "email",
					visible: false

				},

			  {
			  	caption: "Enviar",
			  	width: "4%",
			  	alignment: "left",
			  	cellTemplate: function (container, options) {
			  		var permite_envio = (options.data.notificado == "NO") ? "class='icon-mail-read' style='margin-left:5%; font-size:19px'" : "";
			  		$("<div>")
						.append(
							$("<a " + permite_envio + "></a>").dxButton({
								onClick: function () {
									SrvAlertas.ReprocesarAlerta(options.data.identificacion, options.data.idalerta).then(function (data) {
										CargarConsulta();
									});

								}
							}).removeClass("dx-button dx-button-normal dx-widget")
					)
						.appendTo(container);
			  	}
			  },
			 {

			 	caption: "",
			 	width: "6%",
			 	alignment: "left",
			 	hidingPriority: 0,
			 	cellTemplate: function (container, options) {
			 		//Porcentaje
			 		try {

			 			///Porcentaje
			 			if (options.data.intIdtipo == 1) {
			 				if (options.data.StrResultadoProceso != undefined && options.data.StrResultadoProceso != '') {
			 					var Notificacion = jQuery.parseJSON(options.data.StrResultadoProceso);
			 					container.append($("<td aria-selected='false' role='gridcell' aria-colindex='1' colspan='6' style='text-align: center;'><div style='font-size: 16px;' >Detalle de la Notificación:</div> <div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>Adquiridos</div></td><td aria-selected='false' role='columnheader' aria-colindex='2' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>Procesados</div></td><td aria-selected='false' role='columnheader' aria-colindex='3' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>Disponible</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Valor</div></td><td colspan='5' aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Notifico a:</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <p style='margin-left:5%;' >" + Notificacion.tcompra + "</p></div> </td><td aria-selected='false' role='gridcell' aria-colindex='2' style='text-align: center;'><div> <p style='margin-left:5%;margin-right:5%;' >" + Notificacion.tprocesadas + "</p></div></td><td aria-selected='false' role='gridcell' aria-colindex='3' style='text-align: center;'><div> <p> " + Notificacion.tdisponibles + "</p></div> </td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><p style='margin-left:5%;' >" + Notificacion.valorindicador + "</p><td colspan='5'><div>" + Notificacion.email + "</div></td></div></td></tr></tbody></table></td>"));
			 				}
			 			}
			 			///Sin saldo
			 			if (options.data.intIdtipo == 2) {
			 				if (options.data.StrResultadoProceso != undefined && options.data.StrResultadoProceso != '') {
			 					var Notificacion = jQuery.parseJSON(options.data.StrResultadoProceso);
			 					container.append($("<td colspan='5' aria-selected='false' role='gridcell' aria-colindex='1' colspan='6' style='text-align: center;'><div style='font-size: 16px;' >Detalle de la Notificación:</div> <div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td colspan='5' aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content' style='text-align:left;'>Notifico a:</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content' style='text-align:left;'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td colspan='5'><div>" + Notificacion.StrEmail + "</div></td></div></td></tr></tbody></table></td>"));
			 				}
			 			}
			 			///Cera de Vencer
			 			if (options.data.intIdtipo == 3) {
			 				if (options.data.StrResultadoProceso != undefined && options.data.StrResultadoProceso != '') {
			 					var Notificacion = jQuery.parseJSON(options.data.StrResultadoProceso);
			 					container.append($("<td colspan='5' aria-selected='false' role='gridcell' aria-colindex='1' colspan='6' style='text-align: center;'><div style='font-size: 16px;' >Detalle de la Notificación:</div> <div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td colspan='3' aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>Id de Seguridad del Plan</div></td><td  aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Fecha de Vencimiento</div></td><td colspan='5' aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Notifico a:</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td colspan='3' aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <p style='margin-left:5%;' >" + Notificacion.StrIdSeguridadPlan + "</p></div> </td><td  aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><p style='margin-left:5%;' >" + Notificacion.valorindicador + "</p><td colspan='5'><div>" + Notificacion.email + "</div></td></div></td></tr></tbody></table></td>"));
			 				}
			 			}

			 		} catch (e) {
			 			console.log("Error al cargar el detalle de la Notificación: ", options.data.StrResultadoProceso);
			 		}


			 	}
			 },

			 ]
			 		, filterRow: {
			 			visible: true
			 		}
			});
			setTimeout(CambiarIcono, 1000);
		});
	}

});


