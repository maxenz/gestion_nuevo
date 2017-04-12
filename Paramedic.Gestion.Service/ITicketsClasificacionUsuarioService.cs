using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Service
{
    public interface ITicketsClasificacionUsuarioService : IEntityService<TicketsClasificacionUsuario>
    {
        IEnumerable<TicketsClasificacionUsuario> GetByClasificacionId(int id);
    }
}
