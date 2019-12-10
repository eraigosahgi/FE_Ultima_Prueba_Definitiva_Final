
document.addEventListener('keypress', function (evt) {

	// Si el evento NO es una tecla Enter
	if (evt.key !== 'Enter') {
		return;
	}

	let element = evt.target;

	// Si el evento NO fue lanzado por un elemento con class "focusNext"
	if (!element.classList.contains('focusNext')) {
		return;
	}

	// AQUI logica para encontrar el siguiente
	let tabIndex = element.tabIndex + 1;
	var next = document.querySelector('[tabindex="' + tabIndex + '"]');

	// Si encontramos un elemento
	if (next) {
		next.focus();
		event.preventDefault();
	}
});


var AutenticacionApp = angular.module('AutenticacionApp', ['dx']);
AutenticacionApp.controller('AutenticacionController', function AutenticacionController($scope, $http, $location) {

	var dato_identificacion = "",
    dato_usuario = "",
    dato_contrasena = "";

	var nit = location.search.split('nit=')[1];


	var restabler = location.search.split('restablecer')[1];
	if (restabler) {
		$('#modal_restablecer_clave').removeAttr('style');
		$('#modal_restablecer_clave').attr("style", "display:block; padding-right: 17px;");
		$('#modal_restablecer_clave').addClass("modal fade in");
		$('#panelfondo').addClass("modal-backdrop fade in");
	}

	$scope.formOptions = {

		readOnly: false,
		showColonAfterLabel: true,
		showValidationSummary: true,
		validationGroup: "DatosAutenticacion",
		onInitialized: function (e) {
			formInstance = e.component;

		},
		onContentReady: function (e) {
			if (nit != null && nit != undefined) {
				$('input:text[name=Identificacion]').val(nit);
				dato_identificacion = nit;
			}
		}
        ,
		//Fomulario Autenticación
		items: [{
			itemType: "group",
			items: [
                {
                	dataField: "Identificacion",
                	editorType: "dxTextBox",
                	visible: (nit != null && nit != undefined) ? false : true,
                	label: {
                		text: "Identificación"
                	},
                	validationRules: [{
                		type: (nit != null && nit != undefined) ? "null" : "required",
                		message: "El documento de identificación es requerido."
                	}, {
                		//Valida que el campo solo contenga números
                		type: "pattern",
                		pattern: "^[0-9]+$",
                		message: "El campo no debe contener letras ni caracteres especiales."
                	}]
                },
                {
                	dataField: "Usuario",
                	editorType: "dxTextBox",
                	label: {
                		text: "Usuario"
                	},
                	validationRules: [{
                		type: "required",
                		message: "El usuario es requerido."
                	}]
                },
            {
            	dataField: "Clave",
            	editorType: "dxTextBox",
            	label: {
            		text: "Contraseña"
            	},
            	editorOptions: {
            		mode: "password"
            	},
            	validationRules: [{
            		type: "required",
            		message: "La contraseña es requerida."
            	}]
            }
			]
		}

		]

	};



	//Opciones de botón -  valida el formulario
	$scope.buttonOptions = {
		text: "INGRESAR",
		type: "default",
		useSubmitBehavior: true,
		validationGroup: "DatosAutenticacion"
	};

	//Evento del botón.
	$scope.onFormSubmit = function (e) {

		if (nit == null || nit == undefined) {
			dato_identificacion = $('input:text[name=Identificacion]').val();
		}

		dato_usuario = $('input:text[name=Usuario]').val();
		dato_contrasena = $('input:password[name=Clave]').val();
		try {
			var Id = dato_usuario.toUpperCase() + '_' + dato_identificacion;

			sessionStorage.setItem("Usuario", Id);			

			ga('create', GoogleAnalytics, {
				'clientId': Id
			});

			ga('send', 'pageview');
		} catch (e) { }


		//Obtiene los datos del web api
		//ControladorApi: /Api/Usuario/
		//Datos GET: codigo_empresa - codigo_usuario - clave
		$('#wait').show();
		$http.get('/Api/Usuario?codigo_empresa=' + dato_identificacion + '&codigo_usuario=' + dato_usuario + '&clave=' + dato_contrasena).then(function (response) {
			$('#wait').hide();
			var respuesta = response.data;

			if (respuesta.length != 0) {
				try {
					//Google Analytics
					ga('send', 'event', 'Autenticar', 'Exitoso', (sessionStorage.getItem("Usuario")) ? sessionStorage.getItem("Usuario") : 'Usuario Sin Sesión');
				} catch (e) { }
				window.location.assign("../Pages/Inicio.aspx?ID=" + response.data[0].Token);
			}
			else {
				try {
					//Google Analytics
					ga('send', 'event', 'Autenticar', 'Fallido', (sessionStorage.getItem("Usuario")) ? sessionStorage.getItem("Usuario") : 'Usuario Sin Sesión');
				} catch (e) { }
				DevExpress.ui.notify("Datos de autenticación inválidos.", 'error', 3000);
			}

		}, function errorCallback(response) {
			$('#wait').hide();
			try {
				//Google Analytics
				ga('send', 'event', 'Autenticar', 'Fallido', (sessionStorage.getItem("Usuario")) ? sessionStorage.getItem("Usuario") : 'Usuario Sin Sesión');
			} catch (e) { }
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});

	};

});

//Controlador para Restablecer Contraseña
//ControladorApi: /Api/Usuario/
//Datos PUT: codigo_empresa - codigo_usuario 
AutenticacionApp.controller('RestablecerController', function RestController($scope, $http, $location) {

	$scope.formOptions2 = {

		readOnly: false,
		showColonAfterLabel: true,
		showValidationSummary: true,
		validationGroup: "DatosRestablecer",
		onInitialized: function (e) {
			formInstance = e.component;
		},
		items: [{
			itemType: "group",
			items: [
                {
                	dataField: "TextBoxDocIdentificacion",
                	editorType: "dxTextBox",
                	label: {
                		text: "Documento Identificación"
                	},
                	validationRules: [{
                		type: "required",
                		message: "El documento de identificación es requerido."
                	}, {
                		//Valida que el campo solo contenga números
                		type: "pattern",
                		pattern: "^[0-9]+$",
                		message: "El campo no debe contener letras ni caracteres especiales."
                	}]
                },
                {
                	dataField: "TextBoxUsuario",
                	editorType: "dxTextBox",
                	label: {
                		text: "Código de Usuario"
                	},
                	validationRules: [{
                		type: "required",
                		message: "El Código de Usuario es requerido."
                	}]
                }
			]
		}

		]

	};


	//Opciones de botón -  valida el formulario
	$scope.buttonCerrarRestablecer = {
		text: "CERRAR",
		onClick: function (e) {
			$scope.cerrarmodal();
		}
	};

	$scope.cerrarmodal = function () {
		$('#modal_restablecer_clave').removeAttr('style');
		$('#panelfondo').removeClass("modal-backdrop fade in");
	}

	//Opciones de botón -  valida el formulario
	$scope.buttonRestablecer = {
		text: "ENVIAR",
		type: "success",
		useSubmitBehavior: true,
		validationGroup: "DatosRestablecer"
	};

	//Evento del botón.
	$scope.onFormSubmit = function (e) {

		dato_identificacion = $('input:text[name=TextBoxDocIdentificacion]').val();
		dato_usuario = $('input:text[name=TextBoxUsuario]').val();

		//aqui doy valor a los parametros que van al webapi
		var data = $.param({
			codigo_empresa: dato_identificacion,
			codigo_usuario: dato_usuario
		});
		//Obtiene los datos del web api
		//ControladorApi: /Api/Usuario/
		//Datos PUT: codigo_empresa - codigo_usuario
		$('#wait').show();
		$http.post('/Api/Usuario?' + data).then(function (response) {
			$('#wait').hide();
			//var respuesta = response.data;

			try {
				swal({
					title: 'Solicitud Éxitosa',
					text: 'Se ha enviado un e-mail a la siguiente dirección: "' + response.data + '" para el restablecimiento de contraseña',
					type: 'success',
					confirmButtonColor: '#66BB6A',
					confirmButtonText: 'Aceptar',
					animation: 'pop',
					html: true,
				});
			} catch (e) {

			}

			$('input:text[name=TextBoxDocIdentificacion]').val("");
			$('input:text[name=TextBoxUsuario]').val("");
			$("#modal_restablecer_clave").removeClass("modal fade in").addClass("modal fade");
			$('.modal-backdrop').remove();

		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 8000);
		});
		e.preventDefault();
	};
});

$(function () {
	$("#galleryContainer").dxGallery({
		dataSource: [
            "/Scripts/Images/Autenticacion3.jpg",
            "/Scripts/Images/Autenticacion2.jpg"
		],
		swipeEnabled: true,
		showNavButtons: true
        , loop: true
        , slideshowDelay: 3000
	});
});