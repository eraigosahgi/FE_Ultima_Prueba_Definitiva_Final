using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HGInetMiFacturaElectonicaData.ModeloServicio;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
	public class FacturaController : ApiController
	{

		Factura[] documentos = new Factura[]
		{
			new Factura { Documento = 1, Fecha = new DateTime(2018,1,1), Nota = "Factura #1", Valor = 120000 },
			new Factura { Documento = 2, Fecha = new DateTime(2018,1,2), Nota = "Factura #2", Valor = 125000 },
			new Factura { Documento = 3, Fecha = new DateTime(2018,1,3), Nota = "Factura #3", Valor = 130000 },
			new Factura { Documento = 4, Fecha = new DateTime(2018,1,4), Nota = "Factura #4", Valor = 135000 },
			new Factura { Documento = 5, Fecha = new DateTime(2018,1,5), Nota = "Factura #5", Valor = 140000 },
		};


		// GET: api/Factura
		public IEnumerable<Factura> Get()
		{
			return documentos;
		}

		// GET: api/Factura/5
		public Factura Get(int id)
		{
			return documentos.Where(_doc => _doc.Documento == id).FirstOrDefault();
		}

		// POST: api/Factura
		public void Post([FromBody]string value)
		{
		}

		// PUT: api/Factura/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE: api/Factura/5
		public void Delete(int id)
		{
		}
	}
}
