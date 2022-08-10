
var id_seguridad;
var numero_documento;
var obligado;

App.controller('EventosRadianController', function EventosRadianController($scope, $http, $location, $rootScope) {

	$rootScope.ConsultarEventosRadian = function (IdSeguridad, NumeroDocumento, Obligado) {
		$scope.IdSeguridad = IdSeguridad;
		$("#IdSeguridad").text(IdSeguridad);
		$scope.NumeroDocumento = $("#NumeroDocumento").text(NumeroDocumento);
		$scope.Obligado = $("#Obligado").text(Obligado);

		$http.get('/api/ObtenerEventosRadian?id_seguridad=' + IdSeguridad).then(function (response) {



			$("#GridEventosRadianDocumento").dxDataGrid({
				dataSource: response.data,
				allowColumnResizing: true,
				paging: {
					pageSize: 10
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				},
				onContentReady: function (e) {
					try {
						$("#cmdenviar").dxButton({ visible: response.data[0].Inscribir_Documento });
						$("#cmdenviar1").dxButton({ visible: response.data[0].otros_eventos});
						$("#cmdenviar2").dxButton({ visible: response.data[0].otros_eventos });
						$("#cmdenviar3").dxButton({ visible: response.data[0].otros_eventos });
						$("#cmdenviar4").dxButton({ visible: response.data[0].otros_eventos });
						$("#cmdenviar5").dxButton({ visible: response.data[0].otros_eventos });

					}
					catch (err) {
						$("#cmdenviar").dxButton({ visible: false });
						$("#cmdenviar1").dxButton({ visible: false });
						$("#cmdenviar2").dxButton({ visible: false });
						$("#cmdenviar3").dxButton({ visible: false });
						$("#cmdenviar4").dxButton({ visible: false });
						$("#cmdenviar5").dxButton({ visible: false });
					}
				},
				columns: [
				    {
				    	caption: "Fecha",
				    	dataField: "DatFechaEvento",
				    	cssClass: "col-md-2",
				    	dataType: "date",
				    	format: "yyyy-MM-dd HH:mm:ss",
				    }, {
				    	caption: "Estado",
				    	dataField: "EstadoEvento",
				    	cssClass: "col-md-4",
				    }, {
				    	caption: "Numero Evento",
				    	dataField: "IntNumeroEvento",
				    	cssClass: "col-md-1",
				    },
				   {
				   	caption: "StrUrlEvento",
				   	dataField: "StrUrlEvento",
				   	visible: false
				   },
				    {
				    	caption: "respuesta_evento",
				    	dataField: "respuesta_evento",
				    	visible: false
				    },
				   {
				   	caption: "Archivo",
				   	cssClass: "col-md-1",
				   	cellTemplate: function (container, options) {
				   		var visible_xml = "href='" + options.data.StrUrlEvento + "' class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
				   		$("<div>")
							.append(
							   $("<a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + ">"))
							.appendTo(container);
				   	}
				   },
				   {
				   	caption: "Respuesta",
				   	cssClass: "col-md-1",
				   	cellTemplate: function (container, options) {
				   		var visible_xml_resp = "href='" + options.data.respuesta_evento + "' class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
				   		$("<div>")
							.append(
							   $("<a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml_resp + ">"))
							.appendTo(container);
				   	}
				   },
				]

			});





			$("#cmdenviar").dxButton({
				text: "Inscripción TV",
				type: "default",
				visible: false,
				onClick: function () {
					$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
						//alert(response.data);
						$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					}, function errorCallback(response) {
						
						//Carga notificación de creación con opción de editar formato.
						var myDialog = DevExpress.ui.dialog.custom({
							title: "Proceso Falló",
							message: response.data.ExceptionMessage,
							buttons: [{
								text: "Aceptar",
								onClick: function (e) {
									myDialog.hide();
									$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
								}
							}]
						});
						myDialog.show().done(function (dialogResult) {
						});

						$('#wait').hide();
					});
				}
			});

			$("#cmdenviar1").dxButton({
				text: "Endoso",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar2").dxButton({
				text: "Aval",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar3").dxButton({
				text: "Mandato",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar4").dxButton({
				text: "Limitación",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar5").dxButton({
				text: "Transferencia Derecho",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar6").dxButton({
				text: "Informe Pago",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});

			$("#cmdenviar7").dxButton({
				text: "Pago",
				type: "default",
				visible: false,
				onClick: function () {
					MensajeEventoRadian();
					//$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
					//	//alert(response.data);
					//	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
					//});
				}
			});




		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}

	function MensajeEventoRadian() {
		DevExpress.ui.notify("Evento no disponible temporalmente", 'error', 3000);
	}

});