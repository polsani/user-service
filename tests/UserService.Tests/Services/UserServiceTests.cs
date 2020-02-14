using System;
using Moq;
using UserService.Domain.Data.Repositories;
using UserService.Domain.Data.UnitOfWork;
using UserService.Domain.Entities;
using Xunit;

namespace UserService.Tests.Services
{
    public class UserServiceTests
    {
        private UserService.Services.UserService _subject;

        [Fact]
        public void CheckServiceDependencies()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var previousImportItemRepositoryMock = new Mock<IPreviousImportItemRepository>();

            previousImportItemRepositoryMock.Setup(x => x.GetPreviousImportItem(It.IsAny<Guid>()))
                .Returns(new PreviousImportItem
                {
                    Email = "email@email.com",
                    Gender = "Male",
                    Name = "Name",
                    BirthDate = "01/01/2001",
                    Id = Guid.NewGuid(),
                    ImportId = Guid.NewGuid(),
                    Status = 1
                });
            
            _subject = new UserService.Services.UserService(
                unitOfWorkMock.Object,
                userRepositoryMock.Object,
                previousImportItemRepositoryMock.Object);
            
            _subject.PersistUser(new User("name", "email@email.com", "01/01/2001", "Male"), Guid.NewGuid());
            
            unitOfWorkMock.Verify(x=>x.InitializeContext(), Times.Once);
            previousImportItemRepositoryMock.Verify(x=>x.GetPreviousImportItem(It.IsAny<Guid>()), Times.Once);
        }
    }
}