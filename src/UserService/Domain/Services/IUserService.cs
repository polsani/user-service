using System;
using UserService.Domain.Entities;

namespace UserService.Domain.Services
{
    public interface IUserService
    {
        void PersistUser(User user, Guid userImportRequestId);
    }
}