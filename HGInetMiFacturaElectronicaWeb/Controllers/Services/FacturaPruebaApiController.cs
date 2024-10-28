using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Error;
using System.ServiceModel;

namespace HGInetMiFacturaElectronicaWeb.Controllers.Services
{
    public class FacturaPruebaApiController : ApiController
    {

		Factura[] documentos = new Factura[]
        {
            new Factura { Documento = 1, Fecha = new DateTime(2018,1,1), Nota = "Factura #1", Valor = 120000 },
            new Factura { Documento = 2, Fecha = new DateTime(2018,1,2), Nota = "Factura #2", Valor = 125000 },
            new Factura { Documento = 3, Fecha = new DateTime(2018,1,3), Nota = "Factura #3", Valor = 130000 },
            new Factura { Documento = 4, Fecha = new DateTime(2018,1,4), Nota = "Factura #4", Valor = 135000 },
            new Factura { Documento = 5, Fecha = new DateTime(2018,1,5), Nota = "Factura #5", Valor = 140000 },
            new Factura {
                            CodigoRegistro="4d44203b44ccc3fa67f1548dae4e050618a5b263",
                            Documento= 12184,
                            NumeroResolucion="18762006401118",
                            Prefijo=null,
                            //Cufe="7958005ee2bf17b05da5fff0beb891eeb532d79a",
                            Fecha= new DateTime(2018,1,30,0,0,0),
                            Nota="Resolución Dian Nro 18762006401118 de 2018-01-10 del 12001 al 12818",
                            Moneda="COP",
                            DatosObligado=new Tercero()
                            {
                                Identificacion="811021438",
                                IdentificacionDv=4,
                                TipoIdentificacion=31,
                                TipoPersona=1,
                                Regimen=2,
                                NombreComercial= "HGI",
                                Departamento= "Antioquia",
                                Ciudad= "Medellin",
                                Direccion="Calle 48 Nro. 77C-06",
                                Telefono="4444584",
                                Email="info@hgi.com.co",
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
                                Identificacion="1036929786",
                                IdentificacionDv=0,
                                TipoIdentificacion=22,
                                TipoPersona=2,
                                Regimen=1,
                                NombreComercial="CACHARRERIA PUERTA DEL ORIENTE",
                                Departamento="Antioquia",
                                Ciudad="GUARNE",
                                Direccion="Parque Empresarial Puerta de Oriente, Bodega No 48",
                                Telefono="3164387856",
                                Email="estrella2629@hotmail.com",
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
                            ValorReteFuente=0,
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
                        },

            new Factura(){
                            DataKey = "4d44203b44ccc3fa67f1548dae4e050618a5b263",
                            CodigoRegistro="5489679",
                            Documento= 990000331,
                            NumeroResolucion="9000000033394696",
                            Prefijo="",
                            //Cufe="7ead8ae96acc8bb7d0fe6cd078a5fd90005849ea",
                            Fecha= new DateTime(2018,4,7,0,0,0),
                            Nota="Resolución Dian Nro 9000000033394696 de 2017-07-02 del 990000000 al 995000000",
                            Moneda="COP",
                            DatosObligado=new Tercero()
                                                        {
                                                            Identificacion="811021438",
                                                            IdentificacionDv=4,
                                                            TipoIdentificacion=31,
                                                            TipoPersona=1,
                                                            Regimen=2,
                                                            NombreComercial= "HGI",
                                                            Departamento= "Antioquia",
                                                            Ciudad= "Medellin",
                                                            Direccion="Calle 48 Nro. 77C-06",
                                                            Telefono="4444584",
                                                            Email="info@hgi.com.co",
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
                                                            Identificacion="1152708377",
                                                            IdentificacionDv=4,
                                                            TipoIdentificacion=22,
                                                            TipoPersona=2,
                                                            Regimen=1,
                                                            NombreComercial="",
                                                            Departamento="Antioquia",
                                                            Ciudad="Medellin",
                                                            Direccion="Cll 102 B N 76-47",
                                                            Telefono="4723311",
                                                            Email="atamayo@hgi.com.co",
                                                            PaginaWeb=null,
                                                            CodigoPais="CO",
                                                            RazonSocial="Tamayo Rodríguez Ana María",
                                                            PrimerApellido= "Tamayo",
                                                            SegundoApellido="Rodríguez ",
                                                            PrimerNombre= "Ana",
                                                            SegundoNombre= "María"
                                                        },
                                                        Valor=450000.00M,
                                                        ValorSubtotal=450000.00M,
                                                        ValorDescuento=0.00M,
                                                        ValorIva=72000.00M,
                                                        ValorReteIva=0.00M,
                                                        ValorImpuestoConsumo=0.00M,
                                                        ValorReteFuente=0.00M,
                                                        ValorReteIca = 0.00M,
                                                        Total=522000.00M,
                                                        Neto=522000.00M,
                                                        DocumentoDetalles = new List<DocumentoDetalle>()
                                                        {
                                                            new DocumentoDetalle {Codigo = 1,
                                                                                  ProductoCodigo = "10616",
                                                                                  ProductoNombre = "AA cambio pomgur hojuela",
                                                                                  ProductoDescripcion = "",
                                                                                  Cantidad = 1,
                                                                                  ValorUnitario = 450000.00M,
                                                                                  ValorSubtotal = 450000.00M,
                                                                                  ValorImpuestoConsumo = 0.00M,
                                                                                  IvaValor = 72000.00M,
                                                                                  IvaPorcentaje = 0.16M,
                                                                                  ReteIcaPorcentaje = 0.00M,
                                                                                  ReteIcaValor = 0.00M,
                                                                                  DescuentoValor = 0.00M,
                                                                                  DescuentoPorcentaje = 0.00M,
                                                                                  ReteFuentePorcentaje = 0.00M,
                                                                                  ReteFuenteValor = 0.00M,
                                                            },



                                                        }
                                        }
        };


        // GET: api/Factura
        public IEnumerable<Factura> Get()
        {
            return documentos;
        }

        // GET: api/Factura/990000330
        public DocumentoRespuesta Get(int id)
        {
            try
            {   // genera un id único de la plataforma
                Guid id_peticion = Guid.NewGuid();

                // información del documento de pruebas solicitado
                Factura documento_obj = documentos[6];


                /***
				 * MODIFICA LOS DATOS DEL DOCUMENTO PARA EL PROCESAMIENTO
				 ***/
                documento_obj.Documento = id;
                documento_obj.Fecha = LibreriaGlobalHGInet.Funciones.Fecha.GetFecha();

				// procesa el documento
				DocumentoRespuesta resultado = null; // Ctl_Documentos.Procesar(id_peticion, documento_obj, TipoDocumento.Factura, null, null);


                return resultado;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        // POST: api/Factura
        [HttpPost]
        public HttpResponseMessage Post(Factura value)
        {
            try
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
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }

        }

        // PUT: api/Factura/5
        [HttpPut]
        public HttpResponseMessage Put(Factura value)
        {
            try
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
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        // DELETE: api/Factura/5
        public void Delete(int id)
        {
        }
    }
}
