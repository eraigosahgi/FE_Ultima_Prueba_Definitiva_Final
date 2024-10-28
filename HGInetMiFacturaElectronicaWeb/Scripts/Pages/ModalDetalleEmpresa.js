

var Proc_Email;
var Proc_MailEnvio;
var Proc_MailRecepcion;
var Proc_MailAcuse ;
var Proc_MailPagos ;


var ModalDetalleEmpresasApp = angular.module('ModalDetalleEmpresasApp', ['dx']);

ModalDetalleEmpresasApp.controller('EmpresasModalController', function EmpresasModalController($scope, $http, $rootScope) {
	
	$rootScope.ConsultaDetalleEmpresa = function (idseguridadEmpresa) {
	
	
		$http.get('/api/Empresas?IdSeguridad=' + idseguridadEmpresa).then(function (response) {
			
			$('#modal_detalle_empresa').modal('show');
			$scope.Nit = response.data[0].Identificacion;			
			$scope.RazonSocial = response.data[0].RazonSocial;			
			$scope.adquiriente = (response.data[0].Intadquiriente) ? "SI" : "NO";
			$scope.obligado = (response.data[0].intObligado) ? "SI" : "NO";
			$scope.Habilitacion = BuscarDescripcion(EnumHabilitacion, response.data[0].Habilitacion);
			$scope.InterOp = response.data[0].InterOp ? "SI" : "NO";
			$scope.HabilitacionNom = BuscarDescripcion(EnumHabilitacion, response.data[0].Habilitacion_NominaE);
			$scope.EnvioNominaMail = response.data[0].IntEnvioNominaMail ? "SI" : "NO";
			$scope.Radian = response.data[0].Radian ? "SI" : "NO";
			$scope.IdSeguridad = response.data[0].IdSeguridad;
			$scope.Estado = (response.data[0].Estado==1)?"ACTIVO": (response.data[0].Estado==2)? "INACTIVO":(response.data[0].Estado==3)? "EN PROCESO DE REGISTRO":"";
			$scope.Email = response.data[0].Email;
			$scope.StrMailEnvio = response.data[0].StrMailEnvio;
			$scope.StrMailPagos = response.data[0].StrMailPagos;
			$scope.StrMailRecepcion = response.data[0].StrMailRecepcion;
			$scope.StrMailAcuse = response.data[0].StrMailAcuse;
			$scope.PostPago = (response.data[0].Postpago == 1) ? "SI" : "NO";
			$scope.Administradora = response.data[0].Admin;	
			$scope.Perfil = (response.data[0].intObligado == true && response.data[0].Intadquiriente == true) ? "FACTURADOR y ADQUIRIENTE" : (response.data[0].intObligado == true) ? "FACTURADOR" : "ADQUIRIENTE";
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
			$scope.Version = (response.data[0].VersionDIAN == 2) ? 'VERSIÓN VALIDACIÓN PREVIA' : 'VERSIÓN 1';
			$scope.IntTipoPlan = (response.data[0].IntTipoPlan == 0) ? 'Aplicación' : 'Suscripción';
			$scope.Firma = (response.data[0].IntCertFirma == 0) ? 'HGI SAS' : 'FACTURADOR';
			

			$scope.FechaVenc = (response.data[0].DatCertVence != null && response.data[0].DatCertVence != '0001-01-01') ? response.data[0].DatCertVence : '';

			$scope.MuestraFechaVenc = (response.data[0].IntCertFirma == 0) ? false : true;

			Proc_Email = response.data[0].Proc_Email;
			Proc_MailEnvio = response.data[0].Proc_MailEnvio;
			Proc_MailRecepcion = response.data[0].Proc_MailRecepcion;
			Proc_MailAcuse = response.data[0].Proc_MailAcuse;
			Proc_MailPagos = response.data[0].Proc_MailPagos;
			
			

			setTimeout(ActualizarEstadoEmail, 200);
			
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
		});


		

		
	}

	
});

function ActualizarEstadoEmail() {
		
	AsigEstado("Proc_Email", Proc_Email);
	AsigEstado("Proc_MailEnvio", Proc_MailEnvio);
	AsigEstado("Proc_MailRecepcion", Proc_MailRecepcion);
	AsigEstado("Proc_MailAcuse", Proc_MailAcuse);
	AsigEstado("Proc_MailPagos", Proc_MailPagos);
}


//Colocar Icono de estado de validación de los correos
function AsigEstado(mail, Proceso) {
	var Icono_Registro = "icon-cross2 text-danger-400";
	var Icono_Verificíon = "icon-circles text-orange";
	var Icono_Activo = "icon-checkmark3 text-success";
	var Icono = "";
	var Titulo = "";

	if (Proceso == 0) {
		Titulo = "En proceso de Registro";
		Icono = Icono_Registro;
	}

	if (Proceso == 1) {
		Titulo = "En proceso de Verificación";
		Icono = Icono_Verificíon;
	}

	if (Proceso == 2) {
		Titulo = "Correo Verificado";
		Icono = Icono_Activo;
	}

	$('#Html_Modal' + mail).attr("title", Titulo);
	$('#Html_Modal' + mail).removeClass();
	$('#Html_Modal' + mail).addClass(Icono);

}




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