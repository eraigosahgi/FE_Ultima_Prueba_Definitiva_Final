<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionReportes.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionReportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Pages/ReportDesignerWeb.js?vjs20200921"></script>
	<script src="../../Scripts/Pages/ModalAsignarFormato.js?vjs20200921"></script>
	<script src="../../Scripts/Services/SrvFormatos.js?vjs2020401"></script>
	<style>
		.BtnStyleNone {
			background-color: transparent;
			border-color: transparent;
		}
	</style>

	<!-- CONTENEDOR PRINCIPAL -->
	<div data-ng-app="GestionReportesApp" data-ng-controller="GestionReportesController">

		<div class="col-md-12" style="margin-bottom: 2%">
			<div class="col-md-6">
				<i class="icon-folder-search"></i>
				<label>Filtrar:</label>
				<div data-dx-autocomplete="filtros.FiltrarFormatos" data-ng-model="filtrar.NitEmpresa"></div>
			</div>

			<div class="col-md-6 text-right">
				<a class="btn btn-primary" style="background: #337ab7; margin-top: 3%" data-ng-click="AbrirModal(0, 0)" id="BtnCrearFormato">Crear Formato en Blanco</a>
			</div>
		</div>

		<div class="col-md-12">

			<div data-ng-repeat="datos in FormatosPdfEmpresa | filter:filtrar">
				<div class=" col-sm-6 col-md-2" id="{{datos.CodigoFormato}}">
					<div class="panel" data-ng-class="ClasePanel(datos.Estado)" id="{{datos.ClavePrimaria}}">
						<div class="panel-heading text-center" style="padding: 5px 10px !important">
							<label style="font-size: medium;">{{datos.Titulo}}</label><br />
							<label title="({{datos.EstadoDescripcion}})" style="font-size: smaller; white-space: nowrap; text-overflow: ellipsis; overflow: hidden;">
								({{datos.EstadoDescripcion}})</label>
						</div>

						<div style="margin: 5%; height: 270px;">

							<div style="margin-bottom: 1%;">
								<img src="../../Scripts/Images/FormatoPDF.png" class="img-responsive" style="width: 40%; display: block; margin-left: auto; margin-right: auto;" />
							</div>
							<div style="margin-top: 13%">
								<label class="no-margin text-size-large text-semibold">Código: </label>
								<label class="no-margin text-size-large">{{datos.CodigoFormato}}</label><br />
								<label class="no-margin text-size-large text-semibold">Empresa: </label>
								<label class="no-margin text-size-large">{{datos.NitEmpresa}}</label><br />
								<label class="no-margin text-size-small">{{datos.RazonSocial}}</label><br />
								<label class="no-margin text-size-large text-semibold">Fecha Creación: </label>
								<label class="no-margin text-size-large">{{datos.FechaRegistro}}</label>
							</div>
						</div>

						<div class="text-right" style="margin-top: 1%">

							<ul class="nav nav-tabs nav-tabs-bottom">
								<li class="dropdown">
									<a href="#" class="dropdown-toggle legitRipple" data-toggle="dropdown" aria-expanded="false">
										<i class="icon-cog5"></i>
										<span class="position-right">Opciones</span>
										<span class="caret"></span>
									</a>
									<ul class="dropdown-menu dropdown-menu-right" style="background-color: #fcfcfc">
										<li><a data-ng-click="ImportarFormato(datos)"><i class="icon-download"></i>Importar</a></li>
										<li><a data-ng-click="ExportarFormato(datos.CodigoFormato,datos.NitEmpresa)"><i class="icon-upload"></i>Exportar</a></li>
										<li><a data-ng-click="AbrirModal(datos.CodigoFormato, datos.NitEmpresa)" data-ng-class="OpcionAsignar(datos.Estado)"><i class="icon-file-plus2"></i>Asignar Formato</a></li>
										<li><a data-ng-click="BtnEditar(datos.CodigoFormato,datos.NitEmpresa,datos.Estado)" data-ng-class="OpcionEditar(datos.Generico)"><i class="icon-pencil4"></i>Editar Formato</a></li>
										<li>
											<a id="BtnEstado{{datos.ClavePrimaria}}" data-ng-click="BtnCambioEstado(datos.Estado,datos.NitEmpresa,datos.CodigoFormato)" data-ng-class="OpcionCambioEstado(datos.Estado)">
												<i class="icon-lock5" data-ng-class="{'icon-unlocked2': datos.Estado}"></i>Cambiar Estado
											</a>
										</li>
										<li><a data-ng-click="SolicitarAprobacion(datos.CodigoFormato,datos.NitEmpresa,datos.Estado)" data-ng-class="{'ng-hide': datos.Estado != 2}"><i class="icon-checkmark4"></i>Solicitar aprobación</a></li>
										<li><a data-ng-click="AprobarFormato(datos.CodigoFormato,datos.NitEmpresa,datos.Estado)" data-ng-class="OpcionAprobar(datos.Estado,datos.Administrador)"><i class="icon-file-check2"></i>Aprobar Diseño</a></li>
										<li><a data-ng-click="PublicarFormato(datos.CodigoFormato,datos.NitEmpresa,datos.Estado)" data-ng-class="OpcionPublicar(datos.Estado)"><i class="icon-upload7"></i>Publicar Diseño</a></li>
										<li><a data-ng-click="MailPrueba(datos.CodigoFormato,datos.NitEmpresa)"><i class="icon-mail-read"></i>Envíar Prueba</a></li>
										<li><a data-ng-click="VerAuditoria(datos.CodigoFormato,datos.NitEmpresa)"><i class="icon-file-eye"></i>Auditoría</a></li>
									</ul>
								</li>
							</ul>
						</div>

					</div>
				</div>
			</div>

		</div>

		<div data-ng-include="'ModalAsignarFormato.aspx'"></div>

		<div id="modal_solicitar_aprobacion" class="modal fade" style="display: none;">
			<div class="modal-dialog">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title" id="LblTituloModal"></h5>
					</div>

					<div class="modal-body">

						<div class="col-md-12" id="CampoObservaciones">
							<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Observaciones:</label>
							<div id="TxtObservacionesSolicitud"></div>

						</div>

						<div class="col-md-12" id="CampoMailPrueba">
							<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email:</label>
							<div id="TxtMailPrueba"></div>
						</div>

						<div id="summary"></div>

						<div id="SolicitudAprobacion">
							<div class="col-md-12 text-right">
								<br />
								<div id="BtnSolicitarAprobacion"></div>
							</div>
						</div>

						<div id="AprobacionFormato">
							<div class="col-md-12 text-right">
								<br />
								<div id="BtnRechazarFormato"></div>
								<div id="BtnAprobarFormato"></div>
							</div>
						</div>

					</div>

					<div id="divsombra" class="modal-footer" style="margin-top: 22%">
					</div>

				</div>
			</div>
		</div>

		<div id="modal_auditoria_formato" class="modal fade" style="display: none;">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title">Auditoría</h5>
					</div>
					<div class="modal-body">

						<div class="panel panel-white">
							<div class="panel-body">
								<div id="gridAuditoriaFormato"></div>
							</div>
						</div>

					</div>
					<div class="modal-footer" style="margin-top: 5%">
					</div>

				</div>
			</div>
		</div>

		<div id="modal_importar_formato" class="modal fade" style="display: none;">
			<div class="modal-dialog modal-sm">
				<div class="modal-content">
					<div class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title">Importar Diseño</h5>
					</div>
					<form>
						<div class="modal-body">
							<div class="col-md-12 text-center">
								<p class="text-muted" style="font-size: small">
									El diseño a importar será incluido en el formato #{{CodFormatoImportar}} de la empresa {{NitEmpresaImportar}}-{{NombreEmpresaImportar}}
							y deberá realizar el proceso de solicitud de aprobación.
								</p>

								<div id="UploaderFormato"></div>
								<div id="summaryImportarFormato"></div>

							</div>
						</div>
						<div class="modal-footer" style="margin-top: 2%">
							<div id="BtnImportarFormato"></div>
						</div>
					</form>

				</div>
			</div>
		</div>

	</div>

</asp:Content>
