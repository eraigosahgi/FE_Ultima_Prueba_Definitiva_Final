DevExpress.localization.locale('es-ES');

var path = window.location.pathname;
var ruta = window.location.href;
ruta = ruta.replace(path, "/");
document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/MaestrosEnum.js"></scr' + 'ipt>');

document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/SrvDocumentos.js"></scr' + 'ipt>');


var email_destino = "";
var id_seguridad = "";
var items_recibo = [];
var DocObligadoApp = angular.module('DocObligadoApp', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento']);
DocObligadoApp.controller('DocObligadoController', function DocObligadoController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento) {

    var now = new Date();
    var Estado;

    var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "",
           Datos_Tipo = "0";

    $http.get('/api/DatosSesion/').then(function (response) {
        codigo_facturador = response.data[0].Identificacion;
        consultar();
    });

    SrvMaestrosEnum.ObtenerEnum(6).then(function (data) {
        SrvMaestrosEnum.ObtenerEnum(1).then(function (dataacuse) {
            Estado = data;
            items_recibo = dataacuse;
            cargarFiltros();
        });
    });


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


        var DatosEstados = function () {
            return new DevExpress.data.CustomStore({
                loadMode: "raw",
                key: "ID",
                load: function () {
                    return JSON.parse(JSON.stringify(Estado));
                }
            });
        };


        $("#filtrosEstadoRecibo").dxDropDownBox({
            valueExpr: "ID",
            placeholder: "Seleccione un Item",
            displayExpr: "Descripcion",
            showClearButton: true,
            dataSource: DatosEstados(),
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
                        filterRow: { visible: true },
                        scrolling: { mode: "infinite" },
                        height: 400,
                        selection: { mode: "multiple" },
                        selectedRowKeys: value,
                        onSelectionChanged: function (selectedItems) {
                            var keys = selectedItems.selectedRowKeys;
                            e.component.option("value", keys);
                            estado_dian = keys;
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
                EstadoRecibo: {
                    searchEnabled: true,
                    //Carga la data del control
                    dataSource: new DevExpress.data.ArrayStore({
                        data: items_recibo,
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
                },
                Adquiriente: {
                    placeholder: "Ingrese Identificación del Adquiriente",
                    onValueChanged: function (data) {
                        codigo_adquiriente = data.value;
                    }
                }
            }

        $("#FechaFinal").dxDateBox({ min: now });
        $("#FechaInicial").dxDateBox({ max: now });



        $("#TipoDocumento").dxSelectBox({
            placeholder: "Seleccione el Tipo",
            displayExpr: "Texto",
            dataSource: items_Tipo,
            onValueChanged: function (data) {
                Datos_Tipo = data.value.ID;
            }
        });


    }

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

        SrvDocumento.ObtenerDocumentosAdmin(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, Datos_Tipo).then(function (data) {
            $("#gridDocumentos").dxDataGrid({
                dataSource: data
            });
        });
    }





    function consultar() {
        $('#Total').text("");
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        SrvDocumento.ObtenerDocumentosAdmin(codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, Datos_Tipo).then(function (data) {
            $("#gridDocumentos").dxDataGrid({
                dataSource: data,
                keyExpr: "NumeroDocumento",
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
                    title: "Selector de Columnas"
                },
                groupPanel: {
                    allowColumnDragging: true,
                    visible: true
                }
                /*,stateStoring: {
                    enabled: true,
                    type: "localStorage",
                    storageKey: "storage"
                }*/
                , columns: [
                    {
                        caption: "Archivos",
                        cssClass: "col-md-1",

                        cellTemplate: function (container, options) {

                            var permite_envio = "class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:5%; font-size:19px'";

                            if (!options.data.permiteenvio)
                                permite_envio = "";

                            var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_acuse = " title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

                            if (options.data.Pdf)
                                visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer;'";
                            else
                                options.data.Pdf = "#";

                            if (options.data.Xml && options.data.permiteenvio)
                                visible_xml = "href='" + options.data.Xml + "'  class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
                            else
                                options.data.Xml = "#";

                            if (options.data.EstadoAcuse != 'Pendiente')
                                visible_acuse = "href='" + options.data.RutaAcuse + "' title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
                            else
                                options.data.RutaAcuse = "#";

                            $("<div>")
                                .append(
                                    $("<a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "><a style='margin-left:5%;' target='_blank'  " + visible_xml + ">"),
                                    $("<a " + permite_envio + "></a>").dxButton({
                                        onClick: function () {
                                            $scope.showModal = true;
                                            email_destino = options.data.MailAdquiriente;
                                            id_seguridad = options.data.StrIdSeguridad;
                                            $('input:text[name=EmailDestino]').val("");
                                            SrvDocumento.ConsultarEmailUbl(options.data.StrIdSeguridad).then(function (data) {
                                                $('input:text[name=EmailDestino]').val(data);
                                            });
                                        }
                                    }).removeClass("dx-button dx-button-normal dx-widget")

                            )
                                .appendTo(container);

                        }
                    },
                    {
                        caption: "Documento",
                        cssClass: "col-xs-3 col-md-1",
                        dataField: "NumeroDocumento",

                    },
                    {
                        caption: "Fecha Documento",
                        dataField: "DatFechaDocumento",
                        dataType: "date",
                        format: "yyyy-MM-dd",
                        cssClass: "col-xs-3 col-md-1",
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
                        cssClass: "hidden-xs col-md-1",
                        validationRules: [{
                            type: "required",
                            message: "El campo Fecha es obligatorio."
                        }]
                    },
                    {
                        caption: "IdFacturador",
                        cssClass: "col-xs-1 col-md-1",
                        dataField: "IdFacturador"
                    }, {
                        caption: "Facturador",
                        cssClass: "col-xs-1 col-md-1",
                        dataField: "Facturador"
                    },
                    {
                        caption: "Valor Total",
                        cssClass: "col-xs-2 col-md-1",
                        dataField: "IntVlrTotal"
                    },
                     {
                         caption: "Adquiriente",
                         cssClass: "hidden-xs col-md-1",
                         dataField: "IdentificacionAdquiriente"
                     },
                     {
                         caption: "Tipo Documento",
                         cssClass: "hidden-xs col-md-1",
                         dataField: "tipodoc"
                     },
                      {
                          caption: "Nombre Adquiriente",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "NombreAdquiriente"
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
                      }
                ],

                //**************************************************************
                masterDetail: {
                    enabled: true,
                    template: function (container, options) {

                        if (options.data.permiteenvio) {
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

                            if (options.data.Xml && options.data.permiteenvio)
                                visible_xml = "href='" + options.data.Xml + "' class='icon-file-xml' title='ver XML' style='pointer-events:auto;cursor: pointer;'";
                            else
                                visible_xml = "#";

                            if (options.data.EstadoAcuse != 'Pendiente' && options.data.permiteenvio)
                                visible_acuse = "href='" + options.data.RutaAcuse + "' class='icon-file-eye2'  title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
                            else
                                visible_acuse = "#";

                            if (options.data.XmlAcuse != "#" && options.data.permiteenvio)
                                visible_xml_acuse = "href='" + options.data.XmlAcuse + "' class='icon-file-xml' title='ver XML Respuesta acuse' style='pointer-events:auto;cursor: pointer'";
                            else
                                visible_xml_acuse = "#";


                            if (options.data.zip && options.data.permiteenvio)
                                visible_zip = "href='" + options.data.zip + "' class='icon-file-zip' title='ver anexo' style='pointer-events:auto;cursor: pointer'";
                            else
                                visible_zip = "#";

                            if (options.data.RutaServDian && options.data.permiteenvio)
                                visible_Servicio_DIAN = "class='icon-file-xml' href='" + options.data.RutaServDian + "' title='ver XML' style='pointer-events:auto;cursor: pointer;'";
                            else
                                visible_Servicio_DIAN = "#";

                            container.append($("<td aria-selected='false' role='gridcell' aria-colindex='1' class='dx-cell-focus-disabled dx-master-detail-cell' colspan='6' style='text-align: center;'><div class='master-detail-caption'>Lista de Archivos:</div><div class='dx-widget dx-visibility-change-handler' role='presentation'><div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>PDF Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='2' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='3' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Ver Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Anexo</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Respuesta DIAN</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='2' style='text-align: center;'><div> <a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='3' style='text-align: center;'><div> <a target='_blank'  " + visible_xml_acuse + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank'   " + visible_acuse + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_zip + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='5' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_Servicio_DIAN + "></a></div></td></tr><tr class='dx-row dx-column-lines dx-freespace-row' role='row' style='height: 0px; display: none;'><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td></tr></tbody></table></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-horizontal dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='width: 831px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-vertical dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='height: 35px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div></div></div><span class='dx-datagrid-nodata dx-hidden'></span></div><div class='dx-hidden' style='padding-right: 0px;'></div><div></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-drag-header dx-datagrid-text-content dx-widget' style='display: none;'></div><div class='dx-context-menu dx-has-context-menu dx-widget dx-visibility-change-handler dx-collection dx-datagrid'></div><div class='dx-header-filter-menu'></div><div></div></div></div></td>"
                            ));
                        }
                    }
                },
                //****************************************************************

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
                                $('#Total').text("Total: " + fNumber.go(options.totalValue));
                            }
                        }
                    }
                }
                ,
                filterRow: {
                    visible: true
                }

            });

        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });


    }


});


DocObligadoApp.controller('EnvioEmailController', function EnvioEmailController($scope, $http, $location) {

    //Formulario.
    $scope.formOptionsEmailEnvio = {

        readOnly: false,
        showColonAfterLabel: true,
        showValidationSummary: true,
        validationGroup: "DatosEmail",
        onInitialized: function (e) {
            formInstance = e.component;
        },
        items: [{
            itemType: "group",
            items: [
                {
                    dataField: "EmailDestino",
                    editorType: "dxTextBox",
                    label: {
                        text: "E-mail"
                    },
                    validationRules: [{
                        type: "required",
                        message: "El e-mail de destino es obligatorio."
                    }, {
                        //Valida que el campo solo contenga números
                        type: "pattern",
                        pattern: "[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$",
                        message: "El campo no contiene el formato requerido."
                    }]
                }
            ]
        }
        ]
    };

    //botón Cerrar Modal
    $scope.buttonCerrarModal = {
        text: "CERRAR"
    };

    //Botón Enviar email
    $scope.buttonEnviarEmail = {
        text: "ENVIAR",
        type: "success",
        //useSubmitBehavior: true,
        //validationGroup: "DatosEmail",

        onClick: function (e) {

            try {

                email_destino = $('input:text[name=EmailDestino]').val();

                if (email_destino == "") {
                    throw new DOMException("El e-mail de destino es obligatorio.");
                }
                $('#wait').show();
                $http.get('/api/Documentos?id_seguridad=' + id_seguridad + '&email=' + email_destino).then(function (responseEnvio) {
                    $('#wait').hide();
                    var respuesta = responseEnvio.data;

                    if (respuesta) {
                        swal({
                            title: 'Proceso Éxitoso',
                            text: 'El e-mail ha sido enviado con éxito.',
                            type: 'success',
                            confirmButtonColor: '#66BB6A',
                            confirmButtonTex: 'Aceptar',
                            animation: 'pop',
                            html: true,
                        });
                    } else {
                        swal({
                            title: 'Error',
                            text: 'Ocurrió un error en el envío del e-mail.',
                            type: 'Error',
                            confirmButtonColor: '#66BB6A',
                            confirmButtonTex: 'Aceptar',
                            animation: 'pop',
                            html: true,
                        });
                    }
                    $('input:text[name=EmailDestino]').val("");
                    $('#btncerrarModal').click();
                }, function errorCallback(response) {
                    $('#wait').hide();
                    DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 10000);
                });

            } catch (e) {
                $('#wait').hide();
                DevExpress.ui.notify(e.message, 'error', 10000);
            }
        }
    };



});



var items_Tipo =
    [
        { ID: "0", Texto: 'Todos' },
        { ID: "1", Texto: 'Factura' },
        { ID: "2", Texto: 'Nota Debito' },
        { ID: "3", Texto: 'Nota Crédito' }
    ];


