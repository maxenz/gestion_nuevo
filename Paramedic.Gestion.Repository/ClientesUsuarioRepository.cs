using Paramedic.Gestion.Model;
using System.Data.Entity;


namespace Paramedic.Gestion.Repository
{
    public class ClientesUsuarioRepository : GenericRepository<ClientesUsuario>, IClientesUsuarioRepository
    {
        public ClientesUsuarioRepository(DbContext context)
            : base(context)
        {

        }
    }
}
