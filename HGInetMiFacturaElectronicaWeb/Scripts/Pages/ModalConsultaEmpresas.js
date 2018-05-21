
var ModalEmpresasApp = angular.module('ModalEmpresasApp', ['dx']);
var DatosEmpresa = false;

//Controlador para gestionar la consulta de empresas
ModalEmpresasApp.controller('ModalConsultaEmpresasController', function ModalConsultaEmpresasController($scope, $http, $location) {    
        $("#wait").show();
        $http.get('/api/Empresas').then(function (response) {
            $("#wait").hide();
            DatosEmpresa = response.data;
            $("#gridEmpresas").dxDataGrid({
                dataSource: response.data,
                paging: {
                    pageSize: 10
                },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                }

                   , columns: [

                       {
                           caption: "Identificacion",
                           dataField: "Identificacion",
                           cssClass: "col-md-3"

                       },
                       {
                           caption: "Razón Social",
                           dataField: "RazonSocial",
                           cssClass: "col-md-7"
                       },
                       //{
                       //    caption: "Email",
                       //    dataField: "Email"
                       //},
                       //{
                       //    caption: "Serial",
                       //    dataField: "Serial"
                       //},
                       //{
                       //    dataField: "Perfil"
                       //},
                       //{
                       //    dataField: "Habilitacion"
                       //},
                       {
                           caption: "Empresa",
                           cssClass: "col-md-1 col-xs-2",
                           cellTemplate: function (container, options) {
                               $("<div style='text-align:center'>")
                                   .append($("<a taget=_self class='icon-checkmark4' data-dismiss='modal' title='Seleccionar Empresa Asociada' onclick=ObtenerEmpresa('" + options.data.Identificacion + "')>"))
                                   .appendTo(container);
                           }

                       }
                   ],
                filterRow: {
                    visible: true
                }
            });

        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });    
});
//Funcion para asignar la Empresa Asociada
function ObtenerEmpresa(Empresa) {
    console.log("AAA", DatosEmpresa);
    fLen = DatosEmpresa.length;
    console.log("Longitud", fLen);
    for (i = 0; i < fLen; i++) {
        if (DatosEmpresa[i].Identificacion == Empresa) {
            console.log("Identificacion", DatosEmpresa[i].Identificacion);
            Empresa = Empresa + ' --  ' + DatosEmpresa[i].RazonSocial;
            break;
        }
    }
    $("#txtempresaasociada").dxTextBox({ value: Empresa });
}