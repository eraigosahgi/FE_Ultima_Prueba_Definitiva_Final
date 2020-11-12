DevExpress.localization.locale(navigator.language);

var AppSrvResoluciones = angular.module('AppSrvResoluciones', ['dx'])

.service('SrvResoluciones', function ($http, $location, $q) {

	this.Obtener = function (codigo_facturador, codigo_Resolucion) {

		return $http.get('/api/ObtenerResoluciones?codigo_facturador=' + codigo_facturador + "&codigo_Resolucion=" + codigo_Resolucion).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.ActualizarConDIAN = function (codigo_facturador) {

		return $http.get('/api/ActualizarConDIAN?codigo_facturador=' + codigo_facturador).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
	,
	//Obtiene la lista de resoluciones asociadas a un facturador
	this.ObtenerAsociadas = function (codigo_facturador) {

		return $http.get('/api/ObtenerResolucionesEnum?codigo_facturador=' + codigo_facturador).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}



	//Obtiene la lista de comercios disponibles para este facturador
	this.ObtenerComercios = function (codigo_facturador, serial_cloudservices) {

		return $http.get('/api/ObtenerComercios?codigo_facturador=' + codigo_facturador + '&serial_cloudservices=' + serial_cloudservices).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	//Actualiza los datos de configuración de una resolución	
	this.EditarConfigPago = function (Stridseguridad, Permitepagosparciales, IdComercio, DescripcionComercio, IdComercioTC, DescripcionComercioTC) {

		return $http.get('/api/EditarConfigPago?Stridseguridad=' + Stridseguridad + '&Permitepagosparciales=' + Permitepagosparciales + '&IdComercio=' + IdComercio + '&DescripcionComercio=' + DescripcionComercio + '&IdComercioTC=' + IdComercioTC + '&DescripcionComercioTC=' + DescripcionComercioTC).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 6000);
			return $q.reject(response.data);
		});
	}
	
});