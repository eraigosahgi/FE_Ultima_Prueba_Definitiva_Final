using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Registros;
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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

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
        public TblPagosElectronicos CrearPago(string id_seguridad_pago, string id_plataforma, System.Guid id_seguridad, int tipo_pago, decimal valor)
        {
            try
            {
                TblPagosElectronicos datos_registro = new TblPagosElectronicos();

                if (string.IsNullOrWhiteSpace(id_plataforma))
                    throw new ApplicationException("El ID de seguridad de la plataforma no puede estar vacío.");
                if (id_seguridad == null)
                    throw new ApplicationException("El ID de seguridad no puede estar vacío.");
                if (valor <= 0)
                    throw new ApplicationException("El valor del pago no puede ");

                //Id de seguridad generado por la plataforma de pagos en línea.
                datos_registro.StrIdPlataforma = id_plataforma;
                //Id de seguridad alfanumérico de 20 carcateres.
                datos_registro.StrIdSeguridadPago = id_seguridad_pago;

                //Valida que tipo de pago se realiza (0: Documento - 1: Planes)
                if (tipo_pago == 0)
                    datos_registro.StrIdSeguridadDoc = id_seguridad;
                else if (tipo_pago == 1)
                    datos_registro.StrIdSeguridadPlanes = id_seguridad;

                //Fecha de registro.
                datos_registro.DatFechaRegistro = Fecha.GetFecha();
                //valor del documento.
                datos_registro.IntValorPago = valor;

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
        public TblPagosElectronicos ActualizarPago(string id_seguridad_pago, string id_plataforma, VerificaPago datos_pago)
        {
            try
            {
                TblPagosElectronicos datos_registro = new TblPagosElectronicos();

                datos_registro = Obtener(id_seguridad_pago, id_plataforma);

                if (datos_registro == null)
                    throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el pago", datos_pago.detalle_pago.FirstOrDefault().id_pago));

                if (datos_pago != null)
                {
                    //valida si el objeto contiene detalles.
                    if (datos_pago.detalle_pago.Count > 0)
                    {
                        datos_registro.StrCodigoBanco = datos_pago.detalle_pago.FirstOrDefault().codigo_banco.ToString();
                        datos_registro.StrCodigoFranquicia = datos_pago.detalle_pago.FirstOrDefault().franquicia_codigo;
                        datos_registro.IntClicloTransaccion = datos_pago.detalle_pago.FirstOrDefault().ciclo_transaccion;
                        datos_registro.IntCodigoServicio = datos_pago.detalle_pago.FirstOrDefault().codigo_servicio;
                        datos_registro.IntEstadoPago = datos_pago.detalle_pago.FirstOrDefault().estado_pago;
                        datos_registro.IntFormaPago = datos_pago.detalle_pago.FirstOrDefault().forma_pago_codigo;
                        datos_registro.StrTicketID = datos_pago.detalle_pago.FirstOrDefault().ticketID;
                        datos_registro.StrTransaccionCUS = datos_pago.detalle_pago.FirstOrDefault().codigo_transaccion;
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
        public TblPagosElectronicos Obtener(string id_seguridad_pago, string id_plataforma)
        {
            try
            {
                TblPagosElectronicos datos_pago = (from pago in context.TblPagosElectronicos
                                                   where pago.StrIdSeguridadPago.Equals(id_seguridad_pago)
                                                   && pago.StrIdPlataforma.Equals(id_plataforma)
                                                   select pago).FirstOrDefault();

                return datos_pago;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
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

        /// <summary>
        /// Realiza el registro del pago en la plataforma electrónica.
        /// Consulta el documento por id de seguridad para la construcción del objeto de envío de los datos del pago.
        /// Retorna la url con el id virtual para la redirección a la página de inicio del proceso de pago.
        /// Si el pago es de un documento los datos de la pasarela son tomados desde el comercio asociado en la resolución del mismo, de lo contrario lso datos serán obtenidos del web.config
        /// </summary>
        /// <param name="id_seguridad">Id de seguridad del documento o plan transaccional</param>
        /// <param name="tipo_pago">indica si el pago es para un documento o una compra de planes (0: Documento - 1: Plan).</param>
        /// <param name="registrar_pago">indica si se registra el pago en base de datos.</param>
        /// <returns></returns>
        public string ReportePagoElectronico(System.Guid id_seguridad, int tipo_pago = 0, bool registrar_pago = true)
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

                //Objetos para reportar el pago
                Cliente datos_cliente = new Cliente();
                Pago datos_pago = new Pago();

                //Valida si el pago es de un documento o una compra de planes.
                if (tipo_pago == 0)
                {
                    Ctl_Documento clase_documento = new Ctl_Documento();
                    TblDocumentos datos_documento = clase_documento.ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

                    //valisa que el documento no sea null.
                    if (datos_documento != null)
                    {
                        //consulta la resolución para obtener el comercio que tiene asociado.
                        Ctl_EmpresaResolucion clase_resoluciones = new Ctl_EmpresaResolucion();
                        TblEmpresasResoluciones datos_resolucion = clase_resoluciones.Obtener(datos_documento.StrEmpresaFacturador, datos_documento.StrNumResolucion);

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

                        datos_cliente.email = datos_documento.TblEmpresasAdquiriente.StrMail;
                        datos_cliente.id_cliente = datos_documento.StrEmpresaAdquiriente;
                        datos_cliente.nombre = datos_documento.TblEmpresasAdquiriente.StrRazonSocial;
                        //Empresa no tiene telefono.
                        datos_cliente.telefono = "444 45 84";
                        datos_cliente.tipo_id = datos_documento.TblEmpresasAdquiriente.StrTipoIdentificacion.ToString();

                        datos_pago.descripcion_pago = string.Format("{0}", datos_pago.id_pago);
                        datos_pago.total_con_iva = Convert.ToDouble(datos_documento.IntVlrTotal);
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

                    }
                    else
                    {
                        //Datos de la pasarela electrónica.
                        comercio_id = 2651;
                        comercio_clave = "2651HGI";
                        comercio_ruta = "t_pruebasHGI";
                        codigo_servicio = "2701";

                        //Obtiene el plan de transacciones.
                        Ctl_PlanesTransacciones clase_planes = new Ctl_PlanesTransacciones();
                        TblPlanesTransacciones datos_plan = clase_planes.ObtenerIdSeguridad(id_seguridad);

                        if (datos_plan == null)
                            throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el Plan", id_seguridad));

                        //Obtiene los datos del facturador.
                        Ctl_Empresa clase_empresa = new Ctl_Empresa();
                        TblEmpresas datos_empresa = clase_empresa.Obtener(datos_plan.StrEmpresaFacturador);

                        if (datos_plan == null)
                            throw new ApplicationException(string.Format(RecursoMensajes.ObjectNotExistError, "el Facturador", datos_plan.StrEmpresaFacturador));

                        datos_cliente.email = datos_empresa.StrMail;
                        datos_cliente.id_cliente = datos_plan.StrEmpresaFacturador;
                        datos_cliente.nombre = datos_empresa.StrRazonSocial;
                        //Empresa no tiene telefono.
                        datos_cliente.telefono = "444 45 84";
                        datos_cliente.tipo_id = datos_empresa.StrTipoIdentificacion.ToString();

                        datos_pago.descripcion_pago = string.Format("{0}", datos_plan.StrObservaciones);
                        datos_pago.total_con_iva = Convert.ToDouble(datos_plan.IntValor);
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

                    }

                    datos_pago.id_pago = Texto.DatosAleatorios(9, 2);

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
                        TblPagosElectronicos pago = CrearPago(datos_pago.id_pago, id_plataforma, new Guid("f603c53b-6f72-49e8-bea5-ab2f63f08ea7"), 0, 2500.00M);
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
                        TblEmpresasResoluciones datos_resolucion = clase_resoluciones.Obtener(datos_documento.StrEmpresaFacturador, datos_documento.StrNumResolucion);

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

        #endregion
    }
}
