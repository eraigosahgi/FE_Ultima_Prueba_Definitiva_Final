<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Login.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>HGInet Facturación Electrónica</title>

    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <!-- Global stylesheets -->
    <link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css" />
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
    <script type="text/javascript" src="../../Scripts/assets/js/plugins/forms/styling/uniform.min.js"></script>

    <script type="text/javascript" src="../../Scripts/assets/js/core/app.js"></script>
    <script type="text/javascript" src="../../Scripts/assets/js/pages/login.js"></script>

    <script type="text/javascript" src="../../Scripts/assets/js/plugins/ui/ripple.min.js"></script>
    <!-- /theme JS files -->


    <!-- Estilos CSS -->
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.spa.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />



    <!-- DevExtreme -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.16/angular.min.js"></script>
    <script src="https://cdn3.devexpress.com/jslib/17.2.7/js/dx.all.js"></script>
    <script src="https://unpkg.com/devextreme-aspnet-data@1.3.0"></script>
    <!-- /DevExtreme -->

    <script src="../../Scripts/Pages/Autenticacion.js"></script>

</head>
<body class="login-container" style="background-color: #eeeded">
    <%--Panel carga o Loading--%>
    <div id="wait" style="display: none; z-index: 9999;">
        <div class="modal" style="background-color: white; opacity: 0.6; display: block;"></div>
        <div>
            <img class="divImg" style="position: absolute; left: 43%; top: 30%; z-index: 9999; width: 20%; height: 20%;" src="../../Content/icons/Loading.gif" />
        </div>
    </div>
    <div class="row" style="height: 100%; background-color: white;">
        <div class="col-md-6 col-lg-8 hidden-xs hidden-sm" style="height: 100%;">
            <div id="galleryContainer" style="background-color: white; height: 100%;">
            </div>
        </div>
        <div class="col-md-6 col-lg-4" style="background-color: white; height: 100%;">
            <div ng-app="AutenticacionApp">
                <!-- Contenedor de Página -->
                <div class="page-container" ng-controller="AutenticacionController">

                    <!-- Contenido de Página -->
                    <div class="page-content">

                        <!-- Main content -->
                        <div class="content-wrapper">

                            <!-- Área de Contenido -->
                            <div class="content">

                                <form ng-submit="onFormSubmit($event)">

                                    <div>
                                        <div class="text-center">
                                            <img src="../../Scripts/Images/LogoPlataforma.PNG" class="img-responsive" style="width: 70%; height: 70%; margin-left: 15%; margin-right: 15%" />
                                            <%--<div class="icon-object border-primary-800 text-primary-800"><i class="icon-user"></i></div>--%>
                                            <h5 class="content-group-lg"><small class="display-block">Ingrese los datos de autenticación</small></h5>
                                        </div>

                                        <div class="widget-container">
                                            <div id="form" dx-form="formOptions">
                                            </div>
                                        </div>
                                        <br />
                                        <a data-toggle="modal" data-target="#modal_restablecer_clave" data-popup="tooltip" title="Restablecer contraseña" style="color: #166dba">
                                            <h6>Restablecer contraseña</h6>
                                        </a>
                                        <br />
                                        <div dx-button="buttonOptions"></div>



                                    </div>
                                </form>

                                <!-- /Formulario Autenticación -->

                            </div>
                            <!-- /Área de Contenido -->

                            <span class="help-block text-center no-margin">
                                <img src="/../Scripts/Images/Logo.png" style="align-content: center; width: 130px" class="img-responsive center-block" />
                                Copyright © 2018 <a href="http://www.hgi.com.co" target="_blank">HGI S.A.S.</a>
                                <br />
                            </span>

                        </div>
                        <!-- /main content -->

                    </div>
                    <!-- /Contenido de Página -->

                </div>
                <!-- /Contenedor de Página -->
                <%--Modal Restablecer Contraseña--%>
                <form ng-submit="onFormSubmit($event)" ng-controller="RestablecerController">
                    <div id="modal_restablecer_clave" class="modal fade" style="display: none;">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div id="EncabezadoModal" class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">×</button>
                                    <h5 style="margin-bottom: 10px;" class="modal-title">Restablecer Contraseña</h5>
                                </div>

                                <div class="modal-body">

                                    <div class="col-md-8 col-md-offset-2">

                                        <div id="formulario" class="row">


                                            <div id="formvalidar" dx-form="formOptions2">
                                            </div>



                                        </div>

                                    </div>
                                </div>

                                <div id="divsombra" class="modal-footer" style="margin-top: 22%">
                                    <div dx-button="buttonCerrarRestablecer" data-dismiss="modal"></div>
                                    <div dx-button="buttonRestablecer"></div>
                                </div>

                            </div>
                        </div>
                    </div>

                </form>
            </div>
        </div>
    </div>
    <%--/ Modal Restablecer Contraseña--%>
</body>
</html>
