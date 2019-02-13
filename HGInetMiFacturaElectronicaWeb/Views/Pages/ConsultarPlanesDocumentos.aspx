<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Masters/MasterPrincipal.Master" AutoEventWireup="true" CodeBehind="ConsultarPlanesDocumentos.aspx.cs" Inherits="HGInetMiFacturaElectronicaWeb.Views.Pages.ConsultarPlanesDocumentos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContenidoPagina" runat="server">
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

    <script src="../../Scripts/Services/MaestrosEnum.js?vjs201913"></script>
    <script src="../../Scripts/Pages/ConsultarPlanesDocumentos.js?vjs201913"></script>

    <div data-ng-app="GestionPlanesApp" data-ng-controller="ConsultaPlanesController">

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

						<div class="col-lg-12">

							<div class="row" style=" margin-left:-2%;">

								<div class="dx-fieldset" style="margin-top: -1%;">

                                    <div class="col-md-3" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Tipo de Plan:</label>
										<div id="Tipoplan"></div>
									</div>

									<div class="col-md-3" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Filtro Fecha:</label>
										<div data-dx-select-box="filtros.TipoFiltroFecha"></div>
									</div>

									<div class="col-md-3" style="margin-top: 1%">
										<i class=" icon-calendar"></i>
										<label>Fecha Inicial:</label>
										<div id="FechaInicial"></div>
									</div>


									<div class="col-md-3" style="margin-top: 1%">
										<i class=" icon-calendar"></i>
										<label>Fecha Final:</label>
										<div id="FechaFinal"></div>
									</div>																		

									<div class="col-md-6" style="margin-top: 1%">
										<i class="icon-file-text"></i>
										<label>Estado del Plan:</label>
										<div id="Estadoplan"></div>
									</div>

									<div class="col-md-6 text-right" >
										<div data-dx-button="ButtonOptionsConsultar" style="margin-top: 10%" ></div>
									</div>                               
								</div>

							</div>


						</div>						

						<p data-ng-bind-html="message"></p>

					</div>

				</div>
			</div>

			<!--/FILTROS DE BÚSQUEDA -->

                <%--//Panel Grid--%>
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
                <div class="panel-body" style="margin-top: 2%">
                    <div class="demo-container">
                        <div id="grid"></div>
                    </div>

                </div>

            </div>
        </div>
		<%-- ***************************************************** --%>
		 <div >
        <div id="modal_detalle_plan" class="modal fade" style="top: 20%; display: none; z-index: 999999;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div id="EncabezadoModal" class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">×</button>
                        <h5 style="margin-bottom: 10px;" class="modal-title">Planes y Transacciones</h5>
                    </div>
                    <div class="modal-body">
                        <div class="col-md-12 ">
                            <!-- CONTENEDOR PRINCIPAL -->
                            <div>
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
                                                                                    <span style="font-weight: bold">Detalle del Plan</span>
                                                                                </td>
                                                                            </tr>
																	
																			<tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Fecha de Creación:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{Fecha | date : "yyyy-MM-dd HH:mm"}}</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Tipo de Plan:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{Tipo == 1 ? "Recarga Interna" : Tipo == 2 ? "Compra" : Tipo == 3 ? "Post-Pago":""}}</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Nit:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{CodigoEmpresaFacturador}}</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Facturador:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top"><span style="font-weight: bold">{{EmpresaFacturador}}</span></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Nº Transacciones:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{TCompra}}</td>
                                                                            </tr>                                                                                                                                                       
                                                                            <tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Nº Transacciones Procesadas:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{TProcesadas}}</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Nº Transacciones Disponibles:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{TDisponibles}}</td>
                                                                            </tr>
																			<tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Valor Plan:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{Valor | currency:"$ " }}</td>
                                                                            </tr>
																			<tr>																				
																					<td style="background-color: #efefef; vertical-align: top">Porcentaje:</td>
																					<td><div class="bullet Hgi_plan"></div><label id="hgi_porcentaje_plan" style="font-size:12px; margin:0px; padding:0px;">70%</label></td>																					
																			</tr>
                                                                            <tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Estado:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top"><span    data-ng-class="{'badge bg-danger':Estado == 1 , 'badge bg-success-400':Estado == 0,'badge bg-grey-400':Estado==2}"  style="font-size: 11px !important; font-weight: bold; border-radius: 0px !important;">{{Estado == 0 ? "Habilitado" : Estado == 1 ? "Inhabilitado" : Estado == 2 ? "Procesado":""}}</span></td>
                                                                            </tr> 
																			 <tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Empresa Creación:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{Empresa}}</td>
                                                                            </tr> 
																			 <tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Usuario Creación:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{Usuario}}</td>
                                                                            </tr> 
																			<tr>
                                                                                <td style="background-color: #efefef; vertical-align: top">Fecha de Vencimiento:</td>
                                                                                <td style="background-color: #ffffff; vertical-align: top">{{FechaVence | date : "yyyy-MM-dd"}}</td>
                                                                            </tr>																			                                                                       

																			

                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tbody>
                                                    </table>
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
		<%-- ***************************************************** --%>
    </div>
</asp:Content>
