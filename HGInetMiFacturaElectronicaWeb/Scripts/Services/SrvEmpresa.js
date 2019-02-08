var AppSrvEmpresas = angular.module('AppSrvEmpresas', ['dx'])
.config(function ($httpProvider) {
	$httpProvider.interceptors.push(myInterceptor);
})
.service('SrvEmpresas', function ($http, $location, $q) {


	this.ObtenerEmpresa = function (id_seguridad) {
		return $http.get('/api/Empresas?IdSeguridad=' + id_seguridad).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.GuardarEmpresa = function (data) {
		return $http.post('/api/Empresas?' + data).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.ObtenerHabilitacion = function (Habilitacion) {
		return $http.get('/api/empresas?tipo=' + Habilitacion).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

});
