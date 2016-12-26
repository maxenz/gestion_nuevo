using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
    public interface ITicketService : IEntityService<Ticket>
    {
        Ticket GetById(int id);

        IEnumerable<Ticket> GetTickets(TicketQueryControllerParametersDTO queryParameters);

        IEnumerable<Ticket> FindBy(TicketQueryControllerParametersDTO queryParameters);
    }
}
