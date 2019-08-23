<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmacionEmail.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConfirmacionEmail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Confirmación de Email</title>

	<link href="../../Scripts/assets/css/icons/icomoon/styles.css" rel="stylesheet" type="text/css" />
	<link href="../../Scripts/assets/css/bootstrap.css" rel="stylesheet" type="text/css" />
	<link href="../../Scripts/assets/css/core.css" rel="stylesheet" type="text/css" />
	<link href="../../Scripts/assets/css/components.css" rel="stylesheet" type="text/css" />
	<link href="../../Scripts/assets/css/colors.css" rel="stylesheet" type="text/css" />

</head>
<script type="text/javascript" src="../../Scripts/assets/js/core/libraries/bootstrap.min.js"></script>


<body>
	<form id="form1" runat="server">
		<div class="panel-body">
			<div>

				<div class="panel" style="margin:100px; padding:50px; height:200px;">
					<div class="panel-body" style="text-align:center;"">
						<h4 class="text-bold" style="margin-top: -10px; margin-bottom: 20px"></h4>
						<label runat="server" id="lblResultado"></label>						

					</div>
				</div>

			</div>
		</div>
	</form>
</body>
</html>
