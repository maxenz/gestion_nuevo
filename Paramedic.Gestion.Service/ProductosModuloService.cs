using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
	public class ProductosModuloService : EntityService<ProductosModulo>, IProductosModuloService
	{
		IUnitOfWork _unitOfWork;
		IProductosModuloRepository _repo;

		public ProductosModuloService(IUnitOfWork unitOfWork, IProductosModuloRepository _repo)
			: base(unitOfWork, _repo)
		{
		}
	}
}
