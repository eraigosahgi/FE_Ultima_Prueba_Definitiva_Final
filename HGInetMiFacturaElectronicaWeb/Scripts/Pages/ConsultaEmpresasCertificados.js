DevExpress.localization.locale(navigator.language);
var opc_pagina = "1332";
var TiposHabilitacion = [];
var Lista_Proveedores_Firma = [];
var Regresa_Listado = false;//Indica true o false si puede regresar a la pagina con la lista de empresas
var id_seguridad
//Estas variables son para tener la copia de cada correo y guardar si tienen algun cambio
var Copia_Email_Administracion = "";
var Copia_Email_Recepcion = "";
var Copia_Email_Acuse = "";
var Copia_Email_Envio = "";
var Copia_Email_Pagos = "";


//Estas variables son para identificar el proceso de validación de cada correo
var Proc_Email = 0,
Proc_MailEnvio = 0,
Proc_MailRecepcion = 0,
Proc_MailAcuse = 0,
Proc_MailPagos = 0

var Copia_Proc_Email = 0,
Copia_Proc_MailEnvio = 0,
Copia_Proc_MailRecepcion = 0,
Copia_Proc_MailAcuse = 0,
Copia_Proc_MailPagos = 0

var codigo_facturador = "",
numero_documento = "",
estado_dian = "",
estado_recibo = "",
fecha_inicio = "",
fecha_fin = "",
Habilitacion = "",
Datos_estado = "",
Datos_postpago = 0,
Datos_debug = 0,
Datos_InterOp = 0,
Datos_Radian = 0,
Datos_Email_Recepcion = "",
Datos_Email_Acuse = "",
Datos_Email_Envio = "",
Datos_Email_Pagos = "",
codigo_adquiriente = "",
Datos_VersionDIAN = "",
Datos_CertFirma = "",
Datos_Hgi_Responsable = "",
Datos_Hgi_Notifica = "",
Datos_FechaCert = "",
id_seguridad = "",
Datos_ClaveCert = "",
Datos_Serial = "",
Datos_Serial_Cloud = "",
Datos_EnvioNominaMail = 0,
Datos_proveedores = "",
Datos_NombresRep = "",
Datos_ApellidosRep = "",
Datos_IdentificacionRep = "",
Datos_TipoidentificacionRep = "",
Datos_CargoRep = "",
Datos_TipoPlan = 0;
//Desde hasta en la consulta de la grid
var Desde = 0;
var Hasta = 20;
var CantRegCargados = 0;

var ModalDetalleEmpresasApp = angular.module('ModalDetalleEmpresasApp', []);

var EmpresasApp = angular.module('EmpresasApp', ['ModalDetalleEmpresasApp', 'dx', 'AppSrvFiltro', 'AppMaestrosEnum', 'AppSrvEmpresas', 'AppSrvResoluciones']);

//Controlador para gestionar la consulta de empresas
EmpresasApp.controller('ConsultaEmpresasCertificadosController', function ConsultaEmpresasCertificadosController($scope, $http, $rootScope, $location, SrvEmpresas, SrvFiltro, SrvMaestrosEnum) {

	var Empresas = [];
	var Proveedores = [];
	SrvMaestrosEnum.ObtenerEnum(9, 'publico').then(function (data) {
		Proveedores = data;
	});

	var AlmacenEmpresas = new DevExpress.data.ArrayStore({
		key: "Identificacion",
		data: Empresas
	});

	var estado;
	var item_Responsable = "1";

	$scope.Admin = false;

	$scope.filtros =
    {
    	TipoTercero: {
    		searchEnabled: true,
    		//Carga la data del control
    		dataSource: new DevExpress.data.ArrayStore({
    			data: items_Responsable,
    			key: "ID"
    		}),
    		displayExpr: "Texto",
    		Enabled: true,
    		placeholder: "FACTURADOR",
    		value: "1",
    		onValueChanged: function (data) {
    			item_Responsable = data.value.ID;
    		}
    	}
    }

	var now = new Date();
	fecha_inicio = new Date(now).toISOString();
	fecha_fin = new Date(now).toISOString();
	$("#FechaInicial").dxDateBox({
		value: now,
		width: '100%',
		displayFormat: "yyyy-MM-dd",
		onValueChanged: function (data) {
			fecha_inicio = new Date(data.value).toISOString();
			$("#FechaFinal").dxDateBox({ min: fecha_inicio });
		}

	});

	$("#FechaFinal").dxDateBox({
		value: now,
		width: '100%',
		displayFormat: "yyyy-MM-dd",
		onValueChanged: function (data) {
			fecha_fin = new Date(data.value).toISOString();
			$("#FechaInicial").dxDateBox({ max: fecha_fin });
		}

	});


	SrvFiltro.ObtenerFiltro('Documento Facturador', 'Facturador', 'icon-user-tie', 115, '/api/Empresas?Facturador=true', 'Identificacion', 'RazonSocial', false, 2).then(function (Datos) {
		$scope.Facturador = Datos;
		txt_hgi_Facturador = "*";
	});



	//consultar();
	function consultar() {
		$('#wait').hide();
		txt_hgi_Facturador = (txt_hgi_Facturador == "") ? "*" : txt_hgi_Facturador;

		SrvEmpresas.ObtenerEmpresasCertificados(txt_hgi_Facturador, fecha_inicio, fecha_fin, item_Responsable).then(function (data) {
			$('#wait').hide();

			//*******Consulta de los primeros 20 registros

			AlmacenEmpresas = new DevExpress.data.ArrayStore({
				key: "Identificacion",
				data: data
			});


			$("#gridEmpresas").dxDataGrid({
				dataSource: {
					store: AlmacenEmpresas,
					reshapeOnPush: true
				},
				keyExpr: "Identificacion",
				paging: {
					pageSize: 100
				},
				pager: {
					showPageSizeSelector: true,
					allowedPageSizes: [100, 200, 300],
					showInfo: true
				}
					 , loadPanel: {
					 	enabled: true
					 },
				//, onCellPrepared: function (options) {
				//	var fieldData = options.value,
				//  	fieldHtml = "";
				//	try {
				//		if (options.data.Estado == 1) {
				//			estado = " style='color:green; cursor:default;' title='Activo'";
				//		}
				//		if (options.data.Estado == 2) {
				//			estado = " style='color:red; cursor:default;' title='Inactivo'";
				//		}
				//		if (options.data.Estado == 3) {
				//			estado = " style='color:orange; cursor:default;' title='En proceso de registro'";
				//		}

				//	} catch (err) {

				//	}

				//},
				allowColumnResizing: true
				, onToolbarPreparing: function (e) {
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
				   , columns: [


					   {
					   	caption: "Identificacion",
					   	dataField: "Identificacion"
					   },
					   {
					   	caption: "Razón social",
					   	dataField: "RazonSocial",
					   },
					   {
					   	caption: "Email",
					   	dataField: "Email"
					   },
					   {
					   	caption: "Fecha Vencimiento",
					   	dataField: "DatCertVence",
					   	dataType: "date",
					   	format: "yyyy-MM-dd",
					   },					  
					  
					    {
					    	dataField: "IntCertFirma",
					    	caption: "Responsable",					    	
					    	lookup: {
					    		dataSource: items_Responsable,
					    		displayExpr: "Texto",
					    		valueExpr: "ID"
					    	},
					    },					   
					    {
					    	dataField: "IntCertProveedor",
					    	caption: "Proveedor",
					    	lookup: {
					    		dataSource: Proveedores,
					    		displayExpr: "Descripcion",
					    		valueExpr: "ID"
					    	},
					    },
					   //{
					   //	cssClass: "col-md-1 col-xs-1",
					   //	caption: 'Estado',
					   //	dataField: 'Estado',
					   //	cellTemplate: function (container, options) {
					   //		$("<div style='text-align:center; cursor:default;'>")
					   // 		.append($("<a taget=_self class='icon-circle2'" + estado + ">"))
					   // 		.appendTo(container);
					   //	}


				   ],
				filterRow: {
					visible: true
				}
			});



		}, function errorCallback(response) {
			$('#wait').hide();
			DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
		});




	}


	//Boton Consultar
	$scope.ButtonOptionsConsultar = {
		text: 'Consultar',
		type: 'default',
		onClick: function (e) {
			consultar();
		}
	};


	//Carga las empresas al array
	function cargarEmpresas(data) {
		if (data != "") {
			data.forEach(function (d) {
				Empresas = d;
				AlmacenEmpresas.push([{ type: "insert", data: Empresas }]);
			});
		}
	}

});




//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
	for (var i = 0; i < miArray.length; i += 1) {
		if (ID == miArray[i].ID) {
			return i;
		}
	}
}





var items_Responsable =
[
	{ ID: "*", Texto: 'TODOS' },
	{ ID: "0", Texto: 'HGI' },
	{ ID: "1", Texto: 'FACTURADOR' }
];

var items_Proveedor =
[
	{ ID: "1", Texto: 'Proveedor 1' },
	{ ID: "2", Texto: 'Proveedor 2' },
	{ ID: "3", Texto: 'Proveedor 3' }
];

