using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Web.ViewModels
{
	public class AddonsViewModel
	{
		#region Properties

		public IEnumerable<Noticia> Noticias { get; set; }

		public IEnumerable<ProductosModulo> Modulos { get; set; }

		#endregion

		#region Constructors

		public AddonsViewModel(IEnumerable<Noticia> noticias, IEnumerable<ProductosModulo> modulos)
		{
			this.Noticias = noticias;
			this.Modulos = modulos;
		}

		#endregion
	}
}