DevExpress.localization.locale(navigator.language);

var AppSrvUsuario = angular.module('AppSrvUsuario', ['dx'])

.service('SrvUsuario', function ($http, $location, $q) {  

    this.Obtener_UEC = function (codigo_usuario, codigo_empresa, clave) {
        
        return $http.get('/api/Usuario?codigo_usuario='+codigo_usuario+'&codigo_empresa='+codigo_empresa+'&clave=' + clave).then(function (response) {
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
});


