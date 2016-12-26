using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System.Collections.Generic;

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

        public IEnumerable<Ticket> GetAdministratorTickets()
        {
            return _ticketRepository.GetAll();
        }

    }
}
