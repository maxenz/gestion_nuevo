using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Paramedic.Gestion.Repository
{
    public class VideoRepository : GenericRepository<Video>, IVideoRepository
    {
        public VideoRepository(DbContext context)
            : base(context)
        {

        }

        public new IEnumerable<Video> FindByPage(Expression<Func<Video, bool>> whereExp, string orderExp, int pageSize, int page = 1)
        {
            IEnumerable<Video> query;

            if (whereExp != null)
            {
                query = _dbset.Where(whereExp).Include(x => x.ClientesVideos).OrderBy(orderExp).Skip((page - 1) * pageSize).Take(pageSize);
            }
            else
            {
                query = _dbset.Include(x => x.ClientesVideos).OrderBy(orderExp).Skip((page - 1) * pageSize).Take(pageSize);
            }

            return query;
        }
    }
}
