DevExpress.localization.locale(navigator.language);


var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var EmpresasApp = angular.module('EmpresasApp', ['ModalEmpresasApp', 'dx']);
//Controlador para la gestion de Empresas(Editar, Nueva Empresa)
EmpresasApp.controller('GestionEmpresasController', function GestionEmpresasController($scope, $http, $location) {

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
            $("#txtempresaasociada").dxTextBox({ value: codigo_facturador + ' -- ' + response.data[0].RazonSocial });
        };
    });

    var Datos_Tipoidentificacion = "",
        Datos_Idententificacion = "",
        Datos_Razon_Social = "",
        Datos_Email = "",
        Datos_Adquiriente = "",
        Datos_Obligado = "",
        Datos_Habilitacion = ""
    Datos_IdentificacionDv = "",
    Datos_Tipo = "1",
    Datos_Observaciones = "",
    Datos_empresa_Asociada = ""


    //Define los campos del Formulario  
    $(function () {
        $("#summary").dxValidationSummary({});

        $("#TipoIndentificacion").dxSelectBox({
            placeholder: "Seleccione el tipo de Indetificación",
            displayExpr: "Texto",
            dataSource: TiposIdentificacion,
            onValueChanged: function (data) {
                console.log("EstadoRecibo", data.value.ID);
                Datos_Tipoidentificacion = data.value.ID;
            }
        }).dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe seleccionar el tipo de documento"
            }]
        });

        $("#NumeroIdentificacion").dxTextBox({
            onValueChanged: function (data) {
                console.log("NumeroDocumento", data.value);
                Datos_Idententificacion = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe Indicar el numero de Identificación"
            }, {
                type: "stringLength",
                max: 50,
                message: "El numero de Identificación no puede ser mayor a 50 digitos"
            }, {
                type: "numeric",
                message: "El numero de Identificación debe ser numérico"
            }]
        });

        $("#txtRasonSocial").dxTextBox({
            onValueChanged: function (data) {
                console.log("RasonSocial", data.value);
                Datos_Razon_Social = data.value.toUpperCase();
            }
        })
            .dxValidator({
                validationRules: [{
                    type: "stringLength",
                    max: 200,
                    message: "La razón Social no puede ser mayor a 200 caracteres"
                }, {
                    type: "required",
                    message: "Debe introducir la Razón Social"
                }]
            });

        $("#txtEmail").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtEmail", data.value);
                Datos_Email = data.value.toUpperCase();
            }
        })
            .dxValidator({
                validationRules: [{
                    type: "stringLength",
                    max: 200,
                    message: "El Email no puede ser mayor a 200 caracteres"
                }, {
                    type: "required",
                    message: "Debe introducir el Email"
                }, {
                    type: "email",
                    message: "El campo Email no tiene el formato correcto"
                }]
            });

        $("#txtempresaasociada").dxTextBox({
            readOnly: true,
            value: codigo_facturador,
            name: txtempresaasociada,
            onValueChanged: function (data) {
                console.log("txtempresaasociada", data.value);
                Datos_empresa_Asociada = data.value;
            }
        });

        $("#txtidentificciondev").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtidentificciondev", data.value);
                Datos_Email = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir el codigo del Facturador"
            }]
        });


        $("#Facturador").dxCheckBox({
            name: "PerfilFacturador",
            text: "Facturador Electrónico",
            value: false,
            onValueChanged: function (data) {
                console.log("Datos_Obligado", data.value);
                Datos_Obligado = data.value;
            }
        }).dxValidator({
            validationRules: [
                {
                    type: 'custom', validationCallback: function (options) {
                        if (validar()) {
                            options.rule.message = "Debe Indicar si es Facturador o Adquiriente";
                            return false;
                        } else { return true; }
                    }
                }
            ]
        });

        $("#Adquiriente").dxCheckBox({
            name: "PerfilAdquiriente",
            text: "Adquiriente",
            value: false,
            onValueChanged: function (data) {
                console.log("Datos_Adquiriente", data.value);
                Datos_Adquiriente = data.value;
            }
        }).dxValidator({
            validationRules: [
                {
                    type: 'custom', validationCallback: function (options) {
                        if (validar()) {
                            options.rule.message = "Debe Indicar si es Facturador o Adquiriente";
                            return false;
                        } else { return true; }
                    }
                }
            ]
        });



        $("#Habilitacion").dxRadioGroup({
            searchEnabled: true,
            caption: 'Habilitación',
            dataSource: new DevExpress.data.ArrayStore({
                data: TiposHabilitacion,
                key: "ID"
            }),
            displayExpr: "Texto",
            Enabled: true,
            onValueChanged: function (data) {
                console.log("Datos_Habilitacion", data.value.ID);
                Datos_Habilitacion = data.value.ID;
            }
        }).dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe indicar el tipo de Habilitacion"
            }]
        });

        $("#txtobservaciones").dxTextArea({
            onValueChanged: function (data) {
                console.log("txtobservaciones", data.value);
                Datos_Observaciones = data.value.toUpperCase();
            }
        })
         .dxValidator({
             validationRules: [
             {
                 type: "stringLength",
                 max: 150,
                 message: "El campo Observación no puede ser mayor a 150 caracteres"
             }]
         });


        $("#button").dxButton({
            text: "Guardar",
            type: "default",
            useSubmitBehavior: true
        });


        $("#form1").on("submit", function (e) {
            guardarEmpresa();
            e.preventDefault();
        });

        function validar() {
            console.log("Datos_Adquiriente", Datos_Adquiriente);
            console.log("Datos_Obligado", Datos_Obligado);

            if (Datos_Adquiriente == true || Datos_Obligado == true) {
                console.log("Alguno de los dos es verdadero");
                return false;
            }
            return true;
        }

    });

    //Consultar por el id de seguridad para obtener los datos de la empresa a modificar
    var id_seguridad = location.search.split('IdSeguridad=')[1];
    if (id_seguridad) {
        Datos_Tipo = "2";
        $("#wait").show();
        $http.get('/api/Empresas?IdSeguridad=' + id_seguridad).then(function (response) {
            $("#wait").hide();
            try {

                Datos_Tipoidentificacion = response.data[0].TipoIdentificacion;
                Datos_Idententificacion = response.data[0].Identificacion;
                Datos_Razon_Social = response.data[0].RazonSocial;
                Datos_Email = response.data[0].Email;
                Datos_Adquiriente = (response.data[0].Intadquiriente) ? true : false;
                Datos_Obligado = (response.data[0].intObligado) ? true : false;
                Datos_Habilitacion = response.data[0].Habilitacion;
                Datos_IdentificacionDv = response.data[0].IntIdentificacionDv;
                Datos_Observaciones = response.data[0].StrObservaciones;

                Datos_empresa_Asociada = response.data[0].StrEmpresaAsociada;

                $("#Facturador").dxCheckBox({ value: Datos_Obligado });
                $("#Adquiriente").dxCheckBox({ value: Datos_Adquiriente });

                $("#NumeroIdentificacion").dxTextBox({ value: Datos_Idententificacion });
                $("#NumeroIdentificacion").dxTextBox({ readOnly: true });

                $("#txtRasonSocial").dxTextBox({ value: Datos_Razon_Social });
                $("#txtEmail").dxTextBox({ value: Datos_Email });

                $("#TipoIndentificacion").dxSelectBox({ value: TiposIdentificacion[BuscarID(TiposIdentificacion, Datos_Tipoidentificacion)] });
                $("#TipoIndentificacion").dxSelectBox({ readOnly: true });

                $("#Habilitacion").dxRadioGroup({ value: TiposHabilitacion[BuscarID(TiposHabilitacion, Datos_Habilitacion)] });

                $("#txtempresaasociada").dxTextBox({ value: (Datos_empresa_Asociada) ? Datos_empresa_Asociada : '' });
                if (Datos_Observaciones != null) {
                    $("#txtobservaciones").dxTextArea({ value: Datos_Observaciones });
                }


            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 7000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
        });

    }

    $scope.ButtonGuardar = {
        text: 'Guardar',
        type: 'default',
        validationGroup: "ValidacionDatosEmpresa",
        onClick: function (e) {
            guardarEmpresa();
        }
    };

    function guardarEmpresa() {
        var empresa = null;
        var Asociada ="";
        if (Datos_empresa_Asociada!=null){
            empresa = Datos_empresa_Asociada.split(' -- ');
            Asociada= empresa[0];
        } else {
            empresa = Datos_Idententificacion;
            Asociada=empresa;
        }

        var data = $.param({
            TipoIdentificacion: Datos_Tipoidentificacion,
            Identificacion: Datos_Idententificacion,
            RazonSocial: Datos_Razon_Social,
            Email: Datos_Email,
            Intadquiriente: (Datos_Adquiriente) ? true : false,
            IntObligado: (Datos_Obligado) ? true : false,
            IntHabilitacion: Datos_Habilitacion,
            StrEmpresaAsociada: Asociada,
            tipo: Datos_Tipo,
            StrObservaciones:Datos_Observaciones
        });

        $("#wait").show();
        $http.post('/api/Empresas?' + data).then(function (response) {
            $("#wait").hide();
            try {
                //Aqui se debe colocar los pasos a seguir
                DevExpress.ui.notify({ message: "Empresa Guardada con exito", position: { my: "center top", at: "center top" } }, "success", 6000);                
                $("#button").hide();
                $("#btncancelar").hide();
                setTimeout(IrAConsulta, 2000);
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
                       cssClass: "col-md-1 col-xs-2",
                       cellTemplate: function (container, options) {
                           $("<div style='text-align:center'>")
                               .append($("<a taget=_self class='icon-pencil3' title='Editar' href='GestionEmpresas.aspx?IdSeguridad=" + options.data.IdSeguridad + "'>"))
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
                       dataField: "Perfil"
                   }
                   //,
                   //{
                   //    dataField: "Habilitacion"
                   //}                   
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

//Esta funcion es para ir a la pagina de consulta
function IrAConsulta() {    
    window.location.assign("../Pages/ConsultaEmpresas.aspx");
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
        { ID: "0", Texto: 'VALIDA OBJETO' },
        { ID: "1", Texto: 'PRUEBAS' },
        { ID: "99", Texto: 'PRODCUCCIÓN' }
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
