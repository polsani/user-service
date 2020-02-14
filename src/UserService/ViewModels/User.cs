using System;

namespace UserService.ViewModels
{
    public class User
    {
        public Guid ImportId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        
    }
}