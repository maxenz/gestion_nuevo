using Newtonsoft.Json;
using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.TrialsExpirationManager
{
	class Program
	{
		private static string Url = ConfigurationManager.AppSettings["url"];

		static void Main(string[] args)
		{
			GetHistoriales();
			Console.ReadLine();
		}

		static async void GetHistoriales()
		{
			using (var client = new HttpClient())
			{
				string repUrl = Url + "/ClientesHistoriales/GetHistoriales";
				HttpResponseMessage response = await client.GetAsync(repUrl);
				if (response.IsSuccessStatusCode)
				{
					string result = await response.Content.ReadAsStringAsync();
					List<HistorialDTO> historiales = JsonConvert.DeserializeObject<List<HistorialDTO>>(result);
					foreach (HistorialDTO h in historiales)
					{
						ProcessExpiration(h);
					}

				}
			}
		}

		static void ProcessExpiration(HistorialDTO historial)
		{
			if (historial.FechaVencimiento.Date < DateTime.Now.Date)
			{
				CancelProductoModulo(historial);
			}
		}

		static void CancelProductoModulo(HistorialDTO historial)
		{
			var client = new HttpClient();
			var pmUrl = string.Format("{0}/ClientesHistoriales/CancelProductoModulo?histId={1}",Url, historial.Id);
			var response = client.PostAsync(pmUrl, null).Result;
			if (response.IsSuccessStatusCode)
			{
				Console.WriteLine(response);
			}
		}
	}
}
