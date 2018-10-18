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
    this.ObtenerProveedores = function (identificacion) {
        return $http.get('/api/ObtenerProveedores?identificacion=' + identificacion).then(function (response) {
            return response.data;
        }, function (response) {
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            return $q.reject(response.data);
        });
    }
});