//Documentación
//****************************************************************************************
//**Asignar un Valor:									Set_Nombre del campo(Valor)     **
//**Para bloquear el campo :								Bloquear_Nombre del campo   **
//**para Obtener el valor del campo:					txt_hgi_Nombre del campo        **
//**Para borrar los datos del control y la variable:	Limpiar_Nombre del campo        **
//**Para Desbloquear el campo y permitir busqueda:      Desbloquear_Nombre del campo    **
//**Para indicar campo Valido							Campo_Valido_Nombre del campo   **
//**Para indicar campo Invalido							Campo_Invalido_Nombre del campo **
//****************************************************************************************

//Servicio de Filtro Generico HGI
var AppSrvFiltro = angular.module('AppSrvFiltro', ['dx'])
 .config(function ($httpProvider) {
     $httpProvider.interceptors.push(myInterceptor);
 })
 //Paramtros:
 //Titulo,Nombre, Icono, Tecla, urlapi, Campo_Codigo, Campo_Descripcion,ValidarVacio
 //
.service('SrvFiltro', function ($location, $q) {
	this.ObtenerFiltro = function (Titulo, Nombre, Icono, Tecla, urlapi, Campo_Codigo, Campo_Descripcion, ValidarVacio, indice) {
	
        //Html de vista modal        
		var Modal = '<!DOCTYPE html><html xmlns="http://www.w3.org/1999/xhtml"><head runat="server">    <title></title></head><body>   <form><div id="modal_Buscar_' + Nombre + '" class="modal fade" style="display: none; z-index:999999;" ><div class="modal-dialog">    <div class="modal-content"><div id="EncabezadoModal" class="modal-header"><button type="button" class="close" data-dismiss="modal"></button><h5 style="margin-bottom: 10px;" class="modal-title">Busqueda de ' + Nombre + '</h5></div><div class="modal-body"><div class="col-md-12 "> <div ><div class="col-md-12"><div class="panel panel-white"><div><br /><br /><div class="col-md-12">    <div class="col-md-10"><h6 class="panel-title">Lista de ' + Nombre + '</h6>    </div>   </div>    </div>    <br />    <div class="panel-body"><div class="demo-container" name="DivPrueba">    <div id="grid_' + Nombre + '" name="grid_' + Nombre + '"></div></div>    </div></div></div>    </div></div></div><div id="divsombra" class="modal-footer" style="margin-top: 22%"></div>    </div></div></div>    </form></body></html>';
        //Codigo de selección de la opcion dentro del grid
        var codigo_busqueda = "var Datos_hgi_" + Nombre + ";  var txt_hgi_" + Nombre + "; function Obtener_hgi_" + Nombre + "(Codigo) {    fLen = Datos_hgi_" + Nombre + ".length;    for (i = 0; i < fLen; i++) {        if (Datos_hgi_" + Nombre + "[i]." + Campo_Codigo + " == Codigo) {    txt_hgi_" + Nombre + " =Codigo;        Codigo = Codigo + '--' + Datos_hgi_" + Nombre + "[i]." + Campo_Descripcion + "  ;             break;        }    }        $('#txt_filtro_" + Nombre + "').val(Codigo);  Campo_Valido_"+Nombre +"();   }";
        //Jquery con webapi        
        var json = "<script>  " + codigo_busqueda + "  $.ajax({url: '" + urlapi + "', success: function(result){ Datos_hgi_" + Nombre + "=result;    $('#grid_" + Nombre + "').dxDataGrid({ dataSource: result, paging: {  pageSize: 10     },  pager: {   showPageSizeSelector: true, allowedPageSizes: [5, 10, 20],showInfo: true}, columns: [{caption: 'Código',dataField: '" + Campo_Codigo + "',cssClass: 'col-md-3',cellTemplate: function (container, options) {$('<div style=\"text-align:left\">').append($('<a taget=_self  data-dismiss=\"modal\" title=\"Seleccionar " + Nombre + "\" onclick=Obtener_hgi_" + Nombre + "(' + options.data." + Campo_Codigo + " + ')>' + options.data." + Campo_Codigo + " + '</a>')).appendTo(container);}},{caption: 'Descripción',dataField: '" + Campo_Descripcion + "',cssClass: 'col-md-9'}],filterRow: {visible: true}});  }});    </script>";

        var FnoLlamaFiltro = "<script>  function Click_Hgi_" + Nombre + "() {  $('#modal_Buscar_" + Nombre + "').modal('show'); setTimeout(function(){	$('.dx-texteditor-input:eq("+indice+")').focus();}, 1000); } </script>";
		//Crea función para bloquear campo
        var BloquearCampo = "<script> function Bloquear_" + Nombre + "() { $('#txt_filtro_" + Nombre + "').prop('disabled', true);	$('#I_" + Nombre + "').removeClass('icon-search4');	$('#Div_" + Nombre + "').removeClass('dx-texteditor-buttons-container'); } </script>";

		//Crea función para Desbloquear campo
        var DesbloquearCampo = "<script> function Desbloquear_" + Nombre + "() { $('#txt_filtro_" + Nombre + "').prop('disabled', false);	$('#I_" + Nombre + "').addClass('icon-search4');	$('#Div_" + Nombre + "').addClass('dx-button-content'); } </script>";

		//Crea función para Limpiar campo
        var LimpiarCampo = "<script> function Limpiar_" + Nombre + "() { txt_hgi_" + Nombre + "='';  $('#txt_filtro_" + Nombre + "').val('');  } </script>";

		//Asignar valor
        var AsignarValorCampo = "<script> function Set_" + Nombre + "(Valor) { $('#txt_filtro_" + Nombre + "').val(Valor); var SplitControlHgi;  var ValorSplit; SplitControlHgi = Valor.split('--');  ValorSplit = SplitControlHgi[0];         txt_hgi_" + Nombre + "=ValorSplit;                     } </script>";

		//Campo Valido e Invalido
        var ValidarCampo = "<script> function Campo_Invalido_" + Nombre + " () {$('#" + Nombre + "').addClass('dx-invalid'); $('#txt_filtro_" + Nombre + "').attr('placeholder','Debe indicar un valor real'); }; function Campo_Valido_" + Nombre + " () {$('#" + Nombre + "').removeClass('dx-invalid'); $('#txt_filtro_" + Nombre + "').attr('placeholder',''); };  </script>";

		//Busca el valor ingresado en la lista de busqueda
        var BuscarFiltro = '<script> $("#txt_filtro_' + Nombre + '").blur(function () { if('+ValidarVacio+'){ var elemeto_hgi_' + Nombre + '= Datos_hgi_' + Nombre + '.find(d => d.ID == txt_hgi_' + Nombre + ');	if (elemeto_hgi_' + Nombre + ' == undefined) { 	Campo_Invalido_' + Nombre + '(); Set_' + Nombre + '("");}else{Campo_Valido_' + Nombre + '();}}}); </script>';
		//dx-invalid

        //Impresión de campos
        return $.when().then(function () {
        	return json + '<div class="col-md-12" style="padding-left: 0px; padding-right:0px; "><i class="' + Icono + '"></i><label>' + Titulo + ':</label><div id="' + Nombre + '" class="dx-datebox dx-textbox dx-texteditor dx-dropdowneditor-button-visible dx-widget dx-visibility-change-handler dx-dropdowneditor dx-datebox-date " style="width: 100%;"><div class="dx-dropdowneditor-input-wrapper"><input type="hidden" ><div class="dx-texteditor-container"><input autocomplete="off" id="txt_filtro_' + Nombre + '" name="txt_filtro_' + Nombre + '" ng-model="hgi_filtro_' + Nombre + '" class="dx-texteditor-input" aria-haspopup="true" aria-autocomplete="list" type="text" spellcheck="false" tabindex="0" aria-expanded="false" role="combobox"><div data-dx_placeholder="" class="dx-placeholder dx-state-invisible"></div><div  class="dx-texteditor-buttons-container"><div class="dx-dropdowneditor-button dx-button-normal dx-widget" role="button"><div id=Div_' + Nombre + ' name=Div_' + Nombre + ' class="dx-button-content"><i class="icon-search4" id=I_' + Nombre + ' name=I_' + Nombre + '  onClick="Click_Hgi_' + Nombre + '();"></i></div></div></div></div></div></div></div>  <script> $(function () {  $("#txt_filtro_' + Nombre + '").keyup(function () {    txt_hgi_' + Nombre + ' = $("#txt_filtro_' + Nombre + '").val();     var SplitControlHgi; var ValorSplit;    SplitControlHgi = txt_hgi_' + Nombre + '.split(" -- ");    ValorSplit = SplitControlHgi[0];  txt_hgi_' + Nombre + ' = ValorSplit;    }); });   $("#txt_filtro_' + Nombre + '").keydown(function () {  if (event.keyCode == ' + Tecla + ') {   Click_Hgi_' + Nombre + '();   }        });     </script>   ' + Modal + FnoLlamaFiltro + BloquearCampo + LimpiarCampo + AsignarValorCampo + DesbloquearCampo + BuscarFiltro + ValidarCampo;
        });
    }
});


AppSrvFiltro.directive('hgiFiltro', function ($compile) {
    return {
        compile: function compile(tElement, tAttrs, transclude) {
            return {
                post: function preLink(scope, elem, iAttrs, controller) {
                    var scopePropName = iAttrs['hgiFiltro'];
                    var linkingFunc = $compile(scope[scopePropName]);
                    linkingFunc(scope, function (newElem) {
                        elem.replaceWith(newElem);
                    });
                }
            };
        }
    }
});