<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaUsuarios.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Services/SrvUsuario.js?vjs201924"></script>
    <script src="../../Scripts/Pages/Usuarios.js?vjs201924"></script>
    
    <div data-ng-app="ConsultaUsuarioApp" data-ng-controller="ConsultaUsuarioController">
        
        <div class="col-md-12">
            <div class="panel panel-white">
                <div>
                    <br />

                    <div class="col-md-12">
                        <div class="col-md-10">
                            <h6 class="panel-title">Datos</h6>
                        </div>
                        <div class="col-md-12 text-right" style="margin-top: -2%">
                            <a class="btn btn-primary" style="background: #337ab7" href="GestionUsuarios.aspx">Crear</a>
                            <br />
                        </div>
                    </div>

                </div>
                <br />
                <div class="panel-body" style="margin-top: 2%">
                    <div class="demo-container">
                        <div id="gridUsuarios"></div>
                    </div>
					<%--Loading de cargar Usuarios--%>
					<div data-ng-include="'Partials/LoadingRegistros.Html'"></div>
                </div>
            </div>
        </div>
    </div>   
</asp:Content>
