using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Repository
{
    public class TicketClasificacionUsuarioRepository : GenericRepository<TicketsClasificacionUsuario>, ITicketsClasificacionUsuarioRepository
    {
        public TicketClasificacionUsuarioRepository(DbContext context)
            : base(context)
        {
           
        }

        public IEnumerable<TicketsClasificacionUsuario> GetByClasificacionId(int id)
        {
            return _dbset.Include(x => x.UserProfile).Where(x => x.TicketsClasificacionId == id);
        }
    }
}
