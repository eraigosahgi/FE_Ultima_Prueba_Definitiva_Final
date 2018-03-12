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
             new Factura {
                            CodigoRegistro="20",
                            Documento= 12184,
                            NumeroResolucion="18762006401118",
                            Prefijo=null,
                            Cufe="7958005ee2bf17b05da5fff0beb891eeb532d79a",
                            Fecha= new DateTime(2018,1,30,0,0,0),
                            Nota="Resolución Dian Nro 18762006401118 de 2018-01-10 del 12001 al 12818",
                            Moneda="COP",
                            DatosObligado=new Tercero()
                            {
                                Identificacion=811021438,
                                IdentificacionDv=4,
                                TipoIdentificacion=31,
                                TipoPersona=1,
                                Regimen=2,
                                NombreComercial= "HGI",
                                Departamento= "Antioquia",
                                Ciudad= "Medellin",
                                Direccion="Calle 48 Nro. 77C-06",
                                Telefono="4444584",
                                Mail="info@hgi.com.co",
                                PaginaWeb=null,
                                CodigoPais="CO",
                                RazonSocial="HGI SAS",
                                PrimerApellido=null,
                                SegundoApellido=null,
                                PrimerNombre=null,
                                SegundoNombre=null
                            },
                            DatosAdquiriente = new Tercero()
                            {
                                Identificacion=1036929786,
                                IdentificacionDv=0,
                                TipoIdentificacion=22,
                                TipoPersona=2,
                                Regimen=1,
                                NombreComercial="CACHARRERIA PUERTA DEL ORIENTE",
                                Departamento="Antioquia",
                                Ciudad="GUARNE",
                                Direccion="Parque Empresarial Puerta de Oriente, Bodega No 48",
                                Telefono="3164387856",
                                Mail="estrella2629@hotmail.com",
                                PaginaWeb=null,
                                CodigoPais="CO",
                                RazonSocial="RAMIREZ MONTOYA DANIEL",
                                PrimerApellido= "RAMIREZ",
                                SegundoApellido="MONTOYA ",
                                PrimerNombre= "DANIEL",
                                SegundoNombre= null
                            },
                            Valor=1098019,
                            ValorSubtotal=1098019,
                            ValorDescuento=0,
                            ValorIva=208624,
                            ValorReteIva=0,
                            ValorImpuestoConsumo=0,
                            ValorRetefuente=0,
                            Total=1306643,
                            Neto=1306643,
                            DocumentoDetalles = new List<DocumentoDetalle>()
                            {
                                new DocumentoDetalle {Codigo = 1,
                                                      ProductoCodigo = "0172",
                                                      ProductoNombre = "CONTRATO DE SERVICIO Y ACTUALIZACION HGINET ADMINISTRATIVO",
                                                      ProductoDescripcion = "Desde 2018-03-13 Hasta 2018-12-31",
                                                      Cantidad = 1,
                                                      ValorUnitario = 629274.00M,
                                                      ValorSubtotal = 629274.00M,
                                                      ValorImpuestoConsumo = 0,
                                                      IvaValor = 89062.00M,
                                                      IvaPorcentaje = 0.19M,
                                                      DescuentoValor = 0,
                                                      DescuentoPorcentaje = 0.00M,
                                },
                                new DocumentoDetalle {Codigo = 2,
                                                      ProductoCodigo = "0352",
                                                      ProductoNombre = "ACTUALIZACION VERSION HGINET",
                                                      ProductoDescripcion = "",
                                                      Cantidad = 1,
                                                      ValorUnitario = 468745.00M,
                                                      ValorSubtotal = 468745.00M,
                                                      ValorImpuestoConsumo = 0,
                                                      IvaValor = 119562.00M,
                                                      IvaPorcentaje = 0.19M,
                                                      DescuentoValor = 0,
                                                      DescuentoPorcentaje = 0.00M,
                                },


                            }
            }
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
        public HttpResponseMessage Post(Factura value)
		{

            if (ModelState.IsValid)
            {
                // Los campos enviados en el objeto, cumplen con lo solicitado.

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            //return "post";

        }

		// PUT: api/Factura/5
		public HttpResponseMessage Put(Factura value)
		{
            if (ModelState.IsValid)
            {
                // Los campos enviados en el objeto, cumplen con lo solicitado.

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            //return "put";
        }

		// DELETE: api/Factura/5
		public void Delete(int id)
		{
		}
	}
}
