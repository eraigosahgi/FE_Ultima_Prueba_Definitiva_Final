<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportDesignerWeb.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.ReportDesigner.ReportDesignerWeb" %>

<%@ Register Assembly="DevExpress.XtraReports.v17.2.Web, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register TagPrefix="cc1" Namespace="DevExpress.XtraReports.Web.ClientControls" Assembly="DevExpress.XtraReports.v17.2.Web, Version=17.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script src="../../Scripts/Pages/ReportDesignerWeb.js"></script>
</head>
<body>
	<form id="form1" runat="server">


		<dx:ASPxReportDesigner ID="ASPxReportDesignerWeb" runat="server" ColorScheme="dark" ClientSideEvents-OnServerError="reportDesigner_OnServerError"
			ClientSideEvents-CustomizeMenuActions="reportDesigner_CustomizeMenuActions" DisableHttpHandlerValidation="False"
			ShouldDisposeDataSources="true" OnSaveReportLayout="ASPxReportDesignerWeb_SaveReportLayout">
		</dx:ASPxReportDesigner>

	</form>
</body>
</html>
