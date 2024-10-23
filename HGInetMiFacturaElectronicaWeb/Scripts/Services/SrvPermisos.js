var AppSrvPermisos = angular.module('AppSrvPermisos', ['dx'])
.config(function ($httpProvider) {
	$httpProvider.interceptors.push(myInterceptor);
})
.service('SrvPermisos', function ($http, $location, $q) {

	this.ObtenerPermisos = function (Usuario, Facturador, opc_pagina) {

		return $http.get('/api/Permisos?codigo_usuario=' + Usuario + '&identificacion_empresa=' + Facturador + '&codigo_opcion=' + opc_pagina).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
});
