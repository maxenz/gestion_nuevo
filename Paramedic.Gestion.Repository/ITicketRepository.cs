using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Repository
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Ticket GetById(int id);        

    }
}
