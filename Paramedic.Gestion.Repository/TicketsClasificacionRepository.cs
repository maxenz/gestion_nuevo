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
    }
}
