using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Paramedic.Gestion.Service
{
    public class UserProfileService : EntityService<UserProfile>, IUserProfileService
    {
        #region Properties

        IUnitOfWork _unitOfWork;
        IUserProfileRepository _userProfileRepository;

        #endregion

        #region Constructors

        public UserProfileService(IUnitOfWork unitOfWork, IUserProfileRepository userProfileRepository)
    : base(unitOfWork, userProfileRepository)
        {
            _unitOfWork = unitOfWork;
            _userProfileRepository = userProfileRepository;
        }

        #endregion

        #region Public Methods

        public int GetCurrentUserId(string userIdentity)
        {
            UserProfile user = 
                _userProfileRepository
                .FindBy(x => x.UserName.ToUpper() == userIdentity.ToUpper())
                .FirstOrDefault();

            return user.Id;
        }

        public UserProfile GetById(int id)
        {
            return _userProfileRepository.GetById(id);
        }

        #endregion
    }
}
