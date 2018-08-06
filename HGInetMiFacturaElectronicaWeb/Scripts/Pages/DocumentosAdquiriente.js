DevExpress.localization.locale('es-ES');

var path = window.location.pathname;
var ruta = window.location.href;
ruta = ruta.replace(path, "/");
document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/MaestrosEnum.js"></script>');


var Estado;
var DocAdquirienteApp = angular.module('DocAdquirienteApp', ['dx', 'AppMaestrosEnum']);
//Controlador para la consulta de documentos Adquiriente
DocAdquirienteApp.controller('DocAdquirienteController', function DocAdquirienteController($scope, $rootScope, $http, $location, SrvMaestrosEnum) {   

    var now = new Date();

    var codigo_adquiente = "",
           numero_documento = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "";

    SrvMaestrosEnum.ObtenerEnum(1).then(function (data) {
        Estado = data;
        cargarFiltros();
    });

    $http.get('/api/DatosSesion/').then(function (response) {
        codigo_adquiente = response.data[0].Identificacion;

        consultar();
    }), function errorCallback(response) {
        Mensaje(response.data.ExceptionMessage, "error");
    };

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
                        data: Estado,
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
        $('#Total').text("");
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
                        if (options.column.caption == "Valor Total") {
                            if (fieldData) {
                                var inicial = fNumber.go(fieldData).replace("$-", "-$");
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
                    mode: "select"
                },
                groupPanel: {
                    allowColumnDragging: true,
                    visible: true
                }
                , columns: [
                    {
                        caption: "  Lista de Archivos",
                        cssClass: "col-md-1 col-xs-2",
                        width: "auto",
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
                        cssClass: "col-md-1 col-xs-3",
                        validationRules: [{
                            type: "required",
                            message: "El campo Fecha es obligatorio."
                        }]
                    },
                    {
                        caption: "Fecha Vencimiento",
                        dataField: "DatFechaVencDocumento",
                        dataType: "date",
                        format: "yyyy-MM-dd ",
                        cssClass: "hidden-xs col-md-1",
                        validationRules: [{
                            type: "required",
                            message: "El campo Fecha es obligatorio."
                        }]
                    },
                    {
                        caption: "Valor Total",
                        dataField: "IntVlrTotal",
                        cssClass: "col-md-1 col-xs-1",
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
                           caption: "Tipo Documento",
                           cssClass: "hidden-xs col-md-1",
                           dataField: "tipodoc"
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
                      },
                {

                    cssClass: "col-md-1 ",
                    width: "80px",
                    alignment:"center",

                    cellTemplate: function (container, options) {
                                                
                        var imagen = "";
                        if (options.data.tipodoc == 'Factura' && options.data.poseeIdComercio == 1) {
                            imagen = "<img src='../../Scripts/Images/Pagar.png' data-toggle='modal'  data-target='#modal_Pagos_Electronicos' />";
                        } else {
                            imagen = "";

                        }

                        if (options.data.tipodoc == 'Factura' && options.data.poseeIdComercio == 1 && options.data.FacturaCenlada == 8) {//aqui se debe colocar el status que indica el pago de la factura
                            imagen = "<a class='icon-eye' data-toggle='modal' data-target='#modal_Pagos_Electronicos' ></a>";
                        }
                        

                        $("<div>").removeClass("dx-button-content")
                        // ng-show=' + options.data.tipodoc + '=='Factura'

                        $(imagen).dxButton({
                            onClick: function () {
                                // "68c671b9-90ac-4b35-957a-89eae06f3052"
                                $rootScope.ConsultarPago(options.data.StrIdSeguridad, options.data.IntVlrTotal);
                                /*
                                $http.get('/api/Documentos?strIdSeguridad=' + options.data.StrIdSeguridad + '&pago=true').then(function (response) {
                                    window.open(response.data, "Zona de Pago", $(window).height(), $(window).width());
                                }, function (error) {
                                    DevExpress.ui.notify(error, 'error', 6000);
                                });
                                */
                            }
                        }).removeClass("dx-button-content")
                            .append($("")).removeClass("dx-button dx-button-normal dx-widget")
                            .appendTo(container).removeClass("dx-button-default")
                        $("</div>")
                    }
                },/*
                {
                    width: "auto",                    
                    cellTemplate: function (container, options) {

                        $scope.Poseecodigocomercio = (options.data.IntComercioId != null) ? true : false;
                        var visible_pago = "style='pointer-events:auto;cursor: not-allowed;'";
                        
                        var imagen = "";
                        if (options.data.tipodoc == 'Factura' && options.data.poseeIdComercio==1) {
                            imagen = "<a class='btn-primary' >Pagar</a>'";
                        } else {
                            imagen = "";

                        }

                        $("<div>")
                        .append(
                                    //"<a class='icon-eye' data-toggle='modal' data-target='#modal_Pagos_Electronicos' style='margin-left:5%; font-size:19px'></a>"
                                    $(imagen).dxButton({
                                        onClick: function () {
                                            //LLamo a la funcion del controlador de pagos                                            
                                            $rootScope.ConsultarPago("68c671b9-90ac-4b35-957a-89eae06f3052", options.data.IntVlrTotal);
                                        }
                                    }).removeClass("dx-button dx-button-normal dx-widget")

                            )
                                .appendTo(container);
                    },*/



                ],
                summary: {
                    groupItems: [{
                        column: "IntVlrTotal",
                        summaryType: "sum",
                        displayFormat: " {0} Total ",
                        valueFormat: "currency"
                    }]
                    , totalItems: [{
                        name: "Suma",
                        showInColumn: "IntVlrTotal",
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
                                options.totalValue = options.totalValue + options.value.IntVlrTotal;
                                $('#Total').text("Total: " + fNumber.go(options.totalValue).replace("$-", "-$"));
                            }
                        }
                    }
                },
                filterRow: {
                    visible: true
                },
            });
        });

    }






});

//Controlador de Pagos
DocAdquirienteApp.controller('ModalPagosController', function ModalPagosController($scope, $rootScope, $location, $http) {

    $("#summary").dxValidationSummary({});
    $scope.valoraPendiente = 0;

    $rootScope.ConsultarPago = function (IdSeguridad, Monto) {
        $("#PanelPago").show();

        ///Este es el monto total de la factura
        $scope.montoFactura = Monto;

        ///Id de seguridad del Documento
        $scope.IdSeguridad = IdSeguridad;

        ///Monto total cancelado hasta la fecha
        $scope.TotalPago = 0;

        ///Valor a Cancelar
        $scope.valoraPagar = 0;

        $scope.EnProceso = false;


        $scope.valoraPendiente = Monto;        
        $("#divValorPendiente").text(fNumber.go($scope.valoraPendiente));
        //Consulto los pagos de este documento
        $http.get('/api/ConsultarPagos?StrIdSeguridadDoc=' + IdSeguridad).then(function (response) {

            $("#button").dxButton({ visible: true });

            $("#grid").dxDataGrid({
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
                       if (options.column.caption == "Monto") {
                           if (fieldData) {
                               var inicial = fNumber.go(fieldData).replace("$-", "-$");
                               options.cellElement.html(inicial);
                           }
                       }
                   } catch (err) {
                       DevExpress.ui.notify(err.message, 'error', 3000);
                   }
               }, loadPanel: {
                   enabled: true
               }
               , allowColumnResizing: true
               , allowColumnReordering: true

               , columns: [

                   {
                       caption: "Monto",
                       dataField: "Monto",
                       cssClass: "col-md-1 col-xs-3",
                   },
                   {
                       caption: "FechaVerificacion",
                       dataField: "FechaVerificacion",
                       dataType: "date",
                       format: "yyyy-MM-dd ",
                       cssClass: "col-md-1 col-xs-3"
                   },
                   {
                       caption: "StrIdSeguridadPago",
                       dataField: "StrIdSeguridadPago",
                       cssClass: "col-md-1 col-xs-1"

                   }
                   ,
                    {
                        caption: "StrIdPlataforma",
                        cssClass: "hidden-xs col-md-1",
                        dataField: "StrIdPlataforma"

                    }
               ],

                summary: {
                    totalItems: [{
                        name: "MontoSum",
                        showInColumn: "Monto",
                        displayFormat: "{0}",
                        valueFormat: "currency",
                        summaryType: "custom"
                    }

                    ],
                    calculateCustomSummary: function (options) {
                        if (options.name === "MontoSum") {
                            if (options.summaryProcess === "start") {
                                options.totalValue = 0;
                                $scope.TotalPago = 0;
                            }
                            if (options.summaryProcess === "calculate") {
                                options.totalValue = options.totalValue + options.value.Monto;
                                $scope.TotalPago = $scope.TotalPago + options.totalValue;
                                ///Monto pendiente por pagar
                                $scope.valoraPendiente = $scope.montoFactura - options.totalValue;
                                $("#MontoPago").dxNumberBox({ placeholder: "$ " + $scope.valoraPendiente });
                                $("#divValorPendiente").text(fNumber.go($scope.valoraPendiente));
                                if ($scope.valoraPendiente == 0) {
                                    $("#button").dxButton({ visible: false });
                                    $("#summary").dxValidationSummary({ visible: false });
                                    $("#PanelPago").hide();
                                } else {
                                    $("#button").dxButton({ visible: true });
                                    $("#summary").dxValidationSummary({ visible: true });
                                    $("#PanelPago").show();
                                }
                            }
                        }
                    }
                }
            });



        }), function errorCallback(response) {
            Mensaje(response.data.ExceptionMessage, "error");
        };


        //Valido el monto Total menos el monto pagado

        $("#MontoPago").dxNumberBox({
            format: "$ #,##0",
            placeholder: fNumber.go($scope.valoraPendiente),
            validationGroup: "ValidarPago",
            onValueChanged: function (data) {
                $scope.valoraPagar = data.value;
            }
        })
      .dxValidator({
          validationRules: [{
              type: "required",
              message: "Debe Indicar el monto a pagar"
          },
          {
              type: 'custom', validationCallback: function (options) {
                  if (validar()) {
                      options.rule.message = "El monto a pagar no puede ser mayor al monto pendiente";
                      return false;
                  } else { return true; }
              }
          },
          {
              type: 'custom', validationCallback: function (options) {
                  if (validarMayorACero()) {
                      options.rule.message = "El monto de ser mayor a cero(0)";
                      return false;
                  } else { return true; }
              }
          }


          , {
              type: 'pattern',
              pattern: '^[0-9]+$',
              message: 'No debe Incluir puntos(.) ni caracteres especiales'
          }
          , {
              type: "numeric",
              message: "El monto a pagar debe ser numérico"
          }]
      });
        //Valida que el Monto a Pagar no sea Mayor al monto pendiente
        function validar() {
            if ($scope.valoraPagar > $scope.valoraPendiente) {
                return true;
            }
            return false;
        }

        //Valida que el Monto a Pagar no sea Mayor al monto pendiente
        function validarMayorACero() {
            if ($scope.valoraPagar < 1) {
                return true;
            }
            return false;
        }




        $("#PagoTotal").dxCheckBox({
            name: "chkpagoTotal",
            text: "",
            value: false,
            onValueChanged: function (data) {
                if (data.value) {
                    $("#MontoPago").dxNumberBox({ value: $scope.valoraPendiente });
                } else {
                    $("#MontoPago").dxNumberBox({ value: "" });
                }

            }
        })
    }

    $scope.buttonProcesar = {
        text: 'Pagar',
        type: "success",
        onClick: function (e) {
            DevExpress.ui.notify("Aun no ha terminado el proceso actual", 'error', 3000);
        }
    };





    $("#form").on("submit", function (e) {
        $("#button").dxButton({ visible: false });
        $scope.EnProceso = true;
        $http.get('/api/Documentos?strIdSeguridad=' + $scope.IdSeguridad + '&tipo_pago = 0 &registrar_pago=true&valor_pago=' + $scope.valoraPagar).then(function (response) {
            window.open(response.data, "Zona de Pago", $(window).height(), $(window).width());
            $("#button").dxButton({ visible: true });
            $scope.EnProceso = false;
            $("#MontoPago").dxNumberBox({ value: "" });
            $rootScope.ConsultarPago($scope.IdSeguridad, $scope.montoFactura);
        }, function (error) {

            if (error != undefined) {
                DevExpress.ui.notify(error, 'error', 3000);
                $("#button").dxButton({ visible: true });
                $scope.EnProceso = false;
            }

        });
        e.preventDefault();
    });

    $("#button").dxButton({
        text: "Pagar",
        type: "success",
        useSubmitBehavior: true
    });

});
