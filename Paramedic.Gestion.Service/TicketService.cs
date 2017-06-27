using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System.Collections.Generic;
using LinqKit;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Linq;

namespace Paramedic.Gestion.Service
{
    public class TicketService : EntityService<Ticket>, ITicketService
    {
        #region Properties

        IUnitOfWork _unitOfWork;
        ITicketRepository _ticketRepository;
		IClientesUsuarioRepository _clientesUsuarioRepository;

        #endregion

        #region Constructors

        public TicketService(IUnitOfWork unitOfWork, ITicketRepository ticketRepository, IClientesUsuarioRepository clientesUsuarioRepository)
    : base(unitOfWork, ticketRepository)
        {
            _unitOfWork = unitOfWork;
            _ticketRepository = ticketRepository;
			_clientesUsuarioRepository = clientesUsuarioRepository;
        }

        #endregion

        #region Public Methods

        public Ticket GetById(int Id)
        {
            return _ticketRepository.GetById(Id);
        }

        public IEnumerable<Ticket> GetTickets(TicketQueryControllerParametersDTO queryParameters)
        {
            var predicate = getPredicateByConditions(queryParameters);

            return FindByPage(predicate, "CreatedDate DESC", queryParameters.PageSize, queryParameters.Page);
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

            if (!queryParameters.IsAdmin)
            {
                predicate = predicate.And(p => p.UserProfileId == queryParameters.UserId);
            }

            if (queryParameters.TicketClasificacionId != 0)
            {
                predicate = predicate.And(p => p.TicketsClasificacionId == queryParameters.TicketClasificacionId);
            }

			if (queryParameters.SelectedUserId != 0)
			{
				predicate = predicate.And(p => p.UserProfileId == queryParameters.SelectedUserId);
			}

			if (queryParameters.Estado != 0)
			{
				predicate = predicate.And(p => p.TicketEstadoType == queryParameters.Estado);
			}

			if (queryParameters.ClienteId != 0)
			{
				IEnumerable<int> users = _clientesUsuarioRepository.FindBy(x => x.ClienteId == queryParameters.ClienteId).Select(x => x.UsuarioId);
				predicate = predicate.And(p => users.Any(x => x == p.UserProfileId));
			}

            if (!predicate.IsStarted)
            {
                return null;
            }

            return predicate;
        }

        #endregion
    }
}
