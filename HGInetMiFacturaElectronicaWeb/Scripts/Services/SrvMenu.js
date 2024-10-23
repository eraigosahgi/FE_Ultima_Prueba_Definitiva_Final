
var AppSrvMenu = angular.module('AppSrvMenu', ['dx'])
 .config(function ($httpProvider) {
 	$httpProvider.interceptors.push(myInterceptor);
 })

.service('SrvMenu', function ($location, $q) {
	this.ObtenerMenu = function () {


		//Impresión de campos
		return $.when().then(function () {
			return "<ul class='navigation navigation-main navigation-accordion' id='MenuPrincipal' runat='server'>                             <li class='navigation-header'><span style = 'margin: -10px'> Menú </span> <i class='icon-menu' title='Menú'></i></li><li runat = 'server' class='active'  id='menu_13'> <a href = '#' class='has-ul legitRipple'><i class='icon-typewriter'></i><span style = 'margin: -10px'> Factura Electrónica </span></a>  <ul runat='server' id='ul_13'><li runat = 'server'   id='menu_131'> <a href = '#' class='has-ul legitRipple'><i class='icon-user-tie'></i><span style = 'margin: -10px'> Facturador </span></a>  <ul runat='server' id='ul_131'><li runat = 'server'  id='menu_1311'><a id='link_1311' class='legitRipple' href='DocumentosObligado.aspx'><i class='icon-folder-search'></i><span style = 'margin: -10px'>Documentos Facturador</span></a></li><li runat = 'server'  id='menu_1312'><a id='link_1312' class='legitRipple' href='ConsultaAcuseRecibo.aspx'><i class='icon-history'></i><span style = 'margin: -10px'>Consulta Acuse Recibo</span></a></li><li runat = 'server'  id='menu_1313'><a id='link_1313' class='legitRipple' href='ConsultaFacturadorPagos.aspx'><i class='icon-folder-search'></i><span style = 'margin: -10px'>Pagos Recibidos</span></a></li><li runat = 'server'  id='menu_1314'><a id='link_1314' class='legitRipple' href='ConsultarPlanesDocumentos.aspx'><i class='icon-folder-search'></i><span style = 'margin: -10px'>Estado de Planes</span></a></li></ul> </li><li runat = 'server'   id='menu_132'> <a href = '#' class='has-ul legitRipple'><i class='icon-reading'></i><span style = 'margin: -10px'> Adquiriente </span></a>  <ul runat='server' id='ul_132'><li runat = 'server'  id='menu_1321'><a id='link_1321' class='legitRipple' href='DocumentosAdquiriente.aspx'><i class='icon-folder-search'></i><span style = 'margin: -10px'>Documentos Adquiriente</span></a></li><li runat = 'server'  id='menu_1322'><a id='link_1322' class='legitRipple' href='ConsultaAdquirientePagos.aspx'><i class='icon-folder-search'></i><span style = 'margin: -10px'>Pagos Realizados</span></a></li></ul> </li><li runat = 'server'   id='menu_133'> <a href = '#' class='has-ul legitRipple'><i class='icon-cogs'></i><span style = 'margin: -10px'> Configuración </span></a>  <ul runat='server' id='ul_133'><li runat = 'server'  id='menu_1331'><a id='link_1331' class='legitRipple' href='ConsultaUsuarios.aspx'><i class='icon-user'></i><span style = 'margin: -10px'>Usuarios</span></a></li><li runat = 'server'  id='menu_1332'><a id='link_1332' class='legitRipple' href='ConsultaEmpresas.aspx'><i class='icon-home2'></i><span style = 'margin: -10px'>Empresas</span></a></li><li runat = 'server'  id='menu_1333'><a id='link_1333' class='legitRipple' href='ConsultaEmpresasSerial.aspx'><i class='icon-key'></i><span style = 'margin: -10px'>Activación Serial</span></a></li><li runat = 'server'  id='menu_1334'><a id='link_1334' class='legitRipple' href='ConsultaPlanesTransacciones.aspx'><i class='icon-drawer-in'></i><span style = 'margin: -10px'>Activación de Planes</span></a></li><li runat = 'server'  id='menu_1335'><a id='link_1335' class='legitRipple' href='GestionReportes.aspx'><i class='icon-design'></i><span style = 'margin: -10px'>Gestión Formatos</span></a></li><li runat = 'server'  id='menu_1336'><a id='link_1336' class='legitRipple' href='GestionAlertas.aspx'><i class='icon-info3'></i><span style = 'margin: -10px'>Gestión de Alertas</span></a></li></ul> </li><li runat = 'server'   id='menu_134'> <a href = '#' class='has-ul legitRipple'><i class='icon-tab'></i><span style = 'margin: -10px'> Trazabilidad </span></a>  <ul runat='server' id='ul_134'><li runat = 'server'  id='menu_1341'><a id='link_1341' class='legitRipple' href='ProcesarDocumentos.aspx'><i class='icon-sort-time-asc'></i><span style = 'margin: -10px'>Procesar Documentos</span></a></li><li runat = 'server'  id='menu_1342'><a id='link_1342' class='legitRipple' href='ConsultaDocumentosClientes.aspx'><i class='icon-eye'></i><span style = 'margin: -10px'>Consulta Documento Clientes</span></a></li><li runat = 'server'  id='menu_1343'><a id='link_1343' class='legitRipple' href='DocumentosAdmin.aspx'><i class='icon-folder-search'></i><span style = 'margin: -10px'>Consulta Documentos Administración</span></a></li><li runat = 'server'  id='menu_1344'><a id='link_1344' class='legitRipple' href='ConsultaAcuseTacito.aspx'><i class='icon-libreoffice'></i><span style = 'margin: -10px'>Consulta Acuse Tácito</span></a></li><li runat = 'server'  id='menu_1345'><a id='link_1345' class='legitRipple' href='ConsultarPlanesAdmin.aspx'><i class='icon-folder-search'></i><span style = 'margin: -10px'>Estado de Planes Administración</span></a></li><li runat = 'server'  id='menu_1346'><a id='link_1346' class='legitRipple' href='ConsultaNotificaciones.aspx'><i class='icon-info3'></i><span style = 'margin: -10px'>Alertas y Notificaciones</span></a></li><li runat = 'server'  id='menu_1347'><a id='link_1347' class='legitRipple' href='ConsultaAuditoriaAdmin.aspx'><i class='icon-reading'></i><span style = 'margin: -10px'>Consulta Auditoria Administrador</span></a></li></ul> </li><li runat = 'server'   id='menu_136'> <a href = '#' class='has-ul legitRipple'><i class='icon-split'></i><span style = 'margin: -10px'> Interoperabilidad </span></a>  <ul runat='server' id='ul_136'><li runat = 'server'  id='menu_1361'><a id='link_1361' class='legitRipple' href='ProcesarInteroperabilidad.aspx'><i class='icon-sort-time-asc'></i><span style = 'margin: -10px'>Procesar Documentos</span></a></li><li runat = 'server'  id='menu_1362'><a id='link_1362' class='legitRipple' href='AcusePendienteRecepcion.aspx'><i class='icon-folder-search'></i><span style = 'margin: -10px'>Procesar Acuses</span></a></li><li runat = 'server'  id='menu_1363'><a id='link_1363' class='legitRipple' href='ConsultaProveedores.aspx'><i class='icon-user-tie'></i><span style = 'margin: -10px'>Proveedores</span></a></li></ul> </li> </ul>";
		});
	}
	
});


AppSrvMenu.directive('hgiMenu', function ($compile) {
	return {
		compile: function compile(tElement, tAttrs, transclude) {
			return {
				post: function preLink(scope, elem, iAttrs, controller) {
					var scopePropName = iAttrs['hgiMenu'];
					var linkingFunc = $compile(scope[scopePropName]);
					linkingFunc(scope, function (newElem) {
						elem.replaceWith(newElem);
					});
				}
			};
		}
	}
});

