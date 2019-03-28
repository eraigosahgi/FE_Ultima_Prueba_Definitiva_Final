<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaEmpresasSerial.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaEmpresasSerial1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<!-- JS DocumentosAdquiriente-->
	<script src="../../Scripts/Pages/EmpresasSerial.js?vjs201915"></script>

	<!-- CONTENEDOR PRINCIPAL -->
	<div data-ng-app="SerialEmpresaApp" data-ng-controller="SerialEmpresaController">

		<%--//Panel Grid--%>
		<div class="col-md-12">
			<div class="panel panel-white">
				<div>
					<br />

					<div class="col-md-12">
						<div class="col-md-10">
							<h6 class="panel-title">Datos</h6>
						</div>
					</div>

				</div>

				<br />
				<div class="panel-body" style="margin-top: 0">
					<div class="demo-container">
						<div id="gridEmpresas"></div>
					</div>

				</div>

			</div>
		</div>

		<%--Modal para enviar correos--%>

		<div id="modal_enviar_email" class="modal fade" style="display: none; margin-top: 15%;" modal="showModal">
			<div class="modal-dialog modal-sm">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" id="btncerrarmodal" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px" class="modal-title">Envío E-mail</h5>
					</div>

					<div class="modal-body text-center">
						<h6>El siguiente E-mail corresponde al destinatario de este mensaje.</h6>

						<div class="col-md-2">
							<label style="text-align: left;">Nit:</label>
						</div>
						<div class="col-md-10 col-xs-12">
							<div id="txtnitEmpresamail"></div>
						</div>

						<div class="col-md-2 col-xs-12">
							<label style="text-align: left;">Nombre:</label>
						</div>
						<div class="col-md-10 col-xs-12">
							<div id="txtnombremail"></div>
						</div>

						<div class="col-md-2 col-xs-12" style="text-align: left;">
							<label style="text-align: left;">Serial:</label>
						</div>
						<div class="col-md-10 col-xs-12" style="text-align: left;">
							<div id="txtserialmail"></div>
						</div>
						<div style="margin-bottom: 1%;">
							<div id="formEmailEnvio" dx-form="formOptionsEmailEnvio">
							</div>
						</div>

					</div>
					<div id="divsombra" class="modal-footer">
						<div dx-button="buttonCerrarModal" data-dismiss="modal"></div>
						<div dx-button="buttonEnviarEmail"></div>
					</div>

				</div>
			</div>
		</div>

		<div ng-include="'ModalSerialEmpresa.aspx'"></div>
	</div>
</asp:Content>
