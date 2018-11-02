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
               codigo_Facturador_consulta = "";

    SrvMaestrosEnum.ObtenerSesion().then(function (data) {
        codigo_facturador = data[0].Identificacion;
        consultar();
    });

    $scope.filtros =
           {

               Facturador:{
                   placeholder: "Ingrese Identificación del Facturador",
                   onValueChanged: function (data) {
                       codigo_Facturador_consulta = data.value;
                   }
               },
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

        SrvDocumento.ObtenerDocumentosTacito(codigo_Facturador_consulta, codigo_adquiriente, numero_documento).then(function (data) {
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
                    caption: "Identificación Facturador",
                    dataField: "IdentificacionFacturador"
                },
                    {
                        caption: "Nombre Facturador",
                        dataField: "NombreFacturador"
                    },
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

                    var regex = /(\d+)/g;

                    if (data.length > 0) {
                        if (data.length > 1) {
                            for (var i = 0; i < data.length; i++) {
                                lista += (lista) ? ',' : '';
                                lista += "{Documentos: '" + data[i].NumeroDocumento.match(regex) + "'}";
                            }
                            lista = "[" + lista + "]"
                            $scope.documentos = lista;
                            $('#lbltotaldocumentos').val('Documentos : ' + data.length);

                            $scope.$apply(function () {
                                $scope.total = data.length;
                            });


                        } else {
                            lista += "{Documentos: '" + data[0].NumeroDocumento.match(regex) + "'}";
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