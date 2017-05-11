using Paramedic.Gestion.Model;
using System.Data.Entity;
using System.Linq;

namespace Paramedic.Gestion.Repository
{
    public class SocialServiceTypesRepository : GenericRepository<SocialServiceType>, ISocialServiceTypesRepository
    {
        #region Constructors

        public SocialServiceTypesRepository(DbContext context) : base(context)
        {

        }

        #endregion

        #region Overriden Methods

        public override SocialServiceType GetById(int id)
        {
            return _dbset.Include(x => x.SocialServices).FirstOrDefault(x => x.Id == id);
        }

        #endregion
    }
}
