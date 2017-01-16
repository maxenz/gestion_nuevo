using Paramedic.Gestion.Model;
using System.Data.Entity;


namespace Paramedic.Gestion.Repository
{
    public class ClientesTerminalRepository : GenericRepository<ClientesTerminal>, IClientesTerminalRepository
    {
        public ClientesTerminalRepository(DbContext context)
            : base(context)
        {

        }
    }
}
