DevExpress.localization.locale('es-ES');

var IndicadoresApp = angular.module('IndicadoresApp', ['dx']);
IndicadoresApp.controller('IndicadoresController', function IndicadoresController($scope, $sce, $http, $location) {

    var IndicadoresPorcentuales;



    //Opciones Adicionales de las gráficas.
    var chartOptions = {
        gamaAzul: ["rgb(0,22,245,1)", "rgb(0,22,245,0.7)", "rgb(0,22,245,0.4)"]
    };

    //Construye el indicador de porcentajes
    $scope.htmlTrusted = function (id, color, porcentaje, titulo, descripcion) {
        return $sce.trustAsHtml(PorcentajeGrafico('#' + id, 32, 4, color, porcentaje, "icon-file-download2", color, titulo, descripcion));
    }

    var identificacion_empresa_autenticada = "";

    $http.get('/api/DatosSesion/').then(function (response) {

        identificacion_empresa_autenticada = response.data[0].Identificacion;

        /*************************************************************************** ADQUIRIENTE ***************************************************************************/
        //REPORTE ACUSE ACUMULADO ADQUIRIENTE
        $http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '3' + '&mensual=' + false).then(function (response) {
            $("#wait").hide();
            try {

                $scope.ReporteAcuseAcumuladoAdquiriente = response.data;

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
                                    mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadFacturas + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorFacturas);
                                else if (arg.seriesName == "NotasCredito")
                                    mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasCredito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasCredito);
                                else if (arg.seriesName == "CantidadNotasDebito")
                                    mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasDebito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasDebito);
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
                         { valueField: "CantidadNotasCredito", name: "NotasCredito" },
                         { valueField: "CantidadNotasDebito", name: "NotasDebito" }
                    ],
                    legend: {
                        verticalAlignment: "bottom",
                        horizontalAlignment: "center"
                    },
                    title: ""
                });


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

                $scope.ReporteDocumentosTipoAdquiriente = response.data;

            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 3000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });
        /*************************************************************************** FACTURADOR ***************************************************************************/

        //REPORTE DOCUMENTOS POR ESTADO FACTURADOR
        $http.get('/Api/ReporteDocumentosPorEstado?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2').then(function (response) {
            $("#wait").hide();
            try {

                $scope.ReporteDocumentosEstadoFacturador = response.data;

            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 3000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });

        //REPORTE ACUSE MENSUAL FACTURADOR
        $http.get('/Api/ReporteEstadosAcuse?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '2' + '&mensual=' + true).then(function (response) {
            $("#wait").hide();
            try {

                $scope.ReporteAcuseMensualFacturador = response.data;

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

                $scope.ReporteAcuseAcumuladoFacturador = response.data;

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

                $scope.ReporteDocumentosTipoFacturador = response.data;

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
                                    mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadFacturas + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorFacturas);
                                else if (arg.seriesName == "NotasCredito")
                                    mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasCredito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasCredito);
                                else if (arg.seriesName == "CantidadNotasDebito")
                                    mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasDebito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasDebito);
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
                         { valueField: "CantidadNotasCredito", name: "NotasCredito" },
                         { valueField: "CantidadNotasDebito", name: "NotasDebito" }
                    ],
                    legend: {
                        verticalAlignment: "bottom",
                        horizontalAlignment: "center"
                    },
                    title: ""
                });


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
                $scope.SaldoPlanActual = response.data[0].SaldoPlanActual;
                $scope.SaldoConsumoPlanActual = response.data[0].SaldoConsumoPlanActual;
                $scope.SaldoCompras = response.data[0].SaldoCompras;
                $scope.SaldoDisponible = response.data[0].SaldoDisponible;


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
                            dataField: "FechaCompra",
                            dataType: "date",
                            format: "yyyy-MM-dd HH:mm",
                        },
                        {
                            caption: "Transacciones Plan",
                            dataField: "CantidadCompradas",
                        },
                        {
                            caption: "Transacciones Procesadas",
                            dataField: "CantidadProcesadas",
                        },
                        {
                            caption: "Fecha Vencimiento",
                            dataField: "FechaVencimiento",
                            dataType: "date",
                            format: "yyyy-MM-dd HH:mm",
                        }
                    ]
                });

            } catch (err) {
                DevExpress.ui.notify(err.message, 'error', 3000);
            }
        }, function errorCallback(response) {
            $('#wait').hide();
            DevExpress.ui.notify(response.data.ExceptionMessage, 'error', 3000);
        });

        /*************************************************************************** ADMINISTRADOR ***************************************************************************/

        //REPORTE DOCUMENTOS POR ESTADO ADMINISTRADOR
        $http.get('/Api/ReporteDocumentosPorEstado?identificacion_empresa=' + identificacion_empresa_autenticada + '&tipo_empresa=' + '1').then(function (response) {
            $("#wait").hide();
            try {
                $scope.ReporteDocumentosEstadoAdmin = response.data;

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

                $scope.ReporteAcuseMensualAdmin = response.data;

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

                $scope.ReporteAcuseAcumuladoAdmin = response.data;

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

                $scope.ReporteDocumentosTipoAdmin = response.data;

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
                                if (arg.seriesName == "CantidadFacturas")
                                    mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadFacturas + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorFacturas);
                                else if (arg.seriesName == "CantidadNotasCredito")
                                    mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasCredito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasCredito);
                                else if (arg.seriesName == "CantidadNotasDebito")
                                    mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].CantidadNotasDebito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotasDebito);
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
                         { valueField: "CantidadFacturas", name: "CantidadFacturas" },
                         { valueField: "CantidadNotasCredito", name: "CantidadNotasCredito" },
                         { valueField: "CantidadNotasDebito", name: "CantidadNotasDebito" }
                    ],
                    legend: {
                        verticalAlignment: "bottom",
                        horizontalAlignment: "center"
                    },
                    title: ""
                });


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

    /*************************************************************************** CARGA PORCENTAJES ADMINISTRADOR ***************************************************************************/
    $scope.CargarDocumentosEstadoAdmin = function () {
        var Indicador = $scope.ReporteDocumentosEstadoAdmin
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarAcuseMensualAdmin = function () {
        var Indicador = $scope.ReporteAcuseMensualAdmin
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarAcuseAcumuladoAdmin = function () {
        var Indicador = $scope.ReporteAcuseAcumuladoAdmin
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarDocumentosTipoAdmin = function () {
        var Indicador = $scope.ReporteDocumentosTipoAdmin
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    /*************************************************************************** CARGA PORCENTAJES FACTURADOR ***************************************************************************/

    $scope.CargarDocumentosEstadoFacturador = function () {
        var Indicador = $scope.ReporteDocumentosEstadoFacturador
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }
    
    $scope.CargarAcuseMensualFacturador = function () {
        var Indicador = $scope.ReporteAcuseMensualFacturador
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }
    
    $scope.CargarAcuseAcumuladoFacturador = function () {
        var Indicador = $scope.ReporteAcuseAcumuladoFacturador
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarDocumentosTipoFacturador = function () {
        var Indicador = $scope.ReporteDocumentosTipoFacturador
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    /*************************************************************************** CARGA PORCENTAJES ADQUIRIENTE ***************************************************************************/

    $scope.CargarAcuseAcumuladoAdquiriente = function () {
        var Indicador = $scope.ReporteAcuseAcumuladoAdquiriente
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    $scope.CargarDocumentosTipoAdquiriente = function () {
        var Indicador = $scope.ReporteDocumentosTipoAdquiriente
        for (var i = 0; i < Indicador.length; i++) {
            PorcentajeGrafico('#' + Indicador[i].IdControl, 32, 4, Indicador[i].Color, Indicador[i].Porcentaje, "icon-file-download2", Indicador[i].Color, Indicador[i].Titulo, Indicador[i].Observaciones)
        }
    }

    /*

    //********************************** DATOS DOCUMENTOS POR RESPUESTA ACUSE **********************************
    var datosReporteMesActualEstadoAcuse = [{
        Estado: "Aprobados",
        Cantidad: 150,
        Color: "#000000",
    }, {
        Estado: "Rechazados",
        Cantidad: 80,
        Color: "#ffffff",
    }, {
        Estado: "Pendientes",
        Cantidad: 30,
        Color: "#FF2D00",
    }];

    var TotalReporteMesActualEstadoAcuse = 0;

    for (var i = 0; i < datosReporteMesActualEstadoAcuse.length; i++) {
        TotalReporteMesActualEstadoAcuse += datosReporteMesActualEstadoAcuse[i].Cantidad;

    }

    var datosReporteAcumuladoEstadoAcuse = [{
        Estado: "Aprobados",
        Cantidad: 50,
        Color: "#000000",
    }, {
        Estado: "Rechazados",
        Cantidad: 30,
        Color: "#ffffff",
    }, {
        Estado: "Pendientes",
        Cantidad: 120,
        Color: "#FF2D00",
    }];

    var TotalReporteAcumuladoEstadoAcuse = 0;

    for (var i = 0; i < datosReporteAcumuladoEstadoAcuse.length; i++) {
        TotalReporteAcumuladoEstadoAcuse += datosReporteAcumuladoEstadoAcuse[i].Cantidad;
    }

    //********************************** DATOS DOCUMENTOS POR TIPO ANUAL **********************************
    var datosReporteTipoDocumentoAnual = [
        {
            Mes: "Enero",
            Facturas: 200,
            ValorFactura: 12000000,
            NotasCredito: 45,
            ValorNotaCredito: 1850000,
            NotasDebito: 40,
            ValorNotaDebido: 1850000
        }, {
            Mes: "Febrero",
            Facturas: 200,
            ValorFactura: 13000000,
            NotasCredito: 45,
            ValorNotaCredito: 3500000,
            NotasDebito: 40,
            ValorNotaDebido: 3100000
        }, {
            Mes: "Marzo",
            Facturas: 200,
            ValorFactura: 10000000,
            NotasCredito: 45,
            ValorNotaCredito: 2583000,
            NotasDebito: 40,
            ValorNotaDebido: 1250000
        }, {
            Mes: "Abril",
            Facturas: 200,
            ValorFactura: 1500000,
            NotasCredito: 45,
            ValorNotaCredito: 150000,
            NotasDebito: 40,
            ValorNotaDebido: 150000
        }, {
            Mes: "Mayo",
            Facturas: 200,
            ValorFactura: 11600000,
            NotasCredito: 45,
            ValorNotaCredito: 1990000,
            NotasDebito: 40,
            ValorNotaDebido: 1580000
        }, {
            Mes: "Junio",
            Facturas: 200,
            ValorFactura: 9000000,
            NotasCredito: 45,
            ValorNotaCredito: 150000,
            NotasDebito: 40,
            ValorNotaDebido: 150000
        }, {
            Mes: "Julio",
            Facturas: 200,
            ValorFactura: 2000000,
            NotasCredito: 45,
            ValorNotaCredito: 150000,
            NotasDebito: 40,
            ValorNotaDebido: 150000
        }
    ];

    //********************************** REPORTE DE VENTAS **********************************
    var datosReporteVentas = [
        {
            Mes: "Enero",
            CantidadCortesia: 1500,
            CantidadVentas: 145000,
            ValorVentas: 32000000
        },
        {
            Mes: "Febrero",
            CantidadCortesia: 1800,
            CantidadVentas: 100000,
            ValorVentas: 28000000
        },
        {
            Mes: "Marzo",
            CantidadCortesia: 2000,
            CantidadVentas: 80000,
            ValorVentas: 15000000
        },
        {
            Mes: "Abril",
            CantidadCortesia: 3500,
            CantidadVentas: 20000,
            ValorVentas: 8000000
        },
        {
            Mes: "Mayo",
            CantidadCortesia: 15800,
            CantidadVentas: 140000,
            ValorVentas: 30000000
        }

    ];

    //********************************** REPORTE DE TOP COMPRADORES **********************************
    var datosReporteTopCompradores = [
        {
            Identificacion: "811021438",
            RazonSocial: "HGI SAS",
            Posicion: 1,
            CantidadTransacciones: 160000,
        },
        {
            Identificacion: "12345678",
            RazonSocial: "JEFERSON FLORES",
            Posicion: 2,
            CantidadTransacciones: 152000,
        },
        {
            Identificacion: "1152708377",
            RazonSocial: "ANA MARÍA TAMAYO RODRÍGUEZ",
            Posicion: 3,
            CantidadTransacciones: 151000,
        },
        {
            Identificacion: "78958454",
            RazonSocial: "JHON STIVENS ZEA",
            Posicion: 4,
            CantidadTransacciones: 150000,
        },
        {
            Identificacion: "25875800",
            RazonSocial: "JOAN SALAZAR",
            Posicion: 5,
            CantidadTransacciones: 148000,
        },
        {
            Identificacion: "4587000125",
            RazonSocial: "ELKIN ALVAREZ",
            Posicion: 6,
            CantidadTransacciones: 142000,
        },
        {
            Identificacion: "254578600",
            RazonSocial: "MAURICIO PARAMO",
            Posicion: 7,
            CantidadTransacciones: 140000,
        },

        {
            Identificacion: "45622580",
            RazonSocial: "CLIENTE GENERICO1",
            Posicion: 8,
            CantidadTransacciones: 142010,
        },

        {
            Identificacion: "455852555",
            RazonSocial: "CLIENTE GENERICO2",
            Posicion: 9,
            CantidadTransacciones: 132000,
        },

        {
            Identificacion: "25770003",
            RazonSocial: "CLIENTE GENERICO3",
            Posicion: 10,
            CantidadTransacciones: 102000,
        }
    ];

    //********************************** REPORTE DE TOP MOVIMIENTO **********************************
    var datosReporteTopTransacciones = [
        {
            Identificacion: "811021438",
            RazonSocial: "HGI SAS",
            Posicion: 1,
            CantidadMesAnterior: 125000,
            CantidadMesActual: 125000,
        },
        {
            Identificacion: "12345678",
            RazonSocial: "JEFERSON FLORES",
            Posicion: 2,
            CantidadMesAnterior: 125000,
            CantidadMesActual: 25000,
        },
        {
            Identificacion: "1152708377",
            RazonSocial: "ANA MARÍA TAMAYO RODRÍGUEZ",
            Posicion: 3,
            CantidadMesAnterior: 125000,
            CantidadMesActual: 123000,
        },
        {
            Identificacion: "78958454",
            RazonSocial: "JHON STIVENS ZEA",
            Posicion: 4,
            CantidadMesAnterior: 125000,
            CantidadMesActual: 125000,
        },
        {
            Identificacion: "25875800",
            RazonSocial: "JOAN SALAZAR",
            Posicion: 5,
            CantidadMesAnterior: 125000,
            CantidadMesActual: 105000,
        },
        {
            Identificacion: "4587000125",
            RazonSocial: "ELKIN ALVAREZ",
            Posicion: 6,
            CantidadMesAnterior: 50410,
            CantidadMesActual: 51400,
        },
        {
            Identificacion: "254578600",
            RazonSocial: "MAURICIO PARAMO",
            Posicion: 7,
            CantidadMesAnterior: 10000,
            CantidadMesActual: 14400,
        },

        {
            Identificacion: "45622580",
            RazonSocial: "CLIENTE GENERICO1",
            Posicion: 8,
            CantidadMesAnterior: 10400,
            CantidadMesActual: 14400,
        },

        {
            Identificacion: "455852555",
            RazonSocial: "CLIENTE GENERICO2",
            Posicion: 9,
            CantidadMesAnterior: 15400,
            CantidadMesActual: 14400,
        },

        {
            Identificacion: "25770003",
            RazonSocial: "CLIENTE GENERICO3",
            Posicion: 10,
            CantidadMesAnterior: 15400,
            CantidadMesActual: 14400,
        }
    ];

    //********************************** DATOS DOCUMENTOS POR RESPUESTA ACUSE **********************************
    var datosReporteAcumuladoTipoDocumento = [{
        TipoDocumento: "Facturas",
        Cantidad: 1500,
        Color: "#000000",
    }, {
        TipoDocumento: "Notas Crédito",
        Cantidad: 800,
        NotasDebito: "#ffffff",
    }, {
        TipoDocumento: "Notas Débito",
        Cantidad: 800,
        NotasDebito: "#ffffff",
    }
    ];

    //***************************************************************************** PERFIL ADMINISTRADOR ****************************************************************************

    //ReporteMesActualEstadoAcuse
    $(function () {
        $("#ReporteMesActualEstadoAcuse").dxPieChart({
            palette: chartOptions.paleta,
            dataSource: datosReporteMesActualEstadoAcuse,
            title: "",
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var porcentaje = ((arg.originalValue / TotalReporteMesActualEstadoAcuse) * 100).toFixed(2);

                    return {
                        text: "Estado: " + arg.argumentText + "<br/>" + "Cantidad Documentos: " + arg.originalValue + "<br/>" + "Porcentaje: " + porcentaje + "%"
                    };
                }
            },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "center",
                verticalAlignment: "bottom",
            },
            series: [{
                name: "Porcentaje",
                argumentField: "Estado",
                valueField: "Cantidad"
            }]
        });
    });

    //ReporteAcumuladoEstadoAcuse
    $(function () {
        $("#ReporteAcumuladoEstadoAcuse").dxPieChart({
            palette: chartOptions.paleta,
            dataSource: datosReporteAcumuladoEstadoAcuse,
            title: "",
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var porcentaje = ((arg.originalValue / TotalReporteAcumuladoEstadoAcuse) * 100).toFixed(2);

                    return {
                        text: "Estado: " + arg.argumentText + "<br/>" + "Cantidad Documentos: " + arg.originalValue + "<br/>" + "Porcentaje: " + porcentaje + "%"
                    };
                }
            },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "center",
                verticalAlignment: "bottom",
            },
            series: [{
                name: "Porcentaje",
                argumentField: "Estado",
                valueField: "Cantidad",
            }]
        });
    });

    //ReporteTipoDocumentoAnual
    $(function () {
        $("#ReporteTipoDocumentoAnual").dxChart({
            palette: chartOptions.gamaAzul,
            dataSource: datosReporteTipoDocumentoAnual,
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var datos = datosReporteTipoDocumentoAnual.filter(x => x.Mes == arg.argument);

                    var mensaje = "";

                    if (datos.length > 0) {

                        if (arg.seriesName == "Facturas")
                            mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].Facturas + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorFactura);
                        else if (arg.seriesName == "NotasCredito")
                            mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].NotasCredito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotaCredito);
                        else if (arg.seriesName == "NotasDebido")
                            mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].NotasDebito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotaDebido);
                    }

                    return {
                        text: mensaje
                    };
                }
            },
            commonSeriesSettings: {
                ignoreEmptyPoints: true,
                argumentField: "Mes",
                type: "bar"
            },
            series: [
                { valueField: "ValorFactura", name: "Facturas" },
                 { valueField: "ValorNotaCredito", name: "NotasCredito" },
                 { valueField: "ValorNotaDebido", name: "NotasDebido" }
            ],
            legend: {
                verticalAlignment: "bottom",
                horizontalAlignment: "center"
            },
            title: "",
        });
    });

    //ReporteVentas
    $(function () {
        $("#ReporteVentas").dxChart({
            palette: chartOptions.paleta,
            dataSource: datosReporteVentas,
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {


                    var opc_permiso = datosReporteVentas.filter(x => x.Mes == arg.argument);

                    var mensaje = "";

                    if (opc_permiso.length > 0) {

                        if (arg.seriesName == "CantidadCortesia")
                            mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + opc_permiso[0].CantidadCortesia;
                        else if (arg.seriesName == "CantidadVentas")
                            mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + opc_permiso[0].CantidadVentas + "<br/>" + "Valor: " + fNumber.go(opc_permiso[0].ValorVentas);
                    }
                    return {
                        text: mensaje
                    };
                }
            },
            commonSeriesSettings: {
                ignoreEmptyPoints: true,
                argumentField: "Mes",
                type: "bar"
            },
            series: [
                { valueField: "CantidadVentas", name: "CantidadVentas" },
                { valueField: "CantidadCortesia", name: "CantidadCortesia" }
            ],
            legend: {
                verticalAlignment: "bottom",
                horizontalAlignment: "center"
            },
            title: "",
        });
    });

    //ReporteTopCompradores
    $(function () {
        $("#ReporteTopCompradores").dxDataGrid({
            dataSource: datosReporteTopCompradores,
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
                    if (options.columnIndex == 3) {
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
                    caption: "Cantidad Transacciones",
                    dataField: "CantidadTransacciones",
                },
            ]
        });

    });

    //ReporteTopMovimiento
    $(function () {
        $("#ReporteTopMovimiento").dxDataGrid({
            dataSource: datosReporteTopTransacciones,
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

    });

    //ReporteAcumuladoTipoDocumento
    $(function () {
        $("#ReporteAcumuladoTipoDocumento").dxPieChart({
            palette: chartOptions.gamaAzul,
            dataSource: datosReporteAcumuladoTipoDocumento,
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var porcentaje = ((arg.originalValue / TotalReporteMesActualEstadoAcuse) * 100).toFixed(2);

                    return {
                        text: "Tipo Documento: " + arg.argumentText + "<br/>" + "Cantidad Documentos: " + arg.originalValue + "<br/>" + "Porcentaje: " + porcentaje + "%"
                    };
                }
            },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "center",
                verticalAlignment: "bottom",
            },
            series: [{
                name: "Porcentaje",
                argumentField: "TipoDocumento",
                valueField: "Cantidad"
            }]
        });
    });

    //Carga la información de los documentos pendientes por procesar hasta el día actual.
    PorcentajeGrafico('#ReporteFlujoDiarioPendientes', 32, 4, "#717171", 0.50, "icon-spam", "#717171", 'Pendientes', '5 Documentos');
    //Carga la información de los documentos procesados el día actual.
    PorcentajeGrafico('#ReporteFlujoDiarioProcesados', 32, 4, "#62C415", 0.60, "icon-checkmark4", "#62C415", 'Procesados Hoy', '25 Documentos');


    //Carga la información de los documentos pendientes por procesar hasta el día actual.
    PorcentajeGrafico('#ReporteDocumentosRechazados', 32, 4, "#E70000", 0.10, "icon-cross2", "#E70000", 'Rechazados', '5 Documentos');
    //Carga la información de los documentos procesados el día actual.
    PorcentajeGrafico('#ReporteDocumentosAprobados', 32, 4, "#62C415", 0.90, "icon-checkmark4", "#62C415", 'Aprobados', '25 Documentos');


    //***************************************************************************** PERFIL FACTURADOR ****************************************************************************

    //ReporteMesActualEstadoAcuseFacturador
    $(function () {
        $("#ReporteMesActualEstadoAcuseFacturador").dxPieChart({
            palette: chartOptions.paleta,
            dataSource: datosReporteMesActualEstadoAcuse,
            title: "",
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var porcentaje = ((arg.originalValue / TotalReporteMesActualEstadoAcuse) * 100).toFixed(2);

                    return {
                        text: "Estado: " + arg.argumentText + "<br/>" + "Cantidad Documentos: " + arg.originalValue + "<br/>" + "Porcentaje: " + porcentaje + "%"
                    };
                }
            },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "center",
                verticalAlignment: "bottom",
            },
            series: [{
                name: "Porcentaje",
                argumentField: "Estado",
                valueField: "Cantidad"
            }]
        });
    });

    //ReporteAcumuladoEstadoAcuseFacturador
    $(function () {
        $("#ReporteAcumuladoEstadoAcuseFacturador").dxPieChart({
            palette: chartOptions.paleta,
            dataSource: datosReporteAcumuladoEstadoAcuse,
            title: "",
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var porcentaje = ((arg.originalValue / TotalReporteAcumuladoEstadoAcuse) * 100).toFixed(2);

                    return {
                        text: "Estado: " + arg.argumentText + "<br/>" + "Cantidad Documentos: " + arg.originalValue + "<br/>" + "Porcentaje: " + porcentaje + "%"
                    };
                }
            },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "center",
                verticalAlignment: "bottom",
            },
            series: [{
                name: "Porcentaje",
                argumentField: "Estado",
                valueField: "Cantidad",
            }]
        });
    });

    //ReporteAcumEstadoAcuseAdquiriente
    $(function () {
        $("#ReporteAcumEstadoAcuseAdquiriente").dxPieChart({
            palette: chartOptions.paleta,
            dataSource: datosReporteAcumuladoEstadoAcuse,
            title: "",
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var porcentaje = ((arg.originalValue / TotalReporteAcumuladoEstadoAcuse) * 100).toFixed(2);

                    return {
                        text: "Estado: " + arg.argumentText + "<br/>" + "Cantidad Documentos: " + arg.originalValue + "<br/>" + "Porcentaje: " + porcentaje + "%"
                    };
                }
            },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "center",
                verticalAlignment: "bottom",
            },
            series: [{
                name: "Porcentaje",
                argumentField: "Estado",
                valueField: "Cantidad",
            }]
        });
    });

    //ReporteAcumuladoTipoDocumentoFacturador
    $(function () {
        $("#ReporteAcumuladoTipoDocumentoFacturador").dxPieChart({
            palette: chartOptions.gamaAzul,
            dataSource: datosReporteAcumuladoTipoDocumento,
            title: "",
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var porcentaje = ((arg.originalValue / TotalReporteMesActualEstadoAcuse) * 100).toFixed(2);

                    return {
                        text: "Tipo Documento: " + arg.argumentText + "<br/>" + "Cantidad Documentos: " + arg.originalValue + "<br/>" + "Porcentaje: " + porcentaje + "%"
                    };
                }
            },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "center",
                verticalAlignment: "bottom",
            },
            series: [{
                name: "Porcentaje",
                argumentField: "TipoDocumento",
                valueField: "Cantidad"
            }]
        });
    });

    $scope.ReporteDocumentosEstados = [
       {
           ID: "RecibidosFacturador",
           Color: "#EEE713",
           Porcentaje: 1,
           Icono: "icon-file-download2",
           Titulo: "Recibidos",
           Descripcion: "50 Documentos"
       }, {
           ID: "GeneracionUblFacturador",
           Color: "#EEE713",
           Porcentaje: 0.90,
           Icono: "icon-user",
           Titulo: "Generación UBL",
           Descripcion: "50 Documentos"
       }, {
           ID: "FirmadoUblFacturadorFirmadoUbl",
           Color: "#EEE713",
           Porcentaje: 0.90,
           Icono: "icon-file-download2",
           Titulo: "Firmado UBL",
           Descripcion: "50 Documentos"
       }, {
           ID: "CompresionXmlFacturador",
           Color: "#EEE713",
           Porcentaje: 0.90,
           Icono: "icon-file-download2",
           Titulo: "Compresión XML",
           Descripcion: "50 Documentos"
       }, {
           ID: "EnvioDianFacturador",
           Color: "#EEE713",
           Porcentaje: 0.88,
           Icono: "icon-file-download2",
           Titulo: "Envío DIAN",
           Descripcion: "50 Documentos"
       }, {
           ID: "EnvioAdquirienteFacturador",
           Color: "#EEE713",
           Porcentaje: 0.88,
           Icono: "icon-file-download2",
           Titulo: "Envío Adquiriente",
           Descripcion: "50 Documentos"
       }, {
           ID: "PendienteAcuseFacturador",
           Color: "#EEE713",
           Porcentaje: 0.12,
           Icono: "icon-file-download2",
           Titulo: "Pendiente Acuse",
           Descripcion: "50 Documentos"
       }, {
           ID: "EnvioRespuestaFacturador",
           Color: "#EEE713",
           Porcentaje: 0.88,
           Icono: "icon-file-download2",
           Titulo: "Envío Respuesta",
           Descripcion: "50 Documentos"
       }, {
           ID: "FinalizadosFacturador",
           Color: "#62C415",
           Porcentaje: 0.88,
           Icono: "icon-file-download2",
           Titulo: "Finalizados",
           Descripcion: "50 Documentos"
       }
    ];


    //***************************************************************************** PERFIL ADQUIRIENTE ****************************************************************************

    //ReporteAcumEstadoAcuseAdquiriente
    $(function () {
        $("#ReporteAcumEstadoAcuseAdquiriente").dxPieChart({
            palette: chartOptions.paleta,
            dataSource: datosReporteAcumuladoEstadoAcuse,
            title: "",
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var porcentaje = ((arg.originalValue / TotalReporteAcumuladoEstadoAcuse) * 100).toFixed(2);

                    return {
                        text: "Estado: " + arg.argumentText + "<br/>" + "Cantidad Documentos: " + arg.originalValue + "<br/>" + "Porcentaje: " + porcentaje + "%"
                    };
                }
            },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "center",
                verticalAlignment: "bottom",
            },
            series: [{
                name: "Porcentaje",
                argumentField: "Estado",
                valueField: "Cantidad",
            }]
        });
    });

    //ReporteTipoDocumentoAnualAdquiriente
    $(function () {
        $("#ReporteTipoDocumentoAnualAdquiriente").dxChart({
            palette: chartOptions.gamaAzul,
            dataSource: datosReporteTipoDocumentoAnual,
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var datos = datosReporteTipoDocumentoAnual.filter(x => x.Mes == arg.argument);

                    var mensaje = "";

                    if (datos.length > 0) {

                        if (arg.seriesName == "Facturas")
                            mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].Facturas + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorFactura);
                        else if (arg.seriesName == "NotasCredito")
                            mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].NotasCredito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotaCredito);
                        else if (arg.seriesName == "NotasDebido")
                            mensaje = "Mes: " + arg.argument + "<br/>" + "Cantidad: " + datos[0].NotasDebito + "<br/>" + "Valor: " + fNumber.go(datos[0].ValorNotaDebido);
                    }

                    return {
                        text: mensaje
                    };
                }
            },
            commonSeriesSettings: {
                ignoreEmptyPoints: true,
                argumentField: "Mes",
                type: "bar"
            },
            series: [
                { valueField: "ValorFactura", name: "Facturas" },
                 { valueField: "ValorNotaCredito", name: "NotasCredito" },
                 { valueField: "ValorNotaDebido", name: "NotasDebido" }
            ],
            legend: {
                verticalAlignment: "bottom",
                horizontalAlignment: "center"
            },
            title: ""
        });
    });

    //ReporteAcumuladoTipoDocumentoAdquiriente
    $(function () {
        $("#ReporteAcumuladoTipoDocumentoAdquiriente").dxPieChart({
            palette: chartOptions.gamaAzul,
            dataSource: datosReporteAcumuladoTipoDocumento,
            title: "",
            tooltip: {
                enabled: true,
                format: "largeNumber",
                customizeTooltip: function (arg) {

                    var porcentaje = ((arg.originalValue / TotalReporteMesActualEstadoAcuse) * 100).toFixed(2);

                    return {
                        text: "Tipo Documento: " + arg.argumentText + "<br/>" + "Cantidad Documentos: " + arg.originalValue + "<br/>" + "Porcentaje: " + porcentaje + "%"
                    };
                }
            },
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "center",
                verticalAlignment: "bottom",
            },
            series: [{
                name: "Porcentaje",
                argumentField: "TipoDocumento",
                valueField: "Cantidad"
            }]
        });
    });

    
    $scope.cambiaInclude = function (perfil) {

        switch (perfil) {
            case 1: $scope.include = 'Indicadores/IndicadoresAdministrador.aspx'; break;
            case 2: $scope.include = 'Indicadores/IndicadoresFacturador.aspx'; break;
            case 3: $scope.include = 'Indicadores/IndicadoresAdquiriente.aspx'; break;
        }

    }


    */
});


