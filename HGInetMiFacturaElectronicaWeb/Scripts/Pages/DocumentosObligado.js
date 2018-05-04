DevExpress.localization.locale(navigator.language);


var DemoApp = angular.module('DocObligadoApp', ['dx']);
DemoApp.controller('DocObligadoController', function DemoController($scope, $http, $location) {

    var now = new Date();

    var codigo_facturador = "811021438",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";

    //Define los campos y las opciones
    $scope.filtros =
        {
            //Control defecto ingreso de datos.
            FechaInicial: {
                type: "date",
                value: now,
                displayFormat: "yyyy-MM-dd",
                onValueChanged: function (data) {
                    console.log("FechaInicial", new Date(data.value).toISOString());
                    fecha_inicio = new Date(data.value).toISOString();
                }
            },
            FechaFinal: {
                type: "date",
                value: now,
                displayFormat: "yyyy-MM-dd",
                onValueChanged: function (data) {
                    console.log("FechaFinal", new Date(data.value).toISOString());
                    fecha_fin = new Date(data.value).toISOString();
                }
            }, EstadoDian: {
                searchEnabled: true,
                //Carga la data del control
                dataSource: new DevExpress.data.ArrayStore({
                    data: items_dian,
                    key: "ID"
                }),
                displayExpr: "Texto",
                Enabled: true,
                placeholder: "Seleccione un Item",
                onValueChanged: function (data) {
                    console.log("EstadoDian", data.value.id);
                    estado_dian = data.value.id;
                }
            },
            EstadoRecibo: {
                searchEnabled: true,
                //Carga la data del control
                dataSource: new DevExpress.data.ArrayStore({
                    data: items_recibo,
                    key: "ID"
                }),
                displayExpr: "Texto",
                Enabled: true,
                placeholder: "Seleccione un Item",
                onValueChanged: function (data) {
                    console.log("EstadoRecibo", data.value.id);
                    estado_recibo = data.value.id;
                }
            },
            NumeroDocumento: {
                placeholder: "Ingrese Número Documento",
                onValueChanged: function (data) {
                    console.log("NumeroDocumento", data.value);
                    numero_documento = data.value;
                }
            },
            Adquiriente: {
                placeholder: "Ingrese Identificación del Adquiriente",
                onValueChanged: function (data) {
                    console.log("Adquiriente", data.value);
                    codigo_adquiriente = data.value;
                }
            }
        }

    var mensaje_acuse = "";

    $scope.ButtonOptionsConsultar = {
        text: 'Consultar',
        type: 'default',
        onClick: function (e) {

            console.log("Ingresó al evento del botón");

            if (fecha_inicio == "")
                fecha_inicio = now.toISOString();

            if (fecha_fin == "")
                fecha_fin = now.toISOString();

            console.log("FILTROS DE BÚSQUEDA:\n" + "codigo_adquiriente:", codigo_adquiriente, "\nnumero_documento:", numero_documento, "\nestado_recibo:", estado_recibo, "\nfecha_inicio:", fecha_inicio, "\nfecha_fin:", fecha_fin);

            //Obtiene los datos del web api
            //ControladorApi: /Api/Documentos/
            //Datos GET: codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin
            $http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&estado_dian=' + estado_dian + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {

                console.log("Ingresó a cargar la data.");

                $scope.dataGridOptions = {
                    dataSource: response.data,
                    paging: {
                        pageSize: 10
                    },
                    pager: {
                        showPageSizeSelector: true,
                        allowedPageSizes: [5, 10, 20],
                        showInfo: true
                    },
                    columns: [
                        {
                            caption: "Archivos",
                            cellTemplate: function (container, options) {
                                $("<div>")
                                    .append($("<a target='_blank' class='icon-file-pdf' href='" + options.data.StrUrlArchivoPdf + "'> &nbsp;&nbsp; <a target='_blank' class='icon-file-xml' href='" + options.data.StrUrlArchivoUbl + "'>"))
                                    .append($(""))
                                    .appendTo(container);
                            }
                        },
                        {
                            caption: "Documento",
                            dataField: "NumeroDocumento",
                        },
                        {
                            caption: "Fecha Documento",
                            dataField: "DatFechaDocumento",
                            dataType: "date",
                            validationRules: [{
                                type: "required",
                                message: "El campo Fecha es obligatorio."
                            }]
                        },
                        {
                            caption: "Fecha Vencimiento",
                            dataField: "DatFechaVencDocumento",
                            dataType: "date",
                            validationRules: [{
                                type: "required",
                                message: "El campo Fecha es obligatorio."
                            }]
                        },
                        {
                            caption: "Valor Total",
                            dataField: "IntVlrTotal"
                        },
                         {
                             caption: "Identificación Adquiriente",
                             dataField: "IdentificacionAdquiriente"
                         },
                          {
                              caption: "Nombre Adquiriente",
                              dataField: "NombreAdquiriente"
                          },
                          {
                              caption: "Estado",
                              dataField: "EstadoFactura",
                          },
                          {
                              caption: "Estado Acuse",
                              dataField: "EstadoAcuse",
                          },
                          {
                              caption: "Motivo Rechazo",
                              dataField: "MotivoRechazo",
                          },
                          {
                              dataField: "",
                              cellTemplate: function (container, options) {
                                  $("<div>")
                                      .append($("<a target='_blank' class='icon-envelop3' href='http://app.mifacturaenlinea.com.co/pages/facturae/AcuseRecibo.aspx?id=" + options.data.StrIdSeguridad + "'></a>"))
                                      .appendTo(container);
                              }
                          }
                    ],
                    filterRow: {
                        visible: true
                    },
                };
                console.log("DATOS DE RETORNO DE WEB API", response.data);

                console.log("Salió del método");
            });
        }
    };


});

//Datos del control EstadoRecibo.
var items_recibo =
    [
    { ID: "*", Texto: 'Obtener Todos' },
    { ID: "0", Texto: 'Pendiente' },
    { ID: "1", Texto: 'Aprobado' },
    { ID: "2", Texto: 'Rechazado' }
    ];

var items_dian =
    [
    { ID: "*", Texto: 'Obtener Todos' },
    { ID: "0", Texto: 'Pendiente' },
    { ID: "1", Texto: 'Aprobado' },
    { ID: "2", Texto: 'Rechazado' }
    ];