DevExpress.localization.locale(navigator.language);

var AppMaestrosEnum = angular.module('AppMaestrosEnum', ['dx'])
    .config(function ($httpProvider) {
    $httpProvider.interceptors.push(myInterceptor);
})
.service('SrvMaestrosEnum', function ($http, $q) {
                      
        //Obtiene los datos de un enumerable especifico (Se puede pasar el codigo por parametro)
        this.ObtenerEnum = function (parametro) {           
            return $http.get('/api/MaestrosEnum?tipo_enum='+parametro).then(function (response) {             
                return response.data;
            }, function (response) {               
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
                return $q.reject(response.data);
            });
        }                       
        //Datos GET: codigo_adquiente - numero_documento - estado_recibo - fecha_inicio - fecha_fin
        this.ObtenerDocumentos = function (Parametros) {            
            return $http.get('/api/Documentos?' + Parametros).then(function (response) {              
                return response.data;
            }, function (response) {                
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
                return $q.reject(response.data);
            });
        }    
        //Obtiene los datos de la sesion activa
        this.ObtenerSesion= function () {            
            return $http.get('/api/DatosSesion/').then(function (response) {              
                return response.data;
            }, function (response) {                
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
                return $q.reject(response.data);
            });
        }       
});


