DevExpress.localization.locale(navigator.language);

var email_destino = "";
var id_seguridad = "";

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
        codigo_facturador = response.data[0].Identificacion;
        consultar();
    });

    $("#FechaInicial").dxDateBox({
        value: now,
        displayFormat: "yyyy-MM-dd",
        onValueChanged: function (data) {
            fecha_inicio = new Date(data.value).toISOString();
            if (new Date(data.value).toISOString() > fecha_fin) {
                DevExpress.ui.notify("La fecha inicial no puede ser mayor a la fecha final", 'error', 3000);
                $("#FechaInicial").dxDateBox({ value: fecha_fin });
                fecha_inicio: fecha_fin;
            }
        }

    });

    $("#FechaFinal").dxDateBox({
        value: now,
        displayFormat: "yyyy-MM-dd",
        onValueChanged: function (data) {
            fecha_fin = new Date(data.value).toISOString();
            if (new Date(data.value).toISOString() < fecha_inicio) {
                DevExpress.ui.notify("La fecha final no puede ser menor a la fecha inicial", 'error', 3000);
                $("#FechaFinal").dxDateBox({ value: fecha_inicio });
                fecha_fin: fecha_inicio;
            }
        }

    });
    //Define los campos y las opciones
    $scope.filtros =
        {
            EstadoDian: {
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
        //Datos GET: codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin
        $('#wait').show();
        $http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&estado_dian=' + estado_dian + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {            
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
                    var fieldData = options.value,
                        fieldHtml = "";
                    try {
                        if (options.columnIndex == 4) {
                            if (fieldData) {
                                var inicial = fNumber.go(fieldData);
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
                , columns: [
                    {
                        caption: "Archivos",
                        cssClass: "col-xs-3 col-md-1",
                        cellTemplate: function (container, options) {

                            var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_acuse = " title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white;'";

                            if (options.data.Pdf)
                                visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer;'";
                            else
                                options.data.Pdf = "#";

                            if (options.data.Xml)
                                visible_xml = "href='" + options.data.Xml + "' style='pointer-events:auto;cursor: pointer;'";
                            else
                                options.data.Xml = "#";

                            if (options.data.EstadoAcuse != 'Pendiente')
                                visible_acuse = "href='" + options.data.RutaAcuse + "' title='ver acuse'  style='pointer-events:auto;cursor: pointer; '";
                            else
                                options.data.RutaAcuse = "#";

                            $("<div>")
                                .append(
                                    $("<a target='_blank' class='icon-file-eye2' " + visible_acuse + "></a>&nbsp;&nbsp;<a target='_blank' class='icon-file-pdf'  " + visible_pdf + ">&nbsp;&nbsp;<a target='_blank' class='icon-file-xml' " + visible_xml + ">&nbsp;&nbsp;"),
                                    $("<a class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:12%; font-size:19px'></a>&nbsp;&nbsp;").dxButton({
                                        onClick: function () {
                                            $scope.showModal = true;
                                            email_destino = options.data.MailAdquiriente;
                                            id_seguridad = options.data.StrIdSeguridad;
                                            $('input:text[name=EmailDestino]').val(email_destino);
                                        }
                                    }).removeClass("dx-button dx-button-normal dx-widget")

                                    

                            )
                                .append($(""))
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
                        format:"yyyy-MM-dd",
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
                         caption: "Identificación Adquiriente",
                         cssClass: "hidden-xs col-md-1",
                         dataField: "IdentificacionAdquiriente"
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

