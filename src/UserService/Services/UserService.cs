using System;
using UserService.Domain.Data.Repositories;
using UserService.Domain.Data.UnitOfWork;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Services;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPreviousImportItemRepository _previousImportItemRepository;
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IPreviousImportItemRepository previousImportItemRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _previousImportItemRepository = previousImportItemRepository;
        }
        
        public void PersistUser(User user, Guid userImportRequestId)
        {
            _unitOfWork.InitializeContext();

            var previousImportItem = _previousImportItemRepository.GetPreviousImportItem(userImportRequestId);
            
            if (user.Invalid)
            {
                previousImportItem.Status = (int) PreviousImportItemStatus.Failed;
            }
            else
            {
                var importedUser = _userRepository.GetUserByEmail(user.Email);
                
                if (importedUser == null)
                {
                    _userRepository.CreateUser(user);
                    previousImportItem.Status = (int) PreviousImportItemStatus.Inserted;
                }
                else
                {
                    if (user.Equals(importedUser))
                    {
                        previousImportItem.Status = (int) PreviousImportItemStatus.Ignored;
                    }
                    else
                    {
                        importedUser.Gender = user.Gender;
                        importedUser.Name = user.Name;
                        importedUser.BirthDate = user.BirthDate;
                        
                        _userRepository.UpdateUser(importedUser);
                        previousImportItem.Status = (int) PreviousImportItemStatus.Updated;
                    }
                }
            }
            
            _previousImportItemRepository.UpdatePreviousImportItem(previousImportItem);
            
            _unitOfWork.Commit();
        }
    }
}