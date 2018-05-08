DevExpress.localization.locale(navigator.language);


var DocObligadoApp = angular.module('DocObligadoApp', ['dx']);
DocObligadoApp.controller('DocObligadoController', function DocObligadoController($scope, $http, $location) {

    var now = new Date();

    var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";


    $http.get('/api/DatosSesion/').then(function (response) {

    	console.log(response.data[0]);

    	codigo_facturador = response.data[0].Identificacion;
    	consultar();
    });


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
                    console.log("EstadoDian", data.value.ID);
                    estado_dian = data.value.ID;
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
                    console.log("EstadoRecibo", data.value.ID);
                    estado_recibo = data.value.ID;
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
            consultar();
        }
    };

    function consultar() {

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
            console.log("Datossss", response.data);
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
                columns: [
                    {
                        caption: "Archivos",
                        cellTemplate: function (container, options) {
                            $("<div>")
                                .append($("<a target='_blank' class='icon-file-pdf' href='" + options.data.Pdf + "'>&nbsp;&nbsp;<a target='_blank' class='icon-file-xml' href='" + options.data.Xml + "'>&nbsp;&nbsp;<a target='_blank' class='icon-mail-read' href='" + "#" + "'>"))
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
                                  .append($("<a target='_blank' class='icon-check' href='" + options.data.RutaAcuse + "'>Acuse</a>"))
                                  .appendTo(container);
                          }
                      }
                ],
                filterRow: {
                    visible: true
                },
            });
            console.log("DATOS DE RETORNO DE WEB API", response.data);

            console.log("Salió del método");
        });

    }


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
    { ID: "11", Texto: 'Fin Proceso' }
    ];
