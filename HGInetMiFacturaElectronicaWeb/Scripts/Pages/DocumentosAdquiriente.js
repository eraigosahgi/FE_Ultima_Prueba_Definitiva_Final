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

            console.log(response.data[0]);

            codigo_adquiente = response.data[0].Identificacion;
                      
            consultar();
        }),function errorCallback(response) {                
           Mensaje(response.data.ExceptionMessage, "error");
        };
    
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
                
        console.log("Ingresó al evento del botón");

        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        console.log("FILTROS DE BÚSQUEDA:\n" + "codigo_adquiente:", codigo_adquiente, "\nnumero_documento:", numero_documento, "\nestado_recibo:", estado_recibo, "\nfecha_inicio:", fecha_inicio, "\nfecha_fin:", fecha_fin);

        //Obtiene los datos del web api
        //ControladorApi: /Api/Documentos/
        //Datos GET: codigo_adquiente - numero_documento - estado_recibo - fecha_inicio - fecha_fin
        $('#wait').show();
        $http.get('/api/Documentos?codigo_adquiente=' + codigo_adquiente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
        $('#wait').hide();
            console.log("Ingresó a cargar la data.");         
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
                        if (options.columnIndex == 4) {//Columna de valor Total
                            if (fieldData) {                                
                                var inicial = fNumber.go(fieldData);                                
                                options.cellElement.html(inicial);                                
                            }
                        }
                        //Valida el formato de fecha en la configuracion de fecha
                        if (options.columnIndex == 3 || options.columnIndex == 2) {
                            if (fieldData) {
                                var inicial = convertDateFormat(options.text);
                                options.cellElement.html(inicial);
                            }
                        }
                    } catch (err) {
                       
                    }
                }
                ,columns: [
                    {
                        caption: "Archivos",
                        cssClass: "col-md-1 col-xs-2",
                        cellTemplate: function (container, options) {
                            $("<div>")
                                .append($("<a target='_blank' class='icon-file-pdf' href='" + options.data.Pdf + "'> &nbsp;&nbsp; <a target='_blank' class='icon-file-xml' href='" + options.data.Xml + "'>"))
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
                        cssClass:"hidden-xs col-md-2",                        
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
var items =
    [
    { ID: "*", Texto: 'Obtener Todos' },
    { ID: "0", Texto: 'Pendiente' },
    { ID: "1", Texto: 'Aprobado' },
    { ID: "2", Texto: 'Rechazado' }
    ];

