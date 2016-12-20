using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Repository
{
   public interface IPaisRepository : IGenericRepository<Pais>
    {
        Pais GetById(int id);
    }
}
