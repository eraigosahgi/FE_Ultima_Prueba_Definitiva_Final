<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Views/Masters/MasterEncabezado.Master" AutoEventWireup="true" CodeBehind="ReportDesignerWeb.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.ReportDesigner.ReportDesignerWeb" %>


<%@ Register Assembly="DevExpress.XtraReports.v18.2.Web.WebForms, Version=18.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register TagPrefix="cc1" Namespace="DevExpress.XtraReports.Web.ClientControls" Assembly="DevExpress.XtraReports.v18.2.Web.WebForms, Version=18.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Pages/ReportDesignerWeb.js"></script>
	<!-- jQuery -->
	<script src="//code.jquery.com/jquery-1.11.0.min.js"></script>
	<!-- BS JavaScript -->
	<script type="text/javascript" src="js/bootstrap.js"></script>
	<dx:ASPxReportDesigner ID="ASPxReportDesignerWeb" runat="server" ColorScheme="Light" ClientSideEvents-OnServerError="reportDesigner_OnServerError"
		ClientSideEvents-ExitDesigner="ExitDesignerfunction"
		ClientSideEvents-CustomizeMenuActions="reportDesigner_CustomizeMenuActions" DisableHttpHandlerValidation="False" ClientSideEvents-SaveCommandExecute="reportDesigner_SaveCommandExecute"
		ShouldDisposeDataSources="true" OnSaveReportLayout="ASPxReportDesignerWeb_SaveReportLayout" ClientSideEvents-EndCallback="reportDesigner_EndCallback">
	</dx:ASPxReportDesigner>

	<asp:HiddenField ID="HiddenFieldIdUsuario" runat="server" />

	<%--<input id="InputImportarFormato" type="file" accept="text/xml" name="InputImportarFormato" runat="server" />
	<asp:Button runat="server" ID="BtnSubmit" CssClass="btn btn-default" OnClientClick="this.value = 'Cargando...';" UseSubmitBehavior="false" OnClick="BtnCargarXml_Click" Text="Cargar Diseño!" />--%>

</asp:Content>

