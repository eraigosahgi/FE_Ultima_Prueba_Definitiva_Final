DevExpress.localization.locale(navigator.language);

var path = window.location.pathname;
var ruta = window.location.href;
ruta = ruta.replace(path, "/");
document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/SrvDocumentos.js"></scr' + 'ipt>');

angular.module('ProcesarDocumentosApp', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento'])

.controller('ProcesarDocumentosController', function DocAdquirienteController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento) {

    var codigo_adquiente = "", numero_documento = "", estado_recibo = "", fecha_inicio = "", fecha_fin = "", Estado = [], now = new Date();

    SrvMaestrosEnum.ObtenerEnum(0, "privado").then(function (data) {
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
            name: "txtf",
            value: now,
            width: '100%',
            max: fecha_fin,
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
            min: fecha_inicio,
            onValueChanged: function (data) {
                fecha_fin = new Date(data.value).toISOString();
                $("#FechaInicial").dxDateBox({ max: fecha_fin });
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
                                     title: "Descripcion",
                                     width: 500

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
                            estado_recibo = keys;
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
        $('#panelresultado').hide();
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();


        SrvDocumento.ObtenerDocumentos(numero_documento, estado_recibo, fecha_inicio, fecha_fin).then(function (data) {
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
                        dataField: "IdSeguridad",
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
                }

                , onSelectionChanged: function (selectedEstado) {
                    var lista = '';
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

            SrvDocumento.ProcesarDocumentos($scope.documentos).then(function (data) {
                $("#gridDocumentosProcesados").dxDataGrid({
                    dataSource: data,
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
                        width: '15%',
                        format: "yyyy-MM-dd HH:mm"

                    },

                   {
                       caption: "Fecha Ultimo Proceso",
                       dataField: "FechaUltimoProceso",
                       dataType: "date",
                       width: '15%',
                       format: "yyyy-MM-dd HH:mm"

                   },
                    {
                        caption: "Resultado",
                        width: '70%',
                        dataField: "DescripcionProceso"
                    },
               {
                   caption: "Aceptacion",
                   dataField: "Aceptacion",
                   hidingPriority: 0
               },
                     {
                         caption: "CodigoRegistro",
                         dataField: "CodigoRegistro",
                         hidingPriority: 1
                     },
                     {
                         caption: "Cufe",
                         dataField: "Cufe",
                         hidingPriority: 2
                     },
                     {
                         caption: "Documento",
                         dataField: "Documento",
                         hidingPriority: 3

                     },
                     {
                         caption: "EstadoDian",
                         dataField: "EstadoDian",
                         hidingPriority: 4

                     },
                     {
                         caption: "IdDocumento",
                         dataField: "IdDocumento",
                         hidingPriority: 5
                     },
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
                    }

               ]
                }).dxDataGrid("instance");

                consultar();
                $('#panelresultado').show();
            });
        }

    }

});

