<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ImportarDocumentosRecibidos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ImportarDocumentosRecibidos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<%--<script src="../../Scripts/Pages/ModalConsultaEmpresas.js?vjs20220824"></script>--%>
	<script src="../../Scripts/Pages/ImportacionDocumentosRecibidos.js?vjs20220919"></script>

	<!-- CONTENEDOR PRINCIPAL -->
	<div data-ng-app="App" data-ng-controller="ImportarDocumentosRecibidosController">

		<!-- FILTROS DE BÚSQUEDA -->

		<div class="col-md-12">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Importación de Archivos<a class="heading-elements-toggle"><i class="icon-more"></i></a></h6>
					<div class="heading-elements">
						<ul class="icons-list">
							<li><a data-action="collapse"></a></li>
						</ul>
					</div>
				</div>

				<div class="panel-body">

					<div class="col-lg-12">
						<div class="row">
							<div class="col-md-12" style="margin-top: -10px;">
								<div class="col-md-6" style="margin-top: 0px;">
									<div class="col-md-12" style="margin-top: 0px;">
										<div class="file-uploader-block" style="float: left;">
											<div id="Archivo"></div>
										</div>
									</div>
								</div>

							</div>

						</div>


						</div>
						<div class="col-lg-12 text-right">
							<br />
							<br />
							<div data-dx-button="ButtonOptionsConsultar" style="margin-right: 20px"></div>
						</div>

						<p data-ng-bind-html="message"></p>
					</div>

				</div>
			</div>
		</div>

		<!--/FILTROS DE BÚSQUEDA -->

		<!-- DATOS -->
		<div class="col-md-12">
			<div class="panel panel-white">
				<div class="panel-heading" style="padding-bottom: 40px;">
					 <div class ="col-md-10">
						<h6 class="panel-title ">Datos</h6>
					 </div>
					
					<div class ="col-md-2">
						<div style="float: right; margin-right: 2%; margin-top: -8px;">
							<div id="procesar"></div>
						</div>
					</div>
				</div>

				<div class="panel-body">
					<div class="demo-container">
						<div id="gridDocumentos"></div>
					</div>
				</div>

			</div>
		</div>
		<!-- /DATOS -->

	</div>
	<!-- /CONTENEDOR PRINCIPAL -->

	<%--Panel Botones--%>
	<div class="col-lg-12 text-right" style="z-index: 0; margin-left: -10px;">
		<div id="file-uploader"></div>
		<a id="btncancelar" class="btn btn-default" style="text-transform: initial !important; margin-right: 1%" href="/Views/Pages/ConsultaEmpresas.aspx" style="font-size: 14px; text-align: center;">Cancelar</a>
		<div id="button"></div>
	</div>


</asp:Content>
