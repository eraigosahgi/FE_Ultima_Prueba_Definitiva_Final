


var DemoApp = angular.module('AcuseReciboApp', ['dx']);
DemoApp.controller('AcuseReciboController', function DemoController($scope, $http) {

    var IdSeguridad = location.search.split('id_seguridad=')[1];
    var estado = "";
    var motivo_rechazo = "";

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
            ActualizarDatos();
        }
    };

    //Botón Aceptar.
    $scope.ButtonOptionsRechazar = {
        text: "Rechazar",
        validationGroup: "ObservacionesAcuse",
        useSubmitBehavior: true,
        onClick: function (e) {
            console.log("Entró Rechazar");
            estado = 2;
            ActualizarDatos();
        }
    };

    //Función para actualizar los datos
    function ActualizarDatos() {

        motivo_rechazo = $('input:text[name=Observaciones]').val();

        console.log("Rechazo", motivo_rechazo);

        var data = $.param({
            id_seguridad: IdSeguridad,
            estado: estado,
            motivo_rechazo: motivo_rechazo
        });

        $http.put('/api/Documentos?' + data).success(function (data, response) {
            $scope.ServerResponse = data;
        });

       location.reload();
    }

});
