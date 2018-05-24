DevExpress.localization.locale(navigator.language);


var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var GestionPlanesApp = angular.module('GestionPlanesApp', ['ModalEmpresasApp', 'dx']);
//Controlador para la gestion planes transaccionales
GestionPlanesApp.controller('GestionPlanesController', function GestionPlanesController($scope, $http, $location) {

    var now = new Date();

    var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";
    $http.get('/api/DatosSesion/').then(function (response) {
        console.log("Datos", response.data);
        codigo_facturador = response.data[0].Identificacion;
        if (codigo_facturador == '811021438') {
            $scope.Admin = true;
        } else {
            $('#SelecionarEmpresa').hide();
            $("#EmpresaPlan").dxTextBox({ value: codigo_facturador + ' -- ' + response.data[0].RazonSocial });
        };

    });

    var tipo_proceso = "",
        codigo_empresa = "",
        cantidad_transacciones = "",
        valor_plan = "",
        estado_plan = ""
    datos_empresa_asociada = "";


    //Define los campos del Formulario  
    $(function () {
        $("#summary").dxValidationSummary({});

        //Selección Tipo de Proceso
        $("#TipoProceso").dxRadioGroup({
            searchEnabled: true,
            caption: 'TipoProceso',
            dataSource: new DevExpress.data.ArrayStore({
                data: TiposProceso,
                key: "ID"
            }),
            displayExpr: "Texto",
            Enabled: true,
            onValueChanged: function (data) {
                console.log("tipo_proceso", data.value.ID);
                Datos_Habilitacion = data.value.ID;
            }
        }).dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe indicar el tipo de proceso."
            }]
        });

        //Campo de selección de empresa.
        $("#EmpresaPlan").dxTextBox({
            readOnly: true,
            value: codigo_empresa,
            name: EmpresaPlan,
            onValueChanged: function (data) {
                console.log("EmpresaPlan", data.value);
                datos_empresa_asociada = data.value;
            }
        }).dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe seleccionar una empresa."
            }]
        });

        //Campo cantidad de transacciones del plan
        $("#CantidadTransacciones").dxTextBox({
            onValueChanged: function (data) {
                console.log("CantidadTransacciones", data.value);
                cantidad_transacciones = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe indicar la cantidad de transacciones del plan."
            }, {
                type: "numeric",
                message: "El campo sólo debe contener números."
            }]
        });


        //Campo Valor plan
        $("#ValorPlan").dxTextBox({
            onValueChanged: function (data) {
                console.log("ValorPlan", data.value);
                valor_plan = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe indicar el valor del plan."
            }, {
                type: "numeric",
                message: "El campo sólo debe contener números."
            }]
        });


        //Selección Estado de plan
        $("#EstadoPlan").dxRadioGroup({
            searchEnabled: true,
            caption: 'EstadoPlan',
            dataSource: new DevExpress.data.ArrayStore({
                data: EstadosPlanes,
                key: "ID"
            }),
            displayExpr: "Texto",
            Enabled: true,
            onValueChanged: function (data) {
                console.log("estado_plan", data.value.ID);
                estado_plan = data.value.ID;
            }
        }).dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe indicar el estado del plan."
            }]
        });



        $("#button").dxButton({
            text: "Guardar",
            type: "default",
            useSubmitBehavior: true
        });


        $("#form1").on("submit", function (e) {
            guardarEmpresa();
            e.preventDefault
                ();
        });

    });

    $scope.ButtonGuardar = {
        text: 'Guardar',
        type: 'default',
        validationGroup: "ValidacionDatosPlan",
        onClick: function (e) {
            GuardarPlan();
        }
    };

    function GuardarPlan() {
        var empresa = datos_empresa_asociada.split(' -- ');

        var data = $.param({
            TipoIdentificacion: Datos_Tipoidentificacion,
            Identificacion: Datos_Idententificacion,
            RazonSocial: Datos_Razon_Social,
            Email: Datos_Email,
            Intadquiriente: (Datos_Adquiriente) ? true : false,
            IntObligado: (Datos_Obligado) ? true : false,
            IntHabilitacion: Datos_Habilitacion,
            StrEmpresaAsociada: empresa[0],
            tipo: Datos_Tipo,
            StrResolucionDian: Datos_Resolucion
        });

        $("#wait").show();
        $http.post('/api/Empresas?' + data).then(function (response) {
            $("#wait").hide();
            try {
                DevExpress.ui.notify({ message: "El plan ha sido registrado con exito.", position: { my: "center top", at: "center top" } }, "success", 6000);

            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 3000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });
    }

});







//Controlador para gestionar la consulta de empresas
EmpresasApp.controller('ConsultaEmpresasController', function ConsultaEmpresasController($scope, $http, $location) {

    $("#wait").show();
    $http.get('/api/Empresas').then(function (response) {
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
                       dataField: "Perfil"
                   },
                   {
                       dataField: "Habilitacion"
                   },
                   {
                       caption: "Editar",
                       cssClass: "col-md-1 col-xs-2",
                       cellTemplate: function (container, options) {
                           $("<div style='text-align:center'>")
                               .append($("<a taget=_self class='icon-pencil3' title='Editar' href='GestionEmpresas.aspx?IdSeguridad=" + options.data.IdSeguridad + "'>"))
                               .appendTo(container);
                       }
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

//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
    for (var i = 0; i < miArray.length; i += 1) {
        if (ID == miArray[i].ID) {
            return i;
        }
    }
}




var TiposProceso =
    [
        { ID: "1", Texto: 'Cortesía' },
        { ID: "2", Texto: 'Compra' },
    ];

var EstadosPlanes =
    [
        { ID: "0", Texto: 'Inhabilitar' },
        { ID: "1", Texto: 'Habilitar' },

    ];
