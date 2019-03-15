
var myInterceptor = function ($q) {
    return {
        //Inicia Proceso
        request: function (config) {
            $('#wait').show();
            return config;
        },
        //Termina correctamente
        response: function (result) {
            $('#wait').hide();
            return result;
        },
        //Error en el proceso
        responseError: function (rejection) {            
            $('#wait').hide();
            if (rejection.data.ExceptionMessage == "No se encontraron los datos de autenticación en la sesión; ingrese nuevamente.") {
                sesionexpiro();
            }
            if (rejection.data.ExceptionMessage == "Se ha iniciado sesión desde otra ubicación.") {
            	OtraUbicacion();
            }
            return $q.reject(rejection);
        }
    }
}
