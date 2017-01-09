using Paramedic.Gestion.Model;
using System.Data.Entity;


namespace Paramedic.Gestion.Repository
{
    public class ClientesContactoRepository : GenericRepository<ClientesContacto>, IClientesContactoRepository
    {
        public ClientesContactoRepository(DbContext context)
            : base(context)
        {

        }
    }

}