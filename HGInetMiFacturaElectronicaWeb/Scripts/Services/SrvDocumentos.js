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

    this.ObtenerDocumentosClientes = function (codigo_facturador,  numero_documento,   IdSeguridad,  numero_resolucion) {
        return $http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&IdSeguridad=' + IdSeguridad + '&numero_resolucion=' + numero_resolucion).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });
    }

    this.ObtenerDocumentosAdmin = function (codigo_facturador, numero_documento, codigo_adquiriente, estado_dian,estado_recibo, fecha_inicio, fecha_fin, TipoDocumento) {
        return $http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&estado_dian=' + estado_dian + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&TipoDocumento=' + TipoDocumento).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });
    }
    ///Obtiene El prefijo de los tipos de documentos por resolución
    this.ObtenerResPrefijo = function (codigo_facturador) {
        return $http.get('/api/ObtenerResPrefijo?codigo_facturador=' + codigo_facturador).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });
    }
    
});
