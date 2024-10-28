﻿DevExpress.localization.locale(navigator.language);

var AppSrvDocumento = angular.module('AppSrvDocumento', ['dx'])
 .config(function ($httpProvider) {
 	$httpProvider.interceptors.push(InterceptorEmpty);
 })
.service('SrvDocumento', function ($http, $location, $q) {
	//Obtiene los documentos para ser procesados
	this.ObtenerDocumentos = function () {
		return $http.get('/api/DocumentosAProcesar').then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
	this.ObtenerAttachedDocument = function (Id) {
		return $http.get('/api/ObtenerAttachedDocument?id=' + Id).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
	//Procesa los documentos
	this.ProcesarDocumentos = function (documentos, Consultar) {
		return $http({ url: '/api/Documentos/', data: { Documentos: documentos, Consultar_Documento: Consultar }, method: 'Post' }).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
	//Obtiene los documentos de cliente (Soporte)
	this.ObtenerDocumentosClientes = function (codigo_facturador, numero_documento, IdSeguridad, numero_resolucion) {
		return $http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&IdSeguridad=' + IdSeguridad + '&numero_resolucion=' + numero_resolucion).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
	//Obtiene los documentos Admin
	this.ObtenerDocumentosAdmin = function (codigo_facturador, numero_documento, codigo_adquiriente, estado_dian, estado_recibo, fecha_inicio, fecha_fin, TipoDocumento, tipo_filtro_fecha, Desde, Hasta) {
		return $http.get('/api/Documentos?codigo_facturador=' + codigo_facturador + '&numero_documento=' + numero_documento + '&codigo_adquiriente=' + codigo_adquiriente + '&estado_dian=' + estado_dian + '&estado_recibo=' + estado_recibo + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin + '&TipoDocumento=' + TipoDocumento + '&tipo_fecha=' + tipo_filtro_fecha + '&Desde=' + Desde + '&Hasta=' + Hasta).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.ObtenerDocumentosRechazado = function (Desde, Hasta, tipo) {
		return $http.get('/api/ObtenerDocumentosRechazado?FechaInicial=' + Desde + '&FechaFinal=' + Hasta + '&tipo=' + tipo).then(function (response) {
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

	//Consulta la lista de documentos que no tienen acuse y ya han pasado los dias que el facturador a dado para su acuse
	this.ObtenerDocumentosTacito = function (codigo_facturador, codigo_adquiriente, numero_documento, fecha_inicio, fecha_fin) {
		return $http.get('/api/ConsultaAcuseTacito?codigo_facturador=' + codigo_facturador + '&codigo_adquiriente=' + codigo_adquiriente + '&numero_documento=' + numero_documento + '&fecha_inicio=' + fecha_inicio + '&fecha_fin=' + fecha_fin).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}
	//Genera el acuse Tacito
	this.GenerarAcuseTacito = function (documentos) {
		return $http({ url: '/api/GenerarAcuseTacito/', data: { Documentos: documentos }, method: 'Post' }).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	//Valida el estado de una lista de pagos
	this.ValidarEstadoPagos = function (documentos) {
		return $http({ url: '/api/ListaEstadoPagos/', data: { ListaPagos: documentos }, method: 'Post' }).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}


	//Valida el estado del documento
	this.ActualizaEstatusPago = function (RutaServicio, IdSeguridad, Idregistro) {
		return $http.get(RutaServicio + '?IdSeguridadPago=' + IdSeguridad + "&StrIdSeguridadRegistro=" + Idregistro).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}


	this.ActualizaEstatusPagoInterno = function (IdSeguridad, Idregistro, ObjeRespuestaPI) {
		//var ObjeRespuestaPI = response.data;
		//////////////////////////////////////////////////////////////////////
		return $http.get('/Api/ActualizarEstado?IdSeguridad=' + IdSeguridad + "&StrIdSeguridadRegistro=" + Idregistro).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.ConsultarEmailUbl = function (IdSeguridad) {
		//var ObjeRespuestaPI = response.data;
		//////////////////////////////////////////////////////////////////////
		return $http.get('/Api/ConsultarEmailUbl?IdSeguridad=' + IdSeguridad).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}

	this.ValidarTextoXYPDF = function (guid_facturador, identificacion_facturador, tipo_documento, numero_documento, numero_resolucion, posicion_x, posicion_y) {
		return $http.get('/Api/ValidarTextoXYPDF?guid_facturador=' + guid_facturador + '&identificacion_facturador=' + identificacion_facturador + '&tipo_documento=' + tipo_documento
		+ '&numero_documento=' + numero_documento + '&numero_resolucion=' + numero_resolucion + '&posicion_x=' + posicion_x + '&posicion_y=' + posicion_y).then(function (response) {
			return response.data;
		}, function (response) {
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
			return $q.reject(response.data);
		});
	}


});
