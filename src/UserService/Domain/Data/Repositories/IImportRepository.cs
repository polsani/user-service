using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Domain.Entities;
using UserService.Domain.Models;

namespace UserService.Domain.Data.Repositories
{
    public interface IImportRepository
    {
        Task PreviousImportCreate(Import import);
        IEnumerable<Import> GetPreviousImportNotImported(int page, int pageSize);
        Import GetImport(Guid id);
        void UpdateImport(Import import);
        IEnumerable<PreviousImportItem> GetPreviousImportItems(Guid importId);
        IEnumerable<Import> GetImports(bool? approved, int page, int pageSize);
        ImportResult GetImportResult(Guid importId);
    }
}