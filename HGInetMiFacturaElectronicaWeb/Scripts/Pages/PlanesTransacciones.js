DevExpress.localization.locale(navigator.language);
var opc_pagina = "1334";
var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var GestionPlanesApp = angular.module('GestionPlanesApp', ['ModalEmpresasApp', 'dx', 'AppMaestrosEnum']);

//Controlador para la gestion planes transaccionales
GestionPlanesApp.controller('GestionPlanesController', function GestionPlanesController($scope, $http, $location, SrvMaestrosEnum) {

    var TiposProceso = [];
    var now = new Date();

    var StrIdSeguridad = location.search.split('IdSeguridad=')[1];

    var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";

    $http.get('/api/DatosSesion/').then(function (response) {
        codigo_facturador = response.data[0].Identificacion;
        var tipo = response.data[0].Admin;
        if (tipo) {
            $scope.Admin = true;
        } else {
            // $("#button").hide();
            // $('#SelecionarEmpresa').hide();
            // $("#txtempresaasociada").dxTextBox({ value: codigo_facturador + ' -- ' + response.data[0].RazonSocial });
        };

        //Obtiene el usuario autenticado.
        $http.get('/api/Usuario/').then(function (response) {
            //Obtiene el código del permiso.
            $http.get('/api/Permisos?codigo_usuario=' + response.data[0].CodigoUsuario + '&identificacion_empresa=' + codigo_facturador + '&codigo_opcion=' + opc_pagina).then(function (response) {
                $("#wait").hide();
                try {
                    var respuesta;

                    //Valida si el id_seguridad contiene datos
                    if (StrIdSeguridad)
                        respuesta = response.data[0].Editar;
                    else
                        respuesta = response.data[0].Agregar

                    //Valida la visibilidad del control según los permisos.
                    if (respuesta)
                        $('#button').show();
                    else
                        $('#button').hide();
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });
        });

    });

    var Datos_TiposProceso = "",
        codigo_empresa = "",
        Datos_T_compra = "",
        Datos_valor_plan = "",
        Datos_E_Plan = ""
    datos_empresa_asociada = "";
    Datos_obsrvaciones = "";


    //Define los campos del Formulario  
    $(function () {
        $("#summary").dxValidationSummary({});
        SrvMaestrosEnum.ObtenerEnum(3).then(function (data) {
            TiposProceso = data;
            //Selección Tipo de Proceso
            $("#TipoProceso").dxRadioGroup({
                searchEnabled: true,
                caption: 'TipoProceso',
                layout: "horizontal",
                dataSource: new DevExpress.data.ArrayStore({
                    data: TiposProceso,
                    key: "ID"
                }),
                displayExpr: "Descripcion",
                Enabled: true,
                onValueChanged: function (data) {
                    Datos_TiposProceso = data.value.ID;
                    //Post-Pago
                    if (Datos_TiposProceso == 3) {
                        $('#divValorPlan').hide();
                        $('#divCantTransacciones').hide();
                        $("#CantidadTransacciones").dxNumberBox({ value: 0 });
                        $("#ValorPlan").dxNumberBox({ value: 0 });
                        //Datos_T_compra = "0";
                        //Datos_valor_plan = "0";
                        ValidarCantTransacciones();
                    }
                    //Compra
                    if (Datos_TiposProceso == 2) {
                        $('#divValorPlan').show();
                        $('#divCantTransacciones').show();
                        //Datos_T_compra = "";
                        //Datos_valor_plan = "";
                        ValidarCantTransacciones();
                    }
                    //Cortesía
                    if (Datos_TiposProceso == 1) {
                        $('#divValorPlan').hide();
                        $('#divCantTransacciones').show();
                        $("#ValorPlan").dxNumberBox({ value: 0 });
                        //Datos_T_compra = "";
                        //Datos_valor_plan = "0";
                        ValidarCantTransacciones();
                    }
                }
            }
            ).dxValidator({
                validationRules: [{
                    type: "required",
                    message: "Debe indicar el tipo de proceso."
                }]
            });



            if (StrIdSeguridad != '' && StrIdSeguridad != null) {
                Consultar(StrIdSeguridad);
            }
        });

        function ValidarCantTransacciones() {
            $("#CantidadTransacciones").dxValidator({
                validationRules: [{
                    type: 'custom', validationCallback: function (options) {
                        if ((Datos_TiposProceso == 1 || Datos_TiposProceso == 2) && Datos_T_compra < 1) {
                            options.rule.message = "Debe Indicar la cantidad de transacciones";
                            return false;
                        } else {
                            return true;
                        }
                    }
                }]
            });

            $("#ValorPlan").dxValidator({
                validationRules: [{
                    type: 'custom', validationCallback: function (options) {
                        if ((Datos_TiposProceso == 2) && Datos_valor_plan < 1) {
                            options.rule.message = "Debe Indicar el valor de las transacciones";
                            return false;
                        } else {
                            return true;
                        }
                    }
                }]
            });

        }

        //Campo de selección de empresa.
        $("#txtempresaasociada").dxTextBox({
            readOnly: true,
            value: codigo_empresa,
            name: txtempresaasociada,
            onValueChanged: function (data) {
                datos_empresa_asociada = data.value;
            },
            onFocusIn: function (data) {
                //if ($scope.Admin && StrIdSeguridad == undefined)
                $('#modal_Buscar_empresa').modal('show');
            }
        }).dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe seleccionar una empresa."
            }]
        });

        //Campo cantidad de transacciones del plan
        $("#CantidadTransacciones").dxNumberBox({
            format: "#,##0",
            validationGroup: "ValidacionDatosPlan",
            onValueChanged: function (data) {
                Datos_T_compra = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "stringLength",
                max: 10,
                message: "El número de transacciones no puede mayor a 10 digitos"
            }, {
                type: "required",
                message: "Debe indicar la cantidad de transacciones del plan."
            }, {
                type: "numeric",
                message: "El campo sólo debe contener números."
            }, {
                type: 'pattern',
                pattern: '^[0-9]+$',
                message: 'No debe Incluir puntos(.) ni caracteres especiales'
            },
            {
                type: 'custom', validationCallback: function (options) {
                    if ((Datos_TiposProceso == 1 || Datos_TiposProceso == 2) && Datos_T_compra < 1) {
                        options.rule.message = "Debe Indicar la cantidad de transacciones";
                        return false;
                    } else {
                        return true;
                    }
                }
            }

            ]
        });

        /*
         .dxValidator({
            validationRules: [{
                type: 'custom', validationCallback: function (options) {
                    if ((Datos_empresa != Datos_usuario) && Datos_apellidos == '') {
                        options.rule.message = "Debe Indicar el apellido";
                        return false;
                    } else {
                        return true;
                    }
                }
            }, {
                type: 'stringLength',
                max: 50,
                message: "El apellido no puede ser mayor a 50 caracteres"
            }]
        });
        */

        //Campo Valor plan
        $("#ValorPlan").dxNumberBox({
            format: "$ #,##0",
            onValueChanged: function (data) {
                Datos_valor_plan = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe indicar el valor del plan."
            }, {
                type: "numeric",
                message: "El campo sólo debe contener números."
            }, {
                type: 'pattern',
                pattern: '^[0-9]+$',
                message: 'No debe Incluir puntos(.) ni caracteres especiales'
            }]
        });


        //Selección Estado de plan
        $("#EstadoPlan").dxRadioGroup({
            searchEnabled: true,
            caption: 'EstadoPlan',
            layout: "horizontal",
            dataSource: new DevExpress.data.ArrayStore({
                data: EstadosPlanes,
                key: "ID"
            }),
            displayExpr: "Texto",
            Enabled: true,
            onValueChanged: function (data) {
                Datos_E_Plan = (data.value.ID > 0) ? 'true' : 'false';
            }
        }).dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe indicar el estado del plan."
            }]
        });

        //Observaciones
        $("#txtObservaciones").dxTextArea({
            onValueChanged: function (data) {
                Datos_obsrvaciones = data.value.toUpperCase();
            }
        })
        .dxValidator({
            validationRules: [{
                type: "stringLength",
                max: 200,
                message: "El campo Observaciones no puede tener mas de 200 caracteres."
            }]
        });

        $("#button").dxButton({
            text: "Guardar",
            type: "default",
            useSubmitBehavior: true
        });


        $("#form1").on("submit", function (e) {
            GuardarPlan();
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

        if (Datos_T_compra == "") { Datos_T_compra = "0" }

        if (Datos_valor_plan == "") { Datos_valor_plan = "0" }

        var data = $.param({
            IntTipoProceso: Datos_TiposProceso,
            StrEmpresa: codigo_facturador,
            StrUsuario: $('#LblCodigoUsuario').text(),
            IntNumTransaccCompra: Datos_T_compra,
            IntNumTransaccProcesadas: 0,
            IntValor: Datos_valor_plan,
            BitProcesada: Datos_E_Plan,
            StrObservaciones: Datos_obsrvaciones,
            StrEmpresaFacturador: empresa[0]
        });

        var IdActualizar = (StrIdSeguridad) ? '&' + $.param({ StrIdSeguridad: StrIdSeguridad }) : '';

        $("#wait").show();
        $http.post('/api/PlanesTransacciones?' + data + IdActualizar).then(function (response) {
            $("#wait").hide();
            try {
                DevExpress.ui.notify({ message: "El plan ha sido registrado con exito.", position: { my: "center top", at: "center top" } }, "success", 1500);
                $("#button").hide();
                $("#btncancelar").hide();
                setTimeout(IrAConsulta, 1000);
            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 3000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });
    }


    //Consultar por el id de seguridad para obtener los datos de la empresa a modificar
   

    function Consultar(StrIdSeguridad) {
        $("#wait").show();
        $http.get('/api/PlanesTransacciones?IdSeguridad=' + StrIdSeguridad).then(function (response) {
            $("#wait").hide();
            try {
                Datos_TiposProceso = response.data[0].Tipo;
                codigo_empresa = response.data[0].CodigoEmpresaFacturador;
                Datos_T_compra = response.data[0].TCompra;
                Datos_T_Procesadas = response.data[0].TProcesadas;
                Datos_valor_plan = response.data[0].Valor;
                Datos_obsrvaciones = response.data[0].Observaciones;
                Datos_E_Plan = response.data[0].Estado;
                Datos_Nombre_facturador = response.data[0].EmpresaFacturador;

                if (Datos_TiposProceso == 1 || Datos_TiposProceso == 2 || Datos_TiposProceso == 3) {
                    $("#TipoProceso").dxRadioGroup({ value: TiposProceso[BuscarID(TiposProceso, Datos_TiposProceso)] });
                }

                $("#txtempresaasociada").dxTextBox({ value: codigo_empresa + ' -- ' + Datos_Nombre_facturador });

                $("#CantidadTransacciones").dxNumberBox({ value: Datos_T_compra });

                $("#ValorPlan").dxNumberBox({ value: Datos_valor_plan });

                $("#EstadoPlan").dxRadioGroup({ value: EstadosPlanes[BuscarID(EstadosPlanes, Datos_E_Plan)] });

                if (Datos_obsrvaciones != null) {
                    $("#txtObservaciones").dxTextArea({ value: Datos_obsrvaciones });
                }

                $("#CantidadTransacciones").dxNumberBox({ readOnly: true });
                $("#ValorPlan").dxNumberBox({ readOnly: true });
                $('#SelecionarEmpresa').hide();
                $("#TipoProceso").dxRadioGroup({ readOnly: true });

            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 7000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
        });
    }

});

GestionPlanesApp.controller('ConsultaPlanesController', function ConsultaPlanesController($scope, $http, $location) {
    var now = new Date();

    var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "";

    var estado = "";

    CargarSession();

    function CargarSession() {
        $http.get('/api/DatosSesion/').then(function (response) {
            codigo_facturador = response.data[0].Identificacion;
            CargarConsulta();
        }, function errorCallback(response) {

            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
        });
    }
    function CargarConsulta() {
        $("#wait").show();
        $http.get('/api/PlanesTransacciones?Identificacion=' + codigo_facturador).then(function (response) {
            $("#wait").hide();
            try {
                $("#grid").dxDataGrid({
                    dataSource: response.data,
                    paging: {
                        pageSize: 20

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
                    masterDetail: {
                        enabled: true,
                        template: function (container, options) {
                            var currentEmployeeData = options.data.Observaciones;
                            container.append($('<div> <h4 class="form-control">OBSERVACIONES:</h4> <p > ' + currentEmployeeData + '</p> </div>'));
                        }
                    },


                    //Formatos personalizados a las columnas en este caso para el monto
                    onCellPrepared: function (options) {
                        var fieldData = options.value,
                            fieldHtml = "";
                        try {
                            if (options.columnIndex == 5) {//Columna de valor Total
                                if (fieldData) {
                                    var inicial = fNumber.go(fieldData);
                                    options.cellElement.html(inicial);
                                }
                            }
                            if (options.columnIndex == 4 || options.columnIndex == 7) {
                                if (fieldData) {
                                    var inicial = FormatoNumber.go(fieldData);
                                    options.cellElement.html(inicial);
                                }
                            }

                            if (options.data.Estado == 'Habilitado') {
                                estado = " style='color:green; cursor:default;' title='Habilitado'";
                            } else {
                                estado = " style='color:red; cursor:default;' title='Inhabilitado'";
                            }

                        } catch (err) {

                        }
                    }
                    , loadPanel: {
                        enabled: true
                    }
                      , allowColumnResizing: true
                 , columns: [
                     {
                         cssClass: "col-md-1 col-xs-1",
                         width: 50,
                         cellTemplate: function (container, options) {
                             $("<div style='text-align:center'>")
                                 .append($("<a taget=_self class='icon-pencil3' title='Editar' href='GestionPlanesTransacciones.aspx?IdSeguridad=" + options.data.id + "'>"))
                                 .appendTo(container);
                         }
                     },
                     {
                         cssClass: "col-md-2 col-xs-4",
                         caption: "Fecha",
                         dataField: "Fecha",
                         dataType: "date",
                         format: "yyyy-MM-dd HH:mm"
                     },
                      {
                          cssClass: "col-md-2 col-xs-3",
                          caption: "Empresa Compra",
                          dataField: "EmpresaFacturador"
                      },
                     {
                         cssClass: "col-md-1 col-xs-1",
                         caption: "Transacciones",
                         dataField: "TCompra"
                     },
                      {
                          cssClass: "col-md-1 col-xs-1",
                          caption: "Valor",
                          dataField: "Valor"
                      },
                     {
                         cssClass: "col-md-1 col-xs-2",
                         caption: "Procesadas",
                         dataField: "TProcesadas"
                     }
                     ,
                     {
                         cssClass: "col-md-1 col-xs-2",
                         caption: "Saldo",
                         dataField: "Saldo"
                     }
                      , {
                          cssClass: "col-md-2 hidden-xs",
                          caption: "Empresa",
                          dataField: "Empresa",

                      },
                     {
                         cssClass: "col-md-2 hidden-xs",
                         caption: "Usuario",
                         dataField: "Usuario"
                     }
                     ,
                     {
                         cssClass: "col-md-2",
                         caption: "Tipo",
                         dataField: "Tipoproceso"
                     }

                     ,
                      {
                          cssClass: "col-md-1 col-xs-1",
                          caption: 'Estado',
                          dataField: 'Estado',
                          cellTemplate: function (container, options) {
                              $("<div style='text-align:center'>")
                                  .append($("<a taget=_self class='icon-circle2'" + estado + ">"))
                                  .appendTo(container);
                          }
                      }
                 ], summary: {
                     groupItems: [{
                         column: "Valor",
                         summaryType: "sum",
                         displayFormat: " {0} Total ",
                         valueFormat: "currency"
                     }]

                    , totalItems: [{
                        column: "Valor",
                        displayFormat: "{0}",
                        valueFormat: "currency",
                        summaryType: "sum"
                    }, {
                        column: "TCompra",
                        summaryType: "sum",
                        valueFormat: 'fixedPoint',
                        displayFormat: '{0}'

                    }
                    ]
                 }
                    , filterRow: {
                        visible: true
                    }
                });


            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 7000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
        });
    }
});

//Esta funcion es para ir a la pagina de consulta
function IrAConsulta() {
    window.location.assign("../Pages/ConsultaPlanesTransacciones.aspx");
}

//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
    for (var i = 0; i < miArray.length; i += 1) {
        if (ID == miArray[i].ID) {
            return i;
        }
    }
}
/*
var TiposProceso =
    [
        { ID: "1", Texto: 'Cortesía' },
        { ID: "2", Texto: 'Compra' },
        { ID: "3", Texto: 'Post-Pago' },
    ];
    */
var EstadosPlanes =
    [
        { ID: "0", Texto: 'Inhabilitar' },
        { ID: "1", Texto: 'Habilitar' },

    ];
