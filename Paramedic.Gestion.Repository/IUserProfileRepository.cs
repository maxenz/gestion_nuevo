using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Repository
{
    public interface IUserProfileRepository : IGenericRepository<UserProfile>
    {
        UserProfile GetById(int id);
    }
}
