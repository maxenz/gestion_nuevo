using Newtonsoft.Json;
using Paramedic.Gestion.Model;
using SocialMedia.Services;
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
		private static string Url = ConfigurationManager.AppSettings["urlGestion"];
		private static string AdministratorMail = ConfigurationManager.AppSettings["administratorMail"];
		private static string ImportantStuffMail = ConfigurationManager.AppSettings["importantStuffMail"];

		static void Main(string[] args)
		{
			GetHistoriales().GetAwaiter().GetResult();
			GetExpiredLicenses().GetAwaiter().GetResult();
			GetExpiredLicenseSupports().GetAwaiter().GetResult();
		}

		#region Vencimiento de licencias

		static async Task GetExpiredLicenses()
		{
			using (var client = new HttpClient())
			{
				string repUrl = Url + "/ClientesLicencias/GetExpiredLicenses";
				HttpResponseMessage response = await client.GetAsync(repUrl);
				if (response.IsSuccessStatusCode)
				{
					string result = await response.Content.ReadAsStringAsync();
					IEnumerable<LicenciaDTO> licencias = JsonConvert.DeserializeObject<IEnumerable<LicenciaDTO>>(result);
					foreach (LicenciaDTO lic in licencias)
					{
						ProcessExpiredLicenses(lic);
					}

				}
			}
		}

		static void ProcessExpiredLicenses(LicenciaDTO lic)
		{
			string subject = string.Format("Se venció la licencia del cliente {0}",lic.Cliente);
			string body = string.Format("La licencia Nro {0} del cliente {1} acaba de vencer.", lic.NroLicencia, lic.Cliente);
			string from = AdministratorMail;
			string to = ImportantStuffMail;
			Message message = new EmailMessage(body, from, to, subject);
			MailService.Instance.Send(message);
		}

		#endregion

		#region Vencimiento de soporte de licencia

		static async Task GetExpiredLicenseSupports()
		{
			using (var client = new HttpClient())
			{
				string repUrl = Url + "/ClientesLicencias/GetExpiredLicenseSupports";
				HttpResponseMessage response = await client.GetAsync(repUrl);
				if (response.IsSuccessStatusCode)
				{
					string result = await response.Content.ReadAsStringAsync();
					IEnumerable<LicenciaDTO> licencias = JsonConvert.DeserializeObject<IEnumerable<LicenciaDTO>>(result);
					foreach (LicenciaDTO lic in licencias)
					{
						ProcessExpiredLicenseSupports(lic);
					}
				}
			}
		}

		static void ProcessExpiredLicenseSupports(LicenciaDTO lic)
		{
			string subject = string.Format("Se venció el soporte de la licencia del cliente {0}", lic.Cliente);
			string body = string.Format("El soporte de la licencia Nro {0} del cliente {1} acaba de vencer.", lic.NroLicencia, lic.Cliente);
			string from = AdministratorMail;
			string to = ImportantStuffMail;
			Message message = new EmailMessage(body, from, to, subject);
			MailService.Instance.Send(message);
		}

		#endregion


		#region Historiales

		static async Task GetHistoriales()
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
			var pmUrl = string.Format("{0}/ClientesHistoriales/CancelProductoModulo?histId={1}", Url, historial.Id);
			var response = client.PostAsync(pmUrl, null).Result;
			if (response.IsSuccessStatusCode)
			{
				Console.WriteLine(response);
			}
		}

		#endregion

	}
}
