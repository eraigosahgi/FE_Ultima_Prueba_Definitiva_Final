using HGInetMiFacturaElectonicaController.Configuracion;
using HGInetMiFacturaElectonicaController.Procesos;
using HGInetMiFacturaElectonicaData;
using HGInetMiFacturaElectonicaData.ControllerSql;
using HGInetMiFacturaElectonicaData.Modelo;
using HGInetMiFacturaElectonicaData.ModeloServicio;
using LibreriaGlobalHGInet.Error;
using LibreriaGlobalHGInet.Formato;
using LibreriaGlobalHGInet.Funciones;
using LibreriaGlobalHGInet.Objetos;
using LibreriaGlobalHGInet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HGInetMiFacturaElectonicaController.Registros
{
	public class Ctl_Documento : BaseObject<TblDocumentos>
	{
		#region Constructores 

		public Ctl_Documento() : base(new ModeloAutenticacion()) { }
		public Ctl_Documento(ModeloAutenticacion autenticacion) : base(autenticacion) { }

		public Ctl_Documento(string servidor, string basedatos, string usuario, string clave) : base(servidor, basedatos, usuario, clave) { }
		#endregion

		/// <summary>
		/// Crea un documento en la Base de Datos
		/// </summary>
		/// <param name="documento"></param>
		/// <returns></returns>
		public TblDocumentos Crear(TblDocumentos documento)
		{
			documento = this.Add(documento);

			return documento;
		}




		public TblDocumentos Actualizar(TblDocumentos documento)
		{
			documento = this.Edit(documento);

			return documento;

		}


		#region Obtener

		public TblDocumentos Obtener(string numero_resolucion, int numero_documeto)
		{
			try
			{

				TblDocumentos documento = (from documentos in context.TblDocumentos
										   where (documentos.IntNumero == numero_documeto)
										   && (documentos.TblEmpresasResoluciones.StrNumResolucion.Equals(numero_resolucion))
										   select documentos).FirstOrDefault();

				return documento;


			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}


		/// <summary>
		/// Obtiene los documentos por número
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>
		/// <param name="TipoDocumento">tipo documento 1: factura - 2: nota crédito - 3: nota débito</param>
		/// <param name="Numeros">número de los documentos (recibe varios números separados por coma)</param>
		/// <returns></returns>
		public List<DocumentoRespuesta> ConsultaPorNumeros(string identificacion_obligado, int tipo_documento, string Numeros)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");
				if (tipo_documento < 1 || tipo_documento > 3)
					throw new ApplicationException("Tipo de documento inválido.");
				if (string.IsNullOrWhiteSpace(Numeros))
					throw new ApplicationException("Filtro por números inválido.");

				List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

				//Convierte Numeros en una lista.
				List<string> lista_documentos = Coleccion.ConvertirLista(Numeros);

				var respuesta = from documento in context.TblDocumentos
								join empresa in context.TblEmpresas on documento.StrEmpresaFacturador equals empresa.StrIdentificacion
								where empresa.StrIdentificacion.Equals(identificacion_obligado)
								 && documento.IntDocTipo == tipo_documento
								 && lista_documentos.Contains(documento.IntNumero.ToString())
								select documento;

				//Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
				foreach (TblDocumentos item in respuesta)
				{


					List<System.Guid> List_id_seguridad = new List<Guid>();

					List_id_seguridad.Add(item.StrIdSeguridad);

					Ctl_Documento documento = new Ctl_Documento();

					var lista = documento.ProcesarDocumentos(List_id_seguridad);

					lista_respuesta.Add(Convertir(item));
				}

				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

		/// <summary>
		/// Obtiene los documentos por Código de Registros
		/// </summary>
		/// <param name="DataKey">Clave compuesta (serial + identificación obligado ) en formato Sha1</param>
		/// <param name="Identificacion">identificación obligado</param>
		/// <param name="TipoDocumento">tipo documento 1: factura - 2: nota crédito - 3: nota débito</param>
		/// <param name="CodigosRegistros">código de registro de los documentos (recibe varios códigos separados por coma)</param>
		/// <returns></returns>
		public List<DocumentoRespuesta> ConsultaPorCodigoRegistro(string identificacion_obligado, int tipo_documento, string CodigosRegistros)
		{
			try
			{
				//Valida que los parametros sean correctos.
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");
				if (tipo_documento < 1 || tipo_documento > 3)
					throw new ApplicationException("Tipo de documento inválido.");
				if (string.IsNullOrWhiteSpace(CodigosRegistros))
					throw new ApplicationException("Filtro por números inválido.");

				List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

				//Convierte CodigoRegistros en una lista.
				List<string> lista_documentos = Coleccion.ConvertirLista(CodigosRegistros);

				var respuesta = from documento in context.TblDocumentos
								join empresa in context.TblEmpresas on documento.StrEmpresaFacturador equals empresa.StrIdentificacion
								where empresa.StrIdentificacion.Equals(identificacion_obligado)
								&& documento.IntDocTipo == tipo_documento
								&& (lista_documentos.Contains(documento.StrObligadoIdRegistro) || lista_documentos.Contains(documento.StrIdSeguridad.ToString()))
								select documento;

				//Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
				foreach (TblDocumentos item in respuesta)
				{
					lista_respuesta.Add(Convertir(item));
				}

				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

		/// <summary>
		/// Obtiene los documentos por rangos de fecha de elaboración.
		/// </summary>
		/// <param name="identificacion_obligado">identificación obligado</param>
		/// <param name="tipo_documento">tipo documento 1: factura - 2: nota crédito - 3: nota débito</param>
		/// <param name="FechaInicial">fecha inicial del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <param name="FechaFinal">fecha final del rango de búsqueda - aplica sobre la fecha del registro</param>
		/// <returns></returns>
		public List<DocumentoRespuesta> ConsultaPorFechaElaboracion(string identificacion_obligado, int tipo_documento, DateTime FechaInicial, DateTime FechaFinal)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(identificacion_obligado))
					throw new ApplicationException("Número de identificación del obligado inválido.");
				if (tipo_documento < 1 || tipo_documento > 3)
					throw new ApplicationException("Tipo de documento inválido.");
				if (FechaInicial == null)
					throw new ApplicationException("Fecha inicial inválida.");
				if (FechaFinal == null)
					throw new ApplicationException("Fecha final inválida.");

                FechaInicial = FechaInicial.Date;                
                FechaFinal = new DateTime(FechaFinal.Year, FechaFinal.Month, FechaFinal.Day, 23, 59, 59, 999);

				List<DocumentoRespuesta> lista_respuesta = new List<DocumentoRespuesta>();

				var respuesta = from documento in context.TblDocumentos
								join empresa in context.TblEmpresas on documento.StrEmpresaFacturador equals empresa.StrIdentificacion
								where empresa.StrIdentificacion.Equals(identificacion_obligado)
								 && documento.IntDocTipo == tipo_documento
								 && (documento.DatFechaIngreso >= FechaInicial && documento.DatFechaIngreso <= FechaFinal)
								select documento;

				//Convierte los registros de base de datos a objeto de servicio y los añade a la lista de retorno
				foreach (TblDocumentos item in respuesta)
				{
					lista_respuesta.Add(Convertir(item));
				}

				return lista_respuesta;
			}
			catch (Exception exec)
			{
				Error error = new Error(CodigoError.VALIDACION, exec);
				throw new FaultException<Error>(error, new FaultReason(string.Format("{0}", error.Mensaje)));
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo_adquiente"></param>
        /// <param name="numero_documento"></param>
        /// <param name="estado_recibo"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <returns></returns>
        public List<TblDocumentos> ObtenerPorFechasAdquiriente(string codigo_adquiente, string numero_documento, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {
            //---------------Valida los estos validos para esta vista
            var estado_dian = "";
            Ctl_MaestrosEnum Mastros = new Ctl_MaestrosEnum();

            List<string[]> Lista_recibos = Mastros.ListaEnum(0, "publico");
            estado_dian = Coleccion.ConvertirString(Lista_recibos);
            //---------------

            fecha_inicio = fecha_inicio.Date;
            fecha_fin = fecha_fin.Date;

			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

            int num_doc = -1;
            int.TryParse(numero_documento, out num_doc);

            short cod_estado_recibo = -1;
            short.TryParse(estado_recibo, out cod_estado_recibo);
            
            if (string.IsNullOrWhiteSpace(numero_documento))
                numero_documento = "*";
            if (string.IsNullOrWhiteSpace(estado_recibo))
                estado_recibo = "*";

            var respuesta = (from datos in context.TblDocumentos
                             join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
                             where (empresa.StrIdentificacion.Equals(codigo_adquiente) || codigo_adquiente.Equals("*"))
                                           && (datos.IntNumero == num_doc || numero_documento.Equals("*"))
                                           && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
                                           && (datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin)
                                           && (estado_dian.Contains(datos.IntIdEstado.ToString()))
                             orderby datos.IntNumero descending
                             select datos).ToList();

			return respuesta;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo_facturador"></param>
        /// <param name="numero_documento"></param>
        /// <param name="codigo_tercero"></param>
        /// <param name="estado_dian"></param>
        /// <param name="estado_recibo"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <returns></returns>
        public List<TblDocumentos> ObtenerPorFechasObligado(string codigo_facturador, string numero_documento, string codigo_adquiriente, string estado_dian, string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {

            if (estado_dian == null || estado_dian=="")
            {
                Ctl_MaestrosEnum Mastros = new Ctl_MaestrosEnum();

                List<string[]> Lista_recibos = Mastros.ListaEnum(0, "publico");                
                estado_dian = Coleccion.ConvertirString(Lista_recibos);                
            }


            fecha_inicio = fecha_inicio.Date;           

			fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);

			int num_doc = -1;
			int.TryParse(numero_documento, out num_doc);

            //short cod_estado_dian = -1;
            //short.TryParse(estado_dian, out cod_estado_dian);

			short cod_estado_recibo = -1;
			short.TryParse(estado_recibo, out cod_estado_recibo);

            if (string.IsNullOrWhiteSpace(numero_documento))
                numero_documento = "*";
            if (string.IsNullOrWhiteSpace(codigo_adquiriente))
                codigo_adquiriente = "*";
            //if (string.IsNullOrWhiteSpace(estado_dian))
            //    estado_dian = "*";
            if (string.IsNullOrWhiteSpace(estado_recibo))
                estado_recibo = "*";

            //if (estado_dian.Equals("3"))
            //    estado_dian = "2,3";

            //List<string> estados = Coleccion.ConvertirLista(estado_dian);


            List<TblDocumentos> documentos = (from datos in context.TblDocumentos
                                              join obligado in context.TblEmpresas on datos.StrEmpresaFacturador equals obligado.StrIdentificacion
                                              join adquiriente in context.TblEmpresas on datos.StrEmpresaAdquiriente equals adquiriente.StrIdentificacion
                                              where (obligado.StrIdentificacion.Equals(codigo_facturador) || codigo_facturador.Equals("*"))
                                            && (datos.IntNumero == num_doc || numero_documento.Equals("*"))
                                            && (adquiriente.StrIdentificacion.Equals(codigo_adquiriente) || codigo_adquiriente.Equals("*"))
                                            && (estado_dian.Contains(datos.IntIdEstado.ToString()) )
                                            && (datos.IntAdquirienteRecibo == cod_estado_recibo || estado_recibo.Equals("*"))
                                            && (datos.DatFechaDocumento >= fecha_inicio && datos.DatFechaDocumento <= fecha_fin)
                                              orderby datos.IntNumero descending
                                              select datos).ToList();

			return documentos;
		}

		/// <summary>
		/// Obtiene los documentos por id se seguridad.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <returns></returns>
		public List<TblDocumentos> ObtenerPorIdSeguridad(System.Guid id_seguridad)
		{

			try
			{
				var respuesta = (from datos in context.TblDocumentos
								 where datos.StrIdSeguridad.Equals(id_seguridad)
								 select datos
								 );

				return respuesta.ToList();
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion


		#region Actualizar

		/// <summary>
		/// Actualiza la respuesta del acuse de recibo.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <param name="estado"></param>
		/// <param name="motivo_rechazo"></param>
		/// <returns></returns>
		public List<TblDocumentos> ActualizarRespuestaAcuse(System.Guid id_seguridad, short estado, string motivo_rechazo)
		{
			try
			{
				List<TblDocumentos> retorno = new List<TblDocumentos>();
				TblDocumentos doc = ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				if (doc == null)
					throw new ArgumentException(string.Format(LibreriaGlobalHGInet.Properties.RecursoMensajes.ObjectNotExistError, "el documento", doc.IntNumero));

				doc.IntAdquirienteRecibo = estado;
				doc.StrAdquirienteMvoRechazo = motivo_rechazo;
				doc.DatAdquirienteFechaRecibo = Fecha.GetFecha();
				doc.IntIdEstado = 9;

				// obtiene los datos del facturador electrónico
				TblEmpresas facturador = ctl_empresa.Obtener(doc.StrEmpresaFacturador);

				// obtiene los datos del adquiriente
				TblEmpresas adquiriente = ctl_empresa.Obtener(doc.StrEmpresaAdquiriente);

				try
				{   // envía el correo del acuse de recibo al facturador electrónico
					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
					email.RespuestaAcuse(doc, facturador, adquiriente);
					doc.IntIdEstado = 99;
				}
				catch (Exception) { }

				doc.DatFechaActualizaEstado = Fecha.GetFecha();
				doc = Actualizar(doc);

				doc.TblEmpresasFacturador = facturador;
				doc.TblEmpresasAdquiriente = adquiriente;
				retorno.Add(doc);

				return retorno;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion


		/// <summary>
		/// Convierte un Objeto de Servicio a un Objeto de Bd
		/// </summary>
		/// <param name="respuesta">Objeto de respuesta</param>
		/// <param name="factura">Objeto enviado por el usuario</param>
		/// <param name="empresa"></param>
		/// <param name="adquiriente"></param>
		/// <param name="tipo_doc"></param>
		/// <returns></returns>
		public static TblDocumentos Convertir(DocumentoRespuesta respuesta, object documento, TblEmpresasResoluciones resolucion, TblEmpresas empresa, TipoDocumento tipo_doc)
		{
			try
			{
				//Valida el tipo de documento enviado por el usuario
				var documento_obj = (dynamic)null;

				if (tipo_doc == TipoDocumento.Factura)
				{
					documento_obj = documento;
				}
				else if (tipo_doc == TipoDocumento.NotaCredito)
				{
					documento_obj = documento;
				}
				else if (tipo_doc == TipoDocumento.NotaDebito)
					documento_obj = documento;

				TblDocumentos tbl_documento = new TblDocumentos();

				tbl_documento.DatFechaIngreso = respuesta.FechaRecepcion;
				tbl_documento.IntDocTipo = Convert.ToInt16(tipo_doc);
				tbl_documento.IntNumero = documento_obj.Documento;

				if (tipo_doc == TipoDocumento.NotaCredito || tipo_doc == TipoDocumento.NotaDebito)
				{
					tbl_documento.StrPrefijo = "";
					tbl_documento.DatFechaVencDocumento = documento_obj.Fecha;
				}
				else
				{
					tbl_documento.StrPrefijo = (!string.IsNullOrEmpty(documento_obj.Prefijo)) ? documento_obj.Prefijo : "";
					tbl_documento.DatFechaVencDocumento = documento_obj.FechaVence;
				}
				tbl_documento.DatFechaDocumento = documento_obj.Fecha;
				tbl_documento.StrObligadoIdRegistro = documento_obj.CodigoRegistro;
				tbl_documento.StrNumResolucion = resolucion.StrNumResolucion;
				tbl_documento.StrEmpresaFacturador = empresa.StrIdentificacion;
				tbl_documento.StrEmpresaAdquiriente = documento_obj.DatosAdquiriente.Identificacion;
				tbl_documento.StrCufe = respuesta.Cufe;
				tbl_documento.IntVlrTotal = documento_obj.Total;
				tbl_documento.StrIdSeguridad = Guid.Parse(respuesta.IdDocumento);
				tbl_documento.IntAdquirienteRecibo = 0;
				tbl_documento.IntIdEstado = Convert.ToInt16(respuesta.IdProceso);
				tbl_documento.DatFechaActualizaEstado = Fecha.GetFecha();

				return tbl_documento;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		/// <summary>
		/// Convierte un objeto de tipo de base de datos a objeto de servicio.
		/// </summary>
		/// <param name="respuesta">Objeto de tipo TblDocumentos</param>
		/// <returns></returns>
		public static DocumentoRespuesta Convertir(TblDocumentos respuesta)
		{
			try
			{
				//Valida el tipo de documento enviado por el usuario

				if (respuesta == null)
					throw new ApplicationException(string.Format(RecursoMensajes.ArgumentNullError, "respuesta", "TblDocumentos"));

				DocumentoRespuesta obj_documento = new DocumentoRespuesta();

				obj_documento.Aceptacion = respuesta.IntAdquirienteRecibo;
				obj_documento.CodigoRegistro = respuesta.StrObligadoIdRegistro;
				obj_documento.Cufe = respuesta.StrCufe;
				obj_documento.Documento = respuesta.IntNumero;
				obj_documento.FechaRecepcion = respuesta.DatFechaIngreso;
				obj_documento.FechaUltimoProceso = respuesta.DatFechaActualizaEstado;
				obj_documento.IdDocumento = respuesta.StrIdSeguridad.ToString();
				obj_documento.Identificacion = respuesta.TblEmpresasAdquiriente.StrIdentificacion;
				obj_documento.IdProceso = respuesta.IntIdEstado;
				obj_documento.MotivoRechazo = respuesta.StrAdquirienteMvoRechazo;
				obj_documento.NumeroResolucion = respuesta.TblEmpresasResoluciones.StrNumResolucion;
				obj_documento.Prefijo = respuesta.TblEmpresasResoluciones.StrPrefijo;
				obj_documento.ProcesoFinalizado = 0;
				obj_documento.UrlPdf = respuesta.StrUrlArchivoPdf;
				obj_documento.UrlXmlUbl = respuesta.StrUrlArchivoUbl;


				// obj_documento.DescripcionProceso

				switch (respuesta.IntIdEstado)
				{
					case 1:
						obj_documento.DescripcionProceso = "Recepción - Información del documento.";
						break;

					case 2:
						obj_documento.DescripcionProceso = "Valida la información del documento.";
						break;

					case 3:
						obj_documento.DescripcionProceso = "Genera información en estandar UBL.";
						break;

					case 4:
						obj_documento.DescripcionProceso = "Almacena el archivo XML con la información en estandar UBL.";
						break;

					case 5:
						obj_documento.DescripcionProceso = "Firma el archivo XML con la información en estandar UBL.";
						break;

					case 6:
						obj_documento.DescripcionProceso = "Compresión del archivo XML firmado con la información en estandar UBL.";
						break;

					case 7:
						obj_documento.DescripcionProceso = "Envío del archivo ZIP con el XML firmado a la DIAN.";
						break;

					case 8:
						obj_documento.DescripcionProceso = "Envío correo adquiriente";
						break;

					case 9:
						obj_documento.DescripcionProceso = "Recepción acuse de recibo del Adquiriente";
						break;

					case 10:
						obj_documento.DescripcionProceso = "Envío correo acuse de recibo al facturador";
						break;


					case 90:
						obj_documento.DescripcionProceso = "Finalizado con inconsistencias.";
						obj_documento.ProcesoFinalizado = 1;
						break;

					case 99:
						obj_documento.DescripcionProceso = "Termina proceso";
						obj_documento.ProcesoFinalizado = 1;
						break;

					default:
						obj_documento.DescripcionProceso = "Proceso desconocido";
						break;
				}

				if (respuesta.IntIdEstado == 1)
					obj_documento.DescripcionProceso = "Recepción - Información del documento.";

				return obj_documento;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}

		}



        #region ProcesarDocumentos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo_adquiente"></param>
        /// <param name="numero_documento"></param>
        /// <param name="estado_recibo"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <returns></returns>
        public List<TblDocumentos> ObtenerDocumentosaProcesar(System.Guid? IdSeguridad,  string estado_recibo, DateTime fecha_inicio, DateTime fecha_fin)
        {

            if (estado_recibo == null || estado_recibo == "")
            {
                Ctl_MaestrosEnum Mastros = new Ctl_MaestrosEnum();

                List<string[]> Lista_recibos = Mastros.ListaEnum(0, "privado");
                estado_recibo = Coleccion.ConvertirString(Lista_recibos);
            }

            fecha_inicio = fecha_inicio.Date;
            fecha_fin = new DateTime(fecha_fin.Year, fecha_fin.Month, fecha_fin.Day, 23, 59, 59, 999);     

            List<string> estados = Coleccion.ConvertirLista(estado_recibo);

            var respuesta = (from datos in context.TblDocumentos
                             join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
                             where  ((datos.StrIdSeguridad==IdSeguridad) || IdSeguridad ==null)
                            && (estado_recibo.Contains(datos.IntIdEstado.ToString()))
                            && (datos.DatFechaIngreso >= fecha_inicio && datos.DatFechaIngreso <= fecha_fin)
                             orderby datos.DatFechaIngreso descending
                             select datos).ToList();

			return respuesta;
		}
		#endregion

		#region Reenviar Acuse
		/// <summary>
		/// Actualiza la respuesta del acuse de recibo.
		/// </summary>
		/// <param name="id_seguridad"></param>
		/// <param name="estado"></param>
		/// <param name="motivo_rechazo"></param>
		/// <returns></returns>
		public bool ReenviarRespuestaAcuse(System.Guid id_seguridad, string mail)
		{
			try
			{
				List<TblDocumentos> retorno = new List<TblDocumentos>();
				TblDocumentos doc = ObtenerPorIdSeguridad(id_seguridad).FirstOrDefault();

				Ctl_Empresa ctl_empresa = new Ctl_Empresa();

				if (doc == null)
					throw new ArgumentException(string.Format(LibreriaGlobalHGInet.Properties.RecursoMensajes.ObjectNotExistError, "el documento", doc.IntNumero));


				// obtiene los datos del facturador electrónico
				TblEmpresas facturador = ctl_empresa.Obtener(doc.StrEmpresaFacturador);

				// obtiene los datos del adquiriente
				TblEmpresas adquiriente = ctl_empresa.Obtener(doc.StrEmpresaAdquiriente);

				try
				{   // envía el correo del acuse de recibo al facturador electrónico
					Ctl_EnvioCorreos email = new Ctl_EnvioCorreos();
					email.RespuestaAcuse(doc, facturador, adquiriente, mail);

				}
				catch (Exception) { }


				return true;
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}

		#endregion

		#region Recibir lista de documentos para procesar
		/// <summary>
		/// procesa Lista de Documentos
		/// </summary>
		/// <param name="List_id_seguridad"></param>        
		/// <returns></returns>
		public List<TblDocumentos> ProcesarDocumentos(List<System.Guid> List_id_seguridad)
		{
			try
			{
				List<TblDocumentos> retorno = (from doc in context.TblDocumentos
											   where List_id_seguridad.Contains(doc.StrIdSeguridad)
											   select doc).ToList();
                

				return retorno;

			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
        #endregion




        #region Consulta de documentos por cliente (Soporte)
        /// <summary>
        /// Obtiene un documento por empresa y los primeros 8 digitos del idseguridad o por la empresa-- Resolución y Numero del documento
        /// </summary>
        /// <param name="codigo_adquiente"></param>
        /// <param name="numero_documento"></param>
        /// <param name="estado_recibo"></param>
        /// <param name="fecha_inicio"></param>
        /// <param name="fecha_fin"></param>
        /// <returns></returns>
        public List<TblDocumentos> ObtenerDocumentoCliente(string codigo_facturador, int? numero_documento, string IdSeguridad = "*", string numero_resolucion = "*")
        {


            if (codigo_facturador == "")
                throw new ApplicationException("Debe Indicar el Facturador");

            if (IdSeguridad == "*" && (numero_resolucion == "*" || numero_documento == 0 || numero_documento == null))
                throw new ApplicationException("No se han especificado los criterios de búsqueda");

            if ((IdSeguridad != "*" && IdSeguridad != null) && (numero_resolucion != "*" || numero_documento != null))
                throw new ApplicationException("No se han especificado los criterios de búsqueda");

            if (IdSeguridad != "*" && IdSeguridad != null)
                if ((IdSeguridad.Length != 8))
                    throw new ApplicationException("El codigo de plataforma debe ser de 8 digitos");


            List<TblDocumentos> respuesta = (from datos in context.TblDocumentos
                                             join empresa in context.TblEmpresas on datos.StrEmpresaAdquiriente equals empresa.StrIdentificacion
                                             where datos.StrEmpresaFacturador.Equals(codigo_facturador)
                                             && (datos.StrIdSeguridad.ToString().Contains(IdSeguridad) || IdSeguridad.Equals("*"))
                                             && (numero_resolucion.Contains(datos.StrNumResolucion.ToString()) || numero_resolucion.Equals("*"))
                                             && (datos.IntNumero==numero_documento || numero_documento==null)
                                             orderby datos.DatFechaIngreso descending
                                             select datos).ToList();
            return respuesta;
        }
        #endregion      

    }

    }

	
