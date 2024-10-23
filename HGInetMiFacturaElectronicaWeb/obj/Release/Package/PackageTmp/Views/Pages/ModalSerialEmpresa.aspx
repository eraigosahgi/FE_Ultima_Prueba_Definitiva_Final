<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModalSerialEmpresa.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ModalSerialEmpresa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    
    <form id="form1">
        <div id="modal_Serial_empresa" class="modal fade" style="display: none;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div id="EncabezadoModal" class="modal-header">
                        <button type="button" id="cerrrarModal" class="close" data-dismiss="modal">×</button>
                        <h5 style="margin-bottom: 10px;" class="modal-title">Modulo de Activación</h5>
                    </div>
                    <div data-ng-app="ModalSerialEmpresaApp" data-ng-controller="ModalSerialEmpresaController" data-ng-init="Visibilidad=true">



                        <div class="panel panel-white">
                            <div class="col-md-2">
                                <label>Nit:</label>
                            </div>
                            <div class="col-md-10 col-xs-12">
                                <div id="txtnitEmpresa"></div>
                            </div>

                            <div class="col-md-2 col-xs-12">
                                <label>Nombre:</label>
                            </div>
                            <div class="col-md-10 col-xs-12">
                                <div id="txtnombre"></div>
                            </div>

                            <div class="col-md-2 col-xs-12">
                                <label>Email:</label>
                            </div>

                            <div class="col-md-10 col-xs-12">
                                <div id="txtemail"></div>
                            </div>
                            <div class="col-md-12">
                                <label style="margin: 0px; margin-top: 16px;">Serial de Activación:<strom style="color: red;">*</label>
                                <div id="txtSerial"></div>
                            </div>

                            <div class="col-md-12">
                                <label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nª Resolución:<strom style="color: red;">*</label>
                                <div id="txtResolucion"></div>
                            </div>



                        </div>

                        <div class="dx-fieldset">
                            <div id="summary"></div>
                        </div>
                        <div id="divsombra" class="modal-footer">
                            <input id="btncancelar" type="button" class="btn btn-default" data-dismiss="modal" value="Cancelar" style="margin: 1%;" />
                            
                                <div data-ng-if="Visibilidad"  id="btnActivar" style="margin: 1%;"></div>
                            
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>



</body>
</html>
