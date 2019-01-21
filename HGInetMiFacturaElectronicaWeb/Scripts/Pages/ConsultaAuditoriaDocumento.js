DevExpress.localization.locale(navigator.language);


var AuditDocumentoApp = angular.module('AuditDocumentoApp', ['dx']);
AuditDocumentoApp.controller('AuditDocumentoController', function AuditDocumentoController($scope, $http, $location) {

	var id_seguridad = location.search.split('id_seguridad=')[1];


	$http.get('/api/AuditoriaDocumento?id_seguridad_doc=' + id_seguridad).then(function (response) {

		$scope.DatFecha = response.data[0].DatFecha;
		$scope.StrNumero = response.data[0].StrNumero;
		$scope.StrObligado = response.data[0].StrObligado;
		$scope.StrDesEstado = response.data[0].StrDesEstado;
		$scope.StrDesProceso = response.data[0].StrDesProceso;
		$scope.TipoDocumento = response.data[0].TipoDocumento;
		$scope.StrIdSeguridad = response.data[0].StrIdSeguridad;

		if (response.data[0].RutaXmlAcuse) {
			$scope.VerXmlAcuse = true;
			$scope.RutaXmlAcuse = response.data[0].RutaXmlAcuse;
		}
		else
			$scope.VerXmlAcuse = false;

		if (response.data[0].RutaPdf) {
			$scope.VerPdf = true;
			$scope.RutaPdf = response.data[0].RutaPdf;
			$('#PanelVisorPdf').attr('src', response.data[0].RutaPdf);
		}
		else
			$scope.VerPdf = false;

		if (response.data[0].RutaXml) {
			$scope.VerXml = true;
			$scope.RutaXml = response.data[0].RutaXml;
		}
		else
			$scope.VerXml = false;



		$("#gridAuditDocumento").dxDataGrid({
			dataSource: response.data,
			allowColumnResizing: true,
			allowColumnReordering: true,
			paging: {
				pageSize: 20
			},
			pager: {
				showPageSizeSelector: true,
				allowedPageSizes: [10, 20, 30],
				showInfo: true
			}, loadPanel: {
				enabled: true
			},
			columns: [{
				caption: "Fecha",
				dataField: "DatFecha",
				cssClass: "col-md-2",
			}, {
				caption: "Estado",
				dataField: "StrDesEstado",
				cssClass: "col-md-2"
			}, {
				caption: "Proceso",
				dataField: "StrDesProceso",
				cssClass: "col-md-2"
			}, {
				caption: "Procesado Por",
				dataField: "StrDesProcesadoPor",
				cssClass: "col-md-2"
			}, {
				caption: "Realizado Por",
				dataField: "StrDesRealizadoPor",
				cssClass: "col-md-2"
			}
			],
			masterDetail: {
				enabled: true,
				template: function (container, options) {

					container.append($('<h4 class="form-control">MENSAJE:</h4><p style="width:10%"> ' + options.data.StrMensaje + '</p><br/>'));

					if (options.data.IntIdProceso == 8 || options.data.IntIdProceso == 10) {

						if (options.data.StrResultadoProceso)
							container.append($('<h4 class="form-control">RESPUESTA:</h4><span><p style="width:10%"> ' + options.data.StrResultadoProceso + '</p></span>'));

						if (options.data.StrResultadoProceso) {
							$http.get('/api/DetallesRespuesta?id_proceso=' + options.data.IntIdProceso + '&respuesta=' + options.data.StrResultadoProceso).then(function (response) {

								if (response.data != null) {
									container.append($('<h4 class="form-control">DETALLES RESPUESTA:</h4></br><label><b>Fecha Envío: </b> ' + response.data.Recibido + '</label></br><label><b>ID Remitente : </b> '
									+ response.data.IdRemitente + '</label></br><label><b>ID Contacto : </b> ' + response.data.IdContacto + '</label></br><label><b>Cantidad Adjuntos: </b> '
									+ response.data.Adjuntos + '</label></br><div id="json"></div>'));

									$("#json").dxDataGrid({
										dataSource: response.data.Seguimiento,
										allowColumnResizing: true,
										allowColumnReordering: true,
										paging: {
											pageSize: 10
										},
										pager: {
											showPageSizeSelector: true,
											allowedPageSizes: [5, 10, 20],
											showInfo: true
										}, loadPanel: {
											enabled: true
										},
										columns: [{
											caption: "Fecha Proceso",
											dataField: "FechaEvento",
											dataType: "date",
											format: "yyyy-MM-dd HH:mm:ss",
										},
										{
											dataField: "Tipo Proceso",
											caption: "TipoEvento",
											cellTemplate: function (container, options) {
												$("<div>").append($(ControlTipoEventoMail(options.data.TipoEvento))).appendTo(container);
											}
										},
										]
									}, function (response) {
										$('#wait').hide();
										DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
									});
								}
							});
						}
					} else {
						var cadena_inicio = options.data.StrResultadoProceso.substring(0, 4);

						//Valida si el mensaje de respuesta inicia con http y lo añade como link.
						if (cadena_inicio == "http") {
							container.append($('<h4 class="form-control">RESPUESTA:</h4><pre><a style="margin-left:5%;" target="_blank" href="' + options.data.StrResultadoProceso + '">' + options.data.StrResultadoProceso + '</a></pre>'));
						}
							//Valida si la cadena inicia con {
						else if (options.data.StrResultadoProceso.substring(0, 1) == "{") {
							container.append($('<h4 class="form-control">DETALLES RESPUESTA:</h4>'));

							var datos = angular.fromJson(options.data.StrResultadoProceso)

							var code_html = "";
							//Recorre una cadena json y la carga en código html propiedad por propiedad.
							for (var prop in datos) {

								code_html = code_html + '<label><b>' + prop + ':</b></label>';

								cadena_inicio = datos[prop].toString().substring(0, 4)

								//Valida si el valor de la propiedad es una ruta.
								if (cadena_inicio == "http")
									code_html = code_html + '<a style="margin-left:5%;" target="_blank" href="' + datos[prop] + '">' + datos[prop] + '</a></br>';
								else
									code_html = code_html + '<label>' + datos[prop] + '</label></br>';
							}

							container.append($('<pre>' + code_html + '</pre>'));

						}
						else if (options.data.StrResultadoProceso) {
							container.append($('<h4 class="form-control">RESPUESTA:</h4><span><p style="width:10%"> ' + options.data.StrResultadoProceso + '</p></span>'));
						}

					}
				}
			},
			filterRow: {
				visible: true
			}
		});
	}, function (response) {
		$('#wait').hide();
		DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
	});


});