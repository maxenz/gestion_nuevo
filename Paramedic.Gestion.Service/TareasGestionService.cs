using LinqKit;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
	public class TareasGestionService : EntityService<TareasGestion>, ITareasGestionService
	{
		IUnitOfWork _unitOfWork;
		ITareasGestionRepository _repo;

		public TareasGestionService(IUnitOfWork unitOfWork, ITareasGestionRepository repo)
			: base(unitOfWork, repo)
		{
			_unitOfWork = unitOfWork;
			_repo = repo;
		}

		public IEnumerable<TareasGestion> GetTareasGestion(DateQueryControllerParametersDTO queryParameters)
		{
			var predicate = getPredicateByConditions(queryParameters);

			return FindByPage(predicate, "CreatedDate DESC", queryParameters.PageSize, queryParameters.Page);
		}

		public IEnumerable<TareasGestion> FindBy(DateQueryControllerParametersDTO queryParameters)
		{
			var predicate = getPredicateByConditions(queryParameters);
			return _repo.FindBy(predicate);
		}

		public Expression<Func<TareasGestion, bool>> getPredicateByConditions(DateQueryControllerParametersDTO queryParameters)
		{
			var predicate = PredicateBuilder.New<TareasGestion>();

			if (!string.IsNullOrEmpty(queryParameters.SearchDescription))
			{
				predicate = predicate.And(p => (p.Tarea.Descripcion.Contains(queryParameters.SearchDescription)) || (p.Tarea.Proyecto.Descripcion.Contains(queryParameters.SearchDescription)) || (p.Usuario.UserName.Contains(queryParameters.SearchDescription)));
			}

			if (!queryParameters.IsCurrentUserAdmin)
			{
				predicate = predicate.And(p => p.UsuarioId == queryParameters.UserId);
			}
			

			predicate = predicate.And(p => p.Fecha >= queryParameters.DateFrom);
			predicate = predicate.And(p => p.Fecha <= queryParameters.DateTo);

			return predicate;
		}
	}
}
