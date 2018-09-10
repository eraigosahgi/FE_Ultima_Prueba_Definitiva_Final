
DevExpress.localization.locale(navigator.language);

var AcuseReciboApp = angular.module('AcuseReciboApp', ['dx']);
AcuseReciboApp.controller('AcuseReciboController', function AcuseReciboController($scope, $http) {


    var IdSeguridad = location.search.split('id_seguridad=')[1];
    //Almacena el parametro Zpago para ver si debe enviarlo a la pantalla de pago
    var Zpago = location.search.split('Zpago=')[1];
    $scope.DetalleAcuse = true;
    var estado = "";
    var motivo_rechazo = "";

    $scope.habilitar = function () {

        //$http.get('/api/Documentos?strIdSeguridad=' + IdSeguridad + '&pago=true').then(function (response) {
        var id = IdSeguridad.split('&')[0];
        $http.get('/api/ConsultaSaldoDocumento?StrIdSeguridadDoc=' + id).then(function (response) {
            if (response.data != "PagoPendiente" && response.data != "DocumentoCancelado") {
               
                $http.get('/api/Documentos?strIdSeguridad=' + id + '&tipo_pago = 0 &registrar_pago=true&valor_pago=' + response.data).then(function (response) {

                    window.open("http://cloudservices.hginet.co/Views/Pago.aspx?IdSeguridad=" + response.data.Ruta, "_blank");
                    
                    //Si lo envia a la pantalla de pago, cierra la pantalla actual
                    //if (Zpago)
                       // window.close();
                }, function (error) {
                    DevExpress.ui.notify("Problemas con la plataforma de pago", 'error', 7000);
                    $scope.DetalleAcuse = false;
                });
            } else {
                if (response.data == "PagoPendiente") {
                    DevExpress.ui.notify("No puede hacer pagos mientras tenga pagos pendientes", 'error', 7000);
//                    window.close();
                }
                if (response.data == "DocumentoCancelado") {
                    DevExpress.ui.notify("Este documento ya fue pagado", 'error', 7000);
  //                  window.close();
                }
            }
        }, function (error) {
            $scope.DetalleAcuse = false;
            DevExpress.ui.notify("Problemas con la plataforma de pago", 'error', 7000);

        });
    };

    if (Zpago == 'true') {
        //Si parametro Zpago = true entonces lo envia a la pantalla de pago
        $scope.habilitar();
    }

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
    //ValidarEstado(1);
    $scope.ValidarEstado = function (Estado) {
        if (Estado == 1) {
            $scope.AceptarVar = true;
            $scope.RechazarVar = false;
        } else {
            $scope.AceptarVar = false;
            $scope.RechazarVar = true;
        }
    }

    //Botón Rechazar.
    $scope.ButtonOptionsAceptar = {
        text: "Enviar",
        type: "success",
        onClick: function (e) {
            estado = 1;
            motivo_rechazo = $('textarea[name=Observaciones]').val();
            ActualizarDatos();
        }
    };

    //Botón Aceptar.
    $scope.ButtonOptionsRechazar = {
        text: "Enviar",
        type: "success",
        validationGroup: "ObservacionesAcuse",
        useSubmitBehavior: true,
    };

    $scope.onFormSubmit = function (e) {
        motivo_rechazo = $('textarea[name=Observaciones]').val();
        estado = 2;
        ActualizarDatos();
    }


    //Función para actualizar los datos
    function ActualizarDatos() {


        var id = IdSeguridad.split('&')[0];

        var data = $.param({
            id_seguridad: id,
            estado: estado,
            motivo_rechazo: motivo_rechazo
        });


        $('#wait').show();

        $http.post('/api/Documentos?' + data).then(function (data, response) {
            $scope.ServerResponse = data;
            consultar();
            var id = id;
            $('#wait').hide();
        }, function errorCallback(response) {
            $('#wait').hide();
        });
    }

    $('#btnautenticar').show();
    $http.get('/api/DatosSesion/').then(function (response) {
        $('#btnautenticar').hide();
    }, function errorCallback(response) {
        $('#btnautenticar').show();
    });
});


