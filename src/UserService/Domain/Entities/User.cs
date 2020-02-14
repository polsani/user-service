using System;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; }
        public Email Email { get; }
        public BirthDate BirthDate { get; }

        public User(string name, string email, DateTime birthDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = new Email(email);
            BirthDate = new BirthDate(birthDate);
        }
        
        public User(Guid id, string name, string email, DateTime birthDate)
        {
            Id = id;
            Name = name;
            Email = new Email(email);
            BirthDate = new BirthDate(birthDate);
        }
    }
}