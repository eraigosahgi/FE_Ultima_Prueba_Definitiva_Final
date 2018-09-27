<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoInteroperabilidad.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Demos.DemoInteroperabilidad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
            <asp:Button ID="Button2" runat="server" Text="Login" OnClick="Button2_Click" />
            <br />
            <asp:TextBox ID="txtresp" runat="server" Height="78px" Width="866px"></asp:TextBox>
            <br />
            Token<br />
            <asp:TextBox ID="txttoken" runat="server" Width="859px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Registrar Documentos" />
            <br />
            <br />
        <asp:Button ID="cmdCambiarclave" runat="server" Text="Cambiar Clave" OnClick="cmdCambiarclave_Click" />
            <br />
            <br />
            <asp:Button ID="cmdConsultaruuid" runat="server" Text="Consultar UUID" OnClick="cmdConsultaruuid_Click" />
            <br />
            <br />
            <br />
            <asp:Button ID="cmdConsultaAcuse" runat="server" Text="Conusltar Acuse" OnClick="cmdConsultaAcuse_Click" />
            <br />
            <br />
            <br />
            <asp:TextBox ID="txtresconsulta" runat="server" Height="108px" Width="865px" TextMode="MultiLine"></asp:TextBox>
            <br />
            <br />
            <br />
            <br />
        </div>
        
    </form>
</body>
</html>
