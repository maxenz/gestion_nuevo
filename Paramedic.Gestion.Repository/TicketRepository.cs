using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using LinqKit;
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
                .Include(x => x.Usuario.Emails)
                .Include(x => x.TicketsClasificacion)
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public override IEnumerable<Ticket> FindByPage(Expression<Func<Ticket, bool>> whereExp, string orderExp, int pageSize, int page = 1)
        {
            IEnumerable<Ticket> query;

            if (whereExp != null)
            {
                query = _dbset.Include(x => x.TicketsClasificacion).Where(whereExp).OrderBy(orderExp).Skip((page - 1) * pageSize).Take(pageSize);
            }
            else
            {
                query = _dbset.Include(x => x.TicketsClasificacion).OrderBy(orderExp).Skip((page - 1) * pageSize).Take(pageSize);
            }

            return query;
        }

        public override IEnumerable<Ticket> FindBy(Expression<Func<Ticket, bool>> predicate)
        {
            IEnumerable<Ticket> query;
            if (predicate != null)
            {
                query = _dbset.AsExpandable().Where(predicate).AsEnumerable();
            }
            else
            {
                query = _dbset.AsExpandable().AsEnumerable();
            }

            return query;
        }
    }
}
