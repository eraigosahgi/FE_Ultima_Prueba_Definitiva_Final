


var AutenticacionApp = angular.module('AutenticacionApp', ['dx']);
AutenticacionApp.controller('AutenticacionController', function AutenticacionController($scope, $http, $location) {

    var dato_identificacion = "",
    dato_usuario = "",
    dato_contrasena = "";


    $scope.formOptions = {

        readOnly: false,
        showColonAfterLabel: true,
        showValidationSummary: true,
        validationGroup: "DatosAutenticacion",
        onInitialized: function (e) {
            formInstance = e.component;
        },
        //Fomulario Autenticación
        items: [{
            itemType: "group",
            items: [
                {
                    dataField: "Identificacion",
                    editorType: "dxTextBox",
                    label: {
                        text: "Identificación"
                    },
                    validationRules: [{
                        type: "required",
                        message: "El documento de identificación es requerido."
                    }, {
                        //Valida que el campo solo contenga números
                        type: "pattern",
                        pattern: "^[0-9]+$",
                        message: "El campo no debe contener letras ni caracteres especiales."
                    }]
                },
                {
                    dataField: "Usuario",
                    editorType: "dxTextBox",
                    label: {
                        text: "Usuario"
                    },
                    validationRules: [{
                        type: "required",
                        message: "El usuario es requerido."
                    }]
                },
            {
                dataField: "Clave",
                editorType: "dxTextBox",
                label: {
                    text: "Contraseña"
                },
                editorOptions: {
                    mode: "password"
                },
                validationRules: [{
                    type: "required",
                    message: "La contraseña es requerida."
                }]
            }
            ]
        }

        ]

    };

    //Opciones de botón -  valida el formulario
    $scope.buttonOptions = {
        text: "INGRESAR",
        type: "default",
        useSubmitBehavior: true,
        validationGroup: "DatosAutenticacion"
    };

    //Evento del botón.
    $scope.onFormSubmit = function (e) {
        $('#wait').show();
        console.log("Ingresó al evento del botón");

        dato_identificacion = $('input:text[name=Identificacion]').val();
        dato_usuario = $('input:text[name=Usuario]').val();
        dato_contrasena = $('input:password[name=Clave]').val();

        console.log("Identificación:", dato_identificacion, " Usuario:", dato_usuario, " Contraseña:", dato_contrasena);

        //Obtiene los datos del web api
        //ControladorApi: /Api/Usuario/
        //Datos GET: codigo_empresa - codigo_usuario - clave
        $http.get('/Api/Usuario?codigo_empresa=' + dato_identificacion + '&codigo_usuario=' + dato_usuario + '&clave=' + dato_contrasena).then(function (response) {

            var respuesta = response.data;

            if (respuesta.length != 0) {
                window.location.assign("../Pages/Inicio.aspx?ID=" + response.data[0].Token);
            }            
            else {
                //DevExpress.ui.notify(mensaje, tipo, tiempo);
                DevExpress.ui.notify("Datos de autenticación inválidos.", 'error', 3000);
            }
           

        });

    };

});


//Controlador para Restablecer Contraseña
//ControladorApi: /Api/Usuario/
//Datos PUT: codigo_empresa - codigo_usuario 
AutenticacionApp.controller('RestablecerController', function RestController($scope, $http, $location) {

    $scope.formOptions2 = {

        readOnly: false,
        showColonAfterLabel: true,
        showValidationSummary: true,
        validationGroup: "DatosRestablecer",
        onInitialized: function (e) {
            formInstance = e.component;
        },
        items: [{
            itemType: "group",
            items: [
                {
                    dataField: "TextBoxDocIdentificacion",
                    editorType: "dxTextBox",
                    label: {
                        text: "Documento Identificación"
                    },
                    validationRules: [{
                        type: "required",
                        message: "El documento de identificación es requerido."
                    }, {
                        //Valida que el campo solo contenga números
                        type: "pattern",
                        pattern: "^[0-9]+$",
                        message: "El campo no debe contener letras ni caracteres especiales."
                    }]
                },
                {
                    dataField: "TextBoxUsuario",
                    editorType: "dxTextBox",
                    label: {
                        text: "Código de Usuario"
                    },
                    validationRules: [{
                        type: "required",
                        message: "El Código de Usuario es requerido."
                    }]
                }
            ]
        }

        ]

    };


    //Opciones de botón -  valida el formulario
    $scope.buttonCerrarRestablecer = {
        text: "CERRAR"
    };


    //Opciones de botón -  valida el formulario
    $scope.buttonRestablecer = {
        text: "ENVIAR",
        type: "success",
        useSubmitBehavior: true,
        validationGroup: "DatosRestablecer"
    };

    //Evento del botón.
    $scope.onFormSubmit = function (e) {
        $('#wait').show();
        console.log("Ingresó al evento del botón");

        dato_identificacion = $('input:text[name=TextBoxDocIdentificacion]').val();
        dato_usuario = $('input:text[name=TextBoxUsuario]').val();

        //aqui doy valor a los parametros que van al webapi
        var data = $.param({
            codigo_empresa: dato_identificacion,
            codigo_usuario: dato_usuario
        });
        console.log("Identificación:", dato_identificacion, " Usuario:", dato_usuario);

        //Obtiene los datos del web api
        //ControladorApi: /Api/Usuario/
        //Datos PUT: codigo_empresa - codigo_usuario
        $http.post('/Api/Usuario?' + data).then(function (response) {
            //var respuesta = response.data;
            $('#wait').hide();
            swal({
                title: 'Solicitud Éxitosa',
                text: 'Se ha enviado un e-mail para el restablecimiento de contraseña',
                type: 'success',
                confirmButtonColor: '#66BB6A',
                confirmButtonText: 'Aceptar',
                animation: 'pop',
                html: true,
            });
            //swal("Solicitud Éxitosa", "Se ha enviado un e-mail para el restablecimiento de contraseña " , "success");

            $('input:text[name=TextBoxDocIdentificacion]').val("");
            $('input:text[name=TextBoxUsuario]').val("");
            $("#modal_restablecer_clave").removeClass("modal fade in").addClass("modal fade");

            $('.modal-backdrop').remove();


        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 8000);
        });
        e.preventDefault();
    };

});


$(function () {

    $("#galleryContainer").dxGallery({
        dataSource: [
            "http://habilitacion.mifacturaenlinea.com.co/Scripts/Images/1.png",
            "http://habilitacion.mifacturaenlinea.com.co/Scripts/Images/2.png",
            "http://habilitacion.mifacturaenlinea.com.co/Scripts/Images/3.png",
            "http://habilitacion.mifacturaenlinea.com.co/Scripts/Images/4.png"
        ],
        swipeEnabled: true,
        showNavButtons: true
        , loop: true
        , slideshowDelay: 3000
    });

});