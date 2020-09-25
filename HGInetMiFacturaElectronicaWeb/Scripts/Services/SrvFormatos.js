var AppSrvFormatos = angular.module('AppSrvFormatos', ['dx'])
.config(function ($httpProvider) {
	$httpProvider.interceptors.push(InterceptorEmpty);
})
.service('SrvFormatos', function ($http, $location, $q) {

	this.ImportarFormato = function (datos) {
		return $http({ url: '/api/ImportarFormato/', data: datos, method: 'Put' }).then(function (response) {
			return response.data;
		}, function (response) {
			//DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}


	this.EnviarFormatoPrueba = function (codigo, nit, mail, empresa_documento, prefijo, numero_documento) {
		return $http({
			url: '/api/EnviarFormatoPrueba?id_formato=' + codigo + '&identificacion_empresa=' + nit + '&email_destino=' + mail + '&empresa_documento=' + empresa_documento
			+ '&prefijo=' + prefijo + '&numero_documento=' + numero_documento, method: 'Get'
		}).then(function (response) {
			return response.data;
		}, function (response) {
			return $q.reject(response.data);
		});
	}


});