using System;
using UserService.Data.Annotations;

namespace UserService.Domain.Entities
{
    public class PreviousImportItem : Entity
    {
        [BulkInsertColumnName("id")]
        public Guid Id { get; set; }
        
        [BulkInsertColumnName("name")]
        public string Name { get; set; }
        
        [BulkInsertColumnName("email")]
        public string Email { get; set; }
        
        [BulkInsertColumnName("birth_date")]
        public string BirthDate { get; set; }
        
        [BulkInsertColumnName("gender")]
        public string Gender { get; set; }
        
        [BulkInsertColumnName("status")]
        public int Status { get; set; }

        [BulkInsertColumnName("import_id")]
        public Guid ImportId { get; set; }
        
        [BulkInsertNotMapped]
        public virtual Import Import { get; set; }
    }
}