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

        console.log(response.data[0]);

        codigo_facturador = response.data[0].Identificacion;

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
        Datos_Resolucion=""


    //Define los campos del Formulario  
        $(function () {
            $("#summary").dxValidationSummary({});

            $("#TipoIndentificacion").dxSelectBox({
                placeholder:"Seleccione el tipo de Indetificación",
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
                    min: 4,
                    max:20,
                    message: "El campo debe contener entre 4 y 20 caracteres"
                }]
            });

            $("#txtRasonSocial").dxTextBox({
                onValueChanged: function (data) {
                    console.log("RasonSocial", data.value);
                    Datos_Razon_Social = data.value;
                }
            })
            .dxValidator({
                validationRules: [{
                    type: "required",
                    message: "Debe introducir la Razón Social"
                }]
            });

            $("#txtEmail").dxTextBox({
                onValueChanged: function (data) {
                    console.log("txtEmail", data.value);
                    Datos_Email = data.value;
                }
            })
            .dxValidator({
                validationRules: [{
                    type: "required",
                    message: "Debe introducir el Email"
                }]
            });

            $("#txtempresaasociada").dxTextBox({                
                onValueChanged: function (data) {
                    console.log("txtempresaasociada", data.value);
                    Datos_IdentificacionDv = data.value;
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
            });

            $("#Adquiriente").dxCheckBox({
                name: "PerfilAdquiriente",
                text: "Adquiriente",
                value: false,
                onValueChanged: function (data) {
                    console.log("Datos_Adquiriente", data.value);
                    Datos_Adquiriente = data.value;
                }
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


            $("#txtResolucion").dxTextBox({
                onValueChanged: function (data) {
                    console.log("txtresolucion", data.value);
                    Datos_Resolucion = data.value;
                }
            })
          .dxValidator({
              validationRules: [{
                  type: "required",
                  message: "Debe introducir el codigo Resolución"
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
                Datos_Adquiriente = (response.data[0].Intadquiriente)? true:false;
                Datos_Obligado = (response.data[0].intObligado) ? true : false;
                Datos_Habilitacion = response.data[0].Habilitacion;
                Datos_IdentificacionDv = response.data[0].IntIdentificacionDv;
                Datos_Resolucion = response.data[0].StrResolucionDian;
                                                             
                $("#Facturador").dxCheckBox({ value: Datos_Obligado });
                $("#Adquiriente").dxCheckBox({ value: Datos_Adquiriente });

                $("#NumeroIdentificacion").dxTextBox({ value: Datos_Idententificacion });
                $("#NumeroIdentificacion").dxTextBox({ readOnly: true });
                
                $("#txtRasonSocial").dxTextBox({ value: Datos_Razon_Social });
                $("#txtEmail").dxTextBox({ value: Datos_Email });
                
                $("#TipoIndentificacion").dxSelectBox({ value: TiposIdentificacion[BuscarID(TiposIdentificacion, Datos_Tipoidentificacion)] });
                $("#TipoIndentificacion").dxSelectBox({ readOnly: true });

                $("#Habilitacion").dxRadioGroup({ value: TiposHabilitacion[BuscarID(TiposHabilitacion, Datos_Habilitacion)] });
                $("#txtResolucion").dxTextBox({ value: Datos_Resolucion });

                
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
    //1933
    //1905

    function guardarEmpresa() {

        var data = $.param({
            TipoIdentificacion: Datos_Tipoidentificacion,
            Identificacion: Datos_Idententificacion,
            RazonSocial: Datos_Razon_Social,
            Email: Datos_Email,
            Intadquiriente: (Datos_Adquiriente)?true:false,
            IntObligado: (Datos_Obligado)?true:false,
            IntHabilitacion: Datos_Habilitacion,
            StrEmpresaAsociada: Datos_IdentificacionDv,
            tipo: Datos_Tipo
        });

        $("#wait").show();
        $http.post('/api/Empresas?' + data).then(function (response) {
            $("#wait").hide();
            try {
                //Aqui se debe colocar los pasos a seguir
                DevExpress.ui.notify({ message: "Empresa Guardada con exito", position: { my: "center top", at: "center top" } }, "success", 6000);

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
function BuscarID(miArray,ID) {
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
        { ID: "11", Texto: 'Registro civil'},
        { ID: "12", Texto: 'Tarjeta de identidad'},
        { ID: "13", Texto: 'Cédula de ciudadanía'},
        { ID: "21", Texto: 'Tarjeta de extranjería'},
        { ID: "22", Texto: 'Cédula de extranjería'},
        { ID: "31", Texto: 'NIT'},
        { ID: "41", Texto: 'Pasaporte' },
        { ID: "42", Texto: 'Documento de identificación extranjero'}
    ];
