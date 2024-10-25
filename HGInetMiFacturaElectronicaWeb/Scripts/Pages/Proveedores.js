DevExpress.localization.locale(navigator.language);

var ProveedoresApp = angular.module('ProveedoresApp', ['AppSrvProveedor', 'AppMaestrosEnum', 'dx']);

ProveedoresApp.controller('ProveedoresController', function ProveedoresController($scope, $location, SrvProveedor, SrvMaestrosEnum) {

    var now = new Date();

    SrvMaestrosEnum.ObtenerSesion().then(function (data) {
        var tipo = data[0].Admin;       
        if (tipo) {
            $scope.Admin = true;
        };
    });

    SrvProveedor.ObtenerProveedores().then(function (data) {

        $("#gridProveedores").dxDataGrid({
            dataSource: data,

            paging: {
                pageSize: 20
            },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [5, 10, 20],
                showInfo: true
            }
                             , loadPanel: {
                                 enabled: true
                             }
                                  , allowColumnResizing: true
                           , columns: [
                               {
                                   cssClass: "col-md-1 col-xs-2",
                                   cellTemplate: function (container, options) {
                                       $("<div style='text-align:center'>")
                                           .append($("<a taget=_self class='icon-pencil3' title='Editar' href='GestionProveedores.aspx?IdSeguridad=" + options.data.StrIdSeguridad + "'>"))
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
                                   dataField: "StrMail"
                               },
                               {
                                   caption: "Observaciones",
                                   dataField: "StrObservaciones"
                               },
                               {
                                   caption: "Fecha Ingreso",
                                   dataField: "DatFechaIngreso",
                                   dataType: "date",
                                   format: "yyyy-MM-dd",
                               }
                           ],
            filterRow: {
                visible: true
            }
        });
    });
});



ProveedoresApp.controller('GestionProveedoresController', function GestionProveedoresController($scope, $location, SrvProveedor, SrvMaestrosEnum) {

    SrvMaestrosEnum.ObtenerSesion().then(function (data) {

    });

    var now = new Date();
    now.setFullYear(now.getFullYear() + 1)

    $scope.FechaExpiracionP = new Date(now).toISOString();
    $scope.FechaExpiracionHgi = new Date(now).toISOString();

    $scope.IdSeguridad = location.search.split('IdSeguridad=')[1];

    CargarFormulario();
    function CargarFormulario() {
        $("#Identificacion").dxTextBox({           
            onValueChanged: function (data) {
                $scope.Identificacion = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe Indicar el numero de Identificación"
            }, {
                type: "stringLength",
                max: 50,
                min: 6,
                message: "El numero de Identificación no puede ser mayor a 50 digitos ni menor a 6"
            }, {
                type: "numeric",
                message: "El numero de Identificación debe ser numérico"
            }]
        });


        $("#RazonSocial").dxTextBox({
            onValueChanged: function (data) {
                $scope.RazonSocial = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe Indicar la Razón Social "
            }
            ]
        });


        $("#Email").dxTextBox({
            onValueChanged: function (data) {
                $scope.Email = data.value;
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

        $("#Telefono").dxTextBox({
            onValueChanged: function (data) {
                $scope.Telefono = data.value;
            }
        }).dxValidator({
            validationRules: [{
                type: "stringLength",
                max: 30,
                message: "El Telefono no puede ser mayor a 30 caracteres"
            }, {
                type: "required",
                message: "Debe introducir el Telefono"
            }]
        });


        $("#observaciones").dxTextArea({
            onValueChanged: function (data) {
                $scope.Observaciones = data.value.toUpperCase();
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

        $("#UsuarioProveedor").dxTextBox({
            onValueChanged: function (data) {
                $scope.UsuarioProveedor = data.value;
            }
        }).dxValidator({
            validationRules: [{
                type: "stringLength",
                max: 30,
                message: "El Usuario no puede ser mayor a 30 caracteres"
            }, {
                type: "required",
                message: "Debe introducir el Usuario Receptor"
            }]
        });

        $("#PasswordProveedor").dxTextBox({
            mode: 'password',
            onValueChanged: function (data) {
                $scope.PasswordProveedor = data.value;
            }, onFocusIn: function (data) {

                $scope.PasswordProveedorTemp = $scope.PasswordProveedor;
                $scope.PasswordProveedor = '';
                $("#PasswordProveedor").dxTextBox({ value: '' });
            },
            onFocusOut: function (data) {
                if ($scope.PasswordProveedor == '') {
                    $("#PasswordProveedor").dxTextBox({ value: $scope.PasswordProveedorTemp });
                    $scope.PasswordProveedor = $scope.PasswordProveedorTemp;
                }
            },
        }).dxValidator({
            validationRules: [{
                type: "stringLength",
                max: 200,
                message: "La clave no puede ser mayor a 200 caracteres"
            }, {
                type: "required",
                message: "Debe introducir la clave"
            }]
        });

        $("#FechaExpiracionP").dxDateBox({
            value: now,
            width: '100%',
            displayFormat: "yyyy-MM-dd",
            onValueChanged: function (data) {
                $scope.FechaExpiracionP = new Date(data.value).toISOString();
            }

        });

        $("#FechaExpiracionP").dxDateBox({ min: now });


        $("#urlapi").dxTextBox({
            onValueChanged: function (data) {
                $scope.urlapi = data.value;
            },
            onKeyDown: function (data) {
                $("#btnValidarapi").dxSwitch({ value: false });
            }
        }).dxValidator({
            validationRules: [{
                type: "stringLength",
                max: 200,
                message: "La dirección webapi no puede ser mayor a 200 caracteres"
            }, {
                type: "required",
                message: "Debe introducir la Dirección webapi"
            }]
        });

        $("#ftp").dxTextBox({
            onValueChanged: function (data) {
                $scope.ftp = data.value;
            },
            onKeyDown: function (data) {
                $("#btnValidarsftp").dxSwitch({ value: false });
            },
        }).dxValidator({
            validationRules: [{
                type: "stringLength",
                max: 200,
                message: "La dirección sftp no puede ser mayor a 200 caracteres"
            }, {
                type: "required",
                message: "Debe introducir la dirección sftp"
            }]
        });


        $("#Usuariohgi").dxTextBox({
            onValueChanged: function (data) {
                $scope.Usuariohgi = data.value;
            },
            onKeyDown: function (data) {
                $("#btnValidarsftp").dxSwitch({ value: false });
                $("#btnValidarapi").dxSwitch({ value: false });
            }
        }).dxValidator({
            validationRules: [{
                type: "stringLength",
                max: 50,
                message: "El usuario Emisor no puede ser mayor a 50 caracteres"
            }, {
                type: "required",
                message: "Debe introducir el usuario emisor"
            }]
        });


        $("#Passwordhgi").dxTextBox({
            mode: 'password',
            onValueChanged: function (data) {
                $scope.Passwordhgi = data.value;
            },
            onKeyDown: function (data) {
                $("#btnValidarsftp").dxSwitch({ value: false });
                $("#btnValidarapi").dxSwitch({ value: false });
            },
            onFocusIn: function (data) {

                $scope.PasshgiTemp = $scope.Passwordhgi;
                $scope.Passwordhgi = '';
                $("#Passwordhgi").dxTextBox({ value: '' });
            },
            onFocusOut: function (data) {
                if ($scope.Passwordhgi == '') {
                    $("#Passwordhgi").dxTextBox({ value: $scope.PasshgiTemp });
                    $scope.Passwordhgi = $scope.PasshgiTemp;
                }
            },

        }).dxValidator({
            validationRules: [{
                type: "stringLength",
                max: 200,
                message: "La clave no puede ser mayor a 200 caracteres"
            }, {
                type: "required",
                message: "Debe introducir la clave"
            }]
        });

        $("#FechaExpiracionHgi").dxDateBox({
            value: now,
            width: '100%',
            displayFormat: "yyyy-MM-dd",
            onValueChanged: function (data) {
                $scope.FechaExpiracionHgi = new Date(data.value).toISOString();
            }

        });

        $("#FechaExpiracionHgi").dxDateBox({ min: now });


        $("#estado").dxCheckBox({
            name: "estado",
            text: "Activo",
            value: false,
            onValueChanged: function (data) {
                $scope.estado = data.value;
            }
        });


        $("#button").dxButton({
            text: "Guardar",
            type: "default",
            useSubmitBehavior: true
        });


        $("#form1").on("submit", function (e) {
            e.preventDefault();
            GuardarProveedor();
            /* Se comenta para que permita guardar sin validar
            if ($scope.ftpvalidado) {
                if ($scope.apivalidada) {
                    GuardarProveedor();
                } else {
                    DevExpress.ui.notify('Debe validar la webapi del proveedor', 'error', 6000);
                }

            } else {
                DevExpress.ui.notify('Debe validar el servidor sftp', 'error', 6000);
            }*/
        });



        $("#btnValidarsftp").dxSwitch({
            value: ($scope.IdSeguridad != undefined) ? true : false,
            offText: 'No',
            onText: 'SI',
            onOptionChanged: function (data) {
                $scope.ftpvalidado = data.value;
                if (data.value) {
                    if (data.value) {
                        if (($scope.ftp != '' && $scope.ftp != undefined) && ($scope.Usuariohgi != '' && $scope.Usuariohgi != undefined) && ($scope.Passwordhgi != '' && $scope.Passwordhgi != undefined)) {
                            validarSftp($scope.Usuariohgi, $scope.Passwordhgi, $scope.ftp);
                        } else {
                            $("#btnValidarsftp").dxSwitch({ value: false });
                            DevExpress.ui.notify('Hace falta algun campo', 'error', 6000);
                        }
                    }
                }
            }
        });


        $("#btnValidarapi").dxSwitch({
            value: ($scope.IdSeguridad != undefined) ? true : false,
            offText: 'No',
            onText: 'SI',
            onOptionChanged: function (data) {
                $scope.apivalidada = data.value;
                if (data.value) {
                    if (data.value) {
                        if (($scope.urlapi != '' && $scope.urlapi != undefined) && ($scope.Usuariohgi != '' && $scope.Usuariohgi != undefined) && ($scope.Passwordhgi != '' && $scope.Passwordhgi != undefined)) {
                            Validarapi($scope.Usuariohgi, $scope.Passwordhgi, $scope.urlapi);
                        } else {
                            $("#btnValidarapi").dxSwitch({ value: false });
                            DevExpress.ui.notify('Hace falta algun campo', 'error', 6000);
                        }
                    }
                }
            }
        });

    }

    function validarSftp(u, p, sftp) {
        SrvProveedor.Validarsftp(u, p, sftp).then(function (data) {
            DevExpress.ui.notify(data, 'success', 6000);
        }, function errorCallback(response) {
            $("#btnValidarsftp").dxSwitch({ value: false });
        });
    }

    function Validarapi(u, p, api) {
        SrvProveedor.Validarapi(u, p, api).then(function (data) {
            DevExpress.ui.notify(data, 'success', 6000);
        }, function errorCallback(response) {
            $("#btnValidarapi").dxSwitch({ value: false });
        });
    }

    if ($scope.IdSeguridad != undefined && $scope.IdSeguridad != '') {
        SrvProveedor.ObtenerProveedoresId($scope.IdSeguridad).then(function (data) {
            $scope.estado = data[0].BitActivo;

            var estado2 = $scope.estado;
            $scope.Identificacion = data[0].Identificacion;
            $scope.RazonSocial = data[0].RazonSocial;
            $scope.UsuarioProveedor = data[0].StrUsuario;
            $scope.PasswordProveedor = data[0].StrClave;
            $scope.Passwordhgi = data[0].StrHgiClave;
            $scope.Usuariohgi = data[0].StrHgiUsuario;
            $scope.Email = data[0].StrMail;
            $scope.Observaciones = data[0].StrObservaciones;
            $scope.Telefono = data[0].StrTelefono;
            $scope.urlapi = data[0].StrUrlApi;
            $scope.ftp = data[0].StrUrlFtp;
            $scope.FechaExpiracionHgi = data[0].DatHgiFechaExpiracion;
            $scope.FechaExpiracionP = data[0].DatFechaExpiracion;

            $("#Identificacion").dxTextBox({ readOnly: true });
            $("#RazonSocial").dxTextBox({ value: $scope.RazonSocial });
            $("#Identificacion").dxTextBox({ value: $scope.Identificacion });
            $("#UsuarioProveedor").dxTextBox({ value: $scope.UsuarioProveedor });
            $("#PasswordProveedor").dxTextBox({ value: $scope.PasswordProveedor });
            $("#FechaExpiracionP").dxDateBox({ value: $scope.FechaExpiracionP });
            $("#FechaExpiracionP").dxDateBox({ min: $scope.FechaExpiracionP });
            $("#Usuariohgi").dxTextBox({ value: $scope.Usuariohgi });
            $("#Passwordhgi").dxTextBox({ value: $scope.Passwordhgi });
            $("#FechaExpiracionHgi").dxDateBox({ value: $scope.FechaExpiracionHgi });
            $("#FechaExpiracionHgi").dxDateBox({ min: $scope.FechaExpiracionHgi });
            $("#Email").dxTextBox({ value: $scope.Email });
            $("#observaciones").dxTextArea({ value: $scope.Observaciones });
            $("#Telefono").dxTextBox({ value: $scope.Telefono });
            $("#urlapi").dxTextBox({ value: $scope.urlapi });
            $("#ftp").dxTextBox({ value: $scope.ftp });

            $("#estado").dxCheckBox({ value: estado2 });

            $scope.ftpvalidado = true;
            $scope.apivalidada = true;

        });
    }


    function GuardarProveedor() {

        var datos = $.param({
            StrIdentificacion: $scope.Identificacion,
            StrIdSeguridad: $scope.IdSeguridad,
            StrRazonSocial: $scope.RazonSocial,
            StrMail: $scope.Email,
            StrTelefono: $scope.Telefono,
            StrObservaciones: $scope.Observaciones,
            StrUsuario: $scope.UsuarioProveedor,
            StrClave: $scope.PasswordProveedor,
            StrHgiUsuario: $scope.Usuariohgi,
            StrHgiClave: $scope.Passwordhgi,
            StrUrlApi: $scope.urlapi,
            StrUrlFtp: $scope.ftp,
            BitActivo: $scope.estado,
            FechaExpProveedor: $scope.FechaExpiracionP,
            FechaexpHgi: $scope.FechaExpiracionHgi,
            Editar: ($scope.IdSeguridad != undefined && $scope.IdSeguridad != '') ? true : false
        });

        SrvProveedor.GuardarProveedor(datos).then(function (data) {
            DevExpress.ui.notify({ message: "Proveedor Guardado con exito", position: { my: "center top", at: "center top" } }, "success", 1500);
            setTimeout(IrAConsulta, 2000);
        });

    }

});

function IrAConsulta() {
    window.location.assign("../Pages/ConsultaProveedores.aspx");
}


