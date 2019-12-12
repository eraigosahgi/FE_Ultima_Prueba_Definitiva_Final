DevExpress.localization.locale(navigator.language);

var AppSrvResoluciones = angular.module('AppSrvResoluciones', ['dx'])

.service('SrvResoluciones', function ($http, $location, $q) {
	
	this.Obtener = function (codigo_facturador) {

		return $http.get('/api/ObtenerResoluciones?codigo_facturador=' + codigo_facturador).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

});