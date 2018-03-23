using LibreriaGlobalHGInet.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGInetFirmaDigital
{
	public class ArchivoXml
	{		
		public string RutaSinFirma;
		public string RutaConFirma;
		public string RutaZip;
		public bool ReemplazarOut;

		/// <summary>
		/// Indica si generó o no correctamente el firmado
		/// </summary>
		public bool FirmaValida;

		/// <summary>
		/// Indica si generó o no correctamente el archivo
		/// </summary>
		public bool Generado;

		/// <summary>
		/// Excepción presentada por en el proceso
		/// </summary>
		public Exception Excepcion;

		/// <summary>
		/// Constructor
		/// </summary>
		public ArchivoXml()
		{
		}

		/// <summary>
		/// Valida la existencia del archivo de entrada y la existencia del directorio de salida
		/// </summary>
		public bool Validar()
		{
			try
			{
				if (this.Excepcion != null)
				{
					this.Generado = false;
					return false;
				}

				if (!Archivo.ValidarExistencia(this.RutaSinFirma))
				{
					this.Generado = false;
					this.Excepcion = new ApplicationException("Error al obtener la ruta de almacenamiento del xml sin firma.");
					return false;
				}
				/*
				if (!Directorio.ValidarExistenciaArchivo(this.RutaConFirma))
				{
					this.Generado = false;
					this.Excepcion = new ApplicationException("Error al obtener la ruta de almacenamiento del xml con firma.");
					return false;
				}
				else
				{
					if (ReemplazarOut)
					{
						try
						{
							Archivo.Borrar(this.RutaConFirma);
						}
						catch (Exception excepcion)
						{
							this.Generado = false;
							this.Excepcion = new ApplicationException(string.Format("Error al eliminar el archivo {0}.", this.RutaConFirma), excepcion);
							return false;
						}
					}
					else
					{
						this.Generado = false;
						this.Excepcion = new ApplicationException(string.Format("No fue permitido sobreescribir el archivo: {0}", this.RutaConFirma));
						return false;
					}
				}
				*/
				return true;
			}
			catch (Exception excepcion)
			{
				throw excepcion;
			}
		}

	}
}
