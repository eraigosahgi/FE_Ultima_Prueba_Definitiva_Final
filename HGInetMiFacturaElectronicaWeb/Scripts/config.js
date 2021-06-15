
var CantidadRegEmpresa = 2000;
var CantidadRegDocumentosAdmin = 2000;
var CantidadRegAuditoriaAdmin =2000;
var CantidadRegUsuarios = 2000;



function ColocarEstado(Estado, Descripcion) {

	Descripcion = GetDescripcionEnum(CategoriaEstado, Estado);
	return "<span " + ((Estado == '400') ? " class='badge badge-FallidoDIAN'  title='" + Descripcion + "'" : (Estado == '300') ? " class='badge badge-ValidadoDIAN'  title='" + Descripcion + "'" : (Estado == '200') ? " class='badge badge-envioDian'   title='" + Descripcion + "'" : " class='badge badge-RecibidoPlataforma'  title='" + Descripcion + "'") + " style='border-radius: 0px !important;'  >" + Descripcion + "</span>"
}

function ColocarEstadoAcuse(Estado, Descripcion) {
	Descripcion = GetDescripcionEnum(AdquirienteRecibo, Estado);
	return "<span " + ((Estado == '0') ? " class='badge badge-Entregado'  title='" + Descripcion + "'" : (Estado == '1') ? " class='badge badge-Aprobado'  title='" + Descripcion + "'" : (Estado == '2') ? " class='badge badge-Rechazado'  title='" + Descripcion + "'" : (Estado == '3') ? " class='badge badge-Aprobado'  title='" + Descripcion + "'" : (Estado == '4') ? " class='badge badge-Entregado'   title='" + Descripcion + "'" : (Estado == '5') ? " class='badge badge-Leído'   title='" + Descripcion + "'" : (Estado == '6') ? " class='badge badge-Bloqueado'   title='" + Descripcion + "'" : " class='badge badge-Entregado'  title='" + Descripcion + "'") + " style='border-radius: 0px !important;'  >" + Descripcion + "</span>"
}


function ColocarEstadoEmail(Estado, Descripcion, DescEstado, IdSeguridad) {
	Descripcion = GetDescripcionEnum(EstadoEnvio, Estado);
	DescEstado = GetDescripcionEnum(EstadoEnvio, Estado);
	return "<span " + ((Estado == '0') ? " class='badge badge-Entregado'  title='" + Descripcion + "'" : (Estado == '1') ? " class='badge badge-Entregado'  title='" + Descripcion + "'" : (Estado == '2') ? " class='badge badge-Aprobado'  title='" + Descripcion + "'" : (Estado == '3') ? " class='badge badge-Rechazado'  title='" + Descripcion + "'" : (Estado == '4') ? " class='badge badge-Leído'   title='" + Descripcion + "'" : (Estado == '5') ? " class='badge badge-Bloqueado'  title='" + Descripcion + "'" : " class='badge badge-Entregado'  title='" + Descripcion + "'") + " style='border-radius: 0px !important;' target='_blank' data-toggle='modal' data-target='#modal_audit_documento' onClick=AuditoriaMail('" + IdSeguridad + "')>" + DescEstado + "</span>"
}

//Retorna el código html del diseño del tipo de evento de respuestas de  mailject
function ControlTipoEventoMail(TipoEvento) {
	return "<span " +
	  ((TipoEvento == 'Enviado') ? " class='badge bg-grey-300' title='" + TipoEvento + "'"
	: (TipoEvento == 'Entregado') ? " class='badge bg-grey-600'  title='" + TipoEvento + "'"
	: (TipoEvento == 'Abierto') ? " class='badge bg-green-300'  title='" + TipoEvento + "'"
	: (TipoEvento == 'Presionó') ? " class='badge bg-green-700'  title='" + TipoEvento + "'"
	: (TipoEvento == 'Bloqueado') ? " class='badge bg-slate-800'  title='" + TipoEvento + "'"
	: (TipoEvento == 'spam') ? " class='badge bg-danger'  title='" + TipoEvento + "'"
	: " class='badge badge bg-primary'  title='" + TipoEvento + "'")
	+ " style='border-radius: 0px !important;'>"
	+ "<i " +
	 ((TipoEvento == 'Enviado') ? " class='icon-upload4'"
	 : (TipoEvento == 'Entregado') ? " class='icon-checkmark4'"
	: (TipoEvento == 'Abierto') ? " class='icon-mail-read'"
	: (TipoEvento == 'Presionó') ? " class='icon-cursor2'"
	: (TipoEvento == 'Bloqueado') ? " class='icon-blocked'"
	: (TipoEvento == 'spam') ? " class='icon-warning'"
	: " class='icon-minus-circle2'") + "></i>&nbsp;&nbsp;" + TipoEvento + "</span>"
}

//Retorna el código html del diseño de los estados de los pagos electrónicos.
function ControlEstadoPago(EstadoPago, Descripcion) {
	if (!Descripcion)
		Descripcion = "No Definido.";
	return "<span " +
	  ((EstadoPago == '0') ? " class='badge bg-danger' title='" + Descripcion + "'"
	: (EstadoPago == '1') ? " class='badge bg-success'  title='" + Descripcion + "'"
	: (EstadoPago == '888') ? " class='badge bg-grey-300'  title='" + Descripcion + "'"
	: (EstadoPago == '999') ? " class='badge bg-grey-300'  title='" + Descripcion + "'"
	: " class='badge badge bg-grey-300'  title='" + Descripcion + "'")
	+ " style='border-radius: 0px !important;'>"
	+ "<i " +
	 ((EstadoPago == '0') ? " class='icon-cancel-circle2'"
	: (EstadoPago == '1') ? " class='icon-checkmark4'"
	: (EstadoPago == '888') ? " class='icon-minus-circle2'"
	: (EstadoPago == '999') ? " class='icon-minus-circle2'"
	: " class='icon-minus-circle2'") + "></i>&nbsp;&nbsp;" + Descripcion + "</span>"
}

//Controla el formato de numeros Ejemplo: $ 1.222.333,90
var fNumber = {
	sepMil: ",", // separador para los miles
	sepDec: '.', // separador para los decimales
	simbol: "$" || '', //Simbolo de modena
	formatear: function (num) {
		num += '';
		var splitStr = num.split('.');
		var splitLeft = splitStr[0];
		var splitRight = splitStr.length > 1 ? this.sepDec + splitStr[1] : '';
		var regx = /(\d+)(\d{3})/;
		while (regx.test(splitLeft)) {
			splitLeft = splitLeft.replace(regx, '$1' + this.sepMil + '$2');
		}
		return this.simbol + splitLeft + splitRight;
	},
	go: function (num) {

		return this.formatear(parseFloat(Math.round(num * 100) / 100).toFixed(2));
	}
}

//Controla el formato de numeros Ejemplo: 1.222.333,90
var FormatoNumber = {
	sepMil: ",", // separador para los miles
	sepDec: '.', // separador para los decimales    
	formatear: function (num) {
		num += '';
		var splitStr = num.split('.');
		var splitLeft = splitStr[0];
		var splitRight = splitStr.length > 1 ? this.sepDec + splitStr[1] : '';
		var regx = /(\d+)(\d{3})/;
		while (regx.test(splitLeft)) {
			splitLeft = splitLeft.replace(regx, '$1' + this.sepMil + '$2');
		}
		return splitLeft + splitRight;
	},
	go: function (num) {
		return this.formatear(num);
	}
}

//Controla el formato de numeros Ejemplo: 1.222.333
var FormatoNumSinDecimales = {
	sepMil: ",", // separador para los miles  
	formatear: function (num) {
		num += '';
		var splitStr = num.split('.');
		var splitLeft = splitStr[0];
		var regx = /(\d+)(\d{3})/;
		while (regx.test(splitLeft)) {
			splitLeft = splitLeft.replace(regx, '$1' + this.sepMil + '$2');
		}
		return splitLeft;
	},
	go: function (num) {
		return this.formatear(num);
	}
}


//Sirve para fitlar los datos de un Json
function FiltrarMayorJson(data, condicion) {
	var nuevoJson = [];
	$.each(data, function (i, item) {
		if (item.ID > condicion) {
			nuevoJson.push(item); return item;
		}
	});
	return nuevoJson;
}

//Formato de fecha para los grid.
function convertDateFormat(string) {
	string = string.substr(0, 10);
	var info = string.split('/');
	var part1 = info[1].length;
	var part0 = info[0].length;
	if (part1 == 1) { info[1] = '0' + info[1] }
	if (part0 == 1) { info[0] = '0' + info[0] }
	return info[2] + '/' + info[0] + '/' + info[1];
}

//gráfica un porcentaje en un div.
function PorcentajeGrafico(element, radio, bordo, colorTexto, porcentaje, claseIcono, ColorIcono, textoTitulo, observaciones) {

	// Variables
	var d3Container = d3.select(element),
        startPercent = 0,
        iconSize = 28,
        endPercent = porcentaje,
        twoPi = Math.PI * 2,
        boxSize = radio * 2;

	// Valores Contador
	var count = Math.abs((endPercent - startPercent));

	// Valores step
	var step = endPercent < startPercent ? -0.01 : 0.01;

	// añade elemento SVG
	var container = d3Container.append('svg');

	// Añade grupo SVG
	var svg = container
        .attr('width', boxSize)
        .attr('height', boxSize)
        .append('g')
            .attr('transform', 'translate(' + (boxSize / 2) + ',' + (boxSize / 2) + ')');

	// Arc
	var arc = d3.svg.arc()
        .startAngle(0)
        .innerRadius(radio)
        .outerRadius(radio - bordo);

	// Fondo
	svg.append('path')
        .attr('class', 'd3-progress-background')
        //.attr('d', arc.endAngle(twoPi))
        .style('fill', '#eee');

	// Primer plano
	var foreground = svg.append('path')
        .attr('class', 'd3-progress-foreground')
        .attr('filter', 'url(#blur)')
        .style('fill', colorTexto)
        .style('stroke', colorTexto);

	//Frontal
	var front = svg.append('path')
        .attr('class', 'd3-progress-front')
        .style('fill', colorTexto)
        .style('fill-opacity', 1);

	var n = porcentaje.toString().length;

	var margin = -24;

	if (n <= 2)
		margin = -15;

	// Icono
	d3.select(element)
        .append("label").attr('style', 'top: ' + ((boxSize - iconSize) / 2) + 'px; margin-left:' + margin + 'px; position: absolute; text-align:center;font-size:19px; color:' + ColorIcono).text(endPercent + '%');

	// Título
	d3.select(element)
        .append('div')
            .attr('class', 'text-size-large text-semibold').attr('style', 'margin-top:10px')
            .text(textoTitulo);

	// Subtitulo
	d3.select(element)
        .append('div')
            .attr('class', 'text-size-small text-regular')
            .text(observaciones);

	// Animación
	function updateProgress(progress) {
		foreground.attr('d', arc.endAngle(twoPi * progress));
		front.attr('d', arc.endAngle(twoPi * progress));
	}

	// Animación Texto
	var progress = startPercent;
	(function loops() {
		updateProgress(progress);
		if (count > 0) {
			count--;
			progress += step;
			setTimeout(loops, 10);
		}
	})();
}

function sesionexpiro() {
	swal({
		title: 'Alerta',
		text: 'No se encontraron los datos de autenticación en la sesión; ingrese nuevamente.',
		type: 'warning',
		confirmButtonColor: '#FF7043',
		confirmButtonText: 'Aceptar',
		animation: 'pop',
		html: true,
		closeOnConfirm: false
	});

	setTimeout(IrAPaginaPrincipal, 3000);

}



function OtraUbicacion() {
	swal({
		title: 'Alerta',
		text: 'Se ha iniciado sesión desde otra ubicación.',
		type: 'warning',
		confirmButtonColor: '#FF7043',
		confirmButtonText: 'Aceptar',
		animation: 'pop',
		html: true,
		closeOnConfirm: false
	});

	setTimeout(IrAPaginaPrincipal, 3000);
}




function IrAPaginaPrincipal() {
	window.location.assign("../Login/Default.aspx");
}


function ValidarSesion() {

	$.ajax({
		type: "GET",
		dataType: "json",
		url: "/api/DatosSesion/",
	})
 .done(function (data, textStatus, jqXHR) {
 	//Aqui se podria validar el el usuario tiene movimiento y alargar un poco mas la sesión     
 })
 .fail(function (jqXHR, textStatus, errorThrown) {
 	//Cierro la sesión ya que no hay datos
 	sesionexpiro();
 });

}


var EnumHabilitacion =
[
{ ID: "0", Texto: 'Valida Objeto' },
{ ID: "1", Texto: 'Pruebas' },
{ ID: "99", Texto: 'Producción' }
];

//Busca en un array la descipcion, pasandole como parametros el array y el id 
function BuscarDescripcion(miArray, ID) {
	for (var i = 0; i < miArray.length; i += 1) {
		if (ID == miArray[i].ID) {
			return miArray[i].Texto;
		}
	}
}


function CrearGrafico(porcentaje, titulo, titulo2, color) {
	//Configuración de la barra de porcentaje
	var BarraPorcentaje = {
		startScaleValue: 0,
		endScaleValue: 100,
		tooltip: {
			customizeTooltip: function (arg) {
				return {
					text: titulo + Number(arg.target).toFixed(2) + titulo2
				};
			}
		}
	};

	return datos = $.extend({ value: 100, target: porcentaje, color: color }, BarraPorcentaje);
}


function nivelPlanes(nivel, tipo) {
	if (tipo == 3) {
		nivel = 100;
		color = '#5cb85c'; //Verde
	} else {
		var color = '#FE2E2E';
		if (nivel >= 71 && nivel <= 90) {
			color = '#E8BE0C';
		}

		if (nivel > 90) {
			color = '#FE2E2E';
		}

		if (nivel <= 70) {
			color = '#5cb85c';
		}
	}
	return color;
}


var CrearGraficoBarra = function (container, options) {

	var color = nivelPlanes(options.data.Porcentaje, options.data.CodCompra);
	$("<div/>").dxBullet({
		onIncidentOccurred: null,
		size: {
			width: 80,
			height: 35
		},
		margin: {
			top: 5,
			bottom: 0,
			left: 5
		},
		showTarget: true,
		target: options.data.Porcentaje,
		color: color,
		value: 100,
		startScaleValue: 0,
		endScaleValue: 100,
		tooltip: {
			enabled: true,
			font: {
				size: 18
			},
			paddingTopBottom: 2,
			customizeTooltip: function () {

				if (options.text.indexOf(".") > -1) {
					return { text: "Consumo Actual " + Number(options.text).toFixed(2) + "%" };
				} else {
					return { text: "Consumo Actual " + options.text + "%" };

				}
			},
			zIndex: 5
		}
	}).appendTo(container);
};


var CrearGraficoBarraFecha = function (container, options) {
	if (options.data.porcentajeFecha > 0) {
		var color = nivelPlanes(options.data.porcentajeFecha, options.data.CodCompra);
		$("<div/>").dxBullet({
			onIncidentOccurred: null,
			size: {
				width: 80,
				height: 35
			},
			margin: {
				top: 5,
				bottom: 0,
				left: 5
			},
			showTarget: true,
			target: options.data.porcentajeFecha,
			color: color,
			value: 100,
			startScaleValue: 0,
			endScaleValue: 100,
			tooltip: {
				enabled: true,
				font: {
					size: 18
				},
				paddingTopBottom: 2,
				customizeTooltip: function () {

					if (options.text.indexOf(".") > -1) {
						return { text: "Consumo Actual " + Number(options.text).toFixed(2) + "%" };
					} else {
						return { text: "Consumo Actual " + options.text + "%" };

					}
				},
				zIndex: 5
			}
		}).appendTo(container);
	}
};


///*******************************************MasterDetail de los grid
function ObtenerDetallle(PDF, XML, EstadoAcuse, RutaAcuse, XMLACUSE, ZIP, RutaDIAN, StrIdSeguridad, StrEmpresaFacturador, NumeroDocumento, Tipo) {

	var visible_zip = "";

	var visible_pdf = "style='pointer-events:auto;cursor: not-allowed;'";

	var visible_xml = "style='pointer-events:auto;cursor: not-allowed;'";

	var visible_xml_acuse = "style='pointer-events:auto;cursor: not-allowed;'";

	var visible_acuse = "   title='acuse pendiente' style='pointer-events:auto;cursor: not-allowed; color:white; margin-left:5%;'";

	var visible_Servicio_DIAN = "style='pointer-events:auto;cursor: not-allowed;'";

	if (PDF)
		visible_pdf = "href='" + PDF + "' title='ver PDF' style='pointer-events:auto;cursor: pointer;'";
	else
		visible_pdf = "#";

	if (XML)
		visible_xml = "href='" + XML + "' class='icon-file-xml' title='ver XML' style='pointer-events:auto;cursor: pointer;'";
	else
		visible_xml = "#";

	if (EstadoAcuse == 1 || EstadoAcuse == 2 || EstadoAcuse == 3)
		visible_acuse = "href='" + RutaAcuse + "' class='icon-file-eye2'  title='ver acuse'  style='pointer-events:auto;cursor: pointer; margin-left:5%; '";
	else
		visible_acuse = "#";

	if (XMLACUSE != "#" && XMLACUSE != null)
		visible_xml_acuse = "href='" + XMLACUSE + "' class='icon-file-xml' title='ver XML Respuesta acuse' style='pointer-events:auto;cursor: pointer'";
	else
		visible_xml_acuse = "#";

	if (ZIP)
		visible_zip = "href='" + ZIP + "' class='icon-file-zip' title='ver anexo' style='pointer-events:auto;cursor: pointer'";
	else
		visible_zip = "#";

	if (RutaDIAN)
		visible_Servicio_DIAN = "class='icon-file-xml' href='" + RutaDIAN + "' title='ver XML' style='pointer-events:auto;cursor: pointer;'";
	else
		visible_Servicio_DIAN = "#";

	if (Tipo == "Adquiriente") {
		return "<td aria-selected='false' role='gridcell' aria-colindex='1' class='dx-cell-focus-disabled dx-master-detail-cell' colspan='6' style='text-align: center;'><div class='master-detail-caption'>Lista de Archivos:</div><div class='dx-widget dx-visibility-change-handler' role='presentation'><div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>PDF Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='2' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='3' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Ver Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Anexo</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Respuesta DIAN</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'><col style='width: 16%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='2' style='text-align: center;'><div> <a style='margin-left:5%;margin-right:5%;' target='_blank' " + visible_xml + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='3' style='text-align: center;'><div> <a target='_blank'  " + visible_xml_acuse + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank'   " + visible_acuse + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_zip + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='5' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_Servicio_DIAN + "></a></div></td></tr><tr class='dx-row dx-column-lines dx-freespace-row' role='row' style='height: 0px; display: none;'><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td></tr></tbody></table></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-horizontal dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='width: 831px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-vertical dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='height: 35px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div></div></div><span class='dx-datagrid-nodata dx-hidden'></span></div><div class='dx-hidden' style='padding-right: 0px;'></div><div></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-drag-header dx-datagrid-text-content dx-widget' style='display: none;'></div><div class='dx-context-menu dx-has-context-menu dx-widget dx-visibility-change-handler dx-collection dx-datagrid'></div><div class='dx-header-filter-menu'></div><div></div></div></div></td>";
	} else {
		return "<td aria-selected='false' role='gridcell' aria-colindex='1' class='dx-cell-focus-disabled dx-master-detail-cell' colspan='6' style='text-align: center;'><div class='master-detail-caption'>Lista de Archivos:</div><div class='dx-widget dx-visibility-change-handler' role='presentation'><div class='dx-datagrid dx-gridbase-container dx-datagrid-borders' role='grid' aria-label='Data grid' aria-rowcount='1' aria-colcount='4'><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-headers dx-datagrid-nowrap' role='presentation' style='padding-right: 0px;'><div class='dx-datagrid-content dx-datagrid-scroll-container' role='presentation'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation'><colgroup><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody class=''><tr class='dx-row dx-column-lines dx-header-row' role='row'><td aria-selected='false' role='columnheader' aria-colindex='1' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>PDF Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='2' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Documento</div></td><td aria-selected='false' role='columnheader' aria-colindex='3' class='dx-datagrid-action dx-cell-focus-disabled' aria-sort='none' style='text-align: center;'><div class='dx-column-indicators' style='float: right;'><span class='dx-sort dx-sort-none'></span></div><div class='dx-datagrid-text-content dx-text-content-alignment-center'>XML Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Ver Acuse</div></td><td aria-selected='false' role='columnheader' aria-colindex='4' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Anexo</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Respuesta DIAN</div></td><td aria-selected='false' role='columnheader' aria-colindex='5' aria-sort='none' class='dx-cell-focus-disabled' style='text-align: center;'><div class='dx-datagrid-text-content'>Auditoría</div></td></tr></tbody></table></div></div><div class='dx-datagrid-rowsview dx-datagrid-nowrap dx-scrollable dx-visibility-change-handler dx-scrollable-both dx-scrollable-simulated dx-scrollable-customizable-scrollbars' role='presentation'><div class='dx-scrollable-wrapper'><div class='dx-scrollable-container'><div class='dx-scrollable-content' style='center: 0px; top: 0px; transform: none;'><div class='dx-datagrid-content'><table class='dx-datagrid-table dx-datagrid-table-fixed' role='presentation' style='table-layout: fixed;'><colgroup style=''><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'><col style='width: 14%;'></colgroup><tbody><tr class='dx-row dx-data-row dx-column-lines' role='row' aria-rowindex='1' aria-selected='false'><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'> <div> <a style='margin-left:5%;' target='_blank' class='icon-file-pdf'  " + visible_pdf + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='2' style='text-align: center;'><div> <a style='margin-left:5%;margin-right:5%;' target='_blank'  " + visible_xml + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='3' style='text-align: center;'><div> <a target='_blank'  " + visible_xml_acuse + "></a></div> </td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank'   " + visible_acuse + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='4' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_zip + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='5' class='dx-editor-inline-block dx-cell-focus-disabled dx-editor-cell dx-datagrid-readonly' style='text-align: center;'><div class='dx-datagrid-checkbox-size dx-checkbox dx-state-readonly dx-widget' role='checkbox' aria-checked='false' aria-readonly='true'><input type='hidden' value='false'><a style='margin-left:5%;' target='_blank' " + visible_Servicio_DIAN + "></a></div></td><td aria-selected='false' role='gridcell' aria-colindex='1' tabindex='0' style='text-align: center;'><div><a style='margin-left:5%;' class='icon-file-eye' onClick=ConsultarAuditDoc('" + StrIdSeguridad + "','" + StrEmpresaFacturador + "','" + NumeroDocumento + "') target='_blank' data-toggle='modal' data-target='#modal_audit_documento' title='ver Auditoría'></a></div></td></tr><tr class='dx-row dx-column-lines dx-freespace-row' role='row' style='height: 0px; display: none;'><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td><td style='text-align: center;'></td></tr></tbody></table></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-horizontal dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='width: 831px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div><div class='dx-scrollable-scrollbar dx-widget dx-scrollbar-vertical dx-scrollbar-hoverable' style='display: none;'><div class='dx-scrollable-scroll dx-state-invisible' style='height: 35px; transform: translate(0px, 0px);'><div class='dx-scrollable-scroll-content'></div></div></div></div></div><span class='dx-datagrid-nodata dx-hidden'></span></div><div class='dx-hidden' style='padding-right: 0px;'></div><div></div><div class='dx-hidden'></div><div class='dx-hidden'></div><div class='dx-datagrid-drag-header dx-datagrid-text-content dx-widget' style='display: none;'></div><div class='dx-context-menu dx-has-context-menu dx-widget dx-visibility-change-handler dx-collection dx-datagrid'></div><div class='dx-header-filter-menu'></div><div></div></div></div></td>";
	}
}
///*******************************************************************


var collapsed = false;

//*******************************Enumerables
var TipoAlerta =
    [
        { "ID": 1, "Name": "Porcentaje." },
        { "ID": 2, "Name": "Facturador sin Saldo." },
        { "ID": 3, "Name": "Vencimiento de Planes." },
		{ "ID": 4, "Name": "Por Recarga." },
		{ "ID": 5, "Name": "Por Pago." },
		{ "ID": 6, "Name": "Solicitud Aprobación de Formato." },
		{ "ID": 7, "Name": "Aprobación de Formato." },
		{ "ID": 8, "Name": "Publicación de Formato." },
		{ "ID": 9, "Name": "Alertas Documentos DIAN." }
    ];

var EstadoAlerta =
[
	{ "ID": 0, "Name": "Activa" },
	{ "ID": 1, "Name": "Inactiva" }

];

var TipoPlan =
[
	{ "ID": 1, "Name": "Recarga Interna" },
	{ "ID": 2, "Name": "Compra" },
	{ "ID": 3, "Name": "Post-Pago" }
];

var EstadoPlan =
[
	{ "ID": 0, "Name": "Habilitado" },
	{ "ID": 1, "Name": "Inabilitado" },
	{ "ID": 2, "Name": "Procesado" }
];

var ProcesoEstado =
    [
        { "ID": 1, "Name": "Recepción" },
        { "ID": 2, "Name": "Validación Documento" },
        { "ID": 3, "Name": "Generación UBL" },
		{ "ID": 4, "Name": "Almacenamiento XML" },
		{ "ID": 5, "Name": "Firma XML" },
		{ "ID": 6, "Name": "Compresión XML" },
		{ "ID": 7, "Name": "Envío Dian" },
		{ "ID": 8, "Name": "Envío E-mail Adquiriente" },
		{ "ID": 9, "Name": "Recepción Acuse" },
		{ "ID": 10, "Name": "Envío E-mail Acuse" },
		{ "ID": 11, "Name": "Documento Pendiente Envío Proveedor" },
		{ "ID": 12, "Name": "Envío Exitoso Proveedor" },
		{ "ID": 13, "Name": "Acuse Pendiente Envío Proveedor" },
		{ "ID": 14, "Name": "Consulta DIAN" },
		{ "ID": 15, "Name": "Pago Documento" },
		{ "ID": 16, "Name": "Acuse Visto" },
		{ "ID": 20, "Name": "Almacenamiento Formato PDF" },
		{ "ID": 22, "Name": "Generación Formato PDF" },
		{ "ID": 24, "Name": "Almacenamiento Anexo ZIP" },
		{ "ID": 90, "Name": "Error Dian, Finaliza Proceso" },
		{ "ID": 92, "Name": "Error Prevalidación Dian V2" },
		{ "ID": 93, "Name": "Error Prevalidación Plataforma V2" },
		{ "ID": 94, "Name": "Proceso Pausado Prevalidación Plataforma Dian V2" },
		{ "ID": 99, "Name": "Fin Proceso Exitoso" }

    ];

var CategoriaEstado =
[
	{ "ID": 0, "Name": "No Recibido" },
	{ "ID": 100, "Name": "Recibido Plataforma" },
	{ "ID": 200, "Name": "Envío DIAN" },
	{ "ID": 300, "Name": "Validado DIAN" },
	{ "ID": 400, "Name": "Fallido DIAN" }	
];


var AdquirienteRecibo =
[
	{ "ID": 0, "Name": "Pendiente" },
	{ "ID": 1, "Name": "Aprobado" },
	{ "ID": 2, "Name": "Rechazado" },
	{ "ID": 3, "Name": "Aprobado Tácito" },
	{ "ID": 4, "Name": "Entregado" },
	{ "ID": 5, "Name": "Leído" },
	{ "ID": 6, "Name": "No Entregado" },
	{ "ID": 7, "Name": "Enviado" }
];


var EstadoEnvio =
[
	{ "ID": 0, "Name": "Pendiente" },
	{ "ID": 1, "Name": "Enviado" },
	{ "ID": 2, "Name": "Entregado" },
	{ "ID": 3, "Name": "No Entregado" },
	{ "ID": 4, "Name": "Leído" },
	{ "ID": 5, "Name": "Validar con Adquiriente" }
];


var MensajeEstado =
[
	{ "ID": 0, "Name": "Procesado" },
	{ "ID": 1, "Name": "En cola" },
	{ "ID": 2, "Name": "Enviado" },
	{ "ID": 3, "Name": "Abierto" },
	{ "ID": 4, "Name": "Presionó" },
	{ "ID": 5, "Name": "Rebotado" },
	{ "ID": 6, "Name": "Spam" },
	{ "ID": 7, "Name": "Desuscripción" },
	{ "ID": 8, "Name": "Bloqueado" },
	{ "ID": 9, "Name": "Rebotado" },
	{ "ID": 10, "Name": "Rebotado" },
	{ "ID": 11, "Name": "Diferido" }
];


//Busca en un array la descipcion, pasandole como parametros el array y el id 
function GetDescripcionEnum(miArray, ID) {
	for (var i = 0; i < miArray.length; i += 1) {
		if (ID == miArray[i].ID) {
			return miArray[i].Name;
		}
	}
}
//*******************************Enumerables




