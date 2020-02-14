namespace UserService.Domain.Models
{
    public class ImportResult
    {
        public int Inserted { get; set; }
        public int Updated { get; set; }
        public int Ignored { get; set; }
        public int Failed { get; set; }
    }
}