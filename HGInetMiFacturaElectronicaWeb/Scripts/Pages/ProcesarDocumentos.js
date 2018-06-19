DevExpress.localization.locale(navigator.language);

angular.module('ProcesarDocumentosApp', ['dx', 'AppMaestrosEnum', 'AppSrvUsuario'])
    
.controller('ProcesarDocumentosController', function DocAdquirienteController($scope, SrvMaestrosEnum, SrvUsuario) {
    
    var codigo_adquiente = "", numero_documento = "", estado_recibo = "", fecha_inicio = "", fecha_fin = "", Estado = [], now = new Date();
           
    SrvMaestrosEnum.ObtenerEnum(0).then(function (data) {
        Estado = data;
        cargarFiltros();    
    });
          
    SrvMaestrosEnum.ObtenerSesion().then(function (data) {    
        codigo_adquiente = data[0].Identificacion;        
        consultar();                      
    });
    
    var makeAsyncDataSource = function () {        
        return new DevExpress.data.CustomStore({
            loadMode: "raw",
            key: "ID",
            load: function () {                                
                return JSON.parse(JSON.stringify(Estado));
            }
        });
    };

    function cargarFiltros() {        
        $("#FechaInicial").dxDateBox({
            name:"txtf",
            value: now,
            max: fecha_fin,
            displayFormat: "yyyy-MM-dd",            
            onValueChanged: function (data) {
                fecha_inicio = new Date(data.value).toISOString();
                $("#FechaFinal").dxDateBox({ min: fecha_inicio });
                            
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
            min: fecha_inicio,
            onValueChanged: function (data) {
                fecha_fin = new Date(data.value).toISOString();
                $("#FechaInicial").dxDateBox({ max: fecha_fin });
                if (new Date(data.value).toISOString() < fecha_inicio) {
                    DevExpress.ui.notify("La fecha final no puede ser menor a la fecha inicial", 'error', 3000);
                    $("#FechaFinal").dxDateBox({ value: fecha_inicio });
                    fecha_fin: fecha_inicio;
                }
            }

        });
        
       

        $("#filtrosEstadoRecibo").dxDropDownBox({            
            valueExpr: "ID",
            placeholder: "Seleccionar ",
            displayExpr: "Descripcion",
            showClearButton: true,
            dataSource: makeAsyncDataSource(),
            contentTemplate: function (e) {
                var value = e.component.option("value"),
                    $dataGrid = $("<div>").dxDataGrid({
                        dataSource: e.component.option("dataSource"),
                        allowColumnResizing: true,
                        columns:
                            [
                                 {
                                     caption: "Descripción",
                                     dataField: "Descripcion",
                                     title:"Descripcion",
                                     width:500

                                 }],
                        hoverStateEnabled: true,
                        paging: { enabled: true, pageSize: 10 },
                        //filterRow: { visible: true },
                        scrolling: { mode: "infinite" },
                        height: 300,
                        
                        selection: { mode: "multiple" },
                        selectedRowKeys: value,
                        onSelectionChanged: function (selectedItems) {
                            var keys = selectedItems.selectedRowKeys;
                            e.component.option("value", keys);
                            estado_recibo=keys;
                        }
                    });

                dataGrid = $dataGrid.dxDataGrid("instance");

                e.component.on("valueChanged", function (args) {
                    var value = args.value;
                    dataGrid.selectRows(value, false);
                });

                return $dataGrid;
            }
        });



        //Define los campos y las opciones       
        $scope.filtros =


                {
/*
                    EstadoRecibo: {
                        searchEnabled: true,
                        //Carga la data del control
                        dataSource: new DevExpress.data.ArrayStore({
                            data: Estado
                            , key: "ID"
                        }), displayExpr: "Descripcion",
                        Enabled: true,
                        placeholder: "Seleccione un Item",
                        onValueChanged: function (data) {                            
                            estado_recibo = data.value.ID;
                            console.log("ss", estado_recibo);
                        }
                    },*/
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

        $("#FechaFinal").dxDateBox({ min: now });
        $("#FechaInicial").dxDateBox({ max: now });
    }
    //Consultar DOcumentos
    function consultar() {       

        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        
        SrvMaestrosEnum.ObtenerDocumentos('IdSeguridad=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (data) {                           
            $("#gridDocumentos").dxDataGrid({
                dataSource: data,
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

                , onSelectionChanged: function (selectedEstado) {
                    var lista='' ;
                    var data = selectedEstado.selectedRowsData;
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
                    totalEstado: [{
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
            return $http({
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

