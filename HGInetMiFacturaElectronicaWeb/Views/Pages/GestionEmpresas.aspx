<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="GestionEmpresas.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.GestionEmpresas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">

	<script src="../../Scripts/Services/SrvResoluciones.js?vjs20201020"></script>
	<script src="../../Scripts/Services/SrvEmpresa.js?vjs20201020"></script>
	<script src="../../Scripts/Services/MaestrosEnum.js?vjs20201020"></script>
	<script src="../../Scripts/Services/FiltroGenerico.js?vjs20201020"></script>
	<script src="../../Scripts/Pages/Empresas.js?vjs20201020"></script>
	<script src="../../Scripts/Pages/ModalConsultaEmpresas.js?vjs20201020"></script>

	<div data-ng-app="EmpresasApp" data-ng-controller="GestionEmpresasController" class="col-md-12">
		<div data-ng-include="'Partials/ModalResoluciones.html'"></div>
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


							</div>
							<div class="col-md-12" style="z-index: 9; margin-bottom: 3px;">
								<div class="col-md-3" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nº Horas acuse:</label>
									<div id="txtHorasAcuse"></div>
									<div id="tooltip_txtHorasAcuse">
										Se debe indicar el numero de horas para el acuse tacito o colocar cero(0),<br />
										si no desea que el sistema genere acuse tacito de manera automatica
									</div>
								</div>


								<div class="col-md-3" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Maneja Anexo:</label>
									<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: 20px">
										<div id="Anexo"></div>
										<div id="tooltip_Anexo">Activa la recepción de archivos anexos en los documentos de Factura Electrónica</div>
									</div>
								</div>

								<div class="panel panel-white col-md-5" style="z-index: 9; margin-top: 16px;">
									<div class="col-md-6" style="z-index: 9;">
										<label style="">Maneja Pagos:</label>
										<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: 20px; margin-bottom: 10px;">
											<div id="ManejaPagos"></div>
											<div id="tooltip_ManejaPagos">Indica si el Facturador maneja Pagos  Electrónicos</div>
											<%--<a class='icon-pencil3' data-toggle='modal' data-target='#modal_Resoluciones' style='margin-left:12%; font-size:19px'></a>--%>
										</div>
									</div>

									<div class="col-md-5" style="z-index: 9; margin-top: 20px; margin-bottom: 10px;">
										<div id="ModalConfiguracionPagos"></div>
									</div>
								</div>
								<%--<div class="col-md-2" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">P. Parciales?:</label>
									<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: 20px">
										<div id="PermitePagosParciales"></div>	
										<div id="tooltip_PermitePagosParciales">Indica si el Facturador Permite Pagos Parciales</div>									
									</div>
								</div>

								<div class="col-md-3" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Adte Consulta Todos:</label>
									<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: 20px">
										<div id="PermiteConsultarTodosLosDocumentos"></div>	
										<div id="tooltip_PermiteConsultarTodosLosDocumentos">Indica si el Aquiriente puede consultar todos los documentos pendientes por pago o debe especificar el documento con prefijo</div>									
									</div>
								</div>--%>
							</div>

							<div class="dx-fieldset" style="padding: -80px;">
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
					<div class="text-right" style="z-index: 9999; margin-top: -30px; margin-bottom: -8px; margin-right: 20px;" title="Actualizar">
						<div id="Recargar"></div>
					</div>
				</div>


				<div class="panel-body">
					<div class="col-lg-12">
						<div class="row">

							<div id="divEmailFacturador" class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Administrativo:<strom style="color: red;">*</label>

								<div class="col-md-11" style="z-index: 9; margin-left: -10px;">
									<div id="txtEmail"></div>
								</div>
								<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
									<i id="HtmlProc_Email"></i>
								</div>

								<div id="ttEmail" style="width: 200px"></div>
							</div>
							<div class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Envío Documentos:<strom style="color: red;">*</strom></label>

								<div class="col-md-11" style="z-index: 9; margin-left: -10px;">
									<div id="txtMailEnvio"></div>
								</div>
								<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
									<i id="HtmlProc_MailEnvio"></i>
								</div>

								<div id="ttMailEnvio"></div>
							</div>

							<div class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Recepción Documentos:<strom style="color: red;">*</label>

								<div class="col-md-11" style="z-index: 9; margin-left: -10px;">
									<div id="txtMailRecepcion"></div>
								</div>
								<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
									<i id="HtmlProc_MailRecepcion"></i>
								</div>

								<div id="ttMailRecepcion"></div>
							</div>

							<div class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Recepción Acuse:<strom style="color: red;">*</strom></label>

								<div class="col-md-11" style="z-index: 9; margin-left: -10px;">
									<div id="txtMailAcuse"></div>
								</div>
								<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
									<i id="HtmlProc_MailAcuse"></i>
								</div>

								<div id="ttMailAcuse"></div>
							</div>

							<div class="col-md-12" style="z-index: 9;">
								<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Email Pagos Electrónicos:<strom style="color: red;">*</strom></label>
								<div class="col-md-11" style="z-index: 9; margin-left: -10px;">
									<div id="txtMailPagos"></div>
								</div>
								<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
									<i id="HtmlProc_MailPagos"></i>
								</div>
								<div id="ttMailPagos"></div>
							</div>

						</div>
					</div>
				</div>
			</div>
		</div>
		<%--Panel Plataforma--%>
		<div class="col-md-12" id="PanelAdministracion" data-ng-show="Admin">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Plataforma  {{id_seguridad}}</h6>
				</div>
				<div class="panel-body">
					<div class="col-lg-12">
						<div class="row">

							<div class="col-md-6" style="margin-top: -10px;">

								<div class="col-md-12 " style="margin: 0px; margin-top: 16px; margin-bottom: 1%;">
									<div class="dx-field-label" style="font-size: 14px;">Perfil:<strom style="color: red;">*</strom></div>
									<div id="tooltip_Facturador">Indica si es Facturador</div>
									<div id="Facturador" style="margin-top: 1%"></div>

									<br />
									<div id="Adquiriente" style="margin-top: 1%"></div>
									<div id="tooltip_Adquiriente">Indica si es Adquiriente</div>
									<br />
									<div class="dx-field-label" style="font-size: 14px; margin-top: 1%"></div>
									<div id="Integradora" style="margin-top: 1%"></div>
									<div id="tooltip_Integradora">Indica si esta empresa es integradora</div>
								</div>

								<div class="col-md-12" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%; z-index: 9;">Estado:<strom style="color: red;">*</strom></label>
									<div id="cboestado"></div>
									<div id="tooltip_cboestado">Indica si la empresa esta Activa o Inactiva</div>
								</div>


								<div class="col-md-12 text-left" style="z-index: 8;">
									<label style="margin-top: 16px; padding-top: 1%;">Observaciones:</label>
									<div id="txtobservaciones"></div>
									<div id="tooltip_txtobservaciones">Se pueden especificar observaciones adicionales</div>
								</div>

								<div class="col-md-12 text-left" style="z-index: 8;">
									<label style="margin-top: 16px; padding-top: 1%;">
										Serial:

									</label>
									<div id="txtSerial"></div>
								</div>

								<div class="col-md-12 text-left" style="z-index: 8;">
									<label style="margin-top: 16px; padding-top: 1%;">
										Serial Cluod Services:

									</label>
									<div id="txtSerialCloud"></div>
									<div class="text-right" style="z-index: 9999; margin-top: -76px;" title="Obtener Serial Cloud Services">
										<div id="btnActualizarSerialCloudServices"></div>
									</div>
								</div>



							</div>


							<div class="col-md-6" style="margin-top: -10px;">

								<div class="col-md-12" style="z-index: 9;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%; z-index: 9;">Versión:<strom style="color: red;">*</strom></label>
									<div id="cboVersionDIAM"></div>
									<div id="tooltip_cboVersionDIAM">Indica la versión con la que se configuro el Facturador (DIAN)</div>
								</div>


								<div class="col-md-6" style="z-index: 9; margin: 0px; margin-bottom: 1%">
									<div class="dx-field-label" style="font-size: 14px;" id="idHabilitacion">Ambiente FE:<strom style="color: red;">*</strom></div>
									<div class="dx-field-value">
										<div id="Habilitacion"></div>
										<div id="tooltip_Habilitacion">Indica el ambiente actual de la Empresa en Factura Electrónica</div>
									</div>
								</div>

								<div class="col-md-6" style="z-index: 9; margin: 0px; margin-bottom: 1%">
									<div class="dx-field-label" style="font-size: 14px;" id="idHabilitacion_NominaE">Ambiente Nomina E:<strom style="color: red;">*</strom></div>
									<div class="dx-field-value">
										<div id="Habilitacion_NominaE"></div>
										<div id="tooltip_Habilitacion_NominaE">Indica el ambiente actual de la Empresa en Nomina Electrónica</div>
									</div>
								</div>

								<div class="col-md-6" style="z-index: 9; margin-left: 0px; margin-top: -10px;">
									<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Nº Usuarios:</label>
									<div id="txtUsuarios"></div>
									<div id="tooltip_txtUsuarios">Indica el numero de usuarios Activos que puede tener la empresa</div>
								</div>

								<div class="col-md-12">

									<div class="col-md-6" style="z-index: 9;">
										<label style="margin: 0px; margin-left: 0px; margin-top: 16px; margin-bottom: 1%">Email Recepcion:</label>
										<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: 30px">
											<div id="EmailRecepcion"></div>
											<div id="tooltip_EmailRecepcion">Indica si envia Email al adquiriente cuando la plataforma recibe el documento de Factura Electrónica</div>
										</div>
									</div>

									<div class="col-md-6  text-center" style="z-index: 9;">
										<label style="margin: 0px; margin-left: -25px; margin-top: 16px; margin-bottom: 1%">Post-Pago Auto.:</label>
										<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: -10px">
											<div id="postpagoaut"></div>
											<div id="tooltip_postpagoaut">Indica si se crea un plan post-pago automaticamente el primer dia de cada mes</div>
										</div>
									</div>

								</div>

								<div class="col-md-12" style="margin-top: 1%;">
									<div data-hgi-filtro="EmpresaAsociada"></div>
									<div id="tooltip_EmpresaAsociada">
										Se debe indicar la empresa asociada (Integradora) o
									<br />
										se debe seleccionar la misma empresa
									</div>
								</div>

								<div class="col-md-12" style="margin-top: 1%;">
									<div data-hgi-filtro="EmpresaDescuenta"></div>
									<div class="col-md-12 " id="tooltip_EmpresaDescuenta">
										Se puede indicar una empresa que este asociada para el descuento de saldo de documentos o
									<br />
										se debe seleccionar la misma empresa
									</div>
								</div>

								<div class="col-md-12">

									<div class="col-md-6  text-center" style="z-index: 9;">
										<label style="margin: 0px; margin-left: -25px; margin-top: 16px; margin-bottom: 1%">Debug:</label>
										<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: -10px">
											<div id="debug"></div>
											<div id="tooltip_debug">Indica si a la empresa se le puede hacer seguimiento de pruebas</div>
										</div>
									</div>
									<div class="col-md-6  text-center" style="z-index: 9;">
										<label style="margin: 0px; margin-left: -25px; margin-top: 16px; margin-bottom: 1%">Interoperabilidad:</label>
										<div class="col-md-12" style="z-index: 9; margin-top: 5%; margin-left: -10px">
											<div id="Interoperabilidad"></div>
											<div id="tooltip_Interoper">Indica si a la empresa procesa documentos Externos por Interoperabilidad</div>
										</div>
									</div>

								</div>


							</div>
						</div>


					</div>

				</div>
			</div>
		</div>

		<div class="col-md-12" id="PanelCertificado" data-ng-show="ReponsableFacturadorCertificado || Admin">
			<div class="panel panel-white">
				<div class="panel-heading">
					<h6 class="panel-title">Certificado</h6>
				</div>
				<div class="panel panel-white">


					<div class="panel-body">
						<div class="col-lg-12">
							<div class="row">

								<div class="col-md-12" style="margin-top: -10px;">




									<div class="col-md-6" data-ng-show="Admin">
										<div class="col-md-12" style="margin: 0px; margin-bottom: 1%">
											<div class="dx-field-label" style="font-size: 14px;" id="idCerFirma">Firma:<strom style="color: red;">*</strom></div>
											<div class="dx-field-value">
												<div id="CerFirma"></div>

											</div>
										</div>
									</div>

									<div class="col-md-12" id="PanelFirmaFacturador" data-ng-show="id_seguridad != ''">
										<div class="col-md-6">

											<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%;">Proveedor del Certificado:<strom style="color: red;">*</strom></label>
											<div id="cboProveedor"></div>




											<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Contraseña del Certificado:<strom style="color: red;">*</strom></label>
											<div id="ClaveCert"></div>


											<div class="col-md-12" style="margin-left: -20px; margin-top: 10px;" data-ng-show="Admin">
												<div class="col-md-1">
													<div class="col-md-12" style="margin-top: 5%; margin-left: -10px">
														<div id="Hgi_Responsable"></div>
													</div>
												</div>
												<div class="col-md-5">
													<label style="margin: 0px; margin-top: 5px; margin-bottom: 1%">HGI Responsable</label>
												</div>

												<div class="col-md-1">
													<div class="col-md-12" style="margin-top: 5%; margin-left: -10px">
														<div id="Hgi_Notifica"></div>
													</div>
												</div>
												<div class="col-md-5">
													<label style="margin: 0px; margin-top: 5px; margin-bottom: 1%">Notifica</label>
												</div>
											</div>

										</div>

										<div class="col-md-6" style="margin-top: 0px;" data-ng-show="id_seguridad != ''">
											<div class="col-md-12" style="margin-top: 0px;">
												<div class="file-uploader-block" style="float: right;">
													<div id="Certificado"></div>
												</div>
											</div>
										</div>

										<div class="col-md-5" style="margin-top: 5px;" data-ng-show="id_seguridad != ''">

											<div id="popup">
												<div class="popup"></div>
											</div>
											<label style="margin: 0px; margin-top: 16px; margin-bottom: 1%">Fecha Vencimiento:<strom style="color: red;">*</strom></label>
											<div id="VenceCert"></div>

											<div class="text-right" style="z-index: 9999; margin-top: -36px; margin-right: -40px;" title="Obtener información del certificado">
												<div id="InfCert"></div>
											</div>
										</div>

									</div>


								</div>


							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

	</div>
	<%--Panel Botones--%>
	<div class="col-lg-12 text-right" style="z-index: 0; margin-left: -10px;">
		<div id="file-uploader"></div>
		<a id="btncancelar" class="btn btn-default" style="text-transform: initial !important; margin-right: 1%" href="/Views/Pages/ConsultaEmpresas.aspx" style="font-size: 14px; text-align: center;">Cancelar</a>
		<div id="button"></div>
	</div>


</asp:Content>
