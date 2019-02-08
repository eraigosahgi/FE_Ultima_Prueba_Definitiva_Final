var AppSrvAlertas = angular.module('AppSrvAlertas', ['dx'])
.config(function ($httpProvider) {
	$httpProvider.interceptors.push(myInterceptor);
})
.service('SrvAlertas', function ($http, $location, $q) {

	this.ObtenerAlertas = function () {

		return $http.get('/api/ConsultaNotificacionAlertas').then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.ReprocesarAlerta = function (Facturador,idAlerta) {
		console.log("idAlerta",idAlerta);
		return $http.get('/api/ReprocesarAlerta?Facturador=' + Facturador + '&idAlerta=' + idAlerta).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
	
});


