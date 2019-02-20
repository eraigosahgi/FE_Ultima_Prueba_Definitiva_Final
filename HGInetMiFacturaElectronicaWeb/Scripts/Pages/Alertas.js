
var AlertasApp = angular.module('AlertasApp', ['dx', 'AppSrvAlertas', 'AppMaestrosEnum', 'AppSrvPermisos']);
var Datos;
var opc_pagina = "1336";
AlertasApp.controller('AlertasController', function AlertasController($scope,SrvAlertas, SrvMaestrosEnum, SrvPermisos) {

	//function Crearlapiz() {
	//	$(".dx-datagrid .dx-link").removeClass("dx-link").addClass("icon-pencil3");
	//}

	var Datos_correo = "";
	var validationGroupName = "ValidacionAlerta";
	$scope.Agregar = false;
	$scope.Editar = false;
	SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {

		codigo_facturador = data[0].IdentificacionEmpresa;
		UsuarioSession = data[0].IdSeguridad;

		///////////////////////////////////////		
		SrvPermisos.ObtenerPermisos(data[0].Usuario, codigo_facturador, opc_pagina).then(function (data) {
			try {
				$scope.Editar = data[0].Editar;
				$scope.Agregar = data[0].Agregar
				consultar();
			} catch (err) {
				DevExpress.ui.notify(err.message, 'error', 3000);
			}
		});
		///////////////////////////////////////
	});


	$("#summary").dxValidationSummary({
		validationGroup: validationGroupName
	});
	function consultar() {
		SrvAlertas.ObtenerListaAlertas().then(function (data) {
			$("#gridAlertas").dxDataGrid({
				dataSource: data,
				keyExpr: "IntIdAlerta",
				paging: {
					pageSize: 20

				},
				allowColumnResizing: true,
				filterRow: {
					visible: true
				},
				headerFilter: {
					visible: true
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [5, 10, 20],
					showInfo: true
				},
				//"export": {
				//	enabled: true,
				//	fileName: "Alertas",
				//	allowExportSelectedData: true
				//},
				editing: {
					mode: "popup",
					allowUpdating: $scope.Editar,
					allowAdding: $scope.Agregar,
					popup: {
						title: "Gestión de Alerta",
						showTitle: true,
						width: "50%",
						height: "40%",
						position: {
							my: "center",
							at: "center",
							of: window
						}
					},
					texts: {
						addRow: "Agregar nueva Alerta",
						cancelAllChanges: "Descartar cambios",
						cancelRowChanges: "Cancelar",
						confirmDeleteTitle: "",
						editRow: "Ver",
						saveAllChanges: "Guardar cambios",
						saveRowChanges: "Guardar",
						validationCancelChanges: "Cancelar cambios"
					}
				},
				onRowValidating: function (e) {

					if (validarVista(Datos)) {

						e.isValid = false;
					} else {

						e.isValid = true;
						Guardar(e);

						consultar();
					}
				}
	,
				columns: [

					{
						dataField: "StrDescripcion",
						caption: "Descripción",
						width: "200px",
						validationGroup: "ValidacionAlerta",
						validationRules: [{
							type: "required",
							message: "Debe Indicar la descripción"
						}]

					},
					{
						dataField: "IntValor",
						caption: "Valor",
						width: "70px",
						validationGroup: "ValidacionAlerta",
						validationRules: [{
							type: "required",
							message: "Debe Indicar el valor"
						}]

					},
						{
							caption: "Interno",
							dataField: "IntInterno",
							width: "90px",
							validationGroup: "ValidacionAlerta",
							validationRules: [
							{
								type: 'custom', validationCallback: function (options) {
									if (validar(options)) {
										options.rule.message = "Debe Indicar el correo interno";
										return false;
									} else {
										return true;
									}
								}
							}
							],
							validationGroup: validationGroupName
						},

						{
							caption: "Mail Interno",
							dataField: "StrInternoMails",
							validationGroup: "ValidacionAlerta",
							validationRules: [
							{
								type: 'custom', validationCallback: function (options) {
									if (validar(options)) {
										options.rule.message = "Debe Indicar el correo interno";
										return false;
									} else {
										return true;
									}
								}
							},
								{
									type: "email",
									message: "El campo Email no tiene el formato correcto"
								}
							],
							validationGroup: validationGroupName
						},
				{
					caption: "Cliente",
					dataField: "IntCliente",
					width: "90px",
					validationGroup: "ValidacionAlerta",
				}
				,
				{
					dataField: "IntTipo",
					caption: "Tipo",
					validationGroup: "ValidacionAlerta",
					lookup: {
						dataSource: TipoAlerta,
						displayExpr: "Name",
						valueExpr: "ID"
					},
					validationRules: [{
						type: "required",
						message: "Debe Indicar el tipo de Alerta"
					}]

				},

				{
					dataField: "IntIdEstado",
					caption: "Estado",
					width: "90px",
					validationGroup: "ValidacionAlerta",
					lookup: {
						dataSource: EstadoAlerta,
						displayExpr: "Name",
						valueExpr: "ID"
					},
					validationRules: [{
						type: "required",
						message: "Debe Indicar el tipo de Estado"
					}]

				}

				]
			});
		});
	}

	function Guardar(e) {

		var Alerta = 0;
		var Cliente = false;

		if (e.newData.IntCliente == undefined) {
			if (e.oldData == undefined) {
				Cliente = false;
			} else {
				Cliente = e.oldData.IntCliente;
			}
		} else {
			Cliente = e.newData.IntCliente;
		}

		var Interno = false;
		if (e.newData.IntInterno == undefined) {
			if (e.oldData == undefined) {
				Interno = false;
			} else {
				Interno = e.oldData.IntInterno;
			}
		} else {
			Interno = e.newData.IntInterno;
		}

		var IntIdEstado = (e.newData.IntIdEstado == undefined) ? e.oldData.IntIdEstado : e.newData.IntIdEstado;
		var Tipo = (e.newData.IntTipo == undefined) ? e.oldData.IntTipo : e.newData.IntTipo;
		var Valor = (e.newData.IntValor == undefined) ? e.oldData.IntValor : e.newData.IntValor;
		var Descripcion = (e.newData.StrDescripcion == undefined) ? e.oldData.StrDescripcion : e.newData.StrDescripcion;


		var InternoMails = "";
		if (e.newData.StrInternoMails == undefined) {
			if (e.oldData == undefined) {
				InternoMails = "";
			} else {
				InternoMails = e.oldData.StrInternoMails;
			}
		} else {
			InternoMails = e.newData.StrInternoMails;
		}


		try {
			Alerta = (e.newData.IntIdAlerta == undefined) ? e.oldData.IntIdAlerta : e.newData.IntIdAlerta;
		} catch (e) { }


		var data = {
			IntCliente: Cliente,
			IntIdAlerta: Alerta,
			IntInterno: Interno,
			IntTipo: Tipo,
			IntValor: Valor,
			StrDescripcion: Descripcion,
			StrInternoMails: InternoMails,
			IntIdEstado: IntIdEstado
		};

		SrvAlertas.Guardar(data).then(function (data) {
			console.log(data);
		});
	}

	function validar(options) {
		Datos = options;
	}

	///Valida si existe algun error
	function validarVista(options) {
		//Si existe algun error debe retornar true.

		var IntIdEstado = 0;

		try {
			var descripcion = (options.validator._validationGroup.data.StrDescripcion == undefined) ? options.data.StrDescripcion : options.validator._validationGroup.data.StrDescripcion;
		} catch (e) { }

		if (descripcion == undefined || descripcion == '') {
			DevExpress.ui.notify("Debe indicar la descripción", 'error', 3000);
			return true;
		}

		try {
			var valor = (options.validator._validationGroup.data.IntValor == undefined) ? options.data.IntValor : options.validator._validationGroup.data.IntValor;
		} catch (e) { }

		console.log(valor);
		if ((valor == undefined || valor == '') && valor != 0) {
			DevExpress.ui.notify("Debe indicar el valor.", 'error', 3000);
			return true;
		}

		try {
			var cliente = (options.validator._validationGroup.data.IntCliente == undefined) ? options.data.IntCliente : options.validator._validationGroup.data.IntCliente;

		} catch (e) { }

		try {
			var emailInterno = (options.validator._validationGroup.data.StrInternoMails == undefined) ? options.data.StrInternoMails : options.validator._validationGroup.data.StrInternoMails;
		} catch (e) { }

		try {
			var cliente = (options.validator._validationGroup.data.IntCliente == undefined) ? options.data.IntCliente : options.validator._validationGroup.data.IntCliente;

		} catch (e) { }

		try {
			var interno = (options.validator._validationGroup.data.IntInterno == undefined) ? options.data.IntInterno : options.validator._validationGroup.data.IntInterno;
		} catch (e) { }

		if ((interno == false && cliente == false) || (interno == undefined && cliente == undefined)) {
			DevExpress.ui.notify("Debe indicar si notifica al cliente o interno", 'error', 3000);
			return true;
		}

		if ((options.data.IntInterno == true && (emailInterno == '' || emailInterno == undefined))) {
			DevExpress.ui.notify("Debe indicar el correo interno", 'error', 3000);
			return true;
		}

		if (options.data.IntInterno == false && (emailInterno != '' && emailInterno != undefined)) {
			DevExpress.ui.notify("Indico el correo interno y no marco la opción", 'error', 3000);
			return true;
		}

		try {
			var tipo = (options.validator._validationGroup.data.IntTipo == undefined) ? options.data.IntTipo : options.validator._validationGroup.data.IntTipo;
		} catch (e) { }


		if (tipo == undefined || tipo == '') {
			DevExpress.ui.notify("Debe indicar el tipo de alerta", 'error', 3000);
			return true;
		}

		try {
			IntIdEstado = (options.validator._validationGroup.data.IntIdEstado == undefined) ? options.data.IntIdEstado : options.validator._validationGroup.data.IntIdEstado;
		} catch (e) { }


		if (IntIdEstado == undefined || IntIdEstado == '' && (IntIdEstado != 0)) {
			DevExpress.ui.notify("Debe indicar el estado", 'error', 3000);
			return true;
		}


		return false;
	}

});


