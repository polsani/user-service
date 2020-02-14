using System.Linq;
using UserService.Domain.Entities;
using Xunit;

namespace UserService.Tests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void UserMustHaveValidEmail()
        {
            var user = new User("Name", "email@email,com", "01/01/2001", "Male");
            
            Assert.True(user.Invalid);
            Assert.True(user.Notifications.FirstOrDefault(x=>x.Property == "User.Email") != null);
        }
        
        [Fact]
        public void UserMustHaveEmail()
        {
            var user = new User("Name", "", "01/01/2001", "Male");
            
            Assert.True(user.Invalid);
            Assert.True(user.Notifications.FirstOrDefault(x=>x.Property == "User.Email") != null);
        }
    }
}