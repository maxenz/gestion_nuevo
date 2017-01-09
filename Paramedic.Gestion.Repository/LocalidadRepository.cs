using Paramedic.Gestion.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Paramedic.Gestion.Repository
{
    public class LocalidadRepository : GenericRepository<Localidad>, ILocalidadRepository
    {
        public LocalidadRepository(DbContext context)
            : base(context)
        {

        }

        public override IEnumerable<Localidad> GetAll()
        {
            return _entities.Set<Localidad>().Include(x => x.Provincia).AsEnumerable();
        }

        public Localidad GetById(long id)
        {
            return _dbset.Include(x => x.Provincia).Include(x => x.Provincia.Pais).Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
