using Paramedic.Gestion.Model;
using System;
using System.Data.Entity;


namespace Paramedic.Gestion.Repository
{
    public class ClientesGestionRepository : GenericRepository<ClientesGestion>, IClientesGestionRepository
    {
        public ClientesGestionRepository(DbContext context)
            : base(context)
        {
        }

        public ClientesGestion GetById(int id)
        {
            return _dbset.Find(id);
        }
    }

}