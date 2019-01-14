DevExpress.localization.locale(navigator.language);
var opc_pagina = "1333";
var ModalSerialEmpresaApp = angular.module('ModalSerialEmpresaApp', []);

var SerialEmpresaApp = angular.module('SerialEmpresaApp', ['ModalSerialEmpresaApp', 'dx', 'appsrvusuario']);

//Servicio para gestionar la consulta de las empresas 
angular.module("appsrvusuario", [])
.factory("srvusuario", function ($http) {

    var Scope = function () { }
    Scope.consulta = function () {
        $("#wait").show();
        $http.get('/api/Empresas?Facturador=true').then(function (response) {
            $("#wait").hide();

            $("#gridEmpresas").dxDataGrid({
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
                 }
                      , allowColumnResizing: true
                   , columns: [
                       {
                           //href='GestionEmpresas.aspx?IdSeguridad=" + options.data.IdSeguridad + "'
                           cssClass: "col-md-1 col-xs-2",
                           cellTemplate: function (container, options) {
                               if (options.data.Serial != "" && options.data.Serial != null) {
                                   Scope.estilo = "'icon-mail-read' data-toggle='modal' data-target='#modal_enviar_email' style='margin-left:12%; font-size:19px'";
                               } else {
                                   Scope.estilo = "'icon-mail-read' style='margin-left:12%; font-size:19px; pointer-events:auto;cursor: not-allowed;'";
                               };
                               $("<div style='text-align:center'>")
                                   .append(
                                   $("<a class='icon-pencil3' data-toggle='modal' data-target='#modal_Serial_empresa' style='margin-left:12%; font-size:19px'></a>").dxButton({
                                       onClick: function () {
                                           Scope.email = options.data.Email;
                                           $('#txtemail').dxTextBox({ value: Scope.email });
                                           $("#txtemail").removeClass("dx-textbox dx-texteditor dx-state-readonly dx-widget");

                                           Scope.nitEmpresa = options.data.Identificacion;
                                           $('#txtnitEmpresa').dxTextBox({ value: Scope.nitEmpresa });
                                           $("#txtnitEmpresa").removeClass("dx-textbox dx-texteditor dx-state-readonly dx-widget");

                                           Scope.nombre = options.data.RazonSocial;
                                           $('#txtnombre').dxTextBox({ value: Scope.nombre });
                                           $("#txtnombre").removeClass("dx-textbox dx-texteditor dx-state-readonly dx-widget");

                                           $("#txtResolucion").dxTextBox({ value: options.data.Resolucion });
                                           $("#txtSerial").dxTextBox({ value: options.data.Serial });
                                       }
                                   }).removeClass("dx-button dx-button-normal dx-widget")
                                   ///Envio de Mail                               
                                   , $("<a class=" + Scope.estilo + "></a>").dxButton({
                                       onClick: function () {
                                           Scope.showModal = true;
                                           email_destino = options.data.Email;
                                           Scope.indetificacion = options.data.Identificacion;
                                           $('#txtnitEmpresamail').dxTextBox({ value: options.data.Identificacion });
                                           $("#txtnitEmpresamail").removeClass("dx-textbox dx-texteditor dx-state-readonly dx-widget");

                                           Scope.serial = options.data.Serial;
                                           $("#txtserialmail").dxTextBox({ value: options.data.Serial });
                                           $("#txtserialmail").removeClass("dx-textbox dx-texteditor dx-state-readonly dx-widget");

                                           $('#txtnombremail').dxTextBox({ value: options.data.RazonSocial });
                                           $("#txtnombremail").removeClass("dx-textbox dx-texteditor dx-state-readonly dx-widget");

                                           Scope.mail = email_destino;
                                           $('input:text[name=EmailDestino]').val(email_destino);
                                       }
                                   }).removeClass("dx-button dx-button-normal dx-widget")
                                   ).appendTo(container);
                           }
                       },

                       {
                           caption: "Identificacion",
                           dataField: "Identificacion"
                       },
                       {
                           caption: "Razón Social",
                           dataField: "RazonSocial"
                       },
                       {
                           caption: "Email",
                           dataField: "Email"
                       },
                       {
                           caption: "Serial",
                           dataField: "Serial"
                       },{
                       	caption: "Datakey",
                       	dataField: "datakey"
                       },
					   
                       {
                           caption: "Resolución",
                           dataField: "Resolucion"
                       }
                   ],
                filterRow: {
                    visible: true
                }
            });

        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });
    }
    return Scope;
});

//Controlador para gestionar el envio del Email con el serial desde el nuevo modal
SerialEmpresaApp.controller('SerialEmpresaController', function SerialEmpresaController($scope, $http, $location, srvusuario) {

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
                    }, {
                        type: "email",
                        message: "El email no no tiene el formato correcto"
                    }]
                }
            ]
        }
        ]
    };


    $("#txtnitEmpresamail").dxTextBox({
        readOnly: true,
    });

    $("#txtserialmail").dxTextBox({
        readOnly: true,
    });
    $("#txtnombremail").dxTextBox({
        readOnly: true,
    });


    //botón Cerrar Modal
    $scope.buttonCerrarModal = {
        text: "CERRAR"
    };

    //Botón Enviar email
    $scope.buttonEnviarEmail = {
        text: "ENVIAR",
        type: "success",
        onClick: function (e) {

            try {

                email_destino = $('input:text[name=EmailDestino]').val();

                if (email_destino == "") {
                    throw new DOMException("El e-mail de destino es obligatorio.");
                }

                var data = $.param({
                    Identificacion: srvusuario.indetificacion,
                    Mail: email_destino
                });
                if ($scope.serial == "") {
                    DevExpress.ui.notify("No se puede enviar email si no posee Serial", 'error', 10000);
                } else {
                    $('#wait').show();
                    $http.post('/api/Empresas?' + data).then(function (responseEnvio) {
                        $('#wait').hide();
                        var respuesta = responseEnvio.statusText;

                        if (respuesta) {
                            swal({
                                title: 'Proceso Éxitoso',
                                text: 'Se ha enviado el Serial a la siguiente dirección : ' + email_destino + ' con exito',
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
                        $('#btncerrarmodal').click();
                        $('input:text[name=EmailDestino]').val("");
                    }, function errorCallback(response) {
                        $('#wait').hide();
                        DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 10000);
                    });
                }
            } catch (e) {
                $('#wait').hide();
                DevExpress.ui.notify(e.message, 'error', 10000);
            }
        }
    };

    // $http.get('/api/DatosSesion/').then(function (response) {
    //   var codigo_facturador = response.data[0].Identificacion;
    $scope.consulta = srvusuario.consulta();
    //});
});

//Controlador para gestion el registro del serial
ModalSerialEmpresaApp.controller('ModalSerialEmpresaController', function ModalSerialEmpresaController($scope, $http, $location, srvusuario) {


    $http.get('/api/DatosSesion/').then(function (response) {
        var codigo_facturador = response.data[0].Identificacion;

        //Obtiene el usuario autenticado.
        $http.get('/api/Usuario/').then(function (response) {
            //Obtiene el código del permiso.
            $http.get('/api/Permisos?codigo_usuario=' + response.data[0].CodigoUsuario + '&identificacion_empresa=' + codigo_facturador + '&codigo_opcion=' + opc_pagina).then(function (response) {
                $("#wait").hide();
                try {
                    $scope.Visibilidad = response.data[0].Editar;
                    CargarCampos();
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });
        });
    });




    var Datos_Resolucion = "", Datos_Serial = "", Datos_correo = "";

    //Define los campos del Formulario  
    function CargarCampos() {


        $("#summary").dxValidationSummary({});

        $("#txtSerial").dxTextBox({

            onValueChanged: function (data) {
                Datos_Serial = data.value;
            }
        })
       .dxValidator({
           validationRules: [{
               type: "required",
               message: "Debe introducir el Serial"
           }, {
               type: "stringLength",
               max: 50,
               message: "El Nª de Serial no puede ser mayor a 50 Digitos"
           }]
       });


        $("#txtResolucion").dxTextBox({

            onValueChanged: function (data) {
                Datos_Resolucion = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir el codigo de Resolución"
            }, {
                type: "numeric",
                message: "El codigo de Resolución debe ser numerico"
            }, {
                type: "stringLength",
                max: 20,
                message: "El Nª de Resolución no puede ser mayor a 20 Digitos"
            }]
        });




        $("#txtnitEmpresa").dxTextBox({
            readOnly: true,
            onValueChanged: function (data) {
                $scope.nitEmpresa = data.value;
            }
        });

        $("#txtemail").dxTextBox({
            readOnly: true,
            onValueChanged: function (data) {
                $scope.email = data.value;
            }
        });
        $("#txtnombre").dxTextBox({
            readOnly: true,
        });



        $("#btnActivar").dxButton({
            text: "Guardar",
            type: "default",
            useSubmitBehavior: true
            , onClick: function () {
                guardarSerial();
            }
        });


        $("#form1").on("submit", function (e) {
            e.preventDefault();
        });


    }


    function guardarSerial() {
        if (Datos_Resolucion != '' && Datos_Serial != '') {


            Datos_Identificacion = $scope.nitEmpresa;
            var data = $.param({
                Identificacion: Datos_Identificacion,
                Serial: Datos_Serial,
                Resolucion: Datos_Resolucion
            });

            $("#wait").show();
            $http.post('/api/Empresas?' + data).then(function (response) {
                $("#wait").hide();
                try {
                    //Aqui se debe colocar los pasos a seguir
                    DevExpress.ui.notify({ message: "Se ha enviado un correo a " + $scope.email + " ", position: { my: "center top", at: "center top" } }, "success", 1500);
                    $("#btnActivar").hide();
                    $("#btncancelar").hide();
                    $scope.consulta = srvusuario.consulta();
                    setTimeout(IrAConsulta, 2000);
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });
        } else {

        }
    }


});

//Esta funcion es para ir a la pagina de consulta
function IrAConsulta() {
    $("#btnActivar").show();
    $("#btncancelar").show();
    $("#txtResolucion").dxTextBox({ value: '' });
    $("#txtSerial").dxTextBox({ value: '' });
    //Cierro la modal
    $('#cerrrarModal').click();
    Datos_Resolucion = '';
    Datos_Serial = ''
}

//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
    for (var i = 0; i < miArray.length; i += 1) {
        if (ID == miArray[i].ID) {
            return i;
        }
    }
}

var TiposHabilitacion =
    [
        { ID: "0", Texto: 'Valida Objeto' },
        { ID: "1", Texto: 'Pruebas' },
        { ID: "99", Texto: 'Producción' }
    ];

var TiposIdentificacion =
    [
        { ID: "11", Texto: 'Registro civil' },
        { ID: "12", Texto: 'Tarjeta de identidad' },
        { ID: "13", Texto: 'Cédula de ciudadanía' },
        { ID: "21", Texto: 'Tarjeta de extranjería' },
        { ID: "22", Texto: 'Cédula de extranjería' },
        { ID: "31", Texto: 'NIT' },
        { ID: "41", Texto: 'Pasaporte' },
        { ID: "42", Texto: 'Documento de identificación extranjero' }
    ];
