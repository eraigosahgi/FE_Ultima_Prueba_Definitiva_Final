
function reportDesigner_CustomizeMenuActions(s, e) {

	//Elimina la opción del diseñador mediante el asistente
	var Wizard = e.Actions.filter(function (x) { return x.imageClassName === 'dxrd-image-run-wizard' })[0];
	if (Wizard) {
		Wizard.visible = false;
	}
}

//Evento para la captura de excepciones ocurridas durante la construcción del formato
function reportDesigner_OnServerError(s, e) {
	var errorMessage = e.Error.data.error;
	DevExpress.ui.notify("Ha Ocurrido un Error Durante el Proceso: " + errorMessage, 'error', 20000);
}


function CargaNotificaciones(mensaje, tipo_alerta) {
	var errorMessage = e.Error.data.error;
	DevExpress.ui.notify(mensaje, tipo_alerta, 20000);
}

