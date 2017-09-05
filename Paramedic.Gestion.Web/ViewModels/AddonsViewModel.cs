using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Web.ViewModels
{
	public class AddonsViewModel
	{
		#region Properties

		public IEnumerable<Noticia> Noticias { get; set; }

		public IEnumerable<ProductosModulo> Modulos { get; set; }

		public string License { get; set; }

		#endregion

		#region Constructors

		public AddonsViewModel(IEnumerable<Noticia> noticias, IEnumerable<ProductosModulo> modulos, string license)
		{
			this.Noticias = noticias;
			this.Modulos = modulos;
			this.License = license;
		}

		#endregion
	}
}