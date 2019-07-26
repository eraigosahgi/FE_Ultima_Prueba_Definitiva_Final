<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModalConsultaProveedores.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ModalConsultaProveedores" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<script src="../../Scripts/Pages/ModalConsultaProveedores.js?vjs201917"></script>
	<form>
		<div id="modal_Buscar_proveedores" class="modal fade" style="display: none;">
			<div class="modal-dialog">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title">Búsqueda de Proveedores</h5>
					</div>
					<div class="modal-body">
						<div class="col-md-12 ">

							<!-- JS Modal Proveedores-->

							<!-- CONTENEDOR PRINCIPAL -->
							<div data-ng-app="ModalProveedorApp" data-ng-controller="ModalConsultaProveedoresController">

								<%--//Panel Grid--%>
								<div class="col-md-12">
									<div class="panel panel-white">
										<div>
											<br />
											<br />
											<div class="col-md-12">
												<div class="col-md-10">
													<h6 class="panel-title">Lista de Proveedores</h6>
												</div>

											</div>

										</div>

										<br />
										<div class="panel-body">

											<div class="demo-container">
												<div id="gridProveedores"></div>
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
