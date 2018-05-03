DevExpress.localization.locale(navigator.language);

var DemoApp = angular.module('DocAdquirienteApp', ['dx']);
DemoApp.controller('DocAdquirienteController', function DemoController($scope, $http, $location) {

    var now = new Date();

    var codigo_adquiente = "1152708377",
           numero_documento = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "";

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
            },
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

            console.log("FILTROS DE BÚSQUEDA:\n" + "codigo_adquiente:", codigo_adquiente, "\nnumero_documento:", numero_documento, "\nestado_recibo:", estado_recibo, "\nfecha_inicio:", fecha_inicio, "\nfecha_fin:", fecha_fin);

            //Obtiene los datos del web api
            //ControladorApi: /Api/Documentos/
            //Datos GET: codigo_adquiente - numero_documento - estado_recibo - fecha_inicio - fecha_fin
            $http.get('/api/Documentos?codigo_adquiente=' + codigo_adquiente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {

                console.log("Ingresó a cargar la data.");

                $scope.dataGridOptions = {
                    dataSource: response.data,
                    "export": {
                        enabled: true,
                        fileName: "Reporte" + new Date(),
                        allowExportSelectedData: true
                    },
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
                             caption: "Identificación Facturador",
                             dataField: "IdentificacionFacturador"
                         },
                          {
                              caption: "Nombre Facturador",
                              dataField: "NombreFacturador"
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
                                      .append($("<a target='_blank' href='http://app.mifacturaenlinea.com.co/pages/facturae/AcuseRecibo.aspx?id=" + options.data.StrIdSeguridad + "'>Acuse</a>"))
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
var items =
    [
    { ID: "*", Texto: 'Obtener Todos' },
    { ID: "0", Texto: 'Pendiente' },
    { ID: "1", Texto: 'Aprobado' },
    { ID: "2", Texto: 'Rechazado' }
    ];





function eventoLink(dato) {
    alert("evento");
}
