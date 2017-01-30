using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(DbContext context)
            : base(context)
        {

        }

        public Ticket GetById(int id)
        {
            return _dbset
                .Include(x => x.TicketEventos)
                .Include(x => x.Usuario)
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }
    }
}
