using System;
using UserService.Domain.Entities;

namespace UserService.Domain.Data.Repositories
{
    public interface IPreviousImportItemRepository
    {
        PreviousImportItem GetPreviousImportItem(Guid id);
        void UpdatePreviousImportItem(PreviousImportItem previousImportItem);
    }
}