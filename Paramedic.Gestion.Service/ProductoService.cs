using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class ProductoService : EntityService<Producto>, IProductoService
    {
        IUnitOfWork _unitOfWork;
        IProductoRepository _productoRepository;

        public ProductoService(IUnitOfWork unitOfWork, IProductoRepository productoRepository)
            : base(unitOfWork, productoRepository)
        {
        }
    }
}
