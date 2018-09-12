using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetZonaPagos;
using HGInetZonaPagos.Clases;
using HGInetZonaPagos.Objetos;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using LibreriaGlobalHGInet.General;
using LibreriaGlobalHGInet.Objetos;

namespace HGInetMiFacturaElectonicaController.PagosElectronicos
{
    public class Ctl_PagosElectronicos : BaseObject<TblPagosElectronicos>
    {

        #region Crear 

        /// <summary>
        /// almacena los datos del pago electrónico en base de datos.
        /// </summary>
        /// <param name="datos_pago"></param>
        /// <returns></returns>
        public TblPagosElectronicos Crear(TblPagosElectronicos datos_pago)
        {
            try
            {
                datos_pago = this.Add(datos_pago);

                return datos_pago;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Almacena los pagos en la base de datos, construyendo el registro con los datos de la pasarela
        /// </summary>
        /// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
        /// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
        /// <param name="id_seguridad">ID de seguridad del documento o el plan a cancelar.</param>
        /// <param name="tipo_pago">indica si el pago es para un documento o una compra de planes (0: Documento - 1: Plan).</param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public TblPagosElectronicos CrearPago(Guid StrIdRegistro, System.Guid id_seguridad, int tipo_pago, decimal valor)
        {
            try
            {
                TblPagosElectronicos datos_registro = new TblPagosElectronicos();
                /*
                if (string.IsNullOrWhiteSpace(id_plataforma))
                    throw new ApplicationException("El ID de seguridad de la plataforma no puede estar vacío.");
                if (id_seguridad == null)
                    throw new ApplicationException("El ID de seguridad no puede estar vacío.");
                    */
                if (valor <= 0)
                    throw new ApplicationException("El valor del pago no puede ");

                datos_registro.StrIdRegistro = StrIdRegistro;

                //Valida que tipo de pago se realiza (0: Documento - 1: Planes)
                if (tipo_pago == 0)
                    datos_registro.StrIdSeguridadDoc = id_seguridad;
                else if (tipo_pago == 1)
                    datos_registro.StrIdSeguridadPlanes = id_seguridad;

                //Fecha de registro.
                datos_registro.DatFechaRegistro = Fecha.GetFecha();
                //valor del documento.
                datos_registro.IntValorPago = valor;

                datos_registro.IntEstadoPago = 888;

                Crear(datos_registro);

                return datos_registro;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        #endregion

        #region Actualizar

        /// <summary>
        /// Actualiza los datos del pago electrónico en base de datos
        /// </summary>
        /// <param name="datos_pago"></param>
        /// <returns></returns>
        public TblPagosElectronicos Actualizar(TblPagosElectronicos datos_pago)
        {
            try
            {
                datos_pago = this.Edit(datos_pago);

                return datos_pago;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Actualiza los datos del pago según la respuesta
        /// </summary>
        /// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
        /// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
        /// <param name="datos_pago">datos de la verificación del pago.</param>
        /// <returns></returns>
        public TblPagosElectronicos ActualizarPago(Guid StrIdSeguridadDoc, Guid StrIdSeguridadRegistro, VerificaPago datos_pago)
        {
            try
            {
                TblPagosElectronicos datos_registro = new TblPagosElectronicos();

                datos_registro = Obtener(StrIdSeguridadDoc, StrIdSeguridadRegistro);

                if (datos_registro == null)
                    throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el pago", datos_pago.detalle_pago.FirstOrDefault().id_pago));

                if (datos_pago != null)
                {
                    //valida si el objeto contiene detalles.
                    if (datos_pago.detalle_pago.Count > 0)
                    {
                        if (datos_pago.detalle_pago.FirstOrDefault().estado_pago != 888)
                        {
                            datos_registro.StrCodigoBanco = datos_pago.detalle_pago.FirstOrDefault().codigo_banco.ToString();
                            datos_registro.StrCodigoFranquicia = datos_pago.detalle_pago.FirstOrDefault().franquicia_codigo;
                            datos_registro.IntClicloTransaccion = datos_pago.detalle_pago.FirstOrDefault().ciclo_transaccion;
                            datos_registro.IntCodigoServicio = datos_pago.detalle_pago.FirstOrDefault().codigo_servicio;
                            datos_registro.IntEstadoPago = datos_pago.detalle_pago.FirstOrDefault().estado_pago;
                            datos_registro.IntFormaPago = datos_pago.detalle_pago.FirstOrDefault().forma_pago_codigo;
                            datos_registro.StrTicketID = datos_pago.detalle_pago.FirstOrDefault().ticketID;
                            datos_registro.StrTransaccionCUS = datos_pago.detalle_pago.FirstOrDefault().codigo_transaccion;
                            datos_registro.StrIdSeguridadPago = datos_pago.detalle_pago.FirstOrDefault().id_pago;
                        }
                        else
                        {
                            datos_registro.IntEstadoPago = datos_pago.detalle_pago.FirstOrDefault().estado_pago;
                            datos_registro.StrIdSeguridadPago = datos_pago.detalle_pago.FirstOrDefault().id_pago;
                        }
                    }
                    else
                    {
                        datos_registro.IntEstadoPago = 0;
                    }

                    if (datos_pago.estado_verificacion)
                        datos_registro.DatFechaVerificacion = Fecha.GetFecha();

                    datos_registro.StrMensaje = datos_pago.mensaje;


                    datos_registro = Actualizar(datos_registro);
                }
                return datos_registro;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        #endregion

        #region Obtener

        /// <summary>
        /// Obtiene el pago de la base de datos por códigos principales (StrIdSeguridadPago y StrIdPlataforma).
        /// </summary>
        /// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
        /// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
        /// <returns></returns>
        public TblPagosElectronicos Obtener(Guid id_seguridad_doc, Guid id_Seguridad_Registro)
        {
            try
            {

                TblPagosElectronicos datos_pago = (from pago in context.TblPagosElectronicos
                                                    where pago.StrIdSeguridadDoc == id_seguridad_doc
                                                    && pago.StrIdRegistro == id_Seguridad_Registro
                                                    select pago).FirstOrDefault();

                return datos_pago;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        /// <summary>
        /// Obtiene el pago de la base de datos por códigos principales (StrIdSeguridadPago y StrIdPlataforma).
        /// </summary>
        /// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
        /// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
        /// <returns></returns>
        public List<TblDocumentos> Obtener(System.Guid StrIdSeguridadDoc)
        {
            try
            {               

                List<TblDocumentos> datos_pago = (from documentos in context.TblDocumentos
                                                  where documentos.StrIdSeguridad == StrIdSeguridadDoc
                                                  select documentos).ToList();
             
                return datos_pago;                

            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }


        /// <summary>
        /// Obtiene el Saldo Pendiente de un Documento
        /// </summary>
        /// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>        
        /// <returns></returns>
        public String ConsultaSaldoDocumento(System.Guid StrIdSeguridadDoc)
        {
            try
            {
                TblDocumentos Valor_Documento = (from Doc in context.TblDocumentos
                                                 where Doc.StrIdSeguridad == StrIdSeguridadDoc
                                                 select Doc).FirstOrDefault();


                if (Valor_Documento.IntIdEstado == ProcesoEstado.FinalizacionErrorDian.GetHashCode())
                {
                    return "ErrorDian";
                }


                int Pago_pendiente = (from pagos in context.TblPagosElectronicos
                                      where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
                                      && (pagos.IntEstadoPago == 999 || pagos.IntEstadoPago == 888)
                                      select pagos.IntValorPago).Count();
                if (Pago_pendiente > 0)
                {
                    return "PagoPendiente";
                }

                var Pagos = (from pagos in context.TblPagosElectronicos
                             where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
                             && pagos.IntEstadoPago == 1
                             select pagos.IntValorPago).FirstOrDefault();

                decimal Datos_pago = 0;
                if (Pagos > 0)
                {
                    Datos_pago = (from pagos in context.TblPagosElectronicos
                                  where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
                                   && pagos.IntEstadoPago == 1
                                  select pagos.IntValorPago).Sum();
                }


                if (Datos_pago >= Valor_Documento.IntVlrTotal)
                {
                    return "DocumentoCancelado";
                }

                return (Valor_Documento.IntVlrTotal - Datos_pago).ToString();
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }




        /// <summary>
        /// Retorna uno de los siguientes valores: 
        /// 1 si el documento esta totalmente pagado
        /// 2 si tiene un pago pendiente por verificar 
        /// 3 No pagado totalmente el documento(Es decir que no tiene ningun pago sin verificar, pero si tiene aun saldo pendiente en la factura)
        /// </summary>
        /// <param name="id_seguridad_pago">ID de seguridad del documento.</param>        
        /// <returns></returns>
        public int VerificarSaldo(System.Guid StrIdSeguridadDoc)
        {
            try
            {
                //Obtengo el documento
                TblDocumentos Valor_Documento = (from Doc in context.TblDocumentos
                                                 where Doc.StrIdSeguridad == StrIdSeguridadDoc
                                                 select Doc).FirstOrDefault();
                

                //Luego valido si tiene algún pago pendiente (estatus 888,999)
                int Pago_pendiente = (from pagos in context.TblPagosElectronicos
                                      where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
                                      && (pagos.IntEstadoPago == 999 || pagos.IntEstadoPago == 888)
                                      select pagos.IntValorPago).Count();

                if (Pago_pendiente > 0)
                {
                    return 2;
                }

                //Luego Valido si tiene pagos
                var Pagos = (from pagos in context.TblPagosElectronicos
                             where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
                             && pagos.IntEstadoPago == 1
                             select pagos.IntValorPago).FirstOrDefault();

                decimal Datos_pago = 0;
                if (Pagos > 0)
                {
                    Datos_pago = (from pagos in context.TblPagosElectronicos
                                  where pagos.StrIdSeguridadDoc == StrIdSeguridadDoc
                                   && pagos.IntEstadoPago == 1
                                  select pagos.IntValorPago).Sum();
                }

                //Pago totalmente cancelado
                if (Datos_pago >= Valor_Documento.IntVlrTotal)
                {
                    return 1;
                }
                //no ha pagado totalmente el documento
                return 3;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }









        /// <summary>
        /// Obtiene la lista de pagos del Facturador
        /// </summary>
        /// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
        /// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
        /// <returns></returns>
        public List<TblPagosElectronicos> ObtenerPagosFacturador(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo, string Resolucion)
        {

            fecha_inicio = fecha_inicio.Date;

            fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

            long num_doc = -1;
            long.TryParse(numero_documento, out num_doc);


            short cod_estado_recibo = -1;
            short.TryParse(estado_recibo, out cod_estado_recibo);

            if (string.IsNullOrWhiteSpace(numero_documento))
                numero_documento = "*";
            if (string.IsNullOrWhiteSpace(codigo_adquiriente))
                codigo_adquiriente = "*";

            if (string.IsNullOrWhiteSpace(estado_recibo))
                estado_recibo = "*";

            List<string> LstResolucion = Coleccion.ConvertirLista(Resolucion);



            var documentos = (from Pagos in context.TblPagosElectronicos
                              where Pagos.TblDocumentos.StrEmpresaFacturador.Equals(codigo_facturador)
                              && (Pagos.TblDocumentos.DatFechaDocumento >= fecha_inicio && Pagos.TblDocumentos.DatFechaDocumento <= fecha_fin)
                              && (LstResolucion.Contains(Pagos.TblDocumentos.StrNumResolucion.ToString()) || Resolucion.Equals("*"))
                              && (Pagos.IntEstadoPago.Equals(cod_estado_recibo) || estado_recibo.Equals("*"))
                              && (Pagos.TblDocumentos.IntNumero == num_doc || numero_documento.Equals("*"))
                              && (Pagos.TblDocumentos.StrEmpresaAdquiriente.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
                              orderby Pagos.TblDocumentos.StrEmpresaAdquiriente ascending, Pagos.TblDocumentos.IntNumero ascending
                              select Pagos).ToList();


            return documentos;

        }


        /// <summary>
        /// Obtiene la lista de pagos del Adquiriente
        /// </summary>
        /// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
        /// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
        /// <returns></returns>
        public List<TblPagosElectronicos> ObtenerPagosAdquiriente(string codigo_facturador, string numero_documento, string codigo_adquiriente, DateTime fecha_inicio, DateTime fecha_fin, string estado_recibo)
        {

            fecha_inicio = fecha_inicio.Date;

            fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

            int num_doc = -1;
            int.TryParse(numero_documento, out num_doc);

            short cod_estado_recibo = -1;
            short.TryParse(estado_recibo, out cod_estado_recibo);

            if (string.IsNullOrWhiteSpace(numero_documento))
                numero_documento = "*";
            if (string.IsNullOrWhiteSpace(codigo_adquiriente))
                codigo_adquiriente = "*";

            if (string.IsNullOrWhiteSpace(estado_recibo))
                estado_recibo = "*";


            var documentos = (from Pagos in context.TblPagosElectronicos
                              where Pagos.TblDocumentos.StrEmpresaAdquiriente.Equals(codigo_adquiriente)
                              && Pagos.TblDocumentos.StrEmpresaFacturador.Equals(codigo_facturador) || codigo_facturador.Equals("*")
                              && (Pagos.TblDocumentos.DatFechaDocumento >= fecha_inicio && Pagos.TblDocumentos.DatFechaDocumento <= fecha_fin)
                              && (Pagos.TblDocumentos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
                              && (Pagos.TblDocumentos.IntNumero == num_doc || numero_documento.Equals("*"))
                              orderby Pagos.TblDocumentos.StrEmpresaAdquiriente ascending, Pagos.TblDocumentos.IntNumero ascending
                              select Pagos).ToList();


            return documentos;

        }





        /// <summary>
        /// obtiene los pagos por estado de verificación (consulta por DatFechaVerificacion, si esta null no se encuentra verificado). ¿FECHAS?
        /// </summary>
        /// <param name="fecha_inicio">fecha inicial del rango para el filtro fechas (aplica sobre la fecha de registro del pago).</param>
        /// <param name="fecha_fin">fecha final del rango para el filtro por fechas (aplica sobre la fecha de registro del pago).</param>
        /// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
        /// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
        /// <param name="estado_verificacion">(0:por verificar - 1: verificado)</param>
        /// <returns></returns>
        public List<TblPagosElectronicos> ObtenerPorVerificacion(DateTime fecha_inicio, DateTime fecha_fin, string id_seguridad_pago, string id_plataforma, int estado_verificacion)
        {
            try
            {
                List<TblPagosElectronicos> pagos = (from pago in context.TblPagosElectronicos

                                                    where (pago.DatFechaRegistro.Date >= fecha_inicio && pago.DatFechaRegistro.Date <= fecha_fin)
                                                    && pago.StrIdSeguridadPago.Equals(id_seguridad_pago) || id_seguridad_pago.Equals("*")
                                                    && pago.StrIdPlataforma.Equals(id_plataforma) || id_plataforma.Equals("*")
                                                    && (estado_verificacion == 0) ? pago.DatFechaVerificacion.Value == null : pago.DatFechaVerificacion.Value != null
                                                    select pago).ToList();

                return pagos;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        #endregion

        #region GestionPagos
        /*
        /// <summary>
        /// Realiza el registro del pago en la plataforma electrónica.
        /// Consulta el documento por id de seguridad para la construcción del objeto de envío de los datos del pago.
        /// Retorna la url con el id virtual para la redirección a la página de inicio del proceso de pago.
        /// Si el pago es de un documento los datos de la pasarela son tomados desde el comercio asociado en la resolución del mismo, de lo contrario lso datos serán obtenidos del web.config
        /// </summary>
        /// <param name="id_seguridad">Id de seguridad del documento o plan transaccional</param>
        /// <param name="tipo_pago">indica si el pago es para un documento o una compra de planes (0: Documento - 1: Plan).</param>
        /// <param name="registrar_pago">indica si se registra el pago en base de datos.</param>
        /// <param name="valor_pago">valor a pagar.</param>
        /// <returns></returns>
        public string ReportePagoElectronico(System.Guid id_seguridad, int tipo_pago = 0, bool registrar_pago = true, double valor_pago = 0)
        {
            try
            {
                //Ruta retornada por el servicio, redirecciona a la  página de inicio del pago (Selección de tipo persona, forma pago y banco).
                string ruta_inicio = string.Empty;
                //Datos de la pasarela electrónica.
                int comercio_id = 0;
                string comercio_clave = string.Empty;
                string comercio_ruta = string.Empty;
                string codigo_servicio = string.Empty;
                string identificacion_empresa = string.Empty;

                //Objetos para reportar el pago
                Cliente datos_cliente = new Cliente();
                Pago datos_pago = new Pago();


                datos_pago.id_pago = Texto.DatosAleatorios(9, 2);

                //Valida si el pago es de un documento o una compra de planes.
                if (tipo_pago == 0)
                {
                    Ctl_Documento clase_documento = new Ctl_Documento();
                    TblDocumentos datos_documento = clase_documento.ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

                    identificacion_empresa = datos_documento.StrEmpresaAdquiriente;

                    //valisa que el documento no sea null.
                    if (datos_documento != null)
                    {
                        //consulta la resolución para obtener el comercio que tiene asociado.
                        Ctl_EmpresaResolucion clase_resoluciones = new Ctl_EmpresaResolucion();
                        TblEmpresasResoluciones datos_resolucion = clase_resoluciones.Obtener(datos_documento.StrEmpresaFacturador, datos_documento.StrNumResolucion, datos_documento.StrPrefijo);

                        //Valida que la resolución no sea null.
                        if (datos_resolucion != null)
                        {
                            if (datos_resolucion.IntComercioId.Value <= 0)
                                throw new ApplicationException(string.Format("La Resuloción N.{0} para el Facturador {1}, no tiene configurado un comercio.", datos_documento.StrNumResolucion, datos_documento.StrEmpresaFacturador));

                            //Obtiene los datos de la pasarela.
                            Ctl_EmpresasPasarela clase_pasarela = new Ctl_EmpresasPasarela();
                            TblEmpresasPasarela datos_pasarela = clase_pasarela.Obtener(datos_documento.StrEmpresaFacturador, datos_resolucion.IntComercioId.Value);

                            if (datos_pasarela != null)
                            {
                                //Datos de la pasarela electrónica.
                                comercio_id = datos_pasarela.IntComercioId;
                                comercio_clave = datos_pasarela.StrComercioClave;
                                comercio_ruta = datos_pasarela.StrComercioIdRuta;
                                codigo_servicio = datos_pasarela.StrCodigoServicio;
                            }
                        }
                        else
                            throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el número de Resolución", datos_documento.StrNumResolucion));

                        datos_pago.descripcion_pago = string.Format("{0}", datos_pago.id_pago);

                        if (valor_pago <= 0)
                            valor_pago = Convert.ToDouble(datos_documento.IntVlrTotal);
                        else
                        {
                            if (valor_pago > Convert.ToDouble(datos_documento.IntVlrTotal))
                                throw new ApplicationException("El valor a pagar no puede ser superior al valor total del documento.");
                        }
                    }
                    else
                    {
                        PasarelaPagos pasarela = HgiConfiguracion.GetConfiguration().PasarelaPagos;

                        //Datos de la pasarela electrónica.
                        comercio_id = Convert.ToInt32(pasarela.IdComercio);
                        comercio_clave = pasarela.ClaveComercio;
                        comercio_ruta = pasarela.RutaComercio;
                        codigo_servicio = pasarela.CodigoServicio;

                        //Obtiene el plan de transacciones.
                        Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();
                        TblPlanesTransacciones datos_plan = clase_planes.ObtenerIdSeguridad(id_seguridad);

                        identificacion_empresa = datos_plan.StrEmpresaFacturador;

                        if (datos_plan == null)
                            throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el Plan", id_seguridad));

                        datos_pago.descripcion_pago = string.Format("{0}", datos_plan.StrObservaciones);

                        if (valor_pago <= 0)
                            valor_pago = Convert.ToDouble(datos_plan.IntValor);
                        else
                        {
                            if (valor_pago > Convert.ToDouble(datos_plan.IntValor))
                                throw new ApplicationException("El valor a pagar no puede ser superior al valor total de la compra.");
                        }
                    }


                    datos_pago.total_con_iva = valor_pago;
                    datos_pago.valor_iva = 0;
                    datos_pago.codigo_servicio_principal = codigo_servicio;
                    datos_pago.info_opcional1 = "opciona1";
                    datos_pago.info_opcional2 = "opcional2";
                    datos_pago.info_opcional3 = "opcional3";
                    datos_pago.lista_codigos_servicio_multicredito = null;
                    datos_pago.lista_nit_codigos_servicio_multicredito = null;
                    datos_pago.lista_valores_con_iva = null;
                    datos_pago.lista_valores_iva = null;
                    datos_pago.total_codigos_servicio = 0;


                    //Obtiene los datos del cliente.
                    Ctl_Empresa clase_empresa = new Ctl_Empresa();
                    TblEmpresas datos_empresa = clase_empresa.Obtener(identificacion_empresa);

                    if (datos_empresa == null)
                        throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "la empresa", datos_empresa.StrIdentificacion));

                    datos_cliente.tipo_id = datos_empresa.StrTipoIdentificacion.ToString();
                    datos_cliente.id_cliente = datos_empresa.StrIdentificacion;
                    datos_cliente.nombre = datos_empresa.StrRazonSocial;
                    datos_cliente.telefono = datos_empresa.StrTelefono;
                    datos_cliente.email = datos_empresa.StrMail;


                    ProcesoPago clase_proceso_pago = new ProcesoPago(comercio_id, comercio_clave, comercio_ruta);

                    //Reporta el pago en la plataforma de pagos electrónicos.
                    ruta_inicio = clase_proceso_pago.ReportarPago(datos_cliente, datos_pago, true);

                    //Valida que la ruta no sea null.
                    if (registrar_pago && !string.IsNullOrWhiteSpace(ruta_inicio))
                    {
                        string id_plataforma = string.Empty;
                        //obtiene los valores enviados por get de la url
                        Uri uri = new Uri(ruta_inicio);
                        var queryString = HttpUtility.ParseQueryString(uri.Query);
                        Regex isnumber = new Regex("[^0-9]");
                        foreach (var key in queryString.Keys)
                        {
                            if (key.Equals("identificador"))
                            {
                                id_plataforma = queryString[(string)key];
                                //valida que el id de la plataforma sean solo numeros.
                                if (!string.IsNullOrWhiteSpace(id_plataforma))
                                {
                                    if (isnumber.IsMatch(id_plataforma))
                                        throw new ApplicationException(id_plataforma);
                                }
                                else throw new ApplicationException(string.Format("Identificador de pago inválido."));
                            }
                        }
                        TblPagosElectronicos pago = CrearPago(datos_pago.id_pago, id_plataforma, id_seguridad, tipo_pago, Convert.ToDecimal(valor_pago));
                    }
                }
                else
                    throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el documento", id_seguridad));

                return ruta_inicio;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /*
        /// <summary>
        /// Realiza el proceso de verificar y actualizar el pago.
        /// </summary>
        /// <param name="id_seguridad_pago">ID de seguridad del pago (generado aleatoriamente - sólo números).</param>
        /// <param name="id_plataforma">ID generado por la plataforma de pagos.</param>
        /// <param name="identificacion_obligado">identificación del obligado.</param>
        /// <param name="numero_resolucion">número de la resolución con la cual se generó el documento</param>
        /// <param name="tipo_pago">indica si el pago es para un documento o una compra de planes (0: Documento - 1: Plan).</param>
        /// <returns>TblPagosElectronicos</returns>
        public TblPagosElectronicos VerificarPago(string id_seguridad_pago, string id_plataforma, string identificacion_obligado, string numero_resolucion, int tipo_pago)
        {
            try
            {
                //Datos de la pasarela electrónica.
                int comercio_id = 0;
                string comercio_clave = string.Empty;

                TblPagosElectronicos datos_pago = Obtener(id_seguridad_pago, id_plataforma);

                //Valida si el pago es de un documento
                if (tipo_pago == 0 && datos_pago.StrIdSeguridadDoc != null)
                {
                    Ctl_Documento clase_documento = new Ctl_Documento();
                    TblDocumentos datos_documento = clase_documento.ObtenerPorIdSeguridad(datos_pago.StrIdSeguridadDoc.Value).FirstOrDefault();

                    //valida que el documento no sea null.
                    if (datos_documento != null)
                    {
                        //consulta la resolución para obtener el comercio que tiene asociado.
                        Ctl_EmpresaResolucion clase_resoluciones = new Ctl_EmpresaResolucion();
                        TblEmpresasResoluciones datos_resolucion = clase_resoluciones.Obtener(datos_documento.StrEmpresaFacturador, datos_documento.StrNumResolucion, datos_documento.StrPrefijo);

                        //Valida que la resolución no sea null.
                        if (datos_resolucion != null)
                        {
                            if (datos_resolucion.IntComercioId.Value <= 0)
                                throw new ApplicationException(string.Format("La Resuloción N.{0} para el Facturador {1}, no tiene configurado un comercio.", datos_documento.StrNumResolucion, datos_documento.StrEmpresaFacturador));

                            //Obtiene los datos de la pasarela.
                            Ctl_EmpresasPasarela clase_pasarela = new Ctl_EmpresasPasarela();
                            TblEmpresasPasarela datos_pasarela = clase_pasarela.Obtener(datos_documento.StrEmpresaFacturador, datos_resolucion.IntComercioId.Value);

                            if (datos_pasarela != null)
                            {
                                //Datos de la pasarela electrónica.
                                comercio_id = datos_pasarela.IntComercioId;
                                comercio_clave = datos_pasarela.StrComercioClave;
                            }
                        }
                        else
                            throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el número de Resolución", datos_documento.StrNumResolucion));
                    }
                }
                else
                    throw new ApplicationException("El tipo de pago no corresponde con el id de seguridad del registro de pago.");

                //Valida si el pago es un plan.
                if (tipo_pago == 1 && datos_pago.StrIdSeguridadPlanes != null)
                {
                    comercio_id = 2651;
                    comercio_clave = "2651HGI";
                }
                else throw new ApplicationException("El tipo de pago no corresponde con el id de seguridad del registro de pago.");

                //Realiza el proceso de verificación del pago en la plataforma electrónica
                VerificarPago clase_verificar_pago = new VerificarPago(comercio_id, comercio_clave);
                VerificaPago datos_pago_plataforma = clase_verificar_pago.Verificar(id_seguridad_pago);

                TblPagosElectronicos datos_retorno = ActualizarPago(id_seguridad_pago, id_plataforma, datos_pago_plataforma);

                return datos_retorno;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }
        */
        #endregion

        #region Plataforma intermedia



        /// <summary>
        /// Realiza el registro del pago en la plataforma electrónica.
        /// Consulta el documento por id de seguridad para la construcción del objeto de envío de los datos del pago.
        /// Retorna la url con el id virtual para la redirección a la página de inicio del proceso de pago.
        /// Si el pago es de un documento los datos de la pasarela son tomados desde el comercio asociado en la resolución del mismo, de lo contrario lso datos serán obtenidos del web.config
        /// </summary>
        /// <param name="id_seguridad">Id de seguridad del documento o plan transaccional</param>
        /// <param name="tipo_pago">indica si el pago es para un documento o una compra de planes (0: Documento - 1: Plan).</param>
        /// <param name="registrar_pago">indica si se registra el pago en base de datos.</param>
        /// <param name="valor_pago">valor a pagar.</param>
        /// <returns></returns>
        public dynamic ReportePagoElectronicoPI(System.Guid id_seguridad, int tipo_pago = 0, bool registrar_pago = true, double valor_pago = 0)
        {
            try
            {
                //Ruta retornada por el servicio, redirecciona a la  página de inicio del pago (Selección de tipo persona, forma pago y banco).
                string ruta_inicio = string.Empty;
                //Datos de la pasarela electrónica.
                int comercio_id = 0;
                string comercio_clave = string.Empty;
                string comercio_ruta = string.Empty;
                string codigo_servicio = string.Empty;
                string identificacion_empresa = string.Empty;

                //Objetos para reportar el pago
                Cliente datos_cliente = new Cliente();
                Pago datos_pago = new Pago();


                TblPasarelaPagosPI ObjPago = new TblPasarelaPagosPI();

                ObjPago.StrIdSeguridadDoc = id_seguridad;

                //ObjPago.StrIdSeguridadRegistro = Guid.NewGuid();
                ObjPago.StrIdSeguridadRegistro = Guid.NewGuid();

                ObjPago.DatFechaRegistro = Fecha.GetFecha();

                ObjPago.DatFechaSync = Fecha.GetFecha();
                ObjPago.DatFechaVerificacion = Fecha.GetFecha();

                ObjPago.StrAuthIdEmpresa = "";
                ObjPago.IntSincronizacion = true;

                ObjPago.StrAuthToken = "";

                PlataformaData plataforma = HgiConfiguracion.GetConfiguration().PlataformaData;
                ObjPago.StrRutaSync = plataforma.RutaPublica.ToString() + "/Api/SrcActualizaEstado";

                ObjPago.StrRutaDestino = "";

                ObjPago.StrRutaProcedencia = "";

                ObjPago.StrPagoIdPlataforma = "";

                decimal Monto_Pendiente=0;

                //Valida si el pago es de un documento o una compra de planes.
                if (tipo_pago == 0)
                {
                    Ctl_Documento clase_documento = new Ctl_Documento();
                    TblDocumentos datos_documento = clase_documento.ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

                    //Valido si este documento tiene algún pago
                    var Pagos = (from pagos in context.TblPagosElectronicos
                                 where pagos.StrIdSeguridadDoc == datos_documento.StrIdSeguridad
                                 && pagos.IntEstadoPago == 1
                                 select pagos.IntValorPago).FirstOrDefault();

                    //Si tiene algún pago, entonces valido cuanto es el total de todos los pagos
                    if (Pagos > 0)
                    {
                        //Le asigno la suma de los pagos a la siguiente variable
                        Monto_Pendiente = (from pagos in context.TblPagosElectronicos
                                               where pagos.StrIdSeguridadDoc == datos_documento.StrIdSeguridad
                                                && pagos.IntEstadoPago == 1
                                               select pagos.IntValorPago).Sum();

                        //Luego Resto, el total del documento menos la suma de pagos
                        Monto_Pendiente = datos_documento.IntVlrTotal-Monto_Pendiente;

                    }


                    //Enviar Informacion facturador
                    //ObjPago.StrIdentificacionFacturador= datos_documento.TblEmpresasFacturador.StrIdentificacion;
                    //ObjPago.StrRazonSocialFacturador = datos_documento.TblEmpresasFacturador.StrRazonSocial;
                    //ObjPago.StrTelefonoFacturador = datos_documento.TblEmpresasFacturador.StrTelefono;
                    //ObjPago.StrMailFacturador = datos_documento.TblEmpresasFacturador.StrMail;
                    //ObjPago.StrImagenFacturador = datos_documento.TblEmpresasFacturador.StrImagenEmpresa;


                    //Id del erp que envia el pago
                    ObjPago.StrIdDocumento = datos_documento.StrObligadoIdRegistro;

                    identificacion_empresa = datos_documento.StrEmpresaAdquiriente;

                    //valisa que el documento no sea null.
                    if (datos_documento != null)
                    {

                        //consulta la resolución para obtener el comercio que tiene asociado.
                        Ctl_EmpresaResolucion clase_resoluciones = new Ctl_EmpresaResolucion();
                        TblEmpresasResoluciones datos_resolucion = clase_resoluciones.Obtener(datos_documento.StrEmpresaFacturador, datos_documento.StrNumResolucion, datos_documento.StrPrefijo);

                        //Valida que la resolución no sea null.
                        if (datos_resolucion != null)
                        {
                            if (datos_resolucion.IntComercioId.Value <= 0)
                                throw new ApplicationException(string.Format("La Resuloción N.{0} para el Facturador {1}, no tiene configurado un comercio.", datos_documento.StrNumResolucion, datos_documento.StrEmpresaFacturador));

                            //Obtiene los datos de la pasarela.
                            Ctl_EmpresasPasarela clase_pasarela = new Ctl_EmpresasPasarela();
                            TblEmpresasPasarela datos_pasarela = clase_pasarela.Obtener(datos_documento.StrEmpresaFacturador, datos_resolucion.IntComercioId.Value);

                            if (datos_pasarela != null)
                            {
                                ObjPago.IntComercioId = datos_pasarela.IntComercioId;
                                ObjPago.StrComercioClave = datos_pasarela.StrComercioClave;
                                ObjPago.StrComercioIdRuta = datos_pasarela.StrComercioIdRuta;
                                ObjPago.StrCodigoServicio = datos_pasarela.StrCodigoServicio;
                            }
                        }
                        else
                            throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el número de Resolución", datos_documento.StrNumResolucion));

                        datos_pago.descripcion_pago = string.Format("{0}", datos_pago.id_pago);
                        //Si la variable de pago viene sin valor
                        if (valor_pago <= 0)
                            if (Monto_Pendiente > 0) //Pregunto si el pago pendiente este presente
                            {
                                valor_pago = Convert.ToDouble(Monto_Pendiente);//Si esta presente le asigno la variable pendiente al pago
                            }else
                            {
                                valor_pago = Convert.ToInt32(datos_documento.IntVlrTotal);//Si no, entonces busco el monto total del pago
                            }


                            else
                            {
                                if (valor_pago > Convert.ToDouble(datos_documento.IntVlrTotal))
                                    throw new ApplicationException("El valor a pagar no puede ser superior al valor total del documento.");
                            }
                    }
                    else
                    {
                        PasarelaPagos pasarela = HgiConfiguracion.GetConfiguration().PasarelaPagos;

                        //Datos de la pasarela electrónica.
                        ObjPago.IntComercioId = Convert.ToInt32(pasarela.IdComercio);
                        ObjPago.StrComercioClave = pasarela.ClaveComercio;
                        ObjPago.StrComercioIdRuta = pasarela.RutaComercio;
                        ObjPago.StrCodigoServicio = pasarela.CodigoServicio;

                        //Obtiene el plan de transacciones.
                        Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();
                        TblPlanesTransacciones datos_plan = clase_planes.ObtenerIdSeguridad(id_seguridad);

                        identificacion_empresa = datos_plan.StrEmpresaFacturador;

                        if (datos_plan == null)
                            throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el Plan", id_seguridad));

                        datos_pago.descripcion_pago = string.Format("{0}", datos_plan.StrObservaciones);

                        if (valor_pago <= 0)
                            valor_pago = Convert.ToDouble(datos_plan.IntValor);
                        else
                        {
                            if (valor_pago > Convert.ToDouble(datos_plan.IntValor))
                                throw new ApplicationException("El valor a pagar no puede ser superior al valor total de la compra.");
                        }
                    }


               

                    ObjPago.IntValor = decimal.Parse(valor_pago.ToString());
                    ObjPago.IntValorIva = 0;
                    /*
                    datos_pago.codigo_servicio_principal = codigo_servicio;
                   
                    datos_pago.info_opcional1 = "opciona1";
                    datos_pago.info_opcional2 = "opcional2";
                    datos_pago.info_opcional3 = "opcional3";


                    datos_pago.lista_codigos_servicio_multicredito = null;
                    datos_pago.lista_nit_codigos_servicio_multicredito = null;
                    datos_pago.lista_valores_con_iva = null;
                    datos_pago.lista_valores_iva = null;
                    datos_pago.total_codigos_servicio = 0;
                    */

                    //Obtiene los datos del cliente.
                    Ctl_Empresa clase_empresa = new Ctl_Empresa();
                    TblEmpresas datos_empresa = clase_empresa.Obtener(identificacion_empresa);

                    if (datos_empresa == null)
                        throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "la empresa", datos_empresa.StrIdentificacion));


                    ObjPago.StrClienteTipoId = datos_empresa.StrTipoIdentificacion.ToString();

                    ObjPago.StrClienteIdentificacion = datos_empresa.StrIdentificacion;

                    ObjPago.StrClienteNombre = datos_empresa.StrRazonSocial;

                    ObjPago.StrClienteTelefono = datos_empresa.StrTelefono;

                    ObjPago.StrClienteEmail = datos_empresa.StrMail;

                    //Encriptar datos de seguridad secundaria
                    ObjPago.StrAuthIdEmpresa = Encriptar.Encriptar_SHA256(ObjPago.StrIdSeguridadRegistro.ToString() + "-" + ObjPago.IntComercioId);

                    //Registro el Pago Local                    
                    TblPagosElectronicos pago = CrearPago(ObjPago.StrIdSeguridadRegistro, id_seguridad, tipo_pago, Convert.ToDecimal(valor_pago));

                    //convierto el Objeto de pago en Json
                    var ObjetoPago = JsonConvert.SerializeObject(ObjPago);

                    //Retorno el Objeto Json Cifrado 

                    return new { Ruta = EncriptarObjeto(ObjetoPago.ToString()), IdRegistro = ObjPago.StrIdSeguridadRegistro };

                }
                else
                    throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el documento", id_seguridad));

                return ruta_inicio;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Cifra el Objeto de pago Json para enviarlo a la pagina intermedia de pago
        /// </summary>
        /// <param name="_cadenaAencriptar"></param>
        /// <returns></returns>
        private static string EncriptarObjeto(string _cadenaAencriptar)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_cadenaAencriptar);
            return System.Convert.ToBase64String(plainTextBytes);
        }



        /// <summary>
        /// Objeto Tpado de Pago de plataforma intermedia
        /// </summary>
        public class TblPasarelaPagosPI
        {
            public string StrIdSeguridad { get; set; }
            public System.Guid StrIdSeguridadRegistro { get; set; }
            public System.DateTime DatFechaRegistro { get; set; }
            public string StrComercioData { get; set; }
            public int IntComercioId { get; set; }
            public string StrComercioIdRuta { get; set; }
            public string StrCodigoServicio { get; set; }
            public string StrComercioClave { get; set; }
            public string StrPagoIdPlataforma { get; set; }
            public string StrIdDocumento { get; set; }
            public int IntPagoEstado { get; set; }
            public decimal IntValor { get; set; }
            public decimal IntValorIva { get; set; }
            public string StrDescripcionPago { get; set; }
            public string StrCampo1 { get; set; }
            public string StrCampo2 { get; set; }
            public string StrCampo3 { get; set; }
            public string StrClienteTipoId { get; set; }
            public string StrClienteIdentificacion { get; set; }
            public string StrClienteNombre { get; set; }
            public string StrClienteEmail { get; set; }
            public string StrClienteTelefono { get; set; }
            public string StrPagoTicketID { get; set; }
            public string StrPagoTransaccionCUS { get; set; }
            public int IntPagoClicloTransaccion { get; set; }
            public string StrPagoCodBanco { get; set; }
            public int IntPagoCodServicio { get; set; }
            public int IntPagoFormaPago { get; set; }
            public string StrPagoCodFranquicia { get; set; }
            public System.DateTime DatFechaVerificacion { get; set; }
            public string StrMensajeVerificacion { get; set; }
            public int IntIdAplicativo { get; set; }
            public string StrRutaProcedencia { get; set; }
            public string StrRutaDestino { get; set; }
            public string StrRutaSync { get; set; }
            public string StrAuthToken { get; set; }
            public string StrAuthIdEmpresa { get; set; }
            public int IntAuthCompania { get; set; }
            public int IntAuthEmpresa { get; set; }
            public bool IntSincronizacion { get; set; }
            public System.DateTime DatFechaSync { get; set; }
            public string StrIdSeguridadPago { get; set; }
            public string StrIdPlataforma { get; set; }
            public System.Guid StrIdSeguridadDoc { get; set; }
        }
        #endregion


    }
}
