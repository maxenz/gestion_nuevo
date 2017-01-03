using Paramedic.Gestion.Model;
using System.Data.Entity;

namespace Paramedic.Gestion.Repository
{
    public class RevendedorRepository : GenericRepository<Revendedor>, IRevendedorRepository
    {
        public RevendedorRepository(DbContext context)
            : base(context)
        {

        }
    }
}
