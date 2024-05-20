using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraReports.Native;
using System.Data;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using System.Reflection;
using LibreriaGlobalHGInet.Objetos;
using HGInetFacturaEReports.ReportDesigner;
using HGInetMiFacturaElectonicaData.ModeloServicio.Documentos;
using HGInetMiFacturaElectonicaData.Formatos;

namespace HGInetMiFacturaElectronicaWeb
{
	public class SerializeReport : IDataSerializer
	{
		public const string Name = "MyDataSerializer";
		private TipoDocumento TipoDocumento { get; set; }
		public string DataMember { get; set; }

		public SerializeReport()
		{
		}

		public SerializeReport(TipoDocumento tipo_documento)
		{
			this.TipoDocumento = tipo_documento;
		}

		public bool CanDeserialize(string value, string typeName, object extensionProvider)
		{
			return value == "DataSetReporte";
		}

		public bool CanSerialize(object data, object extensionProvider)
		{
			return data is DataSet;
		}

		public object Deserialize(string value, string typeName, object extensionProvider)
		{
			if (value == "DataSetReporte")
			{
				if (this.TipoDocumento.GetHashCode() < TipoDocumento.AcuseRecibo.GetHashCode())
					return GenerarColumnas();
				else
					return GenerarColumnasNom();
			}
			return null;
		}

		public string Serialize(object data, object extensionProvider)
		{
			var ds = (DataSet)data;
			return ds.DataSetName;
		}

		public DataSet GenerarColumnas()
		{
			var ds = new DataSet();
			ds.DataSetName = "DataSetReporte";

			DataTable DtPrincipal = new DataTable();
			DataColumn principal = new DataColumn();

			XtraReportDesigner reporte = new XtraReportDesigner();
			reporte.Name = "DataSet";

			switch (this.TipoDocumento)
			{
				case TipoDocumento.NotaDebito:
					//DATOS TABLA NOTADEBITO
					DtPrincipal = new DataTable("NotaDebito");
					DtPrincipal.TableName = "NotaDebito";
					foreach (PropertyInfo info in typeof(NotaDebito).GetProperties())
					{
						if (info.PropertyType != typeof(Tercero) && !info.PropertyType.Name.Equals("List`1") && info.PropertyType != typeof(ReferenciaAdicional) && info.PropertyType != typeof(TasaCambio))
						{
							DtPrincipal.Columns.Add(info.Name.ToString());
						}
					}
					//Agrega la tabla principal al dataset
					ds.Tables.Add(DtPrincipal);

					principal = ds.Tables["NotaDebito"].Columns["Documento"];

					this.DataMember = "NotaDebito";
					break;

				case TipoDocumento.NotaCredito:
					//DATOS TABLA NOTACREDITO
					DtPrincipal = new DataTable("NotaCredito");
					DtPrincipal.TableName = "NotaCredito";
					foreach (PropertyInfo info in typeof(NotaCredito).GetProperties())
					{
						if (info.PropertyType != typeof(Tercero) && !info.PropertyType.Name.Equals("List`1") && info.PropertyType != typeof(ReferenciaAdicional) && info.PropertyType != typeof(TasaCambio))
						{
							DtPrincipal.Columns.Add(info.Name.ToString());
						}
					}
					//Agrega la tabla principal al dataset
					ds.Tables.Add(DtPrincipal);

					principal = ds.Tables["NotaCredito"].Columns["Documento"];
					this.DataMember = "NotaCredito";
					break;

				default:
					//DATOS TABLA FACTURA
					DtPrincipal = new DataTable("Factura");
					DtPrincipal.TableName = "Factura";
					foreach (PropertyInfo info in typeof(Factura).GetProperties())
					{
						if (info.PropertyType != typeof(Tercero) && !info.PropertyType.Name.Equals("List`1") && info.PropertyType != typeof(ReferenciaAdicional) && info.PropertyType != typeof(ReferenciaPago) && info.PropertyType != typeof(TasaCambio))
						{
							DtPrincipal.Columns.Add(info.Name.ToString());
						}
					}
					//Agrega la tabla principal al dataset
					ds.Tables.Add(DtPrincipal);

					principal = ds.Tables["Factura"].Columns["Documento"];

					this.DataMember = "Factura";
					break;
			}





			//DATOS TABLA DATOS DEL ADQUIRIENTE
			DataTable DatosAdquiriente = new DataTable("DatosAdquiriente");
			DatosAdquiriente.TableName = "DatosAdquiriente";
			foreach (PropertyInfo info in typeof(Tercero).GetProperties())
			{
				DatosAdquiriente.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(DatosAdquiriente);

			DataColumn adquiriente = ds.Tables["DatosAdquiriente"].Columns["Identificacion"];
			DataRelation relation_adquiriente = new DataRelation("DatosAdquiriente", principal, adquiriente);
			relation_adquiriente.Nested = true;
			ds.Tables["DatosAdquiriente"].ParentRelations.Add(relation_adquiriente);


			//DATOS TABLA DATOS DEL OBLIGADO
			DataTable DatosObligado = new DataTable("DatosObligado");
			DatosObligado.TableName = "DatosObligado";
			foreach (PropertyInfo info in typeof(Tercero).GetProperties())
			{
				DatosObligado.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(DatosObligado);

			DataColumn facturador = ds.Tables["DatosObligado"].Columns["Identificacion"];
			DataRelation relation_facturador = new DataRelation("DatosObligado", principal, facturador);
			relation_facturador.Nested = true;
			ds.Tables["DatosObligado"].ParentRelations.Add(relation_facturador);


			//DATOS TABLA DATOS DE LOS DETALLES DEL DOCUMENTO
			DataTable DocumentoDetalles = new DataTable("DocumentoDetalles");
			DocumentoDetalles.TableName = "DocumentoDetalles";
			foreach (PropertyInfo info in typeof(DocumentoDetalle).GetProperties())
			{
				if (!info.PropertyType.Name.Equals("List`1"))
				{
					DocumentoDetalles.Columns.Add(info.Name.ToString());
				}
			}

			ds.Tables.Add(DocumentoDetalles);

			DataColumn datos_detalles = ds.Tables["DocumentoDetalles"].Columns["Codigo"];
			DataRelation relation_detalles = new DataRelation("DocumentoDetalles", principal, datos_detalles);
			relation_detalles.Nested = true;
			ds.Tables["DocumentoDetalles"].ParentRelations.Add(relation_detalles);


			//DATOS TABLA DATOS DE CAMPO-VALOR DETALLE
			DataTable CamposAdicionalesDet = new DataTable("CamposAdicionales");
			CamposAdicionalesDet.TableName = "CamposAdicionales";
			foreach (PropertyInfo info in typeof(CampoValor).GetProperties())
			{
				CamposAdicionalesDet.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(CamposAdicionalesDet);

			
			DataColumn datos_det_campos_adicionales = ds.Tables["CamposAdicionales"].Columns["Descripcion"];
			DataRelation relation_det_campos_adicionales = new DataRelation("CamposAdicionales", datos_detalles, datos_det_campos_adicionales);
			relation_det_campos_adicionales.Nested = true;
			ds.Tables["CamposAdicionales"].ParentRelations.Add(relation_det_campos_adicionales);


			//DATOS TABLA DATOS DE LAS CUOTAS
			DataTable Cuotas = new DataTable("Cuotas");
			Cuotas.TableName = "Cuotas";
			foreach (PropertyInfo info in typeof(Cuota).GetProperties())
			{
				Cuotas.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(Cuotas);

			DataColumn datos_cuotas = ds.Tables["Cuotas"].Columns["Codigo"];
			DataRelation relation_cuotas = new DataRelation("Cuotas", principal, datos_cuotas);
			relation_cuotas.Nested = true;
			ds.Tables["Cuotas"].ParentRelations.Add(relation_cuotas);


			//DATOS TABLA DATOS DE ORDERREFERENCE
			DataTable Order = new DataTable("OrderReference");
			Order.TableName = "OrderReference";
			foreach (PropertyInfo info in typeof(ReferenciaAdicional).GetProperties())
			{
				Order.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(Order);

			DataColumn datos_orderref = ds.Tables["OrderReference"].Columns["Documento"];
			DataRelation relation_orderref = new DataRelation("OrderReference", principal, datos_orderref);
			relation_orderref.Nested = true;
			ds.Tables["OrderReference"].ParentRelations.Add(relation_orderref);


			//DATOS TABLA DATOS DE DESPATCHREFERENCE
			DataTable DespatchDocument = new DataTable("DespatchDocument");
			DespatchDocument.TableName = "DespatchDocument";
			foreach (PropertyInfo info in typeof(ReferenciaAdicional).GetProperties())
			{
				DespatchDocument.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(DespatchDocument);

			DataColumn datos_Despatchref = ds.Tables["DespatchDocument"].Columns["Documento"];
			DataRelation relation_Despatchref = new DataRelation("DespatchDocument", principal, datos_Despatchref);
			relation_Despatchref.Nested = true;
			ds.Tables["DespatchDocument"].ParentRelations.Add(relation_Despatchref);


			//DATOS TABLA DATOS DE RECEIPTREFERENCE
			DataTable ReceiptDocument = new DataTable("ReceiptDocument");
			ReceiptDocument.TableName = "ReceiptDocument";
			foreach (PropertyInfo info in typeof(ReferenciaAdicional).GetProperties())
			{
				ReceiptDocument.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(ReceiptDocument);

			DataColumn datos_Receipref = ds.Tables["ReceiptDocument"].Columns["Documento"];
			DataRelation relation_Receipref = new DataRelation("ReceiptDocument", principal, datos_Receipref);
			relation_Receipref.Nested = true;
			ds.Tables["ReceiptDocument"].ParentRelations.Add(relation_Receipref);


			//DATOS TABLA DATOS DE RECEIPTREFERENCE
			DataTable DocumentosReferencia = new DataTable("DocumentosReferencia");
			DocumentosReferencia.TableName = "DocumentosReferencia";
			foreach (PropertyInfo info in typeof(ReferenciaAdicional).GetProperties())
			{
				DocumentosReferencia.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(DocumentosReferencia);

			DataColumn datos_Aditionalref = ds.Tables["DocumentosReferencia"].Columns["Documento"];
			DataRelation relation_Aditionalref = new DataRelation("DocumentosReferencia", principal, datos_Aditionalref);
			relation_Aditionalref.Nested = true;
			ds.Tables["DocumentosReferencia"].ParentRelations.Add(relation_Aditionalref);


			//DATOS TABLA DATOS DE CAMPO-VALOR
			DataTable CamposAdicionales = new DataTable("CamposAdicionalesEnc");
			CamposAdicionales.TableName = "CamposAdicionalesEnc";
			foreach (PropertyInfo info in typeof(CampoValor).GetProperties())
			{
				CamposAdicionales.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(CamposAdicionales);

			DataColumn datos_CamposAdicionales = ds.Tables["CamposAdicionalesEnc"].Columns["Descripcion"];
			DataRelation relation_CamposAdicionales = new DataRelation("CamposAdicionalesEnc", principal, datos_CamposAdicionales);
			relation_CamposAdicionales.Nested = true;
			ds.Tables["CamposAdicionalesEnc"].ParentRelations.Add(relation_CamposAdicionales);

			//DATOS TABLA DATOS DE DESCUENTOS
			DataTable Descuentos = new DataTable("Descuentos");
			Descuentos.TableName = "Descuentos";
			foreach (PropertyInfo info in typeof(Descuento).GetProperties())
			{
				Descuentos.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(Descuentos);

			DataColumn datos_Descuentos = ds.Tables["Descuentos"].Columns["Codigo"];
			DataRelation relation_Descuentos = new DataRelation("Descuentos", principal, datos_Descuentos);
			relation_Descuentos.Nested = true;
			ds.Tables["Descuentos"].ParentRelations.Add(relation_Descuentos);

			//DATOS TABLA DATOS DE ReferenciaPago
			DataTable ReferenciaPago = new DataTable("ReferenciaPago");
			ReferenciaPago.TableName = "ReferenciaPago";
			foreach (PropertyInfo info in typeof(ReferenciaPago).GetProperties())
			{
				ReferenciaPago.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(ReferenciaPago);

			DataColumn datos_RefPago = ds.Tables["ReferenciaPago"].Columns["EAN"];
			DataRelation relation_RefPago = new DataRelation("ReferenciaPago", principal, datos_RefPago);
			relation_RefPago.Nested = true;
			ds.Tables["ReferenciaPago"].ParentRelations.Add(relation_RefPago);

			//DATOS TABLA DATOS DE TasaCambio
			DataTable Trm = new DataTable("Trm");
			Trm.TableName = "Trm";
			foreach (PropertyInfo info in typeof(TasaCambio).GetProperties())
			{
				Trm.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(Trm);

			DataColumn datos_TasaCambio = ds.Tables["Trm"].Columns["Moneda"];
			DataRelation relation_TasaCambio = new DataRelation("Trm", principal, datos_TasaCambio);
			relation_TasaCambio.Nested = true;
			ds.Tables["Trm"].ParentRelations.Add(relation_TasaCambio);

			return ds;
		}

		public DataSet GenerarColumnasNom()
		{

			var ds = new DataSet();
			ds.DataSetName = "DataSetReporte";

			DataTable DtPrincipal = new DataTable();
			DataColumn principal = new DataColumn();

			XtraReportDesigner reporte = new XtraReportDesigner();
			reporte.Name = "DataSet";

			switch (this.TipoDocumento)
			{
				

				case TipoDocumento.NominaAjuste:
					//DATOS TABLA NOMINA
					DtPrincipal = new DataTable("NominaAjuste");
					DtPrincipal.TableName = "NominaAjuste";
					foreach (PropertyInfo info in typeof(NominaAjuste).GetProperties())
					{
						if (info.PropertyType != typeof(Empleador) && info.PropertyType != typeof(Trabajador) && !info.PropertyType.Name.Equals("List`1"))
						{
							DtPrincipal.Columns.Add(info.Name.ToString());
						}
					}
					//Agrega la tabla principal al dataset
					ds.Tables.Add(DtPrincipal);

					principal = ds.Tables["NominaAjuste"].Columns["Planilla"];
					this.DataMember = "NominaAjuste";
					break;

				default:
					//DATOS TABLA NOMINA
					DtPrincipal = new DataTable("Nomina");
					DtPrincipal.TableName = "Nomina";
					//foreach (PropertyInfo info in typeof(Nomina).GetProperties())
					//{
					//	if (info.PropertyType != typeof(Empleador) && info.PropertyType != typeof(Trabajador) && !info.PropertyType.Name.Equals("List`1") && info.PropertyType != typeof(Periodo)
					//	&& info.PropertyType != typeof(Pago) && info.PropertyType != typeof(Devengados) && info.PropertyType != typeof(Deducciones) && info.PropertyType != typeof(Transporte)
					//	&& info.PropertyType != typeof(Hora))
					//	{
					//		DtPrincipal.Columns.Add(info.Name.ToString());
					//	}
					//}
					////Agrega la tabla principal al dataset
					//ds.Tables.Add(DtPrincipal);

					//principal = ds.Tables["Nomina"].Columns["Documento"];
					//this.DataMember = "Nomina";

					List<string> lista_excepciones = new List<string>() { };

					foreach (PropertyInfo info in typeof(Planilla).GetProperties())
					{
						if (!info.PropertyType.Name.Equals("List`1"))
						{
							DtPrincipal.Columns.Add(info.Name.ToString());
						}
					}

					//Agrega la tabla principal al dataset
					ds.Tables.Add(DtPrincipal);
					principal = ds.Tables["Nomina"].Columns["Documento"];
					DataMember = "Nomina";

					break;
			}

			//DATOS TABLA DATOS DE LOS DETALLES DEL DOCUMENTO
			DataTable Novedades = new DataTable("Novedades");
			Novedades.TableName = "Novedades";
			foreach (PropertyInfo info in typeof(Novedad).GetProperties())
			{
				if (!info.PropertyType.Name.Equals("List`1"))
				{
					Novedades.Columns.Add(info.Name.ToString());
				}
			}

			ds.Tables.Add(Novedades);

			DataColumn datos_Novedad = ds.Tables["Novedades"].Columns["Concepto"];
			DataRelation relation_Novedad = new DataRelation("Novedades", principal, datos_Novedad);
			relation_Novedad.Nested = true;
			ds.Tables["Novedades"].ParentRelations.Add(relation_Novedad);

			/*
			//DATOS TABLA DATOS DEL EMPLEADOR(OBLIGADO)
			DataTable DatosEmpleador = new DataTable("DatosEmpleador");
			DatosEmpleador.TableName = "DatosEmpleador";
			foreach (PropertyInfo info in typeof(Empleador).GetProperties())
			{
				DatosEmpleador.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(DatosEmpleador);

			DataColumn empleador = ds.Tables["DatosEmpleador"].Columns["Identificacion"];
			DataRelation relation_empleador = new DataRelation("DatosEmpleador", principal, empleador);
			relation_empleador.Nested = true;
			ds.Tables["DatosEmpleador"].ParentRelations.Add(relation_empleador);


			//DATOS TABLA DATOS DEL TRABAJADOR(ADQUIRIENTE)
			DataTable DatosTrabajador = new DataTable("DatosTrabajador");
			DatosTrabajador.TableName = "DatosTrabajador";
			foreach (PropertyInfo info in typeof(Trabajador).GetProperties())
			{
				DatosTrabajador.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(DatosTrabajador);

			DataColumn trabajador = ds.Tables["DatosTrabajador"].Columns["Identificacion"];
			DataRelation relation_trabajador = new DataRelation("DatosTrabajador", principal, trabajador);
			relation_trabajador.Nested = true;
			ds.Tables["DatosTrabajador"].ParentRelations.Add(relation_trabajador);

			//DATOS TABLA DATOS DEL PERIODO DEL DOCUMENTO NOMINA
			DataTable Periodo = new DataTable("Periodo");
			Periodo.TableName = "Periodo";
			foreach (PropertyInfo info in typeof(Periodo).GetProperties())
			{
				Periodo.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(Periodo);

			DataColumn periodo = ds.Tables["Periodo"].Columns["FechaIngreso"];
			DataRelation relation_periodo = new DataRelation("Periodo", principal, periodo);
			relation_periodo.Nested = true;
			ds.Tables["Periodo"].ParentRelations.Add(relation_periodo);

			//DATOS TABLA DATOS DEL PERIODO DEL DOCUMENTO NOMINA
			DataTable Pago = new DataTable("Pago");
			Pago.TableName = "Pago";
			foreach (PropertyInfo info in typeof(Pago).GetProperties())
			{
				Pago.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(Pago);

			DataColumn pago = ds.Tables["Pago"].Columns["Forma"];
			DataRelation relation_pago = new DataRelation("Pago", principal, pago);
			relation_pago.Nested = true;
			ds.Tables["Pago"].ParentRelations.Add(relation_pago);


			//DATOS TABLA DATOS DE LOS DEVENGADOS DEL DOCUMENTO
			DataTable Devengados = new DataTable("Devengados");
			Devengados.TableName = "Devengados";
			foreach (PropertyInfo info in typeof(Devengados).GetProperties())
			{
				if (!info.PropertyType.Name.Equals("List`1"))
				{
					Devengados.Columns.Add(info.Name.ToString());
				}
			}

			ds.Tables.Add(Devengados);

			DataColumn datos_devengados = ds.Tables["Devengados"].Columns["DiasTrabajados"];
			DataRelation relation_devengados = new DataRelation("Devengados", principal, datos_devengados);
			relation_devengados.Nested = true;
			ds.Tables["Devengados"].ParentRelations.Add(relation_devengados);

			//DATOS TABLA DATOS DE LAS DEDUCCIONES DEL DOCUMENTO
			DataTable Deducciones = new DataTable("Deducciones");
			Deducciones.TableName = "Deducciones";
			foreach (PropertyInfo info in typeof(Deducciones).GetProperties())
			{
				if (!info.PropertyType.Name.Equals("List`1"))
				{
					Deducciones.Columns.Add(info.Name.ToString());
				}
			}

			ds.Tables.Add(Deducciones);

			DataColumn datos_deducciones = ds.Tables["Deducciones"].Columns["Salud"];
			DataRelation relation_deducciones = new DataRelation("Deducciones", principal, datos_deducciones);
			relation_deducciones.Nested = true;
			ds.Tables["Deducciones"].ParentRelations.Add(relation_deducciones);

			//DATOS TABLA DATOS DEL TRANSPORTE DEL DOCUMENTO NOMINA
			DataTable Transporte = new DataTable("Transporte");
			Transporte.TableName = "Transporte";
			foreach (PropertyInfo info in typeof(Transporte).GetProperties())
			{
				Transporte.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(Transporte);

			DataColumn datos_transporte = ds.Tables["Transporte"].Columns["AuxilioTransporte"];
			DataRelation relation_transporte = new DataRelation("Transporte", principal, datos_transporte);
			relation_transporte.Nested = true;
			ds.Tables["Transporte"].ParentRelations.Add(relation_transporte);

			//DATOS TABLA DATOS DE HORA EXTRAS O RECARGOS DEL DOCUMENTO NOMINA
			DataTable Hora = new DataTable("Hora");
			Hora.TableName = "DatosHoras";
			foreach (PropertyInfo info in typeof(Hora).GetProperties())
			{
				Hora.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(Hora);

			DataColumn datos_hora = ds.Tables["DatosHoras"].Columns["TipoHora"];
			DataRelation relation_hora = new DataRelation("DatosHoras", principal, datos_hora);
			relation_hora.Nested = true;
			ds.Tables["DatosHoras"].ParentRelations.Add(relation_hora);
			*/

			return ds;

		}

	}
}