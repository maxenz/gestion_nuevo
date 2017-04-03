using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;

namespace Paramedic.Gestion.Service
{
    public class UserProfileEmailService : EntityService<UserProfileEmail>, IUserProfileEmailService
    {
        IUnitOfWork _unitOfWork;
        IUserProfileEmailRepository _repo;

        public UserProfileEmailService(IUnitOfWork unitOfWork, IUserProfileEmailRepository repo)
            : base(unitOfWork, repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;

        }
    }
}
