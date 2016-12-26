using Paramedic.Gestion.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Paramedic.Gestion.Repository
{
    public class ProvinciaRepository : GenericRepository<Provincia>, IProvinciaRepository
    {
        public ProvinciaRepository(DbContext context)
            : base(context)
        {

        }

        public override IEnumerable<Provincia> GetAll()
        {
            return _entities.Set<Provincia>().Include(x => x.Pais).AsEnumerable();
        }

        public Provincia GetById(int id)
        {
            return _dbset.Include(x => x.Pais).Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
