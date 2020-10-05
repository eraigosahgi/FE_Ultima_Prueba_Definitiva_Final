<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultaUsuarios.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultaUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20201005"></script>
	<script src="../../Scripts/Services/SrvUsuario.js?vjs20201005"></script>
	<script src="../../Scripts/Pages/Usuarios.js?vjs20200922"></script>

	<div data-ng-app="ConsultaUsuarioApp" data-ng-controller="ConsultaUsuarioController">



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

				<div class="col-md-3" style="margin-top: 1%" data-ng-show="Admin">
					<div data-hgi-filtro="Facturador"></div>
				</div>
			
				<div class="col-md-3" style="margin-top: 1%">
					<label>Usuario</label>
					<div id="codigo_usuario"></div>
				</div>


				<div class="col-md-4" style="margin-top: 1%">
					<label>Nombre</label>
					<div id="nombre_usuario"></div>
				</div>

				<div class="col-md-2 text-right" style="margin-top: -5px">
					<br />
					<br />
					<div id="BtnConsultar"></div>
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
					<div class="col-md-12 text-right" style="margin-top: -2%">
						<a class="btn btn-primary" style="background: #337ab7" href="GestionUsuarios.aspx">Crear</a>
						<br />
					</div>
				</div>

			</div>
			<br />
			<div class="panel-body" style="margin-top: 2%">
				<div class="demo-container">
					<div id="gridUsuarios"></div>
				</div>
				<%--Loading de cargar Usuarios--%>
				<div data-ng-include="'Partials/LoadingRegistros.Html'"></div>
			</div>
		</div>
	</div>
	</div>   
</asp:Content>
