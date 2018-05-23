DevExpress.localization.locale(navigator.language);


var ModalSerialEmpresaApp = angular.module('ModalSerialEmpresaApp', []);

var SerialEmpresaApp = angular.module('SerialEmpresaApp', ['ModalSerialEmpresaApp', 'dx']);


//Controlador para gestionar la consulta de empresas
SerialEmpresaApp.controller('SerialEmpresaController', function SerialEmpresaController($scope, $http, $location) {


    $("#wait").show();
    $http.get('/api/Empresas?Facturador=true').then(function (response) {
        $("#wait").hide();

        $("#gridEmpresas").dxDataGrid({
            dataSource: response.data,

            paging: {
                pageSize: 20
            },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [5, 10, 20],
                showInfo: true
            }

               , columns: [
                   {
                       //href='GestionEmpresas.aspx?IdSeguridad=" + options.data.IdSeguridad + "'
                       cssClass: "col-md-1 col-xs-2",
                       cellTemplate: function (container, options) {                                             
                           $("<div style='text-align:center'>")
                               .append($("<a class='icon-pencil3' data-toggle='modal' data-target='#modal_Serial_empresa' style='margin-left:12%; font-size:19px'></a>").dxButton({
                                   onClick: function () {                                       
                                       $scope.email = options.data.Email;
                                       $('#txtemail').dxTextBox({ value: $scope.email });
                                       $("#txtemail").removeClass("dx-textbox dx-texteditor dx-state-readonly dx-widget");

                                       $scope.nitEmpresa = options.data.Identificacion;
                                       $('#txtnitEmpresa').dxTextBox({ value: $scope.nitEmpresa });
                                       $("#txtnitEmpresa").removeClass("dx-textbox dx-texteditor dx-state-readonly dx-widget");

                                       $scope.nombre = options.data.RazonSocial;
                                       $('#txtnombre').dxTextBox({ value: $scope.nombre });
                                       $("#txtnombre").removeClass("dx-textbox dx-texteditor dx-state-readonly dx-widget");

                                       $("#txtResolucion").dxTextBox({ value: options.data.Resolucion });
                                       $("#txtSerial").dxTextBox({ value: options.data.Serial });
                                                                                                                    
                                   }
                               }).removeClass("dx-button dx-button-normal dx-widget"))
                               .appendTo(container);
                       }
                   },
                   
                   {
                       caption: "Identificacion",
                       dataField: "Identificacion"
                   },
                   {
                       caption: "Razón Social",
                       dataField: "RazonSocial"
                   },
                   {
                       caption: "Email",
                       dataField: "Email"
                   },
                   {
                       caption: "Serial",
                       dataField: "Serial"
                   },                 
                   {
                       caption: "Resolución",
                       dataField: "Resolucion"
                   }
               ],
            filterRow: {
                visible: true
            }
        });

    }, function errorCallback(response) {
        $('#wait').hide();
        DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
    });

});


ModalSerialEmpresaApp.controller('ModalSerialEmpresaController', function ModalSerialEmpresaController($scope, $http, $location) {

    var Datos_Resolucion = "", Datos_Serial = "", Datos_correo="";

    //Define los campos del Formulario  
    $(function () {
        $("#summary").dxValidationSummary({});

        $("#txtSerial").dxTextBox({

            onValueChanged: function (data) {
                console.log("txtSerial", data.value);
                Datos_Serial = data.value;
            }
        })
       .dxValidator({
           validationRules: [{
               type: "required",
               message: "Debe introducir el Serial"
           }, {
               type: "stringLength",
               max: 50,
               message: "El Nª de Serial no puede ser mayor a 50 Digitos"
           }]
       });


        $("#txtResolucion").dxTextBox({

            onValueChanged: function (data) {
                console.log("txtResolucion", data.value);
                Datos_Resolucion = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir el codigo de Resolución"
            }, {
                type: "numeric",
                message: "El codigo de Resolución debe ser numerico"
            }, {
                type: "stringLength",
                max: 10,
                message: "El Nª de Resolución no puede ser mayor a 10 Digitos"
            }]
        });


   
       
        $("#txtnitEmpresa").dxTextBox({
            readOnly: true,
        });

        $("#txtemail").dxTextBox({
            readOnly: true,
        });
        $("#txtnombre").dxTextBox({
            readOnly: true,
        });



        $("#btnActivar").dxButton({
            text: "Guardar",
            type: "default",
            useSubmitBehavior: true
            ,onClick: function () {
                guardarSerial();
            }    
        });


        $("#form1").on("submit", function (e) {            
            e.preventDefault();
        });


    });


    function guardarSerial() {
        if (Datos_Resolucion != '' && Datos_Serial != '') {
            

            Datos_Identificacion = $scope.nitEmpresa;
            var data = $.param({
                Identificacion: Datos_Identificacion,
                Serial: Datos_Serial,
                Resolucion: Datos_Resolucion
            });

            $("#wait").show();
            $http.post('/api/Empresas?' + data).then(function (response) {
                $("#wait").hide();
                try {
                    //Aqui se debe colocar los pasos a seguir
                    DevExpress.ui.notify({ message: "Se ha enviado un correo a " + $scope.email + " " , position: { my: "center top", at: "center top" } }, "success", 6000);
                    $("#btnActivar").hide();
                    $("#btncancelar").hide();
                    setTimeout(IrAConsulta, 6000);
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);                    
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });            
        } else {

        }
    }


});

//Esta funcion es para ir a la pagina de consulta
function IrAConsulta() {
    $("#btnActivar").show();
    $("#btncancelar").show();
    $("#txtResolucion").dxTextBox({ value: '' });
    $("#txtSerial").dxTextBox({ value: '' });
    //Cierro la modal
    $('#cerrrarModal').click();
    Datos_Resolucion = '';
    Datos_Serial = ''
}


//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
    for (var i = 0; i < miArray.length; i += 1) {
        if (ID == miArray[i].ID) {
            return i;
        }
    }
}

var TiposHabilitacion =
    [
        { ID: "0", Texto: 'Valida Objeto' },
        { ID: "1", Texto: 'Pruebas' },
        { ID: "99", Texto: 'Producción' }
    ];

var TiposIdentificacion =
    [
        { ID: "11", Texto: 'Registro civil' },
        { ID: "12", Texto: 'Tarjeta de identidad' },
        { ID: "13", Texto: 'Cédula de ciudadanía' },
        { ID: "21", Texto: 'Tarjeta de extranjería' },
        { ID: "22", Texto: 'Cédula de extranjería' },
        { ID: "31", Texto: 'NIT' },
        { ID: "41", Texto: 'Pasaporte' },
        { ID: "42", Texto: 'Documento de identificación extranjero' }
    ];
