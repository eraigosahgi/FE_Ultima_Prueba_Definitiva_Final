
DevExpress.localization.locale(navigator.language);

var serial = "";
AutenticacionPagosApp.controller('RegistroUsuarioPagosController', function RegistroUsuarioPagosController($scope, $http, $location) {


	//Validación de Serial de Cliente de Pagos**********************************
	serial = location.search.split('serial=')[1];

	var now = new Date();
	var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "",
           codigo_usuario_sesion = "";

	var Datos_nombres = "",
        Datos_apellidos = "",
        Datos_usuario = "",
        Datos_telefono = "",
	Datos_email = "",
    Datos_estado = "",
	Datos_identificacion = "",
    Datos_Tipo = "1";

	//Define los campos del Formulario  
	$(function () {



		var ValidarGestionRegistro = "ValidarGestionRegistro";
		$("#summary").dxValidationSummary(
		{
			validationGroup: ValidarGestionRegistro
		});


		$("#txtidentificacion").dxTextBox({
			inputAttr: {
				id: "txtidentificacionid",
			},
			onValueChanged: function (data) {
				Datos_identificacion = data.value.toUpperCase();
			}
		})
        .dxValidator({
        	validationGroup: ValidarGestionRegistro,
        	validationRules: [{
        		type: "required",
        		message: "Debe Indicar la Identificación"
        	}, {
        		type: "stringLength",
        		max: 20,
        		message: "la Identificación no puede ser mayor a 20 caracteres"
        	}]
        });


		$("#txtnombres").dxTextBox({
			onValueChanged: function (data) {
				Datos_nombres = data.value.toUpperCase();
			}
		})
        .dxValidator({
        	validationGroup: ValidarGestionRegistro,
        	validationRules: [{
        		type: "required",
        		message: "Debe Indicar el Nombre"
        	}, {
        		type: "stringLength",
        		max: 255,
        		message: "El Nombre no puede ser mayor a 255 caracteres"
        	}]
        });


		$("#txtapellidos").dxTextBox({
			onValueChanged: function (data) {
				Datos_apellidos = data.value.toUpperCase();
			}
		})
        .dxValidator({
        	validationGroup: ValidarGestionRegistro,
        	validationRules: [
			{
				type: 'stringLength',
				max: 50,
				message: "El apellido no puede ser mayor a 50 caracteres"
			}]
        });

		$("#txtusuario").dxTextBox({
			onValueChanged: function (data) {
				Datos_usuario = data.value.toUpperCase();
			}
		})
        .dxValidator({
        	validationGroup: ValidarGestionRegistro,
        	validationRules: [{
        		type: "required",
        		message: "Debe introducir el Usuario"
        	}, {
        		type: "stringLength",
        		max: 20,
        		message: "El Usuario no puede ser mayor a 20 caracteres"
        	}]
        });

		$("#txttelefono").dxTextBox({
			onValueChanged: function (data) {
				Datos_telefono = data.value;
			}
		})
        .dxValidator({
        	validationGroup: ValidarGestionRegistro,
        	validationRules: [
			{
				type: "stringLength",
				max: 50,
				message: "El Telefono no puede ser mayor a 50 caracteres"
			}]
        });



		$("#txtemail").dxTextBox({
			onValueChanged: function (data) {
				Datos_email = data.value;
			},
		})
        .dxValidator({
        	validationGroup: ValidarGestionRegistro,
        	validationRules: [{
        		type: "required",
        		message: "Debe introducir el Email"
        	}, {
        		type: "stringLength",
        		max: 200,
        		message: "El email no puede ser mayor a 200 caracteres"
        	}, {
        		type: "email",
        		message: "El email no tiene el formato correcto"
        	}]
        });





		$("#button").dxButton({
			tabIndex: 10,
			icon: 'save',
			text: "Continuar",
			type: "default",
			validationGroup: ValidarGestionRegistro,
			onClick: function (e) {
				var result = e.validationGroup.validate();
				if (result.isValid) {
					RegistrarUsuario();
				}
			}
		});

	});



	//$scope.ButtonGuardar = {
	//	text: 'Registrarme',
	//	type: 'default',
	//	validationGroup: "ValidacionDatosEmpresa",
	//	onClick: function (e) {
	//		RegistrarUsuario();
	//	}
	//};

	function RegistrarUsuario() {
		//var empresa = Datos_empresa.split(' -- ');

		var data = $.param({
			IdSeguridadFacturador: serial,
			StrEmpresaAdquiriente: Datos_identificacion,
			StrUsuario: Datos_usuario,
			StrNombres: Datos_nombres,
			StrApellidos: Datos_apellidos,
			StrMail: Datos_email,
			StrTelefono: Datos_telefono
		});

		$("#wait").show();
		$http.post('/api/RegistroUsuarioPagos?' + data).then(function (response) {
			$("#wait").hide();
			try {

				DevExpress.ui.notify({ message: "Usuario Registrado con exito, su información sera validada y luego recibira un correo para continuar con el registro", position: { my: "center top", at: "center top" } }, "success", 3500);
				//$("#button").hide();
				//$("#btncancelar").hide();
				$('#CerrarModal').click();
			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 9000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 6000);
		});
	}

});


