<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModalDetallePago.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ModalDetallePago" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
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
<body>
	<form>
		<div id="modal_detalles_pago" class="modal fade" style="display: none;">
			<div class="modal-dialog modal-lg">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" class="close" data-dismiss="modal">×</button>
						<h5 style="margin-bottom: 10px;" class="modal-title">Detalles Pago Electrónico</h5>
					</div>
					<div class="modal-body">
						<div class="col-md-12 ">

							<!-- CONTENEDOR PRINCIPAL -->
							<div data-ng-app="ModalDetallesPagoApp" data-ng-controller="ModalDetallesPagoController">

								<div class="col-md-12">
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
																					<span style="font-weight: bold">Datos del Documento</span>
																				</td>
																			</tr>
																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Fecha Emisión:</td>
																				<td colspan="3" style="background-color: #ffffff; vertical-align: top">{{DatFechaEmisionDoc | date : "yyyy-MM-dd HH:mm"}}</td>

																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Fecha Vencimiento:</td>
																				<td colspan="3" style="background-color: #ffffff; vertical-align: top">{{DatFechaVenceDoc | date : "yyyy-MM-dd"}}</td>
																			</tr>

																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Id Radicado:</td>
																				<td colspan="3" style="background-color: #ffffff; vertical-align: top">{{StrIdSeguridadDoc}}</td>

																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Número Documento:</td>
																				<td colspan="3" style="background-color: #ffffff; vertical-align: top">{{StrNumerDoc}}</td>
																			</tr>

																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Nit Adquiriente:</td>
																				<td colspan="3" style="background-color: #ffffff; vertical-align: top">{{StrClienteIdentificacion}}</td>

																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Razón Social:</td>
																				<td colspan="3" style="background-color: #ffffff; vertical-align: top">{{StrClienteNombre}}</td>
																			</tr>

																			<tr>
																				<td style="background-color: #efefef; vertical-align: top" colspan="10">
																					<span style="font-weight: bold">Datos del Pago</span>
																				</td>
																			</tr>
																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Fecha Registro:</td>
																				<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{DatFechaRegistro | date : "yyyy-MM-dd HH:mm:ss"}}</td>
																			</tr>
																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Id Plataforma:</td>
																				<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{StrIdPlataforma}}</td>
																			</tr>
																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Id Pago:</td>
																				<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{StrIdSeguridadRegistro}}</td>
																			</tr>
																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Valor Pago:</td>
																				<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{IntValor | currency:"$ "}}</td>
																			</tr>

																			<tr>
																				<td style="background-color: #efefef; vertical-align: top" colspan="10">
																					<span style="font-weight: bold">Forma de Pago</span>
																				</td>
																			</tr>
																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Banco:</td>
																				<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{StrPagoDesBanco}}</td>
																			</tr>

																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Forma de Pago:</td>
																				<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{StrPagoDesFormaPago}}</td>
																			</tr>

																			<tr data-ng-show="FranquiciaFormaPago">
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Forma de Pago:</td>
																				<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{StrPagoCodFranquicia}}</td>
																			</tr>

																			<tr>
																				<td style="background-color: #efefef; vertical-align: top" colspan="10">
																					<span style="font-weight: bold">Verificación del Pago</span>
																				</td>
																			</tr>
																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Fecha:</td>
																				<td colspan="6" style="background-color: #ffffff; vertical-align: top">{{DatFechaVerificacion | date : "yyyy-MM-dd HH:mm:ss"}}</td>
																			</tr>

																			<tr>
																				<td colspan="2" style="background-color: #efefef; vertical-align: top">Respuesta:</td>
																				<td colspan="6" style="background-color: #ffffff; vertical-align: top"><div id="RespuestaPago"></div></td>
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
									</div>
								</div>
							</div>


						</div>
					</div>

					<div id="divsombra" class="modal-footer" style="margin-top: 22%">
					</div>

				</div>
			</div>
		</div>



	</form>
</body>
</html>
