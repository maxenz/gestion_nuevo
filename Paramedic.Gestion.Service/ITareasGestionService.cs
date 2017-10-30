using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
	public interface ITareasGestionService : IEntityService<TareasGestion>
	{
		IEnumerable<TareasGestion> GetTareasGestion(DateQueryControllerParametersDTO queryParameters);

		IEnumerable<TareasGestion> FindBy(DateQueryControllerParametersDTO queryParameters);

		Expression<Func<TareasGestion, bool>> getPredicateByConditions(DateQueryControllerParametersDTO queryParameters);
	}
}
