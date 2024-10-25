DevExpress.localization.locale(navigator.language);

var ModalProveedorApp = angular.module('ModalProveedorApp', ['dx', 'AppSrvProveedor'])
var DatosProveedor = false;

//Controlador para gestionar la consulta de proveedores
ModalProveedorApp.controller('ModalConsultaProveedoresController', function ModalConsultaProveedoresController($scope, $location, SrvProveedor) {

	SrvProveedor.ObtenerProveedores().then(function (data) {
		DatosProveedor = data;
		$("#gridProveedores").dxDataGrid({
			dataSource: DatosProveedor,
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
               	cssClass: "col-md-2",
               	cellTemplate: function (container, options) {
               		$("<div style='text-align:left'>")
						.append($("<a taget=_self  data-dismiss='modal' title='Seleccionar Proveedor' onclick=ObtenerProveedor('" + options.data.Identificacion + "')>" + options.data.Identificacion + "</a>"))
						.appendTo(container);
               	}
               }, {
               	caption: "Razón Social",
               	dataField: "RazonSocial",
               	cssClass: "col-md-6"
               }

               , {
                   caption: "Doc. Procesar",
                   dataField: "Dp",
                   cssClass: "col-md-2"
               }

               , {
                   caption: "Acuse Procesar",
                   dataField: "Ap",
                   cssClass: "col-md-2"
               }

               ],
			filterRow: {
				visible: true
			}
		});
	});
});

//Funcion para asignar el proveedor
function ObtenerProveedor(Proveedor) {
	fLen = DatosProveedor.length;
	for (i = 0; i < fLen; i++) {
		if (DatosProveedor[i].Identificacion == Proveedor) {
			Proveedor = Proveedor + ' --  ' + DatosProveedor[i].RazonSocial;
			break;
		}
	}
	$("#txtProveedorTecnologico").dxTextBox({ value: Proveedor });
}

var AppSrvProveedor = angular.module('AppSrvProveedor', ['dx'])
    .config(function ($httpProvider) {
    	$httpProvider.interceptors.push(myInterceptor);
    }).service('SrvProveedor', function ($http, $location, $q) {

	this.ObtenerProveedores = function () {
		return $http.get('/api/ObtenerProveedores').then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
});