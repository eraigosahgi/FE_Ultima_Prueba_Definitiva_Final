DevExpress.localization.locale(navigator.language);


var path = window.location.pathname;
var ruta = window.location.href;
ruta = ruta.replace(path, "/");
document.write('<script type="text/javascript" src="' + ruta + 'Scripts/Services/SrvDocumentos.js"></script>');

var AcuseConsultaApp = angular.module('AcuseConsultaApp', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento']);
AcuseConsultaApp.controller('AcuseConsultaController', function AcuseConsultaController($scope, SrvMaestrosEnum, SrvDocumento) {

    $scope.total = 0;
    $('#lbltotaldocumentos').val("");

    var now = new Date();

    var codigo_facturador = "",
               codigo_adquiriente = "",
               numero_documento = "",
               fecha_inicio = "",
               fecha_fin = "",
               estado_acuse = "";

    SrvMaestrosEnum.ObtenerSesion().then(function (data) {
        codigo_facturador = data[0].Identificacion;
        consultar();
    });

    /*
    $("#FechaInicial").dxDateBox({
        value: now,
        width: '100%',
        displayFormat: "yyyy-MM-dd",
        onValueChanged: function (data) {
            fecha_inicio = new Date(data.value).toISOString();
            $("#FechaFinal").dxDateBox({ min: fecha_inicio });
        }

    });

    $("#FechaFinal").dxDateBox({
        value: now,
        width: '100%',
        displayFormat: "yyyy-MM-dd",
        onValueChanged: function (data) {
            fecha_fin = new Date(data.value).toISOString();
            $("#FechaInicial").dxDateBox({ max: fecha_fin });
        }

    });
    */
    $scope.filtros =
           {
               NumeroDocumento: {
                   placeholder: "Ingrese Número Documento",
                   onValueChanged: function (data) {
                       numero_documento = data.value;
                   }
               },
               Adquiriente: {
                   placeholder: "Ingrese Identificación del Adquiriente",
                   onValueChanged: function (data) {
                       codigo_adquiriente = data.value;
                   }
               }
           }

    $("#FechaFinal").dxDateBox({ min: now });
    $("#FechaInicial").dxDateBox({ max: now });

    $scope.ButtonOptionsConsultar = {
        text: 'Consultar',
        type: 'default',
        onClick: function (e) {
            consultar();
        }
    };



   
    function consultar() {
        if (fecha_inicio == "")
            fecha_inicio = now.toISOString();

        if (fecha_fin == "")
            fecha_fin = now.toISOString();
     
        SrvDocumento.ObtenerDocumentosTacito(codigo_facturador,codigo_adquiriente,numero_documento,fecha_inicio,fecha_fin).then(function (data) {
        //$http.get('/api/ConsultaAcuseTacito?codigo_facturador=' + codigo_facturador + '&codigo_adquiriente=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {            
            $("#gridDocumentos").dxDataGrid({
                dataSource: data,
                paging: {
                    pageSize: 20
                },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                },
                selection: {
                    mode: "multiple",

                }

            
                 , loadPanel: {
                     enabled: true
                 }
                      , allowColumnResizing: true
                , columns: [

                     {
                         caption: "Identificación Adquiriente",
                         dataField: "IdentificacionAdquiriente"
                     },
                      {
                          caption: "Nombre Adquiriente",
                          dataField: "RazonSocial"
                      },
                      {
                          caption: "Número Documento",
                          dataField: "NumeroDocumento",
                      }
                      , {
                          caption: "Fecha Documento",
                          dataField: "Fecha",
                          dataType: "date",
                          format: "yyyy-MM-dd",
                      }, {
                          caption: "Fecha Ingreso",
                          dataField: "FechaIngreso",
                          dataType: "date",
                          format: "yyyy-MM-dd HH:mm",
                      },
                      
                       {
                           caption: "Horas",
                           dataField: "dias",
                       },
                       /*
                      {
                          caption: "Motivo Rechazo",
                          dataField: "MotivoRechazo",
                      }*/
                ],
                filterRow: {
                    visible: true
                }, onSelectionChanged: function (selectedEstado) {
                    var lista = '';
                    var data = selectedEstado.selectedRowsData;
                    if (data.length > 0) {
                        if (data.length > 1) {
                            for (var i = 0; i < data.length; i++) {
                                lista += (lista) ? ',' : '';
                                lista += "{Documentos: '" + data[i].NumeroDocumento + "'}";
                            }
                            lista = "[" + lista + "]"
                            $scope.documentos = lista;
                            $('#lbltotaldocumentos').val('Documentos : ' + data.length);

                            $scope.$apply(function () {
                                $scope.total = data.length;
                            });
                            

                        } else {
                            lista += "{Documentos: '" + data[0].NumeroDocumento + "'}";
                            lista = "[" + lista + "]"
                            $scope.documentos = lista;
                            $('#lbltotaldocumentos').val('Documento : ' + data.length);
                            $scope.$apply(function () {
                                $scope.total = data.length;
                            });

                        }
                    } else {
                        $('#lbltotaldocumentos').val('Ningún Documento seleccionado');
                        $scope.documentos = "Ningún Documento seleccionado";
                        $scope.$apply(function () {
                            $scope.total = 0;
                        });

                    }
                }
            });
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });

    }


    $("#btnProcesar").dxButton({
        text: "Enviar Documentos",
        type: "default",
        onClick: ProcesarDocumentos
    });

    function ProcesarDocumentos() {
        
        SrvDocumento.GenerarAcuseTacito($scope.documentos).then(function (data) {
            DevExpress.ui.notify("Documentos actualizados con exito", "success", 3000);
            consultar();
        });
    }
});