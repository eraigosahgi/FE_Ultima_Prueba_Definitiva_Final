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
				return GenerarColumnas();
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
						if (info.PropertyType != typeof(Tercero) && !info.PropertyType.Name.Equals("List`1") && info.PropertyType != typeof(ReferenciaAdicional))
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
						if (info.PropertyType != typeof(Tercero) && !info.PropertyType.Name.Equals("List`1") && info.PropertyType != typeof(ReferenciaAdicional))
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
						if (info.PropertyType != typeof(Tercero) && !info.PropertyType.Name.Equals("List`1") && info.PropertyType != typeof(ReferenciaAdicional) && info.PropertyType != typeof(ReferenciaPago))
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
				DocumentoDetalles.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(DocumentoDetalles);

			DataColumn datos_detalles = ds.Tables["DocumentoDetalles"].Columns["Codigo"];
			DataRelation relation_detalles = new DataRelation("DocumentoDetalles", principal, datos_detalles);
			relation_detalles.Nested = true;
			ds.Tables["DocumentoDetalles"].ParentRelations.Add(relation_detalles);


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
			DataTable CamposAdicionales = new DataTable("CamposAdicionales");
			CamposAdicionales.TableName = "CamposAdicionales";
			foreach (PropertyInfo info in typeof(CampoValor).GetProperties())
			{
				CamposAdicionales.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(CamposAdicionales);

			DataColumn datos_CamposAdicionales = ds.Tables["CamposAdicionales"].Columns["Descripcion"];
			DataRelation relation_CamposAdicionales = new DataRelation("CamposAdicionales", principal, datos_CamposAdicionales);
			relation_CamposAdicionales.Nested = true;
			ds.Tables["CamposAdicionales"].ParentRelations.Add(relation_CamposAdicionales);

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

			//DATOS TABLA DATOS DE DESCUENTOS
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

			return ds;
		}

	}
}