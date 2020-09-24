var AppSrvFormatos = angular.module('AppSrvFormatos', ['dx'])
.config(function ($httpProvider) {
	$httpProvider.interceptors.push(InterceptorEmpty);
})
.service('SrvFormatos', function ($http, $location, $q) {

	this.ImportarFormato = function (datos) {
		return $http({ url: '/api/ImportarFormato/', data: datos, method: 'Put' }).then(function (response) {
			return response.data;
		}, function (response) {
			ErrorGeneral(response.data);
			return $q.reject(response.data);
		});
	}


});