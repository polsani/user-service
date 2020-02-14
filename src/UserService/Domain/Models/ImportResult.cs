using System;

namespace UserService.Domain.Models
{
    public class ImportResult
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int AmountRows { get; set; }
        public int Inserted { get; set; }
        public int Updated { get; set; }
        public int Ignored { get; set; }
        public int Failed { get; set; }
    }
}