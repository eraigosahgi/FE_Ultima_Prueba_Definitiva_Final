
var id_seguridad;
var numero_documento;
var obligado;
var idEndosatario;

App.controller('EventosRadianController', function EventosRadianController($scope, $http, $location, $rootScope) {

    $rootScope.ConsultarEventosRadian = function (IdSeguridad, NumeroDocumento, Obligado, idEndosatario) {
        $scope.IdSeguridad = IdSeguridad;
        $("#IdSeguridad").text(IdSeguridad);
        $scope.NumeroDocumento = $("#NumeroDocumento").text(NumeroDocumento);
        $scope.Obligado = $("#Obligado").text(Obligado);
        $('#panelEndoso').hide();
        $("#tiposEndoso").dxSelectBox({ value: '' });
        $("#tiposOperacionEvento").dxSelectBox({ value: '' });
        $("#idEndosatario").dxTextBox({ value: '' });
        $("#tasaDescuentoEndoso").dxNumberBox({ value: '' });
        $("#razonSocialEmpresa").text('');

        $http.get('/api/ObtenerEventosRadian?id_seguridad=' + IdSeguridad).then(function (response) {

            $("#GridEventosRadianDocumento").dxDataGrid({
                dataSource: response.data,
                allowColumnResizing: true,
                paging: {
                    pageSize: 10
                },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                },
                onContentReady: function (e) {
                    try {
                        $("#cmdenviar").dxButton({ visible: response.data[0].Inscribir_Documento });
                        $("#cmdenviar1").dxButton({ visible: response.data[0].otros_eventos });
                        $("#cmdenviar2").dxButton({ visible: response.data[0].otros_eventos });
                        $("#cmdenviar3").dxButton({ visible: response.data[0].otros_eventos });
                        $("#cmdenviar4").dxButton({ visible: response.data[0].otros_eventos });
                        $("#cmdenviar5").dxButton({ visible: response.data[0].otros_eventos });

                    }
                    catch (err) {
                        $("#cmdenviar").dxButton({ visible: false });
                        $("#cmdenviar1").dxButton({ visible: false });
                        $("#cmdenviar2").dxButton({ visible: false });
                        $("#cmdenviar3").dxButton({ visible: false });
                        $("#cmdenviar4").dxButton({ visible: false });
                        $("#cmdenviar5").dxButton({ visible: false });
                    }
                },
                columns: [
				    {
				        caption: "Fecha",
				        dataField: "DatFechaEvento",
				        cssClass: "col-md-2",
				        dataType: "date",
				        format: "yyyy-MM-dd HH:mm:ss",
				    }, {
				        caption: "Estado",
				        dataField: "EstadoEvento",
				        cssClass: "col-md-4",
				    }, {
				        caption: "Numero Evento",
				        dataField: "IntNumeroEvento",
				        cssClass: "col-md-1",
				    },
				   {
				       caption: "StrUrlEvento",
				       dataField: "StrUrlEvento",
				       visible: false
				   },
				    {
				        caption: "respuesta_evento",
				        dataField: "respuesta_evento",
				        visible: false
				    },
				   {
				       caption: "Archivo",
				       cssClass: "col-md-1",
				       cellTemplate: function (container, options) {
				           var visible_xml = "href='" + options.data.StrUrlEvento + "' class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
				           $("<div>")
                               .append(
                                  $("<a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + ">"))
                               .appendTo(container);
				       }
				   },
				   {
				       caption: "Respuesta",
				       cssClass: "col-md-1",
				       cellTemplate: function (container, options) {
				           var visible_xml_resp = "href='" + options.data.respuesta_evento + "' class='icon-file-xml' style='pointer-events:auto;cursor: pointer;'";
				           $("<div>")
                               .append(
                                  $("<a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml_resp + ">"))
                               .appendTo(container);
				       }
				   },
                ]

            });





            $("#cmdenviar").dxButton({
                text: "Inscripción TV",
                type: "default",
                visible: false,
                onClick: function () {
                    $http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
                        //alert(response.data);
                        $rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);

                    }, function errorCallback(response) {

                        //Carga notificación de creación con opción de editar formato.
                        var myDialog = DevExpress.ui.dialog.custom({
                            title: "Proceso Falló",
                            message: response.data.ExceptionMessage,
                            buttons: [{
                                text: "Aceptar",
                                onClick: function (e) {
                                    myDialog.hide();
                                    $rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
                                }
                            }]
                        });
                        myDialog.show().done(function (dialogResult) {
                        });

                        $('#wait').hide();
                    });
                }
            });

            // Función para mostrar el panel Tipo Endoso
              $("#cmdenviar1").dxButton({
                text: "Endoso",
                type: "default",
                visible: false,
                onClick: function () {
                    $("#panelEndoso").toggle();
                }
            });

            $("#cmdenviar2").dxButton({
                text: "Aval",
                type: "default",
                visible: false,
                onClick: function () {

                    MensajeEventoRadian();
                    //$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
                    //	//alert(response.data);
                    //	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
                    //});
                }
            });

            $("#cmdenviar3").dxButton({
                text: "Mandato",
                type: "default",
                visible: false,
                onClick: function () {
                    MensajeEventoRadian();
                    //$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
                    //	//alert(response.data);
                    //	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
                    //});
                }
            });

            $("#cmdenviar4").dxButton({
                text: "Limitación",
                type: "default",
                visible: false,
                onClick: function () {
                    MensajeEventoRadian();
                    //$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
                    //	//alert(response.data);
                    //	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
                    //});
                }
            });

            $("#cmdenviar5").dxButton({
                text: "Transferencia Derecho",
                type: "default",
                visible: false,
                onClick: function () {
                    MensajeEventoRadian();
                    //$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
                    //	//alert(response.data);
                    //	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
                    //});
                }
            });

            $("#cmdenviar6").dxButton({
                text: "Informe Pago",
                type: "default",
                visible: false,
                onClick: function () {
                    MensajeEventoRadian();
                    //$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
                    //	//alert(response.data);
                    //	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
                    //});
                }
            });

            $("#cmdenviar7").dxButton({
                text: "Pago",
                type: "default",
                visible: false,
                onClick: function () {
                    MensajeEventoRadian();
                    //$http.post('/api/Documentos?id_seguridad=' + $scope.IdSeguridad + '&estado=6' + '&motivo_rechazo=' + '&usuario=').then(function (response) {
                    //	//alert(response.data);
                    //	$rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
                    //});
                }
            });




        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });
    }

    function MensajeEventoRadian() {
        DevExpress.ui.notify("Evento no disponible temporalmente", 'error', 3000);
    }

    // Cargar datos filtro Tipo Endoso
    const tipoEndosoInstance = $("#tiposEndoso").dxSelectBox({
        placeholder: "Seleccionar...",
        displayExpr: "Texto",
        dataSource: tiposEndoso,
        onValueChanged: function (data) {
            tiposEndoso = data.value.ID;
            console.log(tiposEndoso);

            // Activar botón Enviar si todos los campos están llenos
            var camposLLenos = validarCamposFiltrosEndoso();
            console.log(camposLLenos);
            if (camposLLenos === true) {
                // Activar botón de Enviar
                $('#btnRealizarEndoso').dxButton({
                    disabled: false,
                    elementAttr: {
                        title: "Realizar Endoso.",
                        style: "cursor: default; pointer-events: initial;",
                    },
                });
            }

        },
        elementAttr: {
            id: "tiposEndoso",
            title: "Seleccione el tipo de endoso",
            //  class: "class-name"
        }
    }).dxSelectBox('instance');

    // Cargar datos filtro Tipo Operación Endoso
    const tiposOperacionEventoInstance = $("#tiposOperacionEvento").dxSelectBox({
        placeholder: "Seleccionar...",
        displayExpr: "Texto",
        dataSource: tiposOperacionEvento,
        onValueChanged: function (data) {
            tiposOperacionEvento = data.value.ID;
            console.log(tiposOperacionEvento);

            // Activar botón Enviar si todos los campos están llenos
            var camposLLenos = validarCamposFiltrosEndoso();
            console.log(camposLLenos);
            if (camposLLenos === true) {
                // Activar botón de Enviar
                $('#btnRealizarEndoso').dxButton({
                    disabled: false,
                    elementAttr: {
                        title: "Realizar Endoso.",
                        style: "cursor: default; pointer-events: initial;",
                    },
                });
            }
        },
        elementAttr: {
            id: "tiposOperacionEvento",
            title: "Seleccione el tipo de operación del evento",
            //    class: "class-name"
        }
    }).dxSelectBox('instance');

    // Fltro Tasa de Descuento Endoso
    $("#tasaDescuentoEndoso").dxNumberBox({
        placeholder: "0.0%",
        format: '#0.00%',
        value: null,
        min: 0.01,
        step: 0.01,
      //  max: 10,
        elementAttr: {
            id: "tasaDescuentoEndoso",
            title: "Ingrese el valor del descuento. Permite decimales. Ej: 7.9",
            //    class: "class-name"
        },
        onValueChanged: function (data) {
            tasaDescuentoEndoso = data.value;
            tasaDescuentoEndoso = tasaDescuentoEndoso * 100;
            console.log(tasaDescuentoEndoso);

            // Activar botón Enviar si todos los campos están llenos
            var camposLLenos = validarCamposFiltrosEndoso();
            console.log(camposLLenos);
            if (camposLLenos === true) {
                // Activar botón de Enviar
                $('#btnRealizarEndoso').dxButton({
                    disabled: false,
                    elementAttr: {
                        title: "Realizar Endoso.",
                        style: "cursor: default; pointer-events: initial;",
                    },
                });
            }
        }
    }).dxNumberBox('instance');

    // Campo ID Endosatario
    $("#idEndosatario").dxTextBox({
        placeholder: "Ingresar ID Endosatario...",
        elementAttr: {
            id: "idEndosatario",
            title: "Ingrese el NIT de la empresa que hará de Endosatario",
            //     class: "class-name"
        },
        onValueChanged: function (data) {
            idEndosatario = data.value;
            //console.log(idEndosatario);

            // Validar que el campo ID Endosatario esté lleno.
            if (idEndosatario === '') {
                console.log("No se pudo consultar la empresa porque el campo ID Endotatario está vacío.");

                // Mostrar mensaje
                $("#razonSocialEmpresa").text("No se pudo consultar la empresa porque el campo ID Endotatario está vacío.");

                // Ajustar botón de Enviar
                $('#btnRealizarEndoso').dxButton({
                    disabled: true,
                    elementAttr: {
                        title: "Para realizar el proceso de Endoso primero debe de validar si la empresa existe rellenando el campo ID Endosatario.",
                        //style: "cursor: default; pointer-events: initial;",
                    },
                });

                return;
            }

            // Consultar empresa
            if (!idEndosatario || idEndosatario.trim().length != 0 ) {
                $http.get('/api/ObtenerOperador?identificacion=' + idEndosatario).then(function (response ) {

                    //console.log(response);
                    //console.log(data);
                    //console.log(response["data"]["TipoOperador"]);

                    // Validar si la empresa existe
                    TipoOperador = response["data"]["TipoOperador"];            // Obtener tipo de operador
                    if (TipoOperador != 0) {
                        console.log("La empresa con NIT " + idEndosatario + " existe. Realizar Endoso...");

                        // Mostrar razón social empresa
                        razonSocialEmpresa = response["data"]["RazonSocial"];  // Obtener razón social de la empresa
                        $("#razonSocialEmpresa").text("Razón social empresa: " + razonSocialEmpresa);

                        // Activar botón Enviar si todos los campos están llenos
                        var camposLLenos = validarCamposFiltrosEndoso();
                        console.log(camposLLenos);
                        if (camposLLenos === true) {
                            // Activar botón de Enviar
                            $('#btnRealizarEndoso').dxButton({
                                disabled: false,
                                elementAttr: {
                                    title: "Realizar Endoso.",
                                    style: "cursor: default; pointer-events: initial;",
                                },
                            });
                        }
                    } else {
                        console.log("La empresa con NIT " + idEndosatario + " existe pero no puede realizar Endoso.");

                        // Mostrar mensaje
                        $("#razonSocialEmpresa").text("La empresa con NIT " + idEndosatario + " existe pero no puede realizar Endoso.");

                        // Ajustar botón de Enviar
                        $('#btnRealizarEndoso').dxButton({
                            disabled: true,
                            elementAttr: {
                                title: "La empresa con NIT " + idEndosatario + " existe pero no puede realizar Endoso.",
                                //style: "cursor: default; pointer-events: initial;",
                            },                            
                        });
                    }
                    //debugger;
                });
            }
        }
    });

    $scope.btnRealizarEndoso = {
        text: 'Enviar',
        type: 'default',
        visible: true,
        disabled: true,
        elementAttr: {
            id: "btnRealizarEndoso",
            class: "btnRealizarEndoso",
            title: "Para realizar el proceso de Endoso primero debe de validar si la empresa existe rellenando el campo ID Endosatario.",
            style: "cursor: not-allowed; pointer-events: initial;",
        },
        onClick: function (e) {

            if (tiposEndoso == undefined || tiposEndoso == "") {
                DevExpress.ui.notify('Seleccione el tipo Endoso', 'error', 7000);

                return;
            }

            if (tiposOperacionEvento == undefined || tiposOperacionEvento == "") {
                DevExpress.ui.notify('Seleccione el tipo de operación', 'error', 7000);
            
                return;
            }

            if (tasaDescuentoEndoso == undefined || tasaDescuentoEndoso == "" || tasaDescuentoEndoso == 0.00) {
                DevExpress.ui.notify('Ingrese la tasa de descuento', 'error', 7000);
                
                return;
            }

            $http.post('/api/GenerarEventoRadian?id_seguridad=' + $scope.IdSeguridad + '&tipo_evento=' + tiposEndoso + '&operacion_evento=' + tiposOperacionEvento + '&id_receptor_evento=' + idEndosatario + '&tasa_descuento=' + tasaDescuentoEndoso + '&usuario=').then(function (response) {
                //alert(response.data);
                $rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
            }, function errorCallback(response) {

                //Carga notificación de creación con opción de editar formato.
                var myDialog = DevExpress.ui.dialog.custom({
                    title: "Proceso fallido",
                    message: response.data.ExceptionMessage,
                    buttons: [{
                        text: "Aceptar",
                        onClick: function (e) {
                            myDialog.hide();
                            $rootScope.ConsultarEventosRadian(IdSeguridad, NumeroDocumento, Obligado);
                        }
                    }]
                });
                myDialog.show().done(function (dialogResult) {
                });

                $('#wait').hide();
                $('#panelEndoso').hide();
            });
        }
    };

    // Activar botón
    // Validar campos vacíos panel Filtros para hacer Endoso
    function validarCamposFiltrosEndoso() {
        
        // Obtener valores actuales de los campos
        var tipoEndoso = $('#tiposEndoso').dxSelectBox('instance').option('value');
        var tipoOperacionEvento = $('#tiposOperacionEvento').dxSelectBox('instance').option('value');
        var tasaDescuentoEndoso = $('#tasaDescuentoEndoso').dxNumberBox('instance').option('value');

        if (!tipoEndoso || !tipoOperacionEvento || !tasaDescuentoEndoso) {
            return false;
        } else {
            return true;
        }
    }

});

// Datos para el filtro Tipo Endoso
var tiposEndoso = [
    { ID: "7", Texto: 'Propiedad' },
    //{ ID: "8", Texto: 'Garantía' },
    //{ ID: "15", Texto: 'Procuración' }
];

// Datos para el filtro Tipo Operación Evento
var tiposOperacionEvento = [
    { ID: "0", Texto: 'Con Responsabilidad' },
    //{ ID: "1", Texto: 'Sin Responsabilidad' }
];