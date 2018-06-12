DevExpress.localization.locale(navigator.language);

angular.module('ProcesarDocumentosApp', ['dx'])
.controller('ProcesarDocumentosController', function DocAdquirienteController($scope, $http, $location) {

    var now = new Date();

    var codigo_adquiente = "",
           numero_documento = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "";


    $http.get('/api/DatosSesion/').then(function (response) {       

        codigo_adquiente = response.data[0].Identificacion;

        consultar();
    }), function errorCallback(response) {
        Mensaje(response.data.ExceptionMessage, "error");
    };

    $("#FechaInicial").dxDateBox({
        value: now,
        displayFormat: "yyyy-MM-dd",
        onValueChanged: function (data) {
            fecha_inicio = new Date(data.value).toISOString();
            if (new Date(data.value).toISOString() > fecha_fin) {
                DevExpress.ui.notify("La fecha inicial no puede ser mayor a la fecha final", 'error', 3000);
                $("#FechaInicial").dxDateBox({ value: fecha_fin });
                fecha_inicio: fecha_fin;
            }
        }

    });

    $("#FechaFinal").dxDateBox({
        value: now,
        displayFormat: "yyyy-MM-dd",
        onValueChanged: function (data) {
            fecha_fin = new Date(data.value).toISOString();
            if (new Date(data.value).toISOString() < fecha_inicio) {
                DevExpress.ui.notify("La fecha final no puede ser menor a la fecha inicial", 'error', 3000);
                $("#FechaFinal").dxDateBox({ value: fecha_inicio });
                fecha_fin: fecha_inicio;
            }
        }

    });
    //Define los campos y las opciones
    $scope.filtros =
        {
            EstadoRecibo: {
                searchEnabled: true,
                //Carga la data del control
                dataSource: new DevExpress.data.ArrayStore({
                    data: items,
                    key: "ID"
                }),
                displayExpr: "Texto",
                Enabled: true,
                placeholder: "Seleccione un Item",
                onValueChanged: function (data) {                    
                    estado_recibo = data.value.ID;
                }
            },
            NumeroDocumento: {
                placeholder: "Identificador Documento",
                onValueChanged: function (data) {                    
                    numero_documento = data.value;
                }
            }
        }

    var mensaje_acuse = "";


    $scope.ButtonOptionsConsultar = {
        text: 'Consultar',
        type: 'default',
        onClick: function (e) {
            consultar();
        }
    };

    //Consultar DOcumentos
    function consultar() {       

        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();


        //Obtiene los datos del web api
        //ControladorApi: /Api/Documentos/
        //Datos GET: codigo_adquiente - numero_documento - estado_recibo - fecha_inicio - fecha_fin
        $('#wait').show();
        $http.get('/api/Documentos?IdSeguridad=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
            $('#wait').hide();            
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
                         format: "yyyy-MM-dd hh:mm a",
                         cssClass: "col-md-2"

                     },

                    {
                        caption: "Documento",
                        dataField: "IdSeguridad",
                        cssClass: "col-md-4",
                        headerFilter: {
                            allowSearch: false
                        }

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
                }

                , onSelectionChanged: function (selectedItems) {
                    var lista='' ;
                    var data = selectedItems.selectedRowsData;
                    if (data.length > 0) {
                        if (data.length > 1) {
                            for (var i = 0; i < data.length; i++) {
                                lista += (lista) ? ',' : '';
                                lista += "{Documentos: '" + data[i].IdSeguridad + "'}";
                            }
                            lista = "[" + lista + "]"
                            $scope.documentos = lista;
                            $('#lbltotaldocumentos').val('Documentos a Procesar : ' + data.length);
                            $scope.total = data.length;                            

                        } else {
                            lista += "{Documentos: '" + data[0].IdSeguridad + "'}";
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
                }

                , summary: {
                    totalItems: [{
                        column: "IdSeguridad",
                        caption: "Total Documentos : ",
                        summaryType: "count"
                    }
                    ]
                }


            }).dxDataGrid("instance");
        });
    }

    $("#btnProcesar").dxButton({
        text: "Enviar Documentos",
        type: "default",
        onClick: ProcesarDocumentos
    });

    function ProcesarDocumentos() {
        if ($scope.total > 0) {
            $('#wait').show();
            $http({
                url: '/api/Documentos/',
                data: { Documentos: $scope.documentos },
                method: 'Post'

            }).then(function (response) {
                $('#wait').hide();
                DevExpress.ui.notify("Se procesaron los documentos exitosamente ", 'success', 2000);
            })
            , function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            }
        } else {
            DevExpress.ui.notify("No tiene documentos seleccionados", 'error', 3000);
        }
    }

});

//Datos del control Estado
var items =
    [
    { ID: "*", Texto: 'Obtener Todos' },
    { ID: "1", Texto: 'Recepción' },
    { ID: "2", Texto: 'Validación Documento' },
    { ID: "3", Texto: 'Generación UBL' },
    { ID: "4", Texto: 'Almacenamiento XML' },
    { ID: "5", Texto: 'Firma XML' },
    { ID: "6", Texto: 'Compresión XML' },
    { ID: "7", Texto: 'Envío ZIP' },
    { ID: "8", Texto: 'Envío E-mail Adquiriente' },
    { ID: "9", Texto: 'Recepción Acuse' },
    { ID: "10", Texto: 'Envío E-mail Acuse' },
    { ID: "90", Texto: 'Error Dian, Finaliza Proceso' },
    { ID: "99", Texto: 'Fin Proceso Exitoso' }
    ];

