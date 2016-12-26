using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public class UserProfileService : EntityService<UserProfile>, IUserProfileService
    {
        IUnitOfWork _unitOfWork;
        IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUnitOfWork unitOfWork, IUserProfileRepository userProfileRepository)
            : base(unitOfWork, userProfileRepository)
        {
            _unitOfWork = unitOfWork;
            _userProfileRepository = userProfileRepository;
        }
    }
}
