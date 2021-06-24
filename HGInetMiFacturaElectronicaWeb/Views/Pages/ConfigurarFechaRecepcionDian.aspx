<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConfigurarFechaRecepcionDian.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConfigurarFechaRecepcionDian" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20201019"></script>
	<script src="../../Scripts/Services/SrvEmpresa.js?vjs20201019"></script>
	<script src="../../Scripts/Services/SrvDocumentos.js?vjs20201019"></script>
	<script src="../../Scripts/Pages/ConfigurarFechaRecepcionDian.js?vjs20201019"></script>

	<!-- CONTENEDOR PRINCIPAL -->
	<div data-ng-app="FechaRecepcionDianApp" data-ng-controller="FechaRecepcionDianController">

		<div class="col-md-12">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Activación Fecha Recepción DIAN</h6>
				</div>
				<div class="panel-body">
					<div class="col-lg-12">
						<form id="form1">

							<div class="row">
								<div class="col-md-6 col-xs-12">
									<i class="icon-user-tie"></i>
									<label>Empresa:</label>
									<div id="txtEmpresa"></div>
								</div>
								<div class="col-md-2 col-xs-12">
									<i class="icon-arrow-right7"></i>
									<label>Posición X:</label>
									<div id="txtIntPdfCampoDianPosX"></div>
								</div>

								<div class="col-md-2 col-xs-12">
									<i class="icon-arrow-up7"></i>
									<label>Posición Y:</label>
									<div id="txtIntPdfCampoDianPosY"></div>
								</div>

								<div class="col-md-2 text-right" style="margin-top: 2%">
									<div id="BtnAlmacenarDatos"></div>
								</div>
							</div>
							<div style="margin-top:4%">
							<hr />
							</div>
							
							<div class="row">
								<div class="col-md-6 col-xs-12">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Tipo Documento:</label>
									<div id="txtTipoDocumento"></div>
								</div>
								<div class="col-md-6 col-xs-12">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nº Documento:</label>
									<div id="txtNumeroDocumento"></div>
								</div>
								<div class="col-md-6 col-xs-12">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Resolución:</label>
									<div id="txtNumeroResolucion"></div>
								</div>
								<div class="col-md-12 text-right" style="margin-top: 2%">
									<div id="BtnvisualizarDocumento"></div>
								</div>
							</div>

						</form>
					</div>
				</div>
			</div>
		</div>


		<div id="modal_Buscar_empresa" class="modal fade" style="display: none; z-index: 999999;">
			<div class="modal-dialog">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title">Busqueda de Empresa</h5>
					</div>
					<div class="modal-body">

						<%--//Panel Grid--%>
						<div class="col-md-12">
							<div class="panel panel-white">

								<div class="col-md-12">
									<div class="col-md-10">
										<h6 class="panel-title">Lista de Empresas</h6>
									</div>
								</div>

								<br />

								<div class="panel-body">
									<div class="demo-container">
										<div id="gridEmpresas"></div>
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

	</div>

</asp:Content>
