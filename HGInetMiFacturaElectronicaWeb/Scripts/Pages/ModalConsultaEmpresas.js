
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
                   caption: "Identificación",
                   dataField: "Identificacion",
                   cssClass: "col-md-3",
                   cellTemplate: function (container, options) {
                       $("<div style='text-align:left'>")
                           .append($("<a taget=_self  data-dismiss='modal' title='Seleccionar Empresa' onclick=ObtenerEmpresa('" + options.data.Identificacion + "')>" + options.data.Identificacion + "</a>"))
                           .appendTo(container);
                   }

               }
                   ,
                   {
                       caption: "Razón Social",
                       dataField: "RazonSocial",
                       cssClass: "col-md-9"
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
    fLen = DatosEmpresa.length;
    for (i = 0; i < fLen; i++) {
        if (DatosEmpresa[i].Identificacion == Empresa) {
            Empresa = Empresa + ' --  ' + DatosEmpresa[i].RazonSocial;
            break;
        }
    }
    $("#txtempresaasociada").dxTextBox({ value: Empresa });
}