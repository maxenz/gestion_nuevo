using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System.Collections.Generic;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
    public class TicketService : EntityService<Ticket>, ITicketService
    {
        IUnitOfWork _unitOfWork;
        ITicketRepository _ticketRepository;

        public TicketService(IUnitOfWork unitOfWork, ITicketRepository ticketRepository)
            : base(unitOfWork, ticketRepository)
        {
            _unitOfWork = unitOfWork;
            _ticketRepository = ticketRepository;
        }

        public Ticket GetById(int Id)
        {
            return _ticketRepository.GetById(Id);
        }

        public IEnumerable<Ticket> GetTickets(TicketQueryControllerParametersDTO queryParameters)
        {
            var predicate = getPredicateByConditions(queryParameters);
            return FindByPage(predicate, (x => x.CreatedDate), queryParameters.PageSize, queryParameters.Page);
        }

        public IEnumerable<Ticket> FindBy(TicketQueryControllerParametersDTO queryParameters)
        {
            var predicate = getPredicateByConditions(queryParameters);
            return _ticketRepository.FindBy(predicate);
        }

        public Expression<Func<Ticket, bool>> getPredicateByConditions(TicketQueryControllerParametersDTO queryParameters)
        {
            var predicate = PredicateBuilder.New<Ticket>();

            if (!string.IsNullOrEmpty(queryParameters.SearchDescription))
            {
                predicate = predicate.And(p => (p.Asunto.Contains(queryParameters.SearchDescription)));
            }

            if (!string.IsNullOrEmpty(queryParameters.FutureFeature))
            {
                predicate = predicate.And(p => p.FuturaMejora);
            }

            if (!queryParameters.IsAdmin)
            {
                predicate = predicate.And(p => p.UserProfileId == queryParameters.UserId);
            }

            return predicate;
        }

    }
}
