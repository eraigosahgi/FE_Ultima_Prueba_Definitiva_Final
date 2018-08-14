DevExpress.localization.locale('es-ES');

var path = window.location.pathname;
var ruta = window.location.href;
ruta = ruta.replace(path, "/");
document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/MaestrosEnum.js"></scr' + 'ipt>');


var email_destino = "";
var id_seguridad = "";
var items_recibo = [];
var PagosFacturadorApp = angular.module('PagosFacturadorApp', ['dx', 'AppMaestrosEnum']);
PagosFacturadorApp.controller('PagosFacturadorController', function PagosFacturadorController($scope, $http, $location, SrvMaestrosEnum) {

    var now = new Date();
    var Estado;
    var ResolucionesPrefijo;

    var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";
    resolucion = "";

    SrvMaestrosEnum.ObtenerSesion().then(function (data) {
        codigo_facturador = data[0].Identificacion;
    });

    SrvMaestrosEnum.ObtenerEnum(0, "publico").then(function (data) {
        SrvMaestrosEnum.ObtenerEnum(1).then(function (dataacuse) {
            Estado = data;
            items_recibo = dataacuse;

            $http.get('/api/ObtenerResPrefijo?codigo_facturador=' + codigo_facturador).then(function (response) {
                console.log("Prefijo", response.data);
                ResolucionesPrefijo = response.data;
                cargarFiltros();
            }, function (response) {
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

        });
    });

    function cargarFiltros() {
        $("#FechaInicial").dxDateBox({
            value: now,
            width: '100%',
            displayFormat: "yyyy-MM-dd",
            onValueChanged: function (data) {
                fecha_inicio = new Date(data.value).toISOString();
                $("#FechaFinal").dxDateBox({ min: fecha_inicio });
            }

        });

        $("#FechaFinal").dxDateBox({
            value: now,
            width: '100%',
            displayFormat: "yyyy-MM-dd",
            onValueChanged: function (data) {
                fecha_fin = new Date(data.value).toISOString();
                $("#FechaInicial").dxDateBox({ max: fecha_fin });
            }

        });


        var DataCheckBox = function (Datos) {
            return new DevExpress.data.CustomStore({
                loadMode: "raw",
                key: "ID",
                load: function () {
                    return JSON.parse(JSON.stringify(Datos));
                }
            });
        };




        //Resolucion - Prefijo
        $("#filtrosResolucion").dxDropDownBox({
            valueExpr: "ID",
            placeholder: "Seleccione un Item",
            displayExpr: "Descripcion",
            showClearButton: true,
            dataSource: DataCheckBox(ResolucionesPrefijo),
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
                                     title: "Descripcion",
                                     width: 500

                                 }],
                        hoverStateEnabled: true,
                        paging: { enabled: true, pageSize: 10 },
                        filterRow: { visible: true },
                        scrolling: { mode: "infinite" },
                        height: 240,
                        selection: { mode: "multiple" },
                        selectedRowKeys: value,
                        onSelectionChanged: function (selectedItems) {
                            var keys = selectedItems.selectedRowKeys;
                            e.component.option("value", keys);
                            resolucion = keys;
                            console.log("resolucion", resolucion);
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
                EstadoRecibo: {
                    searchEnabled: true,
                    //Carga la data del control
                    dataSource: new DevExpress.data.ArrayStore({
                        data: items_recibo,
                        key: "ID"
                    }),
                    displayExpr: "Descripcion",
                    Enabled: true,
                    placeholder: "Seleccione un Item",
                    onValueChanged: function (data) {
                        estado_recibo = data.value.ID;
                    }
                },
                NumeroDocumento: {
                    placeholder: "Ingrese Número Documento",
                    onValueChanged: function (data) {
                        numero_documento = data.value;
                    }
                },
                Adquiriente: {
                    placeholder: "Ingrese Identificación del Adquiriente",
                    onValueChanged: function (data) {
                        codigo_adquiriente = data.value;
                    }
                }
            }

        $("#FechaFinal").dxDateBox({ min: now });
        $("#FechaInicial").dxDateBox({ max: now });
        consultar();
    }

    $scope.ButtonOptionsConsultar = {
        text: 'Consultar',
        type: 'default',
        onClick: function (e) {
            consultar2();
        }
    };




    function consultar2() {
        $('#Total').text("");
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        if (resolucion == "")
            resolucion = "*";

        //Obtiene los datos del web api
        //ControladorApi: /Api/Documentos/
        //Datos GET: codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin
        $('#wait').show();
        $http.get('/api/ObtenerPagosFacturador?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&resolucion=' + resolucion).then(function (response) {
            console.log(response.data);
            $('#wait').hide();
            $("#gridDocumentos").dxDataGrid({
                dataSource: response.data,
            })
        });
    }


    function consultar() {
        $('#Total').text("");
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        if (resolucion == "")
            resolucion = "*";

        //Obtiene los datos del web api
        //ControladorApi: /Api/Documentos/
        //Datos GET: codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin
        $('#wait').show();
        $http.get('/api/ObtenerPagosFacturador?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&resolucion=' + resolucion).then(function (response) {
            $('#wait').hide();
            $("#gridDocumentos").dxDataGrid({
                dataSource: response.data,
                keyExpr: "NumeroDocumento",
                paging: {
                    pageSize: 20
                },
                stateStoring: {
                    enabled: true,
                    type: "localStorage",
                    storageKey: "storage"
                },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                }
                //Formatos personalizados a las columnas en este caso para el monto
                , onCellPrepared: function (options) {
                    var fieldData = options.value,
                        fieldHtml = "";
                    try {
                        if (options.column.caption == "Valor") {
                            if (fieldData) {
                                var inicial = fNumber.go(fieldData).replace("$-", "-$");
                                options.cellElement.html(inicial);
                            }
                        }


                    } catch (err) {
                    }

                }, loadPanel: {
                    enabled: true
                },
                headerFilter: {
                    visible: true
                }
        , allowColumnResizing: true
        , allowColumnReordering: true
        , columnChooser: {
            enabled: true,
            mode: "select",
            title: "Selector de Columnas"
        },
                groupPanel: {
                    allowColumnDragging: true,
                    visible: true
                }
        , columns: [
             {
                 caption: "Estado",                 
                 dataField: "EstadoFactura",
             }, {
                 caption: "Adquiriente",                 
                 dataField: "StrEmpresaAdquiriente"
             },

              {
                  caption: "Nombre Adquiriente",                  
                  dataField: "NombreAdquiriente"
              },
              {
                  caption: "Fecha Pago",
                  dataField: "DatAdquirienteFechaRecibo",
                  dataType: "date",
                  format: "yyyy-MM-dd"

              },
            {
                caption: "Documento",                
                dataField: "NumeroDocumento",

            },
             {
                 caption: "Cod. Transacción",                 
                 dataField: "idseguridadpago",
             },
              {
                  caption: "Valor",                  
                  dataField: "PagoFactura"
              },

            {
                caption: "Fecha Verificación",
                dataField: "DatFechaVencDocumento",
                dataType: "date",
                format: "yyyy-MM-dd"
            }

        ],

                summary: {
                    groupItems: [{
                        column: "PagoFactura",
                        summaryType: "sum",
                        displayFormat: " {0} Total ",
                        valueFormat: "currency"
                    }]
                    , totalItems: [{
                        name: "Suma",
                        showInColumn: "PagoFactura",
                        displayFormat: "{0}",
                        valueFormat: "currency",
                        summaryType: "custom"

                    },
                    {
                        showInColumn: "DatFechaVencDocumento",
                        displayFormat: "Total : ",
                        alignment: "right"
                    }
                    ],
                    calculateCustomSummary: function (options) {
                        if (options.name === "Suma") {
                            if (options.summaryProcess === "start") {
                                options.totalValue = 0;
                                $('#Total').text("");
                            }
                            if (options.summaryProcess === "calculate") {
                                options.totalValue = options.totalValue + options.value.PagoFactura;
                                $('#Total').text("Total: " + fNumber.go(options.totalValue).replace("$-", "-$"));
                            }
                        }
                    }
                }
        ,
                filterRow: {
                    visible: true
                }

            });

        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });


    }


});


PagosFacturadorApp.controller('PagosAdquirienteController', function PagosAdquirienteController($scope, $http, $location, SrvMaestrosEnum) {

    var now = new Date();
    var Estado;
    var ResolucionesPrefijo;

    var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";
    resolucion = "";

    SrvMaestrosEnum.ObtenerSesion().then(function (data) {
        codigo_facturador = data[0].Identificacion;
        
        SrvMaestrosEnum.ObtenerEnum(1).then(function (dataacuse) {
            items_recibo = dataacuse;
            cargarFiltros();
        });
    });
   
   
    

    function cargarFiltros() {
        $("#FechaInicial").dxDateBox({
            value: now,
            width: '100%',
            displayFormat: "yyyy-MM-dd",
            onValueChanged: function (data) {
                fecha_inicio = new Date(data.value).toISOString();
                $("#FechaFinal").dxDateBox({ min: fecha_inicio });
            }

        });

        $("#FechaFinal").dxDateBox({
            value: now,
            width: '100%',
            displayFormat: "yyyy-MM-dd",
            onValueChanged: function (data) {
                fecha_fin = new Date(data.value).toISOString();
                $("#FechaInicial").dxDateBox({ max: fecha_fin });
            }

        });


        //Define los campos y las opciones
        $scope.filtros =
            {
                EstadoRecibo: {
                    searchEnabled: true,
                    //Carga la data del control
                    dataSource: new DevExpress.data.ArrayStore({
                        data: items_recibo,
                        key: "ID"
                    }),
                    displayExpr: "Descripcion",
                    Enabled: true,
                    placeholder: "Seleccione un Item",
                    onValueChanged: function (data) {
                        estado_recibo = data.value.ID;
                    }
                },
                NumeroDocumento: {
                    placeholder: "Ingrese Número Documento",
                    onValueChanged: function (data) {
                        numero_documento = data.value;
                    }
                },
                Adquiriente: {
                    placeholder: "Ingrese Identificación del Facturador",
                    onValueChanged: function (data) {
                        codigo_adquiriente = data.value;
                    }
                }
            }

        $("#FechaFinal").dxDateBox({ min: now });
        $("#FechaInicial").dxDateBox({ max: now });
        consultar();
    }

    $scope.ButtonOptionsConsultar = {
        text: 'Consultar',
        type: 'default',
        onClick: function (e) {
            consultar2();
        }
    };




    function consultar2() {
        $('#Total').text("");
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        if (resolucion == "")
            resolucion = "*";

        //Obtiene los datos del web api
        //ControladorApi: /Api/Documentos/
        //Datos GET: codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin
        $('#wait').show();
        $http.get('/api/ObtenerPagosAdquiriente?codigo_facturador=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_facturador + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo).then(function (response) {
            console.log(response.data);
            $('#wait').hide();
            $("#gridDocumentos").dxDataGrid({
                dataSource: response.data,
            })
        });
    }


    function consultar() {
        $('#Total').text("");
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        if (resolucion == "")
            resolucion = "*";

        //Obtiene los datos del web api
        //ControladorApi: /Api/Documentos/
        //Datos GET: codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin
        $('#wait').show();
        $http.get('/api/ObtenerPagosFacturador?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&estado_recibo=' + estado_recibo + '&resolucion=' + resolucion).then(function (response) {
            $('#wait').hide();
            $("#gridDocumentos").dxDataGrid({
                dataSource: response.data,
                keyExpr: "NumeroDocumento",
                paging: {
                    pageSize: 20
                },
                stateStoring: {
                    enabled: true,
                    type: "localStorage",
                    storageKey: "storage"
                },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                }
                //Formatos personalizados a las columnas en este caso para el monto
                , onCellPrepared: function (options) {
                    var fieldData = options.value,
                        fieldHtml = "";
                    try {
                        if (options.column.caption == "Valor") {
                            if (fieldData) {
                                var inicial = fNumber.go(fieldData).replace("$-", "-$");
                                options.cellElement.html(inicial);
                            }
                        }


                    } catch (err) {
                    }

                }, loadPanel: {
                    enabled: true
                },
                headerFilter: {
                    visible: true
                }
        , allowColumnResizing: true
        , allowColumnReordering: true
        , columnChooser: {
            enabled: true,
            mode: "select",
            title: "Selector de Columnas"
        },
                groupPanel: {
                    allowColumnDragging: true,
                    visible: true
                }
        , columns: [
             {
                 caption: "Estado",
                 dataField: "EstadoFactura",
             }, {
                 caption: "Facturador",
                 dataField: "StrEmpresaAdquiriente"
             },

              {
                  caption: "Nombre Facturador",
                  dataField: "NombreAdquiriente"
              },
              {
                  caption: "Fecha Pago",
                  dataField: "DatAdquirienteFechaRecibo",
                  dataType: "date",
                  format: "yyyy-MM-dd"

              },
            {
                caption: "Documento",
                dataField: "NumeroDocumento",

            },
             {
                 caption: "Cod. Transacción",
                 dataField: "idseguridadpago",
             },
              {
                  caption: "Valor",
                  dataField: "PagoFactura"
              },

            {
                caption: "Fecha Verificación",
                dataField: "DatFechaVencDocumento",
                dataType: "date",
                format: "yyyy-MM-dd"
            }

        ],

                summary: {
                    groupItems: [{
                        column: "PagoFactura",
                        summaryType: "sum",
                        displayFormat: " {0} Total ",
                        valueFormat: "currency"
                    }]
                    , totalItems: [{
                        name: "Suma",
                        showInColumn: "PagoFactura",
                        displayFormat: "{0}",
                        valueFormat: "currency",
                        summaryType: "custom"

                    },
                    {
                        showInColumn: "DatFechaVencDocumento",
                        displayFormat: "Total : ",
                        alignment: "right"
                    }
                    ],
                    calculateCustomSummary: function (options) {
                        if (options.name === "Suma") {
                            if (options.summaryProcess === "start") {
                                options.totalValue = 0;
                                $('#Total').text("");
                            }
                            if (options.summaryProcess === "calculate") {
                                options.totalValue = options.totalValue + options.value.PagoFactura;
                                $('#Total').text("Total: " + fNumber.go(options.totalValue).replace("$-", "-$"));
                            }
                        }
                    }
                }
        ,
                filterRow: {
                    visible: true
                }

            });

        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });


    }


});