<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionReportes.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionReportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Pages/ReportDesignerWeb.js"></script>
	<script src="../../Scripts/Pages/ModalAsignarFormato.js"></script>
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
				<a class="btn btn-primary" style="background: #337ab7; margin-top: 3%" data-ng-click="AbrirModal(0, 0)">Crear Formato en Blanco</a>
			</div>
		</div>

		<div class="col-md-12">

			<div data-ng-repeat="datos in FormatosPdfEmpresa | filter:filtrar">
				<div class="col-md-2" id="{{datos.CodigoFormato}}">
					<div class="panel panel-success" data-ng-class="{'panel-danger':!datos.Estado}" id="{{datos.ClavePrimaria}}">
						<div class="panel-heading">
							<h6 class="panel-title">{{datos.Titulo}}</h6>
							<a class="heading-elements-toggle"><i class="icon-more"></i></a>
						</div>

						<div class="panel-body">

							<div style="margin-bottom: 1%;">
								<img src="../../Scripts/Images/FormatoPDF.png" class="img-responsive" style="width: 60%; display: block; margin-left: auto; margin-right: auto;" />
							</div>
							<div style="margin-top: 10%">
								<label class="no-margin text-size-large text-semibold">Código: </label>
								<label class="no-margin text-size-large">{{datos.CodigoFormato}}</label><br />
								<label class="no-margin text-size-large text-semibold">Empresa: </label>
								<label class="no-margin text-size-large">{{datos.NitEmpresa}}</label><br />
								<label class="no-margin text-size-small">{{datos.RazonSocial}}</label><br />
								<label class="no-margin text-size-large text-semibold">Fecha Creación: </label>
								<label class="no-margin text-size-large">{{datos.FechaRegistro}}</label>
							</div>
							<div class="text-right" style="margin-top: 10%">
								<ul class="icons-list">
									<li>
										<a class="icon-pencil4 BtnStyleNone" data-toggle="tooltip" title="Editar Formato" data-ng-click="BtnEditar(datos.CodigoFormato,datos.NitEmpresa)" data-ng-hide="{{datos.Generico}}"></a>
									</li>
									<li>
										<a class="icon-file-plus2 BtnStyleNone" title="Asignar Formato" data-ng-click="AbrirModal(datos.CodigoFormato, datos.NitEmpresa)"></a>
									</li>
									<li>
										<button style="margin-top: -10px; margin-left: -7px" id="BtnEstado{{datos.ClavePrimaria}}" class="icon-lock5 BtnStyleNone" data-ng-class="{'icon-unlocked2': datos.Estado}" title="Modificar Estado" data-ng-click="BtnCambioEstado(datos.Estado,datos.NitEmpresa,datos.CodigoFormato)"></button>
									</li>

								</ul>
							</div>
						</div>


					</div>
				</div>
			</div>

		</div>

		<div data-ng-include="'ModalAsignarFormato.aspx'"></div>

	</div>

</asp:Content>
