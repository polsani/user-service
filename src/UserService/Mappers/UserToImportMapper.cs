using UserService.Domain.Entities;
using UserService.Domain.Mappers;
using UserService.ViewModels;

namespace UserService.Mappers
{
    public class UserToImportMapper : IUserToImportMapper
    {
        public UserImportRequest ConvertToUserImportRequest(PreviousImportItem item)
        {
            return new UserImportRequest
            {
                Email = item.Email,
                Gender = item.Gender,
                Id = item.Id,
                Name = item.Name,
                BirthDate = item.BirthDate,
                ImportId = item.ImportId
            };
        }
    }
}