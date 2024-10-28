DevExpress.localization.locale(navigator.language);

var AppSrvAuditoria = angular.module('AppSrvAuditoria', ['dx'])
 .config(function ($httpProvider) {
 	$httpProvider.interceptors.push(myInterceptor);
 })
 .service('SrvAuditoria', function ($http, $location, $q) {
 	//Obtiene los documentos Admin
 	this.ConsultaAuditoria = function (fecha_inicio, fecha_fin, cod_facturador, numero_documento, estado_dian, proceso_doc, tipo_registro, procedencia,Desde,Hasta) {
 		return $http.get('/Api/ConsultaAuditoria?fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&identificacion_obligado=' + cod_facturador + '&estado=' + estado_dian + '&proceso=' + proceso_doc + '&tipo_registro=' + tipo_registro + '&procedencia=' + procedencia + '&numero_documento=' + numero_documento + '&Desde='+ Desde + '&Hasta=' + Hasta).then(function (response) {
 			return response.data;
 		}, function (response) {
 			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
 			return $q.reject(response.data);
 		});
 	}
 });