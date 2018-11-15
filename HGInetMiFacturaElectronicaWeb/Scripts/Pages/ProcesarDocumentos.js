DevExpress.localization.locale(navigator.language);

var path = window.location.pathname;
var ruta = window.location.href;
ruta = ruta.replace(path, "/");
document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/SrvDocumentos.js"></scr' + 'ipt>');

angular.module('ProcesarDocumentosApp', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento'])

.controller('ProcesarDocumentosController', function DocAdquirienteController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento) {

    $("#btnProcesar").dxButton({
        text: "Enviar Documentos",
        type: "default",
        onClick: ProcesarDocumentos
    });


    //Numero de documentos a Procesar
    $scope.total = 0;
    var codigo_adquiente = "", numero_documento = "", estado_recibo = "", fecha_inicio = "", fecha_fin = "", Estado = [], now = new Date();

    SrvMaestrosEnum.ObtenerEnum(0, "privado").then(function (data) {
        Estado = data;
        cargarFiltros();
    });

    SrvMaestrosEnum.ObtenerSesion().then(function (data) {
        codigo_adquiente = data[0].Identificacion;
        consultar();
    });

    var makeAsyncDataSource = function () {
        return new DevExpress.data.CustomStore({
            loadMode: "raw",
            key: "ID",
            load: function () {
                return JSON.parse(JSON.stringify(Estado));
            }
        });
    };

    function cargarFiltros() {
        $("#FechaInicial").dxDateBox({
            name: "txtf",
            value: now,
            width: '100%',
            max: fecha_fin,
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
            min: fecha_inicio,
            onValueChanged: function (data) {
                fecha_fin = new Date(data.value).toISOString();
                $("#FechaInicial").dxDateBox({ max: fecha_fin });
            }

        });



        $("#filtrosEstadoRecibo").dxDropDownBox({
            valueExpr: "ID",
            placeholder: "Seleccionar ",
            displayExpr: "Descripcion",
            showClearButton: true,
            dataSource: makeAsyncDataSource(),
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
                        //filterRow: { visible: true },
                        scrolling: { mode: "infinite" },
                        height: 300,

                        selection: { mode: "multiple" },
                        selectedRowKeys: value,
                        onSelectionChanged: function (selectedItems) {
                            var keys = selectedItems.selectedRowKeys;
                            e.component.option("value", keys);
                            estado_recibo = keys;
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
                    NumeroDocumento: {
                        placeholder: "Identificador Documento",
                        onValueChanged: function (data) {
                            numero_documento = data.value;
                        }
                    }
                }

        var mensaje_acuse = "";


        $scope.ButtonOptionsConsultar = {
            text: 'Consultar',
            type: 'default',
            onClick: function (e) {
                consultar2();
            }
        };

        $("#FechaFinal").dxDateBox({ min: now });
        $("#FechaInicial").dxDateBox({ max: now });
    }


    function consultar2() {
        $('#panelresultado').hide();
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();


        SrvDocumento.ObtenerDocumentos().then(function (data) {
            $("#gridDocumentos").dxDataGrid({
                dataSource: data
            });
        });
    }



    //Consultar DOcumentos
    function consultar() {
        $('#panelresultado').hide();
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();


        SrvDocumento.ObtenerDocumentos().then(function (data) {
            $("#gridDocumentos").dxDataGrid({
                dataSource: data,
                paging: {
                    pageSize: 20
                },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                },
                selection: {
                    mode: "multiple",
                    width: 10
                }
                  , loadPanel: {
                      enabled: true
                  },
                headerFilter: {
                    visible: true
                }
                 , stateStoring: {
                     enabled: true,
                     type: "localStorage",
                     storageKey: "storage"
                 }
                , allowColumnResizing: true
                , columns: [
                     {
                         caption: "Fecha Recepción",
                         dataField: "DatFechaIngreso",
                         dataType: "date",
                         format: "yyyy-MM-dd HH:mm",
                         

                     },                                 
                     {
                         caption: "Documento",
                         dataField: "NumeroDocumento",                         
                         headerFilter: {
                             allowSearch: false
                         }

                     },

                    {
                        caption: "IdSeguridad",
                        dataField: "IdSeguridad",                        
                        headerFilter: {
                            allowSearch: false
                        }

                    },
                     {
                         caption: "Tipo Documento",                         
                         dataField: "tipodoc"
                     },
                      {
                          caption: "Facturador",
                          dataField: "Facturador",
                          
                      },
                      {
                          caption: "Estado",
                          dataField: "EstadoFactura",                          
                          headerFilter: {
                              allowSearch: true,
                              caption: "Busqueda"
                          }
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
                }

                , onSelectionChanged: function (selectedEstado) {
                    var lista = '';
                    var data = selectedEstado.selectedRowsData;
                    if (data.length > 0) {
                        if (data.length > 1) {
                            for (var i = 0; i < data.length; i++) {
                                lista += (lista) ? ',' : '';
                                lista += "{Documentos: '" + data[i].IdSeguridad + "'}";
                            }
                            lista = "[" + lista + "]"
                            $scope.documentos = lista;
                            $('#lbltotaldocumentos').val('Documentos a Procesar : ' + data.length);
                            $scope.total = data.length;

                        } else {
                            lista += "{Documentos: '" + data[0].IdSeguridad + "'}";
                            lista = "[" + lista + "]"
                            $scope.documentos = lista;
                            $('#lbltotaldocumentos').val('Documento a Procesar : ' + data.length);
                            $scope.total = data.length;

                        }
                    } else {
                        $('#lbltotaldocumentos').val('Ningun Documento Por Procesar');
                        $scope.documentos = "Ningun Documento Por Procesar";
                        $scope.total = 0;

                    }
                    $scope.$apply(function () {
                        $scope.total = $scope.total;
                    });

                }

                , summary: {
                    totalEstado: [{
                        column: "IdSeguridad",
                        caption: "Total Documentos : ",
                        summaryType: "count"
                    }
                    ]
                }


            }).dxDataGrid("instance");
        });
    }

   

    function ProcesarDocumentos() {
        if ($scope.total > 0) {

            SrvDocumento.ProcesarDocumentos($scope.documentos).then(function (data) {
                $("#gridDocumentosProcesados").dxDataGrid({
                    dataSource: data,
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
               , columns: [
                   {
                       caption: 'Estado',
                       dataField: 'Estado',
                       width: '5%',
                       cellTemplate: function (container, options) {
                           $("<div style='text-align:center'>")
                               .append($("<a taget=_self class='icon-circle2'" + ((options.data.CodigoError != '') ? " style='color:red;' title='Error'" : " style='color:green;' title='Proceso exitoso'") + ">"))
                               .appendTo(container);
                       }
                   },
                    {
                        caption: "Fecha Recepción",
                        dataField: "FechaRecepcion",
                        dataType: "date",
                        width: '16%',
                        format: "yyyy-MM-dd HH:mm"

                    },

                   {
                       caption: "Fecha Ultimo Proceso",
                       dataField: "FechaUltimoProceso",
                       dataType: "date",
                       width: '16%',
                       format: "yyyy-MM-dd HH:mm"

                   },
                   {
                       caption: "Documento",
                       dataField: "Documento",
                       width: '10%',                       

                   },
                    {
                        caption: "Resultado",
                        width: '50%',
                        dataField: "DescripcionProceso"
                    },
               {
                   caption: "Aceptacion",
                   dataField: "Aceptacion",
                   hidingPriority: 0
               },
                     {
                         caption: "CodigoRegistro",
                         dataField: "CodigoRegistro",
                         hidingPriority: 1
                     },
                     {
                         caption: "Cufe",
                         dataField: "Cufe",
                         hidingPriority: 2
                     },                     
                     {
                         caption: "EstadoDian",
                         dataField: "EstadoDian",
                         hidingPriority: 4

                     },
                     {
                         caption: "IdDocumento",
                         dataField: "IdDocumento",
                         hidingPriority: 5
                     },
                     {
                         caption: "Identificacion",
                         dataField: "Identificacion",
                         hidingPriority: 6
                     },
                     {
                         caption: "IdProceso",
                         dataField: "IdProceso",
                         hidingPriority: 7
                     },
                    {
                        caption: "MotivoRechazo",
                        dataField: "MotivoRechazo",
                        hidingPriority: 8
                    },
                    {
                        caption: "NumeroResolucion",
                        dataField: "NumeroResolucion",
                        hidingPriority: 9
                    }, {
                        caption: "Descripcion Proceso",
                        dataField: "DescripcionProceso",
                        hidingPriority: 10
                    },
                    {
                        caption: "Tipo de Documento",
                        dataField: "tipodoc",
                        hidingPriority: 11
                    }

               ]
                }).dxDataGrid("instance");

                consultar();
                $('#panelresultado').show();
            });
        }

    }

});

