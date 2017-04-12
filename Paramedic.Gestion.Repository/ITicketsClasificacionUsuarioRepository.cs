using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Repository
{
    public interface ITicketsClasificacionUsuarioRepository : IGenericRepository<TicketsClasificacionUsuario>
    {
        IEnumerable<TicketsClasificacionUsuario> GetByClasificacionId(int id);
    }
}
