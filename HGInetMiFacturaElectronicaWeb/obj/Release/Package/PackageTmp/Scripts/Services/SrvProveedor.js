DevExpress.localization.locale(navigator.language);

var AppSrvProveedor = angular.module('AppSrvProveedor', ['dx'])
 .config(function ($httpProvider) {
     $httpProvider.interceptors.push(myInterceptor);
 })

.service('SrvProveedor', function ($http, $location, $q) {

    //Obtiene todos los proveedores
    this.ObtenerProveedores = function () {
        return $http.get('/api/ObtenerProveedores').then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });
    }

    //Obtiene un proveedor en especifico    
    this.ObtenerProveedoresId = function (identificacion) {
        return $http.get('/api/ObtenerProveedores?identificacion=' + identificacion).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });
    }    
   
    //Obtiene un proveedor en especifico    
    this.GuardarProveedor = function (datos) {        

        return $http.post('/api/GuardarProveedor?' + datos).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });       
    }


    //Valida la api de un proveedor
    this.Validarapi = function (u,p,api) {
        return $http.get('/api/Validarapi?u=' + u + '&p=' + p + '&api=' + api).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 6000);
            return $q.reject(response.data);
        });
    }

    //Valida la api de un proveedor
    this.Validarsftp = function (u, p, sftp) {
        return $http.get('/api/Validarsftp?u=' + u + '&p=' + p + '&sftp=' + sftp).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 6000);
            return $q.reject(response.data);
        });
    }

    

});