using System;
using UserService.Domain.Mappers;

namespace UserService.Mappers
{
    public class UserMapper : IUserMapper
    {
        public ViewModels.User ConvertToViewModel(Domain.Entities.User user)
        {
            return new ViewModels.User
            {
                Email = user.Email.Address,
                Name = user.Name,
                BirthDate = user.BirthDate.Date
            };
        }

        public Domain.Entities.User ConvertToEntity(ViewModels.User user)
        {
            return new Domain.Entities.User(Guid.NewGuid(), user.Name, user.Email, user.BirthDate);
        }
    }
}