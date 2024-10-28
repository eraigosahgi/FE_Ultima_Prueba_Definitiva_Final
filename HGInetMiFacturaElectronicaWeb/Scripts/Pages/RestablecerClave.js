
//Envia la nueva Contraseña y valida si es valido el id_seguridad
//ControladorApi: /Api/usuarios/
//Datos PUT: id_seguridad, y Contraseña
var nivel_segu = 0;

var DemoApp = angular.module('RestablecerClaveApp', ['dx']);
DemoApp.controller('RestablecerClaveController', function DemoController($scope, $http) {

	var id_seguridad = location.search.split('id_seguridad=')[1];

	//Valido si el link viene con el id de seguridad para no mostrar el panel de los datos
	if (id_seguridad != undefined) {
		$http.get('/api/Usuario?id_seguridad=' + id_seguridad).then(function (response) {

			$scope.RespuestaIdSeguridad = response.data;

		}, function errorCallback(response) {
			Mensaje(response.data.ExceptionMessage, "error", 13000);
			$("#divcontenido").hide();
			swal({
				title: "Link Expiró",
				text: "El acceso ha vencido, por lo cual debe realizar nuevamente el proceso de restablecimiento de contraseña.",

				icon: "warning",
				buttons: true,
				buttons: ["Cancelar", true]
			})
		  .then((willDelete) => {
		  	if (willDelete) {
		  		window.location.assign("../Login/Default.aspx?restablecer=true");
		  	} else {
		  		window.location.assign("../Login/Default.aspx");
		  	}
		  });
		});
	} else {
		$("#divcontenido").hide();
	}

	var clave = "";
	var formInstance;

	var formData = {
		"Password": ""
	};

	$scope.formOptions = {
		formData: formData,
		readOnly: false,
		showColonAfterLabel: true,
		showValidationSummary: true,
		validationGroup: "DatosContraseña",
		onInitialized: function (e) {
			formInstance = e.component;
		},
		items: [{
			itemType: "group",
			items: [{
				label: {
					text: "Contraseña"
				},
				dataField: "Password",
				editorOptions: {
					mode: "password"
                    , onKeyUp: function (data) {
                    	//console.log("Data", clave = $('input:password[name=Password]').val());
                    	var datos = $('input:password[name=Password]').val();
                    	if (datos.includes('+')) {
                    		$('input:password[name=Password]').val("");
                    		Mensaje("El caracter '+' no esta permitido", "error");
                    	}
                    }
				},
				validationRules: [{
					type: "required",
					message: "La Contraseña es requerida "
				},
                 {
                 	type: 'custom', validationCallback: function (options) {
                 		if (nivel_segu <= 40) {
                 			options.rule.message = "La contraseña debe tener al menos 60% de Seguridad";
                 			return false;
                 		} else { return true; }
                 	}
                 }
				]
			}, {
				label: {
					text: "Confirmar Contraseña"
				},
				editorType: "dxTextBox",
				editorOptions: {
					mode: "password"
				},
				validationRules: [{
					type: "required",
					message: "Confirmacion de la Contraseña es requerida"
				}, {
					type: "compare",
					message: "La Contraseña y la Confirmaciòn no coinciden",
					comparisonTarget: function () {
						return formInstance.option("formData").Password;
					}
				}]
			}]
		}]
	};

	$scope.buttonAceptar = {
		text: "Aceptar",
		type: "success",
		useSubmitBehavior: true,
		validationGroup: "DatosContraseña"
	};

	$scope.onFormSubmit = function (e) {
		var id_seguridad = location.search.split('id_seguridad=')[1];
		//Asigno valor de la clave a la variable 
		clave = $('input:password[name=Password]').val();

		//aqui doy valor a los parametros que van al webapi
		var data = $.param({
			id_seguridad: id_seguridad,
			clave: clave
		});
		//Aqui Valido si el id de seguridad viene en el get para cancelar la operacion en caso de que no venga
		if (id_seguridad == undefined) {
			Mensaje("Link Incorrecto", "error");
		} else {
			$http.post('/api/Usuario?' + data).then(function (response) {

				$scope.RespuestaIdSeguridad = response.data;


				//Aqui debe ir el                 
				$("#divcontenido").hide();

				swal({
					title: 'Solicitud Exitosa',
					text: 'Su contraseña ha sido restablecida exitosamente.',
					type: 'success',
					confirmButtonColor: '#66BB6A',
					confirmButtonText: 'Aceptar',
					animation: 'pop',
					html: true,
				}).then((value) => {
					window.location.assign("../Login/Default.aspx");
				});

			}, function errorCallback(response) {
				Mensaje(response.data.ExceptionMessage, "error");
			});
		}

		e.preventDefault();
	};


	function Mensaje(strmensaje, tipo, tiempo) {
		if (tiempo == undefined) { tiempo = 3000 }
		DevExpress.ui.notify({
			message: strmensaje,
			position: {
				my: "center top",
				at: "center top"
			}
		}, tipo, tiempo);
	}



	///////////////////////////////////////////////////////////////////////
	$(document).ready(function () {
		var longitud = false,
		  minuscula = false,
		  numero = false,
		  mayuscula = false;

		$('input:password[name=Password]').keyup(function () {
			nivel_segu = 0;
			var pswd = $(this).val();
			if (pswd.length < 8) {
				$('#length').removeClass('valid').addClass('invalid');
				longitud = false;
			} else {
				$('#length').removeClass('invalid').addClass('valid');
				longitud = true;
				nivel_segu += 20;
			}

			//validate letter
			if (pswd.match(/[a-z]/)) {
				$('#letter').removeClass('invalid').addClass('valid');
				minuscula = true;
				nivel_segu += 20;
			} else {
				$('#letter').removeClass('valid').addClass('invalid');
				minuscula = false;
			}

			//validate capital letter
			if (pswd.match(/[A-Z]/)) {
				$('#capital').removeClass('invalid').addClass('valid');
				mayuscula = true;
				nivel_segu += 20;
			} else {
				$('#capital').removeClass('valid').addClass('invalid');
				mayuscula = false;
			}

			//Simbolo
			if (pswd.match(/[-/*-+.?¿!%$&]/)) {
				$('#simbolo').removeClass('invalid').addClass('valid');
				mayuscula = true;
				nivel_segu += 20;
			} else {
				$('#simbolo').removeClass('valid').addClass('invalid');
				mayuscula = false;
			}

			//validate number
			if (pswd.match(/\d/)) {
				$('#number').removeClass('invalid').addClass('valid');
				numero = true;
				nivel_segu += 20;
			} else {
				$('#number').removeClass('valid').addClass('invalid');
				numero = false;
			}

			nivel(nivel_segu);
		}).focus(function () {
			$('#pswd_info').show();
		}).blur(function () {
			$('#pswd_info').hide();
		});


		$("#registro").submit(function (event) {
			if (longitud && minuscula && numero && mayuscula) {
				alert("password correcto");
				$("#registro").submit();

			} else {
				alert("Password invalido.");
				event.preventDefault();
			}

		});

		function nivel(nivel) {
			var color = '#FE2E2E';
			if (nivel <= 100) {
				color = '#5cb85c';
			}
			if (nivel <= 80) {
				color = '#D0FA58';
			}

			if (nivel <= 60) {
				color = '#F7FE2E';
			}

			if (nivel <= 40) {
				color = '#F78181';
			}

			if (nivel <= 20) {
				color = '#FE2E2E';
			}


			$("#progressBarStatus").dxProgressBar({ value: nivel });

			$('div.dx-progressbar-range').css('background-color', color);
			$('div.dx-progressbar-range').css('border', '1px solid  ' + color);
		}
	});
	///////////////////////////////////////////////////////////////////////
	$(function () {
		var progressBarStatus = $("#progressBarStatus").dxProgressBar({
			min: 0,
			max: 100,
			width: "100%",
			statusFormat: function (value) {
				return value * 100 + "%";
			}
		}).dxProgressBar("instance");
	});
	///////////////////////////////////////////////////////////////////////

});


