using Paramedic.Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Service
{
    public interface IAccountService : IEntityService<UserProfile>
    {

        string getSelectedRole(string userName);

        void UpdateRole(string selectedRole, string userName);

        UserProfile DeleteUser(int userId);

    }
}
