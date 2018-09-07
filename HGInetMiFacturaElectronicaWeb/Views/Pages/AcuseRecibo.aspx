<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcuseRecibo.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.AcuseRecibo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>HGInet Factura Electrónica</title>

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



    <!-- JS AcuseRecibo -->
    <script src="../../Scripts/Pages/AcuseRecibo.js"></script>


</head>

<body class="login-container" style="background-color: #eeeded">

    <div runat="server" class="form-horizontal" ng-app="AcuseReciboApp" ng-controller="AcuseReciboController" data-ng-cloak="" data-ng-init="DetalleAcuse=true">
        <div style="margin: 1%;" runat="server" id="PanelInformacion" ng-repeat="datos in RespuestaAcuse">

            <!-- Visualización Información Factura -->
            <div class="col-md-6">
                <div class="panel panel-flat form-horizontal">


                    <img class="img-responsive" alt="" title="" src="http://tz37.mjt.lu/tplimg/tz37/b/4krt/x80qh.png" />

                    <div class="panel-body" style="display: block">

                        <div id="PanelInformacionFactura" style="font-size: 15px" class="dx-fieldset">

                            <h4 class="panel-title text-bold text-center" style="margin-bottom: 5%">Información de {{datos.tipodoc}}</h4>

                            <div>
                                <label id="Label2" class="text-bold">Número Documento: </label>
                                <label id="LblNumeroDocumento"></label>
                                <span>{{datos.NumeroDocumento}}</span>
                            </div>

                            <div style="margin-top: 0%">
                                <label id="Label5" class="text-bold">Identificación Tercero: </label>
                                <span>{{datos.IdAdquiriente}}</span>
                            </div>

                            <div style="margin-top: 0%">
                                <label id="Label9" class="text-bold">Nombre Tercero: </label>
                                <span>{{datos.NombreAdquiriente}}</span>
                            </div>

                            <div style="margin-top: 0%; width: 90%">
                                <label class="text-bold">CUFE: </label>
                                <span>{{datos.Cufe}}</span>
                            </div>

                            <div style="margin-bottom: 0%">
                                <label class="text-bold">Radicado: </label>
                                <span>{{datos.IdSeguridad}}</span>
                            </div>

                        </div>


                        <!-- PANEL CONTIENE LA RESPUESTA SI YA LA TIENE-->
                        <div id="PanelRespuestaAdquiriente" style="margin-top: -25px; font-size: 15px" ng-show="{{datos.RespuestaVisible}}" class="dx-fieldset">

                            <h4 class="panel-title text-bold text-center">Respuesta Adquiriente</h4>

                            <div style="margin-top: 1%;" runat="server" id="DivEstadoRespuesta">
                                <label class="text-bold">Estado: </label>
                                <span>{{datos.EstadoAcuse}}</span>
                            </div>


                            <div style="margin-top: 1%;" runat="server" id="DivObservaciones">
                                <label class="text-bold">Observaciones: </label>
                                <span>{{datos.MotivoRechazo}}</span>
                            </div>

                            <div style="margin-top: 1%;" runat="server" id="Div1">
                                <label class="text-bold">Fecha: </label>
                                <span>{{datos.FechaRespuesta | date: "yyyy-MM-dd HH:mm"}}</span>
                            </div>

                        </div>

                        <!--data-ng-if="DetalleAcuse"-->
                        <div>
                            <!-- PANEL CONTIENE LAS OPCIONES DE RESPUESTA APROBAR/RECHAZAR Y MOTIVO -->
                            <div id="PanelOpcionesAdquiriente" style="margin-top: -25px" data-ng-show="{{datos.CamposVisibles}}" class="dx-fieldset">

                                <form data-ng-submit="onFormSubmit($event)">

                                    <div>
                                        <h4 class="panel-title text-bold text-center">Respuesta Acuse</h4>
                                        <br />
                                        <div class="col-md-12 ">
                                            <label><strong>Aprobar:</strong></label>
                                            <input type="radio" style="cursor: pointer" class="w3-radio" value="1" data-ng-model="value" name="group1" ng-change="ValidarEstado(value)" />

                                            <label style="margin-left: 20px;"><strong>Rechazar:</strong></label>
                                            <input type="radio" style="cursor: pointer" class="w3-radio" value="2" data-ng-model="value" name="group1" ng-change="ValidarEstado(value)" />
                                        </div>
                                        <br />
                                    </div>

                                    <div id="form" data-dx-form="TextAreaObservaciones" style="padding-left: 2%; padding-top: 5%;"></div>
                                    <br />
                                    <div class="col-lg-12 text-right" style="margin-top: -15px">
                                        <div data-dx-button="ButtonOptionsRechazar" data-ng-if="RechazarVar"></div>
                                        <div data-dx-button="ButtonOptionsAceptar" data-ng-if="AceptarVar"></div>
                                        &nbsp;&nbsp;&nbsp;
                                    
                                    </div>

                                </form>

                            </div>
                        </div>
                        <div id="PanelInformacionArchivos" style="font-size: 15px" class="dx-fieldset">

                            <h4 class="panel-title text-bold text-center">Archivos</h4>

                            <div style="margin-top: 1%; text-align: justify;">
                                Para visualizar los archivo en el navegador presione clic o si desea descargarlos presione clic derecho sobre el link y seleccione la opción Guardar como.
                            </div>

                            <div style="margin-top: 1%; text-align: center;">

                                <a href="{{datos.Pdf}}" target="_blank" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px; text-align: center;">Pdf</a>
                                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                <a href="{{datos.Xml}}" target="_blank" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px; text-align: center;">Xml</a>
                            </div>
                            <br />
                            <div style="text-align: center;">

                                <a class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important; font-size: 14px; text-align: center;" data-ng-click="habilitar()" data-ng-show="datos.tipodoc!='Nota Crédito' && datos.poseeIdComercio==true">Pagar</a>
                                <a id="btnautenticar" class="btn btn-default" style="background: rgb(51, 122, 183); color: white; text-transform: initial !important" href="../Login/Default.aspx" style="font-size: 14px; text-align: center;">Autenticar</a>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-top: -15px; margin-bottom: 8%">
                            <img alt="" title="" src="http://tz37.mjt.lu/tplimg/tz37/b/6572/x80n7.png" style="border-style: none; border-color: inherit; border-width: medium; border-radius: ; display: block; font-size: 13px; outline: none; text-decoration: none; width: 100%; height: 100px; margin-left: -10x; margin-right: -10px">
                        </div>
                    </div>

                    <div class="footer text-muted" style="font-size: 14px; text-align: center; margin-bottom: 5%">
                        Copyright © 2018 <a href="http://www.hgi.com.co" target="_blank" style="color: rgb(22, 109, 186);">HGI S.A.S - HGInet Facturación Electrónica</a>
                    </div>
                </div>

            </div>

            <!-- Visualización PDF -->
            <div class="col-md-6">

                <embed width="100%" height="850px" name="plugin" id="plugin" src="{{datos.Pdf}}" type="application/pdf">
            </div>
            <!-- /Visualización PDF -->

        </div>


    </div>
    <%--Panel carga o Loading--%>
    <div id="wait" style="display: none; z-index: 9999;">
        <div class="modal" style="background-color: white; opacity: 0.6; display: block;"></div>
        <div>
            <img class="divImg" style="position: absolute; left: 43%; top: 30%; z-index: 9999; width: 20%; height: 20%;" src="../../Content/icons/Loading.gif" />
        </div>
    </div>
</body>


</html>
