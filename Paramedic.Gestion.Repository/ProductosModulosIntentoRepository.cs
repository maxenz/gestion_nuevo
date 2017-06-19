using Paramedic.Gestion.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Paramedic.Gestion.Repository
{
	public class ProductosModulosIntentoRepository : GenericRepository<ProductosModulosIntento>, IProductosModulosIntentoRepository
	{
		public ProductosModulosIntentoRepository(DbContext context)
			: base(context)
		{

		}

		public IEnumerable<ProductosModulosIntento> GetByModuloId(int modId)
		{
			return _dbset
				.Include(x => x.ProductosModulo)
				.Where(
					x => x.ProductosModuloId == modId
				);				
		}
	}
}
