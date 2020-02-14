using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Data.Repositories;
using UserService.Domain.Data.UnitOfWork;
using UserService.Domain.Entities;

namespace UserService.Data.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public User GetUserByEmail(string email)
        {
            return UnitOfWork.Context.User.FirstOrDefault(x=>x.Email == email);
        }

        public void CreateUser(User user)
        {
            UnitOfWork.Context.User.Add(user);
        }

        public void UpdateUser(User user)
        {
            UnitOfWork.Context.User.Attach(user);
            UnitOfWork.Context.Entry(user).State = EntityState.Modified;
        }
    }
}