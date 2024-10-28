var PermisosUsuario = [];
var OpcionesUsuario = [];
var ValidacionHP = false;


Permisos_Storage = [];
AlmacenPermisos = new DevExpress.data.ArrayStore({
	key: "Codigo",
	data: Permisos_Storage
});


AlmacenPermisosActualizados = new DevExpress.data.ArrayStore({
	key: "Codigo",
	data: Permisos_Storage
});
var loadPanel;


DevExpress.localization.locale(navigator.language);
var opc_pagina = "1315";
var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);
var ConsultaUsuarioPagosApp = angular.module('ConsultaUsuarioPagosApp', ['dx', 'ModalEmpresasApp', 'AppSrvUsuarioPagos', 'AppSrvFiltro']);
var OpcionesUsuario = [];
var PermisosUsuario = [];
var ValidacionHP = false;
//Desde hasta en la consulta de la grid
var Desde = 0;
var Hasta = 20;
var CantRegCargados = 0;

ConsultaUsuarioPagosApp.controller('ConsultaUsuarioPagosController', function ConsultaUsuarioPagosController($scope, $http, $location, SrvUsuarioPagos, SrvFiltro) {


	$("#codigo_usuario").dxTextBox({
		value: ""
	});

	$("#nombre_usuario").dxTextBox({
		value: ""
	});



	$("#BtnConsultar").dxButton({
		text: "Consultar",
		type: "default",
		onClick: function (e) {
			consultar();
		}
	});

	var codigo_usuario = "",
        codigo_facturador = "";

	
	function consultar() {

		//Obtiene los datos del web api
		//ControladorApi: /Api/Usuario/
		//Datos GET: string codigo_usuario, string codigo_empresa
		$('#wait').show();
		//$http.get('/api/Usuario?codigo_usuario=' + "*" + '&codigo_empresa=' + codigo_facturador).then(function (response) {
		SrvUsuarioPagos.ObtenerMisUsuarios($("#codigo_usuario").dxTextBox("instance").option().value, $("#nombre_usuario").dxTextBox("instance").option().value).then(function (data) {
			$('#wait').hide();
			$('#waitRegistros').show();
			//*******Consulta de los primeros 20 registros
			Usuarios = [];
			AlmacenUsuarios = new DevExpress.data.ArrayStore({
				key: "Identificacion",
				data: Usuarios
			});

			cargarUsuarios(data);

			$("#gridUsuarios").dxDataGrid({
				dataSource: {
					store: AlmacenUsuarios,
					reshapeOnPush: true
				},
				paging: {
					pageSize: 20
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				},
				loadPanel: {
					enabled: true
				},  //Formatos personalizados a las columnas en este caso para el monto
				onCellPrepared: function (options) {
					var fieldData = options.value,
                        fieldHtml = "";
					try {
						if (options.data.Estado == 1) {
							estado = " style='color:green; cursor:default;' title='Activo'";
						} else {
							estado = " style='color:red; cursor:default;' title='Inactivo'";
						}

					} catch (err) {

					}

				}, headerFilter: {
					visible: true
				},
				groupPanel: {
					allowColumnDragging: true,
					visible: true
				},
				onToolbarPreparing: function (e) {
					var dataGrid = e.component;

					e.toolbarOptions.items.unshift({

						location: "after",
						widget: "dxButton",
						options: {
							icon: "refresh",
							onClick: function () {
								consultar();
							}
						}
					})
				}
            , allowColumnResizing: true,
				columns: [
                     {
                     	caption: "",
                     	cellTemplate: function (container, options) {
                     		$("<div>")
								.append(
									$("<a class='icon-mail-read' style='margin-right:12%; font-size:19px'></a> &nbsp;&nbsp;").dxButton({
										onClick: function () {
											$http.get('/api/EnviarNotificacion?codigo_usuario=' + options.data.Usuario).then(function (responseEnvio) {

												var respuesta = responseEnvio.data;

												if (respuesta) {
													swal({
														title: 'Proceso Exitoso',
														text: 'El e-mail ha sido enviado con éxito.',
														type: 'success',
														confirmButtonColor: '#66BB6A',
														confirmButtonTex: 'Aceptar',
														animation: 'pop',
														html: true,
													});
												} else {
													swal({
														title: 'Error',
														text: 'Ocurrió un error en el envío del e-mail.',
														type: 'Error',
														confirmButtonColor: '#66BB6A',
														confirmButtonTex: 'Aceptar',
														animation: 'pop',
														html: true,
													});
												}

											}, function errorCallback(response) {
												DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 10000);
											});
										}
									}).removeClass("dx-button dx-button-normal dx-widget")
							)
								.append($("<a taget=_self class='icon-pencil3' style='margin-right:-20%' title='Editar' href='GestionUsuariosPagos.aspx?IdSeguridad=" + options.data.IdSeguridad + "'>"))
								.append($(""))
								.appendTo(container);
                     	}
                     },
                         {
                         	caption: "Identificación Adquiriente",
                         	dataField: "Empresa",
                         },
                         {
                         	caption: "Código Usuario",
                         	dataField: "Usuario",
                         },
                         {
                         	caption: "Nombres",
                         	dataField: "NombreCompleto",
                         },
                         {
                         	caption: "Email",
                         	dataField: "Mail"
                         }, {
                         	cssClass: "col-md-1 col-xs-1",
                         	caption: 'Estado',
                         	dataField: 'Estado',
                         	cellTemplate: function (container, options) {
                         		$("<div style='text-align:center; cursor:default;'>")
									.append($("<a taget=_self class='icon-circle2'" + estado + ">"))
									.appendTo(container);
                         	}
                         }
				], summary: {
					groupItems: [{
						column: "Empresa",
						summaryType: "count",
						displayFormat: " {0} Usuario(s) ",
						valueFormat: "fixedPoint"
					}]

				},
				filterRow: {
					visible: true
				},
			});



		}), function errorCallback(response) {
			$('#wait').hide();
			Mensaje(response.data.ExceptionMessage, "error");
		};
	}

	//Carga los Usuarios al array
	function cargarUsuarios(data) {
		if (data != "") {
			data.forEach(function (d) {
				Usuarios = d;
				AlmacenUsuarios.push([{ type: "insert", data: Usuarios }]);
			});
		}
	}

});


ConsultaUsuarioPagosApp.controller('GestionUsuarioPagosController', function GestionUsuarioPagosController($scope, $http, $location, SrvFiltro) {


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
    Datos_Tipo = "1";

	SrvFiltro.ObtenerFiltro('. Aquiriente', 'Aquiriente', 'icon-user-tie', 115, '/api/ObtenerAdquirientesPagos', 'ID', 'Texto', false, 1).then(function (Datos) {
		$scope.Facturador = Datos;
		txt_hgi_Facturador = "";
	});




	//Define los campos del Formulario  
	$(function () {
		$("#summary").dxValidationSummary({});

		$("#txtnombres").dxTextBox({
			onValueChanged: function (data) {
				Datos_nombres = data.value.toUpperCase();
			}
		})
        .dxValidator({
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






		$("#cboestado").dxSelectBox({
			placeholder: "Seleccione el Estado",
			displayExpr: "Texto",
			dataSource: TiposEstado,
			onValueChanged: function (data) {
				Datos_estado = data.value.ID;
			}
		}).dxValidator({
			validationRules: [{
				type: "required",
				message: "Debe seleccionar el Estado"
			}]
		});




		$("#button").dxButton({
			text: "Guardar",
			type: "default",
			useSubmitBehavior: true
		});


		$("#form1").on("submit", function (e) {
			guardarUsuario();
			e.preventDefault();
		});


	});

	//Consultar por el id de seguridad para obtener los datos del Usuario
	var id_seguridad = location.search.split('IdSeguridad=')[1];

	if (id_seguridad) {

		$("#wait").show();
		$http.get('/api/ObtenerUnUsuario?id_seguridad_usuario=' + id_seguridad).then(function (response) {
			$("#wait").hide();
			try {

				Datos_Tipo = "2";
				Set_Aquiriente(response.data.Empresa);
				Datos_nombres = response.data.Nombres;
				Datos_apellidos = response.data.Apellidos;
				Datos_usuario = response.data.Usuario;
				Datos_telefono = response.data.Telefono;
				Datos_email = response.data.Mail;

				Datos_estado = response.data.Estado;

				$("#txtnombres").dxTextBox({ value: Datos_nombres });
				$("#txtapellidos").dxTextBox({ value: Datos_apellidos });
				$("#txtusuario").dxTextBox({ value: Datos_usuario });
				$("#txtusuario").dxTextBox({ readOnly: true });
				$("#txttelefono").dxTextBox({ value: (Datos_telefono) ? Datos_telefono : '' });
				$("#txtemail").dxTextBox({ value: (Datos_email) ? Datos_email : '' });
				$("#cboestado").dxSelectBox({ value: TiposEstado[BuscarID(TiposEstado, Datos_estado)] });



			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 7000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
		});

	}


	$scope.ButtonGuardar = {
		text: 'Guardar',
		type: 'default',
		validationGroup: "ValidacionDatosEmpresa",
		onClick: function (e) {
			guardarUsuario();
		}
	};

	function guardarUsuario() {
		//var empresa = Datos_empresa.split(' -- ');

		var data = $.param({
			StrEmpresaAdquiriente: txt_hgi_Aquiriente,
			StrUsuario: Datos_usuario,
			StrNombres: Datos_nombres,
			StrApellidos: Datos_apellidos,
			StrMail: Datos_email,
			StrTelefono: Datos_telefono,
			IntIdEstado: Datos_estado,
			Tipo: Datos_Tipo
		});

		$("#wait").show();
		$http.post('/api/GuardarUsuario?' + data).then(function (response) {
			$("#wait").hide();
			try {				

				DevExpress.ui.notify({ message: "Usuario Guardado con exito", position: { my: "center top", at: "center top" } }, "success", 1500);
				$("#button").hide();
				$("#btncancelar").hide();
				setTimeout(IrAConsulta, 3000);

			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 6000);
		});
	}






});



function IrAConsulta() {
	window.location.assign("../Pages/ConsultaUsuariosPagos.aspx");
}

var TiposEstado =
    [
{ ID: "1", Texto: 'ACTIVO' },
    {
    	ID: "2", Texto: 'INACTIVO'
    }
    ];

//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
	for (var i = 0; i < miArray.length; i += 1) {
		if (ID == miArray[i].ID) {
			return i;
		}
	}
}