<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditoriaDocumento.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.AuditoriaDocumento" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>

	<form>
		<div id="modal_audit_documento" class="modal fade" style="display: none;">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title">Auditoría</h5>
					</div>
					<div class="modal-body">
						<div class="col-md-12 ">

							<!-- JS Modal Auditoría Documento-->

							<!-- CONTENEDOR PRINCIPAL -->
							<div data-ng-app="ModalAuditDocumentoApp" data-ng-controller="ModalAuditDocumentoController">

								<%--//Panel Grid--%>
								<div class="col-md-12">
									<div class="panel panel-white">
										<div class="panel-body">

											<div class="row">

												<div class="col-sm-4" style="background-color: #F3F3F3;">
													<asp:Label runat="server" Style="vertical-align: inherit;"><b>IdSeguridad:</b></asp:Label>
												</div>
												<div class="col-sm-8">
													<asp:Label runat="server" Style="vertical-align: inherit;">{{IdSeguridad}}</asp:Label>
												</div>

												<div class="col-sm-4" style="background-color: #F3F3F3;">
													<asp:Label runat="server" Style="vertical-align: inherit;"><b>Número Documento:</b></asp:Label>
												</div>
												<div class="col-sm-8">
													<asp:Label runat="server" Style="vertical-align: inherit;">{{NumeroDocumento}}</asp:Label>
												</div>

												<div class="col-sm-4" style="background-color: #F3F3F3;">
													<asp:Label runat="server" Style="vertical-align: inherit;"><b>Identificación Obligado:</b></asp:Label>
												</div>
												<div class="col-sm-8">
													<asp:Label runat="server" Style="vertical-align: inherit;">{{Obligado}}</asp:Label>
												</div>

											</div>
											<br />

											<div class="demo-container">
												<div id="gridAuditDocumento"></div>
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
