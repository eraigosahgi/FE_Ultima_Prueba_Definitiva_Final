DevExpress.localization.locale('es-ES');

var IndicadoresApp = angular.module('IndicadoresApp', ['dx']);
IndicadoresApp.controller('IndicadoresController', function IndicadoresController($scope, $sce, $http, $location) {

    





    //Opciones Adicionales de las gráficas.
    var chartOptions = {
        gamaAzul: ["rgb(0,22,245,1)", "rgb(0,22,245,0.7)", "rgb(0,22,245,0.4)"]
    };

    var identificacion_empresa_autenticada = "";
    var usuario_autenticado = "";

    $http.get('/api/SesionDatosUsuario/').then(function (response) {

        identificacion_empresa_autenticada = response.data[0].IdentificacionEmpresa;
        usuario_autenticado = response.data[0].Usuario;

        $http.get('/api/DatosSesion/').then(function (response) {

            if (response.data[0].Administrador)
                PerfilAdministrador();

            if (response.data[0].Obligado)
                PerfilFacturador();

            if (response.data[0].Adquiriente)
                PerfilAdquiriente();

            if (response.data[0].Administrador) {
                $('#LiTabAdministrador').addClass('active');
                $('#TabAdministrador').addClass('active');
            }
            else if (!response.data[0].Administrador && response.data[0].Obligado) {
                $('#LiTabFacturador').addClass('active');
                $('#TabFacturador').addClass('active');
            }

            else if (!response.data[0].Administrador && !response.data[0].Obligado) {
                $('#LiTabAdquiriente').addClass('active');
                $('#TabAdquiriente').addClass('active');
            }

        }), function errorCallback(response) {
            Mensaje(response.data.ExceptionMessage, "error");
        };


    }), function errorCallback(response) {
        Mensaje(response.data.ExceptionMessage, "error");
    };


    function PerfilAdministrador() {

        $scope.LinkTabAdministrador = true;

        //PermisosIndicadores string codigo_usuario, string identificacion_empresa, int tipo_perfil
        $http.get('/api/PermisosIndicadores?codigo_usuario=' + usuario_autenticado + '&identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_perfil=' + 1).then(function (response) {

            var opciones_permisos = response.data;
            if (opciones_permisos.length < 1) {
                $scope.IndicadoresAdmin = false;
            } else {
                $scope.IndicadoresAdmin = true;
            }
            for (var i = 0; i < opciones_permisos.length; i++) {
                var cod_div = 'Panel' + opciones_permisos[i].Codigo;
                $scope[cod_div] = opciones_permisos[i].Consultar;
            }

            /*************************************************************************** ADMINISTRADOR ***************************************************************************/

            //REPORTE DOCUMENTOS POR ESTADO ADMINISTRADOR
            $http.get('/Api/ReporteDocumentosPorEstado?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13511 = false;
                    else {
                        $scope.ReporteDocumentosEstadoAdmin = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE DOCUMENTOS POR ESTADO ADMINISTRADOR
            $http.get('/Api/ReporteDocumentosPorEstadoCategoria?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13519 = false;
                    else {
                        $scope.ReporteDocumentosEstadoCategoriaAdmin = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });
            

            //REPORTE ACUSE MENSUAL ADMINISTRADOR
            $http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1' + '&mensual=' + true).then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13512 = false;
                    else {
                        $scope.ReporteAcuseMensualAdmin = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE ACUSE ACUMULADO ADMINISTRADOR
            $http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1' + '&mensual=' + false).then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13513 = false;
                    else {
                        $scope.ReporteAcuseAcumuladoAdmin = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE TIPO DOCUMENTO ACUMULADO ADMINISTRADOR
            $http.get('/Api/ReporteDocumentosPorTipo?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13514 = false;
                    else {
                        $scope.ReporteDocumentosTipoAdmin = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE TIPO DOCUMENTO ANUAL ADMINISTRADOR
            $http.get('/Api/ReporteDocumentosPorTipoAnual?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13515 = false;
                    else {
                        $("#ReporteTipoDocumentoAnualAdmin").dxChart({
                            palette: chartOptions.gamaAzul,
                            dataSource: response.data,
                            tooltip: {
                                enabled: true,
                                format: "largeNumber",
                                customizeTooltip: function (arg) {

                                    var datos = response.data.filter(x => x.DescripcionMes == arg.argument);

                                    var mensaje = "";

                                    if (datos.length > 0) {
                                        if (arg.seriesName == "Facturas")
                                            mensaje = "Facturas" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadFacturas + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorFacturas);
                                        else if (arg.seriesName == "Notas Crédito")
                                            mensaje = "Notas Crédito" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasCredito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasCredito);
                                        else if (arg.seriesName == "Notas Débito")
                                            mensaje = "Notas Débito" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasDebito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasDebito);
                                    }

                                    return {
                                        text: mensaje
                                    };
                                }
                            },
                            commonSeriesSettings: {
                                ignoreEmptyPoints: true,
                                argumentField: "DescripcionMes",
                                type: "bar"
                            },
                            series: [
                                 { valueField: "CantidadFacturas", name: "Facturas" },
                                 { valueField: "CantidadNotasCredito", name: "Notas Crédito" },
                                 { valueField: "CantidadNotasDebito", name: "Notas Débito" }
                            ],
                            legend: {
                                verticalAlignment: "bottom",
                                horizontalAlignment: "center"
                            },
                            title: ""
                        });
                    }

                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE VENTAS ANUALES ADMINISTRADOR
            $http.get('/Api/ReporteVentasAnuales').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13516 = false;
                    else {
                        $("#ReporteVentas").dxChart({
                            palette: chartOptions.gamaAzul,
                            dataSource: response.data,
                            tooltip: {
                                enabled: true,
                                format: "largeNumber",
                                customizeTooltip: function (arg) {

                                    var datos_reporte = response.data.filter(x => x.DescripcionMes == arg.argument);

                                    var mensaje = "";

                                    if (datos_reporte.length > 0) {

                                        if (arg.seriesName == "Cortesias")
                                            mensaje = "Cortesías" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos_reporte[0].CantidadTransaccionesCortesias;
                                        else if (arg.seriesName == "Ventas")
                                            mensaje = "Ventas" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos_reporte[0].CantidadTransaccionesVentas + "<br/>" + "Valor: " + fNumber.go(datos_reporte[0].ValorVentas);
                                        else if (arg.seriesName == "Post-Venta")
                                            mensaje = "Post-Venta" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos_reporte[0].CantidadTransaccionesPostVenta;
                                    }
                                    return {
                                        text: mensaje
                                    };
                                }
                            },
                            commonSeriesSettings: {
                                ignoreEmptyPoints: true,
                                argumentField: "DescripcionMes",
                                type: "bar"
                            },
                            series: [
                                { valueField: "CantidadTransaccionesVentas", name: "Ventas" },
                                { valueField: "CantidadTransaccionesCortesias", name: "Cortesias" },
                                { valueField: "CantidadTransaccionesPostVenta", name: "Post-Venta" }
                            ],
                            legend: {
                                verticalAlignment: "bottom",
                                horizontalAlignment: "center"
                            },
                            title: "",
                        });
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE TOP COMPRADORES ADMINISTRADOR
            $http.get('/Api/ReporteTopCompradores').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13517 = false;
                    else {
                        $("#ReporteTopCompradores").dxDataGrid({
                            dataSource: response.data,
                            showRowLines: true,
                            showBorders: true,
                            showColumnLines: false,
                            loadPanel: {
                                enabled: true
                            },
                            scrolling: {
                                mode: "virtual"
                            },
                            sorting: {
                                mode: "none"
                            },
                            onCellPrepared: function (options) {
                                var fieldData = options.value,
                                    fieldHtml = "";
                                try {
                                    if (options.columnIndex == 4) {
                                        if (fieldData) {
                                            var inicial = FormatoNumber.go(fieldData);
                                            options.cellElement.html(inicial);
                                        }
                                    }
                                    if (options.columnIndex == 3) {
                                        if (fieldData) {
                                            var inicial = fNumber.go(fieldData);
                                            options.cellElement.html(inicial);
                                        }
                                    }
                                } catch (err) {
                                    DevExpress.ui.notify(err.message, 'error', 3000);
                                }
                            },
                            onContentReady: function (e) {
                                e.component.option("loadPanel.enabled", false);
                            }, columns: [
                                {
                                    caption: "",
                                    cellTemplate: function (container, options) {

                                        if (options.data.Posicion) {

                                            var opacidad = 0;
                                            switch (options.data.Posicion) {
                                                case 1:
                                                    opacidad = "1"
                                                    break;
                                                case 2:
                                                    opacidad = "0.9"
                                                    break;
                                                case 3:
                                                    opacidad = "0.8"
                                                    break;
                                                case 4:
                                                    opacidad = "0.7"
                                                    break;
                                                case 5:
                                                    opacidad = "0.6"
                                                    break;
                                                case 6:
                                                    opacidad = "0.5"
                                                    break;
                                                case 7:
                                                    opacidad = "0.4"
                                                    break;
                                                case 8:
                                                    opacidad = "0.3"
                                                    break;
                                                case 9:
                                                    opacidad = "0.2"
                                                    break;
                                                case 10:
                                                    opacidad = "0.1"
                                                    break;
                                            }

                                            var color = "style='margin-left:5%; color: rgba(0, 22, 245, " + opacidad + ")'";
                                        }

                                        $("<div>")
                                            .append($("<label class='icon-coin-dollar' " + color + " />"))
                                            .appendTo(container);
                                    }
                                },
                                {
                                    caption: "Identificación",
                                    dataField: "Identificacion",
                                },
                                {
                                    caption: "Razón Social",
                                    dataField: "RazonSocial",
                                },
                                {
                                    caption: "Valor Compras",
                                    dataField: "ValorCompras",
                                },
                                {
                                    caption: "Cantidad Transacciones",
                                    dataField: "CantidadTransacciones",
                                }
                            ]
                        });
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE TOP TRANSACCIONAL ADMINISTRADOR
            $http.get('/Api/ReporteTopTransaccional').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13518 = false;
                    else {
                        $("#ReporteTopMovimiento").dxDataGrid({
                            dataSource: response.data,
                            showRowLines: true,
                            showBorders: true,
                            showColumnLines: false,
                            customizeColumns: function (columns) {
                                columns[0].width = 70;
                            },
                            loadPanel: {
                                enabled: true
                            },
                            scrolling: {
                                mode: "virtual"
                            },
                            sorting: {
                                mode: "none"
                            },
                            onCellPrepared: function (options) {
                                var fieldData = options.value,
                                    fieldHtml = "";
                                try {
                                    if (options.columnIndex == 3 || options.columnIndex == 4) {
                                        if (fieldData) {
                                            var inicial = FormatoNumber.go(fieldData);
                                            options.cellElement.html(inicial);
                                        }
                                    }
                                } catch (err) {
                                    DevExpress.ui.notify(err.message, 'error', 3000);
                                }
                            },
                            onContentReady: function (e) {
                                e.component.option("loadPanel.enabled", false);
                            }, columns: [
                                {
                                    caption: "",
                                    cellTemplate: function (container, options) {

                                        var indicador = "";

                                        //Decreciente
                                        if (options.data.CantidadMesActual < options.data.CantidadMesAnterior)
                                            indicador = "class='icon-stats-decline' style='margin-left:5%; color:#E70000 '";
                                            //Creciente
                                        else if (options.data.CantidadMesActual > options.data.CantidadMesAnterior)
                                            indicador = "class='icon-stats-growth' style='margin-left:5%; color:#62C415'";
                                            //Estable
                                        else if (options.data.CantidadMesActual == options.data.CantidadMesAnterior)
                                            indicador = "class='icon-sort' style='margin-left:5%; color: #2196f3'";

                                        $("<div>")
                                            .append($("<label " + indicador + " />"))
                                            .appendTo(container);
                                    }
                                },
                                {
                                    caption: "Identificación",
                                    dataField: "Identificacion",
                                },
                                {
                                    caption: "Razón Social",
                                    dataField: "RazonSocial",
                                },
                                {
                                    caption: "Mes Anterior",
                                    dataField: "CantidadMesAnterior",
                                },
                                {
                                    caption: "Mes Actual",
                                    dataField: "CantidadMesActual",
                                }
                            ]
                        });
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });


        }), function errorCallback(response) {
            Mensaje(response.data.ExceptionMessage, "error");
        };

    }

    function PerfilFacturador() {

        $scope.LinkTabFacturador = true;

        //PermisosIndicadores string codigo_usuario, string identificacion_empresa
        $http.get('/api/PermisosIndicadores?codigo_usuario=' + usuario_autenticado + '&identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_perfil=' + 2).then(function (response) {

            var opciones_permisos = response.data;
            if (opciones_permisos.length < 1) {
                $scope.IndicadoresFacturador = false;
            } else {
                $scope.IndicadoresFacturador = true;
            }
            for (var i = 0; i < opciones_permisos.length; i++) {
                var cod_div = 'Panel' + opciones_permisos[i].Codigo;
                $scope[cod_div] = opciones_permisos[i].Consultar;
            }

            /*************************************************************************** FACTURADOR ***************************************************************************/



            //REPORTE DOCUMENTOS POR ESTADO FACTURADOR
            $http.get('/Api/ReporteDocumentosPorEstadoCategoria?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13527 = false;
                    else {
                        $scope.ReporteDocumentosEstadoCategoriaFacturador = response.data;                        
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });


            /*
            //REPORTE DOCUMENTOS POR ESTADO FACTURADOR
            $http.get('/Api/ReporteDocumentosPorEstado?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13521 = false;
                    else {
                        $scope.ReporteDocumentosEstadoFacturador = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });
            */
            //REPORTE ACUSE MENSUAL FACTURADOR
            $http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2' + '&mensual=' + true).then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13522 = false;
                    else {
                        $scope.ReporteAcuseMensualFacturador = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE ACUSE ACUMULADO FACTURADOR
            $http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2' + '&mensual=' + false).then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13523 = false;
                    else {
                        $scope.ReporteAcuseAcumuladoFacturador = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE TIPO DOCUMENTO ACUMULADO FACTURADOR
            $http.get('/Api/ReporteDocumentosPorTipo?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13524 = false;
                    else {
                        $scope.ReporteDocumentosTipoFacturador = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE TIPO DOCUMENTO ANUAL FACTURADOR
            $http.get('/Api/ReporteDocumentosPorTipoAnual?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13525 = false;
                    else {
                        $("#ReporteTipoDocumentoAnualFacturador").dxChart({
                            palette: chartOptions.gamaAzul,
                            dataSource: response.data,
                            tooltip: {
                                enabled: true,
                                format: "largeNumber",
                                customizeTooltip: function (arg) {

                                    var datos = response.data.filter(x => x.DescripcionMes == arg.argument);

                                    var mensaje = "";

                                    if (datos.length > 0) {
                                        if (arg.seriesName == "Facturas")
                                            mensaje = "Facturas" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadFacturas + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorFacturas);
                                        else if (arg.seriesName == "Notas Crédito")
                                            mensaje = "Notas Crédito" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasCredito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasCredito);
                                        else if (arg.seriesName == "Notas Débito")
                                            mensaje = "Notas Débito" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasDebito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasDebito);
                                    }
                                    return {
                                        text: mensaje
                                    };
                                }
                            },
                            commonSeriesSettings: {
                                ignoreEmptyPoints: true,
                                argumentField: "DescripcionMes",
                                type: "bar"
                            },
                            series: [
                                 { valueField: "CantidadFacturas", name: "Facturas" },
                                 { valueField: "CantidadNotasCredito", name: "Notas Crédito" },
                                 { valueField: "CantidadNotasDebito", name: "Notas Débito" }
                            ],
                            legend: {
                                verticalAlignment: "bottom",
                                horizontalAlignment: "center"
                            },
                            title: ""
                        });
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE RESUMEN TRANSACCIONAL
            $http.get('/Api/ReporteResumenPlanesAdquiridos?identificacion_empresa=' + identificacion_empresa_autenticada).then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13526 = false;
                    else {
                        $scope.TransaccionesAdquiridas = response.data[0].TransaccionesAdquiridas;
                        $scope.TransaccionesProcesadas = response.data[0].TransaccionesProcesadas;
                        $scope.TransaccionesDisponibles = response.data[0].TransaccionesDisponibles;
                        $("#ResumenPlanesAdquiridosFacturador").dxDataGrid({
                            dataSource: response.data[0].PlanesAdquiridos,
                            showBorders: false,
                            showRowLines: false,
                            showColumnLines: false,
                            rowAlternationEnabled: true,
                            noDataText: "No se encontraron Planes Transaccionales Registrados.",
                            paging: {
                                pageSize: 5
                            },
                            pager: {
                                showInfo: true
                            },
                            columns: [
                                {
                                    caption: "Fecha Compra",
                                    dataField: "DatFecha",
                                    dataType: "date",
                                    format: "yyyy-MM-dd",
                                },
                                {
                                    caption: "Transacciones Plan",
                                    dataField: "IntNumTransaccCompra",
                                },
                                {
                                    caption: "Transacciones Procesadas",
                                    dataField: "IntNumTransaccProcesadas",
                                },
                                {
                                    caption: "Fecha Vencimiento",
                                    dataField: "DatFechaVencimiento",
                                    dataType: "date",
                                    format: "yyyy-MM-dd",
                                }
                            ]
                        });
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

        }), function errorCallback(response) {
            Mensaje(response.data.ExceptionMessage, "error");
        };
    }

    function PerfilAdquiriente() {

        $scope.LinkTabAdquiriente = true;

        //PermisosIndicadores string codigo_usuario, string identificacion_empresa
        $http.get('/api/PermisosIndicadores?codigo_usuario=' + usuario_autenticado + '&identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_perfil=' + 3).then(function (response) {

            var opciones_permisos = response.data;
            if (opciones_permisos.length < 1) {
                $scope.IndicadoresAdquiriente = false;
            } else {
                $scope.IndicadoresAdquiriente = true;
            }
            for (var i = 0; i < opciones_permisos.length; i++) {
                var cod_div = 'Panel' + opciones_permisos[i].Codigo;
                $scope[cod_div] = opciones_permisos[i].Consultar;
            }

            /*************************************************************************** ADQUIRIENTE ***************************************************************************/
            //REPORTE ACUSE ACUMULADO ADQUIRIENTE
            $http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '3' + '&mensual=' + false).then(function (response) {
                $("#wait").hide();
                try {

                    if (response.data.length == 0)
                        $scope.Panel13531 = false;
                    else {
                        $scope.ReporteAcuseAcumuladoAdquiriente = response.data;
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE TIPO DOCUMENTO ANUAL ADQUIRIENTE
            $http.get('/Api/ReporteDocumentosPorTipoAnual?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '3').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13532 = false;
                    else {
                        $("#ReporteTipoDocumentoAnualAdquiriente").dxChart({
                            palette: chartOptions.gamaAzul,
                            dataSource: response.data,
                            tooltip: {
                                enabled: true,
                                format: "largeNumber",
                                customizeTooltip: function (arg) {

                                    var datos = response.data.filter(x => x.DescripcionMes == arg.argument);

                                    var mensaje = "";

                                    if (datos.length > 0) {
                                        if (arg.seriesName == "Facturas")
                                            mensaje = "Facturas" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadFacturas + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorFacturas);
                                        else if (arg.seriesName == "Notas Crédito")
                                            mensaje = "Notas Crédito" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasCredito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasCredito);
                                        else if (arg.seriesName == "Notas Débito")
                                            mensaje = "Notas Débito" + "<br/>" + "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasDebito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasDebito);
                                    }
                                    return {
                                        text: mensaje
                                    };
                                }
                            },
                            commonSeriesSettings: {
                                ignoreEmptyPoints: true,
                                argumentField: "DescripcionMes",
                                type: "bar"
                            },
                            series: [
                                 { valueField: "CantidadFacturas", name: "Facturas" },
                                 { valueField: "CantidadNotasCredito", name: "Notas Crédito" },
                                 { valueField: "CantidadNotasDebito", name: "Notas Débito" }
                            ],
                            legend: {
                                verticalAlignment: "bottom",
                                horizontalAlignment: "center"
                            },
                            title: ""
                        });
                    }
                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

            //REPORTE TIPO DOCUMENTO ACUMULADO ADQUIRIENTE
            $http.get('/Api/ReporteDocumentosPorTipo?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '3').then(function (response) {
                $("#wait").hide();
                try {
                    if (response.data.length == 0)
                        $scope.Panel13533 = false;
                    else {
                        $scope.ReporteDocumentosTipoAdquiriente = response.data;
                    }

                } catch (err) {
                    DevExpress.ui.notify(err.message, 'error', 3000);
                }
            }, function errorCallback(response) {
                $('#wait').hide();
                DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
            });

        }), function errorCallback(response) {
            Mensaje(response.data.ExceptionMessage, "error");
        };
    }

    

    /*************************************************************************** CARGA PORCENTAJES ADMINISTRADOR ***************************************************************************/

    $scope.CargarDocumentosEstadoCategoriaAdmin = function () {
        var Indicador = $scope.ReporteDocumentosEstadoCategoriaAdmin
       // <label id="totaldocestado">Total Documentos</label>
        //$('#totaldocestado').text("Total Documentos: " + Indicador[0].Cantidad);
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }


    $scope.CargarDocumentosEstadoAdmin = function () {
        var Indicador = $scope.ReporteDocumentosEstadoAdmin
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarAcuseMensualAdmin = function () {
        var Indicador = $scope.ReporteAcuseMensualAdmin
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarAcuseAcumuladoAdmin = function () {
        var Indicador = $scope.ReporteAcuseAcumuladoAdmin
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarDocumentosTipoAdmin = function () {
        var Indicador = $scope.ReporteDocumentosTipoAdmin
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    /*************************************************************************** CARGA PORCENTAJES FACTURADOR ***************************************************************************/

    $scope.CargarDocumentosEstadoCategoriaFacturador = function () {
        var Indicador = $scope.ReporteDocumentosEstadoCategoriaFacturador
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)            
        }              
    }

    /*
    $scope.CargarDocumentosEstadoFacturador = function () {
        var Indicador = $scope.ReporteDocumentosEstadoFacturador
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }
    */
    $scope.CargarAcuseMensualFacturador = function () {
        var Indicador = $scope.ReporteAcuseMensualFacturador
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarAcuseAcumuladoFacturador = function () {
        var Indicador = $scope.ReporteAcuseAcumuladoFacturador
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarDocumentosTipoFacturador = function () {
        var Indicador = $scope.ReporteDocumentosTipoFacturador
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    /*************************************************************************** CARGA PORCENTAJES ADQUIRIENTE ***************************************************************************/

    $scope.CargarAcuseAcumuladoAdquiriente = function () {
        var Indicador = $scope.ReporteAcuseAcumuladoAdquiriente
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarDocumentosTipoAdquiriente = function () {
        var Indicador = $scope.ReporteDocumentosTipoAdquiriente
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 38, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }



    
});


