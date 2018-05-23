<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaEmpresasSerial.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaEmpresasSerial1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
      <!-- JS DocumentosAdquiriente-->
    <script src="../../Scripts/Pages/EmpresasSerial.js"></script>

    <!-- CONTENEDOR PRINCIPAL -->
    <div ng-app="SerialEmpresaApp" ng-controller="SerialEmpresaController">
        
        <%--//Panel Grid--%>
        <div class="col-md-12">
            <div class="panel panel-white">
                <div>
                    <br />
                    
                    <div class="col-md-12">
                        <div class="col-md-10">
                            <h6 class="panel-title">Datos</h6>
                        </div>                      
                    </div>

                </div>

                <br />
                <div class="panel-body" style="margin-top: 2%">
                    <div class="demo-container">
                        <div id="gridEmpresas"></div>
                    </div>

                </div>

            </div>
        </div>
        <div ng-include="'ModalSerialEmpresa.aspx'"></div>
    </div>
</asp:Content>
