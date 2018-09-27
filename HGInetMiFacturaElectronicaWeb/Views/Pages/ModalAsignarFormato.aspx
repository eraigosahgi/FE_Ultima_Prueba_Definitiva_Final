<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModalAsignarFormato.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ModalAsignarFormato" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>

	<script src="../../Scripts/Pages/ModalAsignarFormato.js"></script>

	<form id="FormularioFormato">
		<div id="modal_asignar_formato" class="modal fade" style="display: none;">
			<div class="modal-dialog">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title">Busqueda de Empresa</h5>
					</div>

					<div class="modal-body">
						<div class="col-md-12 ">

							<!-- JS Modal Empresas-->

							<!-- CONTENEDOR PRINCIPAL -->
							<div data-ng-app="ModalAsignarFormatoApp" data-ng-controller="ModalAsignarFormatoController">

								<div class="col-md-12">
									<div class="panel panel-white">
										<div class="panel-body">

											<div class="col-md-12">
												<label style="margin: 0px; margin-top: 16px;">Empresa:<strom style="color: red;">*</label>
												<div id="SelectEmpresa"></div>
											</div>

											<div class="col-md-12">
												<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Estado:<strom style="color: red;">*</label>
												<div id="SelectEstado"></div>
											</div>

											<div class="col-md-12">
												<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Categoría:<strom style="color: red;">*</label>
												<div id="SelectGenerico"></div>
											</div>

											<div class="col-md-12">
												<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Observaciones:</label>
												<div id="TxtObservaciones"></div>
											</div>

											<div class="col-md-12 text-right">
												<br />
												<div id="BtnContinuar"></div>
											</div>


										</div>
									</div>
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
