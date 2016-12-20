using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Repository
{
    public interface ILocalidadRepository : IGenericRepository<Localidad>
    {
        IEnumerable<Localidad> GetAll();
        Localidad GetById(long id);

    }
}
