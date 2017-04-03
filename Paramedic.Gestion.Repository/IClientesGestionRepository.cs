using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Repository
{
    public interface IClientesGestionRepository : IGenericRepository<ClientesGestion>
    {
        ClientesGestion GetById(int id);
    }
}
