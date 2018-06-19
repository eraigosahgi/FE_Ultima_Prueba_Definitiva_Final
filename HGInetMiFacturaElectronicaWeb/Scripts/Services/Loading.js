
var myInterceptor = function ($q) {
    return {
        request: function (config) {
            $('#wait').show();            
            return config;
        },
        response: function (result) {
            $('#wait').hide();            
            return result;
        },
        responseError: function (rejection) {
            $('#wait').hide();            
            return $q.reject(rejection);
        }
    }
}
