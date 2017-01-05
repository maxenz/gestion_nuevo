using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
    public class TipoTerminalRepository : GenericRepository<TipoTerminal>, ITipoTerminalRepository
    {
        public TipoTerminalRepository(DbContext context)
            : base(context)
        {

        }
    }
}
