


var DemoApp = angular.module('AcuseReciboApp', ['dx']);
DemoApp.controller('AcuseReciboController', function DemoController($scope, $http) {

    var IdSeguridad = location.search.split('id_seguridad=')[1];
    var estado = "";
    var motivo_rechazo = "";
    //Obtiene los datos del web api
    //ControladorApi: /Api/Documentos/
    //Datos GET: id_seguridad
    $http.get('/api/Documentos?id_seguridad=' + IdSeguridad).then(function (response) {

        console.log(response.data);
        $scope.RespuestaAcuse = response.data;

        //TextArea
        $scope.editableTextArea = {
            height: 80,
            onValueChanged: function (data) {
                console.log("Observaciones", data.value);
                motivo_rechazo = data.value;
            }
        };

        //Botón Rechazar.
        $scope.ButtonOptionsAceptar = {
            text: "Aceptar",
            type: "success",
            onClick: function (e) {
                estado = 1;
            }
        };

        //Botón Aceptar
        $scope.ButtonOptionsRechazar = {
            text: "Rechazar",
            onClick: function (e) {
                console.log("entró rechazo");
                estado = 2;
          
                var data = $.param({
                    id_seguridad: IdSeguridad,
                    estado: estado,
                    motivo_rechazo: motivo_rechazo
                });

                $http.put('/api/Documentos?' + data).success(function (data, status, headers) {
                    $scope.ServerResponse = data;
                    console.log("data",data);
                    console.log("status", status);
                });

            }
        };


    });

});
