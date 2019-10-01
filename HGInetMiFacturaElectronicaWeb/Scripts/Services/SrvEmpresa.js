var AppSrvEmpresas = angular.module('AppSrvEmpresas', ['dx'])
.config(function ($httpProvider) {
	$httpProvider.interceptors.push(InterceptorEmpty);
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

	//Retorna információn del certificado
	this.ObtenerInfCert = function (IdSeguridad, Clave) {
		return $http.get('/api/ObtenerInfCert?IdSeguridad=' + IdSeguridad + '&Clave=' + Clave).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.ObtenerEmpresas = function (codigo_facturador,Desde, Hasta) {
		return $http.get('/api/ObtenerEmpresas?IdentificacionEmpresa=' + codigo_facturador + '&Desde=' + Desde + '&Hasta=' + Hasta).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

});
