DevExpress.localization.locale(navigator.language);

var ModalEmpresasApp = angular.module('ModalEmpresasApp', []);

var ConsultaUsuarioApp = angular.module('ConsultaUsuarioApp', ['dx', 'ModalEmpresasApp']);

ConsultaUsuarioApp.controller('ConsultaUsuarioController', function ConsultaUsuarioController($scope, $http, $location) {


    var codigo_usuario = "",
        codigo_empresa = "";


    //Define los campos y las opciones
    $scope.filtros =
        {
            //Control defecto ingreso de datos.
            CodigoUsuario: {
                placeholder: "Ingrese Código de Usuario",
                onValueChanged: function (data) {
                    console.log("codigo_usuario", data.value);
                    codigo_usuario = data.value;
                }
            },
            CodigoEmpresa: {
                placeholder: "Ingrese Identificación de la Empresa",
                onValueChanged: function (data) {
                    console.log("codigo_empresa", data.value);
                    codigo_empresa = data.value;
                }
            }
        }
    consultar();
    $scope.ButtonOptionsConsultar = {
        text: 'Consultar',
        type: 'default',
        onClick: function (e) {
            consultar();
        }
    };


    function consultar() {

        if (codigo_usuario == "")
            codigo_usuario = "*";

        if (codigo_empresa == "")
            codigo_empresa = "*";

        //Obtiene los datos del web api
        //ControladorApi: /Api/Usuario/
        //Datos GET: string codigo_usuario, string codigo_empresa
        $('#wait').show();
        $http.get('/api/Usuario?codigo_usuario=' + codigo_usuario + '&codigo_empresa=' + codigo_empresa).then(function (response) {
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
                columns: [
                     {
                         caption: "",
                         cellTemplate: function (container, options) {
                             $("<div>")
                                 .append(
                                     $("<a class='icon-mail-read' style='margin-left:12%; font-size:19px'></a> &nbsp;&nbsp;").dxButton({
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
                                 .append($("<a taget=_self class='icon-pencil3' title='Editar' href='GestionUsuarios.aspx?IdSeguridad=" + options.data.IdSeguridad + "'>"))
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
                         }
                ],
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
           codigo_adquiriente = "";
    $http.get('/api/DatosSesion/').then(function (response) {

        console.log(response.data[0]);

        codigo_facturador = response.data[0].Identificacion;

    });

    var Datos_nombres = "",
        Datos_apellidos = "",
        Datos_usuario = "",
        Datos_clave = "",
        Datos_telefono = "",
        Datos_extension = "",
        Datos_celular = ""
        Datos_email = "",
        Datos_cargo = "1",
        Datos_empresa = "",
        Datos_estado = "",
        Datos_Tipo = "1";
        
    //Define los campos del Formulario  
    $(function () {
        $("#summary").dxValidationSummary({});
      

        $("#txtnombres").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtnombres", data.value);
                Datos_nombres = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe Indicar el Nombre"
            }]
        });

        $("#txtapellidos").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtapellidos", data.value);
                Datos_apellidos = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe indicar el Apellido"
            }]
        });

        $("#txtusuario").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtusuario", data.value);
                Datos_usuario = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir el Usuario"
            }]
        });

        $("#txtclave").dxTextBox({
            mode: "password",
            onValueChanged: function (data) {
                console.log("txtclave", data.value);
                Datos_clave = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir la Clave"
            }]
        });

        $("#txttelefono").dxTextBox({
            onValueChanged: function (data) {
                console.log("txttelefono", data.value);
                Datos_telefono = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir el Teléfono"
            }]
        });

        $("#txtextension").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtextension", data.value);
                Datos_extension = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir la extensión"
            }]
        });

        $("#txtcelular").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtcelular", data.value);
                Datos_celular = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir el Celular"
            }]
        });

        $("#txtemail").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtemail", data.value);
                Datos_email = data.value;
            }
        })
        .dxValidator({
            validationRules: [{
                type: "required",
                message: "Debe introducir el Email"
            }]
        });

        $("#txtcargo").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtcargo", data.value);
                Datos_cargo = data.value;
            }
        })
       .dxValidator({
           validationRules: [{
               type: "required",
               message: "Debe introducir el Cargo"
           }]
       });

        $("#txtempresaasociada").dxTextBox({
            onValueChanged: function (data) {
                console.log("txtempresaasociada", data.value);
                Datos_empresa = data.value;
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
                console.log("cboestado", data.value.ID);
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
            Consulta:1});

        Datos_Tipo = "2";
        $("#wait").show();
        $http.get('/api/Usuario?' + parametros).then(function (response) {
            $("#wait").hide();
            console.log("Funciono",response.data);
            try {

                Datos_nombres = response.data[0].StrNombres;
                Datos_apellidos = response.data[0].StrApellidos;
                Datos_usuario = response.data[0].StrUsuario;
                Datos_clave = response.data[0].StrClave;
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
                $("#txtclave").dxTextBox({ value: Datos_clave });
                $("#txttelefono").dxTextBox({ value: Datos_telefono });
                $("#txtextension").dxTextBox({ value: Datos_extension });
                $("#txtcelular").dxTextBox({ value: Datos_celular });
                $("#txtemail").dxTextBox({ value: Datos_email });
                $("#txtcargo").dxTextBox({ value: Datos_cargo });
                $("#txtempresaasociada").dxTextBox({ value: Datos_empresa });
                $("#txtempresaasociada").dxTextBox({ readOnly: true });
                $("#cboestado").dxSelectBox({ value: TiposEstado[BuscarID(TiposEstado, Datos_estado)] });
                               
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
            guardarUsuario();
        }
    };

    function guardarUsuario() {

        var data = $.param({
            StrEmpresa: Datos_empresa,
            StrUsuario: Datos_usuario,
            StrClave: Datos_clave,
            StrNombres: Datos_nombres,
            StrApellidos: Datos_apellidos,
            StrMail: Datos_email,
            StrTelefono: Datos_telefono,
            StrExtension: Datos_extension,
            StrCelular: Datos_celular,
            StrCargo:Datos_cargo,
            IntIdEstado:Datos_estado,
            Tipo: Datos_Tipo 
        });

        $("#wait").show();
        $http.post('/api/Usuario?' + data).then(function (response) {
            $("#wait").hide();
            try {
                //Aqui se debe colocar los pasos a seguir
                DevExpress.ui.notify({ message: "Usuario Guardado con exito", position: { my: "center top", at: "center top" } }, "success", 6000);

            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 3000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });
    }

});

var TiposEstado =
    [
        { ID: "1", Texto: 'Activo' },
        { ID: "2", Texto: 'Inactivo' }
    ];

//Funcion para Buscar el indice el Array de los objetos, segun el id de base de datos
function BuscarID(miArray, ID) {
    for (var i = 0; i < miArray.length; i += 1) {
        if (ID == miArray[i].ID) {
            return i;
        }
    }
}