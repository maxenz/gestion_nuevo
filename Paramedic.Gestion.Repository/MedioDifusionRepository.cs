using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Repository
{
    public class MedioDifusionRepository : GenericRepository<MedioDifusion>, IMedioDifusionRepository
    {
        public MedioDifusionRepository(DbContext context)
            : base(context)
        {

        }
    }
}
