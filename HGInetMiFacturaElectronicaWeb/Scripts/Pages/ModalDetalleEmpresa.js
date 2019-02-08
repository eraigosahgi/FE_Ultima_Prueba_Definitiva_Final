
var ModalDetalleEmpresasApp = angular.module('ModalDetalleEmpresasApp', ['dx']);

ModalDetalleEmpresasApp.controller('EmpresasModalController', function EmpresasModalController($scope, $http, $rootScope) {
	
	$rootScope.ConsultaDetalleEmpresa = function (idseguridadEmpresa) {
		
		$http.get('/api/Empresas?IdSeguridad=' + idseguridadEmpresa).then(function (response) {
			console.log(response.data[0]);
			$('#modal_detalle_empresa').modal('show');
			$scope.Nit = response.data[0].Identificacion;			
			$scope.RazonSocial = response.data[0].RazonSocial;			
			$scope.adquiriente = (response.data[0].Intadquiriente) ? "SI" : "NO";
			$scope.obligado = (response.data[0].intObligado) ? "SI" : "NO";
			$scope.Habilitacion = BuscarDescripcion(EnumHabilitacion, response.data[0].Habilitacion);
			$scope.IdSeguridad = response.data[0].IdSeguridad;
			$scope.Estado = (response.data[0].Estado==1)?"ACTIVO":"INACTIVO";
			$scope.Email = response.data[0].Email;
			$scope.StrMailEnvio = response.data[0].StrMailEnvio;
			$scope.StrMailPagos = response.data[0].StrMailPagos;
			$scope.StrMailRecepcion = response.data[0].StrMailRecepcion;
			$scope.StrMailAcuse = response.data[0].StrMailAcuse;
			$scope.PostPago = (response.data[0].Postpago == 1) ? "SI" : "NO";
			$scope.Administradora = response.data[0].Admin;	
			$scope.Perfil = (response.data[0].intObligado == true && response.data[0].Intadquiriente == true) ? "Facturador y Adquiriente" : (response.data[0].intObligado == true) ? "Facturador":"Adquiriente";
			$scope.StrObservaciones = response.data[0].StrObservaciones;			
			$scope.Integrador = (response.data[0].IntIntegrador) ? "SI" : "NO";
			$scope.Facturador = response.data[0].intObligado
			$scope.TipoIndentificacion = BuscarDescripcion(TiposIdentificacion, response.data[0].TipoIdentificacion);
			$scope.Anexo = (response.data[0].IntAnexo) ? "SI" : "NO";
			$scope.StrEmpresaAsociada = response.data[0].StrEmpresaAsociada;
			$scope.StrEmpresaDescuenta = response.data[0].StrEmpresaDescuenta;
			$scope.IntNumUsuarios = response.data[0].IntNumUsuarios;
			$scope.IntAcuseTacito = response.data[0].IntAcuseTacito;
			$scope.IntEmailRecepcion = (response.data[0].IntEmailRecepcion)?"SI":"NO";
			
			
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
		});
	}
	
});



var TiposIdentificacion =
    [
        { ID: "11", Texto: 'Registro civil' },
        { ID: "12", Texto: 'Tarjeta de identidad' },
        { ID: "13", Texto: 'Cédula de ciudadanía' },
        { ID: "21", Texto: 'Tarjeta de extranjería' },
        { ID: "22", Texto: 'Cédula de extranjería' },
        { ID: "31", Texto: 'NIT' },
        { ID: "41", Texto: 'Pasaporte' },
        { ID: "42", Texto: 'Documento de identificación extranjero' }
    ];


var TiposEstado =
[
{ ID: "1", Texto: 'ACTIVO' },
{
	ID: "2", Texto: 'INACTIVO'
}
];