
var AlertasApp = angular.module('AlertasApp', ['dx', 'AppSrvAlertas', 'AppMaestrosEnum']);
var Datos;
AlertasApp.controller('AlertasController', function AlertasController($scope, $location, SrvAlertas, SrvMaestrosEnum) {

	var Datos_correo = "";
	var validationGroupName = "ValidacionAlerta";
	SrvMaestrosEnum.ObtenerSesionUsuario().then(function (data) {
		codigo_facturador = data[0].IdentificacionEmpresa;
		UsuarioSession = data[0].IdSeguridad;

	});


	$("#summary").dxValidationSummary({
		validationGroup: validationGroupName
	});

	SrvAlertas.ObtenerListaAlertas().then(function (data) {

		$("#gridAlertas").dxDataGrid({
			dataSource: data,
			keyExpr: "IntIdAlerta",
			showBorders: true,
			paging: {
				pageSize: 20

			},
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
			groupPanel: {
				allowColumnDragging: true,
				visible: true
			},
			editing: {
				mode: "popup",
				allowUpdating: true,
				allowAdding: true,
				popup: {
					title: "Gestión de Alerta",
					showTitle: true,
					width: 700,
					height: 345,
					position: {
						my: "center",
						at: "center",
						of: window
					}
				},
				texts: {
					addRow: "Agregar",
					cancelAllChanges: "Descartar cambios",
					cancelRowChanges: "Cancelar",
					confirmDeleteMessage: "Esta seguro que desea eliminar el registro?",
					confirmDeleteTitle: "",
					deleteRow: "Eliminar",
					editRow: "Ver",
					saveAllChanges: "Guardar cambios",
					saveRowChanges: "Guardar",
					undeleteRow: "Undelete",
					validationCancelChanges: "Cancelar cambios"
				},
				useIcons: true
			},
			onRowValidating: function (e) {
			
				if (validarVista(Datos)) {
					console.log("NO Guarda");
					e.isValid = false;
				} else {
					console.log("Guarda");
					e.isValid = true;
					Guardar(e);
				}
			}
			//,onRowUpdating: function (e) {				
			//	Guardar(e);				
			//}
		, allowColumnResizing: true,
			columns: [
			{
				dataField: "StrDescripcion",
				caption: "Descripción",
				validationGroup: "ValidacionAlerta",
				validationRules: [{
					type: "required",
					message: "Debe Indicar la descripción"
				}]

			},
			{
				dataField: "IntValor",
				caption: "Valor",
				validationGroup: "ValidacionAlerta",
				validationRules: [{
					type: "required",
					message: "Debe Indicar el valor"
				}]

			},
				{
					caption: "Interno",
					dataField: "IntInterno",
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
					caption: "Cliente",
					dataField: "IntCliente",
					validationGroup: "ValidacionAlerta",
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
			}
		},

			]

		}



		);

	});



	function Guardar(e) {
		
		var Alerta = 0;
		var Cliente = (e.newData.IntCliente == undefined) ? e.oldData.IntCliente : e.newData.IntCliente;
		
		var Interno = (e.newData.IntInterno == undefined) ? (e.oldData.IntInterno==undefined)?false : e.oldData.IntInterno:e.newData.IntInterno;
		var Tipo = (e.newData.IntTipo == undefined) ? e.oldData.IntTipo : e.newData.IntTipo;
		var Valor = (e.newData.IntValor == undefined) ? e.oldData.IntValor : e.newData.IntValor;
		var Descripcion = (e.newData.StrDescripcion == undefined) ? e.oldData.StrDescripcion : e.newData.StrDescripcion;
		var InternoMails = (e.newData.StrInternoMails == undefined) ? e.oldData.StrInternoMails : e.newData.StrInternoMails;


		try {
			Alerta = (e.newData.IntIdAlerta == undefined) ? e.oldData.IntIdAlerta : e.newData.IntIdAlerta;
		} catch (e) {}


		var data = {
			IntCliente: Cliente,
			IntIdAlerta: Alerta,
			IntInterno: Interno,
			IntTipo: Tipo,
			IntValor: Valor,
			StrDescripcion: Descripcion,
			StrInternoMails: InternoMails
		};

		console.log(data);

		SrvAlertas.Guardar(data).then(function (data) {			
			console.log(data);
		});
	}





	function validar(options) {
		Datos = options;
		//var emailInterno = (options.data.StrInternoMails == undefined) ? options.validator._validationGroup.oldData.StrInternoMails : options.data.StrInternoMails;
		////console.log("Email Final: ", emailInterno);
		////console.log("Cambio: ", options.data);		
		////console.log("Old data: ", options.validator._validationGroup.oldData);

		//if ((options.data.IntInterno == true && emailInterno == '') || (options.data.IntInterno == false && emailInterno != '')) {
		//	return true;
		//}
		//return false;
	}


	function validarVista(options) {

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

		if (valor == undefined || valor == '') {
			DevExpress.ui.notify("Debe indicar el valor", 'error', 3000);
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

		return false;
	}


});


var TipoAlerta =
    [
        { "ID": 1, "Name": "Porcentaje" },
        { "ID": 2, "Name": "Vencimiento de Planes" },
        { "ID": 3, "Name": "Facturador sin Saldo" },
        { "ID": 4, "Name": "Pago Alto" },
        { "ID": 5, "Name": "Recarga Alta" }
    ];



