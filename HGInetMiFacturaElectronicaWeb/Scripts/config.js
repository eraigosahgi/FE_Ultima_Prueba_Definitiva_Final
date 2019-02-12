


function ColocarEstado(Estado, Descripcion) {
	return "<span " + ((Estado == '400') ? " class='badge badge-FallidoDIAN'  title='" + Descripcion + "'" : (Estado == '300') ? " class='badge badge-ValidadoDIAN'  title='" + Descripcion + "'" : (Estado == '200') ? " class='badge badge-envioDian'   title='" + Descripcion + "'" : " class='badge badge-RecibidoPlataforma'  title='" + Descripcion + "'") + " style='border-radius: 0px !important;'  >" + Descripcion + "</span>"
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
				}else{
					return { text: "Consumo Actual " + options.text + "%" };
				
				}
			},
			zIndex: 5
		}
	}).appendTo(container);
};

var collapsed = false;

