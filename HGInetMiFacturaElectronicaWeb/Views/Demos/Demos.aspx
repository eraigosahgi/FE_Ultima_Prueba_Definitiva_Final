<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Demos.Demos" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <title>Página Demo</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Estilos CSS -->
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.spa.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.common.css" />
    <link rel="dx-theme" data-theme="generic.light" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />

    <!-- Scripts Requeridos-->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.16/angular.min.js"></script>
    <script src="https://cdn3.devexpress.com/jslib/17.2.7/js/dx.all.js"></script>
    <script src="https://unpkg.com/devextreme-aspnet-data@1.3.0"></script>


    <%--<script>window.jQuery || document.write(decodeURIComponent('%3Cscript src="js/jquery-3.1.0.min.js"%3E%3C/script%3E'))</script>
    <script>window.angular || document.write(decodeURIComponent('%3Cscript src="js/angular.min.js"%3E%3C\/script%3E'))</script>--%>


    <!-- JS DataGrid DevExtreme-->
    <script src="../../Scripts/Demos/DataGrid.js"></script>

    <!-- JS Localización -->
    <script src="../../Scripts/devextreme-localization/dx.messages.es.js"></script>


</head>

<body>

    <!-- /Div Principal -->
    <div style="margin-left: 4%; margin-right: 4%; margin-bottom: 10%">
        <br />
        <h2>Implementación DevExtreme</h2>
        <br />
        <h5>NuGet:</h5>
        <p>DevExtreme.Web 17.2.7 - AngularJS 1.6.9 - JQuery v3.3.1</p>
        <br />
        <p>
            Para la implementación de componentes y librerías requeridas, se recomienda que las versiones de los paquetes y Nugets instalados coincidan con las versiones de las referencias,
        y verificar que el código implementado se encuentre dentro de dicha versión (pueden ser referenciadas versiones inferiores a la instalada).<br />
            Las funciones, estilos, mensajes y contenidos son modificables en su gran mayoría, únicamente de debe tener en cuenta la estructura y llamado de cada función y/o etiqueta.
            <br />
            <br />
            <br />
            Para realizar la configuración de idioma de los mensajes se debe añadir la referencia del script del idioma que se desea configurar.
            para español corresponde a:  /devextreme-localization/dx.messages.es.js<br />
            y se debe añadir la siguiente línea en el archivo JS:<br />
            DevExpress.localization.locale(navigator.language);
        </p>
        <br />
        <p>IMPORTANTE: la referencia del script JSZip debe invocarse antes de los scripts DevExtreme.</p>
        <br />
        <h5>Link CssClasses</h5>
        <br />
        <center><h1>DataGrid</h1></center>
        <br />
        <h5>Link Demo:
        <a href="https://js.devexpress.com/Demos/WidgetsGallery/Demo/DataGrid/WebAPIService/AngularJS/Light/">Ir a Demo</a></h5>
        <br />

        <p>La lógica del siguiente grid se encuentra en su totalidad en el archivo js, el archivo .html unicamente contiene el elemento raíz (DemoApp) y el controlador de la aplicación (DemoController).</p>
        <br />
        <h5>Funcionalidades implementadas:</h5>
        <ul>
            <li>Definición y configuración de columnas.</li>
            <li>Selección de columnas.</li>
            <li>Exportación a Excel.</li>
            <li>Número de registros visibles.</li>
            <li>Modificar número de registros visibles.</li>
            <li>Columnas adicionales.</li>
            <li>Validaciones filtros de búsqueda.</li>
            <li>Filtros y ordenamiento de columnas.</li>
            <li>Configuración idioma de mensajes.</li>
        </ul>

        <!-- DemoController -->
        <div class="demo-container" ng-app="DemoApp" ng-controller="DemoController">

            <!-- DataGrid -->
            <div id="gridContainer" dx-data-grid="dataGridOptions">
            </div>
            <!-- /DataGrid -->

            <br />
            <br />

            <center> <h1>Formulario con Validaciones</h1></center>
            <br />
            <h5>Link Demo:<a href="https://js.devexpress.com/Demos/WidgetsGallery/Demo/Form/Validation/AngularJS/Light/">Ir a Demo</a></h5>
            <br />

            <!-- Formulario Validaciones -->
            <form action="your-action" ng-submit="onFormSubmit($event)">
                <div class="widget-container">
                    <div id="form" dx-form="formOptions"></div>
                </div>
                <br />
                <div dx-button="buttonOptions"></div>
            </form>
            <!-- /Formulario Validaciones -->

            <br />
            <br />

            <center><h1>Campos</h1></center>
            <br />
            <h5>Link Demo:<a href="https://js.devexpress.com/Demos/WidgetsGallery/Demo/Form/Validation/AngularJS/Light/">Ir a Demo</a></h5>
            <br />
            <p>Para la construcción de los siguientes campos de ejemplo, se realiza implementación de código HTML en compañía de código JS para la defición de tipos de datos que recibe el contro, validaciones e información contenida en cada uno de ellos.</p>
            <br />

            <!-- Formulario Campos -->
            <div class="form">
                <div class="dx-fieldset">
                    <div class="dx-field">
                        <div class="dx-field-value">
                            <label class="dx-fieldset-header">Defecto</label>
                            <div dx-autocomplete="campos.Defecto"></div>
                        </div>
                    </div>


                    <div class="dx-field">
                        <div class="dx-field-value">
                            <label class="dx-fieldset-header">LimpiaDatos</label>
                            <div dx-autocomplete="campos.LimpiaDatos"></div>
                        </div>
                    </div>

                    <div class="dx-field">
                        <div class="dx-field-value">
                            <label class="dx-fieldset-header">Desabilitado</label>
                            <div dx-autocomplete="campos.Desabilitado"></div>
                        </div>
                    </div>

                    <div class="dx-field">
                        <div class="dx-field-value">
                            <label class="dx-field-label">Checked</label>
                            <div dx-check-box="campos.checked"></div>
                        </div>
                    </div>

                    <div class="dx-field">
                        <div class="dx-field-label">Fecha</div>
                        <div class="dx-field-value">
                            <div dx-date-box="campos.Fecha"></div>
                        </div>
                    </div>

                    <div class="dx-field">
                        <div class="dx-field-label">Hora</div>
                        <div class="dx-field-value">
                            <div dx-date-box="campos.Hora"></div>
                        </div>
                    </div>

                    <div class="dx-field">
                        <div class="dx-field-label">Fecha / Hora</div>
                        <div class="dx-field-value">
                            <div dx-date-box="campos.FechaHora"></div>
                        </div>
                    </div>

                    <div class="dx-field">
                        <div class="dx-field-label">Selección</div>
                        <div class="dx-field-value">
                            <div dx-select-box="campos.Seleccion"></div>
                        </div>
                    </div>


                    <div class="dx-field">
                        <div class="dx-field-label">Multiple Selección</div>
                        <div class="dx-field-value">
                            <div dx-tag-box="maxDisplayedTags"></div>
                        </div>
                    </div>

                </div>
            </div>
            <!-- /Formulario Campos -->

            <br />
            <br />

            <center><h1>Botones</h1></center>
            <br />
            <h5>Link Demo:<a href="https://js.devexpress.com/Demos/WidgetsGallery/Demo/Button/PredefinedTypes/AngularJS/Light/">Ir a Demo</a></h5>
            <br />
            <p>El siguiente ejemplo, muestra los diferentes botones que pueden ser implementados con DevExtreme, pueden también ser modificados en su diseño y contener multiples funciones.</p>
            <br />


            <!-- Botones -->
            <div class="dx-fieldset">
                <div class="dx-field">
                    <label>Defecto</label>
                    <div dx-button="okButtonOptions"></div>
                </div>

                <div class="dx-field">
                    <label>Success</label>
                    <div dx-button="applyButtonOptions"></div>
                </div>

                <div class="dx-field">
                    <label>Default</label>
                    <div dx-button="doneButtonOptions"></div>
                </div>

                <div class="dx-field">
                    <label>Danger</label>
                    <div dx-button="deleteButtonOptions"></div>
                </div>

                <div class="dx-field">
                    <label>Back</label>
                    <div dx-button="backButtonOptions"></div>
                </div>

                <div class="dx-field">
                    <label>icono externo</label>
                    <div dx-button="sendButton"></div>
                </div>

            </div>
            <!-- /Botones -->

            <br />
            <br />

            <center><h1>Mensajes y Notificaciones</h1></center>
            <br />
            <h5>Link Demo: <a href="https://js.devexpress.com/Demos/WidgetsGallery/Demo/Popover/Overview/AngularJS/Light/">Ir a Demo</a></h5>
            <br />

            <!-- Mensajes y Notificaciones -->

            <div class="dx-field">

                <span id="subject1">Popover</span>
                (<a id="link1" href="http://hgi.com.co/">presione aquí</a>)
                        <div id="popover1" dx-popover="defaultOptions">
                            Mensaje Popover o Tooltip
                        </div>

            </div>


            <!-- /Mensajes y Notificaciones -->
            <br />
            <h5>Link Demo:<a href="https://js.devexpress.com/Documentation/Guide/Widgets/Popup/Overview/">Ir a Demo</a></h5>
            <br />

            <div id="popupContainer">
                <p>
                    Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor
                         (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años,
                         sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas "Letraset", 
                        las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum.
                </p>
            </div>
            <div id="buttonContainer"></div>


        </div>
        <!-- /DemoController -->
    </div>
    <!-- /Div Principal -->

</body>

</html>
