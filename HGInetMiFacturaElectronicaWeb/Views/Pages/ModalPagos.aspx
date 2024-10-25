<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModalPagos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ModalPagos" %>


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
	<!--Pagos-->
	<div data-ng-controller="ModalPagosController">
		<div id="modal_Pagos_Electronicos" class="modal fade" style="display: none; z-index: 99999;">
			<div class="modal-dialog">
				<div class="modal-content">
					<div id="EncabezadoModal" class="modal-header">
						<button type="button" class="close" data-dismiss="modal" style="top: 20% !important; font-weight: bold;">×</button>
						<h5 style="margin-bottom: 10px; text-align: center; font-weight: bold;" class="modal-title">Pago Electrónico</h5>
						<hr />
					</div>
					<div class="modal-body">
						<div class="col-md-12 ">

							<!-- JS Modal Pagos-->

							<!-- CONTENEDOR PRINCIPAL -->
							<div data-ng-app="ModalpagosApp" data-ng-controller="ModalPagosController" data-ng-init="Stop=true" data-data-ng-init="SinpagosPendiente=true">
								<form id="form" action="your-action">

									<div class="col-md-12">
										<div class="panel panel-white">

											<div class="panel-body">

												<div class="col-md-12">

													<table class="tg" cellpadding="0" cellspacing="0" style="vertical-align: middle" width="100%" border="0">
														<tbody>

															<td style="word-wrap: break-word; font-size: 0px; padding: 0px">
																<div style="font-size: 14px; font-family: Arial,Ubuntu">

																	<table class="m_5807442735935243114m_-2840829276345264770tg" style="table-layout: fixed; border-color: #ffffff; border-width: 1px; font-size: 14px; font-family: Arial,Ubuntu; width: 98%" align="center">

																		<tbody>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top" colspan="2">
																					<span style="font-weight: bold">Facturador Electrónico</span>
																				</td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top">Nombre:</td>
																				<td style="background-color: #ffffff; vertical-align: top"><span style="font-weight: bold">{{razonsocial}}</span></td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top">Nit:</td>
																				<td style="background-color: #ffffff; vertical-align: top">{{nit}}</td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top">Teléfono:</td>
																				<td style="background-color: #ffffff; vertical-align: top">{{telefono}}</td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top">E-mail:</td>
																				<td style="background-color: #ffffff; vertical-align: top"><a>{{email}}</a></td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top" colspan="2"><span style="font-weight: bold">Documento</span></td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top">Tipo Documento:</td>
																				<td style="background-color: #ffffff; vertical-align: top"><span style="font-weight: bold">{{tipoDoc}}</span></td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top">Número:</td>
																				<td style="background-color: #ffffff; vertical-align: top"><span style="font-weight: bold">{{documento}}</span></td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top">Fecha:</td>
																				<td style="background-color: #ffffff; vertical-align: top">{{fechadoc}}</td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top">Valor:</td>
																				<td style="background-color: #ffffff; vertical-align: top">{{montoFactura | currency:"$ " }}</td>
																			</tr>
																			<tr>
																				<td style="background-color: #efefef; vertical-align: top">
																					<div id="Pagar">Monto a Pagar:</div>
																				</td>
																				<td style="background-color: #ffffff; vertical-align: top">
																					<div id="MontoPago"></div>
																				</td>
																			</tr>
																		</tbody>
																	</table>
																</div>
															</td>


														</tbody>
													</table>



												</div>

												<!--
                                                <div class="col-md-12">
                                                    
                                                    <div class="col-md-4">
                                                        <label>Monto Pendiente:</label>
                                                    </div>
                                                    <div class="col-md-4" id="divValorPendiente">
                                                    </div>
                                                    <div class="col-md-4 text-center">
                                                        <div style="visibility: hidden" id="PagoTotal"></div>
                                                    </div>
                                                </div>-->
												<div data-ng-include="'Partials/ModalFormasDePago.html'"></div>

												<div id="panelPagoPendiente">
													<div class="col-md-12 text-right" id="PanelPago">
														<br />

														<div id="button"></div>


														<div data-dx-button="buttonProcesar" data-ng-if="EnProceso">
															<i class="icon-spinner6 spinner mr-2"></i>
														</div>




													</div>



												</div>
												<div class="col-md-12" id="PanelVerificacion">
													<div class="col-md-8">
														<label style="margin-top:10px; color:red;" id="mensaje"></label>
													</div>
													<div class="col-md-4 text-right">
														<div data-dx-button="buttonVerificar"></div>
													</div>

												</div>

												<div class="dx-fieldset">
													<div id="summary"></div>
												</div>
											</div>


										</div>
									</div>


									<div class="col-md-12" id="Detallepagos">
										<div class="panel panel-white">

											<div class="panel-body">
												<div class="demo-container">
													<div id="grid"></div>
												</div>
											</div>



										</div>
									</div>

								</form>


							</div>


						</div>
					</div>

					<div id="divsombra" class="modal-footer" style="margin-top: 22%">
					</div>

				</div>
			</div>
		</div>
	</div>
	<!--Pagos-->

</body>
</html>

