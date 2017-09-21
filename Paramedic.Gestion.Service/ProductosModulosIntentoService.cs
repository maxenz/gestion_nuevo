using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Paramedic.Gestion.Service
{
	public class ProductosModulosIntentoService : EntityService<ProductosModulosIntento>, IProductosModulosIntentoService
	{
		IUnitOfWork _unitOfWork;
		IProductosModulosIntentoRepository _repo;

		public ProductosModulosIntentoService(IUnitOfWork unitOfWork, IProductosModulosIntentoRepository repo)
			: base(unitOfWork, repo)
		{
			_unitOfWork = unitOfWork;
			_repo = repo;
		}

		public IEnumerable<ProductosModulosIntento> GetIntentosByProductoModuloId(int prodModId)
		{
			return _repo.FindBy(x => x.ProductosModuloId == prodModId).OrderBy(x => x.Orden);
		}
	}
}
