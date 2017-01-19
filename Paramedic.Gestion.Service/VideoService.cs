using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class VideoService : EntityService<Video>, IVideoService
    {
        IUnitOfWork _unitOfWork;
        IVideoRepository _videoRepository;

        public VideoService(IUnitOfWork unitOfWork, IVideoRepository videoRepository)
            : base(unitOfWork, videoRepository)
        {
        }
    }
}
