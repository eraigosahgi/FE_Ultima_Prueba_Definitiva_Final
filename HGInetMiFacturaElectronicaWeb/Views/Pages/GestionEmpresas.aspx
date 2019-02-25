<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionEmpresas.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionEmpresas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Services/SrvEmpresa.js?vjs201913"></script>
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs201913"></script>
	<script src="../../Scripts/Pages/Empresas.js?vjs201913"></script>
	<script src="../../Scripts/Pages/ModalConsultaEmpresas.js?vjs201913"></script>

	<div data-ng-app="EmpresasApp" data-ng-controller="GestionEmpresasController" class="col-md-12">
		<%--Panel información General--%>
		<div class="col-md-6">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Información General</h6>
				</div>

				<div class="panel-body">

					<div class="col-lg-12">


						<div class="row">



							<div class="col-md-12" style="z-index: 9;">
								<div class="col-md-12" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Tipo de Identificación:<strom style="color: red;">*</strom></label>
									<div id="TipoIndentificacion"></div>
								</div>


								<div class="col-md-12" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Numero de Identificación:<strom style="color: red;">*</strom></label>
									<div id="NumeroIdentificacion"></div>
								</div>
							</div>

							<div class="col-md-12" style="z-index: 9;">
								<div class="col-md-12 " style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Razón Social:<strom style="color: red;">*</strom></label>
									<div id="txtRasonSocial"></div>
								</div>
								<div class="col-md-12" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Teléfono:</label>
									<div id="txttelefono"></div>
								</div>
								<div class="col-md-12 " style="margin: 0px; margin-top: 16px; margin-bottom: 1%; z-index: 9;">
									<div class="dx-field-label" style="font-size: 14px;">Perfil:<strom style="color: red;">*</strom></div>
									<div id="Facturador" style="margin-top: 1%"></div>
									<br />
									<div id="Adquiriente" style="margin-top: 1%"></div>
									<br />
									<div class="dx-field-label" style="font-size: 14px; margin-top: 1%"></div>
									<div id="Integradora" style="margin-top: 1%"></div>
								</div>

								<div class="col-md-12" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%; z-index: 9;">Estado:<strom style="color: red;">*</strom></label>
									<div id="cboestado"></div>
								</div>

								<div class="col-md-12 text-left" style="z-index: 8;">
									<label style="margin-top: 16px; padding-top: 40px;">Observaciones:</label>
									<div id="txtobservaciones"></div>
								</div>

							</div>


							<div class="dx-fieldset" style="padding: 20px;">
								<div id="summary"></div>
							</div>

						</div>



					</div>

				</div>
			</div>

		</div>
		<%--Panel Notificaciones--%>
		<div class="col-md-6">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Notificaciones</h6>
				</div>

				<div class="panel-body">
					<div class="col-lg-12">
						<div class="row">

							<div id="divEmailFacturador" class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Administrativo:<strom style="color: red;">*</label>
								<div id="txtEmail"></div>
								<div id="ttEmail" style="width: 200px"></div>
							</div>
							<div class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Envío Documentos:<strom style="color: red;">*</strom></label>
								<div id="txtMailEnvio"></div>
								<div id="ttMailEnvio"></div>
							</div>

							<div class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Recepción Documentos:<strom style="color: red;">*</label>
								<div id="txtMailRecepcion"></div>
								<div id="ttMailRecepcion"></div>
							</div>

							<div class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Recepción Acuse:<strom style="color: red;">*</strom></label>
								<div id="txtMailAcuse"></div>
								<div id="ttMailAcuse"></div>
							</div>

							<div class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Pagos Electrónicos:<strom style="color: red;">*</strom></label>
								<div id="txtMailPagos"></div>
								<div id="ttMailPagos"></div>
							</div>




						</div>
					</div>
				</div>
			</div>
		</div>
		<%--Panel Plataforma--%>
		<div class="col-md-6">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Plataforma</h6>
				</div>

				<div class="panel-body">
					<div class="col-lg-12">
						<div class="row">



							<div class="col-md-12" style="z-index: 9;">


								<div class="col-md-12" style="z-index: 9; margin: 0px; margin-top: 16px; margin-bottom: 1%">
									<div class="dx-field-label" style="font-size: 14px;" id="idHabilitacion">Ambiente:<strom style="color: red;">*</strom></div>
									<div class="dx-field-value">
										<div id="Habilitacion"></div>
									</div>
								</div>
							</div>
							<div class="col-md-6" style="z-index: 9; margin-left: 0px">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nº Usuarios:</label>
								<div id="txtUsuarios"></div>

							</div>

							<div class="col-md-6" style="z-index: 9; margin-left: 0px">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nº Horas acuse:</label>
								<div id="txtDiasAcuse"></div>

							</div>


							<div class="col-md-12">



								<div class="col-md-4 " style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Maneja Anexo:</label>
									<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: 20px">
										<div id="Anexo"></div>
									</div>
								</div>

								<div class="col-md-4" style="z-index: 9;">
									<label style="margin: 0px; margin-left: 0px; margin-top: 16px; margin-bottom: 1%">Email Recepcion:</label>
									<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: 30px">
										<div id="EmailRecepcion"></div>
									</div>
								</div>

								<div class="col-md-4  text-center" style="z-index: 9;">
									<label style="margin: 0px; margin-left: -25px; margin-top: 16px; margin-bottom: 1%">Post-Pago Auto.:</label>
									<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: -10px">
										<div id="postpagoaut"></div>
									</div>
								</div>

							</div>

							<div class="col-md-12" style="margin-top: 1%;">
								<div data-hgi-filtro="EmpresaAsociada"></div>
							</div>

							<div class="col-md-12" style="margin-top: 1%;">
								<div data-hgi-filtro="EmpresaDescuenta"></div>
							</div>


						</div>
					</div>
				</div>
			</div>
		</div>
		<%--Panel Botones--%>
		<div class="col-lg-12 text-right" style="z-index: 0;">
			<a id="btncancelar" class="btn btn-default" style="text-transform: initial !important; margin-right: 1%" href="/Views/Pages/ConsultaEmpresas.aspx" style="font-size: 14px; text-align: center;">Cancelar</a>
			<div id="button"></div>
		</div>
	</div>
</asp:Content>
