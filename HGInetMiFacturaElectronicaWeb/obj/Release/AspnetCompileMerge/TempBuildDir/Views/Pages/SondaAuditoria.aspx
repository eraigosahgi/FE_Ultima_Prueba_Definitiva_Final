<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SondaAuditoria.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.SondaAuditoria" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Migración mongoDB SQL Server</title>
	<link href="../../Scripts/assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
</head>
<body>
	<form id="form1" runat="server">
		<div style="margin: 30px;">

			<h1>Sonda para migración de datos de Auditoria</h1>


			<p>Desde:</p>
			<asp:TextBox ID="TxtFechaInicio" runat="server"></asp:TextBox>
			<p>Hasta:</p>
			<asp:TextBox ID="TxtFechaFin" runat="server"></asp:TextBox>

			<div class="col-md-12">
				<div class="col-md-2">


					<p>Documentos:</p>
					<asp:Button ID="BtnDocumentos" runat="server" OnClick="BtnDocumentos_Click" CssClass="btn btn-primary" Text="Migrar" />

					<p>Formatos:</p>
					<asp:Button ID="BtnFormatos" runat="server" OnClick="BtnFormatos_Click" CssClass="btn btn-primary" Text="Migrar" />

					<p>Notificaciones:</p>
					<asp:Button ID="BtnNotificaciones" runat="server" OnClick="BtnNotificaciones_Click" CssClass="btn btn-primary" Text="Migrar" />

				</div>

				<div class="col-md-4">
					<div runat="server" id="Gif" visible="false">
						<img class="divImg" style="z-index: 9999; max-width: 10%; max-height: 10%;" src="../../Content/icons/Loading.gif" />
					</div>
				</div>
			</div>
			<br />
			<br />
			
			<p style="margin: 30px; margin-top:100px;" >Resultado:</p>
			
			<asp:TextBox ID="lblResultado" Style="margin: 30px;" runat="server" ReadOnly="true" TextMode="MultiLine" CssClass="form-control" Height="400px"></asp:TextBox>


		</div>
	</form>
</body>
</html>
