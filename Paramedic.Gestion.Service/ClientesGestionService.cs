using LinqKit;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
    public class ClientesGestionService : EntityService<ClientesGestion>, IClientesGestionService
    {
        IUnitOfWork _unitOfWork;
        IClientesGestionRepository _clientesGestionRepository;

        public ClientesGestionService(IUnitOfWork unitOfWork, IClientesGestionRepository clientesGestionRepository)
            : base(unitOfWork, clientesGestionRepository)
        {
            _unitOfWork = unitOfWork;
            _clientesGestionRepository = clientesGestionRepository;
        }

        public IEnumerable<ClientesGestion> GetRecontactos(DateTime from, DateTime to)
        {
            var predicate = PredicateBuilder.New<ClientesGestion>();

            predicate = predicate.And(x => x.FechaRecontacto >= from);
            predicate = predicate.And(x => x.FechaRecontacto <= to);

            return FindBy(predicate);

        }

        public IEnumerable<ClientesGestion> GetRecontactosByPage(RecontactosControllerParametersDTO queryParameters)
        {
            var predicate = GetPredicateByConditions(queryParameters);
            return FindByPage(predicate, "Fecha ASC", queryParameters.PageSize, queryParameters.Page);
        }


        public Expression<Func<ClientesGestion, bool>> GetPredicateByConditions(RecontactosControllerParametersDTO queryParameters)
        {
            var predicate = PredicateBuilder.New<ClientesGestion>();

            DateTime genericFrom = DateTime.Now.AddDays(-1);
            DateTime genericTo = DateTime.Now.AddDays(30);

            if (queryParameters.DateFrom != DateTime.MinValue && queryParameters.DateTo != DateTime.MinValue)
            { 
                predicate = predicate.And(x => x.FechaRecontacto >= queryParameters.DateFrom);
                predicate = predicate.And(x => x.FechaRecontacto <= queryParameters.DateTo);
            } else
            {
                predicate = predicate.And(x => x.FechaRecontacto >= genericFrom);
                predicate = predicate.And(x => x.FechaRecontacto <= genericTo);
            }

            if (!string.IsNullOrEmpty(queryParameters.SearchDescription))
            {
                predicate = predicate.And(x => x.Cliente.RazonSocial.ToUpper().Contains(queryParameters.SearchDescription.ToUpper()));
            }

            if (queryParameters.GestionType != GestionType.Default)
            {
                if (queryParameters.GestionType == GestionType.Management)
                {
                    predicate = predicate.And(x => !x.FechaRecontacto.HasValue);
                } else
                {
                    predicate = predicate.And(x => x.FechaRecontacto.HasValue);
                }
            }

            return predicate;

        }
    }
}
