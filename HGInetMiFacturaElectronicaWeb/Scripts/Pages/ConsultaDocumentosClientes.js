DevExpress.localization.locale('es-ES');

//var path = window.location.pathname;
//var ruta = window.location.href;
//ruta = ruta.replace(path, "/");
//document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/SrvDocumentos.js"></scr' + 'ipt>');

//document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/MaestrosEnum.js"></scr' + 'ipt>');

var TipoConsulta;
var email_destino = "";
var id_seguridad = "";
var items_recibo = [{ "ID": 1, "Descripcion": "Código Plataforma" }, { "ID": 2, "Descripcion": "Documento" }];
var DocObligadoApp = angular.module('DocObligadoApp', ['dx', 'ModalEmpresasApp', 'AppSrvDocumento', 'AppMaestrosEnum']);
DocObligadoApp.controller('DocObligadoController', function DocObligadoController($scope, $http, $location, SrvDocumento, SrvMaestrosEnum, $rootScope) {

    var now = new Date();
    var Estado;

    var codigo_facturador = "",
           Datos_codigo_plataforma = "",
           Datos_Resolucion = "",
           Datos_Documento = "";

    SrvMaestrosEnum.ObtenerSesion().then(function (data) {
        codigo_facturador = data[0].Identificacion;
    });


    // function cargarFiltros() {
    $(function () {
        $("#summary").dxValidationSummary({});

        $("#txtempresaasociada").dxTextBox({
            readOnly: true,
            name: txtempresaasociada,
            onValueChanged: function (data) {
                var empresa = null;
                var Asociada = "";
                if (data.value != null && data.value != "") {
                    empresa = data.value.split(' -- ');
                    Asociada = empresa[0];
                    Datos_empresa_Asociada = Asociada;
                    if (TipoConsulta != null && TipoConsulta != '') {
                        validarfiltros(TipoConsulta);
                    }
                } else {
                    empresa = Datos_Idententificacion;
                    Asociada = empresa;
                    Datos_empresa_Asociada = Asociada;
                }

            },
            onFocusIn: function (data) {
                $('#modal_Buscar_empresa').modal('show');
            }
        }).dxValidator({
            validationRules: [{
                type: 'required',
                message: "Debe seleccionar la empresa"
            }]
        });


        $("#txtDocumento").dxTextBox({
            name: txtcodigoplataforma,
            onValueChanged: function (data) {
                Datos_Documento = data.value;
            }
        });

        $("#Tipo").dxSelectBox({
            placeholder: "seleccione el tipo de busqueda",
            displayExpr: "Descripcion",
            dataSource: items_recibo,
            onValueChanged: function (data) {
                TipoConsulta = data.value.ID;
                validarfiltros(TipoConsulta);
            }
        }).dxValidator({
            validationRules: [{
                type: 'required',
                message: "Debe indicar el tipo de busqueda"
            }]
        });



        $("#txtcodigoplataforma").dxTextBox({
            name: txtcodigoplataforma,
            maxLength: 8,
            onValueChanged: function (data) {
                Datos_codigo_plataforma = data.value;
            }
        });

        function validarCodigoPlataforma() {
            if (TipoConsulta == 1) {
                if (Datos_codigo_plataforma.length != 8) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }







        $("#form1").on("submit", function (e) {
            consultar();
            e.preventDefault();
        });

        $("#button").dxButton({
            text: "Consultar",
            type: "default",
            useSubmitBehavior: true
        });

    });

    function validarfiltros(Tipo) {
        if (Tipo != '1') {
            $('#divcodigoplataforma').hide();
            $('#divdocumento').show();
            Datos_codigo_plataforma = "*";
            $("#txtcodigoplataforma").dxTextBox({ value: '' });
            //Obtener lista de Resoluciones
            $http.get('/api/EmpresaResolucion?codigo_facturador=' + Datos_empresa_Asociada).then(function (response) {
                cargarResolucion(JSON.parse(JSON.stringify(response.data)));
            });
        } else {
            $('#divcodigoplataforma').show();
            $('#divdocumento').hide();
            Datos_Resolucion = "*";
            Datos_Documento = "*";
            $("#txtDocumento").dxTextBox({ value: '' });
        }
    }



    function cargarResolucion(Lista) {
        $("#Listaresolucion").dxSelectBox({
            placeholder: "seleccione el código de resolución",
            displayExpr: "Descripcion",
            dataSource: Lista,
            onValueChanged: function (data) {
                Datos_Resolucion = data.value.Descripcion;
            }
        });
    }



    $scope.ButtonOptionsConsultar = {
        text: 'Consultar',
        type: 'default',
        validationGroup: "Consultasoporte",
        onClick: function (e) {
            consultar();
        }
    };





    function consultar() {

        $("#summary").dxValidationSummary({ value: 'Debe ingresar el codigo de plataforma' })

        SrvDocumento.ObtenerDocumentosClientes(Datos_empresa_Asociada, Datos_Documento, Datos_codigo_plataforma, Datos_Resolucion).then(function (data) {
            $("#gridDocumentos").dxDataGrid({
                dataSource: data,
                keyExpr: "NumeroDocumento"
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

                }
                 , allowColumnResizing: true
                , columns: [
                    {
                        caption: "Archivos",
                        width: "10%",
                        alignment: "left",
                        cellTemplate: function (container, options) {

                            var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_xml = " style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_xml_acuse = "style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_acuse = "  title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

                            var permite_envio = "class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:5%; font-size:19px'";

                            if (!options.data.permiteenvio)
                                permite_envio = "";

                            if (options.data.Pdf)
                                visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer;'";
                            else
                                options.data.Pdf = "#";

                            if (options.data.Xml && options.data.permiteenvio)
                                visible_xml = "href='" + options.data.Xml + "' class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
                            else
                                options.data.Xml = "#";

                            if (options.data.EstadoAcuse != 'Pendiente' && options.data.permiteenvio)
                                visible_acuse = "href='" + options.data.RutaAcuse + "' title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
                            else
                                options.data.RutaAcuse = "#";

                            if (options.data.XmlAcuse && options.data.permiteenvio)
                                visible_xml_acuse = "href='" + options.data.XmlAcuse + "' title='ver XML Respuesta acuse' style='pointer-events:auto;cursor: pointer'";
                            else
                                options.data.XmlAcuse = "#";



                            $("<div>")
                                .append(
                                   $("<a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "><a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + ">"),
                                    $("<a " + permite_envio + "></a>").dxButton({
                                        onClick: function () {
                                            $scope.showModal = true;
                                            email_destino = options.data.MailAdquiriente;
                                            id_seguridad = options.data.StrIdSeguridad;
                                            $('input:text[name=EmailDestino]').val("");
                                            if (permite_envio != "") {
                                                SrvDocumento.ConsultarEmailUbl(options.data.StrIdSeguridad).then(function (data) {
                                                    $('input:text[name=EmailDestino]').val(data);
                                                });
                                            }
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
                          caption: "Nombre Adquiriente",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "NombreAdquiriente"
                      },
                      {
                          caption: "Tipo Documento",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "tipodoc"
                      },
                      {
                          caption: "Estado",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "EstadoFactura",
                          cellTemplate: function (container, options) {

                          	$("<div>")
								.append($(ColocarEstado(options.data.Estado, options.data.EstadoFactura)))
								.appendTo(container);
                          }
                      },
                      {
                          caption: "Estado Acuse",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "EstadoAcuse",
                          cellTemplate: function (container, options) {

                          	$("<div>")
								.append($(ColocarEstadoAcuse(options.data.IntAdquirienteRecibo, options.data.EstadoAcuse)))
								.appendTo(container);
                          }
                      },
                      {
                          caption: "Motivo Rechazo",
                          cssClass: "hidden-xs col-md-1",
                          dataField: "MotivoRechazo",
                      },
					  {
					  	caption: "Estado Email",
					  	cssClass: "hidden-xs col-md-1",
					  	dataField: "EstadoEnvioMail",
					  	cellTemplate: function (container, options) {

					  		$("<div>")
		                        .append($(ColocarEstadoEmail(options.data.EnvioMail, options.data.MensajeEnvio, options.data.EstadoEnvioMail)))
		                        .appendTo(container);
					  	}
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

                            container.append($("<td aria-selected='false' role='gridcell' aria-colindex='1' class='dx-cell-focus-disabled dx-master-detail-cell' colspan='6' style='text-align: center;'><div class='master-detail-caption'>Lista de Archivos:</div><div class='dx-widget dx-visibility-change-handler' role='presentation'><div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>PDF Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='2' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='3' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Ver Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Anexo</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Respuesta DIAN</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Auditoría</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='2' style='text-align: center;'><div> <a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='3' style='text-align: center;'><div> <a target='_blank'  " + visible_xml_acuse + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank'   " + visible_acuse + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_zip + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='5' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_Servicio_DIAN + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'><div><a style='margin-left:5%;' class='icon-file-eye' onClick=ConsultarAuditoria('" + options.data.StrIdSeguridad + "','" + options.data.StrEmpresaFacturador + "','" + options.data.NumeroDocumento + "') target='_blank' data-toggle='modal' data-target='#modal_audit_documento' title='ver Auditoría'></a></div></td></tr><tr class='dx-row dx-column-lines dx-freespace-row' role='row' style='height: 0px; display: none;'><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td></tr></tbody></table></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-horizontal dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='width: 831px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-vertical dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='height: 35px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div></div></div><span class='dx-datagrid-nodata dx-hidden'></span></div><div class='dx-hidden' style='padding-right: 0px;'></div><div></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-drag-header dx-datagrid-text-content dx-widget' style='display: none;'></div><div class='dx-context-menu dx-has-context-menu dx-widget dx-visibility-change-handler dx-collection dx-datagrid'></div><div class='dx-header-filter-menu'></div><div></div></div></div></td>"
                            ));
                        }
                    }
                },
                //****************************************************************
            });
        });


    }


    ConsultarAuditoria = function (IdSeguridad, IdFacturador, NumeroDocumento) {
    	$rootScope.ConsultarAuditDoc(IdSeguridad, IdFacturador, NumeroDocumento);
    };

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

//Controlador para gestionar la consulta de Auditoría de Documento 
DocObligadoApp.controller('ModalAuditDocumentoController', function ModalAuditDocumentoController($http, $scope, $location, $rootScope) {

	$rootScope.ConsultarAuditDoc = function (IdSeguridad, IdFacturador, NumeroDocumento) {

		$http.get('/api/AuditoriaDocumento?id_seguridad_doc=' + IdSeguridad).then(function (response) {

			$scope.IdSeguridad = IdSeguridad;
			$scope.NumeroDocumento = NumeroDocumento;
			$scope.Obligado = IdFacturador;

			$("#gridAuditDocumento").dxDataGrid({
				dataSource: response.data,
				allowColumnResizing: true,
				allowColumnReordering: true,
				paging: {
					pageSize: 10
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				}, loadPanel: {
					enabled: true
				},
				columns: [{
					caption: "Fecha",
					dataField: "DatFecha",
					cssClass: "col-md-2",
				}, {
					caption: "Estado",
					dataField: "StrDesEstado",
					cssClass: "col-md-2"
				}, {
					caption: "Proceso",
					dataField: "StrDesProceso",
					cssClass: "col-md-2"
				}, {
					caption: "Procesado Por",
					dataField: "StrDesProcesadoPor",
					cssClass: "col-md-2"
				}, {
					caption: "Realizado Por",
					dataField: "StrDesRealizadoPor",
					cssClass: "col-md-2"
				}
				],
				masterDetail: {
					enabled: true,
					template: function (container, options) {

						container.append($('<h4 class="form-control">MENSAJE:</h4><p style="width:10%"> ' + options.data.StrMensaje + '</p><br/>'));

						if (options.data.IntIdProceso == 8 || options.data.IntIdProceso == 10) {

							if (options.data.StrResultadoProceso)
								container.append($('<h4 class="form-control">RESPUESTA:</h4><span><p style="width:10%"> ' + options.data.StrResultadoProceso + '</p></span>'));

							if (options.data.StrResultadoProceso) {
								$http.get('/api/DetallesRespuesta?id_proceso=' + options.data.IntIdProceso + '&respuesta=' + options.data.StrResultadoProceso).then(function (response) {

									if (response.data != null) {
										container.append($('<h4 class="form-control">DETALLES RESPUESTA:</h4></br><label><b>Fecha Envío: </b> ' + response.data.Recibido + '</label></br><label><b>ID Remitente : </b> '
										+ response.data.IdRemitente + '</label></br><label><b>ID Contacto : </b> ' + response.data.IdContacto + '</label></br><label><b>Cantidad Adjuntos: </b> '
										+ response.data.Adjuntos + '</label></br><div id="json"></div>'));

										$("#json").dxDataGrid({
											dataSource: response.data.Seguimiento,
											allowColumnResizing: true,
											allowColumnReordering: true,
											paging: {
												pageSize: 10
											},
											pager: {
												showPageSizeSelector: true,
												allowedPageSizes: [5, 10, 20],
												showInfo: true
											}, loadPanel: {
												enabled: true
											},
											columns: [{
												caption: "Fecha Proceso",
												dataField: "FechaEvento",
												dataType: "date",
												format: "yyyy-MM-dd HH:mm:ss",
											},
											{
												dataField: "Tipo Proceso",
												caption: "TipoEvento",
												cellTemplate: function (container, options) {
													$("<div>").append($(ControlTipoEventoMail(options.data.TipoEvento))).appendTo(container);
												}
											},
											]
										}, function (response) {
											$('#wait').hide();
											DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 10000);
										});
									}
								});
							}
						} else {
							var cadena_inicio = options.data.StrResultadoProceso.substring(0, 4);

							//Valida si el mensaje de respuesta inicia con http y lo añade como link.
							if (cadena_inicio == "http") {
								container.append($('<h4 class="form-control">RESPUESTA:</h4><pre><a style="margin-left:5%;" target="_blank" href="' + options.data.StrResultadoProceso + '">' + options.data.StrResultadoProceso + '</a></pre>'));
							}
								//Valida si la cadena inicia con {
							else if (options.data.StrResultadoProceso.substring(0, 1) == "{") {
								container.append($('<h4 class="form-control">DETALLES RESPUESTA:</h4>'));

								var datos = angular.fromJson(options.data.StrResultadoProceso)

								var code_html = "";
								//Recorre una cadena json y la carga en código html propiedad por propiedad.
								for (var prop in datos) {

									code_html = code_html + '<label><b>' + prop + ':</b></label>';

									cadena_inicio = datos[prop].toString().substring(0, 4)

									//Valida si el valor de la propiedad es una ruta.
									if (cadena_inicio == "http")
										code_html = code_html + '<a style="margin-left:5%;" target="_blank" href="' + datos[prop] + '">' + datos[prop] + '</a></br>';
									else
										code_html = code_html + '<label>' + datos[prop] + '</label></br>';
								}

								container.append($('<pre>' + code_html + '</pre>'));

							}
							else if (options.data.StrResultadoProceso) {
								container.append($('<h4 class="form-control">RESPUESTA:</h4><span><p style="width:10%"> ' + options.data.StrResultadoProceso + '</p></span>'));
							}

						}
					}
				},
				filterRow: {
					visible: true
				}
			});
		}, function (response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 10000);
		});

	}

});




