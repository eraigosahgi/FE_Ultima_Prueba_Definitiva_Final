<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ProcesarInteroperabilidad.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ProcesarInteroperabilidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Pages/ProcesarInteroperabilidad.js"></script>
	<script src="../../Scripts/Pages/ModalConsultaProveedores.js"></script>

	<div data-ng-app="ProcesarInteroperabilidadApp" data-ng-controller="ProcesarInteroperabilidadController">

		<!-- FILTROS DE BÚSQUEDA -->

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
					<div class="row">
						<div class="col-md-8">
							<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%; z-index: 999">Proveedor Tecnológico:</label>
							<div id="txtProveedorTecnologico" style="width: 80%"></div>
						</div>

						<div class="col-md-4 text-right">
							<br />
							<br />
							<br />
							<div dx-button="ButtonOptionsConsultar" style="margin-top: -10px; z-index: 888"></div>
						</div>

					</div>
				</div>
			</div>
		</div>

		<!--/FILTROS DE BÚSQUEDA -->

		<!-- DATOS -->
		<div class="col-md-12" id="panelresultado">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Datos</h6>
					<input type="text" id="lbltotaldocumentos" style="border-style: none; width: 100%; font-size: small;" readonly></input>
				</div>

				<div class="panel-body">
					<div class="demo-container">
						<div id="gridDocumentos"></div>
						<div class="col-lg-12 text-right">
							<br />
							<div data-ng-show="total>0">
								<div id="btnProcesar" style="margin-right: 20px;"></div>
							</div>
						</div>
					</div>

				</div>


			</div>
		</div>


		<div>
			<div data-ng-include="'ModalConsultaProveedores.aspx'"></div>
		</div>


	</div>
	<!-- /CONTENEDOR PRINCIPAL -->

</asp:Content>
