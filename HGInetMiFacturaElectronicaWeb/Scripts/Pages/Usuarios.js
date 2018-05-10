DevExpress.localization.locale(navigator.language);

var ConsultaUsuarioApp = angular.module('ConsultaUsuarioApp', ['dx']);
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
        $http.get('/api/Usuario?codigo_usuario=' + codigo_usuario + '&codigo_empresa=' + codigo_empresa).then(function (response) {

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
                                     $("<a class='icon-mail-read' style='margin-left:12%; font-size:19px'></a>").dxButton({
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
            Mensaje(response.data.ExceptionMessage, "error");
        };





    }



});