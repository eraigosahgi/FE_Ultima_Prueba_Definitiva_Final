DevExpress.localization.locale(navigator.language);
var opc_pagina = "1331";
var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var ConsultaUsuarioApp = angular.module('ConsultaUsuarioApp', ['dx', 'ModalEmpresasApp']);

var OpcionesUsuario = [];

var PermisosUsuario = [];

var ValidacionHP = false;


ConsultaUsuarioApp.controller('ConsultaUsuarioController', function ConsultaUsuarioController($scope, $http, $location) {


    var codigo_usuario = "",
        codigo_facturador = "";

    $http.get('/api/DatosSesion/').then(function (response) {

        codigo_facturador = response.data[0].Identificacion;

        consultar();
    }), function errorCallback(response) {
        Mensaje(response.data.ExceptionMessage, "error");
    };

    function consultar() {

        //Obtiene los datos del web api
        //ControladorApi: /Api/Usuario/
        //Datos GET: string codigo_usuario, string codigo_empresa
        $('#wait').show();
        $http.get('/api/Usuario?codigo_usuario=' + "*" + '&codigo_empresa=' + codigo_facturador).then(function (response) {
            $('#wait').hide();
            $("#gridUsuarios").dxDataGrid({
                dataSource: response.data,
                paging: {
                    pageSize: 20
                },
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                },
                loadPanel: {
                    enabled: true
                },  //Formatos personalizados a las columnas en este caso para el monto
                onCellPrepared: function (options) {
                    var fieldData = options.value,
                        fieldHtml = "";
                    try {
                        if (options.data.Estado == 1) {                            
                            estado = " style='color:green; cursor:default;' title='Activo'";
                        } else {
                            estado = " style='color:red; cursor:default;' title='Inactivo'";
                        }

                    } catch (err) {

                    }

                }, headerFilter: {
                    visible: true
                },
                groupPanel: {
                    allowColumnDragging: true,
                    visible: true
                }
            , allowColumnResizing: true,
                columns: [
                     {
                         caption: "",
                         cellTemplate: function (container, options) {
                             $("<div>")
                                 .append(
                                     $("<a class='icon-mail-read' style='margin-right:12%; font-size:19px'></a> &nbsp;&nbsp;").dxButton({
                                         onClick: function () {
                                             $http.get('/api/Usuario?codigo_usuario=' + options.data.Usuario).then(function (responseEnvio) {

                                                 var respuesta = responseEnvio.data;

                                                 if (respuesta) {
                                                     swal({
                                                         title: 'Proceso Éxitoso',
                                                         text: 'El e-mail ha sido enviado con éxito.',
                                                         type: 'success',
                                                         confirmButtonColor: '#66BB6A',
                                                         confirmButtonTex: 'Aceptar',
                                                         animation: 'pop',
                                                         html: true,
                                                     });
                                                 } else {
                                                     swal({
                                                         title: 'Error',
                                                         text: 'Ocurrió un error en el envío del e-mail.',
                                                         type: 'Error',
                                                         confirmButtonColor: '#66BB6A',
                                                         confirmButtonTex: 'Aceptar',
                                                         animation: 'pop',
                                                         html: true,
                                                     });
                                                 }

                                             }, function errorCallback(response) {
                                                 DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 10000);
                                             });
                                         }
                                     }).removeClass("dx-button dx-button-normal dx-widget")
                             )
                                 .append($("<a taget=_self class='icon-pencil3' style='margin-right:-20%' title='Editar' href='GestionUsuarios.aspx?IdSeguridad=" + options.data.IdSeguridad + "'>"))
                                 .append($(""))
                                 .appendTo(container);
                         }
                     },
                         {
                             caption: "Identificación Empresa",
                             dataField: "Empresa",
                         },
                         {
                             caption: "Nombre Empresa",
                             dataField: "RazonSocial",
                         },
                         {
                             caption: "Código Usuario",
                             dataField: "Usuario",
                         },
                         {
                             caption: "Nombres",
                             dataField: "NombreCompleto",
                         },
                         {
                             caption: "Email",
                             dataField: "Mail"
                         }, {
                             cssClass: "col-md-1 col-xs-1",
                             caption: 'Estado',
                             dataField: 'Estado',
                             cellTemplate: function (container, options) {
                                 $("<div style='text-align:center; cursor:default;'>")
                                     .append($("<a taget=_self class='icon-circle2'" + estado + ">"))
                                     .appendTo(container);
                             }
                         }
                ], summary: {
                    groupItems: [{
                        column: "Empresa",
                        summaryType: "count",
                        displayFormat: " {0} Usuario(s) ",
                        valueFormat: "fixedPoint"
                    }]

                },
                filterRow: {
                    visible: true
                },
            });
        }), function errorCallback(response) {
            $('#wait').hide();
            Mensaje(response.data.ExceptionMessage, "error");
        };
    }
});


ConsultaUsuarioApp.controller('GestionUsuarioController', function GestionUsuarioController($scope, $http, $location) {

    var now = new Date();
    var codigo_facturador = "",
           numero_documento = "",
           estado_dian = "",
           estado_recibo = "",
           fecha_inicio = "",
           fecha_fin = "",
           codigo_adquiriente = "",
           codigo_usuario_sesion = "";

    $http.get('/api/DatosSesion/').then(function (response) {

        codigo_facturador = response.data[0].Identificacion;
        var tipo = response.data[0].Admin;
        if (tipo) {
            $scope.Admin = true;
        } else {
            $('#SelecionarEmpresa').hide();
            $("#txtempresaasociada").dxTextBox({ value: codigo_facturador + ' -- ' + response.data[0].RazonSocial });
        };

        //Obtiene el usuario autenticado.
        $http.get('/api/Usuario/').then(function (response) {
            //Obtiene el código del permiso.
            $http.get('/api/Permisos?codigo_usuario=' + response.data[0].CodigoUsuario + '&identificacion_empresa=' + codigo_facturador + '&codigo_opcion=' + opc_pagina).then(function (response) {
                $("#wait").hide();
                try {
                    var respuesta;

                    //Valida si el id_seguridad contiene datos
                    if (id_seguridad)
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

    var Datos_nombres = "",
        Datos_apellidos = "",
        Datos_usuario = "",
        Datos_telefono = "",
        Datos_extension = "",
        Datos_celular = ""
    Datos_email = "",
    Datos_cargo = "",
    Datos_empresa = "",
    Datos_estado = "",
    Datos_Tipo = "1";

    //Define los campos del Formulario  
    $(function () {
        $("#summary").dxValidationSummary({});

        $("#txtnombres").dxTextBox({
            onValueChanged: function (data) {
                Datos_nombres = data.value.toUpperCase();
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe Indicar el Nombre"
            }, {
                type: "stringLength",
                max: 255,
                message: "El Nombre no puede ser mayor a 255 caracteres"
            }]
        });


        $("#txtapellidos").dxTextBox({
            onValueChanged: function (data) {
                Datos_apellidos = data.value.toUpperCase();
            }
        })
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

        $("#txtusuario").dxTextBox({
            onValueChanged: function (data) {
                Datos_usuario = data.value.toUpperCase();
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir el Usuario"
            }, {
                type: "stringLength",
                max: 20,
                message: "El Usuario no puede ser mayor a 20 caracteres"
            }]
        });

        $("#txttelefono").dxTextBox({
            onValueChanged: function (data) {
                Datos_telefono = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: 'custom', validationCallback: function (options) {
                    if ((Datos_empresa != Datos_usuario) && Datos_telefono == '') {
                        options.rule.message = "Debe introducir el Teléfono";
                        return false;
                    } else {
                        return true;
                    }
                }
            }, {
                type: "stringLength",
                max: 50,
                message: "El Telefono no puede ser mayor a 50 caracteres"
            }]
        });

        $("#txtextension").dxTextBox({
            onValueChanged: function (data) {
                Datos_extension = data.value;
            }
        })
        .dxValidator({
            validationRules: [
            {
                type: "stringLength",
                max: 10,
                message: "La extenasión no puede ser mayor a 10 caracteres"
            }]
        });

        $("#txtcelular").dxTextBox({
            onValueChanged: function (data) {
                Datos_celular = data.value;
            }
        })
        .dxValidator({
            validationRules: [
            {
                type: "stringLength",
                max: 50,
                message: "El Celular no puede ser mayor a 50 caracteres"
            }]
        });

        $("#txtemail").dxTextBox({
            onValueChanged: function (data) {
                Datos_email = data.value;
            },
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir el Email"
            }, {
                type: "stringLength",
                max: 200,
                message: "El email no puede ser mayor a 200 caracteres"
            }, {
                type: "email",
                message: "El email no tiene el formato correcto"
            }]
        });

        $("#txtcargo").dxTextBox({
            onValueChanged: function (data) {
                Datos_cargo = data.value.toUpperCase();
            }
        })
       .dxValidator({
           validationRules: [
           {
               type: "stringLength",
               max: 50,
               message: "El Cargo no puede ser mayor a 50 caracteres"
           }]
       });

        $("#txtempresaasociada").dxTextBox({
            readOnly: true,
            onValueChanged: function (data) {
                Datos_empresa = data.value;
            },
            onFocusIn: function (data) {
                if ($scope.Admin)
                    $('#modal_Buscar_empresa').modal('show');
            }
        })
         .dxValidator({
             validationRules: [{
                 type: "required",
                 message: "Debe indicar cual es la Empresa Asociada"
             }]
         });


        $("#cboestado").dxSelectBox({
            placeholder: "Seleccione el Estado",
            displayExpr: "Texto",
            dataSource: TiposEstado,
            onValueChanged: function (data) {
                Datos_estado = data.value.ID;
            }
        }).dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe seleccionar el Estado"
            }]
        });




        $("#button").dxButton({
            text: "Guardar",
            type: "default",
            useSubmitBehavior: true
        });


        $("#form1").on("submit", function (e) {
            guardarUsuario();
            e.preventDefault();
        });


    });

    //Consultar por el id de seguridad para obtener los datos del Usuario
    var id_seguridad = location.search.split('IdSeguridad=')[1];

    if (id_seguridad) {
        var parametros = $.param({
            StrIdSeguridad: id_seguridad,
            Consulta: 1
        });

        Datos_Tipo = "2";
        $("#wait").show();
        $http.get('/api/Usuario?' + parametros).then(function (response) {
            $("#wait").hide();
            try {

                Datos_nombres = response.data[0].StrNombres;
                Datos_apellidos = response.data[0].StrApellidos;
                Datos_usuario = response.data[0].StrUsuario;
                Datos_telefono = response.data[0].StrTelefono;
                Datos_extension = response.data[0].StrExtension;
                Datos_celular = response.data[0].StrCelular;
                Datos_email = response.data[0].StrMail;
                Datos_cargo = response.data[0].StrCargo;
                Datos_empresa = response.data[0].StrEmpresa;
                Datos_estado = response.data[0].IntIdEstado;

                $("#txtnombres").dxTextBox({ value: Datos_nombres });
                $("#txtapellidos").dxTextBox({ value: Datos_apellidos });
                $("#txtusuario").dxTextBox({ value: Datos_usuario });
                $("#txtusuario").dxTextBox({ readOnly: true });
                $("#txttelefono").dxTextBox({ value: (Datos_telefono) ? Datos_telefono : '' });
                $("#txtextension").dxTextBox({ value: (Datos_extension) ? Datos_extension : '' });
                $("#txtcelular").dxTextBox({ value: (Datos_celular) ? Datos_celular : '' });
                $("#txtemail").dxTextBox({ value: (Datos_email) ? Datos_email : '' });
                $("#txtcargo").dxTextBox({ value: (Datos_cargo) ? Datos_cargo : '' });
                $("#txtempresaasociada").dxTextBox({ value: (Datos_empresa) ? Datos_empresa : '' });
                $("#txtempresaasociada").dxTextBox({ readOnly: true });
                $("#cboestado").dxSelectBox({ value: TiposEstado[BuscarID(TiposEstado, Datos_estado)] });
                $('#SelecionarEmpresa').hide();

                var empresa = Datos_empresa.split(' -- ');

                $http.get('/api/Usuario/').then(function (response) {
                    //string usuario_autenticado, string empresa_autenticada, string codigo_usuario, string identificacion_empresa
                    $("#wait").show();
                    $http.get('/api/Permisos?usuario_autenticado=' + response.data[0].CodigoUsuario + '&empresa_autenticada=' + codigo_facturador + '&codigo_usuario=' + Datos_usuario + '&identificacion_empresa=' + empresa[0]).then(function (response) {
                        $("#wait").hide();
                        try {

                            PermisosUsuario = response.data;

                            var data = {};
                            //Recorre la información retornada y la añade a la lista de permisos para almacenar en bd
                            for (i = 0; i < PermisosUsuario.length; i++) {

                                if (PermisosUsuario[i].Consultar || PermisosUsuario[i].Agregar || PermisosUsuario[i].Editar || PermisosUsuario[i].Eliminar || PermisosUsuario[i].Anular || PermisosUsuario[i].Gestion) {

                                    var data = {};
                                    data = { IntIdOpcion: PermisosUsuario[i].Codigo, IntConsultar: PermisosUsuario[i].Consultar, IntAgregar: PermisosUsuario[i].Agregar, IntEditar: PermisosUsuario[i].Editar, IntEliminar: PermisosUsuario[i].Eliminar, IntAnular: PermisosUsuario[i].Anular, IntGestion: PermisosUsuario[i].Gestion };
                                    OpcionesUsuario.push(data);
                                }
                            }
                            GenerarTreeList(response.data);

                        } catch (err) {
                            DevExpress.ui.notify(err.message, 'error', 3000);
                        }
                    }, function errorCallback(response) {
                        $('#wait').hide();
                        DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
                    });
                });


                if (Datos_empresa == Datos_usuario) {
                    $('#lblapellido').text('Apellidos:');
                    $('#lbltelefono').text('Teléfono:');
                }

            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 7000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 7000);
        });

    }

    if (id_seguridad == null) {
        $http.get('/api/Usuario/').then(function (response) {
            console.log("Datos para crear Treelist: ", response.data);
            $("#wait").show();
            $http.get('/api/Permisos?codigo_usuario=' + response.data[0].CodigoUsuario + '&codigo_empresa=' + response.data[0].Empresa).then(function (response) {
                $("#wait").hide();
                try {

                    GenerarTreeList(response.data);

                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });


        });


    };

    $scope.ButtonGuardar = {
        text: 'Guardar',
        type: 'default',
        validationGroup: "ValidacionDatosEmpresa",
        onClick: function (e) {
            guardarUsuario();
        }
    };

    function guardarUsuario() {
        var empresa = Datos_empresa.split(' -- ');

        var data = $.param({
            StrEmpresa: empresa[0],
            StrUsuario: Datos_usuario,
            StrNombres: Datos_nombres,
            StrApellidos: Datos_apellidos,
            StrMail: Datos_email,
            StrTelefono: Datos_telefono,
            StrExtension: Datos_extension,
            StrCelular: Datos_celular,
            StrCargo: Datos_cargo,
            IntIdEstado: Datos_estado,
            Tipo: Datos_Tipo
        });

        $("#wait").show();
        $http.post('/api/Usuario?' + data).then(function (response) {
            $("#wait").hide();
            try {

                guardarPermisos();

                DevExpress.ui.notify({ message: "Usuario Guardado con exito", position: { my: "center top", at: "center top" } }, "success", 1500);
                $("#button").hide();
                $("#btncancelar").hide();
                setTimeout(IrAConsulta, 3000);

            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 3000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response, 'error', 6000);
        });
    }

    //chkOculto = nomenclatura inicial para los opcion hidden
    //chkVisible = nomenclatura inicial para los opcion que estan visibles
    function ValidarConsultar(Nombre, chkOculto, chkVisible) {
        var menu = 6;
        if (ValidacionHP == false) {
            var optActual = $("input[name='" + Nombre + "']").val();
            try {
                ValidacionHP = true;
                //Valido si estoy desmarcando la opción
                if (optActual == "false" || optActual == 0) {
                    var chkopcion = Nombre.replace(chkOculto, chkVisible);
                    //Recorro y borro todos mis hijos
                    for (var i = 1; i < menu; i++) {
                        console.log("opcion: ", "#" + Nombre.replace(chkOculto, chkVisible) + i);
                        $("#" + chkopcion + i).dxCheckBox({ value: 0 });
                        var hijo = chkopcion + i;
                        //Valido si tengo mas hijos
                        for (var j = 1; j < menu; j++) {
                            $("#" + hijo + j).dxCheckBox({ value: 0 });
                        }
                    }

                    ///Valido si desmarco a mi padre  
                    //Padre
                    var padre = chkopcion.substr(0, chkopcion.length - 1);
                    padre = padre.replace(chkVisible, chkOculto)
                    var otroHijoMarcado = false;
                    for (var i = 1; i < menu; i++) {
                        var otroHijo = $("input[name='" + padre + i + "']").val();
                        if (otroHijo == "true" || otroHijo == 1) {
                            otroHijoMarcado = true;
                            break;
                        }
                    }

                    if (!otroHijoMarcado) {
                        padre = padre.replace(chkOculto, chkVisible);
                        $("#" + padre).dxCheckBox({ value: 0 });
                    }
                    ////////////////////////Fin validación padre

                    otroHijoMarcado = false;
                    ////////////////////////////Entra validacion padre de mi padre
                    ///Valido si desmarco a mi padre  
                    //Padre de mi padre
                    var padre = padre.substr(0, padre.length - 1);
                    padre = padre.replace(chkVisible, chkOculto)
                    var otroHijoMarcado = false;
                    for (var i = 1; i < menu; i++) {
                        var otroHijo = $("input[name='" + padre + i + "']").val();
                        if (otroHijo == "true" || otroHijo == 1) {
                            otroHijoMarcado = true;
                            break;
                        }
                    }

                    if (!otroHijoMarcado) {
                        padre = padre.replace(chkOculto, chkVisible);
                        $("#" + padre).dxCheckBox({ value: 0 });
                    }
                    ////////////////////////////////////////Finvalidacion padre de mi padre

                    ValidacionHP = false;
                } else {
                    ValidacionHP = true;
                    var chkopcion = Nombre.replace(chkOculto, chkVisible);
                    //Debo Marcar al padre ya que estoy marcando al Hijo                                        


                    //Marco todos los Hijos
                    for (var i = 1; i < menu; i++) {
                        $("#" + chkopcion + i).dxCheckBox({ value: 1 });
                        var hijo = chkopcion + i;
                        //Valido si tengo mas hijos
                        for (var j = 1; j < menu; j++) {
                            $("#" + hijo + j).dxCheckBox({ value: 1 });
                        }
                    }
                    //Marco a Mi padre
                    var padre = chkopcion.substr(0, chkopcion.length - 1);
                    $("#" + padre).dxCheckBox({ value: 1 });

                    //Marco al padre de mi padre
                    var padrePadre = padre.substr(0, padre.length - 1);
                    $("#" + padrePadre).dxCheckBox({ value: 1 });

                    ValidacionHP = false;
                }

            } catch (err) {

            }
        }
    }




    //Construye el TreeList
    function GenerarTreeList(data) {
        $("#treeListOpcionesPermisos").dxTreeList({
            dataSource: data,
            keyExpr: "Codigo",
            parentIdExpr: "Dependencia",
            showRowLines: false,
            columnAutoWidth: true,
            useSubmitBehavior: false,
            autoExpandAll: true,
            cacheEnabled: true,
            columns: [
                {
                    dataField: "Descripcion",
                    caption: "Descripción",
                },
        {
            caption: "Consultar",
            cssClass: "col-xs-3 col-md-1",
            cellTemplate: function (container, options) {
                $("<div  id='Consultar_" + options.data.Codigo + "'>")
                    .append($("<input type='checkbox' ng-model='fieldView' ng-checked='checked' ng-disabled='checked'>")).dxCheckBox({
                        value: PermisosActuales(options.data.Codigo, 1),
                        name: 'IConsultar_' + options.data.Codigo,
                        onValueChanged: function (e) {
                            ValidarConsultar("IConsultar_" + options.data.Codigo, "IConsultar", "Consultar");
                            IncluirItem(e, 1, options.data.Codigo);
                        }
                    }).removeClass("dx-button dx-button-normal dx-widget")
                    .append($(""))
                  .appendTo(container);
            }
        },
    {
        caption: "Crear",
        cssClass: "col-xs-3 col-md-1",
        cellTemplate: function (container, options) {
            $("<div  id='Crear_" + options.data.Codigo + "'>")
                .append($("<input type='checkbox' ng-model='fieldView' ng-checked='checked' ng-disabled='checked'>")).dxCheckBox({
                    value: PermisosActuales(options.data.Codigo, 2),
                    name: 'ICrear_' + options.data.Codigo,
                    onValueChanged: function (e) {
                        ValidarConsultar("ICrear_" + options.data.Codigo, "ICrear", "Crear");
                        IncluirItem(e, 2, options.data.Codigo);
                    }
                }).removeClass("dx-button dx-button-normal dx-widget")
                .append($(""))
              .appendTo(container);
        }
    },
                    {
                        caption: "Editar",
                        cssClass: "col-xs-3 col-md-1",
                        cellTemplate: function (container, options) {
                            $("<div  id='Editar_" + options.data.Codigo + "'>")
                                .append($("<input type='checkbox' ng-model='fieldView' ng-checked='checked' ng-disabled='checked'>")).dxCheckBox({
                                    value: PermisosActuales(options.data.Codigo, 3),
                                    name: 'IEditar_' + options.data.Codigo,
                                    onValueChanged: function (e) {
                                        ValidarConsultar("IEditar_" + options.data.Codigo, "IEditar", "Editar");
                                        IncluirItem(e, 3, options.data.Codigo);
                                    }
                                }).removeClass("dx-button dx-button-normal dx-widget")
                                .append($(""))
                              .appendTo(container);
                        }
                    },
        {
            caption: "Eliminar",
            cssClass: "col-xs-3 col-md-1",
            cellTemplate: function (container, options) {
                $("<div  id='Eliminar_" + options.data.Codigo + "'>")
            .append($("<input type='checkbox' ng-model='fieldView' ng-checked='checked' ng-disabled='checked'>")).dxCheckBox({
                value: PermisosActuales(options.data.Codigo, 4),
                name: 'IEliminar_' + options.data.Codigo,
                onValueChanged: function (e) {
                    ValidarConsultar("IEliminar_" + options.data.Codigo, "IEliminar", "Eliminar");
                    IncluirItem(e, 4, options.data.Codigo);
                }
            }).removeClass("dx-button dx-button-normal dx-widget")
            .append($(""))
          .appendTo(container);
            }
        },
    {
        caption: "Anular",
        cssClass: "col-xs-3 col-md-1",
        cellTemplate: function (container, options) {
            $("<div  id='Anular_" + options.data.Codigo + "'>")
                .append($("<input type='checkbox' ng-model='fieldView' ng-checked='checked' ng-disabled='checked'>")).dxCheckBox({
                    value: PermisosActuales(options.data.Codigo, 5),
                    name: 'IAnular_' + options.data.Codigo,
                    onValueChanged: function (e) {
                        ValidarConsultar("IAnular_" + options.data.Codigo, "IAnular", "Anular");
                        IncluirItem(e, 5, options.data.Codigo);
                    }
                }).removeClass("dx-button dx-button-normal dx-widget")
                .append($(""))
              .appendTo(container);
        }
    },
    {
        caption: "Gestión",
        cssClass: "col-xs-3 col-md-1",
        cellTemplate: function (container, options) {
            $("<div  id='Gestion_" + options.data.Codigo + "'>")
        .append($("<input type='checkbox' ng-model='fieldView' ng-checked='checked' ng-disabled='checked'>")).dxCheckBox({
            value: PermisosActuales(options.data.Codigo, 6),
            name: 'IGestion_' + options.data.Codigo,
            onValueChanged: function (e) {
                ValidarConsultar("IGestion_" + options.data.Codigo, "IGestion", "Gestion");
                IncluirItem(e, 6, options.data.Codigo);
            }
        }).removeClass("dx-button dx-button-normal dx-widget")
        .append($(""))
      .appendTo(container);
        }
    }
            ],
        });


    }

    function ActivarDepencias(codigo) {

        var dependencia = [];

        var valor_campo = false;

        $("#wait").show();
        $http.get('/api/Permisos?codigo_opcion=' + codigo).then(function (response) {
            $("#wait").hide();
            try {

                dependencia = response.data;

            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 3000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });


    }

    //Almacena los permisos 
    //Envía el objeto de permisos, el nit de la empresa y el usuario que se gestiona en el momento.
    function guardarPermisos() {

        var empresa = Datos_empresa.split(' -- ');

        $http({
            url: '/api/Permisos/',
            data: { OpcionesUsuario: OpcionesUsuario, Datos_empresa: empresa[0], Datos_usuario: Datos_usuario },
            method: 'Post'

        }), function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 1500);
        }

    }

});


//Retorna el valor de los check, según la opción y el permiso
function PermisosActuales(codigo, opcion) {

    var opc_permiso = PermisosUsuario.filter(x => x.Codigo == codigo);

    var valor_campo = false;

    if (opc_permiso.length > 0) {

        switch (opcion) {
            case 1:
                valor_campo = opc_permiso[0].Consultar;
                break;
            case 2:
                valor_campo = opc_permiso[0].Agregar;
                break;
            case 3:
                valor_campo = opc_permiso[0].Editar;
                break;
            case 4:
                valor_campo = opc_permiso[0].Eliminar;
                break;
            case 5:
                valor_campo = opc_permiso[0].Anular;
                break;
            case 6:
                valor_campo = opc_permiso[0].Gestion;
                break;
        }
    }
    return valor_campo;

}

/****** Inicio IncluirItem ******/
//Incluye el item en el array
//se ejecuta en el evento onValueChanged de los check
function IncluirItem(e, opcion, codigo) {

    var item_array = {};
    item_array = OpcionesUsuario.filter(d => d.IntIdOpcion == codigo);

    //Valida la existencia de la opcion en el array.
    if (item_array.length == 0) {
        //añade el objeto en el array por primera vez.
        OpcionesUsuario.push(IncluirItemArray(codigo, opcion, e.value));
    } else {
        for (i = 0; i < OpcionesUsuario.length; i++) {

            if (OpcionesUsuario[i].IntIdOpcion == codigo) {
                //Elimina el objeto del array.
                const index = OpcionesUsuario.indexOf(OpcionesUsuario[i]);

                OpcionesUsuario.splice(index, 1);

                //Actualiza el objeto existente.
                var obj_nuevo = {};
                obj_nuevo = ActualizarItemObjeto(opcion, e.value, item_array);

                //Valida si alguna de las funciones se encuentra en verdadero y la genera añade al array.
                if (obj_nuevo.IntConsultar || obj_nuevo.IntAgregar || obj_nuevo.IntEditar || obj_nuevo.IntEliminar || obj_nuevo.IntAnular || obj_nuevo.IntGestion)
                    OpcionesUsuario.push(obj_nuevo);
            }
        }

    }
}
/****** Fin IncluirItem ******/

//Actualiza el valor del item.
function ActualizarItemObjeto(opcion, valor, objeto) {

    switch (opcion) {
        case 1:
            objeto[0].IntConsultar = valor;
            break;
        case 2:
            objeto[0].IntAgregar = valor;
            break;
        case 3:
            objeto[0].IntEditar = valor;
            break;
        case 4:
            objeto[0].IntEliminar = valor;
            break;
        case 5:
            objeto[0].IntAnular = valor;
            break;
        case 6:
            objeto[0].IntGestion = valor;
            break;
    }

    return objeto[0];
}

//Crea el item en el array por primera vez.
function IncluirItemArray(codigo, opcion) {

    var data = {};

    switch (opcion) {
        case 1:
            data = { IntIdOpcion: codigo, IntConsultar: true, IntAgregar: false, IntEditar: false, IntEliminar: false, IntAnular: false, IntGestion: false };
            break;
        case 2:
            data = { IntIdOpcion: codigo, IntConsultar: false, IntAgregar: true, IntEditar: false, IntEliminar: false, IntAnular: false, IntGestion: false };
            break;
        case 3:
            data = { IntIdOpcion: codigo, IntConsultar: false, IntAgregar: false, IntEditar: true, IntEliminar: false, IntAnular: false, IntGestion: false };
            break;
        case 4:
            data = { IntIdOpcion: codigo, IntConsultar: false, IntAgregar: false, IntEditar: false, IntEliminar: true, IntAnular: false, IntGestion: false };
            break;
        case 5:
            data = { IntIdOpcion: codigo, IntConsultar: false, IntAgregar: false, IntEditar: false, IntEliminar: false, IntAnular: true, IntGestion: false };
            break;
        case 6:
            data = { IntIdOpcion: codigo, IntConsultar: false, IntAgregar: false, IntEditar: false, IntEliminar: false, IntAnular: false, IntGestion: true };
            break;
    }

    return data;
}

function IrAConsulta() {
    window.location.assign("../Pages/ConsultaUsuarios.aspx");
}

var TiposEstado =
    [
{ ID: "1", Texto: 'ACTIVO' },
    {
        ID: "2", Texto: 'INACTIVO'
    }
    ];

//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
    for (var i = 0; i < miArray.length; i += 1) {
        if (ID == miArray[i].ID) {
            return i;
        }
    }
}