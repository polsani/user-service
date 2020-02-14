using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using UserService.Domain.Entities;

namespace UserService.Domain.Services
{
    public interface IImportService
    {
        Import PreviousImport(IFormFile file);
        IEnumerable<Import> GetPreviousImportNotImported();
        void ApproveImport();
    }
}