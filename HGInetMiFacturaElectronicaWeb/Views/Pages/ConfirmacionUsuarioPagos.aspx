<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmacionUsuarioPagos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConfirmacionUsuarioPagos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Confirmación de Usuario</title>

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

				<div class="panel" style="margin: 100px; padding: 50px; height: 200px;">
					<div class="panel-body">
						<div class="col-md-12">
							<div class="col-md-1">

								<asp:Panel runat="server" ID="imgExitoso" Visible="false">
									<div class="mr-3">
										<a href="#" class="btn bg-transparent border-success text-success rounded-round border-2 btn-icon" style="border: 2px solid; border-radius: 100px!important;">
											<i class="icon-checkmark3"></i>
										</a>
									</div>
								</asp:Panel>

								<asp:Panel runat="server" ID="imgFallido" Visible="false">
								<div class="mr-3">
									<a  href="#" class="btn bg-transparent border-warning-400 text-warning-400 rounded-round border-2 btn-icon" style="border: 2px solid; border-radius: 100px!important;">
										<i class="icon-cross2 text-danger"></i>
									</a>
								</div>
								</asp:Panel>								
							</div>
							<div class="col-md-11" style="margin-top: 10px;">
								<label runat="server" id="lblResultado"></label>
							</div>
						</div>
					</div>
				</div>

			</div>
		</div>
	</form>
</body>
</html>
