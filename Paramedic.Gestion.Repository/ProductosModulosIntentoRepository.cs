using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
	public class ProductosModulosIntentoRepository : GenericRepository<ProductosModulosIntento>, IProductosModulosIntentoRepository
	{
		public ProductosModulosIntentoRepository(DbContext context)
			: base(context)
		{

		}
	}
}
