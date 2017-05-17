using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ProductoService : EntityService<Producto>, IProductoService
    {
        public ProductoService(IUnitOfWork unitOfWork, IProductoRepository productoRepository)
            : base(unitOfWork, productoRepository)
        {
        }
    }
}
