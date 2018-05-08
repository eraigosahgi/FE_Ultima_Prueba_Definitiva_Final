


var AcuseReciboApp = angular.module('AcuseReciboApp', ['dx']);
AcuseReciboApp.controller('AcuseReciboController', function AcuseReciboController($scope, $http) {

    var IdSeguridad = location.search.split('id_seguridad=')[1];
    var estado = "";
    var motivo_rechazo = "";

    //Obtiene los datos del web api
    //ControladorApi: /Api/Documentos/
    //Datos GET: id_seguridad
    $http.get('/api/Documentos?id_seguridad=' + IdSeguridad).then(function (response) {
        $scope.RespuestaAcuse = response.data;
        console.log(response.data);
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

        $http.post('/api/Documentos?'+ data).success(function (data, response) {
            $scope.ServerResponse = data;
        });

        location.reload();
    }

});
