using System;

namespace UserService.Domain.ValueObjects
{
    public class BirthDate
    {
        public DateTime Date { get; private set; }

        public BirthDate(DateTime birthDate)
        {
            Date = birthDate;
            
            if(birthDate >= DateTime.Now)
                throw new Exception("Invalid birth date");
        }
    }
}