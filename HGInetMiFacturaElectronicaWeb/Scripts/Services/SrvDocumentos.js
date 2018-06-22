DevExpress.localization.locale(navigator.language);

var AppSrvDocumento = angular.module('AppSrvDocumento', ['dx'])
 .config(function ($httpProvider) {
     $httpProvider.interceptors.push(myInterceptor);
 })
.service('SrvDocumento', function ($http, $location, $q) {
    
    this.ObtenerDocumentos = function (numero_documento, estado_recibo, fecha_inicio, fecha_fin) {
        return $http.get('/api/Documentos?IdSeguridad=' + numero_documento + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });
    }

    this.ProcesarDocumentos = function (documentos) {
        return $http({url: '/api/Documentos/',data: { Documentos: documentos },method: 'Post'}).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });
    }
    
});
