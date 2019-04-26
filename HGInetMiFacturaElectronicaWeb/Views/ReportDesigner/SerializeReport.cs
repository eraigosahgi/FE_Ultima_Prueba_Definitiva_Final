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
						if (info.PropertyType != typeof(Tercero) && !info.PropertyType.Name.Equals("List`1"))
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
						if (info.PropertyType != typeof(Tercero) && !info.PropertyType.Name.Equals("List`1"))
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
						if (info.PropertyType != typeof(Tercero) && !info.PropertyType.Name.Equals("List`1"))
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
			foreach (PropertyInfo info in typeof(DocumentoDetalle).GetProperties())
			{
				Cuotas.Columns.Add(info.Name.ToString());
			}

			ds.Tables.Add(Cuotas);

			DataColumn datos_cuotas = ds.Tables["Cuotas"].Columns["Codigo"];
			DataRelation relation_cuotas = new DataRelation("Cuotas", principal, datos_cuotas);
			relation_detalles.Nested = true;
			ds.Tables["Cuotas"].ParentRelations.Add(relation_cuotas);

			return ds;
		}

	}
}