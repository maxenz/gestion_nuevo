﻿using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Service
{
    public interface IUserProfileService : IEntityService<UserProfile>
    {
        int GetCurrentUserId(string userIdentity);

        UserProfile GetById(int id);
    }
}
