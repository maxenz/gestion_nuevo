using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(DbContext context)
            : base(context)
        {

        }
    }
}
