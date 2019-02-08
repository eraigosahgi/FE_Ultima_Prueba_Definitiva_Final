
var ModalEmpresasApp = angular.module('ModalEmpresasApp', ['dx', 'AppSrvEmpresa'])
var DatosEmpresa = false;

//Controlador para gestionar la consulta de empresas
ModalEmpresasApp.controller('ModalConsultaEmpresasController', function ModalConsultaEmpresasController($scope, $location, SrvEmpresa) {

    SrvEmpresa.ObtenerFacturadores().then(function (data) {          
        DatosEmpresa = data;
        $("#gridEmpresas").dxDataGrid({
            dataSource: DatosEmpresa,
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
                   }, 

               ],
               
				
               
            filterRow: {
            	visible: true,            	
            }
            
        });   
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

var AppSrvEmpresa = angular.module('AppSrvEmpresa', ['dx'])
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push(myInterceptor);
    })
.service('SrvEmpresa', function ($http, $location, $q) {

    this.ObtenerFacturadores = function () {
        return $http.get('/api/Empresas?Facturador=true').then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });
    }
});