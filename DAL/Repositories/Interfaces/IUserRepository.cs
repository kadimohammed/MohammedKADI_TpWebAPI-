using DAL.Entities;

namespace DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
         Task<User> GetUserByUsernameAsync(string username);
         Task<User> RegisterUserAsync(User user);
    }
}
