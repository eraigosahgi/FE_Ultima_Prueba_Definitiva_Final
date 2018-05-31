
//Envia la nueva Contraseña y valida si es valido el id_seguridad
//ControladorApi: /Api/usuarios/
//Datos PUT: id_seguridad, y Contraseña

var DemoApp = angular.module('RestablecerClaveApp', ['dx']);
DemoApp.controller('RestablecerClaveController', function DemoController($scope, $http) {

    var id_seguridad = location.search.split('id_seguridad=')[1];
    
    //Valido si el link viene con el id de seguridad para no mostrar el panel de los datos
    if (id_seguridad != undefined) {
        $http.get('/api/Usuario?id_seguridad=' + id_seguridad).then(function (response) {

            $scope.RespuestaIdSeguridad = response.data;

        }, function errorCallback(response) {
            Mensaje(response.data.ExceptionMessage, "error",13000);
            $("#divcontenido").hide();
        });
    } else {
        $("#divcontenido").hide();        
    }

    var clave = "";
    var formInstance;

    var formData = {
        "Password": ""
    };

    $scope.formOptions = {
        formData: formData,
        readOnly: false,
        showColonAfterLabel: true,
        showValidationSummary: true,
        validationGroup: "DatosContraseña",
        onInitialized: function (e) {
            formInstance = e.component;
        },
        items: [{
            itemType: "group",
            items: [{
                label: {
                    text: "Contraseña"
                },
                dataField: "Password",
                editorOptions: {
                    mode: "password"
                },
                validationRules: [{
                    type: "required",
                    message: "La Contraseña es requerida "
                }]
            }, {
                label: {
                    text: "Confirmar Contraseña"
                },
                editorType: "dxTextBox",
                editorOptions: {
                    mode: "password"
                },
                validationRules: [{
                    type: "required",
                    message: "Confirmacion de la Contraseña es requerida"
                }, {
                    type: "compare",
                    message: "La Contraseña y la Confirmaciòn no coinciden",
                    comparisonTarget: function () {
                        return formInstance.option("formData").Password;
                    }
                }]
            }]
        }]
    };

    $scope.buttonAceptar = {
        text: "Aceptar",
        type: "success",
        useSubmitBehavior: true,
        validationGroup: "DatosContraseña"
    };

    $scope.onFormSubmit = function (e) {
        var id_seguridad = location.search.split('id_seguridad=')[1];
        //Asigno valor de la clave a la variable 
        clave = $('input:password[name=Password]').val();

        //aqui doy valor a los parametros que van al webapi
        var data = $.param({
            id_seguridad: id_seguridad,
            clave: clave
        });        
        //Aqui Valido si el id de seguridad viene en el get para cancelar la operacion en caso de que no venga
        if (id_seguridad == undefined) {
            Mensaje("Link Incorrecto", "error");
        } else {
            $http.post('/api/Usuario?'+ data).then(function (response) {

                $scope.RespuestaIdSeguridad = response.data;                

                
                //Aqui debe ir el                 
                $("#divcontenido").hide();

                swal({
                    title: 'Solicitud Éxitosa',
                    text: 'Su contraseña ha sido restablecida exitosamente.',
                    type: 'success',
                    confirmButtonColor: '#66BB6A',
                    confirmButtonText: 'Aceptar',
                    animation: 'pop',
                    html: true,
                }).then((value) => {
                    window.location.assign("../Login/Default.aspx");
                });

            }, function errorCallback(response) {                
                Mensaje(response.data.ExceptionMessage, "error");
            });
        }

        e.preventDefault();
    };


    function Mensaje(strmensaje, tipo, tiempo) {
        if (tiempo == undefined) { tiempo = 3000 }
        DevExpress.ui.notify({
            message: strmensaje,
            position: {
                my: "center top",
                at: "center top"
            }
        }, tipo, tiempo);
    }
});


