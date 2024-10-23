DevExpress.localization.locale(navigator.language);

var AppSrvLog = angular.module('AppSrvLog', ['dx'])

.service('SrvLog', function ($http, $location, $q) {

	this.Obtener = function (Fecha, Categoria, Tipo, Accion) {

		return $http.get('/api/ConsultaLog?Fecha=' + Fecha + '&Categoria=' + Categoria + '&Tipo=' + Tipo + '&Accion=' + Accion).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

});
