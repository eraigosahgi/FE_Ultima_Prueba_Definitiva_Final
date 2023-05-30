<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModalComprarPlan.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ModalComprarPlan" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>    
	<script src="../../Scripts/Pages/ModalCompraPlan.js?vjs20220401"></script>
	<form id="FormularioCompraPlan">
		<div id="modal_comprar_plan" class="modal fade" style="display: none;">
			<div class="modal-dialog">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title">Activar Plan de Documentos</h5>
					</div>

					<div class="modal-body">
						<div class="col-md-12 ">

							<!-- JS Modal Empresas-->

							<!-- CONTENEDOR PRINCIPAL -->
							<div data-ng-controller="ModalComprarPlanController">

								<div class="col-md-12">
									<label style="margin: 0px; margin-top: 16px;">Plan <strom style="color: red;" id="txtPlan"></label>
									<%--<div id="MPlan"></div>--%>
								</div>

								<div class="col-md-12">
									<label style="margin: 0px; margin-top: 16px;">Desde:</label>
									<div id="txtMin"></div>
								</div>

								<div class="col-md-12">
									<label style="margin: 0px; margin-top: 16px;">Hasta:</label>
									<div id="txtMax"></div>
								</div>

								<div class="col-md-12">
									<label style="margin: 0px; margin-top: 16px;">Valor Unitario:</label>
									<div id="txtValUni"></div>
								</div>

								<div class="col-md-12">
									<label style="margin: 0px; margin-top: 16px;">Cantidad:<strom style="color: red;">*</label>
									<div id="txtCantidad"></div>
								</div>

								<div class="col-md-12">
									<label style="margin: 0px; margin-top: 16px;">Total:</label>
									<div id="txtTotal"></div>
								</div>

								<%--<div class="col-md-12">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Tipo Documento:<strom style="color: red;">*</label>
									<div id="SelectTipoDoc"></div>
								</div>--%>

								<div class="col-md-12 text-right">
									<br />
									<div id="BtnComprar"></div>
								</div>


							</div>

						</div>
					</div>

					<div id="divsombra" class="modal-footer" style="margin-top: 22%">
					</div>

				</div>
			</div>
		</div>

	</form>
</body>
</html>
