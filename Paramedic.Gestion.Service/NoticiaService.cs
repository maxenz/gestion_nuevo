using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System.Collections.Generic;

namespace Paramedic.Gestion.Service
{
	public class NoticiaService : EntityService<Noticia>, INoticiaService
	{
		IUnitOfWork _unitOfWork;
		INoticiaRepository _repo;

		public NoticiaService(IUnitOfWork unitOfWork, INoticiaRepository repo)
			: base(unitOfWork, repo)
		{
			_unitOfWork = unitOfWork;
			_repo = repo;
		}

		public IEnumerable<Noticia> GetNoticiasNoVencidas()
		{
			return _repo.GetNoticiasNoVencidas();
		}

	}
}
