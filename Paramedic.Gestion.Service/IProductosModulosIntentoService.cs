using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Service
{
	public interface IProductosModulosIntentoService : IEntityService<ProductosModulosIntento>
	{
		IEnumerable<ProductosModulosIntento> GetIntentosByProductoModuloId(int id);
	}
}
