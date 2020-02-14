using UserService.Domain.Entities;

namespace UserService.Domain.Data.Repositories
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        void CreateUser(User user);
        void UpdateUser(User user);
    }
}