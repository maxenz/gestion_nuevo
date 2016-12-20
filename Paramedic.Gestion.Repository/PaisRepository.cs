using Paramedic.Gestion.Model;
using System.Data.Entity;
using System.Linq;

namespace Paramedic.Gestion.Repository
{
    public class PaisRepository : GenericRepository<Pais>, IPaisRepository
    {
        public PaisRepository(DbContext context)
            : base(context)
        {

        }

        public Pais GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
    }
}
