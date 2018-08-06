<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModalPagos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ModalPagos" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <!--Link de Js con logica de WebApi-->
    <!--<script src="../../Scripts/Pages/ModalPagos.js"></script>-->


    <!--Pagos-->
    <div data-ng-controller="ModalPagosController">
        <div id="modal_Pagos_Electronicos" class="modal fade" style="top: 30%; display: none; z-index: 999999;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div id="EncabezadoModal" class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">×</button>
                        <h5 style="margin-bottom: 10px;" class="modal-title">Pago Electronico</h5>
                    </div>
                    <div class="modal-body">
                        <div class="col-md-12 ">

                            <!-- JS Modal Pagos-->

                            <!-- CONTENEDOR PRINCIPAL -->
                            <div data-ng-app="ModalpagosApp" data-ng-controller="ModalPagosController" data-ng-init="Stop=true">
                                <form id="form" action="your-action">

                                    <div class="col-md-12" id="PanelPago">
                                        <div class="panel panel-white">

                                            <div class="panel-body">

                                                <div class="col-md-12">
                                                    <div class="col-md-4">
                                                        <label>Total Factura:</label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        {{montoFactura | currency }}
                                                    </div>
                                                </div>

                                                <div class="col-md-12">
                                                    <div class="col-md-4">
                                                        <label>Monto Pendiente:</label>
                                                    </div>
                                                    <div class="col-md-4" id="divValorPendiente">
                                                    </div>
                                                </div>


                                                <div class="col-md-12">
                                                    <div class="col-md-4 ">
                                                        <label>Monto a Pagar:</label>
                                                        
                                                    </div>
                                                    <div class="col-md-4 text-center">
                                                        <div id="MontoPago"></div>
                                                        <label>Pagar el total:</label>
                                                        <br />

                                                        <div id="PagoTotal"></div>                                                        
                                                    </div>
                                                    <div class="col-md-4 text-right">
                                                        <br />

                                                        <div id="button"></div>


                                                        <div data-dx-button="buttonProcesar" data-ng-if="EnProceso">
                                                            <i class="icon-spinner6 spinner mr-2"></i>
                                                        </div>


                                                    </div>
                                                </div>

                                                <div class="dx-fieldset">
                                                    <div id="summary"></div>
                                                </div>
                                            </div>


                                        </div>
                                    </div>


                                    <div class="col-md-12">
                                        <div class="panel panel-white">

                                            <div class="panel-body">
                                                <div class="demo-container">
                                                    <div id="grid"></div>
                                                </div>
                                            </div>



                                        </div>
                                    </div>

                                </form>


                            </div>


                        </div>
                    </div>

                    <div id="divsombra" class="modal-footer" style="margin-top: 22%">
                    </div>

                </div>
            </div>
        </div>
    </div>
    <!--Pagos-->

</body>
</html>

