


var AcuseReciboApp = angular.module('AcuseReciboApp', ['dx']);
AcuseReciboApp.controller('AcuseReciboController', function AcuseReciboController($scope, $http) {
    

    var IdSeguridad = location.search.split('id_seguridad=')[1];
    var estado = "";
    var motivo_rechazo = "";
    consultar();
    function consultar() {
        //Obtiene los datos del web api
        //ControladorApi: /Api/Documentos/
        //Datos GET: id_seguridad
        $http.get('/api/Documentos?id_seguridad=' + IdSeguridad).then(function (response) {
            $scope.RespuestaAcuse = response.data;

        });

        $scope.TextAreaObservaciones = {

            readOnly: false,
            showColonAfterLabel: true,
            showValidationSummary: true,
            validationGroup: "ObservacionesAcuse",
            onInitialized: function (e) {
                formInstance = e.component;
            },
            items: [{
                itemType: "group",
                items: [
                    {
                        dataField: "Observaciones",
                        editorType: "dxTextArea",
                        label: {
                            text: "Observaciones"
                        },
                        validationRules: [{
                            type: "required",
                            message: "Las observaciones son requeridas."
                        }]
                    }
                ]
            }]
        };
    }

    //Botón Rechazar.
    $scope.ButtonOptionsAceptar = {
        text: "Aceptar",
        type: "success",
        onClick: function (e) {
            estado = 1;
            motivo_rechazo = $('textarea[name=Observaciones]').val();
            ActualizarDatos();
        }
    };

    //Botón Aceptar.
    $scope.ButtonOptionsRechazar = {
        text: "Rechazar",
        validationGroup: "ObservacionesAcuse",
        useSubmitBehavior: true,
    };

    $scope.onFormSubmit = function (e) {

        console.log($('textarea[name=Observaciones]').val());

        motivo_rechazo = $('textarea[name=Observaciones]').val();

        console.log("Entró Rechazar", motivo_rechazo);
        estado = 2;
        ActualizarDatos();
    }


    //Función para actualizar los datos
    function ActualizarDatos() {

        var data = $.param({
            id_seguridad: IdSeguridad,
            estado: estado,
            motivo_rechazo: motivo_rechazo
        });

        $('#wait').show();

        $http.post('/api/Documentos?' + data).then(function (data, response) {
            $scope.ServerResponse = data;
            consultar();
            var id = IdSeguridad;
            $('#wait').hide();
        }, function errorCallback(response) {
            $('#wait').hide();
            console.log("Error..", response.data.ExceptionMessage);
        });
    }


    $http.get('/api/DatosSesion/').then(function (response) {
        
        console.log("Usuario Logeado");
    }, function errorCallback(response) {
        $('#btnautenticar').show();
        console.log("Usuario no autenticado");
    });
   

});

