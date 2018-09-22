<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportDesignerWeb.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.ReportDesigner.ReportDesignerWeb" %>

<%@ Register Assembly="DevExpress.XtraReports.v17.2.Web, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register TagPrefix="cc1" Namespace="DevExpress.XtraReports.Web.ClientControls" Assembly="DevExpress.XtraReports.v17.2.Web, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta charset="utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />

	<title>HGInet Factura Electrónica</title>
	<!-- Global stylesheets -->
	<link href="https://fonts.googleapis.com/css?family=Roboto:400,300,100,500,700,900" rel="stylesheet" type="text/css" />
	<!-- /Global stylesheets -->

	<script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
	<script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.17.1/moment.min.js"></script>

	<!-- Estilos CSS -->
	<%-- <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.spa.css" />--%>
	<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.common.css" />
	<link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/17.2.7/css/dx.light.css" />

	<!-- Scripts Requeridos
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>-->

	<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.16/angular.min.js"></script>
	<!--<script src="https://cdn3.devexpress.com/jslib/17.2.7/js/dx.all.js"></script>    -->

	<script src="https://unpkg.com/devextreme-aspnet-data@1.3.0"></script>

	<!-- JS Localización -->
	<script src="../../Scripts/devextreme-localization/dx.messages.es.js"></script>
	<script src="../../Scripts/config.js"></script>
	<script src="../../Scripts/Services/Loading.js"></script>

	<script src="../../Scripts/Pages/ReportDesignerWeb.js"></script>

</head>
<body>
	<form id="form1" runat="server">

		<dx:ASPxReportDesigner ID="ASPxReportDesignerWeb" runat="server" ColorScheme="dark" ClientSideEvents-OnServerError="reportDesigner_OnServerError"
			ClientSideEvents-CustomizeMenuActions="reportDesigner_CustomizeMenuActions" DisableHttpHandlerValidation="False" ClientSideEvents-SaveCommandExecute="reportDesigner_SaveCommandExecute"
			ShouldDisposeDataSources="true" OnSaveReportLayout="ASPxReportDesignerWeb_SaveReportLayout" ClientSideEvents-EndCallback="reportDesigner_EndCallback">
		</dx:ASPxReportDesigner>

		<asp:Button runat="server" ID="BtnGenerar" Text="¡ Hola Mundo !" OnClick="BtnGenerar_Click" />

	</form>
</body>
</html>
