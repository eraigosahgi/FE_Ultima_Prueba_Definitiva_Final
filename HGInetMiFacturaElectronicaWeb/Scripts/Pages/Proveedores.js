DevExpress.localization.locale(navigator.language);



var ProveedoresApp = angular.module('ProveedoresApp', ['AppSrvProveedor', 'dx']);

ProveedoresApp.controller('ProveedoresController', function ProveedoresController($scope, $location, SrvProveedor) {



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
                                           .append($("<a taget=_self class='icon-pencil3' title='Editar' href='GestionProveedores.aspx?IdSeguridad=" + options.data.IdSeguridad + "'>"))
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



ProveedoresApp.controller('GestionProveedoresController', function GestionProveedoresController($scope, $location, SrvProveedor) {
    //Define los campos del Formulario  

    //Consultar por el id de seguridad para obtener los datos de la empresa a modificar
    $scope.IdSeguridad = location.search.split('IdSeguridad=')[1];



    SrvProveedor.ObtenerProveedores($scope.IdSeguridad).then(function (data) {
        console.log("Prodeedor: ",data);
    });



    CargarFormulario();
    function CargarFormulario() {
        //$("#summary").dxValidationSummary({});


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
        });

        $("#PasswordProveedor").dxTextBox({
            onValueChanged: function (data) {
                $scope.PasswordProveedor = data.value;
            }
        });

        $("#urlapi").dxTextBox({
            onValueChanged: function (data) {
                $scope.urlapi = data.value;
            }
        });

        $("#ftp").dxTextBox({
            onValueChanged: function (data) {
                $scope.ftp = data.value;
            }
        });


        $("#Usuariohgi").dxTextBox({
            onValueChanged: function (data) {
                $scope.Usuariohgi = data.value;
            }
        });

        $("#Passwordhgi").dxTextBox({
            onValueChanged: function (data) {
                $scope.Passwordhgi = data.value;
            }
        });


        $("#estado").dxCheckBox({
            name: "estado",
            text: "Activo",
            value: false,
            onValueChanged: function (data) {
                $scope.estado = data.value;                
            }
        });

       //.dxValidator({
        //    validationRules: [
        //        {
        //            type: 'custom', validationCallback: function (options) {
        //                if (validar()) {
        //                    options.rule.message = "Debe Indicar si es Facturador o Adquiriente";
        //                    return false;
        //                } else { return true; }
        //            }
        //        }
        //    ]
        //});



        //$("#txtUsuarios").dxNumberBox({
        //    value: Datos_Numero_usuarios,
        //    onValueChanged: function (data) {
        //        Datos_Numero_usuarios = data.value;
        //    }
        //}).dxValidator({
        //    validationRules: [
        //        {
        //            type: "required",
        //            message: "Debe introducir el número de usuarios"
        //        }]
        //});







        


        //$("#button").dxButton({
        //    text: "Guardar",
        //    type: "default",
        //    useSubmitBehavior: true
        //});


        //$("#form1").on("submit", function (e) {
        //    guardarEmpresa();
        //    e.preventDefault();
        //});


    }
});
//Esta funcion es para ir a la pagina de consulta
function IrAConsulta() {
    window.location.assign("../Pages/ConsultaProveedores.aspx");
}


