DevExpress.localization.locale(navigator.language);

var ModalDetalleEmpresasApp = angular.module('ModalDetalleEmpresasApp', []);

var ConsultaNotificacionApp = angular.module('ConsultaNotificacionApp', ['ModalDetalleEmpresasApp', 'dx', 'AppMaestrosEnum', 'AppSrvAlertas']);
ConsultaNotificacionApp.controller('ConsultaNotificacionController', function ConsultaNotificacionController($scope,$rootScope, SrvAlertas) {

	CargarConsulta();

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



				loadPanel: {
					enabled: true
				}
				  , allowColumnResizing: true
			 , columns: [
			 {
			 	caption: "Detalle",
			 	alignment: "center",
			 	cellTemplate: function (container, options) {
			 		var ver = "class='icon-file-eye' title='ver Detalle Empresa' style='margin-left:5%; font-size:19px'" ;
			 		$("<div>")
						.append(
							$("<a " + ver + "></a>").dxButton({
								onClick: function () {									
									$rootScope.ConsultaDetalleEmpresa(options.data.idseguridadEmpresa);
								}
							}).removeClass("dx-button dx-button-normal dx-widget")
					)
						.appendTo(container);
			 	}
			 },
				 {

				 	caption: "Fecha de Vencimiento",
				 	dataField: "fechavencimiento",
				 	dataType: "date",
				 	format: "yyyy-MM-dd"
				 },

			 {
			 	caption: "Doc. Facturador",
			 	dataField: "identificacion"
			 },

			 {
			 	caption: "Facturador",
			 	dataField: "facturador"
			 },

				  {
				  	caption: "Nº Planes",
				  	dataField: "nplanes"
				  },
				   {
				   	caption: "Compra",
				   	dataField: "tcompra"
				   },

				  {
				  	caption: "Procesadas",
				  	dataField: "tprocesadas"
				  }
				  ,
				 {
				 	caption: "Disponible",
				 	dataField: "tdisponibles"
				 }
				 ,
				 {
				 	caption: "Email",
				 	dataField: "email"
				 },
				 {
				 	caption: "Notificado",
				 	dataField: "notificado"
				 },
				 {
				 	caption: "Alerta",
				 	dataField: "alerta"
				 },


			 {
			 	dataField: "valorindicador",
			 	caption: "Indicador",
			 },
			  {
			  	caption: "Procesar",
			  	width: "10%",
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
			  }





			 ]
				, filterRow: {
					visible: true
				}
			});

		});
	}

});


