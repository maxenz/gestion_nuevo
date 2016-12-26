using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Repository
{
    public interface ILocalidadRepository : IGenericRepository<Localidad>
    {
        Localidad GetById(long id);

    }
}
