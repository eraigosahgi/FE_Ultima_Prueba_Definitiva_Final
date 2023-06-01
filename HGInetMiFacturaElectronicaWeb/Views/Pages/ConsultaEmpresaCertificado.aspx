<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaEmpresaCertificado.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaEmpresaCertificado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
	<script src="../../Scripts/Services/SrvResoluciones.js?vjs20201019"></script>
	<script src="../../Scripts/config.js?vjs20201019"></script>
	<script src="../../Scripts/Services/SrvEmpresa.js?vjs20201019"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs20201019"></script>
	<script src="../../Scripts/Services/SrvEmpresa.js?vjs20201019"></script>
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20201019"></script>
	<script src="../../Scripts/Pages/ConsultaEmpresasCertificados.js"></script>
	<script src="../../Scripts/Pages/ModalDetalleEmpresa.js?vjs20201019"></script>
	<div data-ng-app="EmpresasApp" data-ng-controller="ConsultaEmpresasCertificadosController">


		<div class="col-md-12">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Filtros de Búsqueda<a class="heading-elements-toggle"><i class="icon-more"></i></a></h6>
					<div class="heading-elements">
						<ul class="icons-list">
							<li><a data-action="collapse"></a></li>
						</ul>
					</div>
				</div>

				<div class="panel-body">

					<div class="col-md-3" style="margin-top: 1%">
						<i class="icon-file-text"></i>
						<label>Responsable:</label>
						<div data-dx-select-box="filtros.TipoTercero"></div>
					</div>

					<div class="col-md-3" style="margin-top: 1%">
						<div data-hgi-filtro="Facturador"></div>
					</div>

					<div class="col-md-2" style="margin-top: 1%">
						<i class=" icon-calendar"></i>
						<label>Fecha Inicial:</label>
						<div id="FechaInicial"></div>
					</div>


					<div class="col-md-2" style="margin-top: 1%">
						<i class=" icon-calendar"></i>
						<label>Fecha Final:</label>
						<div id="FechaFinal"></div>
					</div>

					<div class="col-md-2 text-right" style="">

						<div data-dx-button="ButtonOptionsConsultar"></div>
					</div>

				</div>
			</div>
		</div>

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
				<div class="panel-body" style="margin-top: -20px;">
					<div class="demo-container">
						<div id="gridEmpresas"></div>
					</div>
					<%--Loading de cargar Empresas--%>
					<div data-ng-include="'Partials/LoadingRegistros.Html'"></div>
				</div>

			</div>
		</div>
		<div data-ng-include="'ModalDetalleEmpresa.aspx'"></div>
	</div>
</asp:Content>
