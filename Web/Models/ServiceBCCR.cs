using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ServiceBCCR
	{
		private readonly string TOKEN = "A0RMLN8LGO";
		private readonly string NOMBRE = "Gabriela Bolaños";
		private readonly string CORREO = "mariagbolanosv@gmail.com";

		public decimal GetDolar(decimal colones)
		{
			DataSet dataset = null;
			string fechaActual;

			DateTime fecha = DateTime.Now;
			// Se convierten las fechas a string en el formato solicitado
		    fechaActual=fecha.ToString("dd/MM/yyyy");

			//si es compra o venta 318 o 317

			// Protocolo de comunicaciones
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

			// Se instancia al Servicio Web(Por seguridad)
			BCCR.wsindicadoreseconomicosSoapClient client =
				new BCCR.wsindicadoreseconomicosSoapClient("wsindicadoreseconomicosSoap12");

			// Se invoca.
			dataset = client.ObtenerIndicadoresEconomicos("318", fechaActual,
						  fechaActual, NOMBRE, "N", CORREO, TOKEN);

			DataTable table = dataset.Tables[0];
			decimal compra =0;

			foreach (DataRow row in table.Rows)
			{
				// Validar el error. No es la forma correcta pero bueno.
				if (row[0].ToString().Contains("error"))
				{
					throw new Exception(row[0].ToString());
				}
				compra = Convert.ToDecimal(row[2].ToString());
			}

			return colones / compra;

		}

		
	}
}