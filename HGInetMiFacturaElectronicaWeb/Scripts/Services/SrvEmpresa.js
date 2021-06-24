var AppSrvEmpresas = angular.module('AppSrvEmpresas', ['dx'])
.config(function ($httpProvider) {
	$httpProvider.interceptors.push(InterceptorEmpty);
})
.service('SrvEmpresas', function ($http, $location, $q) {

	this.ObtenerEmpresa = function (id_seguridad) {
		return $http.get('/api/Empresas?IdSeguridad=' + id_seguridad).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.GuardarEmpresa = function (data) {
		return $http.post('/api/Empresas?' + data).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.ObtenerHabilitacion = function (Habilitacion) {
		return $http.get('/api/empresas?tipo=' + Habilitacion).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	//Retorna információn del certificado
	this.ObtenerInfCert = function (IdSeguridad, Clave, Datos_proveedores) {
		return $http.get('/api/ObtenerInfCert?IdSeguridad=' + IdSeguridad + '&Clave=' + Clave + "&Certificadora=" + Datos_proveedores).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 6000);
			return $q.reject(response.data);
		});
	}

	this.ObtenerEmpresas = function (codigo_facturador, Desde, Hasta, Tipo, Nit, razon_social) {
		return $http.get('/api/ObtenerEmpresas?IdentificacionEmpresa=' + codigo_facturador + '&Desde=' + Desde + '&Hasta=' + Hasta + '&Tipo=' + Tipo + '&Nit=' + Nit + '&razon_social=' + razon_social).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	//Actualiza los datos de configuración de comercio de la Empresa	
	this.EditarConfigPago = function (Stridseguridad, Permitepagosparciales, IdComercio, DescripcionComercio) {

		return $http.get('/api/EmpresaEditarConfigPago?Stridseguridad=' + Stridseguridad + '&Permitepagosparciales=' + Permitepagosparciales + '&IdComercio=' + IdComercio + '&DescripcionComercio=' + DescripcionComercio).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 6000);
			return $q.reject(response.data);
		});
	}

	//Obtiene la lista de comercios disponibles para este facturador
	this.ObtenerSerialCloud = function (codigo_facturador) {

		return $http.get('/api/ObtenerSerialCloud?codigo_facturador=' + codigo_facturador).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	//Actualiza los datos de campos dian	
	this.EditarCamposDian = function (StrIdseguridad, posicion_x, posicion_y) {

		return $http.get('/api/EmpresaEditarCamposDian?StrIdseguridad=' + StrIdseguridad + '&posicion_x=' + posicion_x + '&posicion_y=' + posicion_y).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 6000);
			return $q.reject(response.data);
		});
	}

	this.ObtenerFacturadores = function () {
		return $http.get('/api/Empresas?Facturador=true').then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

});
