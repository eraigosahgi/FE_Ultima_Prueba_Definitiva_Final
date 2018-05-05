


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

            if (respuesta) {
                console.log(respuesta);
                window.location.assign("../Pages/Inicio.aspx");
            }
            else {
                //DevExpress.ui.notify(mensaje, tipo, tiempo);
                DevExpress.ui.notify("Datos de autenticación inválidos.", 'error', 3000);
            }

        });

    };

});

