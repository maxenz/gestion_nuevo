using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Repository
{
    public class TicketsClasificacionRepository : GenericRepository<TicketsClasificacion>, ITicketsClasificacionRepository
    {
        public TicketsClasificacionRepository(DbContext context)
            : base(context)
        {

        }

        public override IEnumerable<TicketsClasificacion> GetAll()
        {
            return _dbset
                .Include(x => x.TicketsClasificacionesUsuarios)
                .Include(x => x.TicketsClasificacionesUsuarios.Select(p => p.UserProfile.Emails))
                .AsEnumerable();
        }

        public TicketsClasificacion GetById(int id)
        {
            return _dbset
                .Include(x => x.TicketsClasificacionesUsuarios)
                .Include(x => x.TicketsClasificacionesUsuarios.Select(p => p.UserProfile.Emails))
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }
    }
}
