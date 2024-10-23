DevExpress.localization.locale('es-ES');

var App = angular.module('App', ['dx', 'AppMaestrosEnum', 'AppSrvLog']);
App.controller('LogController', function LogController($scope, $http, $location, SrvMaestrosEnum, SrvLog) {


	var now = new Date();
	var Estado;

	var codigo_facturador = "",
           Datos_MensajeCategoria,
           Datos_MensajeTipo,
           Datos_MensajeAccion,
		   Filtro_Accion="*",
		   Filtro_Tipo = "*",
		   Filtro_Categoria = "*",
		   fecha=now;
           
	SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {
		codigo_facturador = data[0].IdentificacionEmpresa;
		UsuarioSession = data[0].IdSeguridad;
		//consultar();
	});

	SrvMaestrosEnum.ObtenerEnum(10).then(function (MensajeCategoria) {
		SrvMaestrosEnum.ObtenerEnum(11).then(function (MensajeTipo) {
			SrvMaestrosEnum.ObtenerEnum(12).then(function (MensajeAccion) {
				Datos_MensajeCategoria = MensajeCategoria;
				Datos_MensajeTipo = MensajeTipo;
				Datos_MensajeAccion = MensajeAccion;
				cargarFiltros();
			});
		});
	});


	function cargarFiltros() {
		$("#Fecha").dxDateBox({
			value: now,
			width: '100%',
			displayFormat: "yyyy-MM-dd",
			onValueChanged: function (data) {
				fecha = new Date(data.value).toISOString();
			}

		});

		var DatosEstados = function () {
			return new DevExpress.data.CustomStore({
				loadMode: "raw",
				key: "ID",
				load: function () {
					return JSON.parse(JSON.stringify(Estado));
				}
			});
		};



		//Define los campos y las opciones
		$scope.filtros =
            {

            	Categoria: {
            		searchEnabled: true,
            		//Carga la data del control
            		dataSource: new DevExpress.data.ArrayStore({
            			data: Datos_MensajeCategoria,
            			key: "ID"
            		}),
            		displayExpr: "Descripcion",
            		Enabled: true,
            		placeholder: "Seleccione un Item",
            		onValueChanged: function (data) {
            			Filtro_Categoria = data.value.ID;
            		}
            	},
            	Tipo: {
            		searchEnabled: true,
            		//Carga la data del control
            		dataSource: new DevExpress.data.ArrayStore({
            			data: Datos_MensajeTipo,
            			key: "ID"
            		}),
            		displayExpr: "Descripcion",
            		Enabled: true,
            		placeholder: "Seleccione un Item",
            		onValueChanged: function (data) {
            			Filtro_Tipo = data.value.ID;
            		}
            	},
            	Accion: {
            		searchEnabled: true,
            		//Carga la data del control
            		dataSource: new DevExpress.data.ArrayStore({
            			data: Datos_MensajeAccion,
            			key: "ID"
            		}),
            		displayExpr: "Descripcion",
            		Enabled: true,
            		placeholder: "Seleccione un Item",
            		onValueChanged: function (data) {
            			Filtro_Accion = data.value.ID;
            		}
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
		SrvLog.Obtener(fecha, Filtro_Categoria, Filtro_Tipo, Filtro_Accion).then(function (data) {
			$("#gridLog").dxDataGrid({
				dataSource: data,
				keyExpr: "Id",
				paging: {
					pageSize: 20
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				}



			});

		});
	}
});