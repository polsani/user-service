using System;
using System.Collections.Generic;

namespace UserService.Domain.Entities
{
    public class Import
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Approved { get; set; }
        public DateTime? ImportDate { get; set; }
        public int AmountRows { get; set; }

        public virtual IEnumerable<PreviousImportItem> PreviousImportItems { get; private set; }

        public Import()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
            Approved = false;
        }

        public void InitializeImport(IEnumerable<PreviousImportItem> previousImportItems)
        {
            PreviousImportItems = previousImportItems;
        }
    }
}