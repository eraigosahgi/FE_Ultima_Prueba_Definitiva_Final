DevExpress.localization.locale(navigator.language);
var opc_pagina = "1314";
var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var GestionPlanesApp = angular.module('GestionPlanesApp', ['ModalEmpresasApp', 'dx', 'AppMaestrosEnum']);
GestionPlanesApp.controller('ConsultaPlanesController', function ConsultaPlanesController($scope, $http, $location) {
    var now = new Date();

    var date = new Date();
    date.setMonth(date.getMonth() - 6);

    var fecha_inicio = date.toISOString();

    var codigo_facturador = "",
           Estado_plan = "",
           Tipo_plan = "",
           tipo_filtro_fecha = 1,          
           fecha_fin = "";

    var estado = "";
    
   

    $http.get('/api/DatosSesion/').then(function (response) {
        codigo_facturador = response.data[0].Identificacion;

        //Parametros
        //Titulo
        //Icono
        CargarSession();

    }, function errorCallback(response) {

        DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
    });
    


   

    function CargarSession() {

            $scope.filtros =
      {
          TipoFiltroFecha: {
              //Carga la data del control
              dataSource: new DevExpress.data.ArrayStore({
                  data: TiposFiltroFecha,
                  key: "ID"
              }),
              displayExpr: "Texto",
              value: TiposFiltroFecha[0],

              onValueChanged: function (data) {
                  tipo_filtro_fecha = data.value.ID;                 
              }
          }
      }

            $("#FechaInicial").dxDateBox({
                value: fecha_inicio,
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



            $("#FechaFinal").dxDateBox({ min: now });
            $("#FechaInicial").dxDateBox({ max: now });

            var DataCheckBox = function (Datos) {
                return new DevExpress.data.CustomStore({
                    loadMode: "raw",
                    key: "ID",
                    load: function () {
                        return JSON.parse(JSON.stringify(Datos));
                    }
                });
            };

            var DataCheckBox1 = function (Datos) {
                return new DevExpress.data.CustomStore({
                    loadMode: "raw",
                    key: "ID",
                    load: function () {
                        return JSON.parse(JSON.stringify(Datos));
                    }
                });
            };

            $("#Tipoplan").dxDropDownBox({
                valueExpr: "ID",
                placeholder: "Seleccione un Item",
                displayExpr: "Texto",
                showClearButton: true,
                dataSource: DataCheckBox1(Lista_Tipo_plan),
                contentTemplate: function (e) {
                    var value = e.component.option("value"),
                        $dataGrid1 = $("<div>").dxDataGrid({
                            dataSource: e.component.option("dataSource"),
                            allowColumnResizing: true,
                            columns:
                                [
                                     {
                                         caption: "Descripción",
                                         dataField: "Texto",
                                         title: "Descripción",
                                         width: 300

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
                                Tipo_plan = keys;
                            }
                        });

                    dataGrid1 = $dataGrid1.dxDataGrid("instance");

                    e.component.on("valueChanged", function (args) {
                        var value = args.value;
                        dataGrid1.selectRows(value, false);
                    });

                    return $dataGrid1;
                }
            });


            $("#Estadoplan").dxDropDownBox({
                valueExpr: "ID",
                placeholder: "Seleccione un Item",
                displayExpr: "Texto",
                showClearButton: true,
                dataSource: DataCheckBox(Lista_Estado_plan),
                contentTemplate: function (e) {
                    var value = e.component.option("value"),
                        $dataGrid = $("<div>").dxDataGrid({
                            dataSource: e.component.option("dataSource"),
                            allowColumnResizing: true,
                            columns:
                                [
                                     {
                                         caption: "Descripción",
                                         dataField: "Texto",
                                         title: "Descripción",
                                         width: 300

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
                                Estado_plan = keys;
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


            $scope.ButtonOptionsConsultar = {
                text: 'Consultar',
                type: 'default',
                onClick: function (e) {
                    CargarConsulta(codigo_facturador);
                }
            };

            CargarConsulta(codigo_facturador);

    }

    function CargarConsulta(Facturador) {

        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        $("#wait").show();
        $http.get('/api/ConsultarPlanesFacturador?Identificacion=' + Facturador + '&TipoPlan=' + Tipo_plan + '&Estado=' + Estado_plan + '&TipoFecha=' + tipo_filtro_fecha + '&FechaInicio=' + fecha_inicio + '&FechaFin=' + fecha_fin).then(function (response) {
            $("#wait").hide();
            try {
                $("#grid").dxDataGrid({
                    dataSource: response.data,
                    paging: {
                        pageSize: 20

                    },
                    headerFilter: {
                        visible: true
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [5, 10, 20],
                        showInfo: true
                    },
                    groupPanel: {
                        allowColumnDragging: true,
                        visible: true
                    },
                    masterDetail: {
                        enabled: true,
                        template: function (container, options) {

                            //************************************************************************************
                            $('#Total').text("");
                            if (fecha_inicio == "")
                                fecha_inicio = now.toISOString();

                            if (fecha_fin == "")
                                fecha_fin = now.toISOString();


                            var resolucion = "*";
                            var tipo_filtro_fecha = 1;
                            var Muestradetalle = true;

                            $http.get('/api/DocumentosPlanes?IdPlan=' + options.data.id).then(function (response) {
                                if (response.data.length > 0) {


                                    $("<div style='padding:20px'>").addClass("demo-container").dxDataGrid({
                                        dataSource: response.data,
                                        keyExpr: "NumeroDocumento",
                                        paging: {
                                            pageSize: 10
                                        }

                                        , pager: {
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
                                            {
                                                caption: "Fecha Documento",
                                                dataField: "DatFechaDocumento",
                                                dataType: "date",
                                                format: "yyyy-MM-dd"

                                            },

                                            {
                                                caption: "Documento",
                                                dataField: "NumeroDocumento",

                                            },
                                            {
                                                caption: "Facturador",
                                                dataField: "Cod_Facturador",

                                            },
                                            {
                                                caption: "Nombre Facturador",
                                                dataField: "NombreFacturador",

                                            },


                                        ]

                                    }).appendTo(container);
                                    $('div[class="dx-datagrid"]').attr("style", "padding:5px;");
                                }
                            }, function errorCallback(response) {
                                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
                            });


                            //*************************************************************************************
                        }
                    },

                    //Formatos personalizados a las columnas en este caso para el monto
                    onCellPrepared: function (options) {
                        var fieldData = options.value,
                            fieldHtml = "";
                        try {
                            if (options.columnIndex == 4) {//Columna de valor Total
                                if (fieldData) {
                                    var inicial = fNumber.go(fieldData);
                                    options.cellElement.html(inicial);
                                }
                            }                           

                            if (options.data.CodigoEstado == 0) {
                                estado = " style='color:green; cursor:default;' title='Habilitado'";
                            }
                            if (options.data.CodigoEstado == 1) {
                                estado = " style='color:red; cursor:default;' title='Inhabilitado'";
                            }
                            if (options.data.CodigoEstado == 2) {
                                estado = " style='color:grey; cursor:default;' title='Procesado'";
                            }


                        } catch (err) {

                        }
                    }
                    , loadPanel: {
                        enabled: true
                    }
                      , allowColumnResizing: true
                 , columns: [
                     {
                         cssClass: "col-md-1 col-xs-1",
                         width: 50,
                         cellTemplate: function (container, options) {
                             $("<div style='text-align:center'>")
                                 //.append($("<a taget=_self class='icon-pencil3' title='Editar' href='GestionPlanesTransacciones.aspx?IdSeguridad=" + options.data.id + "'>"))
                                 .appendTo(container);
                         }
                     },
                     {
                         cssClass: "col-md-2 col-xs-4",
                         caption: "Fecha",
                         dataField: "Fecha",
                         dataType: "date",
                         format: "yyyy-MM-dd HH:mm"
                     },
                      {
                          cssClass: "col-md-2 col-xs-3",
                          caption: "Empresa Compra",
                          dataField: "EmpresaFacturador"
                      },

                      {
                          cssClass: "col-md-1 col-xs-1",
                          caption: "Valor",
                          dataField: "Valor"
                      },
					  {
					  	cssClass: "col-md-1 col-xs-1",
					  	caption: "Transacciones",
					  	dataField: "TCompra"
					  }
                      ,
                     {
                         cssClass: "col-md-1 col-xs-2",
                         caption: "Procesadas",
                         dataField: "TProcesadas"
                     }
                     ,
                     {
                         cssClass: "col-md-1 col-xs-2",
                         caption: "Saldo",
                         dataField: "Saldo"
                     },
                  {
                          cssClass: "col-md-2 hidden-xs",
                          caption: "Empresa",
                          dataField: "Empresa",

                      },
                     {
                         cssClass: "col-md-2 hidden-xs",
                         caption: "Usuario",
                         dataField: "Usuario"
                     }
                     ,
                     {
                         cssClass: "col-md-2",
                         caption: "Tipo",
                         dataField: "Tipoproceso"
                     }

                     ,
                      {
                          cssClass: "col-md-1 col-xs-1",
                          caption: 'Estado',
                          dataField: 'Estado',
                          cellTemplate: function (container, options) {
                              $("<div style='text-align:center'>")
                                  .append($("<a taget=_self class='icon-circle2'" + estado + ">"))
                                  .appendTo(container);
                          }
                      }
                 ], summary: {
                     groupItems: [{
                         column: "Valor",
                         summaryType: "sum",
                         displayFormat: " {0} Total ",
                         valueFormat: "currency"
                     }]

                    , totalItems: [{
                        column: "Valor",
                        displayFormat: "{0}",
                        valueFormat: "currency",
                        summaryType: "sum"
                    }, {
                        column: "TCompra",
                        summaryType: "sum",
                        valueFormat: 'fixedPoint',
                        displayFormat: '{0}'

                    }
                    ]
                 }
                    , filterRow: {
                        visible: true
                    }
                });


            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 7000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
        });
    }

});


//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
    for (var i = 0; i < miArray.length; i += 1) {
        if (ID == miArray[i].ID) {
            return i;
        }
    }
}

var EstadosPlanes =
    [
        { ID: 1, Texto: 'Inhabilitar' },
        { ID: 0, Texto: 'Habilitar' },

    ];
var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Recarga' },
    { ID: "2", Texto: 'Vencimiento' }
    ];


var Lista_Tipo_plan =
    [
    { ID: "1", Texto: 'Cortesía' },
    { ID: "2", Texto: 'Compra' },
    { ID: "3", Texto: 'Post-Pago' }
    ];

var Lista_Estado_plan =
    [
    { ID: "0", Texto: 'Habilitado' },
    { ID: "1", Texto: 'Inhabilitado' },
    { ID: "2", Texto: 'Procesado' }
    ];