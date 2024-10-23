

//******************************************************CONSOLE.LOG******************************************************
//El console.log es implementado para la captura de datos en la consola del navegador y así poder llevar trazabilidad de los datos y respuestas de los controladores, 
//por ende se recomienda quitar cada uno de las líneas implementadas para esta función al salir a producción o culminar un proceso y/o tarea.
//******************************************************CONSOLE.LOG******************************************************


//******************************************************NOTIFY******************************************************
//DevExpress.ui.notify(mensaje, tipo, tiempo);
//Tipos: 'info', 'warning', 'error', 'success'
//EJEMPLO: DevExpress.ui.notify("Datos de autenticación inválidos.", 'error', 3000);
//******************************************************NOTIFY******************************************************


//Establece la localización - lenguaje.
DevExpress.localization.locale(navigator.language);



//Define el elemento raíz de la aplicación.
var DemoApp = angular.module('DemoApp', ['dx']);
//Define el controlador de la aplicación: puede escribir código y hacer funciones y variables, que serán partes de un objeto, disponibles dentro del elemento HTML actual.
DemoApp.controller('DemoController', function DemoController($scope, $http) {

    //******************************************************DATAGRID******************************************************

    console.log("Ingresó al método");

    //Carga de datos con petición http.
    $http.get('/api/Factura/').then(function (response) {

        console.log("Ingresó a cargar la data.");

        $scope.dataGridOptions = {
            dataSource: response.data,
            //Permite la selección de columnas.
            selection: {
                mode: "multiple"
            },
            //Exporta los datos de la grid a Excel.
            "export": {
                enabled: true,
                fileName: "Reporte" + new Date(),
                allowExportSelectedData: true
            },
            paging: {
                //Número de registros visibles en cada página.
                pageSize: 10
            },
            pager: {
                //Permite modificar la cantidad de registros por página.
                showPageSizeSelector: true,
                allowedPageSizes: [5, 10, 20],
                showInfo: true
            },
            //Columnas para cargar en la grid (el nombramiento debe ser igual al de retorno de la data).
            columns: ["Documento", {
                //Nombre de la columna.
                caption: "Fecha Doc",
                //Dato de Columna (el nombramiento debe ser igual al de retorno de la data)
                dataField: "Fecha",
                //Tipo de dato.
                dataType: "date",
                //Permite realizar las validaciones del campo de búsqueda.
                validationRules: [{
                    type: "required",
                    message: "El campo Fecha es obligatorio."
                }]
            }, {
                dataField: "FechaVence",
                dataType: "date",
                validationRules: [{
                    type: "required",
                    message: "El campo FechaVence es obligatorio."
                }]
            }, "Total", "DatosAdquiriente.RazonSocial",
               {
                   //Añade columna con contenido adicional a la grid.
                   dataField: "Documento",
                   caption: "Archivos",
                   cellTemplate: '<button onclick="eventoLink("' + "Hola" + '")">Click me</button> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a href="">XML</a>',

               }],

            //La fila de filtro permite a un usuario filtrar datos por valores de columnas individuales (ingreso de filtro y ordenamiento de columnnas).
            filterRow: {
                visible: true
            },
            //filtrar valores en una columna individual incluyéndolos / excluyéndolos en / desde el filtro aplicado (Selección de filtros)
            headerFilter: {
                visible: true
            },

        };
        console.log("DATOS DE RETORNO DE WEB API", response.data);

        console.log("Salió del método");
    });



    //******************************************************FORMULARIO CON VALIDACIONES******************************************************

    var formInstance;

    $scope.formOptions = {

        //formData: formData,
        readOnly: false,
        showColonAfterLabel: true,
        showValidationSummary: true,
        validationGroup: "customerData",
        onInitialized: function (e) {
            formInstance = e.component;
        },

        //Fomulario #1
        items: [{
            itemType: "group",
            caption: "Fomulario Login",
            items: [{
                dataField: "Usuario",
                validationRules: [{
                    type: "required",
                    message: "El usuario es requerido."
                }]
            }, {
                dataField: "Contraseña",
                editorOptions: {
                    mode: "password"
                },
                validationRules: [{
                    type: "required",
                    message: "La contraseña es requerida."
                }]
            }, {
                label: {
                    text: "Confirmación Contraseña"
                },
                editorType: "dxTextBox",
                editorOptions: {
                    mode: "password"
                },
                validationRules: [{
                    type: "required",
                    message: "La confirmación de la contraseña es requerida."
                }, {
                    type: "compare",
                    message: "'Contraseña' y 'confirmación de contraseña' no coinciden",
                    comparisonTarget: function () {
                        return formInstance.option("formData").Password;
                    }
                }]
            }]
        },


        //Fomulario #2
        {
            itemType: "group",
            //Titulo del formulario
            caption: "Datos Personales",

            //Campos formulario 2
            items: [{
                dataField: "Campo de texto sin caracteres especiales.",
                //Reglas de validación del campo.
                validationRules: [{
                    //tipo de validación.
                    type: "required",
                    //mensaje de validación.
                    message: "El nombre es requerido"
                }, {
                    type: "pattern",
                    //Validación caracteres. 
                    pattern: "^[^0-9]+$",
                    message: "El campo no debe contener números."
                }]

            }, {
                dataField: "Campo de fecha",
                editorType: "dxDateBox",
                //Texto del control.
                label: {
                    text: "Campo de fecha."
                },
                //Validación formato de fecha.
                editorOptions: {
                    invalidDateMessage: "La fecha debe tener el siguiente formato: yyyy/MM/dd."
                },
                validationRules: [
                    //Validación fecha requerida.
                    {
                        type: "required",
                        message: "La fecha es requerida."
                    },
                    //Validación minimo de edad.
                    {
                        type: "range",
                        max: new Date().setYear(new Date().getYear() - 18),
                        message: "Debe tener más de 18 años."
                    }]
            }]
            //Fin Campos furmulario 2
        },


        //Fomulario #3
        {
            itemType: "group",
            caption: "Otros Campos",
            items: [{
                dataField: "Campo de selección",
                //Tipo de control.
                editorType: "dxSelectBox",
                //Carga de datos del control de selección.
                editorOptions: {
                    dataSource: items
                },
                validationRules: [{
                    type: "required",
                    message: "El campo es requerido."
                }]
            }, {
                dataField: "Campo mínimo de caracteres",
                editorType: "dxAutocomplete",
                validationRules: [{
                    type: "pattern",
                    pattern: "^[^0-9]+$",
                    message: "El campo no debe contener números"
                },
                //Validación mínimo de caracteres 
                {
                    type: "stringLength",
                    min: 3,
                    message: "El campo debe contener al menos 3 caracteres."
                },
                //Validación campo requerido
                {
                    type: "required",
                    message: "Campo mínimo de caracteres requerido."
                }]
            }]
        }

        ]

    };

    $scope.buttonOptions = {
        text: "Guardar",
        type: "success",
        useSubmitBehavior: true,
        validationGroup: "customerData"
    };
    $scope.onFormSubmit = function (e) {
        DevExpress.ui.notify({
            message: "Presionó el botón",
            position: {
                my: "center top",
                at: "center top"
            }
        }, "success", 3000);

        e.preventDefault();
    };



    //******************************************************CAMPOS******************************************************

    var now = new Date();

    //Define los campos y las opciones
    $scope.campos = {
        //Control defecto ingreso de datos.
        Defecto: {
            placeholder: "Campo defecto",
            onValueChanged: function (data) {
                firstName = data.value;
            }
        },
        //Control con limpieza de datos.
        LimpiaDatos: {
            placeholder: "Contiene botón para limpiar el control",
            showClearButton: true,
            onValueChanged: function (data) {
                lastName = data.value;
            }
        },
        //Control Inhabilitado.
        Desabilitado: {
            placeholder: "Campo Deshabilitado",
            disabled: true
        },
        //Control CheckBox
        checked: {
            value: true
        },
        //Control únicamente fecha.
        Fecha: {
            type: "date",
            value: now
        },
        //Control únicamente hora.
        Hora: {
            type: "time",
            value: now
        },
        //Control fecha y hora.
        FechaHora: {
            type: "datetime",
            value: now
        },
        //Control única selección.
        Seleccion: {
            //Activa la busqueda en el control.
            searchEnabled: true,
            //carga los items.
            items: items
        }
    };

    //Configuración control selección multiple.
    $scope.maxDisplayedTags = {
        items: items,
        showSelectionControls: true,
        maxDisplayedTags: 3
    };



    //******************************************************BOTONES******************************************************

    //Botón Default
    $scope.okButtonOptions = {
        text: 'OK',
        type: 'normal',
        onClick: function (e) {
            DevExpress.ui.notify("The OK button was clicked");
        }
    };

    //Botón Succes.
    $scope.applyButtonOptions = {
        text: "Apply",
        type: "success",
        onClick: function (e) {
            DevExpress.ui.notify("The Apply button was clicked");
        }
    };

    //Botón Default
    $scope.doneButtonOptions = {
        text: "Done",
        type: "default",
        onClick: function (e) {
            DevExpress.ui.notify("Presionó botón Azúl.");
        }
    };

    //Botón Eliminar or Error.
    $scope.deleteButtonOptions = {
        text: "Delete",
        type: "danger",
        onClick: function (e) {
            DevExpress.ui.notify("Presionó botón Rojo.");
        }
    };

    //Botón Atrás.
    $scope.backButtonOptions = {
        type: "back",
        onClick: function (e) {
            DevExpress.ui.notify("Presionó botón atrás");
        }
    };

    //Botón con icono externo.
    $scope.sendButton = {
        icon: 'fa fa-envelope-o',
        text: "Send",
        onClick: function (e) {
            DevExpress.ui.notify("Presionó botón con icono");
        }
    };



    //******************************************************MENSAJES Y NOTIFICACIONES******************************************************

    //Popover
    $scope.defaultOptions = {
        target: "#link1",
        showEvent: "mouseenter",
        hideEvent: "mouseleave",
        position: "top",
        width: 300,
        visible: false
    };

});


//Función para abrir una modal - pop up
$(function () {
    //Contenido del pop--up
    $("#popupContainer").dxPopup({
        title: "Título de pop up"
    });
    //Botón para abrir el pop-up
    $("#buttonContainer").dxButton({
        text: "Abrir Modal",
        onClick: function () {
            $("#popupContainer").dxPopup("show");
        }
    });
});




function eventoLink(dato) {
    alert("evento");
}



var items = ['Item 1', 'Item 2', 'Item 3', 'Item 4', 'Item 5', 'Item 6', 'Item 7', 'Item 8'];
