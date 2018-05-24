<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaPlanesTransacciones.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaPlanesTransacciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

    <script src="../../Scripts/Pages/PlanesTransacciones.js"></script>

    <div ng-app="ConsultaPlanesApp" ng-controller="ConsultaPlanesController">
    </div>

</asp:Content>
