using LibreriaGlobalHGInet.Parametros;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using LibreriaGlobalHGInet.Properties;

namespace LibreriaGlobalHGInet.General
{
	public class Imagen
	{
		/// <summary>
		/// Guarda la imagen representada en los bits enviados para el servicio
		/// </summary>
		/// <param name="datos_imagen">bytes de la imagen</param>
		/// <param name="id_servicio">id del servicio</param>
		/// <param name="tamano_maximo">tamaño en pixeles para redimensionar</param>
		/// <returns></returns>
		public bool Guardar(byte[] datos_imagen, string nombre_imagen, string carpeta_imagen, int tamano_maximo = 0)
		{
			try
			{
				string ruta = Directorio.CrearDirectorio(string.Format("{0}{1}\\{2}\\", Parametro.RutaServidor, RecursoArchivosParametros.DirectorioArchivosApp, carpeta_imagen));

				ruta = string.Format("{0}{1}.png", ruta, nombre_imagen);

				MemoryStream ms = new MemoryStream(datos_imagen);
				Bitmap bmap = new Bitmap(ms);

				// borrar imagen si existe
				Archivo.Borrar(ruta);

				// guardar la imagen
				bmap.Save(ruta, ImageFormat.Png);

				// redimensionar la imagen
				if(tamano_maximo != 0)
					RedimencionarImagen(ruta, tamano_maximo, ImageFormat.Png);

				bmap.Dispose();

				return true;

			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message);
			}
		}

		/// <summary>
		/// almacena las imagenes en una ruta y un formato especifico
		/// </summary>
		/// <param name="datos_imagen"></param>
		/// <param name="nombre_imagen"></param>
		/// <param name="directorio_imagen"></param>
		/// <param name="carpeta_imagen"></param>
		/// <param name="formato"></param>
		/// <returns></returns>
		public bool GuardarRutaEspecifica(byte[] datos_imagen, string nombre_imagen, string directorio, ImageFormat formato)
		{
			try
			{
				string ruta = Directorio.CrearDirectorio(directorio);

				ruta = string.Format("{0}{1}.{2}", ruta, nombre_imagen, formato.ToString().ToLower());

				MemoryStream ms = new MemoryStream(datos_imagen);
				Bitmap img = new Bitmap(ms);

				// borrar imagen si existe
				Archivo.Borrar(ruta);

				// guardar la imagen
				img.Save(ruta, formato);

				img.Dispose();

				return true;
			}
			catch (Exception exec)
			{
				throw new ApplicationException(exec.Message);
			}
		}

		/// <summary>
		/// Redimensiona una imagen
		/// </summary>
		/// <param name="ruta">ruta física de la imagen</param>
		/// <param name="tamano">tamaño a redimensionar</param>
		/// <param name="extension">extensión de la imagen</param>
		/// <returns>ruta física de la imagen</returns>
		public static string RedimencionarImagen(string ruta, int tamano, ImageFormat extension)
		{
			try
			{
				if (!File.Exists(ruta))
					throw new ApplicationException("La ruta: " + ruta + " del archivo no existe");

				//imagen original
				System.Drawing.Image imagen_actual = Bitmap.FromFile(ruta);

				//imagen nueva redimensionada
				System.Drawing.Image imagen_nueva = Bitmap.FromFile(ruta);

				int destinoWidth = 0;
				int destinoHeight = 0;

				//calcula las nuevas dimensiones de la imagen
				if (imagen_actual.Width > imagen_actual.Height)
				{
					destinoWidth = tamano;
					destinoHeight = (imagen_actual.Height * tamano) / imagen_actual.Width;
				}
				else
				{
					destinoHeight = tamano;
					destinoWidth = (imagen_actual.Width * tamano) / imagen_actual.Height;
				}

				//crea una matriz para dibujar la imagen
				Bitmap bitmap_imagen = new Bitmap(destinoWidth, destinoHeight, PixelFormat.Format32bppRgb);

				//crea un objeto que le permita dibujar sobre la matriz
				Graphics g = Graphics.FromImage(bitmap_imagen);

				//da las propiedades a la imagen
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.PixelOffsetMode = PixelOffsetMode.HighQuality;

				//dibuja la imagen nueva teniendo en cuenta las dimensiones nuevas
				g.DrawImage(imagen_nueva, new Rectangle(0, 0, destinoWidth, destinoHeight), new Rectangle(0, 0, imagen_actual.Width, imagen_actual.Height), GraphicsUnit.Pixel);

				//cierra las imagenes
				imagen_nueva.Dispose();
				imagen_actual.Dispose();

				//borra la imagen anterior
				Archivo.Borrar(ruta);

				//guarda la nueva imagen en la ruta nueva
				bitmap_imagen.Save(ruta, extension);

				//cierra la nueva imagen
				bitmap_imagen.Dispose();

				//cierra el objeto qwue dibujo la imagen
				g.Dispose();

				return ruta;
			}
			catch (Exception exec)
			{
				throw new ApplicationException("No se pudo redimensionar la imagen", exec);
			}
		}

		/// <summary>
		/// Valida si un vector de bytes es una imagen válida
		/// </summary>
		/// <param name="bytes">vector de bytes</param>
		/// <returns>indica si es válida o no</returns>
		public static Image ValidarImagen(byte[] bytes)
		{
			try
			{
				Image img = null;

				using (MemoryStream ms = new MemoryStream(bytes))
					img = Image.FromStream(ms);

				return img;
			}
			catch (ArgumentException)
			{
				return null;
			}
		}
	}
}
