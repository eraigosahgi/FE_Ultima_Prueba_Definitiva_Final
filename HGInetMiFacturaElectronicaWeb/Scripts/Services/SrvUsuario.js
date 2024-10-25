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
	///Obtiene los usuarios con Paginación
    this.ObtenerConPag = function (codigo_facturador, Desde, Hasta,codigo_usuario,nombre_usuario) {

    	return $http.get('/api/Usuario?codigo_usuario=' + codigo_usuario + '&codigo_empresa=' + codigo_facturador + '&Desde=' + Desde + '&Hasta=' + Hasta + "&nombre_usuario=" + nombre_usuario).then(function (response) {
    		return response.data;
    	}, function (response) {
    		DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
    		return $q.reject(response.data);
    	});
    }

});


