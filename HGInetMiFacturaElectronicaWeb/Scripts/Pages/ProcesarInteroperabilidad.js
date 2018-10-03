DevExpress.localization.locale(navigator.language);

var ModalProveedorApp = angular.module('ModalProveedorApp', []);

var ProcesarInteroperabilidadApp = angular.module('ProcesarInteroperabilidadApp', ['dx', 'ModalProveedorApp']);

ProcesarInteroperabilidadApp.controller('ProcesarInteroperabilidadController', function ProcesarInteroperabilidadController($scope, $http, $location) {

	$('#panelresultado').hide();

	var seleccion_empresa;

	$("#btnProcesar").dxButton({
		text: "Enviar Documentos",
		type: "default",
		onClick: ProcesarDocumentos
	});

	$("#txtProveedorTecnologico").dxTextBox({
		readOnly: true,
		onValueChanged: function (data) {
			seleccion_empresa = data.value;
		},
		onFocusIn: function (data) {
			$('#modal_Buscar_proveedores').modal('show');
		}
	}).dxValidator({
		validationRules: [{
			type: "required",
			message: "Debe indicar cual es la Empresa Asociada"
		}],
	});

	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
		    consultar();
		    $('#panelresultado').hide();
		}
	};

	//Consultar DOcumentos
	function consultar() {
		var identificacion_proveedor = seleccion_empresa.split(' -- ');

		$http.get('/api/ObtenerPendientesProceso?identificacion_proveedor=' + identificacion_proveedor[0]).then(function (response) {
			$("#wait").hide();
			try {
				//$('#panelresultado').show();

				$("#gridDocumentos").dxDataGrid({
					dataSource: response.data,
					paging: {
						pageSize: 20
					},
					pager: {
						showPageSizeSelector: true,
						allowedPageSizes: [5, 10, 20],
						showInfo: true
					},
					selection: {
						mode: "multiple",
						width: 10
					}
					  , loadPanel: {
					  	enabled: true
					  },
					headerFilter: {
						visible: true
					}

					, allowColumnResizing: true
					, columns: [
						 {
						 	caption: "Fecha Recepción",
						 	dataField: "DatFechaIngreso",
						 	dataType: "date",
						 	format: "yyyy-MM-dd HH:mm",
						 	cssClass: "col-md-1"

						 },
						 {
						 	caption: "Documento",
						 	dataField: "NumeroDocumento",
						 	cssClass: "col-md-1",
						 	headerFilter: {
						 		allowSearch: false
						 	}
						 },
						{
							caption: "IdSeguridad",
							dataField: "StrIdSeguridad",
							cssClass: "col-md-3",
							headerFilter: {
								allowSearch: false
							}
						},
						 {
						 	caption: "Tipo Documento",
						 	cssClass: "col-md-2",
						 	dataField: "tipodoc"
						 },
						  {
						  	caption: "Facturador",
						  	dataField: "Facturador",
						  	cssClass: "col-md-2 "
						  },
						  {
						  	caption: "Estado",
						  	dataField: "EstadoFactura",
						  	cssClass: "col-md-2",
						  	headerFilter: {
						  		allowSearch: true,
						  		caption: "Busqueda"
						  	}
						  }
					],
					filterRow: {
						visible: true
					}, onSelectionChanged: function (selectedEstado) {
						var lista = '';
						var data = selectedEstado.selectedRowsData;
						if (data.length > 0) {
							if (data.length > 1) {
								for (var i = 0; i < data.length; i++) {
									lista += (lista) ? ',' : '';
									lista += "{Documentos: '" + data[i].StrIdSeguridad + "'}";
								}
								lista = "[" + lista + "]"
								$scope.documentos = lista;
								$('#lbltotaldocumentos').val('Documentos a Procesar : ' + data.length);
								$scope.total = data.length;

							} else {
								lista += "{Documentos: '" + data[0].StrIdSeguridad + "'}";
								lista = "[" + lista + "]"
								$scope.documentos = lista;
								$('#lbltotaldocumentos').val('Documento a Procesar : ' + data.length);
								$scope.total = data.length;

							}
						} else {
							$('#lbltotaldocumentos').val('Ningun Documento Por Procesar');
							$scope.documentos = "Ningun Documento Por Procesar";
							$scope.total = 0;

						}
						$scope.$apply(function () {
							$scope.total = $scope.total;
						});

					}, summary: {
						totalEstado: [{
							column: "IdSeguridad",
							caption: "Total Documentos : ",
							summaryType: "count"
						}
						]
					}
				}).dxDataGrid("instance");
			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});
	}

	function ProcesarDocumentos() {

		$http({ url: '/api/Interoperabilidad/', data: { Documentos: $scope.documentos }, method: 'Post' }).then(function (response) {
		    try {
		        $('#Lblresultado').html(response.meMensajeZip);

		        $("#gridDocumentosProcesados").dxDataGrid({
		            dataSource: response.data,
		            paging: {
		                pageSize: 20
		            },
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
              , columns: [
                  /*
                  {
                      caption: 'Estado',
                      dataField: 'Estado',
                      width: '5%',
                      cellTemplate: function (container, options) {
                          $("<div style='text-align:center'>")
                              .append($("<a taget=_self class='icon-circle2'" + ((options.data.CodigoError != '') ? " style='color:red;' title='Error'" : " style='color:green;' title='Proceso exitoso'") + ">"))
                              .appendTo(container);
                      }
                  },
                   {
                       caption: "Fecha Recepción",
                       dataField: "FechaRecepcion",
                       dataType: "date",
                       width: '16%',
                       format: "yyyy-MM-dd HH:mm"

                   },
                   */
                  {
                      caption: "Fecha Ultimo Proceso",
                      dataField: "FechaUltimoProceso",
                      dataType: "date",
                      width: '16%',
                      format: "yyyy-MM-dd HH:mm"

                  },
                  {
                      caption: "Documento",
                      dataField: "Documento",
                      width: '10%',

                  },
                   {
                       caption: "Mensaje",
                       width: '50%',
                       dataField: "Mensaje"
                   },
              {
                  caption: "UUID",
                  dataField: "uuid",
                  hidingPriority: 0
              }
              
              ,
                    {
                        caption: "Codigo Proceso",
                        dataField: "codigoError",
                        hidingPriority: 1
                    },
                    {
                        caption: "Tipo Documento",
                        dataField: "tipodoc",
                        hidingPriority: 2
                    }
                    ,
                    {
                        caption: "EstadoFactura",
                        dataField: "EstadoFactura",
                        hidingPriority: 4

                    },
                    {
                        caption: "Nombre Archivo",
                        dataField: "Nombre",
                        hidingPriority: 5
                    },
                    /*
                    {
                        caption: "Identificacion",
                        dataField: "Identificacion",
                        hidingPriority: 6
                    },
                    {
                        caption: "IdProceso",
                        dataField: "IdProceso",
                        hidingPriority: 7
                    },
                   {
                       caption: "MotivoRechazo",
                       dataField: "MotivoRechazo",
                       hidingPriority: 8
                   },
                   {
                       caption: "NumeroResolucion",
                       dataField: "NumeroResolucion",
                       hidingPriority: 9
                   }, {
                       caption: "Descripcion Proceso",
                       dataField: "DescripcionProceso",
                       hidingPriority: 10
                   },
                   {
                       caption: "Tipo de Documento",
                       dataField: "tipodoc",
                       hidingPriority: 11
                   }*/

              ]
		        }).dxDataGrid("instance");






			    $('#panelresultado').show();
				consultar();
			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});

	}

});


