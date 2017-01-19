using Paramedic.Gestion.Model;
using System.Data.Entity;


namespace Paramedic.Gestion.Repository
{
    public class VideoRepository : GenericRepository<Video>, IVideoRepository
    {
        public VideoRepository(DbContext context)
            : base(context)
        {

        }
    }
}
