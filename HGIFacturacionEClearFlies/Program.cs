using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGIFacturacionEClearFlies
{
    class Program
    {

        static string carpeta_resoluciones = "XmlFacturaEResoluciones";

        static string carpeta_xml_sin_firmar = "XmlFacturaE";
        static string carpeta_P_FacturaEConsultaDian = "FacturaEConsultaDian";
        static string carpeta_P_XmlAcuse = "XmlAcuse";
        static string carpeta_P_FacturaEDian = "FacturaEDian";
        static int archivos_eliminados = 0;
        static int numero_archivos_a_eliminar = 1;
        static bool muestra_resumen = false;



        static void Main(string[] args)
        {
            ProcesarArchivos();
        }


        public static void ProcesarArchivos()
        {
            //var appSettings = ConfigurationManager.AppSettings;

            muestra_resumen = Convert.ToBoolean(ConfigurationManager.AppSettings["muestra_resumen"]);
            numero_archivos_a_eliminar = Convert.ToInt32(ConfigurationManager.AppSettings["numero_archivos_a_eliminar"]);
            List<Empresa> lista_empresas = new List<Empresa>();
            Empresa clase_empresa = new Empresa();
            string directorio_dms = string.Empty;
            string contenido_archivo = string.Empty;

            try
            {

                directorio_dms = string.Format(@"{0}\{1}", ObtenerDirectorioRaiz(), "facturaelectronica");

                //contenido_archivo = string.Format("Obteniendo directorios de la ruta {0}", directorio_dms);

                List<string> directorios_nits = Directory.GetDirectories(directorio_dms).OrderByDescending(dir => dir).ToList();

                if (directorios_nits.Any())
                {
                    //Console.WriteLine(string.Format("Se encontraron {0} directorios.", directorios_nits.Count));

                    //contenido_archivo += string.Format("{0}{1}", System.Environment.NewLine, string.Format("Se encontraron {0} directorios.", directorios_nits.Count));
                    // recorre carpetas de empresas
                    foreach (string item in directorios_nits)
                    {

                        if (archivos_eliminados >= numero_archivos_a_eliminar)
                        {
                            break;
                        }

                        string empresa = Path.GetFileName(item.TrimEnd(Path.DirectorySeparatorChar));
                        clase_empresa = new Empresa();
                        clase_empresa.empresa = empresa;

                        //Console.WriteLine(string.Format("{0}\n", empresa));


                        //// Fechas de consulta de archivos a borrar
                        DateTime inicial = GetFecha().AddYears(-6);
                        DateTime final = GetFecha().AddDays(-104);

                        string directorio_xml_sin_firmar = string.Format(@"{0}\{1}", item, carpeta_xml_sin_firmar);

                        if (Directory.Exists(directorio_xml_sin_firmar))
                        {
                            List<FileInfo> archivos_sin_firmar = GetFilesBetween(directorio_xml_sin_firmar, inicial, final).ToList();
                            Console.WriteLine(string.Format("Se encontraron {0} archivos en {1}. - {2}", archivos_sin_firmar.Count, carpeta_xml_sin_firmar, item));

                            List<Cl_Archivos> lista_de_archivos = new List<Cl_Archivos>();
                            foreach (var archivo in archivos_sin_firmar)
                            {
                                try
                                {
                                    if (archivos_eliminados >= numero_archivos_a_eliminar)
                                    {
                                        break;
                                    }
                                    Cl_Archivos archivo_eliminado = new Cl_Archivos();
                                    archivo.Delete();
                                    archivo_eliminado.nombre_archivo = archivo.Name.ToString();
                                    lista_de_archivos.Add(archivo_eliminado);
                                    archivos_eliminados++;

                                }
                                catch (Exception)
                                {
                                }
                            }

                            //clase_empresa.lista_archivos = lista_de_archivos;

                            //lista_empresas.Add(clase_empresa);
                        }


                        //*****************
                        string directorio_FacturaEConsultaDian = string.Format(@"{0}\{1}", item, carpeta_P_FacturaEConsultaDian);

                        if (Directory.Exists(directorio_FacturaEConsultaDian))
                        {
                            List<FileInfo> archivos_sin_firmar = GetFilesBetween(directorio_FacturaEConsultaDian, inicial, final).ToList();
                            Console.WriteLine(string.Format("Se encontraron {0} archivos en {1}.", archivos_sin_firmar.Count, carpeta_P_FacturaEConsultaDian));

                            List<Cl_Archivos> lista_de_archivos = new List<Cl_Archivos>();
                            foreach (var archivo in archivos_sin_firmar)
                            {
                                try
                                {
                                    if (archivos_eliminados >= numero_archivos_a_eliminar)
                                    {
                                        break;
                                    }
                                    Cl_Archivos archivo_eliminado = new Cl_Archivos();
                                    archivo.Delete();
                                    archivo_eliminado.nombre_archivo = archivo.Name.ToString();
                                    lista_de_archivos.Add(archivo_eliminado);
                                    archivos_eliminados++;

                                }
                                catch (Exception)
                                {
                                }
                            }

                            //clase_empresa.lista_archivos = lista_de_archivos;

                            //lista_empresas.Add(clase_empresa);
                        }
                        //*****************



                        //*****************
                        string directorio_XmlAcuse = string.Format(@"{0}\{1}", item, carpeta_P_XmlAcuse);

                        if (Directory.Exists(directorio_XmlAcuse))
                        {
                            List<FileInfo> archivos_sin_firmar = GetFilesBetween(directorio_XmlAcuse, inicial, final).ToList();
                            Console.WriteLine(string.Format("Se encontraron {0} archivos en {1}.", archivos_sin_firmar.Count, carpeta_P_XmlAcuse));

                            List<Cl_Archivos> lista_de_archivos = new List<Cl_Archivos>();
                            foreach (var archivo in archivos_sin_firmar)
                            {
                                try
                                {
                                    if (archivos_eliminados >= numero_archivos_a_eliminar)
                                    {
                                        break;
                                    }
                                    Cl_Archivos archivo_eliminado = new Cl_Archivos();
                                    archivo.Delete();
                                    archivo_eliminado.nombre_archivo = archivo.Name.ToString();
                                    lista_de_archivos.Add(archivo_eliminado);
                                    archivos_eliminados++;

                                }
                                catch (Exception)
                                {
                                }
                            }

                            //clase_empresa.lista_archivos = lista_de_archivos;

                            //lista_empresas.Add(clase_empresa);
                        }
                        //*****************

                        //*****************
                        string directorio_FacturaEDian = string.Format(@"{0}\{1}", item, carpeta_P_FacturaEDian);

                        if (Directory.Exists(directorio_FacturaEDian))
                        {
                            List<FileInfo> archivos_sin_firmar = GetFilesBetween(directorio_FacturaEDian, inicial, final).ToList();
                            Console.WriteLine(string.Format("Se encontraron {0} archivos en {1}.", archivos_sin_firmar.Count, carpeta_P_FacturaEDian));

                            List<Cl_Archivos> lista_de_archivos = new List<Cl_Archivos>();
                            foreach (var archivo in archivos_sin_firmar)
                            {
                                try
                                {
                                    if (archivos_eliminados >= numero_archivos_a_eliminar)
                                    {
                                        break;
                                    }
                                    Cl_Archivos archivo_eliminado = new Cl_Archivos();
                                    archivo.Delete();
                                    archivo_eliminado.nombre_archivo = archivo.Name.ToString();
                                    lista_de_archivos.Add(archivo_eliminado);
                                    archivos_eliminados++;

                                }
                                catch (Exception)
                                {
                                }
                            }

                            //clase_empresa.lista_archivos = lista_de_archivos;

                            //lista_empresas.Add(clase_empresa);
                        }
                        //*****************


                        // fechas de consulta resoluciones
                        //DateTime inicial = GetFecha().AddYears(-3);
                        //DateTime final = GetFecha().AddDays(-2);
                        string directorio_resoluciones = string.Format(@"{0}\{1}", item, carpeta_resoluciones);

                        if (Directory.Exists(directorio_resoluciones))
                        {
                            List<FileInfo> archivos = GetFilesBetween(directorio_resoluciones, inicial, final).ToList();
                            Console.WriteLine(string.Format("Se encontraron {0} archivos en {1}.", archivos.Count, carpeta_resoluciones));

                            //clase_empresa.descreipcion = string.Format("Se encontraron {0} archivos en {1}.", archivos.Count, carpeta_resoluciones);

                            List<Cl_Archivos> lista_de_archivos = new List<Cl_Archivos>();
                            foreach (var archivo in archivos)
                            {
                                try
                                {
                                    if (archivos_eliminados >= numero_archivos_a_eliminar)
                                    {
                                        break;
                                    }
                                    Cl_Archivos archivo_eliminado = new Cl_Archivos();
                                    archivo.Delete();
                                    archivo_eliminado.nombre_archivo = archivo.Name.ToString();
                                    lista_de_archivos.Add(archivo_eliminado);
                                    archivos_eliminados++;

                                }
                                catch (Exception)
                                {
                                }
                            }

                            //clase_empresa.lista_archivos = lista_de_archivos;

                            //lista_empresas.Add(clase_empresa);
                        }

                    }

                }



            }
            catch (Exception exc)
            {
                string error = exc.Message;
            }
            finally
            {
                if (archivos_eliminados == 0)
                {
                    contenido_archivo = "No se encontraton archivos para eliminar";
                }
                else
                {
                    contenido_archivo = string.Format("Archivos eliminados {0}", archivos_eliminados);
                    //contenido_archivo += string.Format(@"{0}{1}", System.Environment.NewLine, JsonConvert.SerializeObject(lista_empresas));
                }

                CrearArchivo(directorio_dms, contenido_archivo);
                if (muestra_resumen)
                {
                    Console.WriteLine("\n\n");
                    Console.WriteLine("Presione Enter para salir...");
                    Console.ReadLine();
                }

            }
        }





        public class Empresa
        {
            public string empresa { get; set; }
            public string descreipcion { get; set; }
            public List<Cl_Archivos> lista_archivos { get; set; }
        }

        public class Cl_Archivos
        {
            public string nombre_archivo { get; set; }

        }

        public static void CrearArchivo(string directorio, string contenido)
        {
            string ruta_archivo = string.Format(@"{0}\{1}{2}", directorio, DateTime.Now.ToString("yyyy -mm-dd"), "_Resultado.txt");
            if (!File.Exists(ruta_archivo))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(ruta_archivo))
                {
                    sw.WriteLine(contenido);
                }
            }
        }


        /// <summary>
        /// Obtiene el directorio raíz donde se encuentra el aplicativo instalado
        /// </summary>
        /// <returns type="string">ruta física completa</returns>
        public static string ObtenerDirectorioRaiz()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// busca los archivos de un directorio con la posibilidad de enviar un patrón de bísqueda
        /// </summary>
        /// <param name="ruta">ruta física</param>
        /// <param name="patron_busqueda">patrón por ej: *.* o *.png</param>
        /// <returns>archivos del directorio de acuerdo con el patrón enviado</returns>
        public static IEnumerable<FileInfo> ObtenerArchivosDirectorio(string ruta, string patron_busqueda = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(patron_busqueda))
                    patron_busqueda = "*";

                return new DirectoryInfo(ruta).EnumerateFiles(patron_busqueda, SearchOption.TopDirectoryOnly);
            }
            catch (Exception exec)
            {
                throw new ApplicationException(exec.Message);
            }
        }


        public static IEnumerable<FileInfo> GetFilesBetween(string path, DateTime start, DateTime end)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles();
            return files.Where(f => f.LastWriteTime < end.Date && f.LastWriteTime > start.Date);
        }


        /// <summary>
        /// Obtiene la fecha actual en UTC Local
        /// </summary>
        /// <returns type="DateTime">fecha completa actual</returns>
        public static DateTime GetFecha()
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }


    }


}

