<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SondaAuditoria.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.SondaAuditoria" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    

		<H1>Sonda para migración de datos de Auditoria</H1>
		

		<p>Desde:</p> <asp:TextBox ID="TxtFechaInicio" runat="server"></asp:TextBox>
		<p>Hasta:</p> <asp:TextBox ID="TxtFechaFin" runat="server"></asp:TextBox>

		<p>Documentos:</p>
    	<asp:Button ID="BtnDocumentos" runat="server" OnClick="BtnDocumentos_Click" Text="Migrar" />

		<p>Formatos:</p>
    	<asp:Button ID="BtnFormatos" runat="server" OnClick="BtnFormatos_Click" Text="Migrar" />

		<p>Notificaciones:</p>
    	<asp:Button ID="BtnNotificaciones" runat="server" OnClick="BtnNotificaciones_Click" Text="Migrar" />

		<p>Resultado:</p>
		<asp:Label ID="lblResultado" runat="server" Text="."></asp:Label>
    
    </div>
    </form>
</body>
</html>
