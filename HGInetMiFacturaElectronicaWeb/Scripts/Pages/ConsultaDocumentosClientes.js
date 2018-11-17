DevExpress.localization.locale('es-ES');

var path = window.location.pathname;
var ruta = window.location.href;
ruta = ruta.replace(path, "/");
document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/SrvDocumentos.js"></scr' + 'ipt>');

document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/MaestrosEnum.js"></scr' + 'ipt>');

var TipoConsulta;
var email_destino = "";
var id_seguridad = "";
var items_recibo = [{ "ID": 1, "Descripcion": "Código Plataforma" }, { "ID": 2, "Descripcion": "Documento" }];
var DocObligadoApp = angular.module('DocObligadoApp', ['dx', 'ModalEmpresasApp', 'AppSrvDocumento', 'AppMaestrosEnum']);
DocObligadoApp.controller('DocObligadoController', function DocObligadoController($scope, $http, $location, SrvDocumento, SrvMaestrosEnum) {

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
                        caption: "  Lista de Archivos",
                        cssClass: "col-xs-3 col-md-1",

                        cellTemplate: function (container, options) {

                            var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

                            var visible_acuse = " title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

                            if (options.data.Pdf)
                                visible_pdf = "href='" + options.data.Pdf + "' style='pointer-events:auto;cursor: pointer;'";
                            else
                                options.data.Pdf = "#";

                            if (options.data.Xml)
                                visible_xml = "href='" + options.data.Xml + "' style='pointer-events:auto;cursor: pointer;'";
                            else
                                options.data.Xml = "#";

                            if (options.data.EstadoAcuse != 'Pendiente')
                                visible_acuse = "href='" + options.data.RutaAcuse + "' title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
                            else
                                options.data.RutaAcuse = "#";

                            $("<div>")
                                .append(
                                    $("<a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "><a style='margin-left:5%;' target='_blank' class='icon-file-xml' " + visible_xml + ">"),
                                    $("<a class='icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:5%; font-size:19px'></a>").dxButton({
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
                                .append($("<a target='_blank' class='icon-file-eye2' " + visible_acuse + "></a>"))
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

            });
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






