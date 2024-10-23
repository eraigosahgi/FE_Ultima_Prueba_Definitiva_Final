using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace LibreriaGlobalHGInet.General
{
    public static class Certificado
    {
        /// <summary>
        /// Obtiene la información del certificado digital por el nombre o serial
        /// </summary>
        /// <param name="nombre">Nombre del certificado</param>
        /// <param name="serial">Serial del certificado</param>
        /// <returns></returns>
        public static X509Certificate2 Obtener(string nombre, string serial)
        {
            X509Store Almacen = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            Almacen.Open(OpenFlags.ReadOnly);
            string error = string.Empty;
            try
            {
                X509Certificate2Collection certificados = new X509Certificate2Collection();

                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    certificados = Almacen.Certificates.Find(X509FindType.FindBySubjectName, nombre, true);
                }
                else if (!string.IsNullOrWhiteSpace(serial))
                {
                    certificados = Almacen.Certificates.Find(X509FindType.FindBySerialNumber, serial, true);
                }

                if (certificados.Count == 0)
                {
                    error = "No se encontraron certificados";
                    throw new ApplicationException(error);
                }

                X509Certificate2 certificado = certificados[0];
                if (certificado.PrivateKey == null)
                {
                    error = "El certificado no contiene un llave privada";
                    throw new ApplicationException(error);
                }

                return certificado;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
            finally
            {
                Almacen.Close();
            }
        }

        /// <summary>
        /// Permite seleccionar el certificado digital que se va a utilizar
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2Collection SeleccionarCertificado()
        {
            try
            {
                // Abro el contenedor de certificados X.509 del usuario actual
                X509Store store = new X509Store(StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                // Pongo todos los certificados en un contenedor 
                X509Certificate2Collection certificates = store.Certificates;
                X509Certificate2Collection foundCertificates = certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection selectedCertificates = X509Certificate2UI.SelectFromCollection(foundCertificates,
                    "Seleccione un certificado.",
                    "Seleccione un certificado de la siguiente lista:",
                    X509SelectionFlag.SingleSelection);

                return selectedCertificates;
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }

        /// <summary>
        /// Verifica la firma digital de un documento
        /// </summary>
        /// <param name="rutaDoc"></param>
        /// <param name="certificate"></param>
        /// <returns></returns>        
        public static bool Verificar(string ruta_documento, X509Certificate2 certificate)
        {
            try
            {
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(ruta_documento);

                SignedXml signedXml = new SignedXml(XmlDoc);
                XmlNodeList nodeList = XmlDoc.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");

                if (nodeList.Count <= 0)
                {
                    nodeList = XmlDoc.GetElementsByTagName("Signature");
                    if (nodeList.Count <= 0)
                        throw new CryptographicException("No se encontro un elemento Signature en el documento.");
                }

                signedXml.LoadXml((XmlElement)nodeList[0]);
                return signedXml.CheckSignature(certificate.PrivateKey);
            }
            catch (Exception excepcion)
            {
                throw new ApplicationException(excepcion.Message, excepcion.InnerException);
            }
        }
    }
}
