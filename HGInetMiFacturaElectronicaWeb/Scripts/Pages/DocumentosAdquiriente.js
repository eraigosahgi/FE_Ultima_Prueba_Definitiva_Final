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
           fecha_fin = "",
           tipo_filtro_fecha = 2;

    SrvMaestrosEnum.ObtenerEnum(5).then(function (data) {
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
                TipoFiltroFecha: {
                    //Carga la data del control
                    dataSource: new DevExpress.data.ArrayStore({
                        data: TiposFiltroFecha,
                        key: "ID"
                    }),
                    displayExpr: "Texto",
                    value: TiposFiltroFecha[1],

                    onValueChanged: function (data) {
                        if (data.value != null) {
                            tipo_filtro_fecha = data.value.ID;
                        } else {
                            tipo_filtro_fecha = 1;
                        }
                    }
                }
                /*
                ,EstadoRecibo: {
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
                }
                */
                ,NumeroDocumento: {
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
            consultar2();
        }
    };


    function consultar2() {
        $('#Total').text("");
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();
        
        $('#wait').show();
        $http.get('/api/Documentos?codigo_adquiente=' + codigo_adquiente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_filtro_fecha=' + tipo_filtro_fecha).then(function (response) {
            $('#wait').hide();
            $("#gridDocumentos").dxDataGrid({
                dataSource: response.data
            });
        });
    }



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
        $http.get('/api/Documentos?codigo_adquiente=' + codigo_adquiente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_filtro_fecha=' + tipo_filtro_fecha).then(function (response) {
            $('#wait').hide();
            $("#gridDocumentos").dxDataGrid({
                dataSource: response.data,
                paging: {
                    pageSize: 20
                },
               /* stateStoring: {
                    enabled: true,
                    type: "localStorage",
                    storageKey: "storage"
                },*/
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
                            var visible_xml_acuse = "style='pointer-events:auto;cursor: not-allowed;'";

                            if (options.data.Pdf)
                                visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer'";
                            else
                                options.data.Pdf = "#";

                            if (options.data.Xml)
                                visible_xml = "href='" + options.data.Xml + "' style='pointer-events:auto;cursor: pointer'";
                            else
                                options.data.Xml = "#";

                            if (options.data.XmlAcuse)
                                visible_xml_acuse = "href='" + options.data.XmlAcuse + "' title='ver XML Respuesta acuse' style='pointer-events:auto;cursor: pointer'";
                            else
                                options.data.XmlAcuse = "#";

                            $("<div>")
                                .append($("<a target='_blank' class='icon-file-pdf'  " + visible_pdf + ">&nbsp;&nbsp;<a target='_blank' class='icon-file-xml' " + visible_xml + ">&nbsp;&nbsp;<a target='_blank' class='icon-file-xml' " + visible_xml_acuse + ">&nbsp;&nbsp;"))
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
                          caption: "Acuse",
                          cssClass: "col-md-1 col-xs-2",
                          cellTemplate: function (container, options) {
                              $("<div>")
                                  .append($("<a target='_blank' href='" + options.data.RutaAcuse + "'>Acuse</a>"))
                                  .appendTo(container);
                          }
                      },
                {
                    ///Opción de pago
                    cssClass: "col-md-1 ",
                    caption: "Pago",
                    width: "80px",
                    alignment: "center",

                    cellTemplate: function (container, options) {

                        var RazonSocial = options.data.NombreFacturador.replace(" ", "_%%_");
                        var click = "onClick=ConsultarPago1('" + options.data.StrIdSeguridad + "','" + options.data.IntVlrTotal + "','" + options.data.PagosParciales + "')";

                        var imagen = "";
                        if (options.data.tipodoc != 'Nota Crédito' && options.data.poseeIdComercio == 1) {
                            imagen = "<a " + click + " target='_blank' data-toggle='modal' data-target='#modal_Pagos_Electronicos' >Pagar</a>";
                        } else {
                            imagen = "";

                        }

                        if (options.data.tipodoc != 'Nota Crédito' && options.data.poseeIdComercio == 1 && options.data.FacturaCenlada == 100) {//aqui se debe colocar el status que indica el pago de la factura                            
                            imagen = "<a " + click + " target='_blank' data-toggle='modal' data-target='#modal_Pagos_Electronicos' >Ver</a>"
                        }

                        $("<div>")
                        .append($(imagen))

                         .appendTo(container);
                    }
                },

                ],
                //**************************************************************
                masterDetail: {
                    enabled: true,
                    template: function (container, options) {
                        var visible_zip = "";

                        var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

                        var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

                        var visible_xml_acuse = "style='pointer-events:auto;cursor: not-allowed;'";

                        var visible_acuse = "   title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

                        var visible_Servicio_DIAN = "style='pointer-events:auto;cursor: not-allowed;'";

                        if (options.data.Pdf)
                            visible_pdf = "href='" + options.data.Pdf + "' title='ver PDF' style='pointer-events:auto;cursor: pointer;'";
                        else
                            visible_pdf = "#";

                        if (options.data.Xml)
                            visible_xml = "href='" + options.data.Xml + "' title='ver XML' style='pointer-events:auto;cursor: pointer;'";
                        else
                            visible_xml = "#";

                        if (options.data.EstadoAcuse != 'Pendiente')
                            visible_acuse = "href='" + options.data.RutaAcuse + "' class='icon-file-eye2'  title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
                        else
                            visible_acuse = "#";

                        if (options.data.XmlAcuse != "#")
                            visible_xml_acuse = "href='" + options.data.XmlAcuse + "' class='icon-file-xml' title='ver XML Respuesta acuse' style='pointer-events:auto;cursor: pointer'";
                        else
                            visible_xml_acuse = "#";


                        if (options.data.zip)
                            visible_zip = "href='" + options.data.zip + "' class='icon-file-zip' title='ver anexo' style='pointer-events:auto;cursor: pointer'";
                        else
                            visible_zip = "#";

                        if (options.data.RutaServDian)
                            visible_Servicio_DIAN = "class='icon-file-xml' href='" + options.data.RutaServDian + "' title='ver XML' style='pointer-events:auto;cursor: pointer;'";
                        else
                            visible_Servicio_DIAN = "#";
                        
                        container.append($("<td aria-selected='false' role='gridcell' aria-colindex='1' class='dx-cell-focus-disabled dx-master-detail-cell' colspan='6' style='text-align: center;'><div class='master-detail-caption'>Lista de Archivos:</div><div class='dx-widget dx-visibility-change-handler' role='presentation'><div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>PDF Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='2' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='3' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Ver Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Anexo</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Respuesta DIAN</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='2' style='text-align: center;'><div> <a style='margin-left:5%;margin-right:5%;' target='_blank' class='icon-file-xml' " + visible_xml + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='3' style='text-align: center;'><div> <a target='_blank'  " + visible_xml_acuse + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank'   " + visible_acuse + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_zip + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='5' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_Servicio_DIAN + "></a></div></td></tr><tr class='dx-row dx-column-lines dx-freespace-row' role='row' style='height: 0px; display: none;'><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td></tr></tbody></table></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-horizontal dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='width: 831px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-vertical dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='height: 35px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div></div></div><span class='dx-datagrid-nodata dx-hidden'></span></div><div class='dx-hidden' style='padding-right: 0px;'></div><div></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-drag-header dx-datagrid-text-content dx-widget' style='display: none;'></div><div class='dx-context-menu dx-has-context-menu dx-widget dx-visibility-change-handler dx-collection dx-datagrid'></div><div class='dx-header-filter-menu'></div><div></div></div></div></td>"
                        ));
                    }
                },
                //****************************************************************
                //Totales y agrupaciones
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

    //Redirecciona el pago interno al metodo del controlador de pago    
    ConsultarPago1 = function (IdSeguridad, Monto, PagosParciales) {
        $rootScope.ConsultarPago(IdSeguridad, Monto, PagosParciales);
    };


});

//Controlador de Pagos
DocAdquirienteApp.controller('ModalPagosController', function ModalPagosController($scope, $rootScope, $location, $http, $timeout) {

    $("#summary").dxValidationSummary({});
    $scope.valoraPendiente = 0;

    /////Parametros de la tabla
    $scope.fechadoc = "";
    $scope.documento = "";
    $scope.tipoDoc = "";
    $scope.email = "";
    $scope.telefono = "";
    $scope.nit = "";
    $scope.razonsocial = "";
    $scope.SinpagosPendiente = true;

    //Variable contadora para verificar pagos
    $scope.NumVerificacion = 1;

    //Esta variable es para marcarla como true si hay un pago pendiente y asi poder seguir consultando su estado
    $scope.pagoenVerificacion = false;

    //Este es el new Guid con el que se guarda el documento
    //Se utiliza aqui solo si se va a verificar el estado de un pago
    $scope.Idregistro = "";
    $scope.PermitePagosParciales = false;    
    $rootScope.ConsultarPago = function (IdSeguridad, Monto, PagosParciales) {
        $("#PanelPago").show();
        $("#MontoPago").dxNumberBox({ readOnly: (PagosParciales == "1") ? false : true });
        $("#Detallepagos").hide();
        $("#panelPagoPendiente").show();
        $("#PanelVerificacion").hide();

        $scope.PermitePagosParciales = PagosParciales;

        $scope.pagoenVerificacion = false;

        ///Este es el monto total de la factura
        $scope.montoFactura = Monto;

        ///Id de seguridad del Documento        
        $scope.$apply(function () {
            $scope.IdSeguridad = IdSeguridad;
        });

        ///Monto total cancelado hasta la fecha
        $scope.TotalPago = 0;

        ///Valor a Cancelar
        $scope.valoraPagar = 0;

        $scope.EnProceso = false;



        $scope.valoraPendiente = Monto;
        $scope.valoraPagar = Monto;
        $("#divValorPendiente").text(fNumber.go($scope.valoraPendiente));
        $("#MontoPago").dxNumberBox({ value: $scope.valoraPendiente });
        //Consulto los pagos de este documento        
        $http.get('/api/ConsultarPagos?StrIdSeguridadDoc=' + IdSeguridad).then(function (response) {

            /////Parametros de la tabla
            $scope.fechadoc = response.data[0].FechaDocumento;
            $scope.documento = response.data[0].IntNumero;
            $scope.tipoDoc = response.data[0].DocTipo;
            $scope.email = response.data[0].Mail;//(Email != "null" && Email != "undefined") ? Email : "";
            $scope.telefono = response.data[0].Telefono;//(Telefono != "null" && Telefono != "undefined") ? Telefono : "";
            $scope.nit = response.data[0].NitFacturador;
            $scope.razonsocial = response.data[0].RazonSocialFacturador;




            $("#button").dxButton({ visible: true });

            $("#grid").dxDataGrid({
                dataSource: response.data[0].Pagos,
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
                       if (options.column.caption == "Valor") {
                           if (fieldData) {
                               var inicial = fNumber.go(fieldData).replace("$-", "-$");
                               options.cellElement.html(inicial);
                           }
                       }

                       if (options.column.caption == "Estado") {
                           if (fieldData) {
                               if (fieldData != "Aprobado" && fieldData != "Rechazado") {
                                   if (response.data[0].Pagos[0].Monto > 0) {
                                       $("#panelPagoPendiente").hide();
                                       $("#PanelVerificacion").show();
                                       $scope.Idregistro = options.data.IdRegistro;
                                       $scope.pagoenVerificacion = true;
                                       if ($scope.NumVerificacion <= 4) {
                                           EsperayValida();
                                       }
                                   }

                               }
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
                       caption: "Cód.Transacción",
                       dataField: "StrIdSeguridadPago"

                   },
                   {
                       caption: "Fecha Pago",
                       dataField: "FechaRegistro",
                       dataType: "date",
                       format: "yyyy-MM-dd HH:mm"
                   },

                   {
                       caption: "Valor",
                       dataField: "Monto"
                   },
                   {
                       caption: "Estado",
                       dataField: "Estado"
                   },

                   {
                       caption: "Fecha Verificación",
                       dataField: "FechaVerificacion",
                       dataType: "date",
                       format: "yyyy-MM-dd HH:mm"
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
                                if (options.value.Estado == "Aprobado") {
                                    options.totalValue = options.totalValue + options.value.Monto;
                                    $scope.TotalPago = $scope.TotalPago + options.totalValue;
                                    ///Monto pendiente por pagar
                                    $scope.valoraPendiente = $scope.montoFactura - options.totalValue;
                                    $("#MontoPago").dxNumberBox({ value: $scope.valoraPendiente });
                                    $("#divValorPendiente").text(fNumber.go($scope.valoraPendiente));
                                }
                                if ($scope.valoraPendiente == 0) {
                                    $("#button").dxButton({ visible: false });
                                    $("#summary").dxValidationSummary({ visible: false });
                                    $("#MontoPago").hide();
                                    $("#PanelPago").hide();
                                    $("#Pagar").hide();
                                } else {
                                    $("#button").dxButton({ visible: true });
                                    $("#summary").dxValidationSummary({ visible: true });
                                    $("#PanelPago").show();
                                    $("#MontoPago").show();
                                    $("#Pagar").show();

                                }
                            }
                        }
                    }
                }
            });


            try {
                if (response.data[0].Pagos[0].Monto > 0) {
                    $("#Detallepagos").show();
                } else {
                    $("#panelPagoPendiente").show();
                }
            } catch (e) {

            }

            //-------------------------
            var Vpago = window.open("", "Pagos", "width=10,height=10");            
            if (Vpago == null || Vpago == undefined) {
                $("#button").dxButton({ visible: false });
                DevExpress.ui.notify({ message: "Las ventanas emergentes estan bloqueadas, para realizar pagos, debe habilitarlas", position: { my: "center top", at: "center top" } }, "error", 6000);
            } else {
                $("#button").dxButton({
                    text: "Pagar",
                    type: "success",
                    useSubmitBehavior: true
                });
                Vpago.close();
            }
            //-------------------------

        }), function errorCallback(response) {
            Mensaje(response.data.ExceptionMessage, "error");
        };


        //Valido el monto Total menos el monto pagado

        $("#MontoPago").dxNumberBox({
            format: "$ #,##0.##",
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
              pattern: '^[0-9-.]+$',
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
    

    $scope.buttonVerificar = {
        text: 'Verificar',
        type: "success",
        onClick: function (e) {
            VerificarEstado();
        }
    }


    $("#form").on("submit", function (e) {        
        
        var idseg = $scope.IdSeguridad;
        var valor_pago = $scope.valoraPagar;        
        if (idseg != null && idseg != undefined) {
            $scope.EnProceso = true;
            $("#button").dxButton({ visible: false });
            $http.get('/api/Documentos?strIdSeguridad=' + idseg + '&tipo_pago=0&registrar_pago=true&valor_pago=' + valor_pago).then(function (response) {
                //Inicializo la variable en uno(1) cuando guardo el pago ya que luego debo consultar unas tres veces al servidor
                $scope.NumVerificacion = 1;
                //Ruta servicio
                var RutaServicio = $('#Hdf_RutaPagos').val() + "?IdSeguridad=";
                $scope.Idregistro = response.data.IdRegistro;
                var Vpago = window.open(RutaServicio + response.data.Ruta, "_blank");                
                $timeout(function callAtTimeout() {
                    VerificarEstado();
                }, 60000);

            }, function (error) {

                if (error != undefined) {
                    DevExpress.ui.notify(error, 'error', 3000);
                    $("#button").dxButton({ visible: true });
                    $scope.EnProceso = false;
                }

                $scope.$apply(function () {
                    $scope.EnProceso = false;
                });


            });            
        }
        e.preventDefault();
    });

    
    
    

    function EsperayValida() {
        ///Se va a ejecutar automaticamente la sonda de consulta mientras el pago este pendiente
        if ($scope.pagoenVerificacion) {
            $timeout(function callAtTimeout() {
                VerificarEstado();
            }, 60000);
        }
    }


    function VerificarEstado() {

        var RutaServicio = $('#Hdf_RutaSrvPagos').val();

       // $http.get(RutaServicio + '?IdSeguridadPago=' + $scope.IdSeguridad + "&StrIdSeguridadRegistro=" + $scope.Idregistro).then(function (response) {
            //Esto retorna un objeto  de la plataforma intermedia que sirve para actualizar el pago local
         //   var ObjeRespuestaPI = response.data;
            //////////////////////////////////////////////////////////////////////
            //$http.get('/Api/ActualizarEstado?IdSeguridad=' + $scope.IdSeguridad + '&StrIdSeguridadRegistro=' + $scope.Idregistro + '&Pago=' + ObjeRespuestaPI + "'").then(function (response) {
        $http.get('/Api/ActualizarEstado?IdSeguridad=' + $scope.IdSeguridad + '&StrIdSeguridadRegistro=' + $scope.Idregistro ).then(function (response) {
                $('#wait').hide();

                //Incremento la variable consulta para llegar a un maximo de tres consultar al servicio de zona virtual
                $scope.NumVerificacion = $scope.NumVerificacion + 1;

                $rootScope.ConsultarPago($scope.IdSeguridad, $scope.montoFactura, $scope.PermitePagosParciales);

                $scope.EnProceso = false;

                /////////////////////////////////////////////////////////////////////////////////////////

            }), function (response) {

                $scope.EnProceso = false;
                Mensaje(response.data.ExceptionMessage, "error");
            };
        /*
        }), function (response) {

            $scope.EnProceso = false;
            Mensaje(response.data.ExceptionMessage, "error");
        }
        */

    }

});

var TiposFiltroFecha =
    [
    { ID: "1", Texto: 'Fecha Recepción' },
    { ID: "2", Texto: 'Fecha Documento' }
    ];