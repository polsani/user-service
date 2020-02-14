using System;
using UserService.Domain.Enums;
using UserService.Domain.Mappers;
using UserService.ViewModels;

namespace UserService.Mappers
{
    public class UserMapper : IUserMapper
    {
        public Domain.Entities.User ConvertToEntity(UserImportRequest userImportRequest)
        {
            return new Domain.Entities.User(userImportRequest.Id, userImportRequest.Name, userImportRequest.Email,
                userImportRequest.BirthDate, userImportRequest.Gender);
        }

        public User ConvertToEntity(Domain.Entities.User user)
        {
            return new User
            {
                Email = user.Email,
                Name = user.Name,
                BirthDate = user.BirthDate.Date.ToString("dd/MM/yyyy"),
                Gender = Enum.GetName(typeof(Gender), user.Gender),
                Id = user.Id
            };
        }
    }
}