<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModalDetalleEmpresa.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ModalDetalleEmpresa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>

	<script src="../../Scripts/config.js?vjs201926"></script>
	<script src="../../Scripts/Pages/ModalDetalleEmpresa.js?vjs201926"></script>

	<style type="text/css">
		#outlook a {
			padding: 0;
		}

		.ReadMsgBody {
			width: 100%;
		}

		.ExternalClass {
			width: 100%;
		}

			.ExternalClass * {
				line-height: 100%;
			}

		body {
			margin: 0;
			padding: 0;
			-webkit-text-size-adjust: 100%;
			-ms-text-size-adjust: 100%;
		}

		table, td {
			border-collapse: collapse;
			mso-table-lspace: 0pt;
			mso-table-rspace: 0pt;
		}

		img {
			border: 0;
			height: auto;
			line-height: 100%;
			outline: none;
			text-decoration: none;
			-ms-interpolation-mode: bicubic;
		}

		p {
			display: block;
			margin: 10px 0;
		}

		.tg td {
			font-family: Arial, Ubuntu;
			padding: 2px;
			border-style: solid;
			border-width: 4px;
			overflow: hidden;
			word-break: normal;
			border-color: #FFFFFF;
		}

		.tg th {
			font-family: Arial, Ubuntu;
			font-size: 14px;
			font-weight: normal;
			padding: 10px 5px;
			border-style: none;
			border-width: 1px;
			overflow: hidden;
			word-break: normal;
			border-color: Gray;
		}

		.modal-dialog {
			width: 60%;
			margin: 30px auto;
		}
	</style>


</head>
<body style="background-color: #ffffff" class="login-container">
	<form runat="server">

		<div data-ng-app="ModalDetalleEmpresasApp" data-ng-controller="EmpresasModalController">
			<div id="modal_detalle_empresa" class="modal fade" style="top: 10%; display: none; z-index: 999999;">
				<div class="modal-dialog">
					<div class="modal-content">
						<div class="col-sm-10 col-sm-offset-1 col-lg-10 col-lg-offset-1" style="margin-top: 2%">

							<div class="panel panel-white">

								<div class="panel-body">

									<div class="col-md-12">

										<table class="tg" cellpadding="0" cellspacing="0" style="vertical-align: middle" width="100%" border="0">
											<tbody>
												<tr>
													<td style="word-wrap: break-word; font-size: 0px; padding: 0px">
														<div style="font-size: 14px; font-family: Arial,Ubuntu">

															<table class="m_5807442735935243114m_-2840829276345264770tg" style="table-layout: fixed; border-color: #ffffff; border-width: 1px; font-size: 14px; font-family: Arial,Ubuntu; width: 98%" align="center">
																<tbody>


																	<tr>
																		<td style="background-color: #efefef; vertical-align: top" colspan="10">
																			<span style="font-weight: bold">Datos Basicos</span>
																		</td>
																	</tr>
																	<tr>
																		<td colspan="3" style="background-color: #efefef; vertical-align: top">Id de Seguridad:</td>
																		<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{IdSeguridad}}</td>
																	</tr>
																	<tr>
																		<td colspan="3" style="background-color: #efefef; vertical-align: top">{{TipoIndentificacion}}:</td>
																		<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{Nit}}</td>
																	</tr>

																	<tr>
																		<td colspan="3" style="background-color: #efefef; vertical-align: top">Razón Social:</td>
																		<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{RazonSocial}}</td>

																	</tr>

																	<tr>
																		<td colspan="3" style="background-color: #efefef; vertical-align: top">Perfil:</td>
																		<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{Perfil}}</td>
																	</tr>

																	<tr>
																		<td colspan="3" style="background-color: #efefef; vertical-align: top">Estado:</td>
																		<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{Estado}}</td>

																	</tr>
																	<tr>
																		<td colspan="3" style="background-color: #efefef; vertical-align: top">Observaciones:</td>
																		<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{StrObservaciones}}</td>
																	</tr>
																</tbody>
															</table>
															<div data-ng-if="Facturador">
																<table class="tg" cellpadding="0" cellspacing="0" style="vertical-align: middle" width="100%" border="0">
																	<tbody>
																		<tr>
																			<td style="word-wrap: break-word; font-size: 0px; padding: 0px">
																				<div style="font-size: 14px; font-family: Arial,Ubuntu">

																					<table class="m_5807442735935243114m_-2840829276345264770tg" style="table-layout: fixed; border-color: #ffffff; border-width: 1px; font-size: 14px; font-family: Arial,Ubuntu; width: 100%" align="center">
																						<tbody>


																							<tr>
																								<td style="background-color: #efefef; vertical-align: top" colspan="10">
																									<span style="font-weight: bold">Datos Facturador</span>
																								</td>
																							</tr>

																							<tr data-ng-if="Administradora">
																								<td colspan="10" style="background-color: #efefef; vertical-align: top; text-align: center">Empresa Administradora</td>
																							</tr>

																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Integrador:</td>
																								<td colspan="2" style="background-color: #ffffff; vertical-align: top">{{Integrador}}</td>

																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Email Recepcion:</td>
																								<td colspan="2" style="background-color: #ffffff; vertical-align: top">{{IntEmailRecepcion}}</td>
																							</tr>
																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Correo Administrativo:</td>
																								<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{Email}}</td>
																								<td colspan="1" style="background-color: #ffffff; vertical-align: top">
																									<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
																										<i id="Html_ModalProc_Email" class="icon-cross2 text-danger-400" title="En proceso de Registro"></i>
																									</div>
																								</td>
																							</tr>
																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Correo Envío:</td>
																								<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{StrMailEnvio}}</td>
																								<td colspan="1" style="background-color: #ffffff; vertical-align: top">
																									<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
																										<i id="Html_ModalProc_MailEnvio" class="icon-cross2 text-danger-400" title="En proceso de Registro"></i>
																									</div>
																								</td>
																							</tr>
																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Correo de Recepción:</td>
																								<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{StrMailRecepcion}}</td>
																								<td colspan="1" style="background-color: #ffffff; vertical-align: top">
																									<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
																										<i id="Html_ModalProc_MailRecepcion" class="icon-cross2 text-danger-400" title="En proceso de Registro"></i>
																									</div>
																								</td>
																							</tr>

																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Correo Acuse:</td>
																								<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{StrMailAcuse}}</td>
																								<td colspan="1" style="background-color: #ffffff; vertical-align: top">
																									<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
																										<i id="Html_ModalProc_MailAcuse" class="icon-cross2 text-danger-400" title="En proceso de Registro"></i>
																									</div>
																								</td>
																							</tr>

																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Correo Pagos:</td>
																								<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{StrMailPagos}}</td>
																								<td colspan="1" style="background-color: #ffffff; vertical-align: top">
																									<div class="col-md-1" style="z-index: 9; margin-top: 6px;">
																										<i id="Html_ModalProc_MailPagos" class="icon-cross2 text-danger-400" title="En proceso de Registro"></i>
																									</div>
																								</td>
																							</tr>

																							
																							
																							
																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Empresa Asociada:</td>
																								<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{StrEmpresaAsociada}}</td>
																							</tr>
																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Empresa Descuenta Planes:</td>
																								<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{StrEmpresaDescuenta}}</td>
																							</tr>
																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Post-Pago Aut.:</td>
																								<td colspan="2" style="background-color: #ffffff; vertical-align: top">{{PostPago}}</td>

																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Maneja Anexos:</td>
																								<td colspan="2" style="background-color: #ffffff; vertical-align: top">{{Anexo}}</td>
																							</tr>
																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Nº Usuarios:</td>
																								<td colspan="2" style="background-color: #ffffff; vertical-align: top">{{IntNumUsuarios}}</td>

																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Nº Horas acuse:</td>
																								<td colspan="2" style="background-color: #ffffff; vertical-align: top">{{IntAcuseTacito}}</td>
																							</tr>

																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Habilitación:</td>
																								<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{Habilitacion}}</td>
																							</tr>
																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">versión:</td>
																								<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{Version}}</td>
																							</tr>
																							<tr>
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Firma:</td>
																								<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{Firma}}</td>
																							</tr>
																							<tr data-ng-show="MuestraFechaVenc">
																								<td colspan="3" style="background-color: #efefef; vertical-align: top">Fecha Vence:</td>
																								<td colspan="7" style="background-color: #ffffff; vertical-align: top">{{FechaVenc}}</td>
																							</tr>
																						</tbody>
																					</table>
																				</div>
																			</td>
																		</tr>
																	</tbody>
																</table>
															</div>


														</div>
													</td>
												</tr>

											</tbody>
										</table>

									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>

		</div>

	</form>
</body>
</html>
