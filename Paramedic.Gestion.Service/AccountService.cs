using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System.Web.Security;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Paramedic.Gestion.Service
{
    public class AccountService : EntityService<UserProfile>, IAccountService
    {
        IUnitOfWork _unitOfWork;
        IAccountRepository _accountRepository;

        public AccountService(IUnitOfWork unitOfWork, IAccountRepository accountRepository)
            : base(unitOfWork, accountRepository)
        {
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }

        public string getSelectedRole(string userName)
        {
            if (HasRole("Cliente", userName))
            {
                return "Cliente";
            }

            if (HasRole("Administrador", userName))
            {
                return "Administrador";
            }

            if (HasRole("Cliente ticket", userName))
            {
                return "Cliente ticket";
            }

			if (HasRole("Colaborador", userName))
			{
				return "Colaborador";
			}

			return "";

        }

        private bool HasRole(string role, string userName)
        {
            return Roles.FindUsersInRole(role, userName).Length > 0;
        }

        public void UpdateRole(string selectedRole, string userName)
        {
            Roles.RemoveUserFromRole(userName, getSelectedRole(userName));
            Roles.AddUserToRole(userName, selectedRole);
        }

        public UserProfile DeleteUser(int userId)
        {
            UserProfile user = _accountRepository.FindBy(x => x.Id == userId).FirstOrDefault();
            Roles.RemoveUserFromRole(user.UserName, getSelectedRole(user.UserName));
            return user;

        }

    }
}
