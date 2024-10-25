DevExpress.localization.locale(navigator.language);

var AppSrvUsuarioPagos = angular.module('AppSrvUsuarioPagos', ['dx'])

.service('SrvUsuarioPagos', function ($http, $location, $q) {

	this.Obtener_UEC = function (codigo_usuario, codigo_empresa, clave) {

		return $http.get('/api/UsuarioPagos?codigo_usuario=' + codigo_usuario + '&codigo_empresa=' + codigo_empresa + '&clave=' + clave).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.Obtener_UE = function (codigo_usuario, codigo_empresa) {

		return $http.get('/api/Usuario?codigo_usuario=' + codigo_usuario + '&codigo_empresa=' + codigo_empresa).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
	///Obtiene los usuarios 
	this.ObtenerMisUsuarios = function (codigo_usuario, nombre_usuario) {

		return $http.get('/api/ObtenerMisUsuarios?codigo_usuario=' + codigo_usuario + "&nombre_usuario=" + nombre_usuario).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

});
