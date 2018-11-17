DevExpress.localization.locale(navigator.language);

var AcuseConsultaApp = angular.module('AcuseConsultaApp', ['dx', 'AppMaestrosEnum']);
AcuseConsultaApp.controller('AcuseConsultaController', function AcuseConsultaController($scope, $http, $location, SrvMaestrosEnum) {

    var now = new Date();

    var codigo_facturador = "",
               codigo_adquiriente = "",
               numero_documento = "",
               fecha_inicio = "",
               fecha_fin = "",
               estado_acuse = "";
    Filtro_fecha = 2;

    SrvMaestrosEnum.ObtenerSesion().then(function (data) {
        codigo_facturador = data[0].Identificacion;
        consultar();
    });


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

    $scope.filtros =
           {
               Fecha: {
                   //Carga la data del control
                   dataSource: new DevExpress.data.ArrayStore({
                       data: items_Fecha,
                       key: "ID"
                   }),
                   displayExpr: "Texto",
                   value: items_Fecha[1],

                   onValueChanged: function (data) {
                       if (data.value != null) {
                           Filtro_fecha = data.value.ID;
                       } else {
                           Filtro_fecha = 1;
                       }
                   }
               },
               EstadoRecibo: {
                   //Carga la data del control
                   dataSource: new DevExpress.data.ArrayStore({
                       data: items_recibo,
                       key: "ID"
                   }),
                   displayExpr: "Texto",
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
                   placeholder: "Identificación del Adquiriente",
                   onValueChanged: function (data) {
                       codigo_adquiriente = data.value;
                   }
               }
           }


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
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();
        
        $('#wait').show();
        $http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&codigo_adquiriente=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_acuse + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
            $('#wait').hide();
            $("#gridDocumentos").dxDataGrid({
                dataSource: response.data
            });
        });
    }

    function consultar() {
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();

        //Obtiene los datos del web api
        //ControladorApi: /Api/Documentos/
        //Datos GET: codigo_facturador, codigo_adquiriente, numero_documento, estado_recibo, fecha_inicio, fecha_fin
        $('#wait').show();
        $http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&codigo_adquiriente=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&estado_recibo=' + estado_acuse + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&tipo_fecha=' + Filtro_fecha).then(function (response) {
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
            , loadPanel: {
                enabled: true
            },
                headerFilter: {
                    visible: true
                }
                /*, stateStoring: {
                    enabled: true,
                    type: "localStorage",
                    storageKey: "storage"
                }*/
            , allowColumnResizing: true
            , allowColumnReordering: true
            , columnChooser: {
                enabled: true,
                mode: "select",
                title: "Selector de Columnas"
            }

                , columns: [
                    {
                        caption: "Archivos",
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
                                    $("<a target='_blank' class='icon-file-pdf'  " + visible_pdf + ">&nbsp;&nbsp;<a target='_blank' class='icon-file-xml' " + visible_xml + ">&nbsp;&nbsp;"),
                                    $("<a class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:12%; font-size:19px'></a>&nbsp;&nbsp;").dxButton({
                                        onClick: function () {
                                            $scope.showModal = true;
                                            email_destino = options.data.MailAdquiriente;
                                            id_seguridad = options.data.StrIdSeguridad;
                                            $('input:text[name=EmailDestino]').val(email_destino);
                                        }
                                    }).removeClass("dx-button dx-button-normal dx-widget"))
                                        .append($(""))
                                        .appendTo(container);
                        }
                    },
                     {
                         caption: "Identificación Adquiriente",
                         dataField: "IdentificacionAdquiriente"
                     },
                      {
                          caption: "Nombre Adquiriente",
                          dataField: "RazonSocial"
                      },
                      {
                          caption: "Número Documento",
                          dataField: "NumeroDocumento",
                      },
                    {
                        caption: "Fecha Respuesta",
                        dataField: "FechaRespuesta",
                        dataType: "date",
                        format: "yyyy-MM-dd HH:mm",
                        validationRules: [{
                            type: "required",
                            message: "El campo Fecha es obligatorio."
                        }]
                    },
                       {
                           caption: "Estado Acuse",
                           dataField: "Estado",
                       },
                      {
                          caption: "Motivo Rechazo",
                          dataField: "MotivoRechazo",
                      }
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

                        //container.append($("<td aria-selected='false' role='gridcell' aria-colindex='1' class='dx-cell-focus-disabled dx-master-detail-cell' colspan='6' style='text-align: center;'><div class='master-detail-caption'>Lista de Archivos:</div><div class='dx-widget dx-visibility-change-handler' role='presentation'><div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 20%;'><col style='width: 20%;'><col style='width: 20%;'><col style='width: 20%;'><col style='width: 20%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>PDF Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='2' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='3' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Ver Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Anexo</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 20%;'><col style='width: 20%;'><col style='width: 20%;'><col style='width: 20%;'><col style='width: 20%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='2' style='text-align: center;'><div> <a style='margin-left:5%;margin-right:5%;' target='_blank' class='icon-file-xml' " + visible_xml + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='3' style='text-align: center;'><div> <a target='_blank'  " + visible_xml_acuse + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank'   " + visible_acuse + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_zip + "></a></div></td></tr><tr class='dx-row dx-column-lines dx-freespace-row' role='row' style='height: 0px; display: none;'><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td></tr></tbody></table></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-horizontal dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='width: 831px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-vertical dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='height: 35px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div></div></div><span class='dx-datagrid-nodata dx-hidden'></span></div><div class='dx-hidden' style='padding-right: 0px;'></div><div></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-drag-header dx-datagrid-text-content dx-widget' style='display: none;'></div><div class='dx-context-menu dx-has-context-menu dx-widget dx-visibility-change-handler dx-collection dx-datagrid'></div><div class='dx-header-filter-menu'></div><div></div></div></div></td>"                       
                        container.append($("<td aria-selected='false' role='gridcell' aria-colindex='1' class='dx-cell-focus-disabled dx-master-detail-cell' colspan='6' style='text-align: center;'><div class='master-detail-caption'>Lista de Archivos:</div><div class='dx-widget dx-visibility-change-handler' role='presentation'><div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>PDF Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='2' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='3' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Ver Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Anexo</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Respuesta DIAN</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='2' style='text-align: center;'><div> <a style='margin-left:5%;margin-right:5%;' target='_blank' class='icon-file-xml' " + visible_xml + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='3' style='text-align: center;'><div> <a target='_blank'  " + visible_xml_acuse + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank'   " + visible_acuse + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_zip + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='5' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_Servicio_DIAN + "></a></div></td></tr><tr class='dx-row dx-column-lines dx-freespace-row' role='row' style='height: 0px; display: none;'><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td></tr></tbody></table></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-horizontal dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='width: 831px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-vertical dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='height: 35px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div></div></div><span class='dx-datagrid-nodata dx-hidden'></span></div><div class='dx-hidden' style='padding-right: 0px;'></div><div></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-drag-header dx-datagrid-text-content dx-widget' style='display: none;'></div><div class='dx-context-menu dx-has-context-menu dx-widget dx-visibility-change-handler dx-collection dx-datagrid'></div><div class='dx-header-filter-menu'></div><div></div></div></div></td>"
                        ));
                    }
                },
                //****************************************************************
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

AcuseConsultaApp.controller('EnvioEmailController', function AcuseConsultaApp($scope, $http, $location) {

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
                $http.post('/api/Documentos?id_seguridad=' + id_seguridad + '&mail=' + email_destino).then(function (responseEnvio) {
                    $('#wait').hide();

                    swal({
                        title: 'Proceso Éxitoso',
                        text: 'El e-mail ha sido enviado con éxito.',
                        type: 'success',
                        confirmButtonColor: '#66BB6A',
                        confirmButtonTex: 'Aceptar',
                        animation: 'pop',
                        html: true,
                    });

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

var items_recibo =
    [
    { ID: "*", Texto: 'Obtener Todos' },
    { ID: "1", Texto: 'Aprobado' },
    { ID: "2", Texto: 'Rechazado' }
    ];


var items_Fecha =
    [
    { ID: "1", Texto: 'Fecha-Documento' },
    { ID: "2", Texto: 'Fecha-Acuse' }
    ];