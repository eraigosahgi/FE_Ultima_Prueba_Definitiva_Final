<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sonda.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.Sonda" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- Global stylesheets -->
    <link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css" />
    <!-- /Global stylesheets -->

    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.17.1/moment.min.js"></script>
    <!-- Global stylesheets -->
    <link href="../../Scripts/assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/assets/css/core.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/assets/css/components.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/assets/css/colors.css" rel="stylesheet" type="text/css" />
    <!-- /global stylesheets -->

    <!-- Core JS files -->
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/loaders/pace.min.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/core/libraries/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/core/libraries/bootstrap.min.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/loaders/blockui.min.js"></script>
    <!-- /core JS files -->

    <!-- Theme JS files -->
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/visualization/d3/d3.min.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/visualization/d3/d3_tooltip.js"></script>

    <script type="text/javascript" src="../../Scripts/assets/js/plugins/forms/styling/switchery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/forms/styling/uniform.min.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/forms/selects/bootstrap_multiselect.js"></script>

    <script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/moment/moment.min.js"></script>

    <script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/headroom/headroom.min.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/headroom/headroom_jquery.min.js"></script>


    <script type="text/javascript" src="../../Scripts/assets/js/plugins/notifications/bootbox.min.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/notifications/pnotify.min.js"></script>
    <%--<script type="text/javascript" src="../../Scripts/assets/js/plugins/notifications/sweet_alert.min.js"></script>--%>

    <script type="text/javascript" src="../../Scripts/assets/js/plugins/media/fancybox.min.js"></script>

    <script type="text/javascript" src="../../Scripts/assets/js/core/app.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/pages/user_pages_team.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/pages/components_popups.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/pages/components_modals.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/pages/layout_navbar_hideable.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/extensions/session_timeout.min.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/pages/gallery.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/ripple.min.js"></script>

    <!-- /theme JS files -->


    <!-- Estilos CSS -->
    <%-- <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.spa.css" />--%>
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />

    <!-- Scripts Requeridos
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>-->

    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.16/angular.min.js"></script>
    <!--<script src="https://cdn3.devexpress.com/jslib/17.2.7/js/dx.all.js"></script>    -->
    <script src="../../Scripts/devall.js"></script>
    <script src="https://unpkg.com/devextreme-aspnet-data@1.3.0"></script>


    <link rel="stylesheet" type="text/css" href="https://www.w3schools.com/w3css/4/w3.css" />
    <script src="../../Scripts/Services/SrvDocumentos.js"></script>
    <script src="../../Scripts/Services/Loading.js"></script>
    <script src="../../Scripts/Services/MaestrosEnum.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div data-ng-app="ProcesarDocumentosApp" data-ng-controller="ProcesarDocumentosController" data-ng-init="total=0">
            <div class="col-md-12">
                <div class="panel panel-white">
                    <div class="panel-heading">
                        <h6 class="panel-title">Datos : 
                        <input type="text" id="lbltotaldocumentos" style="border-style: none; width: 100%; font-size: small;" readonly></input></h6>
                        <div class="col-md-8 ">
                        </div>
                        <div class="col-md-4 text-right">
                            <div data-dx-button="ButtonOptionsConsultar" style="margin-top: -10%;"></div>
                        </div>
                    </div>

                    <div class="panel-body">
                        <div class="demo-container">
                            <div id="gridDocumentos"></div>
                           
                            <div class="demo-container" style="padding-top: 80px;">
                                <div id="panelresultado">
                                    <div class="panel-heading">
                                        <h6 class="panel-title">Resultado de documentos procesados : </h6>

                                    </div>
                                    <hr />
                                    <div id="gridDocumentosProcesados"></div>
                                </div>
                            </div>

                        </div>

                    </div>


                </div>
            </div>
        </div>
    </form>



    <script>
        DevExpress.localization.locale(navigator.language);

        

        angular.module('ProcesarDocumentosApp', ['dx', 'AppMaestrosEnum', 'AppSrvDocumento'])

        .controller('ProcesarDocumentosController', function DocAdquirienteController($scope, $http, $location, SrvMaestrosEnum, SrvDocumento) {

           


            var lista = "";
            //Numero de documentos a Procesar
            $scope.total = 0;
            var codigo_adquiente = "", numero_documento = "", estado_recibo = "", fecha_inicio = "", fecha_fin = "", Estado = [], now = new Date();

           

            SrvMaestrosEnum.ObtenerSesion().then(function (data) {
                codigo_adquiente = data[0].Identificacion;
                consultar();
            });

            var makeAsyncDataSource = function () {
                return new DevExpress.data.CustomStore({
                    loadMode: "raw",
                    key: "ID",
                    load: function () {
                        return JSON.parse(JSON.stringify(Estado));
                    }
                });
            };

           


             



             
              

              
            
            //Consultar DOcumentos
            function consultar() {
               

                SrvDocumento.ObtenerDocumentos().then(function (data) {
                    

                    for (var i = 0; i < data.length; i++) {
                        lista += (lista) ? ',' : '';
                        lista += "{Documentos: '" + data[i].IdSeguridad + "'}";
                    }
                    lista = "[" + lista + "]"
                    $scope.documentos = lista;
                    $('#lbltotaldocumentos').val('Documentos a Procesar : ' + data.length);
                    $scope.total = data.length;

                    console.log("Lista de documentos: ", $scope.documentos);

                    if (data.length > 0) {
                        
                        ProcesarDocumentos();
                    }
                   
                });

               
            }



            function ProcesarDocumentos() {
                if ($scope.total > 0) {

                    SrvDocumento.ProcesarDocumentos($scope.documentos).then(function (data) {
                        //Aqui se debe seguir validando si existen documentos
                       // consultar();                       
                    });
                }

            }

        });


    </script>
</body>
</html>
