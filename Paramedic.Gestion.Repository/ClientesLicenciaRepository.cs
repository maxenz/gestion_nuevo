using Paramedic.Gestion.Model;
using System.Data.Entity;
using System.Linq;

namespace Paramedic.Gestion.Repository
{
    public class ClientesLicenciaRepository : GenericRepository<ClientesLicencia>, IClientesLicenciaRepository
    {
        #region Constructors

        public ClientesLicenciaRepository(DbContext context)
    : base(context)
        {

        }

        #endregion

        #region Public Methods

        public ClientesLicencia GetById(int id)
        {
            return _dbset.Include(x => x.ClientesLicenciasProductos).Where(x => x.Id == id).FirstOrDefault();
        }

        #endregion
    }
}
