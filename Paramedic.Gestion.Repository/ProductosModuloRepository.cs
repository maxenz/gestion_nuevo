using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
	public class ProductosModuloRepository : GenericRepository<ProductosModulo>, IProductosModuloRepository
	{
		public ProductosModuloRepository(DbContext context)
			: base(context)
		{

		}
	}
}
