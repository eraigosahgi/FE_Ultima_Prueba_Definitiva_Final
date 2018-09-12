
DevExpress.localization.locale(navigator.language);

var AcuseReciboApp = angular.module('AcuseReciboApp', ['dx']);
AcuseReciboApp.controller('AcuseReciboController', function AcuseReciboController($scope, $http) {


    var IdSeguridad = location.search.split('id_seguridad=')[1];
    //Almacena el parametro Zpago para ver si debe enviarlo a la pantalla de pago
    var Zpago = location.search.split('Zpago=')[1];
    $scope.DetalleAcuse = true;
    var estado = "";
    var motivo_rechazo = "";
    //Id unico de registro de pago
    $scope.Idregistro;
    $scope.IdSeguridad = IdSeguridad;

    $scope.habilitar = function () {

        //$http.get('/api/Documentos?strIdSeguridad=' + IdSeguridad + '&pago=true').then(function (response) {
        var id = IdSeguridad.split('&')[0];
        $http.get('/api/ConsultaSaldoDocumento?StrIdSeguridadDoc=' + id).then(function (response) {
            if (response.data != "PagoPendiente" && response.data != "DocumentoCancelado" && response.data != "ErrorDian") {
               
                $http.get('/api/Documentos?strIdSeguridad=' + id + '&tipo_pago = 0 &registrar_pago=true&valor_pago=' + response.data).then(function (response) {

                    //window.open("http://cloudservices.hginet.co/Views/Pago.aspx?IdSeguridad=" + response.data.Ruta, "_blank");
                    window.open("http://atila.hginet.co:8890/CloudServices/Views/Pago.aspx?IdSeguridad=" + response.data.Ruta, "_blank");

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
                }
                if (response.data == "DocumentoCancelado") {
                    DevExpress.ui.notify("Este documento ya fue pagado", 'error', 7000);
                }

                if (response.data == "ErrorDian") {
                    DevExpress.ui.notify("El estado actual del documento, no permite hacer pagos", 'error', 7000);                    
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
            console.log("Estatus del pago: ", response.data[0].Estatus);
            //Si estatus es igual a 2, entonces asigo los valores a las variables para ejecutar la consulta de saldo
            if (response.data[0].Estatus==2) {
                $scope.Idregistro = response.data[0].pago[0].StrIdRegistro                
                VerificarEstado();
            }
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



    ////Verificación de pago pendiente

    function EsperayValida() {
        ///Se va a ejecutar automaticamente la sonda de consulta mientras el pago este pendiente
        if ($scope.pagoenVerificacion) {
            $timeout(function callAtTimeout() {
                VerificarEstado();
            }, 60000);
        }
    }


    function VerificarEstado() {

            $http.get('http://atila.hginet.co:8890/CloudServices/api/VerificarEstado?IdSeguridadPago=' + $scope.IdSeguridad + "&StrIdSeguridadRegistro=" + $scope.Idregistro).then(function (response) {
            //$http.get('http://localhost:50145/api/VerificarEstado?IdSeguridadPago=' + $scope.IdSeguridad + "&StrIdSeguridadRegistro=" + $scope.Idregistro).then(function (response) {
            // $http.get('http://cloudservices.hginet.co/api/VerificarEstado?IdSeguridadPago=' + $scope.IdSeguridad + "&StrIdSeguridadRegistro=" + $scope.Idregistro).then(function (response) {
            //Esto retorna un objeto  de la plataforma intermedia que sirve para actualizar el pago local
            var ObjeRespuestaPI = response.data;
            //////////////////////////////////////////////////////////////////////
            $http.get('/Api/ActualizarEstado?IdSeguridad=' + $scope.IdSeguridad + "&StrIdSeguridadRegistro=" + $scope.Idregistro + '&Pago=' + ObjeRespuestaPI).then(function (response) {
                
              
            }), function (response) {

                $scope.EnProceso = false;
                Mensaje(response.data.ExceptionMessage, "error");
            };

        }), function (response) {

            $scope.EnProceso = false;
            Mensaje(response.data.ExceptionMessage, "error");
        }

    }





});


