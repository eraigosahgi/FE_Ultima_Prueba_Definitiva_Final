DevExpress.localization.locale(navigator.language);

var DocAdquirienteApp = angular.module('DocAdquirienteApp', ['dx']);
DocAdquirienteApp.controller('DocAdquirienteController', function DocAdquirienteController($scope, $http, $location) {

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
                placeholder: "Ingrese Número Documento",
                onValueChanged: function (data) {
                    numero_documento = data.value;
                }
            }
            
        }

    var mensaje_acuse = "";
    $("#FechaFinal").dxDateBox({ min: now });
    $("#FechaInicial").dxDateBox({ max: now });

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
        $http.get('/api/Documentos?codigo_adquiente=' + codigo_adquiente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
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
                }
                //Formatos personalizados a las columnas en este caso para el monto
                , onCellPrepared: function (options) {
                    var fieldData = options.value,
                        fieldHtml = "";
                    try {
                        if (options.column.caption == "Valor Total") {
                            if (fieldData) {
                                var inicial = fNumber.go(fieldData);
                                options.cellElement.html(inicial);
                            }
                        }
                    } catch (err) {
                        DevExpress.ui.notify(err.message, 'error', 3000);
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
                    emptyPanelText: "Prueba"
                },
                groupPanel: {
                    allowColumnDragging: true,
                    emptyPanelText: "Arrastre la columna aqui",
                    visible: true
                }
                , columns: [
                    {
                        caption: "Archivos",
                        cssClass: "col-md-1 col-xs-2",
                        cellTemplate: function (container, options) {
                            var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

                            if (options.data.Pdf)
                                visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer'";
                            else
                                options.data.Pdf = "#";

                            if (options.data.Xml)
                                visible_xml = "href='" + options.data.Xml + "' style='pointer-events:auto;cursor: pointer'";
                            else
                                options.data.Xml = "#";
                            $("<div>")
                                .append($("<a target='_blank' class='icon-file-pdf'  " + visible_pdf + ">&nbsp;&nbsp;<a target='_blank' class='icon-file-xml' " + visible_xml + ">&nbsp;&nbsp;"))
                                .append($(""))
                                .appendTo(container);
                        }
                    },
                    {
                        caption: "Documento",
                        dataField: "NumeroDocumento",
                        cssClass: "col-md-1 col-xs-3",
                    },
                    {
                        caption: "Fecha Documento",
                        dataField: "DatFechaDocumento",
                        dataType: "date",
                        format: "yyyy-MM-dd ",
                        cssClass: "col-md-2 col-xs-3",
                        validationRules: [{
                            type: "required",
                            message: "El campo Fecha es obligatorio."
                        }]
                    },
                    {
                        caption: "Fecha Vencimiento",
                        dataField: "DatFechaVencDocumento",
                        dataType: "date",
                        format: "yyyy-MM-dd",
                        cssClass: "hidden-xs col-md-2",
                        validationRules: [{
                            type: "required",
                            message: "El campo Fecha es obligatorio."
                        }]
                    },
                    {
                        caption: "Valor Total",
                        dataField: "IntVlrTotal",
                        cssClass: "col-md-1 col-xs-2",
                        width: '12%',
                        Type: Number,
                    }
                    ,
                     {
                         caption: "Identificación Facturador",
                         cssClass: "hidden-xs col-md-1",
                         dataField: "IdentificacionFacturador"

                     },
                      {
                          caption: "Nombre Facturador",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "NombreFacturador"
                      },
                      {
                          caption: "Estado",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "EstadoFactura",
                      },
                      {
                          caption: "Estado Acuse",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "EstadoAcuse",
                      },
                      {
                          caption: "Motivo Rechazo",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "MotivoRechazo",
                      },
                      {
                          dataField: "",
                          cssClass: "col-md-1 col-xs-2",
                          cellTemplate: function (container, options) {
                              $("<div>")
                                  .append($("<a target='_blank' href='" + options.data.RutaAcuse + "'>Acuse</a>"))
                                  .appendTo(container);
                          }
                      }
                ],
                summary: {
                    groupItems: [{
                        column: "IntVlrTotal",
                        summaryType: "sum",
                        displayFormat: " {0} Total ",
                        valueFormat: "currency"
                    }],
                    totalItems: [{
                        column: "IntVlrTotal",
                        summaryType: "sum",
                        displayFormat: "{0}",
                        valueFormat: "currency"
                    }]
                },
                filterRow: {
                    visible: true
                },
            });
        });
    }


});



//Datos del control EstadoRecibo.
var items =
    [
    { ID: "*", Texto: 'Obtener Todos' },
    { ID: "0", Texto: 'Pendiente' },
    { ID: "1", Texto: 'Aprobado' },
    { ID: "2", Texto: 'Rechazado' }
    ];

