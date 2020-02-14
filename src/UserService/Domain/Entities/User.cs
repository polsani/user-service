using System;
using System.Text.RegularExpressions;
using Manatee.Validation.Notifications;
using Microsoft.AspNetCore.Http;
using UserService.Domain.Enums;
using UserService.Domain.Validation;

namespace UserService.Domain.Entities
{
    public class User : Entity
    {
        private const string EmailRegex =
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Gender Gender { get; private set; }
        
        //TODO Map Email and Birth date as ValueObjects
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }

        private User()
        {
            
        }
        
        public User(string name, string email, string birthDate, string gender)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;

            if(DateTime.TryParse(birthDate, out var date))
            {
                BirthDate = date;
                if(BirthDate >= DateTime.Now.AddMinutes(-1))
                    AddNotification(StatusCodes.Status422UnprocessableEntity,
                        "User.BirthDate", 
                        "Birth date can't be the current date", 
                        birthDate, 
                        new Constraint(BirthDateValidations.InvalidBirthDate, null));
            }
            else
                AddNotification(StatusCodes.Status422UnprocessableEntity,
                    "User.BirthDate", 
                    "Invalid date format", 
                    birthDate, 
                    new Constraint(BirthDateValidations.InvalidDateFormat, null));

            if (Enum.TryParse(gender, true, out Gender enumGender))
                Gender = enumGender;
            else
                AddNotification(StatusCodes.Status422UnprocessableEntity,
                    "User.Gender",
                    "Invalid gender",
                    gender,
                    new Constraint(UserValidations.InvalidGender, new []
                    {
                        new Param("Gender","Male"),
                        new Param("Gender","Female"),
                        new Param("Gender","NotInformed"),
                    }));

            if(string.IsNullOrEmpty(Name))
                AddNotification(StatusCodes.Status422UnprocessableEntity, 
                    "User.Name", 
                    "Name can't be empty", 
                    Name, 
                    new Constraint(UserValidations.NameNotProvided, null));

            if(string.IsNullOrEmpty(Email))
                AddNotification(StatusCodes.Status422UnprocessableEntity, 
                    "EmailAddress.Address", 
                    "E-mail não pode ser vazio", 
                    Email, 
                    new Constraint(EmailValidations.EmailNotProvided, null));
            
            else if(!Regex.IsMatch(Email, EmailRegex))
                AddNotification(StatusCodes.Status422UnprocessableEntity,
                    "EmailAddress.Address",
                    "E-mail inválido",
                    Email,
                    new Constraint(EmailValidations.InvalidEmailAddress, null));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var user = (User) obj;

            return Email == user.Email && Name == user.Name && BirthDate == user.BirthDate && Gender == user.Gender;
        }
    }
}