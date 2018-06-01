DevExpress.localization.locale(navigator.language);

var AcuseConsultaApp = angular.module('AcuseConsultaApp', ['dx']);
AcuseConsultaApp.controller('AcuseConsultaController', function AcuseConsultaController($scope, $http, $location) {

    var now = new Date();

    var codigo_facturador = "",
               codigo_adquiriente = "",
               numero_documento = "",
               fecha_inicio = "",
               fecha_fin = "",
               estado_acuse = "";


    $http.get('/api/DatosSesion/').then(function (response) {
        codigo_facturador = response.data[0].Identificacion;
        consultar();
    });


    $scope.filtros =
           {
               //Control defecto ingreso de datos.
               FechaInicial: {
                   type: "date",
                   value: now,
                   displayFormat: "yyyy-MM-dd",
                   onValueChanged: function (data) {
                       fecha_inicio = new Date(data.value).toISOString();
                   }
               },
               FechaFinal: {
                   type: "date",
                   value: now,
                   displayFormat: "yyyy-MM-dd",
                   onValueChanged: function (data) {
                       fecha_fin = new Date(data.value).toISOString();
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
                       estado_acuse = data.value.ID;
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

    $scope.ButtonOptionsConsultar = {
        text: 'Consultar',
        type: 'default',
        onClick: function (e) {
            consultar();
        }
    };


    function consultar() {
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        //Obtiene los datos del web api
        //ControladorApi: /Api/Documentos/
        //Datos GET: codigo_facturador, codigo_adquiriente, numero_documento, estado_recibo, fecha_inicio, fecha_fin
        $('#wait').show();
        $http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&codigo_adquiriente=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_acuse + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
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
                    var fieldData = options.value;
                    try {
                        //Valida el formato de fecha en la configuracion de fecha
                        if (options.columnIndex == 4) {
                            if (fieldData) {
                                var inicial = convertDateFormat(options.text);
                                options.cellElement.html(inicial);
                            }
                        }

                    } catch (err) {
                        DevExpress.ui.notify(err.message, 'error', 3000);
                    }

                }

                , columns: [
                    {
                        caption: "Archivos",
                        cssClass: "col-xs-3 col-md-1",
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
                                .append(
                                    $("<a target='_blank' class='icon-file-pdf'  " + visible_pdf + ">&nbsp;&nbsp;<a target='_blank' class='icon-file-xml' " + visible_xml + ">&nbsp;&nbsp;"))
                                .append($(""))
                                .appendTo(container);
                        }
                    },
                     {
                         caption: "Identificación Adquiriente",
                         cssClass: "hidden-xs col-md-1",
                         dataField: "IdentificacionAdquiriente"
                     },
                      {
                          caption: "Nombre Adquiriente",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "RazonSocial"
                      },
                      {
                          caption: "Número Documento",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "NumeroDocumento",
                      },
                    {
                        caption: "Fecha Respuesta",
                        dataField: "FechaRespuesta",
                        dataType: "date",
                        cssClass: "col-xs-3 col-md-1",
                        validationRules: [{
                            type: "required",
                            message: "El campo Fecha es obligatorio."
                        }]
                    },
                       {
                           caption: "Estado Acuse",
                           cssClass: "hidden-xs col-md-1",
                           dataField: "Estado",
                       },
                      {
                          caption: "Motivo Rechazo",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "MotivoRechazo",
                      },
                      {
                          dataField: "",
                          cssClass: "col-xs-2 col-md-1",
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
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });

    }


});

var items_recibo =
    [
    { ID: "*", Texto: 'Obtener Todos' },
    { ID: "1", Texto: 'Aprobado' },
    { ID: "2", Texto: 'Rechazado' }
    ];