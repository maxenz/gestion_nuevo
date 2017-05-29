using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
