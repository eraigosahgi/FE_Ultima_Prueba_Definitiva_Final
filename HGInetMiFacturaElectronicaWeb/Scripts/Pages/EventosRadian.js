



App.controller('EventosRadianController', function EventosRadianController($scope, $http, $location, $rootScope) {

	var id_seguridad;
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
					$("#cmdenviar").dxButton({ visible: response.data[0].Inscribir_Documento });
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
				]

			});

			

			
			
			$("#cmdenviar").dxButton({
				text: "Enviar evento Titulo Valor",
				type: "default",
				visible:false,
				onClick: function () {
					$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
						alert(response.data);
					});
				}
			});


		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}

});