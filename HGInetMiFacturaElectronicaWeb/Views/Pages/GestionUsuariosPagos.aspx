<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionUsuariosPagos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionUsuariosPagos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20201019"></script>
	<script src="../../Scripts/Services/SrvUsuarioPagos.js?vjs20201019"></script>
	<script src="../../Scripts/Pages/UsuariosPagos.js?vjs20200922"></script>

	<div data-ng-app="ConsultaUsuarioPagosApp" data-ng-controller="GestionUsuarioPagosController">
		<div class="col-md-12">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Gestión de Usuario</h6>
				</div>

				<div class="panel-body" >

					<div class="col-lg-12">


						<div class="row">

							<div class="dx-fieldset">

								<div class="col-md-6  " style="padding-top: 22px;">
									<div data-hgi-filtro="Facturador"></div>
								</div>

								<div class="col-md-6 col-xs-12">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Usuario:<strom style="color: red;">*</strom></label>
									<div id="txtusuario"></div>
								</div>

								<div class="col-md-6 col-xs-12">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nombres:<strom style="color: red;">*</strom></label>
									<div id="txtnombres"></div>
								</div>

								<div class="col-md-6 col-xs-12">
									<label id="lblapellido" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Apellidos:<strom style="color: red;">*</strom></label>
									<div id="txtapellidos"></div>
								</div>


								<div class="col-md-6 col-xs-12">
									<label id="lbltelefono" style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Teléfono:<strom style="color: red;">*</strom></label>
									<div id="txttelefono"></div>
								</div>

								<div class="col-md-6 col-xs-12">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email:<strom style="color: red;">*</strom></label>
									<div id="txtemail"></div>
								</div>



								<div class="col-md-6 col-xs-12">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Estado:<strom style="color: red;">*</strom></label>
									<div id="cboestado"></div>
								</div>

							</div>

							<div class="dx-fieldset">
								<div id="summary"></div>
							</div>

						</div>

						<div class="col-md-12 text-right">
							<br />
							<br />
							<a id="btncancelar" class="btn btn-default" style="text-transform: initial !important; margin-right: 1%" href="/Views/Pages/ConsultaUsuariosPagos.aspx" style="font-size: 14px; text-align: center;">Cancelar</a>
							<div id="button"></div>
						</div>

					</div>

				</div>


			</div>
		</div>


	</div>

</asp:Content>

