using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HGInetMiFacturaElectonicaController.Configuracion
{
	public class Ctl_MaestrosEnum
	{
		public static List<string[]> ListaEnum(int tipo_enum, string tipo_ambiente = "*")
		{
			try
			{
				List<string[]> datos = new List<string[]>();
				switch (tipo_enum)
				{
					case 0:
						//Obttiene la lista de items del enumerable ProcesoEstado, y si le pasamos el tipo de ambiente(publico, privado) crea un filtro de resultados
						foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.ProcesoEstado)))
						{
							FieldInfo fi = value.GetType().GetField(value.ToString());
							AmbientValueAttribute[] ambiente = (AmbientValueAttribute[])fi.GetCustomAttributes(typeof(AmbientValueAttribute), false);
							if (tipo_ambiente != "*")
							{
								if (ambiente[0].Value.ToString() == tipo_ambiente)
								{
									string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>((int)value)))).Split(',');
									datos.Add(datos_enum);
								}
							}
							else
							{
								string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>((int)value)))).Split(',');
								datos.Add(datos_enum);
							}
						}
						break;
					case 1:
						foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.AdquirienteRecibo)))
						{
							string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.AdquirienteRecibo>((int)value)))).Split(',');
							datos.Add(datos_enum);
						}
						break;
					case 2:
						//Metodo para obtener los datos del enumerable de tipo de documento
						foreach (var value in Enum.GetValues(typeof(LibreriaGlobalHGInet.Objetos.TipoDocumento)))
						{
							string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<LibreriaGlobalHGInet.Objetos.TipoDocumento>((int)value)))).Split(',');
							if (Convert.ToInt32(datos_enum[0]) < 10)
							{
								datos.Add(datos_enum);
							}
						}
						break;
					case 3:
						//Metodo para obtener los datos del enumerable de tipo de Compra: 1: Cortesía, 2 Compra, 3 Post-Pago
						foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.TipoCompra)))
						{
							string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.TipoCompra>((int)value)))).Split(',');
							datos.Add(datos_enum);
						}
						break;
					case 4:
						//Metodo para obtener los datos del enumerable de Estatus del pago
						foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.Enumerables.EstadoPago)))
						{
							string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Enumerables.EstadoPago>((int)value)))).Split(',');
							datos.Add(datos_enum);
						}
						break;
					case 5:
						//Metodo para obtener los datos del enumerable de Estatus del pago
						foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.CategoriaEstado)))
						{
							//if (!value.Equals(HGInetMiFacturaElectonicaData.CategoriaEstado.NoRecibido))
							//{
								string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CategoriaEstado>((int)value)))).Split(',');
								datos.Add(datos_enum);
							//}
						}
						break;
					case 6:
						//Obtiene la lista de items del enumerable ProcesoEstado, y si le pasamos el tipo de ambiente(publico, privado) crea un filtro de resultados
						foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.ProcesoEstado)))
						{
							FieldInfo fi = value.GetType().GetField(value.ToString());
							CategoryAttribute[] Categoria = (CategoryAttribute[])fi.GetCustomAttributes(typeof(CategoryAttribute), false);
							string[] datos_enum = string.Format("{0},{1} - {2}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.ProcesoEstado>((int)value))), Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.CategoriaEstado>(Convert.ToInt16(Categoria[0].Category.ToString())))).Split(',');
							datos.Add(datos_enum);
						}
						break;
					case 7:
						//Obtiene la lista de items del enumerable TipoRegistro de la Auditoria
						foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.TipoRegistro)))
						{
							string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.TipoRegistro>((int)value)))).Split(',');
							datos.Add(datos_enum);
						}
						break;
					case 8:
						//Obtiene la lista de items del enumerable Procedencia de la Auditoria
						foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.Procedencia)))
						{
							string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.Procedencia>((int)value)))).Split(',');
							datos.Add(datos_enum);
						}
						break;
					case 9:
						//Obtiene la lista de items del enumerable EnumCertificadoras, y si le pasamos el tipo de ambiente(publico, privado) crea un filtro de resultados
						// retorno:  ID,Descripcion,CamposRequeridos
						foreach (var value in Enum.GetValues(typeof(HGInetFirmaDigital.EnumCertificadoras)))
						{
							FieldInfo fi = value.GetType().GetField(value.ToString());
							AmbientValueAttribute[] ambiente = (AmbientValueAttribute[])fi.GetCustomAttributes(typeof(AmbientValueAttribute), false);

							CategoryAttribute[] Categoria = (CategoryAttribute[])fi.GetCustomAttributes(typeof(CategoryAttribute), false);

							if (tipo_ambiente != "*")
							{
								if (ambiente[0].Value.ToString() == tipo_ambiente)
								{
									string[] datos_enum = string.Format("{0},{1},{2}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetFirmaDigital.EnumCertificadoras>((int)value))), Categoria[0].Category.ToString()).Split(',');
									datos.Add(datos_enum);
								}
							}
							else
							{
								string[] datos_enum = string.Format("{0},{1},{2}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetFirmaDigital.EnumCertificadoras>((int)value))), Categoria[0].Category.ToString()).Split(',');
								datos.Add(datos_enum);
							}
						}
						break;
					case 10:
						//Metodo para obtener los datos del enumerable de tipo de documento
						foreach (var value in Enum.GetValues(typeof(LibreriaGlobalHGInet.Objetos.TipoDocumento)))
						{
							string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<LibreriaGlobalHGInet.Objetos.TipoDocumento>((int)value)))).Split(',');
							if (Convert.ToInt32(datos_enum[0]) >= 10)
							{
								datos.Add(datos_enum);
							}
						}
						break;
					case 11:
						//Metodo para obtener los datos del enumerable de tipo de Compra: 0: Mixto, 1 Documento, 2 Nomina
						foreach (var value in Enum.GetValues(typeof(HGInetMiFacturaElectonicaData.TipoDocPlanes)))
						{
							string[] datos_enum = string.Format("{0},{1}", (int)value, (Enumeracion.GetDescription(Enumeracion.GetEnumObjectByValue<HGInetMiFacturaElectonicaData.TipoDocPlanes>((int)value)))).Split(',');
							datos.Add(datos_enum);
						}
						break;
				}
				return (datos);
			}
			catch (Exception excepcion)
			{
				throw new ApplicationException(excepcion.Message, excepcion.InnerException);
			}
		}
	}
}
