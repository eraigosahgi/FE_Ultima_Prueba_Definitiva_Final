<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.ReportDesigner.ReportViewer" %>

<%@ Register Assembly="DevExpress.XtraReports.v18.2.Web.WebForms, Version=18.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register TagPrefix="cc1" Namespace="DevExpress.XtraReports.Web.ClientControls" Assembly="DevExpress.XtraReports.v18.2.Web.WebForms, Version=18.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>


	<asp:Label ID="lblresultado" runat="server" Font-Bold="True" Font-Size="XX-Large"></asp:Label>

	<form runat="server">
		<dx:ASPxWebDocumentViewer runat="server" ID="ASPxWebDocumentViewerWeb">
		</dx:ASPxWebDocumentViewer>
	</form>

	

</body>
</html>
