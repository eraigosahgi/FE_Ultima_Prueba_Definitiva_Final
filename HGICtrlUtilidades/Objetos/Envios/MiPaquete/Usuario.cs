using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGICtrlUtilidades.Objetos.Envios
{
	public class GetUser
	{
		/*{
		"_id": "61bd01f43fc5580ae9c8216d",
		"name": "HGI",
		"surname": "S.A.S",
		"businessName": "HGI S.A.S",
		"personType": "Persona Jurídica",
		"tradeName": "Herramientas de Gestión Informática S.A.S",
		"documentType": "NIT",
		"documentNumber": "811021438",
		"salesChannel": "socialMedia",
		"productType": "textile",
		"cellPhone": "3136469239",
		"address": "calle 48 #77c-06",
		"locationCode": "05001000",
		"email": "tic@hgi.com.co",
		"prefix": "+57",
		"tyc": true,
		"newTyc": {"iAgree": true, "ip": "181.62.60.239", "acceptanceDate": "2021-12-17T21:32:36.224Z"},
		"accountEnabled": true,
		"cash": 300000,
		"createdAt": "2021-12-17T21:32:36.379Z",
		"updatedAt": "2022-01-05T15:47:16.590Z",
		"alternativeDirections": [{	"name": "calle 48 #77c-06", "address": "calle 48 #77c-06" }],
		"updateAt": "2022-01-04T20:17:34.109Z"
		} */
		/// <summary>
		/// Id de usuario en mipaquete.com
		/// </summary>
		public string _id { get; set; }

		/// <summary>
		/// nombre de usuario
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// apellido de usuario
		/// </summary>
		public string surname { get; set; }

		/// <summary>
		/// Nombre Comercial
		/// </summary>
		public string businessName { get; set; }

		/// <summary>
		/// Tipo de persona (Natural - Juridica)
		/// </summary>
		public string personType { get; set; }

		/// <summary>
		/// Razón Social
		/// </summary>
		public string tradeName { get; set; }

		/// <summary>
		/// Tipo de documento (CC - TI - PA - CE - NIT)
		/// </summary>
		public string documentType { get; set; }

		/// <summary>
		/// Número documento identificación
		/// </summary>
		public string documentNumber { get; set; }

		/// <summary>
		/// Canal de venta que utiliza el usuario
		/// Redes sociales - WhatsApp - Tienda Online Woocommerce - Tienda Online Shopify - Tienda online Magento - Tienda online Komercia -
		/// Tienda online Jumpseller - Otros - Ninguno
		/// </summary>
		public string salesChannel { get; set; }

		/// <summary>
		/// Tipo de productos
		/// Moda, ropa y textiles - Tecnología y electrónicos - Belleza y cuidado personal - Juguetería - Papelería, arte y cultura - Artículos deportivos
		/// Alimentos no perecederos - Artículos para vehículos - Artículos para mascotas - Artículos de hogar -Accesorios y bisutería - Otros
		/// </summary>
		public string productType { get; set; }

		/// <summary>
		/// Número celular
		/// </summary>
		public string cellPhone { get; set; }

		/// <summary>
		/// Dirección
		/// </summary>
		public string address { get; set; }

		/// <summary>
		/// Código Lozalización
		/// </summary>
		public string locationCode { get; set; }

		/// <summary>
		/// Correo electrónico
		/// </summary>
		public string email { get; set; }

		/// <summary>
		/// Prefijo
		/// </summary>
		public string prefix { get; set; }

		/// <summary>
		/// Aceptó términos y condiciones
		/// </summary>
		public bool tyc { get; set; }

		/// <summary>
		/// Estado de cuenta - cuenta habilitada
		/// </summary>
		public bool accountEnabled { get; set; }

		/// <summary>
		/// Saldo Disponble
		/// </summary>
		public decimal cash { get; set; }

		/// <summary>
		/// Fecha de creación
		/// </summary>
		public DateTime createdAt { get; set; }

		/// <summary>
		/// Fecha Actualización
		/// </summary>
		public DateTime updatedAt { get; set; }

		/// <summary>
		/// Fecha Actualización
		/// </summary>
		public DateTime updateAt { get; set; }

		/// <summary>
		/// Direcciones Alternativas
		/// </summary>
		public List<alternativeDirections> alternativeDirections { get; set; }
	}

	public class CreateUser
	{
		/*
				 {
			"name": "prueba api 2 register", // required
			"surname": "V2", // required
			"businessName": "tienda jesus", // required
			"documentType": "CC", // required
			"documentNumber": "10000001010", // required
			"salesChannel": "facebook", // required
			"productType": "otros", // required
			"personType": "Natural", // required
			"cellPhone": "3007782877", // required
			"address": "carrera 18 #10-104", // required
			"locationCode": "100101010", // required
			"email": "pruebav8@yopmail.com", // required
			"password": "123456789*Aa", // required
			"averageShipments": "30 a 200", // required
			"prefix": "+57", // required
			"tyc": true, // required
			"accountBank": {
				"bank": "bancolombia",// required
				"beneficiaryName": "pruebav2@yopmail.com",// required
				"accountType": "ahorros",// required
				"accountNumber": 20102112001,// required
				"typeId": "NIT",// required
				"numberId": "1037663886"// required
			}
		}	 
		*/

		/// <summary>
		/// nombre de usuario
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// apellido de usuario
		/// </summary>
		public string surname { get; set; }

		/// <summary>
		/// Nombre Comercial
		/// </summary>
		public string businessName { get; set; }

		/// <summary>
		/// Tipo de documento (CC - TI - PA - CE - NIT)
		/// </summary>
		public string documentType { get; set; }

		/// <summary>
		/// Número documento identificación
		/// </summary>
		public string documentNumber { get; set; }

		/// <summary>
		/// Canal de venta que utiliza el usuario
		/// Redes sociales - WhatsApp - Tienda Online Woocommerce - Tienda Online Shopify - Tienda online Magento - Tienda online Komercia -
		/// Tienda online Jumpseller - Otros - Ninguno
		/// </summary>
		public string salesChannel { get; set; }

		/// <summary>
		/// Tipo de productos
		/// Moda, ropa y textiles - Tecnología y electrónicos - Belleza y cuidado personal - Juguetería - Papelería, arte y cultura - Artículos deportivos
		/// Alimentos no perecederos - Artículos para vehículos - Artículos para mascotas - Artículos de hogar -Accesorios y bisutería - Otros
		/// </summary>
		public string productType { get; set; }

		/// <summary>
		/// Tipo de persona (Natural - Juridica)
		/// </summary>
		public string personType { get; set; }

		/// <summary>
		/// Número celular
		/// </summary>
		public string cellPhone { get; set; }

		/// <summary>
		/// Dirección
		/// </summary>
		public string address { get; set; }

		/// <summary>
		/// Código Lozalización
		/// </summary>
		public string locationCode { get; set; }

		/// <summary>
		/// Correo electrónico
		/// </summary>
		public string email { get; set; }

		/// <summary>
		/// Contraseña
		/// </summary>
		public string password { get; set; }

		/// <summary>
		/// Prefijo
		/// </summary>
		public string prefix { get; set; }

		/// <summary>
		/// Aceptó términos y condiciones
		/// </summary>
		public bool tyc { get; set; }

		/// <summary>
		/// Cuenta bancaria
		/// </summary>
		public accountBank accountBank { get; set; }
	}

	public class UpdateUser
	{
		/*{
    "name": "HGI",
    "surname": "S.A.S",
    "businessName": "HGI S.A.S",
    "documentType": "NIT",
    "documentNumber": "811021438",
    "salesChannel": "socialMedia",
    "productType": "textile",
    "cellPhone": "3136469239",
    "address": "calle 48 #77c-06",
    "locationCode": "05001000",
    "email": "tic@hgi.com.co",
    "password": "123456789"  
}*/

		/// <summary>
		/// nombre de usuario
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// apellido de usuario
		/// </summary>
		public string surname { get; set; }

		/// <summary>
		/// Nombre Comercial
		/// </summary>
		public string businessName { get; set; }

		/// <summary>
		/// Tipo de documento (CC - TI - PA - CE - NIT)
		/// </summary>
		public string documentType { get; set; }

		/// <summary>
		/// Número documento identificación
		/// </summary>
		public string documentNumber { get; set; }

		/// <summary>
		/// Canal de venta que utiliza el usuario
		/// Redes sociales - WhatsApp - Tienda Online Woocommerce - Tienda Online Shopify - Tienda online Magento - Tienda online Komercia -
		/// Tienda online Jumpseller - Otros - Ninguno
		/// </summary>
		public string salesChannel { get; set; }

		/// <summary>
		/// Tipo de productos
		/// Moda, ropa y textiles - Tecnología y electrónicos - Belleza y cuidado personal - Juguetería - Papelería, arte y cultura - Artículos deportivos
		/// Alimentos no perecederos - Artículos para vehículos - Artículos para mascotas - Artículos de hogar -Accesorios y bisutería - Otros
		/// </summary>
		public string productType { get; set; }

		/// <summary>
		/// Número celular
		/// </summary>
		public string cellPhone { get; set; }

		/// <summary>
		/// Dirección
		/// </summary>
		public string address { get; set; }

		/// <summary>
		/// Código Lozalización
		/// </summary>
		public string locationCode { get; set; }

		/// <summary>
		/// Correo electrónico
		/// </summary>
		public string email { get; set; }

		/// <summary>
		/// Contraseña
		/// </summary>
		public string password { get; set; }
	}

	public class alternativeDirections
	{
		public string name { get; set; }
		public string address { get; set; }
	}

	public class accountBank
	{
		/* "bank": "bancolombia",
        "beneficiaryName": "pruebav2@yopmail.com",
        "accountType": "ahorros",
        "accountNumber": 20102112001,
        "typeId": "NIT",
        "numberId": "1037663886"*/

		/// <summary>
		/// Banco
		///  'BANCOLOMBIA-(Desembolso: $0)', 'NEQUI-(Desembolso: $2.000)', 'DAVIPLATA-(Desembolso: $7.400)', 'BANCO AV VILLAS-(Desembolso: $7.400)',
		///  'BANCO CAJA SOCIAL-(Desembolso: $7.400)', 'BANCO DAVIVIENDA-(Desembolso: $7.400)', 'BANCO DE BOGOTA-(Desembolso: $7.400)',
		///  'BANCO DE OCCIDENTE-(Desembolso: $7.400)', 'BANCO FINANDINA-(Desembolso: $7.400)', 'BANCO GNB SUDAMERIS (Costo de Desembolso: $7.400)',
		///  'BANCO MULTIBANK-(Desembolso: $7.400)', 'BANCO POPULAR-(Desembolso: $7.400)', 'BANCO SANTANDER-(Desembolso: $7.400)', 'BANCOOMEVA-(Desembolso: $7.400)',
		///  'BBVA-(Desembolso: $7.400)', 'CITIBANK-(Desembolso: $7.400)', 'COLPATRIA-(Desembolso: $7.400)', 'COLTEFINANCIERA-(Desembolso: $7.400)',
		///  'FALLABELLA-(Desembolso: $7.400)', 'ITAU-(Desembolso: $7.400)',
		/// </summary>
		public string bank { get; set; }

		/// <summary>
		/// Nombre beneficiario
		/// </summary>
		public string accountType { get; set; }

		/// <summary>
		/// Número de cuenta
		/// </summary>
		public string accountNumber { get; set; }

		/// <summary>
		/// Beneficiario de la cuenta , correo
		/// </summary>
		public string beneficiaryName { get; set; }

		/// <summary>
		/// Tipo de documento titular cuenta (CC - TI - PA - CE - NIT)
		/// </summary>
		public string typeId { get; set; }

		/// <summary>
		/// Número documento identificación titular cuenta
		/// </summary>
		public string numberId { get; set; }

	}

}
