var AppSrvPlanes = angular.module('AppSrvPlanes', ['dx'])
 .config(function ($httpProvider) {
 	$httpProvider.interceptors.push(myInterceptor);
 })
.service('SrvPlanes', function ($http, $location, $q) {
	//Obtiene el detalle de un plan
	this.ObtenerPlan = function (IdSeguridad) {
		return $http.get('/api/PlanesTransacciones?IdSeguridad=' + IdSeguridad).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
});