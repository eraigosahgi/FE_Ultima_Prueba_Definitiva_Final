<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestablecerClave.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Login.RestablecerClave" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>HGInet Facturación Electrónica</title>

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
    <link rel="dx-theme" data-theme="generic.light" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />

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

    <!-- Contenedor de Página -->
    <div class="page-container" ng-app="RestablecerClaveApp" ng-controller="RestablecerClaveController">

        <!-- Contenido de Página -->
        <div class="page-content">

            <!-- Main content -->
            <div class="content-wrapper">

                <!-- Área de Contenido -->
                <div class="content">

                    <form ng-submit="onFormSubmit($event)">

                        <div class="panel panel-body login-form widget-container">
                            <div class="text-center">
                                <div class="icon-object border-primary-800 text-primary-800"><i class="icon-lock2"></i></div>
                                <h5 class="content-group-lg">Restablecimiento Contraseña<small class="display-block">Ingrese su nueva contraseña</small></h5>
                            </div>

                            <div class="widget-container">
                                <div id="form" dx-form="formOptions">
                                </div>
                            </div>
                            <br />
                            <div dx-button="buttonOptions"></div>

                        </div>

                    </form>

                    <!-- /Formulario Autenticación -->

                </div>
                <!-- /Área de Contenido -->

            </div>
            <!-- /main content -->

        </div>
        <!-- /Contenido de Página -->

    </div>
    <!-- /Contenedor de Página -->

</body>
</html>
